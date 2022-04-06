namespace Fora.Server.Services.MessageService
{
    public interface IMessageService
    {
        Task<ServiceResponseModel<string>> EditMessage(int id,string message);
        Task<bool> DeleteMessage(int id);
    }
}
