using System.ComponentModel.DataAnnotations;

namespace ForFreePalestine.Models
{
    public class HistoryInfo
    {
        [Key]
        public int HistoryId { get; set; }

        [Required (ErrorMessage = "Title Required")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Description Required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Display(Name = "Url")]
        public string? Url { get; set; }

        [Display(Name = "Image")]
        public string? Image { get; set; } = null;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true; // Is it airing?
        [Display(Name = "Created Date")]

        public DateTime CreatedDate { get; set; } = DateTime.Now; 
    }
}