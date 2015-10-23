using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Contour.Operators;
using Contour.Testing.Transport.RabbitMq;

using NUnit.Framework;

namespace Contour.RabbitMq.Tests
{
    /// <summary>
    /// ������������ ���������� ���� ���������.
    /// </summary>
    public class OperatorsSpecs
    {
        /// <summary>
        /// ����� ���������� ������ � �������� ������ �� ��������� ��� �������.
        /// </summary>
        [TestFixture]
        [Category("Integration")]
        public class WhenRequestingUsingOperator : RabbitMqFixture
        {
            /// <summary>
            /// ������ ��������� �����.
            /// </summary>
            [Test]
            public void ShouldReturnResponse()
            {
                int result = 0;

                IBus producer = this.StartBus(
                    "producer", 
                    cfg => cfg.Route("dummy.request")
                               .WithDefaultCallbackEndpoint());
                this.StartBus(
                    "consumer", 
                    cfg => cfg.On<DummyRequest>("dummy.request")
                               .ReactWith(
                                   new PipelineConsumerOf<DummyRequest>(
                               new Translator(m => new DummyResponse(((DummyRequest)m.Payload).Num * 2)), 
                               new Reply())));

                Task<int> response = producer.RequestAsync<DummyRequest, DummyResponse>("dummy.request", new DummyRequest(13))
                    .ContinueWith(m => result = m.Result.Num);

                response.Wait(3.Seconds())
                    .Should()
                    .BeTrue();
                result.Should()
                    .Be(26);
            }
        }

        /// <summary>
        /// ����� ������������ ���������� <c>JsonPath</c>.
        /// </summary>
        [TestFixture]
        [Category("Integration")]
        public class WhenFilteringUsingJsonpathExpression : RabbitMqFixture
        {
            /// <summary>
            /// ����� ������ ����������� ���������.
            /// </summary>
            [Test]
            public void ShouldFilter()
            {
                int result = 0;
                CountdownEvent countdown = new CountdownEvent(2);

                IBus producer = this.StartBus(
                    "producer", 
                    cfg => cfg.Route("booz"));
                this.StartBus(
                    "consumer", 
                    cfg => cfg.On<Booz>("booz")
                               .ReactWith(
                                   new PipelineConsumerOf<Booz>(
                               new JsonPathFilter(@"Items[?(@.Num > 12)]"), 
                               new Enricher(
                               message =>
                                   {
                                       Interlocked.Add(ref result, ((Booz)message.Payload).Items[0].Num);
                                       countdown.Signal();
                                   }))));

                producer.Emit("booz", new Booz { Items = new[] { new BooMessage(11) } });
                producer.Emit("booz", new Booz { Items = new[] { new BooMessage(12) } });
                producer.Emit("booz", new Booz { Items = new[] { new BooMessage(13) } });
                producer.Emit("booz", new Booz { Items = new[] { new BooMessage(14) } });

                countdown.Wait(3.Seconds())
                    .Should()
                    .BeTrue();
                result.Should()
                    .Be(13 + 14);
            }

            private class Booz
            {
                /// <summary>
                /// ������ ���������.
                /// </summary>
                public IList<BooMessage> Items { get; set; }
            }
        }

        /// <summary>
        /// ���� ������������ ������������ �������������.
        /// </summary>
        [TestFixture]
        [Category("Integration")]
        public class WhenDynamicRouting : RabbitMqFixture
        {
            /// <summary>
            /// ����� ��������� ������ ������������ �� ������ ��������.
            /// </summary>
            [Test]
            public void ShouldRoute()
            {
                var documentItRouted = "document.it.routed";
                var commandRouteThis = "command.route.this";
                var consumedEvent = new AutoResetEvent(false);
                var storage = new InMemoryKeyValueStorage<ExpandoObject>();
                var router = new DynamicRouter<ExpandoObject>((m, s) => documentItRouted.ToMessageLabel(), storage);
                this.StartBus(
                    "pipeline", 
                    cfg =>
                        {
                            cfg.On(commandRouteThis)
                                .ReactWith(new PipelineConsumerOf<ExpandoObject>(router))
                                .RequiresAccept();
                            cfg.Route(documentItRouted);
                        });

                var producer = this.StartBus(
                    "producer", 
                    cfg => { cfg.Route(commandRouteThis); });

                this.StartBus(
                    "consumer", 
                    cfg =>
                        {
                            cfg.On(documentItRouted)
                                .ReactWith<ExpandoObject>((m, ctx) => { consumedEvent.Set(); });
                        });

                producer.Emit(commandRouteThis, new { });
                var consumed = consumedEvent.WaitOne(5000);
                consumed.Should()
                    .BeTrue("������ ���� �������� ��������� ��������������� ��������.");
            }

