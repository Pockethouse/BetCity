namespace BetCity.APIs
{
    public class BetCityApiService
    {
        private readonly HttpClient _client;

        public BetCityApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> SendGetRequestAsync(string endpoint, string parameter = "")
        {
            var requestUri = string.IsNullOrEmpty(parameter) ? endpoint : $"{endpoint}/{parameter}";
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUri));

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> SendGetRequestWithQueryAsync(string endpoint, string queryParameter)
        {
            var requestUri = $"{endpoint}/{queryParameter}";
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUri));

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}