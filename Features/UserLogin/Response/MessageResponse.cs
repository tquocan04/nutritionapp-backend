namespace Features.UserLogin.Response
{
    public class MessageResponse<T>
    {
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
    }
}
