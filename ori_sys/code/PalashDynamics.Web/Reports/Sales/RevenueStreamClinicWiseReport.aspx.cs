using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PalashDynamics.Web.Reports.Sales
{
    public partial class RevenueStreamClinicWiseReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string strPath;
            string strConnection;
            String strReportName = "Revenue Stream Clinic Wise Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + ""; 
            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;
            long  ClinicID = 0;
            bool IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);      

            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID =Convert.ToInt64(Request.QueryString["ClinicID"]);
            }

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }

            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] !="")
            {
                DTT = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                DTP = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }
            if (!IsExporttoExcel)
            {
            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptRevenueStreamClinicWiseReport.rpt"));
            myDoc.SetParameterValue("@ClinicID", ClinicID);
            myDoc.SetParameterValue("@FromDate", DTF);
            myDoc.SetParameterValue("@ToDate", DTT);
            myDoc.SetParameterValue("@ToDatePrint", DTP);
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
           // myDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "");
            }
             else
             {
                 SqlCommand cmd = new SqlCommand();
                 DataTable dt = new DataTable();
                 SqlConnection sqlConnection1 = new SqlConnection(strConnection);
                 sqlConnection1.Open();
                     cmd.Connection = sqlConnection1;
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.CommandText = "CIMS_rptRevenueStreamClinicwise";
                     SqlParameter param = new SqlParameter();

                     param.DbType = DbType.DateTime;
                     param.Value = DTF;
                     param.ParameterName = "FromDate";
                     cmd.Parameters.Add(param);

                     param = new SqlParameter();
                     param.DbType = DbType.DateTime;
                     param.Value = DTT;
                     param.ParameterName = "ToDate";
                     cmd.Parameters.Add(param);

                     param = new SqlParameter();
                     param.DbType = DbType.DateTime;
                     param.Value = DTP;
                     param.ParameterName = "ToDatePrint";
                     cmd.Parameters.Add(param);

                     param = new SqlParameter();
                     param.DbType = DbType.Int64;
                     param.Value = ClinicID;
                     param.ParameterName = "ClinicID";
                     cmd.Parameters.Add(param);

                     cmd.ExecuteNonQuery();

                     SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                     adapter.Fill(dt);

                     ExportToExcel.CExcelExport objExcel = new ExportToExcel.CExcelExport();
                     DataTable dtReturn = new DataTable();
                     objExcel.ConnectionString = strConnection;

                     if (dt.Rows.Count > 0)
                     {
                         //Change Order as define in Report
                         var varlist = from c in dt.AsEnumerable()

                                       select new
                                       {
                                           ClinicName = "",
                                           SrNo = 0,
                                           PatientCount = 0,
                                           TotalRevenue = 0.0,
                                       };
                         PropertyInfo[] oProps = null;
                         foreach (var rec in varlist)
                         {
                             if (oProps == null)
                             {
                                 oProps = ((Type)rec.GetType()).GetProperties();
                                 foreach (PropertyInfo pi in oProps)
                                 {
                                     Type colType = pi.PropertyType;
                                     if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                                     == typeof(Nullable<>)))
                                     {
                                         colType = colType.GetGenericArguments()[0];
                                     }
                                     dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                                 }
                             }
                             DataRow dr = dtReturn.NewRow();
                             foreach (PropertyInfo pi in oProps)
                             {
                                 dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                                 (rec, null);
                             }
                             dtReturn.Rows.Add(dr);
                         }
                     }

                     if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtReturn.Columns.Count; i++)
                            {
                                if (dtReturn.Columns[i].ColumnName.Equals("SrNo"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Sr. No";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("ClinicName"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Clinic Name";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("PatientCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Patient Count";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("TotalRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Total Revenue";
                                }
                            }

                            for (int i= 0; i < dtReturn.Rows.Count; i++)
                            {
                                 dtReturn.Rows[i]["Sr. No"]=i+1;
                                 if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["ClinicName"])))
                                 {
                                     dtReturn.Rows[i]["Clinic Name"] = dt.Rows[i]["ClinicName"];
                                 }
                                 if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PatientCount"])))
                                 {
                                     dtReturn.Rows[i]["Patient Count"] = dt.Rows[i]["PatientCount"];
                                 }
                                 if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalRevenue"])))
                                 {
                                     dtReturn.Rows[i]["Total Revenue"] = dt.Rows[i]["TotalRevenue"];
                                 }
                            }                            
                            objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                        }
                        else
                        {
                            dt = new DataTable();
                            dt.Columns.Add("Clinic Name");
                            dt.Columns.Add("Sr. No");
                            dt.Columns.Add("Patient Count");
                            dt.Columns.Add("Total Revenue");
                            objExcel.ExportToExcel("", strReportName, strPath, dt);
                        }
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Clinic Name");
                        dt.Columns.Add("Sr. No");
                        dt.Columns.Add("Patient Count");
                        dt.Columns.Add("Total Revenue");
                        objExcel.ExportToExcel("", strReportName, strPath, dt);
                    }
                
                //Response.Redirect(Request.ApplicationPath + "\\Reports\\Sales\\" + strReportName + ".xls");
                     string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/Sales/" + strReportName + ".xls";
                     Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");
                   
                sqlConnection1.Close();
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