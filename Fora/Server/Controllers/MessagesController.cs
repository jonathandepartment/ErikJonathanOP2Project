using Fora.Server.Services.MessageService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> AddMessage(AddMessageModel message)
        {
            var addResult = await _messageService.AddMessage(message);
            if (addResult.success)
            {
                return Ok(addResult.Data);
            }
            return BadRequest(addResult);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<ActionResult> ChangeMessage(int id, AddMessageModel value)
        {
            if (string.IsNullOrEmpty(value.Message))
            {
                return BadRequest("New message can't be empty");
            }
            var result = await _messageService.EditMessage(id, value.Message);
            if (result.success)
            {
                return Ok();
            }
            return BadRequest(result.message);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveMessage(int id)
        {
            var result = await _messageService.DeleteMessage(id);
            if (result.success)
            {
                return NoContent();
            }
            return BadRequest(result.message);
        }

    }
}
