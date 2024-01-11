using SystemeSolaireNet8.Data.Enums;
using SystemeSolaireNet8.Data.Type;

namespace SystemeSolaireNet8.Data.Entity;

  public class SpaceEntity
  {
      public string? id { get; set; }
      public string? name { get; set; }
      public string? englishName { get; set; }

      public bool isPlanet { get; set; }

      public Moon[]? moons { get; set; } = Array.Empty<Moon>(); 

      public long semimajorAxis { get; set; }
      public long perihelion { get; set; }

      public long aphelion { get; set; }
      public decimal eccentricity { get; set; }
      public decimal inclination { get; set; }

      public SpaceDictionary? mass { get; set; }
      public SpaceDictionary? vol { get; set; }

      public decimal density { get; set; }
      public decimal gravity { get; set; }
      public decimal escape { get; set; }

      public decimal meanRadius { get; set; }
      public decimal equaRadius { get; set; }
      public decimal polarRadius { get; set; }

      public string? dimension { get; set; }
      public decimal sideralOrbit { get; set; }
      public decimal sideralRotation { get; set; }

      public Planet? aroundPlanet { get; set; }
      public string? discoveredBy { get; set; }
      public string? discoveryDate { get; set; }
      public string? alternativeName { get; set; }

      public decimal axialTilt { get; set; }
      public int avgTemp { get; set; }
      public decimal mainAnomaly { get; set; }
      public decimal argPeriapsis { get; set; }
      public decimal longAscNode { get; set; }

      public BodyType bodyType { get; set; }

      public override string ToString()
      {
          return $"{name}";
      }
  }