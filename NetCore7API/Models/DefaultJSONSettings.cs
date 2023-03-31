using System.Text.Json;

namespace NetCore7API.Models
{
    public static class DefaultJsonOptions
    {
        public static JsonSerializerOptions Web => new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
        };
    }
}