namespace Features.FoodItems.DTOs
{
    public class MealsDTO
    {
        public BreakfastDTO? Breakfast { get; set; }
        public LunchDTO? Lunch { get; set; }
        public DinnerDTO? Dinner { get; set; }
    }

    public class BreakfastDTO
    {
        public float TotalCalories { get; set; }
        public IEnumerable<ItemDTO>? Items { get; set; }
    }
    
    public class LunchDTO
    {
        public float TotalCalories { get; set; }
        public IEnumerable<ItemDTO>? Items { get; set; }
    }
    
    public class DinnerDTO
    {
        public float TotalCalories { get; set; }
        public IEnumerable<ItemDTO>? Items { get; set; }
    }

    public class ItemDTO
    {
        //public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public float Calories { get; set; }
    }
}
