namespace Contour
{
    using System;

    /// <summary>
    ///   ���������� ����������� � ������ ������������ ���� � ������ (�������� �������� �������������).
    /// </summary>
    public class BusNotReadyException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BusNotReadyException"/>.
        /// </summary>
        internal BusNotReadyException()
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BusNotReadyException"/>.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        internal BusNotReadyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BusNotReadyException"/>.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        internal BusNotReadyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
