using System.Collections.Generic;
using System.Linq;
using System.Net;

using RestSharp;
using RestSharp.Authenticators;

namespace Contour.Testing.Plumbing
{
    /// <summary>
    /// ������������ ����������� � ������� <c>RabbitMQ</c>.
    /// </summary>
    public class Broker
    {
        private readonly string connection;

        private readonly string username;

        private readonly string password;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="Broker"/>. 
        /// </summary>
        /// <param name="connection">������ ����������� � �������.</param>
        /// <param name="username">��� ������������.</param>
        /// <param name="password">������ ������������.</param>
        public Broker(string connection, string username, string password)
        {
            this.connection = connection;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// ������� �������� ����� ������������� � �������.
        /// </summary>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <param name="exchangeName">��� ����� �������������.</param>
        /// <param name="queueName">��� �������.</param>
        public void CreateBind(string vhostName, string exchangeName, string queueName)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/bindings/{vhost}/e/{exchange}/q/{queue}", Method.POST);
            request.AddUrlSegment("vhost", vhostName);
            request.AddUrlSegment("exchange", exchangeName);
            request.AddUrlSegment("queue", queueName);
            client.Execute(request);
        }

        /// <summary>
        /// ���������� ������ ��������� �� �������.
        /// </summary>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <param name="queueName">��� �������.</param>
        /// <param name="count">���������� ���������� ��������� �� �������.</param>
        /// <param name="requeue">���� <c>true</c> - ����� ����� ���������� ��������� � �������.</param>
        /// <param name="encoding">��������� ��������� (�� ��������� <c>auto</c>).</param>
        /// <returns>������ ��������� �� �������.</returns>
        public List<Message> GetMessages(string vhostName, string queueName, int count, bool requeue, string encoding = "auto")
        {
            var options = new
                              {
                                  vhost = vhostName,
                                  name = queueName,
                                  count,
                                  requeue,
                                  encoding
                              };
            var client = this.CreateClient();
            var request = CreateRequest("/api/queues/{vhost}/{queue}/get", Method.POST);
            request.AddUrlSegment("vhost", vhostName);
            request.AddUrlSegment("queue", queueName);
            request.AddBody(options);
            var response = client.Execute<List<Message>>(request);
            return response.StatusCode == HttpStatusCode.OK ? response.Data : new List<Message>();
        }

        /// <summary>
        /// ���������� ������ �������� � ����������� �����.
        /// </summary>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <returns>������ �������� ������������ ����� �������..</returns>
        public IList<Queue> GetQueues(string vhostName)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/queues/{name}", Method.GET);
            request.AddUrlSegment("name", vhostName);
            var response = client.Execute<List<Queue>>(request);

            return response.StatusCode == HttpStatusCode.OK ? response.Data : new List<Queue>();
        }

        /// <summary>
        /// ��������� ������� ����� ������������� � ����������� ����� �������.
        /// </summary>
        /// <param name="exchangeName">��� ����� ������������� �������.</param>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <returns><c>true</c> - ���� ����� ������������� �������.</returns>
        public bool HasExchange(string exchangeName, string vhostName)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/exchanges/{name}", Method.GET);
            request.AddUrlSegment("name", vhostName);
            var response = client.Execute<List<Exchange>>(request);

            return response.StatusCode == HttpStatusCode.OK && response.Data.Any(ex => ex.Name == exchangeName);
        }

        /// <summary>
        /// ��������� ������� ������� � ����������� ����� �������.
        /// </summary>
        /// <param name="queueName">��� �������.</param>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <returns><c>true</c> - ���� ������� �������.</returns>
        public bool HasQueue(string queueName, string vhostName)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/queues/{name}", Method.GET);
            request.AddUrlSegment("name", vhostName);
            var response = client.Execute<List<Queue>>(request);

            return response.StatusCode == HttpStatusCode.OK && response.Data.Any(queue => queue.Name == queueName);
        }

        /// <summary>
        /// ������� ����������� ���� � �������.
        /// </summary>
        /// <param name="vhostName">��� ������������ �����.</param>
        public void CreateHost(string vhostName)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/vhosts/{name}", Method.PUT);
            request.AddUrlSegment("name", vhostName);
            client.Execute(request);
        }

        /// <summary>
        /// ������� ������� � ����������� ����� �������.
        /// </summary>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <param name="queueName">��� ����������� �������.</param>
        public void CreateQueue(string vhostName, string queueName)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/queues/{vhost}/{queue}", Method.PUT);
            request.AddUrlSegment("vhost", vhostName);
            request.AddUrlSegment("queue", queueName);
            request.AddHeader("Accept", string.Empty);
            client.Execute(request);
        }

        /// <summary>
        /// ������� ������������ � ����������� ����� �������.
        /// </summary>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <param name="user">��� ������������.</param>
        /// <param name="password">������ ������������.</param>
        public void CreateUser(string vhostName, string user, string password)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/users/{name}", Method.PUT);
            request.AddUrlSegment("name", vhostName);
            request.AddUrlSegment("user", user);
            request.AddBody(new { password, tags = "administrator" });
            client.Execute(request);
        }

        /// <summary>
        /// ������������� ����� ������������.
        /// </summary>
        /// <param name="vhostName">����������� ���� �������.</param>
        /// <param name="user">��� ������������.</param>
        public void SetPermissions(string vhostName, string user)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/permissions/{name}/{user}", Method.PUT);
            request.AddUrlSegment("name", vhostName);
            request.AddUrlSegment("user", user);
            request.AddBody(new { Configure = ".*", Write = ".*", Read = ".*" });
            client.Execute(request);
        }

        /// <summary>
        /// ������� ����������� ���� �������.
        /// </summary>
        /// <param name="vhostName">��� ������������ ����� �������.</param>
        public void DeleteHost(string vhostName)
        {
            var client = this.CreateClient();
            var request = CreateRequest("/api/vhosts/{name}", Method.DELETE);
            request.AddUrlSegment("name", vhostName);
            client.Execute(request);
        }

        private static RestRequest CreateRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method)
                              {
                                  JsonSerializer = new RabbitJsonSerializer(), RequestFormat = DataFormat.Json
                              };
            request.AddHeader("Accept", string.Empty);
            return request;
        }

        private IRestClient CreateClient()
        {
            var client = new RestClient(this.connection)
                             {
                                 Authenticator = new HttpBasicAuthenticator(this.username, this.password)
                             };
            client.AddDefaultHeader("Content-Type", "application/json; charset=utf-8");
            return client;
        }
    }
}
