namespace Contour.RabbitMq.Tests
{
    /// <summary>
    /// ����� ��� �������������� ������.
    /// </summary>
    public class DummyResponse
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DummyResponse"/>.
        /// </summary>
        /// <param name="num">
        /// ������ ������.
        /// </param>
        public DummyResponse(int num)
        {
            this.Num = num;
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DummyResponse"/>.
        /// </summary>
        protected DummyResponse()
        {
        }

        /// <summary>
        /// ������ ������.
        /// </summary>
        public int Num { get; private set; }
    }
}
