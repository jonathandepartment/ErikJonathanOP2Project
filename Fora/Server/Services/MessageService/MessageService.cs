namespace Fora.Server.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;

        public MessageService(AppDbContext context)
        {
            _context = context;
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
    }
}
