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
    public partial class ExpiredReturnItem : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();

            long str = Convert.ToInt64(Request.QueryString["ID"]);
            long str1 = Convert.ToInt64(Request.QueryString["UnitId"]);

            myDoc.Load(Server.MapPath("rpt_ExpiredItemReturn.rpt"));
            myDoc.SetParameterValue("@ID", str);
            myDoc.SetParameterValue("@UnitId", str1);
            //myDoc.SetParameterValue("@PatientId", PatientId, "SubRptPatientDetails"); 
            myDoc.SetParameterValue("@UnitId", str1, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", str1, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO

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