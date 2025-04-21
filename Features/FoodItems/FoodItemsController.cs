using Features.FoodItems.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Features.FoodItems
{
    [Route("api/food-items")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public FoodItemsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewFoodItem([FromBody] FoodItemRequest req)
        {
            await _service.FoodItemService.CreateNewFoodItemAsync(req);
            return Ok("Successful.");
        }
    }
}
