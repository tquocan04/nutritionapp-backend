using Features.UserFeatures.DTOs;
using Features.UserFeatures.Requests.Register;

namespace Features.UserFeatures.Service
{
    public interface IUserService
    {
        Task<NewUserResponseDTO> Register(RegisterRequest req);
    }
}
