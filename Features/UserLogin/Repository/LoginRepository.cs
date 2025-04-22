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

        public async Task<DailyPlan?> GetDailyPlanByUserId(Guid userId, DateOnly date, bool tracking)
        {
            if (!tracking)
            {
                return await _context.DailyPlans
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dp => dp.User_id == userId && dp.Date == date);
            }

            return await _context.DailyPlans
                    .FirstOrDefaultAsync(dp => dp.User_id == userId && dp.Date == date);
        }

        public async Task AddNewBreakfastAsync(Breakfast breakfast)
        {
            await _context.Breakfasts.AddAsync(breakfast);
        }
        
        public async Task AddNewLunchAsync(Lunch Lunch)
        {
            await _context.Lunches.AddAsync(Lunch);
        }
        
        public async Task AddNewDinnerAsync(Dinner Dinner)
        {
            await _context.Dinners.AddAsync(Dinner);
        }
        
        public async Task AddNewDailyPlanAsync(DailyPlan daily)
        {
            await _context.DailyPlans.AddAsync(daily);
        }
    }
}
