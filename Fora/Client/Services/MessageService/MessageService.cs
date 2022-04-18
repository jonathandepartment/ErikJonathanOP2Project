using Fora.Shared.DTO;
using Fora.Shared.ViewModels;
using Newtonsoft.Json;
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

        public async Task<MessageViewModel> AddMessage(AddMessageModel message)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/messages", message);
            var content = await response.Content.ReadAsStringAsync();

            var msg = JsonConvert.DeserializeObject<MessageViewModel>(content);

            return msg != null ? msg : null;
        }

        public Task EditMessage(int id, AddMessageModel message)
        {
            throw new NotImplementedException();
        }

        public async Task<ThreadViewModel> GetThread(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ThreadViewModel>($"/api/threads/getthread/{id}");

            return response;
        }

        public async Task<List<MessageViewModel>> GetThreadMessages(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<List<MessageViewModel>>($"/api/messages/{id}");
            return result;
        }

        public async Task RemoveMessage(int id)
        {
            var result = await _httpClient.DeleteAsync($"/api/messages/{id}");
        }
    }
}
