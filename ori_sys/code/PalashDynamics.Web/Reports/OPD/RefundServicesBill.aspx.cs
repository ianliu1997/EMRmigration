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

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class RefundServicesBill : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            long RefundID = 0;
            long UnitID = 0;
            long IsFromIPD = 0;
            DateTime PrintDate = DateTime.Now;
            myDoc = new ReportDocument();
            if (Request.QueryString["PrintFomatID"] == "1")
            {
                myDoc.Load(Server.MapPath("RefundServicesBillReport.rpt"));
            }
            else if (Request.QueryString["PrintFomatID"] == "2")
            {
                myDoc.Load(Server.MapPath("RefundServicesBillReportWithoutHeader.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("RefundServicesBillReport.rpt"));
            }            
            if (Request.QueryString["IsFromIPD"] != null)
            {
                IsFromIPD = Convert.ToInt64(Request.QueryString["IsFromIPD"]);
            }
            if (Request.QueryString["RefundID"] != null)
            {
                RefundID = Convert.ToInt64(Request.QueryString["RefundID"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["PrintDate"] != null)
            {
                PrintDate = Convert.ToDateTime(Request.QueryString["PrintDate"]);
            }
            myDoc.SetParameterValue("@RefundID", RefundID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@PrintDate", PrintDate);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt"); //Added By Bhushanp 24032017
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); 
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            MemoryStream oStream = new MemoryStream();
            oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
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