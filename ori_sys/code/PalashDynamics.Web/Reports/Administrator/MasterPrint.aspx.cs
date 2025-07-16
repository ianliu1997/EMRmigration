using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
namespace PalashDynamics.Web.Reports.Administrator
{
    public partial class MasterPrint : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {

            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long id = 0;
            if (Request.QueryString["id"] != null)
            {
                id = Convert.ToInt64(Request.QueryString["id"]);
            }
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptMasterPrint.rpt"));
            myDoc.SetParameterValue("@id", id);
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            MemoryStream oStream = new MemoryStream();
            oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();
        }
        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            try
            {
                if (myDoc != null)
                    myDoc.Dispose();
                CrystalReportViewer1.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}