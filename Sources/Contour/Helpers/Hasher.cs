using System;

using Newtonsoft.Json;

namespace Contour.Helpers
{
    /// <summary>
    /// ������������ �������� ���-������� ��� �����������.
    /// </summary>
    public class Hasher
    {
        /// <summary>
        /// ��������� �������� ���-������� ��� �������.
        /// </summary>
        /// <param name="obj">������, ��� �������� ����������� �������� ���-�������.</param>
        /// <returns>����������� �������� ���-�������.</returns>
        [Obsolete("���������� ������������ ����� �������� �������� ���-������� �� ������ ��������� ��������� (IMessage).")]
        public long CalculateHashOf(object obj)
        {
            return JsonConvert.SerializeObject(obj).GetHashCode();
        }

        /// <summary>
        /// ��������� �������� ���-������� ��� ���������.
        /// </summary>
        /// <param name="message">���������, ��� �������� ����������� �������� ���-�������.</param>
        /// <returns>����������� �������� ���-�������.</returns>
        public long CalculateHashOf(IMessage message)
        {
            unchecked 
            {         
                int hash = 27;
                hash = (13 * hash) + JsonConvert.SerializeObject(message.Payload).GetHashCode();
                hash = (13 * hash) + message.Label.GetHashCode();
                return hash;
            }
        }
    }
}
