using Datas;
using Domains;
using Features.UserFeatures.Requests.Register;
using Microsoft.EntityFrameworkCore;

namespace Features.UserFeatures.Repository
{
    public class UserRepository(Context context) : BaseRepository<User>(context), IUserRepository
    {
        private readonly Context _context = context;

        public async Task<bool> CheckEmailExist(RegisterRequest req)
        {
            var result = await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == req.Email)
                .FirstOrDefaultAsync();

            return result != null;
        }
        
        public async Task<bool> CheckUsernameExist(RegisterRequest req)
        {
            var result = await _context.Users
                .AsNoTracking()
                .Where(u => u.Username == req.Username)
                .FirstOrDefaultAsync();

            return result != null;
        }
    }
}
