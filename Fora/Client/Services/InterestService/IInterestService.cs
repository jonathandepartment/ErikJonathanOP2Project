using Fora.Shared.ViewModels;

namespace Fora.Client.Services.InterestService
{
    public interface IInterestService
    {
        Task<List<InterestViewModel>> GetAllInterests();
    }
}
