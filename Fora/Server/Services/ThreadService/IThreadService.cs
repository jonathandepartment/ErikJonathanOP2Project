﻿using Fora.Shared.ViewModels;

namespace Fora.Server.Services.ThreadService
{
    public interface IThreadService
    {
        Task<ThreadViewModel> GetThread(int id);
        Task<List<ThreadViewModel>> GetThreads(int id);
        Task<List<ThreadViewModel>> GetMyThreads();
        Task<ServiceResponseModel<ThreadViewModel>> ChangeThreadName(int id, string name);
        Task<ServiceResponseModel<ThreadModel>> DeleteThread(int id);
        Task<ServiceResponseModel<ThreadViewModel>> AddThread(int interestId, string name);
    }
}
