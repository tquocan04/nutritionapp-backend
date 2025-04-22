using AutoMapper;
using Features.DailyJobs.DTOs;
using Features.DailyJobs.Exceptions;
using Features.DailyJobs.Requests;

namespace Features.DailyJobs.Services
{
    public class DailyPlanService(IRepositoryManager manager, IMapper mapper) 
        : IDailyPlanService
    {
        private readonly IRepositoryManager _manager = manager;
        private readonly IMapper _mapper = mapper;

        public async Task<DailyPlanDTO> GetDailyPlanAsync(Guid userId, DateRequest req)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            DateOnly dateRequest = new DateOnly(req.Year, req.Month, req.Date);

            if (dateRequest > today)
            {
                throw new DateBadRequestException();
            }

            var result = await _manager.DailyPlan.GetDailyPlanAsync(userId, dateRequest, false)
                ?? throw new DailyPlanOfUserNotFoundException(userId, dateRequest);

            return _mapper.Map<DailyPlanDTO>(result);
        }
    }
}
