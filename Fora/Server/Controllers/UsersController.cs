using Fora.Server.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{username}")]
        public async Task<ActionResult> GetUser(string username)
        {
            var user = await _userService.GetUser(username);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut]
        [Route("[action]/{username}")]
        public async Task<ActionResult> ToggleUserBan(string username)
        {
            var banResult = await _userService.ToggleBanUser(username);
            if (banResult.success)
            {
                return Ok(banResult);
            }
            return BadRequest(banResult.message);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("[action]/{username}")]
        public async Task<ActionResult> ToggleRemoveFlag(string username)
        {
            var flagResult = await _userService.FlagUserRemoved(username);
            if (flagResult.success)
            {
                return Ok(flagResult);
            }
            return BadRequest(flagResult.message);
        }
    }
}
