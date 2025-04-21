using ExceptionHandler;

namespace Features.FoodItems.Exceptions
{
    public class DailyPlanNotFoundException : NotFoundException
    {
        public DailyPlanNotFoundException(DateOnly date) 
            : base($"Daily Plan for {date} not found in database") { }
    }
}
