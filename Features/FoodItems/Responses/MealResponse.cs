namespace Features.FoodItems.Responses
{
    public class MealResponse<T>
    {
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
    }
}
