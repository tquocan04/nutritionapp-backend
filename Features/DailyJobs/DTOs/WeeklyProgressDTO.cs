namespace Features.DailyJobs.DTOs
{
    public class WeeklyProgressDTO
    {
        public CaloriesDTO? Calories { get; set; }
        public IList<DailyProgress>? DailyProgresses { get; set; }
        public WeightDTO? Weight { get; set; }
        public IList<Macros>? Macros { get; set; }
        public IList<Nutrition>? Nutrtion { get; set; }
    }

    public class CaloriesDTO
    {
        public float Current { get; set; } = 0;
        public float Target { get; set; } = 0;
    }

    public class DailyProgress
    {
        public string Date {  get; set; }
        public float? Calories { get; set; }
        public float? Goal { get; set; }
    }

    public class WeightDTO
    {
        public float Current { get; set; } = 0;
        public float Change { get; set; } = 0;
        public IList<WeightList>? Data { get; set; }
    }

    public class WeightList
    {
        public string Date { get; set; }
        public float? Value { get; set; }
    }

    public class Macros
    {
        public string? Name { get; set; }
        public float? Value { get; set; }
    }

    public class Nutrition
    {
        public string Date { get; set; }
        public float? Carbs { get; set; }
        public float? Protein { get; set; }
        public float? Fat { get; set; }
    }
}
