using Fora.Shared.ViewModels;
using System.Net.Http.Json;

namespace Fora.Client.Services.InterestService
{
    public class InterestService : IInterestService
    {
        private readonly HttpClient _httpClient;

        public InterestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<InterestViewModel>> GetAllInterests()
        {
            var response = await _httpClient.GetFromJsonAsync<List<InterestViewModel>>("api/interests");

            return response;
        }
    }
}
