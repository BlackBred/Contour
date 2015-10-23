using System;

namespace Contour.Transport.RabbitMQ.Topology
{
    /// <summary>
    /// ����������� �������.
    /// </summary>
    public class QueueBuilder
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="QueueBuilder"/>.
        /// </summary>
        /// <param name="name">
        /// ��� �������.
        /// </param>
        internal QueueBuilder(string name)
        {
            this.Instance = new Queue(name);
        }

        /// <summary>
        /// ������� ������ ��������� �������������.
        /// </summary>
        public QueueBuilder AutoDelete
        {
            get
            {
                this.Instance.AutoDelete = true;
                return this;

                // TODO: �� �������� ������ (get) ���������� ��������� �������. ���� ���������.
            }
        }

        /// <summary>
        /// ������� ������ ���� ��������.
        /// </summary>
        public QueueBuilder Durable
        {
            get
            {
                this.Instance.Durable = true;
                return this;

                // TODO: �� �������� ������ (get) ���������� ��������� �������. ���� ���������.
            }
        }

        /// <summary>
        /// ������� ������ ���� ������������.
        /// </summary>
        public QueueBuilder Exclusive
        {
            get
            {
                this.Instance.Exclusive = true;
                return this;

                // TODO: �� �������� ������ (get) ���������� ��������� �������. ���� ���������.
            }
        }

        /// <summary>
        /// ��������� �������.
        /// </summary>
        internal Queue Instance { get; private set; }

        /// <summary>
        /// ��������� � ����������� ��������� ������� ����� ��������� � �������.
        /// </summary>
        /// <param name="ttl">����� ����� ��������� � �������.</param>
        /// <returns>����������� �������.</returns>
        public QueueBuilder WithTtl(TimeSpan ttl)
        {
            this.Instance.Ttl = ttl;
            return this;
        }
    }
}
