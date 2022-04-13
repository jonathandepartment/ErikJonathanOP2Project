using Fora.Shared.ViewModels;

namespace Fora.Server.Services.ThreadService
{
    public interface IThreadService
    {
        Task<List<ThreadViewModel>> GetThreads(int id);
        Task<List<ThreadViewModel>> GetMyThreads();
        Task<ServiceResponseModel<ThreadViewModel>> ChangeThreadName(int id, string name);
        Task<ServiceResponseModel<ThreadModel>> DeleteThread(int id);
        Task<ServiceResponseModel<ThreadModel>> AddThread(int interestId, string name);
    }
}
