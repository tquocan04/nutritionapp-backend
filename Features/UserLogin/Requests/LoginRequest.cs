using System.ComponentModel.DataAnnotations;

namespace Features.UserLogin.Requests
{
    public record LoginRequest(string Username, string Password);
}
