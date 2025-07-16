using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.Pathology
{
    public partial class TestReceiptReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            long PatientID = 0;
            long PatientUnitID = 0;
            string TestIDList = string.Empty;
            string SampleNoList = String.Empty;
            long ParameterCategoryID = 0;
            long AgeInDays = 0;
            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptTestReceiptReport.rpt"));
            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }
            if (Request.QueryString["PatientUnitID"] != null)
            {
                PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }
            if (Request.QueryString["SampleNoList"] != null)
            {
                SampleNoList = Convert.ToString(Request.QueryString["SampleNoList"]);
            }
            if (Request.QueryString["TestIDList"] != null)
            {
                TestIDList = Convert.ToString(Request.QueryString["TestIDList"]);
            }
            if (Request.QueryString["ParameterCategoryID"] != null)
            {
                ParameterCategoryID = Convert.ToInt64(Request.QueryString["ParameterCategoryID"]);
            }
            if (Request.QueryString["AgeInDays"] != null)
            {
                AgeInDays = Convert.ToInt64(Request.QueryString["AgeInDays"]);
            }
            myDoc.SetParameterValue("@SampleNO", SampleNoList);
            myDoc.SetParameterValue("@TestID", TestIDList);
            myDoc.SetParameterValue("@AgeInDays", AgeInDays);
            myDoc.SetParameterValue("@CategoryID", ParameterCategoryID);

            myDoc.SetParameterValue("@PatientID", PatientID, "rptPatientInfo");
            myDoc.SetParameterValue("@UnitID", PatientUnitID, "rptPatientInfo");
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