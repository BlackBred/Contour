using System;

namespace Contour.RabbitMq.Tests
{
    /// <summary>
    /// The wrong boo message.
    /// </summary>
    public class WrongBooMessage
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="WrongBooMessage"/>.
        /// </summary>
        /// <param name="num">
        /// ������ ���������.
        /// </param>
        public WrongBooMessage(DateTime num)
        {
            this.Num = num;
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="WrongBooMessage"/>.
        /// </summary>
        protected WrongBooMessage()
        {
        }

        /// <summary>
        /// ������ ���������.
        /// </summary>
        public DateTime Num { get; private set; }
    }
}
