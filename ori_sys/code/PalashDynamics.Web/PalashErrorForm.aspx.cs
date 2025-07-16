using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PalashDynamics.Web
{
    public partial class PalashErrorForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = " Activation Incomplete.";
            lblMessage.Visible = true;
        }
    }
}