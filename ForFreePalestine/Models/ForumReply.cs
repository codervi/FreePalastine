using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFreePalestine.Models
{
    public class ForumReply
    {
        [Key]
        public int ReplyId { get; set; }

        [Required(ErrorMessage = "Content cannot be empty.")]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // What topic was it written about?
        public int ThreadId { get; set; }
        [ForeignKey("ThreadId")]
        public virtual ForumThread Thread { get; set; }

        //  Who wrote this? 
        // We're changing the string part to int.
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }
    }
}