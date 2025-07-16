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
    public partial class OTSchedulesDetailsReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptOTSchedulesDetails.rpt"));
            long ScheduleID = 0;
            long OTDetailsID = 0;
            long PatientID = 0;
            long unitid = 0;
            try
            {
             //   HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/OperationTheatre/OTSchedulesDetailsReport.aspx?PatientID=" + PatientID + "&ScheduleID=" + lScheduleID + "&OTDetailsID=" + lOTDetailsID + "&UnitID=" + unitid), "_blank");
                OTDetailsID = Convert.ToInt64(Request.QueryString["OTDetailsID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                ScheduleID = Convert.ToInt64(Request.QueryString["ScheduleID"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            catch (Exception) { }

            //header
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@UnitID", unitid);
            myDoc.SetParameterValue("@ScheduleID", ScheduleID);
           // myDoc.SetParameterValue("@IsOPDIPD", ScheduleID);

            myDoc.SetParameterValue("@UnitID", unitid, "rptCommonReportHeader.rpt");
            // myDoc.SetParameterValue("@UnitID", lUnitId, "Header_SubReport");

            myDoc.SetParameterValue("@UnitID", unitid, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO

            myDoc.SetParameterValue("@OTDetailsID", OTDetailsID, "rptGetOTDetailsForOTDetailsID.rpt");
            myDoc.SetParameterValue("@OTDetailsID", OTDetailsID, "rptOTSurgeryDetails.rpt");
            myDoc.SetParameterValue("@OTDetailsID", OTDetailsID, "rptGetDoctorNotesDetailsByOTDetailsID.rpt");
            myDoc.SetParameterValue("@OTDetailsID", OTDetailsID, "rptCIMS_GetOTNotesDetailsByOTDetailsID_SurgeyNotes.rpt");
            myDoc.SetParameterValue("@OTDetailsID", OTDetailsID, "rptGetOTNotesDetailsByOTDetailsID_AnesthesiaNotes.rpt");
           // myDoc.SetParameterValue("@OTDetailsID", OTDetailsID, "rptGetDoctorNotesDetailsByOTDetailsID.rpt");
            myDoc.SetParameterValue("@OTDetailsID", OTDetailsID, "rptGetItemDetailsByOTDetailsID.rpt");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            
            //myDoc.SetParameterValue("@PrescriptionID", OTDetailsID, "rptEMR_Prescription_GetRadiologyDetails.rpt");
            //myDoc.SetParameterValue("@IsOPDIPD", ScheduleID, "rptEMR_Prescription_GetRadiologyDetails.rpt");

            //myDoc.SetParameterValue("@PatientID", PatientID, "rptGetEMR_Prescription_LaboratoryDetails.rpt");
            //myDoc.SetParameterValue("@UnitID", unitid, "rptGetEMR_Prescription_LaboratoryDetails.rpt");
            //myDoc.SetParameterValue("@PrescriptionID", OTDetailsID, "rptGetEMR_Prescription_LaboratoryDetails.rpt");
            //myDoc.SetParameterValue("@IsOPDIPD", ScheduleID, "rptGetEMR_Prescription_LaboratoryDetails.rpt");


            //myDoc.SetParameterValue("@PatientID", PatientID, "rptPrescription_Details.rpt");
            //myDoc.SetParameterValue("@UnitID", unitid, "rptPrescription_Details.rpt");
            //myDoc.SetParameterValue("@PrescriptionID", OTDetailsID, "rptPrescription_Details.rpt");
            //myDoc.SetParameterValue("@IsOPDIPD", ScheduleID, "rptPrescription_Details.rpt");

            //////    myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            //////history
            //////myDoc.SetParameterValue("@PatientID", PatientID, "IPDEMRHistory");
            //////myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDEMRHistory");
            //////myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDEMRHistory");

            //////myDoc.SetParameterValue("@PatientID", PatientID, "IPDVital");
            //////myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDVital");
            //////myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDVital");
            //////myDoc.SetParameterValue("@UnitID", unitid, "IPDVital");
            //////myDoc.SetParameterValue("@PatientUnitID", unitid, "IPDVital");

            //////myDoc.SetParameterValue("@PatientId", PatientID, "IPDEMRDiagnosis");
            //////myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDEMRDiagnosis");
            //////myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDEMRDiagnosis");

            //////myDoc.SetParameterValue("@PatientId", PatientID, "IPDPhysicalexam");
            //////myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDPhysicalexam");
            //////myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDPhysicalexam");

            //////myDoc.SetParameterValue("@PatientId", PatientID, "IPDProcedure");
            //////myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDProcedure");
            //////myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDProcedure");

            //////myDoc.SetParameterValue("@PatientID", PatientID, "EMREduction");
            //////myDoc.SetParameterValue("@VisitID", PrescriptionID, "EMREduction");
            //////myDoc.SetParameterValue("@isopdipd", bIsOPDIPD, "EMREduction");

            ////myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
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