using System;
using System.Web;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Web.Configuration;

namespace PalashDynamics.Web.Reports.IVF
{
    public partial class PatientIVF_AND_ANCSummary : System.Web.UI.Page
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
            String strReportName = "Patient IVF & ANC Summary Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
            string FromDate = null;
            string ToDate = null;
            string ToDatePrint = null;
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
            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("PatientIVFANCSummary.rpt"));
            myDoc.SetParameterValue("@FromDate", FrDT);
                myDoc.SetParameterValue("@ToDate", ToDT);
                myDoc.SetParameterValue("@ToDatePrint", ToDP);
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