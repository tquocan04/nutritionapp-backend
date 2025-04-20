using Datas;
using Domains;
using Features.UserLogin.Requests;
using Microsoft.EntityFrameworkCore;

namespace Features.UserLogin.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly Context _context;

        public LoginRepository(Context context)
        {
            _context = context;
        }

        public async Task<User?> CheckLogin(LoginRequest login)
        {
            var result = await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == login.Email && u.Password == login.Password)
                .FirstOrDefaultAsync();
            
            return result;
        }
        
        public async Task<User?> Getuser(Guid? id)
        {
            var result = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            
            return result;
        }
    }
}
