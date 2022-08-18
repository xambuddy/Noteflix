using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Noteflix.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntityState
    {
        Created,
        Updated,
        Deleted,
        Unmodified,
    }
}
