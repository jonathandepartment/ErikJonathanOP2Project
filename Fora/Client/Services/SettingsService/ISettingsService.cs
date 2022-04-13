using Fora.Shared.ViewModels;

namespace Fora.Client.Services.SettingsService
{
    public interface ISettingsService
    {
        Task<List<InterestViewModel>> GetMyInterests();
        Task<List<InterestViewModel>> GetAllInterests();
        Task<List<ThreadViewModel>> GetMyThreads();
        Task CreateNewInterest();
        Task AddNewInterest();
        Task RemoveUserInterest();
        Task RemoveInterest();
        Task ChangePassword();
        Task MakeAdmin();
        Task RemoveAdmin();
        Task BanUser();
        Task UnBanUser();
        Task DeleteUser();
        Task FlagUserAsRemoved();
    }
}
