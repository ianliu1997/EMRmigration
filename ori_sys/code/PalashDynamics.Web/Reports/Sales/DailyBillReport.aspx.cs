using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.Sales
{
    public partial class DailyBillReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            bool Excel = false;
           
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            long ClinicID = 0;
            long DoctorID = 0;
            long DepartmentID = 0;
            bool GroupBy = false; 

            myDoc = new ReportDocument();
            long BillTypeId = 0;
            long BillInitialID = 0;
            long counterID = 0;
            long lUnitId = 0;
            string SendClinicID = string.Empty;

            if (Request.QueryString["BillTypeID"] != null)
            {
                BillTypeId = Convert.ToInt64(Request.QueryString["BillTypeID"]);
            }
            if (BillTypeId == 0)
            {
                myDoc.Load(Server.MapPath("DoctorBasedConsultationBillReport.rpt"));
            }  
            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["DepartmentID"] != null)
            {
                DepartmentID = Convert.ToInt64(Request.QueryString["DepartmentID"]);
            }
            if (Request.QueryString["DoctorID"] != null)
            {
                DoctorID = Convert.ToInt64(Request.QueryString["DoctorID"]);
            }
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            if (Request.QueryString["BillInitialID"] != null)
            {
                BillInitialID = Convert.ToInt64(Request.QueryString["BillInitialID"]);
            }
            if (Request.QueryString["counterID"] != null)
            {
                counterID = Convert.ToInt64(Request.QueryString["counterID"]);
            }
            if (Request.QueryString["CategoryWise"] != null)
            {
                GroupBy = Convert.ToBoolean(Request.QueryString["CategoryWise"]);
            }
            if (Request.QueryString["LoginUnitID"] != null)
            {
                lUnitId = Convert.ToInt64(Request.QueryString["LoginUnitID"]);
            }
            if (Request.QueryString["SendClinicID"] != null)
            {
                SendClinicID = Convert.ToString(Request.QueryString["SendClinicID"]);
            }
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ClinicID", ClinicID);
            myDoc.SetParameterValue("@DepartmentID", DepartmentID);
            myDoc.SetParameterValue("@DoctorID", DoctorID);
            myDoc.SetParameterValue("@BillTypeId", BillTypeId);
            myDoc.SetParameterValue("@GroupBy", GroupBy);
            myDoc.SetParameterValue("@BillInitialID", BillInitialID);
            myDoc.SetParameterValue("@counterID", counterID);
            myDoc.SetParameterValue("@SendClinicID", SendClinicID);
            myDoc.SetParameterValue("@UnitID", lUnitId, "rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@UnitID", lUnitId, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            if (Excel == true)
            {                
                System.IO.Stream oStream = null;
                byte[] byteArray = null;
                oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(byteArray);
                Response.End();
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