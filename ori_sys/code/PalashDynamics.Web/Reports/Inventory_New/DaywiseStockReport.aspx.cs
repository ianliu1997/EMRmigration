using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using System.Configuration;

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class DaywiseStockReport : System.Web.UI.Page
    {
        ReportDocument myDoc;

        protected void Page_Load(object sender, EventArgs e)
        {

            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? DTF = null;
            long clinicID = 0;
            long StoreID = 0;
            bool Excel = false;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptDaywiseStockReport.rpt"));

            if (Request.QueryString["StockDate"] != null && Request.QueryString["StockDate"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["StockDate"]);
            }
            if (Request.QueryString["ClinicID"] != null)
            {
                clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["StoreID"] != null)
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            myDoc.SetParameterValue("@UnitID", clinicID);
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@StockDate", DTF);
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