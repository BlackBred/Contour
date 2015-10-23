using Contour.Transport.RabbitMQ.Topology;

using NUnit.Framework;

namespace Contour.Common.Tests
{
    /// <summary>
    /// ����� ������ ��� ���������� ����.
    /// </summary>
    public class NameGeneratorSpecs
    {
        /// <summary>
        /// ��� ��������� ���������� �����.
        /// </summary>
        [TestFixture]
        [Category("Unit")]
        public class WhenGenerateRandomName
        {
            /// <summary>
            /// ������ ������� ���� ������.
            /// </summary>
            [Test]
            public void ShouldReturnOneSymbol()
            {
                var result = NameGenerator.GetRandomName(1);

                Assert.AreEqual(1, result.Length);
            }

            /// <summary>
            /// ������ ������� ������ ������.
            /// </summary>
            [Test]
            public void ShouldReturnZeroSymbols()
            {
                var result = NameGenerator.GetRandomName(0);

                Assert.AreEqual(0, result.Length);
            }

            /// <summary>
            /// ������ 5 ��������.
            /// </summary>
            [Test]
            public void ShouldReturnFiveSymbols()
            {
                var result = NameGenerator.GetRandomName(5);

                Assert.AreEqual(5, result.Length);
            }

            /// <summary>
            /// ������ 40 ��������.
            /// </summary>
            [Test]
            public void ShouldReturnFortySymbols()
            {
                var result = NameGenerator.GetRandomName(40);

                Assert.AreEqual(40, result.Length);
            }
        }
    }
}
