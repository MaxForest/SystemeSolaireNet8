using Newtonsoft.Json;

namespace SolarSystemN9.Data.Entity;

public class SolarSystem
{
    [JsonProperty("bodies")]
    public CelestialBody[]? Bodies { get; set; }
}