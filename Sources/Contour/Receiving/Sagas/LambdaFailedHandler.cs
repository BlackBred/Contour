using System;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������������ ������ ����������� ��� ���������� ���� ����.
    /// </summary>
    /// <typeparam name="TS">��� ������ ����.</typeparam>
    /// <typeparam name="TM">��� ��������� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class LambdaFailedHandler<TS, TM, TK> : ISagaFailedHandler<TS, TM, TK>
        where TM : class
    {
        private readonly Action<IConsumingContext<TM>> notFoundAction;

        private readonly Action<ISagaContext<TS, TK>, IConsumingContext<TM>, Exception> failedAction;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="LambdaFailedHandler{TS,TM,TK}"/>. 
        /// </summary>
        /// <param name="notFoundAction">���������� ��������, ����� ���� �� ������� �� ��������������.</param>
        /// <param name="failedAction">���������� ��������, ����� ��� ���������� ���� ���� ��������� ������.</param>
        public LambdaFailedHandler(Action<IConsumingContext<TM>> notFoundAction, Action<ISagaContext<TS, TK>, IConsumingContext<TM>, Exception> failedAction)
        {
            this.notFoundAction = notFoundAction;
            this.failedAction = failedAction;
        }

        /// <summary>
        /// ������������ ��������, ����� ���� �� �������.
        /// </summary>
        /// <param name="context">�������� ��������� ���������, � ������� �������� ��� ��������.</param>
        public void SagaNotFoundHandle(IConsumingContext<TM> context)
        {
            this.notFoundAction(context);
        }

        /// <summary>
        /// ������������ ���������� ��������� ��� ���������� ���� ����.
        /// </summary>
        /// <param name="sagaContext">������ ����.</param>
        /// <param name="context">�������� ����������� ���������.</param>
        /// <param name="exception">�������������� ����������.</param>
        public void SagaFailedHandle(ISagaContext<TS, TK> sagaContext, IConsumingContext<TM> context, Exception exception)
        {
            this.failedAction(sagaContext, context, exception);
        }
    }
}
