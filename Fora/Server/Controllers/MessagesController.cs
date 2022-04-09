using Fora.Server.Services.MessageService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetThreadMessages(int id)
        {
            var result = await _messageService.GetThreadMessages(id);
            if (result.success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddMessage()
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ChangeMessage(int id, [FromBody] string value)
        {
            
            // get user from request
            //var userName = User.Identity.Name;
            var result = await _messageService.EditMessage(id, value);
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
