using Features.FoodItems.DTOs;
using Features.FoodItems.Requests;
using Features.FoodItems.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Features.FoodItems
{
    [Route("api/food-items")]
    [ApiController]
    [Authorize]
    public class FoodItemsController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpPost]
        public async Task<IActionResult> CreateNewFoodItem([FromBody] FoodItemRequest req)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _service.FoodItemService.CreateNewFoodItemAsync(userId, req);
            return Ok("Successful.");
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMeal(int year, int month, int date)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            DateOnly dateRequest = new(year, month, date);

            var result = await _service.FoodItemService.GetMealAsync(userId, dateRequest);

            return Ok(new MealResponse<MealsDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
