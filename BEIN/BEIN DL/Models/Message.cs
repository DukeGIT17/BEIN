using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class Message
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Please provide a message.")]
        [StringLength(2000, ErrorMessage = "Message must be less than 2000 characters.")]
        [Display(Name = "Message")]
        public string Text { get; set; }

        public User Sender { get; set; }
    }
}
