using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;


namespace PalashDynamics.Web.Reports.IVFDashboard
{
    public partial class IVFDashboard_PatientStimulationReport : System.Web.UI.Page
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
                myDoc.Load(Server.MapPath("Rpt_IVFDashboard_Stimulation.rpt"));
            }
            else if (Request.QueryString["PrintFomatID"] == "2")
            {
                myDoc.Load(Server.MapPath("Rpt_IVFDashboard_Stimulation.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("Rpt_IVFDashboard_Stimulation.rpt"));
            }

            long PatientUnitID = 0;
            long PatientID = 0;

            long TherapyID = 0;
            long TherapyUnitID = 0;

            if (Request.QueryString["PatientUnitID"] != null)
            {
                PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }
            if (Request.QueryString["TherapyId"] != null)
            {
                TherapyID = Convert.ToInt64(Request.QueryString["TherapyId"]);
            }
            if (Request.QueryString["TherapyUnitId"] != null)
            {
                TherapyUnitID = Convert.ToInt64(Request.QueryString["TherapyUnitId"]);
            }
            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }

            long unitid = 0;
            if (Request.QueryString["UnitID"] != null)
            {
                unitid = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            myDoc.SetParameterValue("@UnitID", unitid, "rptUnitLogo.rpt");
            myDoc.SetParameterValue("@UnitID", unitid, "rptCommonReportHeader.rpt"); 

            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
            myDoc.SetParameterValue("@TherapyID", TherapyID);
            myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID);

            //myDoc.SetParameterValue("@PatientID", PatientID, "TherapyDocument");
            //myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "TherapyDocument");
            //myDoc.SetParameterValue("@TherapyID", TherapyID, "TherapyDocument");
            //myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "TherapyDocument");

            myDoc.SetParameterValue("@PatientID", PatientID, "Rpt_RightLeftStimulation.rpt");
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_RightLeftStimulation.rpt");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "Rpt_RightLeftStimulation.rpt");
            myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "Rpt_RightLeftStimulation.rpt");

            myDoc.SetParameterValue("@PatientID", PatientID, "Rpt_InvestStimulation.rpt");
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_InvestStimulation.rpt");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "Rpt_InvestStimulation.rpt");
            myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "Rpt_InvestStimulation.rpt");
            
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();
            
        }
    }
}