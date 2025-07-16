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
    public partial class ItemMasterReport : System.Web.UI.Page
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
            myDoc.Load(Server.MapPath("ItemMasterReports.rpt"));

            long a = 0, b = 0, c = 0, d = 0, f = 0, g = 0, h = 0, i = 0, uid = 0;
            string ItemName = "";

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
            if (Request.QueryString["d"] != null)
            {
                d = Convert.ToInt64(Request.QueryString["d"]);
            }
            if (Request.QueryString["f"] != null)
            {
                f = Convert.ToInt64(Request.QueryString["f"]);
            }
            if (Request.QueryString["g"] != null)
            {
                g = Convert.ToInt64(Request.QueryString["g"]);
            }
            if (Request.QueryString["h"] != null)
            {
                h = Convert.ToInt64(Request.QueryString["h"]);
            }
            if (Request.QueryString["i"] != null)
            {
                i = Convert.ToInt64(Request.QueryString["i"]);
            }

            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            ItemName = Convert.ToString(Request.QueryString["ItemName"]);
            uid = Convert.ToInt64(Request.QueryString["Uid"]);

            myDoc.SetParameterValue("@UnitID", uid, "RptReportHeader");
            myDoc.SetParameterValue("@ItemName", ItemName);
            myDoc.SetParameterValue("@MoleculeName", a);
            myDoc.SetParameterValue("@ItemGroup", b);
            myDoc.SetParameterValue("@ItemCategory", c);
            myDoc.SetParameterValue("@DispensingType", d);
            myDoc.SetParameterValue("@StorageType", f);
            myDoc.SetParameterValue("@PregnancyClass", g);
            myDoc.SetParameterValue("@TherapeuticClass", h);
            myDoc.SetParameterValue("@ManufacturedBy", i);
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