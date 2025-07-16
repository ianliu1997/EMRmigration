using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class IPDPatientPathologydetails : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptIPDPatientPathologyDetails.rpt"));
            long VisitID = 0;
            long PatientID = 0;
            long bIsOPDIPD = 0;
            long unitid = 0;
            long DoctorId=0;
            Boolean GetOPDIPD=false;
            try
            {
                VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitId"]);
                bIsOPDIPD = Convert.ToInt64(Request.QueryString["IsOPdIPD"]);
                DoctorId = Convert.ToInt64(Request.QueryString["DoctorId"]);
            }
            catch (Exception) { }
            if (bIsOPDIPD == 1)
            {
                GetOPDIPD = true;
            }
            else
            {
                GetOPDIPD = false;
            }
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@VisitID", VisitID);
            myDoc.SetParameterValue("@UnitID", unitid);
            myDoc.SetParameterValue("@PatientID", PatientID, "IPDLAB");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDLAB");
            myDoc.SetParameterValue("@IsOPDIPD", bIsOPDIPD, "IPDLAB");
            myDoc.SetParameterValue("@DoctorId", DoctorId, "IPDLAB");
            myDoc.SetParameterValue("@PatientID", PatientID, "emrdiag");
            myDoc.SetParameterValue("@VisitID", VisitID, "emrdiag");
            myDoc.SetParameterValue("@IsOPDIPD", GetOPDIPD, "emrdiag");
            myDoc.SetParameterValue("@DoctorId", DoctorId, "emrdiag");

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