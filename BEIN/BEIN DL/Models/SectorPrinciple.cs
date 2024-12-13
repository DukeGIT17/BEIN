
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEIN_DL.Models
{
    public class SectorPrinciple
    {
        [Key]
        public string Id { get; set; } = "";

        [Required(ErrorMessage = "Please provide sector principle.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "A sector principle should contain between 3 and 20 characters.")]
        public string Principle { get; set; }

        [Required(ErrorMessage = "Please provide sector principle description.")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "A sector principle description should contain between 10 and 250 characters.")]
        public string Description { get; set; }

        #region Foreign Keys
        [ForeignKey(nameof(SectorInformation))]
        public string SectorInformationId { get; set; }
        #endregion

        #region Navigation Properties
        public SectorInformation? SectorInformation { get; set; }
        #endregion
    }
}
