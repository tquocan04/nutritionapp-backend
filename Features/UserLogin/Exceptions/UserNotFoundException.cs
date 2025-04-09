using ExceptionHandler;

namespace Features.UserLogin.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(Guid id) : base($"UserId: {id} not found in database.") { }
    }
}
