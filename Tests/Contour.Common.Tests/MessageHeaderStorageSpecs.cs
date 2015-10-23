using System.Collections.Generic;

using NUnit.Framework;

namespace Contour.Common.Tests
{
    /// <summary>
    /// ����� ��� �������� ��������� ���������� ��������� ���������.
    /// </summary>
    public class MessageHeaderStorageSpecs
    {
        /// <summary>
        /// ��� ���������� ����������.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class WhenStoreHeaders
        {
            /// <summary>
            /// ������ ����������� ��������� �� ������� ������.
            /// </summary>
            [Test]
            public void ShouldFilterHeadersOfBlackList()
            {
                var blackList = new List<string> { "b", "c" };

                var sut = new MessageHeaderStorage(blackList);

                sut.Store(new Dictionary<string, object> { { "a", 1 }, { "b", 2 }, { "c", 3 }, { "d", 4 } });

                CollectionAssert.DoesNotContain(sut.Load().Keys, "b", "��������� ������ ���� �������� ��� ����������.");
                CollectionAssert.DoesNotContain(sut.Load().Keys, "c", "��������� ������ ���� �������� ��� ����������.");
            }
        }
    }
}
