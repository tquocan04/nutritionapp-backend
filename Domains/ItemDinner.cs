using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class ItemDinner
    {
        public Guid Id { get; set; }
        public ushort Amount { get; set; }
        public string Name { get; set; } = null!;
        public float Calories { get; set; }
        public float Carb { get; set; }
        public float Fat { get; set; }
        public float Protein { get; set; }
        public string? Image { get; set; }

        // n-1 Dinner
        [ForeignKey(nameof(Dinner_id))]
        public Guid Dinner_id { get; set; }
        public Dinner? Dinner { get; set; }
    }
}
