
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace DineBook.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings[
                    "DineBookConnection"].ConnectionString;

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query = @"SELECT UserID, Name, Email,
                                [Password], Role
                                FROM Users
                                WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue(
                        "@Email", txtEmail.Text.Trim());

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblMessage.Text =
                                "Invalid email or password.";
                            return;
                        }

                        string storedPassword =
                            reader["Password"].ToString();

                        string[] parts =
                            storedPassword.Split(':');

                        if (parts.Length != 2)
                        {
                            lblMessage.Text =
                                "Invalid email or password.";
                            return;
                        }

                        byte[] salt =
                            Convert.FromBase64String(parts[0]);

                        byte[] savedHash =
                            Convert.FromBase64String(parts[1]);

                        byte[] enteredHash;

                        using (var pbkdf2 =
                            new Rfc2898DeriveBytes(
                                txtPassword.Text,
                                salt,
                                100000))
                        {
                            enteredHash =
                                pbkdf2.GetBytes(32);
                        }

                        bool valid = savedHash.Length ==
                                     enteredHash.Length;

                        int difference = 0;

                        if (valid)
                        {
                            for (int i = 0;
                                 i < savedHash.Length; i++)
                            {
                                difference |=
                                    savedHash[i] ^ enteredHash[i];
                            }

                            valid = difference == 0;
                        }

                        if (!valid)
                        {
                            lblMessage.Text =
                                "Invalid email or password.";
                            return;
                        }

                        Session["UserID"] =
                            Convert.ToInt32(reader["UserID"]);

                        Session["UserName"] =
                            reader["Name"].ToString();

                        Session["UserEmail"] =
                            reader["Email"].ToString();

                        Session["Role"] =
                            reader["Role"].ToString();

                        string role =
                            reader["Role"].ToString();

                        if (role == "Admin")
                        {
                            Response.Redirect(
                                "~/Admin/Dashboard.aspx");
                        }
                        else
                        {
                            Response.Redirect(
                                "~/User/Dashboard.aspx");
                        }
                    }
                }
            }
        }
    }
}
