using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.Templates
{
    public partial class Hysteroscopy : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath, strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            String strReportName = "Patient Hysteroscopy Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
            long UnitId;
            long TemplateId;
            long PatientId;
            long PatientUnitId;
            long EMRId;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptHysteroscopy.rpt"));
            UnitId = Convert.ToInt64(Request.QueryString["UnitId"]);
            TemplateId = Convert.ToInt64(Request.QueryString["TemplateId"]);
            PatientId = Convert.ToInt64(Request.QueryString["PatientId"]);
            PatientUnitId = Convert.ToInt64(Request.QueryString["PatientUnitId"]);
            EMRId = Convert.ToInt64(Request.QueryString["EMRID"]);

            #region Commented
            //if (!string.IsNullOrEmpty(FromDate))
            //{
            //    FrDT = Convert.ToDateTime(FromDate);
            //}

            //if (!string.IsNullOrEmpty(ToDate))
            //{
            //    ToDT = Convert.ToDateTime(ToDate);
            //    // ToDP = ToDT;
            //}
            //if (!string.IsNullOrEmpty(ToDatePrint))
            //{
            //    ToDP = Convert.ToDateTime(ToDatePrint);
            //}
            //if (Request.QueryString["ClinicID"] != null)
            //{
            //    clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            //}
            #endregion
            myDoc.SetParameterValue("@UnitId", UnitId);
            myDoc.SetParameterValue("@TemplateId", TemplateId);
            myDoc.SetParameterValue("@PatientID", PatientId);
            myDoc.SetParameterValue("@PatientUnitID", PatientUnitId);
            myDoc.SetParameterValue("@EMRId", EMRId);
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