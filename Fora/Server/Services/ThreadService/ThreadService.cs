using Fora.Shared.ViewModels;
using System.Security.Claims;

namespace Fora.Server.Services.ThreadService
{
    public class ThreadService : IThreadService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public ThreadService(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<ServiceResponseModel<ThreadModel>> AddThread(int interestId, string name)
        {
            // check if thread exists
            var thread = await _context.Threads
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());

            if (thread == null)
            {
                // get user from context
                var currentUser = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

                // get matching user from db
                var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUser);

                ThreadModel newThread = new();
                newThread.InterestId = interestId;
                newThread.UserId = userFromDb.Id;
                newThread.Name = name;

                _context.Threads.Add(newThread);
                await _context.SaveChangesAsync();

                return new ServiceResponseModel<ThreadModel>
                {
                    Data = null,
                    message = $"Thread {newThread.Name} created",
                    success = true
                };
            }

            return new ServiceResponseModel<ThreadModel>()
            {
                Data = null,
                message = $"Thread: {thread.Name} allready exists",
                success = false
            };
        }

        public async Task<ServiceResponseModel<ThreadViewModel>> ChangeThreadName(int id, string newName)
        {
            ServiceResponseModel<ThreadViewModel> response = new ServiceResponseModel<ThreadViewModel>
            {
                Data = null,
                message = "",
                success = false
            };

            var thread = await _context.Threads
                .Include(t => t.Messages)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (thread != null)
            {
                // check if correct user or admin
                var currentUsername = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
                var currentUserRole = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

                var threadAuthor = await _context.Users.FirstOrDefaultAsync(u => u.Id == thread.UserId);

                if (currentUsername == threadAuthor.Username || currentUserRole == "Admin")
                {
                    // check if there are 0 messages
                    if (thread.Messages.Count < 1)
                    {
                        thread.Name = newName;
                        _context.Threads.Update(thread);
                        await _context.SaveChangesAsync();

                        response.Data = new ThreadViewModel
                        {
                            Name = newName
                        };
                        response.success = true;
                        response.message = $"Thread name changed to {newName}";
                        return response;
                    }
                    else
                    {
                        response.message = "The thread is not empty";
                        return response;
                    }
                }
                else
                {
                    response.message = "Invalid User";
                    return response;
                }
            }
            else
            {
                response.message = "No matching thread";
                return response;
            }
        }

        public async Task<ServiceResponseModel<ThreadModel>> DeleteThread(int id)
        {
            var thread = await _context.Threads.FirstOrDefaultAsync(t => t.Id == id);

            if (thread != null)
            {
                _context.Threads.Remove(thread);
                await _context.SaveChangesAsync();

                return new ServiceResponseModel<ThreadModel>
                {
                    Data = thread,
                    message = $"{thread.Name} was deleted",
                    success = true
                };
            }

            return new ServiceResponseModel<ThreadModel>
            {
                Data = null,
                message = "Thread not found",
                success = false,
            };
        }

        public async Task<List<ThreadViewModel>> GetThreads(int id)
        {
            var threads = await _context.Threads
                .Include(t => t.User)
                .Include(t => t.Messages)
                .Where(t => t.InterestId == id)
                .ToListAsync();
            if (threads != null)
            {
                return ConvertToVmList(threads);
            }
            return null;
        }

        private List<ThreadViewModel> ConvertToVmList(List<ThreadModel> threads)
        {
            List<ThreadViewModel> threadsVmList = new();

            foreach (var thread in threads)
            {
                threadsVmList.Add(new ThreadViewModel
                {
                    Id = thread.Id,
                    Name = thread.Name,
                    MessageCount = thread.Messages.Count,
                    User = new UserViewModel
                    {
                        Id = thread.User.Id,
                        Name = thread.User.Username,
                        Banned = thread.User.Banned,
                        Deleted = thread.User.Deleted
                    }
                });
            }
            return threadsVmList;
        }
    }
}
