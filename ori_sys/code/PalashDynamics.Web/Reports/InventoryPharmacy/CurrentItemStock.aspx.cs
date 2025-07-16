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
    public partial class CurrentItemStock : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {

            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? ToDate = null;
            DateTime? PrintDate = null;
            string ItemName = null;
            string BatchCode = null;
            long UnitID = 0;
            long UserID = 0;
            long StoreID = 0;
            bool Excel = false;
            

            //Added by AniketK on 23-Oct-2018
            long ItemGroupID = 0;
            long ItemCategoryID = 0;
           

             myDoc = new ReportDocument();
             //myDoc.Load(Server.MapPath("CurrentItemStockReport.rpt"));
             myDoc.Load(Server.MapPath("CurrentItemStockReportWithoutTax.rpt"));

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

            if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "0")
            {
                UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            }

            if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"] != "0")
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }

            if (Request.QueryString["PrintDate"] != null && Request.QueryString["PrintDate"] != "")
            {
                PrintDate = Convert.ToDateTime(Request.QueryString["PrintDate"]);
            }

            //Begin:: Added by AniketK on 10-Dec-2018
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            //End:: Added by AniketK on 10-Dec-2018

            //Begin::Added by aniketK on 23-Oct-2018
            if (Request.QueryString["ItemGroupID"] != null && Request.QueryString["ItemGroupID"] != "0")
            {
                ItemGroupID = Convert.ToInt64(Request.QueryString["ItemGroupID"]);
            }

            if (Request.QueryString["ItemCategoryID"] != null && Request.QueryString["ItemCategoryID"] != "0")
            {
                ItemCategoryID = Convert.ToInt64(Request.QueryString["ItemCategoryID"]);
            }

            
            //End::Added by aniketK on 23-Oct-2018



            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@BatchCode", BatchCode);
            myDoc.SetParameterValue("@ItemName", ItemName);
            myDoc.SetParameterValue("@PrintDate", PrintDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ItemID", null);

            //Begin:: Added by AniketK on 23-OCT-2018
            myDoc.SetParameterValue("@ItemGroupID", ItemGroupID);
            myDoc.SetParameterValue("@ItemCategoryID",ItemCategoryID);  
            //End:: Added by AniketK on 23-OCT-2018


            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //MemoryStream oStream1 = new MemoryStream();
            //oStream1 = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(oStream1.ToArray());
            //Response.End();

            if (Excel == true)
            {
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                //Response.Clear();
                //Response.Buffer = true;
                System.IO.Stream oStream = null;
                byte[] byteArray = null;
                oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                Response.Clear();
                Response.Buffer = true;
                var version = Request.Browser.Version;                
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