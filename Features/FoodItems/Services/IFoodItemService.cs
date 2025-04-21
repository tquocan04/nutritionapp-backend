using Features.FoodItems.Requests;

namespace Features.FoodItems.Services
{
    public interface IFoodItemService
    {
        Task CreateNewFoodItemAsync(FoodItemRequest req);
    }
}
