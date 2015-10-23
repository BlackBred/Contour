using System.Security.Cryptography;
using System.Text;

namespace Contour.Transport.RabbitMQ.Topology
{
    /// <summary>
    /// ��������� ���� ��������� ���������.
    /// </summary>
    internal static class NameGenerator
    {
        /// <summary>
        /// ���������� ��� ��������� �� ���������� ������ ���� � ����.
        /// </summary>
        /// <param name="size">����������� ���������� �������� � �����.</param>
        /// <returns>��� �� ���������� ������ ���� � ����.</returns>
        internal static string GetRandomName(int size)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[(size + 1) / 2];

            cryptoProvider.GetBytes(randomBytes);

            var builder = new StringBuilder();

            foreach (var randomByte in randomBytes)
            {
                builder.Append(randomByte.ToString("X2"));
            }

            return builder.ToString().Substring(0, size);
        }
    }
}
