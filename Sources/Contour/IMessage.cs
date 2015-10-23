namespace Contour
{
    using System.Collections.Generic;

    /// <summary>
    ///   ��������� ��� ���������.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the headers.
        /// </summary>
        IDictionary<string, object> Headers { get; }

        /// <summary>
        ///   ����� ���������.
        /// </summary>
        MessageLabel Label { get; }

        /// <summary>
        ///   ���������� ���������.
        /// </summary>
        object Payload { get; }

        /// <summary>
        /// ������� ����� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">
        /// ����� ����� ���������.
        /// </param>
        /// <returns>
        /// ����� ���������.
        /// </returns>
        IMessage WithLabel(MessageLabel label);

        /// <summary>
        /// ������� ����� ��������� � ��������� ����������.
        /// </summary>
        /// <typeparam name="T">��� �����������.</typeparam>
        /// <param name="payload">���������� ���������.</param>
        /// <returns>����� ���������.</returns>
        IMessage WithPayload<T>(T payload) where T : class;
    }
}
