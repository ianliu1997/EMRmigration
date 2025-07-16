using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.Patient
{
    public partial class PatientARTReportNew : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPatientARTReportNew.rpt"));
            long UnitId = 0;
            long PatientID = 0;
            long TherapyID = 0;           

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

            myDoc.SetParameterValue("@PatientID", PatientID, "rptInvestigation");
            myDoc.SetParameterValue("@PatientUnitID", UnitId, "rptInvestigation");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "rptInvestigation");

            myDoc.SetParameterValue("@PatientID", PatientID, "rptdrugnew");
            myDoc.SetParameterValue("@PatientUnitID", UnitId, "rptdrugnew");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "rptdrugnew");

            myDoc.SetParameterValue("@PatientID", PatientID, "rptETNew");
            myDoc.SetParameterValue("@PatientUnitID", UnitId, "rptETNew");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "rptETNew");

            myDoc.SetParameterValue("@PatientID", PatientID, "rprsourceCryoNew");
            myDoc.SetParameterValue("@PatientUnitID", UnitId, "rprsourceCryoNew");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "rprsourceCryoNew");


            myDoc.SetParameterValue("@PatientID", PatientID, "rpt_PatientPrescriptionNew");
            myDoc.SetParameterValue("@PatientUnitID", UnitId, "rpt_PatientPrescriptionNew");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "rpt_PatientPrescriptionNew");
         

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