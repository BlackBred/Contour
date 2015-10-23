namespace Contour.Transport.RabbitMQ.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using Common.Logging;

    using Contour.Configuration;
    using Contour.Sending;

    using global::RabbitMQ.Client;

    /// <summary>
    /// ����������� ���������.
    /// ����������� ��������� ��� ���������� ����� ��������� � ���������� �������� �����.
    /// � ������ �������� ������� � ��������� ������, ����������� ������� ���������� ��������� ���������.
    /// </summary>
    internal class Producer : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ����������� ������������� �������.
        /// </summary>
        private readonly IPublishConfirmationTracker confirmationTracker = new NullPublishConfirmationTracker();

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Producer"/>.
        /// </summary>
        /// <param name="bus">
        /// �������� �����, ��� ������� ��������� �����������.
        /// </param>
        /// <param name="label">
        /// ����� ���������, ������� ����� �������������� ��� ����������� ���������.
        /// </param>
        /// <param name="routeResolver">
        /// ������������ ���������, �� ������� ����� �������� � �������� ���������.
        /// </param>
        /// <param name="confirmationIsRequired">
        /// ���� <c>true</c> - ����� ����������� ����� ������� ������������� � ���, ��� ��������� ���� ��������� � �������.
        /// </param>
        public Producer(RabbitBus bus, MessageLabel label, IRouteResolver routeResolver, bool confirmationIsRequired)
        {
            this.Channel = bus.OpenChannel();
            this.Label = label;
            this.RouteResolver = routeResolver;
            this.ConfirmationIsRequired = confirmationIsRequired;

            if (this.ConfirmationIsRequired)
            {
                this.confirmationTracker = new DefaultPublishConfirmationTracker(this.Channel);
                this.Channel.EnablePublishConfirmation();
                this.Channel.OnConfirmation(this.confirmationTracker.HandleConfirmation);
            }

            this.Failed += _ => ((IBusAdvanced)bus).Panic();
        }

        /// <summary>
        /// ������� � ������� ���������� ��������.
        /// </summary>
        public event Action<Producer> Failed = p => { };

        /// <summary>
        /// ����� ���������, ������� ������������ ��� �������� ���������.
        /// </summary>
        public MessageLabel Label { get; private set; }

        /// <summary>
        /// <c>true</c>, ���� � ����������� ��������� �����.
        /// </summary>
        public bool HasFailed
        {
            get
            {
                return !this.Channel.IsOpen;
            }
        }

        /// <summary>
        /// ��������� (����������) �������� ��������� �� ������.
        /// </summary>
        protected Listener CallbackListener { get; private set; }

        /// <summary>
        /// ����� ����������� � �������.
        /// </summary>
        protected RabbitChannel Channel { get; private set; }

        /// <summary>
        /// <c>true</c> - ���� ��� �������� ���������� ������������� � ���, ��� ������ �������� ���������.
        /// </summary>
        protected bool ConfirmationIsRequired { get; private set; }

        /// <summary>
        /// ������������ ��������� �������� � ���������.
        /// </summary>
        protected IRouteResolver RouteResolver { get; private set; }

        /// <summary>
        /// ����������� ������� �������.
        /// </summary>
        public void Dispose()
        {
            Logger.Trace(m => m("Disposing producer of [{0}].", this.Label));

            if (this.confirmationTracker != null)
            {
                this.confirmationTracker.Dispose();
            }

            if (this.Channel != null)
            {
                this.Channel.Dispose();
            }
        }

        /// <summary>
        /// ���������� ��������� <paramref name="message"/> � ���� ���������.
        /// </summary>
        /// <param name="message">
        /// ������������ ���������.
        /// </param>
        /// <returns>
        /// ������, ������� ���������� ��������� � ���� ���������.
        /// </returns>
        public Task Publish(IMessage message)
        {
            var nativeRoute = (RabbitRoute)this.RouteResolver.Resolve(this.Channel.Bus.Endpoint, message.Label);

            Logger.Trace(m => m("Emitting message [{0}] through [{1}].", message.Label, nativeRoute));

            Action<IBasicProperties> propsVisitor = p => ApplyPublishingOptions(p, message.Headers);

            // getting next seqno and publishing should go together
            lock (this.confirmationTracker)
            {
                Task confirmation = this.confirmationTracker.Track();
                this.Channel.Publish(nativeRoute, message, propsVisitor);
                return confirmation;
            }
        }

        /// <summary>
        /// ��������� ������ � ���� ��������� ��������� ��������� <see cref="IMessage"/> � ������� ����� � ����� <paramref name="expectedResponseType"/>.
        /// </summary>
        /// <param name="request">
        /// ���������� ������������ ��� ������.
        /// </param>
        /// <param name="expectedResponseType">
        /// ��������� ��� ������.
        /// </param>
        /// <returns>
        /// ���������� ������, ������� ��������� ������ � ���� ���������.
        /// </returns>
        /// <exception cref="Exception">
        /// ���������� ������������ ��� ������������� ���� �� ����� �������� �������.
        /// </exception>
        /// <exception cref="MessageRejectedException">
        /// ���������� ������������, ���� ������ ��������� ������ � ���� ���������.
        /// </exception>
        public Task<IMessage> Request(IMessage request, Type expectedResponseType)
        {
            request.Headers[Headers.ReplyRoute] = this.ResolveCallbackRoute();

            var timeout = Headers.Extract<TimeSpan?>(request.Headers, Headers.Timeout);
            var correlationId = (string)request.Headers[Headers.CorrelationId];

            Task<IMessage> responseTask = this.CallbackListener.Expect(correlationId, expectedResponseType, timeout);

            // TODO: join with responseTask (AttachToParent has proven to be too slow)
            this.Publish(request)
                .ContinueWith(
                    t =>
                        {
                            if (t.IsFaulted)
                            {
                                if (t.Exception != null)
                                {
                                    throw t.Exception.Flatten().InnerException;
                                }

                                throw new MessageRejectedException();
                            }
                        }, 
                    TaskContinuationOptions.ExecuteSynchronously)
                .Wait();

            return responseTask;
        }

        /// <summary>
        /// ��������� �����������, ����� ����� ����� �������� ������ �� �������.
        /// </summary>
        public void Start()
        {
            Logger.Trace(m => m("Starting producer of [{0}].", this.Label));
            if (this.CallbackListener != null)
            {
                this.CallbackListener.StartConsuming();
            }
        }

        /// <summary>
        /// ������������� �����������, ����� ����� ������ �� ������� �� ����� ��������������.
        /// </summary>
        public void Stop()
        {
            Logger.Trace(m => m("Stopping producer of [{0}].", this.Label));
            if (this.CallbackListener != null)
            {
                this.CallbackListener.StopConsuming();
            }
        }

        /// <summary>
        /// ������������ ��������� ������ �� ������.
        /// </summary>
        /// <param name="listener">
        /// ��������� ������ �� ������.
        /// </param>
        /// <exception cref="BusConfigurationException">
        /// ������������, ���� ��� ��������������� ��������� ������ �� ������.
        /// </exception>
        public void UseCallbackListener(Listener listener)
        {
            if (this.CallbackListener != null)
            {
                throw new BusConfigurationException("Callback listener for producer [{0}] is already defined.".FormatEx(this.Label));
            }

            this.CallbackListener = listener;
            this.CallbackListener.Failed += l =>
                {
                    this.CallbackListener = null;
                    this.Failed(this);
                };
        }

        /// <summary>
        /// ������������� ��������� ��������� � �������� ���������.
        /// </summary>
        /// <param name="props">�������� ���������, ���� ��������������� ���������.</param>
        /// <param name="headers">��������������� ��������� ���������.</param>
        private static void ApplyPublishingOptions(IBasicProperties props, IDictionary<string, object> headers)
        {
            if (headers == null)
            {
                return;
            }

            var persist = Headers.Extract<bool?>(headers, Headers.Persist);
            var ttl = Headers.Extract<TimeSpan?>(headers, Headers.Ttl);
            var correlationId = Headers.Extract<string>(headers, Headers.CorrelationId);
            var replyRoute = Headers.Extract<RabbitRoute>(headers, Headers.ReplyRoute);

            if (persist.HasValue && persist.Value)
            {
                props.DeliveryMode = 2;
            }

            if (ttl.HasValue)
            {
                props.Expiration = ttl.Value.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
            }

            if (correlationId != null)
            {
                props.CorrelationId = correlationId;
            }

            if (replyRoute != null)
            {
                props.ReplyToAddress = new PublicationAddress("direct", replyRoute.Exchange, replyRoute.RoutingKey);
            }
        }

        /// <summary>
        /// ���������� ������� ��������� ���������.
        /// </summary>
        /// <returns>
        /// ������� ��������� ���������.
        /// </returns>
        /// <exception cref="BusConfigurationException">
        /// ������������ � ������, ���� ��� �������� �����, ������� ����� ��������� ��������� � ����� ������ ��� � ������, ���� ��� �� ���������� �������� �������� ���������.
        /// </exception>
        private IRoute ResolveCallbackRoute()
        {
            if (this.CallbackListener == null)
            {
                throw new BusConfigurationException(string.Format("No reply endpoint is defined for publisher of [{0}].", this.Label));
            }

            if (this.CallbackListener.Endpoint.CallbackRouteResolver == null)
            {
                throw new BusConfigurationException(string.Format("No callback route resolver is defined for listener on [{0}].", this.CallbackListener.Endpoint.ListeningSource));
            }

            return this.CallbackListener.Endpoint.CallbackRouteResolver.Resolve(this.Channel.Bus.Endpoint, MessageLabel.Any);
        }
    }
}
