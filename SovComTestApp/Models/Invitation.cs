using System.ComponentModel.DataAnnotations.Schema;

namespace SovComTestApp.Models
{
    public class Invitation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int ApiId { get; set; } = 4;
        public string Message { get; set; } = string.Empty;

    }
}
