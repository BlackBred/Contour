namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ����������� �������������� ���� �� ������ ��������� ���������.
    /// </summary>
    /// <typeparam name="TM">��� ��������� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaIdSeparator<TM, TK>
        where TM : class
    {
        /// <summary>
        /// ��������� ������������� ���� �� ������ ��������� ���������.
        /// </summary>
        /// <param name="message">���������, � ������� ��������� ������������� ����.</param>
        /// <returns>������������� ����.</returns>
        TK GetId(Message<TM> message);
    }
}
