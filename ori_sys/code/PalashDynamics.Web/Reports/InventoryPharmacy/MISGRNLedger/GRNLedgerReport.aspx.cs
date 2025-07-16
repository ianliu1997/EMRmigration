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

namespace PalashDynamics.Web.Reports.InventoryPharmacy.MISGRNLedger
{
    public partial class GRNLedgerReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;
            long clinic = 0;
            bool IsExcel = false;

            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("GRNLedgerReportFile.rpt"));

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                DTT = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                DTP = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }

            if (Request.QueryString["clinic"] != null)
            {
                clinic = Convert.ToInt64(Request.QueryString["clinic"]);
                if (clinic == 0)
                {
                    clinic = 1;
                }
            }
            if (Request.QueryString["IsExcel"] != null)
            {
                IsExcel = Convert.ToBoolean(Request.QueryString["IsExcel"]);
            }

            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@UnitID", clinic);
            myDoc.SetParameterValue("@ToDatePrint", DTP);

            myDoc.SetParameterValue("@UnitID", clinic, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
            myDoc.SetParameterValue("@UnitID", clinic, "rptCommonReportHeader.rpt");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (IsExcel)
            {
                Stream stream = null;
                MemoryStream oStream = new MemoryStream();
                stream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);  //ExportFormatType.CharacterSeparatedValues
                stream.CopyTo(oStream);
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