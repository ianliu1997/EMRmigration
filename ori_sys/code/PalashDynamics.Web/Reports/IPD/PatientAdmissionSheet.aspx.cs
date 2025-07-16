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

namespace PalashDynamics.Web.Reports.IPD
{
    public partial class PatientAdmissionSheet : System.Web.UI.Page
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
                string imgVirtualDirectory = "http://localhost/" + System.Configuration.ConfigurationManager.AppSettings["RegImgVirtualDir"] + "/";

                long AdmissionID = 0;
                long AdmissionUnitID = 0;

                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptPatientAdmissionSheet.rpt"));

                if (Request.QueryString["AdmissionID"] != null)
                {
                    AdmissionID = Convert.ToInt64(Request.QueryString["AdmissionID"]);
                }
                if (Request.QueryString["AdmissionUnitID"] != null)
                {
                    AdmissionUnitID = Convert.ToInt64(Request.QueryString["AdmissionUnitID"]);
                }
                myDoc.SetParameterValue("@AdmissionID", AdmissionID);
                myDoc.SetParameterValue("@AdmissionUnitID", AdmissionUnitID);
                myDoc.SetParameterValue("@ImgDirectory", imgVirtualDirectory);
                myDoc.SetParameterValue("@UnitID", AdmissionUnitID, "rptCommonReportHeader.rpt");

                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();

                // Added on 22Feb2019 for Package Flow in IPD
                Stream oStream = null;
                byte[] byteArray = null;

                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);

                // Added on 22Feb2019 for Package Flow in IPD
                oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                //Response.BinaryWrite(oStream.ToArray());
                Response.BinaryWrite(byteArray);        // Added on 22Feb2019 for Package Flow in IPD
                Response.End();

            }
            catch (Exception)
            {
                throw;
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