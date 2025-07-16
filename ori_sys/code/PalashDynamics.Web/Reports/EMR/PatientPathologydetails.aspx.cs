using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class PatientPathologydetails : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPatientPathologyDetails.rpt"));

            long VisitID = 0;
            long PatientID = 0;
            bool bIsOPDIPD = false;
            long unitid = 0;
            try
            {
                VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitId"]);
                bIsOPDIPD = Convert.ToBoolean(Request.QueryString["IsOPdIPD"]);
            }
            catch (Exception) { }

            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@VisitID", VisitID);
            myDoc.SetParameterValue("@UnitID", unitid);
            //myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD);


            myDoc.SetParameterValue("@PatientID", PatientID, "EMRPath");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMRPath");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "EMRPath");
            myDoc.SetParameterValue("@UnitID", unitid, "EMRPath");


            myDoc.SetParameterValue("@PatientID", PatientID, "EMRDiagnosis");
            myDoc.SetParameterValue("@VisitID", VisitID, "EMRDiagnosis");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "EMRDiagnosis");
            myDoc.SetParameterValue("@UnitID", unitid, "EMRDiagnosis");

            //RptHeader
            myDoc.SetParameterValue("@UnitID", unitid, "rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@UnitID", unitid, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO 
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