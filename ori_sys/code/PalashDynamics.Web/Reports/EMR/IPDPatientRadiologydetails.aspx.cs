using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class IPDPatientRadiologydetails : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptIPDPatientRadiologyDetails.rpt"));

            long VisitID = 0;
            long PatientID = 0;
            long bIsOPDIPD = 0;
            long unitid = 0;
            long DoctorID = 0;
            Boolean GetOpdIpd = false;
            try
            {
                VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                bIsOPDIPD = Convert.ToInt64(Request.QueryString["IsOPdIPD"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitId"]);
                DoctorID = Convert.ToInt64(Request.QueryString["DoctorId"]);
            }
            catch (Exception) { }

            if (bIsOPDIPD == 1)
            {
                GetOpdIpd = true;
            }
            else
            {
                GetOpdIpd = false;
            }

            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@VisitID", VisitID);
            myDoc.SetParameterValue("@UnitID", unitid);
            myDoc.SetParameterValue("@PatientID", PatientID, "IPDEMRDIG");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDEMRDIG");
            myDoc.SetParameterValue("@IsOPDIPD", GetOpdIpd, "IPDEMRDIG");
            myDoc.SetParameterValue("@DoctorId", DoctorID, "IPDEMRDIG");
            myDoc.SetParameterValue("@PatientID", PatientID, "IPDRadiology");
            myDoc.SetParameterValue("@VisitID", VisitID, "IPDRadiology");
            myDoc.SetParameterValue("@IsOPDIPD", GetOpdIpd, "IPDRadiology");
            myDoc.SetParameterValue("@DoctorId", DoctorID, "IPDRadiology");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc); //rptDiagnosis
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