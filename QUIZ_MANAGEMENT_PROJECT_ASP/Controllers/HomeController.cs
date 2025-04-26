using Microsoft.AspNetCore.Mvc;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region Index
        public IActionResult Index()
        {
            int UserCount = GetUserCount();
            int QuizCount = GetQuizCount();
            int QuestionCount = GetQuestionCount();
            int QuestionLevelCount = GetQuestionLevelCount();
            int QuizWiseQuestionCount = GetQuizWiseQuestionCount();

            // Pass counts to the view via ViewData
            ViewData["UserCount"] = UserCount;
            ViewData["QuizCount"] = QuizCount;
            ViewData["QuestionCount"] = QuestionCount;
            ViewData["QuestionLevelCount"] = QuestionLevelCount;
            ViewData["QuizWiseQuestionCount"] = QuizWiseQuestionCount;
            return View();
        }

        #endregion

        #region GetUserCount
        public int GetUserCount()
        {
            int count = 0;
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_User_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            count = table.Rows.Count;


            return count;
        }

        #endregion

        #region GetQuizCount
        public int GetQuizCount()
        {
            int count = 0;
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_Quiz_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            count = table.Rows.Count;

            return count;
        }

        #endregion

        #region GetQuestionCount
        public int GetQuestionCount()
        {
            int count = 0;
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_Question_SelectALL";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            count = table.Rows.Count;


            return count;
        }

        #endregion

        #region GetQuestionLevelCount
        public int GetQuestionLevelCount()
        {
            int count = 0;
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_QuestionLevel_SelectALL";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            count = table.Rows.Count;


            return count;
        }

        #endregion

        #region GetQuizWiseQuestionCount
        public int GetQuizWiseQuestionCount()
        {
            int count = 0;
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST__QuizWiseQuestions_SelectALL";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            count = table.Rows.Count;


            return count;
        }

        #endregion

        #region Profile
        public IActionResult Profile()
        {
            return View();
        }

        #endregion
    }
}