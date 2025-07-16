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
using System.Windows.Forms;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class ItemLedgerMIS : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            string ItemIds = null;
            long clinicID = 0;
            long StoreID = 0;
            bool Excel = false;
            long UserID = 0;
            bool IsOldReport = false;
            string ItemName = string.Empty;

            myDoc = new ReportDocument();
            if (Request.QueryString["IsOldReport"] != null)
            {
                IsOldReport = Convert.ToBoolean(Request.QueryString["IsOldReport"]);
            }

            if (!IsOldReport)   
            {
                myDoc.Load(Server.MapPath("rptItemLedgerMIS.rpt"));

                if (Request.QueryString["ItemIDs"] != null && Request.QueryString["ItemIDs"] != "" && Request.QueryString["ItemIDs"] != "0")
                {
                    ItemIds = Request.QueryString["ItemIDs"].ToString();
                }


                if (Request.QueryString["ClinicID"] != null)
                {
                    clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
                }

                if (Request.QueryString["StoreID"] != null)
                {
                    StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
                }

                if (Request.QueryString["ItemName"] != null)
                {
                    ItemName = Request.QueryString["ItemName"];
                }

                if (Request.QueryString["Excel"] != null)
                {
                    Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
                }

                if (Request.QueryString["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Request.QueryString["UserID"]);
                }

                DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
                DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

                myDoc.SetParameterValue("@ItemID", ItemIds);
                myDoc.SetParameterValue("@UnitID", clinicID);
                myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt");
                myDoc.SetParameterValue("@StoreID", StoreID);
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@UserID", UserID);
                myDoc.SetParameterValue("@ItemName", ItemName);
            }
            else  // //Added by Ashish z. as per Discussion with Dr. Gautham(Milann) and Mangesh on dated 28082016 for Old Report.
            {
                myDoc.Load(Server.MapPath("rptItemLedgerMIS_Old.rpt"));

                if (Request.QueryString["ItemIDs"] != null && Request.QueryString["ItemIDs"] != "" && Request.QueryString["ItemIDs"] != "0")
                {
                    ItemIds = Request.QueryString["ItemIDs"].ToString();
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

                if (Request.QueryString["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Request.QueryString["UserID"]);
                }

                DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
                DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

                myDoc.SetParameterValue("@ItemID", ItemIds);
                myDoc.SetParameterValue("@UnitID", clinicID);
                myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt");
                myDoc.SetParameterValue("@StoreID", StoreID);
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@UserID", UserID);
            }


            myDoc.SetParameterValue("@UnitID", clinicID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
        
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