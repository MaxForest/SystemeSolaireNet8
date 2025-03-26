using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SolarSystemN9.Data.Entity;
using System;
using System.Net.Http.Json;

namespace SolarSystemN9.Data.Service;

public class SolarSystemService(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, ILogger<SolarSystemService> logger)
{
    private const string BodiesApiEndpoint = "https://api.le-systeme-solaire.net/rest/bodies";
    private readonly HttpClient _client = httpClientFactory.CreateClient();

    public async Task<IQueryable<CelestialBody>?> GetCelestialBodiesAsync()
    {
        try
        {
            return await memoryCache.GetOrCreateAsync("SolarSystem", async cache =>
            {
                return await GetSolarSystem(BodiesApiEndpoint);
            });
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error fetching bodies from API.");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            return null;
        }
    }

    public async Task<IQueryable<CelestialBody>?> GetCelestialBodiesAsync(string query)
    {
        try
        {
            return await GetSolarSystem($"{BodiesApiEndpoint}?filter[]=name,cs,{query}");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error fetching bodies with query from API.");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            return null;
        }
    }

    private async Task<IQueryable<CelestialBody>?> GetSolarSystem(string requestUri)
    {
        HttpResponseMessage response = await _client.GetAsync(requestUri).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        SolarSystem? solarSystem = await response.Content.ReadFromJsonAsync<SolarSystem>().ConfigureAwait(false);
        return solarSystem?.Bodies?.AsQueryable();
    }

    public async Task<CelestialBody?> GetCelestialBody(string id)
    {
        try
        {
            //return await memoryCache.GetOrCreateAsync(id, async cache =>
            //{
            HttpResponseMessage response = await _client.GetAsync($"{BodiesApiEndpoint}/{id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            CelestialBody? celestialBody = await response.Content.ReadFromJsonAsync<CelestialBody>().ConfigureAwait(false);

            if (celestialBody == null)
                return null;

            await LoadMoons(celestialBody);
            await LoadAroundPlanet(celestialBody);

            return celestialBody;
            //});
        }
        catch (Exception)
        {
            // Log the exception (logging mechanism not shown here)
            return null;
        }
    }

    private async Task LoadMoons(CelestialBody spaceEntity)
    {
        if (spaceEntity?.Moons?.Length > 0)
        {
            foreach ((int index, Moon m) in spaceEntity.Moons.Index()) 
            {
                HttpResponseMessage response = await _client.GetAsync(m.rel).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                Moon? moon = await response.Content.ReadFromJsonAsync<Moon>().ConfigureAwait(false);

                if (moon != null)
                {
                    spaceEntity.Moons[index] = moon;
                }
            }
        }
    }

    private async Task LoadAroundPlanet(CelestialBody spaceEntity)
    {
        if (spaceEntity?.AroundPlanet != null)
        {
            HttpResponseMessage response = await _client.GetAsync(spaceEntity.AroundPlanet.rel).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            spaceEntity.AroundPlanet = await response.Content.ReadFromJsonAsync<Planet>().ConfigureAwait(false);
        }
    }
}