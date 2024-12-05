using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "The name field is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please provide a valid name. A name must be between 50 and 3 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The surname field is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please provide a valid surname. A surname must be between 50 and 3 characters.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "The email address field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
