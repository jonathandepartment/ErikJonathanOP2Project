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
        public async Task<ActionResult> CreateUser(SignUpModel user)
        {
            // skapa användare
            var createdUser = await _userService.AddUser(user);

            if (createdUser != null)
            {
                return Created("", user);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LogIn()
        {
            // sign in user
            // return token
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
