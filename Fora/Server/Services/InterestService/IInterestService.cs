﻿using Fora.Shared.ViewModels;

namespace Fora.Server.Services.InterestService
{
    public interface IInterestService
    {
        Task<InterestModel> GetInterest(int id);
        Task<List<InterestViewModel>> GetInterests();
        Task<List<InterestViewModel>> GetUserInterests();
        Task<InterestViewModel> PostNewInterest(AddInterestModel interest);
        Task<bool> DeleteInterest(int id);
        Task<bool> DeleteUserInterest(int id);
        Task<bool> ChangeInterestName(int Id, UpdateInterestModel interest);
        Task<bool> AddUserInterest(int id);
        Task<bool> AddUserInterests(List<int> id);
    }
}