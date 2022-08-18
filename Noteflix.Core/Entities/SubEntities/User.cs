using Newtonsoft.Json;

namespace Noteflix.Core.Entities
{
    public class User
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
    }
}
