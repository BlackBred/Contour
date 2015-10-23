using System.Collections.Generic;

namespace Contour.Operators
{
    /// <summary>
    /// ��������, ������� �������� ����� �� ������ � �������� ��������� ������.
    /// </summary>
    public class TransparentReply : IMessageOperator
    {
        /// <summary>
        /// �������� �� ������ � �������� ��������� ������.
        /// </summary>
        /// <param name="message">�������� ���������.</param>
        /// <returns>�������� ��������� ��� ��������� <c>ReplyRoute</c>.</returns>
        public IEnumerable<IMessage> Apply(IMessage message)
        {
            message.Headers.Remove(Headers.ReplyRoute);
            BusProcessingContext.Current.Delivery.ReplyWith(message);
            yield return message;
        }
    }
}
