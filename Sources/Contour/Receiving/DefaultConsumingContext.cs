using System.Collections.Generic;

using Contour.Caching;

namespace Contour.Receiving
{
    /// <summary>
    /// �������� ��������� ����������� ���������.
    /// </summary>
    /// <typeparam name="T">��� ����������� ���������.</typeparam>
    internal class DefaultConsumingContext<T> : IConsumingContext<T>, IDeliveryContext
        where T : class
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DefaultConsumingContext{T}"/>.
        /// </summary>
        /// <param name="message">�������� ���������.</param>
        /// <param name="delivery">���������� ���������.</param>
        public DefaultConsumingContext(Message<T> message, IDelivery delivery)
        {
            this.Message = message;
            this.Delivery = delivery;
            this.Bus = delivery.Channel.Bus;
        }

        /// <summary>
        /// �������� �����, � ������� ��������������� ������� ���������� ���������.
        /// </summary>
        public IBusContext Bus { get; private set; }

        /// <summary>
        /// ���������� ������������ ���������.
        /// </summary>
        public IDelivery Delivery { get; private set; }

        /// <summary>
        /// ���������� ���������.
        /// </summary>
        public Message<T> Message { get; private set; }

        /// <summary>
        /// �����, ���� ����� �������� �� ��� ���������.
        /// </summary>
        public bool CanReply
        {
            get
            {
                return this.Delivery.CanReply;
            }
        }

        /// <summary>
        /// �������� ��������� ��� ������������.
        /// </summary>
        public void Accept()
        {
            this.Delivery.Accept();
        }

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        public void Forward(MessageLabel label)
        {
            this.Forward(label, this.Message.Payload);
        }

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        public void Forward(string label)
        {
            this.Forward(label, this.Message.Payload);
        }

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        /// <param name="payload">����� ���������� ���������.</param>
        /// <typeparam name="TOut">��� ���������.</typeparam>
        public void Forward<TOut>(MessageLabel label, TOut payload = default(TOut)) where TOut : class
        {
            this.Delivery.Forward(label, payload);
        }

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        /// <param name="payload">����� ���������� ���������.</param>
        /// <typeparam name="TOut">��� ���������.</typeparam>
        public void Forward<TOut>(string label, TOut payload = default(TOut)) where TOut : class
        {
            this.Forward(label.ToMessageLabel(), payload);
        }

        /// <summary>
        /// �������� ��������� ��� ��������������.
        /// </summary>
        /// <param name="requeue">
        /// ��������� ��������� ������� �� �������� ������� ��� ��������� ���������.
        /// </param>
        public void Reject(bool requeue)
        {
            this.Delivery.Reject(requeue);
        }

        /// <summary>
        /// ���������� �������� ���������, ��������� ���������� � ��������
        /// ��������� �������� ����� � ������������� ���������.
        /// </summary>
        /// <typeparam name="TResponse">.NET ��� ������������� ���������.</typeparam>
        /// <param name="response">������������ ���������.</param>
        /// <param name="expires">���������, ������� ���������� ����� ���� ����� ��������.</param>
        public void Reply<TResponse>(TResponse response, Expires expires = null) where TResponse : class
        {
            var headers = new Dictionary<string, object>();

            if (expires != null)
            {
                headers[Headers.Expires] = expires.ToString();
            }

            this.Delivery.ReplyWith(new Message<TResponse>(MessageLabel.Empty, headers, response));
        }
    }
}
