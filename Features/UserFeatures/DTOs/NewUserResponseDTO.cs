namespace Features.UserFeatures.DTOs
{
    public record NewUserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string Username { get; init; } = null!;
    }
}
