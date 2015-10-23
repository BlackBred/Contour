using System.Collections.Generic;

namespace Contour
{
    /// <summary>
    /// ��������� ���������� �������� ���������.
    /// </summary>
    public interface IIncomingMessageHeaderStorage
    {
        /// <summary>
        /// ��������� ��������� ��������� ���������.
        /// </summary>
        /// <param name="headers">��������� ��������� ���������.</param>
        void Store(IDictionary<string, object> headers);

        /// <summary>
        /// ���������� ����������� ��������� ��������� ���������.
        /// </summary>
        /// <returns>��������� ��������� ���������.</returns>
        IDictionary<string, object> Load();
    }
}
