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
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class LostpatientList : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath;
            string strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
          
            String strReportName = "Lost Patient Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            string FromDate = null;
            string ToDate = null;
            string ToDatePrint = null;

            DateTime? FrDT = null;
            DateTime? ToDT = null;
            DateTime? ToDP = null;

            long ClinicID = 0;
            long VisitTypeID = 0;
         
            string AppPath = Request.ApplicationPath;

            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptLostPatientList.rpt"));

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
            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["VisitTypeID"] != null)
            {
                VisitTypeID = Convert.ToInt64(Request.QueryString["VisitTypeID"]);
            }


            //rohinee
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);


            myDoc.SetParameterValue("@ClinicID", ClinicID);
            myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
            myDoc.SetParameterValue("@VisitTypeID", VisitTypeID);
            myDoc.SetParameterValue("@UnitID", ClinicID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", ClinicID, "rptUnitLogo.rpt");
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