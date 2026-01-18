namespace Engine.Models
{
    public class ErrorResponse
    {
        public string message { get; set; }
        public ErrorResponse(string error)
        {
            message = error;
        }
    }
}
