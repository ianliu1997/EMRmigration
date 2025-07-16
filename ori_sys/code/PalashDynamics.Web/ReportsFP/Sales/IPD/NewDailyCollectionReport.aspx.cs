using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using CrystalDecisions.Shared;


namespace PalashDynamics.Web.ReportsFP.Sales.IPD
{
    public partial class NewDailyCollectionReport : System.Web.UI.Page
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
            bool Excel = false;
            long ClinicID = 0;
            long PaymentModeID = 0;
            long BillTypeId = 0;
            long counterID = 0;
            long lUnitId = 0;
            long Collectiontype = 0;
            string SendClinicID = string.Empty;
            long UserID = 0;
            if (Request.QueryString["BillTypeID"] != null)
            {
                BillTypeId = Convert.ToInt64(Request.QueryString["BillTypeID"]);
            }
            myDoc = new ReportDocument();
            //if (BillTypeId == 0) //***//
            // {
            myDoc.Load(Server.MapPath("rptNewDailyCollectionReport.rpt"));
            // }
            // else
            //  {
            // myDoc.Load(Server.MapPath("rptIPDDailyCollectionReport.rpt"));
            // }
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
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            if (Request.QueryString["counterID"] != null)
            {
                counterID = Convert.ToInt64(Request.QueryString["counterID"]);
            }
            if (Request.QueryString["LoginUnitID"] != null)
            {
                lUnitId = Convert.ToInt64(Request.QueryString["LoginUnitID"]);
            }
            if (Request.QueryString["CollectionType"] != null)
            {
                Collectiontype = Convert.ToInt64(Request.QueryString["CollectionType"]);
            }
            if (Request.QueryString["SendClinicID"] != null)
            {
                SendClinicID = Convert.ToString(Request.QueryString["SendClinicID"]);
            }

            if (Request.QueryString["UserID"] != null)
            {
                UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            }

            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ClinicID", ClinicID);
            myDoc.SetParameterValue("@BillTypeId", BillTypeId);
            myDoc.SetParameterValue("@FromDate", FromDate, "refundAmount");
            myDoc.SetParameterValue("@ToDate", ToDate, "refundAmount");
            myDoc.SetParameterValue("@ClinicID", ClinicID, "refundAmount");
            myDoc.SetParameterValue("@counterID", counterID, "refundAmount");
            myDoc.SetParameterValue("@counterID", counterID);
            myDoc.SetParameterValue("@CollectionType", Collectiontype);
            myDoc.SetParameterValue("@SendClinicID", SendClinicID);
            myDoc.SetParameterValue("@UserID", UserID);

            myDoc.SetParameterValue("@UnitID", lUnitId, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", lUnitId, "rptUnitLogo.rpt");//For Unit Logo

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