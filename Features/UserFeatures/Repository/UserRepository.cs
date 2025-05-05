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

        public async Task AddNewBreakfastAsync(Breakfast item)
        {
            await _context.Breakfasts.AddAsync(item);
        }

        public async Task AddNewLunchAsync(Lunch item)
        {
            await _context.Lunches.AddAsync(item);
        }

        public async Task AddNewDinnerAsync(Dinner item)
        {
            await _context.Dinners.AddAsync(item);
        }
        
        public async Task AddNewDailyPlanAsync(DailyPlan req)
        {
            await _context.DailyPlans.AddAsync(req);
        }
    }
}
