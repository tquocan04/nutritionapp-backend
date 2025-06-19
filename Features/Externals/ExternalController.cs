using Features.Externals.Requests;
using Features.Externals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Features.Externals
{
    [Route("api/external")]
    [ApiController]
    public class ExternalController(IIndexingService indexingService,
        IRecommendationService recommendationService) 
        : ControllerBase
    {
        [HttpPost("indexing/csv")]
        public async Task<IActionResult> RunCsvIndexing(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Choose CSV to upload.");
            }

            using var stream = file.OpenReadStream();
            await indexingService.ProcessFileAsync(stream);

            return Ok("Successful.");
        }
        
        [HttpGet("recommendations/meal")]
        [Authorize]
        public async Task<IActionResult> GetRecommendMeals()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await recommendationService.GetRecommendationsForMealAsync(userId);

            return Ok(result);
        }
    }
}
