using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fora.Server.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(SignInManager<ApplicationUser> signInManager, AppDbContext context, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }
        public async Task<ApplicationUser> AddUser(SignUpModel user)
        {
            ApplicationUser newUser = new ApplicationUser();
            newUser.UserName = user.Username;
            newUser.Email = user.Email;

            var result = await _signInManager.UserManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                UserModel userModelToDb = new();
                userModelToDb.Username = newUser.UserName;
                _context.Users.Add(userModelToDb);
                await _context.SaveChangesAsync();

                return newUser;
            }
            return null;
        }

        public async Task<bool> ChangePassword(string id, string oldPassword, string newPassword)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var changePasswordResult = await _signInManager.UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (changePasswordResult.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var removeResult = await _signInManager.UserManager.DeleteAsync(user);
                if (removeResult.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public Task GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginUser(UserDTO user)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
            if (signInResult.Succeeded)
            {
                var confirmedUser = await _signInManager.UserManager.FindByNameAsync(user.Username);

                // Check if user is admin
                var adminCheck = await _signInManager.UserManager.IsInRoleAsync(confirmedUser, "admin");

                // return token with claims

                if (adminCheck)
                {
                    return CreateToken(confirmedUser, true);
                }
                return CreateToken(confirmedUser);
            }
            return null;
        }

        public Task MakeAdmin(string id)
        {
            throw new NotImplementedException();
        }

        private string CreateToken(ApplicationUser user, bool admin = false)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            if (admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
