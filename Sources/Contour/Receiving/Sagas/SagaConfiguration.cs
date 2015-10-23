using System;

namespace Contour.Receiving.Sagas
{
    /// <summary>
    /// ������������ ����.
    /// </summary>
    /// <typeparam name="TS">��� ���������������� ������ ����������� � ����.</typeparam>
    /// <typeparam name="TM">��� ��������� ���������.</typeparam>
    /// <typeparam name="TK">��� �������������� ����.</typeparam>
    internal class SagaConfiguration<TS, TM, TK> : ISagaConfigurator<TS, TM, TK>
        where TM : class
    {
        private readonly IReceiverConfigurator<TM> receiverConfigurator;

        private SagaConsumerOf<TS, TM, TK> sagaConsumer;

        private DefaultSagaLifecycle<TS, TM, TK> sagaLifecycle;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SagaConfiguration{TS,TM,TK}"/>. 
        /// </summary>
        /// <param name="receiverConfigurator">������������ ���������� ��������� ���������.</param>
        /// <param name="sagaRepository">��������� ����.</param>
        /// <param name="sagaIdSeparator">����������� �������������� ����.</param>
        /// <param name="sagaFactory">������� ����.</param>
        /// <param name="sagaStep">�������� ����������� ��� ��������� ����.</param>
        /// <param name="sagaFailedHandler">���������� ����������� ������.</param>
        public SagaConfiguration(
            IReceiverConfigurator<TM> receiverConfigurator,
            ISagaRepository<TS, TK> sagaRepository,
            ISagaIdSeparator<TM, TK> sagaIdSeparator,
            ISagaFactory<TS, TK> sagaFactory,
            ISagaStep<TS, TM, TK> sagaStep,
            ISagaFailedHandler<TS, TM, TK> sagaFailedHandler)
        {
            this.receiverConfigurator = receiverConfigurator;

            this.sagaLifecycle = new DefaultSagaLifecycle<TS, TM, TK>(sagaRepository, sagaIdSeparator, sagaFactory);
            this.sagaConsumer = new SagaConsumerOf<TS, TM, TK>(this.sagaLifecycle, sagaStep, false, sagaFailedHandler);
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> ReactWith(Action<ISagaContext<TS, TK>, IConsumingContext<TM>> action)
        {
            this.sagaConsumer.SagaStep = new LambdaSagaStep<TS, TM, TK>(action);
            this.receiverConfigurator.ReactWith(() => this.sagaConsumer);

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> ReactWith(bool canInitate, Action<ISagaContext<TS, TK>, IConsumingContext<TM>> action)
        {
            this.sagaConsumer.SagaStep = new LambdaSagaStep<TS, TM, TK>(action);
            this.sagaConsumer.CanInitiate = canInitate;
            this.receiverConfigurator.ReactWith(() => this.sagaConsumer);

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> ReactWith(ISagaStep<TS, TM, TK> sagaStep)
        {
            this.sagaConsumer.SagaStep = sagaStep;
            this.receiverConfigurator.ReactWith(() => this.sagaConsumer);

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> ReactWith(bool canInitate, ISagaStep<TS, TM, TK> sagaStep)
        {
            this.sagaConsumer.SagaStep = sagaStep;
            this.sagaConsumer.CanInitiate = canInitate;
            this.receiverConfigurator.ReactWith(() => this.sagaConsumer);

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> OnSagaFailed(ISagaFailedHandler<TS, TM, TK> sagaFailedHandler)
        {
            this.sagaConsumer.SagaFailedHandler = sagaFailedHandler;

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> OnSagaFailed(Action<IConsumingContext<TM>> notFoundHandler, Action<ISagaContext<TS, TK>, IConsumingContext<TM>, Exception> failedAction)
        {
            this.sagaConsumer.SagaFailedHandler = new LambdaFailedHandler<TS, TM, TK>(notFoundHandler, failedAction);

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> UseLifeCycle(ISagaLifecycle<TS, TM, TK> sagaLifecycle)
        {
            this.sagaConsumer.SagaLifecycle = sagaLifecycle;

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> UseSagaFactory(ISagaFactory<TS, TK> sagaFactory)
        {
            this.sagaLifecycle.SagaFactory = sagaFactory;

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> UseSagaFactory(Func<TK, ISagaContext<TS, TK>> factoryById, Func<TK, TS, ISagaContext<TS, TK>> factoryByData)
        {
            this.sagaLifecycle.SagaFactory = new LambdaSagaFactory<TS, TK>(factoryById, factoryByData);

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> UseSagaRepository(ISagaRepository<TS, TK> sagaRepository)
        {
            this.sagaLifecycle.SagaRepository = sagaRepository;

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> UseSagaIdSeparator(ISagaIdSeparator<TM, TK> sagaIdSeparator)
        {
            this.sagaLifecycle.SagaIdSeparator = sagaIdSeparator;

            return this;
        }

        /// <inheritdoc />
        public ISagaConfigurator<TS, TM, TK> UseSagaIdSeparator(Func<Message<TM>, TK> separator)
        {
            this.sagaLifecycle.SagaIdSeparator = new LambdaSagaSeparator<TM, TK>(separator);

            return this;
        }

        /// <inheritdoc />
        public IReceiverConfigurator<TM> AsReceiver()
        {
            return this.receiverConfigurator;
        }
    }
}
