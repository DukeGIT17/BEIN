using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class Sector
    {
        [Key]
        [BindNever]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide a sector title.")]
        [StringLength(250, ErrorMessage = "Sector titles should not be greater than 250 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide a sector description..")]
        [StringLength(5000, ErrorMessage = "Sector description should not be greater than 5000 characters.")]
        public string Description { get; set; }

        #region Navigation Properties
        public List<SectorProduct>? Products { get; set; }
        public SectorInformation? SectorInformation { get; set; }
        #endregion
    }
}
