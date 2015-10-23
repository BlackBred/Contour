using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Logging;

using Contour.Filters;
using Contour.Sending;

namespace Contour.Transport.RabbitMQ.Internal
{
    /// <summary>
    /// ����������� ��������� � ������� ������� <c>RabbitMQ</c>.
    /// </summary>
    internal class RabbitSender : AbstractSender
    {
        /// <summary>
        /// ������ ���������� �����������.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ������ ����������� ���������.
        /// </summary>
        private readonly ProducerRegistry producerRegistry;

        /// <summary>
        /// ��������� ���������.
        /// </summary>
        private Producer producer;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="RabbitSender"/>.
        /// </summary>
        /// <param name="endpoint">�������� �����, ��� ������� ��������� �����������.</param>
        /// <param name="configuration">������������ ����������� ���������.</param>
        /// <param name="producerRegistry">������ ����������� ���������.</param>
        /// <param name="filters">������� ���������.</param>
        public RabbitSender(IEndpoint endpoint, ISenderConfiguration configuration, ProducerRegistry producerRegistry, IEnumerable<IMessageExchangeFilter> filters)
            : base(endpoint, configuration, filters)
        {
            this.producerRegistry = producerRegistry;
        }

        /// <summary>
        /// ���� <c>true</c> - �������, ����� <c>false</c>.
        /// </summary>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// ���� <c>true</c> - ����������� �������� ��� �����, ����� <c>false</c>.
        /// </summary>
        public override bool IsHealthy
        {
            get
            {
                return !producer.HasFailed;
            }
        }

        /// <summary>
        /// ����������� ������� �������. � ������������� �����������.
        /// </summary>
        public override void Dispose()
        {
            Logger.Trace(m => m("Disposing sender of [{0}].", this.Configuration.Label));
            this.Stop();
        }

        /// <summary>
        /// ��������� �����������.
        /// </summary>
        public override void Start()
        {
            Logger.Trace(m => m("Starting sender of [{0}].", this.Configuration.Label));

            if (this.IsStarted)
            {
                return;
            }

            this.IsStarted = true;

            this.EnsureProducerIsReady();
        }

        /// <summary>
        /// ������������� �����������.
        /// </summary>
        public override void Stop()
        {
            Logger.Trace(m => m("Stopping sender of [{0}].", this.Configuration.Label));
            this.IsStarted = false;

            this.UnbindProducer();
        }

        /// <summary>
        /// ��������� �������� ���������.
        /// </summary>
        /// <param name="exchange">���������� �� ��������.</param>
        /// <returns>������ �������� �������� ���������.</returns>
        protected override Task<MessageExchange> InternalSend(MessageExchange exchange)
        {
            this.EnsureProducerIsReady();

            if (exchange.IsRequest)
            {
                return this.producer.Request(exchange.Out, exchange.ExpectedResponseType)
                    .ContinueWith(
                        t =>
                            {
                                if (t.IsFaulted)
                                {
                                    exchange.Exception = t.Exception;
                                }
                                else
                                {
                                    exchange.In = t.Result;
                                }

                                return exchange;
                            });
            }

            return this.producer.Publish(exchange.Out)
                .ContinueWith(
                    t =>
                        {
                            if (t.IsFaulted)
                            {
                                exchange.Exception = t.Exception;
                            }

                            return exchange;
                        });
        }

        /// <summary>
        /// �����������, ��� ��������� ��������� �������.
        /// </summary>
        private void EnsureProducerIsReady()
        {
            if (this.producer == null)
            {
                Logger.Trace(m => m("Resolving producer for sender of [{0}].", this.Configuration.Label));
                this.producer = this.producerRegistry.ResolveFor(this.Configuration);
                this.producer.Start();
            }
        }

        /// <summary>
        /// ������������ �� ���������� ���������.
        /// </summary>
        private void UnbindProducer()
        {
            Logger.Trace(m => m("Unbinding producer from sender of [{0}].", this.Configuration.Label));
            this.producer = null;
        }
    }
}
