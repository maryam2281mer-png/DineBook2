
using System;

namespace DineBook.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null ||
                Session["Role"] == null ||
                Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/Admin/AdminLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblAdminName.Text = Server.HtmlEncode(
                    Session["UserName"] == null
                    ? "Admin"
                    : Session["UserName"].ToString()
                );
            }
        }
    }
}
