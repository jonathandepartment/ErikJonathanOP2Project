using Fora.Shared.ViewModels;

namespace Fora.Client.Services.ThreadService
{
    public interface IThreadService
    {
        Task<List<ThreadViewModel>> GetThreadsByInterest(int id);
    }
}
