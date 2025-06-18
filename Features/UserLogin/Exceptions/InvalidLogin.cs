using ExceptionHandler;

namespace Features.UserLogin.Exceptions
{
    public class InvalidLogin : BadRequestException
    {
        public InvalidLogin() : base("Invalid Username or Password! Please check and try again.") { }
    }
}
