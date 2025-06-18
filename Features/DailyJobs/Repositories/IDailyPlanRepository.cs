using Domains;

namespace Features.DailyJobs.Repositories
{
    public interface IDailyPlanRepository
    {
        Task<DailyPlan?> GetDailyPlanAsync(Guid userId, DateOnly today, bool tracking);
        Task<User> GetUserAsync(Guid userId, bool tracking);
        Task<IEnumerable<DailyPlan>?> GetDailyPlanInWeekAsync(Guid userId, DateOnly start, DateOnly end,
            bool tracking);
    }
}
