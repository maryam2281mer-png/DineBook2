
using System;

namespace DineBook
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLoggedIn = Session["UserID"] != null;
            string role = Session["Role"] as string;

            lnkLogin.Visible = !isLoggedIn;
            lnkRegister.Visible = !isLoggedIn;

            lnkReservations.Visible =
                isLoggedIn && role == "User";

            lnkAdmin.Visible = !isLoggedIn;
        }
    }
}
