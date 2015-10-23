using Contour.Receiving;
using Contour.Topology;
using Contour.Transport.RabbitMQ.Internal;

namespace Contour.Transport.RabbitMQ.Topology
{
    /// <summary>
    /// ����������� ���������.
    /// </summary>
    public class TopologyBuilder : ITopologyBuilder
    {
        /// <summary>
        /// ���������� � ����� ���������, ����� ������� ������������� ���������.
        /// </summary>
        private readonly RabbitChannel rabbitChannel;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="TopologyBuilder"/>.
        /// </summary>
        /// <param name="channel">
        /// ���������� � ����� ���������, ����� ������� ������������� ���������.
        /// </param>
        public TopologyBuilder(IChannel channel)
        {
            // TODO: ����� �������� � ����� ����� ����� IChannel.
            this.rabbitChannel = (RabbitChannel)channel;
        }

        /// <summary>
        /// ����������� ����� ������ (�������������) � ��������.
        /// </summary>
        /// <param name="exchange">
        /// ����� ������ (�������������) ���������, �� ������� ��������� ���������. ����� ������ ��������� �� ������ ����� ������������� <paramref name="routingKey"/>.
        /// </param>
        /// <param name="queue">
        /// �������, � ������� ����� ��������� ��������� �� ��������������.
        /// </param>
        /// <param name="routingKey">
        /// ���� �������������, ������������ ��� ����������� �������, � ������� ������ ���� ���������� ���������.
        /// </param>
        public void Bind(Exchange exchange, Queue queue, string routingKey = "")
        {
            this.rabbitChannel.Bind(queue, exchange, routingKey);
        }

        /// <summary>
        /// ������� ��������� �������� ����� �������� �� ���������.
        /// ������ ������������ ��� ����������� �������� ������������: ������-�����.
        /// </summary>
        /// <returns>
        /// �������� ����� �������� �� ��������� <see cref="ISubscriptionEndpoint"/>.
        /// </returns>
        public ISubscriptionEndpoint BuildTempReplyEndpoint()
        {
            var queue = new Queue(this.rabbitChannel.DeclareDefaultQueue());

            return new SubscriptionEndpoint(queue, new StaticRouteResolver(string.Empty, queue.Name));
        }

        /// <summary>
        /// ������� ��������� �������� ����� ��� ��������� ���������.
        /// </summary>
        /// <param name="endpoint">�������� ����� ���� ��������� ��� ������� ��������� ��������.</param>
        /// <param name="label">����� ���������, �� ������� ��������� ��������� ������.</param>
        /// <returns>
        /// �������� ����� �������� ��� ��������� ���������.
        /// </returns>
        public ISubscriptionEndpoint BuildTempReplyEndpoint(IEndpoint endpoint, MessageLabel label)
        {
            var queue = Queue.Named(string.Format("{0}.replies-{1}-{2}", endpoint.Address, label.IsAny ? "any" : label.Name, NameGenerator.GetRandomName(8)))
                .AutoDelete.Exclusive.Instance;

            this.rabbitChannel.Declare(queue);

            return new SubscriptionEndpoint(queue, new StaticRouteResolver(string.Empty, queue.Name));
        }

        /// <summary>
        /// ������� ����� ������ (�������������), �� ������� ��������� ��������� � �������.
        /// </summary>
        /// <param name="builder">
        /// ����������� ����� ������ (�������������).
        /// </param>
        /// <returns>
        /// ����� ������ (�������������) <see cref="Exchange"/>.
        /// </returns>
        public Exchange Declare(ExchangeBuilder builder)
        {
            Exchange exchange = builder.Instance;

            this.rabbitChannel.Declare(exchange);

            return exchange;
        }

        /// <summary>
        /// ������� ������� � ������� ��� ���� ��������� ������� � ���������.
        /// </summary>
        /// <param name="builder">
        /// ����������� �������.
        /// </param>
        /// <returns>
        /// ������� ��������� <see cref="Queue"/> ��� ���������.
        /// </returns>
        public Queue Declare(QueueBuilder builder)
        {
            Queue queue = builder.Instance;

            this.rabbitChannel.Declare(queue);

            return queue;
        }
    }
}
