using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.QuestionModel;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.QuizModel;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.UserModel;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    [CheckAccess]
    public class QuizWiseQuestionController : Controller
    {
        private readonly IConfiguration _configuration;

        //constructor
        public QuizWiseQuestionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region AddEditQuizWiseQuestion
        public IActionResult AddEditQuizWiseQuestion(int QuizWiseQuestionID)
        {

            if (QuizWiseQuestionID == 0)
            {
                TempData["PageTitle"] = "Add QuizWiseQuestion";
            }
            else
            {
                TempData["PageTitle"] = "Edit QuizWiseQuestion";
            }

            ViewBag.QuizWiseQuestionID = QuizWiseQuestionID;

            string connectionString1 = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection1 = new SqlConnection(connectionString1))
            {
                connection1.Open();

                SqlCommand command1 = connection1.CreateCommand();
                command1.CommandType = CommandType.StoredProcedure;
                command1.CommandText = "PR_MST_User_SelectAll";

                SqlDataReader reader1 = command1.ExecuteReader();

                DataTable dataTable1 = new DataTable();
                dataTable1.Load(reader1);

                List<UserDropDownModel> userList = new List<UserDropDownModel>();


                foreach (DataRow row in dataTable1.Rows)
                {
                    UserDropDownModel userDropDownModel = new UserDropDownModel();
                    userDropDownModel.UserID = Convert.ToInt32(@row["UserID"]);
                    userDropDownModel.UserName = @row["UserName"].ToString();
                    userList.Add(userDropDownModel);
                }
                ViewBag.UserList = userList;
            }


            using(SqlConnection connection2 = new SqlConnection(connectionString1))
            {
                connection2.Open();

                SqlCommand command2 = connection2.CreateCommand();
                command2.CommandType = CommandType.StoredProcedure;
                command2.CommandText = "PR_MST_Quiz_SelectAll";

                SqlDataReader reader = command2.ExecuteReader();

                DataTable dataTable2 = new DataTable();
                dataTable2.Load(reader);

                List<QuizDropDownModel> quizList = new List<QuizDropDownModel>();

                foreach (DataRow row in dataTable2.Rows)
                {
                    QuizDropDownModel quizDropDownModel = new QuizDropDownModel();
                    quizDropDownModel.QuizID = Convert.ToInt32(row["QuizID"]);
                    quizDropDownModel.QuizName = row["QuizName"].ToString();
                    quizList.Add(quizDropDownModel);
                }
                ViewBag.QuizList = quizList;
            }


            using (SqlConnection connection3 = new SqlConnection(connectionString1))
            {
                connection3.Open();

                SqlCommand command3 = connection3.CreateCommand();
                command3.CommandType = CommandType.StoredProcedure;
                command3.CommandText = "PR_MST_Question_SelectALL";

                SqlDataReader reader3 = command3.ExecuteReader();

                DataTable dataTable3 = new DataTable();
                dataTable3.Load(reader3);

                List<QuestionDropDownModel> questionList = new List<QuestionDropDownModel>();

                foreach(DataRow row in dataTable3.Rows)
                {
                    QuestionDropDownModel questionDropDownModel = new QuestionDropDownModel();
                    questionDropDownModel.QuestionID = Convert.ToInt32(row["QuestionID"]);
                    questionDropDownModel.QuestionText = row["QuestionText"].ToString();
                    questionList.Add(questionDropDownModel);
                }
                ViewBag.QuestionList = questionList;
            }

            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_QuizWiseQuestions_SelectByPK";

                command.Parameters.AddWithValue("@QuizWiseQuestionID", QuizWiseQuestionID);

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                QuizWiseQuestionsModel quizWiseQuestionsModel = new QuizWiseQuestionsModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    quizWiseQuestionsModel.QuizWiseQuestionsID = Convert.ToInt32(row["QuizWiseQuestionsID"]);
                    quizWiseQuestionsModel.QuizID = Convert.ToInt32(row["QuizID"]);
                    quizWiseQuestionsModel.QuestionID = Convert.ToInt32(row["QuestionID"]);
                    quizWiseQuestionsModel.UserID = Convert.ToInt32(row["UserID"]);
                }
                return View(quizWiseQuestionsModel); 
            }
        }

        #endregion

        #region SaveQuizWiseQuestion
        public IActionResult SaveQuizWiseQuestion(QuizWiseQuestionsModel quizWiseQuestionsModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = connection.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;

                        if (quizWiseQuestionsModel.QuizWiseQuestionsID <= 0)
                        {
                            command.CommandText = "PR_MST_QuizWiseQuestions_INSERT";
                            TempData["Message"] = "QuizWise Question Added Successfully";
                        }
                        else
                        {
                            command.CommandText = "PR_MST_QuizWiseQuestions_UPDATE";
                            command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = quizWiseQuestionsModel.QuizWiseQuestionsID;
                            TempData["Message"] = "QuizWise Question Updated Successfully";
                        }
                        command.Parameters.Add("@UserID",SqlDbType.Int).Value = quizWiseQuestionsModel.UserID;
                        command.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizWiseQuestionsModel.QuizID;
                        command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = quizWiseQuestionsModel.QuestionID;

                        command.ExecuteNonQuery();

                        return RedirectToAction("QuizWiseQuestionList");
                    }
                }
                return View("AddEditQuizWiseQuestion", quizWiseQuestionsModel);
            }
            catch (Exception E)
            {
                TempData["ErrorMessage"] = "You Can Not Add More Question To This Quiz You have Reached The Question Limit";
                Console.WriteLine(E.ToString());
                return RedirectToAction("AddEditQuizWiseQuestion");
            }
        }

        #endregion

        #region DetailsQuizWiseQuestion
        public IActionResult DetailsQuizWiseQuestion(int QuizWiseQuestionID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_QuizWiseQuestions_SelectByPK";

                command.Parameters.AddWithValue("@QuizWiseQuestionID", QuizWiseQuestionID);

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                QuizWiseQuestionsModel quizWiseQuestionsModel = new QuizWiseQuestionsModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    quizWiseQuestionsModel.QuizWiseQuestionsID = Convert.ToInt32(row["QuizWiseQuestionsID"]);
                    quizWiseQuestionsModel.QuizID = Convert.ToInt32(row["QuizID"]);
                    quizWiseQuestionsModel.QuestionID = Convert.ToInt32(row["QuestionID"]);
                    quizWiseQuestionsModel.UserID = Convert.ToInt32(row["UserID"]);
                    quizWiseQuestionsModel.Created = Convert.ToDateTime(row["Created"]);
                    quizWiseQuestionsModel.Modified = Convert.ToDateTime(row["Modified"]);
                }
                return View(quizWiseQuestionsModel);
            }
        }

        #endregion

        #region DeleteQuizWiseQuestion
        public IActionResult DeleteQuizWiseQuestion(int QuizWiseQuestionsID)
        {
            try
            {
                Console.WriteLine("QuizWiseQuestionsID to delete: " + QuizWiseQuestionsID);
                //Console.WriteLine("QuizID Mesasage Foreign Message:  Due to foreign key Refrence QuziID d=cannot deleted");

                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_QuizWiseQuestions_DELETE";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = QuizWiseQuestionsID;
                    command.ExecuteNonQuery();
                }   
                TempData["DeleteMessage"] = "QuizWiseQuestion Deleted SuccessFully";
            }
            catch (Exception E)
            {
                TempData["ErrorMessage"] = "Quiz Wise Question Can't be deleted";
                Console.WriteLine(E.ToString());
            }

            return RedirectToAction("QuizWiseQuestionList");
        }

        #endregion

        #region QuizWiseQuestionList
        public IActionResult QuizWiseQuestionList()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "PR_MST__QuizWiseQuestions_SelectALL";
                SqlDataReader reader = command.ExecuteReader();
                
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                return View(dataTable);
            }
        }

        #endregion

        #region ExportToExcel

        public IActionResult ExportToExcel()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST__QuizWiseQuestions_SelectALL";

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("QuizWiseQuestion");
                    var currentRow = 1;

                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[currentRow, i + 1].Value = dataTable.Columns[i].ColumnName;
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        currentRow++;
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            worksheet.Cells[currentRow, i + 1].Value = row[i];
                        }
                    }

                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuizWiseQuestion.xlsx");
                    }
                }
            }
        }

        #endregion
    }
}
