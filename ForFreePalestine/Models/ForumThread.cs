using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFreePalestine.Models
{
    public class ForumThread
    {
        [Key]
        public int ThreadId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Konuyu açan kişi
        public int UserId { get; set; }

        [ForeignKey("UserId")] // Açık açık EF Core'a "Üstteki UserId'yi kullan" diyoruz.
        public virtual UserInfo User { get; set; } // 'Users' yerine 'User' (Tekil) yapmak her zaman en temizidir.

        // Cevaplar listesi
        public virtual List<ForumReply> Replies { get; set; }
        // ForumThread.cs içine eklenecekler:

        public int CategoryId { get; set; } // Foreign Key
        public virtual ForumCategory Category { get; set; } // Navigation Property
    }
}
