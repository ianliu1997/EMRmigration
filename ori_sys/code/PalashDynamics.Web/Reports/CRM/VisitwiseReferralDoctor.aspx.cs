using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class VisitwiseReferralDoctor : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath;
            string strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            //DateTime FromDate = DateTime.Now.Date;
            //DateTime ToDate = DateTime.Now.Date;

            String strReportName = "Visit Wise Referral Doctor";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            string FromDate = null;
            string ToDate = null;
            string ToDatePrint = null;


            DateTime? FrDT = null;
            DateTime? ToDT = null;
            DateTime? ToDP = null;

            long ReferredDoctorID = 0;
            long UnitId = 0;
            bool IsExporttoExcel = false;
            string AppPath = Request.ApplicationPath;

            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("VisitwiseReferralDoctor1.rpt"));

            FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            ToDate = Convert.ToString(Request.QueryString["ToDate"]);

            IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);

            if (!string.IsNullOrEmpty(FromDate))
            {
                FrDT = Convert.ToDateTime(FromDate);
            }

            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDT = Convert.ToDateTime(ToDate);
                // ToDP = ToDT;
            }
            if (!string.IsNullOrEmpty(ToDatePrint))
            {
                ToDP = Convert.ToDateTime(ToDatePrint);
            }

            if (Request.QueryString["DoctorID"] != null)
            {
                ReferredDoctorID = Convert.ToInt64(Request.QueryString["DoctorID"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitId = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            myDoc.SetParameterValue("@FromDate", FrDT);
            myDoc.SetParameterValue("@ToDate", ToDT);
         //   long lUnitId =
            myDoc.SetParameterValue("@ReferredDoctorID", ReferredDoctorID);
            //    myDoc.SetParameterValue("@ToDatePrint", ToDP);
            myDoc.SetParameterValue("@UnitID", UnitId, "rptCommonReportHeader.rpt");
            // myDoc.SetParameterValue("@UnitID", lUnitId, "Header_SubReport");

            myDoc.SetParameterValue("@UnitID", UnitId, "rptUnitLogo.rpt");

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