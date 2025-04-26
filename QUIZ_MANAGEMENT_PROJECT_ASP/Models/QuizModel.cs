using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Models
{
    public class QuizModel
    {
        internal List<UserModel> Users;

        [Key]
        public int QuizID { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "QuizName Cannot Exceed 100 Characters")]
        public string QuizName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total Questions must be greater than zero.")]
        public int TotalQuestions { get; set; }

        [Required]
        public DateTime QuizDate { get; set; }

        [Required]
        [ForeignKey("UserModel")]
        public int UserID { get; set; }

        //public string UserName { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }

        public class QuizDropDownModel()
        {
            public int QuizID { get; set; }
            public string  QuizName { get; set; }
        }
    }
}