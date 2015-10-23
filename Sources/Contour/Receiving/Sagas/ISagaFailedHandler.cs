using System;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ���������� ������ ����.
    /// </summary>
    /// <typeparam name="TS">��� ��������� ����.</typeparam>
    /// <typeparam name="TM">��� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaFailedHandler<TS, TM, TK>
        where TM : class
    {
        /// <summary>
        /// ������������ ��������, ����� ���� �� �������.
        /// </summary>
        /// <param name="context">�������� ��������� ���������, � ������� �������� ��� ��������.</param>
        void SagaNotFoundHandle(IConsumingContext<TM> context);

        /// <summary>
        /// ������������ ���������� ��������� ��� ���������� ���� ����.
        /// </summary>
        /// <param name="sagaContext">������ ����.</param>
        /// <param name="context">�������� ����������� ���������.</param>
        /// <param name="exception">�������������� ����������.</param>
        void SagaFailedHandle(ISagaContext<TS, TK> sagaContext, IConsumingContext<TM> context, Exception exception);
    }
}
