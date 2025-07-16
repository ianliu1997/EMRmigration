using System;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System.Data;
using Microsoft.SqlServer.Server;

public class CReportAuthentication
{
    public static void Impersonate(ReportDocument myRpt)
    {
        // Set the login info dynamically for the report
        string username = ConfigurationManager.AppSettings["DBUserName"];
        string password = ConfigurationManager.AppSettings["DBPassword"];
        string Server = ConfigurationManager.AppSettings["DBServer"];
        string Database = ConfigurationManager.AppSettings["DBName"];
        TableLogOnInfo logonInfo = new TableLogOnInfo();

        //Table table = default(Table);

        foreach (Table table in myRpt.Database.Tables)
        {


            logonInfo = table.LogOnInfo;
            logonInfo.ConnectionInfo.ServerName = Server;
            logonInfo.ConnectionInfo.DatabaseName = Database;
            logonInfo.ConnectionInfo.UserID = username;
            logonInfo.ConnectionInfo.Password = password;
            table.ApplyLogOnInfo(logonInfo);
            //Previously in VS2003, table.Location would report "DATABASE.dbo.NAME"  - 
            //and it was possible to use this to change the Location, but in vs2005 table.
            //Location only reports back the NAME.  See below for a fix.
            //http://vstoolsforum.com/blogs/crystal_reports/archive/2007/06.aspx
            table.Location = Database + ".dbo." + table.Name;


            CrystalDecisions.CrystalReports.Engine.Sections crSections = myRpt.ReportDefinition.Sections;
            CrystalDecisions.CrystalReports.Engine.ReportObjects crReportObjects;
            CrystalDecisions.CrystalReports.Engine.ReportDocument crSubreportDocument;
            CrystalDecisions.CrystalReports.Engine.SubreportObject crSubreportObject;
            CrystalDecisions.CrystalReports.Engine.Database crDatabase;
            CrystalDecisions.CrystalReports.Engine.Tables crTables;
            CrystalDecisions.Shared.TableLogOnInfo crTableLogOnInfo;
            System.Text.StringBuilder m_strDebugPage = new System.Text.StringBuilder(4096);

            foreach (CrystalDecisions.CrystalReports.Engine.Section crSection in crSections)
            {
                crReportObjects = crSection.ReportObjects;
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject crReportObject in crReportObjects)
                {

                    if (crReportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        crSubreportObject = (CrystalDecisions.CrystalReports.Engine.SubreportObject)crReportObject;
                        ////open the subreport object and logon as for the general report 
                        crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
                        crDatabase = crSubreportDocument.Database;
                        crTables = crDatabase.Tables;




                        foreach (CrystalDecisions.CrystalReports.Engine.Table bTable in crTables)
                        {
                            

                             
                            crTableLogOnInfo = bTable.LogOnInfo;
                            crTableLogOnInfo.ConnectionInfo.ServerName = Server;
                            crTableLogOnInfo.ConnectionInfo.DatabaseName = Database;
                            crTableLogOnInfo.ConnectionInfo.UserID = username;
                            crTableLogOnInfo.ConnectionInfo.Password = password;
                            bTable.ApplyLogOnInfo(crTableLogOnInfo);



                            //OutputDebugLine("BEFORE");
                            //OutputDebugLine("TABLE NAME: " + bTable.Name);
                            //OutputDebugLine("TABLE LOC: " + bTable.Location);
                            //OutputDebugLine("SERVER: " + crTableLogOnInfo.ConnectionInfo.ServerName);
                            //OutputDebugLine("DB: " + crTableLogOnInfo.ConnectionInfo.DatabaseName);
                            //OutputDebugLine("UID: " + crTableLogOnInfo.ConnectionInfo.UserID);
                            //OutputDebugLine("PWD: " + crTableLogOnInfo.ConnectionInfo.Password);
                            //OutputDebugLine("RN: " + crTableLogOnInfo.ReportName);
                            //OutputDebugLine("TN: " + crTableLogOnInfo.TableName);

                            //crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                            //bTable.ApplyLogOnInfo(crTableLogOnInfo);
                            //strLocation = crConnectionInfo.DatabaseName + ".dbo." + bTable.Location.Substring(bTable.Location.LastIndexOf(".") + 1);
                            //OutputDebugLine("New Location: " + strLocation);
                            //try
                            //{
                            //    bTable.Location = strLocation;
                            //}
                            //catch (Exception ex)
                            //{
                            //    OutputDebugLine("Set Location Error: " + ex.ToString());
                            //    blnErrors = true;
                            //}

                            //OutputDebugLine("AFTER");
                            //OutputDebugLine("TABLE NAME: " + bTable.Name);
                            //OutputDebugLine("TABLE LOC: " + bTable.Location);
                            //OutputDebugLine("SERVER: " + crTableLogOnInfo.ConnectionInfo.ServerName);
                            //OutputDebugLine("DB: " + crTableLogOnInfo.ConnectionInfo.DatabaseName);
                            //OutputDebugLine("UID: " + crTableLogOnInfo.ConnectionInfo.UserID);
                            //OutputDebugLine("PWD: " + crTableLogOnInfo.ConnectionInfo.Password);
                            //OutputDebugLine("RN: " + crTableLogOnInfo.ReportName);
                            //OutputDebugLine("TN: " + crTableLogOnInfo.TableName);
                            //try
                            //{
                            //    blnTest = bTable.TestConnectivity();
                            //    OutputDebugLine("CONNECTED? " + blnTest.ToString());
                            //}
                            //catch (Exception ex)
                            //{
                            //    OutputDebugLine("CONNECTED? NO");
                            //    OutputDebugLine(ex.ToString());
                            //    blnErrors = true;
                            //}

                        }
                    }
                }
            }




        }
    }


    //public static System.Boolean OutputDebugLine(System.String strLine)
    //{

    //    m_strDebugPage.Append("<div>" + Microsoft.SqlServer.Server.HtmlEncode(strLine) + "</div>");

    //}


    //    // --------------------------------------------------------------------------
    //    // SetupReport
    //    //
    //    // Description:
    //    //    sets up the crystal report
    //    //
    //    // Arguments:
    //    //    objCrystalReportDocument             - crystal report document object
    //    //
    //    // Dependencies:
    //    //    GetConnnectionInfo
    //    //    CrystalDescisions
    //    //
    //    // History:
    //    // 03/17/2006 - WSR : created
    //    //
    //    private System.Boolean SetupReport(ref CrystalDecisions.CrystalReports.Engine.ReportDocument objCrystalReportDocument)
    //{

    //    // a heck of a lot of objects used
    //    CrystalDecisions.Shared.ParameterDiscreteValue crParameterDiscreteValue;
    //    CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinitions crParameterFieldDefinitions ;
    //    CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition crParameterFieldLocation ;
    //    CrystalDecisions.Shared.ParameterValues crParameterValues ;
    //    CrystalDecisions.CrystalReports.Engine.Sections crSections ;
    ////	CrystalDecisions.CrystalReports.Engine.Section crSection ;
    //    CrystalDecisions.CrystalReports.Engine.ReportObjects crReportObjects ;
    ////	CrystalDecisions.CrystalReports.Engine.ReportObject crReportObject ;
    //    CrystalDecisions.CrystalReports.Engine.ReportDocument crSubreportDocument ;
    //    CrystalDecisions.CrystalReports.Engine.SubreportObject crSubreportObject ;
    //    CrystalDecisions.Shared.ConnectionInfo crConnectionInfo ;
    //    CrystalDecisions.CrystalReports.Engine.Database crDatabase ;
    //    CrystalDecisions.CrystalReports.Engine.Tables crTables ;
    //    //CrystalDecisions.CrystalReports.Engine.Table aTable ;
    //    //CrystalDecisions.CrystalReports.Engine.Table bTable ;
    //    CrystalDecisions.Shared.TableLogOnInfo crTableLogOnInfo ;
    //    System.Boolean blnTest = false;
    //    System.String strLocation = null;
    //    System.Boolean blnErrors = false;

    //    // instantiate the debug page
    //    System.Text.StringBuilder m_strDebugPage = new System.Text.StringBuilder(4096);
    //    blnErrors = false;

    //    crConnectionInfo = GetConnectionInfo();

    //    crDatabase = objCrystalReportDocument.Database;

    //    crTables = crDatabase.Tables;

    //    //For intCounter = 0 To objCrystalReportDocument.Database.Tables.Count - 1

    //    foreach (CrystalDecisions.CrystalReports.Engine.Table aTable in crTables) {
    //        crTableLogOnInfo = aTable.LogOnInfo;

    //        OutputDebugLine("BEFORE");
    //        OutputDebugLine("TABLE NAME: " + aTable.Name);
    //        OutputDebugLine("TABLE LOC: " + aTable.Location);
    //        OutputDebugLine("SERVER: " + crTableLogOnInfo.ConnectionInfo.ServerName);
    //        OutputDebugLine("DB: " + crTableLogOnInfo.ConnectionInfo.DatabaseName);
    //        OutputDebugLine("UID: " + crTableLogOnInfo.ConnectionInfo.UserID);
    //        OutputDebugLine("PWD: " + crTableLogOnInfo.ConnectionInfo.Password);
    //        OutputDebugLine("RN: " + crTableLogOnInfo.ReportName);
    //        OutputDebugLine("TN: " + crTableLogOnInfo.TableName);

    //        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
    //        aTable.ApplyLogOnInfo(crTableLogOnInfo);
    //        strLocation = crConnectionInfo.DatabaseName + ".dbo." + aTable.Location.Substring(aTable.Location.LastIndexOf(".") + 1);
    //        OutputDebugLine("New Location: " + strLocation);
    //        try {
    //            aTable.Location = strLocation;
    //        } catch (Exception ex) {
    //            OutputDebugLine("Set Location Error: " + ex.ToString());
    //            blnErrors = true;
    //        }

    //        OutputDebugLine("AFTER");
    //        OutputDebugLine("TABLE NAME: " + aTable.Name);
    //        OutputDebugLine("TABLE LOC: " + aTable.Location);
    //        OutputDebugLine("SERVER: " + crTableLogOnInfo.ConnectionInfo.ServerName);
    //        OutputDebugLine("DB: " + crTableLogOnInfo.ConnectionInfo.DatabaseName);
    //        OutputDebugLine("UID: " + crTableLogOnInfo.ConnectionInfo.UserID);
    //        OutputDebugLine("PWD: " + crTableLogOnInfo.ConnectionInfo.Password);
    //        OutputDebugLine("RN: " + crTableLogOnInfo.ReportName);
    //        OutputDebugLine("TN: " + crTableLogOnInfo.TableName);
    //        try {
    //            blnTest = aTable.TestConnectivity();
    //            OutputDebugLine("CONNECTED? " + blnTest.ToString());
    //        } catch (Exception ex) {
    //            OutputDebugLine("CONNECTED? NO");
    //            OutputDebugLine(ex.ToString());
    //            blnErrors = true;
    //        }

    //        //// THIS STUFF HERE IS FOR REPORTS HAVING SUBREPORTS 
    //        //// set the sections object to the current report's section 
    //        crSections = objCrystalReportDocument.ReportDefinition.Sections;

    //        //// loop through all the sections to find all the report objects 

    //        foreach (CrystalDecisions.CrystalReports.Engine.Section crSection in crSections) {
    //            crReportObjects = crSection.ReportObjects;

    //            ////loop through all the report objects in there to find all subreports 

    //            foreach ( CrystalDecisions.CrystalReports.Engine.ReportObject crReportObject in crReportObjects) {

    //                if (crReportObject.Kind == ReportObjectKind.SubreportObject) {
    //                    crSubreportObject = (CrystalDecisions.CrystalReports.Engine.SubreportObject)crReportObject;
    //                    ////open the subreport object and logon as for the general report 
    //                    crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
    //                    crDatabase = crSubreportDocument.Database;
    //                    crTables = crDatabase.Tables;


    //                    foreach (CrystalDecisions.CrystalReports.Engine.Table bTable in crTables) {
    //                        crTableLogOnInfo = bTable.LogOnInfo;

    //                        OutputDebugLine("BEFORE");
    //                        OutputDebugLine("TABLE NAME: " + bTable.Name);
    //                        OutputDebugLine("TABLE LOC: " + bTable.Location);
    //                        OutputDebugLine("SERVER: " + crTableLogOnInfo.ConnectionInfo.ServerName);
    //                        OutputDebugLine("DB: " + crTableLogOnInfo.ConnectionInfo.DatabaseName);
    //                        OutputDebugLine("UID: " + crTableLogOnInfo.ConnectionInfo.UserID);
    //                        OutputDebugLine("PWD: " + crTableLogOnInfo.ConnectionInfo.Password);
    //                        OutputDebugLine("RN: " + crTableLogOnInfo.ReportName);
    //                        OutputDebugLine("TN: " + crTableLogOnInfo.TableName);

    //                        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
    //                        bTable.ApplyLogOnInfo(crTableLogOnInfo);
    //                        strLocation = crConnectionInfo.DatabaseName + ".dbo." + bTable.Location.Substring(bTable.Location.LastIndexOf(".") + 1);
    //                        OutputDebugLine("New Location: " + strLocation);
    //                        try {
    //                            bTable.Location = strLocation;
    //                        } catch (Exception ex) {
    //                            OutputDebugLine("Set Location Error: " + ex.ToString());
    //                            blnErrors = true;
    //                        }

    //                        OutputDebugLine("AFTER");
    //                        OutputDebugLine("TABLE NAME: " + bTable.Name);
    //                        OutputDebugLine("TABLE LOC: " + bTable.Location);
    //                        OutputDebugLine("SERVER: " + crTableLogOnInfo.ConnectionInfo.ServerName);
    //                        OutputDebugLine("DB: " + crTableLogOnInfo.ConnectionInfo.DatabaseName);
    //                        OutputDebugLine("UID: " + crTableLogOnInfo.ConnectionInfo.UserID);
    //                        OutputDebugLine("PWD: " + crTableLogOnInfo.ConnectionInfo.Password);
    //                        OutputDebugLine("RN: " + crTableLogOnInfo.ReportName);
    //                        OutputDebugLine("TN: " + crTableLogOnInfo.TableName);
    //                        try {
    //                            blnTest = bTable.TestConnectivity();
    //                            OutputDebugLine("CONNECTED? " + blnTest.ToString());
    //                        } catch (Exception ex) {
    //                            OutputDebugLine("CONNECTED? NO");
    //                            OutputDebugLine(ex.ToString());
    //                            blnErrors = true;
    //                        }

    //                    }

    //                }

    //            }

    //        }

    //    }

    //    // get parameter fields from report
    //    crParameterFieldDefinitions = objCrystalReportDocument.DataDefinition.ParameterFields;

    //    //    ' Set the first parameter
    //    //    ' - Get the parameter, tell it to use the current values vs default value.
    //    //    ' - Tell it the parameter contains 1 discrete value vs multiple values.
    //    //    ' - Set the parameter's value.
    //    //    ' - Add it and apply it.
    //    //    ' - Repeat these statements for each parameter.
    //    //    '
    //    crParameterFieldLocation = crParameterFieldDefinitions.Item("@psindex");
    //    crParameterValues = crParameterFieldLocation.CurrentValues;
    //    crParameterDiscreteValue = new CrystalDecisions.Shared.ParameterDiscreteValue();
    //    crParameterDiscreteValue.Value = m_intReportID;
    //    crParameterValues.Add(crParameterDiscreteValue);
    //    crParameterFieldLocation.ApplyCurrentValues(crParameterValues);

    //    // if there were errors

    //    if (blnErrors) {
    //        // display debug page
    //        OutputDebugPage();

    //    }

    //}
    //    // --------------------------------------------------------------------------


    //    // --------------------------------------------------------------------------
    //    // GetConnectionInfo
    //    //
    //    // Description:
    //    //    retrieves the connection information from the data layer object
    //    //
    //    // Arguments: none
    //    //
    //    // Dependencies:
    //    //    DataLayer.GetConnectInfo
    //    //
    //    // History:
    //    // 03/17/2006 - WSR : created
    //    //
    //    private CrystalDecisions.Shared.ConnectionInfo GetConnectionInfo()
    //    {

    //        CrystalDecisions.Shared.ConnectionInfo objConnectionInfo = default(CrystalDecisions.Shared.ConnectionInfo);
    //        System.String strDSN = null;
    //        System.String strDB = null;
    //        System.String strUID = null;
    //        System.String strPWD = null;
    //        System.Boolean blnTrust = false;

    //        // get connection information from data layer
    //        m_objDataLayer.GetConnectInfo(strDSN, strDB, strUID, strPWD, blnTrust);

    //        // create new crystal connection info object
    //        objConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();

    //        // populate it
    //        objConnectionInfo.IntegratedSecurity = false;
    //        objConnectionInfo.ServerName = strDSN;
    //        objConnectionInfo.UserID = strUID;
    //        objConnectionInfo.Password = strPWD;
    //        objConnectionInfo.DatabaseName = strDB;

    //        // return object
    //        return objConnectionInfo;

    //    }
    //    // --------------------------------------------------------------------------

    //    // --------------------------------------------------------------------------
    //    // OutputDebugLine
    //    //
    //    // Description: appends a line to the debug string builder
    //    //
    //    // Arguments: text to add
    //    //
    //    // Dependencies:
    //    //    m_strDebugPage
    //    //
    //    // History:
    //    // 03/17/2006 - WSR : created
    //    // 2007.04.25 - WSR : revised to use string builder
    //    //
    //public System.Boolean OutputDebugLine(System.String strLine)
    //{

    //    m_strDebugPage.Append("<div>" + Microsoft.SqlServer.Server.HtmlEncode(strLine) + "</div>");

    //}
    //    // --------------------------------------------------------------------------


    //    // --------------------------------------------------------------------------
    //    // OutputDebugPage
    //    //
    //    // Description: sends debug string builder to response
    //    //
    //    // Arguments: none
    //    //
    //    // Dependencies:
    //    //    m_strDebugPage
    //    //
    //    // History:
    //    // 2007.04.25 - WSR : created
    //    //
    //    public System.Boolean OutputDebugPage()
    //    {

    //        var _with1 = Response;
    //        _with1.ClearHeaders();
    //        _with1.ClearContent();
    //        _with1.ContentType = "text/html";

    //        Response.Write("<html><head><title>Debug Page</title></head><body>");
    //        Response.Write(m_strDebugPage.ToString());
    //        Response.Write("</body></html>");

    //        Response.Flush();
    //        Response.End();

    //    }
    //    // --------------------------------------------------------------------------



}
