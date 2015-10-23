using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Contour
{
    internal sealed class MessageHeaderStorage : IIncomingMessageHeaderStorage
    {
        private readonly HashSet<string> blackHeaderSet;

        private readonly string storageKey = "Contour.MessageHeaderStorage";

        public MessageHeaderStorage(IEnumerable<string> blackHeaders)
        {
            this.blackHeaderSet = new HashSet<string>(blackHeaders);
        }

        /// <summary>
        /// ��������� ��������� ��������� ���������.
        /// </summary>
        /// <param name="headers">��������� ��������� ���������.</param>
        public void Store(IDictionary<string, object> headers)
        {
            var refindedHeaders = headers
                .Where(p => !this.blackHeaderSet.Contains(p.Key))
                .ToDictionary(p => p.Key, p => p.Value);
            CallContext.LogicalSetData(this.storageKey, refindedHeaders);
        }

        /// <summary>
        /// ���������� ����������� ��������� ��������� ���������.
        /// </summary>
        /// <returns>��������� ��������� ���������.</returns>
        public IDictionary<string, object> Load()
        {
            return (IDictionary<string, object>)CallContext.LogicalGetData(this.storageKey);
        }
    }
}
