using System;

using Contour.Receiving;
using Contour.Topology;

namespace Contour.Transport.RabbitMQ.Topology
{
    /// <summary>
    /// �������� ������� <c>RabbitMq</c>.
    /// </summary>
    public class Queue : ITopologyEntity, IListeningSource
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Queue"/>.
        /// </summary>
        /// <param name="name">
        /// ��� �������.
        /// </param>
        internal Queue(string name)
        {
            this.Name = name;
            this.Durable = false;
            this.Exclusive = false;
            this.AutoDelete = false;
        }

        /// <summary>
        /// ����� �������.
        /// </summary>
        public string Address
        {
            get
            {
                return this.Name;
            }
        }

        /// <summary>
        /// ������� ��������� �������.
        /// </summary>
        public bool AutoDelete { get; internal set; }

        /// <summary>
        /// ������� ���������� �������.
        /// </summary>
        public bool Durable { get; internal set; }

        /// <summary>
        /// ������� �������������� �������.
        /// </summary>
        public bool Exclusive { get; internal set; }

        /// <summary>
        /// ��� �������.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// ����� ����� ��������� � �������.
        /// </summary>
        public TimeSpan? Ttl { get; internal set; }

        /// <summary>
        /// ������� ��������� ����������� ������� � ������ <paramref name="name"/>.
        /// </summary>
        /// <param name="name">
        /// ��� ����������� �������.
        /// </param>
        /// <returns>
        /// ����������� ������� <see cref="QueueBuilder"/>.
        /// </returns>
        public static QueueBuilder Named(string name)
        {
            return new QueueBuilder(name);

            // TODO: �������� ����������� ������������ ������� �� ���������.
        }

        /// <summary>
        /// ��������� ��������� ���� ��������.
        /// </summary>
        /// <param name="obj">
        /// ������������ �������.
        /// </param>
        /// <returns>
        /// <c>true</c> - ���� ������� �����, ����� <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Queue)obj);
        }

        /// <summary>
        /// ������������ ��� ����������� ��� �������.
        /// </summary>
        /// <returns>
        /// ��� ����������� �������� �������.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Name != null ? this.Name.GetHashCode() : 0;
        }

        /// <summary>
        /// ������������ ������� � ������.
        /// </summary>
        /// <returns>
        /// ��������� ������������� �������.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", this.Name);
        }

        /// <summary>
        /// ��������� ��������� ���� ��������.
        /// </summary>
        /// <param name="other">
        /// ������������ �������.
        /// </param>
        /// <returns>
        /// <c>true</c> - ���� ������� �����, ����� <c>false</c>.
        /// </returns>
        protected bool Equals(Queue other)
        {
            return string.Equals(this.Name, other.Name);
        }
    }
}
