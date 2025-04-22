namespace Features.DailyJobs.Responses
{
    public record DailyPlanResponse<T>
    {
        public string Message { get; init; } = null!;
        public T? Data { get; init; }
    }
}
