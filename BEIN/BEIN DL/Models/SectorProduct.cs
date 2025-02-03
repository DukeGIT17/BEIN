
using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class SectorProduct
    {
        [Key]
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public SoftwareProduct? Product { get; set; }

        [Key]
        public string SectorId { get; set; } = Guid.NewGuid().ToString();
        public string SectorTitle { get; set; }
        public Sector? Sector { get; set; }
    }
}
