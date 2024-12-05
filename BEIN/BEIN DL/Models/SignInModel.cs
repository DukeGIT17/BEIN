using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "The email field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
