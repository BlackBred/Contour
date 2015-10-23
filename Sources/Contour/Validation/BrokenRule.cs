// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrokenRule.cs" company="">
//   
// </copyright>
// <summary>
//   �������� ����������� ������� ��������� ���������.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Contour.Validation
{
    /// <summary>
    ///   �������� ����������� ������� ��������� ���������.
    /// </summary>
    public class BrokenRule
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="BrokenRule"/>. 
        /// �������� �������� ����������� ������� ���������.
        /// </summary>
        /// <param name="description">
        /// ��������� �������� ������ ���������.
        /// </param>
        public BrokenRule(string description)
        {
            this.Description = description;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   ��������� �������� ������ ���������.
        /// </summary>
        public string Description { get; private set; }

        #endregion
    }
}
