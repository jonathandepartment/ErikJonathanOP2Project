using Fora.Server.Services.UserService;
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

        [HttpPost]
        public async Task<ActionResult> CreateUser(SignUpModel request)
        {
            // skapa användare
            var createdUser = await _userService.AddUser(request);

            if (createdUser != null)
            {
                return Created("", request);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LogIn(UserDTO request)
        {
            // sign in user
            var token = await _userService.LoginUser(request);
            // return token
            if (token != null)
            {
                return Ok(token);
            }
            // return unauthorized
            return Unauthorized();
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
        public async Task MakeAdmin()
        {

        }
    }
}
