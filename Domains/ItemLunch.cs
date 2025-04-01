using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class ItemLunch
    {
        public Guid Id { get; set; }
        public ushort Amount { get; set; }

        // n-1 Food
        [ForeignKey(nameof(Food_id))]
        public Guid Food_id { get; set; }
        public Food? Food { get; set; }

        // n-1 Lunch
        [ForeignKey(nameof(Lunch_id))]
        public Guid Lunch_id { get; set; }
        public Lunch? Lunch { get; set; }
    }
}
