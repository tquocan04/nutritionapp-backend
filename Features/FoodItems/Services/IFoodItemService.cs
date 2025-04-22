using Domains;
using Features.FoodItems.DTOs;
using Features.FoodItems.Requests;

namespace Features.FoodItems.Services
{
    public interface IFoodItemService
    {
        Task CreateNewFoodItemAsync(Guid userId, FoodItemRequest req);
        Task<MealsDTO> GetMealAsync(Guid userId, DateOnly date);
    }
}
