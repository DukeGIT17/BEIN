using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEIN_DL.Models
{
    public class Review
    {
        [Key]
        public string Id { get; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Please provide a review head.")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "Review head should have characters between 3 and 70 characters.")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Please provide a review text.")]
        [StringLength(1200, MinimumLength = 50, ErrorMessage = "Review should have between 50 and 1200 characters.")]
        public string ReviewText { get; set; }

        [Required(ErrorMessage = "Review date and time not provided.")]
        public DateTime ReviewTime { get; set; }

        #region Foreign Keys
        [ForeignKey(nameof(Rating))]
        public string RatingId { get; set; }
        #endregion

        #region Navigation Properties
        public Rating Rating { get; set; }
        #endregion
    }
}
