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

namespace PalashDynamics.Web.Reports.Patient
{
    public partial class rptPatientCaseReportNew : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {            
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string imgVirtualDirectory = "http://localhost/" + System.Configuration.ConfigurationManager.AppSettings["RegImgVirtualDir"] + "/";

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptPatientCaseRecordNewManipal.rpt"));
            long UnitId = 0;
            long ID = 0;
            long IsDoctorID =0;
            long IsEmployee= 0;
            long DoctorID = 0;
            long EmployeeID = 0;
            if (Request.QueryString["UnitID"] != null)
            {
                UnitId = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["ID"] != null)
            {
                ID = Convert.ToInt64(Request.QueryString["ID"]);
            }
            myDoc.SetParameterValue("@ID", ID);
            myDoc.SetParameterValue("@UnitId", UnitId);
            myDoc.SetParameterValue("@IsDoctorID", IsDoctorID);
            myDoc.SetParameterValue("@IsEmployee", IsEmployee);
            myDoc.SetParameterValue("@DoctorID", DoctorID);
            myDoc.SetParameterValue("@EmployeeID", EmployeeID);
            myDoc.SetParameterValue("@ImgDirectory",imgVirtualDirectory);


            myDoc.SetParameterValue("@UnitId", UnitId, "rptCommonReportHeader.rpt");

            myDoc.SetParameterValue("@UnitID", UnitId, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            Stream oStream = null;
            byte[] byteArray = null;

            oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
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