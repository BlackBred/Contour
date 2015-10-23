using System.Collections.Concurrent;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ��������� ��� � ������.
    /// </summary>
    /// <typeparam name="TS">��� ���������������� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class InMemorySagaRepository<TS, TK> : ISagaRepository<TS, TK>
    {
        private readonly ConcurrentDictionary<TK, TS> sagas = new ConcurrentDictionary<TK, TS>();

        private readonly ISagaFactory<TS, TK> sagaFactory;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="InMemorySagaRepository{TS,TK}"/>. 
        /// </summary>
        /// <param name="sagaFactory">������� ��� ������������ ��� �������� ���� ������������ �� ���������.</param>
        public InMemorySagaRepository(ISagaFactory<TS, TK> sagaFactory)
        {
            this.sagaFactory = sagaFactory;
        }

        /// <summary>
        /// �������� ����������� ����.
        /// </summary>
        /// <param name="sagaId">������������� ������������� ����.</param>
        /// <returns>������������� ����.</returns>
        public ISagaContext<TS, TK> Get(TK sagaId)
        {
            TS sagaData;
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            if (sagaId == null || !this.sagas.TryGetValue(sagaId, out sagaData))
            {
                return null;
            }

            return this.sagaFactory.Create(sagaId, sagaData);
        }

        /// <summary>
        /// ��������� ����.
        /// </summary>
        /// <param name="sagaContext">����������� ����.</param>
        public void Store(ISagaContext<TS, TK> sagaContext)
        {
            this.sagas.AddOrUpdate(sagaContext.SagaId, sagaContext.Data, (s, list) => list);
        }

        /// <summary>
        /// ������� ����.
        /// </summary>
        /// <param name="sagaContext">��������� ����.</param>
        public void Remove(ISagaContext<TS, TK> sagaContext)
        {
            TS sagaData;
            this.sagas.TryRemove(sagaContext.SagaId, out sagaData);
        }
    }
}
