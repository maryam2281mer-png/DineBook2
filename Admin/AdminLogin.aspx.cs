using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace DineBook.Admin
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DineBookConnection"].ConnectionString;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] != null &&
                Session["Role"].ToString() == "Admin")
            {
                Response.Redirect("~/Admin/Dashboard.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string query = @"
                SELECT UserID, Name, Email, Password, Role
                FROM Users
                WHERE Email = @Email AND Role = 'Admin'";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value =
                    txtEmail.Text.Trim();

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedPassword = reader["Password"].ToString();

                        if (VerifyPassword(txtPassword.Text, storedPassword))
                        {
                            Session["UserID"] = Convert.ToInt32(reader["UserID"]);
                            Session["UserName"] = reader["Name"].ToString();
                            Session["UserEmail"] = reader["Email"].ToString();
                            Session["Role"] = "Admin";

                            Response.Redirect("~/Admin/Dashboard.aspx");
                            return;
                        }
                    }
                }
            }

            lblMessage.Text = "Invalid admin email or password.";
        }

        private bool VerifyPassword(string password, string storedPassword)
        {
            try
            {
                string[] parts = storedPassword.Split(':');

                if (parts.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] savedHash = Convert.FromBase64String(parts[1]);

                using (Rfc2898DeriveBytes pbkdf2 =
                    new Rfc2898DeriveBytes(password, salt, 100000))
                {
                    byte[] enteredHash = pbkdf2.GetBytes(32);

                    if (enteredHash.Length != savedHash.Length)
                        return false;

                    int difference = 0;

                    for (int i = 0; i < enteredHash.Length; i++)
                    {
                        difference |= enteredHash[i] ^ savedHash[i];
                    }

                    return difference == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
