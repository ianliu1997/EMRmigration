using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;

namespace PalashDynamics.Web.Reports.IVFDashboard
{
    public partial class IVFDashboard_PatientPGDReport : System.Web.UI.Page
    {
        ReportDocument myDoc, myDoc2;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string imgVirtualDirectory = "https://localhost/" + System.Configuration.ConfigurationManager.AppSettings["IVFImgVirtualDir"] + "/";

            myDoc = new ReportDocument();

            //if (Request.QueryString["PrintFomatID"] == "1")
            //{
            //    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_PGDReport.rpt"));
            //}
            //else if (Request.QueryString["PrintFomatID"] == "2")
            //{
            //    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_PGDReportWithoutHeader.rpt"));
            //}
            //else
            //{
            //    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_PGDReport.rpt"));
            //}

            long PatientUnitID = 0;
            long PatientID = 0;

            long TherapyID = 0;
            long TherapyUnitID = 0;

            if (Request.QueryString["PatientUnitID"] != null)
            {
                PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }
            if (Request.QueryString["TherapyId"] != null)
            {
                TherapyID = Convert.ToInt64(Request.QueryString["TherapyId"]);
            }
            if (Request.QueryString["TherapyUnitId"] != null)
            {
                TherapyUnitID = Convert.ToInt64(Request.QueryString["TherapyUnitId"]);
            }
            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }

            myDoc.Load(Server.MapPath("Rpt_IVFDashboard_NewPGDReport.rpt"));           

            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
            myDoc.SetParameterValue("@TherapyID", TherapyID);
            myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID);

            myDoc.SetParameterValue("@ImgDirectory", imgVirtualDirectory,"rpt_EmbryoDetails");

            myDoc.SetParameterValue("@UnitID", TherapyUnitID, "rptUnitLogo.rpt");
            myDoc.SetParameterValue("@UnitID", TherapyUnitID, "rptCommonReportHeader.rpt");

            

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();

            //Stream oStream = null;
            //byte[] byteArray = null;
            //oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //byteArray = new byte[oStream.Length];
            //oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(byteArray);
            //Response.End();

            //myDoc2 = new ReportDocument();

            //if (Request.QueryString["PrintFomatID"] == "1")
            //{
            //    myDoc2.Load(Server.MapPath("rpt_IVFDashboard_PGDEMBReport.rpt"));
            //}
            //else if (Request.QueryString["PrintFomatID"] == "2")
            //{
            //    myDoc2.Load(Server.MapPath("rpt_IVFDashboard_PGDEMBReportWithoutHeader.rpt"));
            //}
            //else
            //{
            //    myDoc2.Load(Server.MapPath("rpt_IVFDashboard_PGDEMBReport.rpt"));
            //}

            //myDoc2.Load(Server.MapPath("Rpt_IVFDashboard_NewPGDReport.rpt"));

            //myDoc.SetParameterValue("@UnitId", TherapyUnitID, "rptCommonReportHeader");
            //myDoc.SetParameterValue("@UnitID", TherapyUnitID, "rptUnitLogo");

            //myDoc2.SetParameterValue("@PatientID", PatientID);
            //myDoc2.SetParameterValue("@PatientUnitID", PatientUnitID);
            //myDoc2.SetParameterValue("@TherapyID", TherapyID);
            //myDoc2.SetParameterValue("@TherapyUnitID", TherapyUnitID);

            //myDoc2.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            //CReportAuthentication.Impersonate(myDoc2);

            //CrystalReportViewer2.ReportSource = myDoc2;

            //CrystalReportViewer2.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            //CrystalReportViewer2.DataBind();
        }
    }
}