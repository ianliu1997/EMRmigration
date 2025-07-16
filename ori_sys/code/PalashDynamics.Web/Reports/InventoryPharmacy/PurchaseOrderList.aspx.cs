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
    public partial class PurchaseOrderList : System.Web.UI.Page
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
            string SupplierIds = null;
            DateTime? DTP = null;
            long UnitID = 0;
            long UserID = 0;
            long StoreID = 0;
            bool Excel = false;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPurchaseOrderList.rpt"));

            if (Request.QueryString["SupplierIDs"] != null && Request.QueryString["SupplierIDs"] != "" && Request.QueryString["SupplierIDs"] != "0")
            {
                SupplierIds = Request.QueryString["SupplierIDs"].ToString();
            }

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                DTT = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["Excel"] != null && Request.QueryString["Excel"] != "")
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);

            if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "0")
            {
                UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            }

            if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"] != "0")
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }

            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@SupplierID", SupplierIds);
            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            //myDoc.SetParameterValue("@ToDatePrint", DTP);
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (Excel)
            {
                Stream stream = null;
                MemoryStream oStream = new MemoryStream();
                stream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                stream.CopyTo(oStream);
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