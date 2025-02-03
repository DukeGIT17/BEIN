using System.ComponentModel.DataAnnotations;

namespace BEIN_DL.Models
{
    public class Visit
    {
        [Key]
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Surname { get; set; }
        public User? User { get; set; }

        [Key]
        public string SoftwareProductId { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public SoftwareProduct? Product { get; set; }
    }
}
