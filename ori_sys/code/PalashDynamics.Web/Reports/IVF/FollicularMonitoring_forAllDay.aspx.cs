using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Data.SqlClient;


namespace PalashDynamics.Web.Reports.IVF
{
    public partial class FollicularMonitoring_forAllDay : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rpt_FollicularMonitoring_daywise.rpt"));
            long UnitId = 0;
            long PatientID = 0;
            long TherapyID = 0;
            long CycleNo = 0;

            if (Request.QueryString["PatientUnitID"] != null)
            {
                UnitId = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }
            if (Request.QueryString["TherapyID"] != null)
            {
                TherapyID = Convert.ToInt64(Request.QueryString["TherapyID"]);
            }
            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }          

            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@PatientUnitID", UnitId);
            myDoc.SetParameterValue("@TherapyID", TherapyID);
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