using Newtonsoft.Json;
using SolarSystemN9.Data.Type;

namespace SolarSystemN9.Data.Entity;

public class CelestialBody
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("englishName")]
    public string? EnglishName { get; set; }

    [JsonProperty("isPlanet")]
    public bool IsPlanet { get; set; }

    [JsonProperty("moons")]
    public Moon[] Moons { get; set; } = [];

    [JsonProperty("semimajorAxis")]
    public long SemimajorAxis { get; set; }

    [JsonProperty("perihelion")]
    public long Perihelion { get; set; }

    [JsonProperty("aphelion")]
    public long Aphelion { get; set; }

    [JsonProperty("eccentricity")]
    public decimal Eccentricity { get; set; }

    [JsonProperty("inclination")]
    public decimal Inclination { get; set; }

    [JsonProperty("mass")]
    public SpaceDictionary? Mass { get; set; }

    [JsonProperty("vol")]
    public SpaceDictionary? Vol { get; set; }

    [JsonProperty("density")]
    public decimal Density { get; set; }

    [JsonProperty("gravity")]
    public decimal Gravity { get; set; }

    [JsonProperty("escape")]
    public decimal Escape { get; set; }

    [JsonProperty("meanRadius")]
    public decimal MeanRadius { get; set; }

    [JsonProperty("equaRadius")]
    public decimal EquaRadius { get; set; }

    [JsonProperty("polarRadius")]
    public decimal PolarRadius { get; set; }

    [JsonProperty("dimension")]
    public string? Dimension { get; set; }

    [JsonProperty("sideralOrbit")]
    public decimal SideralOrbit { get; set; }

    [JsonProperty("sideralRotation")]
    public decimal SideralRotation { get; set; }

    [JsonProperty("aroundPlanet")]
    public Planet? AroundPlanet { get; set; }

    [JsonProperty("discoveredBy")]
    public string? DiscoveredBy { get; set; }

    [JsonProperty("discoveryDate")]
    public string? DiscoveryDate { get; set; }

    [JsonProperty("alternativeName")]
    public string? AlternativeName { get; set; }

    [JsonProperty("axialTilt")]
    public decimal AxialTilt { get; set; }

    [JsonProperty("avgTemp")]
    public int AvgTemp { get; set; }

    [JsonProperty("mainAnomaly")]
    public decimal MainAnomaly { get; set; }

    [JsonProperty("argPeriapsis")]
    public decimal ArgPeriapsis { get; set; }

    [JsonProperty("longAscNode")]
    public decimal LongAscNode { get; set; }

    [JsonProperty("bodyType")]
    public string? BodyType { get; set; }

    public override string ToString()
    {
        return $"{Name} (ID: {Id}, Type: {BodyType})";
    }
}