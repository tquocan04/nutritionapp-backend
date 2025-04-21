namespace Features.FoodItems.Requests
{
    public record FoodItemRequest
    (
        string Name,
        float Grams,
        float Calories,
        float Carbs,
        float Fat,
        float Protein
    );
}
