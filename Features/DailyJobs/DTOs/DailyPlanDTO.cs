namespace Features.DailyJobs.DTOs
{
    public class DailyPlanDTO
    {
        public Guid Id { get; set; }
        public float TotalCalories { get; set; } = 0;
        public float TotalCarbs { get; set; } = 0;
        public float TotalFats { get; set; } = 0;
        public float TotalProteins { get; set; } = 0;
        public float TargetCalories { get; set; } = 0;
        public float TargetCarbs { get; set; } = 0;
        public float TargetFats { get; set; } = 0;
        public float TargetProteins { get; set; } = 0;
        public DateOnly? Date { get; set; }
    }
}
