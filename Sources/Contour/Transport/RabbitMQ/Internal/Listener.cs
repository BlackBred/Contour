using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;

using Contour.Helpers;
using Contour.Helpers.Scheduler;
using Contour.Helpers.Timing;
using Contour.Receiving;
using Contour.Receiving.Consumers;
using Contour.Validation;

using global::RabbitMQ.Client.Events;

namespace Contour.Transport.RabbitMQ.Internal
{
    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal class Listener : IDisposable
    {
        /// <summary>
        /// ��������� �������.
        /// </summary>
        private readonly IChannelProvider channelProvider;

        /// <summary>
        /// ����������� ���������.
        /// ������� ����������� ������������� ���� ����� ���������.
        /// </summary>
        private readonly IDictionary<MessageLabel, ConsumingAction> consumers = new Dictionary<MessageLabel, ConsumingAction>();

        /// <summary>
        /// ���� ������, �� ������� �������� ���������.
        /// </summary>
        private readonly ISubscriptionEndpoint endpoint;

        /// <summary>
        /// �������� �������� ��������� �� ������.
        /// </summary>
        private readonly IDictionary<string, Expectation> expectations = new Dictionary<string, Expectation>();

        /// <summary>
        /// ��������� ���������� ��������� ���������.
        /// </summary>
        private readonly IIncomingMessageHeaderStorage messageHeaderStorage;

        /// <summary>
        /// ������ �������������.
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// ������ ������.
        /// </summary>
        private readonly ILog logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ������ ���������� �������� ���������.
        /// </summary>
        private readonly MessageValidatorRegistry validatorRegistry;

        /// <summary>
        /// �������� ������� ������ �����.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// �������: ��������� ���������� ���������.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private bool isConsuming;

        /// <summary>
        /// ������, ������� �����������, ��� ����� �������� ������ �� ������ �����.
        /// </summary>
        private ITicketTimer ticketTimer;

        /// <summary>
        /// ������� ������ ����������� ��������� ���������.
        /// </summary>
        private IList<IWorker> workers;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Listener"/>.
        /// </summary>
        /// <param name="channelProvider">
        /// ��������� �������.
        /// </param>
        /// <param name="endpoint">
        /// �������������� ����.
        /// </param>
        /// <param name="receiverOptions">
        /// ��������� ����������.
        /// </param>
        /// <param name="validatorRegistry">
        /// ������ ���������� �������� ���������.
        /// </param>
        public Listener(IChannelProvider channelProvider, ISubscriptionEndpoint endpoint, RabbitReceiverOptions receiverOptions, MessageValidatorRegistry validatorRegistry)
        {
            this.endpoint = endpoint;
            this.channelProvider = channelProvider;
            this.validatorRegistry = validatorRegistry;

            this.ReceiverOptions = receiverOptions;
            this.ReceiverOptions.GetIncomingMessageHeaderStorage();
            this.messageHeaderStorage = this.ReceiverOptions.GetIncomingMessageHeaderStorage().Value;

            // TODO: refactor
            this.Failed += _ =>
                {
                    if (HasFailed)
                    {
                        return;
                    }

                    this.HasFailed = true;
                    ((IBusAdvanced)channelProvider).Panic();
                }; // restarting the whole bus
        }

        /// <summary>
        /// ��� ����������� ���������.
        /// </summary>
        /// <param name="delivery">
        /// �������� ���������.
        /// </param>
        internal delegate void ConsumingAction(RabbitDelivery delivery);

        /// <summary>
        /// ������� � ���� �� ����� ������������� �����.
        /// </summary>
        public event Action<Listener> Failed = l => { };

        /// <summary>
        /// ����� ���������, ������� ����� ���������� ���������.
        /// </summary>
        public IEnumerable<MessageLabel> AcceptedLabels
        {
            get
            {
                return this.consumers.Keys;
            }
        }

        /// <summary>
        /// ���� ������, ������� �������������� �� �������� ���������.
        /// </summary>
        public ISubscriptionEndpoint Endpoint
        {
            get
            {
                return this.endpoint;
            }
        }

        /// <summary>
        /// ��������� ����������.
        /// </summary>
        public RabbitReceiverOptions ReceiverOptions { get; private set; }

        public bool HasFailed { get; private set; }

        /// <summary>
        /// ������� �������� ���������.
        /// </summary>
        /// <param name="channel">
        /// �����, �� �������� �������� ���������.
        /// </param>
        /// <param name="args">
        /// ���������, � �������� �������� ���������.
        /// </param>
        /// <returns>
        /// �������� ���������.
        /// </returns>
        public RabbitDelivery BuildDeliveryFrom(RabbitChannel channel, BasicDeliverEventArgs args)
        {
            return new RabbitDelivery(channel, args, this.ReceiverOptions.IsAcceptRequired());
        }

        /// <summary>
        /// ����������� �������.
        /// </summary>
        public void Dispose()
        {
            this.StopConsuming();
        }

        /// <summary>
        /// ���������� ������, � ��������� ������ �� ������.
        /// </summary>
        /// <param name="correlationId">
        /// �������������� �������������, � ������� �������� ������������ �������������� ������ ������������� �������.
        /// </param>
        /// <param name="expectedResponseType">
        /// ��������� ��� ������.
        /// </param>
        /// <param name="timeout">
        /// ����� �������� ������.
        /// </param>
        /// <returns>
        /// ������ �������� ������ �� ������.
        /// </returns>
        public Task<IMessage> Expect(string correlationId, Type expectedResponseType, TimeSpan? timeout)
        {
            Expectation expectation;
            long? timeoutTicket = null;

            lock (this.locker)
            {
                if (!this.expectations.TryGetValue(correlationId, out expectation))
                {
                    // TODO: refactor
                    if (timeout.HasValue)
                    {
                        timeoutTicket = this.ticketTimer.Acquire(
                            timeout.Value, 
                            () =>
                                {
                                    Expectation ex;
                                    if (this.expectations.TryGetValue(correlationId, out ex))
                                    {
                                        this.expectations.Remove(correlationId);
                                        ex.Timeout();
                                    }
                                });
                    }

                    expectation = new Expectation(d => this.BuildResponse(d, expectedResponseType), timeoutTicket);
                    this.expectations[correlationId] = expectation;
                }
            }

            return expectation.Task;
        }

        /// <summary>
        /// ��������� ��� ������ ����������� ���������.
        /// </summary>
        /// <param name="label">
        /// ����� ���������, ������� ����� ���� ����������.
        /// </param>
        /// <param name="consumer">
        /// ���������� ���������.
        /// </param>
        /// <param name="validator">
        /// �������� �������� ��������� ���������.
        /// </param>
        /// <typeparam name="T">
        /// ��� ��������� ���������.
        /// </typeparam>
        public void RegisterConsumer<T>(MessageLabel label, IConsumerOf<T> consumer, IMessageValidator validator) where T : class
        {
            ConsumingAction consumingAction = delivery =>
                {
                    IConsumingContext<T> context = delivery.BuildConsumingContext<T>(label);

                    if (validator != null)
                    {
                        validator.Validate(context.Message).ThrowIfBroken();
                    }
                    else
                    {
                        this.validatorRegistry.Validate(context.Message);
                    }

                    consumer.Handle(context);
                };

            this.consumers[label] = consumingAction;
        }

        /// <summary>
        /// ��������� ��������� �������� ���������.
        /// </summary>
        public void StartConsuming()
        {
            if (this.isConsuming)
            {
                return;
            }

            this.logger.InfoFormat("Starting consuming on [{0}].", this.endpoint.ListeningSource);

            this.cancellationTokenSource = new CancellationTokenSource();
            this.ticketTimer = new RoughTicketTimer(TimeSpan.FromSeconds(1));

            this.workers = Enumerable.Range(
                0, 
                (int)this.ReceiverOptions.GetParallelismLevel().Value)
                .Select(_ => ThreadWorker.StartNew(this.Consume, this.cancellationTokenSource.Token))
                .ToList();

            this.isConsuming = true;
        }

        /// <summary>
        /// ������������� ��������� �������� ���������.
        /// </summary>
        public void StopConsuming()
        {
            // TODO: make more reliable
            lock (this.locker)
            {
                if (!this.isConsuming)
                {
                    return;
                }

                this.isConsuming = false;

                this.logger.InfoFormat("Stopping consuming on [{0}].", this.endpoint.ListeningSource);

                this.cancellationTokenSource.Cancel();

                WaitHandle.WaitAll(this.workers.Select(w => w.CompletionHandle).ToArray());
                this.workers.ForEach(w => w.Dispose());
                this.workers.Clear();

                this.ticketTimer.Dispose();
                this.expectations.Values.ForEach(e => e.Cancel());
            }
        }

        /// <summary>
        /// ��������� ������������ ��������� ��������� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">
        /// ����� ���������.
        /// </param>
        /// <returns>
        /// ���� <c>true</c> - ��������� ������������ ��������� ���������, ����� - <c>false</c>.
        /// </returns>
        public bool Supports(MessageLabel label)
        {
            return this.AcceptedLabels.Contains(label);
        }

        /// <summary>
        /// ���������� ��������� �� �����������.
        /// </summary>
        /// <param name="delivery">
        /// �������� ���������.
        /// </param>
        protected void Deliver(RabbitDelivery delivery)
        {
            this.logger.Trace(m => m("Received delivery labeled [{0}] from [{1}] with consumer [{2}].", delivery.Label, delivery.Args.Exchange, delivery.Args.ConsumerTag));

            if (delivery.Headers.ContainsKey(Headers.OriginalMessageId))
            {
                this.logger.Trace(m => m("�������� ������������� ��������� [{0}].", Headers.GetString(delivery.Headers, Headers.OriginalMessageId)));
            }

            if (delivery.Headers.ContainsKey(Headers.Breadcrumbs))
            {
                this.logger.Trace(m => m("��������� ���� ���������� � �������� ������: [{0}].", Headers.GetString(delivery.Headers, Headers.Breadcrumbs)));
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                // TODO: refactor
                bool processed = this.TryHandleAsResponse(delivery);

                if (!processed)
                {
                    processed = this.TryHandleAsSubscription(delivery);
                }

                if (!processed)
                {
                    this.OnUnhandled(delivery);
                }
            }
            catch (Exception ex)
            {
                this.OnFailure(delivery, ex);
            }

            stopwatch.Stop();
            this.logger.Trace(m => m("Message labeled [{0}] processed in {1} ms.", delivery.Label, stopwatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// ��������� ����� �� ������.
        /// </summary>
        /// <param name="delivery">
        /// �������� ���������, ������� �������� �������.
        /// </param>
        /// <param name="responseType">
        /// ��� ��������� ���������.
        /// </param>
        /// <returns>
        /// ��������� � ������� �� ������.
        /// </returns>
        private IMessage BuildResponse(IDelivery delivery, Type responseType)
        {
            IMessage response = delivery.UnpackAs(responseType);

            this.validatorRegistry.Validate(response);

            return response;
        }

        /// <summary>
        /// ������������ ���������.
        /// </summary>
        /// <param name="cancellationToken">
        /// ���������� ������ ���������� ���������� ���������� ���������.
        /// </param>
        private void Consume(CancellationToken cancellationToken)
        {
            try
            {
                this.InternalConsume(cancellationToken);
            }
            catch (EndOfStreamException ex)
            {
                // The consumer was cancelled, the model closed, or the
                // connection went away.
                this.logger.DebugFormat("Reached EOS while listening on [{0}]. Stopping consuming.", ex, this.endpoint.ListeningSource);
                this.Failed(this);
            }
            catch (OperationCanceledException)
            {
                // do nothing, everything is fine
            }
            catch (Exception ex)
            {
                this.logger.ErrorFormat("Unexpected exception while listening on [{0}]. Stopping consuming.", ex, this.endpoint.ListeningSource);
                this.Failed(this);
            }
        }

        private void InternalConsume(CancellationToken cancellationToken)
        {
            var channel = (RabbitChannel)this.channelProvider.OpenChannel();
            channel.Failed += (ch, args) => this.Failed(this);

            if (this.ReceiverOptions.GetQoS().HasValue)
            {
                channel.SetQos(
                    this.ReceiverOptions.GetQoS().Value);
            }

            CancellableQueueingConsumer consumer = channel.BuildCancellableConsumer(cancellationToken);
            channel.StartConsuming(this.endpoint.ListeningSource, this.ReceiverOptions.IsAcceptRequired(), consumer);

            consumer.ConsumerCancelled += (sender, args) =>
            {
                this.logger.InfoFormat("Consumer [{0}] was cancelled.", args.ConsumerTag);
                this.Failed(this);
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                BasicDeliverEventArgs message = consumer.Dequeue();
                this.Deliver(this.BuildDeliveryFrom(channel, message));
            }

            // TODO: resolve deadlock
            // channel.TryStopConsuming(consumerTag);
        }

        /// <summary>
        /// ���������� ������� � ���� ��������� ���������.
        /// </summary>
        /// <param name="delivery">
        /// ���������, ��������� �������� ������� � ����.
        /// </param>
        /// <param name="exception">
        /// ���������� ��������������� �� ����� ����.
        /// </param>
        private void OnFailure(RabbitDelivery delivery, Exception exception)
        {
            this.logger.Warn(m => m("Failed to process message labeled [{0}] on queue [{1}].", delivery.Label, this.endpoint.ListeningSource), exception);

            this.ReceiverOptions.GetFailedDeliveryStrategy()
                .Value.Handle(new RabbitFailedConsumingContext(delivery, exception));
        }

        /// <summary>
        /// ���������� ������� � ����������� ����������� ���������.
        /// </summary>
        /// <param name="delivery">
        /// ���������, ��� �������� �� ������ ����������.
        /// </param>
        private void OnUnhandled(RabbitDelivery delivery)
        {
            this.logger.Warn(m => m("No handler for message labeled [{0}] on queue [{1}].", delivery.Label, this.endpoint.ListeningSource));

            this.ReceiverOptions.GetUnhandledDeliveryStrategy()
                .Value.Handle(new RabbitUnhandledConsumingContext(delivery));
        }

        /// <summary>
        /// �������� ���������� ��������� ��� ������.
        /// </summary>
        /// <param name="delivery">
        /// �������� ���������.
        /// </param>
        /// <returns>
        /// ���� <c>true</c> - ����� ��������� ���������� ��� ������, ����� - <c>false</c>.
        /// </returns>
        private bool TryHandleAsResponse(RabbitDelivery delivery)
        {
            if (!delivery.IsResponse)
            {
                return false;
            }

            string correlationId = delivery.CorrelationId;

            lock (this.locker)
            {
                if (this.expectations.ContainsKey(correlationId))
                {
                    Expectation e = this.expectations[correlationId];

                    if (e.TimeoutTicket.HasValue)
                    {
                        this.ticketTimer.Cancel(e.TimeoutTicket.Value);
                    }

                    this.expectations.Remove(correlationId);
                    e.Complete(delivery);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// �������� ���������� ��������� ��� �������������.
        /// </summary>
        /// <param name="delivery">
        /// �������� ���������.
        /// </param>
        /// <returns>
        /// ���� <c>true</c> - �������� ��������� ����������, ����� <c>false</c>.
        /// </returns>
        private bool TryHandleAsSubscription(RabbitDelivery delivery)
        {
            ConsumingAction consumingAction;
            if (!this.consumers.TryGetValue(delivery.Label, out consumingAction))
            {
                // NOTE: this is needed for compatibility with v1 of ServiceBus
                if (this.consumers.Count == 1)
                {
                    consumingAction = this.consumers.Values.Single();
                }

                if (consumingAction == null)
                {
                    this.consumers.TryGetValue(MessageLabel.Any, out consumingAction);
                }
            }

            if (consumingAction != null)
            {
                this.messageHeaderStorage.Store(delivery.Headers);
                consumingAction(delivery);
                return true;
            }

            return false;
        }

        /// <summary>
        /// �������� ������ �� ������.
        /// </summary>
        internal class Expectation
        {
            /// <summary>
            /// �������� ���������� �������� �� ��������� ���������� ������.
            /// </summary>
            private readonly TaskCompletionSource<IMessage> completionSource;

            /// <summary>
            /// ����������� ������.
            /// </summary>
            private readonly Func<IDelivery, IMessage> responseBuilderFunc;

            /// <summary>
            /// ���������� ��� ������ ������������ �������� ������.
            /// </summary>
            private readonly Stopwatch completionStopwatch;

            /// <summary>
            /// �������������� ����� ��������� ������ <see cref="Expectation"/>.
            /// </summary>
            /// <param name="responseBuilderFunc">
            /// ����������� ������.
            /// </param>
            /// <param name="timeoutTicket">
            /// ������ �� ����� ������� �������� ������.
            /// </param>
            public Expectation(Func<IDelivery, IMessage> responseBuilderFunc, long? timeoutTicket)
            {
                this.responseBuilderFunc = responseBuilderFunc;
                this.TimeoutTicket = timeoutTicket;

                this.completionSource = new TaskCompletionSource<IMessage>();
                this.completionStopwatch = Stopwatch.StartNew();
            }

            /// <summary>
            /// ������ ���������� ��������.
            /// </summary>
            public Task<IMessage> Task
            {
                get
                {
                    return this.completionSource.Task;
                }
            }

            /// <summary>
            /// ������ �� ����� ������� �������� ������.
            /// </summary>
            public long? TimeoutTicket { get; private set; }

            /// <summary>
            /// �������� �������� ������.
            /// </summary>
            public void Cancel()
            {
                this.completionSource.TrySetException(new OperationCanceledException());
            }

            /// <summary>
            /// ��������� ��������� ������ �� ������.
            /// </summary>
            /// <param name="delivery">
            /// �������� ��������� - ����� �� ������.
            /// </param>
            public void Complete(RabbitDelivery delivery)
            {
                try
                {
                    this.completionStopwatch.Stop();
                    IMessage response = this.responseBuilderFunc(delivery);
                    this.completionSource.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    this.completionSource.TrySetException(ex);
                    throw;
                }
            }

            /// <summary>
            /// �������������, ��� ��� �������� ����� �����, �� ������� ������ ��� ���� ������� �����.�
            /// </summary>
            public void Timeout()
            {
                this.completionSource.TrySetException(new TimeoutException());
            }
        }
    }
}
