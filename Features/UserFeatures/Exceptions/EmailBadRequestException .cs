using ExceptionHandler;

namespace Features.UserFeatures.Exceptions
{
    public class EmailBadRequestException : BadRequestException
    {
        public EmailBadRequestException(string email) : base($"Email {email} already exists. " +
                                                                $"Please try again with a different email.") { }
    }
}
