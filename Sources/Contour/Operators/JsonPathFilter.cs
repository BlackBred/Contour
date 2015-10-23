namespace Contour.Operators
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// ������ �� ������ ��������� JsonPath.
    /// </summary>
    public class JsonPathFilter : Filter
    {
        /// <summary>
        /// �������������� ��������.
        /// </summary>
        /// <param name="jsonPath">��������� JsonPath.</param>
        public JsonPathFilter(string jsonPath)
            : base(m => Predicate(m, jsonPath))
        {
        }

        private static bool Predicate(IMessage message, string jsonPath)
        {
            var json = JObject.FromObject(message.Payload);
            return json.SelectToken(jsonPath) != null;
        }
    }
}
