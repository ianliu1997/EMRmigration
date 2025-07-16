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
    public partial class StockStatement : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string ItemIds = null;
            long clinicID = 0;
            long StoreID = 0;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptStockStatement.rpt"));

            if (Request.QueryString["ItemIDs"] != null && Request.QueryString["ItemIDs"] != "" && Request.QueryString["ItemIDs"] != "0")
            {
                ItemIds = Request.QueryString["ItemIDs"].ToString();
            }          

            if (Request.QueryString["ClinicID"] != null)
            {
                clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }

            if (Request.QueryString["StoreID"] != null)
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }
            myDoc.SetParameterValue("@ItemID", ItemIds);
            myDoc.SetParameterValue("@UnitID", clinicID);
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@BatchCode", null);
            myDoc.SetParameterValue("@ItemName", null);
            myDoc.SetParameterValue("@PrintDate", null);


            myDoc.SetParameterValue("@UnitID", StoreID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

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