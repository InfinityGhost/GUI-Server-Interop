using Newtonsoft.Json;

namespace Library.IPC
{
    public class JsonExtensions
    {
        public string ToJson<T>(T obj, bool indent = false) where T : class
        {
            return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None);
        }

        public T FromJson<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}