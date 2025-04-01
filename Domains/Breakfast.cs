using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domains
{
    public class Breakfast
    {
        public Guid Id { get; set; }
        public float TotalCalories { get; set; } = 0;
        public float TotalCarbs { get; set; } = 0;
        public float TotalFats { get; set; } = 0;
        public float TotalProteins { get; set; } = 0;

        //1-n ItemBreakfast
        public ICollection<ItemBreakfast>? ItemBreakfasts { get; set; }

        //n-1 DailyPlan
        [ForeignKey(nameof(DailyPlan_id))]
        public Guid DailyPlan_id { get; set; }
        public DailyPlan? DailyPlan { get; set; }
    }
}
