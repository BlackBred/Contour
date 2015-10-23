using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contour.Configuration;

namespace Contour.Sending
{
    /// <summary>
    /// ����������� ���������.
    /// </summary>
    public interface ISender : IBusComponent
    {
        /// <summary>
        /// ��������� ����������� ������� ������� ��� ��������� ����� ���������.
        /// </summary>
        /// <param name="label">����� ���������, ��� ������� ���������� ��������� �������.</param>
        /// <returns><c>true</c> - ���� ��� ��������� ����� ����� ��������� �������.</returns>
        bool CanRoute(MessageLabel label);

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="payload">��������� �������.</param>
        /// <param name="options">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        [Obsolete("���������� ������������ ����� Request � ��������� ����� ���������.")]
        Task<T> Request<T>(object payload, RequestOptions options) where T : class;

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="payload">��������� �������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        [Obsolete("���������� ������������ ����� Request � ��������� ����� ���������.")]
        Task<T> Request<T>(object payload, IDictionary<string, object> headers) where T : class;

        /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="payload">���� ���������.</param>
        /// <param name="headers">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        [Obsolete("���������� ������������ ����� Send � ��������� ����� ���������.")]
        Task Send(object payload, IDictionary<string, object> headers);

        /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        [Obsolete("���������� ������������ ����� Send � ��������� ����� ���������.")]
        Task Send(object payload, PublishingOptions options);

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="payload">��������� �������.</param>
        /// <param name="options">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        Task<T> Request<T>(MessageLabel label, object payload, RequestOptions options) where T : class;

        /// <summary>
        /// ���������� ��������� � ������� ������-�����.
        /// </summary>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="payload">��������� �������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <typeparam name="T">��� ��������� ������.</typeparam>
        /// <returns>������ ���������� �������.</returns>
        Task<T> Request<T>(MessageLabel label, object payload, IDictionary<string, object> headers) where T : class;

        /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="headers">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        Task Send(MessageLabel label, object payload, IDictionary<string, object> headers);

        /// <summary>
        /// ���������� ������������� ���������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">���� ���������.</param>
        /// <param name="options">��������� ���������.</param>
        /// <returns>������ ���������� �������� ���������.</returns>
        Task Send(MessageLabel label, object payload, PublishingOptions options);
    }
}
