using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEIN_DL.Models
{
    public class SoftwareProduct
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Display(Name = "Image Name")]
        public string? ImageName { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Please provide the software's name.")]
        [StringLength(100, ErrorMessage = "Names of softwares should be between 1 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide the software's description.")]
        [StringLength(700, ErrorMessage = "A description cannot be longer than 700 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please provide the vendor's name.")]
        [Display(Name = "Vendor Name")]
        [StringLength(100, ErrorMessage = "Vendor name cannot be greater than 100 characters.")]
        public string Vendor { get; set; }

        public int Rating { get; set; }

        [StringLength(2500, ErrorMessage = "A review must contain between 5 and 2500 characters.")]
        public string? Review { get; set; }
        
        public string ProjectStage { get; set; }

        public List<string> Professions { get; set; }

        #region Navigation Properties
        public List<SectorProduct>? Sectors { get; set; }

        public List<Feature>? Features { get; set; }

        public List<Visit>? Visits { get; set; }
        #endregion
    }
}
