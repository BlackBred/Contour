using System;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������� �������� ���� ��� ���������� ���� ����.
    /// </summary>
    /// <typeparam name="TS">��� ������ ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class LambdaSagaFactory<TS, TK> : ISagaFactory<TS, TK>
    {
        private readonly Func<TK, ISagaContext<TS, TK>> factoryById;

        private readonly Func<TK, TS, ISagaContext<TS, TK>> factoryByData;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="LambdaSagaFactory{TS,TK}"/>. 
        /// </summary>
        /// <param name="factoryById">������� ����� �������� ���� � ��������� ���������������.</param>
        /// <param name="factoryByData">������� ����� �������� ���� � ��������� ��������������� � �������.</param>
        public LambdaSagaFactory(Func<TK, ISagaContext<TS, TK>> factoryById, Func<TK, TS, ISagaContext<TS, TK>> factoryByData)
        {
            this.factoryById = factoryById;
            this.factoryByData = factoryByData;
        }

        /// <summary>
        /// ������� ���� �� ������ ����������� ��������������.
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        /// <returns>��������� ����.</returns>
        public ISagaContext<TS, TK> Create(TK sagaId)
        {
            return this.factoryById(sagaId);
        }

        /// <summary>
        /// ������� ���� �� ������ �������������� � ��������� ����.
        /// </summary>
        /// <param name="sagaId">������������� ����.</param>
        /// <param name="data">��������� ����.</param>
        /// <returns>��������� ����.</returns>
        public ISagaContext<TS, TK> Create(TK sagaId, TS data)
        {
            return this.factoryByData(sagaId, data);
        }
    }
}
