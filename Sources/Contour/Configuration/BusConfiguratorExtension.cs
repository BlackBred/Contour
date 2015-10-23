using System;

using Contour.Receiving.Sagas;

namespace Contour.Configuration
{
    /// <summary>
    /// ����� ���������� ���������� <see cref="IBusConfigurator"/>.
    /// ����� ��� ����, ����� ������ ��������� ��� ���������� ������, �� ���� ����������� �������� � ���� ������ ����������.
    /// ��� ���������� ������ ����������� � ��������� <see cref="IBusConfigurator"/> � ��� ����������.
    /// </summary>
    internal static class BusConfiguratorExtension
    {
        public static ISagaConfigurator<TS, TM, TK> AsSagaStep<TS, TM, TK>(this IBusConfigurator busConfigurator, MessageLabel label) where TM : class
        {
            var receiverConfigurator = busConfigurator.On<TM>(label);

            return new SagaConfiguration<TS, TM, TK>(
                receiverConfigurator,
                SingletonSagaRepository<TS, TK>.Instance,
                new CorrelationIdSeparator<TM, TK>(),
                SingletonSagaFactory<TS, TK>.Instance, 
                new LambdaSagaStep<TS, TM, TK>((sc, cc) => { }), 
                new LambdaFailedHandler<TS, TM, TK>(cc => { throw new ArgumentException("���� �� �������."); }, (sc, cc, e) => { }));
        }
    }
}
