using System;
using System.Collections.Generic;

using Contour.Receiving;
using Contour.Receiving.Consumers;

namespace Contour.Operators
{
    /// <summary>
    /// ������������ �������������. 
    /// ��������� ���������� �������� �� ������ ���������������� ��������� �� ������ ����������.
    /// <a href="http://www.eaipatterns.com/DynamicRouter.html"><c>Dynamic Router</c></a>.
    /// </summary>
    /// <typeparam name="T">��� ������������ �������.</typeparam>
    internal class DynamicRouter<T> : IMessageOperator
        where T : class
    {
        private readonly Func<IMessage, IKeyValueStorage<T>, MessageLabel> routeResolverFunc;

        private readonly IKeyValueStorage<T> storage;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DynamicRouter{T}"/>. 
        /// </summary>
        /// <param name="routeResolverFunc">������� ���������� ������������ ������� �������������.</param>
        /// <param name="storage">��������� ������ �������������.</param>
        public DynamicRouter(Func<IMessage, IKeyValueStorage<T>, MessageLabel> routeResolverFunc, IKeyValueStorage<T> storage)
        {
            this.routeResolverFunc = routeResolverFunc;
            this.storage = storage;
        }

        /// <summary>
        /// ������������ �������� ���������, ��������� ��� ���� ����� ������� � ���������� ��� ��������� ���������.
        /// </summary>
        /// <param name="message">�������� ���������.</param>
        /// <returns>��������� � ����� ������.</returns>
        public IEnumerable<IMessage> Apply(IMessage message)
        {
            yield return message.WithLabel(this.routeResolverFunc(message, this.storage));
        }

        /// <summary>
        /// ���������� ����������� ���������.
        /// </summary>
        /// <typeparam name="TV">��� ������� �������������.</typeparam>
        public class DynamicRouterControlConsumer<TV> : IConsumerOf<T>
            where TV : class
        {
            private readonly Action<IMessage, IKeyValueStorage<TV>> createRoute;

            private readonly IKeyValueStorage<TV> storage;

            /// <summary>
            /// �������������� ����� ��������� ������ <see cref="DynamicRouterControlConsumer{TV}"/>. 
            /// </summary>
            /// <param name="createRoute"> ������� ���������� ������� �������������. </param>
            /// <param name="storage"> ��������� ������ �������������. </param>
            public DynamicRouterControlConsumer(Action<IMessage, IKeyValueStorage<TV>> createRoute, IKeyValueStorage<TV> storage)
            {
                this.createRoute = createRoute;
                this.storage = storage;
            }

            /// <summary>
            /// ������������ ����������� ��������� � ����� ������������� ������ �������������. 
            /// </summary>
            /// <param name="context">�������� ����������� ���������.</param>
            public void Handle(IConsumingContext<T> context)
            {
                this.createRoute(context.Message, this.storage);
                context.Accept();
            }
        }
    }
}
