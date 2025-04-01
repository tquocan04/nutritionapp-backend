using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class DailyPlan
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

        // n-1
        [ForeignKey(nameof(User_id))]
        public Guid User_id { get; set; }
        public User? User { get; set; }
        //1-1
        [ForeignKey(nameof(Breakfast_id))]
        public Guid Breakfast_id { get; set; }
        public Breakfast? Breakfast { get; set; }
        //1-1
        [ForeignKey(nameof(Lunch_id))]
        public Guid Lunch_id { get; set; }
        public Lunch? Lunch { get; set; }
        //1-1
        [ForeignKey(nameof(Dinner_id))]
        public Guid Dinner_id { get; set; }
        public Dinner? Dinner { get; set; }
    }
}
