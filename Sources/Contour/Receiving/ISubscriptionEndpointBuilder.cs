using Contour.Sending;
using Contour.Topology;

namespace Contour.Receiving
{
    /// <summary>
    /// ����������� �������� ����� ��� ��������� ���������.
    /// </summary>
    public interface ISubscriptionEndpointBuilder
    {
        /// <summary>
        /// �������� ����� ���� ���������.
        /// </summary>
        IEndpoint Endpoint { get; }

        /// <summary>
        /// ������������ ����������.
        /// </summary>
        IReceiverConfiguration Receiver { get; }

        /// <summary>
        /// ����������� ���������.
        /// </summary>
        ITopologyBuilder Topology { get; }

        /// <summary>
        /// ������� �������� ����� �������� �� ��������� ��������
        /// </summary>
        /// <param name="listeningSource">�������� ��������.</param>
        /// <param name="callbackRouteResolver">����������� �������� ��������� ���������.</param>
        /// <returns>�������� ����� ��������.</returns>
        ISubscriptionEndpoint ListenTo(IListeningSource listeningSource, IRouteResolver callbackRouteResolver = null);

        /// <summary>
        /// ������������� �������� ����� ��� ������� � ����������� �� ���������.
        /// </summary>
        /// <returns>�������� ����� �������� �� ���������.</returns>
        ISubscriptionEndpoint UseDefaultTempReplyEndpoint();

        /// <summary>
        /// ������������ �������� ����� ��� ������� � ����������� �� ���������
        /// </summary>
        /// <param name="senderConfiguratoration">��������� �����������.</param>
        /// <returns>�������� ����� �������� �� ���������.</returns>
        ISubscriptionEndpoint UseDefaultTempReplyEndpoint(ISenderConfiguration senderConfiguratoration);
    }
}
