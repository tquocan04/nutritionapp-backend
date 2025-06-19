using Features.Externals.Requests;
using Features.Externals.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Features.Externals
{
    [Route("api/external")]
    [ApiController]
    public class ExternalController(IIndexingService indexingService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostFoodMass([FromBody] IList<FoodMassesGramRequest> req)
        {
            return Ok(req);
        }

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
    }
}
