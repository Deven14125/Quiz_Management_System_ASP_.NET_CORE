/*using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.QuizQuestionModel;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    public class QuizQuestionController : Controller
    {
        private readonly IConfiguration _configuration;

        //constructor
        public QuizQuestionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult GetQuizData(int QuizID)
        {
            TempData["Title"] = "QuizQuestion_List";

            ViewBag.QuizID = QuizID;

            string connectionstring = _configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection1 = new SqlConnection(connectionstring))
            {
                connection1.Open();

                SqlCommand command1 = connection1.CreateCommand();
                command1.CommandType = CommandType.StoredProcedure;
                command1.CommandText = "PR_MST_Quiz_SelectAll";

                SqlDataReader reader1 = command1.ExecuteReader();

                DataTable dataTable1 = new DataTable();
                dataTable1.Load(reader1);

                List<QuizQuestionDropDownModel> quizQuestionList = new List<QuizQuestionDropDownModel>();

                foreach (DataRow row in dataTable1.Rows)
                {
                    QuizQuestionDropDownModel quizQuestionDropDownModel = new QuizQuestionDropDownModel();
                    quizQuestionDropDownModel.QuizID = Convert.ToInt32(row["QuizID"]);
                    quizQuestionDropDownModel.QuizName = row["QuizName"].ToString();
                    quizQuestionList.Add(quizQuestionDropDownModel);
                }
                ViewBag.quizQuestionList = quizQuestionList;
            }

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Quiz_QuestionList_Select";

                command.Parameters.AddWithValue("@QuizID", QuizID);

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
            }
            return View();
        }

        public IActionResult QuizQuestion_List()
        {
            return View();
        }
    }
}
*/

using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using System.Collections.Generic;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.QuizQuestionModel;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    public class QuizQuestionController : Controller
    {
        private readonly IConfiguration _configuration;

        // Constructor
        public QuizQuestionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region GetQuizData

        // Get the quiz list and questions (if quiz is selected)
        //[Route("QuizQuestion/GetQuizData/{QuizID?}")]
        public IActionResult GetQuizData(int QuizID)
        {
            TempData["Title"] = "Quiz Question List";

            // Fetch quiz list for the dropdown
            string connectionstring = _configuration.GetConnectionString("ConnectionString");

            List<QuizQuestionDropDownModel> quizQuestionList = new List<QuizQuestionDropDownModel>();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_Quiz_SelectAll";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                foreach (DataRow row in dataTable.Rows)
                {
                    QuizQuestionDropDownModel quizQuestionDropDownModel = new QuizQuestionDropDownModel
                    {
                        QuizID = Convert.ToInt32(row["QuizID"]),
                        QuizName = row["QuizName"].ToString()
                    };
                    quizQuestionList.Add(quizQuestionDropDownModel);
                }
            }

            ViewBag.quizQuestionList = quizQuestionList;

            // If a quiz is selected, fetch the questions for that quiz
            if (QuizID != null && QuizID > 0)
            {
                List<QuizQuestionModel> quizQuestions = new List<QuizQuestionModel>();

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Quiz_QuestionList_Select";
                    command.Parameters.AddWithValue("@QuizID", QuizID);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        QuizQuestionModel quizQuestionModel = new QuizQuestionModel
                        {
                            QuizID = Convert.ToInt32(row["QuizID"]),
                            QuestionID = Convert.ToInt32(row["QuestionID"]),
                            QuestionText = row["QuestionText"].ToString(),
                            OptionA = row["OptionA"].ToString(),
                            OptionB = row["OptionB"].ToString(),
                            OptionC = row["OptionC"].ToString(),
                            OptionD = row["OptionD"].ToString(),
                            CorrectOption = row["CorrectOption"].ToString(),
                            QuestionMarks = Convert.ToInt32(row["QuestionMarks"])
                        };
                        quizQuestions.Add(quizQuestionModel);
                    }
                }

                // Return the questions to the view
                return View("QuizQuestion_List", quizQuestions);
            }

            // No quiz selected, return empty view
            return View("QuizQuestion_List", new List<QuizQuestionModel>());
        }

        #endregion


        // Clear action to reset the form and remove fetched questions
        #region ClearQuizData
        public IActionResult ClearQuizData()
        {
            return RedirectToAction("GetQuizData");
        }

        #endregion
    }
}
