using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fora.Server.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public UserService(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<ServiceResponseModel<string>> ToggleBanUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
            if (user != null)
            {
                user.Banned = !user.Banned;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return new ServiceResponseModel<string>
                {
                    Data = user.Banned ? "Ban" : "Unban",
                    success = true,
                    message = $"{user.Username}"
                };
            }
            return new ServiceResponseModel<string>
            {
                Data = null,
                success = false,
                message = "User not found"
            };
        }

        public async Task<ServiceResponseModel<string>> FlagUserRemoved(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
            if (user != null)
            {
                // check if correct user
                var currentUser = _accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

                if (currentUser == user.Username)
                {
                    user.Deleted = !user.Deleted;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return new ServiceResponseModel<string>
                    {
                        Data = user.Deleted ? "Removed" : "Revived",
                        success = true,
                        message = $"{user.Username}"
                    };
                }
                else
                {
                    return new ServiceResponseModel<string>
                    {
                        Data = null,
                        success = false,
                        message = "Invalid user credentials"
                    };
                }
            }
            return new ServiceResponseModel<string>
            {
                Data = null,
                success = false,
                message = "User not found"
            };
        }

        public async Task<UserModel> GetUser(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            return user;
        }

    }
}
