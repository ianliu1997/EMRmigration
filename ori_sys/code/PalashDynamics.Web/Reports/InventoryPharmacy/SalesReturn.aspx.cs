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

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class SalesReturn : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;
            long clinic = 0;
            long UserID = 0;
            long store = 0;
            bool Excel = false;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptSaleReturn.rpt"));
                        
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
            //Added By Pallavi
            if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "")
            {
                clinic = Convert.ToInt64(Request.QueryString["clinic"]);
            }

            if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "")
            {
                UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            }

            if (Request.QueryString["store"] != null && Request.QueryString["store"] != "")
            {
                store = Convert.ToInt64(Request.QueryString["store"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
        
            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@ToDatePrint", DTP);

            //Added By Pallavi
            myDoc.SetParameterValue("@ClinicId", clinic);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@StoreId", store);

            myDoc.SetParameterValue("@UnitID", clinic, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetParameterValue("@UnitID", clinic, "rptCommonReportHeader.rpt");

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