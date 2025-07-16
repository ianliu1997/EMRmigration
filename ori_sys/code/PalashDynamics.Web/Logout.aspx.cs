using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PalashDynamics.ValueObjects;
using PalashDynamics.BusinessLayer;


namespace PalashDynamics.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Session.Abandon();
            Session["UserID"] = null;
            Response.Write(Session["UserID"]);
            Session.Clear();

            Response.Redirect("Login.aspx");
          // Response.Redirect("PalashDynamicsTestPage.aspx");
        }
    }
}