using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.OperationTheatre
{
    public partial class ProceduralClearance : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptOTProceduralClearance.rpt"));            
            long OTBillID = 0;         
            long unitid = 0;
            try
            {
                OTBillID = Convert.ToInt64(Request.QueryString["OTBillID"]);                            
                unitid = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            catch (Exception) { }

            //header
            myDoc.SetParameterValue("@OTBillID", OTBillID);
            myDoc.SetParameterValue("@UnitID", unitid);           
            myDoc.SetParameterValue("@UnitID", unitid, "rptCommonReportHeader.rpt");           
            myDoc.SetParameterValue("@UnitID", unitid, "rptUnitLogo.rpt");

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