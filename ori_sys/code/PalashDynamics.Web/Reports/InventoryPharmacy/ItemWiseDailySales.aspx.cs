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
    public partial class ItemWiseDailySales : System.Web.UI.Page
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
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;
            long ItemGroup = 0;
            long ItemCategory = 0;
            long DispencingType = 0;
            long TherClass = 0;
            long MoleculeName = 0;
            string ItemIds =null;
            bool Excel = false;
            long UserID = 0;
            long DoctorId = 0;
            bool IsPackage = false;
            string ItemName = string.Empty;
            //Added by AniketK on 12-Dec-2018
            bool IsPackageSalesShow = false;

            myDoc = new ReportDocument();

            IsPackage = Convert.ToBoolean(Request.QueryString["IsPackage"]);

            if (IsPackage == true)
            {
                myDoc.Load(Server.MapPath("crItemWisePackageIssueReport.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("crItemWiseDailySalesReport.rpt"));
            }

            if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            if (Request.QueryString["UserID"] != null && Request.QueryString["UserID"] != "0")
            {
                UserID = Convert.ToInt64(Request.QueryString["UserID"]);
            }

            if (Request.QueryString["StoreID"] != null && Request.QueryString["StoreID"] != "0")
            {
                StoreID = Convert.ToInt64(Request.QueryString["StoreID"]);
            }

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ItemIDs"] != null && Request.QueryString["ItemIDs"] != "")
            {
                ItemIds = Request.QueryString["ItemIDs"].ToString();
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }
            if (Request.QueryString["ItemGroup"] != null && Request.QueryString["ItemGroup"] != "0")
            {
                ItemGroup = Convert.ToInt64(Request.QueryString["ItemGroup"]);
            }
            if (Request.QueryString["ItemCategory"] != null && Request.QueryString["ItemCategory"] != "0")
            {
                ItemCategory = Convert.ToInt64(Request.QueryString["ItemCategory"]);
            }
            if (Request.QueryString["DispencingType"] != null && Request.QueryString["DispencingType"] != "0")
            {
                DispencingType = Convert.ToInt64(Request.QueryString["DispencingType"]);
            }
            if (Request.QueryString["TherClass"] != null && Request.QueryString["TherClass"] != "0")
            {
                TherClass = Convert.ToInt64(Request.QueryString["TherClass"]);
            }
            if (Request.QueryString["MoleculeName"] != null && Request.QueryString["MoleculeName"] != "0")
            {
                MoleculeName = Convert.ToInt64(Request.QueryString["MoleculeName"]);
            }

            if (Request.QueryString["DoctorId"] != null && Request.QueryString["DoctorId"] != "0") //***//
            {
                DoctorId = Convert.ToInt64(Request.QueryString["DoctorId"]);
            }

            if (Request.QueryString["IsPackage"] != null && Request.QueryString["IsPackage"] != "") //***//
            {
                IsPackage = Convert.ToBoolean(Request.QueryString["IsPackage"]);
            }

            if (Request.QueryString["Excel"] != null && Request.QueryString["Excel"] != "")
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }

            if (Request.QueryString["ItemName"] != null)
            {
                ItemName = Request.QueryString["ItemName"];
            }

            //Begin::Added by AniketK on 12-Dec-2018
            if (Request.QueryString["IsPackageSalesShow"] != null && Request.QueryString["IsPackageSalesShow"] != "")
            {
                IsPackageSalesShow = Convert.ToBoolean(Request.QueryString["IsPackageSalesShow"]);
            }
            //End::Added by AniketK on 12-Dec-2018

            myDoc.SetParameterValue("@ClinicID", UnitID);
            myDoc.SetParameterValue("@UserID", UserID);
            myDoc.SetParameterValue("@StoreID", StoreID);
            myDoc.SetParameterValue("@FromDate", FromDate);
            myDoc.SetParameterValue("@ToDate", ToDate);
            myDoc.SetParameterValue("@ItemID", ItemIds);
            myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
            myDoc.SetParameterValue("@FilterGroupId", ItemGroup);
            myDoc.SetParameterValue("@FilterCategoryId", ItemCategory);
            myDoc.SetParameterValue("@FilterDispensingId", DispencingType);
            myDoc.SetParameterValue("@FilterTherClassId", TherClass);
            myDoc.SetParameterValue("@FilterMolecule", MoleculeName);
            myDoc.SetParameterValue("@DoctorId", DoctorId);
            myDoc.SetParameterValue("@IsPackage", IsPackage);
            myDoc.SetParameterValue("@ItemName", ItemName);
            myDoc.SetParameterValue("@IsPackageSalesShow", IsPackageSalesShow);//Added by AniketK on 12-Dec-2018
            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            if (Excel==true)
            {
                System.IO.Stream oStream = null;
                byte[] byteArray = null;
                oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(byteArray);
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