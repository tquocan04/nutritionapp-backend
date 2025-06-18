using Datas;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Features.DailyJobs.Repositories
{
    public class DailyPlanRepository(Context context) : IDailyPlanRepository
    {
        private readonly Context _context = context;

        public async Task<DailyPlan?> GetDailyPlanAsync(Guid userId, DateOnly today, bool tracking)
        {
            if (!tracking)
            {
                return await _context.DailyPlans
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dp => dp.Date == today && dp.User_id == userId);
            }

            return await _context.DailyPlans
                    .FirstOrDefaultAsync(dp => dp.Date == today && dp.User_id == userId);
        }
        
        public async Task<User> GetUserAsync(Guid userId, bool tracking)
        {
            if (!tracking)
            {
                return await _context.Users
                    .AsNoTracking()
                    .FirstAsync(dp => dp.Id == userId);
            }

            return await _context.Users
                    .FirstAsync(dp => dp.Id == userId);
        }
        
        public async Task<IEnumerable<DailyPlan>?> GetDailyPlanInWeekAsync(Guid userId, DateOnly start, 
            DateOnly end, bool tracking)
        {
            if (!tracking)
            {
                return await _context.DailyPlans
                    .AsNoTracking()
                    .Where(dp => dp.User_id == userId
                                            && dp.Date >= start 
                                            && dp.Date <= end)
                    .ToListAsync();
            }

            return await _context.DailyPlans
                    .Where(dp => dp.User_id == userId
                                            && dp.Date >= start
                                            && dp.Date <= end)
                    .ToListAsync();
        }

        public async Task CreateNewWeightTracking(WeightTracking weight)
        {
            await _context.WeightTrackings.AddAsync(weight);
        }
    }
}