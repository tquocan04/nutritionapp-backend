namespace Features.UserLogin.Response
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Image { get; set; } = null!;
        public bool Status { get; set; }
        public string? Name { get; set; }
    }
}
