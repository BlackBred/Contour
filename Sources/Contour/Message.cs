namespace Contour
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///   �������� �������������� ��������� ���������.
    /// </summary>
    public sealed class Message : IMessage
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Message"/>.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <param name="payload">
        /// The payload.
        /// </param>
        public Message(MessageLabel label, IDictionary<string, object> headers, object payload)
        {
            this.Label = label;
            this.Headers = headers;
            this.Payload = payload;
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Message"/>.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="payload">
        /// The payload.
        /// </param>
        public Message(MessageLabel label, object payload)
            : this(label, new Dictionary<string, object>(), payload)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the headers.
        /// </summary>
        public IDictionary<string, object> Headers { get; private set; }

        /// <summary>
        ///   ����� ���������.
        /// </summary>
        public MessageLabel Label { get; private set; }

        /// <summary>
        ///   ���������� ���������.
        /// </summary>
        public object Payload { get; private set; }

        #endregion

        /// <summary>
        /// ������� ����� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">
        /// ����� ����� ���������.
        /// </param>
        /// <returns>
        /// ����� ���������.
        /// </returns>
        public IMessage WithLabel(MessageLabel label)
        {
            return new Message(label, Headers, Payload);
        }

        /// <summary>
        /// ������� ����� ��������� � ��������� ����������.
        /// </summary>
        /// <typeparam name="T">��� �����������.</typeparam>
        /// <param name="payload">���������� ���������.</param>
        /// <returns>����� ���������.</returns>
        public IMessage WithPayload<T>(T payload) where T : class
        {
            return new Message(Label, Headers, payload);
        }
    }

    /// <summary>
    /// ������ �������������� ��������� ���������.
    /// </summary>
    /// <typeparam name="T">
    /// ��� ����������� ���������
    /// </typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public sealed class Message<T> : IMessage
        where T : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Message{T}"/>.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <param name="payload">
        /// The payload.
        /// </param>
        public Message(MessageLabel label, IDictionary<string, object> headers, T payload)
        {
            this.Label = label;
            this.Headers = headers;
            this.Payload = payload;
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Message{T}"/>.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="payload">
        /// The payload.
        /// </param>
        public Message(MessageLabel label, T payload)
            : this(label, new Dictionary<string, object>(), payload)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the headers.
        /// </summary>
        public IDictionary<string, object> Headers { get; private set; }

        /// <summary>
        ///   ����� ���������.
        /// </summary>
        public MessageLabel Label { get; private set; }

        /// <summary>
        ///   ���������� ���������.
        /// </summary>
        public T Payload { get; private set; }

        #endregion

        #region Explicit Interface Properties

        /// <summary>
        /// Gets the payload.
        /// </summary>
        object IMessage.Payload
        {
            get
            {
                return this.Payload;
            }
        }

        #endregion

        /// <summary>
        /// ������� ����� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">
        /// ����� ����� ���������.
        /// </param>
        /// <returns>
        /// ����� ���������.
        /// </returns>
        public IMessage WithLabel(MessageLabel label)
        {
            return new Message<T>(label, Headers, Payload);
        }

        /// <summary>
        /// ������� ����� ��������� � ��������� ����������.
        /// </summary>
        /// <typeparam name="T1">��� �����������.</typeparam>
        /// <param name="payload">���������� ���������.</param>
        /// <returns>����� ���������.</returns>
        public IMessage WithPayload<T1>(T1 payload) where T1 : class
        {
            return new Message<T1>(Label, Headers, payload);
        }
    }
}
