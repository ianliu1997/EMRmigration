using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class BlockStockCheck : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long StoreLocationID = 0;
            long StoreLocationUnitID = 0;
            myDoc.Load(Server.MapPath("rptBlockStockCheck.rpt"));

            if (Request.QueryString["StoreLocationID"] != null)
            {
                StoreLocationID = Convert.ToInt64(Request.QueryString["StoreLocationID"]);
            }

            if (Request.QueryString["StoreLocationUnitID"] != null)
            {
                StoreLocationUnitID = Convert.ToInt64(Request.QueryString["StoreLocationUnitID"]);
            }

            myDoc.SetParameterValue("@StoreLocationID", StoreLocationID);
            myDoc.SetParameterValue("@StoreLocationUnitID", StoreLocationUnitID);

            myDoc.SetParameterValue("@UnitID", StoreLocationUnitID, "rptCommonReportHeader.rpt");

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();

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