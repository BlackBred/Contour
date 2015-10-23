using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;

using Contour.Configuration;
using Contour.Receiving;
using Contour.Sending;
using Contour.Serialization;

namespace Contour
{
    /// <summary>
    /// ���� ���������, ������� �� ����� � ������������ ������.
    /// </summary>
    internal abstract class AbstractBus : IBus
    {
        /// <summary>
        /// ����������� ������ ����������� ���� ���������.
        /// </summary>
        private readonly IBusComponentTracker componentTracker = new BusComponentTracker();

        /// <summary>
        /// ������������ ���� ���������.
        /// </summary>
        private readonly BusConfiguration configuration;

        /// <summary>
        /// ������ ���� ���������.
        /// </summary>
        private readonly ILog logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="AbstractBus"/>.
        /// </summary>
        /// <param name="configuration">������������ ���� ���������.</param>
        protected AbstractBus(BusConfiguration configuration)
        {
            this.configuration = configuration;

            if (this.configuration.LifecycleHandler != null)
            {
                this.Starting += this.configuration.LifecycleHandler.OnStarting;
                this.Started += this.configuration.LifecycleHandler.OnStarted;
                this.Stopping += this.configuration.LifecycleHandler.OnStopping;
                this.Stopped += this.configuration.LifecycleHandler.OnStopped;
            }
        }

        /// <summary>
        /// ���������� ��������� ������ <see cref="AbstractBus"/>. 
        /// </summary>
        ~AbstractBus()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// ������� ��������� ����������.
        /// </summary>
        public event Action<IBus, EventArgs> Connected = (bus, args) => { };

        /// <summary>
        /// ������� ������� ����������.
        /// </summary>
        public event Action<IBus, EventArgs> Disconnected = (bus, args) => { };

        /// <summary>
        /// ������� ������� ���� ���������.
        /// </summary>
        public event Action<IBus, EventArgs> Started = (bus, args) => { };

        /// <summary>
        /// ������� ������ ������� ���� ���������.
        /// </summary>
        public event Action<IBus, EventArgs> Starting = (bus, args) => { };

        /// <summary>
        /// ������� �������� ���� ���������.
        /// </summary>
        public event Action<IBus, EventArgs> Stopped = (bus, args) => { };

        /// <summary>
        /// ������� ������ �������� ���� ���������.
        /// </summary>
        public event Action<IBus, EventArgs> Stopping = (bus, args) => { };

        /// <summary>
        /// ����������� ������ ����������� ����.
        /// </summary>
        public IBusComponentTracker ComponentTracker
        {
            get
            {
                return this.componentTracker;
            }
        }

        /// <summary>
        /// ������������ ����.
        /// </summary>
        public BusConfiguration Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        /// <summary>
        /// �������� ����� ����.
        /// </summary>
        public IEndpoint Endpoint
        {
            get
            {
                return this.configuration.Endpoint;
            }
        }

        /// <summary>
        /// ��������� �� � ���� ������������.
        /// </summary>
        public bool IsConfigured { get; internal set; }

