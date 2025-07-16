using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class StockLedgerReport : System.Web.UI.Page
    {
        long clinic = 0;
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
                String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
                String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
                String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
                DateTime? Date = null;
                string StoreID = null;
                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptStockLedgerReportrptrpt.rpt"));

                if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
                {
                    Date = Convert.ToDateTime(Request.QueryString["FromDate"]);
                }
                if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"] != "" && Request.QueryString["StoreID"] != "0")
                {
                    StoreID = Request.QueryString["StoreID"].ToString();
                }
                if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "" && Request.QueryString["clinic"] != "0")
                {
                    clinic = Convert.ToInt64(Request.QueryString["clinic"].ToString());
                }
                myDoc.SetParameterValue("@FromDate", Date);
                myDoc.SetParameterValue("@StoreID", StoreID);
                myDoc.SetParameterValue("@clinicID", clinic);
                myDoc.SetParameterValue("@UnitID", StoreID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();             
            }
            catch (Exception ex)
            {
                throw ex;
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