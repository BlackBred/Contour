using System;
using System.Collections.Generic;

namespace Contour
{
    /// <summary>
    ///   ������������� �������� ����������.
    /// </summary>
    public sealed class FaultException
    {
        private const int MaxNestingLevel = 5;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="FaultException"/>. 
        /// ������� ��������� �������� ����������.
        /// </summary>
        /// <param name="exception">
        /// ����������, �� �������� ��������� ��������.
        /// </param>
        public FaultException(Exception exception)
            : this(exception, 0)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="FaultException"/>.
        /// </summary>
        /// <param name="exception">����������, �� �������� ��������� ��������.</param>
        /// <param name="level">������� ������� ����������� ����������.</param>
        private FaultException(Exception exception, int level)
        {
            this.Message = exception.Message;
            this.Type = exception.GetType().FullName;
            this.StackTrace = exception.StackTrace;
            this.InnerExceptions = new List<FaultException>();

            if (level >= MaxNestingLevel)
            {
                return;
            }

            this.GetInnerExceptionsOf(exception, level);
        }

        /// <summary>
        ///   ��������� ����������.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        ///   ���� ����������.
        /// </summary>
        public string StackTrace { get; private set; }

        /// <summary>
        ///   ��� ����������.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// ��������� ����������, ������� ����� ���������� ��������� ����������.
        /// </summary>
        public IList<FaultException> InnerExceptions { get; private set; }

        /// <summary>
        /// �������� ���������� ���������� � ������������ �� � ��������.
        /// </summary>
        /// <param name="exception">�������������� ����������.</param>
        /// <param name="level">������� ������� ����������� ����������.</param>
        private void GetInnerExceptionsOf(Exception exception, int level)
        {
            var aggregateException = exception as AggregateException;
            if (aggregateException != null && aggregateException.InnerExceptions != null)
            {
                foreach (var innerExceptin in aggregateException.InnerExceptions)
                {
                    this.InnerExceptions.Add(new FaultException(innerExceptin, level + 1));
                }
            }
            else if (exception.InnerException != null)
            {
                this.InnerExceptions.Add(new FaultException(exception.InnerException, level + 1));
            }
        }
    }
}
