using System;

using Contour.Receiving.Consumers;
using Contour.Validation;

namespace Contour.Receiving
{
    /// <summary>
    /// ������������ ���������� ���������.
    /// </summary>
    public interface IReceiverConfigurator
    {
        /// <summary>
        /// ���������� �������������� ������������ ����������.
        /// </summary>
        /// <typeparam name="T">��� ������������� ����������.</typeparam>
        /// <returns>������������ ����������.</returns>
        IReceiverConfigurator<T> As<T>() where T : class;

        /// <summary>
        /// ������������ ��������� ��������� ���������, ��������� ������� ����������� ��������.
        /// </summary>
        /// <param name="failedDeliveryStrategy">��������� ��������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������.</returns>
        IReceiverConfigurator OnFailed(IFailedDeliveryStrategy failedDeliveryStrategy);

        /// <summary>������������ ���������� ���������, ��������� ������� ����������� ��������.</summary>
        /// <param name="failedDeliveryHandler">���������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������</returns>
        IReceiverConfigurator OnFailed(Action<IFailedConsumingContext> failedDeliveryHandler);

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith<T>(Action<T> handlerAction) where T : class;

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith<T>(Action<T, IConsumingContext<T>> handlerAction) where T : class;

        /// <summary>������������ ������� ������������ ��������� ���������.</summary>
        /// <param name="consumerFactoryFunc">������� ������������ �������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith<T>(Func<IConsumerOf<T>> consumerFactoryFunc) where T : class;

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="consumer">���������� ��������� ���������.</param>
        /// <typeparam name="T">��� ��������������� ���������.</typeparam>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith<T>(IConsumerOf<T> consumer) where T : class;

        /// <summary>
        /// ������������� ������������� ������ ������������� �������� ��������� ���������.
        /// </summary>
        /// <returns>
        /// ������������ ���������� � ����� �������������� �������� ��������� ���������.
        /// </returns>
        IReceiverConfigurator RequiresAccept();

        /// <summary>
        /// ������������ �������� �������� ��������� ���������.
        /// </summary>
        /// <param name="validator">�������� �������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� �������� �������� ���������.</returns>
        IReceiverConfigurator WhenVerifiedBy(IMessageValidator validator);

        /// <summary>
        /// ������������� ��������� �������� ���������.
        /// </summary>
        /// <param name="alias">��������� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� �������� ���������.</returns>
        IReceiverConfigurator WithAlias(string alias);

        /// <summary>
        /// ������������ ����������� �����, �� �������� �������� ��������� �������� ���������.
        /// </summary>
        /// <param name="endpointBuilder">����������� ����� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ ����� �������� ���������.</returns>
        IReceiverConfigurator WithEndpoint(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> endpointBuilder);

        /// <summary>
        /// ������������� ���������� ������������� ������������ �������� ���������.
        /// </summary>
        /// <param name="parallelismLevel">���������� ������������� ������������ �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator WithParallelismLevel(uint parallelismLevel);

        /// <summary>
        /// ������������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ����������.</returns>
        IReceiverConfigurator WithIncomingMessageHeaderStorage(IIncomingMessageHeaderStorage storage);
    }

    /// <summary>
    /// ������������ ���������� ���������.
    /// </summary>
    /// <typeparam name="T">��� ����������� ���������.</typeparam>
    public interface IReceiverConfigurator<T> : IReceiverConfigurator
        where T : class
    {
        /// <summary>
        /// ������������ ��������� ��������� ���������, ��������� ������� ����������� ��������.
        /// </summary>
        /// <param name="failedDeliveryStrategy">��������� ��������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������.</returns>
        new IReceiverConfigurator<T> OnFailed(IFailedDeliveryStrategy failedDeliveryStrategy);

        /// <summary>������������ ���������� ���������, ��������� ������� ����������� ��������.</summary>
        /// <param name="failedDeliveryHandler">���������� ���������, ��������� ������� ����������� ��������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ��������� ���������</returns>
        new IReceiverConfigurator<T> OnFailed(Action<IFailedConsumingContext> failedDeliveryHandler);

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith(Action<T> handlerAction);

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="handlerAction">���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith(Action<T, IConsumingContext<T>> handlerAction);

        /// <summary>������������ ������� ������������ ��������� ���������.</summary>
        /// <param name="consumerFactoryFunc">������� ������������ �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith(Func<IConsumerOf<T>> consumerFactoryFunc);

        /// <summary>������������ ���������� ��������� ���������.</summary>
        /// <param name="consumer">���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ �������� ���������.</returns>
        IReceiverConfigurator<T> ReactWith(IConsumerOf<T> consumer);

        /// <summary>
        /// ������������� ������������� ������ ������������� �������� ��������� ���������.
        /// </summary>
        /// <returns>
        /// ������������ ���������� � ����� �������������� �������� ��������� ���������.
        /// </returns>
        new IReceiverConfigurator<T> RequiresAccept();

        /// <summary>
        /// ������������ �������� �������� ��������� ���������.
        /// </summary>
        /// <param name="validator">�������� �������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� �������� �������� ���������.</returns>
        IReceiverConfigurator<T> WhenVerifiedBy(IMessageValidatorOf<T> validator);

        /// <summary>
        /// ������������� ��������� �������� ���������.
        /// </summary>
        /// <param name="alias">��������� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� �������� ���������.</returns>
        new IReceiverConfigurator<T> WithAlias(string alias);

        /// <summary>
        /// ������������ ����������� �����, �� �������� �������� ��������� �������� ���������.
        /// </summary>
        /// <param name="endpointBuilder">����������� ����� �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ������������ ����� �������� ���������.</returns>
        new IReceiverConfigurator<T> WithEndpoint(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> endpointBuilder);

        /// <summary>
        /// ������������� ���������� ������������� ������������ �������� ���������.
        /// </summary>
        /// <param name="parallelismLevel">���������� ������������� ������������ �������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ����������� ������������� ������������ �������� ���������.</returns>
        new IReceiverConfigurator<T> WithParallelismLevel(uint parallelismLevel);

        /// <summary>
        /// ������������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ��������� ���������.</param>
        /// <returns>������������ ���������� � ������������� ���������� ����������.</returns>
        new IReceiverConfigurator WithIncomingMessageHeaderStorage(IIncomingMessageHeaderStorage storage);
    }
}
