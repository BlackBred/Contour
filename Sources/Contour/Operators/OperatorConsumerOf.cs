using System.Collections.Generic;

using Contour.Helpers;
using Contour.Receiving;
using Contour.Receiving.Consumers;

namespace Contour.Operators
{
    /// <summary>
    /// ����������� ���������, ������� ��������� �������� ��� ��������� ���������.
    /// </summary>
    /// <typeparam name="T">��� ���������.</typeparam>
    public class OperatorConsumerOf<T> : IConsumerOf<T>
        where T : class
    {
        private readonly IMessageOperator @operator;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="OperatorConsumerOf{T}"/>.
        /// </summary>
        /// <param name="operator">
        /// �������� ����������� ��� ��������� ���������.
        /// </param>
        public OperatorConsumerOf(IMessageOperator @operator)
        {
            this.@operator = @operator;
        }

        /// <summary>
        /// ������������ �������� ���������.
        /// </summary>
        /// <param name="context">�������� ��������� ���������.</param>
        public void Handle(IConsumingContext<T> context)
        {
            BusProcessingContext.Current = new BusProcessingContext(((IDeliveryContext)context).Delivery);

            this.@operator
                .Apply(context.Message)
                .ForEach(
                    m =>
                        {
                            var headers = new Dictionary<string, object>(m.Headers);
                            Headers.ApplyBreadcrumbs(headers, context.Bus.Endpoint.Address);
                            Headers.ApplyOriginalMessageId(headers);
                            context.Bus.Emit(m.Label, m.Payload, headers);
                        });

            context.Accept();
        }
    }
}
