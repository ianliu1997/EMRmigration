using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class AvailableStock : System.Web.UI.Page
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
            string ItemIds = null;


            myDoc.Load(Server.MapPath("rptAvailableStock.rpt"));
            
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

            long clinic = 0;
            long store = 0;

            if (Request.QueryString["clinic"] != null)
            {
                clinic = Convert.ToInt64(Request.QueryString["clinic"]);
            }
            if (Request.QueryString["store"] != null)
            {
                store = Convert.ToInt64(Request.QueryString["store"]);
            }

            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@clincId", clinic);
            myDoc.SetParameterValue("@storeId", store);
            myDoc.SetParameterValue("@ToDatePrint", DTP);

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            //Doc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "");

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