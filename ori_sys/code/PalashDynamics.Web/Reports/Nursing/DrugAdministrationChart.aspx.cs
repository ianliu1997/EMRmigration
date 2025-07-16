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


namespace PalashDynamics.Web.Reports.Nursing
{
    public partial class DrugAdministrationChart : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long PrescriptionID = 0;
            long PrescriptionUnitID = 0;            
            long UnitID = 0;

            long Opd_Ipd_Id = 0;
            long Opd_Ipd_UnitID = 0;
            long OPD_IPD = 0;

            ReportDocument myDoc = new ReportDocument();

            myDoc.Load(Server.MapPath("Rpt_DrugAdministrationChart.rpt"));

            if (Request.QueryString["PrescriptionID"] != null && Request.QueryString["PrescriptionID"] != "")
            {
                PrescriptionID = Convert.ToInt64(Request.QueryString["PrescriptionID"]);
            }
            if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "")
            {
                PrescriptionUnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

         
            if (Request.QueryString["Opd_Ipd_Id"] != null && Request.QueryString["Opd_Ipd_Id"] != "")
            {
                Opd_Ipd_Id = Convert.ToInt64(Request.QueryString["Opd_Ipd_Id"]);
            }
            if (Request.QueryString["Opd_Ipd_UnitID"] != null && Request.QueryString["Opd_Ipd_UnitID"] != "")
            {
                Opd_Ipd_UnitID = Convert.ToInt64(Request.QueryString["Opd_Ipd_UnitID"]);
            }
            if (Request.QueryString["OPD_IPD"] != null && Request.QueryString["OPD_IPD"] != "")
            {
                OPD_IPD = Convert.ToInt64(Request.QueryString["OPD_IPD"]);
            }

            myDoc.SetParameterValue("@PrescriptionID", PrescriptionID);
            myDoc.SetParameterValue("@PrescriptionUnitID", PrescriptionUnitID);

            myDoc.SetParameterValue("@Opd_Ipd_Id", Opd_Ipd_Id);
            myDoc.SetParameterValue("@Opd_Ipd_UnitId", Opd_Ipd_UnitID);
            myDoc.SetParameterValue("@Opd_Ipd", OPD_IPD);         

            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");

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