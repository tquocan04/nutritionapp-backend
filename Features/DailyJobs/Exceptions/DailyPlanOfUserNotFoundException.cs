using ExceptionHandler;

namespace Features.DailyJobs.Exceptions
{
    public class DailyPlanOfUserNotFoundException : NotFoundException
    {
        public DailyPlanOfUserNotFoundException(Guid userId, DateOnly date) 
            : base($"UserId: {userId} has no daily plan on {date}.") { }
    }
}
