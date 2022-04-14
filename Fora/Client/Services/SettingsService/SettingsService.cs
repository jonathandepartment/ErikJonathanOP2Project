using Fora.Shared;
using Fora.Shared.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Fora.Client.Services.SettingsService
{
    public class SettingsService : ISettingsService
    {
        private readonly HttpClient _httpClient;

        public SettingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<InterestViewModel> AddNewInterest(AddInterestModel interest)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/interests", interest);
            var response = await result.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<InterestViewModel>(response);

            return content;
        }

        public async Task AddNewUserInterest(int id)
        {
            await _httpClient.PostAsync($"/api/interests/{id}", null);
        }

        public Task BanUser()
        {
            throw new NotImplementedException();
        }

        public Task ChangePassword()
        {
            throw new NotImplementedException();
        }

        public Task CreateNewInterest()
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser()
        {
            throw new NotImplementedException();
        }

        public Task FlagUserAsRemoved()
        {
            throw new NotImplementedException();
        }

        public async Task<List<InterestViewModel>> GetAllInterests()
        {
            var response = await _httpClient.GetFromJsonAsync<List<InterestViewModel>>("/api/interests");
            if (response != null)
            {
                return response;
            }
            return new List<InterestViewModel>();
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

        public Task<List<ThreadViewModel>> GetMyThreads()
        {
            throw new NotImplementedException();
        }

        public Task MakeAdmin()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAdmin()
        {
            throw new NotImplementedException();
        }

        public async Task RemoveInterest(int id)
        {
            await _httpClient.DeleteAsync($"/api/interests/{id}");
        }

        public async Task RemoveUserInterest(int id)
        {
            await _httpClient.DeleteAsync($"/api/interests/deleteuserinterest/{id}");
        }

        public Task UnBanUser()
        {
            throw new NotImplementedException();
        }
    }
}
