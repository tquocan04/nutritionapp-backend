using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class WeightTracking
    {
        public Guid Id { get; set; }
        public float Weight { get; set; }
        public DateOnly Date { get; set; }

        // FK User
        [ForeignKey(nameof(User_id))]
        public Guid User_id { get; set; }
        public User? User { get; set; }
    }
}
