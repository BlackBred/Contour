namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������ ����, ��� ������� ��� ������������ �� ��������� ����� ����������� � ����� ����������.
    /// </summary>
    /// <typeparam name="TS">��� ���������������� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class SingletonSagaFactory<TS, TK>
    {
        private static readonly ISagaFactory<TS, TK> SagaFactory = new DefaultSagaFactory<TS, TK>();

        /// <summary>
        /// ���������� ������������ ��������� ������� ����.
        /// </summary>
        public static ISagaFactory<TS, TK> Instance
        {
            get
            {
                return SagaFactory;
            }
        }
    }
}
