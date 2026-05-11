using System.ComponentModel.DataAnnotations;

namespace ForFreePalestine.Models
{
    public class ForumCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı boş olamaz.")]
        public string Name { get; set; } // Örn: Genel, Tarih, Haberler

        public string? Description { get; set; } // Kategori ne işe yarar?

        // Bu kategorinin altındaki konular
        public virtual List<ForumThread> Threads { get; set; } = new List<ForumThread>();
    }
}