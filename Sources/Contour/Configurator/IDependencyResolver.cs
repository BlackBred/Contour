namespace Contour.Configurator
{
    using System;

    /// <summary>
    ///   ��������� ��� ����������� ������������ ��� ���������������� ������� ���� ���������.
    /// </summary>
    public interface IDependencyResolver
    {
        #region Public Methods and Operators

        /// <summary>
        /// ���������� ��������� �� ����� � ����.
        /// </summary>
        /// <param name="name">
        /// ��� ����������.
        /// </param>
        /// <param name="type">
        /// ��� ����������.
        /// </param>
        /// <returns>
        /// ��������� ������, ���� null.
        /// </returns>
        object Resolve(string name, Type type);

        #endregion
    }
}
