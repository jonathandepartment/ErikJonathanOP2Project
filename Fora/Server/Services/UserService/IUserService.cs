namespace Fora.Server.Services.UserService
{
    public interface IUserService
    {
        Task<ApplicationUser> AddUser(SignUpModel user);
        Task<string> LoginUser(UserDTO user);
        Task<bool> DeleteUser(string id);
        Task GetUser(string id);
        Task<bool> ChangePassword(string id, string oldPassword, string newPassword);
        Task MakeAdmin(string id);
    }
}
