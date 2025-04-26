namespace QUIZ_MANAGEMENT_PROJECT_ASP.Models
{
    public class QuizQuestionModel
    {
        public int QuizID { get; set; }

        public string QuizName { get; set; }

        public int QuestionID { get; set; }

        public string QuestionText { get; set; }

        public string OptionA { get; set; }

        public string OptionB { get; set; }

        public string OptionC { get; set; }

        public string OptionD { get; set; }

        public string CorrectOption { get; set; }

        public int QuestionMarks { get; set; }

        public class QuizQuestionDropDownModel
        {
            public int QuizID { get; set; }

            public string QuizName { get; set; }
        }
    }
}
