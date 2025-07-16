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

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class SettleClinicalBill : System.Web.UI.Page
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
                long BillID = 0;
                long UnitID = 0;
                long PaymentID = 0;
                ReportDocument myDoc = new ReportDocument();
                if (Request.QueryString["PrintFomatID"] == "1")
                {
                    myDoc.Load(Server.MapPath("rptSettleClinicalBill.rpt"));
                }
                else if (Request.QueryString["PrintFomatID"] == "2")
                {
                    myDoc.Load(Server.MapPath("rptSettleClinicalBillWithoutHeader.rpt"));
                }
                else
                {
                    myDoc.Load(Server.MapPath("rptSettleClinicalBill.rpt"));
                }
                if (Request.QueryString["BillID"] != null)
                {
                    BillID = Convert.ToInt64(Request.QueryString["BillID"]);
                }

                if (Request.QueryString["UnitID"] != null)
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }
                if (Request.QueryString["PaymentID"] != null)
                {
                    PaymentID = Convert.ToInt64(Request.QueryString["PaymentID"]);
                }
                myDoc.SetParameterValue("@BillID", BillID);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@IsBilled", true);
                myDoc.SetParameterValue("@PaymentID", PaymentID);
                myDoc.SetParameterValue("@PaymentID", PaymentID, "Settle bill summary");
                myDoc.SetParameterValue("@UnitID", UnitID, "Settle bill summary");
                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO

                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();

                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);

                // Added on 22Feb2019 for Package Flow in IPD
                Stream oStream = null;      
                byte[] byteArray = null;

                // Added on 22Feb2019 for Package Flow in IPD
                oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                //Response.BinaryWrite(oStream.ToArray());
                Response.BinaryWrite(byteArray);    // Added on 22Feb2019 for Package Flow in IPD
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