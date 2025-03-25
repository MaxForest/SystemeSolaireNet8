using Newtonsoft.Json;
using System.Text.Json;

namespace SolarSystemN9.Data.Entity
{
    public class Moon : Bodies
    {
        [JsonProperty("moon")]
        public new string? Name { get; set; }

        [JsonProperty("rel")]
        public string? Relationship { get; set; }
    }
}