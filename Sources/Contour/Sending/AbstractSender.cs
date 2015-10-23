using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Logging;

using Contour.Configuration;
using Contour.Filters;
using Contour.Helpers;

namespace Contour.Sending
{
    /// <summary>
    /// �����������, ������� �� ����� � ������������ ������.
    /// </summary>
    internal abstract class AbstractSender : ISender
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ������� ��������� ���������.
        /// </summary>
        private readonly IList<IMessageExchangeFilter> filters;

        /// <summary>
        /// �������� �����, �� ����� ������� �������� �����������.
        /// </summary>
        private readonly IEndpoint endpoint;

        /// <summary>
        /// ��������� ����� � ���� ���������.
        /// </summary>
        private readonly string breadCrumbsTail;

        // TODO: refactor, don't copy filters

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="AbstractSender"/>.
        /// </summary>
        /// <param name="endpoint">�������� �����, �� ����� ������� �������� �����������.</param>
        /// <param name="configuration">������������ �����������.</param>
        /// <param name="filters">������ �������� ��������� ���������.</param>
        protected AbstractSender(IEndpoint endpoint, ISenderConfiguration configuration, IEnumerable<IMessageExchangeFilter> filters)
        {
            this.endpoint = endpoint;
            this.breadCrumbsTail = ";" + endpoint.Address;

            this.filters = new SendingExchangeFilter(this.InternalSend)
                .ToEnumerable()
                .Union(filters)
                .ToList();

            this.Configuration = configuration;
        }

        /// <summary>
        /// ������������ �����������.
        /// </summary>
        public ISenderConfiguration Configuration { get; private set; }

        /// <summary>
        /// ���� �� ���� � ������ �����������.
        /// </summary>
        public abstract bool IsHealthy { get; }

        /// <summary>
        /// ��������� ����������� ������� ������� ��� ����� ���������.
        /// </summary>
        /// <param name="label">����� ���������, ��� ������� ����� ������� �������.</param>
        /// <returns><c>true</c> - ���� ����� ������� �������.</returns>
        public virtual bool CanRoute(MessageLabel label)
        {
            return label.IsAlias ? label.Name.Equals(this.Configuration.Alias) : label.Equals(this.Configuration.Label);
        }

        /// <summary>
        /// ����������� �������.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="payload">��������� �������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        public Task<T> Request<T>(object payload, IDictionary<string, object> headers) where T : class
        {
            var message = new Message(this.Configuration.Label, headers, payload);

            var exchange = new MessageExchange(message, typeof(T));
            var invoker = new MessageExchangeFilterInvoker(this.filters);

            return invoker.Process(exchange)
                .ContinueWith(
                    t =>
                        {
                            t.Result.ThrowIfFailed();
                            return (T)t.Result.In.Payload;
                        });
        }

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="payload">��������� �������.</param>
        /// <param name="options">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        public Task<T> Request<T>(object payload, RequestOptions options) where T : class
        {
            var headers = this.ApplyOptions(options);
            if (!headers.ContainsKey(Headers.CorrelationId))
            {
                headers[Headers.CorrelationId] = Guid.NewGuid().ToString("n");
            }

            return this.Request<T>(payload, headers);
        }

            /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="payload">���� ���������.</param>
        /// <param name="headers">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        [Obsolete("���������� ������������ ����� Send � ��������� ����� ���������.")]
        public Task Send(object payload, IDictionary<string, object> headers)
        {
            var message = new Message(this.Configuration.Label, headers, payload);

            return this.ProcessFilter(message);
        }

