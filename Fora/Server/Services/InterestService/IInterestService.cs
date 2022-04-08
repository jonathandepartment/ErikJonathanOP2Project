namespace Fora.Server.Services.InterestService
{
    public interface IInterestService
    {
        Task<List<InterestModel>> GetInterests();
        Task<List<UserInterestModel>> GetUserInterests(int id);
        Task PostNewInterest(AddInterestModel interest);
        Task<List<InterestModel>> DeleteInterest(int id);
        Task<List<InterestModel>> PutUserInterests(int Id);
    }
}