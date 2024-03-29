﻿using Fora.Shared.ViewModels;
using System.Security.Claims;

namespace Fora.Server.Services.InterestService
{
    public class InterestService : IInterestService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public InterestService(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<InterestModel> GetInterest(int id)
        {
            var interest = await _context.Interests
                .FirstOrDefaultAsync(i => i.Id == id);

            if (interest != null)
            {
                return interest;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<InterestViewModel>> GetInterests()
        {
            // Kontakta databasen och hämta alla intressen

            var interests = await _context.Interests
                .Include(i => i.Threads)
                .ToListAsync();

            if (interests != null)
            {
                List<InterestViewModel> vmList = new();

                foreach (var interest in interests)
                {
                    vmList.Add(new InterestViewModel
                    {
                        Id = interest.Id,
                        AuthorId = interest.UserId,
                        Name = interest.Name,
                        ThreadCount = interest.Threads.Count
                    });
                }

                return vmList;
            }
            return null;
        }

        public async Task<List<InterestViewModel>> GetUserInterests()
        {
            var currentUsername = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);
            // Kontakta databasen och hämta alla intressen som en specifik användare har
            var userinterests = await _context.UserInterests
                .Include(u => u.Interest)
                .Include(u => u.Interest.Threads)
                .Where(u => u.UserId == userInDb.Id).ToListAsync();

            if (userinterests != null)
            {
                List<InterestViewModel> userVmInterests = new();

                foreach (var ui in userinterests)
                {
                    userVmInterests.Add(new InterestViewModel
                    {
                        Id = ui.Interest.Id,
                        AuthorId = ui.Interest.UserId,
                        Name = ui.Interest.Name,
                        ThreadCount = ui.Interest.Threads.Count
                    });
                }
                return userVmInterests;
            }
            return null;
        }

        public async Task<InterestViewModel> PostNewInterest(AddInterestModel interest)
        {
            // get current user from request
            var currentUsername = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

            // get matching user from db
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

            // check if interest allready exists
            var interestInDb = await _context.Interests
                .FirstOrDefaultAsync(i => i.Name.ToLower() == interest.Name.ToLower());

            if (interestInDb == null)
            {
                InterestModel interestToAdd = new InterestModel();
                interestToAdd.Name = interest.Name;
                interestToAdd.UserId = userInDb.Id;

                _context.Interests.Add(interestToAdd);
                await _context.SaveChangesAsync();

                return new InterestViewModel
                {
                    Id = interestToAdd.Id,
                    Name = interestToAdd.Name,
                    AuthorId = interestToAdd.Id
                };
            }
            return null;
        }


        public async Task<bool> ChangeInterestName(int Id, UpdateInterestModel interest)
        {
            var interestToUpdate = await _context.Interests
                .Include(i => i.Threads)
                .FirstOrDefaultAsync(i => i.Id == Id);

            if (interestToUpdate != null)
            {
                if (interestToUpdate.Threads.Count < 1)
                {
                    var currentUsername = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                    var currentUserRole = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

                    var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == interestToUpdate.UserId);

                    if (currentUserRole == "Admin" || currentUsername == userInDb.Username)
                    {
                        interestToUpdate.Name = interest.Name;
                        _context.Interests.Update(interestToUpdate);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeleteInterest(int id)
        {
            var interestToDelete = await _context.Interests.FirstOrDefaultAsync(i => i.Id == id);
            if (interestToDelete != null)
            {
                var currentUsername = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                var currentUserRole = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

                var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

                if (currentUserRole == "Admin" || interestToDelete.UserId == userInDb.Id)
                {
                    _context.Interests.Remove(interestToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;


        }

        public async Task<bool> AddUserInterest(int id)
        {
            var currentUsername = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

            // check if interest is allready added
            //var userInterests = await _context.UserInterests.FirstOrDefaultAsync(u => u.InterestId == id);
            var userInterests = await _context.UserInterests
                .Where(u => u.UserId == userInDb.Id && u.InterestId == id)
                .FirstOrDefaultAsync();

            if (userInterests == null)
            {
                UserInterestModel userInterest = new UserInterestModel();
                userInterest.InterestId = id;
                userInterest.UserId = userInDb.Id;
                _context.UserInterests.Add(userInterest);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AddUserInterests(List<int> interestsToAdd)
        {
            // hämta användare
            var currentUsername = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

            if (interestsToAdd.Count == 0)
            {
                // if choices is blank, add standard interests
                var standardInterests = await _context.Interests
                    .Select(i => i.Id)
                    .Where(i  => i < 5)
                    .ToListAsync();

                
                interestsToAdd.AddRange(standardInterests);
            }
            // hämta intressen med matchande ids
            var interests = await _context.Interests.Where(i => interestsToAdd.Contains(i.Id))
                .ToListAsync();

            if (interests != null)
            {
                List<UserInterestModel> userInterests = new();

                foreach (var i in interests)
                {
                    if (i != null)
                    {
                        userInterests.Add(new UserInterestModel
                        {
                            UserId = userInDb.Id,
                            InterestId = i.Id
                        });
                    }
                }

                if (userInterests.Count > 0)
                {
                    _context.UserInterests.AddRange(userInterests);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            // lägg intressen till userinterests
            return false;
        }

        public async Task<bool> DeleteUserInterest(int id)
        {
            var currentUsername = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var currentUserInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

            var userInterest = await _context.UserInterests
                .FirstOrDefaultAsync(ui => ui.InterestId == id && ui.UserId == currentUserInDb.Id);

            if (userInterest != null)
            {
                _context.UserInterests.Remove(userInterest);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
