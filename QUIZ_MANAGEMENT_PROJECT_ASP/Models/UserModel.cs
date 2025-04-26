using System.ComponentModel.DataAnnotations;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(10,MinimumLength = 4, ErrorMessage = "Username cannot be less than 4 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        //    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Email must be a valid format (e.g., example@domain.com)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits")]
        public string Mobile { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }

        // Nested model for dropdown
        public class UserDropDownModel
        {
            public int UserID { get; set; }

            [Required(ErrorMessage = "Username is required")]
            public string UserName { get; set; }
        }
    }
}
