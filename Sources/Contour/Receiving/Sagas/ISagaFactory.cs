namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������� ����.
    /// </summary>
    /// <typeparam name="TD">��� ��������� ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaFactory<TD, TK>
    {
        /// <summary>
        /// ������� ���� �� ������ ����������� ��������������.
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        /// <returns>��������� ����.</returns>
        ISagaContext<TD, TK> Create(TK sagaId);

        /// <summary>
        /// ������� ���� �� ������ �������������� � ��������� ����.
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        /// <param name="data">��������� ����.</param>
        /// <returns>��������� ����.</returns>
        ISagaContext<TD, TK> Create(TK sagaId, TD data);
    }
}
