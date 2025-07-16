using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;


namespace PalashDynamics.Web.Reports.IVF
{
    public partial class PGDPGSReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath;
            string strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            String strReportName = "PGDPGSdetails";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
            string FromDate = null;
            string ToDate = null;
            string ToDatePrint = null;

            DateTime? FrDT = null;
            DateTime? ToDT = null;
            DateTime? ToDP = null;
            long clinicID = 0;
            string AppPath = Request.ApplicationPath;
            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPGDPGSReport.rpt"));
            FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            ToDate = Convert.ToString(Request.QueryString["ToDate"]);
            if (!string.IsNullOrEmpty(FromDate))
            {
                FrDT = Convert.ToDateTime(FromDate);
            }
            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDT = Convert.ToDateTime(ToDate);
                // ToDP = ToDT;
            }
            if (Request.QueryString["ClinicID"] != null)
            {
                clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            myDoc.SetParameterValue("@FromDate", FrDT);
            myDoc.SetParameterValue("@ToDate", ToDT);
            myDoc.SetParameterValue("@ToDatePrint", ToDT);
            myDoc.SetParameterValue("@ClinicID", clinicID);
            myDoc.SetParameterValue("@SendClinicID", clinicID);
            myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt");
            // myDoc.SetParameterValue("@UnitID", lUnitId, "Header_SubReport");

            myDoc.SetParameterValue("@UnitID", clinicID, "rptUnitLogo.rpt");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();


            //Stream oStream = null;
            //byte[] byteArray = null;
            //oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //byteArray = new byte[oStream.Length];
            //oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(byteArray);
            //Response.End();


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