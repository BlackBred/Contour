using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Contour.Sending;
using Contour.Serialization;

namespace Contour
{
    /// <summary>
    ///   �������� ���� ���������.
    ///   ������������ ��� �������� � ��������� ���������.
    /// </summary>
    public interface IBusContext
    {
        /// <summary>
        ///   �������� ���������� ����
        /// </summary>
        IEndpoint Endpoint { get; }

        /// <summary>
        ///   ���������� ��� ������ ���������� ����.
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// ���������� ����� ���������.
        /// </summary>
        IMessageLabelHandler MessageLabelHandler { get; }

        /// <summary>
        /// ����������� ���� ���������, � �������� �������������.
        /// </summary>
        IPayloadConverter PayloadConverter { get; }

        /// <summary>
        /// ������� ������ ���������� ����.
        /// </summary>
        WaitHandle WhenReady { get; }

        /// <summary>
        /// ��������� ����������� ��������� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ���������.</param>
        /// <returns>
        /// ���� <c>true</c> - ����� ���� ����� ������������ ��������� � ����� ������, ����� - <c>false</c>.
        /// </returns>
        bool CanHandle(string label);

        /// <summary>
        /// ��������� ����������� ��������� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ���������.</param>
        /// <returns>
        /// ���� <c>true</c> - ����� ���� ����� ������������ ��������� � ����� ������, ����� - <c>false</c>.
        /// </returns>
        bool CanHandle(MessageLabel label);

        /// <summary>
        /// ��������� ����������� �������� � �������������� ��������� �����.
        /// </summary>
        /// <param name="label">����� ���������.</param>
        /// <returns>
        /// ���� <c>true</c> - ����� ���� ����� ���������� ��������� � ����� ������, ����� - <c>false</c>.
        /// </returns>
        bool CanRoute(string label);

        /// <summary>
        /// ��������� ����������� �������� � �������������� ��������� �����.
        /// </summary>
        /// <param name="label">
        /// ����� ���������.
        /// </param>
        /// <returns>
        /// ���� <c>true</c> - ����� ���� ����� ���������� ��������� � ����� ������, ����� - <c>false</c>.
        /// </returns>
        bool CanRoute(MessageLabel label);

        /// <summary>
        /// ���������� ��������� � ��������� ������.
        /// </summary>
        /// <remarks>����� ��������� ������ ���� ����������������.</remarks>
        /// <typeparam name="T"><c>.NET</c> ��� ������������� ���������.
        /// </typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">������������ ���������.</param>
        /// <param name="options">��������� �����������.</param>
        /// <returns>
        /// ������ �������� ��������.
        /// </returns>
        Task Emit<T>(string label, T payload, PublishingOptions options = null) where T : class;

        /// <summary>
        /// ���������� ��������� � ��������� ������.
        /// </summary>
        /// <typeparam name="T"><c>.NET</c> ��� ������������� ���������.</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">������������ ���������.</param>
        /// <param name="options">��������� ��������.</param>
        /// <returns>������ �������� ��������.</returns>
        Task Emit<T>(MessageLabel label, T payload, PublishingOptions options = null) where T : class;

        /// <summary>
        /// ���������� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">������������ ���������.</param>
        /// <param name="headers">��������� ������������� ���������.</param>
        /// <returns>
        /// <returns>������ �������� ��������.</returns>
        /// </returns>
        Task Emit(MessageLabel label, object payload, IDictionary<string, object> headers);

        /// <summary>
        /// ��������� ���������� ������ ������ � ��������� ������.
        /// </summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������. </param>
        void Request<TRequest, TResponse>(string label, TRequest request, Action<TResponse> responseAction) where TRequest : class where TResponse : class;

        /// <summary>
        /// ��������� ���������� ������ ������ � ��������� ������.
        /// </summary>
        /// <typeparam name="TRequest">��� ������ �������. </typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        void Request<TRequest, TResponse>(MessageLabel label, TRequest request, Action<TResponse> responseAction) where TRequest : class where TResponse : class;

        /// <summary>��������� ���������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �����������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        void Request<TRequest, TResponse>(string label, TRequest request, RequestOptions options, Action<TResponse> responseAction) where TRequest : class where TResponse : class;

        /// <summary>��������� ���������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �����������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        void Request<TRequest, TResponse>(MessageLabel label, TRequest request, RequestOptions options, Action<TResponse> responseAction) where TRequest : class where TResponse : class;

        /// <summary>��������� ���������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        void Request<TRequest, TResponse>(MessageLabel label, TRequest request, IDictionary<string, object> headers, Action<TResponse> responseAction)
            where TRequest : class
            where TResponse : class;

        /// <summary>��������� ����������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �������.</param>
        /// <returns>������ ��������� ��������� ���������.</returns>
        Task<TResponse> RequestAsync<TRequest, TResponse>(string label, TRequest request, RequestOptions options = null) where TRequest : class where TResponse : class;

        /// <summary>��������� ����������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �����������.</param>
        /// <returns>������ ��������� ��������� ���������.</returns>
        Task<TResponse> RequestAsync<TRequest, TResponse>(MessageLabel label, TRequest request, RequestOptions options = null) where TRequest : class where TResponse : class;

        /// <summary>��������� ����������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="headers">��������� �������.</param>
        /// <returns>������ ��������� ��������� ���������.</returns>
        Task<TResponse> RequestAsync<TRequest, TResponse>(MessageLabel label, TRequest request, IDictionary<string, object> headers)
            where TRequest : class
            where TResponse : class;
    }
}
