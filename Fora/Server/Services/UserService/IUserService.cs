﻿namespace Fora.Server.Services.UserService
{
    public interface IUserService
    {
        Task<ApplicationUser> AddUser(SignUpModel user);
        Task<string> LoginUser(UserDTO user);
        Task DeleteUser(string id);
        Task GetUser(string id);
        Task ChangePassword(string newPassword);
        Task MakeAdmin(string id);
    }
}
