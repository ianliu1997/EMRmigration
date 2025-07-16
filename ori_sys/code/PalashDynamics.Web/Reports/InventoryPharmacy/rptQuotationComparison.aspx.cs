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
    public partial class rptQuotationComparison1 : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            ReportDocument myDoc = new ReportDocument();
          //  long StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            string QuotationIDs = Convert.ToString(Request.QueryString["QuotationIDs"]);
        //    string ItemIDs = Convert.ToString(Request.QueryString["ItemIDs"]);
            long UnitID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            myDoc.Load(Server.MapPath("rptQuotationCMONew.rpt"));
            myDoc.SetParameterValue("@QuotationIDs", QuotationIDs);
            //myDoc.SetParameterValue("@ItemIDs", ItemIDs);
            //myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@UnitID", UnitID, "sbrptQuotationComparison");

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