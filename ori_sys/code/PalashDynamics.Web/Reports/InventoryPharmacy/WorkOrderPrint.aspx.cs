using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class WorkOrderPrint : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string POid = null;
            long PClinicID = 0;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptWorkOrderPrint.rpt"));

            if (Request.QueryString["POId"] != null && Request.QueryString["POId"] != "" && Request.QueryString["POId"] != "0")
            {
                POid = Request.QueryString["POId"].ToString();
            }

            if (Request.QueryString["PUnitID"] != null)
            {
                PClinicID = Convert.ToInt64(Request.QueryString["PUnitID"]);
            }

            myDoc.SetParameterValue("@ID", POid);
            myDoc.SetParameterValue("@UnitID", PClinicID);

            myDoc.SetParameterValue("@UnitID", PClinicID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
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