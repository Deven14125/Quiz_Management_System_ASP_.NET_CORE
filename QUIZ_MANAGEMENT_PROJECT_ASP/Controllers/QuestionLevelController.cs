using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.UserModel;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    [CheckAccess]

    public class QuestionLevelController : Controller
    {
        private readonly IConfiguration _configuration; 

        public QuestionLevelController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region AddEditQuestionLevel
        public IActionResult AddEditQuestionLevel(int QuestionLevelID)
        {

            if (QuestionLevelID == 0)
            {
                TempData["PageTitle"] = "Add Question Level";
            }
            else
            {
                TempData["PageTitle"] = "Edit Question Level";
            }

            ViewBag.QuestionLevelID = QuestionLevelID;

            string connectionString1 = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection1 = new SqlConnection(connectionString1))
            {
                connection1.Open();

                SqlCommand command1 = connection1.CreateCommand();
                command1.CommandType = System.Data.CommandType.StoredProcedure;
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
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_QuestionLevel_SelectByPK";

                command.Parameters.AddWithValue("@QuestionLevelID", QuestionLevelID);

                SqlDataReader reader = command.ExecuteReader();
                
                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                QuestionLevelModel questionLevelModel = new QuestionLevelModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    questionLevelModel.QuestionLevelID = Convert.ToInt32(row["QuestionLevelID"]);
                    questionLevelModel.QuestionLevel = row["QuestionLevel"].ToString();
                    questionLevelModel.UserID = Convert.ToInt32(row["UserID"]);
                }
                return View(questionLevelModel);
            }
        }

        #endregion

        #region SaveQuestionLevel
        public IActionResult SaveQuestionLevel(QuestionLevelModel questionLevelModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;

                    if (questionLevelModel.QuestionLevelID <= 0)
                    {
                        command.CommandText = "PR_MST_QuestionLevel_Insert";
                        TempData["Message"] = "QuestionLevel Added Successfully.";
                    }
                    else
                    {
                        command.CommandText = "PR_MST_QuestionLevel_UpdateByPK";
                        command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = questionLevelModel.QuestionLevelID;
                        TempData["Message"] = "QuestionLevel Updated Successfully";
                    }

                    // Common parameters for both Add and Update
                    command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = questionLevelModel.QuestionLevel;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = questionLevelModel.UserID;
                    //command.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                    command.ExecuteNonQuery();

                    return RedirectToAction("ViewQuestionLevelList");
                }
            }

            // If ModelState is not valid, return the same view with the current model
            return View("AddEditQuestionLevel", questionLevelModel);
        }

        #endregion

        #region DetailsQuestionLevel
        public IActionResult DetailsQuestionLevel(int QuestionLevelID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_QuestionLevel_SelectByPK";

                command.Parameters.AddWithValue("@QuestionLevelID", QuestionLevelID);

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                QuestionLevelModel questionLevelModel = new QuestionLevelModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    questionLevelModel.QuestionLevelID = Convert.ToInt32(row["QuestionLevelID"]);
                    questionLevelModel.QuestionLevel = row["QuestionLevel"].ToString();
                    questionLevelModel.UserID = Convert.ToInt32(row["UserID"]);
                    questionLevelModel.Created = Convert.ToDateTime(row["Created"]);
                    questionLevelModel.Modified = Convert.ToDateTime(row["Modified"]);
                }
                return View(questionLevelModel);
            }
        }

        #endregion

        #region DeleteQuestionLevel
        public IActionResult DeleteQuestionLevel(int QuestionLevelID)
        {
            try
            {
                Console.WriteLine("QuestionLevelID Is Deleted: " + QuestionLevelID);

                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_QuestionLevel_DeleteByPK";

                    command.Parameters.Add("@QuestionLevelID",SqlDbType.Int).Value=QuestionLevelID;
                    command.ExecuteNonQuery();
                }
                TempData["DeleteMessage"] = "QuestionLevel Deleted SuccessFully";
            }
            catch (Exception E)
            {
                TempData["ErrorMessage"] = "QuestionLevelID Can't be deleted";
                Console.WriteLine(E.ToString());
            }
            return RedirectToAction("ViewQuestionLevelList");
        }

        #endregion

        #region ViewQuestionLevelList
        public IActionResult ViewQuestionLevelList()    
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();  
                SqlCommand command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "PR_MST_QuestionLevel_SelectALL";
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
                command.CommandText = "PR_MST_QuestionLevel_SelectALL";

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("QuestionLevel");
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
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuestionLevelList.xlsx");
                    }
                }
            }
        }

        #endregion
    }
}
