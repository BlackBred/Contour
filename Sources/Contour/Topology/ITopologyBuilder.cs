using Contour.Receiving;

namespace Contour.Topology
{
    /// <summary>
    /// ����������� ���������.
    /// </summary>
    public interface ITopologyBuilder
    {
        /// <summary>
        /// ������� ��������� �������� ����� ��� ��������� ���������.
        /// </summary>
        /// <returns>�������� ����� �������� ��� ��������� ���������.</returns>
        ISubscriptionEndpoint BuildTempReplyEndpoint();

        /// <summary>
        /// ������� ��������� �������� ����� ��� ��������� ���������.
        /// </summary>
        /// <param name="endpoint">�������� ����� ���� ��������� ��� ������� ��������� ��������.</param>
        /// <param name="label">����� ���������, �� ������� ��������� ��������� ������.</param>
        /// <returns>
        /// �������� ����� �������� ��� ��������� ���������.
        /// </returns>
        ISubscriptionEndpoint BuildTempReplyEndpoint(IEndpoint endpoint, MessageLabel label);
    }
}
