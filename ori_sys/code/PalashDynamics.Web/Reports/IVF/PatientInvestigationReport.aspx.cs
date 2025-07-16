using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Web.Configuration;

namespace PalashDynamics.Web.Reports.IVF
{
    public partial class PatientInvestigationReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            string strPath;
            string strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            String strReportName = "Patient Investigation Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            string FromDate = null;
            string ToDate = null;
            string ToDatePrint = null;
            long CategoryId = 0;
            long ParamId = 0;
            long TestId = 0;
            long ResultId = 0;
            DateTime? FrDT = null;
            DateTime? ToDT = null;
            DateTime? ToDP = null;
            string AppPath = Request.ApplicationPath;
            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToString(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToString(Request.QueryString["ToDatePrint"]);
            }

            if (Request.QueryString["CategoryId"] != null)
            {
                CategoryId = Convert.ToInt64(Request.QueryString["CategoryId"]);
            }

            if (Request.QueryString["TestId"] != null)
            {
                TestId = Convert.ToInt64(Request.QueryString["TestId"]);
            }

            if (Request.QueryString["ParamId"] != null)
            {
                ParamId = Convert.ToInt64(Request.QueryString["ParamId"]);
            }

            if (Request.QueryString["ResultId"] != null)
            {
                ResultId = Convert.ToInt64(Request.QueryString["ResultId"]);
            }
            if (!string.IsNullOrEmpty(FromDate))
            {
                FrDT = Convert.ToDateTime(FromDate);
            }
            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDT = Convert.ToDateTime(ToDate);

            }
            if (!string.IsNullOrEmpty(ToDatePrint))
            {
                ToDP = Convert.ToDateTime(ToDatePrint);
            }

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("Patient Investigation Report1.rpt"));
            myDoc.SetParameterValue("@FromDate", FrDT);
            myDoc.SetParameterValue("@ToDate", ToDT);
            myDoc.SetParameterValue("@ToDatePrint", ToDT);
            myDoc.SetParameterValue("@CategoryId", CategoryId);
            myDoc.SetParameterValue("@TestId", TestId);
            myDoc.SetParameterValue("@ParamId", ParamId);
            myDoc.SetParameterValue("@ResultId", ResultId);
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