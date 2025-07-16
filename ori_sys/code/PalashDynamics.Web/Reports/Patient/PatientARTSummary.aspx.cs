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
    public partial class PatientARTSummary : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

             myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPatientARTSummary.rpt"));
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
            if (Request.QueryString["CycleNo"] != null)
            {
                CycleNo = Convert.ToInt64(Request.QueryString["CycleNo"]);
            }
          
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@PatientUnitID", UnitId);
            myDoc.SetParameterValue("@TherapyID", TherapyID);
            myDoc.SetParameterValue("@CycleNo", CycleNo);

            myDoc.SetParameterValue("@PatientID", PatientID,"rptPatientLabDay0");
            myDoc.SetParameterValue("@PatientUnitID", UnitId,"rptPatientLabDay0");
            myDoc.SetParameterValue("@TherapyID", TherapyID,"rptPatientLabDay0");
            //myDoc.SetParameterValue("@CycleNo", CycleNo, "rptPatientLabDay0");

             myDoc.SetParameterValue("@PatientID", PatientID,"rptPatientLabDay1");
            myDoc.SetParameterValue("@PatientUnitID", UnitId,"rptPatientLabDay1");
            myDoc.SetParameterValue("@TherapyID", TherapyID,"rptPatientLabDay1");
            //myDoc.SetParameterValue("@CycleNo", CycleNo, "rptPatientLabDay1");
             
            myDoc.SetParameterValue("@PatientID", PatientID,"rptPatientLabDay2");
            myDoc.SetParameterValue("@PatientUnitID", UnitId,"rptPatientLabDay2");
            myDoc.SetParameterValue("@TherapyID", TherapyID,"rptPatientLabDay2");
            //myDoc.SetParameterValue("@CycleNo", CycleNo, "rptPatientLabDay2");

             myDoc.SetParameterValue("@PatientID", PatientID,"rptPatientLabDay3");
            myDoc.SetParameterValue("@PatientUnitID", UnitId,"rptPatientLabDay3");
            myDoc.SetParameterValue("@TherapyID", TherapyID,"rptPatientLabDay3");
            //myDoc.SetParameterValue("@CycleNo", CycleNo, "rptPatientLabDay3");

             myDoc.SetParameterValue("@PatientID", PatientID,"rptPatientLabDay4");
            myDoc.SetParameterValue("@PatientUnitID", UnitId,"rptPatientLabDay4");
            myDoc.SetParameterValue("@TherapyID", TherapyID,"rptPatientLabDay4");
            //myDoc.SetParameterValue("@CycleNo", CycleNo, "rptPatientLabDay4");

             myDoc.SetParameterValue("@PatientID", PatientID,"rptPatientLabDay5");
            myDoc.SetParameterValue("@PatientUnitID", UnitId,"rptPatientLabDay5");
            myDoc.SetParameterValue("@TherapyID", TherapyID,"rptPatientLabDay5");
            //myDoc.SetParameterValue("@CycleNo", CycleNo, "rptPatientLabDay5");

            myDoc.SetParameterValue("@PatientID", PatientID, "rptPatientLabDay6");
            myDoc.SetParameterValue("@PatientUnitID", UnitId, "rptPatientLabDay6");
            myDoc.SetParameterValue("@TherapyID", TherapyID, "rptPatientLabDay6");
            //myDoc.SetParameterValue("@CycleNo", CycleNo, "rptPatientLabDay6");

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