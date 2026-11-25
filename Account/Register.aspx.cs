using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace DineBook.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text;

            string hashedPassword = HashPassword(password);

            string connectionString =
                ConfigurationManager.ConnectionStrings[
                    "DineBookConnection"].ConnectionString;

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Users
                        (Name, Email, Password, Phone, Role)
                        VALUES
                        (@Name, @Email, @Password, @Phone, 'User')";

                    using (SqlCommand cmd =
                        new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password",
                            hashedPassword);
                        cmd.Parameters.AddWithValue("@Phone", phone);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.Text =
                    "Account created successfully! You can now log in.";
                lblMessage.ForeColor =
                    System.Drawing.Color.Green;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    lblMessage.Text =
                        "This email is already registered.";
                }
                else
                {
                    lblMessage.Text =
                        "Database error. Please try again.";
                }

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, 100000))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                return Convert.ToBase64String(salt) + ":" +
                       Convert.ToBase64String(hash);
            }
        }
    }
}
