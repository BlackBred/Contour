namespace Contour.Configurator
{
    using System.Configuration;

    /// <summary>
    /// ������� ������������ �������� �������� �����������.
    /// </summary>
    internal class CachingElement : ConfigurationElement
    {
        /// <summary>
        /// ������� ��������� ��� ���������� �����������.
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get
            {
                return (bool)(base["enabled"]);
            }
        }
    }
}
