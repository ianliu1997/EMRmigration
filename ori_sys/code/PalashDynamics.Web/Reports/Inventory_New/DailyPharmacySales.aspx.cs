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
    public partial class DailyPharmacySales : System.Web.UI.Page
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
            DateTime? ToDatePrint = null;
            long clinic = 0;
            long store = 0;

            string ItemIds = "";
            bool Excel = false;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("crDailyPharmacySalesSummary.rpt"));
            //myDoc.Load(Server.MapPath("rptIndentPendingForApprovalMainReport.rpt"));
            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            #region Added By Pallavi

            if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "")
            {
                clinic = Convert.ToInt64(Request.QueryString["clinic"]);
            }
            if (Request.QueryString["store"] != null && Request.QueryString["store"] != "")
            {
                store = Convert.ToInt64(Request.QueryString["store"]);
            }
            #endregion



            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);

            #region Added By Pallavi
            myDoc.SetParameterValue("@clinicId", clinic);
            myDoc.SetParameterValue("@storeId", store);
            #endregion


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

            //myDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "");
            //MemoryStream oStream = new MemoryStream();

            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";

            //Response.BinaryWrite(oStream.ToArray());
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