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

namespace PalashDynamics.Web.ReportsBI.OPD
{
    public partial class PharmacyBill : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            ReportDocument myDoc = new ReportDocument();
            long BillID = 0;
            long UnitID = 0;
            long RID = 0;
            long IsIPDBill = 0;
            long PackageID = 0;

            if (Request.QueryString["ReportID"] != null)
            {
                RID = Convert.ToInt64(Request.QueryString["ReportID"]);
            }

            if (RID == Convert.ToInt64(1))
            {

                if (Request.QueryString["PrintFomatID"] == "1")
                {
                    myDoc.Load(Server.MapPath("ItemSalesReturnReport.rpt"));
                }
                else if (Request.QueryString["PrintFomatID"] == "2")
                {
                    myDoc.Load(Server.MapPath("ItemSalesReturnReportWithoutHeader.rpt"));
                }
                else
                {
                    myDoc.Load(Server.MapPath("ItemSalesReturnReport.rpt"));
                }

                if (Request.QueryString["BillID"] != null)
                {
                    BillID = Convert.ToInt64(Request.QueryString["BillID"]);
                }

                if (Request.QueryString["UnitID"] != null)
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }

                if (Request.QueryString["IsIPDBill"] != null)
                {
                    IsIPDBill = Convert.ToInt64(Request.QueryString["IsIPDBill"]);
                }



                myDoc.SetParameterValue("@BillID", BillID);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@IsBilled", true);
                myDoc.SetParameterValue("@IsIPDBill", IsIPDBill);

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
            else if (RID == Convert.ToInt64(2))
            {
                if (Request.QueryString["PackageID"] != null)
                {
                    PackageID = Convert.ToInt64(Request.QueryString["PackageID"]);
                }

                if (Request.QueryString["PrintFomatID"] == "1")
                {
                    myDoc.Load(Server.MapPath("CounterSale_Rpt_New.rpt"));
                }
                else if (Request.QueryString["PrintFomatID"] == "2")
                {
                    myDoc.Load(Server.MapPath("CounterSale_Rpt_New.rpt"));
                }
                else if (Request.QueryString["IsIPDBill"] != null) //Added by AJ Date 16/2/2017
                {
                    myDoc.Load(Server.MapPath("IPDPharmacySale_Rpt_New.rpt"));
                }
                else if (PackageID > 0) //***//
                {
                    myDoc.Load(Server.MapPath("CounterSalePackage_Rpt_New.rpt"));
                }
                else
                {
                    myDoc.Load(Server.MapPath("CounterSale_Rpt_New.rpt"));
                }


                if (Request.QueryString["BillID"] != null)
                {
                    BillID = Convert.ToInt64(Request.QueryString["BillID"]);
                }

                if (Request.QueryString["UnitID"] != null)
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }


                if (Request.QueryString["IsIPDBill"] != null)
                {
                    IsIPDBill = Convert.ToInt64(Request.QueryString["IsIPDBill"]);
                }

                myDoc.SetParameterValue("@BillID", BillID);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@IsBilled", true);
                myDoc.SetParameterValue("@IsIPDBill", IsIPDBill);

                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

                if (PackageID == 0)
                {
                    myDoc.SetParameterValue("@BillID", BillID, "rptPayMode");
                    myDoc.SetParameterValue("@UnitID", UnitID, "rptPayMode");
                    myDoc.SetParameterValue("@IsBilled", true, "rptPayMode");
                }

                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO
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


        }
    }
}