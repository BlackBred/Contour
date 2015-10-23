namespace Contour.Configurator.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FluentAssertions;

    using NUnit.Framework;

    /// <summary>
    /// The config section reading specs.
    /// </summary>
    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
    public class ConfigSectionReadingSpecs
    {
        /// <summary>
        /// The when_connection_string_is_not_provided.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_connection_string_is_not_provided
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_throw.
            /// </summary>
            [Test]
            public void should_throw()
            {
                const string config = @"<endpoints>
							<endpoint name=""Tester"" />
						</endpoints>";

                Action readingConfig = () => new XmlEndpointsSection(config);

                readingConfig.ShouldThrow<ConfigurationErrorsException>();
            }

            #endregion
        }

        /// <summary>
        /// The when_declaring_caching_for_endpoint.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_caching_for_endpoint
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_read_configuration_property.
            /// </summary>
            [Test]
            public void should_read_configuration_property()
            {
                const string config = @"<endpoints>
							<endpoint name=""a"" connectionString=""amqp://localhost:666"">
								<caching enabled=""false"" />
							</endpoint>
							<endpoint name=""b"" connectionString=""amqp://localhost:777"">
								<caching enabled=""true"" />
							</endpoint>
						</endpoints>";

                var section = new XmlEndpointsSection(config);

                section.Endpoints["a"].Caching.Enabled.Should().
                    BeFalse();
                section.Endpoints["b"].Caching.Enabled.Should().
                    BeTrue();
            }

            #endregion
        }

        /// <summary>
        /// The when_declaring_complex_configuration.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_complex_configuration
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_read_everything.
            /// </summary>
            [Test(Description = "A shortcut cover-all test. No time. Decompose.")]
            public void should_read_everything()
            {
                const string config = @"<endpoints>
							<endpoint name=""tester"" connectionString=""amqp://localhost:666"">
								<incoming>
									<on key=""a"" label=""msg.a"" react=""DynamicHandler"" requiresAccept=""true"" />
									<on key=""b"" label=""msg.b"" react=""TransformB"" />
								</incoming>
								<outgoing>
									<route key=""a"" label=""msg.out.a"" persist=""true"">
										<callbackEndpoint default=""true"" />
									</route>
								</outgoing>
							</endpoint>
						</endpoints>";

                var section = new XmlEndpointsSection(config);

                section.Endpoints.Should().
                    HaveCount(1);

                EndpointElement endpoint = section.Endpoints["tester"];
                endpoint.Incoming.Should().
                    HaveCount(2);
                endpoint.Outgoing.Should().
                    HaveCount(1);

                List<IncomingElement> incoming = endpoint.Incoming.OfType<IncomingElement>().
                    ToList();

                incoming[0].RequiresAccept.Should().
                    BeTrue();
                incoming[1].RequiresAccept.Should().
                    BeFalse();
            }

            #endregion
        }

        /// <summary>
        /// The when_declaring_endpoint_with_name.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_endpoint_with_name
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_read_endpoint.
            /// </summary>
            [Test]
            public void should_read_endpoint()
            {
                const string config = @"<endpoints>
							<endpoint name=""tester"" connectionString=""amqp://localhost:666"">
							</endpoint>
						</endpoints>";

                var section = new XmlEndpointsSection(config);

                section.Endpoints.Should().
                    HaveCount(1);
                section.Endpoints["tester"].Should().
                    NotBeNull();
            }

            #endregion
        }

        /// <summary>
        /// The when_declaring_endpoint_without_name.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_endpoint_without_name
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_throw.
            /// </summary>
            [Test]
            public void should_throw()
            {
                const string config = @"<endpoints>
							<endpoint connectionString=""amqp://localhost:666"">
							</endpoint>
						</endpoints>";

                Action readingConfig = () => new XmlEndpointsSection(config);

                readingConfig.ShouldThrow<ConfigurationErrorsException>();
            }

            #endregion
        }

        /// <summary>
        /// The when_declaring_multiple_endpoints_with_different_names.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_multiple_endpoints_with_different_names
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_read_endpoints.
            /// </summary>
            [Test]
            public void should_read_endpoints()
            {
                const string config = @"<endpoints>
							<endpoint name=""a"" connectionString=""amqp://localhost:666"">
							</endpoint>
							<endpoint name=""b"" connectionString=""amqp://localhost:777"">
							</endpoint>
						</endpoints>";

                var section = new XmlEndpointsSection(config);

                section.Endpoints.Should().
                    HaveCount(2);
                section.Endpoints["a"].Should().
                    NotBeNull();
                section.Endpoints["b"].Should().
                    NotBeNull();
            }

            #endregion
        }

        /// <summary>
        /// The when_declaring_multiple_endpoints_with_same_name.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_multiple_endpoints_with_same_name
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_throw.
            /// </summary>
            [Test]
            public void should_throw()
            {
                const string config = @"<endpoints>
							<endpoint name=""tester"" connectionString=""amqp://localhost:666"">
							</endpoint>
							<endpoint name=""tester"" connectionString=""amqp://localhost:777"">
							</endpoint>
						</endpoints>";

                Action readingConfig = () => new XmlEndpointsSection(config);

                readingConfig.ShouldThrow<ConfigurationErrorsException>();
            }

            #endregion
        }

        /// <summary>
        /// The when_endpoint_connection_string_is_provided.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_endpoint_connection_string_is_provided
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_read_connection_string.
            /// </summary>
            [Test]
            public void should_read_connection_string()
            {
                const string config = @"<endpoints>
							<endpoint name=""Tester"" connectionString=""amqp://localhost:666"" />
						</endpoints>";

                var section = new XmlEndpointsSection(config);

                section.Endpoints["Tester"].ConnectionString.Should().
                    Be("amqp://localhost:666");
            }

            #endregion
        }

        /// <summary>
        /// The when_endpoint_lifecycle_handler_is_provided.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_endpoint_lifecycle_handler_is_provided
        {
            #region Public Methods and Operators

            /// <summary>
            /// The should_read_handler_name.
            /// </summary>
            [Test]
            public void should_read_handler_name()
            {
                const string config = @"<endpoints>
							<endpoint name=""Tester"" connectionString=""amqp://localhost:666"" lifecycleHandler=""handler"" />
						</endpoints>";

                var section = new XmlEndpointsSection(config);

                section.Endpoints["Tester"].LifecycleHandler.Should().
                    Be("handler");
            }

            #endregion
        }

        /// <summary>
        /// �������� ������������, ������� ��������� ������������ ������ ���������������� �������� QoS.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_qos_for_endpoint
        {
            /// <summary>
            /// ���� ����� QoS ��� �������� �����, ����� ����� �������� �������� �� ������������.
            /// </summary>
            [Test]
            public void should_read_configuration_property()
            {
                const string Config = 
                    @"<endpoints>
                        <endpoint name=""a"" connectionString=""amqp://localhost:666"">
                            <qos prefetchCount=""8"" />
                        </endpoint>
                    </endpoints>";

                var section = new XmlEndpointsSection(Config);
                Assert.AreEqual(8, section.Endpoints["a"].Qos.PrefetchCount, "������ ���� ����������� �������� QoS.");
            }
        }

        /// <summary>
        /// �������� ������������, ������� ��������� ������������ ������ ���������������� �������� ���������� ������������ ���������.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_parallelismlevel_for_endpoint
        {
            /// <summary>
            /// ���� ������ ���������� ������������ ��������� ��� ������ ������� �������� �����, ����� ����� �������� �������� �� ������������.
            /// </summary>
            [Test]
            public void should_read_configuration_property()
            {
                const string Config =
                    @"<endpoints>
                        <endpoint name=""a"" connectionString=""amqp://localhost:666"" parallelismLevel=""8"">
                        </endpoint>
                    </endpoints>";

                var section = new XmlEndpointsSection(Config);
                Assert.AreEqual(8, section.Endpoints["a"].ParallelismLevel, "������ ���� ����������� ���������� ������������ �������.");
            }
        }

        /// <summary>
        /// �������� ������������, ������� ��������� ������������ ������ ���������������� �������� ������������ ������������� ��������� ���������.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class when_declaring_dynamic_outgoing
        {
            /// <summary>
            /// ���� ������ ������������ ������������� ��������� ��������� ��� ������ �������� �����, ����� ����� �������� �������� �� ������������.
            /// </summary>
            [Test]
            public void should_read_configuration_property()
            {
                const string Config =
                    @"<endpoints>
                        <endpoint name=""a"" connectionString=""amqp://localhost:666"">
                            <dynamic outgoing=""true"" />
                        </endpoint>
                    </endpoints>";

                var section = new XmlEndpointsSection(Config);
                Assert.IsNotNull(section.Endpoints["a"].Dynamic.Outgoing, "������ ���� ��������� ��������� ������������ �������������.");
                Assert.IsTrue(section.Endpoints["a"].Dynamic.Outgoing.Value, "������ ���� �������� ������������ ������������� ��������� ���������.");
            }
        }
    }

    // ReSharper restore InconsistentNaming
}
