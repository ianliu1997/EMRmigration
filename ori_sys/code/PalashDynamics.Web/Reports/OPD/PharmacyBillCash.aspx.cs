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
    public partial class PharmacyBillCash : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long BillID = 0;
            long UnitID = 0;

            string LoginUserName = "";

            
            if (Request.QueryString["PrintFomatID"] == "1")
            {
                //myDoc.Load(Server.MapPath("CashPharmacyBillReport.rpt"));
                myDoc.Load(Server.MapPath("CounterSale_Rpt_New.rpt"));
               
            }
            else if (Request.QueryString["PrintFomatID"] == "2")
            {
               // myDoc.Load(Server.MapPath("CashPharmacyBillReportWithoutHeader.rpt"));
                myDoc.Load(Server.MapPath("CounterSale_Rpt_New.rpt"));
                
            }
            else
            {
                //myDoc.Load(Server.MapPath("CashPharmacyBillReport.rpt"));
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

            //if (Request.QueryString["LoginUserName"] != null)
            //{
            //    LoginUserName = Convert.ToString(Request.QueryString["LoginUserName"]);
            //}

         //   myDoc.SetParameterValue("@LoginUserName", LoginUserName);
            myDoc.SetParameterValue("@BillID", BillID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@IsBilled", true);
            myDoc.SetParameterValue("@isIPDBill", false);

            //myDoc.SetParameterValue("@BillID", BillID, "rptPharmacybillCash");
            //myDoc.SetParameterValue("@UnitID", UnitID, "rptPharmacybillCash");
            //myDoc.SetParameterValue("@IsBilled", true, "rptPharmacybillCash");
            myDoc.SetParameterValue("@BillID", BillID, "rptPayMode");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptPayMode");
            myDoc.SetParameterValue("@IsBilled", true, "rptPayMode");

            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            

            //myDoc.SetParameterValue("@LoginUserName", LoginUserName);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO 
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();

            //MemoryStream oStream = new MemoryStream();

            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";

            //Response.BinaryWrite(oStream.ToArray());
            //Response.End();

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