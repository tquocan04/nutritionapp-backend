using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class ItemBreakfast
    {
        public Guid Id { get; set; }
        public ushort Amount { get; set; }
        public string Name { get; set; } = null!;
        public float Calories { get; set; }
        public float Carb { get; set; }
        public float Fat { get; set; }
        public float Protein { get; set; }
        public string? Image { get; set; }
        
        // n-1 Breakfast
        [ForeignKey(nameof(Breakfast_id))]
        public Guid Breakfast_id { get; set; }
        public Breakfast? Breakfast { get; set; }
    }
}
