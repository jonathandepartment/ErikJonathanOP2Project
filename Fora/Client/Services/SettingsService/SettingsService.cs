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

        public async Task ChangePassword(ChangePasswordModel password)
        {
            await _httpClient.PutAsJsonAsync("/api/account", password);
        }

        public async Task DeleteUser(string id)
        {
            await _httpClient.DeleteAsync($"/api/account/{id}");
        }

        public async Task DemoteAdmin(string username)
        {
            await _httpClient.PutAsync($"/api/account/demoteadmin/{username}", null);
        }

        public async Task FlagUserAsRemoved(string username)
        {
            await _httpClient.PutAsync($"/api/users/toggleremoveflag/{username}", null);
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

        public async Task<List<ThreadViewModel>> GetMyThreads()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ThreadViewModel>>("/api/threads");
            return response;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var response = await _httpClient.GetFromJsonAsync<List<UserViewModel>>("/api/account");

            return response;
        }

        public async Task PromoteAdmin(string username)
        {
            await _httpClient.PutAsync($"/api/account/promoteadmin/{username}", null);
        }

        public async Task RemoveInterest(int id)
        {
            await _httpClient.DeleteAsync($"/api/interests/{id}");
        }

        public async Task RemoveUserInterest(int id)
        {
            await _httpClient.DeleteAsync($"/api/interests/deleteuserinterest/{id}");
        }

        public async Task ToggleBan(string username)
        {
            await _httpClient.PutAsync($"/api/users/toggleuserban/{username}", null);
        }
    }
}
