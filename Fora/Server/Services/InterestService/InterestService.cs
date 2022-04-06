namespace Fora.Server.Services.InterestService
{
    public class InterestService : IInterestService
    {
        private readonly AppDbContext _context;

        public InterestService(AppDbContext context)
        {
            _context = context;
        }
        public Task<List<InterestModel>> DeleteInterest(int id)
        {
            throw new NotImplementedException();
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

            // *********************************************
            // ERROR!! A possible object cycle was detected.
            // *********************************************
            var users = await _context.Users.Include(i => i.UserInterests).ToListAsync();
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return user.UserInterests;

            }
            return null;
        }

        // Kontakta databasen och hämta alla intressen som en specifik användare har
        //var userInterestsList = await _context.Users.Include(i => i.Interests).ToListAsync();
        //    if (userInterestsList != null)
        //    {
        //        return null;
        //    }
        //    return null;



        public Task<List<InterestModel>> PostNewInterest()
        {
            throw new NotImplementedException();
        }

        public Task<List<InterestModel>> PutUserInterests(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
