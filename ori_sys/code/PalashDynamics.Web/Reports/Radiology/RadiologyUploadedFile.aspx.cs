using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace PalashDynamics.Web.Reports
{
    public partial class RadiologyUploadedFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Response.TransmitFile(Request.QueryString["Report"]);

            string pdfPath = Server.MapPath(Request.QueryString["Report"]);
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(pdfPath);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", buffer.Length.ToString());
            Response.BinaryWrite(buffer);
       
            
            //Convert.ToInt64(Request.QueryString["ID"]);
        }
    }
}