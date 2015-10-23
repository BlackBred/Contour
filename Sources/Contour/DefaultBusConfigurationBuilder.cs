using Contour.Configuration;
using Contour.Serialization;
using Contour.Transport.RabbitMQ;

namespace Contour
{
    /// <summary>
    /// ����������� ������������ ���� � ����������� �� ���������.
    /// </summary>
    internal static class DefaultBusConfigurationBuilder
    {
        /// <summary>
        /// ������ ������������ ����.
        /// </summary>
        /// <returns>
        /// ������������ ���� � ���������� �� ���������.
        /// </returns>
        public static BusConfiguration Build()
        {
            var c = new BusConfiguration();

            c.UseRabbitMq();
            c.UsePayloadConverter(new JsonNetPayloadConverter());

            // c.EnableCaching();
            return c;
        }
    }
}
