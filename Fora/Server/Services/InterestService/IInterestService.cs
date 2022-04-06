namespace Fora.Server.Services.InterestService
{
    public interface IInterestService
    {
        Task<List<InterestModel>> GetInterests();
        Task<List<UserInterestModel>> GetUserInterests(int id);
        Task<List<InterestModel>> PostNewInterest();
        Task<List<InterestModel>> DeleteInterest(int id);
        Task<List<InterestModel>> PutUserInterests(int Id);
    }
}