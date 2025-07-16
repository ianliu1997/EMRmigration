using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.EMR
{
    public partial class NewPatientHystroscopyandLaproscopyHistory : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            string imgVirtualDirectory = "http://localhost/" + System.Configuration.ConfigurationManager.AppSettings["EMRImgVirtualDir"] + "/";

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("PatientHystroscopyandLaproscopyHistory.rpt"));
            bool bIsOPDIPD = false;
            long VisitID = 0;
            long PatientID = 0;
            long unitid = 0;
            long TemplateID = 0;
            long EmrID = 0;
            try
            {
                VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                bIsOPDIPD = Convert.ToBoolean(Request.QueryString["IsOPdIPD"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitId"]);
                TemplateID = Convert.ToInt64(Request.QueryString["TemplateID"]);
                EmrID = Convert.ToInt64(Request.QueryString["EmrID"]);
            }
            catch (Exception) { }

            //header
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@UnitID", unitid);
            myDoc.SetParameterValue("@VisitID", VisitID);
            myDoc.SetParameterValue("@EmrID", EmrID);

            //history
            myDoc.SetParameterValue("@PatientID", PatientID, "EmrHistory");
            myDoc.SetParameterValue("@VisitID", VisitID, "EmrHistory");
            myDoc.SetParameterValue("@ISOPDIPD", bIsOPDIPD, "EmrHistory");
            myDoc.SetParameterValue("@EmrID", EmrID, "EmrHistory");
            myDoc.SetParameterValue("@TemplateID", TemplateID, "EmrHistory");
            myDoc.SetParameterValue("@UnitID", unitid, "EmrHistory");

            //Image
            myDoc.SetParameterValue("@PatientID", PatientID, "GetImage");
            myDoc.SetParameterValue("@VisitID", VisitID, "GetImage");
            myDoc.SetParameterValue("@TemplateID", TemplateID, "GetImage");
            myDoc.SetParameterValue("@UnitID", unitid, "GetImage");
            myDoc.SetParameterValue("@EmrID", EmrID, "GetImage");
            myDoc.SetParameterValue("@GetImgPath", imgVirtualDirectory, "GetImage");
            myDoc.SetParameterValue("@ISOPDIPD", bIsOPDIPD, "GetImage");

            //rptHeader
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