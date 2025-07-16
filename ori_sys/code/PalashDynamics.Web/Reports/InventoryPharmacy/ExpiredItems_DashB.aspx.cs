using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class ExpiredItems_DashB : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            bool Excel = false;

            myDoc = new ReportDocument();

            long Days = Convert.ToInt64(Request.QueryString["Days"]);
            long StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
           // string ItemName = Convert.ToString(Request.QueryString["ItemName"]);
            long UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            long UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            DateTime ExpiryFromDate = Convert.ToDateTime(Request.QueryString["ExpiryFromDate"]);
            DateTime ExpiryToDate = Convert.ToDateTime(Request.QueryString["ExpiryToDate"]);
           
            //Begin::Added by AniketK on 26-Oct-2018
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            //End::Added by AniketK on 26-Oct-2018

            myDoc.Load(Server.MapPath("rptExpiredItems_DashB.rpt"));
         //   myDoc.SetParameterValue("@Days", Days);
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@ExpiryFromDate", ExpiryFromDate);
            myDoc.SetParameterValue("@ExpiryToDate", ExpiryToDate);

            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
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
                var version = Request.Browser.Version;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(byteArray);
                Response.End();
            }
            //MemoryStream oStream = new MemoryStream();
            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(oStream.ToArray());
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