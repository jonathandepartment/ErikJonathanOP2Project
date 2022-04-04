using Microsoft.EntityFrameworkCore;

namespace Fora.Server.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public UserService(SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
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

        public Task ChangePassword(string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task LoginUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task MakeAdmin(string id)
        {
            throw new NotImplementedException();
        }
    }
}
