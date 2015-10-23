namespace Contour
{
    using System;
    using System.IO;

    /// <summary>
    ///   ����� (������) ����������� � ����.
    /// </summary>
    public interface IChannel : IDisposable
    {
        #region Public Events

        /// <summary>
        ///   ������� ��� ��������� ����������� � ����������� ���� � ������ ������.
        /// </summary>
        event Action<IChannel, ErrorEventArgs> Failed;

        #endregion

        #region Public Properties

        /// <summary>
        ///   ��������� ����, � ������ �������� ��� ������ �����.
        /// </summary>
        IBusContext Bus { get; }

        #endregion
    }
}
