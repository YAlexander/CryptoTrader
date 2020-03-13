using System.Text.Json;

namespace Persistence.Helpers
{
    public class JsonHelper<T> where T : new()
    {
        public T FromJson(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        
        public string ToJson(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}