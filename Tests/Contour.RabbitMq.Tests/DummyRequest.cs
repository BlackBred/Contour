namespace Contour.RabbitMq.Tests
{
    /// <summary>
    /// ������ ��� �������������� ������.
    /// </summary>
    public class DummyRequest
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DummyRequest"/>.
        /// </summary>
        /// <param name="num">
        /// ������ �������.
        /// </param>
        public DummyRequest(int num)
        {
            this.Num = num;
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DummyRequest"/>.
        /// </summary>
        protected DummyRequest()
        {
        }

        /// <summary>
        /// ������ �������.
        /// </summary>
        public int Num { get; private set; }
    }
}
