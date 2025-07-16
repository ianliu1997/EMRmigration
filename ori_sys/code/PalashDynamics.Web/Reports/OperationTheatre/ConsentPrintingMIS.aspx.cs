using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.OperationTheatre
{
    public partial class ConsentPrintingMIS : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("ConsentPrintingRpt.rpt"));
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            long ConsentID = 0;
            long PatientID = 0;
            long UnitID = 0;
            DateTime? date = null;
            string tempate = null;
            if (Request.QueryString["ConsentID"] != null && Request.QueryString["ConsentID"] != "0")
            {
                ConsentID = Convert.ToInt64(Request.QueryString["ConsentID"]);
            }
            if (Request.QueryString["PatientID"] != null && Request.QueryString["PatientID"] != "0")
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }
            if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            if (Request.QueryString["Date"] != null && Request.QueryString["Date"] != "0")
            {
                date = Convert.ToDateTime(Request.QueryString["Date"]);
            }
            if (Request.QueryString["TemplateName"] != null && Request.QueryString["TemplateName"] != "0")
            {
                tempate = Convert.ToString(Request.QueryString["TemplateName"]);
            }
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            myDoc.SetParameterValue("@ConsentID", ConsentID);
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@ProcDate", date);
            myDoc.SetParameterValue("@TemplateName", tempate);
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