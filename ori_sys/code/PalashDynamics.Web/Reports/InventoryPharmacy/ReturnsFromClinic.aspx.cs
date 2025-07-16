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
    public partial class ReturnsFromClinic : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? FromDate=null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;
            long FromStore = 0;
            long ToStore = 0;
            long UnitID = 0;
            bool Excel = false;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptReturnsFromClinic1.rpt"));

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["FromStore"] != null)
            {
                FromStore = Convert.ToInt64(Request.QueryString["FromStore"]);
            }

            if (Request.QueryString["ToStore"] != null)
            {
                ToStore = Convert.ToInt64(Request.QueryString["ToStore"]);
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }

            if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "")
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            if (Request.QueryString["Excel"] != null && Request.QueryString["Excel"] != "")
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@FromStore", FromStore);
            myDoc.SetParameterValue("@ToStore", ToStore);
            myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
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