            /// <summary>
            /// ����� ����� �������� ������� ���������.
            /// </summary>
            [Test]
            public void ShouldChangeRoute()
            {
                var documentItRouted = "document.it.routed";
                var documentItDirected = "document.it.directed";
                var commandRouteThis = "command.route.this";
                var commandChangeRoute = "command.change.route";
                var key = "key";
                var switchCondition = 4;
                var consumedEvent = new AutoResetEvent(false);
                var directedEvent = new AutoResetEvent(false);
                var storage = new InMemoryKeyValueStorage<ExpandoObject>();
                var router = new DynamicRouter<ExpandoObject>(
                    (m, s) =>
                        {
                            dynamic data = s.Get(key);
                            if (data == null || data.Count < switchCondition)
                            {
                                return documentItRouted.ToMessageLabel();
                            }

                            return documentItDirected.ToMessageLabel();
                        }, 
                    storage);
                var control = new DynamicRouter<ExpandoObject>.DynamicRouterControlConsumer<ExpandoObject>(
                    (message, routeStorage) =>
                        {
                            dynamic data = routeStorage.Get(key);
                            if (data == null)
                            {
                                dynamic item = new ExpandoObject();
                                item.Count = 1;
                                routeStorage.Set(key, item);
                                directedEvent.Set();
                                return;
                            }

                            data.Count++;
                            routeStorage.Set(key, data);
                            directedEvent.Set();
                        }, 
                    storage);
                this.StartBus(
                    "pipeline", 
                    cfg =>
                        {
                            cfg.On(commandRouteThis)
                                .ReactWith(new PipelineConsumerOf<ExpandoObject>(router))
                                .RequiresAccept();
                            cfg.On(commandChangeRoute)
                                .ReactWith(control);
                            cfg.Route(documentItRouted);
                            cfg.Route(documentItDirected);
                        });

                var producer = this.StartBus(
                    "producer", 
                    cfg => { cfg.Route(commandRouteThis); });

                this.StartBus(
                    "director", 
                    cfg =>
                        {
                            cfg.On(documentItRouted)
                                .ReactWith<ExpandoObject>(
                                    (m, ctx) => { ctx.Bus.Emit(commandChangeRoute, new { }); });
                            cfg.Route(commandChangeRoute);
                        });

                this.StartBus(
                    "consumer", 
                    cfg =>
                        {
                            cfg.On(documentItDirected)
                                .ReactWith<ExpandoObject>((m, ctx) => { consumedEvent.Set(); });
                        });

                for (int i = 0; i <= switchCondition; i++)
                {
                    producer.Emit(commandRouteThis, new { });

                    if (i < switchCondition)
                    {
                        directedEvent.WaitOne(5000).Should().BeTrue("��������� ������ ���� ����������.");
                    }
                }

                consumedEvent.WaitOne(5000).Should().BeTrue("������ ���� �������� ��������� ��������������� ��������.");
            }
        }

        /// <summary>
        /// � ������ ������������� ������������ ����������.
        /// </summary>
        [TestFixture]
        [Category("Integration")]
        public class WhenDynamicFilter : RabbitMqFixture
        {
            /// <summary>
            /// ������ ����������� �������� ���������.
            /// </summary>
            [Test]
            public void ShouldFilter()
            {
                var documentItFiltered = "document.it.filtered";
                var commandFilterThis = "command.filter.this";
                var consumedEvent = new AutoResetEvent(false);
                var storage = new InMemoryKeyValueStorage<ExpandoObject>();
                var filter = new DynamicFilter<ExpandoObject>((m, s) => true, storage);
                this.StartBus(
                    "pipeline", 
                    cfg =>
                        {
                            cfg.On(commandFilterThis)
                                .ReactWith(new PipelineConsumerOf<ExpandoObject>(filter, new StaticRouter(documentItFiltered)))
                                .RequiresAccept();
                            cfg.Route(documentItFiltered);
                        });

                var producer = this.StartBus(
                    "producer", 
                    cfg => { cfg.Route(commandFilterThis); });

                this.StartBus(
                    "consumer", 
                    cfg =>
                        {
                            cfg.On(documentItFiltered)
                                .ReactWith<ExpandoObject>((m, ctx) => { consumedEvent.Set(); });
                        });

                producer.Emit(commandFilterThis, new { });
                var consumed = consumedEvent.WaitOne(5000);
                consumed.Should().BeTrue("������ ���� �������� ��������� ������������ �������.");
            }

