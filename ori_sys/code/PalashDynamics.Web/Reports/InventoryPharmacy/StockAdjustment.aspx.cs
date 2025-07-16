using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class StockAdjustment : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long StoreID = 0;
            long UnitID = 0;
            DateTime FrDate;
            DateTime ToDate;
           // DateTime PrintDate;

            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptStockAdjustment.rpt"));

         if (Request.QueryString["FrDate"] != null && Request.QueryString["FrDate"] != "")
            {
                FrDate = Convert.ToDateTime(Request.QueryString["FrDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }           
            if (Request.QueryString["StoreID"] != null)
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }

             if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@FrDate", null);
            myDoc.SetParameterValue("@ToDate", null);
            // myDoc.SetParameterValue("@PrintDate", null);


            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
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

        
    
