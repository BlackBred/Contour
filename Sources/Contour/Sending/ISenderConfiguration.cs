using Contour.Receiving;

namespace Contour.Sending
{
    /// <summary>
    /// ������������ �����������.
    /// </summary>
    public interface ISenderConfiguration
    {
        /// <summary>
        /// ��������� ��� ����� ������������ ���������.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// ������������ ���������� �������� ���������.
        /// </summary>
        IReceiverConfiguration CallbackConfiguration { get; }

        /// <summary>
        /// ����� ������������ ���������.
        /// </summary>
        MessageLabel Label { get; }

        /// <summary>
        /// ��������� �����������.
        /// </summary>
        SenderOptions Options { get; }

        /// <summary>
        /// ���������� ���������� �� ������������ �������� ����� ��� ��������� �������� ���������.
        /// </summary>
        bool RequiresCallback { get; }

        /// <summary>
        /// ��������� ������������ ������������ �����������.
        /// </summary>
        void Validate();
    }
}
