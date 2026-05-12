using System.ComponentModel.DataAnnotations;

namespace ForFreePalestine.Models
{
    public class BoycottedProducts
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string ProductName { get; set; }
        [StringLength(30)]
        public string? CompanyName { get; set; }
        [StringLength(100)]
        public string Image { get; set; }
    }
}
