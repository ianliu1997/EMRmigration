using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.IPD
{
    public partial class BedTransfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            string appPath = HttpContext.Current.Request.ApplicationPath;
            string physicalPath = HttpContext.Current.Request.MapPath(appPath) + "\\Reports\\";

            long ReportID = 0;
            if (Request.QueryString["ReportID"] != null)
            {
                ReportID = Convert.ToInt64(Request.QueryString["ReportID"]);
            }
            if (ReportID.Equals(22))      //if (ReportID.Equals((int)PalashDynamics.ValueObjects.Reports.Admpatientstickrereport))      // Admpatientstickrereport = 22
            {
                long AdmId = 0;
                long AdmUnitId = 0;

                if (Request.QueryString["AdmID"] != null)
                {
                    AdmId = Convert.ToInt64(Request.QueryString["AdmID"]);
                }

                if (Request.QueryString["AdmUnitID"] != null)
                {
                    AdmUnitId = Convert.ToInt64(Request.QueryString["AdmUnitID"]);
                }

                ReportDocument myIPDBillDoc = new ReportDocument();
                myIPDBillDoc.Load(physicalPath + "\\IPD\\rptAdmissionStickerNormal.rpt");
                myIPDBillDoc.SetParameterValue("@AdmID", AdmId);
                myIPDBillDoc.SetParameterValue("@AdmUnitID", AdmUnitId);
                myIPDBillDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myIPDBillDoc);

                CrystalReportViewer1.ReportSource = myIPDBillDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();

                System.Web.HttpContext.Current.Session["ReportSession"] = null;
            }
            else if (ReportID.Equals(23))   //(ReportID.Equals((int)PalashDynamics.ValueObjects.Reports.IntakOutputReport))     // IntakOutputReport = 23
                {
                    long IntakeOutputID = 0;
                    long IntakeOutputIDUnitID = 0;

                    if (Request.QueryString["IntakeOutputID"] != null)
                    {
                        IntakeOutputID = Convert.ToInt64(Request.QueryString["IntakeOutputID"]);
                    }

                    if (Request.QueryString["IntakeOutputIDUnitID"] != null)
                    {
                        IntakeOutputIDUnitID = Convert.ToInt64(Request.QueryString["IntakeOutputIDUnitID"]);
                    }

                    ReportDocument myIPDBillDoc = new ReportDocument();
                    myIPDBillDoc.Load(Server.MapPath("rptIntakeOutputChart.rpt"));
                    myIPDBillDoc.SetParameterValue("@ID", IntakeOutputID);
                    myIPDBillDoc.SetParameterValue("@UnitID", IntakeOutputIDUnitID);
                    myIPDBillDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                    CReportAuthentication.Impersonate(myIPDBillDoc);

                    CrystalReportViewer1.ReportSource = myIPDBillDoc;
                    CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                    CrystalReportViewer1.DataBind();

                    System.Web.HttpContext.Current.Session["ReportSession"] = null;
                }
            else 
            {
                long AdmID = 0;
                long AdmUnitID = 0;

                ReportDocument myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptIPDBedtransferList.rpt"));

                if (Request.QueryString["AdmID"] != null)
                {
                    AdmID = Convert.ToInt64(Request.QueryString["AdmID"]);
                }

                if (Request.QueryString["AdmUnitID"] != null)
                {
                    AdmUnitID = Convert.ToInt64(Request.QueryString["AdmUnitID"]);
                }

                myDoc.SetParameterValue("@LinkServer", null);
                myDoc.SetParameterValue("@LinkServerAlias", null);
                myDoc.SetParameterValue("@LinkServerDBName", null);
                myDoc.SetParameterValue("@AdmID", AdmID);
                myDoc.SetParameterValue("@AdmUnitID", AdmUnitID);

                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);

                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
            }


        }
    }
}