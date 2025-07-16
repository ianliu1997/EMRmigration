using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class PatientAnalysisReport : System.Web.UI.Page
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

            String strReportName = "Patient Analysis Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;

            string AgeFilter = null;
            long StateID = 0;
            long UnitID = 0;
            int Age = 0;
            long ProtocolTypeID = 0;
            long TreatmentPlanID = 0;
            long SrcOocyteId = 0;
            long SrcSemenID = 0;
            //bool chkToDate = true;
            long DrugID = 0;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("crPatientAnalysisReport.rpt"));


            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }
            if (Request.QueryString["ClinicID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["StateID"] != null)
            {
                StateID = Convert.ToInt64(Request.QueryString["StateID"]);
            }
           
            if (Request.QueryString["ProtocolTypeID"] != null)
            {
                ProtocolTypeID = Convert.ToInt64(Request.QueryString["ProtocolTypeID"]);
            }
            if (Request.QueryString["TreatmentPlanID"] != null)
            {
                TreatmentPlanID = Convert.ToInt64(Request.QueryString["TreatmentPlanID"]);
            }
            if (Request.QueryString["SrcOocyteId"] != null)
            {
                SrcOocyteId = Convert.ToInt64(Request.QueryString["SrcOocyteId"]);
            }
            if (Request.QueryString["SrcSemenID"] != null)
            {
                SrcSemenID = Convert.ToInt64(Request.QueryString["SrcSemenID"]);
            }
            if (Request.QueryString["DrugID"] != null)
            {
                DrugID = Convert.ToInt64(Request.QueryString["DrugID"]);
            }
            if (Request.QueryString["AgeFilter"] != null && Request.QueryString["AgeFilter"] != "")
            {
                AgeFilter = Convert.ToString(Request.QueryString["AgeFilter"]);
            }
            if (Request.QueryString["Age"] != null)
            {
                Age = Convert.ToInt32(Request.QueryString["Age"]);
            }
            //rohinee
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@StateID",StateID);
            myDoc.SetParameterValue("@ProtocolTypeID", ProtocolTypeID);
            myDoc.SetParameterValue("@TreatmentPlanID", TreatmentPlanID);
            myDoc.SetParameterValue("@SrcOocyteId", SrcOocyteId);
            myDoc.SetParameterValue("@SrcSemenID", SrcSemenID);
            myDoc.SetParameterValue("@DrugID", DrugID);
            myDoc.SetParameterValue("@Age", Age);
            myDoc.SetParameterValue("@AgeFilter", AgeFilter);
            myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
            //myDoc.SetParameterValue("", );
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
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