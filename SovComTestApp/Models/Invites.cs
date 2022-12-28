namespace SovComTestApp.Models
{
    public class Invites
    {
        public IList<string> PhoneNumbers { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty;
        public int ApiId { get; set; } = 4;
    }
}
