namespace Contour
{
    /// <summary>
    /// ���������� � ��������� ����� ���������.
    /// </summary>
    public interface IMessageLabelHandler
    {
        /// <summary>
        /// ���������� ����� ���������.
        /// </summary>
        /// <param name="raw">������, ���� ���������� ����� ���������.</param>
        /// <param name="label">������������ ����� ���������.</param>
        void Inject(object raw, MessageLabel label);

        /// <summary>
        /// ���������� ����� ��������� �� �������.
        /// </summary>
        /// <param name="raw">������, �� ������ �������� ������������ ����� ���������.</param>
        /// <returns>����� ���������.</returns>
        MessageLabel Resolve(object raw);
    }
}
