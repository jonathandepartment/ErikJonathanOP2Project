using Fora.Server.Services.ThreadService;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<ActionResult> PostThread(AddThreadModel addThread)
        {
            // !!! 6 is placeholder
            // get user id from token, set to thread userid
            var result = await _threadService.AddThread(addThread.InterestId, 6, addThread.Name);
            if (result.success)
            {
                return Created("/api/threads", result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ChangeThreadName(int id,[FromBody] string newThreadName)
        {
            if (string.IsNullOrEmpty(newThreadName))
            {
                return BadRequest("Thread name can't be empty");
            }

            // kolla om det är rätt författare eller admin
            // om rätt användare, ändra om antalet meddelanden är 0
            var result = await _threadService.ChangeThreadName(id, newThreadName);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteThread(int id)
        {
            // Ta bort om en är författaren eller admin

            var result = await _threadService.DeleteThread(id);
            if (result.success)
            {
                return NoContent();
            }
            return BadRequest(result);
        }
    }
}
