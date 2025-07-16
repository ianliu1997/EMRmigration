using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class PaymentDetailsLog : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
                String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
                String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
                String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
                long BillId = 0;
                long UnitId = 0;
                ReportDocument myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("crptPaymentModeDetailReport.rpt"));
                if (Request.QueryString["BillID"] != null)
                {
                    BillId = Convert.ToInt64(Request.QueryString["BillID"]);
                }

                if (Request.QueryString["UnitID"] != null)
                {
                    UnitId = Convert.ToInt64(Request.QueryString["UnitID"]);
                }
                myDoc.SetParameterValue("@UnitID", UnitId);
                myDoc.SetParameterValue("@BillID", BillId);
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
            }
            catch(Exception E)
            {

            }
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