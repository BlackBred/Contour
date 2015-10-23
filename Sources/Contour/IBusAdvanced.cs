namespace Contour
{
    using System.Collections.Generic;

    using Contour.Configuration;
    using Contour.Receiving;
    using Contour.Sending;

    /// <summary>
    ///   ����������� ��������� ���� ��� ����� �������.
    /// </summary>
    public interface IBusAdvanced
    {
        #region Public Properties

        /// <summary>
        ///   ������ ����������� ����, ��������� �� ������������ ����������� � �������.
        /// </summary>
        IBusComponentTracker ComponentTracker { get; }

        /// <summary>
        ///   ������ ����������� ��������� (�� ������ �� ������ ����������� ��������).
        /// </summary>
        IEnumerable<IReceiver> Receivers { get; }

        /// <summary>
        ///   ������ ������������ ��������� (�� ������ �� ������ ����������� ���������).
        /// </summary>
        IEnumerable<ISender> Senders { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// ��������/�������� ������ ������, ��������� ������� �����������.
        /// </summary>
        /// <returns>
        /// The <see cref="IChannel"/>.
        /// </returns>
        IChannel OpenChannel();

        /// <summary>
        ///   �������������� ���������� ������� ����.
        /// </summary>
        void Panic();

        #endregion
    }
}
