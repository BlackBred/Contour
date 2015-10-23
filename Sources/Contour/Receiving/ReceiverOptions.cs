using System;

using Contour.Configuration;
using Contour.Helpers;

namespace Contour.Receiving
{
    /// <summary>
    /// ��������� ����������.
    /// </summary>
    public class ReceiverOptions : BusOptions
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="ReceiverOptions"/>.
        /// </summary>
        public ReceiverOptions()
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="ReceiverOptions"/>.
        /// </summary>
        /// <param name="parent">������� ���������.</param>
        public ReceiverOptions(BusOptions parent)
            : base(parent)
        {
        }

        /// <summary>
        /// ������������� ������������� ������ ������������� �������� ���������.
        /// </summary>
        public Maybe<bool> AcceptIsRequired { protected get; set; }

        /// <summary>
        /// ����������� ����� ��������� �������� ���������.
        /// </summary>
        public Maybe<Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint>> EndpointBuilder { protected get; set; }

        /// <summary>
        /// ���������� ���������, ��� �������� ����������� ��������.
        /// </summary>
        public Maybe<IFailedDeliveryStrategy> FailedDeliveryStrategy { protected get; set; }

        /// <summary>
        /// ���������� ������������� ������������ ���������.
        /// </summary>
        public Maybe<uint> ParallelismLevel { protected get; set; }

        /// <summary>
        /// ���������� ���������, ��� ������� �� ������ �����������.
        /// </summary>
        public Maybe<IUnhandledDeliveryStrategy> UnhandledDeliveryStrategy { protected get; set; }

        /// <summary>
        /// ��������� ���������� ��������� ���������.
        /// </summary>
        public Maybe<IIncomingMessageHeaderStorage> IncomingMessageHeaderStorage { protected get; set; }

        /// <summary>
        /// ������� ����� ��������� �������� ��� ����� �������������.
        /// </summary>
        /// <returns>
        /// ����� ��������� ��������.
        /// </returns>
        public override BusOptions Derive()
        {
            return new ReceiverOptions(this);
        }

        /// <summary>
        /// ���������� ����������� ����� ���������� ���������.
        /// </summary>
        /// <returns>
        /// ����������� ����� ���������� ���������.
        /// </returns>
        public Maybe<Func<ISubscriptionEndpointBuilder, ISubscriptionEndpoint>> GetEndpointBuilder()
        {
            return this.Pick(o => ((ReceiverOptions)o).EndpointBuilder);
        }

        /// <summary>
        /// ���������� ���������� ���������, ��� ������� �������� ����������� ��������.
        /// </summary>
        /// <returns>
        /// ���������� ���������, ��� ������� �������� ����������� ��������.
        /// </returns>
        public Maybe<IFailedDeliveryStrategy> GetFailedDeliveryStrategy()
        {
            return this.Pick(o => ((ReceiverOptions)o).FailedDeliveryStrategy);
        }

        /// <summary>
        /// ���������� ���������� ������������� ������������ ���������.
        /// </summary>
        /// <returns>
        /// ���������� ������������� ������������ ���������.
        /// </returns>
        public Maybe<uint> GetParallelismLevel()
        {
            return this.Pick(o => ((ReceiverOptions)o).ParallelismLevel);
        }

        /// <summary>
        /// ���������� ���������� ���������, ��� ������� �� ������� ����� �����������.
        /// </summary>
        /// <returns>
        /// ���������� ���������, ��� ������� �� ������� ����� �����������.
        /// </returns>
        public Maybe<IUnhandledDeliveryStrategy> GetUnhandledDeliveryStrategy()
        {
            return this.Pick(o => ((ReceiverOptions)o).UnhandledDeliveryStrategy);
        }

        /// <summary>
        /// ���������� ������� ������������� ���� ������������ ������� ������������ ���������.
        /// </summary>
        /// <returns>
        /// ���� <c>true</c> - ����� ���������� ������������ ������� ������������ ���������, ����� - <c>false</c>.
        /// </returns>
        public Maybe<bool> IsAcceptRequired()
        {
            return this.Pick(o => ((ReceiverOptions)o).AcceptIsRequired);
        }

        /// <summary>
        /// ���������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <returns>��������� ���������� ��������� ���������.</returns>
        public Maybe<IIncomingMessageHeaderStorage> GetIncomingMessageHeaderStorage()
        {
            return this.Pick(o => ((ReceiverOptions)o).IncomingMessageHeaderStorage);
        }
    }
}
