using Contour.Sending;
using Contour.Topology;

namespace Contour.Receiving
{
    /// <summary>
    /// ����������� �������� ����� �������� �� �������� ���������.
    /// </summary>
    public class SubscriptionEndpointBuilder : ISubscriptionEndpointBuilder
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SubscriptionEndpointBuilder"/>.
        /// </summary>
        /// <param name="endpoint">�������� ����� ���� ���������.</param>
        /// <param name="topology">����������� ��������� ���� ���������.</param>
        /// <param name="receiver">������������ ���������� �������� ���������.</param>
        public SubscriptionEndpointBuilder(IEndpoint endpoint, ITopologyBuilder topology, IReceiverConfiguration receiver)
        {
            this.Endpoint = endpoint;
            this.Topology = topology;
            this.Receiver = receiver;
        }

        /// <summary>
        /// �������� ����� ���� ���������.
        /// </summary>
        public IEndpoint Endpoint { get; private set; }

        /// <summary>
        /// ������������ ���������� �������� ���������.
        /// </summary>
        public IReceiverConfiguration Receiver { get; private set; }

        /// <summary>
        /// ����������� ��������� ���� ���������.
        /// </summary>
        public ITopologyBuilder Topology { get; private set; }

        /// <summary>
        /// ������� �������� �� ��������� �������� ��������� ��� ���������� ���������.
        /// </summary>
        /// <param name="listeningSource">�������� �������� ���������.</param>
        /// <param name="callbackRouteResolver">����������� �������� ��������� ���������.</param>
        /// <returns>�������� ����� ��������.</returns>
        public ISubscriptionEndpoint ListenTo(IListeningSource listeningSource, IRouteResolver callbackRouteResolver)
        {
            return new SubscriptionEndpoint(listeningSource, callbackRouteResolver);
        }

        /// <summary>
        /// ������� �������� ����� ��� ������� � ���������� �� ���������.
        /// </summary>
        /// <returns>�������� ����� ��������.</returns>
        public ISubscriptionEndpoint UseDefaultTempReplyEndpoint()
        {
            return this.Topology.BuildTempReplyEndpoint();
        }

        /// <summary>
        /// ������� �������� ����� ��� ������� � ����������� �� ���������
        /// </summary>
        /// <param name="senderConfiguration">��������� �����������.</param>
        /// <returns>�������� ����� �������� �� ���������.</returns>
        public ISubscriptionEndpoint UseDefaultTempReplyEndpoint(ISenderConfiguration senderConfiguration)
        {
            return this.Topology.BuildTempReplyEndpoint(this.Endpoint, senderConfiguration.Label);
        }
    }
}
