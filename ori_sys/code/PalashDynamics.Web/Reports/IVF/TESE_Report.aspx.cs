using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.IVF
{
    public partial class TESE_Report : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rpt_TESE_Report.rpt"));
            long TESE_UnitID = 0;
            long TESEID = 0;
            if (Request.QueryString["TESE_UnitID"] != null)
            {
                TESE_UnitID = Convert.ToInt64(Request.QueryString["TESE_UnitID"]);
            }
            if (Request.QueryString["TESEID"] != null)
            {
                TESEID = Convert.ToInt64(Request.QueryString["TESEID"]);
            }
            myDoc.SetParameterValue("@TESEID", TESEID);
            myDoc.SetParameterValue("@TESEUnitID", TESE_UnitID);
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
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