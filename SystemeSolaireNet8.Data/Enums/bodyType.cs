using System.Runtime.Serialization;

namespace SystemeSolaireNet8.Data.Enums;

public enum BodyType
{
  Star,
  Planet,

  [EnumMember(Value = "Dwarf Planet")]
  DwarfPlanet,

  Asteroid,
  Comet,
  Moon
}