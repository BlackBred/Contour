using System.Configuration;

namespace Contour.Configurator
{
    /// <summary>
    /// ���������������� ������� ��� ��������� ���������� ������������ �������������.
    /// </summary>
    public class DynamicElement : ConfigurationElement
    {
        /// <summary>
        /// ��������� ������������ ������������� ��� ��������� ���������.
        /// </summary>
        [ConfigurationProperty("outgoing", IsRequired = true)]
        public bool? Outgoing
        {
            get
            {
                return (bool?)(base["outgoing"]);
            }
        }
    }
}
