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
    public partial class ServiceMasterReportASPX : System.Web.UI.Page
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
            myDoc.Load(Server.MapPath("Rpt_Service_MasterReport.rpt"));

            long a=0, b=0, c=0,uid = 0;
            string SC = "", CD = "", SD = "", BSR = "", SN = "";
           
            if (Request.QueryString["a"] != null)
            {
                a = Convert.ToInt64(Request.QueryString["a"]);
            }
            if (Request.QueryString["b"] != null)
            {
                b = Convert.ToInt64(Request.QueryString["b"]);
            }
            if (Request.QueryString["c"] != null)
            {
                c = Convert.ToInt64(Request.QueryString["c"]);
            }

            if (Request.QueryString["SC"] != null)
            {
                SC = Convert.ToString(Request.QueryString["SC"]);
            }
            if (Request.QueryString["CD"] != null)
            {
                CD = Convert.ToString(Request.QueryString["CD"]);
            }
            if (Request.QueryString["SD"] != null)
            {
                SD = Convert.ToString(Request.QueryString["SD"]);
            }
            if (Request.QueryString["BSR"] != null)
            {
                BSR = Convert.ToString(Request.QueryString["BSR"]);
            }
            if (Request.QueryString["SN"] != null && Request.QueryString["SN"] != "")
            {
                SN = Convert.ToString(Request.QueryString["SN"]);
            }
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            
            uid = Convert.ToInt64(Request.QueryString["Uid"]);

            myDoc.SetParameterValue("@UnitID", uid, "RepReportHeader");
            myDoc.SetParameterValue("@CT", a);
            myDoc.SetParameterValue("@S", b);
            myDoc.SetParameterValue("@SS", c);
            myDoc.SetParameterValue("@SC", SC);
            myDoc.SetParameterValue("@CD", CD);
            myDoc.SetParameterValue("@SD", SD);
            myDoc.SetParameterValue("@BSR", BSR);
            myDoc.SetParameterValue("@SN", SN);
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