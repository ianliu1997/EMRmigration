using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class PatientIPDClinicalSummary : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptIPDPatientClinicalSummary.rpt"));
            bool bIsOPDIPD = false;
            long VisitID = 0;
            long PatientID = 0;
            long unitid = 0;
            try
            {
                VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                bIsOPDIPD = Convert.ToBoolean(Request.QueryString["IsOPdIPD"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitId"]);
            }
            catch (Exception) { }

            //header
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@UnitID", unitid);
            myDoc.SetParameterValue("@VisitID", VisitID);

            //history
            myDoc.SetParameterValue("@PatientID", PatientID, "IPDEMRHistory");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDEMRHistory");
            myDoc.SetParameterValue("@ISOPDIPD", bIsOPDIPD, "IPDEMRHistory");

            myDoc.SetParameterValue("@PatientID", PatientID, "IPDVital");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDVital");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDVital");
            myDoc.SetParameterValue("@UnitID", unitid, "IPDVital");
            myDoc.SetParameterValue("@PatientUnitID", unitid, "IPDVital");

            myDoc.SetParameterValue("@PatientId", PatientID, "IPDEMRDiagnosis");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDEMRDiagnosis");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDEMRDiagnosis");

            myDoc.SetParameterValue("@PatientId", PatientID, "IPDPhysicalexam");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDPhysicalexam");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDPhysicalexam");

            myDoc.SetParameterValue("@PatientId", PatientID, "IPDProcedure");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDProcedure");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDProcedure");

            myDoc.SetParameterValue("@PatientID", PatientID, "EMREduction");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMREduction");
            myDoc.SetParameterValue("@isopdipd", bIsOPDIPD, "EMREduction");

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            MemoryStream oStream = new MemoryStream();
            oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";

            Response.BinaryWrite(oStream.ToArray());
            Response.End();
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