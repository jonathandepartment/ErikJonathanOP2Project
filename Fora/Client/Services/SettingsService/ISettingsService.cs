using Fora.Shared;
using Fora.Shared.ViewModels;

namespace Fora.Client.Services.SettingsService
{
    public interface ISettingsService
    {
        Task<List<InterestViewModel>> GetMyInterests();
        Task<List<InterestViewModel>> GetAllInterests();
        Task<List<ThreadViewModel>> GetMyThreads();
        Task<InterestViewModel> AddNewInterest(AddInterestModel interestToAdd);
        Task AddNewUserInterest(int id);
        Task RemoveUserInterest(int id);
        Task RemoveInterest(int id);
        Task ChangePassword(ChangePasswordModel password);
        Task MakeAdmin();
        Task RemoveAdmin();
        Task BanUser();
        Task UnBanUser();
        Task DeleteUser(string id);
        Task FlagUserAsRemoved(string username);
    }
}
