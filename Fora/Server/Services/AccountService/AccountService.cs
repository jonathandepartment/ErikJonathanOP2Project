﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fora.Server.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(SignInManager<ApplicationUser> signInManager, AppDbContext context, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceResponseModel<ApplicationUser>> AddUser(SignUpModel user)
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

                return new ServiceResponseModel<ApplicationUser>
                {
                    Data = newUser,
                    success = true,
                    message = $"User {newUser.UserName} created"
                };
            }
            var errors = CreateErrorString(result.Errors);

            return new ServiceResponseModel<ApplicationUser>
            {
                Data = null,
                success = false,
                message = errors
            };
        }

        private string CreateErrorString(IEnumerable<IdentityError> errors)
        {
            var errorMsgs = errors.Select(e => e.Description).ToList();
            return string.Join("\n", errorMsgs);
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
                    var userModelToRemove = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.UserName);
                    _context.Users.Remove(userModelToRemove);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CheckIfAdmin(string id)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return await _signInManager.UserManager.IsInRoleAsync(user, "Admin");
            }
            return false;
        }

        public async Task<ServiceResponseModel<string>> LoginUser(UserDTO user)
        {
            ServiceResponseModel<string> response = new ServiceResponseModel<string>();

            var signInResult = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

            // Check credentials
            if (signInResult.Succeeded)
            {
                // Check if user is banned
                var userModel = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (userModel.Banned)
                {
                    response.success = false;
                    response.message = "User is banned";
                }
                else
                {
                    var confirmedUser = await _signInManager.UserManager.FindByNameAsync(user.Username);

                    // Check if user is admin
                    var adminCheck = await _signInManager.UserManager.IsInRoleAsync(confirmedUser, "admin");

                    // return token with claims

                    if (adminCheck)
                    {
                        response.Data = CreateToken(confirmedUser, true);
                    }
                    else
                    {
                        response.Data = CreateToken(confirmedUser);
                    }
                    response.success = true;
                }
            }
            else
            {
                response.success = false;
                response.message = signInResult.ToString();
            }
            return response;
        }

        public async Task<bool> MakeAdmin(string id)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _signInManager.UserManager.AddToRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
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

        public async Task<bool> RemoveAdmin(string id)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _signInManager.UserManager.RemoveFromRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<ApplicationUser>> GetUsers()
        {
            // snacka med databasen, hämta alla användare
            var users = await _signInManager.UserManager.Users.ToListAsync();
            if (users != null)
            {
                return users;
            }
            return null;
        }
    }
}
