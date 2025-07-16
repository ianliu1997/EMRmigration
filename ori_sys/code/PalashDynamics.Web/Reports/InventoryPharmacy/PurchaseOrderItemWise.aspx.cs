using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CrystalDecisions.Shared;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class PurchaseOrderItemWise : System.Web.UI.Page
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
            long ClinicID = 0;
            long UserID = 0;
            long StoreID = 0;
            long SupplierID = 0;
            bool GRNWithItem = false;
            bool Excel = false;
            string ItemName = string.Empty;
            
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            myDoc.Load(Server.MapPath("PurchaseOrderListItemWise.rpt"));

                if (Request.QueryString["ItemIDs"] != null && Request.QueryString["ItemIDs"] != "" && Request.QueryString["ItemIDs"] != "0")
                {
                    ItemIds = Request.QueryString["ItemIDs"].ToString();
                }

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
                    ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
                }

                if (Request.QueryString["StoreID"] != null)
                {
                    StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
                }
                if (Request.QueryString["SupplierID"] != null)
                {
                    SupplierID = Convert.ToInt64(Request.QueryString["SupplierID"]);
                }

                if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "0")
                {
                    UserID = Convert.ToInt64(Request.QueryString["UserID"]);
                }

                if (Request.QueryString["ItemName"] != null)
                {
                    ItemName = Request.QueryString["ItemName"];
                }

                myDoc.SetParameterValue("@ItemId", ItemIds);
                myDoc.SetParameterValue("@FromDate", DTF);
                myDoc.SetParameterValue("@ToDate", DTT);
              //  myDoc.SetParameterValue("@ToDatePrint", DTP);
                myDoc.SetParameterValue("@UnitID", ClinicID);
                myDoc.SetParameterValue("@UserID", UserID);
                myDoc.SetParameterValue("@StoreID", StoreID);
                myDoc.SetParameterValue("@SupplierID", SupplierID);

                myDoc.SetParameterValue("@ItemName", ItemName);
                myDoc.SetParameterValue("@UnitID", ClinicID, "rptCommonReportHeader.rpt");

                myDoc.SetParameterValue("@UnitID", ClinicID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
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
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(oStream.ToArray());
                Response.End();
            }

            //Stream oStream1 = null;
            //byte[] byteArray = null;
            //oStream1 = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //byteArray = new byte[oStream1.Length];
            //oStream1.Read(byteArray, 0, Convert.ToInt32(oStream1.Length - 1));

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(byteArray);
            //Response.End();
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