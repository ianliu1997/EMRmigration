using System;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class ConsignmentSaleReport : System.Web.UI.Page
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
            long lClinicId = 0;
            long lStoreId = 0;
            bool Excel = false;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptConsignmentItemSaleReport.rpt"));
            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ClinicID"] != null && Request.QueryString["ClinicID"] != "")
            {
                lClinicId = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"] != "")
            {
                lStoreId = Convert.ToInt64(Request.QueryString["StoreID"]);
            }
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);

            myDoc.SetParameterValue("@clinicId", lClinicId);
            myDoc.SetParameterValue("@StoreId", lStoreId);
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