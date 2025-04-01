using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class ItemDinner
    {
        public Guid Id { get; set; }
        public ushort Amount { get; set; }

        // n-1 Food
        [ForeignKey(nameof(Food_id))]
        public Guid Food_id { get; set; }
        public Food? Food { get; set; }

        // n-1 Dinner
        [ForeignKey(nameof(Dinner_id))]
        public Guid Dinner_id { get; set; }
        public Dinner? Dinner { get; set; }
    }
}
