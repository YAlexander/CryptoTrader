using System.Text.Json;

namespace Core.OrleansInfrastructure
{
    public static class KeyHelper
    {
        public static GrainKeyExtension ToExtended (this string key)
        {
            return JsonSerializer.Deserialize<GrainKeyExtension>(key);
        }
        
        public static string ToString (this GrainKeyExtension key)
        {
            return JsonSerializer.Serialize(key);
        }
    }
}