using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.UserModel;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    [CheckAccess]

    public class QuizController : Controller
    {
        private readonly IConfiguration _configuration;

        public QuizController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region QuizAddEdit
        public IActionResult QuizAddEdit(int QuizID)
        {
            if (QuizID == 0)
            {
                TempData["PageTitle"] = "Add Quiz";
            }
            else
            {
                TempData["PageTitle"] = "Edit Quiz";
            }

            ViewBag.QuizID = QuizID;

            //this is for dropdown of user who registerd or login

            string connectionString1 = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection1 = new SqlConnection(connectionString1))
            {
                connection1.Open();
                SqlCommand command1 = connection1.CreateCommand();
                command1.CommandType = CommandType.StoredProcedure;
                command1.CommandText = "PR_MST_User_SelectAll";

                SqlDataReader reader1 = command1.ExecuteReader();
                DataTable dataTable1 = new DataTable();
                dataTable1.Load(reader1);


                List<UserDropDownModel> userList = new List<UserDropDownModel>();

                if (dataTable1.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable1.Rows)
                    {
                        UserDropDownModel userDropDownModel = new UserDropDownModel();
                        userDropDownModel.UserID = Convert.ToInt32(row["UserID"]);
                        userDropDownModel.UserName = row["UserName"].ToString();
                        userList.Add(userDropDownModel);
                    }
                }
                else
                {
                    // Handle case where no users are found
                    userList.Add(new UserDropDownModel { UserID = 0, UserName = "No Users Found" });
                }

                ViewBag.UserList = userList;
            }


            //this is for select data
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_Quiz_SelectByPK";

                command.Parameters.AddWithValue("@QuizID", QuizID);

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                QuizModel quizModel = new QuizModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    quizModel.QuizID = Convert.ToInt32(@row["QuizID"]);
                    quizModel.QuizName = @row["QuizName"].ToString();
                    quizModel.TotalQuestions = Convert.ToInt32(@row["TotalQuestions"]);
                    quizModel.UserID = Convert.ToInt32(@row["UserID"]);
                }
                return View(quizModel);
            }
        }

        #endregion

        #region SaveQuiz
        public IActionResult SaveQuiz(QuizModel quizModel)
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

                        if (quizModel.QuizID <= 0)
                        {
                            command.CommandText = "PR_MST_Quiz_Insert";
                            TempData["Message"] = "Quiz Added Successfully";
                        }
                        else
                        {
                            command.CommandText = "PR_MST_Quiz_UpdateByPK";
                            command.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizModel.QuizID;
                            TempData["Message"] = "Quiz Updated Successfully";
                        }

                        command.Parameters.Add("@QuizName", SqlDbType.VarChar).Value = quizModel.QuizName;
                        command.Parameters.Add("@TotalQuestions", SqlDbType.Int).Value = quizModel.TotalQuestions;
                        command.Parameters.Add("@QuizDate", SqlDbType.DateTime).Value = quizModel.QuizDate;
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = quizModel.UserID;

                        command.ExecuteNonQuery();

                        return RedirectToAction("QuizList");
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000) // Error codes for unique constraint violation (e.g., duplicate entry)
                {
                    TempData["ErrorMessage"] = "A quiz with the same name already exists. Please choose a different name.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while saving the quiz. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
            }

            return View("QuizAddEdit", quizModel);
        }

        #endregion

        #region DetailsQuiz
        public IActionResult DetailsQuiz(int QuizID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_Quiz_SelectByPK";

                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                QuizModel quizModel = new QuizModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    quizModel.QuizID = Convert.ToInt32(@row["QuizID"]);
                    quizModel.QuizName = @row["QuizName"].ToString();
                    quizModel.TotalQuestions = Convert.ToInt32(@row["TotalQuestions"]);
                    quizModel.QuizDate = Convert.ToDateTime(row["QuizDate"]);
                    quizModel.UserID = Convert.ToInt32(@row["UserID"]);
                    //quizModel.UserName = @row["UserName"].ToString();
                    quizModel.Created = Convert.ToDateTime(row["Created"]);
                    quizModel.Modified = Convert.ToDateTime(row["Modified"]);
                }
                return View(quizModel);
            }
        }

        #endregion

        #region QuizDelete
        public IActionResult QuizDelete(int QuizID)
        {
            try
            {
                Console.WriteLine("QuizID to delete: " + QuizID);
                Console.WriteLine("QuizID Mesasage Foreign Message:  Due to foreign key Refrence QuziID d=cannot deleted");

                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_Quiz_DeleteByPK";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;
                    command.ExecuteNonQuery();
                }
                TempData["DeleteMessage"] = "Quiz Deleted SuccessFully";
            }
            catch (Exception E)
            {
                TempData["ErrorMessage"] = "Quiz Can't be deleted";
                Console.WriteLine(E.ToString());
            }

            return RedirectToAction("QuizList");
        }

        #endregion

        #region QuizList
        public IActionResult QuizList()
        {
            string connectionString =this._configuration.GetConnectionString("ConnectionString");
            
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "PR_MST_Quiz_SelectAll";
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

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_Quiz_SelectAll";

                SqlDataReader reader  = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Quiz");
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
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuizList.xlsx");
                    }
                }
            }
        }
        #endregion
    }
}