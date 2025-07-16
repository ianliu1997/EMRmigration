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
    public partial class ClinicWisefortheselectedduration : System.Web.UI.Page
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
            String strReportName = "Clinic Wise Report For The Selected Duration";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
            DateTime? DTF = null;
            DateTime? DTT = null;
            DateTime? DTP = null;
            long ClinicID = 0;
            bool IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
            if (Request.QueryString["ClinicID"] != null && Request.QueryString["ClinicID"] != "")
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                DTT = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                DTP = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }
            if (!IsExporttoExcel)
            {
                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptClinicWisefortheselectedduration.rpt"));

                myDoc.SetParameterValue("@ClinicID", ClinicID);
                myDoc.SetParameterValue("@FromDate", DTF);
                myDoc.SetParameterValue("@ToDate", DTT);
                myDoc.SetParameterValue("@ToDatePrint", DTP);

                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);

                CrystalReportViewer1.ReportSource = myDoc;

                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                CrystalReportViewer1.DataBind();
            }

            else
            {
                DataTable dt = new DataTable();

                SqlConnection sqlConnection1 = new SqlConnection(strConnection);
                sqlConnection1.Open();
            
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection1;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CIMS_ClinicWiseSalesReport";
                    
                SqlParameter param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = ClinicID;
                param.ParameterName = "ClinicID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
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
                cmd.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                    ExportToExcel.CExcelExport objExcel = new ExportToExcel.CExcelExport();
                    DataTable dtReturn = new DataTable();
                    objExcel.ConnectionString = strConnection;
                    if (dt.Rows.Count > 0)
                    {
                      
                        //Change Order ad define in Report
                        var varlist = from c in dt.AsEnumerable()

                        select new
                        {
                            SrNo = 0,
                            Date="",
                            Description ="",
                            patientvisitCount = 0,
                            CountOfConsultancy = 0,
                            consultationrevenue = 0.0,
                            CountOfOtherServices =0,
                            otherrevenue = 0.0,
                            CountOfPath = 0,
                            diagnosticrevenue = 0.0,
                            PharmacyServices = 0,
                            pharmacyrevenue = 0.0,
                            LoyaltyCardCount = 0,
                            LoyaltyCardRevenue =0.0,
                            TotalRevenue = 0.0,
                            LastMonthPatCount = 0,
                            SecondLastMonthCountTilDate = 0,
                            IncreaseDecreasePatCount = "",
                            LastMonthrevenue = 0.0,
                            SecondLastMonthrevenue = 0.0,
                            PercentageIncreaseDecreaseForRevenue = "",
                            AvgPatientCount = 0.0,
                            YTDPatientCountTilDate = 0,
                            YTDrevenue = 0.0
                            
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
                    
                    if (dtReturn != null)
                    {
                        if (dtReturn.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtReturn.Columns.Count; i++)
                            {
                                if (dtReturn.Columns[i].ColumnName.Equals("SrNo"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Sr. No";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("Description"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Clinic Name";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("patientvisitCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Patient Count";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfConsultancy"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Consultancy";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("consultationrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Consultation Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfOtherServices"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Other Services";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("otherrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Other Services Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfPath"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Diagnostics";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("diagnosticrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Diagnostics Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfPharmacy"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Pharmarcy";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("pharmacyrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Pharmarcy Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("TotalRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Total Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("AvgBillingRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Avg Revenue";
                                }
                                //
                                if (dtReturn.Columns[i].ColumnName.Equals("LoyaltyCardCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Loyalty Card";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("LoyaltyCardRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Loyalty Card Revenue";
                                }
                                //if (dtReturn.Columns[i].ColumnName.Equals("AvgRevenue"))
                                //{
                                //    dtReturn.Columns[i].ColumnName = "Averege Revenue";
                                //}
                                if (dtReturn.Columns[i].ColumnName.Equals("LastMonthPatCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "LDT Patient Count";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("SecondLastMonthCountTilDate"))
                                {
                                    dtReturn.Columns[i].ColumnName = "LLDT Patient Count";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("IncreaseDecreasePatCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Percentage Increase/Decrease";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("LastMonthrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "LDT Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("SecondLastMonthrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "LLDT Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("PercentageIncreaseDecreaseForRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Percentage Increase / Decrease";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("AvgPatientCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Average Patients Per Day";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("YTDPatientCountTilDate"))
                                {
                                    dtReturn.Columns[i].ColumnName = "YTD Patient";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("YTDrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "YTD Revenue";
                                }
                            }
                            for (int i = 0; i < dtReturn.Rows.Count; i++)
                            {
                                dtReturn.Rows[i]["Sr. No"] = i + 1;
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Description"])))
                                {
                                    dtReturn.Rows[i]["Clinic Name"] = dt.Rows[i]["Description"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Date"])))
                                {
                                    dtReturn.Rows[i]["Date"] =((DateTime) dt.Rows[i]["Date"]).ToString("dd-MMM-yyyy");
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["patientvisitCount"])))
                                {
                                    dtReturn.Rows[i]["Patient Count"] = dt.Rows[i]["patientvisitCount"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CountOfConsultancy"])))
                                {
                                    dtReturn.Rows[i]["Consultancy"] = dt.Rows[i]["CountOfConsultancy"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["consultationrevenue"])))
                                {
                                    dtReturn.Rows[i]["Consultation Revenue"] = dt.Rows[i]["consultationrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CountOfOtherServices"])))
                                {
                                    dtReturn.Rows[i]["Other Services"] = dt.Rows[i]["CountOfOtherServices"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["otherrevenue"])))
                                {
                                    dtReturn.Rows[i]["Other Services Revenue"] = dt.Rows[i]["otherrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PharmacyServices"])))
                                {
                                    dtReturn.Rows[i]["PharmacyServices"] = dt.Rows[i]["PharmacyServices"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["diagnosticrevenue"])))
                                {
                                    dtReturn.Rows[i]["Diagnostics Revenue"] = dt.Rows[i]["diagnosticrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CountOfPath"])))
                                {
                                    dtReturn.Rows[i]["Diagnostics"] = dt.Rows[i]["CountOfPath"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["pharmacyrevenue"])))
                                {
                                    dtReturn.Rows[i]["Pharmarcy Revenue"] = dt.Rows[i]["pharmacyrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LoyaltyCardCount"])))
                                {
                                    dtReturn.Rows[i]["Loyalty Card"] = dt.Rows[i]["LoyaltyCardCount"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LoyaltyCardRevenue"])))
                                {
                                    dtReturn.Rows[i]["Loyalty Card Revenue"] = dt.Rows[i]["LoyaltyCardRevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalRevenue"])))
                                {
                                    dtReturn.Rows[i]["Total Revenue"] = dt.Rows[i]["TotalRevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LastMonthPatCount"])))
                                {
                                    dtReturn.Rows[i]["LDT Patient Count"] = dt.Rows[i]["LastMonthPatCount"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SecondLastMonthCountTilDate"])))
                                {
                                    dtReturn.Rows[i]["LLDT Patient Count"] = dt.Rows[i]["SecondLastMonthCountTilDate"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["IncreaseDecreasePatCount"])))
                                {
                                    dtReturn.Rows[i]["Percentage Increase/Decrease"] = dt.Rows[i]["IncreaseDecreasePatCount"];
                                }
                               if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LastMonthrevenue"])))
                               {
                                   dtReturn.Rows[i]["LDT Revenue"] = dt.Rows[i]["LastMonthrevenue"];
                               }
                               if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SecondLastMonthrevenue"])))
                               {
                                   dtReturn.Rows[i]["LLDT Revenue"] = dt.Rows[i]["SecondLastMonthrevenue"];
                               }
                               if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PercentageIncreaseDecreaseForRevenue"])))
                               {
                                   dtReturn.Rows[i]["Percentage Increase / Decrease"] = dt.Rows[i]["PercentageIncreaseDecreaseForRevenue"];
                               }
                               if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["AvgPatientCount"])))
                               {
                                   dtReturn.Rows[i]["Average Patients Per Day"] = dt.Rows[i]["AvgPatientCount"];
                               }
                               if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["YTDPatientCountTilDate"])))
                               {
                                   dtReturn.Rows[i]["YTD Patient"] = dt.Rows[i]["YTDPatientCountTilDate"];
                               }
                               if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["YTDrevenue"])))
                               {
                                   dtReturn.Rows[i]["YTD Revenue"] = dt.Rows[i]["YTDrevenue"];
                               }                              
                         
                            }
                            
                            objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                        }
                        else
                        {
                            dt = new DataTable();
                            dt.Columns.Add("Sr. No");
                            dt.Columns.Add("Clinc Name");
                            dt.Columns.Add("Patient Count");
                            dt.Columns.Add("Consultancy");
                            dt.Columns.Add("Consultation Revenue");
                            dt.Columns.Add("Other Services");
                            dt.Columns.Add("Other Services Revenue");
                            dt.Columns.Add("Diagnostics ");
                            dt.Columns.Add("Diagnostics Revenue");
                            dt.Columns.Add("Pharmarcy ");
                            dt.Columns.Add("Pharmarcy Revenue");
                            dt.Columns.Add("LDT Patient Count");
                            dt.Columns.Add("LLDT Patient Count");
                            dt.Columns.Add("Percentage Increase/Decrease");
                            dt.Columns.Add("LDT Revenue");
                            dt.Columns.Add("LLDT Revenue");
                            dt.Columns.Add("Percentage Increase/Decrease");
                            dt.Columns.Add("Average Patients Per Day");
                            dt.Columns.Add("YTD Patient");
                            dt.Columns.Add("YTD Revenue");
                            dt.Columns.Add("Total Revenue");
                            dt.Columns.Add("Avg Revenue");
                            objExcel.ExportToExcel("", strReportName, strPath, dt);
                        }

                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Sr. No");
                        dt.Columns.Add("Clinc Name");
                        dt.Columns.Add("Patient Count");
                        dt.Columns.Add("Consultancy");
                        dt.Columns.Add("Consultation Revenue");
                        dt.Columns.Add("Other Services");
                        dt.Columns.Add("Other Services Revenue");
                        dt.Columns.Add("Diagnostics ");
                        dt.Columns.Add("Diagnostics Revenue");
                        dt.Columns.Add("Pharmarcy ");
                        dt.Columns.Add("Pharmarcy Revenue");
                        dt.Columns.Add("LDT Patient Count");
                        dt.Columns.Add("LLDT Patient Count");
                        dt.Columns.Add("Percentage Increase/Decrease");
                        dt.Columns.Add("LDT Revenue");
                        dt.Columns.Add("LLDT Revenue");
                        dt.Columns.Add("Percentage Increase/Decrease");
                        dt.Columns.Add("Average Patients Per Day");
                        dt.Columns.Add("YTD Patient");
                        dt.Columns.Add("YTD Revenue");
                        dt.Columns.Add("Total Revenue");
                        dt.Columns.Add("Avg Revenue");
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