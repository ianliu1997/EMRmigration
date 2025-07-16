using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.IVFDashboard
{
    public partial class SemenExamination : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            myDoc = new ReportDocument();
            if (Request.QueryString["PrintFomatID"] == "1")
            {
                myDoc.Load(Server.MapPath("Rpt_IVFDashboard_SemenExamination.rpt"));
            }
            else if (Request.QueryString["PrintFomatID"] == "2")
            {
                myDoc.Load(Server.MapPath("Rpt_IVFDashboard_SemenExamination_WithoutHeader.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("Rpt_IVFDashboard_SemenExamination.rpt"));
            }
            long UnitID = 0;
            long ID = 0;         
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["ID"] != null)
            {
                ID = Convert.ToInt64(Request.QueryString["ID"]);
            }
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@ID", ID);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");            
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); 
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            Stream oStream = null;
            byte[] byteArray = null;
            //   oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteArray);
            Response.End();

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