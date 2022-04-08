using Fora.Server.Services.InterestService;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly IInterestService _interestService;

        public InterestsController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllInterests()
        {
            // Call service
            var result = await _interestService.GetInterests();
            // if success return list of all interests
            if (result != null)
            {
                var InterestList = result.ToList();
                return Ok(InterestList);
            }
            return BadRequest();

            // else return null
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult> GetUserInterests(int id)
        {
            // Call service
            var result = await _interestService.GetUserInterests(id);
            // If success return list of the users interests
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> PostNewInterest(AddInterestModel interest)
        {
            string InterestName = interest.Name;
            //if(!string.IsNullOrEmpty(interest))
            if (!string.IsNullOrEmpty(InterestName))
            {
                await _interestService.PostNewInterest(interest);
                return Ok("Interest added!");
            }
            return BadRequest();
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteInterest(int id)
        {
            var result = await _interestService.DeleteInterest(id);
            if (result != null)
            {

            }
            return Ok();
        }
    }
}
