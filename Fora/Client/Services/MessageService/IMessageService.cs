using Fora.Shared.DTO;
using Fora.Shared.ViewModels;

namespace Fora.Client.Services.MessageService
{
    public interface IMessageService
    {
        Task<ThreadViewModel> GetThread(int id);
        Task<List<MessageViewModel>> GetThreadMessages(int id);
        Task<MessageViewModel> AddMessage(AddMessageModel message);
        Task EditMessage(int id, AddMessageModel message);
        Task RemoveMessage(int id);
    }
}
