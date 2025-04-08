using AutoMapper;
using Domains;
using Features.UserFeatures.DTOs;
using Features.UserFeatures.Exceptions;
using Features.UserFeatures.Requests.Register;

namespace Features.UserFeatures.Service
{
    public class UserService(IRepositoryManager repositoryManager, IMapper mapper) : IUserService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;
        
        public async Task<NewUserResponseDTO> Register(RegisterRequest req)
        {
            if (await _repositoryManager.User.CheckEmailExist(req))
                throw new EmailBadRequestException(req.Email);
            
            if (await _repositoryManager.User.CheckUsernameExist(req))
                throw new UsernameBadRequestException(req.Username);

            User newUser = _mapper.Map<User>(req);

            await _repositoryManager.User.Create(newUser);

            await _repositoryManager.SaveAsync();
            return _mapper.Map<NewUserResponseDTO>(newUser);
        }
    }
}
