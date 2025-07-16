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

namespace PalashDynamics.Web.Reports.Sales.IPD
{
    public partial class PatientAdvanceReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            long ClinicID = 0;
            bool Excel = false;
            long lUnitId = 0;
            string SendClinicID = string.Empty;
            bool Detailed = false; //Added by AniketK on 15Feb2019
            bool Consolidated = false; //Added by AniketK on 15Feb2019

            myDoc = new ReportDocument();

            long BillTypeId = 0;
            long AdvanceType=0;
            if (Request.QueryString["AdvanceType"] != null)
            {
                AdvanceType = Convert.ToInt64(Request.QueryString["AdvanceType"]);
            }

            //if (BillTypeId == 0)
            //{
                myDoc.Load(Server.MapPath("rptPatientAdvance.rpt"));
            //}
            //else
            //{
            //    myDoc.Load(Server.MapPath("rptIPDRefundReportNew.rpt"));

            //}

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);

            }
            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            if (Request.QueryString["LoginUnitID"] != null)
            {
                lUnitId = Convert.ToInt64(Request.QueryString["LoginUnitID"]);
            }

            if (Request.QueryString["SendClinicID"] != null)
            {
                SendClinicID = Convert.ToString(Request.QueryString["SendClinicID"]);
            }

            //Begin::Added by AniketK on 15Feb2019
            if (Request.QueryString["Detailed"] != null)
            {
                Detailed = Convert.ToBoolean(Request.QueryString["Detailed"]);
            }

            if (Request.QueryString["Consolidated"] != null)
            {
                Consolidated = Convert.ToBoolean(Request.QueryString["Consolidated"]);
            }
            //End::Added by AniketK on 15Feb2019

            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);

            myDoc.SetParameterValue("@UnitID", ClinicID);
            myDoc.SetParameterValue("@AdvanceType", AdvanceType);
            myDoc.SetParameterValue("@SendClinicID", SendClinicID);
            myDoc.SetParameterValue("@IsConsolidated", Consolidated); //Added by AniketK on 15Feb2019

            myDoc.SetParameterValue("@UnitID", lUnitId, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", lUnitId, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //if (Excel == true)
            //{
            //    MemoryStream oStream = new MemoryStream();
            //    oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.BinaryWrite(oStream.ToArray());
            //    Response.End();

            //}

            if (Excel == true)
            {
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);

                System.IO.Stream oStream = null;
                byte[] byteArray = null;
                oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";

                //Response.BinaryWrite(oStream.ToArray());
                Response.BinaryWrite(byteArray);

                Response.End();

            }
        }

        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            myDoc.Dispose();
            CrystalReportViewer1.Dispose();
            GC.Collect();
        }
    }
}