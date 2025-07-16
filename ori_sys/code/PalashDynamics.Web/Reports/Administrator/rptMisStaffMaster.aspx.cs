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
namespace PalashDynamics.Web.Reports.Administrator
{
    public partial class rptMisStaffMaster : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {            
            bool Excel = false;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rpt_UserMasterReport.rpt"));

            long a=0, b=0, c=0, Uid=1;
            string d;
            if (Request.QueryString["Check"] != null && Convert.ToInt64(Request.QueryString["Check"]) > 0)
            {
                a = Convert.ToInt64(Request.QueryString["Check"]);
            }
            if (Request.QueryString["Type"] != null && Convert.ToInt64(Request.QueryString["Type"]) > 0)
            {
                b = Convert.ToInt64(Request.QueryString["Type"]);
            }
            if (Request.QueryString["Role"] != null && Convert.ToInt64(Request.QueryString["Role"]) > 0)
            {
                c = Convert.ToInt64(Request.QueryString["Role"]);
            }
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            Uid = Convert.ToInt64(Request.QueryString["Uid"]);
            myDoc.SetParameterValue("@Locked", a);
            myDoc.SetParameterValue("@UserType", b);
            myDoc.SetParameterValue("@UserRole", c);
            myDoc.SetParameterValue("@UnitID", Uid, "RptReportHeader");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            if (Excel == true)
            {
                MemoryStream oStream = new MemoryStream();
                oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(oStream.ToArray());
                Response.End();

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