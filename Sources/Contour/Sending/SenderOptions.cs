using System;

using Contour.Configuration;
using Contour.Helpers;

namespace Contour.Sending
{
    /// <summary>
    /// ��������� �����������.
    /// </summary>
    public class SenderOptions : BusOptions
    {
        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SenderOptions"/>.
        /// </summary>
        public SenderOptions()
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SenderOptions"/>.
        /// </summary>
        /// <param name="parent">������� ���������.</param>
        public SenderOptions(BusOptions parent)
            : base(parent)
        {
        }

        /// <summary>
        /// ��������� �� ������������� �� �������� ��������.
        /// </summary>
        public Maybe<bool> ConfirmationIsRequired { protected get; set; }

        /// <summary>
        /// ��������� �� ���������� ���������.
        /// </summary>
        public Maybe<bool> Persistently { protected get; set; }

        /// <summary>
        /// ����� �������� ������ �� ������.
        /// </summary>
        public Maybe<TimeSpan?> RequestTimeout { protected get; set; }

        /// <summary>
        /// ����������� ����������� ��������.
        /// </summary>
        public Maybe<Func<IRouteResolverBuilder, IRouteResolver>> RouteResolverBuilder { protected get; set; }

        /// <summary>
        /// ����� ����� ���������.
        /// </summary>
        public Maybe<TimeSpan?> Ttl { protected get; set; }

        /// <summary>
        /// ��������� �������� ���������.
        /// </summary>
        public Maybe<IIncomingMessageHeaderStorage> IncomingMessageHeaderStorage { protected get; set; }

        /// <summary>
        /// ������� ����� ��������� �����������, ������� ��������� ������������.
        /// </summary>
        /// <returns>����� ��������� �����������, ����������� ������������ ���������.</returns>
        public override BusOptions Derive()
        {
            return new SenderOptions(this);
        }

        /// <summary>
        /// ���������� ����� �������� ������ �� ������.
        /// </summary>
        /// <returns>
        /// ����� �������� ������ �� ������.
        /// </returns>
        public Maybe<TimeSpan?> GetRequestTimeout()
        {
            return this.Pick(o => ((SenderOptions)o).RequestTimeout);
        }

        /// <summary>
        /// ���������� ����������� ����������� ��������.
        /// </summary>
        /// <returns>����������� ����������� ��������.</returns>
        public Maybe<Func<IRouteResolverBuilder, IRouteResolver>> GetRouteResolverBuilder()
        {
            return this.Pick(o => ((SenderOptions)o).RouteResolverBuilder);
        }

        /// <summary>
        /// ���������� ����� ����� ���������.
        /// </summary>
        /// <returns>����� ����� ���������.</returns>
        public Maybe<TimeSpan?> GetTtl()
        {
            return this.Pick(o => ((SenderOptions)o).Ttl);
        }

        /// <summary>
        /// ��������� �� ������������� �������� ���������.
        /// </summary>
        /// <returns>������������� �������� ���������.</returns>
        public Maybe<bool> IsConfirmationRequired()
        {
            return this.Pick(o => ((SenderOptions)o).ConfirmationIsRequired);
        }

        /// <summary>
        /// ���������� �� ��������� ���������.
        /// </summary>
        /// <returns>���� <c>true</c>, ����� ����� ��������� ���������, ����� - <c>false</c>.</returns>
        public Maybe<bool> IsPersistently()
        {
            return this.Pick(o => ((SenderOptions)o).Persistently);
        }

        /// <summary>
        /// ���������� ��������� ���������� ��������� ���������.
        /// </summary>
        /// <returns>��������� ��������� ��������� ���������.</returns>
        public Maybe<IIncomingMessageHeaderStorage> GetIncomingMessageHeaderStorage()
        {
            return this.Pick(o => ((SenderOptions)o).IncomingMessageHeaderStorage);
        }
    }
}
