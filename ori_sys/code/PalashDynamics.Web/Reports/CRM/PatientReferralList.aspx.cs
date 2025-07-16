using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class PatientReferralList : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {

            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            DateTime? ToDatePrint = null;
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            string ServiceName = null;
            long Specialization = 0;
            long SubSpecialization = 0;
            long clinic = 0;
            long ReferralType = 0;
      
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("ReferralPatientListRpt.rpt"));
            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);

            }
            if (Request.QueryString["ServiceName"] != null && Request.QueryString["ServiceName"] != "")
            {
                ServiceName = Convert.ToString(Request.QueryString["ServiceName"]);

            }
            if (Request.QueryString["Specialization"] != null && Request.QueryString["Specialization"] != "")
            {
                Specialization = Convert.ToInt64(Request.QueryString["Specialization"]);

            }
            if (Request.QueryString["SubSpecialization"] != null && Request.QueryString["SubSpecialization"] != "")
            {
                SubSpecialization = Convert.ToInt64(Request.QueryString["SubSpecialization"]);

            }
            if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "")
            {
                clinic = Convert.ToInt64(Request.QueryString["clinic"]);

            }
            if (Request.QueryString["ReferralType"] != null && Request.QueryString["ReferralType"] != "")
            {
                ReferralType = Convert.ToInt64(Request.QueryString["ReferralType"]);
            }
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
            myDoc.SetParameterValue("@ServiceName", ServiceName);
            myDoc.SetParameterValue("@Specialization", Specialization);
            myDoc.SetParameterValue("@SubSpecialization", SubSpecialization);
            myDoc.SetParameterValue("@clinic", clinic);
            myDoc.SetParameterValue("@ReferralType", ReferralType);
            myDoc.SetParameterValue("@UnitID", clinic, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", clinic, "rptUnitLogo.rpt");
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