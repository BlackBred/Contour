namespace Contour.RabbitMq.Tests
{
    public class BooMessage
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BooMessage"/>.
        /// </summary>
        /// <param name="num">
        /// ������ ���������.
        /// </param>
        public BooMessage(int num)
        {
            this.Num = num;
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BooMessage"/>.
        /// </summary>
        protected BooMessage()
        {
        }

        /// <summary>
        /// ������ ���������.
        /// </summary>
        public int Num { get; private set; }
    }
}
