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
    public partial class DPROperationalMetrics : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool Excel = false;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            long uid = 0;
            int type = 0;
            DateTime month;

            if (Request.QueryString["IsExporttoExcel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
            }

            uid = Convert.ToInt64(Request.QueryString["Uid"]);
            type = Convert.ToInt32(Request.QueryString["Type"]);
            month = Convert.ToDateTime(Request.QueryString["Month"]);

            myDoc = new ReportDocument();

            if (type == 1)            
                myDoc.Load(Server.MapPath("rptDPROperationalMetrics.rpt"));               
            else if (type == 2)
                myDoc.Load(Server.MapPath("rptDPRCash.rpt"));

            myDoc.SetParameterValue("@UnitID", uid, "rptUnitLogo.rpt"); 

            myDoc.SetParameterValue("@ClinicID", uid);
            myDoc.SetParameterValue("@Month", month);

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();            

            if (Excel)
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