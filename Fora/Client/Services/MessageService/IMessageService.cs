using Fora.Shared.ViewModels;

namespace Fora.Client.Services.MessageService
{
    public interface IMessageService
    {
        Task<List<MessageViewModel>> GetThreadMessages(int id);
    }
}
