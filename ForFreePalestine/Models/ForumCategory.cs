using System.ComponentModel.DataAnnotations;

namespace ForFreePalestine.Models
{
    public class ForumCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category names cannot be empty.")]
        [StringLength(40, ErrorMessage = "Category names cannot exceed 40 characters.")]
        public string Name { get; set; } 

        public virtual List<ForumThread> Threads { get; set; } = new List<ForumThread>();
    }
}