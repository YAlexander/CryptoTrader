using System.Text.Json;

namespace Persistence.Helpers
{
    public class OptionsHelper<T> where T : new()
    {
        public T FromJson(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        
        public string Serialize(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}