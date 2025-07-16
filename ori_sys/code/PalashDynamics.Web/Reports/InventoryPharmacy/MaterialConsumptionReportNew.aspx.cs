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

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class MaterialConsumptionReportNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;
            long UnitID = 0;
            long StoreID = 0;     
            string MRN = "";
            string PName = "";
            bool IsExcel = false;      //Added by Sayali 17th Aug 2018

            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            ReportDocument myDoc = new ReportDocument();
                    
          

            if (Request.QueryString["MRN"] != null)
            {
                MRN = Convert.ToString(Request.QueryString["MRN"]);
            }

            if (Request.QueryString["PName"] != null)
            {
                PName = Convert.ToString(Request.QueryString["PName"]);
            }

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

            //Added By Pallavi
            if (Request.QueryString["clinic"] != null && Request.QueryString["clinic"] != "")
            {
                UnitID = Convert.ToInt64(Request.QueryString["clinic"]);
            }
            if (Request.QueryString["store"] != null && Request.QueryString["store"] != "")
            {
                StoreID = Convert.ToInt64(Request.QueryString["store"]);
            }

            //Added by Sayali 17th Aug 2018
            if (Request.QueryString["IsExcel"] != null)
            {
                IsExcel = Convert.ToBoolean(Request.QueryString["IsExcel"]);
            }
            //

            myDoc.Load(Server.MapPath("MaterialConsumptionNew.rpt"));
                         
            myDoc.SetParameterValue("@UnitId", UnitID);
            myDoc.SetParameterValue("@Fromdate", FromDate);
            myDoc.SetParameterValue("@Todate", ToDate);
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@MRN", MRN);
            myDoc.SetParameterValue("@PName", PName);
            //myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
            //Addded By Bhushanp 23052017
            long lUnitID = UnitID;
            if (UnitID == 0)
            {
                lUnitID = 1;
            }
            myDoc.SetParameterValue("@UnitID", lUnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
                               

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //Added by Sayali 17th Aug 2018
            if (IsExcel)
            {
                Stream stream = null;
                MemoryStream o_Stream = new MemoryStream();
                stream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);  //ExportFormatType.CharacterSeparatedValues
                stream.CopyTo(o_Stream);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(o_Stream.ToArray());
                Response.End();
            }
            //

            //Stream oStream = null;
            //byte[] byteArray = null;
            ////   oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            //oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //byteArray = new byte[oStream.Length];
            //oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(byteArray);
            //Response.End();
        }
    }
}