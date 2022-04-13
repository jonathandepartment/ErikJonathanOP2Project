﻿using Fora.Shared.ViewModels;
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
        public Task AddNewInterest()
        {
            throw new NotImplementedException();
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

        public Task<List<InterestViewModel>> GetAllInterests()
        {
            throw new NotImplementedException();
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

        public Task RemoveInterest()
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserInterest()
        {
            throw new NotImplementedException();
        }

        public Task UnBanUser()
        {
            throw new NotImplementedException();
        }
    }
}
