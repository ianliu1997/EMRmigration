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

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class PendingPOForDashBoard : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            ReportDocument myDoc = new ReportDocument();

            long Store = Convert.ToInt64(Request.QueryString["Store"]);
            long ToSupplier = Convert.ToInt64(Request.QueryString["Supplier"]);
            string PONo = Convert.ToString(Request.QueryString["PONo"]);
            long UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            long UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            //  bool IsIndent = true;
            myDoc.Load(Server.MapPath("rptPendingPOForDashBoard.rpt"));
            myDoc.SetParameterValue("@Freezed", 1);
            myDoc.SetParameterValue("@StoreID", Store);
            myDoc.SetParameterValue("@SupplierID", ToSupplier);
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@UnitID", UnitID,"subClinicName");
         //   myDoc.SetParameterValue("@Isindent", true);
            myDoc.SetParameterValue("@PONO", PONo);

            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //MemoryStream oStream = new MemoryStream();
            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);

            Stream oStream = null;
            byte[] byteArray = null;
            oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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