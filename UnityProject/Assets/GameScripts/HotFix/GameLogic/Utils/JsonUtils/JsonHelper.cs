using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utils.JsonUtils
{
    public static class JsonHelper
    {
        public static JObject FromJson(string json)
        {
            return JsonConvert.DeserializeObject<JObject>(json);
        }

        public static string ToJson(JObject jObject)
        {
            return JsonConvert.SerializeObject(jObject);
        }

        public static bool ContainsKey(JObject data, string key)
        {
            if (data == null)
            {
                return false;
            }

            return data.ContainsKey(key);
        }
    }
}