namespace Contour.Configurator
{
    using System.Configuration;

    /// <summary>
    /// The endpoint element.
    /// </summary>
    internal class EndpointElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the caching.
        /// </summary>
        [ConfigurationProperty("caching")]
        public CachingElement Caching
        {
            get
            {
                return (CachingElement)base["caching"];
            }
        }

        /// <summary>
        /// ��������� QoS ��� �������� �����.
        /// </summary>
        [ConfigurationProperty("qos")]
        public QosElement Qos
        {
            get
            {
                return (QosElement)base["qos"];
            }
        }

        /// <summary>
        /// ��������� ������������ �������������.
        /// </summary>
        [ConfigurationProperty("dynamic")]
        public DynamicElement Dynamic
        {
            get
            {
                return (DynamicElement)base["dynamic"];
            }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return (string)this["connectionString"];
            }

            set
            {
                this["connectionString"] = value;
            }
        }

        /// <summary>
        /// ���������� ������������� ������������ ��������� �� �������� �������� �����, ������� ������� �������� ���������.
        /// </summary>
        [ConfigurationProperty("parallelismLevel", IsRequired = false)]
        public uint? ParallelismLevel
        {
            get
            {
                return (uint?)this["parallelismLevel"];
            }

            set
            {
                this["parallelismLevel"] = value;
            }
        }

        /// <summary>
        /// Gets the incoming.
        /// </summary>
        [ConfigurationProperty("incoming")]
        public IncomingCollection Incoming
        {
            get
            {
                return (IncomingCollection)base["incoming"];
            }
        }

        /// <summary>
        /// Gets or sets the lifecycle handler.
        /// </summary>
        [ConfigurationProperty("lifecycleHandler")]
        public string LifecycleHandler
        {
            get
            {
                return (string)this["lifecycleHandler"];
            }

            set
            {
                this["lifecycleHandler"] = value;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        /// <summary>
        /// Gets the outgoing.
        /// </summary>
        [ConfigurationProperty("outgoing")]
        public OutgoingCollection Outgoing
        {
            get
            {
                return (OutgoingCollection)base["outgoing"];
            }
        }

        /// <summary>
        /// Gets the validators.
        /// </summary>
        [ConfigurationProperty("validators")]
        public ValidatorCollection Validators
        {
            get
            {
                return (ValidatorCollection)base["validators"];
            }
        }
    }
}
