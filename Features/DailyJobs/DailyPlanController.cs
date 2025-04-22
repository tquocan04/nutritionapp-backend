using Features.DailyJobs.DTOs;
using Features.DailyJobs.Requests;
using Features.DailyJobs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Features.DailyJobs
{
    [Route("api/dailyplans")]
    [ApiController]
    [Authorize]
    public class DailyPlanController(IServiceManager service) : ControllerBase
    {
        private readonly IServiceManager _service = service;

        [HttpGet]
        public async Task<IActionResult> GetDailyPlan(int year, int month, int date)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            DateRequest req = new(date, month, year);

            var result = await _service.DailyPlanService.GetDailyPlanAsync(userId, req);

            return Ok(new DailyPlanResponse<DailyPlanDTO>
            {
                Message = "Successful.",
                Data = result
            });
        }
    }
}
