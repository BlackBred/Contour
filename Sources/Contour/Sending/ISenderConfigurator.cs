using System;

using Contour.Receiving;

namespace Contour.Sending
{
    /// <summary>
    /// ������������ �����������.
    /// </summary>
    public interface ISenderConfigurator
    {
        /// <summary>
        /// ������� ������������ �� ������ ����������� ����������� ���������.
        /// </summary>
        /// <param name="routeResolverBuilder">
        /// ����������� ����������� ���������.
        /// </param>
        /// <returns>
        /// ������������ �����������.
        /// </returns>
        ISenderConfigurator ConfiguredWith(Func<IRouteResolverBuilder, IRouteResolver> routeResolverBuilder);

        /// <summary>
        /// ��������� ������ ����������� �� ���� ��� �������� ��������.
        /// </summary>
        /// <returns>
        /// ������������ �����������.
        /// </returns>
        ISenderConfigurator Persistently();

        /// <summary>
        /// ������������� ��������� ����� ������������� ���������.
        /// </summary>
        /// <param name="alias">��������� ����� ������������� ���������.</param>
        /// <returns>������������ �����������.</returns>
        ISenderConfigurator WithAlias(string alias);

        /// <summary>
        /// ������������� ����������� �������� ����� ��������� ������.
        /// </summary>
        /// <param name="callbackEndpointBuilder">����������� �������� ����� ��� ��������� �������� ���������.</param>
        /// <returns>������������ �����������.</returns>
        ISenderConfigurator WithCallbackEndpoint(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> callbackEndpointBuilder);

        /// <summary>
        /// �������������, ��� ��������� ������������� ��������� ��������� ��������.
        /// </summary>
        /// <returns>������������ �����������.</returns>
        ISenderConfigurator WithConfirmation();

        /// <summary>
        /// ��� ��������� ��������� ��������� ������ �������������� ����� ��������, ����������� �� ��������� ������������ �����������.
        /// </summary>
        /// <returns>������������ �����������.</returns>
        ISenderConfigurator WithDefaultCallbackEndpoint();

        /// <summary>
        /// ������������� ������������ ����� �������� ������ �� ������.
        /// </summary>
        /// <param name="timeout">����� �������� ������ �� ������.</param>
        /// <returns>������������ �����������.</returns>
        ISenderConfigurator WithRequestTimeout(TimeSpan? timeout);

        /// <summary>
        /// ������������� TTL (����� �����) ��� ������������ ���������.
        /// </summary>
        /// <param name="ttl">�������� ����� ����� ���������.</param>
        /// <returns>������������ �����������.</returns>
        ISenderConfigurator WithTtl(TimeSpan ttl);

        /// <summary>
        /// ������������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ��������� ���������.</param>
        /// <returns>������������ ����������� � ������������� ��������� ���������� ��������� ���������.</returns>
        ISenderConfigurator WithIncomingMessageHeaderStorage(IIncomingMessageHeaderStorage storage);
    }
}
