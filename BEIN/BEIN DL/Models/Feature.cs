using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class Feature
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide a feature title.", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Feature title should have characters between 3 and 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide a feature description.", AllowEmptyStrings = false)]
        [StringLength(3000, MinimumLength = 3, ErrorMessage = "Feature description should have between 3 and 3000 characters.")]
        public string Description { get; set; }

        public SoftwareProduct SoftwareProduct { get; set; }

        [ForeignKey(nameof(SoftwareProduct))]
        public string SoftwareProductId { get; set; }
    }
}
