using System;
using System.Threading.Tasks;

namespace Contour.Receiving
{
    /// <summary>
    /// ������������ ���������.
    /// </summary>
    public interface IDelivery
    {
        /// <summary>
        /// ����� �������� ���������.
        /// </summary>
        IChannel Channel { get; }

        /// <summary>
        /// ����� ������������ ���������.
        /// </summary>
        MessageLabel Label { get; }

        /// <summary>
        /// ������� ������ �� ������������ ���������.
        /// </summary>
        IRoute ReplyRoute { get; }

        /// <summary>
        /// �����, ���� ����� �������� �� ��� ���������.
        /// </summary>
        bool CanReply { get; }

        /// <summary>
        /// ������������ �������� ���������.
        /// </summary>
        void Accept();

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        /// <param name="payload">����� ���������� ���������.</param>
        /// <returns>������ ��������� ���������.</returns>
        Task Forward(MessageLabel label, object payload);

        /// <summary>
        /// �������� ��������� ��� ��������������.
        /// </summary>
        /// <param name="requeue">
        /// ��������� ��������� ������� �� �������� ������� ��� ��������� ���������.
        /// </param>
        void Reject(bool requeue);

        /// <summary>
        /// �������� �������� ���������.
        /// </summary>
        /// <param name="message">�������� ���������.</param>
        void ReplyWith(IMessage message);

        /// <summary>
        /// ������������ ���������� ���������� � ��������� ���������� ����.
        /// </summary>
        /// <param name="type">��� ���������.</param>
        /// <returns>��������� ���������� ����.</returns>
        IMessage UnpackAs(Type type);
    }
}
