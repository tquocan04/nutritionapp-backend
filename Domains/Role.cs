namespace Domains
{
    public class Role
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ICollection<User>? Users { get; set; }
    }
}
