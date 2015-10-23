namespace Contour.Configuration
{
    using System;

    /// <summary>
    ///   ���������� ������������ � ���������� (�������), ���� ���������� ���� ��������.
    /// </summary>
    public class BusConnectionException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BusConnectionException"/>.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        internal BusConnectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BusConnectionException"/>.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        internal BusConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
