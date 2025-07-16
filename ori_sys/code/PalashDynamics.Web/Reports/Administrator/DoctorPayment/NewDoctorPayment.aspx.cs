using System;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.Administrator.DoctorPayment
{
    public partial class NewDoctorPayment : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();


            myDoc.Load(Server.MapPath("DoctorPaymentReport.rpt"));

            long DoctorID = 0;
            long DoctorPaymentID = 0;
            string DoctorPaymentVoucherNo = null;
            bool Excel = false;
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            long UnitID = 0;

            //long ClinicID = 0;
            if (Request.QueryString["ID"] != null || Request.QueryString["ID"] != "")
            {
                DoctorID = Convert.ToInt64(Request.QueryString["ID"]);
            }
            if (Request.QueryString["UnitID"] != null || Request.QueryString["UnitID"] != "")
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            //if (Request.QueryString["DoctorPaymentID"] != null || Request.QueryString["DoctorPaymentID"] != "")
            //{
            //    DoctorPaymentID = Convert.ToInt64(Request.QueryString["DoctorPaymentID"]);
            //}
            if (Request.QueryString["DoctorVoucherNumber"] != null || Request.QueryString["DoctorVoucherNumber"] != "")
            {
                DoctorPaymentVoucherNo = Convert.ToString(Request.QueryString["DoctorVoucherNumber"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@DoctorID", DoctorID);
         //  myDoc.SetParameterValue("@DoctorPaymentID", DoctorPaymentID);
            myDoc.SetParameterValue("@DoctorPaymentVoucherNo", DoctorPaymentVoucherNo);

            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.AllowedExportFormats = 0;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //if (Excel == true)
            //{
            //    MemoryStream oStream = new MemoryStream();
            //    oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.BinaryWrite(oStream.ToArray());
            //    Response.AddHeader("Sr. no,", "  ");
            //    Response.End();

            //}


        }
        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            myDoc.Dispose();
            CrystalReportViewer1.Dispose();
            GC.Collect();
        }
    }
}