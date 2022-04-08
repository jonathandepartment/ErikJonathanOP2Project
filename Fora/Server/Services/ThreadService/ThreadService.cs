namespace Fora.Server.Services.ThreadService
{
    public class ThreadService : IThreadService
    {
        private readonly AppDbContext _context;

        public ThreadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponseModel<ThreadModel>> AddThread(int interestId, int userId, string name)
        {
            // kolla om tråden redan finns
            var thread = await _context.Threads
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());

            if (thread == null)
            {
                ThreadModel newThread = new();
                newThread.InterestId = interestId;
                newThread.UserId = userId;
                newThread.Name = name;

                _context.Threads.Add(newThread);
                await _context.SaveChangesAsync();

                return new ServiceResponseModel<ThreadModel>
                {
                    Data = newThread,
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

        public async Task<ThreadModel> ChangeThreadName(int id, string newName)
        {
            var thread = await _context.Threads.FirstOrDefaultAsync(t => t.Id == id);
            if (thread != null)
            {
                thread.Name = newName;
                _context.Threads.Update(thread);
                await _context.SaveChangesAsync();
                return thread;
            }
            return null;
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

        public async Task<List<ThreadModel>> GetThreads(int id)
        {
            var threads = await _context.Threads
                .Where(t => t.InterestId == id)
                .ToListAsync();
            if (threads != null)
            {
                return threads;
            }
            return null;
        }
    }
}
