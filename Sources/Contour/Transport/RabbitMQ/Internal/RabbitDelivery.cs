using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Contour.Configuration;
using Contour.Receiving;

using global::RabbitMQ.Client;

using global::RabbitMQ.Client.Events;

namespace Contour.Transport.RabbitMQ.Internal
{
    /// <summary>
    /// ������������ ��������� ����� ������ <c>RabbitMQ</c>.
    /// </summary>
    internal class RabbitDelivery : IDelivery
    {
        /// <summary>
        /// ��������� ���������.
        /// </summary>
        private readonly Lazy<IDictionary<string, object>> headers;

        /// <summary>
        /// �����, ���� ��������� ������� ������������� ���������.
        /// </summary>
        private readonly bool requiresAccept;

        /// <summary>
        /// �����, ���� ��������� ��������� ������������.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private volatile bool isAccepted;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="RabbitDelivery"/>.
        /// </summary>
        /// <param name="channel">����� �������� ���������.</param>
        /// <param name="args">��������� �������� ���������.</param>
        /// <param name="requiresAccept">�����, ���� ��������� ������������� ��������.</param>
        public RabbitDelivery(RabbitChannel channel, BasicDeliverEventArgs args, bool requiresAccept)
        {
            this.Channel = channel;
            this.Label = channel.Bus.MessageLabelHandler.Resolve(args);
            this.Args = args;
            this.requiresAccept = requiresAccept;

            this.headers = new Lazy<IDictionary<string, object>>(
                () => this.ExtractHeadersFrom(args));
        }

        /// <summary>
        /// ��������� �������� ���������.
        /// </summary>
        public BasicDeliverEventArgs Args { get; private set; }

        /// <summary>
        /// ����� �������� ���������.
        /// </summary>
        public RabbitChannel Channel { get; private set; }

        /// <summary>
        /// ������ ����������� ���������.
        /// </summary>
        public string ContentType
        {
            get
            {
                return this.Args.BasicProperties.ContentType;
            }
        }

        /// <summary>
        /// ������������� ���������.
        /// </summary>
        public string CorrelationId
        {
            get
            {
                return this.Args.BasicProperties.CorrelationId;
            }
        }

        /// <summary>
        /// ��������� ���������.
        /// </summary>
        public IDictionary<string, object> Headers
        {
            get
            {
                return this.headers.Value;
            }
        }

        /// <summary>
        /// ������� ��������� ���������.
        /// </summary>
        public RabbitRoute IncomingRoute
        {
            get
            {
                return new RabbitRoute(this.Args.Exchange, this.Args.RoutingKey);
            }
        }

        /// <summary>
        /// �����, ���� ��������� ������.
        /// </summary>
        public bool IsRequest
        {
            get
            {
                return this.CorrelationId != null && this.ReplyRoute != null;
            }
        }

        /// <summary>
        /// �����, ���� ��������� �����.
        /// </summary>
        public bool IsResponse
        {
            get
            {
                return this.CorrelationId != null && this.ReplyRoute == null;
            }
        }

        /// <summary>
        /// ����� ����������� ���������.
        /// </summary>
        public MessageLabel Label { get; private set; }

        /// <summary>
        /// �����, ���� ����� �������� �� ���������� ���������.
        /// </summary>
        public bool CanReply
        {
            get
            {
                return this.ReplyRoute != null && !string.IsNullOrWhiteSpace(this.CorrelationId);
            }
        }

        /// <summary>
        /// ����� �������� ���������.
        /// </summary>
        IChannel IDelivery.Channel
        {
            get
            {
                return this.Channel;
            }
        }

        /// <summary>
        /// ������� ������ �� ������������ ���������.
        /// </summary>
        IRoute IDelivery.ReplyRoute
        {
            get
            {
                return this.ReplyRoute;
            }
        }

        /// <summary>
        /// ������� ������ �� ������������ ���������.
        /// </summary>
        private RabbitRoute ReplyRoute
        {
            get
            {
                if (this.Args.BasicProperties.ReplyTo == null)
                {
                    return null;
                }

                PublicationAddress replyAddress = this.Args.BasicProperties.ReplyToAddress;
                return new RabbitRoute(replyAddress.ExchangeName, replyAddress.RoutingKey);
            }
        }

