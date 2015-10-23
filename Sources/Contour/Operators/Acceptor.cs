using System.Collections.Generic;

namespace Contour.Operators
{
    /// <summary>
    /// �������� ��������� ��� ����������� � ������, ���� ����� ����� ������������� ��������� ���������.
    /// ����� ������������� ��������� ���������� ������.
    /// </summary>
    public class Acceptor : IMessageOperator
    {
        /// <summary>
        /// �������� ��������� ��� �����������.
        /// </summary>
        /// <param name="message">�������� ���������, ������� ����� �������� ��� ������������.</param>
        /// <returns>�������� ���������, ���������� ��� ������������.</returns>
        public IEnumerable<IMessage> Apply(IMessage message)
        {
            BusProcessingContext.Current.Delivery.Accept();
            yield return message;
        }
    }
}
