using System.Text.Json;
using BetCity.APIs;
using BetCity.Config;
using BetCity.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed; // For distributed cache

namespace BetCity.Services;

public class SportsEventService
{
    private readonly BetCityApiService _apiService;
    private readonly ApiSetting _apiSetting;
    private readonly ILogger<SportsEventService> _logger;
    private readonly IDistributedCache _cache; // Use IDistributedCache for Redis

    public SportsEventService(BetCityApiService apiService, IOptions<ApiSetting> apiSetting, ILogger<SportsEventService> logger, IDistributedCache cache)
    {
        _apiSetting = apiSetting.Value;
        _apiService = apiService;
        _logger = logger;
        _cache = cache; // Initialize Redis distributed cache
    }

    // GetLeaguesAsync with Redis Caching
    public async Task<Dictionary<string, List<string>>> GetLeaguesAsync()
    {
        var cacheKey = "GetLeagues";
        Dictionary<string, List<string>> leagues;

        // Try to get cached data
        var cachedLeagues = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedLeagues))
        {
            // Deserialize cached data if found
            leagues = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(cachedLeagues);
        }
        else
        {
            // Cache miss - make an API call
            var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetLeagues;
            var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            leagues = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonResponse, options);

            // Serialize and store the result in Redis cache
            var serializedLeagues = JsonSerializer.Serialize(leagues);
            await _cache.SetStringAsync(cacheKey, serializedLeagues, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache expiration
            });
        }

        return leagues;
    }

    // GetSportsAsync with Redis Caching
    public async Task<List<string>> GetSportsAsync()
    {
        var cacheKey = "GetSports";
        List<string> sports;

        // Try to get cached data
        var cachedSports = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedSports))
        {
            // Deserialize cached data if found
            sports = JsonSerializer.Deserialize<List<string>>(cachedSports);
        }
        else
        {
            // Cache miss - fetch from the API
            var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetSports;
            var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            sports = JsonSerializer.Deserialize<List<string>>(jsonResponse, options);

            // Serialize and store the result in Redis cache
            var serializedSports = JsonSerializer.Serialize(sports);
            await _cache.SetStringAsync(cacheKey, serializedSports, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache expiration
            });
        }

        return sports;
    }

    // GetEventWithMarketsAsync with Redis Caching
    public async Task<EventMarket> GetEventWithMarketsAsync(string eventId)
    {
        var cacheKey = $"GetEventWithMarkets_{eventId}";
        EventMarket eventMarket;

        // Try to get cached data
        var cachedEventMarket = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedEventMarket))
        {
            // Deserialize cached data if found
            eventMarket = JsonSerializer.Deserialize<EventMarket>(cachedEventMarket);
        }
        else
        {
            // Cache miss - fetch from the API
            var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetEventWithMarkets.Replace("{eventId}", eventId);
            var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            eventMarket = JsonSerializer.Deserialize<EventMarket>(jsonResponse, options);

            // Serialize and store the result in Redis cache
            var serializedEventMarket = JsonSerializer.Serialize(eventMarket);
            await _cache.SetStringAsync(cacheKey, serializedEventMarket, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache expiration
            });
        }

        return eventMarket;
    }

    // GetSportEventsAsync with Redis Caching
    public async Task<List<SportsEvent>> GetSportEventsAsync(string sportName)
    {
        var cacheKey = $"GetSportEvents_{sportName}";
        List<SportsEvent> sportsEvents;

        // Try to get cached data
        var cachedSportsEvents = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedSportsEvents))
        {
            // Deserialize cached data if found
            sportsEvents = JsonSerializer.Deserialize<List<SportsEvent>>(cachedSportsEvents);
        }
        else
        {
            // Cache miss - fetch from the API
            var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetSportEvents.Replace("{sport}", sportName);
            var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            sportsEvents = JsonSerializer.Deserialize<List<SportsEvent>>(jsonResponse, options);

            // Serialize and store the result in Redis cache
            var serializedSportsEvents = JsonSerializer.Serialize(sportsEvents);
            await _cache.SetStringAsync(cacheKey, serializedSportsEvents, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Cache expiration
            });
        }

        return sportsEvents;
    }
}
