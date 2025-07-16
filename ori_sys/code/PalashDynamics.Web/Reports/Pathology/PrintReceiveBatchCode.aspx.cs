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
    public partial class PrintReceiveBatchCode : System.Web.UI.Page
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
                myDoc.Load(Server.MapPath("PrintReceiveBatchCode1.rpt"));
                string BatchNo = "";
                long UnitID = 0;
                if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }
                if (Request.QueryString["BatchNo"] != null && Request.QueryString["BatchNo"] != string.Empty)
                {
                    BatchNo = Convert.ToString(Request.QueryString["BatchNo"]);
                }
                myDoc.SetParameterValue("@BatchNo", BatchNo);
                myDoc.SetParameterValue("@UnitID", UnitID, "Header");

                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 23/11/2016 Get Unit LOGO
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
                MemoryStream oStream1 = new MemoryStream();
                oStream1 = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(oStream1.ToArray());
                Response.End();
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