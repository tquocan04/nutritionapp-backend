using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class Lunch
    {
        public Guid Id { get; set; }
        public float TotalCalories { get; set; } = 0;
        public float TotalCarbs { get; set; } = 0;
        public float TotalFats { get; set; } = 0;
        public float TotalProteins { get; set; } = 0;

        //1-n ItemLunch
        public ICollection<ItemLunch>? ItemLunches { get; set; }

        //n-1 DailyPlan
        [ForeignKey(nameof(DailyPlan_id))]
        public Guid DailyPlan_id { get; set; }
        public DailyPlan? DailyPlan { get; set; }
    }
}
