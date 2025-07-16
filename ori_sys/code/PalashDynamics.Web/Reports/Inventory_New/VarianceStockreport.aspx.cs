using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class VarianceStockreport : System.Web.UI.Page
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
            long lUnitId = 0;
            long StoreID = 0;

            long ID = 0;
            long UnitID = 0;

            myDoc.Load(Server.MapPath("RptVarianceStockreport.rpt"));

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
            if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"] != "")
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }
            if (Request.QueryString["LoginUnitID"] != null)
            {
                lUnitId = Convert.ToInt64(Request.QueryString["LoginUnitID"]);
            }

            if (Request.QueryString["ID"] != null)
            {
                ID = Convert.ToInt64(Request.QueryString["ID"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@ToDatePrint", DTP);
            myDoc.SetParameterValue("@StoreID", StoreID);
            //myDoc.SetParameterValue("@UnitID", lUnitId, "Header_SubReport");
            myDoc.SetParameterValue("@UnitID", lUnitId, "rptCommonReportHeader.rpt");
            // myDoc.SetParameterValue("@UnitID", lUnitId, "Header_SubReport");

            myDoc.SetParameterValue("@UnitID", lUnitId, "rptUnitLogo.rpt"); 
          //  myDoc.SetParameterValue("@ID", ID);
            myDoc.SetParameterValue("@UnitID", UnitID);

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

        }
        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            myDoc.Dispose();
            CrystalReportViewer1.Dispose();
            GC.Collect();
        }
    }
}