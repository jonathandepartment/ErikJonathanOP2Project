using Fora.Shared.DTO;
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

        public async Task AddInitialInterests(AddInitialInterests initialInterests)
        {
            var result = await _httpClient.PostAsJsonAsync<AddInitialInterests>("api/interests/AddUserInterests", initialInterests);
        }

        public async Task<bool> DeleteUserInterest(int id)
        {
            //var result = await _httpClient.DeleteAsync($"api/interests/deleteuserinterest/{id}");
            var userInterest = await _httpClient.DeleteAsync($"api/interests/deleteuserinterest/{id}");
            return userInterest != null;


        }

        public async Task<List<InterestViewModel>> GetAllInterests()
        {
            var response = await _httpClient.GetFromJsonAsync<List<InterestViewModel>>("api/interests");

            return response;
        }

        public async Task<List<InterestViewModel>> GetMyInterests()
        {
            var response = await _httpClient.GetFromJsonAsync<List<InterestViewModel>>("/api/interests/getuserinterests/");

            if (response != null)
            {
                return response;
            }
            return new List<InterestViewModel>();
        }
    }
}
