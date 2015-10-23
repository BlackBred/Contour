namespace Contour.Helpers.Timing
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Timers;

    using Timer = System.Timers.Timer;

    /// <summary>
    /// �����������, ������� �������� ���������� ������ �� ��������� ��������� ������� ��������.
    /// �������� ������������� ��������� ������ ���������� � ������������ ����������. ����������, �������� ����� ��������� ������ ��������, 
    /// � ������� ����� ���������� ������. 
    /// ������ ������������� ��������� �� ������������ ��� ������ �� ����������.
    /// ��� ����������� ������ � ������������ ������������ ���������, �� ������ �������, ����� ������� ������ �� ������������.
    /// </summary>
    internal class RoughTicketTimer : ITicketTimer
    {
        /// <summary>
        /// ������� ��������� � ����� �� ����������.
        /// </summary>
        private readonly IDictionary<long, Job> jobs = new ConcurrentDictionary<long, Job>();

        /// <summary>
        /// ������, ������� �������� �������� �� ���������� ����� ����� �������� ��������� �������.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// ����� ��������� �������� ��������� ������.
        /// </summary>
        private long lastTicketNo;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="RoughTicketTimer"/>.
        /// </summary>
        /// <param name="checkInterval">
        /// �������� �������, � ������� ����� ����������� ������������� ��������� ������.
        /// </param>
        public RoughTicketTimer(TimeSpan checkInterval)
        {
            this.timer = new Timer(checkInterval.TotalMilliseconds);
            this.timer.Elapsed += this.TimerOnElapsed;
            this.timer.Start();
        }

        /// <summary>
        /// ���������� ��������� ������ <see cref="RoughTicketTimer"/>. 
        /// </summary>
        ~RoughTicketTimer()
        {
            this.Dispose();
        }

        /// <summary>
        /// ���������� ���������� �����, ������� ������� ������ ����������.
        /// </summary>
        public int JobCount
        {
            get
            {
                return this.jobs.Count;
            }
        }

        /// <summary>
        /// ������������ ������ � ������������.
        /// </summary>
        /// <param name="span">
        /// ��������, ����� ������� ������ ���� ������� ������.
        /// </param>
        /// <param name="callback">
        /// �������� ���������� ��� ���������� ������.
        /// </param>
        /// <returns>
        /// ��������� ������������������ ������. ��������� ����� ���� ������������ ��� �������� ������ �� ������������.
        /// </returns>
        public long Acquire(TimeSpan span, Action callback)
        {
            long ticket = Interlocked.Increment(ref this.lastTicketNo);
            this.jobs.Add(ticket, new Job(DateTime.Now.Add(span), callback));
            return ticket;
        }

        /// <summary>
        /// �������� ���������� ������ � ������������.
        /// ������ ����� �������.
        /// </summary>
        /// <param name="ticket">
        /// ��������� ������, ������� ����� ��������.
        /// </param>
        public void Cancel(long ticket)
        {
            this.jobs.Remove(ticket);
        }

        /// <summary>
        /// ����������� ������� �������.
        /// </summary>
        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Dispose();
            }
        }

        /// <summary>
        /// ���������� ������� �� �������� ������� ��������.
        /// ��������� ������������� ��������� ������.
        /// ���� ���� ������, ������� ����� ���������, ����� ��������� �� � ������� �� ������������.
        /// </summary>
        /// <param name="sender">
        /// ��������� �������.
        /// </param>
        /// <param name="elapsedEventArgs">
        /// ���������� � �������.
        /// </param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            DateTime now = DateTime.Now;

            foreach (var ticket in this.jobs)
            {
                if (ticket.Value.Expires < now)
                {
                    this.jobs.Remove(ticket);
                    try
                    {
                        ticket.Value.Callback();
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        // TODO: something better than eating exceptions
                    }
                }
            }
        }

        /// <summary>
        /// ����������� ������.
        /// </summary>
        public class Job
        {
            // TODO: ������ ����� �� ���������� �������������.

            /// <summary>
            /// �������������� ����� ��������� ������ <see cref="Job"/>.
            /// </summary>
            /// <param name="expires">
            /// �����, ����� ���������� ��������� ������.
            /// </param>
            /// <param name="callback">
            /// �������� ���������� ��� ���������� ������.
            /// </param>
            public Job(DateTime expires, Action callback)
            {
                this.Expires = expires;
                this.Callback = callback;
            }

            /// <summary>
            /// �������� ���������� ��� ���������� ������.
            /// </summary>
            public Action Callback { get; private set; }

            /// <summary>
            /// �����, ����� ���������� ��������� ������.
            /// </summary>
            public DateTime Expires { get; private set; }
        }
    }
}
