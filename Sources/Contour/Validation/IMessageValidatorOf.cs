// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageValidatorOf.cs" company="">
//   
// </copyright>
// <summary>
//   ��������� ���������.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Contour.Validation
{
    /// <summary>
    /// ��������� ���������.
    /// </summary>
    /// <typeparam name="T">
    /// ��� ����������� ���������.
    /// </typeparam>
    public interface IMessageValidatorOf<T> : IMessageValidator
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// ��������� ���������� ���������.
        /// </summary>
        /// <param name="message">
        /// ��������� ��� ��������.
        /// </param>
        /// <returns>
        /// ��������� ���������.
        /// </returns>
        ValidationResult Validate(Message<T> message);

        #endregion
    }
}
