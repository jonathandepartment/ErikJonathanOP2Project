using Fora.Server.Services.AccountService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            // call service
            var result = await _accountService.GetUsers();
            // if successful return list + ok
            if (result != null)
            {                
                return Ok(result);
            }
            // else return badrequest
            return BadRequest();
        }
        // ska tas bort?
        [HttpGet("{id}")]
        public async Task<ActionResult> CheckIfAdmin(string id)
        {
            var result = await _accountService.CheckIfAdmin(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(SignUpModel request)
        {
            // skapa användare
            var createdUser = await _accountService.AddUser(request);

            if (createdUser.success)
            {
                return Created("", createdUser.message);
            }
            return BadRequest(createdUser.message);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LogIn(UserDTO request)
        {
            // sign in user
            var result = await _accountService.LoginUser(request);
            // return token
            if (result.success)
            {
                return Ok(result.Data);
            }
            // return unauthorized
            return Unauthorized(result.message);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveUser([FromRoute] string id)
        {
        
            var removeResult = await _accountService.DeleteUser(id);
            if (removeResult)
            {
                return NoContent();
            }
            return BadRequest("No matching user was found");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            var passwordChangeResult = await _accountService.ChangePassword(model.OldPassword, model.NewPassword);
            if (passwordChangeResult)
            {
                return Ok("Password was changed");
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut]
        [Route("[action]/{username}")]
        public async Task<ActionResult> PromoteAdmin(string username)
        {
            var makeAdminResult = await _accountService.MakeAdmin(username);
            if (makeAdminResult)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut]
        [Route("[action]/{username}")]
        public async Task<ActionResult> DemoteAdmin(string username)
        {
            var makeAdminResult = await _accountService.RemoveAdmin(username);
            if (makeAdminResult)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
