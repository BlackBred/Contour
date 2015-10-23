namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// �����������, ��� ��������� ���� ������������ �� ��������� ��������� 
    /// </summary>
    /// <typeparam name="TS">��� ���������������� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class SingletonSagaRepository<TS, TK>
    {
        private static readonly ISagaRepository<TS, TK> SagaRepository = new InMemorySagaRepository<TS, TK>(SingletonSagaFactory<TS, TK>.Instance);

        /// <summary>
        /// ���������� ������������ ��������� ��������� ����.
        /// </summary>
        public static ISagaRepository<TS, TK> Instance 
        {
            get
            {
                return SagaRepository;
            }
        }
    }
}
