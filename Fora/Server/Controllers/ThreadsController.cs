using Fora.Server.Services.ThreadService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly IThreadService _threadService;

        public ThreadsController(IThreadService threadService)
        {
            _threadService = threadService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetThreadsByInterest(int id)
        {
            var result = await _threadService.GetThreads(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("No matching interest id");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> PostThread(AddThreadModel addThread)
        {

            var result = await _threadService.AddThread(addThread.InterestId, addThread.Name);
            if (result.success)
            {
                return Created("/api/threads", result);
            }
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<ActionResult> ChangeThreadName(int id, [FromBody] string newThreadName)
        {
            if (string.IsNullOrEmpty(newThreadName))
            {
                return BadRequest("Thread name can't be empty");
            }

            var result = await _threadService.ChangeThreadName(id, newThreadName);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteThread(int id)
        {
            var result = await _threadService.DeleteThread(id);
            if (result.success)
            {
                return NoContent();
            }
            return BadRequest(result);
        }
    }
}
