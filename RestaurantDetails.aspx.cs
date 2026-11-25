using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DineBook
{
    public partial class RestaurantDetails : System.Web.UI.Page
    {
        private string ConnectionString =>
            ConfigurationManager.ConnectionStrings[
                "DineBookConnection"].ConnectionString;

        private int ServiceID
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id)
                    ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ServiceID == 0)
            {
                Response.Redirect("~/Restaurants.aspx");
                return;
            }

            if (!IsPostBack)
                LoadRestaurant();
        }

        private void LoadRestaurant()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Services
                                 WHERE ServiceID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ServiceID;
                    con.Open();

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            lblName.Text = Server.HtmlEncode(
                                r["ServiceName"].ToString());

                            lblDescription.Text = Server.HtmlEncode(
                                r["Description"].ToString());

                            lblPrice.Text = Convert.ToDecimal(
                                r["Price"]).ToString("N2");

                            imgRestaurant.ImageUrl =
                                r["ServicePhoto"].ToString();
                        }
                        else
                        {
                            Response.Redirect("~/Restaurants.aspx");
                        }
                    }
                }
            }
        }

        protected void btnLoadTimes_Click(object sender, EventArgs e)
        {
            DateTime selectedDate;

            if (!DateTime.TryParseExact(
                txtDate.Text, "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out selectedDate)
                || selectedDate.Date < DateTime.Today)
            {
                ShowMessage("Please choose a valid future date.");
                return;
            }

            ddlTime.Items.Clear();
            ddlTime.Items.Add(
                new System.Web.UI.WebControls.ListItem(
                    "Choose a time", ""));

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT AvailableTime
                    FROM Availability
                    WHERE ServiceID = @ID
                    AND AvailableDate = @Date
                    ORDER BY AvailableTime";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ServiceID;
                    cmd.Parameters.Add("@Date", SqlDbType.Date).Value =
                        selectedDate.Date;

                    con.Open();

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            TimeSpan time = (TimeSpan)r["AvailableTime"];

                            ddlTime.Items.Add(
                                time.ToString(@"hh\:mm"));
                        }
                    }
                }
            }

            if (ddlTime.Items.Count == 1)
                ShowMessage("No available times for this date.");
            else
                ShowMessage("Available times loaded.");
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            DateTime date;
            TimeSpan time;
            int guests;

            if (!DateTime.TryParseExact(
                txtDate.Text, "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date)
                || date.Date < DateTime.Today
                || !TimeSpan.TryParse(ddlTime.SelectedValue, out time)
                || !int.TryParse(ddlGuests.SelectedValue, out guests))
            {
                ShowMessage("Please select a valid date, time and guest count.");
                return;
            }

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                string checkQuery = @"SELECT COUNT(*) FROM Availability
                    WHERE ServiceID=@ID
                    AND AvailableDate=@Date
                    AND AvailableTime=@Time";

                using (SqlCommand check = new SqlCommand(checkQuery, con))
                {
                    check.Parameters.Add("@ID", SqlDbType.Int).Value = ServiceID;
                    check.Parameters.Add("@Date", SqlDbType.Date).Value = date.Date;
                    check.Parameters.Add("@Time", SqlDbType.Time).Value = time;

                    if ((int)check.ExecuteScalar() == 0)
                    {
                        ShowMessage("This time is no longer available.");
                        return;
                    }
                }

                string query = @"INSERT INTO Appointments
                    (UserID, ServiceID, AppointmentDate,
                     AppointmentTime, NumberOfGuests,
                     SpecialRequest, Status)
                    VALUES
                    (@UserID, @ServiceID, @Date,
                     @Time, @Guests, @Request, 'Confirmed')";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value =
                        Convert.ToInt32(Session["UserID"]);

                    cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value =
                        ServiceID;

                    cmd.Parameters.Add("@Date", SqlDbType.Date).Value = date.Date;
                    cmd.Parameters.Add("@Time", SqlDbType.Time).Value = time;
                    cmd.Parameters.Add("@Guests", SqlDbType.Int).Value = guests;

                    cmd.Parameters.Add("@Request", SqlDbType.NVarChar, 500).Value =
                        string.IsNullOrWhiteSpace(txtRequest.Text)
                            ? (object)DBNull.Value
                            : txtRequest.Text.Trim();

                    cmd.ExecuteNonQuery();
                }
            }

            Response.Redirect("~/User/MyReservations.aspx");
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = Server.HtmlEncode(message);
        }
    }
}
