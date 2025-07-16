using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class AvailedAndPendingPackageServices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long PATIENTID = 0;
            long PATIENTUNITID = 0;
            long TARIFFID = 0;
            long PACKAGEID = 0;
            string PatientName = "", PackageName = "";           
            ReportDocument myDoc = new ReportDocument();

            myDoc.Load(Server.MapPath("rptAvailedAndPendingPackageServices.rpt"));

            if (Request.QueryString["PatientID"] != null)
            {
                PATIENTID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }

            if (Request.QueryString["PatientUnitID"] != null)
            {
                PATIENTUNITID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }

            if (Request.QueryString["TariffID"] != null)
            {
                TARIFFID = Convert.ToInt64(Request.QueryString["TariffID"]);
            }

            if (Request.QueryString["PackageID"] != null)
            {
                PACKAGEID = Convert.ToInt64(Request.QueryString["PackageID"]);
            }

            if (Request.QueryString["PatientName"] != null)
            {
                PatientName = Convert.ToString(Request.QueryString["PatientName"]);
            }
            if (Request.QueryString["PackageName"] != null)
            {
                PackageName = Convert.ToString(Request.QueryString["PackageName"]);
            }

            myDoc.SetParameterValue("@PatientID", PATIENTID);
            myDoc.SetParameterValue("@PatientUnitID", PATIENTUNITID);
            myDoc.SetParameterValue("@TariffID", TARIFFID);
            myDoc.SetParameterValue("@PackageID", PACKAGEID);
            myDoc.SetParameterValue("@PatientName", PatientName);
            myDoc.SetParameterValue("@PackageName", PackageName);

            myDoc.SetParameterValue("@UnitID", PATIENTUNITID, "rptCommonReportHeader.rpt"); //Added by ajit jadhav date 6/12/2016 Get Unit LOGO
            myDoc.SetParameterValue("@UnitID", PATIENTUNITID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 6/12/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();
        
        }
    }
}