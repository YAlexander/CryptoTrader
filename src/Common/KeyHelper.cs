using System.Text.Json;

namespace Common
{
    public static class KeyHelper
    {
        public static GrainKeyExtension ToExtended (this string key)
        {
            return JsonSerializer.Deserialize<GrainKeyExtension>(key);
        }
    }
}