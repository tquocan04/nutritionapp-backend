using System.ComponentModel.DataAnnotations;

namespace Features.UserFeatures.Requests.Register
{
    public record RegisterRequest
    {
        public string Name { get; init; } = null!;
        [EmailAddress]
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
