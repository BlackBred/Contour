namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ��������� ��������� ����.
    /// </summary>
    /// <typeparam name="TD">��� ������������ ��������� ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaRepository<TD, TK>
    {
        /// <summary>
        /// �������� ����������� ����.
        /// </summary>
        /// <param name="sagaId">������������� ������������� ����.</param>
        /// <returns>������������� ���� ���� ��� ����������, ����� <c>null</c>.</returns>
        ISagaContext<TD, TK> Get(TK sagaId);

        /// <summary>
        /// ��������� ����.
        /// </summary>
        /// <param name="sagaContext">����������� ����.</param>
        void Store(ISagaContext<TD, TK> sagaContext);

        /// <summary>
        /// ������� ����.
        /// </summary>
        /// <param name="sagaContext">��������� ����.</param>
        void Remove(ISagaContext<TD, TK> sagaContext);
    }
}
