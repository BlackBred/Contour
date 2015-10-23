namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������������ ����� ����� ����.
    /// </summary>
    /// <typeparam name="TS">��� ������ ����.</typeparam>
    /// <typeparam name="TM">��� ������ ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class DefaultSagaLifecycle<TS, TM, TK> : ISagaLifecycle<TS, TM, TK>
        where TM : class
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DefaultSagaLifecycle{TS,TM,TK}"/>. 
        /// </summary>
        /// <param name="repository">��������� ���������� ��������� ����.</param>
        /// <param name="separator">����������� �������������� ���� �� ���������.</param>
        /// <param name="factory">������� ��������� ����.</param>
        public DefaultSagaLifecycle(ISagaRepository<TS, TK> repository, ISagaIdSeparator<TM, TK> separator, ISagaFactory<TS, TK> factory)
        {
            this.SagaRepository = repository;
            this.SagaIdSeparator = separator;
            this.SagaFactory = factory;
        }

        /// <summary>
        /// ��������� ���������� ��������� ����.
        /// </summary>
        public ISagaRepository<TS, TK> SagaRepository { get; internal set; }

        /// <summary>
        /// ����������� �������������� ���� �� ���������.
        /// </summary>
        public ISagaIdSeparator<TM, TK> SagaIdSeparator { get; internal set; }

        /// <summary>
        /// ������� ��������� ����.
        /// </summary>
        public ISagaFactory<TS, TK> SagaFactory { get; internal set; }

        /// <summary>
        /// �������������� ����.
        /// </summary>
        /// <param name="context">�������� ��������� ��������� ���������.</param>
        /// <param name="canInitiate">���� <c>true</c> - ����� ��� ������������� ����� ��������� ����� ����.</param>
        /// <returns>���� ��������������� ���������, ����� ���� ��� <c>null</c>, ���� ���� �������� ��� ������� ����������.</returns>
        public ISagaContext<TS, TK> InitializeSaga(IConsumingContext<TM> context, bool canInitiate)
        {
            var sagaId = this.SagaIdSeparator.GetId(context.Message);
            var saga = this.SagaRepository.Get(sagaId);

            if (saga == null)
            {
                if (canInitiate)
                {
                    saga = this.SagaFactory.Create(sagaId);
                }
            }

            return saga;
        }

        /// <summary>
        /// ��������� ��������� ����. 
        /// </summary>
        /// <param name="sagaContext">����������� ����.</param>
        public void FinilizeSaga(ISagaContext<TS, TK> sagaContext)
        {
            if (sagaContext.Completed)
            {
                this.SagaRepository.Remove(sagaContext);
            }
            else
            {
                this.SagaRepository.Store(sagaContext);
            }
        }
    }
}
