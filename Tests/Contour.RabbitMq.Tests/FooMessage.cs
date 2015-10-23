namespace Contour.RabbitMq.Tests
{
    /// <summary>
    /// The foo message.
    /// </summary>
    public class FooMessage
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="FooMessage"/>.
        /// </summary>
        /// <param name="num">
        /// ������ ���������.
        /// </param>
        public FooMessage(int num)
        {
            this.Num = num;
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="FooMessage"/>.
        /// </summary>
        protected FooMessage()
        {
        }

        /// <summary>
        /// ������ ���������.
        /// </summary>
        public int Num { get; private set; }
    }
}
