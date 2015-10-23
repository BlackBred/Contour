namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ���������� ���� ����.
    /// </summary>
    /// <typeparam name="TS">��� ��������� ����.</typeparam>
    /// <typeparam name="TM">��� ��������� ������������� ��� ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaStep<TS, TM, TK>
        where TM : class
    {
        void Handle(ISagaContext<TS, TK> sagaContext, IConsumingContext<TM> consumingContext);
    }
}
