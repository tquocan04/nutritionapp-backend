using ExceptionHandler;

namespace Features.UserFeatures.Exceptions
{
    public class UsernameBadRequestException : BadRequestException
    {
        public UsernameBadRequestException(string username) : base($"Username {username} already exists. " +
                                                                $"Please try again with a different username.") { }
    }
}
