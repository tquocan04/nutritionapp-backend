using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class ItemBreakfast
    {
        public Guid Id { get; set; }
        public ushort Amount { get; set; }

        // n-1 Food
        [ForeignKey(nameof(Food_id))]
        public Guid Food_id { get; set; }
        public Food? Food { get; set; }
        
        // n-1 Breakfast
        [ForeignKey(nameof(Breakfast_id))]
        public Guid Breakfast_id { get; set; }
        public Breakfast? Breakfast { get; set; }
    }
}
