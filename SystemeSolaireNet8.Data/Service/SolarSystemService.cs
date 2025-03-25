using Microsoft.Extensions.Caching.Memory;
using SolarSystemN9.Data.Entity;
using System.Net.Http.Json;

namespace SolarSystemN9.Data.Service;

public class SolarSystemService(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
{
    private const string BodiesApiEndpoint = "https://api.le-systeme-solaire.net/rest/bodies";
    private readonly HttpClient _client = httpClientFactory.CreateClient();

    public async Task<IQueryable<Bodies>?> GetBodiesAsync()
    {
        try
        {
            return await memoryCache.GetOrCreateAsync("SolarSystem", async cache =>
            {
                HttpResponseMessage response = await _client.GetAsync(BodiesApiEndpoint);
                response.EnsureSuccessStatusCode();

                SolarSystem? solarSystem = await response.Content.ReadFromJsonAsync<SolarSystem>();
                return solarSystem?.Bodies?.AsQueryable();
            });
        }
        catch (Exception)
        {
            // Log the exception (logging mechanism not shown here)
            return null;
        }
    }

    public async Task<IQueryable<Bodies>?> GetBodies(string query)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"{BodiesApiEndpoint}?filter[]=name,cs,{query}");
            response.EnsureSuccessStatusCode();

            SolarSystem? solarSystem = await response.Content.ReadFromJsonAsync<SolarSystem>();
            return solarSystem?.Bodies?.AsQueryable();
        }
        catch (Exception)
        {
            // Log the exception (logging mechanism not shown here)
            return null;
        }
    }

    public async Task<Bodies?> GetEntity(string id)
    {
        try
        {
            //return await memoryCache.GetOrCreateAsync(id, async cache =>
            //{
            HttpResponseMessage response = await _client.GetAsync($"{BodiesApiEndpoint}/{id}");
            response.EnsureSuccessStatusCode();

            Bodies? spaceEntity = await response.Content.ReadFromJsonAsync<Bodies>();
            if (spaceEntity == null) return null;

            await LoadMoons(spaceEntity);
            //await LoadAroundPlanet(spaceEntity);

            return spaceEntity;
            //});
        }
        catch (Exception)
        {
            // Log the exception (logging mechanism not shown here)
            return null;
        }
    }

    private async Task LoadMoons(Bodies spaceEntity)
    {
        if (spaceEntity?.Moons?.Length > 0)
        {
            foreach (var moon in spaceEntity.Moons.ToList().Select((x, i) => new { Value = x, Index = i }))
            {
                HttpResponseMessage response = await _client.GetAsync($"{BodiesApiEndpoint}/{moon.Value.Relationship}");
                response.EnsureSuccessStatusCode();

                Moon? tmpMoon = await response.Content.ReadFromJsonAsync<Moon>();
                if (tmpMoon != null)
                {
                    spaceEntity.Moons[moon.Index] = tmpMoon;
                }
            }
        }
    }

    private async Task LoadAroundPlanet(Bodies spaceEntity)
    {
        if (spaceEntity?.AroundPlanet != null)
        {
            HttpResponseMessage response = await _client.GetAsync($"{BodiesApiEndpoint}/{spaceEntity.AroundPlanet.Relationship}");
            response.EnsureSuccessStatusCode();

            spaceEntity.AroundPlanet = await response.Content.ReadFromJsonAsync<Planet>();
        }
    }
}