using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Noteflix.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NoteTaskStatus
    {
        Pending,
        Completed,
        Archived
    }
}
