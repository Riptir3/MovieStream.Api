namespace TaskManager.Api.Models
{
    public sealed class Response<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        private Response(bool success, string? message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public static Response<T> OK(string? message = null, T? data = default)
                => new(true, message, data);

        public static Response<T> OK(string? message = null)
            => new(true, message);

        public static Response<T> Fail(string message)
            => new(false, message);
    }
}
