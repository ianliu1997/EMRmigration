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

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class HSPharmacyCollectionModeWise : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            long ClinicID = 0;
            long PaymentModeID = 0;

            bool ExportToExcel = false;
            bool IsDateRange = false;

            myDoc = new ReportDocument();

            if (Request.QueryString["IsDateRange"] != null)
            {
                IsDateRange = Convert.ToBoolean(Request.QueryString["IsDateRange"]);
            }
            myDoc.Load(Server.MapPath("HSPharmacyCollectionModeWiserptDateRange.rpt"));

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }



            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ClinicID", ClinicID);
            myDoc.SetParameterValue("@PaymentModeID", PaymentModeID);
            myDoc.SetParameterValue("@IsDateRange", IsDateRange);
            myDoc.SetParameterValue("@FromDate", FromDate, "RefundSubReport");
            myDoc.SetParameterValue("@ToDate", ToDate, "RefundSubReport");
            myDoc.SetParameterValue("@ClinicID", ClinicID, "RefundSubReport");
            myDoc.SetParameterValue("@PaymentModeID", PaymentModeID, "RefundSubReport");
            myDoc.SetParameterValue("@IsDateRange", IsDateRange, "RefundSubReport");

            if (Request.QueryString["ExportToExcel"] != null)
            {
                ExportToExcel = Convert.ToBoolean(Request.QueryString["ExportToExcel"]);
            }

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (ExportToExcel == true)
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