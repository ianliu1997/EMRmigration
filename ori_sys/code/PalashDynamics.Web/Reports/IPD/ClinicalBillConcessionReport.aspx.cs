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


namespace PalashDynamics.Web.Reports.IPD
{
    public partial class ClinicalBillConcessionReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            long BillID = 0;
            long UnitID = 0;
            myDoc = new ReportDocument();
        //    myDoc.Load(Server.MapPath("rptIPDClinicalConcessionBillReport.rpt"));
             myDoc.Load(Server.MapPath("Rpt_IPDBillReport.rpt"));
           
            if (Request.QueryString["BillID"] != null)
            {
                BillID = Convert.ToInt64(Request.QueryString["BillID"]);
            }

            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            myDoc.SetParameterValue("@BillID", BillID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@IsBilled", true);
            //myDoc.SetParameterValue("@BillID", BillID, "Clinical Summary");
            //myDoc.SetParameterValue("@UnitID", UnitID,"Clinical Summary");
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