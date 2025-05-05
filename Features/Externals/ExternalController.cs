using Features.Externals.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Features.Externals
{
    [Route("api/external")]
    [ApiController]
    public class ExternalController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostFoodMass([FromBody] IList<FoodMassesGramRequest> req)
        {
            return Ok(req);
        }
    }
}
