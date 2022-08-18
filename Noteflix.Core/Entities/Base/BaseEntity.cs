using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Noteflix.Core.Enums;

namespace Noteflix.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; }

        [JsonProperty(PropertyName = "isDeleted")]
        public bool IsDeleted { get; protected set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "type")]
        public EntityType Type { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "ttl")]
        public int Ttl { get; set; }

        [JsonIgnore]
        public EntityState State { get; set; }

        public virtual void SetDeleted() => this.IsDeleted = true;

        public abstract void SetPartitionKey();
    }
}
