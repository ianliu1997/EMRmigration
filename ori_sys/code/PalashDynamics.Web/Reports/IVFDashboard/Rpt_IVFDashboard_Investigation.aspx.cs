using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.IVFDashboard
{
    public partial class Rpt_IVFDashboard_Investigation : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            long FPatientID = 0;
            long FPatientUnitID = 0;
            long MPatientID = 0;
            long MPatientUnitID = 0;
            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("Rpt_IVFDashboardInvestigation.rpt"));
            if (Request.QueryString["FPatientID"] != null && Request.QueryString["FPatientID"] != "")
            {
                FPatientID = Convert.ToInt64(Request.QueryString["FPatientID"]);
            }
            if (Request.QueryString["FPatientUnitID"] != null && Request.QueryString["FPatientUnitID"] != "")
            {
                FPatientUnitID = Convert.ToInt64(Request.QueryString["FPatientUnitID"]);
            }           
            if (Request.QueryString["MPatientID"] != null && Request.QueryString["MPatientID"] != "")
            {
                MPatientID = Convert.ToInt64(Request.QueryString["MPatientID"]);

            }
            if (Request.QueryString["MPatientUnitID"] != null && Request.QueryString["MPatientUnitID"] != "")
            {
                MPatientUnitID = Convert.ToInt64(Request.QueryString["MPatientUnitID"]);
            }          

            myDoc.SetParameterValue("@FPatientID", FPatientID);
            myDoc.SetParameterValue("@FPatientUnitID", FPatientUnitID);
            myDoc.SetParameterValue("@MPatientID", MPatientID);
            myDoc.SetParameterValue("@MPatientUnitID", MPatientUnitID);
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