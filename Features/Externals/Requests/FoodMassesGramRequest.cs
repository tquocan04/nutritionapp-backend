namespace Features.Externals.Requests
{
    public record FoodMassesGramRequest
    {
        public string Food { get; init; } = null!;
        public float Mass { get; init; }
    }
}
