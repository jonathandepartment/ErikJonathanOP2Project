namespace Fora.Server.Services.ThreadService
{
    public interface IThreadService
    {
        Task<List<ThreadModel>> GetThreads(int id);
        Task<ThreadModel> ChangeThreadName(int id, string name);
        Task<ServiceResponseModel<ThreadModel>> DeleteThread(int id);
        Task<ServiceResponseModel<ThreadModel>> AddThread(int interestId, int userId, string name);
    }
}
