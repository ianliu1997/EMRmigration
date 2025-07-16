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
    public partial class PatientBillsHistory : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rpt_PatientBillsHistory.rpt"));
            string MrNo = string.Empty;

            DateTime? TransactionDate = null;
            string BillType = string.Empty;
            string PaymentBilltype = string.Empty;
            long PatientUnitID = 0;
            long PatientID = 0;

            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }
            if (Request.QueryString["PatientUnitID"] != null && Request.QueryString["PatientUnitID"] != "")
            {
                PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }
            //if (Request.QueryString["BillType"] != null)
            //{
            //    BillType = Convert.ToString(Request.QueryString["BillType"]);
            //}
            //if (Request.QueryString["PaymentBilltype"] != null)
            //{
            //    PaymentBilltype = Convert.ToString(Request.QueryString["PaymentBilltype"]);
            //}
            //if (Request.QueryString["UnitID"] != null)
            //{
            //    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            //}

            myDoc.SetParameterValue("@PatientID", PatientID);
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
            //myDoc.SetParameterValue("@BillType", BillType);
            //myDoc.SetParameterValue("@PaymentBillType", PaymentBilltype);

            //myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
            //myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

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