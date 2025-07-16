using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.Sales
{
    public partial class DoctorWise_Revenue : System.Web.UI.Page
    {
        ReportDocument myDoc;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string FromDate = null;
            string ToDate = null;

            DateTime? FrDT = null;
            DateTime? ToDT = null;

            long lUnitId = 0;

            DateTime? ToDatePrint = null;
            long clinicID = 0;
            long UnitID = 0, DoctorID = 0, DrType = 0;
            string SendClinicID = string.Empty;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptDoctorWiseRevenue.rpt"));
            FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            ToDate = Convert.ToString(Request.QueryString["ToDate"]);


            if (!string.IsNullOrEmpty(FromDate))
            {
                FrDT = Convert.ToDateTime(FromDate);
            }

            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDT = Convert.ToDateTime(ToDate);
                // ToDP = ToDT;
            }


          
            //if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            //{
            //    FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            //}
            //if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            //{
            //    ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            //}

            //if (Request.QueryString["ClinicID"] != null && Request.QueryString["ClinicID"] != "")
            //{
            //    UnitID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            //}
            if (Request.QueryString["ClinicID"] != null)
            {
                clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["DoctorID"] != null && Request.QueryString["DoctorID"] != "")
            {
                DoctorID = Convert.ToInt64(Request.QueryString["DoctorID"]);
            }
            if (Request.QueryString["DrType"] != null && Request.QueryString["DrType"] != "")
            {
                DrType = Convert.ToInt64(Request.QueryString["DrType"]);
            }

            if (Request.QueryString["SendClinicID"] != null)
            {
                SendClinicID = Convert.ToString(Request.QueryString["SendClinicID"]);
            }
            if (Request.QueryString["LoginUnitID"] != null)
            {
                lUnitId = Convert.ToInt64(Request.QueryString["LoginUnitID"]);
            }
            ///////////////////////////
        
            myDoc.SetParameterValue("@FromDate", FrDT);//
            myDoc.SetParameterValue("@ToDate", ToDT);//
          //  myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
            myDoc.SetParameterValue("@DoctorID", DoctorID);//
            myDoc.SetParameterValue("@DrType", DrType);//
            myDoc.SetParameterValue("@UnitID", clinicID);//

            myDoc.SetParameterValue("@SendClinicID", SendClinicID);//



            myDoc.SetParameterValue("@UnitID", lUnitId,"rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@UnitID", lUnitId, "rptUnitLogo.rpt");

         

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