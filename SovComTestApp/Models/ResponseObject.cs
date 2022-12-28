namespace SovComTestApp.Models
{
    public class ResponseObject
    {
        public ResponseObject(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
