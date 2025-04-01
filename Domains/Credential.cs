using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class Credential
    {
        public string Id { get; set; } = null!;
        public string? Provider { get; set; }

        [ForeignKey(nameof(User_id))]
        public Guid User_id { get; set; }
        public User? User { get; set; }
    }
}
