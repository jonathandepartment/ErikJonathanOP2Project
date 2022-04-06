namespace Fora.Server.Services.UserService
{
    public interface IUserService
    {
        Task<UserModel> GetUser(string username);
        Task<ServiceResponseModel<string>> ToggleBanUser(string username);
        Task<ServiceResponseModel<string>> FlagUserRemoved(string username);

    }
}
