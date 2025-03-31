using System.ComponentModel.DataAnnotations;

namespace Features.UserLogin.Requests
{
    public record LoginRequest([EmailAddress] string Username, string Password);
}
