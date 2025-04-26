using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Models
{
    public class QuestionLevelModel
    {
        [Key]
        public int QuestionLevelID { get; set; }

        [Required ]
        public string QuestionLevel { get; set; }

        [Required]
        [ForeignKey("UserModel")]
        public int UserID { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; } = DateTime.Now;

        public class QuestionLevelDropDownModel
        {
            public int QuestionLevelID { get; set; }
            public string QuestionLevel { get; set; }
        }
    }
}
