
using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class Sector
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Please provide a sector title.")]
        [StringLength(50, ErrorMessage = "Sector titles should not be greater than 50 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide a short sector description")]
        [Display(Name = "Short Description")]
        [StringLength(110, ErrorMessage = "Short sector description should not be greater than 110 characters.")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Please provide a sector description..")]
        [StringLength(700, ErrorMessage = "Sector description should not be greater than 700 characters.")]
        public string Description { get; set; }

        #region Navigation Properties
        public List<SectorProduct>? Products { get; set; }
        #endregion
    }
}
