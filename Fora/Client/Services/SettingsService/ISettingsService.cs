using Fora.Shared;
using Fora.Shared.ViewModels;

namespace Fora.Client.Services.SettingsService
{
    public interface ISettingsService
    {
        Task<List<InterestViewModel>> GetMyInterests();
        Task<List<InterestViewModel>> GetAllInterests();
        Task<List<ThreadViewModel>> GetMyThreads();
        Task<List<UserViewModel>> GetUsers();
        Task<InterestViewModel> AddNewInterest(AddInterestModel interestToAdd);
        Task AddNewUserInterest(int id);
        Task RemoveUserInterest(int id);
        Task RemoveInterest(int id);
        Task ChangePassword(ChangePasswordModel password);
        Task PromoteAdmin(string username);
        Task DemoteAdmin(string username);
        Task ToggleBan(string username);
        Task DeleteUser(string id);
        Task FlagUserAsRemoved(string username);
    }
}
