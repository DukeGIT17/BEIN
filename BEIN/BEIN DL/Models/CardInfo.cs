using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEIN_DL.Models
{
    public class CardInfo
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide a header.")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "A header should consist of between 3 and 35 characters.")]
        public string Header { get; set; }


        [Required(ErrorMessage = "Please provide information.")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "An information field should consist of between 10 and 250 characters.")]
        public string Information { get; set; }

        #region Foreign Keys
        [ForeignKey(nameof(SectorInformation))]
        public string SectorInformationId { get; set; }
        #endregion

        #region Navigation Properties
        public SectorInformation? SectorInformation { get; set; }
        #endregion
    }
}
