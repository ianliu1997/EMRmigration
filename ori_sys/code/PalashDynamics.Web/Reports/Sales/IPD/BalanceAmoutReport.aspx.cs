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

namespace PalashDynamics.Web.Reports.Sales.IPD
{
    public partial class BalanceAmoutReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;

            long ClinicID = 0;
            long BillTypeId = 0;
            long CreditGivenBy = 0;
            bool Excel = false;
            long counterID = 0;
            long SorceID = 0;
            long lUnitId = 0;
            string SendClinicID = string.Empty;
            long IsOutstandingReport = 0; //Added by AniketK on 25Feb2019

            if (Request.QueryString["BillTypeID"] != null)
            {
                BillTypeId = Convert.ToInt64(Request.QueryString["BillTypeID"]);
            }


            myDoc = new ReportDocument();

            if (BillTypeId == 0)
            {
                myDoc.Load(Server.MapPath("rptBalanceAmountReport.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("rptIPDBalanceAmountReport.rpt"));
            }

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            //if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            //{
            //    ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            //}
            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }

            if (Request.QueryString["CreditGivenBy"] != null)
            {
                CreditGivenBy = Convert.ToInt64(Request.QueryString["CreditGivenBy"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }



            if (Request.QueryString["SorceID"] != null)
            {
                SorceID = Convert.ToInt64(Request.QueryString["SorceID"]);
            }
            if (Request.QueryString["counterID"] != null)
            {
                counterID = Convert.ToInt64(Request.QueryString["counterID"]);
            }

            if (Request.QueryString["LoginUnitID"] != null)
            {
                lUnitId = Convert.ToInt64(Request.QueryString["LoginUnitID"]);
            }

            if (Request.QueryString["SendClinicID"] != null)
            {
                SendClinicID = Convert.ToString(Request.QueryString["SendClinicID"]);
            }

            //Begin :: Added by AniketK on 25Feb2019
            if (Request.QueryString["IsOutstandingReport"] != null)
            {
                IsOutstandingReport = Convert.ToInt64(Request.QueryString["IsOutstandingReport"]);
            }
            //End :: Added by AniketK on 25Feb2019

            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);

            myDoc.SetParameterValue("@ClinicID", ClinicID);
            myDoc.SetParameterValue("@UserID", CreditGivenBy);
            myDoc.SetParameterValue("@BillTypeId", BillTypeId);
            myDoc.SetParameterValue("@SorceID", SorceID);
            myDoc.SetParameterValue("@counterID", counterID);
            myDoc.SetParameterValue("@SendClinicID", SendClinicID);
            myDoc.SetParameterValue("@IsOutstandingReport", IsOutstandingReport); //Added by AniketK on 25Feb2019

            myDoc.SetParameterValue("@UnitID", lUnitId, "rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@UnitID", lUnitId, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //if (Excel == true)
            //{
            //    MemoryStream oStream = new MemoryStream();
            //    oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.BinaryWrite(oStream.ToArray());
            //    Response.End();

            //}


            if (Excel == true)
            {
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);

                System.IO.Stream oStream = null;
                byte[] byteArray = null;
                oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";

                //Response.BinaryWrite(oStream.ToArray());
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