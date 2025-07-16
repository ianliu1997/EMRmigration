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

namespace PalashDynamics.Web.Reports.PatientFile
{
    public partial class Patientfile : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            myDoc = new ReportDocument();
            if (Request.QueryString["PrintFomatID"] == "1")
            {
                myDoc.Load(Server.MapPath("PatientFile1.rpt"));
            }
            else if (Request.QueryString["PrintFomatID"] == "2")
            {
                myDoc.Load(Server.MapPath("PatientFile1WithoutHeader.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("PatientFile1.rpt"));
            }
            long PatientId = 0;
            long PatientUnitId = 0;
            if (Request.QueryString["PatientId"] != null)
            {
                PatientId = Convert.ToInt64(Request.QueryString["PatientId"]);
            }
            if (Request.QueryString["PatientUnitId"] != null)
            {
                PatientUnitId = Convert.ToInt64(Request.QueryString["PatientUnitId"]);
            }
            myDoc.SetParameterValue("@PatientId", PatientId);
            myDoc.SetParameterValue("@PatientUnitId", PatientUnitId);
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