using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.IVFDashboard
{
    public partial class IVFDashboard_PatientFISHReport : System.Web.UI.Page
    {
        ReportDocument myDoc, myDoc2;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rpt_IVFDashboard_PGDFISHReport.rpt"));
            long PatientUnitID = 0;
            long PatientID = 0;
          long  OocyteNumber= 0;
          long SerialOocyteNumber = 0;
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
            if (Request.QueryString["OocyteNumber"] != null)
            {
                OocyteNumber = Convert.ToInt64(Request.QueryString["OocyteNumber"]);
            }
            if (Request.QueryString["SerialOocyteNumber"] != null)
            {
                SerialOocyteNumber = Convert.ToInt64(Request.QueryString["SerialOocyteNumber"]);
            }
            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
            myDoc.SetParameterValue("@TherapyID", TherapyID);
            myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID);
            myDoc.SetParameterValue("@OocyteNumber", OocyteNumber);
            myDoc.SetParameterValue("@SerialOocyteNumber", SerialOocyteNumber);

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();

            myDoc2 = new ReportDocument();
            myDoc2.Load(Server.MapPath("rpt_IVFDashboard_PGDKaryotypingReport.rpt"));
            myDoc2.SetParameterValue("@PatientID", PatientID);
            myDoc2.SetParameterValue("@PatientUnitID", PatientUnitID);
            myDoc2.SetParameterValue("@TherapyID", TherapyID);
            myDoc2.SetParameterValue("@TherapyUnitID", TherapyUnitID);
            myDoc2.SetParameterValue("@OocyteNumber", OocyteNumber);
            myDoc2.SetParameterValue("@SerialOocyteNumber", SerialOocyteNumber);

            myDoc2.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc2);

            CrystalReportViewer2.ReportSource = myDoc2;

            CrystalReportViewer2.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer2.DataBind();
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