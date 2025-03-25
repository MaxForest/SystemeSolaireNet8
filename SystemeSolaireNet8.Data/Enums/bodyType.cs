using System.Runtime.Serialization;

namespace SolarSystemN9.Data.Enums;

public enum BodyType
{
    [EnumMember(Value = "Star")]
    Star,

    [EnumMember(Value = "Planet")]
    Planet,

    [EnumMember(Value = "Dwarf Planet")]
    DwarfPlanet,

    [EnumMember(Value = "Asteroid")]
    Asteroid,

    [EnumMember(Value = "Comet")]
    Comet,

    [EnumMember(Value = "Moon")]
    Moon
}