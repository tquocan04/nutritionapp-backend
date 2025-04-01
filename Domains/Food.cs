using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class Food
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public uint Calories { get; set; }
        public uint Carb { get; set; }
        public uint Fat { get; set; }
        public uint Protein { get; set; }
        public string? Image { get; set; }

        // 1-n
        public ICollection<ItemBreakfast>? ItemBreakfasts { get; set; }
        public ICollection<ItemLunch>? ItemLunches { get; set; }
        public ICollection<ItemDinner>? ItemDinners { get; set; }

        //n-1 Category
        [ForeignKey(nameof(Category_id))]
        public Guid Category_id { get; set; }
        public Category? Category { get; set; }
    }
}
