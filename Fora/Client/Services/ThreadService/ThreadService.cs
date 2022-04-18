using Fora.Shared.DTO;
using Fora.Shared.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Fora.Client.Services.ThreadService
{
    public class ThreadService : IThreadService
    {
        private readonly HttpClient _httpClient;

        public ThreadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ThreadViewModel> AddThread(AddThreadModel thread)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/threads", thread);

            var responseString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<ThreadViewModel>(responseString);

            return content;
        }

        public async Task DeleteThread(int id)
        {
            await _httpClient.DeleteAsync($"/api/threads/{id}");
        }

        public async Task EditThreadName(int id, string newName)
        {
            var result = await _httpClient.PutAsJsonAsync($"/api/threads/{id}", newName);
        }

        public async Task<List<ThreadViewModel>> GetThreadsByInterest(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ThreadViewModel>>($"/api/threads/{id}");
            return result;
        }
    }
}
