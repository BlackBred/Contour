namespace Contour
{
    /// <summary>
    /// �������� ����� ���� ���������.
    /// ������������� ������� ��� �������� � ��������� ��������� �� ����.
    /// </summary>
    internal class Endpoint : IEndpoint
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Endpoint"/>.
        /// </summary>
        /// <param name="address">����� �������� �����. ����� ������ ���� ���������� ��� ���� �������� ����� ���� ���������.</param>
        public Endpoint(string address)
        {
            this.Address = address;
        }

        /// <summary>
        /// ����� �������� �����. 
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// ���������� ��� �������.
        /// </summary>
        /// <param name="obj">������������ ������.</param>
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

            return this.Equals((Endpoint)obj);
        }

        /// <summary>
        /// ��������� ��� ����������� �������� �����.
        /// </summary>
        /// <returns>��� ����������� �������� �����.</returns>
        public override int GetHashCode()
        {
            return this.Address != null ? this.Address.GetHashCode() : 0;
        }

        /// <summary>
        /// ��������� ��������� ������������� �������� �����.
        /// </summary>
        /// <returns>��������� ������������� �������� �����.</returns>
        public override string ToString()
        {
            return this.Address;
        }

        /// <summary>
        /// ���������� ��� �������.
        /// </summary>
        /// <param name="other">������������ ������.</param>
        /// <returns>
        /// <c>true</c> - ���� ������� �����, ����� <c>false</c>.
        /// </returns>
        protected bool Equals(Endpoint other)
        {
            return string.Equals(this.Address, other.Address);
        }
    }
}
