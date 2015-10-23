using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Contour.Configuration;
using Contour.Sending;

namespace Contour
{
    /// <summary>
    ///   ������ ���� ���������.
    ///   ������������ ��� ���������� ������ ����� ������� ����, � ����� ��� �������� ���������.
    /// </summary>
    public interface IBus : IBusContext, IDisposable
    {
        /// <summary>
        ///   ������� ����������� � �������
        /// </summary>
        event Action<IBus, EventArgs> Connected;

        /// <summary>
        ///   ������� ������� ���������� � ��������
        /// </summary>
        event Action<IBus, EventArgs> Disconnected;

        /// <summary>
        ///   ������� ��������� ������� ����
        /// </summary>
        event Action<IBus, EventArgs> Started;

        /// <summary>
        ///   ������� ������ ������� ����
        /// </summary>
        event Action<IBus, EventArgs> Starting;

        /// <summary>
        ///   ������� ��������� ��������� ����
        /// </summary>
        event Action<IBus, EventArgs> Stopped;

        /// <summary>
        ///   ������� ������ ��������� ����
        /// </summary>
        event Action<IBus, EventArgs> Stopping;

        /// <summary>
        ///   ������������ ����
        /// </summary>
        IBusConfiguration Configuration { get; }

        /// <summary>
        ///   ��������� ����������, ������� �� ������ ��������� ����
        /// </summary>
        bool IsStarted { get; }

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
        new Task Emit<T>(string label, T payload, PublishingOptions options = null) where T : class;

        /// <summary>
        /// ���������� ��������� � ��������� ������.
        /// </summary>
        /// <typeparam name="T"><c>.NET</c> ��� ������������� ���������.</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">������������ ���������.</param>
        /// <param name="options">��������� ��������.</param>
        /// <returns>������ �������� ��������.</returns>
        new Task Emit<T>(MessageLabel label, T payload, PublishingOptions options = null) where T : class;

        /// <summary>
        /// ���������� ��������� � ��������� ������.
        /// </summary>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="payload">������������ ���������.</param>
        /// <param name="headers">��������� ������������� ���������.</param>
        /// <returns>
        /// <returns>������ �������� ��������.</returns>
        /// </returns>
        new Task Emit(MessageLabel label, object payload, IDictionary<string, object> headers);

        /// <summary>
        /// ��������� ���������� ������ ������ � ��������� ������.
        /// </summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������. </param>
        new void Request<TRequest, TResponse>(string label, TRequest request, Action<TResponse> responseAction)
            where TRequest : class
            where TResponse : class;

        /// <summary>
        /// ��������� ���������� ������ ������ � ��������� ������.
        /// </summary>
        /// <typeparam name="TRequest">��� ������ �������. </typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        new void Request<TRequest, TResponse>(MessageLabel label, TRequest request, Action<TResponse> responseAction)
            where TRequest : class
            where TResponse : class;

        /// <summary>��������� ���������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �����������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        new void Request<TRequest, TResponse>(string label, TRequest request, RequestOptions options, Action<TResponse> responseAction)
            where TRequest : class
            where TResponse : class;

        /// <summary>��������� ���������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� �������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �����������.</param>
        /// <param name="responseAction">�������� ������� ����� ��������� ��� ��������� ��������� ���������.</param>
        new void Request<TRequest, TResponse>(MessageLabel label, TRequest request, RequestOptions options, Action<TResponse> responseAction)
            where TRequest : class
            where TResponse : class;

        /// <summary>��������� ����������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������.</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �������.</param>
        /// <returns>������ ��������� ��������� ���������.</returns>
        new Task<TResponse> RequestAsync<TRequest, TResponse>(string label, TRequest request, RequestOptions options = null)
            where TRequest : class
            where TResponse : class;

        /// <summary>��������� ����������� ������ ������ � ��������� ������.</summary>
        /// <typeparam name="TRequest">��� ������ �������.</typeparam>
        /// <typeparam name="TResponse">��� ���������� ������</typeparam>
        /// <param name="label">����� ������������� ���������.</param>
        /// <param name="request">������������ ���������.</param>
        /// <param name="options">��������� �����������.</param>
        /// <returns>������ ��������� ��������� ���������.</returns>
        new Task<TResponse> RequestAsync<TRequest, TResponse>(MessageLabel label, TRequest request, RequestOptions options = null)
            where TRequest : class
            where TResponse : class;

        /// <summary>
        ///   ��������� ���� � ������������ ���� ��������.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// ���������������� � ������ ������� ����.
        /// </summary>
        /// <param name="waitForReadiness">
        /// ���������� ������� ������ ���������� ����.
        /// </param>
        void Start(bool waitForReadiness = true);

        /// <summary>
        ///   ��������� ������� ����.
        /// </summary>
        void Stop();
    }
}
