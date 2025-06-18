using Features.DailyJobs.DTOs;
using Features.DailyJobs.Requests;

namespace Features.DailyJobs.Services
{
    public interface IDailyPlanService
    {
        Task<DailyPlanDTO> GetDailyPlanAsync(Guid userId, DateRequest req);
        Task<WeeklyProgressDTO> GetCaloriesInWeekAsync(Guid userId);
    }
}
