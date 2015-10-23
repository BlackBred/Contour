using System;

using Contour.Helpers;
using Contour.Helpers.CodeContracts;
using Contour.Receiving;

namespace Contour.Sending
{
    /// <summary>
    /// ������������ ����������� ���������.
    /// ������������ ��� ���������������� ��������� ������������� ������������ � ������������ � ������� ������-�����.
    /// </summary>
    internal class SenderConfiguration : ISenderConfiguration, ISenderConfigurator
    {
        /// <summary>
        /// ��������� ���������� ���������.
        /// ������������ ��� ���������������� ��������� ������ �� ������.
        /// </summary>
        private readonly ReceiverOptions receiverOptions;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SenderConfiguration"/>.
        /// </summary>
        /// <param name="label">����� ������������ ���������.</param>
        /// <param name="parentOptions">��������� �����������.</param>
        /// <param name="receiverOptions">��������� ���������� (��� �������� ���������).</param>
        public SenderConfiguration(MessageLabel label, SenderOptions parentOptions, ReceiverOptions receiverOptions)
        {
            this.receiverOptions = receiverOptions;
            Requires.Format(MessageLabel.IsValidLabel(label.Name), "label");
            this.Label = label;

            this.Options = (SenderOptions)parentOptions.Derive();
        }

        /// <summary>
        /// ��������� ����� ���������.
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// ������������ ���������� �������� ���������.
        /// </summary>
        public IReceiverConfiguration CallbackConfiguration { get; private set; }

        /// <summary>
        /// ����� ������������ ���������.
        /// </summary>
        public MessageLabel Label { get; private set; }

        /// <summary>
        /// ��������� ����������� ���������.
        /// </summary>
        public SenderOptions Options { get; private set; }

        /// <summary>
        /// ����� �� �������� ����� ��� �������� ���������.
        /// </summary>
        public bool RequiresCallback
        {
            get
            {
                return this.CallbackConfiguration != null;
            }
        }

        /// <summary>
        /// ��������� ������������.
        /// </summary>
        /// <param name="routeResolverBuilder">����������� ����������� ��������.</param>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator ConfiguredWith(Func<IRouteResolverBuilder, IRouteResolver> routeResolverBuilder)
        {
            this.Options.RouteResolverBuilder = routeResolverBuilder;

            return this;
        }

        /// <summary>
        /// ������������� ������������� ��������� ��������� �� �����.
        /// </summary>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator Persistently()
        {
            this.Options.Persistently = true;

            return this;
        }

        /// <summary>
        /// ��������� ������������ ������������.
        /// </summary>
        public void Validate()
        {
            // TODO: this
        }

        /// <summary>
        /// ������������� ��������� ��� ����� ���������.
        /// </summary>
        /// <param name="alias">��������� ����� ���������.</param>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator WithAlias(string alias)
        {
            string tempAlias = MessageLabel.AliasPrefix + alias;
            Requires.Format(MessageLabel.IsValidAlias(tempAlias), "alias");

            this.Alias = tempAlias;

            return this;
        }

        /// <summary>
        /// ������������� �������� ����� ��������� ������ ��� ��������� �������� ���������.
        /// </summary>
        /// <param name="callbackEndpointBuilder">����������� �������� ����� ��� �������� ���������.</param>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator WithCallbackEndpoint(Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint> callbackEndpointBuilder)
        {
            IReceiverConfigurator configurator = new ReceiverConfiguration(MessageLabel.Any, this.receiverOptions).WithEndpoint(callbackEndpointBuilder);

            this.CallbackConfiguration = (IReceiverConfiguration)configurator;

            return this;
        }

        /// <summary>
        /// ������������� ������������� ������������� �������� ���������.
        /// </summary>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator WithConfirmation()
        {
            this.Options.ConfirmationIsRequired = true;

            return this;
        }

        /// <summary>
        /// ������������� �������� ����� ��������� ������ �� ��������� ��� ��������� ���������.
        /// </summary>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator WithDefaultCallbackEndpoint()
        {
            return this.WithCallbackEndpoint(seb => seb.UseDefaultTempReplyEndpoint(this));
        }

        /// <summary>
        /// ������������� ��������� �������� �� ������� ������ ���� ������� ����� �� ������.
        /// </summary>
        /// <param name="timeout">��������� �������� �� ������� ������ ���� ������� ����� �� ������.</param>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator WithRequestTimeout(TimeSpan? timeout)
        {
            this.Options.RequestTimeout = timeout;

            return this;
        }

        /// <summary>
        /// ������������� ����� ����� ������������� ���������.
        /// </summary>
        /// <param name="ttl">����� ����� ������������� ���������.</param>
        /// <returns>������������ �����������.</returns>
        public ISenderConfigurator WithTtl(TimeSpan ttl)
        {
            this.Options.Ttl = ttl;

            return this;
        }

        /// <summary>
        /// ������������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ��������� ���������.</param>
        /// <returns>������������ ����������� � ������������� ��������� ���������� ��������� ���������.</returns>
        public ISenderConfigurator WithIncomingMessageHeaderStorage(IIncomingMessageHeaderStorage storage)
        {
            this.Options.IncomingMessageHeaderStorage = new Maybe<IIncomingMessageHeaderStorage>(storage);

            return this;
        }
    }
}