        /// <summary>
        /// ������ �� ���� � ������.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return this.WhenReady.WaitOne(0);
            }
        }

        /// <summary>
        /// ���� ��������� � �������� ���������� ������.
        /// </summary>
        public bool IsShuttingDown { get; internal set; }

        /// <summary>
        /// �������� �� ����.
        /// </summary>
        public bool IsStarted { get; internal set; }

        /// <summary>
        /// ���������� ����� ���������.
        /// </summary>
        public IMessageLabelHandler MessageLabelHandler
        {
            get
            {
                return this.Configuration.MessageLabelHandler;
            }
        }

        /// <summary>
        /// ��������������� ���������.
        /// </summary>
        public IPayloadConverter PayloadConverter
        {
            get
            {
                return this.Configuration.Serializer;
            }
        }

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        public IEnumerable<IReceiver> Receivers
        {
            get
            {
                return this.componentTracker.AllOf<IReceiver>();
            }
        }

        /// <summary>
        /// ����������� ���������.
        /// </summary>
        public IEnumerable<ISender> Senders
        {
            get
            {
                return this.componentTracker.AllOf<ISender>();
            }
        }

        /// <summary>
        /// ������ ������������� �������� ���������� ���� ���������.
        /// </summary>
        public abstract WaitHandle WhenReady { get; }

        /// <summary>
        /// ������������ ����.
        /// </summary>
        IBusConfiguration IBus.Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        /// <summary>
        /// ��������� ����������� ������������ ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ���������.</param>
        /// <returns><c>true</c> - ���� ���� ��������� ����� ������������ ��������� ����� ���������.</returns>
        public bool CanHandle(string label)
        {
            return this.CanHandle(label.ToMessageLabel());
        }

        /// <summary>
        /// ��������� ����������� ������������ ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ���������.</param>
        /// <returns><c>true</c> - ���� ���� ��������� ����� ������������ ��������� ����� ���������.</returns>
        public bool CanHandle(MessageLabel label)
        {
            return this.configuration.ReceiverConfigurations.Any(cc => cc.Label.Equals(label));
        }

        /// <summary>
        /// ��������� ����������� ��������� ������� ��� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ���������.</param>
        /// <returns><c>true</c> - ���� ���� ��������� ����� ������� ������� ��� ��������� ����� ���������.</returns>
        public bool CanRoute(string label)
        {
            return this.CanRoute(label.ToMessageLabel());
        }

        /// <summary>
        /// ��������� ����������� ��������� ������� ��� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ���������.</param>
        /// <returns><c>true</c> - ���� ���� ��������� ����� ������� ������� ��� ��������� ����� ���������.</returns>
        public bool CanRoute(MessageLabel label)
        {
            return this.configuration.SenderConfigurations.Any(pc => pc.Label.Equals(label) || (pc.Alias != null && pc.Alias.Equals(label.Name)));
        }

        /// <summary>
        /// ���������� ���� ���������.
        /// </summary>
        /// <param name="disposing"><c>true</c> - ���� ����� ���������� �������.</param>
        public virtual void Dispose(bool disposing)
        {
            this.Shutdown();
        }

        /// <summary>
        /// ���������� ���� ���������.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// �������� ���������.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� �������� ���������.</param>
        /// <typeparam name="T">��� ������������� ���������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        public Task Emit<T>(string label, T payload, PublishingOptions options = null) where T : class
        {
            return this.Emit(label.ToMessageLabel(), payload, options);
        }

        /// <summary>
        /// �������� ���������.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� �������� ���������.</param>
        /// <typeparam name="T">��� ������������� ���������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        public Task Emit<T>(MessageLabel label, T payload, PublishingOptions options = null) where T : class
        {
            EnsureCanSendUsing(label);

            return this.InternalSend(label, payload, options);
        }

        /// <summary>
        /// �������� ���������.
        /// ����� ��������� ����������� �� ������ ���� ������������� ���������.
        /// </summary>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� �������� ���������.</param>
        /// <typeparam name="T">��� ������������� ���������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        public Task Emit<T>(T payload, PublishingOptions options = null) where T : class
        {
            return this.Emit(this.Configuration.MessageLabelResolver.ResolveFrom<T>(), payload, options);
        }

        /// <summary>
        /// �������� ���������.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="headers">��������� ���������.</param>
        /// <returns>������ �������� ���������.</returns>
        public Task Emit(MessageLabel label, object payload, IDictionary<string, object> headers)
        {
            this.EnsureIsReady();

            return this
                .GetSenderFor(label)
                .Send(label, payload, headers);
        }

        /// <summary>
        /// �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="request">���� �������.</param>
        /// <param name="options">��������� �������� �������.</param>
        /// <param name="responseAction">�������� ���������� ��� ��������� ������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        public void Request<TRequest, TResponse>(MessageLabel label, TRequest request, RequestOptions options, Action<TResponse> responseAction) where TResponse : class where TRequest : class
        {
            EnsureCanSendUsing(label);

            this.InternalRequest<TResponse>(label, request, options)
                .ContinueWith(
                    t =>
                        {
                            if (t.IsFaulted)
                            {
                                if (t.Exception != null)
                                {
                                    throw t.Exception;
                                }

                                throw new Exception();
                            }

                            if (t.IsCanceled)
                            {
                                throw new OperationCanceledException();
                            }

                            responseAction(t.Result);
                        })
                .Wait();
        }

        /// <summary>��������� ���������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        public void Request<TRequest, TResponse>(MessageLabel label, TRequest request, IDictionary<string, object> headers, Action<TResponse> responseAction) where TRequest : class where TResponse : class
        {
            EnsureCanSendUsing(label);

            this.InternalRequest<TResponse>(label, request, headers)
                .ContinueWith(
                    t =>
                    {
                        if (t.IsFaulted)
                        {
                            if (t.Exception != null)
                            {
                                throw t.Exception;
                            }

                            throw new Exception();
                        }

                        if (t.IsCanceled)
                        {
                            throw new OperationCanceledException();
                        }

                        responseAction(t.Result);
                    })
                .Wait();
        }

        /// <summary>
        /// �������� ��������� � ������� ������-�����.
        /// ����� ��������� ����������� �� ������ ���� �������.
        /// </summary>
        /// <param name="request">���� �������.</param>
        /// <param name="options">��������� �������� �������.</param>
        /// <param name="responseAction">�������� ���������� ��� ��������� ������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        public void Request<TRequest, TResponse>(TRequest request, RequestOptions options, Action<TResponse> responseAction) where TRequest : class where TResponse : class
        {
            Request(this.Configuration.MessageLabelResolver.ResolveFrom<TRequest>(), request, options, responseAction);
        }

        /// <summary>
        /// �������� ��������� � ������� ������-�����.
        /// ����� ��������� ����������� �� ������ ���� �������.
        /// </summary>
        /// <param name="request">���� �������.</param>
        /// <param name="responseAction">�������� ���������� ��� ��������� ������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        public void Request<TRequest, TResponse>(TRequest request, Action<TResponse> responseAction) where TRequest : class where TResponse : class
        {
            this.RequestAsync<TRequest, TResponse>(this.Configuration.MessageLabelResolver.ResolveFrom<TRequest>(), request);
        }

        /// <summary>
        /// �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="request">���� �������.</param>
        /// <param name="options">��������� �������� �������.</param>
        /// <param name="responseAction">�������� ���������� ��� ��������� ������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        public void Request<TRequest, TResponse>(string label, TRequest request, RequestOptions options, Action<TResponse> responseAction) where TRequest : class where TResponse : class
        {
            Request(label.ToMessageLabel(), request, options, responseAction);
        }

        /// <summary>
        /// �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="request">���� �������.</param>
        /// <param name="responseAction">�������� ���������� ��� ��������� ������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        public void Request<TRequest, TResponse>(MessageLabel label, TRequest request, Action<TResponse> responseAction) where TRequest : class where TResponse : class
        {
            Request(label, request, (RequestOptions)null, responseAction);
        }

        /// <summary>
        /// �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="request">���� �������.</param>
        /// <param name="responseAction">�������� ���������� ��� ��������� ������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        public void Request<TRequest, TResponse>(string label, TRequest request, Action<TResponse> responseAction) where TResponse : class where TRequest : class
        {
            Request(label.ToMessageLabel(), request, responseAction);
        }

        /// <summary>
        /// ���������� �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="request">���� �������.</param>
        /// <param name="options">��������� �������� �������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        public Task<TResponse> RequestAsync<TRequest, TResponse>(MessageLabel label, TRequest request, RequestOptions options = null) where TResponse : class where TRequest : class
        {
            EnsureCanSendUsing(label);

            return this.InternalRequest<TResponse>(label, request, options);
        }

        /// <summary>��������� ����������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <returns>������ ��������� ��������� ���������.</returns>
        public Task<TResponse> RequestAsync<TRequest, TResponse>(MessageLabel label, TRequest request, IDictionary<string, object> headers) where TRequest : class where TResponse : class
        {
            EnsureCanSendUsing(label);

            return this.InternalRequest<TResponse>(label, request, headers);
        }

        /// <summary>
        /// ���������� �������� ��������� � ������� ������-�����.
        /// ����� ��������� ����������� �� ������ ���� �������.
        /// </summary>
        /// <param name="request">���� �������.</param>
        /// <param name="options">��������� �������� �������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request, RequestOptions options = null) where TRequest : class where TResponse : class
        {
            return this.RequestAsync<TRequest, TResponse>(this.Configuration.MessageLabelResolver.ResolveFrom<TRequest>(), request, options);
        }

        /// <summary>
        /// ���������� �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="request">���� �������.</param>
        /// <param name="options">��������� �������� �������.</param>
        /// <typeparam name="TRequest">��� �������.</typeparam>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        public Task<TResponse> RequestAsync<TRequest, TResponse>(string label, TRequest request, RequestOptions options = null) where TResponse : class where TRequest : class
        {
            return this.RequestAsync<TRequest, TResponse>(label.ToMessageLabel(), request, options);
        }

        /// <summary>
        /// ��������� ������ ���� ���������.
        /// </summary>
        public abstract void Shutdown();

        /// <summary>
        /// ��������� ���� ���������.
        /// </summary>
        /// <param name="waitForReadiness">���� <c>true</c> - ����� ���������� ��������� ���������� ���� ���������.</param>
        public abstract void Start(bool waitForReadiness = true);

        /// <summary>
        /// ������������� ���� ���������.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// ��������� ����������� ��� ��������� ����� ���������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <returns>����������� ��������� � ��������� ������.</returns>
        /// <exception cref="BusConfigurationException">������������ ����������, ���� ��� ����������� ��� ��������� �����.</exception>
        protected ISender GetSenderFor(MessageLabel label)
        {
            ISender sender = this.Senders.FirstOrDefault(s => s.CanRoute(label)) ?? this.Senders.FirstOrDefault(s => s.CanRoute(MessageLabel.Any));

            if (sender == null)
            {
                throw new BusConfigurationException("No sender is configured to send [{0}].".FormatEx(label));
            }

            return sender;
        }

        /// <summary>
        /// ���������� �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="payload">���� �������.</param>
        /// <param name="options">��������� �������� �������.</param>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        protected Task<TResponse> InternalRequest<TResponse>(MessageLabel label, object payload, RequestOptions options) where TResponse : class
        {
            this.EnsureIsReady();

            return this.GetSenderFor(label)
                    .Request<TResponse>(label, payload, options ?? new RequestOptions());
        }

        /// <summary>
        /// ���������� �������� ��������� � ������� ������-�����.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="payload">���� �������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <typeparam name="TResponse">��� ������.</typeparam>
        /// <returns>������ �������� ���������.</returns>
        protected Task<TResponse> InternalRequest<TResponse>(MessageLabel label, object payload, IDictionary<string, object> headers) where TResponse : class
        {
            this.EnsureIsReady();

            return this.GetSenderFor(label)
                    .Request<TResponse>(label, payload, headers ?? new Dictionary<string, object>());
        }

        /// <summary>
        /// ���������� �������� ���������.
        /// ��� �������� ���������� ������� ����� ���������, �� ������ ������� �������� ������� ���������.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� �������� ���������.</param>
        /// <returns>������ �������� ���������.</returns>
        protected Task InternalSend(MessageLabel label, object payload, PublishingOptions options)
        {
            this.EnsureIsReady();

            return this.GetSenderFor(label)
                    .Send(label, payload, options ?? new PublishingOptions());
        }

        /// <summary>
        /// ���������� ������� �� ��������� ����������.
        /// </summary>
        protected virtual void OnConnected()
        {
            this.Connected(this, null);
        }

        /// <summary>
        /// ���������� ������� � ������� ����������.
        /// </summary>
        protected virtual void OnDisconnected()
        {
            this.Disconnected(this, null);
        }

        /// <summary>
        /// ���������� ������� � ������� ���� ���������.
        /// </summary>
        protected virtual void OnStarted()
        {
            this.Started(this, null);

            this.logger.InfoFormat(
                "Started [{0}] with endpoint [{1}].".FormatEx(
                    this.GetType().Name, 
                    this.Endpoint));
        }

        /// <summary>
        /// ���������� ������� � ������ ������� ���� ���������.
        /// </summary>
        protected virtual void OnStarting()
        {
            this.logger.InfoFormat(
                "Starting [{0}] with endpoint [{1}].".FormatEx(
                    this.GetType().Name, 
                    this.Endpoint));

            this.Starting(this, null);
        }

        /// <summary>
        /// ���������� ������� �� ��������� ���� ���������.
        /// </summary>
        protected virtual void OnStopped()
        {
            this.Stopped(this, null);

            this.logger.InfoFormat(
                "Stopped [{0}] with endpoint [{1}].".FormatEx(
                    this.GetType().Name, 
                    this.Endpoint));
        }

        /// <summary>
        /// ���������� ������� � ������ ��������� ���� ���������.
        /// </summary>
        protected virtual void OnStopping()
        {
            this.logger.InfoFormat(
                "Stopping [{0}] with endpoint [{1}].".FormatEx(
                    this.GetType().Name, 
                    this.Endpoint));

            this.IsStarted = false;

            this.Stopping(this, null);
        }

        /// <summary>
        /// ������������� � ������ ��������� ���� ���������.
        /// </summary>
        /// <param name="waitForReadiness"><c>true</c> - ���� ����� ��������� ���������� ����.</param>
        protected abstract void Restart(bool waitForReadiness = true);

        /// <summary>
        /// ��������� ����������� ��������� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <exception cref="InvalidOperationException">������������ ���� ������ ���������� ��������� ����� ���������.</exception>
        // ReSharper disable UnusedParameter.Local
        private static void EnsureCanSendUsing(MessageLabel label)
            // ReSharper restore UnusedParameter.Local
        {
            if (label.IsAny)
            {
                throw new InvalidOperationException("Can't send using Any label.");
            }

            if (label.IsEmpty)
            {
                throw new InvalidOperationException("Can't send using Empty label.");
            }
        }

        /// <summary>
        /// ���������, ��� ���� ��������� ������ � ������.
        /// </summary>
        /// <exception cref="BusNotReadyException">���� ���� �� ������ � ������, ������������ ����������.</exception>
        private void EnsureIsReady()
        {
            if (!this.WhenReady.WaitOne(TimeSpan.FromSeconds(5)))
            {
                throw new BusNotReadyException();
            }
        }
    }
}
