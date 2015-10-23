using System;

using Contour.Receiving.Consumers;
using Contour.Validation;

namespace Contour.Receiving
{
    /// <summary>
    /// �������������� ������������ ���������� ���������.
    /// </summary>
    /// <typeparam name="T">��� ����������� ���������.</typeparam>
    internal class TypedReceiverConfigurationDecorator<T> : IReceiverConfigurator<T>
        where T : class
    {
        /// <summary>
        /// ������������ ���������� ���������.
        /// </summary>
        private readonly ReceiverConfiguration configuration;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="TypedReceiverConfigurationDecorator{T}"/>.
        /// </summary>
        /// <param name="configuration">������������ ���������� ���������.</param>
        public TypedReceiverConfigurationDecorator(ReceiverConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// ������������ ���������� ���������.
        /// </summary>
        public ReceiverConfiguration Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        /// <summary>
        /// ���������� �������������� ������������ ����������.
        /// </summary>
        /// <typeparam name="T1">��� ������������� ����������.</typeparam>
        /// <returns>������������ ����������.</returns>
        public IReceiverConfigurator<T1> As<T1>() where T1 : class
        {
            return this.configuration.As<T1>();
        }

        /// <summary>
        /// ������������ ��������� ��������� ���������, ��������� ������� ����������� ��������.
        /// </summary>
        /// <param name="failedDeliveryStrategy">��������� ��������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������.</returns>
        public IReceiverConfigurator OnFailed(IFailedDeliveryStrategy failedDeliveryStrategy)
        {
            return this.configuration.OnFailed(failedDeliveryStrategy);
        }

        /// <summary>������������ ���������� ���������, ��������� ������� ����������� ��������.</summary>
        /// <param name="failedDeliveryHandler">���������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������</returns>
        public IReceiverConfigurator OnFailed(Action<IFailedConsumingContext> failedDeliveryHandler)
        {
            return this.configuration.OnFailed(failedDeliveryHandler);
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith(Action<T> handlerAction)
        {
            this.configuration.ReactWith(handlerAction);

            return this;
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith(Action<T, IConsumingContext<T>> handlerAction)
        {
            this.configuration.ReactWith(handlerAction);

            return this;
        }

        /// <summary>������������ ������� ������������ ��������� ���������.</summary>
        /// <param name="consumerFactoryFunc">������� ������������ �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith(Func<IConsumerOf<T>> consumerFactoryFunc)
        {
            this.configuration.ReactWith(consumerFactoryFunc);

            return this;
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="consumer">���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith(IConsumerOf<T> consumer)
        {
            this.configuration.ReactWith(consumer);

            return this;
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <typeparam name="T1">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T1> ReactWith<T1>(Action<T1> handlerAction) where T1 : class
        {
            return this.configuration.ReactWith(handlerAction);
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <typeparam name="T1">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T1> ReactWith<T1>(Action<T1, IConsumingContext<T1>> handlerAction) where T1 : class
        {
            return this.configuration.ReactWith(handlerAction);
        }

        /// <summary>������������ ������� ������������ ��������� ���������.</summary>
        /// <param name="consumerFactoryFunc">������� ������������ �������� ���������.</param>
        /// <typeparam name="T1">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T1> ReactWith<T1>(Func<IConsumerOf<T1>> consumerFactoryFunc) where T1 : class
        {
            return this.configuration.ReactWith(consumerFactoryFunc);
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="consumer">���������� ��������� ���������.</param>
        /// <typeparam name="T1">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T1> ReactWith<T1>(IConsumerOf<T1> consumer) where T1 : class
        {
            return this.configuration.ReactWith(consumer);
        }

        /// <summary>
        /// ������������� ������������� ������ ������������� �������� ��������� ���������.
        /// </summary>
        /// <returns>
        /// ������������ ���������� � ����� �������������� �������� ��������� ���������.
        /// </returns>
        public IReceiverConfigurator RequiresAccept()
        {
            return this.configuration.RequiresAccept();
        }

        /// <summary>
        /// ������������ �������� �������� ��������� ���������.
        /// </summary>
        /// <param name="validator">�������� �������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� �������� �������� ���������.</returns>
        public IReceiverConfigurator WhenVerifiedBy(IMessageValidator validator)
        {
            this.configuration.WhenVerifiedBy(validator);

            return this;
        }

        /// <summary>
        /// ������������ �������� �������� ��������� ���������.
        /// </summary>
        /// <param name="validator">�������� �������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� �������� �������� ���������.</returns>
        public IReceiverConfigurator<T> WhenVerifiedBy(IMessageValidatorOf<T> validator)
        {
            this.configuration.WhenVerifiedBy(validator);

            return this;
        }

        /// <summary>
        /// ������������� ��������� �������� ���������.
        /// </summary>
        /// <param name="alias">��������� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� �������� ���������.</returns>
        public IReceiverConfigurator<T> WithAlias(string alias)
        {
            this.configuration.WithAlias(alias);

            return this;
        }

        /// <summary>
        /// ������������ ����������� �����, �� �������� �������� ��������� �������� ���������.
        /// </summary>
        /// <param name="endpointBuilder">����������� ����� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ ����� �������� ���������.</returns>
        public IReceiverConfigurator WithEndpoint(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> endpointBuilder)
        {
            return this.configuration.WithEndpoint(endpointBuilder);
        }

        /// <summary>
        /// ������������� ���������� ������������� ������������ �������� ���������.
        /// </summary>
        /// <param name="parallelismLevel">���������� ������������� ������������ �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator WithParallelismLevel(uint parallelismLevel)
        {
            return this.configuration.WithParallelismLevel(parallelismLevel);
        }

        /// <summary>
        /// ������������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ����������.</returns>
        IReceiverConfigurator IReceiverConfigurator<T>.WithIncomingMessageHeaderStorage(IIncomingMessageHeaderStorage storage)
        {
            this.configuration.WithIncomingMessageHeaderStorage(storage);

            return this;
        }

        /// <summary>
        /// ������������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ����������.</returns>
        IReceiverConfigurator IReceiverConfigurator.WithIncomingMessageHeaderStorage(IIncomingMessageHeaderStorage storage)
        {
            this.configuration.WithIncomingMessageHeaderStorage(storage);

            return this;
        }

        /// <summary>
        /// ������������ ��������� ��������� ���������, ��������� ������� ����������� ��������.
        /// </summary>
        /// <param name="failedDeliveryStrategy">��������� ��������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������.</returns>
        IReceiverConfigurator<T> IReceiverConfigurator<T>.OnFailed(IFailedDeliveryStrategy failedDeliveryStrategy)
        {
            this.configuration.OnFailed(failedDeliveryStrategy);

            return this;
        }

        /// <summary>������������ ���������� ���������, ��������� ������� ����������� ��������.</summary>
        /// <param name="failedDeliveryHandler">���������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������</returns>
        IReceiverConfigurator<T> IReceiverConfigurator<T>.OnFailed(Action<IFailedConsumingContext> failedDeliveryHandler)
        {
            this.configuration.OnFailed(failedDeliveryHandler);

            return this;
        }

        /// <summary>
        /// ������������� ������������� ������ ������������� �������� ��������� ���������.
        /// </summary>
        /// <returns>
        /// ������������ ���������� � ����� �������������� �������� ��������� ���������.
        /// </returns>
        IReceiverConfigurator<T> IReceiverConfigurator<T>.RequiresAccept()
        {
            this.configuration.RequiresAccept();

            return this;
        }

        /// <summary>
        /// ������������� ��������� �������� ���������.
        /// </summary>
        /// <param name="alias">��������� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� �������� ���������.</returns>
        IReceiverConfigurator IReceiverConfigurator.WithAlias(string alias)
        {
            return this.WithAlias(alias);
        }

        /// <summary>
        /// ������������ ����������� �����, �� �������� �������� ��������� �������� ���������.
        /// </summary>
        /// <param name="endpointBuilder">����������� ����� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ ����� �������� ���������.</returns>
        IReceiverConfigurator<T> IReceiverConfigurator<T>.WithEndpoint(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> endpointBuilder)
        {
            this.configuration.WithEndpoint(endpointBuilder);

            return this;
        }

        /// <summary>
        /// ������������� ���������� ������������� ������������ �������� ���������.
        /// </summary>
        /// <param name="parallelismLevel">���������� ������������� ������������ �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> IReceiverConfigurator<T>.WithParallelismLevel(uint parallelismLevel)
        {
            this.configuration.WithParallelismLevel(parallelismLevel);

            return this;
        }
    }
}
