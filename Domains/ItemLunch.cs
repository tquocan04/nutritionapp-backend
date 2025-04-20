using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class ItemLunch
    {
        public Guid Id { get; set; }
        public ushort Amount { get; set; }
        public string Name { get; set; } = null!;
        public float Calories { get; set; }
        public float Carb { get; set; }
        public float Fat { get; set; }
        public float Protein { get; set; }
        public string? Image { get; set; }

        // n-1 Lunch
        [ForeignKey(nameof(Lunch_id))]
        public Guid Lunch_id { get; set; }
        public Lunch? Lunch { get; set; }
    }
}
