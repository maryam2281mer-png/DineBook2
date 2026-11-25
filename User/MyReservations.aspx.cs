using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DineBook.User
{
    public partial class MyReservations : System.Web.UI.Page
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
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadReservations();
            }
        }

        private void LoadReservations()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            string query = @"
                SELECT
                    S.ServiceName,
                    A.AppointmentDate,
                    A.AppointmentTime,
                    A.NumberOfGuests,
                    A.SpecialRequest,
                    A.Status
                FROM Appointments A
                INNER JOIN Services S ON A.ServiceID = S.ServiceID
                WHERE A.UserID = @UserID
                ORDER BY A.AppointmentDate DESC, A.AppointmentTime DESC";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gvReservations.DataSource = dt;
                gvReservations.DataBind();

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text = "You have no reservations yet.";
                }
                else
                {
                    lblMessage.Text = "";
                }
            }
        }
    }
}
