using Fora.Server.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            // call service
            var result = await _userService.GetUsers();
            // if successful return list + ok
            if (result != null)
            {
                var userList = result
                    .Select(u => new { u.Id, u.UserName })
                    .ToList();
                return Ok(userList);
            }
            // else return badrequest
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> CheckIfAdmin(string id)
        {
            var result = await _userService.CheckIfAdmin(id);
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
            var createdUser = await _userService.AddUser(request);

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
            var result = await _userService.LoginUser(request);
            // return token
            if (result.success)
            {
                return Ok(result.Data);
            }
            // return unauthorized
            return Unauthorized(result.message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveUser([FromRoute] string id)
        {
            var removeResult = await _userService.DeleteUser(id);
            if (removeResult)
            {
                return NoContent();
            }
            return BadRequest("No matching user was found");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ChangePassword([FromRoute] string id, ChangePasswordModel model)
        {
            var passwordChangeResult = await _userService.ChangePassword(id, model.OldPassword, model.NewPassword);
            if (passwordChangeResult)
            {
                return Ok("Password was changed");
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<ActionResult> PromoteAdmin(string id)
        {
            var makeAdminResult = await _userService.MakeAdmin(id);
            if (makeAdminResult)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DemoteAdmin(string id)
        {
            var makeAdminResult = await _userService.RemoveAdmin(id);
            if (makeAdminResult)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
