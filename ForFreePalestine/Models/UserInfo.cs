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
        [StringLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string UserRealName { get; set; }
        [Display(Name = "Surname")]     
        [Required]
        [StringLength(20, ErrorMessage = "Surname cannot exceed 20 characters.")]
        public string UserRealSurName { get; set; }
        [Display(Name = "Phone Number")]
        [Required]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string UserPhone { get; set; }
        [Display(Name = "Username")]
        [Required]
        [StringLength(18, ErrorMessage = "Username cannot exceed 18 characters.")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
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
        [Display(Name = "User Role")]
        public UserRoles Role { get; set; } = UserRoles.StandardUser; // By default, everyone should start with 'Standard'
    }
}
