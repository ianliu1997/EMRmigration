using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;


namespace PalashDynamics.Web.Reports.EMR
{
    public partial class AllReferralLetterPrint : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("AllReferralLetterPrint_1.rpt"));           
            long VisitID = 0;
            long PatientID = 0;
            long PrescriptionID = 0;
            long ReferalID = 0;
            long unitid = 0;
            try
            {              
                VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                PrescriptionID= Convert.ToInt64(Request.QueryString["PrescriptionID"]);
                ReferalID= Convert.ToInt64(Request.QueryString["ID"]);
                unitid = Convert.ToInt64(Request.QueryString["UnitID"]); 
            }
            catch (Exception){ }
           
            myDoc.SetParameterValue("@PatientID", PatientID);          
            myDoc.SetParameterValue("@VisitID", VisitID);
            myDoc.SetParameterValue("@PrescriptionID", PrescriptionID);
            myDoc.SetParameterValue("@ReferralID", ReferalID);

            myDoc.SetParameterValue("@PatientID", PatientID, "Header");
            myDoc.SetParameterValue("@VisitID", VisitID, "Header");
            myDoc.SetParameterValue("@UnitID", unitid, "Header");
 
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