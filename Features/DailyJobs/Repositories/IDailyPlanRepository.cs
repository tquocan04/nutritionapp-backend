using Domains;

namespace Features.DailyJobs.Repositories
{
    public interface IDailyPlanRepository
    {
        Task<DailyPlan?> GetDailyPlanAsync(Guid userId, DateOnly today, bool tracking);
    }
}
