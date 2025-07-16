using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.Patient
{
    public partial class PatientNewCaseRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            ReportDocument myDoc = new ReportDocument();

            ReportDocument myDoc1 = new ReportDocument();

            string ServerPath = "";

            //if (Convert.ToInt64(Request.QueryString["Type"]) == 1)
            //{
            //    if (Request.QueryString["PrintFomatID"] == "1")
            //    {
            //        ServerPath = Server.MapPath("rptPatientCaseRecord.rpt");
            //    }
            //    else if (Request.QueryString["PrintFomatID"] == "2")
            //    {
            //        ServerPath = Server.MapPath("rptPatientCaseRecord.rpt");
            //    }
            //    else
            //    {
            //        ServerPath = Server.MapPath("rptPatientCaseRecord.rpt");
            //    }
            //}
            //else if (Convert.ToInt64(Request.QueryString["Type"]) == 2)
            //{
            //    if (Request.QueryString["PrintFomatID"] == "1")
            //    {
            //        ServerPath = Server.MapPath("rptPatientCaseReferral.rpt");
            //    }
            //    else if (Request.QueryString["PrintFomatID"] == "2")
            //    {
            //        ServerPath = Server.MapPath("rptPatientCaseReferral.rpt");
            //    }
            //    else
            //    {
            //        ServerPath = Server.MapPath("rptPatientCaseReferral.rpt");
            //    }
            //}
            //else if (Convert.ToInt64(Request.QueryString["Type"]) == 3)
            //{

            //    if (Request.QueryString["PrintFomatID"] == "1")
            //    {
            //        ServerPath = Server.MapPath("rtpEMR_Print.rpt");
            //    }
            //    else if (Request.QueryString["PrintFomatID"] == "2")
            //    {
            //        ServerPath = Server.MapPath("rtpEMR_PrintWthoutHeader.rpt");
            //    }
            //    else
            //    {
            ServerPath = Server.MapPath("rtpNewEMR_Print.rpt");
            //    }
            //}

            myDoc.Load(ServerPath);
            //DateTime CurrentVisitDate;
            //DateTime ToDate;
            long UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            long VisitID = Convert.ToInt64(Request.QueryString["VisitID"]);
            long PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            long PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            long TemplateID = Convert.ToInt64(Request.QueryString["TemplateID"]);
            string ISIvfHistory = Convert.ToString(Request.QueryString["ISIvfHistory"]);
            //if (Convert.ToInt64(Request.QueryString["Type"]) == 3)
            //{
            //    CurrentVisitDate = Convert.ToDateTime(Request.QueryString["CurrentDate"]);
            //    ToDate = CurrentVisitDate.Date.AddDays(1);
            //    myDoc.SetParameterValue("@FromDate", CurrentVisitDate);
            //    myDoc.SetParameterValue("@ToDate", ToDate);
            //}
           // long EMRId = Convert.ToInt64(Request.QueryString["EMRId"]);

            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@VisitID", VisitID);
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
            myDoc.SetParameterValue("@TemplateID", TemplateID);
            myDoc.SetParameterValue("@Status", true);
            myDoc.SetParameterValue("@ISIvfHistory", ISIvfHistory);
           // myDoc.SetParameterValue("@EMRId", EMRId);

            myDoc.SetParameterValue("@UnitId", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();

            Stream oStream = null;
            byte[] byteArray = null;
            oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteArray);
            Response.End();
        }
    }
}