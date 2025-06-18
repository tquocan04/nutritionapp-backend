using System.ComponentModel.DataAnnotations.Schema;

namespace Domains
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public ushort Age { get; set; }
        public string? Gender { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public float? TargetWeight { get; set; }
        public int Time { get; set; } = 0;
        public float? R { get; set; }   //he so van dong
        public float? BMR { get; set; }
        public float? TDEE { get; set; }
        public string? Image { get; set; } = "https://res.cloudinary.com/dn5rrigtr/image/upload/v1746462028/pc2facdxhwzlbqjp20qp.jpg";
        public bool IsActive { get; set; } = false;
    }
}
