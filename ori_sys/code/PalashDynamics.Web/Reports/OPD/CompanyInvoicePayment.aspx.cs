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
    public partial class CompanyInvoicePayment : System.Web.UI.Page
    {
        ReportDocument myDoc = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long CompanyID = 0;
            long UnitID = 0;
            string InvoiceNumber = null;
            long ID = 0;

            myDoc.Load(Server.MapPath("rptBillInvoicePaymentReport1.rpt"));

            if (Request.QueryString["ID"] != null)
            {
                ID = Convert.ToInt64(Request.QueryString["ID"]);
            }
            if (Request.QueryString["CompanyID"] != null)
            {
                CompanyID = Convert.ToInt64(Request.QueryString["CompanyID"]);
            }

            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["InvoiceNumber"] != null)
            {
                InvoiceNumber = Convert.ToString(Request.QueryString["InvoiceNumber"]);
            }


            myDoc.SetParameterValue("@CompanyID", CompanyID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@InvoiceNumber", InvoiceNumber);
            myDoc.SetParameterValue("@ID", ID);
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "InvoicePaymentSummary");
            myDoc.SetParameterValue("@InvoiceID", ID, "InvoicePaymentSummary");


            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 30/9/2016 Get Unit LOGO 
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;


            CrystalReportViewer1.DataBind();

            //MemoryStream oStream = new MemoryStream();
            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);

            System.IO.Stream oStream = null;
            byte[] byteArray = null;
            oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";

            //Response.BinaryWrite(oStream.ToArray());
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