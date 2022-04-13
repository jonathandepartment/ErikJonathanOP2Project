using Fora.Shared.ViewModels;
using System.Security.Claims;

namespace Fora.Server.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public MessageService(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<ServiceResponseModel<MessageViewModel>> AddMessage(AddMessageModel message)
        {
            ServiceResponseModel<MessageViewModel> response = new ServiceResponseModel<MessageViewModel>
            {
                Data = null,
                message = "",
                success = false
            };

            var thread = await _context.Threads.FirstOrDefaultAsync(t => t.Id == message.ThreadId);
            if (thread != null)
            {
                var currentUserName = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

                var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == currentUserName.ToLower());
                if (userInDb != null)
                {
                    MessageModel messageToAdd = new();
                    messageToAdd.UserId = userInDb.Id;
                    messageToAdd.ThreadId = message.ThreadId;
                    messageToAdd.Message = message.Message;
                    messageToAdd.Created = DateTime.Now;

                    _context.Messages.Add(messageToAdd);
                    await _context.SaveChangesAsync();

                    response.message = $"Message: {message.Message}, created";
                    response.success = true;
                    return response;
                }

                response.message = "No matching user found";
                return response;
            }

            response.message = "No matching thread";
            return response;
        }

        public async Task<ServiceResponseModel<string>> DeleteMessage(int id)
        {
            ServiceResponseModel<string> response = new ServiceResponseModel<string>
            {
                Data = null,
                message = "",
                success = false
            };

            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message != null)
            {
                var currentUserName = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
                var currentUserRole = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

                var currentUserInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUserName);
                // if correct user or admin
                if (currentUserRole == "Admin" || currentUserInDb.Id == message.UserId)
                {
                    // toggle removed
                    message.Deleted = true;
                    var removeResult = _context.Messages.Update(message);
                    await _context.SaveChangesAsync();

                    response.message = "Message removed";
                    response.success = true;
                    return response;
                }
                response.message = "Invalid User";
                return response;
            }
            response.message = "No matching id";
            return response;
        }

        public async Task<ServiceResponseModel<string>> EditMessage(int id, string message)
        {
            ServiceResponseModel<string> response = new ServiceResponseModel<string>
            {
                Data = null,
                message = "",
                success = false
            };

            var messageToEdit = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (messageToEdit != null)
            {
                
                var currentUserName = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
                var currentUserRole = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

                var messageAuthor = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUserName);
                // if correct user or admin
                if (currentUserRole == "Admin" || messageAuthor.Id == messageToEdit.UserId)
                {
                    messageToEdit.Message = message;
                    messageToEdit.Edited = true;
                    _context.Messages.Update(messageToEdit);
                    await _context.SaveChangesAsync();

                    response.Data = messageToEdit.Message;
                    response.success = true;
                    response.message = $"Message, id: {messageToEdit.Id} edited";
                    return response;
                }
                response.message = "Invalid User";
                return response;
            }
            response.message = "Message could not be found";
            return response;
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
