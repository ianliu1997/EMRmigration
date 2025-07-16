using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using CrystalDecisions.Shared;
using System.IO;

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class HSPurchaseInvoiceWise : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;
            string ItemIds = null;
            long clinic = 0;
            long store = 0;
            long SupplierId = 0;
            bool Excel = false;

            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("HSPurchaseInvoiceWiserpt.rpt"));

           

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

            //Added by pallavi
            if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "")
            {
                clinic = Convert.ToInt64(Request.QueryString["clinic"]);
            }
            if (Request.QueryString["store"] != null && Request.QueryString["store"] != "")
            {
                store = Convert.ToInt64(Request.QueryString["store"]);
            }

            if (Request.QueryString["SupplierId"] != null && Request.QueryString["SupplierId"] != "")
            {
                SupplierId = Convert.ToInt64(Request.QueryString["SupplierId"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
           

          
            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@ToDatePrint", DTP);
            myDoc.SetParameterValue("@SupplierId", SupplierId);

            //Added by pallavi
            myDoc.SetParameterValue("@ItemID", null);
           
            myDoc.SetParameterValue("@StoreID", store);
            myDoc.SetParameterValue("@ClinicId", clinic);
            

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (Excel == true)
            {
                MemoryStream oStream = new MemoryStream();
                oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
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