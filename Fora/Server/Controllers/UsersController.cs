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

        [HttpDelete]
        public async Task RemoveUser()
        {

        }

        [HttpPut]
        public async Task ChangePassword()
        {

        }

        [HttpPut]
        public async Task MakeAdmin()
        {

        }
    }
}
