using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForFreePalestine.Models
{
    public class UserInfo
    {
        [Key]
        public int UserId { get; set; }
        [Display(Name = "Name")]
        [Required]
        public string UserRealName { get; set; }
        [Display(Name = "Surname")]     
        [Required]
        public string UserRealSurName { get; set; }
        [Display(Name = "Phone Number")]
        [Required]
        [MaxLength(15)]
        public string UserPhone { get; set; }
        [Display(Name = "Username")]
        [Required]
        [MaxLength(18)]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Confirm Password field (Not mapped to the database)
        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
