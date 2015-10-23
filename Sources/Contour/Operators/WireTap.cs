namespace Contour.Operators
{
    /// <summary>
    /// ��������, ������� ��������� �������������� ��������� ������������ � ���� (���������� ������� <see href="http://www.eaipatterns.com/WireTap.html"/>).
    /// </summary>
    public class WireTap : RecipientList
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="WireTap"/>.
        /// </summary>
        /// <param name="messageLabel">����� ���������, ���� ���� ������������� �������� ���������.</param>
        public WireTap(MessageLabel messageLabel)
            : base(message => new[] { messageLabel, message.Label })
        {
        }
    }
}
