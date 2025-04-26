using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using QUIZ_MANAGEMENT_PROJECT_ASP.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace QUIZ_MANAGEMENT_PROJECT_ASP.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        public UserController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        #region RegisterUser
        public IActionResult RegisterUser(int UserID)
        {
            if (UserID == 0)
            {
                TempData["PageTitle"] = "Register User Here";
            }
            else
            {
                TempData["PageTitle"] = "Edit User Here";
            }

            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand(); 
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_User_SelectByPK";

                command.Parameters.Add("@UserID",SqlDbType.Int).Value = UserID;
                SqlDataReader reader = command.ExecuteReader();
               
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);


                UserModel userModel = new UserModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    userModel.UserID = Convert.ToInt32(row["UserID"]);
                    userModel.UserName = row["UserName"].ToString();
                    userModel.Password = row["Password"].ToString();
                    userModel.Email = row["Email"].ToString();
                    userModel.Mobile = row["Mobile"].ToString();
                }
                return View(userModel);
            }
        }

        #endregion

        #region LoginUser
        public IActionResult LoginUser()
        {
            return View();
        }

        #endregion

        #region Login
        public IActionResult Login(UserLoginModel userLoginModel)
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
                        command.CommandText = "PR_MST_User_SelectByUsernamePassword";

                        command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                        command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;

                        SqlDataReader reader = command.ExecuteReader();

                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        if (dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                HttpContext.Session.SetString("UserID", row["UserID"].ToString());
                                HttpContext.Session.SetString("UserName", row["UserName"].ToString());
                            }
                            TempData["SuccessLogin"] = "Logged In Successfully";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "UserName or Password Invalid. Please Try Again";
                            return RedirectToAction("LoginUser", "User");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("LoginUser", "User");
            }
            return RedirectToAction("LoginUser", "User");
        }

        #endregion

        #region Logout
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Optionally, set a logout message if needed
            TempData["SuccessLogout"] = "You have logged out successfully.";

            // Redirect to login page or home page after logout
            return RedirectToAction("LoginUser", "User");
        }

        #endregion

        #region SaveUser
        public IActionResult SaveUser(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;

                    if (userModel.UserID <= 0)
                    {
                        command.CommandText = "PR_MST_User_Insert";
                        TempData["Message"] = "User Added SuccessFully";
                    }
                    else
                    {
                        command.CommandText = "PR_MST_User_UpdateByPK";
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;
                        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;
                        command.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = userModel.IsAdmin;
                        TempData["Message"] = "User Updated SuccessFully";
                    }
                    command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                    command.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = userModel.Mobile;

                    command.ExecuteNonQuery();
                    return RedirectToAction("UserList");
                }
            }

            return View("RegisterUser", userModel);
        }

        #endregion

        [CheckAccess]

        #region DetailsUser
        public IActionResult DetailsUser(int UserID)
        {
            //ViewBag.UserID = UserID;

            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_User_SelectByPK";
                
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                UserModel userModel = new UserModel();

                foreach (DataRow row in dataTable.Rows)
                {
                    userModel.UserID = Convert.ToInt32(row["UserID"]);
                    userModel.UserName = row["UserName"].ToString();
                    userModel.Password = row["Password"].ToString();
                    userModel.Email = row["Email"].ToString();
                    userModel.Mobile = row["Mobile"].ToString();
                    userModel.IsActive = Convert.ToBoolean(row["IsActive"]);
                    userModel.IsAdmin = Convert.ToBoolean(row["IsAdmin"]);
                    userModel.Created = Convert.ToDateTime(row["Created"]);
                    userModel.Modified = Convert.ToDateTime(row["Modified"]);
                }
                return View(userModel);
            }
        }

        #endregion

        [CheckAccess]

        #region DeleteUser
        public IActionResult DeleteUser(int UserID)
        {
            try
            {
                Console.WriteLine("UserID to delete: " + UserID);

                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_User_DeleteByPK";
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                    command.Parameters.Add("@IsActive",SqlDbType.Bit).Value = 0;
                    command.ExecuteNonQuery();
                    TempData["DeleteUser"] = "User Deleted Successfully";
                }
            }
            catch (Exception E)
            {
                TempData["ErrorMessage"] = "User Can't be deleted";
                Console.WriteLine(E.ToString());
            }

            return RedirectToAction("UserList");
        }

        #endregion

        [CheckAccess]

        #region UserList
        public IActionResult UserList()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_User_SelectAll";

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
                command.CommandText = "PR_MST_User_SelectAll";

                SqlDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(reader);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("User");
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
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UserList.xlsx");
                    }
                }
            }
        }

        #endregion
    }
}
