namespace Engine.Models
{
    public class SucessResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; } = default;
        public string Message { get; set; }
    }
}
