using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Noteflix.Core.Enums;

namespace Noteflix.Core.Entities.SubEntities
{
    public class NoteTask
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "status")]
        public NoteTaskStatus Status { get; set; }
    }
}
