using System;

using Contour.Validation;

namespace Contour.Receiving
{
    /// <summary>
    /// �������������� ������������ ���������� ���������.
    /// </summary>
    public interface IReceiverConfiguration
    {
        /// <summary>
        /// ��������� ����������� ���������.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// ����� ����������� ���������.
        /// </summary>
        MessageLabel Label { get; }

        /// <summary>
        /// ��������� ���������� ���������.
        /// </summary>
        ReceiverOptions Options { get; }

        /// <summary>
        /// ����������� ����������� ���������.
        /// </summary>
        Action<IReceiver> ReceiverRegistration { get; }

        /// <summary>
        /// �������� �������� ���������.
        /// </summary>
        IMessageValidator Validator { get; }

        /// <summary>
        /// ��������� ������������ ����������.
        /// </summary>
        void Validate();
    }
}
