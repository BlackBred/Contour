using System.Configuration;

namespace Contour.Configurator
{
    /// <summary>
    /// ���������������� ������� ��� ��������� ���������� <c>QoS</c> (<a href="http://www.rabbitmq.com/blog/2012/05/11/some-queuing-theory-throughput-latency-and-bandwidth/"><c>QoS</c></a>).
    /// </summary>
    public class QosElement : ConfigurationElement
    {
        /// <summary>
        /// ���������� ����������� ��������� �� �������.
        /// </summary>
        [ConfigurationProperty("prefetchCount", IsRequired = true)]
        public ushort? PrefetchCount
        {
            get
            {
                return (ushort?)base["prefetchCount"];
            }
        }
    }
}