            /// <summary>
            /// ������ ��������� ������� ����������.
            /// </summary>
            [Test]
            public void ShouldChangeFilterCondition()
            {
                var documentItFiltered = "document.it.fitered";
                var commandFilterThis = "command.filter.this";
                var commandChangeFilter = "command.change.filter";
                var key = "key";
                var switchCondition = 4;
                var callCount = 0;
                var filteredEvent = new AutoResetEvent(false);
                var consumedEvent = new ManualResetEvent(false);
                var storage = new InMemoryKeyValueStorage<ExpandoObject>();
                var filter = new DynamicFilter<ExpandoObject>(
                    (m, s) =>
                        {
                            dynamic data = s.Get(key);
                            filteredEvent.Set();
                            return !(data == null || data.Count < switchCondition);
                        }, 
                    storage);
                var control = new DynamicRouter<ExpandoObject>.DynamicRouterControlConsumer<ExpandoObject>(
                    (message, filterStorage) =>
                        {
                            dynamic data = filterStorage.Get(key);
                            if (data == null)
                            {
                                dynamic item = new ExpandoObject();
                                item.Count = 1;
                                filterStorage.Set(key, item);
                                filteredEvent.Set();
                                return;
                            }

                            data.Count++;
                            filterStorage.Set(key, data);
                            filteredEvent.Set();
                        }, 
                    storage);
                this.StartBus(
                    "pipeline", 
                    cfg =>
                        {
                            cfg.On(commandFilterThis)
                                .ReactWith(new PipelineConsumerOf<ExpandoObject>(filter, new StaticRouter(documentItFiltered)))
                                .RequiresAccept();
                            cfg.On(commandChangeFilter)
                                .ReactWith(control);
                            cfg.Route(documentItFiltered);
                        });

                var producer = this.StartBus(
                    "producer", 
                    cfg =>
                        {
                            cfg.Route(commandFilterThis);
                            cfg.Route(commandChangeFilter);
                        });

                this.StartBus(
                    "consumer", 
                    cfg =>
                        {
                            cfg.On(documentItFiltered)
                                .ReactWith<ExpandoObject>(
                                    (m, ctx) =>
                                        {
                                            callCount++;
                                            consumedEvent.Set();
                                        });
                        });

                for (int i = 0; i <= switchCondition; i++)
                {
                    producer.Emit(commandFilterThis, new { });
                    filteredEvent.WaitOne();
                    producer.Emit(commandChangeFilter, new { });
                    filteredEvent.WaitOne();
                }

                consumedEvent.WaitOne().Should().BeTrue("������ ���� �������� ��������� ��������������� ��������.");
                callCount.Should().Be(1, "��������� ������ ���� �������� ������ ���� ���.");
            }
        }

        /// <summary>
        /// ��������� ��� ����-�������� � ������ ��������.
        /// </summary>
        /// <typeparam name="T">
        /// ��� ������� �������������.
        /// </typeparam>
        internal class InMemoryKeyValueStorage<T> : IKeyValueStorage<T>
            where T : class
        {
            private readonly ConcurrentDictionary<string, T> storage = new ConcurrentDictionary<string, T>();

            /// <summary>
            /// ���������� �������� �� �����.
            /// </summary>
            /// <param name="key">
            /// ����, �� �������� �������� ��������..
            /// </param>
            /// <returns>
            /// �������� ��� <c>null</c>, ���� �� ����� ������ �� �������.
            /// </returns>
            public T Get(string key)
            {
                T value;
                this.storage.TryGetValue(key, out value);
                return value;
            }

            /// <summary>
            /// ��������� �������� � ��������� ������.
            /// </summary>
            /// <param name="key">
            /// ���� ��������.
            /// </param>
            /// <param name="value">
            /// ����������� ��������.
            /// </param>
            public void Set(string key, T value)
            {
                this.storage.AddOrUpdate(key, value, (s, o) => value);
            }
        }
    }
}
