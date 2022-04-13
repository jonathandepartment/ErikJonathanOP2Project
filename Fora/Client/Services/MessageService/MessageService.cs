using Fora.Shared.ViewModels;
using System.Net.Http.Json;

namespace Fora.Client.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<MessageViewModel>> GetThreadMessages(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<List<MessageViewModel>>($"/api/messages/{id}");
            return result;
        }
    }
}
