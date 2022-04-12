namespace Fora.Server.Services.InterestService
{
    public class InterestService : IInterestService
    {
        private readonly AppDbContext _context;

        public InterestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<InterestModel>> GetInterests()
        {
            // Kontakta databasen och hämta alla intressen

            var interests = await _context.Interests.ToListAsync();
            if (interests != null)
            {
                return interests;
            }
            return null;
        }

        public async Task<List<UserInterestModel>> GetUserInterests(int id)
        {
            // Kontakta databasen och hämta alla intressen som en specifik användare har
            var userinterests = await _context.UserInterests.Include(u => u.Interest).Where(u => u.UserId == id).ToListAsync();

            if (userinterests != null)
            {
                return userinterests;
            }
            return null;



            // *********************************************
            // ERROR!! A possible object cycle was detected.
            // *********************************************

            //var users = await _context.Users.Include(i => i.UserInterests).ToListAsync();
            //var user = users.FirstOrDefault(u => u.Id == id);
            //if (user != null)
            //{
            //    return user.UserInterests;

            //}
            //return null;
        }

        public async Task<bool> PostNewInterest(AddInterestModel interest)
        {
            {
                InterestModel interestToAdd = new InterestModel();
                interestToAdd.Name = interest.Name;

                //*************************************************************
                //Hämta användare
                //Sätt interestToAdd = var user.Id
                //interestToAdd.UserId
                //*************************************************************
                if (interestToAdd != null)
                {
                    var newInterest = _context.Interests.Add(interestToAdd);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;

            }
        }


        public async Task<bool> PutUserInterests(int Id, UpdateInterestModel interest)
        {

            var InterestToUpdate = await _context.Interests.FirstOrDefaultAsync(i => i.Id == Id);

            if (InterestToUpdate != null)
            {
                // Kolla så att korrekt användare eller admin försöker uppdatera intresset
                InterestToUpdate.Name = interest.Name;
                _context.Interests.Update(InterestToUpdate);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteInterest(int id)
        {
            var interestToDelete = await _context.Interests.FirstOrDefaultAsync(i => i.Id == id);
            if (interestToDelete != null)
            {
                _context.Interests.Remove(interestToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;


        }

        public async Task<bool> AddUserInterest(int id)
        {
            // Temp Id för test. Hämtas sedan från token för att få rätt användare.
            var tempUserId = 1;

            UserInterestModel userInterest = new UserInterestModel();
            userInterest.InterestId = id;
            userInterest.UserId = tempUserId;
            _context.UserInterests.Add(userInterest);
            await _context.SaveChangesAsync();
            return true;
        }



        //private List<MessageViewModel> ConvertToViewModels(List<MessageModel> messages)
        //{
        //    List<MessageViewModel> convertedMessages = new();

        //    foreach (var message in messages)
        //    {
        //        convertedMessages.Add(new MessageViewModel
        //        {
        //            Id = message.Id,
        //            Message = message.Message,
        //            User = new UserViewModel
        //            {
        //                Id = message.User.Id,
        //                Name = message.User.Username,
        //                Banned = message.User.Banned,
        //                Deleted = message.User.Deleted
        //            }
        //        });
        //    }

        //    return convertedMessages;
        //}
    }
}
