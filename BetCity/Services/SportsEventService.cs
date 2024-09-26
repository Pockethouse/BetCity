using System.Text.Json;
using BetCity.APIs;
using BetCity.Config;
using BetCity.Models;
using Microsoft.Extensions.Options;

namespace BetCity.Services;

public class SportsEventService
{
    private readonly BetCityApiService _apiService;
    private readonly ApiSetting _apiSetting;
    private readonly ILogger<SportsEventService> _logger;

    public SportsEventService(BetCityApiService apiService, IOptions<ApiSetting> apiSetting,ILogger<SportsEventService> logger)
    {
        _apiSetting = apiSetting.Value;
        _apiService = apiService;
        _logger = logger;
        
    }

    public async Task<Dictionary<string, List<string>>> GetLeaguesAsync()
    {
        var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetLeagues;
        var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        // Deserialize the response into a dictionary
        return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonResponse, options);
    }
    
    // Fetches a list of sport events
    public async Task<List<string>> GetSportsAsync()
    {
        var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetSports;
        var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);
    
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        // Deserialize the response into List<string> (because the API returns sport names)
        return JsonSerializer.Deserialize<List<string>>(jsonResponse, options);
    }
    
    
    public async Task<EventMarket> GetEventWithMarketsAsync(string eventId)
    {
        var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetEventWithMarkets.Replace("{eventId}", eventId);
        var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        // Deserialize the response into an EventMarket object
        return JsonSerializer.Deserialize<EventMarket>(jsonResponse, options);
    }
    

    // Fetch a single event by sport name
    public async Task<List<SportsEvent>> GetSportEventsAsync(string sportName)
    {
        // Replace the placeholder {sport} in the endpoint with the actual sport name
        var endpoint = _apiSetting.BaseUrl + _apiSetting.Endpoints.GetSportEvents.Replace("{sport}", sportName);
    
        // Call the API
        var jsonResponse = await _apiService.SendGetRequestAsync(endpoint);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        // Deserialize the response into List<SportsEvent>
        return JsonSerializer.Deserialize<List<SportsEvent>>(jsonResponse, options);
    }
}
