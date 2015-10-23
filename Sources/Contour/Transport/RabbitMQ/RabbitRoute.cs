// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RabbitRoute.cs" company="">
//   
// </copyright>
// <summary>
//   �������� �������� ��� �������� ��������� ����� RabbitMQ.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Contour.Transport.RabbitMQ
{
    /// <summary>
    ///   �������� �������� ��� �������� ��������� ����� RabbitMQ.
    /// </summary>
    public class RabbitRoute : IRoute
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="RabbitRoute"/>. 
        /// ������� �������� ������� ��� �������� ��������� ����� RabbitMQ.
        /// </summary>
        /// <param name="exchange">
        /// Exchange.
        /// </param>
        /// <param name="routingKey">
        /// Routing key.
        /// </param>
        public RabbitRoute(string exchange, string routingKey = "")
        {
            this.Exchange = exchange;
            this.RoutingKey = routingKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   ��� exchange.
        /// </summary>
        public string Exchange { get; private set; }

        /// <summary>
        ///   Routing key.
        /// </summary>
        public string RoutingKey { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Exchange: {0}, RoutingKey: {1}", this.Exchange, this.RoutingKey);
        }

        #endregion
    }
}
