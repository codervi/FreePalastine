using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFreePalestine.Models
{
    public class ForumThread
    {
        [Key]
        public int ThreadId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // The person who brought up the topic
        public int UserId { get; set; }

        [ForeignKey("UserId")] // 
        public virtual UserInfo User { get; set; }

        // List of answers
        public virtual List<ForumReply> Replies { get; set; }

        public int CategoryId { get; set; } // Foreign Key
        public virtual ForumCategory Category { get; set; } // Navigation Property
    }
}
