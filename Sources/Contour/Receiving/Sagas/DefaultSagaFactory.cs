namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������� ���� ������������ �� ���������.
    /// </summary>
    /// <typeparam name="TS">��� ���������������� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class DefaultSagaFactory<TS, TK> : ISagaFactory<TS, TK>
    {
        /// <summary>
        /// ������� ���� �� ������ ����������� ��������������.
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        /// <returns>��������� ����.</returns>
        public ISagaContext<TS, TK> Create(TK sagaId)
        {
            return new SagaContext<TS, TK>(sagaId);
        }

        /// <summary>
        /// ������� ���� �� ������ �������������� � ��������� ����.
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        /// <param name="data">��������� ����.</param>
        /// <returns>��������� ����.</returns>
        public ISagaContext<TS, TK> Create(TK sagaId, TS data)
        {
            return new SagaContext<TS, TK>(sagaId, data);
        }
    }
}
