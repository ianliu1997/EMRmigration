using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class OpeningBalance : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;

            long Clinic = 0;
            long UserID = 0;
            long Store = 0;
            bool IsExcel = false;


            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("OpeningBalanceReport.rpt"));

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                DTT = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                DTP = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }

            if (Request.QueryString["ClinicID"] != null)
            {
                Clinic = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["UserID"] != null)
            {
                UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            }
            if (Request.QueryString["StoreID"] != null)
            {
                Store = Convert.ToInt64(Request.QueryString["StoreID"]);
            }

            if (Request.QueryString["IsExcel"] != null)
            {
                IsExcel = Convert.ToBoolean(Request.QueryString["IsExcel"]);
            }

            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@StoreID", Store);
            myDoc.SetParameterValue("@UnitID", Clinic);
            myDoc.SetParameterValue("@UnitID", Clinic, "sbrptOpeningBalance");
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@ToDatePrint", DTP);


            myDoc.SetParameterValue("@UnitID", Clinic, "rptUnitLogo.rpt"); //Added by ajit jadhav date 30/9/2016 Get Unit LOGO 
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //MemoryStream oStream = new MemoryStream();
            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);

            if (IsExcel)
            {
                Stream stream = null;
                MemoryStream oStream = new MemoryStream();
                stream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                stream.CopyTo(oStream);
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(oStream.ToArray());
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