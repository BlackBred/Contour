namespace Contour
{
    /// <summary>
    ///   ������ ���������� ������.
    /// </summary>
    public static class StringEx
    {
        #region Public Methods and Operators

        /// <summary>
        /// ����������� ������ � �������������� ����������.
        /// </summary>
        /// <param name="s">
        /// ������ ������.
        /// </param>
        /// <param name="args">
        /// ��������� ��� �����������.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormatEx(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        /// <summary>
        /// ����������� ������ � ����� ��������� <see cref="MessageLabel"/>.
        /// </summary>
        /// <param name="s">
        /// ��������� ������������� �����.
        /// </param>
        /// <returns>
        /// ������ �������������� ����� ���������.
        /// </returns>
        public static MessageLabel ToMessageLabel(this string s)
        {
            return MessageLabel.From(s);
        }

        #endregion
    }
}
