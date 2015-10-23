using System;
using System.Collections.Generic;
using System.Linq;

namespace Contour.Operators
{
    /// <summary>
    /// ��������, ������� �������������� ������� ����������� ������������ ����������� (���������� ������� <see href="http://www.eaipatterns.com/RecipientList.html"/>).
    /// �������� ����������� �������� ���������, ���������� ������ ������ ����������� � �������������� ��������� ��.
    /// </summary>
    public class RecipientList : IMessageOperator
    {
        private readonly Func<IMessage, MessageLabel[]> determineRecipientList;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="RecipientList"/>.
        /// </summary>
        /// <param name="determineRecipientList">������� ����������� ������ ����������� ���������.</param>
        public RecipientList(Func<IMessage, MessageLabel[]> determineRecipientList)
        {
            this.determineRecipientList = determineRecipientList;
        }

        /// <summary>
        /// ������������ �������� ���������, ���������� ������ ����������� � �������� ��������� ���� �����������.
        /// </summary>
        /// <param name="message">�������� ���������.</param>
        /// <returns>��������� ��� �����������, ������������ �� ������ ����������� ��������� ���������.</returns>
        public virtual IEnumerable<IMessage> Apply(IMessage message)
        {
            return this.determineRecipientList(message).Select(message.WithLabel);
        }
    }
}
