namespace Contour.Configurator
{
    using Contour.Configuration;

    /// <summary>
    ///   ������������� ���������� �� ���������������� IBus
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// ������������� service bus. ������������� ������� ��� �������� ���������.
        /// </summary>
        /// <param name="endpointName">
        /// </param>
        /// <param name="currentConfiguration">
        /// </param>
        /// <returns>
        /// The <see cref="IBusConfigurator"/>.
        /// </returns>
        IBusConfigurator Configure(string endpointName, IBusConfigurator currentConfiguration);

        /// <summary>
        /// �������� ��� ��������� �� ��� �����
        /// </summary>
        /// <param name="endpointName">
        /// </param>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetEvent(string endpointName, string key);

        /// <summary>
        /// The get request config.
        /// </summary>
        /// <param name="endpointName">
        /// The endpoint name.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="IRequestConfiguration"/>.
        /// </returns>
        IRequestConfiguration GetRequestConfig(string endpointName, string key);
    }
}
