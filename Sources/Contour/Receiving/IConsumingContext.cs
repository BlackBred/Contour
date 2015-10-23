using Contour.Caching;

namespace Contour.Receiving
{
    /// <summary>
    /// �������� ��������� ����������� ���������.
    /// </summary>
    /// <typeparam name="T">��� ����������� ���������.</typeparam>
    public interface IConsumingContext<T>
        where T : class
    {
        /// <summary>
        /// �������� �����, � ������� ��������������� ������� ���������� ���������.
        /// </summary>
        IBusContext Bus { get; }

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        Message<T> Message { get; }

        /// <summary>
        /// �����, ���� ����� �������� �� ��� ���������.
        /// </summary>
        bool CanReply { get; }

        /// <summary>
        /// �������� ��������� ��� ������������.
        /// </summary>
        void Accept();

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        void Forward(MessageLabel label);

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        void Forward(string label);

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        /// <param name="payload">����� ���������� ���������.</param>
        /// <typeparam name="TOut">��� ���������.</typeparam>
        void Forward<TOut>(MessageLabel label, TOut payload = null) where TOut : class;

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        /// <param name="payload">����� ���������� ���������.</param>
        /// <typeparam name="TOut">��� ���������.</typeparam>
        void Forward<TOut>(string label, TOut payload = null) where TOut : class;

        /// <summary>
        /// �������� ��������� ��� ��������������.
        /// </summary>
        /// <param name="requeue">
        /// ��������� ��������� ������� �� �������� ������� ��� ��������� ���������.
        /// </param>
        void Reject(bool requeue);

        /// <summary>
        /// ���������� �������� ���������, ��������� ���������� � ��������
        /// ��������� �������� ����� � ������������� ���������.
        /// </summary>
        /// <typeparam name="TResponse">.NET ��� ������������� ���������.</typeparam>
        /// <param name="response">������������ ���������.</param>
        /// <param name="expires">���������, ������� ���������� ����� ���� ����� ��������.</param>
        void Reply<TResponse>(TResponse response, Expires expires = null) where TResponse : class;
    }
}
