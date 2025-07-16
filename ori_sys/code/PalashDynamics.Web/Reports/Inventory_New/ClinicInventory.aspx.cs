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
    public partial class ClinicInventory : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long ItemCategory = 0;
            long ClinicID = 0;
            myDoc.Load(Server.MapPath("rptClinicInventory.rpt"));



            if (Request.QueryString["ItemCategory"] != null)
            {
                ItemCategory = Convert.ToInt64(Request.QueryString["ItemCategory"]);
            }

            if (Request.QueryString["Clinic"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["Clinic"]);
            }


            myDoc.SetParameterValue("@ItemCategory", ItemCategory);
            myDoc.SetParameterValue("@UnitID", ClinicID);



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