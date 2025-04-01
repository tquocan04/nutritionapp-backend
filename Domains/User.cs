using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public ushort Age { get; set; }
        public string? Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float TargetWeight { get; set; }

        [ForeignKey("RoleId")]
        public string RoleId { get; set; } = null!;
        public Role? Role { get; set; }
    }
}
