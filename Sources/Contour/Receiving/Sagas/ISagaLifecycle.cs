namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������������ ����� ����� ����.
    /// </summary>
    /// <typeparam name="TS">��� ������ ����.</typeparam>
    /// <typeparam name="TM">��� ������ ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaLifecycle<TS, TM, TK>
        where TM : class
    {
        /// <summary>
        /// �������������� ����.
        /// </summary>
        /// <param name="context">�������� ��������� ��������� ���������.</param>
        /// <param name="canInitiate">���� <c>true</c> - ����� ��� ������������� ����� ��������� ����� ����.</param>
        /// <returns>���� ��������������� ���������, ����� ���� ��� <c>null</c>, ���� ���� �������� ��� ������� ����������.</returns>
        ISagaContext<TS, TK> InitializeSaga(IConsumingContext<TM> context, bool canInitiate);

        /// <summary>
        /// ��������� ��������� ����. 
        /// </summary>
        /// <param name="sagaContext">����������� ����.</param>
        void FinilizeSaga(ISagaContext<TS, TK> sagaContext);
    }
}
