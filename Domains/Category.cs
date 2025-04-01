namespace Domains
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        //1-n Food
        public ICollection<Food>? Foods { get; set; }
    }
}
