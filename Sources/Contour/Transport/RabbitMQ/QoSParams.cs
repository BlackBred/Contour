// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QoSParams.cs" company="">
//   
// </copyright>
// <summary>
//   QoS ��������� ��� RabbitMQ.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Contour.Transport.RabbitMQ
{
    /// <summary>
    ///   QoS ��������� ��� RabbitMQ.
    /// </summary>
    public class QoSParams
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="QoSParams"/>. 
        /// The qo s params.
        /// </summary>
        /// <param name="prefetchCount">
        /// ���������� ��������� ���������� �� ���� �� ���� ���������, �.�. ������ ������ ������.
        /// </param>
        /// <param name="prefetchSize">
        /// ���������� ���������, ������� ������ ���������� ����������, ������ ��� ������� ����� ������
        ///   ������.
        /// </param>
        public QoSParams(ushort prefetchCount, uint prefetchSize)
        {
            this.PrefetchCount = prefetchCount;
            this.PrefetchSize = prefetchSize;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the prefetch count.
        /// </summary>
        public ushort PrefetchCount { get; private set; }

        /// <summary>
        /// Gets the prefetch size.
        /// </summary>
        public uint PrefetchSize { get; private set; }

        #endregion
    }
}
