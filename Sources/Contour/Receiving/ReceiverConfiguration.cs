using System;

using Contour.Helpers;
using Contour.Receiving.Consumers;
using Contour.Validation;

namespace Contour.Receiving
{
    /// <summary>
    /// ������������ ����������.
    /// </summary>
    internal class ReceiverConfiguration : IReceiverConfiguration, IReceiverConfigurator
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="ReceiverConfiguration"/>.
        /// </summary>
        /// <param name="label">����� ����������� ���������.</param>
        /// <param name="parentOptions">��������� ����������.</param>
        public ReceiverConfiguration(MessageLabel label, ReceiverOptions parentOptions)
        {
            this.Label = label;

            this.Options = (ReceiverOptions)parentOptions.Derive();
        }

        /// <summary>
        /// ��������� ����������� ���������.
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// ����� ����������� ���������.
        /// </summary>
        public MessageLabel Label { get; private set; }

        /// <summary>
        /// ��������� ����������.
        /// </summary>
        public ReceiverOptions Options { get; private set; }

        /// <summary>
        /// ����������� ����������.
        /// </summary>
        public Action<IReceiver> ReceiverRegistration { get; protected internal set; }

        /// <summary>
        /// �������� �������� ���������� ���������.
        /// </summary>
        public IMessageValidator Validator { get; private set; }

        /// <summary>
        /// ���������� �������������� ������������ ����������.
        /// </summary>
        /// <typeparam name="T">��� ������������� ����������.</typeparam>
        /// <returns>������������ ����������.</returns>
        public IReceiverConfigurator<T> As<T>() where T : class
        {
            return new TypedReceiverConfigurationDecorator<T>(this);
        }

        /// <summary>
        /// ������������ ��������� ��������� ���������, ��������� ������� ����������� ��������.
        /// </summary>
        /// <param name="failedDeliveryStrategy">��������� ��������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������.</returns>
        public IReceiverConfigurator OnFailed(IFailedDeliveryStrategy failedDeliveryStrategy)
        {
            this.Options.FailedDeliveryStrategy = failedDeliveryStrategy.Maybe();

            return this;
        }

        /// <summary>������������ ���������� ���������, ��������� ������� ����������� ��������.</summary>
        /// <param name="failedDeliveryHandler">���������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������</returns>
        public IReceiverConfigurator OnFailed(Action<IFailedConsumingContext> failedDeliveryHandler)
        {
            return this.OnFailed(new LambdaFailedDeliveryStrategy(failedDeliveryHandler));
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith<T>(Action<T> handlerAction) where T : class
        {
            this.ReceiverRegistration = l => l.RegisterConsumer(this.Label, new LambdaConsumerOf<T>(ctx => handlerAction(ctx.Message.Payload)));

            return new TypedReceiverConfigurationDecorator<T>(this);
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith<T>(Action<T, IConsumingContext<T>> handlerAction) where T : class
        {
            this.ReceiverRegistration = l => l.RegisterConsumer(this.Label, new LambdaConsumerOf<T>(ctx => handlerAction(ctx.Message.Payload, ctx)));

            return new TypedReceiverConfigurationDecorator<T>(this);
        }

        /// <summary>������������ ������� ������������ ��������� ���������.</summary>
        /// <param name="consumerFactoryFunc">������� ������������ �������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith<T>(Func<IConsumerOf<T>> consumerFactoryFunc) where T : class
        {
            this.ReceiverRegistration = l => l.RegisterConsumer(this.Label, new FactoryConsumerOf<T>(consumerFactoryFunc));

            return new TypedReceiverConfigurationDecorator<T>(this);
        }

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="consumer">���������� ��������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator<T> ReactWith<T>(IConsumerOf<T> consumer) where T : class
        {
            this.ReceiverRegistration = l => l.RegisterConsumer(this.Label, consumer);

            return new TypedReceiverConfigurationDecorator<T>(this);
        }

        /// <summary>
        /// ������������� ������������� ������ ������������� �������� ��������� ���������.
        /// </summary>
        /// <returns>
        /// ������������ ���������� � ����� �������������� �������� ��������� ���������.
        /// </returns>
        public IReceiverConfigurator RequiresAccept()
        {
            this.Options.AcceptIsRequired = true;

            return this;
        }

        /// <summary>
        /// ��������� ������������.
        /// </summary>
        public virtual void Validate()
        {
            // TODO: this
        }

        /// <summary>
        /// ������������ �������� �������� ��������� ���������.
        /// </summary>
        /// <param name="validator">�������� �������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� �������� �������� ���������.</returns>
        public IReceiverConfigurator WhenVerifiedBy(IMessageValidator validator)
        {
            this.Validator = validator;

            return this;
        }

        /// <summary>
        /// ������������� ��������� �������� ���������.
        /// </summary>
        /// <param name="alias">��������� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� �������� ���������.</returns>
        public IReceiverConfigurator WithAlias(string alias)
        {
            this.Alias = MessageLabel.AliasPrefix + alias;

            return this;
        }

        /// <summary>
        /// ������������ ����������� �����, �� �������� �������� ��������� �������� ���������.
        /// </summary>
        /// <param name="endpointBuilder">����������� ����� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ ����� �������� ���������.</returns>
        public IReceiverConfigurator WithEndpoint(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> endpointBuilder)
        {
            this.Options.EndpointBuilder = endpointBuilder;

            return this;
        }

        /// <summary>
        /// ������������� ���������� ������������� ������������ �������� ���������.
        /// </summary>
        /// <param name="parallelismLevel">���������� ������������� ������������ �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� ������������� ������������ �������� ���������.</returns>
        public IReceiverConfigurator WithParallelismLevel(uint parallelismLevel)
        {
            this.Options.ParallelismLevel = parallelismLevel;

            return this;
        }

        /// <summary>
        /// ������������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ����������.</returns>
        public IReceiverConfigurator WithIncomingMessageHeaderStorage(IIncomingMessageHeaderStorage storage)
        {
            this.Options.IncomingMessageHeaderStorage = new Maybe<IIncomingMessageHeaderStorage>(storage);

            return this;
        }
    }
}
