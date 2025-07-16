using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.Pathology.MIS
{
    public partial class RptMISPathologyOutSource : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {

            string strPath, strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            //String strReportName = "History Report";
            //strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            //strPath = Server.MapPath(".");
            //strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            string FromDate = null;
            string ToDate = null;
            string ToDatePrint = null;
            long lUnitId = 0;

            DateTime? FrDT = null;
            DateTime? ToDT = null;
            DateTime? ToDP = null;

            long clinicID = 0;
            bool IsExporttoExcel = false;
            string AppPath = Request.ApplicationPath;
            string SendClinicID = string.Empty;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("RptMISOutSource.rpt"));
            FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            ToDate = Convert.ToString(Request.QueryString["ToDate"]);
            ToDatePrint = Convert.ToString(Request.QueryString["ToDatePrint"]);
            IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);

            if (!string.IsNullOrEmpty(FromDate))
            {
                FrDT = Convert.ToDateTime(FromDate);
            }

            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDT = Convert.ToDateTime(ToDate);
            }
            if (!string.IsNullOrEmpty(ToDatePrint))
            {
                ToDP = Convert.ToDateTime(ToDatePrint);
            }
            if (Request.QueryString["ClinicID"] != null)
            {
                clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }

            if (Request.QueryString["LoginUnitID"] != null)
            {
                lUnitId = Convert.ToInt64(Request.QueryString["LoginUnitID"]);
            }

            if (Request.QueryString["SendClinicID"] != null)
            {
                SendClinicID = Convert.ToString(Request.QueryString["SendClinicID"]);
            }

            myDoc.SetParameterValue("@FromDate", FrDT);
            myDoc.SetParameterValue("@ToDate", ToDT);
            myDoc.SetParameterValue("@ClinicID", clinicID);

            myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", clinicID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //MemoryStream oStream1 = new MemoryStream();
            //oStream1 = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(oStream1.ToArray());
            //Response.End();

            if (IsExporttoExcel == true)
            {
                System.IO.Stream oStream = null;
                byte[] byteArray = null;
                oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(byteArray);
                Response.End();
            }
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