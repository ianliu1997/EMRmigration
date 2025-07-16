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

namespace PalashDynamics.Web.Reports.Pathology
{
    public partial class ResultEntry : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
                String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
                String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
                String dbName = ConfigurationManager.AppSettings["DBName"].ToString();                
                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("ResultEntryTemplate1.rpt"));
                long ID=0;
                long UnitID = 0;
                bool IsFinalized = false;
                string ResultId = null;
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "0")
                {
                    ID = Convert.ToInt64(Request.QueryString["ID"]);
                }
                if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }
                if (Request.QueryString["IsFinalized"] != null && Request.QueryString["IsFinalized"] != "0")
                {
                    IsFinalized = Convert.ToBoolean(Request.QueryString["IsFinalized"]);
                }
                
                myDoc.SetParameterValue("@Id", ID);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@IsDelivered", IsFinalized);
                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;   
                CrystalReportViewer1.DataBind();               
            }
            catch (Exception ex)
            {
                throw ex;
            }            
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