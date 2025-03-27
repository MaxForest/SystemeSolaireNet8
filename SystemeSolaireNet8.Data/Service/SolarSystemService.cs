using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SolarSystemN9.Data.Entity;
using System.Net.Http.Json;

namespace SolarSystemN9.Data.Service;

public class SolarSystemService(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, ILogger<SolarSystemService> logger)
{
    private const string BodiesApiEndpoint = "https://api.le-systeme-solaire.net/rest/bodies";
    private readonly HttpClient _client = httpClientFactory.CreateClient();

    public async Task<IQueryable<CelestialBody>?> GetCelestialBodiesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await memoryCache.GetOrCreateAsync("SolarSystem", async cache =>
            {
                return await GetSolarSystem(BodiesApiEndpoint, cancellationToken);
            }).ConfigureAwait(false);
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

    public async Task<IQueryable<CelestialBody>?> GetCelestialBodiesAsync(string query, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrEmpty(query))
        {
            return await GetCelestialBodiesAsync(cancellationToken);
        }

        return await GetSolarSystem($"{BodiesApiEndpoint}?filter[]=name,cs,{query}", cancellationToken);
    }

    private async Task<IQueryable<CelestialBody>?> GetSolarSystem(string requestUri, CancellationToken cancellationToken = default)
    {
        try
        {
            SolarSystem? solarSystem = await GetAsync<SolarSystem>(requestUri, cancellationToken);

            return solarSystem?.Bodies?.AsQueryable();
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

    public async Task<CelestialBody?> GetCelestialBody(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await memoryCache.GetOrCreateAsync(id, async cache =>
            {
                CelestialBody? celestialBody = await GetAsync<CelestialBody>($"{BodiesApiEndpoint}/{id}");

                if (celestialBody == null)
                    return null;

                await LoadMoons(celestialBody, cancellationToken);
                await LoadAroundPlanet(celestialBody, cancellationToken);

                return celestialBody;
            }).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error occurred.");
            return null;
        }
    }

    private async Task<T?> GetAsync<T>(string? uri, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error occurred.");
            return null;
        }
    }

    private async Task LoadMoons(CelestialBody spaceEntity, CancellationToken cancellationToken = default)
    {
        if (spaceEntity?.Moons?.Length > 0)
        {
            IEnumerable<Task> moonTasks = spaceEntity.Moons.Select(async (m, index) =>
            {
                Moon? moon = await GetAsync<Moon>(m.rel, cancellationToken).ConfigureAwait(false);
                if (moon != null)
                {
                    spaceEntity.Moons[index] = moon;
                }
            });

            await Task.WhenAll(moonTasks).ConfigureAwait(false);
        }
    }

    private async Task LoadAroundPlanet(CelestialBody spaceEntity, CancellationToken cancellationToken = default)
    {
        if (spaceEntity?.AroundPlanet != null)
        {
            spaceEntity.AroundPlanet = await GetAsync<Planet>(spaceEntity.AroundPlanet.rel, cancellationToken);
        }
    }
}