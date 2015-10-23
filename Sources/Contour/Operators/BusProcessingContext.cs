namespace Contour.Operators
{
    using System;

    using Contour.Receiving;

    /// <summary>
    /// �������� ��������� ���������.
    /// </summary>
    public class BusProcessingContext
    {
        [ThreadStatic]
        private static BusProcessingContext current;

        /// <summary>
        /// ������� ��������.
        /// </summary>
        public static BusProcessingContext Current
        {
            get
            {
                return current;
            }

            set
            {
                current = value;
            }
        }

        /// <summary>
        /// �������� ��������.
        /// </summary>
        public IDelivery Delivery { get; private set; }

        /// <summary>
        /// ������� ������ ���������.
        /// </summary>
        /// <param name="delivery">��������.</param>
        public BusProcessingContext(IDelivery delivery)
        {
            this.Delivery = delivery;
        }
    }
}
