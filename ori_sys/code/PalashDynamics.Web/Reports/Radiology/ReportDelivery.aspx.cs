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

namespace PalashDynamics.Web.Reports.Radiology
{
    public partial class ReportDelivery : System.Web.UI.Page
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
                ReportDocument myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("ReportDelivery1.rpt"));
                long OrderID = 0;
                long TestID = 0;
                long UnitID = 0;                
                if (Request.QueryString["OrderID"] != null && Request.QueryString["OrderID"] != "0")
                {
                    OrderID = Convert.ToInt64(Request.QueryString["OrderID"]);
                }
                if (Request.QueryString["TestID"] != null && Request.QueryString["TestID"] != "0")
                {
                    TestID = Convert.ToInt64(Request.QueryString["TestID"]);
                }
                if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }

                myDoc.SetParameterValue("@ID", OrderID);
                myDoc.SetParameterValue("@TestID", TestID);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
                MemoryStream oStream = new MemoryStream();
                oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(oStream.ToArray());
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