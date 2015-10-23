// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageValidationException.cs" company="">
//   
// </copyright>
// <summary>
//   ���������� ��������� ��������� ���������� ���������.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Contour.Validation
{
    using System;

    /// <summary>
    ///   ���������� ��������� ��������� ���������� ���������.
    /// </summary>
    public class MessageValidationException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="MessageValidationException"/>.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        internal MessageValidationException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
