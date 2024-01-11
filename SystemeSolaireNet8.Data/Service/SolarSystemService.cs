using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Reflection;
using SystemeSolaireNet8.Data.Entity;

namespace SystemeSolaireNet8.Data.Service
{
  public class SolarSystemService(IMemoryCache memoryCache)
  {
    private readonly HttpClient client = new();

    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<IQueryable<SpaceEntity>?> GetBodies()
    {
      return await _memoryCache.GetOrCreateAsync("SolarSystem", async cache =>
      {
        HttpResponseMessage response = await client.GetAsync("https://api.le-systeme-solaire.net/rest/bodies/");
        if (response.IsSuccessStatusCode)
        {
          return JsonConvert.DeserializeObject<SolarSystem>(await response.Content.ReadAsStringAsync())?.Bodies?.AsQueryable();
        }

        throw new Exception();
      });
    }

    public async Task<IQueryable<SpaceEntity>?> GetBodies(string query)
    {
      HttpResponseMessage response = await client.GetAsync($"https://api.le-systeme-solaire.net/rest/bodies?filter[]=name,cs,{query}");
      if (response.IsSuccessStatusCode)
      {
        return JsonConvert.DeserializeObject<SolarSystem>(await response.Content.ReadAsStringAsync())?.Bodies?.AsQueryable();
      }

      throw new Exception();
    }

    public async Task<SpaceEntity?> GetEntity(string id)
    {
      return await _memoryCache.GetOrCreateAsync(id, async cache =>
      {
        HttpResponseMessage response = await client.GetAsync($"https://api.le-systeme-solaire.net/rest/bodies/{id}");

        if (response.IsSuccessStatusCode)
        {
          SpaceEntity? spaceEntity = JsonConvert.DeserializeObject<SpaceEntity?>(await response.Content.ReadAsStringAsync());

          if (spaceEntity?.moons?.Length > 0)
          {
            foreach (var moon in spaceEntity.moons.ToList().Select((x, i) => new { Value = x, Index = i }))
            {
              response = await client.GetAsync(moon.Value.rel);

              if (response.IsSuccessStatusCode)
              {
                Moon? tmpMoon = JsonConvert.DeserializeObject<Moon>(await response.Content.ReadAsStringAsync());
                if (tmpMoon != null)
                {
                  spaceEntity.moons[moon.Index] = tmpMoon;
                }
              }
            }
          }

          if (spaceEntity?.aroundPlanet != null)
          {
            response = await client.GetAsync(spaceEntity.aroundPlanet.rel);

            if (response.IsSuccessStatusCode)
            {
              spaceEntity.aroundPlanet = JsonConvert.DeserializeObject<Planet>(await response.Content.ReadAsStringAsync());
            }
          }

          return spaceEntity;
        }

        throw new Exception();
      });
    }
  }
}