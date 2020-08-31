using Newtonsoft.Json;

namespace StackExchangePopularTags.Models
{
    public class TagViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        public decimal Popularity { get; set; }

    }
}
