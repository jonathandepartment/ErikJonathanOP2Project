using Fora.Shared.ViewModels;

namespace Fora.Server.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;

        public MessageService(AppDbContext context)
        {
            _context = context;
        }

        public Task<ServiceResponseModel<MessageModel>> AddMessage(int userId, int threadId, string message)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteMessage(int id)
        {
            // if correct user or admin

            // remove message
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message != null)
            {
                var removeResult = _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ServiceResponseModel<string>> EditMessage(int id, string message)
        {
            // if correct user

            //edit message
            var messageToEdit = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (messageToEdit != null)
            {
                messageToEdit.Message = message;
                _context.Messages.Update(messageToEdit);
                await _context.SaveChangesAsync();

                return new ServiceResponseModel<string>
                {
                    Data = messageToEdit.Message,
                    success = true,
                    message = $"Message, id: {messageToEdit.Id} edited"
                };
            }
            return new ServiceResponseModel<string>
            {
                Data = null,
                success = false,
                message = "Incorrect user or message could not be found"
            };
        }

        public async Task<ServiceResponseModel<List<MessageViewModel>>> GetThreadMessages(int threadId)
        {
            var thread = await _context.Threads.FirstOrDefaultAsync(t => t.Id == threadId);
            if (thread != null)
            {
                var messages = await _context.Messages
                    .Include(m => m.User)
                    .Where(m => m.ThreadId == threadId)
                    .ToListAsync();

                
                if (messages != null || messages.Count > 0)
                {
                    // convert list to new model
                    var messagesVmList = ConvertToViewModels(messages);
                    return new ServiceResponseModel<List<MessageViewModel>>
                    {
                        Data =messagesVmList,
                        message = "",
                        success = true,
                    };
                }

                return new ServiceResponseModel<List<MessageViewModel>>
                {
                    Data = null,
                    message = "No messages in thread",
                    success = false
                };
            }

            return new ServiceResponseModel<List<MessageViewModel>>
            {
                Data = null,
                message = "No thread with that id",
                success = false
            };
        }

        private List<MessageViewModel> ConvertToViewModels(List<MessageModel> messages)
        {
            List<MessageViewModel> convertedMessages = new();

            foreach (var message in messages)
            {
                convertedMessages.Add(new MessageViewModel
                {
                    Id = message.Id,
                    Message = message.Message,
                    User = new UserViewModel
                    {
                        Id = message.User.Id,
                        Name = message.User.Username,
                        Banned = message.User.Banned,
                        Deleted = message.User.Deleted
                    }
                });
            }

            return convertedMessages;
        }
    }
}
