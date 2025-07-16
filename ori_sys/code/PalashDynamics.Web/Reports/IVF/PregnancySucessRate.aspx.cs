using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace PalashDynamics.Web.Reports.IVF
{
    public partial class PregnancySucessRate : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath;
            string strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            String strReportName = "Search Patient Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
            myDoc = new ReportDocument();
            DateTime? Month = null;
            bool PieChart = false;
            // BY BHUSHAN . . . . . .
            string FromDate = null;
            string ToDate = null;
            long ReportID = 0;
            long UnitID = 0;
            FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            ToDate = Convert.ToString(Request.QueryString["ToDate"]);
            ReportID = Convert.ToInt32(Request.QueryString["ReportID"]);

            if (Request.QueryString["Month"] != null && Request.QueryString["Month"] != "")
            {
                Month = Convert.ToDateTime(Request.QueryString["Month"]);
            }
            if (Request.QueryString["PieChart"] != null && Request.QueryString["PieChart"] != "")
            {
                PieChart = Convert.ToBoolean(Request.QueryString["PieChart"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (PieChart == true)
            {
                myDoc.Load(Server.MapPath("PregnancySucessRate1.rpt"));
                myDoc.SetParameterValue("@Month", Month);
                myDoc.SetParameterValue("@UnitID", UnitID);
            }
            else
            {
                //  myDoc.Load(Server.MapPath("Piechart.rpt"));
                // myDoc.Load(Server.MapPath("rpt_PregnancySuccessRate_Self.rpt"));
                myDoc.Load(Server.MapPath("rpt_PregnancySuccessRateNew_Self.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@UnitID", UnitID);
            }
            
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //Stream oStream = null;
            //byte[] byteArray = null;
            //oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //byteArray = new byte[oStream.Length];
            //oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(byteArray);
            //Response.End();
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