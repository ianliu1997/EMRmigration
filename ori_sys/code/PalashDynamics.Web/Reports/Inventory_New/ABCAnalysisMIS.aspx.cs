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
    public partial class ABCAnalysisMIS : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            myDoc = new ReportDocument();
            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;
            long clinic = 0;
            long store = 0;

            
            myDoc.Load(Server.MapPath("ABCAnalysisrpt.rpt"));

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
            if (Request.QueryString["store"] != null && Request.QueryString["store"] != "")
            {
                store = Convert.ToInt64(Request.QueryString["store"]);
            }

            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@ToDatePrint", DTP);

            //Added By Pallavi
            myDoc.SetParameterValue("@UnitId", clinic);
            myDoc.SetParameterValue("@StoreId", store);

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