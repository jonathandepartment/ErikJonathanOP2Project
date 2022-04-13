using Fora.Shared.ViewModels;
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
        public async Task<List<ThreadViewModel>> GetThreadsByInterest(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ThreadViewModel>>($"/api/threads/{id}");
            return result;
        }
    }
}
