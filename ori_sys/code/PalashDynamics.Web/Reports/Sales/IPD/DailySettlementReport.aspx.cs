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

namespace PalashDynamics.Web.Reports.Sales.IPD
{
    public partial class DailySettlementReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? FromDate = null;
            DateTime? ToDate = null;

            bool Excel = false;
            long ClinicID = 0;
            long PaymentModeID = 0;
            long BillTypeId = 0;
            long counterID = 0;
            if (Request.QueryString["BillTypeID"] != null)
            {
                BillTypeId = Convert.ToInt64(Request.QueryString["BillTypeID"]);
            }


            ReportDocument myDoc = new ReportDocument();


            if (BillTypeId == 0)
            {
                myDoc.Load(Server.MapPath("rptDailyCollectionDuesReport.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("rptIPDDailyCollectionDuesReport.rpt"));
            }


            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            //if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            //{
            //    ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            //}
            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }


            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            if (Request.QueryString["counterID"] != null)
            {
                counterID = Convert.ToInt64(Request.QueryString["counterID"]);
            }

            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);

            myDoc.SetParameterValue("@ClinicID", ClinicID);
            //    myDoc.SetParameterValue("@PaymentModeID", PaymentModeID);
            myDoc.SetParameterValue("@BillTypeId", BillTypeId);


            myDoc.SetParameterValue("@FromDate", FromDate, "refundAmount");
            myDoc.SetParameterValue("@ToDate", ToDate, "refundAmount");
            myDoc.SetParameterValue("@ClinicID", ClinicID, "refundAmount");
            myDoc.SetParameterValue("@counterID", counterID, "refundAmount");
            myDoc.SetParameterValue("@counterID", counterID);


            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (Excel == true)
            {
                MemoryStream oStream = new MemoryStream();
                oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(oStream.ToArray());
                Response.End();

            }


        }
    }
}