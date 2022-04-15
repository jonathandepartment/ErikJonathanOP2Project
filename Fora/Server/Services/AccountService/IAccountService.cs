using Fora.Shared.ViewModels;

namespace Fora.Server.Services.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResponseModel<ApplicationUser>> AddUser(SignUpModel user);
        Task<ServiceResponseModel<string>> LoginUser(UserDTO user);
        Task<List<UserViewModel>> GetUsers();
        Task<bool> DeleteUser(string id);
        Task<bool> CheckIfAdmin(string id);
        Task<bool> ChangePassword(string oldPassword, string newPassword);
        Task<bool> MakeAdmin(string id);
        Task<bool> RemoveAdmin(string id);
    }
}
