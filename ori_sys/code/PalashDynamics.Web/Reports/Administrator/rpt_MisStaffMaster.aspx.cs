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
    public partial class rpt_MisStaffMaster : System.Web.UI.Page
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
            myDoc.Load(Server.MapPath("rpt_StaffMasterReport.rpt"));
            long a=0, b=0, c=0, uid=0;
            string d;
            if (Request.QueryString["ClinicName"] != null && Convert.ToInt64(Request.QueryString["ClinicName"]) > 0)
            {
                a = Convert.ToInt64(Request.QueryString["ClinicName"]);
            }
            if (Request.QueryString["Designation"] != null && Convert.ToInt64(Request.QueryString["Designation"]) > 0)
            {
                b = Convert.ToInt64(Request.QueryString["Designation"]);
            }
            if (Request.QueryString["Department"] != null && Convert.ToInt64(Request.QueryString["Department"]) > 0)
            {
                c = Convert.ToInt64(Request.QueryString["Department"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            uid = Convert.ToInt64(Request.QueryString["Uid"]);
            myDoc.SetParameterValue("@UnitID", uid, "RptReportHeader");
            myDoc.SetParameterValue("@ClinicName", a);
            myDoc.SetParameterValue("@Designation", b);
            myDoc.SetParameterValue("@Department", c);
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