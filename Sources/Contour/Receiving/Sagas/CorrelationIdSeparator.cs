namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ��������� ������������� ���� �� ������ ��������� <c>x-correlation-id</c> ��������� ���������.
    /// </summary>
    /// <typeparam name="TM">��� ��������� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class CorrelationIdSeparator<TM, TK> : ISagaIdSeparator<TM, TK>
        where TM : class
    {
        /// <summary>
        /// ��������� ������������� ���� �� ������ ��������� ���������.
        /// </summary>
        /// <param name="message">���������, � ������� ��������� ������������� ����.</param>
        /// <returns>������������� ����.</returns>
        public TK GetId(Message<TM> message)
        {
            return Headers.Extract<TK>(message.Headers, Headers.CorrelationId);
        }
    }
}
