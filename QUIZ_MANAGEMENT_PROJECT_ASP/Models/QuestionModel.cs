using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Models
{
    public class QuestionModel
    {
        [Key]
        public int QuestionID { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        [ForeignKey("QuestionLevelModel")]
        public int QuestionLevelID { get; set; }

        [Required]
        public string OptionA { get; set; }

        [Required]
        public string OptionB { get; set; }

        [Required]
        public string OptionC { get; set; }

        [Required]
        public string OptionD { get; set; }

        [Required]
        public string CorrectOption { get; set; }

        [Required]
        public int QuestionMarks { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("UserModel")]
        public int UserID { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }

        public class QuestionDropDownModel()
        {
            public int QuestionID { get; set; }
            public string QuestionText { get; set; }
        }
    }
}
