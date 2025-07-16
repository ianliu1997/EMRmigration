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


namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class CurrentItemStock : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {

            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            //DateTime? PrintDate = null;
            DateTime PrintDate = DateTime.Now;
            DateTime? FromDate = null;
            //DateTime? ToDate = null;
            string ItemName = null;
            string BatchCode = null;
            long UnitID = 0;
            long StoreID = 0;
            bool IsStockZero = false;
            bool Excel = false;
            bool IsConsignment = false;
          

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("CurrentItemStockReport.rpt"));

            if (Request.QueryString["ItemName"] != null && Request.QueryString["ItemName"] != "" )
            {
                ItemName = Request.QueryString["ItemName"].ToString();
            }


            if (Request.QueryString["BatchCode"] != null && Request.QueryString["BatchCode"] != "")
            {
                BatchCode = Request.QueryString["BatchCode"];
            }

            if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"] != "0")
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }

            if (Request.QueryString["PrintDate"] != null && Request.QueryString["PrintDate"] != "")
            {
                PrintDate = Convert.ToDateTime(Request.QueryString["PrintDate"]);
            }

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            //if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            //{
            //    ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            //}

            IsStockZero = Convert.ToBoolean(Request.QueryString["IsStockZero"]);

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            IsConsignment = Convert.ToBoolean(Request.QueryString["IsConsignment"]);

            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@BatchCode", BatchCode);
            myDoc.SetParameterValue("@ItemName", ItemName);
            myDoc.SetParameterValue("@PrintDate", PrintDate);
            myDoc.SetParameterValue("@ItemID", null);

            myDoc.SetParameterValue("@IsStockZero", IsStockZero);
            //myDoc.SetParameterValue("@FromDate", FromDate);
            //myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@IsConsignment", IsConsignment);


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