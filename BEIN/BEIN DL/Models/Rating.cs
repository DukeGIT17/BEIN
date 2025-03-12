using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEIN_DL.Models
{
    public class Rating
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Please provide a rating.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int RatingValue { get; set; }

        public DateTime DateOfRating { get; set; } = DateTime.Now;

        #region Foreign Keys
        [ForeignKey(nameof(Software))]
        public string SoftwareId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        #endregion

        #region Navigation Properties
        public SoftwareProduct? Software { get; set; }
        public Review? Review { get; set; }
        public User? User { get; set; }
        #endregion
    }
}
