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
            //_context.Interests.Remove()
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

        public async Task PostNewInterest(AddInterestModel interest)
        {
            {
                InterestModel interestToAdd = new InterestModel();
                interestToAdd.Name = interest.Name;
                var newInterest = _context.Interests.Add(interestToAdd);
                await _context.SaveChangesAsync();

            }
        }


        public Task<List<InterestModel>> PutUserInterests(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
