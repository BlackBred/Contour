namespace Contour.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    ///   ������ ����������� ������� ����. ��������� �������� � ������������ �������.
    /// </summary>
    public interface IBusComponentTracker
    {
        #region Public Methods and Operators

        /// <summary>
        /// ��������� �������� ������ � ������ ������������������ �����������.
        /// </summary>
        /// <typeparam name="T">
        /// ���������� ��� �����������, ������� ���������� ��������.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        IEnumerable<T> AllOf<T>() where T : class, IBusComponent;

        /// <summary>
        /// ���������������� ���������.
        /// </summary>
        /// <param name="component">
        /// ���������.
        /// </param>
        void Register(IBusComponent component);

        /// <summary>
        ///   ���������� ���� ������������������ �����������.
        /// </summary>
        void RestartAll();

        /// <summary>
        ///   ������ ���� ������������������ �����������.
        /// </summary>
        void StartAll();

        /// <summary>
        ///   ��������� ���� ������������������ �����������.
        /// </summary>
        void StopAll();

        #endregion

        /// <summary>
        /// �������� ����������������� ������������������ �����������.
        /// </summary>
        /// <returns>��������� ��������.
        /// </returns>
        bool CheckHealth();
    }
}
