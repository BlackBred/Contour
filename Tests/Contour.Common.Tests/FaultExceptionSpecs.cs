using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Contour.Common.Tests
{
    /// <summary>
    /// ����� ������ ��������� ��������� ���������.
    /// </summary>
    public class FaultExceptionSpecs
    {
        /// <summary>
        /// ���� ��������� ������ ������
        /// </summary>
        [TestFixture]
        public class WhenCreateFaultException
        {
            /// <summary>
            /// ����� � ����� ���� ��� ���������� ����������, ������ ��������� ������ ��� ���������� �������� ������.
            /// </summary>
            [Test]
            public void IfExceptionWithoutInnerExceptionsShouldBeWithoutInnerExceptions()
            {
                var convertedException = new ArgumentException("������� ����� ��������.");

                var sut = new FaultException(convertedException);

                CollectionAssert.IsEmpty(sut.InnerExceptions, "������ ���������� ����� ���� ������.");
            }

            /// <summary>
            /// ����� � ������, ���� ���� ���������� ������, ������ ����������� ������ � ���������� �������� ������.
            /// </summary>
            [Test]
            public void IfExceptionWithInnerExceptionShouldContainInnerException()
            {
                try
                {
                    this.GetType()
                        .GetMethod("ThrowException", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod)
                        .Invoke(this, new object[] { });
                }
                catch (TargetInvocationException ex)
                {
                    var sut = new FaultException(ex);

                    CollectionAssert.IsNotEmpty(sut.InnerExceptions, "������ ���� ���������� ����������.");
                    Assert.AreEqual(1, sut.InnerExceptions.Count, "������ ���� ���� ���������� ����������.");
                }
            }

            /// <summary>
            /// ����� � ������, ���� ������ ������������ ����������� ������ <see cref="System.AggregateException"/>, ������ ������ ������ ��������� ���������� ������� ������.
            /// </summary>
            [Test]
            public void IfExceptionWIthAggregatedExceptionShouldContainException()
            {
                try
                {
                    var task = new Task<int>(
                        () =>
                            {
                                this.ThrowException();
                                return 0;
                            });
                    task.Start();
                    task.Wait(10);
                }
                catch (AggregateException exception)
                {
                    var sut = new FaultException(exception);

                    CollectionAssert.IsNotEmpty(sut.InnerExceptions, "������ ���� ���������� ����������.");
                    Assert.AreEqual(1, sut.InnerExceptions.Count, "������ ���� ���� ���������� ����������.");
                }
            }

            /// <summary>
            /// � ������, ���� ������ ������������ ����������� ������ <see cref="System.AggregateException"/>,
            /// � ������ �������� ���������� ������ ������ <see cref="System.AggregateException"/>, ������� � ���� ������� �������� ������,
            /// ������ ������ <see cref="FaultException"/> ������ ��������� ��� ���������� ������� ������.
            /// ����� �� ������ ���� ������� ��� �������������� ������ � <see cref="FaultException"/>
            /// </summary>
            [Test]
            public void IfAggregatedExceptionContainsInnerAggregateExceptionsFaultExceptionShouldContainAllExceptions()
            {
                try
                {
                    throw new AggregateException(new AggregateException(new TimeoutException()));
                }
                catch (AggregateException exception)
                {
                    Assert.DoesNotThrow(() => { new FaultException(exception); });

                    var sut = new FaultException(exception);
                    CollectionAssert.IsNotEmpty(sut.InnerExceptions, "������ ���� ���������� ����������.");
                    Assert.AreEqual(1, sut.InnerExceptions.Count, "������ ���� ���� ���������� ����������.");
                    Assert.IsNotEmpty(sut.InnerExceptions.First().InnerExceptions, "��������� ���������� ������ ��������� ����������.");
                    Assert.AreEqual(1, sut.InnerExceptions.First().InnerExceptions.Count, "��������� ���������� ������ ��������� ���� ���������� ����������.");
                    Assert.AreEqual(exception.InnerExceptions.First().InnerException.GetType().FullName, sut.InnerExceptions.First().InnerExceptions.First().Type);
                }
            }

            private void ThrowException()
            {
                throw new NotImplementedException();
            }
        }
    }
}
