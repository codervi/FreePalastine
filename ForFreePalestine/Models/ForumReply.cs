using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFreePalestine.Models
{
    public class ForumReply
    {
        [Key]
        public int ReplyId { get; set; }

        [Required(ErrorMessage = "Mesaj alanı boş bırakılamaz.")]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Hangi konuya yazıldı?
        public int ThreadId { get; set; }
        [ForeignKey("ThreadId")]
        public virtual ForumThread Thread { get; set; }

        // Kim yazdı?
        // string olan yeri int yapıyoruz
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }
    }
}