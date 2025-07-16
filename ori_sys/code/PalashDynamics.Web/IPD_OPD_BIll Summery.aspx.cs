using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web
{
    public partial class IPD_OPD_BIll_Summery : System.Web.UI.Page
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
            bool IsBilled = true;
            long isInline = 0;
            myDoc = new ReportDocument();

            isInline = Convert.ToInt64(Request.QueryString["isInline"]);

            if (Request.QueryString["IsBilled"] != null)
            {
                IsBilled = Convert.ToBoolean(Request.QueryString["IsBilled"]);
            }
            //if (Request.QueryString["PrintFomatID"] == "1")
            //{
            //    myDoc.Load(Server.MapPath("rptClinicalBillReport.rpt"));
            //}
            //else if (Request.QueryString["PrintFomatID"] == "2")
            //{
            //    myDoc.Load(Server.MapPath("rptClinicalBillReportWithoutHeader.rpt"));
            //}
            //else if (Request.QueryString["PrintFomatID"] == "3")
            //{
            //    myDoc.Load(Server.MapPath("rptClinicalDraftBill.rpt"));
            //    IsBilled = false;
            //}
            //else if (Request.QueryString["PrintFomatID"] == "4") // added by Bhushanp its for IsBilled = true OR Final Bill
            //{
            //    if (isInline == 1)
            //    {
            //        myDoc.Load(Server.MapPath("rptClinicalInlineBill.rpt"));   //rptClinicalInlineBill.rpt
            //    }
            //    else
            //    {
            //        myDoc.Load(Server.MapPath("rptClinicalDraftBill.rpt"));
            //    }


            //}
            //else
            //{
            //    myDoc.Load(Server.MapPath("rptClinicalBillReport.rpt"));
            //}
            myDoc.Load(Server.MapPath("rptClinicalBillReport_New.rpt"));
            if (Request.QueryString["BillID"] != null)
            {
                BillID = Convert.ToInt64(Request.QueryString["BillID"]);
            }

            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            //if (Request.QueryString["PrintFomatID"] == "4") // added by yk to show package details if bill is package for IPD Bill
            //{
            //    if (isInline == 0)
            //    {
            //        myDoc.SetParameterValue("@BillID", BillID, "ServiceDetailsPackage.rpt");//Added by yk
            //        myDoc.SetParameterValue("@UnitID", UnitID, "ServiceDetailsPackage.rpt");//Added by yk
            //    }
            //}

            myDoc.SetParameterValue("@BillID", BillID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@IsBilled", IsBilled);
          //  myDoc.SetParameterValue("@BillID", BillID, "rptClinicalIPDFinalBill.rpt");
            //myDoc.SetParameterValue("@UnitID", UnitID, "rptClinicalIPDFinalBill.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");

            myDoc.SetParameterValue("@BillID", BillID,"rptClinicalDraftBill_New.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID,"rptClinicalDraftBill_New.rpt");
            myDoc.SetParameterValue("@IsBilled", IsBilled,"rptClinicalDraftBill_New.rpt");
            //if (isInline == 1)
            //{
               
            //}




            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");


            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            Stream oStream = null;
            byte[] byteArray = null;
            oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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