using System.Threading.Tasks;

using Contour.Filters;
using Contour.Helpers;

namespace Contour.Caching
{
    /// <summary>
    /// ������, ������� �������� �������� ���������.
    /// </summary>
    public class CacheMessageExchangeFilter : IMessageExchangeFilter
    {
        /// <summary>
        /// ��������� ����.
        /// </summary>
        private readonly ICacheProvider cacheProvider;

        /// <summary>
        /// ��������� ��� ��������.
        /// </summary>
        private readonly Hasher hasher = new Hasher();

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="CacheMessageExchangeFilter"/>.
        /// </summary>
        /// <param name="cacheProvider">��������� ����.</param>
        public CacheMessageExchangeFilter(ICacheProvider cacheProvider)
        {
            this.cacheProvider = cacheProvider;
        }

        /// <summary>
        /// ���� � ����, ���� �������� ��������� �� �������� ���������, ����� ���������� ������ �� ����.
        /// </summary>
        /// <param name="exchange">�������� ��������� ���������.</param>
        /// <param name="invoker">������� ���������� ��������.</param>
        /// <returns>������ ��������� ���������.</returns>
        public Task<MessageExchange> Process(MessageExchange exchange, MessageExchangeFilterInvoker invoker)
        {
            if (!exchange.IsIncompleteRequest)
            {
                return invoker.Continue(exchange);
            }

            string hash = this.hasher.CalculateHashOf(exchange.Out).ToString();
            Maybe<object> cached = this.cacheProvider.Find<object>(hash);
            if (cached.HasValue)
            {
                exchange.In = new Message(MessageLabel.Empty, cached.Value);
                return Filter.Result(exchange);
            }

            return invoker.Continue(exchange)
                .ContinueWith(
                    t =>
                        {
                            MessageExchange resultExchange = t.Result;
                            if (!resultExchange.IsCompleteRequest)
                            {
                                return resultExchange;
                            }

                            string expiresHeader = Headers.GetString(resultExchange.In.Headers, Headers.Expires);
                            if (!string.IsNullOrEmpty(expiresHeader))
                            {
                                Expires expiration = Expires.Parse(expiresHeader);

                                if (expiration.Period.HasValue)
                                {
                                    this.cacheProvider.Put(hash, resultExchange.In.Payload, expiration.Period.Value);
                                }
                                else
                                {
                                    this.cacheProvider.Put(hash, resultExchange.In.Payload, expiration.Date.Value);
                                }
                            }

                            return resultExchange;
                        });
        }
    }
}
