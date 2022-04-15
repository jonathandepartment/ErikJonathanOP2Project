using Fora.Shared;
using Fora.Shared.DTO;
using Fora.Shared.ViewModels;

namespace Fora.Client.Services.InterestService
{
    public interface IInterestService
    {
        Task<List<InterestViewModel>> GetAllInterests();
        Task AddInitialInterests(AddInitialInterests initialInterests);
        Task<List<InterestViewModel>> GetMyInterests();
        Task<bool> DeleteUserInterest(int id);
        Task<bool> ChangeInterestName(int Id, UpdateInterestModel interest);

    }
}
