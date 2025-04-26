using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Models
{
    public class QuizWiseQuestionsModel
    {
        [Key]
        public int QuizWiseQuestionsID { get; set; }

        [Required]
        [ForeignKey("QuizModel")]
        public int QuizID { get; set; }

        [Required]
        [ForeignKey("QuestionModel")]
        public int QuestionID { get; set; }

        [Required]
        [ForeignKey("UserModel")]
        public int UserID { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }
    }
}
