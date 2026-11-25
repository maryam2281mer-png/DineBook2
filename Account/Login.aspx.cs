
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace DineBook.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            string cs = ConfigurationManager
                .ConnectionStrings["DineBookConnection"]
                .ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT UserID, Name, Email,
                    Password, Role FROM Users WHERE Email=@Email";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword =
                                reader["Password"].ToString();

                            if (VerifyPassword(password, storedPassword))
                            {
                                Session["UserID"] = reader["UserID"];
                                Session["UserName"] = reader["Name"];
                                Session["UserEmail"] = reader["Email"];
                                Session["Role"] = reader["Role"];

                                if (reader["Role"].ToString() == "Admin")
                                    Response.Redirect("~/Admin/Dashboard.aspx");
                                else
                                    Response.Redirect("~/User/Dashboard.aspx");
                            }
                            else
                            {
                                ShowError();
                            }
                        }
                        else
                        {
                            ShowError();
                        }
                    }
                }
            }
        }

        private bool VerifyPassword(string password, string stored)
        {
            try
            {
                string[] parts = stored.Split(':');

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expectedHash = Convert.FromBase64String(parts[1]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password, salt, 100000))
                {
                    byte[] actualHash = pbkdf2.GetBytes(32);

                    return CryptographicOperations.FixedTimeEquals(
                        actualHash, expectedHash);
                }
            }
            catch
            {
                return false;
            }
        }

        private void ShowError()
        {
            lblMessage.Text = "Invalid email or password.";
            lblMessage.ForeColor =
                System.Drawing.Color.Red;
        }
    }
}
