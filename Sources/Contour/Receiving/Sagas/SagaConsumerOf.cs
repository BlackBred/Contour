using System;

using Contour.Receiving.Consumers;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ���������� ���������, ������� ��������� � ����.
    /// </summary>
    /// <typeparam name="TS">��� ������ ��� �������� ��������� ������ - ��������� ����.</typeparam>
    /// <typeparam name="TM">��� ��������� ���������, ������� ��������� � ����.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class SagaConsumerOf<TS, TM, TK> : IConsumerOf<TM>
        where TM : class
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SagaConsumerOf{TS,TM,TK}"/>. 
        /// </summary>
        /// <param name="sagaLifecycle">��������� ��������� ������ ����.</param>
        /// <param name="sagaStep">���������� ���� ����.</param>
        /// <param name="canInitiate">���� <c>true</c> - ����� ���� ����� ���� ������� ������������ ���������.</param>
        /// <param name="failedHandler">���������� ������ � ����.</param>
        public SagaConsumerOf(ISagaLifecycle<TS, TM, TK> sagaLifecycle, ISagaStep<TS, TM, TK> sagaStep, bool canInitiate, ISagaFailedHandler<TS, TM, TK> failedHandler)
        {
            this.SagaLifecycle = sagaLifecycle;
            this.SagaStep = sagaStep;
            this.CanInitiate = canInitiate;
            this.SagaFailedHandler = failedHandler;
        }

        /// <summary>
        /// ������� ����������� �������� ���� �� ���� ����.
        /// ���� <c>false</c>, ����� ��� �� ��������� � ��������� ���� ������, ����� <c>true</c>.
        /// </summary>
        public bool CanInitiate { get; internal set; }

        /// <summary>
        /// ����������� �������� �� ���� ����.
        /// </summary>
        public ISagaStep<TS, TM, TK> SagaStep { get; internal set; }

        /// <summary>
        /// ���������� ������ ��� ��������� ���������.
        /// </summary>
        public ISagaFailedHandler<TS, TM, TK> SagaFailedHandler { get; internal set; }

        /// <summary>
        /// ��������� ���� ����.
        /// </summary>
        public ISagaLifecycle<TS, TM, TK> SagaLifecycle { get; internal set; }

        /// <summary>
        /// ������������ �������� ���������.
        /// </summary>
        /// <param name="context">�������� ��������� ��������� ���������.</param>
        public void Handle(IConsumingContext<TM> context)
        {
            var saga = this.SagaLifecycle.InitializeSaga(context, this.CanInitiate);

            if (saga == null)
            {
                this.SagaFailedHandler.SagaNotFoundHandle(context);

                return;
            }

            try
            {
                this.SagaStep.Handle(saga, context);
            }
            catch (Exception exception)
            {
                this.SagaFailedHandler.SagaFailedHandle(saga, context, exception);
                throw;
            }

            this.SagaLifecycle.FinilizeSaga(saga);
        }
    }
}
