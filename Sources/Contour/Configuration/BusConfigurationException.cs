namespace Contour.Configuration
{
    using System;

    /// <summary>
    ///   ���������� ��������� � ������������ ������������� ����.
    /// </summary>
    public class BusConfigurationException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BusConfigurationException"/>.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        internal BusConfigurationException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
