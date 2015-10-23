namespace Contour.Configuration
{
    using System;

    using Contour.Filters;
    using Contour.Receiving;
    using Contour.Sending;
    using Contour.Serialization;
    using Contour.Validation;

    /// <summary>
    ///   ��������� ��� ���������������� ���������� ���� �������
    /// </summary>
    public interface IBusConfigurator
    {
        /// <summary>
        /// The enable caching.
        /// </summary>
        void EnableCaching();

        /// <summary>
        /// ������������� ���������� ���������� ����� �������� �����.
        /// </summary>
        /// <param name="lifecycleHandler">
        /// ���������� ���������� �����.
        /// </param>
        void HandleLifecycleWith(IBusLifecycleHandler lifecycleHandler);

        /// <summary>
        /// ������������ ����������� ��������� ���������� ����.
        /// </summary>
        /// <remarks>
        /// �������� ������ ���� ���������� ��������� ���������� ����.
        /// </remarks>
        /// <typeparam name="T">
        /// .NET ��� ����������� ���������.
        /// </typeparam>
        /// <param name="label">
        /// ��� ��������� (��������� ��������).
        /// </param>
        /// <returns>
        /// ������������ ���������� ���� ���������.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// ���������� ���������� � ������ ��������� ����������� ���������� ��
        ///   ��������� ��� ���������.
        /// </exception>
        IReceiverConfigurator<T> On<T>(string label) where T : class;

        /// <summary>
        /// ������������ ����������� ��������� ���������� ����.
        /// </summary>
        /// <remarks>
        /// �������� ������ ���� ���������� ��������� ���������� ����.
        /// </remarks>
        /// <typeparam name="T">
        /// .NET ��� ����������� ���������.
        /// </typeparam>
        /// <param name="label">
        /// ��� ���������.
        /// </param>
        /// <returns>
        /// ������������ ���������� ���� ���������.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// ���������� ���������� � ������ ��������� ����������� ���������� ��
        ///   ��������� ��� ���������.
        /// </exception>
        IReceiverConfigurator<T> On<T>(MessageLabel label) where T : class;

        /// <summary>
        ///   ������������ ����������� ���������, � ������ ��������� � ��������� ������.
        /// </summary>
        /// <remarks>
        ///   �������� ������ ���� ���������� ��������� ���������� ����.
        /// </remarks>
        /// <typeparam name="T">.NET ��� ����������� ���������.</typeparam>
        /// <returns>������������ ���������� ���� ���������.</returns>
        /// <exception cref="InvalidOperationException">
        ///   ���������� ���������� � ������ ��������� ����������� ���������� ��
        ///   ��������� ��� ���������.
        /// </exception>
        IReceiverConfigurator<T> On<T>() where T : class;

        /// <summary>
        /// ������������ ����������� ��������� ���������� ����.
        /// </summary>
        /// <remarks>
        /// �������� ������ ���� ���������� ��������� ���������� ����.
        /// </remarks>
        /// <param name="label">
        /// ��� ���������.
        /// </param>
        /// <returns>
        /// ������������ ���������� ���� ���������.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// ���������� ���������� � ������ ��������� ����������� ���������� ��
        ///   ��������� ��� ���������.
        /// </exception>
        IReceiverConfigurator On(string label);

        /// <summary>
        /// ������������ ����������� ��������� ���������� ����.
        /// </summary>
        /// <remarks>
        /// �������� ������ ���� ���������� ��������� ���������� ����.
        /// </remarks>
        /// <param name="label">
        /// ��� ���������.
        /// </param>
        /// <returns>
        /// ������������ ���������� ���� ���������.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// ���������� ���������� � ������ ��������� ����������� ���������� ��
        ///   ��������� ��� ���������.
        /// </exception>
        IReceiverConfigurator On(MessageLabel label);

        /// <summary>
        /// ������������� ���������� ������������ � ������� ���������.
        /// </summary>
        /// <param name="failedDeliveryStrategy">
        /// ��������� ��������� ����������� ������������ ���������.
        /// </param>
        void OnFailed(IFailedDeliveryStrategy failedDeliveryStrategy);

        /// <summary>
        /// ������������� ���������� ������������ � ������� ���������.
        /// </summary>
        /// <param name="failedDeliveryHandler">
        /// ���������� ����������� ������������ ���������.
        /// </param>
        void OnFailed(Action<IFailedConsumingContext> failedDeliveryHandler);

        /// <summary>
        /// ������������� ���������� �������������� (� ������������� ���������) ���������.
        /// </summary>
        /// <param name="unhandledDeliveryStrategy">
        /// ��������� ��������� �������������� ���������.
        /// </param>
        void OnUnhandled(IUnhandledDeliveryStrategy unhandledDeliveryStrategy);

        /// <summary>
        /// ������������� ���������� �������������� (� ������������� ���������) ���������.
        /// </summary>
        /// <param name="unhandledDeliveryHandler">
        /// ���������� ��������������� ���������.
        /// </param>
        void OnUnhandled(Action<IFaultedConsumingContext> unhandledDeliveryHandler);

        /// <summary>
        /// The register filter.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        void RegisterFilter(IMessageExchangeFilter filter);

        /// <summary>
        /// ������������ ���������� ��������� ���� ���������.
        /// </summary>
        /// <param name="validator">
        /// ���������.
        /// </param>
        void RegisterValidator(IMessageValidator validator);

        /// <summary>
        /// ������������ ������ ����������� ���� ���������.
        /// </summary>
        /// <param name="validatorGroup">
        /// ������ �����������.
        /// </param>
        void RegisterValidators(MessageValidatorGroup validatorGroup);

        /// <summary>
        /// ������������ ��������� ���������� ����.
        /// </summary>
        /// <remarks>
        /// ���������� ���������������� ��� ��������� ���������.
        /// </remarks>
        /// <param name="label">
        /// ��� ��������� (��������� ���������).
        /// </param>
        /// <returns>
        /// ������������ ���������� ���� ���������.
        /// </returns>
        ISenderConfigurator Route(string label);

        /// <summary>
        /// ������������ ��������� ���������� ����.
        /// </summary>
        /// <remarks>
        /// ���������� ���������������� ��� ��������� ���������.
        /// </remarks>
        /// <param name="label">
        /// ��� ���������.
        /// </param>
        /// <returns>
        /// ������������ ���������� ���� ���������.
        /// </returns>
        ISenderConfigurator Route(MessageLabel label);

        /// <summary>
        ///   ������������ ���������, ������� ����� �� ��������� ���������� ����.
        /// </summary>
        /// <remarks>
        ///   ���������� ���������������� ��� ��������� ���������.
        /// </remarks>
        /// <returns>������������ ���������� ���� ���������.</returns>
        ISenderConfigurator Route<T>() where T : class;

        /// <summary>
        /// ������������� ������ ���������� � ��������.
        /// </summary>
        /// <param name="connectionString">
        /// ������ ����������� � �������.
        /// </param>
        void SetConnectionString(string connectionString);

        /// <summary>
        /// ������������� ������ ���������� �� ����� �� ����������������� �����.
        /// </summary>
        /// <param name="connectionStringName">
        /// ��� ������ ����������.
        /// </param>
        void SetConnectionStringName(string connectionStringName);

        /// <summary>
        /// ������������� ����� �������� ����� ����������.
        /// </summary>
        /// <param name="address">
        /// ����� �������� ����� ����������.
        /// </param>
        void SetEndpoint(string address);

        /// <summary>
        /// ������������� ������� ������������ (���������� ������� ��������� ���������) �� ���������.
        /// </summary>
        /// <param name="parallelismLevel">
        /// ������� ������������.
        /// </param>
        void UseParallelismLevel(uint parallelismLevel);

        /// <summary>
        /// �������������� ��������� ���� ���������.
        /// </summary>
        /// <param name="converter">
        /// </param>
        void UsePayloadConverter(IPayloadConverter converter);

        /// <summary>
        /// ������������� ������� ��� �������� �� ���������.
        /// </summary>
        /// <param name="timeout">
        /// �������.
        /// </param>
        void UseRequestTimeout(TimeSpan? timeout);

        /// <summary>
        /// ������������� ������������
        /// </summary>
        /// <param name="routeResolverBuilder">
        /// </param>
        void UseRouteResolverBuilder(Func<IRouteResolverBuilder, IRouteResolver> routeResolverBuilder);

        /// <summary>
        /// ������������� ���������� ����� ���������.
        /// </summary>
        /// <summary>
        /// ������������� ������������ �������� ����� �������� �� ���������.
        /// </summary>
        /// <param name="endpointBuilder">
        /// </param>
        void UseSubscriptionEndpointBuilder(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> endpointBuilder);
    }
}
