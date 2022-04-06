using Fora.Server.Services.MessageService;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> ChangeMessage(int id,string newMessage)
        {
            // get user from request
            //var userName = User.Identity.Name;
            var result = await _messageService.EditMessage(id,newMessage);
            if (result != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveMessage(int id)
        {
            // get user from request
            //var userName = User.Identity.Name;
            var result = await _messageService.DeleteMessage(id);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }

    }
}
