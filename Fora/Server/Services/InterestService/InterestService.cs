using Microsoft.EntityFrameworkCore;

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

        public Task<List<InterestModel>> GetUserIneterests(int id)
        {
            throw new NotImplementedException();
        }

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
