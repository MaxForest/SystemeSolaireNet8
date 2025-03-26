using Newtonsoft.Json;

namespace SolarSystemN9.Data.Entity;

public class Planet : CelestialBody
{
    //[JsonProperty("planet")]
    public string? planet { get; set; }

    //[JsonProperty("rel")]
    public string? rel { get; set; }
}