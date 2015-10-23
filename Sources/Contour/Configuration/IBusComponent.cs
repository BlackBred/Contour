namespace Contour.Configuration
{
    using System;

    /// <summary>
    ///   ��������� ������� ���� ���������, ������������ � ������� ����� ����� ������ �������.
    /// </summary>
    public interface IBusComponent : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        ///   ������ ����������.
        /// </summary>
        void Start();

        /// <summary>
        ///   ��������� ����������.
        /// </summary>
        void Stop();

        #endregion

        /// <summary>
        /// �������� ����������������� ����������.
        /// </summary>
        bool IsHealthy { get; }
    }
}
