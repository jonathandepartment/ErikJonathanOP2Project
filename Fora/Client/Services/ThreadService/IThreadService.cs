﻿using Fora.Shared.DTO;
using Fora.Shared.ViewModels;

namespace Fora.Client.Services.ThreadService
{
    public interface IThreadService
    {
        Task<ThreadViewModel> AddThread(AddThreadModel thread);
        Task<List<ThreadViewModel>> GetThreadsByInterest(int id);
    }
}
