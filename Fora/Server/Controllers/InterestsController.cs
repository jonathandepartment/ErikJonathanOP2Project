using Fora.Server.Services.InterestService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult> GetAllInterests()
        {
            // Call service
            var result = await _interestService.GetInterests();
            // if success return list of all interests
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetInterest(int id)
        {
            var result = await _interestService.GetInterest(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult> GetUserInterests()
        {
            // Call service
            var result = await _interestService.GetUserInterests();
            // If success return list of the users interests
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult> PostNewInterest(AddInterestModel interest)
        {
            string interestName = interest.Name;
            if (!string.IsNullOrEmpty(interestName))
            {
                var result = await _interestService.PostNewInterest(interest);
                if (result)
                {
                    return Ok("Interest added!");
                }
                return BadRequest("Failed to add interest. Duplicate found");
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult> AddUserInterest(int id)
        {
            var result = await _interestService.AddUserInterest(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> AddUserInterests(List<int> interestsToAdd)
        {
            if (interestsToAdd != null || interestsToAdd.Count > 0)
            {
                var result = await _interestService.AddUserInterests(interestsToAdd);
                if (result)
                {
                    return Ok();
                }
                return BadRequest("");
            }
            return BadRequest();
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<InterestModel>> DeleteInterest(int id)
        {
            // kolla om rätt användare eller admin
            await _interestService.DeleteInterest(id);
            return Ok("Interest removed");
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<InterestModel>> PutUserInterests(int Id, UpdateInterestModel interest)
        {
            // kolla om rätt användare eller admin
            // ändra om trådantalet är 0
            var putResult = await _interestService.PutUserInterests(Id, interest);
            if (putResult)
            {
                return Ok("Interest updated");
            }
            else
            { return BadRequest(); }
        }
    }
}

