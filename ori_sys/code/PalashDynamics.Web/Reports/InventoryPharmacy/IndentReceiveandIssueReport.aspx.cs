using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class IndentReceiveandIssueReport : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;
            long store = 0;
            long clinic = 0;
            bool Excel = false;
            bool IsIndent = false;

            if (Request.QueryString["IsIndent"] != null && Request.QueryString["IsIndent"] != "")
            {
                IsIndent = Convert.ToBoolean(Request.QueryString["IsIndent"]);
            }

            if (IsIndent == true)
            {
                myDoc.Load(Server.MapPath("rptIndentReceiveandIssueReport.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("rptDirectReceiveandIssueReport.rpt"));
            }

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                DTT = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                DTP = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }
            if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "")
            {
                clinic = Convert.ToInt64(Request.QueryString["clinic"]);
            }
            if (Request.QueryString["store"] != null && Request.QueryString["store"] != "")
            {
                store = Convert.ToInt64(Request.QueryString["store"]);
            }
            if (Request.QueryString["Excel"] != null && Request.QueryString["Excel"] != "")
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
                        
            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@ToDatePrint", DTP);
            myDoc.SetParameterValue("@ClinicID", clinic);
            myDoc.SetParameterValue("@StoreID", store);

            myDoc.SetParameterValue("@UnitID", clinic, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", clinic, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (Excel)
            {
                Stream stream = null;
                MemoryStream oStream = new MemoryStream();
                stream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                stream.CopyTo(oStream);
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(oStream.ToArray());
                Response.End();
            }
            //myDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "");
        }

        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            myDoc.Dispose();
            CrystalReportViewer1.Dispose();
            GC.Collect();
        }
    }
}