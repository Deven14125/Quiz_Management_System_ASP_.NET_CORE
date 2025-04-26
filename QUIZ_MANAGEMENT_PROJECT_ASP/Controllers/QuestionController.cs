using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.QuestionLevelModel;
using static QUIZ_MANAGEMENT_PROJECT_ASP.Models.UserModel;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    [CheckAccess]

    public class QuestionController : Controller
    {
        private readonly IConfiguration _configuration;
        public QuestionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region QuestionAddEdit
        public IActionResult QuestionAddEdit(int QuestionID)
        {
            if (QuestionID == 0)
            {
                TempData["PageTitle"] = "Add Question";
            }
            else
            {
                TempData["PageTitle"] = "Edit Question";
            }

            ViewBag.QuestionID = QuestionID;

            string connectionString1 = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection1 = new SqlConnection(connectionString1))
            {
                connection1.Open();

                SqlCommand command1  = connection1.CreateCommand();
                command1.CommandType = System.Data.CommandType.StoredProcedure;
                command1.CommandText = "PR_MST_User_SelectAll";

                SqlDataReader reader1 = command1.ExecuteReader();

                DataTable dataTable1  = new DataTable();
                dataTable1.Load(reader1);

                List<UserDropDownModel> userList = new List<UserDropDownModel>();

                foreach (DataRow row in dataTable1.Rows)
                {
                    UserDropDownModel userDropDownModel     = new UserDropDownModel();
                    userDropDownModel.UserID                = Convert.ToInt32(@row["UserID"]);
                    userDropDownModel.UserName              = @row["UserName"].ToString();
                    userList.Add(userDropDownModel);
                }

                ViewBag.UserList = userList;
            }

            string connectionString2 = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection2   = new SqlConnection(connectionString2))
            {
                connection2.Open();

                SqlCommand command2 = connection2.CreateCommand();
                command2.CommandType = CommandType.StoredProcedure;
                command2.CommandText = "PR_MST_QuestionLevel_SelectALL";

                SqlDataReader reader2 = command2.ExecuteReader();

                DataTable dataTable2 = new DataTable();
                dataTable2.Load(reader2);

                List<QuestionLevelDropDownModel> questionLevelList = new List<QuestionLevelDropDownModel>();

                foreach (DataRow row in dataTable2.Rows)
                {
                    QuestionLevelDropDownModel questionLevelDropDownModel   = new QuestionLevelDropDownModel();
                    questionLevelDropDownModel.QuestionLevelID              = Convert.ToInt32(row["QuestionLevelID"]);
                    questionLevelDropDownModel.QuestionLevel                = row["QuestionLevel"].ToString();
                    questionLevelList.Add(questionLevelDropDownModel);
                }
                ViewBag.QuestionLevelList = questionLevelList;
            }

            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command  = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Question_SelectByPK";

                command.Parameters.AddWithValue("@QuestionID", QuestionID);

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                QuestionModel questionModel = new QuestionModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    questionModel.QuestionID         = Convert.ToInt32(row["QuestionID"]);
                    questionModel.QuestionText       = row["QuestionText"].ToString();
                    questionModel.CorrectOption      = row["CorrectOption"].ToString();
                    questionModel.QuestionMarks      = Convert.ToInt32(row["QuestionMarks"]);
                    questionModel.OptionA            = row["OptionA"].ToString(); 
                    questionModel.OptionB            = row["OptionB"].ToString();
                    questionModel.OptionC            = row["OptionC"].ToString();
                    questionModel.OptionD            = row["OptionD"].ToString();
                    questionModel.IsActive           = Convert.ToBoolean(row["IsActive"]);
                    questionModel.UserID             = Convert.ToInt32(row["UserID"]);
                    questionModel.QuestionLevelID    = Convert.ToInt32(row["QuestionLevelID"]);
                }
                return View(questionModel);
            }
        }

        #endregion

        #region SaveQuestion
        public IActionResult SaveQuestion(QuestionModel questionModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command  = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    
                    if (questionModel.QuestionID <= 0)
                    {
                        command.CommandText = "PR_Question_Insert";
                        TempData["Message"] = "Question Added SuccessFully";
                    }
                    else
                    {
                        command.CommandText = "PR_Question_UpdateByPK";
                        command.Parameters.Add("@QuestionID",SqlDbType.Int).Value = questionModel.QuestionID;
                        command.Parameters.Add("@IsActive",SqlDbType.Bit).Value   = questionModel.IsActive;
                        TempData["Message"] = "Question Updated SuccessFully";
                    }
                    command.Parameters.Add("@QuestionText",SqlDbType.VarChar).Value   = questionModel.QuestionText;
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value   = questionModel.QuestionLevelID;
                    command.Parameters.Add("@CorrectOption",SqlDbType.VarChar).Value  = questionModel.CorrectOption;
                    command.Parameters.Add("@QuestionMarks", SqlDbType.Int).Value     = questionModel.QuestionMarks;
                    command.Parameters.Add("@OptionA", SqlDbType.VarChar).Value       = questionModel.OptionA;
                    command.Parameters.Add("@OptionB", SqlDbType.VarChar).Value       = questionModel.OptionB;
                    command.Parameters.Add("@OptionC", SqlDbType.VarChar).Value       = questionModel.OptionC;
                    command.Parameters.Add("@OptionD", SqlDbType.VarChar).Value       = questionModel.OptionD;
                    command.Parameters.Add("@UserID",SqlDbType.Int).Value             = questionModel.UserID;
                    command.ExecuteNonQuery();

                    return RedirectToAction("QuestionList");
                }
            }
            return View("QuestionAddEdit",questionModel);
        }

        #endregion

        #region DetailsQuestion
        public IActionResult DetailsQuestion(int QuestionID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Question_SelectByPK";

                command.Parameters.AddWithValue("@QuestionID", QuestionID);

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                QuestionModel questionModel = new QuestionModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    questionModel.QuestionID = Convert.ToInt32(row["QuestionID"]);
                    questionModel.QuestionText = row["QuestionText"].ToString();
                    questionModel.CorrectOption = row["CorrectOption"].ToString();
                    questionModel.QuestionMarks = Convert.ToInt32(row["QuestionMarks"]);
                    questionModel.OptionA = row["OptionA"].ToString();
                    questionModel.OptionB = row["OptionB"].ToString();
                    questionModel.OptionC = row["OptionC"].ToString();
                    questionModel.OptionD = row["OptionD"].ToString();
                    questionModel.IsActive = Convert.ToBoolean(row["IsActive"]);
                    questionModel.UserID = Convert.ToInt32(row["UserID"]);
                    questionModel.QuestionLevelID = Convert.ToInt32(row["QuestionLevelID"]);
                    questionModel.Created = Convert.ToDateTime(row["Created"]);
                    questionModel.Modified = Convert.ToDateTime(row["Modified"]);
                }
                return View(questionModel);
            }
        }

        #endregion

        #region QuestionDelete
        public IActionResult QuestionDelete(int QuestionID)
        {
            try
            {
                Console.WriteLine("QuestionID to delete: " + QuestionID);
                //Console.WriteLine("QuizID Mesasage Foreign Message:  Due to foreign key Refrence QuziID d=cannot deleted");

                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command  = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Questions_DeleteByPK";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;
                    command.ExecuteNonQuery();
                }

                TempData["DeleteMessage"] = "Question Deleted SuccessFully";
            }
            catch (Exception E)
            {
                TempData["ErrorMessage"] = "Question Can't be deleted";
                Console.WriteLine(E.ToString());
            }

            return RedirectToAction("QuestionList");
        }

        #endregion

        #region QuestionList
        public IActionResult QuestionList()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command   = connection.CreateCommand();
                command.CommandType  = System.Data.CommandType.StoredProcedure;
                command.CommandText  = "PR_MST_Question_SelectALL";
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
                command.CommandText = "PR_MST_Question_SelectALL";

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Question");
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
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuestionList.xlsx");
                    }
                }
            }
        }

        #endregion

        #region ExportToJpeg

        public IActionResult ExportToJpeg()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_Question_SelectALL";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                int columnWidth = 200; // Set a fixed column width
                int rowHeight = 70; // Set row height
                int width = columnWidth * dataTable.Columns.Count; // Calculate the total width
                int height = (dataTable.Rows.Count + 1) * rowHeight; // Calculate the total height

                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.FillRectangle(Brushes.White, 0, 0, width, height);
                        Font font = new Font("Arial", 12);
                        Brush textBrush = Brushes.Black;
                        Pen gridPen = new Pen(Color.Black);

                        int currentY = 0;

                        // Draw the table headers with background color
                        Brush headerBrush = Brushes.LightGray;
                        graphics.FillRectangle(headerBrush, 0, currentY, width, rowHeight);
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            graphics.DrawRectangle(gridPen, i * columnWidth, currentY, columnWidth, rowHeight); // Draw cell border
                            graphics.DrawString(dataTable.Columns[i].ColumnName, font, textBrush, new RectangleF(i * columnWidth, currentY, columnWidth, rowHeight),
                                                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        }
                        currentY += rowHeight;

                        // Draw the table rows with alternating background color for better readability
                        Brush rowBrush1 = Brushes.White;
                        Brush rowBrush2 = Brushes.LightYellow;
                        Brush currentRowBrush;

                        foreach (DataRow row in dataTable.Rows)
                        {
                            currentRowBrush = (currentY / rowHeight) % 2 == 0 ? rowBrush1 : rowBrush2;

                            for (int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                graphics.FillRectangle(currentRowBrush, i * columnWidth, currentY, columnWidth, rowHeight); // Apply background
                                graphics.DrawRectangle(gridPen, i * columnWidth, currentY, columnWidth, rowHeight); // Draw cell border
                                graphics.DrawString(row[i].ToString(), font, textBrush, new RectangleF(i * columnWidth, currentY, columnWidth, rowHeight),
                                                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                            }
                            currentY += rowHeight;
                        }
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return File(stream.ToArray(), "image/jpeg", "QuestionList.jpg");
                    }
                }
            }
        }

        #endregion

    }
}
