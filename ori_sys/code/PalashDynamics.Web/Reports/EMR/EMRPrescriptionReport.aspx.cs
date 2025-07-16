using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class EMRPrescriptionReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPrescriptionDetails.rpt"));
            bool bIsOPDIPD = false;
            long PrescriptionID = 0;
            long PatientID = 0;
            long unitid = 0;
            try
            {
                PrescriptionID = Convert.ToInt64(Request.QueryString["PrescriptionID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                bIsOPDIPD = Convert.ToBoolean(Request.QueryString["IsOPdIPD"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitId"]);
            }
            catch (Exception) { }

            //header
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@UnitID", unitid);
            myDoc.SetParameterValue("@PrescriptionID", PrescriptionID);
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD);

            myDoc.SetParameterValue("@UnitID", unitid, "rptCommonReportHeader.rpt");
            // myDoc.SetParameterValue("@UnitID", lUnitId, "Header_SubReport");

            myDoc.SetParameterValue("@UnitID", unitid, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO

            myDoc.SetParameterValue("@PatientID", PatientID, "rptEMR_Prescription_GetRadiologyDetails.rpt");
            myDoc.SetParameterValue("@UnitID", unitid, "rptEMR_Prescription_GetRadiologyDetails.rpt");
            myDoc.SetParameterValue("@PrescriptionID", PrescriptionID, "rptEMR_Prescription_GetRadiologyDetails.rpt");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "rptEMR_Prescription_GetRadiologyDetails.rpt");

            myDoc.SetParameterValue("@PatientID", PatientID, "rptGetEMR_Prescription_LaboratoryDetails.rpt");
            myDoc.SetParameterValue("@UnitID", unitid, "rptGetEMR_Prescription_LaboratoryDetails.rpt");
            myDoc.SetParameterValue("@PrescriptionID", PrescriptionID, "rptGetEMR_Prescription_LaboratoryDetails.rpt");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "rptGetEMR_Prescription_LaboratoryDetails.rpt");


            myDoc.SetParameterValue("@PatientID", PatientID, "rptPrescription_Details.rpt");
            myDoc.SetParameterValue("@UnitID", unitid, "rptPrescription_Details.rpt");
            myDoc.SetParameterValue("@PrescriptionID", PrescriptionID, "rptPrescription_Details.rpt");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "rptPrescription_Details.rpt");
            
        //    myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            //history
            //myDoc.SetParameterValue("@PatientID", PatientID, "IPDEMRHistory");
            //myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDEMRHistory");
            //myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDEMRHistory");

            //myDoc.SetParameterValue("@PatientID", PatientID, "IPDVital");
            //myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDVital");
            //myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDVital");
            //myDoc.SetParameterValue("@UnitID", unitid, "IPDVital");
            //myDoc.SetParameterValue("@PatientUnitID", unitid, "IPDVital");

            //myDoc.SetParameterValue("@PatientId", PatientID, "IPDEMRDiagnosis");
            //myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDEMRDiagnosis");
            //myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDEMRDiagnosis");

            //myDoc.SetParameterValue("@PatientId", PatientID, "IPDPhysicalexam");
            //myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDPhysicalexam");
            //myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDPhysicalexam");

            //myDoc.SetParameterValue("@PatientId", PatientID, "IPDProcedure");
            //myDoc.SetParameterValue("@VisitID", PrescriptionID, "IPDProcedure");
            //myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDProcedure");

            //myDoc.SetParameterValue("@PatientID", PatientID, "EMREduction");
            //myDoc.SetParameterValue("@VisitID", PrescriptionID, "EMREduction");
            //myDoc.SetParameterValue("@isopdipd", bIsOPDIPD, "EMREduction");

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //MemoryStream oStream = new MemoryStream();
            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";

            //Response.BinaryWrite(oStream.ToArray());
            //Response.End();
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