using ExceptionHandler;

namespace Features.DailyJobs.Exceptions
{
    public class DateBadRequestException : BadRequestException
    {
        public DateBadRequestException() : base("Invalid date")
        {
        }
    }
}
