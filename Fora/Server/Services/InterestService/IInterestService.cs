﻿namespace Fora.Server.Services.InterestService
{
    public interface IInterestService
    {
        Task<List<InterestModel>> GetInterests();
        Task<List<UserInterestModel>> GetUserInterests(int id);
        Task PostNewInterest(AddInterestModel interest);
        Task DeleteInterest(int id);
        Task PutUserInterests(int Id, UpdateInterestModel interest);
    }
}