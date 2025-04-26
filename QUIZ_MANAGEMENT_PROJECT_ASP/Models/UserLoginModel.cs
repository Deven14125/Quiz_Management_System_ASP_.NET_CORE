using System.ComponentModel.DataAnnotations;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Models
{
    public class UserLoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
