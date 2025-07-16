using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace PalashDynamics.Web.Reports.IVFDashboard
{
    public partial class IVFDashboard_FollicularMonitoring : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long TherapyId = 0;
            long TherapyUnitId = 0;
            long CoupleID = 0;
            long CoupleUnitID = 0;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("Rpt_IVFDashboard_FollicularMonitoring.rpt"));

            if (Request.QueryString["TherapyId"] != null && Request.QueryString["TherapyId"] != "")
            {
                TherapyId = Convert.ToInt64(Request.QueryString["TherapyId"]);

            }
            if (Request.QueryString["TherapyUnitId"] != null && Request.QueryString["TherapyUnitId"] != "")
            {
                TherapyUnitId = Convert.ToInt64(Request.QueryString["TherapyUnitId"]);

            }            
            myDoc.SetParameterValue("@TherapyId", TherapyId);
            myDoc.SetParameterValue("@TherapyUnitId", TherapyUnitId);

            myDoc.SetParameterValue("@TherapyId", TherapyId,"rpt_PatientInfo");
            myDoc.SetParameterValue("@TherapyUnitId", TherapyUnitId,"rpt_PatientInfo");
            myDoc.SetParameterValue("@UnitId", TherapyUnitId, "rpt_Header");
            myDoc.SetParameterValue("@UnitID", TherapyUnitId, "rpt_UnitLogo");
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