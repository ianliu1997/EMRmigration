using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.Sales
{
	public partial class DoctorWisePatientDetail : System.Web.UI.Page
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
            long DoctorID = 0;
            long DepartmentID = 0;
            long UnitID = 0;
            string MRNO = null;
            string FirstName = null;
            string MiddleName = null;
            string LastName = null;
            Int32 rptid = 0;
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
            if (Request.QueryString["DoctorID"] != null)
            {
                DoctorID = Convert.ToInt64(Request.QueryString["DoctorID"]);
            }
            if (Request.QueryString["DepartmentID"] != null)
            {
                DepartmentID = Convert.ToInt64(Request.QueryString["DepartmentID"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);

            }
            if (Request.QueryString["MRNO"] != null && Request.QueryString["MRNO"] != "")
            {
                MRNO = Convert.ToString(Request.QueryString["MRNO"]);
            }
            if (Request.QueryString["FirstName"] != null && Request.QueryString["FirstName"] != "")
            {
                FirstName = Convert.ToString(Request.QueryString["FirstName"]);
            }
            if (Request.QueryString["MiddleName"] != null && Request.QueryString["MiddleName"] != "")
            {
                MiddleName = Convert.ToString(Request.QueryString["MiddleName"]);
            }
            if (Request.QueryString["LastName"] != null && Request.QueryString["LastName"] != "")
            {
                LastName = Convert.ToString(Request.QueryString["LastName"]);
            }
            if (Request.QueryString["rptid"] != null)
            {
                rptid = Convert.ToInt32(Request.QueryString["rptid"]);
            }
            if (rptid == 1)
            {
                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("DoctorWisePatientDetailsRpt.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
                myDoc.SetParameterValue("@DoctorID", DoctorID);
                myDoc.SetParameterValue("@SpecialisationID", DepartmentID);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
            }
            else if (rptid == 2)
            {
                ReportDocument myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("PatientWiseDoctorDetailsRpt.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
                myDoc.SetParameterValue("@MRNO", MRNO);
                myDoc.SetParameterValue("@FirstName", FirstName);
                myDoc.SetParameterValue("@MiddleName", MiddleName);
                myDoc.SetParameterValue("@LastName", LastName);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
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