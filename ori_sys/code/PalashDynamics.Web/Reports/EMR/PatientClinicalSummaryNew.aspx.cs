using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class PatientClinicalSummaryNew : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPatientClinicalSummaryNew.rpt"));
            bool bIsOPDIPD = false;
            long VisitID = 0;
            long PatientID = 0;
            long unitid=0;
            long HistoryFlag = 0;
            long TemplateID = 0;
            try
            {
                VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                bIsOPDIPD = Convert.ToBoolean(Request.QueryString["IsOPdIPD"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitId"]);
                HistoryFlag = Convert.ToInt64(Request.QueryString["HistoryFlag"]);
                TemplateID = Convert.ToInt64(Request.QueryString["TemplateID"]);
            }
            catch (Exception) { }

            //header
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@UnitID", unitid);
            myDoc.SetParameterValue("@VisitID", VisitID);
                        
            //history
            myDoc.SetParameterValue("@PatientID", PatientID, "EMRHistory");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMRHistory");
            myDoc.SetParameterValue("@ISOPDIPD", bIsOPDIPD, "EMRHistory");
            myDoc.SetParameterValue("@HistoryFlag", HistoryFlag, "EMRHistory");
            myDoc.SetParameterValue("@TemplateID", TemplateID, "EMRHistory");
            //followup
            myDoc.SetParameterValue("@PatientId", PatientID, "followupnote");
            myDoc.SetParameterValue("@VisitId", VisitID, "followupnote");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "followupnote");
            myDoc.SetParameterValue("@HistoryFlag", HistoryFlag, "followupnote");
            myDoc.SetParameterValue("@UnitID", unitid, "followupnote");

            myDoc.SetParameterValue("@PatientID", PatientID, "EMRVitals");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMRVitals");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "EMRVitals");
            myDoc.SetParameterValue("@UnitID", unitid, "EMRVitals");

            //rptHeader
            myDoc.SetParameterValue("@UnitID", unitid, "rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@PatientID", PatientID, "EMRDiagnosis");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMRDiagnosis");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "EMRDiagnosis");
            myDoc.SetParameterValue("@UnitID", unitid, "EMRDiagnosis");

            myDoc.SetParameterValue("@PatientID", PatientID, "EMRProcdure");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMRProcdure");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "EMRProcdure");
            myDoc.SetParameterValue("@UnitID", unitid, "EMRProcdure");

            myDoc.SetParameterValue("@PatientID", PatientID, "EMREduction");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMREduction");
            myDoc.SetParameterValue("@isopdipd", bIsOPDIPD, "EMREduction");
            myDoc.SetParameterValue("@UnitID", unitid, "EMREduction");

            myDoc.SetParameterValue("@PatientID", PatientID, "physicalExam");
            myDoc.SetParameterValue("@VisitID", VisitID, "physicalExam");
            myDoc.SetParameterValue("@ISOPDIDP", bIsOPDIPD, "physicalExam");
            myDoc.SetParameterValue("@UnitID", unitid, "physicalExam");

            myDoc.SetParameterValue("@UnitID", unitid, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            Stream oStream = null;
            byte[] byteArray = null;
            //   oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteArray);
            Response.End();

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