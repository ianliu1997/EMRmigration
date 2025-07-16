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
    public partial class Statistics_Report : System.Web.UI.Page
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

            String strReportName = " Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
            myDoc = new ReportDocument();
            string FromDate = null;
            string ToDate = null;
            long clinic = 0;            
            long   ReportID=0;
            FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            ToDate = Convert.ToString(Request.QueryString["ToDate"]);
            ReportID = Convert.ToInt32(Request.QueryString["ReportID"]);
            clinic = Convert.ToInt32(Request.QueryString["clinic"]);
            if (ReportID == 1)
            {
                myDoc.Load(Server.MapPath("Implantion_rate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);          
                myDoc.SetParameterValue("@clinic", clinic);
            }
            if (ReportID == 2)
            {
                myDoc.Load(Server.MapPath("Fertilization_rate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@clinic", clinic);                        
            }
            if (ReportID == 3)
            {
                myDoc.Load(Server.MapPath("On_Going_pregancy_rate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@clinic", clinic);
            }
            if (ReportID == 4)
            {
                myDoc.Load(Server.MapPath("Biochemical_pregancy_rate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@clinic", clinic);
            }
            if (ReportID == 5)
            {
                myDoc.Load(Server.MapPath("rpt_ChemicalpreganancyRate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@clinic", clinic);
            }
            if (ReportID == 6)
            {
                myDoc.Load(Server.MapPath("rpt_CumulativeRate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@clinic", clinic);
            }

            if (ReportID == 7)
            {
                myDoc.Load(Server.MapPath("rpt_LiveBirthRate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@clinic", clinic);
            }

            if (ReportID == 8)
            {
                myDoc.Load(Server.MapPath("rpt_CleavageRate.rpt"));
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@clinic", clinic);
            }
            myDoc.SetParameterValue("@UnitID", clinic, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", clinic, "rptUnitLogo.rpt");
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