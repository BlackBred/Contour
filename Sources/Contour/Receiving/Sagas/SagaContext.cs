namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// �������� ���� �� ������������ ����.
    /// </summary>
    /// <typeparam name="TS">��� ���������������� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class SagaContext<TS, TK> : ISagaContext<TS, TK>
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SagaContext{TS,TK}"/>. 
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        public SagaContext(TK sagaId)
        {
            this.SagaId = sagaId;
            this.Data = default(TS);
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SagaContext{TS,TK}"/>. 
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        /// <param name="data">���������������� ������ ����������� � ����.</param>
        public SagaContext(TK sagaId, TS data)
            : this(sagaId)
        {
            this.Data = data;
        }

        /// <summary>
        /// ������������� ����.
        /// </summary>
        public TK SagaId { get; private set; }

        /// <summary>
        /// ���� <c>true</c> - ����� ���� ���������, ����� - <c>false</c>.
        /// </summary>
        public bool Completed { get; private set; }

        /// <summary>
        /// ���������������� ������ ����������� � ����.
        /// </summary>
        public TS Data { get; private set; }

        /// <summary>
        /// ��������� ���������������� ������ ����������� � ����.
        /// </summary>
        /// <param name="data">���������������� ������ ����������� � ����.</param>
        public void UpdateData(TS data)
        {
            this.Data = data;
        }

        /// <summary>
        /// �������� ���� ��� �����������.
        /// </summary>
        public void Complete()
        {
            this.Completed = true;
        }
    }
}
