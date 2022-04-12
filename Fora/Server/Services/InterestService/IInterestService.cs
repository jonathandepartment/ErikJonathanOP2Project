namespace Fora.Server.Services.InterestService
{
    public interface IInterestService
    {
        Task<List<InterestModel>> GetInterests();
        Task<List<UserInterestModel>> GetUserInterests(int id);
        Task<bool> PostNewInterest(AddInterestModel interest);
        Task<bool> DeleteInterest(int id);
        Task<bool> PutUserInterests(int Id, UpdateInterestModel interest);
        Task<bool> AddUserInterest(int id);
    }
}