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
    public partial class RefundAgainstBill : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            long RefundID = 0;
            long UnitID = 0;
            ReportDocument myDoc = new ReportDocument();           

            if (Request.QueryString["PrintFomatID"] == "1")
            {
                myDoc.Load(Server.MapPath("rptRefundAgainstBill.rpt"));
            }
            else if (Request.QueryString["PrintFomatID"] == "2")
            {
                myDoc.Load(Server.MapPath("rptRefundAgainstBillWithoutHeader.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("rptRefundAgainstBill.rpt"));
            }
            if (Request.QueryString["RefundID"] != null)
            {
                RefundID = Convert.ToInt64(Request.QueryString["RefundID"]);
            }

            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            myDoc.SetParameterValue("@RefundID", RefundID);
            myDoc.SetParameterValue("@UnitID", UnitID);
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