using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class SoftwareProduct
    {
        [Key]
        [BindNever]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide the software's name.", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Names of softwares should be between 1 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide the software's description.", AllowEmptyStrings = false)]
        [StringLength(10000, ErrorMessage = "A description cannot be longer than 10000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please provide the vendor's name.", AllowEmptyStrings = false)]
        [Display(Name = "Vendor Name")]
        [StringLength(100, ErrorMessage = "Vendor name cannot be greater than 100 characters.")]
        public string Vendor { get; set; }

        public int Rating { get; set; }

        [StringLength(5000, ErrorMessage = "A review must contain between 5 and 5000 characters.")]
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
