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

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class ReOrderLevel : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            ReportDocument myDoc = new ReportDocument();

            long StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            long SuplierID = Convert.ToInt64(Request.QueryString["SuplierID"]);
            string ItemName = Convert.ToString(Request.QueryString["ItemName"]);
            long UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            long UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            myDoc.Load(Server.MapPath("rptReOrderLevel.rpt"));
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@SuplierID", SuplierID);
            myDoc.SetParameterValue("@ItemName", ItemName);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            MemoryStream oStream = new MemoryStream();
            oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();
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