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
    }
}