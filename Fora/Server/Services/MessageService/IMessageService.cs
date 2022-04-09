using Fora.Shared.ViewModels;

namespace Fora.Server.Services.MessageService
{
    public interface IMessageService
    {
        Task<ServiceResponseModel<List<MessageViewModel>>> GetThreadMessages(int threadId);
        Task<ServiceResponseModel<string>> EditMessage(int id,string message);
        Task<bool> DeleteMessage(int id);
        Task<ServiceResponseModel<MessageModel>> AddMessage(int userId, int threadId, string message);
    }
}
