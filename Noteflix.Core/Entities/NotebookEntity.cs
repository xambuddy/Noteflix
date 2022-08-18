using Newtonsoft.Json;
using Noteflix.Core.Entities.Base;

namespace Noteflix.Core.Entities
{
    public class NotebookEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "pageCount")]
        public int PageCount { get; set; }

        public override void SetPartitionKey()
        {
            PartitionKey = Id;
        }
    }
}
