using System;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������������ ����.
    /// </summary>
    /// <typeparam name="TS">��� ���������������� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TM">��� ��������� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal interface ISagaConfigurator<TS, TM, TK>
        where TM : class
    {
        /// <summary>
        /// ������������ ����� ���������� ��������� ��������� ���������� ��� ���������� ���� ����.
        /// </summary>
        /// <param name="action">���������� ��������� ��������� ���������� ��� ���������� ���� ����.</param>
        /// <returns>������������ ���� � ������������������ ������������ ��������� ���������.</returns>
        ISagaConfigurator<TS, TM, TK> ReactWith(Action<ISagaContext<TS, TK>, IConsumingContext<TM>> action);

        /// <summary>
        /// ������������ ����� ���������� ��������� ��������� ���������� ��� ���������� ���� ����.
        /// </summary>
        /// <param name="canInitate">�����, ���� �������� ��������� ����� ������� ����� ����.</param>
        /// <param name="action">���������� ��������� ��������� ���������� ��� ���������� ���� ����.</param>
        /// <returns>������������ ���� � ������������������ ������������ ��������� ���������.</returns>
        ISagaConfigurator<TS, TM, TK> ReactWith(bool canInitate, Action<ISagaContext<TS, TK>, IConsumingContext<TM>> action);

        /// <summary>
        /// ������������ ��� ���� ����������� ��� ��������� ��������� ���������.
        /// </summary>
        /// <param name="sagaStep">���������� ��������� ��������� ���������� ��� ���������� ���� ����.</param>
        /// <returns>������������ ���� � ������������������ ������������ ��������� ���������.</returns>
        ISagaConfigurator<TS, TM, TK> ReactWith(ISagaStep<TS, TM, TK> sagaStep);

        /// <summary>
        /// ������������ ����� ���������� ��������� ��������� ���������� ��� ���������� ���� ����.
        /// </summary>
        /// <param name="canInitate">�����, ���� �������� ��������� ����� ������� ����� ����.</param>
        /// <param name="sagaStep">����������� ��� ����, ������� ���������� ��� ��������� ��������� ���������.</param>
        /// <returns>������������ ���� � ������������������ ������������ ��������� ���������.</returns>
        ISagaConfigurator<TS, TM, TK> ReactWith(bool canInitate, ISagaStep<TS, TM, TK> sagaStep);

        /// <summary>
        /// ������������ ���������� ������, ������� ��������� ��� ���������� ���� ����.
        /// </summary>
        /// <param name="sagaFailedHandler">���������� ������.</param>
        /// <returns>������������ ���� � ������������������ ������������ ������.</returns>
        ISagaConfigurator<TS, TM, TK> OnSagaFailed(ISagaFailedHandler<TS, TM, TK> sagaFailedHandler);

        /// <summary>
        /// ������������ ����������� ������, ������� ��������� ��� ���������� ���� ����.
        /// </summary>
        /// <param name="notFoundHandler">���������� ��������, ����� ���� �� �������.</param>
        /// <param name="failedAction">���������� ��������, ����� ��� ���������� ���� ���� ��������� ����������.</param>
        /// <returns>������������ ���� � ������������������ ������������ ������.</returns>
        ISagaConfigurator<TS, TM, TK> OnSagaFailed(Action<IConsumingContext<TM>> notFoundHandler, Action<ISagaContext<TS, TK>, IConsumingContext<TM>, Exception> failedAction);

        /// <summary>
        /// ������������ ��������� ���� ����.
        /// </summary>
        /// <param name="sagaLifecycle">��������� ���� ����.</param>
        /// <returns>������������ ���� � ������������������ ��������� ������ ����.</returns>
        ISagaConfigurator<TS, TM, TK> UseLifeCycle(ISagaLifecycle<TS, TM, TK> sagaLifecycle);

        /// <summary>
        /// ������������ ������� ����.
        /// </summary>
        /// <param name="sagaFactory">������� ����.</param>
        /// <returns>������������ ���� � ������������������ ��������� ������ ����.</returns>
        ISagaConfigurator<TS, TM, TK> UseSagaFactory(ISagaFactory<TS, TK> sagaFactory);

        /// <summary>
        /// ������������ ������ ������� ����.
        /// </summary>
        /// <param name="factoryById">������� ��������� ���� �� ������ �� ��������������.</param>
        /// <param name="factoryByData">������� ��������� ���� �� ������ �� �������������� � ���������������� ������.</param>
        /// <returns>������������ ���� � ������������������ �������� ����.</returns>
        ISagaConfigurator<TS, TM, TK> UseSagaFactory(
            Func<TK, ISagaContext<TS, TK>> factoryById, 
            Func<TK, TS, ISagaContext<TS, TK>> factoryByData);

        /// <summary>
        /// ������������ ��������� ����.
        /// </summary>
        /// <param name="sagaRepository">��������� ����.</param>
        /// <returns>������������ ���� � ������������������ �������� ����.</returns>
        ISagaConfigurator<TS, TM, TK> UseSagaRepository(ISagaRepository<TS, TK> sagaRepository);

        /// <summary>
        /// ������������ ����������� �������������� ����.
        /// </summary>
        /// <param name="sagaIdSeparator">����������� �������������� ����.</param>
        /// <returns>������������ ���� � ������������������ ������������ ��������������.</returns>
        ISagaConfigurator<TS, TM, TK> UseSagaIdSeparator(ISagaIdSeparator<TM, TK> sagaIdSeparator);

        /// <summary>
        /// ������������ ����������� �������������� ����.
        /// </summary>
        /// <param name="separator">����������� �������������� ����.</param>
        /// <returns>������������ ���� � ������������������ ������������ ��������������.</returns>
        ISagaConfigurator<TS, TM, TK> UseSagaIdSeparator(Func<Message<TM>, TK> separator);

        /// <summary>
        /// ���������� ������������ ���������� ��������� ���������.
        /// </summary>
        /// <returns>������������ ���������� ��������� ���������.</returns>
        IReceiverConfigurator<TM> AsReceiver();
    }
}