        /// <summary>
        /// ������������ �������� ���������.
        /// </summary>
        public void Accept()
        {
            if (!this.requiresAccept || this.isAccepted)
            {
                return;
            }

            this.Channel.Accept(this);
            this.isAccepted = true;
        }

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        /// <typeparam name="T">��� ����������� ���������.</typeparam>
        /// <returns>������ ��������� ���������.</returns>
        public IConsumingContext<T> BuildConsumingContext<T>(MessageLabel label = null) where T : class
        {
            Message<T> message = this.UnpackAs<T>();

            if (label != null && !label.IsAny && !label.IsEmpty)
            {
                message = (Message<T>)message.WithLabel(label);
            }

            return new DefaultConsumingContext<T>(message, this);
        }

        /// <summary>
        /// ���������� ���������, ������������ ��������� �����.
        /// </summary>
        /// <param name="label">����� �����, � ������� ������������ ���������.</param>
        /// <param name="payload">����� ���������� ���������.</param>
        /// <returns>������ ��������� ���������.</returns>
        public Task Forward(MessageLabel label, object payload)
        {
            var headers = new Dictionary<string, object>(this.Headers);
            headers[Contour.Headers.CorrelationId] = this.CorrelationId;
            headers[Contour.Headers.ReplyRoute] = this.ReplyRoute;
            Contour.Headers.ApplyBreadcrumbs(headers, this.Channel.Bus.Endpoint.Address);
            Contour.Headers.ApplyOriginalMessageId(headers);

            return this.Channel.Bus.Emit(label, payload, headers);
        }

        /// <summary>
        /// �������� ��������� ��� ��������������.
        /// </summary>
        /// <param name="requeue">
        /// ��������� ��������� ������� �� �������� ������� ��� ��������� ���������.
        /// </param>
        public void Reject(bool requeue)
        {
            if (!this.requiresAccept || this.isAccepted)
            {
                return;
            }

            this.Channel.Reject(this, requeue);
            this.isAccepted = true;
        }

        /// <summary>
        /// �������� �������� ���������.
        /// </summary>
        /// <param name="message">�������� ���������.</param>
        public void ReplyWith(IMessage message)
        {
            if (!this.CanReply)
            {
                throw new BusConfigurationException("No ReplyToAddress or CorrelationId were found in delivery. Make sure that you have callback endpoint set.");
            }

            this.Channel.Reply(message, this.ReplyRoute, this.CorrelationId);
        }

        /// <summary>
        /// ������������ ���������� ���������� � ��������� ���������� ����.
        /// </summary>
        /// <typeparam name="T">��� ���������.</typeparam>
        /// <returns>��������� ���������� ����.</returns>
        public Message<T> UnpackAs<T>() where T : class
        {
            IMessage message = this.Channel.UnpackAs(typeof(T), this);
            return new Message<T>(message.Label, message.Headers, (T)message.Payload);
        }

        /// <summary>
        /// ������������ ���������� ���������� � ��������� ���������� ����.
        /// </summary>
        /// <param name="type">��� ���������.</param>
        /// <returns>��������� ���������� ����.</returns>
        public IMessage UnpackAs(Type type)
        {
            return this.Channel.UnpackAs(type, this);
        }

        /// <summary>
        /// ��������� ��������� �� ���������� ���������.
        /// </summary>
        /// <param name="args">��������� �������� ���������.</param>
        /// <returns>��������� ���������.</returns>
        private IDictionary<string, object> ExtractHeadersFrom(BasicDeliverEventArgs args)
        {
            var h = new Dictionary<string, object>(args.BasicProperties.Headers);

            if (this.CorrelationId != null)
            {
                h[Contour.Headers.CorrelationId] = this.CorrelationId;
            }

            if (this.ReplyRoute != null)
            {
                h[Contour.Headers.ReplyRoute] = this.ReplyRoute;
            }

            return h;
        }
    }
}
