// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResult.cs" company="">
//   
// </copyright>
// <summary>
//   ��������� ��������� ���������.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Contour.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   ��������� ��������� ���������.
    /// </summary>
    public sealed class ValidationResult
    {
        #region Fields

        /// <summary>
        /// The _broken rules.
        /// </summary>
        private readonly IList<BrokenRule> _brokenRules;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="ValidationResult"/>. 
        /// �������� ���������� ���������.
        /// </summary>
        /// <param name="brokenRules">
        /// ������ ���������� ������.
        /// </param>
        public ValidationResult(IEnumerable<BrokenRule> brokenRules)
        {
            this._brokenRules = brokenRules.ToList();
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="ValidationResult"/>. 
        /// �������� ���������� ���������.
        /// </summary>
        /// <param name="brokenRules">
        /// ������ ���������� ������.
        /// </param>
        public ValidationResult(params BrokenRule[] brokenRules)
        {
            this._brokenRules = brokenRules.ToList();
        }

        /// <summary>
        /// ������������� ����� ������������ �� ��������� ��� ������ <see cref="ValidationResult"/>.
        /// </summary>
        private ValidationResult()
        {
            this._brokenRules = new BrokenRule[0];
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   ���������� �������� ��������� ���������.
        /// </summary>
        public static ValidationResult Valid
        {
            get
            {
                return new ValidationResult();
            }
        }

        /// <summary>
        ///   ������ ���������� ������.
        /// </summary>
        public IEnumerable<BrokenRule> BrokenRules
        {
            get
            {
                return this._brokenRules;
            }
        }

        /// <summary>
        ///   ��������� �� ��� ������� ���������.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this._brokenRules.Count == 0;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   �������� ���������� � ������ ��������� ���� �� ������ ������� ���������.
        /// </summary>
        public void ThrowIfBroken()
        {
            if (this.IsValid)
            {
                return;
            }

            throw new MessageValidationException(string.Join(", ", this.BrokenRules.Select(r => r.Description)));
        }

        #endregion
    }
}
