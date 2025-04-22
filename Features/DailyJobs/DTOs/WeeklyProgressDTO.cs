namespace Features.DailyJobs.DTOs
{
    public class WeeklyProgressDTO
    {
        public IList<CaloriesDTO>? Calories { get; set; }
        public WeightDTO? Weight { get; set; }
    }

    public class CaloriesDTO
    {
        public DateOnly Date { get; set; }
        public float Total { get; set; } = 0;
        public float Target { get; set; } = 0;
    }

    public class WeightDTO
    {
        public float Weight { get; set; }
    }
}
