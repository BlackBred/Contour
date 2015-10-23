using System;
using System.Collections.Generic;
using System.Text;

using Contour.Helpers;

namespace Contour
{
    /// <summary>
    /// ��������� ���������.
    /// </summary>
    public class Headers
    {
        /// <summary>
        /// �������������� �������������, ����� ��� ����������� � ������ ������ ���������.
        /// </summary>
        public static readonly string CorrelationId = "x-correlation-id";

        /// <summary>
        /// ���������, ������� �������� ������� ����������� ������.
        /// �������������� ��������� ������: <c>x-expires: at 2014-04-01T22:00:33Z</c> ��� <c>x-expires: in 100</c>.
        /// </summary>
        public static readonly string Expires = "x-expires";

        /// <summary>
        /// ����� ���������, � ������� ��� ���� ����������.
        /// ������������� ����� ��������� �� �������������.
        /// </summary>
        public static readonly string MessageLabel = "x-message-type";

        /// <summary>
        /// �������� ���������� � ���, ��� ��������� ���������� ���������.
        /// </summary>
        public static readonly string Persist = "x-persist";

        /// <summary>
        /// ����� ��������� ��������� �� ������.
        /// </summary>
        public static readonly string ReplyRoute = "x-reply-route";

        /// <summary>
        /// ����� ������ �� ������.
        /// </summary>
        public static readonly string Timeout = "x-timeout";

        /// <summary>
        /// ����� ����� ���������.
        /// </summary>
        public static readonly string Ttl = "x-ttl";

        /// <summary>
        /// ����� ����� ��������� � �������.
        /// </summary>
        public static readonly string QueueMessageTtl = "x-message-ttl";

        /// <summary>
        /// ���� ��������� �� �������. �������� ������ ���� �������� �����, ����� ������� ������ ���������, ����������� �������� ";".
        /// </summary>
        public static readonly string Breadcrumbs = "x-breadcrumbs";

        /// <summary>
        /// ������������� ��������� ��������������� ����� �����������.
        /// </summary>
        public static readonly string OriginalMessageId = "x-original-message-id";

        /// <summary>
        /// �������� �������� ��������� �� ��������� � ������� ��� �� ������ ���������� ���������.
        /// </summary>
        /// <param name="headers">
        /// ������ ���������� ���������.
        /// </param>
        /// <param name="key">
        /// ��������� ���������, ��� �������� ����� ��������.
        /// </param>
        /// <typeparam name="T">��� ����������� ��������.</typeparam>
        /// <returns>���� ��������� ����������, ����� ��� ��������, ����� <c>null</c> ��� 0.</returns>
        public static T Extract<T>(IDictionary<string, object> headers, string key)
        {
            object value;
            if (headers.TryGetValue(key, out value))
            {
                headers.Remove(key);
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// �������� ��������� �������� ��������� �� ������ ����������.
        /// </summary>
        /// <param name="headers">��������� ���������� ���������.</param>
        /// <param name="key">��� ���������.</param>
        /// <returns>��������� �������� ��������� ��� ������ ������, ���� ��������� �� ���������� � ������.</returns>
        public static string GetString(IDictionary<string, object> headers, string key)
        {
            object value;
            if (headers.TryGetValue(key, out value))
            {
                if (value is string)
                {
                    return (string)value;
                }

                if (value is byte[])
                {
                    return Encoding.UTF8.GetString((byte[])value);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// ��������� � ��������� ���������� ��������� ��������� <c>Breadcrumbs</c>.
        /// </summary>
        /// <param name="headers">�������� ��������� ����������, ������� ������������ ����������.</param>
        /// <param name="endpoint">��� �������� ����� ������������ � ���������.</param>
        /// <returns>�������� ���������� ���������� � �����������.</returns>
        public static IDictionary<string, object> ApplyBreadcrumbs(IDictionary<string, object> headers, string endpoint)
        {
            if (!headers.ContainsKey(Breadcrumbs))
            {
                headers[Breadcrumbs] = endpoint;
            }
            else
            {
                headers[Breadcrumbs] = GetString(headers, Breadcrumbs) + ";" + endpoint;
            }

            return headers;            
        }

        /// <summary>
        /// ��������� � ��������� ���������� ��������� ��������� <c>OriginalMessageId</c>.
        /// </summary>
        /// <param name="headers">�������� ��������� ����������, ������� ������������ ����������.</param>
        /// <returns>�������� ���������� ���������� � �����������.</returns>
        public static IDictionary<string, object> ApplyOriginalMessageId(IDictionary<string, object> headers)
        {
            if (!headers.ContainsKey(OriginalMessageId))
            {
                headers[OriginalMessageId] = Guid.NewGuid().ToString("n");
            }

            return headers;
        }

        /// <summary>
        /// ��������� � ��������� ���������� ��������� ��������� <c>Persist</c>.
        /// </summary>
        /// <param name="headers">�������� ��������� ����������, ������� ������������ ����������.</param>
        /// <param name="persistently">��������� ��������������� ���������.</param>
        /// <returns>�������� ���������� ���������� � �����������.</returns>
        public static IDictionary<string, object> ApplyPersistently(IDictionary<string, object> headers, Maybe<bool> persistently)
        {
            if (persistently != null && persistently.HasValue)
            {
                headers[Persist] = persistently.Value;
            }

            return headers;
        }

        /// <summary>
        /// ��������� � ��������� ���������� ��������� ��������� <c>Ttl</c>.
        /// </summary>
        /// <param name="headers">�������� ��������� ����������, ������� ������������ ����������.</param>
        /// <param name="ttl">��������� ��������������� ���������.</param>
        /// <returns>�������� ���������� ���������� � �����������.</returns>
        public static IDictionary<string, object> ApplyTtl(IDictionary<string, object> headers, Maybe<TimeSpan?> ttl)
        {
            if (ttl != null && ttl.HasValue)
            {
                headers[Ttl] = ttl.Value;
            }

            return headers;
        }
    }
}
