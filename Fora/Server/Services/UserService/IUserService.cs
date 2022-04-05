namespace Fora.Server.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponseModel<ApplicationUser>> AddUser(SignUpModel user);
        Task<ServiceResponseModel<string>> LoginUser(UserDTO user);
        Task<bool> DeleteUser(string id);
        Task<bool> GetUser(string id);
        Task<bool> ChangePassword(string id, string oldPassword, string newPassword);
        Task<bool> MakeAdmin(string id);
        Task<bool> RemoveAdmin(string id);
    }
}
