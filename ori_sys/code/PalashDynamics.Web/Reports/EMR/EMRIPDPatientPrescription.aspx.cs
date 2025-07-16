using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class EMRIPDPatientPrescription : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            string ServerPath = "";
            ServerPath = Server.MapPath("EMRIPDRoundPatientPrescription.rpt");
            myDoc.Load(ServerPath);

            long VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
            long PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            Boolean IsOPDIPD = false;
            if (Request.QueryString["IsOPDIPD"] == "0")
            {
                IsOPDIPD = false;
            }
            else
            {
                IsOPDIPD = true;
            }
            long UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            long DoctorID = Convert.ToInt64(Request.QueryString["DoctorId"]);
            myDoc.SetParameterValue("@VisitID", VisitID);
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@IsOPDIPd", IsOPDIPD);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@DoctorId", DoctorID);

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