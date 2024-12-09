using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BEIN_DL.Models
{
    public class SectorInformation
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide a header.")]
        [StringLength(35, ErrorMessage = "Headers consist of between 1 and 35 characters.")]
        public string Header { get; set; }

        #region Foreign Keys
        [ForeignKey(nameof(Sector))]
        public string SectorId { get; set; }
        #endregion

        #region Navigation Properties
        public Sector Sector { get; set; }

        public List<CardInfo> CardInformation { get; set; }

        public List<SectorPrinciple> SectorPrinciples { get; set; }
        #endregion
    }
}
