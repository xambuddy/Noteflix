using Newtonsoft.Json;
using Noteflix.Core.Entities.Base;
using Noteflix.Core.Entities.SubEntities;

namespace Noteflix.Core.Entities
{
    public class NoteEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public User CreatedBy { get; set; }

        [JsonProperty(PropertyName = "notebookId")]
        public string NotebookId { get; set; }

        [JsonProperty(PropertyName = "tasks")]
        public IEnumerable<NoteTask> Tasks { get; set; }

        public override void SetPartitionKey()
        {
            PartitionKey = NotebookId;
        }
    }
}
