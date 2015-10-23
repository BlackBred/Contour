using System;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ���� ���������� ����.
    /// </summary>
    /// <typeparam name="TS">��� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TM">��� ��������� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class LambdaSagaStep<TS, TM, TK> : ISagaStep<TS, TM, TK>
        where TM : class
    {
        private readonly Action<ISagaContext<TS, TK>, IConsumingContext<TM>> action;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="LambdaSagaStep{TS,TM,TK}"/>. 
        /// </summary>
        /// <param name="action">�������� ����������� �� ��������� ���� ����.</param>
        public LambdaSagaStep(Action<ISagaContext<TS, TK>, IConsumingContext<TM>> action)
        {
            this.action = action;
        }

        /// <summary>
        /// ��������� ��� ����.
        /// </summary>
        /// <param name="sagaContext">�������� ���� ��������� �� ������ ���� ����������.</param>
        /// <param name="consumingContext">�������� ��������� ��������� ���������.</param>
        public void Handle(ISagaContext<TS, TK> sagaContext, IConsumingContext<TM> consumingContext)
        {
            this.action(sagaContext, consumingContext);
        }
    }
}
