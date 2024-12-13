using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class User
    {
        [Key]
        [BindNever]
        public string Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please provide a valid name. A name must be between 50 and 3 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The surname field is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please provide a valid surname. A surname must be between 50 and 3 characters.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "The email address field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid number.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "The profession field is required.")]
        public string? Profession { get; set; }

        public int YearsOfExperience { get; set; }

        #region Navigation Properties
        public List<Visit>? Visits { get; set; }
        #endregion
    }
}
