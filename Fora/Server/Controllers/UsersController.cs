using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fora.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(UserModel user)
        {
            
            return Created("", user);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn()
        {
            // sign in user
            // return token
            // return unauthorized
            return Unauthorized();
        }
    }
}