        /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        [Obsolete("���������� ������������ ����� Send � ��������� ����� ���������.")]
        public Task Send(object payload, PublishingOptions options)
        {
            return this.Send(payload, this.ApplyOptions(options));
        }

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="payload">��������� �������.</param>
        /// <param name="options">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        public Task<T> Request<T>(MessageLabel label, object payload, RequestOptions options) where T : class
        {
            var headers = this.ApplyOptions(options);

            return this.Request<T>(label, payload, headers);
        }

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="payload">��������� �������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        public Task<T> Request<T>(MessageLabel label, object payload, IDictionary<string, object> headers) where T : class
        {
            if (!headers.ContainsKey(Headers.CorrelationId))
            {
                headers[Headers.CorrelationId] = Guid.NewGuid().ToString("n");
            }

            var message = new Message(this.Configuration.Label.Equals(MessageLabel.Any) ? label : this.Configuration.Label, headers, payload);

            var exchange = new MessageExchange(message, typeof(T));
            var invoker = new MessageExchangeFilterInvoker(this.filters);

            return invoker.Process(exchange)
                .ContinueWith(
                    t =>
                    {
                        t.Result.ThrowIfFailed();
                        return (T)t.Result.In.Payload;
                    });
        }

        /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="headers">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        public Task Send(MessageLabel label, object payload, IDictionary<string, object> headers)
        {
            var message = new Message(this.Configuration.Label.Equals(MessageLabel.Any) ? label : this.Configuration.Label, headers, payload);

            return this.ProcessFilter(message);
        }

        /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        public Task Send(MessageLabel label, object payload, PublishingOptions options)
        {
            return this.Send(label, payload, this.ApplyOptions(options));
        }

        /// <summary>
        /// ��������� �����������.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// ������������� �����������.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// ������ ��������� ���������, ������� �������� ���������.
        /// </summary>
        /// <param name="exchange">���������� ���������.</param>
        /// <returns>������ ���������� �������.</returns>
        protected abstract Task<MessageExchange> InternalSend(MessageExchange exchange);

        /// <summary>
        /// ������������ ��������� � ������� ������������������ ��������.
        /// </summary>
        /// <param name="message">�������������� ���������.</param>
        /// <returns>������ ��������� ��������� � ������� ��������.</returns>
        private Task ProcessFilter(IMessage message)
        {
            var exchange = new MessageExchange(message, null);
            var invoker = new MessageExchangeFilterInvoker(this.filters);

            return invoker.Process(exchange);
        }

        /// <summary>
        /// ������������ ��������� ���������� ��������� � ��������� ���������.
        /// </summary>
        /// <param name="options">��������� ���������� ���������.</param>
        /// <returns>��������� ���������.</returns>
        private IDictionary<string, object> ApplyOptions(PublishingOptions options)
        {
            var storage = this.Configuration.Options.GetIncomingMessageHeaderStorage().Value;
            var inputHeaders = storage.Load() ?? new Dictionary<string, object>();
            var outputHeaders = new Dictionary<string, object>(inputHeaders);

            Headers.ApplyBreadcrumbs(outputHeaders, this.endpoint.Address);
            Headers.ApplyOriginalMessageId(outputHeaders);

            Logger.Trace(m => m("������������� ������� ��������� [{0}].", Headers.GetString(outputHeaders, Headers.OriginalMessageId)));

            Maybe<bool> persist = BusOptions.Pick(options.Persistently, this.Configuration.Options.IsPersistently());
            Headers.ApplyPersistently(outputHeaders, persist);

            Maybe<TimeSpan?> ttl = BusOptions.Pick(options.Ttl, this.Configuration.Options.GetTtl());
            Headers.ApplyTtl(outputHeaders, ttl);

            return outputHeaders;
        }

        /// <summary>
        /// ������������ ��������� ���������� ��������� � ��������� ���������.
        /// </summary>
        /// <param name="requestOptions">��������� ���������� ���������.</param>
        /// <returns>��������� ���������.</returns>
        private IDictionary<string, object> ApplyOptions(RequestOptions requestOptions)
        {
            IDictionary<string, object> headers = this.ApplyOptions(requestOptions as PublishingOptions);

            Maybe<TimeSpan?> timeout = BusOptions.Pick(requestOptions.Timeout, this.Configuration.Options.GetRequestTimeout());
            if (timeout != null && timeout.HasValue)
            {
                headers[Headers.Timeout] = timeout.Value;
            }

            return headers;
        }
    }
}
