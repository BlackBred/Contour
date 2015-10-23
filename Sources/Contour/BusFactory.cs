namespace Contour
{
    using System;

    using Contour.Configuration;

    /// <summary>
    ///   ������� ��� �������� ������� ���� ��������� �� ������ ������������.
    /// </summary>
    public class BusFactory
    {
        /// <summary>
        /// ������� ��������� ���� �������
        /// </summary>
        /// <param name="configure">
        /// �������, ������������ ��� ������� �������� ���� �������
        /// </param>
        /// <param name="autoStart">
        /// ��������� ������ ����� ��������
        /// </param>
        /// <returns>
        /// ������������������ ��������� ���� �������
        /// </returns>
        public IBus Create(Action<IBusConfigurator> configure, bool autoStart = true)
        {
            BusConfiguration config = DefaultBusConfigurationBuilder.Build();

            configure(config);

            config.Validate();

            IBus bus = config.BusFactoryFunc(config);

            if (autoStart)
            {
                bus.Start();
            }

            return bus;
        }
    }
}
