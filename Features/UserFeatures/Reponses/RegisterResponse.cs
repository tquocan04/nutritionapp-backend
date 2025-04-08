namespace Features.UserFeatures.Reponses
{
    public class RegisterResponse<T>
    {
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
    }
}
