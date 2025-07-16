using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.Patient
{
    public partial class PatientBillTransactionHistoryReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPatientBillTransactionHistoryReport.rpt"));
            string MrNo = string.Empty;
            DateTime? TransactionDate = null;
            string BillType = string.Empty;
            string PaymentBilltype = string.Empty;
            long UnitID = 0;

            if (Request.QueryString["MrNo"] != null)
            {
                MrNo = Convert.ToString(Request.QueryString["MrNo"]);
            }
            if (Request.QueryString["TransactionDate"] != null && Request.QueryString["TransactionDate"] != "")
            {
                TransactionDate = Convert.ToDateTime(Request.QueryString["TransactionDate"]);
            }
            if (Request.QueryString["BillType"] != null)
            {
                BillType = Convert.ToString(Request.QueryString["BillType"]);
            }
            if (Request.QueryString["PaymentBilltype"] != null)
            {
                PaymentBilltype = Convert.ToString(Request.QueryString["PaymentBilltype"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            myDoc.SetParameterValue("@MrNo", MrNo);
            myDoc.SetParameterValue("@TransactionDate", TransactionDate);
            myDoc.SetParameterValue("@BillType", BillType);
            myDoc.SetParameterValue("@PaymentBillType", PaymentBilltype);

            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

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