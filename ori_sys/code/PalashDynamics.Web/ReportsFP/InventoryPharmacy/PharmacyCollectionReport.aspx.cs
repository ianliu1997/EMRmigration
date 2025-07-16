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

namespace PalashDynamics.Web.ReportsFP.InventoryPharmacy
{
    public partial class PharmacyCollectionReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long UnitID = 0;
            long StoreID = 0;

            //Begin::Added by AniketK on 25-Oct-2018
            long RegistrationTypeID = 0;
            //End::Added by AniketK on 25-Oct-2018

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;
            bool Excel = false;
            long UserID = 0;

            string SendClinicID = string.Empty;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPharmacyCollectionReport.rpt"));


            if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "0")
            {
                UnitID = Convert.ToInt64(Request.QueryString["clinic"]);
            }

            if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "0")
            {
                UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            }

            if (Request.QueryString["store"] != null && Request.QueryString["store"] != "0")
            {
                StoreID = Convert.ToInt64(Request.QueryString["store"]);
            }

            //Begin::Added by AniketK on 25-Oct-2018
            if (Request.QueryString["RegistrationTypeID"] != null && Request.QueryString["RegistrationTypeID"] != "0")
            {
                RegistrationTypeID = Convert.ToInt64(Request.QueryString["RegistrationTypeID"]);
            }
            //End::Added by AniketK on 25-Oct-2018

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }

            if (Request.QueryString["IsExcel"] != null && Request.QueryString["IsExcel"] != "")
            {
                Excel = Convert.ToBoolean(Request.QueryString["IsExcel"]);
            }

            #region Added by Prashant Channe on 08 March 2019

            if (Request.QueryString["SendClinicID"] != null && Request.QueryString["SendClinicID"] != "0")
            {
                SendClinicID = (Request.QueryString["SendClinicID"]).ToString();
            }

            #endregion

            myDoc.SetParameterValue("@ClinicID", UnitID);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@RegistrationTypeID", RegistrationTypeID); //Added by AniketK on 25-Oct-2018
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);

            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO

            myDoc.SetParameterValue("@SendClinicID", SendClinicID);  //Added by Prashant Channe on 08 March 2019
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (Excel)
            {
                Stream stream = null;
                MemoryStream oStream = new MemoryStream();
                stream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                stream.CopyTo(oStream);
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
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