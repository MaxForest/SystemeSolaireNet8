using Newtonsoft.Json;

namespace SolarSystemN9.Data.Entity;

public class Planet : Bodies
{
    [JsonProperty("planet")]
    public new string? Name { get; set; }

    [JsonProperty("rel")]
    public string? Relationship { get; set; }
}