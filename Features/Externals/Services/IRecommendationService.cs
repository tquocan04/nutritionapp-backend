using Features.Externals.Models;

namespace Features.Externals.Services
{
    public interface IRecommendationService
    {
        Task<IEnumerable<RecipeDto>> GetRecommendationsForMealAsync(Guid userId);
    }
}
