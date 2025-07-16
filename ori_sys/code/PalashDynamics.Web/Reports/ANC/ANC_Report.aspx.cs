using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.ANC
{
    public partial class ANC_Report : System.Web.UI.Page
    {
        ReportDocument myDoc1;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc1 = new ReportDocument();
            myDoc1.Load(Server.MapPath("rpt_ANC_Report.rpt"));

            long PatientID = 0;
            long PatientUnitID = 0;
            long AncId = 0;

            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }
            if (Request.QueryString["PatientUnitID"] != null)
            {
                PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }
            if (Request.QueryString["AncId"] != null)
            {
                AncId = Convert.ToInt64(Request.QueryString["AncId"]);
            }
            myDoc1.SetParameterValue("@PatientID", PatientID);
            myDoc1.SetParameterValue("@PatientUnitID", PatientUnitID);
            myDoc1.SetParameterValue("@AncId", AncId);

            myDoc1.SetParameterValue("@PatientID", PatientID, "Rpt_History");
            myDoc1.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_History");
            myDoc1.SetParameterValue("@AncId", AncId, "Rpt_History");

            myDoc1.SetParameterValue("@PatientID", PatientID, "Rpt_SubHistory");
            myDoc1.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_SubHistory");
            myDoc1.SetParameterValue("@AncId", AncId, "Rpt_SubHistory");

            myDoc1.SetParameterValue("@PatientID", PatientID, "Rpt_Investigation");
            myDoc1.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_Investigation");
            myDoc1.SetParameterValue("@AncId", AncId, "Rpt_Investigation");

            myDoc1.SetParameterValue("@PatientID", PatientID, "Rpt_USG");
            myDoc1.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_USG");
            myDoc1.SetParameterValue("@AncId", AncId, "Rpt_USG");

            myDoc1.SetParameterValue("@PatientID", PatientID, "Rpt_Examination");
            myDoc1.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_Examination");
            myDoc1.SetParameterValue("@AncId", AncId, "Rpt_Examination");

            myDoc1.SetParameterValue("@PatientID", PatientID, "Rpt_Suggestion");
            myDoc1.SetParameterValue("@PatientUnitID", PatientUnitID, "Rpt_Suggestion");
            myDoc1.SetParameterValue("@AncId", AncId, "Rpt_Suggestion");

            myDoc1.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc1);
            CrystalReportViewer1.ReportSource = myDoc1;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

        }
        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            try
            {
                if (myDoc1 != null)
                    myDoc1.Dispose();
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