using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Pathology;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.MachineParameter;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsOrderBookingDAL : clsBaseOrderBookingDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsOrderBookingDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 

                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Adds Patholgy Test Master Details
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public override IValueObject AddPathoTestMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathoTestMasterBizActionVO BizAction = valueObject as clsAddPathoTestMasterBizActionVO;
            try
            {
                clsPathoTestMasterVO objPathoTest = BizAction.TestDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestMasterNew1");
                dbServer.AddInParameter(command, "Code", DbType.String, objPathoTest.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objPathoTest.Description);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "TestPrintName", DbType.String, objPathoTest.TestPrintName);
                dbServer.AddInParameter(command, "IsSubTest", DbType.Boolean, objPathoTest.IsSubTest);//objPathoTest.IsSubTest);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objPathoTest.CategoryID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objPathoTest.ServiceID);
                dbServer.AddInParameter(command, "IsParameter", DbType.Boolean, false); // objPathoTest.IsParameter);
                //BY ROHINI DATED 20.1.16
                dbServer.AddInParameter(command, "TurnAroundTime", DbType.Double, objPathoTest.TurnAroundTime);
                dbServer.AddInParameter(command, "TubeID", DbType.Int64, objPathoTest.TubeID);
                dbServer.AddInParameter(command, "IsFormTemplate", DbType.Int64, objPathoTest.IsFormTemplate);
                dbServer.AddInParameter(command, "IsAbnormal", DbType.Boolean, objPathoTest.IsAbnormal);
                //
                dbServer.AddInParameter(command, "Note", DbType.String, objPathoTest.Note);
                dbServer.AddInParameter(command, "HasNormalRange", DbType.Boolean, objPathoTest.HasNormalRange);
                dbServer.AddInParameter(command, "HasObserved", DbType.Boolean, objPathoTest.HasObserved);
                dbServer.AddInParameter(command, "PrintTestName", DbType.Boolean, objPathoTest.PrintTestName);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPathoTest.Status);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "Time", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //dbServer.AddInParameter(command, "HasThreeLevels", DbType.Boolean, objPathoTest.HasThreeLevels);
                dbServer.AddInParameter(command, "FreeFormat", DbType.Boolean, objPathoTest.FreeFormat);
                dbServer.AddInParameter(command, "NeedAuthorization", DbType.Boolean, objPathoTest.NeedAuthorization);
                dbServer.AddInParameter(command, "IsCultureSenTest", DbType.Boolean, objPathoTest.IsCultureSenTest);
                dbServer.AddInParameter(command, "MachineID", DbType.Int64, objPathoTest.MachineID);
                dbServer.AddInParameter(command, "Technique", DbType.String, objPathoTest.Technique);
                if(objPathoTest.FootNote !=string.Empty)
                   dbServer.AddInParameter(command, "FootNote", DbType.String, objPathoTest.FootNote);
                else
                    dbServer.AddInParameter(command, "FootNote", DbType.String, null);
                dbServer.AddInParameter(command, "AppTo", DbType.Int32, objPathoTest.Applicableto);
                dbServer.AddInParameter(command, "ReportTemplate", DbType.Int32, objPathoTest.ReportTemplate);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPathoTest.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);

                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizAction.SuccessStatus == 1)
                {
                    BizAction.TestDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                    //Adding in Patho ParameterDetails table
                    if (BizAction.TestDetails.IsFromParameter == true)
                    {

                        DbCommand command88 = dbServer.GetStoredProcCommand("CIMS_DeletePathologyTemplate");
                        dbServer.AddInParameter(command88, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                        int intStatus88 = dbServer.ExecuteNonQuery(command88);

                        if (objPathoTest.IsSubTest == false)
                        {

                            DbCommand command99 = dbServer.GetStoredProcCommand("CIMS_DeletePathologyParaSubTest");
                            dbServer.AddInParameter(command99, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                            dbServer.AddInParameter(command99, "IsfromSubTest", DbType.Boolean, false);
                            int intStatus99 = dbServer.ExecuteNonQuery(command99);


                            DbCommand command999 = dbServer.GetStoredProcCommand("CIMS_DeletePathologySampleSubTest");
                            dbServer.AddInParameter(command999, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                            dbServer.AddInParameter(command999, "IsfromSubTest", DbType.Boolean, false);
                            int intStatus999 = dbServer.ExecuteNonQuery(command999);

                            foreach (clsPathoTestParameterVO item in objPathoTest.TestParameterList)
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestParameter");
                                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "PathoTestID", DbType.Int64, BizAction.TestDetails.ID);
                                dbServer.AddInParameter(command1, "ParamSTID", DbType.Int64, item.ParamSTID);
                                dbServer.AddInParameter(command1, "IsParameter", DbType.Boolean, true); // item.IsParameter);
                                dbServer.AddInParameter(command1, "PrintNameType", DbType.Int64, 0);
                                dbServer.AddInParameter(command1, "PrintName", DbType.String, null);
                                dbServer.AddInParameter(command1, "PrintPosition", DbType.Int64, item.PrintPosition);
                                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                                int addPathoParameterSucessStaus = dbServer.ExecuteNonQuery(command1);

                            }
                            foreach (clsPathoSubTestVO item in objPathoTest.SubTestList)
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestParameter");
                                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "PathoTestID", DbType.Int64, BizAction.TestDetails.ID);
                                dbServer.AddInParameter(command1, "ParamSTID", DbType.Int64, item.ParamSTID);
                                dbServer.AddInParameter(command1, "IsParameter", DbType.Boolean, false);//item.IsParameter);
                                dbServer.AddInParameter(command1, "PrintNameType", DbType.Int64, 0);
                                dbServer.AddInParameter(command1, "PrintName", DbType.String, null);
                                dbServer.AddInParameter(command1, "PrintPosition", DbType.Int64, item.PrintPosition);
                                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                                int addPathoParameterSucessStaus = dbServer.ExecuteNonQuery(command1);
                            }
                            foreach (clsPathoTestSampleVO item in objPathoTest.TestSampleList)
                            {
                                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestSample");
                                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "PathoTestID", DbType.Int64, BizAction.TestDetails.ID);
                                dbServer.AddInParameter(command2, "SampleID", DbType.Int64, item.SampleID);
                                //dbServer.AddInParameter(command2, "FreqUnitID", DbType.Int64, item.FreqUnitID);
                                dbServer.AddInParameter(command2, "Frequency", DbType.String, item.Frequency);
                                dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
                                dbServer.AddInParameter(command2, "Status", DbType.Double, true);

                                dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddInParameter(command2, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                                int addPathoSampleSucessStaus = dbServer.ExecuteNonQuery(command2);
                            }

                        }
                        else
                        {
                            DbCommand command99 = dbServer.GetStoredProcCommand("CIMS_DeletePathologyParaSubTest");
                            dbServer.AddInParameter(command99, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                            dbServer.AddInParameter(command99, "IsfromSubTest", DbType.Boolean, true);
                            int intStatus99 = dbServer.ExecuteNonQuery(command99);

                            DbCommand command999 = dbServer.GetStoredProcCommand("CIMS_DeletePathologySampleSubTest");
                            dbServer.AddInParameter(command999, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                            dbServer.AddInParameter(command999, "IsfromSubTest", DbType.Boolean, true);
                            int intStatus999 = dbServer.ExecuteNonQuery(command999);

                            foreach (clsPathoTestParameterVO item in objPathoTest.TestParameterList)
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoSubTestParameter");
                                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "PathoTestID", DbType.Int64, BizAction.TestDetails.ID);
                                dbServer.AddInParameter(command1, "ParamSTID", DbType.Int64, item.ParamSTID);
                                dbServer.AddInParameter(command1, "IsParameter", DbType.Boolean, true); // item.IsParameter);
                                dbServer.AddInParameter(command1, "PrintNameType", DbType.Int64, item.SelectedPrintName.ID);
                                dbServer.AddInParameter(command1, "PrintName", DbType.String, item.Print);
                                dbServer.AddInParameter(command1, "PrintPosition", DbType.Int64, item.PrintPosition);
                                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                                int addPathoParameterSucessStaus = dbServer.ExecuteNonQuery(command1);

                            }
                            foreach (clsPathoTestSampleVO item in objPathoTest.TestSampleList)
                            {
                                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoSubTestSample");
                                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "PathoTestID", DbType.Int64, BizAction.TestDetails.ID);
                                dbServer.AddInParameter(command2, "SampleID", DbType.Int64, item.SampleID);
                                dbServer.AddInParameter(command2, "Frequency", DbType.String, item.Frequency);
                                dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
                                dbServer.AddInParameter(command2, "Status", DbType.Double, true);

                                dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddInParameter(command2, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                                int addPathoSampleSucessStaus = dbServer.ExecuteNonQuery(command2);
                            }
                        }
                        // Adding Subtest details Added by Saily P on 24.11.11

                    }
                    //Adding into PathoTestSample Details


                    //Adding into PathoTestItem Details

                    foreach (clsPathoTestItemDetailsVO item in objPathoTest.TestItemList)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestItemDetails");
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                        dbServer.AddInParameter(command3, "ItemID", DbType.Int64, item.ItemID);
                        dbServer.AddInParameter(command3, "Quantity", DbType.Double, item.Quantity);
                        dbServer.AddInParameter(command3, "Status", DbType.Int64, true);
                        //by rohini dated 22.1.16
                        //dbServer.AddInParameter(command3, "UID", DbType.Int64, item.UID);
                        //dbServer.AddInParameter(command3, "UName", DbType.String, item.UName);
                        //dbServer.AddInParameter(command3, "DID", DbType.Int64, item.DID);
                        //dbServer.AddInParameter(command3, "DName", DbType.String, item.DName);
                        //dbServer.AddInParameter(command3, "UOMid", DbType.Int64, item.UOMid);

                        dbServer.AddInParameter(command3, "UID", DbType.Int64, item.SelectedUID.ID);
                        dbServer.AddInParameter(command3, "UName", DbType.String, item.SelectedUID.Description);
                        dbServer.AddInParameter(command3, "DID", DbType.Int64, item.SelectedDID.ID);
                        dbServer.AddInParameter(command3, "DName", DbType.String, item.SelectedDID.Description);
                        dbServer.AddInParameter(command3, "UOMid", DbType.Int64, item.SelectedUOM.ID);
                        //
                        dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command3, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int addPathoSampleSucessStaus = dbServer.ExecuteNonQuery(command3);
                    }


                    //added by rohini dated 21.1.16                 

                    if (BizAction.TestDetails.IsFromParameter == false)
                    {
                        DbCommand command99 = dbServer.GetStoredProcCommand("CIMS_DeletePathologyParaSubTest");
                        dbServer.AddInParameter(command99, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                        dbServer.AddInParameter(command99, "IsfromSubTest", DbType.Boolean, false);
                        int intStatus99 = dbServer.ExecuteNonQuery(command99);

                        if (BizAction.IsUpdate == true)
                        {
                            if (objPathoTest.TestTemplateList != null && objPathoTest.TestTemplateList.Count > 0)
                            {

                                DbCommand command444 = dbServer.GetStoredProcCommand("CIMS_DeletePathologyTemplate");
                                dbServer.AddInParameter(command444, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                                int intStatus2 = dbServer.ExecuteNonQuery(command444);
                            }

                            foreach (var ObjTemplate in objPathoTest.TestTemplateList)
                            {
                                DbCommand command111 = dbServer.GetStoredProcCommand("CIMS_AddPathoTestTemplateDetailMaster");

                                dbServer.AddInParameter(command111, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                                dbServer.AddInParameter(command111, "TemplateID", DbType.Int64, ObjTemplate.TemplateID);
                                dbServer.AddInParameter(command111, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command111, "Status", DbType.Boolean, ObjTemplate.Status);
                                dbServer.AddInParameter(command111, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command111, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command111, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command111, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command111, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddParameter(command111, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTemplate.ID);

                                int iStatus = dbServer.ExecuteNonQuery(command111);
                                ObjTemplate.ID = (long)dbServer.GetParameterValue(command111, "ID");

                            }
                        }
                        else
                        {

                            foreach (var ObjTemplate in objPathoTest.TestTemplateList)
                            {
                                DbCommand command111 = dbServer.GetStoredProcCommand("CIMS_AddPathoTestTemplateDetailMaster");

                                dbServer.AddInParameter(command111, "TestID", DbType.Int64, BizAction.TestDetails.ID);
                                dbServer.AddInParameter(command111, "TemplateID", DbType.Int64, ObjTemplate.TemplateID);
                                dbServer.AddInParameter(command111, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command111, "Status", DbType.Boolean, ObjTemplate.Status);
                                dbServer.AddInParameter(command111, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command111, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command111, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command111, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command111, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddParameter(command111, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTemplate.ID);

                                int iStatus = dbServer.ExecuteNonQuery(command111);
                                ObjTemplate.ID = (long)dbServer.GetParameterValue(command111, "ID");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        /// <summary>
        /// Gets Pathology Test Master List
        /// </summary>
        /// <param name="valueObject">clsGetPathoTestDetailsBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetPathoTestDetailsBizActionVO object</returns>
        public override IValueObject GetPathoTestMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestDetailsBizActionVO BizActionObj = valueObject as clsGetPathoTestDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestList");

                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                //dbServer.AddInParameter(command, "Category", DbType.Int64, BizActionObj.Category);
                //dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsPathoTestMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO objPathoTestVO = new clsPathoTestMasterVO();
                        objPathoTestVO.ID = (long)reader["ID"];
                        objPathoTestVO.UnitID = (long)reader["UnitID"];
                        objPathoTestVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objPathoTestVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        //BY ROHINI DATED20.1.16
                        objPathoTestVO.TurnAroundTime = Convert.ToDouble(DALHelper.HandleDBNull(reader["TurnAroundTime"]));
                        objPathoTestVO.TubeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TubeID"]));
                        objPathoTestVO.IsFormTemplate = Convert.ToInt16(DALHelper.HandleDBNull(reader["IsFormTemplate"]));
                        objPathoTestVO.IsAbnormal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAbnormal"]));
                        //
                        objPathoTestVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objPathoTestVO.CategoryID = (long)reader["CategoryID"];
                        objPathoTestVO.ServiceID = (long)reader["ServiceID"];
                        objPathoTestVO.TestPrintName = (string)DALHelper.HandleDBNull(reader["TestPrintName"]);
                        objPathoTestVO.IsSubTest = (bool)DALHelper.HandleDBNull(reader["IsSubTest"]);
                        objPathoTestVO.Applicableto = Convert.ToInt16(DALHelper.HandleDBNull(reader["Applicableto"]));
                        objPathoTestVO.IsParameter = (bool)DALHelper.HandleDBNull(reader["IsParameter"]);
                        objPathoTestVO.Note = (string)DALHelper.HandleDBNull(reader["Note"]);
                        objPathoTestVO.HasNormalRange = (bool)DALHelper.HandleDBNull(reader["HasNormalRange"]);
                        objPathoTestVO.HasObserved = (bool)DALHelper.HandleDBNull(reader["HasObserved"]);
                        objPathoTestVO.PrintTestName = (bool)DALHelper.HandleDBNull(reader["PrintTestName"]);
                        objPathoTestVO.Time = (DateTime)DALHelper.HandleDBNull(reader["Time"]);
                        objPathoTestVO.NeedAuthorization = (bool)DALHelper.HandleDBNull(reader["NeedAuthorization"]);
                        objPathoTestVO.FreeFormat = (bool)DALHelper.HandleDBNull(reader["FreeFormat"]);
                        objPathoTestVO.MachineID = (long)DALHelper.HandleDBNull(reader["MachineID"]);
                        objPathoTestVO.Technique = (string)DALHelper.HandleDBNull(reader["Technique"]);
                        objPathoTestVO.FootNote = (string)DALHelper.HandleDBNull(reader["FootNote"]);
                        objPathoTestVO.IsCultureSenTest = (bool)DALHelper.HandleDBNull(reader["IsCultureSenTest"]);
                        objPathoTestVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objPathoTestVO.ReportTemplate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ReportTemplate"]));

                        BizActionObj.TestList.Add(objPathoTestVO);
                    }


                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetPathoSubTestMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoSubTestMasterBizActionVO BizActionObj = valueObject as clsGetPathoSubTestMasterBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoSubTestTestList");

                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                //dbServer.AddInParameter(command, "Category", DbType.Int64, BizActionObj.Category);
                //dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsPathoTestMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO objPathoTestVO = new clsPathoTestMasterVO();
                        objPathoTestVO.ID = (long)reader["ID"];
                        objPathoTestVO.UnitID = (long)reader["UnitID"];
                        objPathoTestVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objPathoTestVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objPathoTestVO.TurnAroundTime = (double)DALHelper.HandleDBNull(reader["TurnAroundTime"]);
                        //BY ROHINI DAYED 20.1.16
                        objPathoTestVO.TubeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TubeID"]));
                        objPathoTestVO.IsFormTemplate = Convert.ToInt16(DALHelper.HandleDBNull(reader["IsFormTemplate"]));
                        //
                        objPathoTestVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objPathoTestVO.CategoryID = (long)reader["CategoryID"];
                        objPathoTestVO.ServiceID = (long)reader["ServiceID"];
                        objPathoTestVO.TestPrintName = (string)DALHelper.HandleDBNull(reader["TestPrintName"]);
                        objPathoTestVO.IsSubTest = (bool)DALHelper.HandleDBNull(reader["IsSubTest"]);
                        objPathoTestVO.IsParameter = (bool)DALHelper.HandleDBNull(reader["IsParameter"]);
                        objPathoTestVO.Note = (string)DALHelper.HandleDBNull(reader["Note"]);
                        objPathoTestVO.HasNormalRange = (bool)DALHelper.HandleDBNull(reader["HasNormalRange"]);
                        objPathoTestVO.HasObserved = (bool)DALHelper.HandleDBNull(reader["HasObserved"]);
                        objPathoTestVO.PrintTestName = (bool)DALHelper.HandleDBNull(reader["PrintTestName"]);
                        objPathoTestVO.Time = (DateTime)DALHelper.HandleDBNull(reader["Time"]);
                        objPathoTestVO.NeedAuthorization = (bool)DALHelper.HandleDBNull(reader["NeedAuthorization"]);
                        objPathoTestVO.FreeFormat = (bool)DALHelper.HandleDBNull(reader["FreeFormat"]);
                        objPathoTestVO.MachineID = (long)DALHelper.HandleDBNull(reader["MachineID"]);
                        objPathoTestVO.Technique = (string)DALHelper.HandleDBNull(reader["Technique"]);
                        objPathoTestVO.FootNote = (string)DALHelper.HandleDBNull(reader["FootNote"]);
                        objPathoTestVO.IsCultureSenTest = (bool)DALHelper.HandleDBNull(reader["IsCultureSenTest"]);
                        objPathoTestVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.TestList.Add(objPathoTestVO);
                    }


                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        /// <summary>
        ///  Fills Parameter,Sample & Items related to test id
        /// </summary>
        /// <param name="valueObject">clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO object</returns>
        public override IValueObject GetPathoParameterSampleAndItemDetailsByTestID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO BizActionObj = valueObject as clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoParameterSampleAndItemDetails");

                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                dbServer.AddInParameter(command, "IsFormSubTest", DbType.Int64, BizActionObj.IsFormSubTest);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    //parameter
                    if (BizActionObj.ParameterList == null)
                        BizActionObj.ParameterList = new List<clsPathoTestParameterVO>();
                    while (reader.Read())
                    {
                        clsPathoTestParameterVO ObjTemp = new clsPathoTestParameterVO();
                        ObjTemp.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjTemp.PathoTestID = (long)DALHelper.HandleDBNull(reader["PathoTestID"]);
                        ObjTemp.ParamSTID = (long)DALHelper.HandleDBNull(reader["ParamSTID"]);
                        ObjTemp.Status = false;
                        ObjTemp.IsParameter = true;// (bool)DALHelper.HandleDBNull(reader["IsParameter"]);
                        ObjTemp.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        ObjTemp.SelectedPrintName.ID = (long)DALHelper.HandleDBNull(reader["PrintNameType"]);
                        ObjTemp.SelectedPrintName = ObjTemp.PrintName.FirstOrDefault(q => q.ID == ObjTemp.SelectedPrintName.ID);
                        ObjTemp.Print = (string)DALHelper.HandleDBNull(reader["PrintName"]);
                        ObjTemp.PrintPosition = (long)DALHelper.HandleDBNull(reader["PrintPosition"]);
                        ObjTemp.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"]));
                        BizActionObj.ParameterList.Add(ObjTemp);
                    }
                }
                reader.NextResult();

                //subtest

                if (BizActionObj.SubTestList == null)
                    BizActionObj.SubTestList = new List<clsPathoSubTestVO>();
                while (reader.Read())
                {
                    clsPathoSubTestVO ObjTemp = new clsPathoSubTestVO();
                    ObjTemp.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    ObjTemp.PathoTestID = (long)DALHelper.HandleDBNull(reader["PathoTestID"]);
                    ObjTemp.ParamSTID = (long)DALHelper.HandleDBNull(reader["ParamSTID"]);
                    ObjTemp.Status = false;
                    ObjTemp.IsParameter = false;// (bool)DALHelper.HandleDBNull(reader["IsParameter"]);
                    ObjTemp.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                    ObjTemp.SelectedPrintName.ID = (long)DALHelper.HandleDBNull(reader["PrintNameType"]);
                    ObjTemp.SelectedPrintName = ObjTemp.PrintName.FirstOrDefault(q => q.ID == ObjTemp.SelectedPrintName.ID);
                    ObjTemp.Print = (string)DALHelper.HandleDBNull(reader["PrintName"]);
                    ObjTemp.PrintPosition = (long)DALHelper.HandleDBNull(reader["PrintPosition"]);
                    BizActionObj.SubTestList.Add(ObjTemp);
                }
                reader.NextResult();

                //sample
                if (BizActionObj.SampleList == null)
                    BizActionObj.SampleList = new List<clsPathoTestSampleVO>();

                while (reader.Read())
                {
                    clsPathoTestSampleVO ObjSample = new clsPathoTestSampleVO();
                    ObjSample.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    ObjSample.PathoTestID = (long)DALHelper.HandleDBNull(reader["PathoTestID"]);
                    ObjSample.SampleID = (long)DALHelper.HandleDBNull(reader["SampleID"]);
                    ObjSample.Quantity = (float)(double)DALHelper.HandleDBNull(reader["Quantity"]);
                    ObjSample.SampleName = (string)DALHelper.HandleDBNull(reader["Description"]);
                    ObjSample.Frequency = (string)DALHelper.HandleDBNull(reader["Frequency"]);
                    ObjSample.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                    //ObjSample.FreqUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreqUnitID"]));
                    //ObjSample.FreqUnit = (string)DALHelper.HandleDBNull(reader["FreqUnit"]);

                    BizActionObj.SampleList.Add(ObjSample);
                }
                reader.NextResult();

                if (BizActionObj.ItemList == null)
                    BizActionObj.ItemList = new List<clsPathoTestItemDetailsVO>();

                while (reader.Read())
                {
                    clsPathoTestItemDetailsVO ObjItem = new clsPathoTestItemDetailsVO();
                    ObjItem.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    ObjItem.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                    ObjItem.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                    ObjItem.Quantity = (float)(double)DALHelper.HandleDBNull(reader["Quantity"]);
                    ObjItem.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                    ObjItem.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                    //by rohini dated 22.1.16
                    ObjItem.UID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UID"]));
                    ObjItem.UName = Convert.ToString(DALHelper.HandleDBNull(reader["UName"]));
                    ObjItem.DID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DID"]));
                    ObjItem.DName = Convert.ToString(DALHelper.HandleDBNull(reader["DName"]));
                    ObjItem.UOMid = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOMid"]));
                    ObjItem.UOMName = Convert.ToString(DALHelper.HandleDBNull(reader["UOMName"]));
                    BizActionObj.ItemList.Add(ObjItem);
                }

                reader.NextResult();

                if (BizActionObj.TemplateList == null)
                    BizActionObj.TemplateList = new List<PalashDynamics.ValueObjects.Pathology.PathologyMasters.clsPathoTemplateVO>();

                while (reader.Read())
                {
                    PalashDynamics.ValueObjects.Pathology.PathologyMasters.clsPathoTemplateVO Obj = new PalashDynamics.ValueObjects.Pathology.PathologyMasters.clsPathoTemplateVO();
                    Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    Obj.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                    Obj.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    Obj.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                    Obj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                    BizActionObj.TemplateList.Add(Obj);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        /// <summary>
        /// Adds patho profile master
        /// </summary>
        /// <param name="valueObject">AddPathoProfileMasterBizActionVO</param>
        /// <param name="UserVo">UserVO</param>
        /// <returns>AddPathoProfileMasterBizActionVO</returns>
        public override IValueObject GetPathoParameterUnitsByParamID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoParameterUnitsByParamIDBizActionVO BizActionObj = valueObject as clsGetPathoParameterUnitsByParamIDBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoParameterUnitFromParameterID");
                dbServer.AddInParameter(command, "ParamID", DbType.Int64, BizActionObj.ParamID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                //if (reader.HasRows)
                //{

                //    if (BizActionObj.MasterList == null)
                //    {
                //        BizActionObj.MasterList = new List<MasterListItem>();
                //    }
                //    //Reading the record from reader and stores in list
                //    while (reader.Read())
                //    {
                //        BizActionObj.MasterList.Add(new MasterListItem((long)reader["ID"], reader["Description"].ToString()));
                //    }
                //}
                if (reader.HasRows)
                {
                    //parameter
                    if (BizActionObj.SampleList == null)
                        BizActionObj.SampleList = new List<clsPathoParameterUnitMaterVO>();
                    while (reader.Read())
                    {

                        clsPathoParameterUnitMaterVO ObjSample = new clsPathoParameterUnitMaterVO();
                        ObjSample.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjSample.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        // ObjSample.SampleID = (long)DALHelper.HandleDBNull(reader["SampleID"]);
                        // ObjSample.Quantity = (float)(double)DALHelper.HandleDBNull(reader["Quantity"]);
                        ObjSample.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        // ObjSample.Frequency = (string)DALHelper.HandleDBNull(reader["Frequency"]);
                        ObjSample.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.SampleList.Add(ObjSample);
                    }
                    reader.NextResult();
                }
            }
            catch
            {
                throw;
            }
            return BizActionObj;
        }
        public override IValueObject AddPathoPathoProfileMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathoProfileMasterBizActionVO BizAction = valueObject as clsAddPathoProfileMasterBizActionVO;
            try
            {
                clsPathoProfileMasterVO objPathoProfileTest = BizAction.ProfileDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoProfileMaster");
                //dbServer.AddInParameter(command, "Code", DbType.String, objPathoProfileTest.Code);
                //dbServer.AddInParameter(command, "Description", DbType.String, objPathoProfileTest.Description);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objPathoProfileTest.ServiceID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPathoProfileTest.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPathoProfileTest.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);

                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (BizAction.SuccessStatus == 1)
                {
                    BizAction.ProfileDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                    //Adding in Patho Profile Test Details

                    foreach (clsPathoProfileTestDetailsVO item in objPathoProfileTest.PathoTestList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoProfileTestDetails");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "ProfileID", DbType.Int64, BizAction.ProfileDetails.ID);
                        dbServer.AddInParameter(command1, "TestID", DbType.Int64, item.TestID);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int addPathoProfileTestSucessStaus = dbServer.ExecuteNonQuery(command1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        /// <summary>
        /// Gets patho profile master
        /// </summary>
        /// <param name="valueObject">clsGetPathoProfileDetailsBizActionVO</param>
        /// <param name="UserVo">UserVO</param>
        /// <returns>clsGetPathoProfileDetailsBizActionVO</returns>
        public override IValueObject GetPathoProfileDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileDetailsBizActionVO BizActionObj = valueObject as clsGetPathoProfileDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoProfileList");

                //dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                //dbServer.AddInParameter(command, "Category", DbType.Int64, BizActionObj.Category);
                //dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ProfileList == null)
                        BizActionObj.ProfileList = new List<clsPathoProfileMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoProfileMasterVO objPathoProfileVO = new clsPathoProfileMasterVO();
                        objPathoProfileVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPathoProfileVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        //objPathoProfileVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        //objPathoProfileVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objPathoProfileVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        objPathoProfileVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);

                        objPathoProfileVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.ProfileList.Add(objPathoProfileVO);
                    }


                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;
        }

        /// <summary>
        /// Gets patho profile master
        /// </summary>
        /// <param name="valueObject">clsGetPathoProfileTestByIDBizActionVO</param>
        /// <param name="UserVo">UserVO</param>
        /// <returns>clsGetPathoProfileTestByIDBizActionVO</returns>
        public override IValueObject FillPathoProfileTestByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileTestByIDBizActionVO BizActionObj = valueObject as clsGetPathoProfileTestByIDBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoProfileTestByID");

                dbServer.AddInParameter(command, "ProfileID", DbType.Int64, BizActionObj.ProfileID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    //parameter
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsPathoProfileTestDetailsVO>();
                    while (reader.Read())
                    {
                        clsPathoProfileTestDetailsVO ObjTemp = new clsPathoProfileTestDetailsVO();
                        ObjTemp.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjTemp.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        ObjTemp.ProfileID = (long)DALHelper.HandleDBNull(reader["ProfileID"]);
                        ObjTemp.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        ObjTemp.Status = false;
                        ObjTemp.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        //added by rohini dated 11.4.16
                        ObjTemp.IsEntryFromTestMaster = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEntryFromTestMaster"]));

                        BizActionObj.TestList.Add(ObjTemp);
                    }


                }





                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;

        }

        public override IValueObject FillPathoProfileService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileServicesBizActionVO BizActionObj = valueObject as clsGetPathoProfileServicesBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoProfileServices");

                //  dbServer.AddInParameter(command, "ProfileID", DbType.Int64, BizActionObj.ProfileID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    //parameter
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem ObjTemp = new MasterListItem();
                        ObjTemp.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjTemp.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        ObjTemp.Status = true;
                        BizActionObj.ServiceList.Add(ObjTemp);
                    }

                }


                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;

        }

        #region Pathology Work Order


        public override IValueObject GetPathoOutSourcedTestList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsGetPathoOutSourceTestListBizActionVO objTest = valueObject as clsGetPathoOutSourceTestListBizActionVO;
            clsPathoTestOutSourceDetailsVO objTestVO = objTest.PathoOutSourceTestDetails;
            clsPathoTestOutSourceDetailsVO objVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetOrderBookingOutsourcingTestList");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objTest.sortExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objTest.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objTest.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objTest.MaximumRows);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objTestVO.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objTestVO.ToDate);
                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objTestVO.AgencyID);
                dbServer.AddInParameter(command, "FirstName", DbType.String, objTestVO.FirstName);
                dbServer.AddInParameter(command, "LastName", DbType.String, objTestVO.LastName);
                dbServer.AddInParameter(command, "TestName", DbType.String, objTestVO.TestName);
                dbServer.AddInParameter(command, "OutSourceType", DbType.Boolean, objTestVO.OutSourceType);
                dbServer.AddInParameter(command, "MrNo", DbType.String, objTestVO.MRNo);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objVO = new clsPathoTestOutSourceDetailsVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                        objVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        objVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        objVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        objVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MrNo"]));
                        objVO.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objVO.OrderNo = Convert.ToString(DALHelper.HandleDBNull(reader["OrderNo"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.OrderDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objVO.TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                        objVO.IsOutSourced = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced"]));
                        objVO.IsOutSourced1 = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced1"]));
                        objVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtAgencyID"]));
                        objVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        objVO.IsChangedAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChangedAgency"]));
                        objVO.ReasonToChangeAgency = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyChangeReason"]));
                        objVO.AgencyAssignReason = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyAssignReason"]));
                        objVO.IsOutSourced = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced"]));
                        objVO.IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"]));
                        objVO.SampleDispatchDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]));
                        objVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Pre"]));
                        objTest.PathoOutSourceTestList.Add(objVO);
                    }
                }
                reader.NextResult();
                objTest.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objTest;
        }

        public override IValueObject GetPathoUnAssignedAgencyTestList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsGetPathoOutSourceTestListBizActionVO objTest = valueObject as clsGetPathoOutSourceTestListBizActionVO;
            clsPathoTestOutSourceDetailsVO objTestVO = objTest.PathoOutSourceTestDetails;
            clsPathoTestOutSourceDetailsVO objVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetOrderBookingOutsourcingTestListForUnAssignedAgency");
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objVO = new clsPathoTestOutSourceDetailsVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                        objVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        objVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        objVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        objVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MrNo"]));
                        objVO.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objVO.OrderNo = Convert.ToString(DALHelper.HandleDBNull(reader["OrderNo"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.OrderDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objVO.TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                        objVO.IsOutSourced = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced"]));
                        objVO.IsOutSourced1 = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced1"]));
                        objVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtAgencyID"]));
                        objVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                        objVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        objVO.IsChangedAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChangedAgency"]));
                        objVO.ReasonToChangeAgency = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyChangeReason"]));
                        objVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objTest.UnAssignedAgnecyTestList.Add(objVO);
                    }
                }
                reader.NextResult();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objTest;
        }

        public override IValueObject ChangePathoTestAgency(IValueObject valueObject, clsUserVO UserVO)
        {
            clsChangePathoTestAgencyBizActionVO BizAction = valueObject as clsChangePathoTestAgencyBizActionVO;
            if (BizAction.IsOutsource == true)
            {
                #region For  only OutSourcing the Test
                DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_PathoTestOutSourced");
                try
                {
                    dbServer.AddInParameter(command3, "OrderDetailID", DbType.Int64, BizAction.OutSourceID);
                    dbServer.AddInParameter(command3, "TestId", DbType.Int64, BizAction.PathoOutSourceTestDetails.TestID);

                    dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVO.ID);
                    dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    dbServer.ExecuteNonQuery(command3);
                }
                catch (Exception)
                {
                    if (command3 != null)
                    {
                        command3.Dispose();
                        command3.Connection.Close();
                    }
                }
                finally
                {
                    if (command3 != null)
                    {
                        command3.Dispose();
                        command3.Connection.Close();
                    }
                }
                #endregion
            }
            if (BizAction.PathoOutSourceTestDetails.IsForUnassignedAgencyTest)
            {
                #region For UnAssigned Agency Test
                foreach (clsPathoTestOutSourceDetailsVO item in BizAction.AssignedAgnecyTestList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AssignPathoTestAgency");
                    try
                    {
                        dbServer.AddInParameter(command1, "OrderDetailID", DbType.Int64, item.OrderDetailID);
                        dbServer.AddInParameter(command1, "AgencyID", DbType.Int64, item.ChangedAgencyID);
                        dbServer.AddInParameter(command1, "ChangeReason", DbType.String, item.ReasonToChangeAgency);
                        dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVO.ID);
                        dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        dbServer.ExecuteNonQuery(command1);
                    }
                    catch (Exception)
                    {
                        if (command1 != null)
                        {
                            command1.Dispose();
                            command1.Connection.Close();
                        }
                    }
                    finally
                    {
                        if (command1 != null)
                        {
                            command1.Dispose();
                            command1.Connection.Close();
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region For Chnging Agency
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_ModifyPathoTestAgency");
                try
                {
                    clsPathoTestOutSourceDetailsVO objVO = BizAction.PathoOutSourceTestDetails;
                    dbServer.AddInParameter(command1, "OrderDetailID", DbType.Int64, objVO.OrderDetailID);
                    dbServer.AddInParameter(command1, "AgencyID", DbType.Int64, objVO.ChangedAgencyID);
                    dbServer.AddInParameter(command1, "ChangeReason", DbType.String, objVO.ReasonToChangeAgency);
                    dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVO.ID);
                    dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.ExecuteNonQuery(command1);
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (command1 != null)
                    {
                        command1.Dispose();
                        command1.Connection.Close();
                    }
                }
                #endregion
            }
            return valueObject;
        }

        public override IValueObject GetPathOrderBookingList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderBookingListBizActionVO BizActionObj = valueObject as clsGetPathOrderBookingListBizActionVO;

            if (BizActionObj.IsFrom == "ResultEntry")
            {
                #region rohini saprated all sp
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForResultEntry");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "CatagoryID", DbType.Int64, BizActionObj.CatagoryID);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                    dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                    dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                    dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                    dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                    dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.IsDelivered);

                    //added by rohini 29.2.2016
                    if (BizActionObj.IsDispatchedClinic == true)
                    {
                        dbServer.AddInParameter(command, "IsDispatchedClinic", DbType.Int64, BizActionObj.IsDispatchedClinic);
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "IsDispatchedClinic", DbType.Int64, 0);

                    }                 
                    
                    if (BizActionObj.MRNO != null && BizActionObj.MRNO.Length != 0)
                    {
                        dbServer.AddInParameter(command, "MRNo", DbType.String, "%" + BizActionObj.MRNO + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "MRNo", DbType.String, null);

                    //added by rohini dated 11.2.16

                    if (BizActionObj.BillNo != null && BizActionObj.BillNo.Length != 0)
                    {
                        dbServer.AddInParameter(command, "BillNo", DbType.String, "%" + BizActionObj.BillNo + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "BillNo", DbType.String, null);
                    //

                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "FirstName", DbType.String, "%" + BizActionObj.FirstName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "LastName", DbType.String, "%" + BizActionObj.LastName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "LastName", DbType.String, null);

                   
                    // Changes ForCR Points
                    dbServer.AddInParameter(command, "PatientType", DbType.Int64, BizActionObj.PatientType); //Set for Patient Type - 1 : OPD 2 : IPD
                    //temp
                    dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.UserID);
                    dbServer.AddInParameter(command, "StatusID", DbType.Int64, BizActionObj.StatusID);
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                    //
                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    
                    DbDataReader reader;

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingList == null)
                            BizActionObj.OrderBookingList = new List<clsPathOrderBookingVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingVO objOrderBookingVO = new clsPathOrderBookingVO();
                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            objOrderBookingVO.OrderDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objOrderBookingVO.TestProfile = ((long?)DALHelper.HandleDBNull(reader["TestType"]));
                            objOrderBookingVO.OrderNo = (string)DALHelper.HandleDBNull(reader["OrderNo"]);
                            objOrderBookingVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                            objOrderBookingVO.SampleType = (Boolean)DALHelper.HandleDBNull(reader["SampleType"]);
                            objOrderBookingVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objOrderBookingVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                            objOrderBookingVO.PatientUnitID = (long)DALHelper.HandleIntegerNull(reader["PatientUnitID"]);
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);                          
                            objOrderBookingVO.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                            objOrderBookingVO.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                            objOrderBookingVO.PaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                            objOrderBookingVO.Balance = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                            objOrderBookingVO.IsResultEntry = (bool)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.ReferredBy = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                            objOrderBookingVO.GenderID = (long)DALHelper.HandleIntegerNull(reader["GenderID"]);
                            objOrderBookingVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                            objOrderBookingVO.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorEmailID"]));
                            objOrderBookingVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                            objOrderBookingVO.RegistrationTime = (DateTime?)DALHelper.HandleDBNull(reader["RegistrationTime"]);
                            objOrderBookingVO.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmailId"]));
                            objOrderBookingVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objOrderBookingVO.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIPDPatient"]));  //Set to identify whether it is IPD Patient or OPD
                            objOrderBookingVO.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixID"]));
                            objOrderBookingVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            objOrderBookingVO.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillId"]));
                            objOrderBookingVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CID"])); // by rohini
                            objOrderBookingVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["IsIPD"]));  // by rohini
                            //temp
                            objOrderBookingVO.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader["RS"]));
                            
                            BizActionObj.OrderBookingList.Add(objOrderBookingVO); //commented by rohini for CR Points

                            #region rohini For Color Code CR
                            //try
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPathoOrderColorStatus");

                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, objOrderBookingVO.ID);
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objOrderBookingVO.UnitId);
                            //    dbServer.AddInParameter(command1, "IsFrom", DbType.String, BizActionObj.IsFrom);
                            //    DbDataReader reader1;

                            //    reader1 = (DbDataReader)dbServer.ExecuteReader(command1);

                            //    if (reader1.HasRows)
                            //    {
                            //        if (BizActionObj.OrderBookingList == null)
                            //            BizActionObj.OrderBookingList = new List<clsPathOrderBookingVO>();
                            //        while (reader1.Read())
                            //        {
                            //            objOrderBookingVO.ResultColor = Convert.ToInt64(reader1["ResultStatus"]);
                            //            //BizActionObj.OrderBookingList.Add(objOrderBookingVO);
                            //        }

                            //    }
                            //    reader1.Close();
                               
                            //    BizActionObj.OrderBookingList.Add(objOrderBookingVO);   //by rohini for CR Points

                            //}
                            //catch (Exception ex)
                            //{
                            //    throw;
                            //}
                            //finally
                            //{

                            //}
                            #endregion
                        }

                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

        


                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else if (BizActionObj.IsFrom == "Authorization")
            {
                #region rohini saprated all sp
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForAuthorization");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    //dbServer.AddInParameter(command, "CatagoryID", DbType.Int64, BizActionObj.CatagoryID);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                   // dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                   // dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                    dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                    //dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                   // dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.IsDelivered);

                    //added by rohini 29.2.2016 comented by rohini
                    //if (BizActionObj.IsDispatchedClinic == true)
                    //{
                    //    dbServer.AddInParameter(command, "IsDispatchedClinic", DbType.Int64, BizActionObj.IsDispatchedClinic);
                    //}
                    //else
                    //{
                    //    dbServer.AddInParameter(command, "IsDispatchedClinic", DbType.Int64, 0);

                    //}

                    if (BizActionObj.MRNO != null && BizActionObj.MRNO.Length != 0)
                    {
                        dbServer.AddInParameter(command, "MRNo", DbType.String, "%" + BizActionObj.MRNO + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "MRNo", DbType.String, null);

                    //added by rohini dated 11.2.16

                    if (BizActionObj.BillNo != null && BizActionObj.BillNo.Length != 0)
                    {
                        dbServer.AddInParameter(command, "BillNo", DbType.String, "%" + BizActionObj.BillNo + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "BillNo", DbType.String, null);
                    //

                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "FirstName", DbType.String, "%" + BizActionObj.FirstName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "LastName", DbType.String, "%" + BizActionObj.LastName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "LastName", DbType.String, null);


                    // Changes ForCR Points
                    dbServer.AddInParameter(command, "PatientType", DbType.Int64, BizActionObj.PatientType); //Set for Patient Type - 1 : OPD 2 : IPD
                    //dbServer.AddInParameter(command, "IsStatus", DbType.Int64, BizActionObj.IsStatus);
                    //dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.UserID);
                    dbServer.AddInParameter(command, "StatusID", DbType.Int64, BizActionObj.StatusID);
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                    //
                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    DbDataReader reader;

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingList == null)
                            BizActionObj.OrderBookingList = new List<clsPathOrderBookingVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingVO objOrderBookingVO = new clsPathOrderBookingVO();
                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            objOrderBookingVO.OrderDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objOrderBookingVO.TestProfile = ((long?)DALHelper.HandleDBNull(reader["TestType"]));
                            objOrderBookingVO.OrderNo = (string)DALHelper.HandleDBNull(reader["OrderNo"]);
                            objOrderBookingVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                            objOrderBookingVO.SampleType = (Boolean)DALHelper.HandleDBNull(reader["SampleType"]);
                            objOrderBookingVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objOrderBookingVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                            objOrderBookingVO.PatientUnitID = (long)DALHelper.HandleIntegerNull(reader["PatientUnitID"]);
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);
                            objOrderBookingVO.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                            objOrderBookingVO.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                            objOrderBookingVO.PaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                            objOrderBookingVO.Balance = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                            objOrderBookingVO.IsResultEntry = (bool)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.ReferredBy = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                            objOrderBookingVO.GenderID = (long)DALHelper.HandleIntegerNull(reader["GenderID"]);
                            objOrderBookingVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                            objOrderBookingVO.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorEmailID"]));
                            objOrderBookingVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                            objOrderBookingVO.RegistrationTime = (DateTime?)DALHelper.HandleDBNull(reader["RegistrationTime"]);
                            objOrderBookingVO.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmailId"]));
                            objOrderBookingVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objOrderBookingVO.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIPDPatient"]));  //Set to identify whether it is IPD Patient or OPD
                            objOrderBookingVO.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixID"]));
                            objOrderBookingVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            objOrderBookingVO.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillId"]));
                            objOrderBookingVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CID"])); // by rohini
                            objOrderBookingVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["IsIPD"]));  // by rohini
                            objOrderBookingVO.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader["AS1"]));

                            BizActionObj.OrderBookingList.Add(objOrderBookingVO); //commented by rohini for CR Points

                  
                        }

                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");




                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else if (BizActionObj.IsFrom == "Delivery")
            {
                #region rohini saprated all sp
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForDelivery");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);                  
                    dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);            
                    dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);                 

                    if (BizActionObj.MRNO != null && BizActionObj.MRNO.Length != 0)
                    {
                        dbServer.AddInParameter(command, "MRNo", DbType.String, "%" + BizActionObj.MRNO + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "MRNo", DbType.String, null);

                    //added by rohini dated 11.2.16

                    if (BizActionObj.BillNo != null && BizActionObj.BillNo.Length != 0)
                    {
                        dbServer.AddInParameter(command, "BillNo", DbType.String, "%" + BizActionObj.BillNo + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "BillNo", DbType.String, null);
                    //

                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "FirstName", DbType.String, "%" + BizActionObj.FirstName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "LastName", DbType.String, "%" + BizActionObj.LastName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "LastName", DbType.String, null);
                    // Changes ForCR Points
                    dbServer.AddInParameter(command, "PatientType", DbType.Int64, BizActionObj.PatientType); //Set for Patient Type - 1 : OPD 2 : IPD                  
                    dbServer.AddInParameter(command, "StatusID", DbType.Int64, BizActionObj.StatusID);
                    dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.UserID);
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                    dbServer.AddInParameter(command, "TypeID", DbType.Int64, BizActionObj.TypeID);
                    //
                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    DbDataReader reader;

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingList == null)
                            BizActionObj.OrderBookingList = new List<clsPathOrderBookingVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingVO objOrderBookingVO = new clsPathOrderBookingVO();
                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            objOrderBookingVO.OrderDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objOrderBookingVO.TestProfile = ((long?)DALHelper.HandleDBNull(reader["TestType"]));
                            objOrderBookingVO.OrderNo = (string)DALHelper.HandleDBNull(reader["OrderNo"]);
                            objOrderBookingVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                            objOrderBookingVO.SampleType = (Boolean)DALHelper.HandleDBNull(reader["SampleType"]);
                            objOrderBookingVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objOrderBookingVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                            objOrderBookingVO.PatientUnitID = (long)DALHelper.HandleIntegerNull(reader["PatientUnitID"]);
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);
                            objOrderBookingVO.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                            objOrderBookingVO.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                            objOrderBookingVO.PaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                            objOrderBookingVO.Balance = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                            objOrderBookingVO.IsResultEntry = (bool)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.ReferredBy = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                            objOrderBookingVO.GenderID = (long)DALHelper.HandleIntegerNull(reader["GenderID"]);
                            objOrderBookingVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                            objOrderBookingVO.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorEmailID"]));
                            objOrderBookingVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                            objOrderBookingVO.RegistrationTime = (DateTime?)DALHelper.HandleDBNull(reader["RegistrationTime"]);
                            objOrderBookingVO.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmailId"]));
                            objOrderBookingVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objOrderBookingVO.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIPDPatient"]));  //Set to identify whether it is IPD Patient or OPD
                            objOrderBookingVO.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixID"]));
                            objOrderBookingVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            objOrderBookingVO.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillId"]));
                            objOrderBookingVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CID"])); // by rohini
                            objOrderBookingVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["IsIPD"]));  // by rohini
                            objOrderBookingVO.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader["DS"]));

                            BizActionObj.OrderBookingList.Add(objOrderBookingVO); //commented by rohini for CR Points


                        }

                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");




                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else if (BizActionObj.IsFrom == "UploadReport")
            {
                #region rohini saprated all sp
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForUploadReport");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                    dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                    dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);

                    if (BizActionObj.MRNO != null && BizActionObj.MRNO.Length != 0)
                    {
                        dbServer.AddInParameter(command, "MRNo", DbType.String, "%" + BizActionObj.MRNO + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "MRNo", DbType.String, null);

                    //added by rohini dated 11.2.16

                    if (BizActionObj.BillNo != null && BizActionObj.BillNo.Length != 0)
                    {
                        dbServer.AddInParameter(command, "BillNo", DbType.String, "%" + BizActionObj.BillNo + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "BillNo", DbType.String, null);
                    //

                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "FirstName", DbType.String, "%" + BizActionObj.FirstName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "LastName", DbType.String, "%" + BizActionObj.LastName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "LastName", DbType.String, null);
                    // Changes ForCR Points
                    dbServer.AddInParameter(command, "PatientType", DbType.Int64, BizActionObj.PatientType); //Set for Patient Type - 1 : OPD 2 : IPD                  
                    dbServer.AddInParameter(command, "StatusID", DbType.Int64, BizActionObj.StatusID);                  
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);                  
                    //
                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    DbDataReader reader;

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingList == null)
                            BizActionObj.OrderBookingList = new List<clsPathOrderBookingVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingVO objOrderBookingVO = new clsPathOrderBookingVO();
                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            objOrderBookingVO.OrderDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objOrderBookingVO.TestProfile = ((long?)DALHelper.HandleDBNull(reader["TestType"]));
                            objOrderBookingVO.OrderNo = (string)DALHelper.HandleDBNull(reader["OrderNo"]);
                            objOrderBookingVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                            objOrderBookingVO.SampleType = (Boolean)DALHelper.HandleDBNull(reader["SampleType"]);
                            objOrderBookingVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objOrderBookingVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                            objOrderBookingVO.PatientUnitID = (long)DALHelper.HandleIntegerNull(reader["PatientUnitID"]);
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);
                            objOrderBookingVO.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                            objOrderBookingVO.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                            objOrderBookingVO.PaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                            objOrderBookingVO.Balance = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                            objOrderBookingVO.IsResultEntry = (bool)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.ReferredBy = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                            objOrderBookingVO.GenderID = (long)DALHelper.HandleIntegerNull(reader["GenderID"]);
                            objOrderBookingVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                            objOrderBookingVO.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorEmailID"]));
                            objOrderBookingVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                            objOrderBookingVO.RegistrationTime = (DateTime?)DALHelper.HandleDBNull(reader["RegistrationTime"]);
                            objOrderBookingVO.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmailId"]));
                            objOrderBookingVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objOrderBookingVO.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIPDPatient"]));  //Set to identify whether it is IPD Patient or OPD
                            objOrderBookingVO.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixID"]));
                            objOrderBookingVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            objOrderBookingVO.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillId"]));
                            objOrderBookingVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CID"])); // by rohini
                            objOrderBookingVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["IsIPD"]));  // by rohini
                            objOrderBookingVO.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader["DS"]));

                            BizActionObj.OrderBookingList.Add(objOrderBookingVO); //commented by rohini for CR Points
                        }

                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else if (BizActionObj.IsFrom == "SampleCollection")
            {
                #region rohini saprated all sp
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForSampleCollection");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                    dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                    dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);

                    if (BizActionObj.MRNO != null && BizActionObj.MRNO.Length != 0)
                    {
                        dbServer.AddInParameter(command, "MRNo", DbType.String, "%" + BizActionObj.MRNO + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "MRNo", DbType.String, null);

                    //added by rohini dated 11.2.16

                    if (BizActionObj.BillNo != null && BizActionObj.BillNo.Length != 0)
                    {
                        dbServer.AddInParameter(command, "BillNo", DbType.String, "%" + BizActionObj.BillNo + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "BillNo", DbType.String, null);
                    //

                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "FirstName", DbType.String, "%" + BizActionObj.FirstName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "LastName", DbType.String, "%" + BizActionObj.LastName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "LastName", DbType.String, null);
                    // Changes ForCR Points
                    dbServer.AddInParameter(command, "PatientType", DbType.Int64, BizActionObj.PatientType); //Set for Patient Type - 1 : OPD 2 : IPD                  
                    dbServer.AddInParameter(command, "StatusID", DbType.Int64, BizActionObj.StatusID);
                    dbServer.AddInParameter(command, "IsSubOptimal", DbType.Boolean, BizActionObj.IsSubOptimal);
                    dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.UserID);
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    DbDataReader reader;

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingList == null)
                            BizActionObj.OrderBookingList = new List<clsPathOrderBookingVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingVO objOrderBookingVO = new clsPathOrderBookingVO();
                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            objOrderBookingVO.OrderDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objOrderBookingVO.TestProfile = ((long?)DALHelper.HandleDBNull(reader["TestType"]));
                            objOrderBookingVO.OrderNo = (string)DALHelper.HandleDBNull(reader["OrderNo"]);
                            objOrderBookingVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                            objOrderBookingVO.SampleType = (Boolean)DALHelper.HandleDBNull(reader["SampleType"]);
                            objOrderBookingVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objOrderBookingVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                            objOrderBookingVO.PatientUnitID = (long)DALHelper.HandleIntegerNull(reader["PatientUnitID"]);
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);
                            objOrderBookingVO.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                            objOrderBookingVO.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                            objOrderBookingVO.PaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                            objOrderBookingVO.Balance = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                            objOrderBookingVO.IsResultEntry = (bool)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.ReferredBy = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                            objOrderBookingVO.GenderID = (long)DALHelper.HandleIntegerNull(reader["GenderID"]);
                            objOrderBookingVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                            objOrderBookingVO.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorEmailID"]));
                            objOrderBookingVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                            objOrderBookingVO.RegistrationTime = (DateTime?)DALHelper.HandleDBNull(reader["RegistrationTime"]);
                            objOrderBookingVO.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmailId"]));
                            objOrderBookingVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objOrderBookingVO.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIPDPatient"]));  //Set to identify whether it is IPD Patient or OPD
                            objOrderBookingVO.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixID"]));
                            objOrderBookingVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            objOrderBookingVO.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillId"]));
                            objOrderBookingVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CID"])); // by rohini
                            objOrderBookingVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["IsIPD"]));  // by rohini
                            objOrderBookingVO.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader["CS"]));

                            BizActionObj.OrderBookingList.Add(objOrderBookingVO); //commented by rohini for CR Points
                        }

                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");




                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else
            {
                #region rohini saprated all sp
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderBookingList2");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                    //added by rohini 29.2.2016
                    if (BizActionObj.IsDispatchedClinic == true)
                    {
                        dbServer.AddInParameter(command, "IsDispatchedClinic", DbType.Int64, BizActionObj.IsDispatchedClinic);
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "IsDispatchedClinic", DbType.Int64, 0);

                    }
                    //added by rohini for CR Points
                    //if (BizActionObj.IsFromResultEntry == true)
                    //{
                    //    dbServer.AddInParameter(command, "IsFromResultEntry", DbType.Boolean, BizActionObj.IsFromResultEntry);
                    //}
                    //else
                    //{
                    //    dbServer.AddInParameter(command, "IsFromResultEntry", DbType.Boolean, 0);
                    //}

                    dbServer.AddInParameter(command, "CatagoryID", DbType.Int64, BizActionObj.CatagoryID);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                    dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                    dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                    dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                    dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                    dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.IsDelivered);

                    if (BizActionObj.MRNO != null && BizActionObj.MRNO.Length != 0)
                    {
                        dbServer.AddInParameter(command, "MRNo", DbType.String, "%" + BizActionObj.MRNO + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "MRNo", DbType.String, null);

                    //added by rohini dated 11.2.16

                    if (BizActionObj.BillNo != null && BizActionObj.BillNo.Length != 0)
                    {
                        dbServer.AddInParameter(command, "BillNo", DbType.String, "%" + BizActionObj.BillNo + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "BillNo", DbType.String, null);
                    //

                    if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "FirstName", DbType.String, "%" + BizActionObj.FirstName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                    if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    {
                        dbServer.AddInParameter(command, "LastName", DbType.String, "%" + BizActionObj.LastName + "%");
                    }
                    else
                        dbServer.AddInParameter(command, "LastName", DbType.String, null);


                    dbServer.AddInParameter(command, "PatientType", DbType.Int64, BizActionObj.PatientType);  // Set for Patient Type - 1 : OPD 2 : IPD

                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    //  dbServer.AddOutParameter(command, "TotalRows1", DbType.Int32, int.MaxValue);


                    DbDataReader reader;


                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingList == null)
                            BizActionObj.OrderBookingList = new List<clsPathOrderBookingVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingVO objOrderBookingVO = new clsPathOrderBookingVO();

                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.OrderDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            objOrderBookingVO.TestProfile = ((long?)DALHelper.HandleDBNull(reader["TestType"]));
                            objOrderBookingVO.OrderNo = (string)DALHelper.HandleDBNull(reader["OrderNo"]);
                            objOrderBookingVO.BillNo = (string)DALHelper.HandleDBNull(reader["BillNo"]);
                            objOrderBookingVO.SampleType = (Boolean)DALHelper.HandleDBNull(reader["SampleType"]);
                            objOrderBookingVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                            objOrderBookingVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                            objOrderBookingVO.PatientUnitID = (long)DALHelper.HandleIntegerNull(reader["PatientUnitID"]);
                            objOrderBookingVO.Prefix = (string)DALHelper.HandleDBNull(reader["Pre"]); //by rohini
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                            objOrderBookingVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                            objOrderBookingVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.TotalAmount = (double)DALHelper.HandleDBNull(reader["TotalAmount"]);

                            #region Newly added
                            objOrderBookingVO.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                            objOrderBookingVO.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                            #endregion

                            objOrderBookingVO.PaidAmount = (double)DALHelper.HandleDBNull(reader["PaidAmount"]);
                            objOrderBookingVO.Balance = (double)DALHelper.HandleDBNull(reader["BalanceAmount"]);
                            objOrderBookingVO.IsResultEntry = (bool)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.ReferredBy = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                            objOrderBookingVO.GenderID = (long)DALHelper.HandleIntegerNull(reader["GenderID"]);
                            objOrderBookingVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                            objOrderBookingVO.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorEmailID"]));
                            objOrderBookingVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));

                            objOrderBookingVO.RegistrationTime = (DateTime?)DALHelper.HandleDBNull(reader["RegistrationTime"]);
                            objOrderBookingVO.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmailId"]));

                            objOrderBookingVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objOrderBookingVO.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIPDPatient"]));  //Set to identify whether it is IPD Patient or OPD

                            #region Newly added
                            objOrderBookingVO.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixID"]));
                            objOrderBookingVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            objOrderBookingVO.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                            #endregion

                            //objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillId"]));
                            objOrderBookingVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CID"])); // by rohini
                            objOrderBookingVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["IsIPD"]));  // by rohini



                            BizActionObj.OrderBookingList.Add(objOrderBookingVO);

                        }

                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                    //BizActionObj.TotalRows1 = (int)dbServer.GetParameterValue(command, "TotalRows1");

                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            return valueObject;
        }

        public override IValueObject GetPathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderBookingDetailListBizActionVO BizActionObj = valueObject as clsGetPathOrderBookingDetailListBizActionVO;
            //temp ResultEntry=ResultEntry1
            if (BizActionObj.IsFrom == "ResultEntry")
            {
                #region
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForResultEntry");

                    dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);                    
                    dbServer.AddInParameter(command, "TestCategoryID", DbType.Int64, BizActionObj.TestCategoryID);                    
                    dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    if (BizActionObj.CheckExtraCriteria == true)
                    {
                        #region Newly Added IN Parameters
                        dbServer.AddInParameter(command, "SearchFromCollection", DbType.Boolean, BizActionObj.IsFromCollection);
                        dbServer.AddInParameter(command, "SearchFromReceive", DbType.Boolean, BizActionObj.IsFromReceive);
                        dbServer.AddInParameter(command, "SearchFromDispatch", DbType.Boolean, BizActionObj.IsFromDispatch);
                        dbServer.AddInParameter(command, "SeacrhFromAcceptReject", DbType.Boolean, BizActionObj.IsFromAcceptRejct);
                        dbServer.AddInParameter(command, "SearchFromResult", DbType.Boolean, BizActionObj.IsFromResult);
                        dbServer.AddInParameter(command, "SearchFromAutoriation", DbType.Boolean, BizActionObj.IsFromAuthorization);
                        dbServer.AddInParameter(command, "SearchFromUpload", DbType.Boolean, BizActionObj.IsFromUpload);
                        #endregion

                        dbServer.AddInParameter(command, "CheckExtraCriteria", DbType.Boolean, BizActionObj.CheckExtraCriteria);
                        dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                        dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                        dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                        dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                        dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);
                       
                        #region added by rohini
                        dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered);
                        dbServer.AddInParameter(command, "IsDelivered1", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered1);
                        dbServer.AddInParameter(command, "IsDeliverdthroughEmail", DbType.Boolean, BizActionObj.OrderDetail.IsDeliverdthroughEmail);
                        dbServer.AddInParameter(command, "IsDirectDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDirectDelivered);
                        dbServer.AddInParameter(command, "IsResultEntry", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry);
                        dbServer.AddInParameter(command, "IsResultEntry1", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry1);
                        dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, BizActionObj.OrderDetail.IsBilled);
                        dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, BizActionObj.OrderDetail.IsSampleCollected);
                        dbServer.AddInParameter(command, "IsSampleDispatched", DbType.Boolean, BizActionObj.OrderDetail.IsSampleDispatch);
                        dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, BizActionObj.OrderDetail.IsSampleReceive);
                        dbServer.AddInParameter(command, "IsAccepted", DbType.Boolean, true);
                        dbServer.AddInParameter(command, "IsRejected", DbType.Boolean, BizActionObj.OrderDetail.IsRejected);
                        dbServer.AddInParameter(command, "IsSubOptimal", DbType.Boolean, BizActionObj.OrderDetail.IsSubOptimal);
                        dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, BizActionObj.OrderDetail.IsOutSourced);
                        dbServer.AddInParameter(command, "SampleCollectedBy", DbType.String, BizActionObj.OrderDetail.SampleCollectedBy);
                        dbServer.AddInParameter(command, "DispatchBy", DbType.String, BizActionObj.OrderDetail.DispatchBy);
                        dbServer.AddInParameter(command, "AcceptedOrRejectedByName", DbType.String, BizActionObj.OrderDetail.AcceptedOrRejectedByName);
                        dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, BizActionObj.OrderDetail.SampleReceiveBy);

                        dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.OrderDetail.AgencyID);
                        dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.OrderDetail.SampleNo);
                        dbServer.AddInParameter(command, "IsExternalPatient", DbType.Boolean, BizActionObj.OrderDetail.IsExternalPatient);
                        dbServer.AddInParameter(command, "CatagoryID", DbType.Int64, BizActionObj.OrderDetail.CategoryID);
                        dbServer.AddInParameter(command, "ResultEntryUserID", DbType.Int64, BizActionObj.OrderDetail.ResultEntryUserID);
                        //Added BY Bhushanp 19012017 For Add Date Filter
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                        dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                        //by rohinee
                       // dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.IsFromReceive);
                        #endregion
                    }
                    DbDataReader reader;


                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingDetailList == null)
                            BizActionObj.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();

                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.OrderBookingID = (long)DALHelper.HandleDBNull(reader["OrderID"]);
                            objOrderBookingVO.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.SampleCollected = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollected"]);
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.IsCompleted = (bool)DALHelper.HandleDBNull(reader["IsCompleted"]);
                            objOrderBookingVO.IsDelivered = (bool)DALHelper.HandleDBNull(reader["IsDelivered"]);
                            objOrderBookingVO.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                            objOrderBookingVO.TestCode = (String)DALHelper.HandleDBNull(reader["TestCode"]);
                            objOrderBookingVO.TestName = (string)DALHelper.HandleDBNull(reader["TestName"]);
                            objOrderBookingVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                            objOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objOrderBookingVO.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                            objOrderBookingVO.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);                 
                            objOrderBookingVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                            objOrderBookingVO.IsOutSourced = (bool)DALHelper.HandleDBNull(reader["IsOutSourced"]);
                            objOrderBookingVO.Quantity = (double?)DALHelper.HandleDBNull(reader["Quantity"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.IsResultEntry = (Boolean)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.IsFinalized = (Boolean)DALHelper.HandleDBNull(reader["IsFinalized"]);
                            objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);
                            objOrderBookingVO.IsDirectDelivered = (bool)DALHelper.HandleBoolDBNull(reader["IsDirectDelivered"]);
                            objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);
                            objOrderBookingVO.IsDeliverdthroughEmail = (bool)DALHelper.HandleBoolDBNull(reader["IsDeliverdthroughEmail"]);
                            #region Newly Added
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.SampleCollectedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]);
                            objOrderBookingVO.IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                            objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);
                            objOrderBookingVO.IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));
                            objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);
                            objOrderBookingVO.SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader["SampleAcceptRejectStatus"]));
                            objOrderBookingVO.SampleAcceptanceDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                            objOrderBookingVO.SampleRejectionDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);
                            objOrderBookingVO.SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"]));
                            objOrderBookingVO.FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FirstLevel"]));
                            objOrderBookingVO.SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SecondLevel"]));
                            objOrderBookingVO.ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevel"]));
                            objOrderBookingVO.IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCheckedResults"]));
                            objOrderBookingVO.ReportTemplate = (Boolean)DALHelper.HandleDBNull(reader["ReportTemplate"]);
                            objOrderBookingVO.RefDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefDoctorID"]));
                            if (objOrderBookingVO.ReportTemplate == false)
                            {
                                objOrderBookingVO.ReportType = "Parameter";
                            }
                            else
                            {
                                objOrderBookingVO.ReportType = "Template";
                            }
                            objOrderBookingVO.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader["AppendSampleNo"]));
                            objOrderBookingVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                            objOrderBookingVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            objOrderBookingVO.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            objOrderBookingVO.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevelCheckResult"]));
                            objOrderBookingVO.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValueMessage"]));
                            objOrderBookingVO.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValForSecLevel"]));
                            objOrderBookingVO.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"]));
                            objOrderBookingVO.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));                           
                            objOrderBookingVO.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                            objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleGenerated"]));
                            objOrderBookingVO.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsServiceRefunded"]));
                            //Added By Bhushan New Date Coloumn 13/01/2017
                            objOrderBookingVO.ADateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                            BizActionObj.OrderBookingDetailList.Add(objOrderBookingVO);

                        }

                    }
                    reader.NextResult();

                    if (BizActionObj.objOutsourceOrderBookingDetail == null)
                        BizActionObj.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOutsourcingOrderBookingVO = new clsPathOrderBookingDetailVO();
                        objOutsourcingOrderBookingVO.OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objOutsourcingOrderBookingVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objOutsourcingOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objOutsourcingOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        objOutsourcingOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));

                        BizActionObj.objOutsourceOrderBookingDetail.Add(objOutsourcingOrderBookingVO);
                    }

                            #endregion

                    reader.Close();


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                  #endregion
            }
            else if (BizActionObj.IsFrom == "Authorization")
            {
                #region
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForAuthorization");

                    dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "TestCategoryID", DbType.Int64, BizActionObj.TestCategoryID);
                    dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    if (BizActionObj.CheckExtraCriteria == true)
                    {
                        #region Newly Added IN Parameters
                        dbServer.AddInParameter(command, "SearchFromCollection", DbType.Boolean, BizActionObj.IsFromCollection);
                        dbServer.AddInParameter(command, "SearchFromReceive", DbType.Boolean, BizActionObj.IsFromReceive);
                        dbServer.AddInParameter(command, "SearchFromDispatch", DbType.Boolean, BizActionObj.IsFromDispatch);
                        dbServer.AddInParameter(command, "SeacrhFromAcceptReject", DbType.Boolean, BizActionObj.IsFromAcceptRejct);
                        dbServer.AddInParameter(command, "SearchFromResult", DbType.Boolean, BizActionObj.IsFromResult);
                        dbServer.AddInParameter(command, "SearchFromAutoriation", DbType.Boolean, BizActionObj.IsFromAuthorization);
                        dbServer.AddInParameter(command, "SearchFromUpload", DbType.Boolean, BizActionObj.IsFromUpload);
                        #endregion

                        dbServer.AddInParameter(command, "CheckExtraCriteria", DbType.Boolean, BizActionObj.CheckExtraCriteria);
                        dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                        dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                        dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                        dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                        dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);

                        #region added by rohini
                        dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered);
                        dbServer.AddInParameter(command, "IsDelivered1", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered1);
                        dbServer.AddInParameter(command, "IsDeliverdthroughEmail", DbType.Boolean, BizActionObj.OrderDetail.IsDeliverdthroughEmail);
                        dbServer.AddInParameter(command, "IsDirectDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDirectDelivered);
                        dbServer.AddInParameter(command, "IsResultEntry", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry);
                        dbServer.AddInParameter(command, "IsResultEntry1", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry1);
                        dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, BizActionObj.OrderDetail.IsBilled);
                        dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, BizActionObj.OrderDetail.IsSampleCollected);
                        dbServer.AddInParameter(command, "IsSampleDispatched", DbType.Boolean, BizActionObj.OrderDetail.IsSampleDispatch);
                        dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, BizActionObj.OrderDetail.IsSampleReceive);
                        dbServer.AddInParameter(command, "IsAccepted", DbType.Boolean, true);
                        dbServer.AddInParameter(command, "IsRejected", DbType.Boolean, BizActionObj.OrderDetail.IsRejected);
                        dbServer.AddInParameter(command, "IsSubOptimal", DbType.Boolean, BizActionObj.OrderDetail.IsSubOptimal);
                        dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, BizActionObj.OrderDetail.IsOutSourced);
                        dbServer.AddInParameter(command, "SampleCollectedBy", DbType.String, BizActionObj.OrderDetail.SampleCollectedBy);
                        dbServer.AddInParameter(command, "DispatchBy", DbType.String, BizActionObj.OrderDetail.DispatchBy);
                        dbServer.AddInParameter(command, "AcceptedOrRejectedByName", DbType.String, BizActionObj.OrderDetail.AcceptedOrRejectedByName);
                        dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, BizActionObj.OrderDetail.SampleReceiveBy);

                        dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.OrderDetail.AgencyID);
                        dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.OrderDetail.SampleNo);
                        dbServer.AddInParameter(command, "IsExternalPatient", DbType.Boolean, BizActionObj.OrderDetail.IsExternalPatient);
                        //dbServer.AddInParameter(command, "CatagoryID", DbType.Int64, BizActionObj.OrderDetail.CategoryID);
                        dbServer.AddInParameter(command, "AuthUserID", DbType.Int64, BizActionObj.OrderDetail.ResultEntryUserID);
                        //Added BY Bhushanp 19012017 For Add Date Filter
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                        dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                        //by rohinee
                        // dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.IsFromReceive);
                        #endregion
                    }
                    DbDataReader reader;


                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingDetailList == null)
                            BizActionObj.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();

                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.OrderBookingID = (long)DALHelper.HandleDBNull(reader["OrderID"]);
                            objOrderBookingVO.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.SampleCollected = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollected"]);
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.IsCompleted = (bool)DALHelper.HandleDBNull(reader["IsCompleted"]);
                            objOrderBookingVO.IsDelivered = (bool)DALHelper.HandleDBNull(reader["IsDelivered"]);
                            objOrderBookingVO.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                            objOrderBookingVO.TestCode = (String)DALHelper.HandleDBNull(reader["TestCode"]);
                            objOrderBookingVO.TestName = (string)DALHelper.HandleDBNull(reader["TestName"]);
                            objOrderBookingVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                            objOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objOrderBookingVO.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                            objOrderBookingVO.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                            objOrderBookingVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                            objOrderBookingVO.IsOutSourced = (bool)DALHelper.HandleDBNull(reader["IsOutSourced"]);
                            objOrderBookingVO.Quantity = (double?)DALHelper.HandleDBNull(reader["Quantity"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.IsResultEntry = (Boolean)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.IsFinalized = (Boolean)DALHelper.HandleDBNull(reader["IsFinalized"]);
                            objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);
                            objOrderBookingVO.IsDirectDelivered = (bool)DALHelper.HandleBoolDBNull(reader["IsDirectDelivered"]);
                            objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);
                            objOrderBookingVO.IsDeliverdthroughEmail = (bool)DALHelper.HandleBoolDBNull(reader["IsDeliverdthroughEmail"]);
                            #region Newly Added
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.SampleCollectedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]);
                            objOrderBookingVO.IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                            objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);
                            objOrderBookingVO.IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));
                            objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);
                            objOrderBookingVO.SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader["SampleAcceptRejectStatus"]));
                            objOrderBookingVO.SampleAcceptanceDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                            objOrderBookingVO.SampleRejectionDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);
                            objOrderBookingVO.SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"]));
                            objOrderBookingVO.FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FirstLevel"]));
                            objOrderBookingVO.SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SecondLevel"]));
                            objOrderBookingVO.ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevel"]));
                            objOrderBookingVO.IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCheckedResults"]));
                            objOrderBookingVO.ReportTemplate = (Boolean)DALHelper.HandleDBNull(reader["ReportTemplate"]);
                            objOrderBookingVO.RefDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefDoctorID"]));
                            if (objOrderBookingVO.ReportTemplate == false)
                            {
                                objOrderBookingVO.ReportType = "Parameter";
                            }
                            else
                            {
                                objOrderBookingVO.ReportType = "Template";
                            }
                            objOrderBookingVO.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader["AppendSampleNo"]));
                            objOrderBookingVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                            objOrderBookingVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            objOrderBookingVO.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            objOrderBookingVO.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevelCheckResult"]));
                            objOrderBookingVO.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValueMessage"]));
                            objOrderBookingVO.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValForSecLevel"]));
                            objOrderBookingVO.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"]));
                            objOrderBookingVO.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));
                            objOrderBookingVO.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                            objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleGenerated"]));
                            objOrderBookingVO.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsServiceRefunded"]));
                            //Added By Bhushan New Date Coloumn 13/01/2017
                            objOrderBookingVO.ADateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                            BizActionObj.OrderBookingDetailList.Add(objOrderBookingVO);

                        }

                    }
                    reader.NextResult();

                    if (BizActionObj.objOutsourceOrderBookingDetail == null)
                        BizActionObj.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOutsourcingOrderBookingVO = new clsPathOrderBookingDetailVO();
                        objOutsourcingOrderBookingVO.OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objOutsourcingOrderBookingVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objOutsourcingOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objOutsourcingOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        objOutsourcingOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));

                        BizActionObj.objOutsourceOrderBookingDetail.Add(objOutsourcingOrderBookingVO);
                    }

                            #endregion

                    reader.Close();


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else if (BizActionObj.IsFrom == "Delivery")
            {
                #region
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForDelivery");

                    dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "TestCategoryID", DbType.Int64, BizActionObj.TestCategoryID);
                    //dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    if (BizActionObj.CheckExtraCriteria == true)
                    {                        
                        //dbServer.AddInParameter(command, "SearchFromCollection", DbType.Boolean, BizActionObj.IsFromCollection);
                        //dbServer.AddInParameter(command, "SearchFromReceive", DbType.Boolean, BizActionObj.IsFromReceive);
                        //dbServer.AddInParameter(command, "SearchFromDispatch", DbType.Boolean, BizActionObj.IsFromDispatch);
                        //dbServer.AddInParameter(command, "SeacrhFromAcceptReject", DbType.Boolean, BizActionObj.IsFromAcceptRejct);
                        //dbServer.AddInParameter(command, "SearchFromResult", DbType.Boolean, BizActionObj.IsFromResult);
                        //dbServer.AddInParameter(command, "SearchFromAutoriation", DbType.Boolean, BizActionObj.IsFromAuthorization);
                        //dbServer.AddInParameter(command, "SearchFromUpload", DbType.Boolean, BizActionObj.IsFromUpload);                      

                        dbServer.AddInParameter(command, "CheckExtraCriteria", DbType.Boolean, BizActionObj.CheckExtraCriteria);
                        dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                        dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                        dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                        dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                        dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);

                        #region added by rohini                       
                        dbServer.AddInParameter(command, "IsDeliverdthroughEmail", DbType.Boolean, BizActionObj.OrderDetail.IsDeliverdthroughEmail);
                        dbServer.AddInParameter(command, "IsDirectDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDirectDelivered);                      
                        dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.OrderDetail.AgencyID);
                        dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.OrderDetail.SampleNo);
                        dbServer.AddInParameter(command, "IsExternalPatient", DbType.Boolean, BizActionObj.OrderDetail.IsExternalPatient);                     
                        dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.OrderDetail.UserID);                     
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                        dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                        //dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered);
                        //dbServer.AddInParameter(command, "IsDelivered1", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered1);
                        //dbServer.AddInParameter(command, "IsResultEntry", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry);
                        //dbServer.AddInParameter(command, "IsResultEntry1", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry1);
                        //dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, BizActionObj.OrderDetail.IsBilled);
                        //dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, BizActionObj.OrderDetail.IsSampleCollected);
                        //dbServer.AddInParameter(command, "IsSampleDispatched", DbType.Boolean, BizActionObj.OrderDetail.IsSampleDispatch);
                        //dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, BizActionObj.OrderDetail.IsSampleReceive);
                        //dbServer.AddInParameter(command, "IsAccepted", DbType.Boolean, true);
                        //dbServer.AddInParameter(command, "IsRejected", DbType.Boolean, BizActionObj.OrderDetail.IsRejected);
                        //dbServer.AddInParameter(command, "IsSubOptimal", DbType.Boolean, BizActionObj.OrderDetail.IsSubOptimal);
                        //dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, BizActionObj.OrderDetail.IsOutSourced);
                        //dbServer.AddInParameter(command, "SampleCollectedBy", DbType.String, BizActionObj.OrderDetail.SampleCollectedBy);
                        //dbServer.AddInParameter(command, "DispatchBy", DbType.String, BizActionObj.OrderDetail.DispatchBy);
                        //dbServer.AddInParameter(command, "AcceptedOrRejectedByName", DbType.String, BizActionObj.OrderDetail.AcceptedOrRejectedByName);
                        //dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, BizActionObj.OrderDetail.SampleReceiveBy);

                     
                        #endregion
                    }
                    DbDataReader reader;


                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingDetailList == null)
                            BizActionObj.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();

                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.OrderBookingID = (long)DALHelper.HandleDBNull(reader["OrderID"]);
                            objOrderBookingVO.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.SampleCollected = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollected"]);
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.IsCompleted = (bool)DALHelper.HandleDBNull(reader["IsCompleted"]);
                            objOrderBookingVO.IsDelivered = (bool)DALHelper.HandleDBNull(reader["IsDelivered"]);
                            objOrderBookingVO.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                            objOrderBookingVO.TestCode = (String)DALHelper.HandleDBNull(reader["TestCode"]);
                            objOrderBookingVO.TestName = (string)DALHelper.HandleDBNull(reader["TestName"]);
                            objOrderBookingVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                            objOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objOrderBookingVO.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                            objOrderBookingVO.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                            objOrderBookingVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                            objOrderBookingVO.IsOutSourced = (bool)DALHelper.HandleDBNull(reader["IsOutSourced"]);
                            objOrderBookingVO.Quantity = (double?)DALHelper.HandleDBNull(reader["Quantity"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.IsResultEntry = (Boolean)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.IsFinalized = (Boolean)DALHelper.HandleDBNull(reader["IsFinalized"]);
                            objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);
                            objOrderBookingVO.IsDirectDelivered = (bool)DALHelper.HandleBoolDBNull(reader["IsDirectDelivered"]);
                            objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);
                            objOrderBookingVO.IsDeliverdthroughEmail = (bool)DALHelper.HandleBoolDBNull(reader["IsDeliverdthroughEmail"]);
                            #region Newly Added
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.SampleCollectedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]);
                            objOrderBookingVO.IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                            objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);
                            objOrderBookingVO.IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));
                            objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);
                            objOrderBookingVO.SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader["SampleAcceptRejectStatus"]));
                            objOrderBookingVO.SampleAcceptanceDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                            objOrderBookingVO.SampleRejectionDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);
                            objOrderBookingVO.SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"]));
                            objOrderBookingVO.FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FirstLevel"]));
                            objOrderBookingVO.SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SecondLevel"]));
                            objOrderBookingVO.ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevel"]));
                            objOrderBookingVO.IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCheckedResults"]));
                            objOrderBookingVO.ReportTemplate = (Boolean)DALHelper.HandleDBNull(reader["ReportTemplate"]);
                            objOrderBookingVO.RefDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefDoctorID"]));
                            if (objOrderBookingVO.ReportTemplate == false)
                            {
                                objOrderBookingVO.ReportType = "Parameter";
                            }
                            else
                            {
                                objOrderBookingVO.ReportType = "Template";
                            }
                            objOrderBookingVO.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader["AppendSampleNo"]));
                            objOrderBookingVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                            objOrderBookingVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            objOrderBookingVO.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            objOrderBookingVO.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevelCheckResult"]));
                            objOrderBookingVO.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValueMessage"]));
                            objOrderBookingVO.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValForSecLevel"]));
                            objOrderBookingVO.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"]));
                            objOrderBookingVO.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));
                            objOrderBookingVO.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                            objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleGenerated"]));
                            objOrderBookingVO.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsServiceRefunded"]));
                            //Added By Bhushan New Date Coloumn 13/01/2017
                            objOrderBookingVO.ADateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                            BizActionObj.OrderBookingDetailList.Add(objOrderBookingVO);

                        }

                    }
                    reader.NextResult();

                    if (BizActionObj.objOutsourceOrderBookingDetail == null)
                        BizActionObj.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOutsourcingOrderBookingVO = new clsPathOrderBookingDetailVO();
                        objOutsourcingOrderBookingVO.OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objOutsourcingOrderBookingVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objOutsourcingOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objOutsourcingOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        objOutsourcingOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));

                        BizActionObj.objOutsourceOrderBookingDetail.Add(objOutsourcingOrderBookingVO);
                    }

                            #endregion

                    reader.Close();


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else if (BizActionObj.IsFrom == "UploadReport")
            {
                #region
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForUpload");

                    dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "TestCategoryID", DbType.Int64, BizActionObj.TestCategoryID);
                    //dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    if (BizActionObj.CheckExtraCriteria == true)
                    {
                        //dbServer.AddInParameter(command, "SearchFromCollection", DbType.Boolean, BizActionObj.IsFromCollection);
                        //dbServer.AddInParameter(command, "SearchFromReceive", DbType.Boolean, BizActionObj.IsFromReceive);
                        //dbServer.AddInParameter(command, "SearchFromDispatch", DbType.Boolean, BizActionObj.IsFromDispatch);
                        //dbServer.AddInParameter(command, "SeacrhFromAcceptReject", DbType.Boolean, BizActionObj.IsFromAcceptRejct);
                        //dbServer.AddInParameter(command, "SearchFromResult", DbType.Boolean, BizActionObj.IsFromResult);
                        //dbServer.AddInParameter(command, "SearchFromAutoriation", DbType.Boolean, BizActionObj.IsFromAuthorization);
                        //dbServer.AddInParameter(command, "SearchFromUpload", DbType.Boolean, BizActionObj.IsFromUpload);                      

                        dbServer.AddInParameter(command, "CheckExtraCriteria", DbType.Boolean, BizActionObj.CheckExtraCriteria);
                        dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                        dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                        dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                       // dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                        dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);

                        #region added by rohini
                        dbServer.AddInParameter(command, "IsDeliverdthroughEmail", DbType.Boolean, BizActionObj.OrderDetail.IsDeliverdthroughEmail);
                        dbServer.AddInParameter(command, "IsDirectDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDirectDelivered);
                        dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);
                        //dbServer.AddInParameter(command, "StatusID", DbType.Int64, BizActionObj.StatusID);
                     
 
                        dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.OrderDetail.SampleNo);
                        dbServer.AddInParameter(command, "IsExternalPatient", DbType.Boolean, BizActionObj.OrderDetail.IsExternalPatient);
                        dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.OrderDetail.UserID);
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                        dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                        //dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered);
                        //dbServer.AddInParameter(command, "IsDelivered1", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered1);
                        //dbServer.AddInParameter(command, "IsResultEntry", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry);
                        //dbServer.AddInParameter(command, "IsResultEntry1", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry1);
                        //dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, BizActionObj.OrderDetail.IsBilled);
                        //dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, BizActionObj.OrderDetail.IsSampleCollected);
                        //dbServer.AddInParameter(command, "IsSampleDispatched", DbType.Boolean, BizActionObj.OrderDetail.IsSampleDispatch);
                        //dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, BizActionObj.OrderDetail.IsSampleReceive);
                        //dbServer.AddInParameter(command, "IsAccepted", DbType.Boolean, true);
                        //dbServer.AddInParameter(command, "IsRejected", DbType.Boolean, BizActionObj.OrderDetail.IsRejected);
                        //dbServer.AddInParameter(command, "IsSubOptimal", DbType.Boolean, BizActionObj.OrderDetail.IsSubOptimal);
                        //dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, BizActionObj.OrderDetail.IsOutSourced);
                        //dbServer.AddInParameter(command, "SampleCollectedBy", DbType.String, BizActionObj.OrderDetail.SampleCollectedBy);
                        //dbServer.AddInParameter(command, "DispatchBy", DbType.String, BizActionObj.OrderDetail.DispatchBy);
                        //dbServer.AddInParameter(command, "AcceptedOrRejectedByName", DbType.String, BizActionObj.OrderDetail.AcceptedOrRejectedByName);
                        //dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, BizActionObj.OrderDetail.SampleReceiveBy);


                        #endregion
                    }
                    DbDataReader reader;


                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingDetailList == null)
                            BizActionObj.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();

                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.OrderBookingID = (long)DALHelper.HandleDBNull(reader["OrderID"]);
                            objOrderBookingVO.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.SampleCollected = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollected"]);
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.IsCompleted = (bool)DALHelper.HandleDBNull(reader["IsCompleted"]);
                            objOrderBookingVO.IsDelivered = (bool)DALHelper.HandleDBNull(reader["IsDelivered"]);
                            objOrderBookingVO.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                            objOrderBookingVO.TestCode = (String)DALHelper.HandleDBNull(reader["TestCode"]);
                            objOrderBookingVO.TestName = (string)DALHelper.HandleDBNull(reader["TestName"]);
                            objOrderBookingVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                            objOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objOrderBookingVO.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                            objOrderBookingVO.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                            objOrderBookingVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                            objOrderBookingVO.IsOutSourced = (bool)DALHelper.HandleDBNull(reader["IsOutSourced"]);
                            objOrderBookingVO.Quantity = (double?)DALHelper.HandleDBNull(reader["Quantity"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.IsResultEntry = (Boolean)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.IsFinalized = (Boolean)DALHelper.HandleDBNull(reader["IsFinalized"]);
                            objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);
                            objOrderBookingVO.IsDirectDelivered = (bool)DALHelper.HandleBoolDBNull(reader["IsDirectDelivered"]);
                            objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);
                            objOrderBookingVO.IsDeliverdthroughEmail = (bool)DALHelper.HandleBoolDBNull(reader["IsDeliverdthroughEmail"]);
                            #region Newly Added
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.SampleCollectedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]);
                            objOrderBookingVO.IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                            objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);
                            objOrderBookingVO.IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));
                            objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);
                            objOrderBookingVO.SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader["SampleAcceptRejectStatus"]));
                            objOrderBookingVO.SampleAcceptanceDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                            objOrderBookingVO.SampleRejectionDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);
                            objOrderBookingVO.SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"]));
                            objOrderBookingVO.FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FirstLevel"]));
                            objOrderBookingVO.SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SecondLevel"]));
                            objOrderBookingVO.ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevel"]));
                            objOrderBookingVO.IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCheckedResults"]));
                            objOrderBookingVO.ReportTemplate = (Boolean)DALHelper.HandleDBNull(reader["ReportTemplate"]);
                            objOrderBookingVO.RefDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefDoctorID"]));
                            if (objOrderBookingVO.ReportTemplate == false)
                            {
                                objOrderBookingVO.ReportType = "Parameter";
                            }
                            else
                            {
                                objOrderBookingVO.ReportType = "Template";
                            }
                            objOrderBookingVO.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader["AppendSampleNo"]));
                            objOrderBookingVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                            objOrderBookingVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            objOrderBookingVO.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            objOrderBookingVO.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevelCheckResult"]));
                            objOrderBookingVO.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValueMessage"]));
                            objOrderBookingVO.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValForSecLevel"]));
                            objOrderBookingVO.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"]));
                            objOrderBookingVO.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));
                            objOrderBookingVO.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                            objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleGenerated"]));
                            objOrderBookingVO.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsServiceRefunded"]));
                            //Added By Bhushan New Date Coloumn 13/01/2017
                            objOrderBookingVO.ADateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                            objOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtAgencyID"]));  //user do not detete

                            BizActionObj.OrderBookingDetailList.Add(objOrderBookingVO);

                        }

                    }
                    reader.NextResult();

                    if (BizActionObj.objOutsourceOrderBookingDetail == null)
                        BizActionObj.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOutsourcingOrderBookingVO = new clsPathOrderBookingDetailVO();
                        objOutsourcingOrderBookingVO.OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objOutsourcingOrderBookingVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objOutsourcingOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objOutsourcingOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        objOutsourcingOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));

                        BizActionObj.objOutsourceOrderBookingDetail.Add(objOutsourcingOrderBookingVO);
                    }

                            #endregion

                    reader.Close();


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else if (BizActionObj.IsFrom == "SampleCollection")
            {
                #region
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForCollection");

                    dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                   // dbServer.AddInParameter(command, "TestCategoryID", DbType.Int64, BizActionObj.TestCategoryID);                 
                    if (BizActionObj.CheckExtraCriteria == true)
                    {   
                        dbServer.AddInParameter(command, "CheckExtraCriteria", DbType.Boolean, BizActionObj.CheckExtraCriteria);
                        dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                        dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                        dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                        dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                        dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);
                        #region added by rohini                   
                        dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.AgencyID);                     
                        dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.OrderDetail.SampleNo);
                        dbServer.AddInParameter(command, "IsExternalPatient", DbType.Boolean, BizActionObj.OrderDetail.IsExternalPatient);
                        dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.OrderDetail.UserID);
                        dbServer.AddInParameter(command, "StatusID", DbType.Int64, BizActionObj.StatusID);
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                        dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                   
                        
                        #endregion
                    }
                    DbDataReader reader;


                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingDetailList == null)
                            BizActionObj.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();

                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.OrderBookingID = (long)DALHelper.HandleDBNull(reader["OrderID"]);
                            objOrderBookingVO.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.SampleCollected = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollected"]);
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.IsCompleted = (bool)DALHelper.HandleDBNull(reader["IsCompleted"]);
                            objOrderBookingVO.IsDelivered = (bool)DALHelper.HandleDBNull(reader["IsDelivered"]);
                            objOrderBookingVO.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                            objOrderBookingVO.TestCode = (String)DALHelper.HandleDBNull(reader["TestCode"]);
                            objOrderBookingVO.TestName = (string)DALHelper.HandleDBNull(reader["TestName"]);
                            objOrderBookingVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                            objOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objOrderBookingVO.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                            objOrderBookingVO.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                            objOrderBookingVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                            objOrderBookingVO.IsOutSourced = (bool)DALHelper.HandleDBNull(reader["IsOutSourced"]);
                            objOrderBookingVO.Quantity = (double?)DALHelper.HandleDBNull(reader["Quantity"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.IsResultEntry = (Boolean)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.IsFinalized = (Boolean)DALHelper.HandleDBNull(reader["IsFinalized"]);
                            objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);
                            objOrderBookingVO.IsDirectDelivered = (bool)DALHelper.HandleBoolDBNull(reader["IsDirectDelivered"]);
                            objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);
                            objOrderBookingVO.IsDeliverdthroughEmail = (bool)DALHelper.HandleBoolDBNull(reader["IsDeliverdthroughEmail"]);
                            #region Newly Added
                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.SampleCollectedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]);
                            objOrderBookingVO.IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                            objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);
                            objOrderBookingVO.IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));
                            objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);
                            objOrderBookingVO.SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader["SampleAcceptRejectStatus"]));
                            objOrderBookingVO.SampleAcceptanceDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                            objOrderBookingVO.SampleRejectionDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);
                            objOrderBookingVO.SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"]));
                            objOrderBookingVO.FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FirstLevel"]));
                            objOrderBookingVO.SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SecondLevel"]));
                            objOrderBookingVO.ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevel"]));
                            objOrderBookingVO.IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCheckedResults"]));
                            objOrderBookingVO.ReportTemplate = (Boolean)DALHelper.HandleDBNull(reader["ReportTemplate"]);
                            objOrderBookingVO.RefDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefDoctorID"]));
                            if (objOrderBookingVO.ReportTemplate == false)
                            {
                                objOrderBookingVO.ReportType = "Parameter";
                            }
                            else
                            {
                                objOrderBookingVO.ReportType = "Template";
                            }
                            objOrderBookingVO.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader["AppendSampleNo"]));
                            objOrderBookingVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                            objOrderBookingVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            objOrderBookingVO.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            objOrderBookingVO.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevelCheckResult"]));
                            objOrderBookingVO.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValueMessage"]));
                            objOrderBookingVO.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValForSecLevel"]));
                            objOrderBookingVO.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"]));
                            objOrderBookingVO.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));
                            objOrderBookingVO.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                            objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleGenerated"]));
                            objOrderBookingVO.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsServiceRefunded"]));
                            //Added By Bhushan New Date Coloumn 13/01/2017
                            objOrderBookingVO.ADateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                            BizActionObj.OrderBookingDetailList.Add(objOrderBookingVO);

                        }

                    }
                    reader.NextResult();

                    if (BizActionObj.objOutsourceOrderBookingDetail == null)
                        BizActionObj.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOutsourcingOrderBookingVO = new clsPathOrderBookingDetailVO();
                        objOutsourcingOrderBookingVO.OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objOutsourcingOrderBookingVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objOutsourcingOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objOutsourcingOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        objOutsourcingOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));

                        BizActionObj.objOutsourceOrderBookingDetail.Add(objOutsourcingOrderBookingVO);
                    }

                            #endregion

                    reader.Close();


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
                #endregion
            }
            else
            {
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderBookingDetailList");

                    dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    // Added By Anumani on 22/03/2016
                    dbServer.AddInParameter(command, "TestCategoryID", DbType.Int64, BizActionObj.TestCategoryID);
                    //
                    dbServer.AddInParameter(command, "AuthenticationLevel", DbType.Int64, BizActionObj.AuthenticationLevel);
                    if (BizActionObj.CheckExtraCriteria == true)
                    {
                        #region Newly Added IN Parameters
                        dbServer.AddInParameter(command, "SearchFromCollection", DbType.Boolean, BizActionObj.IsFromCollection);
                        dbServer.AddInParameter(command, "SearchFromReceive", DbType.Boolean, BizActionObj.IsFromReceive);
                        dbServer.AddInParameter(command, "SearchFromDispatch", DbType.Boolean, BizActionObj.IsFromDispatch);
                        dbServer.AddInParameter(command, "SeacrhFromAcceptReject", DbType.Boolean, BizActionObj.IsFromAcceptRejct);
                        dbServer.AddInParameter(command, "SearchFromResult", DbType.Boolean, BizActionObj.IsFromResult);
                        dbServer.AddInParameter(command, "SearchFromAutoriation", DbType.Boolean, BizActionObj.IsFromAuthorization);
                        dbServer.AddInParameter(command, "SearchFromUpload", DbType.Boolean, BizActionObj.IsFromUpload);
                        #endregion

                        dbServer.AddInParameter(command, "CheckExtraCriteria", DbType.Boolean, BizActionObj.CheckExtraCriteria);
                        dbServer.AddInParameter(command, "CheckSampleType", DbType.Boolean, BizActionObj.CheckSampleType);
                        dbServer.AddInParameter(command, "SampleType", DbType.Boolean, BizActionObj.SampleType);
                        dbServer.AddInParameter(command, "CheckUploadStatus", DbType.Boolean, BizActionObj.CheckUploadStatus);
                        dbServer.AddInParameter(command, "IsUploaded", DbType.Boolean, BizActionObj.IsUploaded);
                        dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);
                        // dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.IsDelivered);


                        #region added by rohini
                        dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered);
                        dbServer.AddInParameter(command, "IsDelivered1", DbType.Boolean, BizActionObj.OrderDetail.IsDelivered1);
                        dbServer.AddInParameter(command, "IsDeliverdthroughEmail", DbType.Boolean, BizActionObj.OrderDetail.IsDeliverdthroughEmail);
                        dbServer.AddInParameter(command, "IsDirectDelivered", DbType.Boolean, BizActionObj.OrderDetail.IsDirectDelivered);
                        dbServer.AddInParameter(command, "IsResultEntry", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry);
                        dbServer.AddInParameter(command, "IsResultEntry1", DbType.Boolean, BizActionObj.OrderDetail.IsResultEntry1);
                        dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, BizActionObj.OrderDetail.IsBilled);
                        dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, BizActionObj.OrderDetail.IsSampleCollected);
                        dbServer.AddInParameter(command, "IsSampleDispatched", DbType.Boolean, BizActionObj.OrderDetail.IsSampleDispatch);
                        dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, BizActionObj.OrderDetail.IsSampleReceive);
                        dbServer.AddInParameter(command, "IsAccepted", DbType.Boolean, BizActionObj.OrderDetail.IsAccepted);
                        dbServer.AddInParameter(command, "IsRejected", DbType.Boolean, BizActionObj.OrderDetail.IsRejected);
                        dbServer.AddInParameter(command, "IsSubOptimal", DbType.Boolean, BizActionObj.OrderDetail.IsSubOptimal);
                        dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, BizActionObj.OrderDetail.IsOutSourced);
                        dbServer.AddInParameter(command, "SampleCollectedBy", DbType.String, BizActionObj.OrderDetail.SampleCollectedBy);
                        dbServer.AddInParameter(command, "DispatchBy", DbType.String, BizActionObj.OrderDetail.DispatchBy);
                        dbServer.AddInParameter(command, "AcceptedOrRejectedByName", DbType.String, BizActionObj.OrderDetail.AcceptedOrRejectedByName);
                        dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, BizActionObj.OrderDetail.SampleReceiveBy);

                        dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.OrderDetail.AgencyID);
                        dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.OrderDetail.SampleNo);
                        dbServer.AddInParameter(command, "IsExternalPatient", DbType.Boolean, BizActionObj.OrderDetail.IsExternalPatient);
                        dbServer.AddInParameter(command, "CatagoryID", DbType.Int64, BizActionObj.OrderDetail.CategoryID);
                   
                        //temp
                      // dbServer.AddInParameter(command, "ResultEntryUserID", DbType.Int64, BizActionObj.OrderDetail.ResultEntryUserID);
                        //Added BY Bhushanp 19012017 For Add Date Filter
                        dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                        dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                        //by rohinee
                       // dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.IsFromReceive);
                        #endregion
                    }
                    DbDataReader reader;


                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.OrderBookingDetailList == null)
                            BizActionObj.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        while (reader.Read())
                        {
                            clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();

                            objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objOrderBookingVO.OrderBookingID = (long)DALHelper.HandleDBNull(reader["OrderID"]);
                            objOrderBookingVO.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                            objOrderBookingVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            objOrderBookingVO.SampleCollected = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollected"]);

                            //   objOrderBookingVO.IsSampleDispatched =Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                            //objOrderBookingVO.IsSampleReceived = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));

                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.IsCompleted = (bool)DALHelper.HandleDBNull(reader["IsCompleted"]);
                            objOrderBookingVO.IsDelivered = (bool)DALHelper.HandleDBNull(reader["IsDelivered"]);
                            objOrderBookingVO.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                            objOrderBookingVO.TestCode = (String)DALHelper.HandleDBNull(reader["TestCode"]);
                            objOrderBookingVO.TestName = (string)DALHelper.HandleDBNull(reader["TestName"]);
                            objOrderBookingVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);

                            objOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            // Uncommented on 9.06.2016 in order to get Path file for uploaded report
                            objOrderBookingVO.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                            objOrderBookingVO.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                            //
                            objOrderBookingVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                            objOrderBookingVO.IsOutSourced = (bool)DALHelper.HandleDBNull(reader["IsOutSourced"]);

                            objOrderBookingVO.Quantity = (double?)DALHelper.HandleDBNull(reader["Quantity"]);
                            objOrderBookingVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                            objOrderBookingVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                            objOrderBookingVO.IsResultEntry = (Boolean)DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            objOrderBookingVO.IsFinalized = (Boolean)DALHelper.HandleDBNull(reader["IsFinalized"]);
                            //  objOrderBookingVO.ReportTemplate = (Boolean)DALHelper.HandleDBNull(reader["ReportTemplate"]);
                            objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);
                            objOrderBookingVO.IsDirectDelivered = (bool)DALHelper.HandleBoolDBNull(reader["IsDirectDelivered"]);
                            objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);
                            objOrderBookingVO.IsDeliverdthroughEmail = (bool)DALHelper.HandleBoolDBNull(reader["IsDeliverdthroughEmail"]);

                            #region Newly Added

                            objOrderBookingVO.IsSampleCollected = (bool)DALHelper.HandleDBNull(reader["IsSampleCollected"]);
                            objOrderBookingVO.SampleCollectedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]);

                            objOrderBookingVO.IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                            objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);

                            objOrderBookingVO.IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));
                            objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);

                            objOrderBookingVO.SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader["SampleAcceptRejectStatus"]));
                            objOrderBookingVO.SampleAcceptanceDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                            objOrderBookingVO.SampleRejectionDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);
                            objOrderBookingVO.SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"]));
                            objOrderBookingVO.FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FirstLevel"]));
                            objOrderBookingVO.SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SecondLevel"]));
                            objOrderBookingVO.ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevel"]));
                            objOrderBookingVO.IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCheckedResults"]));
                            objOrderBookingVO.ReportTemplate = (Boolean)DALHelper.HandleDBNull(reader["ReportTemplate"]);
                            objOrderBookingVO.RefDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefDoctorID"]));
                            if (objOrderBookingVO.ReportTemplate == false)
                            {
                                objOrderBookingVO.ReportType = "Parameter";
                            }
                            else
                            {
                                objOrderBookingVO.ReportType = "Template";
                            }
                            objOrderBookingVO.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader["AppendSampleNo"]));
                            objOrderBookingVO.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                            objOrderBookingVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            objOrderBookingVO.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            objOrderBookingVO.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevelCheckResult"]));
                            objOrderBookingVO.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValueMessage"]));
                            objOrderBookingVO.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValForSecLevel"]));
                            objOrderBookingVO.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"]));
                            objOrderBookingVO.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));

                            //added by rohini dayted  
                            objOrderBookingVO.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                            objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                            objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));

                            objOrderBookingVO.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            objOrderBookingVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            objOrderBookingVO.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleGenerated"]));
                            objOrderBookingVO.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsServiceRefunded"]));
                            //Added By Bhushan New Date Coloumn 13/01/2017
                            objOrderBookingVO.ADateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                            BizActionObj.OrderBookingDetailList.Add(objOrderBookingVO);

                        }

                    }
                    reader.NextResult();

                    if (BizActionObj.objOutsourceOrderBookingDetail == null)
                        BizActionObj.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOutsourcingOrderBookingVO = new clsPathOrderBookingDetailVO();
                        objOutsourcingOrderBookingVO.OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        objOutsourcingOrderBookingVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objOutsourcingOrderBookingVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objOutsourcingOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                        objOutsourcingOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));

                        BizActionObj.objOutsourceOrderBookingDetail.Add(objOutsourcingOrderBookingVO);
                    }

                            #endregion

                    reader.Close();


                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
            }
            return valueObject;
        }


        public override IValueObject AddPathOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbTransaction myTrans, DbConnection myCon)
        {
            clsAddPathOrderBookingBizActionVO BizAction = valueObject as clsAddPathOrderBookingBizActionVO;

            DbConnection con = null;
            DbTransaction trans = null;

            try
            {

                if (myCon != null)
                    con = myCon;
                else
                    con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (myTrans != null)
                    trans = myTrans;
                else
                    con.BeginTransaction();


                clsPathOrderBookingVO objVO = BizAction.PathOrderBooking;

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPathOrderBooking");

                dbServer.AddInParameter(command1, "LinkServer", DbType.String, objVO.LinkServer);
                if (objVO.LinkServer != null)
                    dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command1, "OrderNo", DbType.String, objVO.OrderNo);

                dbServer.AddInParameter(command1, "BillID", DbType.String, objVO.BillID);

                dbServer.AddInParameter(command1, "BillNo", DbType.String, objVO.BillNo);


                dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, objVO.ChargeID);
                dbServer.AddInParameter(command1, "TariffServiceID", DbType.Int64, objVO.TariffServiceID);
                dbServer.AddInParameter(command1, "TestCharges", DbType.Single, objVO.TestCharges);
                dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, objVO.DoctorID);

                dbServer.AddInParameter(command1, "Date", DbType.DateTime, objVO.OrderDate);
                dbServer.AddInParameter(command1, "Time", DbType.DateTime, objVO.Time);
                dbServer.AddInParameter(command1, "SampleType", DbType.Boolean, objVO.SampleType);
                dbServer.AddInParameter(command1, "Opd_Ipd_External_ID", DbType.Int64, objVO.Opd_Ipd_External_ID);
                dbServer.AddInParameter(command1, "Opd_Ipd_External_UnitID", DbType.Int64, objVO.Opd_Ipd_External_UnitID);
                dbServer.AddInParameter(command1, "Opd_Ipd_External", DbType.Int64, objVO.Opd_Ipd_External);
                dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, objVO.IsApproved);
                dbServer.AddInParameter(command1, "IsDelivered", DbType.Boolean, objVO.IsDelivered);
                dbServer.AddInParameter(command1, "IsCompleted", DbType.Boolean, objVO.IsCompleted);
                dbServer.AddInParameter(command1, "IsPrinted", DbType.Boolean, objVO.IsPrinted);
                dbServer.AddInParameter(command1, "IsOrderGenerated", DbType.Boolean, objVO.IsOrderGenerated);
                //added by rohini dated 8.3.16
                dbServer.AddInParameter(command1, "IsExternalPatient", DbType.Boolean, objVO.IsExternalPatient);
                //
                dbServer.AddInParameter(command1, "TestType", DbType.Int64, objVO.TestType);
                dbServer.AddInParameter(command1, "IsCancelled", DbType.Boolean, objVO.IsCancelled);
                dbServer.AddInParameter(command1, "IsResultEntry", DbType.Boolean, objVO.IsResultEntry);

                dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, objVO.Status);
                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //Commented  By Bhushanp 13/01/2017
               // dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime,  Convert.ToDateTime(DateTime.Now));
                dbServer.AddParameter(command1, "AddedDateTime", DbType.DateTime, ParameterDirection.InputOutput, null, DataRowVersion.Default, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = 0;

                // if (BizAction.myTransaction == false)
                dbServer.ExecuteNonQuery(command1, trans);
                // else
                // dbServer.ExecuteNonQuery(command1);


                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                BizAction.PathOrderBooking.ID = (long)dbServer.GetParameterValue(command1, "ID");
                BizAction.PathOrderBooking.Date = Convert.ToDateTime(dbServer.GetParameterValue(command1, "AddedDateTime"));

                if (BizAction.PathOrderBookingDetailList != null && BizAction.PathOrderBooking.ID != 0)
                {
                    List<clsPathOrderBookingDetailVO> objDetailVO = BizAction.PathOrderBookingDetailList;
                    int count = BizAction.PathOrderBookingDetailList.Count;

                    for (int i = 0; i < count; i++)
                    {

                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddPathOrderBookingDetail");

                        dbServer.AddInParameter(command2, "LinkServer", DbType.String, objDetailVO[i].LinkServer);
                        if (objDetailVO[i].LinkServer != null)
                            dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objDetailVO[i].LinkServer.Replace(@"\", "_"));
                        //Added BY Bhushanp 13/01/2017
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, BizAction.PathOrderBooking.Date);
                        dbServer.AddInParameter(command2, "OrderID", DbType.Int64, BizAction.PathOrderBooking.ID);
                        dbServer.AddInParameter(command2, "TestID", DbType.Int64, objDetailVO[i].TestID);
                        dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, objDetailVO[i].ChargeID);
                        dbServer.AddInParameter(command2, "TariffServiceID", DbType.Int64, objDetailVO[i].TariffServiceID);
                        dbServer.AddInParameter(command2, "IsEmergency", DbType.Boolean, objDetailVO[i].IsEmergency);
                        dbServer.AddInParameter(command2, "TestCharges", DbType.Single, objDetailVO[i].TestCharges);
                        //dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, objDetailVO[i].DoctorID);
                        dbServer.AddInParameter(command2, "PathologistID", DbType.Int64, objDetailVO[i].PathologistID);
                        dbServer.AddInParameter(command2, "Specimen", DbType.String, objDetailVO[i].Specimen);
                        dbServer.AddInParameter(command2, "ClinicalNote", DbType.String, objDetailVO[i].ClinicalNote);
                        dbServer.AddInParameter(command2, "SampleNo", DbType.String, objDetailVO[i].SampleNo);
                        dbServer.AddInParameter(command2, "FirstLevel", DbType.Boolean, objDetailVO[i].FirstLevel);
                        dbServer.AddInParameter(command2, "SecondLevel", DbType.Boolean, objDetailVO[i].SecondLevel);
                        dbServer.AddInParameter(command2, "ThirdLevel", DbType.Boolean, objDetailVO[i].ThirdLevel);
                        dbServer.AddInParameter(command2, "FirstLevelID", DbType.Int64, objDetailVO[i].FirstLevelID);
                        dbServer.AddInParameter(command2, "SecondLevelID", DbType.Int64, objDetailVO[i].SecondLevelID);
                        dbServer.AddInParameter(command2, "ThirdLevelID", DbType.Int64, objDetailVO[i].ThirdLevelID);
                        dbServer.AddInParameter(command2, "IsApproved", DbType.Boolean, objDetailVO[i].IsApproved);
                        dbServer.AddInParameter(command2, "IsCompleted", DbType.Boolean, objDetailVO[i].IsCompleted);
                        dbServer.AddInParameter(command2, "IsDelivered", DbType.Boolean, objDetailVO[i].IsDelivered);
                        dbServer.AddInParameter(command2, "IsPrinted", DbType.Boolean, objDetailVO[i].IsPrinted);
                        dbServer.AddInParameter(command2, "MicrobiologistID", DbType.Int64, objDetailVO[i].MicrobiologistID);
                        dbServer.AddInParameter(command2, "Pathologist_1_ID", DbType.Int64, objDetailVO[i].Pathologist_1_ID);
                        dbServer.AddInParameter(command2, "Pathologist_2_ID", DbType.Int64, objDetailVO[i].Pathologist_2_ID);
                        dbServer.AddInParameter(command2, "RefDoctor", DbType.String, objDetailVO[i].RefDoctor);
                        dbServer.AddInParameter(command2, "SampleCollectionNO", DbType.String, objDetailVO[i].SampleCollectionNO);
                        dbServer.AddInParameter(command2, "IsOutSourced", DbType.Boolean, objDetailVO[i].IsOutSourced);
                        dbServer.AddInParameter(command2, "ExtAgencyID", DbType.Int64, objDetailVO[i].AgencyID);
                        dbServer.AddInParameter(command2, "Quantity", DbType.Double, objDetailVO[i].Quantity);
                        dbServer.AddInParameter(command2, "IsSampleCollected", DbType.Boolean, objDetailVO[i].IsSampleCollected);
                        dbServer.AddInParameter(command2, "SampleCollected", DbType.DateTime, objDetailVO[i].SampleCollected);
                        dbServer.AddInParameter(command2, "ItemConsID", DbType.Int64, objDetailVO[i].ItemConsID);
                        dbServer.AddInParameter(command2, "IsResultEntry", DbType.Boolean, objDetailVO[i].IsResultEntry);
                        dbServer.AddInParameter(command2, "IsFinalized", DbType.Boolean, objDetailVO[i].IsFinalized);


                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objVO.Status);
                        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        //dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailVO[i].ID);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                        //if (BizAction.myTransaction == false)
                        intStatus = dbServer.ExecuteNonQuery(command2, trans);
                        //else
                        // intStatus = dbServer.ExecuteNonQuery(command2);


                        BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        BizAction.PathOrderBookingDetailList[i].ID = (long)dbServer.GetParameterValue(command2, "ID");



                    }
                }

                BizAction.SuccessStatus = 0;
                if (myCon == null) trans.Commit();
            }
            catch (Exception ex)
            {

                BizAction.SuccessStatus = -1;
                if (myCon == null) trans.Rollback();

            }
            finally
            {
                if (myCon == null)
                {
                    con.Close();
                    con = null;
                    trans = null;

                }
            }

            return valueObject;
        }

        public override IValueObject UpdatePathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            string BatchNo = "";
            clsUpdatePathOrderBookingDetailListBizActionVO BizAction = valueObject as clsUpdatePathOrderBookingDetailListBizActionVO;
            bool flag = false;
            int count = BizAction.OrderBookingDetailList.Count;
            ////for(int i=0 ; i<count ; i++)
            ////{
            //// if (BizAction.OrderBookingDetailList[i].IsSampleCollected)
            //if (BizAction.OrderBookingDetaildetails.IsSampleCollected)
            //{
            try
            {
                DbCommand command = null;
                //added by rohini dated 28.4.16
                if (BizAction.IsGenerateBatch == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetBatchCode");
                    try
                    {
                        dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                        dbServer.AddInParameter(command, "BatchNo", DbType.String, null);
                        dbServer.AddInParameter(command, "Dispatch", DbType.String, null);
                        dbServer.AddInParameter(command, "Unit", DbType.String, null);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UID);
                        dbServer.AddInParameter(command, "DispatchUnitID", DbType.Int64, BizAction.DID);

                        BatchNo = Convert.ToString(dbServer.ExecuteScalar(command));
                        //BatchNo = Convert.ToString(dbServer.GetParameterValue(command, "BatchNo"));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (command != null)
                        {
                            command.Dispose();
                            command.Connection.Close();
                        }
                    }
                }
                //------------------//
                foreach (var item in BizAction.OrderBookingDetailList)
                {

                    if (item.IsFromSampleColletion == true || BizAction.IsSampleGenerated==true)
                    {
                        #region Update Order Booking details for Sample Collection
                        flag = true;
                        try
                        {
                            ////clsPathOrderBookingDetailVO objVO = BizAction.OrderBookingDetailList[i];
                            clsPathOrderBookingDetailVO objVO = BizAction.OrderBookingDetaildetails;
                            command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetail");
                            dbServer.AddInParameter(command, "LinkServer", DbType.String, objVO.LinkServer);
                            if (objVO.LinkServer != null)
                                dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objVO.LinkServer.Replace(@"\", "_"));
                            
                            dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                            //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            if (BizAction.IsSampleGenerated == true)
                            {
                                dbServer.AddInParameter(command, "IsSampleGenerated", DbType.Boolean, BizAction.IsSampleGenerated);
                                dbServer.AddInParameter(command, "GenerateSampleFalseAtCollection", DbType.Boolean, true);
                               
                            }                             
                            else
                            {
                              
                                dbServer.AddInParameter(command, "IsSampleGenerated", DbType.Boolean, true);
                                dbServer.AddInParameter(command, "GenerateSampleFalseAtCollection", DbType.Boolean, false);
                            }
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                            dbServer.AddInParameter(command, "SampleNo", DbType.String, item.SampleNo);
                            dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, item.IsOutSourced);
                            dbServer.AddInParameter(command, "AgencyID", DbType.Int64, item.AgencyID);
                            dbServer.AddInParameter(command, "Quantity", DbType.Double, item.Quantity);
                            dbServer.AddInParameter(command, "SampleCollected", DbType.DateTime, BizAction.SampleCollectionDate);
                            if (BizAction.IsSampleGenerated == true)
                            {
                                dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, 0);
                            }
                            else
                            {
                                dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, item.IsSampleCollected);
                            }
                            dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command, "SampleCollectedDateTime", DbType.DateTime, item.SampleCollectedDateTime);
                            // dbServer.AddInParameter(command, "SampleReceivedDateTime", DbType.DateTime, item.SampleReceivedDateTime);
                            dbServer.AddInParameter(command, "SampleCollectionCenter", DbType.String, item.SampleCollectionCenter);

                            //added by rohini dated 5.2.16
                            dbServer.AddInParameter(command, "FastingStatusID", DbType.Int64, item.FastingStatusID);
                            dbServer.AddInParameter(command, "FastingStatusHrs", DbType.Int16, item.FastingStatusHrs);
                            dbServer.AddInParameter(command, "CollectionID", DbType.Int64, item.CollectionID);
                            dbServer.AddInParameter(command, "Gestation", DbType.String, item.Gestation);
                            dbServer.AddInParameter(command, "FastingStatusName", DbType.String, item.FastingStatusName);
                            dbServer.AddInParameter(command, "CollectionName", DbType.String, item.CollectionName);
                            dbServer.AddInParameter(command, "CollectionCenter", DbType.String, item.CollectionCenter);
                            dbServer.AddInParameter(command, "SampleCollectedBy", DbType.String, item.SampleCollectedBy);
                            //
                            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus = dbServer.ExecuteNonQuery(command);
                            BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (command != null)
                            {
                                command.Dispose();
                                command.Connection.Close();
                            }
                        }
                        #endregion
                    }
                    else if (item.IsFromSampleDispatch == true)
                    {
                        #region Update Orderbooking Details for Sample Dispatch
                        command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleDispatch");
                        try
                        {
                            dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                            dbServer.AddInParameter(command, "IsSampleDispatched", DbType.Boolean, item.IsSampleDispatch);
                            //added by rohini dated 5.2.16
                            dbServer.AddInParameter(command, "DispatchToID", DbType.Int64, item.DispatchToID);
                            dbServer.AddInParameter(command, "DispatchToName", DbType.String, item.DispatchToName);
                            dbServer.AddInParameter(command, "DispatchBy", DbType.String, item.DispatchBy);
                            dbServer.AddInParameter(command, "BatchNo", DbType.String, BatchNo);
                            dbServer.AddInParameter(command, "OrderID", DbType.Int64, item.OrderID);
                            //
                            dbServer.AddInParameter(command, "SampleDispatchDateTime", DbType.DateTime, item.SampleDispatchDateTime);
                            dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus = dbServer.ExecuteNonQuery(command);
                            BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                            BizAction.returnBatch = BatchNo;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (command != null)
                            {
                                command.Dispose();
                                command.Connection.Close();
                            }
                        }
                        #endregion
                    }
                    else if (item.IsFromSampleReceive == true)
                    {
                        //code need to change
                        if (BizAction.IsExternalPatient == true)
                        {
                            #region Update Order Booking details for Sample Collection
                            flag = true;
                            try
                            {
                                ////clsPathOrderBookingDetailVO objVO = BizAction.OrderBookingDetailList[i];
                                clsPathOrderBookingDetailVO objVO = BizAction.OrderBookingDetaildetails;
                                command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetail");
                                dbServer.AddInParameter(command, "LinkServer", DbType.String, objVO.LinkServer);
                                if (objVO.LinkServer != null)
                                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objVO.LinkServer.Replace(@"\", "_"));

                                dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                                dbServer.AddInParameter(command, "SampleNo", DbType.String, item.SampleNo);
                                dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, item.IsOutSourced);
                                dbServer.AddInParameter(command, "AgencyID", DbType.Int64, item.AgencyID);
                                dbServer.AddInParameter(command, "Quantity", DbType.Double, item.Quantity);
                                dbServer.AddInParameter(command, "SampleCollected", DbType.DateTime, System.DateTime.Now);
                                dbServer.AddInParameter(command, "IsSampleCollected", DbType.Boolean, true);

                                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                dbServer.AddInParameter(command, "SampleCollectedDateTime", DbType.DateTime, System.DateTime.Now);
                                // dbServer.AddInParameter(command, "SampleReceivedDateTime", DbType.DateTime, item.SampleReceivedDateTime);
                                dbServer.AddInParameter(command, "SampleCollectionCenter", DbType.String, item.SampleCollectionCenter);

                                //added by rohini dated 5.2.16
                                dbServer.AddInParameter(command, "FastingStatusID", DbType.Int64, item.FastingStatusID);
                                dbServer.AddInParameter(command, "FastingStatusHrs", DbType.Int16, item.FastingStatusHrs);
                                dbServer.AddInParameter(command, "CollectionID", DbType.Int64, item.CollectionID);
                                dbServer.AddInParameter(command, "Gestation", DbType.String, item.Gestation);
                                dbServer.AddInParameter(command, "FastingStatusName", DbType.String, item.FastingStatusName);
                                dbServer.AddInParameter(command, "CollectionName", DbType.String, item.CollectionName);
                                dbServer.AddInParameter(command, "CollectionCenter", DbType.String, item.CollectionCenter);
                                dbServer.AddInParameter(command, "SampleCollectedBy", DbType.String, item.SampleCollectedBy);
                                //
                                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus = dbServer.ExecuteNonQuery(command);
                                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                            #endregion

                            #region Update Orderbooking Details for Sample Dispatch
                            command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleDispatch");
                            try
                            {
                                dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                                dbServer.AddInParameter(command, "IsSampleDispatched", DbType.Boolean, true);
                                //added by rohini dated 5.2.16
                                dbServer.AddInParameter(command, "DispatchToID", DbType.Int64, BizAction.UnitID);  //INHOUSE DISPATCH
                                dbServer.AddInParameter(command, "DispatchToName", DbType.String, item.DispatchToName);
                                dbServer.AddInParameter(command, "DispatchBy", DbType.String, item.DispatchBy);
                                //
                                dbServer.AddInParameter(command, "SampleDispatchDateTime", DbType.DateTime, System.DateTime.Now);
                                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                                int intStatus = dbServer.ExecuteNonQuery(command);
                                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                            #endregion

                            #region Update Order Booking Details For Sample Receive
                            command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleReceive");
                            try
                            {
                                dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                                dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, item.IsSampleReceive);
                                dbServer.AddInParameter(command, "SampleReceivedDateTime", DbType.DateTime, item.SampleReceivedDateTime);
                                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                                //added by rohini dated 12.2.16  
                                dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, item.SampleReceiveBy);
                                //
                                int intStatus = dbServer.ExecuteNonQuery(command);
                                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            finally
                            {
                                if (command != null)
                                {
                                    command.Dispose();
                                    command.Connection.Close();
                                }
                            }
                            #endregion
                        }
                        else
                        {

                            #region Update Order Booking Details For Sample Receive
                            command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleReceive");
                            try
                            {
                                dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                                dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, item.IsSampleReceive);
                                dbServer.AddInParameter(command, "SampleReceivedDateTime", DbType.DateTime, item.SampleReceivedDateTime);
                                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                                //added by rohini dated 12.2.16  
                                dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, item.SampleReceiveBy);
                                //
                                int intStatus = dbServer.ExecuteNonQuery(command);
                                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            finally
                            {
                                if (command != null)
                                {
                                    command.Dispose();
                                    command.Connection.Close();
                                }
                            }
                            #endregion

                        }

                    }
                    else if (item.IsFromSampleAcceptReject)
                    {
                        #region Update Order Booking Details For Sample Receive
                        command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleReceive");
                        try
                        {
                            dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                            dbServer.AddInParameter(command, "IsSampleReceived", DbType.Boolean, item.IsSampleReceive);
                            dbServer.AddInParameter(command, "SampleReceivedDateTime", DbType.DateTime, item.SampleReceivedDateTime);
                            dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                            //added by rohini dated 12.2.16  
                            dbServer.AddInParameter(command, "SampleReceiveBy", DbType.String, item.AcceptedOrRejectedByName);
                            //
                            int intStatus = dbServer.ExecuteNonQuery(command);
                            BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (command != null)
                            {
                                command.Dispose();
                                command.Connection.Close();
                            }
                        }
                        #endregion

                        #region Update Order Booking Details Fro Sample Acceptance/Rejection
                        command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleAcceptReject");
                        try
                        {
                            dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                            dbServer.AddInParameter(command, "IsSampleAccepted", DbType.Boolean, item.IsSampleAccepted);
                            dbServer.AddInParameter(command, "SampleAcceptRejectStatus", DbType.Byte, item.SampleAcceptRejectStatus);
                            if (item.IsSampleAccepted)
                                dbServer.AddInParameter(command, "SampleAcceptDateTime", DbType.DateTime, item.SampleAcceptanceDateTime);
                            else
                                dbServer.AddInParameter(command, "SampleRejectDateTime", DbType.DateTime, item.SampleRejectionDateTime);
                            dbServer.AddInParameter(command, "RejectionRemark", DbType.String, item.SampleRejectionRemark);

                            //added by rohini dated 5.2.16
                            dbServer.AddInParameter(command, "AcceptedOrRejectedByID", DbType.String, item.AcceptedOrRejectedByID);
                            dbServer.AddInParameter(command, "AcceptedOrRejectedByName", DbType.String, item.AcceptedOrRejectedByName);
                            dbServer.AddInParameter(command, "IsAccepted", DbType.Boolean, item.IsAccepted);
                            dbServer.AddInParameter(command, "IsRejected", DbType.Boolean, item.IsRejected);
                            dbServer.AddInParameter(command, "IsSubOptimal", DbType.Boolean, item.IsSubOptimal);
                            dbServer.AddInParameter(command, "Remark", DbType.String, item.Remark);
                            // dbServer.AddInParameter(command, "IsResendForNewSample", DbType.String, item.IsResendForNewSample);

                            dbServer.AddInParameter(command, "IsResendForNewSample", DbType.Boolean, item.IsResendForNewSample);

                            //
                            dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus = dbServer.ExecuteNonQuery(command);
                            BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (command != null)
                            {
                                command.Dispose();
                                command.Connection.Close();
                            }
                        }
                        #endregion
                    }

                }
                BatchNo = string.Empty;
            }
            catch (Exception ex)
            {

            }
            //}


            //}
            if (flag == true)
            {
                try
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBooking");
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, BizAction.OrderID);
                    //dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizAction.UnitID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                }
                catch (Exception ex)
                {

                }
            }

            return valueObject;
        }

        public override IValueObject AddPathPatientReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathPatientReportBizActionVO BizAction = valueObject as clsAddPathPatientReportBizActionVO;

            if (BizAction.OrderPathPatientReportList.ID == 0)
                BizAction = AddResult(BizAction, UserVo);
            else
                BizAction = UpdateResult(BizAction, UserVo);

            return valueObject;

        }

        private clsAddPathPatientReportBizActionVO AddResult(clsAddPathPatientReportBizActionVO BizAction, clsUserVO UserVo)
        {
            #region Add Result Entry
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsPathOrderBookingDetailVO Ml = new clsPathOrderBookingDetailVO();
                BizAction.MasterList = new List<clsPathOrderBookingDetailVO>();
                foreach (var item in BizAction.OrderList)
                {
                    #region Add Pathology Patient Report

                   
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");
                        dbServer.AddInParameter(command, "OrderID", DbType.Int64, item.OrderID);
                        dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, item.PathOrderBookingDetailID);
           
                        dbServer.AddInParameter(command, "SampleNo", DbType.String, item.SampleNo);
                        dbServer.AddInParameter(command, "SampleCollectionTime", DbType.DateTime, item.SampleCollectionTime);
                        dbServer.AddInParameter(command, "PathologistID1", DbType.Int64, item.PathologistID1);//BizAction.OrderPathPatientReportList.PathologistID1);
                        dbServer.AddInParameter(command, "PathologistID2", DbType.Int64, item.PathologistID2);
                        dbServer.AddInParameter(command, "PathologistID3", DbType.Int64, item.PathologistID3);
                        dbServer.AddInParameter(command, "RefDoctorID", DbType.Int64, item.RefDoctorID);
                        dbServer.AddInParameter(command, "ReferredBy", DbType.String, item.ReferredBy);

                        dbServer.AddInParameter(command, "IsFirstLevel", DbType.Boolean, item.IsFirstLevel);


                        dbServer.AddInParameter(command, "IsSecondLevel", DbType.Boolean, item.IsSecondLevel);
                        dbServer.AddInParameter(command, "IsThirdLevel", DbType.Boolean, item.IsThirdLevel);

                        dbServer.AddInParameter(command, "IsAutoApproved", DbType.Int64, item.IsAutoApproved);
                       

                        dbServer.AddInParameter(command, "IsFinalized ", DbType.Boolean, item.IsFinalized);
                        dbServer.AddInParameter(command, "ResultAddedDate ", DbType.DateTime, item.ResultAddedDateTime);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.Name);
                        dbServer.AddInParameter(command, "IsDoctorAuthorization ", DbType.Boolean, item.IsDoctorAuthorization);
                      //  dbServer.AddInParameter(command, "IsResultModified ", DbType.Boolean, item.IsResultModified);
                        dbServer.AddInParameter(command, "DocAuthorizationID ", DbType.Int64, UserVo.ID);
                        //      dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.ID);
                     
                        dbServer.AddInParameter(command, "ApproveBy", DbType.String, item.ApproveBy);


                        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


                        int intStatus = dbServer.ExecuteNonQuery(command, trans);

                        item.ID = (long)dbServer.GetParameterValue(command, "ID");

                    #endregion

                        #region code before merging

                        //   clsPathPatientReportVO objVO = BizAction.OrderPathPatientReportList;
                        // foreach (var item in BizAction.OrderList)
                        //{
                        //DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");

                        //dbServer.AddInParameter(command, "OrderID", DbType.String, objVO.OrderID);
                        //dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objVO.PathOrderBookingDetailID);
                        //dbServer.AddInParameter(command, "SampleNo", DbType.Int64, objVO.SampleNo);
                        //dbServer.AddInParameter(command, "SampleCollectionTime", DbType.DateTime, objVO.SampleCollectionTime);
                        //dbServer.AddInParameter(command, "PathologistID1", DbType.Int64, objVO.PathologistID1);
                        //dbServer.AddInParameter(command, "PathologistID2", DbType.Int64, objVO.PathologistID2);
                        //dbServer.AddInParameter(command, "PathologistID3", DbType.Int64, objVO.PathologistID3);
                        //dbServer.AddInParameter(command, "ReferredBy", DbType.String, objVO.ReferredBy);
                        //dbServer.AddInParameter(command, "IsFinalized ", DbType.Boolean, objVO.IsFinalized);

                        //#region Newly added 

                        //dbServer.AddInParameter(command, "IsFirstLevel", DbType.Boolean, item.IsFirstLevel);
                        //dbServer.AddInParameter(command, "IsSecondLevel", DbType.Boolean, item.IsSecondLevel);
                        //dbServer.AddInParameter(command, "IsThirdLevel", DbType.Boolean, item.IsThirdLevel);

                        //#endregion

                        //dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                        //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        //dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);


                        //int intStatus = dbServer.ExecuteNonQuery(command);

                        //BizAction.OrderPathPatientReportList.ID = (long)dbServer.GetParameterValue(command, "ID");



                        //if (objVO.TestList != null && objVO.TestList.Count > 0)
                        //{
                        //    foreach (var TestList in objVO.TestList)
                        //    {
                        //        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientParameterDetails");

                        //        dbServer.AddInParameter(command2, "OrderID", DbType.Int64, objVO.OrderID);
                        //        dbServer.AddInParameter(command2, "PathPatientReportID", DbType.Int64, objVO.ID);
                        //        dbServer.AddInParameter(command2, "IsNumeric", DbType.Int64, TestList.IsNumeric);

                        //        dbServer.AddInParameter(command2, "TestID", DbType.Int64, objVO.TestID);
                        //        dbServer.AddInParameter(command2, "ParameterID", DbType.Int64, TestList.ParameterID);
                        //        dbServer.AddInParameter(command2, "CategoryID", DbType.Int64, TestList.CategoryID);
                        //        dbServer.AddInParameter(command2, "Category", DbType.String, TestList.Category);

                        //        dbServer.AddInParameter(command2, "SubTestID", DbType.Int64, TestList.PathoSubTestID);
                        //        dbServer.AddInParameter(command2, "ParameterName", DbType.String, TestList.ParameterName);
                        //        dbServer.AddInParameter(command2, "ParameterUnit", DbType.String, TestList.ParameterUnit);
                        //        dbServer.AddInParameter(command2, "ParameterPrintName", DbType.String, TestList.Print);
                        //        dbServer.AddInParameter(command2, "ResultValue", DbType.String, TestList.ResultValue);
                        //        dbServer.AddInParameter(command2, "DefaultValue", DbType.String, TestList.DefaultValue);
                        //        dbServer.AddInParameter(command2, "NormalRange", DbType.String, TestList.NormalRange);
                        //        dbServer.AddInParameter(command2, "HelpValue", DbType.String, TestList.HelpValue);
                        //        dbServer.AddInParameter(command2, "SuggetionNote", DbType.String, TestList.Note);
                        //        dbServer.AddInParameter(command2, "FootNote", DbType.String, TestList.FootNote);
                        //        dbServer.AddInParameter(command2, "SubTest", DbType.String, TestList.PathoSubTestName);
                        //        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        //        dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        //        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        //        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        //        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                        //        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, TestList.ID);
                        //        int intStatus2 = dbServer.ExecuteNonQuery(command2);
                        //        TestList.ID = (long)dbServer.GetParameterValue(command2, "ID");
                        //    }
                        //}



                        //if (objVO.ItemList != null && objVO.ItemList.Count > 0)
                        //{
                        //    foreach (var ItemList in objVO.ItemList)
                        //    {
                        //        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientItemDetails");

                        //        dbServer.AddInParameter(command1, "OrderID", DbType.Int64, objVO.OrderID);
                        //        dbServer.AddInParameter(command1, "PathPatientReportID", DbType.Int64, objVO.ID);
                        //        dbServer.AddInParameter(command1, "TestID", DbType.Int64, objVO.TestID);
                        //        dbServer.AddInParameter(command1, "StoreID", DbType.Int64, ItemList.StoreID);
                        //        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, ItemList.ItemID);
                        //        dbServer.AddInParameter(command1, "BatchID", DbType.Int64, ItemList.BatchID);
                        //        dbServer.AddInParameter(command1, "IdealQuantity", DbType.Double, ItemList.Quantity);
                        //        dbServer.AddInParameter(command1, "ActualQantity", DbType.Double, ItemList.ActualQantity);
                        //        dbServer.AddInParameter(command1, "BalQuantity", DbType.Double, ItemList.BalanceQuantity);
                        //        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, ItemList.ExpiryDate);
                        //        dbServer.AddInParameter(command1, "Remarks", DbType.String, ItemList.Remarks);


                        //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                        //        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        //        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        //        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        //        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                        //        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ItemList.ID);
                        //        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                        //        ItemList.ID = (long)dbServer.GetParameterValue(command1, "ID");


                        //        if (BizAction.AutoDeductStock == true)
                        //        {

                        //            clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                        //            clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
                        //            clsItemStockVO StockDetails = new clsItemStockVO();

                        //            StockDetails.ItemID = ItemList.ItemID;
                        //            StockDetails.BatchID = ItemList.BatchID;
                        //            StockDetails.TransactionTypeID = InventoryTransactionType.PathologyTestConsumption;
                        //            StockDetails.TransactionQuantity = ItemList.Quantity;
                        //            StockDetails.TransactionID = BizAction.OrderPathPatientReportList.ID;
                        //            StockDetails.Date = DateTime.Now;
                        //            StockDetails.Time = DateTime.Now;
                        //            StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        //            StockDetails.StoreID = ItemList.StoreID;
                        //            obj.Details = StockDetails;
                        //            obj.Details.ID = 0;

                        //            obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo);

                        //            if (obj.SuccessStatus == -1)
                        //            {
                        //                throw new Exception();
                        //            }

                        //            StockDetails.ID = obj.Details.ID;
                        //        }


                        //    }
                        //}









                        #endregion

                        #region Add Pathology Test Parameter Details
                        if (BizAction.TestList != null && BizAction.TestList.Count > 0)
                        {
                            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeletePathPatientParameterDetails");

                            dbServer.AddInParameter(command3, "OrderID", DbType.Int64, item.OrderID);
                            dbServer.AddInParameter(command3, "PathPatientReportID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);

                            foreach (var TestList in BizAction.TestList)
                            {
                                if (item.TestID == TestList.PathoTestID && item.SampleNo == TestList.SampleNo)  //modified by rohini
                                {
                                //if (item.TestID == TestList.PathoTestID )  //change by rohini
                                //{

                                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientParameterDetails");

                                    dbServer.AddInParameter(command2, "OrderID", DbType.Int64, item.OrderID);
                                    dbServer.AddInParameter(command2, "PathPatientReportID", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command2, "IsNumeric", DbType.Int64, TestList.IsNumeric);

                                    dbServer.AddInParameter(command2, "TestID", DbType.Int64, TestList.PathoTestID);
                                    dbServer.AddInParameter(command2, "ParameterID", DbType.Int64, TestList.ParameterID);
                                    dbServer.AddInParameter(command2, "CategoryID", DbType.Int64, TestList.CategoryID);
                                    dbServer.AddInParameter(command2, "Category", DbType.String, TestList.Category);

                                    dbServer.AddInParameter(command2, "SubTestID", DbType.Int64, TestList.PathoSubTestID);
                                    dbServer.AddInParameter(command2, "ParameterName", DbType.String, TestList.ParameterName);
                                    dbServer.AddInParameter(command2, "ParameterUnit", DbType.String, TestList.ParameterUnit);
                                    dbServer.AddInParameter(command2, "ParameterPrintName", DbType.String, TestList.Print);
                                    dbServer.AddInParameter(command2, "ResultValue", DbType.String, TestList.ResultValue);
                                    dbServer.AddInParameter(command2, "DefaultValue", DbType.String, TestList.DefaultValue);
                                    dbServer.AddInParameter(command2, "NormalRange", DbType.String, TestList.NormalRange);
                                    dbServer.AddInParameter(command2, "HelpValue", DbType.String, TestList.HelpValue);
                                    if( TestList.Note != string.Empty)
                                        dbServer.AddInParameter(command2, "SuggetionNote", DbType.String, TestList.Note);
                                    else
                                        dbServer.AddInParameter(command2, "SuggetionNote", DbType.String,null);
                                    if(TestList.FootNote != string.Empty)
                                        dbServer.AddInParameter(command2, "FootNote", DbType.String, TestList.FootNote);
                                    else
                                        dbServer.AddInParameter(command2, "FootNote", DbType.String, null);
                                    dbServer.AddInParameter(command2, "IsFirstLevel", DbType.String, TestList.IsFirstLevel);
                                    dbServer.AddInParameter(command2, "IsSecondLevel", DbType.String, TestList.IsSecondLevel);
                                    //dbServer.AddInParameter(command2, "IsSecondLevel", DbType.String, TestList.IsFinalize);
                                    dbServer.AddInParameter(command2, "SubTest", DbType.String, TestList.PathoSubTestName);
                                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                                    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    // referenceRange on 4.05.2016
                                    dbServer.AddInParameter(command2, "ReferenceRange", DbType.String, TestList.ReferenceRange);
                                    // Deltacheck 
                                    dbServer.AddInParameter(command2, "DeltaCheck", DbType.Double, TestList.DeltaCheckValue);
                                    dbServer.AddInParameter(command2, "ParameterDefaultValueId", DbType.String, TestList.ParameterDefaultValueId);
                                    //

                                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, TestList.ID);
                                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                                    TestList.ID = (long)dbServer.GetParameterValue(command2, "ID");
                                }
                            }
                            DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_CheckPathOrderBookingStatus");
                            dbServer.AddInParameter(command4, "OrderID", DbType.Int64, item.OrderID);
                            dbServer.AddInParameter(command4, "TestID", DbType.Int64, item.TestID);
                            dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddParameter(command4, "IsBalence", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.IsBalence);
                            dbServer.AddParameter(command4, "IsAbnormal", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.IsAbnormal);
                            dbServer.AddParameter(command4, "SampleNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????");
                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            item.IsBalence = Convert.ToInt16(dbServer.GetParameterValue(command4, "IsBalence"));
                            item.IsAbnormal = Convert.ToInt16(dbServer.GetParameterValue(command4, "IsAbnormal"));

                            string smpleNo = dbServer.GetParameterValue(command4, "SampleNo").ToString();

                            //foreach (var item in MasterList)
                            //{
                              clsPathOrderBookingDetailVO obj = new clsPathOrderBookingDetailVO();

                            obj.TestID = item.TestID;
                            obj.SampleNo = Convert.ToString(dbServer.GetParameterValue(command4, "SampleNo"));
                            obj.Status = item.ResultStatus;
                            BizAction.MasterList.Add(obj);
                            //}

                            //BizAction.MasterList.Add(new clsPathOrderBookingDetailVO(item.TestID, Convert.ToString(dbServer.GetParameterValue(command4, "SampleNo")), item.ResultStatus));

                        }
                        #endregion
                    }

                
                #region Add Pathology Template Report

                if (BizAction.OrderPathPatientReportList.TestID > 0)
                {
                    if (BizAction.OrderPathPatientReportList.ISTEMplate == true)
                    {
                        DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");
                        dbServer.AddInParameter(command11, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                        dbServer.AddInParameter(command11, "PathOrderBookingDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                        dbServer.AddInParameter(command11, "SampleNo", DbType.String, BizAction.OrderPathPatientReportList.SampleNo);
                        dbServer.AddInParameter(command11, "SampleCollectionTime", DbType.DateTime, BizAction.OrderPathPatientReportList.SampleCollectionTime);
                        dbServer.AddInParameter(command11, "PathologistID1", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                        dbServer.AddInParameter(command11, "PathologistID2", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID2);
                        dbServer.AddInParameter(command11, "PathologistID3", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID3);
                        dbServer.AddInParameter(command11, "ReferredBy", DbType.String, BizAction.OrderPathPatientReportList.ReferredBy);
                        dbServer.AddInParameter(command11, "RefDoctorID", DbType.Int64, BizAction.OrderPathPatientReportList.RefDoctorID);
                        dbServer.AddInParameter(command11, "IsFinalized ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFinalized);

                        dbServer.AddInParameter(command11, "IsFirstLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFirstLevel);
                        dbServer.AddInParameter(command11, "IsSecondLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsSecondLevel);
                        dbServer.AddInParameter(command11, "IsThirdLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsThirdLevel);

                        dbServer.AddInParameter(command11, "ResultAddedDate ", DbType.DateTime, BizAction.OrderPathPatientReportList.ResultAddedDateTime);
                        //dbServer.AddInParameter(command, "ResultAddedTime ", DbType.DateTime, item.ResultAddedTime);

                        // dbServer.AddInParameter(command, "SampleReceiveDate ", DbType.DateTime, item.SampleReceiveDate);
                        //dbServer.AddInParameter(command, "SampleReceiveTime ", DbType.DateTime, item.SampleReceiveTime);

                        dbServer.AddInParameter(command11, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command11, "Status", DbType.Boolean, BizAction.OrderPathPatientReportList.Status);
                        dbServer.AddInParameter(command11, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command11, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command11, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command11, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        //dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.UserName);
                        dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.Name);
                       
                        //  dbServer.AddInParameter(command, "IsResultModified ", DbType.Boolean, item.IsResultModified);
                        dbServer.AddInParameter(command11, "DocAuthorizationID ", DbType.Int64, UserVo.ID);
                        //      dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.ID);
                        dbServer.AddInParameter(command11, "IsDoctorAuthorization ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsDoctorAuthorization);
                        dbServer.AddInParameter(command11, "ApproveBy", DbType.String, BizAction.OrderPathPatientReportList.ApproveByTemplate);
                         dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, (long)BizAction.OrderPathPatientReportList.ID);


                        int intStatus = dbServer.ExecuteNonQuery(command11, trans);

                        BizAction.OrderPathPatientReportList.ID = (long)dbServer.GetParameterValue(command11, "ID");







                        DbCommand command33 = dbServer.GetStoredProcCommand("CIMS_DeletePathPatientTemplateDetails");

                        dbServer.AddInParameter(command33, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                        dbServer.AddInParameter(command33, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                        dbServer.AddInParameter(command33, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        int intStatus3 = dbServer.ExecuteNonQuery(command33);


                        DbCommand command222 = dbServer.GetStoredProcCommand("CIMS_AddPathoResultEntryTemplate");

                        dbServer.AddInParameter(command222, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                        dbServer.AddInParameter(command222, "OrderDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                        dbServer.AddInParameter(command222, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                        dbServer.AddInParameter(command222, "TestID", DbType.Int64, BizAction.OrderPathPatientReportList.TestID);
                        dbServer.AddInParameter(command222, "Pathologist", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                        dbServer.AddInParameter(command222, "Template", DbType.String, BizAction.OrderPathPatientReportList.TemplateDetails.Template);
                        dbServer.AddInParameter(command222, "TemplateID", DbType.Int64, BizAction.OrderPathPatientReportList.TemplateDetails.TemplateID);

                        dbServer.AddInParameter(command222, "Status", DbType.Boolean, true);
                        if (BizAction.UnitID > 0)
                            dbServer.AddInParameter(command222, "UnitId", DbType.Int64, BizAction.UnitID);
                        else
                            dbServer.AddInParameter(command222, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command222, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command222, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command222, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command222, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command222, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command222, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.OrderPathPatientReportList.TemplateDetails.ID);
                        int intStatus2 = dbServer.ExecuteNonQuery(command222, trans);
                        BizAction.OrderPathPatientReportList.TemplateDetails.ID = (long)dbServer.GetParameterValue(command222, "ID");
                    }
                }
                #endregion

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizAction.OrderList = null;
                BizAction.OrderPathPatientReportList = null;
                BizAction.TestList = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizAction;
            #endregion

        }

        private clsAddPathPatientReportBizActionVO UpdateResult(clsAddPathPatientReportBizActionVO BizAction, clsUserVO UserVo)
        {
            #region OLD Code Before Merging
            //try
            //{
            //    clsPathPatientReportVO objVO = BizAction.OrderPathPatientReportList;

            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePathPatientReport");

            //    dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
            //    dbServer.AddInParameter(command, "OrderID", DbType.String, objVO.OrderID);
            //    dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objVO.PathOrderBookingDetailID);

            //    dbServer.AddInParameter(command, "SampleNo", DbType.String, objVO.SampleNo);
            //    dbServer.AddInParameter(command, "SampleCollectionTime", DbType.DateTime, objVO.SampleCollectionTime);
            //    dbServer.AddInParameter(command, "PathologistID1", DbType.Int64, objVO.PathologistID1);
            //    dbServer.AddInParameter(command, "PathologistID2", DbType.Int64, objVO.PathologistID2);
            //    dbServer.AddInParameter(command, "PathologistID3", DbType.Int64, objVO.PathologistID3);
            //    dbServer.AddInParameter(command, "ReferredBy", DbType.String, objVO.ReferredBy);
            //    dbServer.AddInParameter(command, "IsFinalized ", DbType.Boolean, objVO.IsFinalized);
            //    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

            //    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
            //    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
            //    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

            //    int intStatus = dbServer.ExecuteNonQuery(command);

            //    if (objVO.TestList != null && objVO.TestList.Count > 0)
            //    {
            //        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeletePathPatientParameterDetails");

            //        dbServer.AddInParameter(command3, "OrderID", DbType.Int64, objVO.OrderID);
            //        dbServer.AddInParameter(command3, "PathPatientReportID", DbType.Int64, objVO.ID);
            //        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //        int intStatus2 = dbServer.ExecuteNonQuery(command3);
            //    }

            //    if (objVO.TestList != null && objVO.TestList.Count > 0)
            //    {
            //        foreach (var TestList in objVO.TestList)
            //        {
            //            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientParameterDetails");

            //            dbServer.AddInParameter(command2, "OrderID", DbType.Int64, objVO.OrderID);
            //            dbServer.AddInParameter(command2, "PathPatientReportID", DbType.Int64, objVO.ID);
            //            dbServer.AddInParameter(command2, "IsNumeric", DbType.Int64, TestList.IsNumeric);

            //            dbServer.AddInParameter(command2, "TestID", DbType.Int64, objVO.TestID);
            //            dbServer.AddInParameter(command2, "ParameterID", DbType.Int64, TestList.ParameterID);
            //            dbServer.AddInParameter(command2, "CategoryID", DbType.Int64, TestList.CategoryID);
            //            dbServer.AddInParameter(command2, "Category", DbType.String, TestList.Category);

            //            dbServer.AddInParameter(command2, "SubTestID", DbType.Int64, TestList.PathoSubTestID);
            //            dbServer.AddInParameter(command2, "ParameterName", DbType.String, TestList.ParameterName);
            //            dbServer.AddInParameter(command2, "ParameterUnit", DbType.String, TestList.ParameterUnit);
            //            dbServer.AddInParameter(command2, "ParameterPrintName", DbType.String, TestList.Print);
            //            dbServer.AddInParameter(command2, "ResultValue", DbType.String, TestList.ResultValue);
            //            dbServer.AddInParameter(command2, "DefaultValue", DbType.String, TestList.DefaultValue);
            //            dbServer.AddInParameter(command2, "NormalRange", DbType.String, TestList.NormalRange);
            //            dbServer.AddInParameter(command2, "HelpValue", DbType.String, TestList.HelpValue);
            //            dbServer.AddInParameter(command2, "SuggetionNote", DbType.String, TestList.Note);
            //            dbServer.AddInParameter(command2, "FootNote", DbType.String, TestList.FootNote);
            //            dbServer.AddInParameter(command2, "SubTest", DbType.String, TestList.PathoSubTestName);
            //            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
            //            dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
            //            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
            //            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
            //            dbServer.AddInParameter(command2, "LoginName", DbType.String, UserVo.UserLoginInfo.UserName);
            //            // referenceRange on 4.05.2016
            //            dbServer.AddInParameter(command2, "ReferenceRange", DbType.String, TestList.ReferenceRange);
            //            //



            //            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, TestList.ID);
            //            int intStatus2 = dbServer.ExecuteNonQuery(command2);
            //            TestList.ID = (long)dbServer.GetParameterValue(command2, "ID");
            //        }
            //    }

            //    if (objVO.ItemList != null && objVO.ItemList.Count > 0)
            //    {
            //        DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_DeletePathPatientItemDetails");

            //        dbServer.AddInParameter(command4, "OrderID", DbType.Int64, objVO.OrderID);
            //        dbServer.AddInParameter(command4, "PathPatientReportID", DbType.Int64, objVO.ID);
            //        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //        int intStatus2 = dbServer.ExecuteNonQuery(command4);
            //    }

            //    if (objVO.ItemList != null && objVO.ItemList.Count > 0)
            //    {
            //        foreach (var ItemList in objVO.ItemList)
            //        {
            //            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientItemDetails");

            //            dbServer.AddInParameter(command1, "OrderID", DbType.Int64, objVO.OrderID);
            //            dbServer.AddInParameter(command1, "PathPatientReportID", DbType.Int64, objVO.ID);
            //            dbServer.AddInParameter(command1, "TestID", DbType.Int64, objVO.TestID);
            //            dbServer.AddInParameter(command1, "StoreID", DbType.Int64, ItemList.StoreID);
            //            dbServer.AddInParameter(command1, "ItemID", DbType.Int64, ItemList.ItemID);
            //            dbServer.AddInParameter(command1, "BatchID", DbType.Int64, ItemList.BatchID);
            //            dbServer.AddInParameter(command1, "IdealQuantity", DbType.Double, ItemList.Quantity);
            //            dbServer.AddInParameter(command1, "ActualQantity", DbType.Double, ItemList.ActualQantity);
            //            dbServer.AddInParameter(command1, "BalQuantity", DbType.Double, ItemList.BalanceQuantity);
            //            dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, ItemList.ExpiryDate);
            //            dbServer.AddInParameter(command1, "Remarks", DbType.String, ItemList.Remarks);


            //            dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
            //            dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
            //            dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
            //            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



            //            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ItemList.ID);
            //            int intStatus1 = dbServer.ExecuteNonQuery(command1);
            //            ItemList.ID = (long)dbServer.GetParameterValue(command1, "ID");


            //            if (BizAction.AutoDeductStock == true)
            //            {

            //                clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
            //                clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
            //                clsItemStockVO StockDetails = new clsItemStockVO();

            //                StockDetails.ItemID = ItemList.ItemID;
            //                StockDetails.BatchID = ItemList.BatchID;
            //                StockDetails.TransactionTypeID = InventoryTransactionType.PathologyTestConsumption;
            //                StockDetails.TransactionQuantity = ItemList.Quantity;
            //                StockDetails.TransactionID = BizAction.OrderPathPatientReportList.ID;
            //                StockDetails.Date = DateTime.Now;
            //                StockDetails.Time = DateTime.Now;
            //                StockDetails.OperationType = InventoryStockOperationType.Subtraction;
            //                StockDetails.StoreID = ItemList.StoreID;
            //                obj.Details = StockDetails;
            //                obj.Details.ID = 0;

            //                obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo);

            //                if (obj.SuccessStatus == -1)
            //                {
            //                    throw new Exception();
            //                }

            //                StockDetails.ID = obj.Details.ID;
            //            }


            //        }
            //    }


            //}
            //catch (Exception ex)
            //{

            //}
            #endregion


            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsPathOrderBookingDetailVO Ml = new clsPathOrderBookingDetailVO();
                BizAction.MasterList = new List<clsPathOrderBookingDetailVO>();
                foreach (var item in BizAction.OrderList)
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePathPatientReport");
                    //dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "OrderID", DbType.String, item.OrderID);

                    dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, item.PathOrderBookingDetailID);
                    dbServer.AddInParameter(command, "PathologistID1", DbType.Int64, item.PathologistID1);
                    dbServer.AddInParameter(command, "RefDoctorID", DbType.Int64, item.RefDoctorID);
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, item.ReferredBy);
                    dbServer.AddInParameter(command, "IsFinalized ", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "IsSecondLevel ", DbType.Boolean, item.IsSecondLevel);
                    dbServer.AddInParameter(command, "IsAutoApproved", DbType.Int64, item.IsAutoApproved);
                    dbServer.AddInParameter(command, "ApproveBy", DbType.String, item.ApproveBy);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    int intStatus = dbServer.ExecuteNonQuery(command);

                    item.ID = (long)dbServer.GetParameterValue(command, "ID");

                    #region Add Pathology Test Parameter Details
                    if (BizAction.TestList != null && BizAction.TestList.Count > 0)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeletePathPatientParameterDetails");

                        dbServer.AddInParameter(command3, "OrderID", DbType.Int64, item.OrderID);
                        dbServer.AddInParameter(command3, "PathPatientReportID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);

                        foreach (var TestList in BizAction.TestList)
                        {
                            if (item.TestID == TestList.PathoTestID && item.SampleNo == TestList.SampleNo)  //modified by rohini
                            {
                                //if (item.TestID == TestList.PathoTestID )  //change by rohini
                                //{

                                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientParameterDetails");

                                dbServer.AddInParameter(command2, "OrderID", DbType.Int64, item.OrderID);
                                dbServer.AddInParameter(command2, "PathPatientReportID", DbType.Int64, item.ID);
                                dbServer.AddInParameter(command2, "IsNumeric", DbType.Int64, TestList.IsNumeric);

                                dbServer.AddInParameter(command2, "TestID", DbType.Int64, TestList.PathoTestID);
                                dbServer.AddInParameter(command2, "ParameterID", DbType.Int64, TestList.ParameterID);
                                dbServer.AddInParameter(command2, "CategoryID", DbType.Int64, TestList.CategoryID);
                                dbServer.AddInParameter(command2, "Category", DbType.String, TestList.Category);

                                dbServer.AddInParameter(command2, "SubTestID", DbType.Int64, TestList.PathoSubTestID);
                                dbServer.AddInParameter(command2, "ParameterName", DbType.String, TestList.ParameterName);
                                dbServer.AddInParameter(command2, "ParameterUnit", DbType.String, TestList.ParameterUnit);
                                dbServer.AddInParameter(command2, "ParameterPrintName", DbType.String, TestList.Print);
                                dbServer.AddInParameter(command2, "ResultValue", DbType.String, TestList.ResultValue);
                                dbServer.AddInParameter(command2, "DefaultValue", DbType.String, TestList.DefaultValue);
                                dbServer.AddInParameter(command2, "NormalRange", DbType.String, TestList.NormalRange);
                                dbServer.AddInParameter(command2, "HelpValue", DbType.String, TestList.HelpValue);
                                if (TestList.Note != string.Empty)
                                    dbServer.AddInParameter(command2, "SuggetionNote", DbType.String, TestList.Note);
                                else
                                    dbServer.AddInParameter(command2, "SuggetionNote", DbType.String, null);
                                if (TestList.FootNote != string.Empty)
                                    dbServer.AddInParameter(command2, "FootNote", DbType.String, TestList.FootNote);
                                else
                                    dbServer.AddInParameter(command2, "FootNote", DbType.String, null);
                                dbServer.AddInParameter(command2, "IsFirstLevel", DbType.String, TestList.IsFirstLevel);
                                dbServer.AddInParameter(command2, "IsSecondLevel", DbType.String, TestList.IsSecondLevel);
                                //dbServer.AddInParameter(command2, "IsSecondLevel", DbType.String, TestList.IsFinalize);
                                dbServer.AddInParameter(command2, "SubTest", DbType.String, TestList.PathoSubTestName);
                                dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                                dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                // referenceRange on 4.05.2016
                                dbServer.AddInParameter(command2, "ReferenceRange", DbType.String, TestList.ReferenceRange);
                                // Deltacheck 
                                dbServer.AddInParameter(command2, "DeltaCheck", DbType.Double, TestList.DeltaCheckValue);
                                dbServer.AddInParameter(command2, "ParameterDefaultValueId", DbType.String, TestList.ParameterDefaultValueId);
                                //

                                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, TestList.ID);
                                int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                                TestList.ID = (long)dbServer.GetParameterValue(command2, "ID");
                            }
                        }
                        DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_CheckPathOrderBookingStatus");
                        dbServer.AddInParameter(command4, "OrderID", DbType.Int64, item.OrderID);
                        dbServer.AddInParameter(command4, "TestID", DbType.Int64, item.TestID);
                        dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddParameter(command4, "IsBalence", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.IsBalence);
                        dbServer.AddParameter(command4, "IsAbnormal", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.IsAbnormal);
                        dbServer.AddParameter(command4, "SampleNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????");
                        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                        item.IsBalence = Convert.ToInt16(dbServer.GetParameterValue(command4, "IsBalence"));
                        item.IsAbnormal = Convert.ToInt16(dbServer.GetParameterValue(command4, "IsAbnormal"));

                        string smpleNo = dbServer.GetParameterValue(command4, "SampleNo").ToString();

                        //foreach (var item in MasterList)
                        //{
                        clsPathOrderBookingDetailVO obj = new clsPathOrderBookingDetailVO();

                        obj.TestID = item.TestID;
                        obj.SampleNo = Convert.ToString(dbServer.GetParameterValue(command4, "SampleNo"));
                        obj.Status = item.ResultStatus;
                        BizAction.MasterList.Add(obj);
                        //}

                        //BizAction.MasterList.Add(new clsPathOrderBookingDetailVO(item.TestID, Convert.ToString(dbServer.GetParameterValue(command4, "SampleNo")), item.ResultStatus));

                    }
                    #endregion

              }
                #region Add Pathology Template Report

                if (BizAction.OrderPathPatientReportList.TestID > 0)
                {
                    if (BizAction.OrderPathPatientReportList.ISTEMplate == true)
                    {
                        DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");
                        dbServer.AddInParameter(command11, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                        dbServer.AddInParameter(command11, "PathOrderBookingDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                        dbServer.AddInParameter(command11, "SampleNo", DbType.String, BizAction.OrderPathPatientReportList.SampleNo);
                        dbServer.AddInParameter(command11, "SampleCollectionTime", DbType.DateTime, BizAction.OrderPathPatientReportList.SampleCollectionTime);
                        dbServer.AddInParameter(command11, "PathologistID1", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                        dbServer.AddInParameter(command11, "PathologistID2", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID2);
                        dbServer.AddInParameter(command11, "PathologistID3", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID3);
                        dbServer.AddInParameter(command11, "ReferredBy", DbType.String, BizAction.OrderPathPatientReportList.ReferredBy);
                        dbServer.AddInParameter(command11, "RefDoctorID", DbType.Int64, BizAction.OrderPathPatientReportList.RefDoctorID);
                        dbServer.AddInParameter(command11, "IsFinalized ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFinalized);

                        dbServer.AddInParameter(command11, "IsFirstLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFirstLevel);
                        dbServer.AddInParameter(command11, "IsSecondLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsSecondLevel);
                        dbServer.AddInParameter(command11, "IsThirdLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsThirdLevel);

                        dbServer.AddInParameter(command11, "ResultAddedDate ", DbType.DateTime, BizAction.OrderPathPatientReportList.ResultAddedDateTime);
                        //dbServer.AddInParameter(command, "ResultAddedTime ", DbType.DateTime, item.ResultAddedTime);

                        // dbServer.AddInParameter(command, "SampleReceiveDate ", DbType.DateTime, item.SampleReceiveDate);
                        //dbServer.AddInParameter(command, "SampleReceiveTime ", DbType.DateTime, item.SampleReceiveTime);

                        dbServer.AddInParameter(command11, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command11, "Status", DbType.Boolean, BizAction.OrderPathPatientReportList.Status);
                        dbServer.AddInParameter(command11, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command11, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command11, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command11, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        //dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.UserName);
                        dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.Name);

                        //  dbServer.AddInParameter(command, "IsResultModified ", DbType.Boolean, item.IsResultModified);
                        dbServer.AddInParameter(command11, "DocAuthorizationID ", DbType.Int64, UserVo.ID);
                        //      dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.ID);
                        dbServer.AddInParameter(command11, "IsDoctorAuthorization ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsDoctorAuthorization);
                        dbServer.AddInParameter(command11, "ApproveBy", DbType.String, BizAction.OrderPathPatientReportList.ApproveByTemplate);
                        dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, (long)BizAction.OrderPathPatientReportList.ID);


                        int intStatus = dbServer.ExecuteNonQuery(command11, trans);

                        BizAction.OrderPathPatientReportList.ID = (long)dbServer.GetParameterValue(command11, "ID");

                        DbCommand command33 = dbServer.GetStoredProcCommand("CIMS_DeletePathPatientTemplateDetails");

                        dbServer.AddInParameter(command33, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                        dbServer.AddInParameter(command33, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                        dbServer.AddInParameter(command33, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        int intStatus3 = dbServer.ExecuteNonQuery(command33);


                        DbCommand command222 = dbServer.GetStoredProcCommand("CIMS_AddPathoResultEntryTemplate");

                        dbServer.AddInParameter(command222, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                        dbServer.AddInParameter(command222, "OrderDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                        dbServer.AddInParameter(command222, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                        dbServer.AddInParameter(command222, "TestID", DbType.Int64, BizAction.OrderPathPatientReportList.TestID);
                        dbServer.AddInParameter(command222, "Pathologist", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                        dbServer.AddInParameter(command222, "Template", DbType.String, BizAction.OrderPathPatientReportList.TemplateDetails.Template);
                        dbServer.AddInParameter(command222, "TemplateID", DbType.Int64, BizAction.OrderPathPatientReportList.TemplateDetails.TemplateID);

                        dbServer.AddInParameter(command222, "Status", DbType.Boolean, true);
                        if (BizAction.UnitID > 0)
                            dbServer.AddInParameter(command222, "UnitId", DbType.Int64, BizAction.UnitID);
                        else
                            dbServer.AddInParameter(command222, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command222, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command222, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command222, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command222, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command222, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command222, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.OrderPathPatientReportList.TemplateDetails.ID);
                        int intStatus2 = dbServer.ExecuteNonQuery(command222, trans);
                        BizAction.OrderPathPatientReportList.TemplateDetails.ID = (long)dbServer.GetParameterValue(command222, "ID");
                    }
                }
                #endregion

                trans.Commit();               
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizAction.OrderList = null;
                BizAction.OrderPathPatientReportList = null;
                BizAction.TestList = null;

            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizAction;
        }

        public override IValueObject UploadReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPathoUploadReportBizActionVO BizActionObj = valueObject as clsPathoUploadReportBizActionVO;

            try
            {
                clsPathPatientReportVO objVO = BizActionObj.UploadReportDetails;
                if (BizActionObj.IsResultEntry == true)
                {
                    //If Result entry is completed for patho order
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUploadReport");

                    dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objVO.PathOrderBookingDetailID);
                    dbServer.AddInParameter(command, "PathPatientReportID", DbType.Int64, objVO.PathPatientReportID);
                    dbServer.AddInParameter(command, "SourceURL", DbType.String, objVO.SourceURL);
                    dbServer.AddInParameter(command, "Report", DbType.Binary, objVO.Report);
                    dbServer.AddInParameter(command, "Notes", DbType.String, objVO.Notes);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, objVO.Remarks);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, objVO.Time);

                    if (BizActionObj.UnitID > 0)
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    else
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                }
                else
                {
                    //if result entry not done for patho order

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPathPatientReportWithOutResultEntry");


                    dbServer.AddInParameter(command, "SourceURL", DbType.String, objVO.SourceURL);
                    dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objVO.PathOrderBookingDetailID);
                   
                    dbServer.AddInParameter(command, "OrderID", DbType.Int64, objVO.OrderID);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, objVO.SampleNo);
                    dbServer.AddInParameter(command, "Report", DbType.Binary, objVO.Report);

                    dbServer.AddInParameter(command, "Notes", DbType.String, objVO.Notes);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, objVO.Remarks);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, objVO.Time);

                    if (BizActionObj.UnitID > 0)
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    else
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.UploadReportDetails.ServiceID);  //by rohini to update service rate in transaction as per mangesh told
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, BizActionObj.UploadReportDetails.AgencyID);  //by rohini to update service rate in transaction as per mangesh told
                     

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);

                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.UploadReportDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                }



            }
            catch (Exception ex)
            {
                
                throw;
            }
            return BizActionObj;
        }
        public override IValueObject UpdatePathOrderBookindDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePathOrderBookingDetailBizActionVO BizAction = valueObject as clsUpdatePathOrderBookingDetailBizActionVO;

            try
            {
                //clsPathOrderBookingDetailVO objVO = BizAction.PathOrderBookingDetail;
                List<clsPathOrderBookingDetailVO> objDetailVO = BizAction.PathOrderBookList;
                int count = objDetailVO.Count;
                DbCommand command = null;

                for (int i = 0; i < count; i++)
                {

                    //DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailForReportDelieverd");
                    command = dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailForDirectDelievery");

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailVO[i].LinkServer);
                    if (objDetailVO[i].LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailVO[i].LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailVO[i].ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailVO[i].UnitId);


                    dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, objDetailVO[i].UpdatedUnitId);

                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID); //changed by rohini for user save after delivery
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objDetailVO[i].UpdatedOn);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailVO[i].UpdatedDateTime);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objDetailVO[i].UpdatedWindowsLoginName);


                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);

                    BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
            }
            catch (Exception ex)
            {

            }

            return valueObject;
        }

        //Added by priyanka

        public override IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathTestDetailsBizActionVO BizActionObj = valueObject as clsGetPathTestDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestListForBill");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsPathOrderBookingVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingVO objPathoVO = new clsPathOrderBookingVO();
                        objPathoVO.ID = (long)reader["ID"];
                        objPathoVO.UnitId = (long)reader["UnitID"];
                        objPathoVO.TestID = (long)reader["TestID"];
                        objPathoVO.ServiceID = (long)reader["ServiceID"];
                        objPathoVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.TestList.Add(objPathoVO);
                    }


                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }

        // Added BY CDS modified by bhushan p

        public override IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsGetPathTestDetailsBizActionVO BizActionObj = valueObject as clsGetPathTestDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestListForBillWithPOBDID");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "POBID", DbType.Int64, BizActionObj.pobID);
                dbServer.AddInParameter(command, "POBUnitID", DbType.Int64, BizActionObj.pobUnitID);
                dbServer.AddInParameter(command, "ChargeID", DbType.Int64, BizActionObj.pChargeID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command,trans);
                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsPathOrderBookingVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingVO objPathoVO = new clsPathOrderBookingVO();
                        objPathoVO.ID = Convert.ToInt64(reader["ID"]);
                        objPathoVO.UnitId = Convert.ToInt64(reader["UnitID"]);
                        objPathoVO.TestID = Convert.ToInt64(reader["TestID"]);
                        objPathoVO.ServiceID = Convert.ToInt64(reader["ServiceID"]);
                        objPathoVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        objPathoVO.POBDID = Convert.ToInt64(reader["POBDID"]);
                        BizActionObj.TestList.Add(objPathoVO);
                    }


                }

                reader.Close();
            }            
            catch (Exception ex)
            {                
                trans.Rollback();
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject GetPathoTestItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestItemDetailsBizActionVO BizActionObj = (clsGetPathoTestItemDetailsBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestItemDetails");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsPathoTestItemDetailsVO>();

                    while (reader.Read())
                    {
                        clsPathoTestItemDetailsVO ObjItem = new clsPathoTestItemDetailsVO();
                        ObjItem.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjItem.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        ObjItem.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        ObjItem.Quantity = (float)(double)DALHelper.HandleDBNull(reader["Quantity"]);
                        ObjItem.ActualQantity = (float)(double)DALHelper.HandleDBNull(reader["Quantity"]);
                        ObjItem.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        ObjItem.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //ObjItem.BatchesRequired = (bool)DALHelper.HandleDBNull(reader["BatchesRequired"]);

                        BizActionObj.ItemList.Add(ObjItem);
                    }


                }
            }
            catch (Exception ex)
            {
            }

            return BizActionObj;
        }


        public override IValueObject GetPathoTestForResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestDetailsForResultEntryBizActionVO BizActionObj = (clsGetPathoTestDetailsForResultEntryBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestDetailsForResultEntry");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsPathoTestParameterVO>();

                    while (reader.Read())
                    {
                        clsPathoTestParameterVO ObjTest = new clsPathoTestParameterVO();

                        ObjTest.PathoTestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        ObjTest.PathoTestName = (string)DALHelper.HandleDBNull(reader["TestName"]);
                        ObjTest.ParamSTID = (long)DALHelper.HandleDBNull(reader["ParamSTID"]);
                        ObjTest.Print = (string)DALHelper.HandleDBNull(reader["PrintName"]);
                        ObjTest.Note = (string)DALHelper.HandleDBNull(reader["Note"]);
                        ObjTest.FootNote = (string)DALHelper.HandleDBNull(reader["FootNote"]);
                        ObjTest.IsParameter = (bool)DALHelper.HandleDBNull(reader["IsParameter"]);
                        ObjTest.IsNumeric = (bool)DALHelper.HandleDBNull(reader["IsNumeric"]);
                        ObjTest.ParameterUnit = (string)DALHelper.HandleDBNull(reader["ParameterUnit"]);
                        if (ObjTest.IsParameter == false)
                            ObjTest.PathoSubTestID = (long)DALHelper.HandleDBNull(reader["ParamSTID"]);
                        BizActionObj.TestList.Add(ObjTest);

                    }

                    reader.Close();


                    List<clsPathoTestParameterVO> objTest2 = new List<clsPathoTestParameterVO>();
                    //for (int i = 0; i < BizActionObj.TestList.Count; i++)
                    //{
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPathoTestParameterORPathoTestSubTestForResultEntry");
                    DbDataReader reader1 = null;
                    dbServer.AddInParameter(command1, "TestID", DbType.Int64, BizActionObj.TestID);
                    dbServer.AddInParameter(command1, "IsParameter", DbType.Boolean, BizActionObj.TestList[0].IsParameter);
                    dbServer.AddInParameter(command1, "IsNumeric", DbType.Boolean, BizActionObj.TestList[0].IsNumeric);
                    reader1 = (DbDataReader)dbServer.ExecuteReader(command1);

                    //Purpose:For Getting SubTest Name and Parameter name

                    while (reader1.Read())
                    {
                        if (BizActionObj.TestList[0].IsParameter == true)
                        {

                            foreach (var TestItem in BizActionObj.TestList.Where(x => x.ParamSTID == (long)DALHelper.HandleDBNull(reader1["ID"])))
                            {
                                if (TestItem.PathoTestID == BizActionObj.TestID)
                                {
                                    if (TestItem.IsParameter == true)
                                        TestItem.ParameterName = (string)DALHelper.HandleDBNull(reader1["Description"]);

                                }
                            }

                        }

                        else
                        {
                            foreach (var TestItem in BizActionObj.TestList.Where(x => x.ParamSTID == (long)DALHelper.HandleDBNull(reader1["ID"])))
                            {
                                if (TestItem.PathoTestID == BizActionObj.TestID)
                                {
                                    if (TestItem.IsParameter == false)
                                        TestItem.PathoSubTestName = (string)DALHelper.HandleDBNull(reader1["Description"]);

                                }
                            }
                        }

                    }
                    reader1.NextResult();

                    //Purpose:For getting normal range,default values  of parameter test 
                    if (BizActionObj.TestList[0].IsParameter == true)
                    {
                        long ChkID = 0;
                        while (reader1.Read())
                        {
                            //Purpose:For getting normal range,default values  of parameter test if parameter is numeric 
                            if (BizActionObj.TestList[0].IsNumeric == true)
                            {
                                clsPathoTestParameterVO objNew = new clsPathoTestParameterVO();
                                foreach (var item in BizActionObj.TestList)
                                {

                                    if (item.PathoTestID == BizActionObj.TestID)
                                    {
                                        if (item.ParamSTID == (long)DALHelper.HandleDBNull(reader1["ID"]))
                                        {
                                            objNew.PathoTestID = item.PathoTestID;
                                            objNew.PathoTestName = item.PathoTestName;
                                            objNew.ParamSTID = item.ParamSTID;
                                            objNew.Print = item.Print;
                                            objNew.Note = item.Note;
                                            objNew.FootNote = item.FootNote;
                                            objNew.IsParameter = item.IsParameter;
                                            objNew.IsNumeric = item.IsNumeric;

                                            objNew.ParameterUnit = item.ParameterUnit;
                                            if (objNew.IsParameter == false)
                                                objNew.PathoSubTestID = item.ParamSTID;

                                            objNew.ParameterName = item.ParameterName;
                                            objNew.PathoSubTestName = item.PathoSubTestName;

                                            objNew.MinValue = (float)(double)DALHelper.HandleDBNull(reader1["MinValue"]);
                                            objNew.MaxValue = (float)(double)DALHelper.HandleDBNull(reader1["MaxValue"]);
                                            objNew.NormalRange = ((double)DALHelper.HandleDBNull(reader1["MinValue"]) + " - " + (double)DALHelper.HandleDBNull(reader1["MaxValue"])).ToString();
                                            objNew.DefaultValue = ((double)DALHelper.HandleDBNull(reader1["DefaultValue"])).ToString();
                                            objNew.Category = (string)DALHelper.HandleDBNull(reader1["Category"]);
                                            // Added on 4.05.2016
                                            // ReferenceRange for setting the various flags as per low and high values.
                                            objNew.ReferenceRange = ((double)DALHelper.HandleDBNull(reader1["LowReffValue"]) + " - " + (double)DALHelper.HandleDBNull(reader1["HighReffValue"])).ToString();
                                            //

                                            objTest2.Add(objNew);
                                        }
                                    }
                                }
                            }
                            else
                            { //Purpose:For getting normal range,default values  of parameter test if parameter is Text 

                                clsPathoTestParameterVO objNew = new clsPathoTestParameterVO();

                                foreach (var item in BizActionObj.TestList)
                                {

                                    if (item.PathoTestID == BizActionObj.TestID)
                                    {
                                        if (item.ParamSTID == (long)DALHelper.HandleDBNull(reader1["ID"]))
                                        {


                                            if (item.ParamSTID == ChkID)
                                            {
                                                objTest2.Clear();
                                            }
                                            objNew.PathoTestID = item.PathoTestID;
                                            objNew.PathoTestName = item.PathoTestName;
                                            objNew.ParamSTID = item.ParamSTID;
                                            objNew.Print = item.Print;
                                            objNew.Note = item.Note;
                                            objNew.FootNote = item.FootNote;
                                            objNew.IsParameter = item.IsParameter;
                                            objNew.IsNumeric = item.IsNumeric;

                                            objNew.ParameterUnit = item.ParameterUnit;
                                            if (objNew.IsParameter == false)
                                                objNew.PathoSubTestID = item.ParamSTID;

                                            objNew.ParameterName = item.ParameterName;
                                            objNew.PathoSubTestName = item.PathoSubTestName;
                                            ChkID = item.ParamSTID;

                                            objTest2.Add(objNew);

                                            MasterListItem ObjHelp = new MasterListItem();

                                            ObjHelp.Description = (string)DALHelper.HandleDBNull(reader1["HelpValue"]);
                                            ObjHelp.ID = (long)DALHelper.HandleDBNull(reader1["HelpValueID"]);
                                            ObjHelp.Status = (bool)DALHelper.HandleDBNull(reader1["IsDefault"]);
                                            BizActionObj.HelpValueList.Add(ObjHelp);
                                        }
                                    }
                                }
                            }
                        }
                        reader1.NextResult();

                    }

                    //Purpose:For getting parameters if test is subtest 
                    if (BizActionObj.TestList[0].IsParameter == false)
                    {

                        while (reader1.Read())
                        {
                            foreach (var TestItem in BizActionObj.TestList.Where(x => x.ParamSTID == (long)DALHelper.HandleDBNull(reader1["ID"])))
                            {
                                if (TestItem.PathoTestID == BizActionObj.TestID)
                                {

                                    TestItem.ParameterName = (string)DALHelper.HandleDBNull(reader1["Description"]);
                                    TestItem.IsNumeric = (bool)DALHelper.HandleDBNull(reader1["IsNumeric"]);

                                }
                            }

                        }
                        reader1.NextResult();

                    }


                    //Purpose:For getting normal range,default values if test is subtest
                    if (BizActionObj.TestList[0].IsParameter == false)
                    {
                        while (reader1.Read())
                        {
                            clsPathoTestParameterVO objNew = new clsPathoTestParameterVO();
                            foreach (var item in BizActionObj.TestList)
                            {

                                if (item.PathoTestID == BizActionObj.TestID)
                                {
                                    if (item.ParamSTID == (long)DALHelper.HandleDBNull(reader1["ID"]))
                                    {
                                        objNew.PathoTestID = item.PathoTestID;
                                        objNew.PathoTestName = item.PathoTestName;
                                        objNew.ParamSTID = item.ParamSTID;
                                        objNew.Print = item.Print;
                                        objNew.Note = item.Note;
                                        objNew.FootNote = item.FootNote;
                                        objNew.IsParameter = item.IsParameter;
                                        objNew.IsNumeric = item.IsNumeric;

                                        objNew.ParameterUnit = item.ParameterUnit;
                                        if (objNew.IsParameter == false)
                                            objNew.PathoSubTestID = item.ParamSTID;

                                        objNew.ParameterName = item.ParameterName;
                                        objNew.PathoSubTestName = item.PathoSubTestName;

                                        objNew.MinValue = (float)(double)DALHelper.HandleDBNull(reader1["MinValue"]);
                                        objNew.MaxValue = (float)(double)DALHelper.HandleDBNull(reader1["MaxValue"]);
                                        objNew.NormalRange = ((double)DALHelper.HandleDBNull(reader1["MinValue"]) + " - " + (double)DALHelper.HandleDBNull(reader1["MaxValue"])).ToString();
                                        objNew.DefaultValue = ((double)DALHelper.HandleDBNull(reader1["DefaultValue"])).ToString();
                                        objNew.Category = (string)DALHelper.HandleDBNull(reader1["Category"]);
                                        // Added on 4.05.2016
                                        // ReferenceRange for setting the various flags as per low and high values.
                                        objNew.ReferenceRange = ((double)DALHelper.HandleDBNull(reader1["LowReffValue"]) + " - " + (double)DALHelper.HandleDBNull(reader1["HighReffValue"])).ToString();
                                        //
                                        objTest2.Add(objNew);
                                    }
                                }
                            }


                        }

                    }

                    reader1.Close();
                    //}
                    BizActionObj.TestList.Clear();
                    foreach (var item in objTest2)
                    {
                        BizActionObj.TestList.Add(item);
                    }



                }
            }
            catch (Exception ex)
            {
            }

            return BizActionObj;
        }

        public override IValueObject GetPathoResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoResultEntryBizActionVO BizActionObj = valueObject as clsGetPathoResultEntryBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoResultEntry");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizActionObj.DetailID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    if (BizActionObj.ResultEntryDetails == null)
                        BizActionObj.ResultEntryDetails = new clsPathPatientReportVO();
                    while (reader.Read())
                    {
                        BizActionObj.ResultEntryDetails.ID = (long)DALHelper.HandleDBNull(reader["PathPatientReportID"]);
                        BizActionObj.ResultEntryDetails.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizActionObj.ResultEntryDetails.SampleNo = (string)DALHelper.HandleDBNull(reader["SampleNo"]);
                        BizActionObj.ResultEntryDetails.SampleCollectionTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectionTime"]);
                        BizActionObj.ResultEntryDetails.PathologistID1 = (long)DALHelper.HandleDBNull(reader["PathologistID1"]);
                        BizActionObj.ResultEntryDetails.ReferredBy = (string)DALHelper.HandleDBNull(reader["ReferredBy"]);
                    }
                    reader.NextResult();

                    if (BizActionObj.ResultEntryDetails.TestList == null)
                        BizActionObj.ResultEntryDetails.TestList = new List<clsPathoTestParameterVO>();

                    while (reader.Read())
                    {
                        clsPathoTestParameterVO ObjTest = new clsPathoTestParameterVO();
                        ObjTest.PathoTestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        ObjTest.PathoSubTestID = (long)DALHelper.HandleDBNull(reader["SubTestID"]);
                        ObjTest.ParameterName = (string)DALHelper.HandleDBNull(reader["ParameterName"]);
                        ObjTest.ParameterID = (long)DALHelper.HandleDBNull(reader["ParameterID"]);
                        ObjTest.Category = (string)DALHelper.HandleDBNull(reader["Category"]);
                        ObjTest.CategoryID = (long?)DALHelper.HandleDBNull(reader["CategoryID"]);
                        ObjTest.ParameterID = (long)DALHelper.HandleDBNull(reader["ParameterID"]);
                        ObjTest.ParameterName = (string)DALHelper.HandleDBNull(reader["ParameterName"]);
                        ObjTest.ParameterUnit = (string)DALHelper.HandleDBNull(reader["ParameterUnit"]);
                        ObjTest.Print = (string)DALHelper.HandleDBNull(reader["ParameterPrintName"]);
                        ObjTest.ResultValue = (string)DALHelper.HandleDBNull(reader["ResultValue"]);
                        ObjTest.DefaultValue = (string)DALHelper.HandleDBNull(reader["DefaultValue"]);
                        ObjTest.NormalRange = (string)DALHelper.HandleDBNull(reader["NormalRange"]);
                        ObjTest.Note = (string)DALHelper.HandleDBNull(reader["SuggetionNote"]);
                        ObjTest.FootNote = (string)DALHelper.HandleDBNull(reader["FootNote"]);
                        ObjTest.PathoSubTestName = (string)DALHelper.HandleDBNull(reader["SubTest"]);
                        ObjTest.IsNumeric = (bool)DALHelper.HandleDBNull(reader["IsNumeric"]);
                        // Refernce Range on 4.05.2016
                        ObjTest.ReferenceRange = (string)DALHelper.HandleDBNull(reader["ReferenceRange"]);
                        //
                        BizActionObj.ResultEntryDetails.TestList.Add(ObjTest);



                    }
                    reader.NextResult();

                    if (BizActionObj.ResultEntryDetails.ItemList == null)
                        BizActionObj.ResultEntryDetails.ItemList = new List<clsPathoTestItemDetailsVO>();

                    while (reader.Read())
                    {
                        clsPathoTestItemDetailsVO ObjItem = new clsPathoTestItemDetailsVO();

                        ObjItem.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        ObjItem.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        ObjItem.BatchID = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                        ObjItem.Quantity = (float)(double)DALHelper.HandleDBNull(reader["IdealQuantity"]);
                        ObjItem.ActualQantity = (float)(double)DALHelper.HandleDBNull(reader["ActualQantity"]);
                        ObjItem.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        ObjItem.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        ObjItem.ExpiryDate = (DateTime?)DALHelper.HandleDBNull(reader["ExpiryDate"]);
                        ObjItem.BalanceQuantity = (float)(double)DALHelper.HandleDBNull(reader["BalQuantity"]);



                        BizActionObj.ResultEntryDetails.ItemList.Add(ObjItem);
                    }
                    reader.NextResult();

                    if (BizActionObj.ResultEntryDetails.TemplateDetails == null)
                        BizActionObj.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();

                    while (reader.Read())
                    {
                        BizActionObj.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();
                        BizActionObj.ResultEntryDetails.TemplateDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


                    }




                    reader.Close();


                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        #endregion

        #region Commented code
        //Code-Harish
        //public override IValueObject AddPathPatientReport(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddPathPatientReportBizActionVO BizAction = valueObject as clsAddPathPatientReportBizActionVO;


        //    try
        //    {
        //        clsPathPatientReportVO objVO = BizAction.OrderPathPatientReportList;

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");



        //        dbServer.AddInParameter(command, "SourceURL", DbType.String, objVO.SourceURL);
        //        dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objVO.PathOrderBookingDetailID);
        //        dbServer.AddInParameter(command, "Report", DbType.Binary, objVO.Report);

        //        dbServer.AddInParameter(command, "Notes", DbType.String, objVO.Notes);
        //        dbServer.AddInParameter(command, "Remarks", DbType.String, objVO.Remarks);
        //        dbServer.AddInParameter(command, "Time", DbType.DateTime, objVO.Time);

        //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objVO.UnitId);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command);

        //        BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        BizAction.OrderPathPatientReportList.ID = (long)dbServer.GetParameterValue(command, "ID");

        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //    return valueObject;
        //}

        #endregion

        public override IValueObject GetPathoTestParameterAndSubTesrForResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestParameterAndSubTestDetailsBizActionVO BizActionObj = valueObject as clsGetPathoTestParameterAndSubTestDetailsBizActionVO;

            #region OLD COde Before Merging
            //try
            //{
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestParameterAndSubTestDetails");


            //    dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
            //    dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);


            //    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            //    if (reader.HasRows)
            //    {
            //        if (BizActionObj.TestList == null)
            //            BizActionObj.TestList = new List<clsPathoTestParameterVO>();


            //        while (reader.Read())
            //        {
            //            clsPathoTestParameterVO objVO = new clsPathoTestParameterVO();

            //            objVO.PathoTestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
            //            objVO.PathoTestName = (string)DALHelper.HandleDBNull(reader["Test"]);
            //            objVO.ParameterName = (string)DALHelper.HandleDBNull(reader["Parameter"]);
            //            objVO.ParameterID = (long)DALHelper.HandleDBNull(reader["ParameterID"]);
            //            objVO.MinValue = (float)(double)DALHelper.HandleDBNull(reader["MinValue"]);
            //            objVO.MaxValue = (float)(double)DALHelper.HandleDBNull(reader["MaxValue"]);
            //            objVO.NormalRange = ((double)DALHelper.HandleDBNull(reader["MinValue"]) + " - " + (double)DALHelper.HandleDBNull(reader["MaxValue"])).ToString();
            //            objVO.DefaultValue = ((double)DALHelper.HandleDBNull(reader["DefaultValue"])).ToString();
            //            objVO.Category = (string)DALHelper.HandleDBNull(reader["Category"]);
            //            objVO.CategoryID = (long?)DALHelper.HandleDBNull(reader["CategoryID"]);
            //            objVO.IsNumeric = (bool)DALHelper.HandleDBNull(reader["IsNumeric"]);


            //            BizActionObj.TestList.Add(objVO);

            //        }
            //        reader.NextResult();

            //        while (reader.Read())
            //        {
            //            BizActionObj.Note = (string)DALHelper.HandleDBNull(reader["Note"]);
            //            BizActionObj.FootNote = (string)DALHelper.HandleDBNull(reader["FootNote"]);
            //        }
            //    }

            //    reader.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            #endregion

            #region Newly Added Code

            try
            {
                if (BizActionObj.IsForResultEntryLog == true)
                {
                    #region Get Result Entry log For Parameters
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_ResultEntryModificationLog");
                    dbServer.AddInParameter(command1, "FromDate", DbType.DateTime, BizActionObj.ParameterDetails.FromDate);
                    dbServer.AddInParameter(command1, "ToDate", DbType.DateTime, BizActionObj.ParameterDetails.ToDate);
                    dbServer.AddInParameter(command1, "FirstName", DbType.String, BizActionObj.ParameterDetails.FirstName);
                    dbServer.AddInParameter(command1, "LastName", DbType.String, BizActionObj.ParameterDetails.LastName);
                    dbServer.AddInParameter(command1, "TestName", DbType.String, BizActionObj.ParameterDetails.PathoTestName);
                    dbServer.AddInParameter(command1, "MrNo", DbType.String, BizActionObj.ParameterDetails.MrNo);
                    dbServer.AddInParameter(command1, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command1, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command1, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command1, "TotalRows", DbType.Int32, int.MaxValue);
                    DbDataReader reader1 = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader1.HasRows)
                    {
                        if (BizActionObj.TestList == null)
                            BizActionObj.TestList = new List<clsPathoTestParameterVO>();
                        while (reader1.Read())
                        {
                            clsPathoTestParameterVO objVO = new clsPathoTestParameterVO();
                            objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ID"]));
                            objVO.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["OrderID"]));
                            objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["UnitID"]));
                            objVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader1["FirstName"]));
                            objVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader1["LastName"]));
                            objVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader1["PatientName"]));
                            objVO.MrNo = Convert.ToString(DALHelper.HandleDBNull(reader1["MrNo"]));
                            objVO.PathoTestName = Convert.ToString(DALHelper.HandleDBNull(reader1["TestName"]));
                            objVO.PathoTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["TestID"]));
                            objVO.PathoSubTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["SubTestID"]));
                            objVO.ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader1["ParameterName"]));
                            objVO.ParameterUnit = Convert.ToString(DALHelper.HandleDBNull(reader1["ParameterUnit"]));
                            objVO.ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader1["ResultValue"]));
                            objVO.DefaultValue = Convert.ToString(DALHelper.HandleDBNull(reader1["DefaultValue"]));
                            objVO.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["IsNumeric"]));
                            objVO.OrderDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader1["OrderDate"]));
                            objVO.NormalRange = Convert.ToString(DALHelper.HandleDBNull(reader1["NormalRange"]));
                            objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["Status"]));
                            objVO.CreatedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["CreatedUnitID"]));
                            objVO.UpdatedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["UpdatedUnitID"]));
                            objVO.AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader1["AddedBy"]));
                            objVO.AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader1["AddedOn"]));
                            objVO.AddedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader1["AddedDateTime"]));
                            objVO.UpdatedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader1["UpdatedDateTime"]));
                            objVO.UpdatedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader1["UpdatedBy"]));
                            objVO.UpdatedOn = Convert.ToString(DALHelper.HandleDBNull(reader1["UpdatedOn"]));
                            objVO.AddedWindowsLoginName = Convert.ToString(DALHelper.HandleDBNull(reader1["AddedWindowsLoginName"]));
                            objVO.UpdateWindowsLoginName = Convert.ToString(DALHelper.HandleDBNull(reader1["UpdateWindowsLoginName"]));
                            objVO.LoginName = Convert.ToString(DALHelper.HandleDBNull(reader1["LoginName"]));
                            objVO.UserName = Convert.ToString(DALHelper.HandleDBNull(reader1["UserName"]));
                            objVO.TestCategory = Convert.ToString(DALHelper.HandleDBNull(reader1["CategoryName"]));
                            // Refernce Range on 4.05.2016
                            objVO.ReferenceRange = (string)DALHelper.HandleDBNull(reader1["ReferenceRange"]);
                            //
                            BizActionObj.TestList.Add(objVO);

                        }
                        reader1.NextResult();
                        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command1, "TotalRows");
                        reader1.Close();
                    }
                    #endregion
                }
                else
                {
                    #region Get PathoTest Parameters
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestParameterAndSubTestDetailsListBySomnath");
                    dbServer.AddInParameter(command, "TestID", DbType.String, BizActionObj.TestID);
                    dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);
                    dbServer.AddInParameter(command, "AgeInDays", DbType.Int64, BizActionObj.AgeInDays);
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.MultipleSampleNo);

                    //by rohini
                    dbServer.AddInParameter(command, "DetailID", DbType.String, BizActionObj.DetailID);

                    //Added by Anumani on 22.02.2016
                    dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.PatientId);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "PatientUnitId", DbType.Int64, BizActionObj.PatientUnitId);
                    //


                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.TestList == null)
                            BizActionObj.TestList = new List<clsPathoTestParameterVO>();
                        while (reader.Read())
                        {
                            clsPathoTestParameterVO objVO = new clsPathoTestParameterVO();

                            objVO.PathoTestID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TestID"]));
                            objVO.PathoTestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                            objVO.PathoSubTestID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["SubTestID"]));
                            objVO.PathoSubTestName = Convert.ToString(DALHelper.HandleDBNull(reader["SubTestDescription"]));
                            objVO.ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["Parameter"]));
                            objVO.ParameterCode = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterCode"]));  //by rohini 9.1.17
                            objVO.ParameterID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ParameterID"]));
                            objVO.ParameterUnit = Convert.ToString(DALHelper.HandleDBNull(reader["Unit"]));
                            objVO.MinValue = (float)(double)DALHelper.HandleDBNull(reader["MinValue"]);
                            objVO.MaxValue = (float)(double)DALHelper.HandleDBNull(reader["MaxValue"]);
                            objVO.NormalRange = ((double)DALHelper.HandleDBNull(reader["MinValue"]) + " - " + (double)DALHelper.HandleDBNull(reader["MaxValue"])).ToString();
                            objVO.DefaultValue = ((double)DALHelper.HandleDBNull(reader["DefaultValue"])).ToString();

                            objVO.ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterValue"]));
                            objVO.Category = Convert.ToString(DALHelper.HandleDBNull(reader["Category"]));
                            objVO.CategoryID = (long?)DALHelper.HandleIntegerNull(reader["CategoryID"]);
                            objVO.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"]));
                            objVO.TestCategory = Convert.ToString(DALHelper.HandleDBNull(reader["TestCategory"]));
                            objVO.TestCategoryID = (long?)DALHelper.HandleDBNull(reader["TestCategoryID"]);
                            objVO.PreviousResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["PreviousResultValue"]));
                            objVO.IsReflexTesting = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReflexTesting"]));
                            objVO.IsMachine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMachine"]));
                            objVO.DeltaCheckDefaultValue = (double)DALHelper.HandleDBNull(reader["DeltaCheck"]);
                            objVO.MinImprobable = (double)DALHelper.HandleDBNull(reader["MinImprobable"]);
                            objVO.MaxImprobable = (double)DALHelper.HandleDBNull(reader["MaxImprobable"]);
                            objVO.HighReffValue = (double)DALHelper.HandleDBNull(reader["HighReffValue"]);
                            objVO.LowReffValue = (double)DALHelper.HandleDBNull(reader["LowReffValue"]);
                            objVO.UpperPanicValue = (double)DALHelper.HandleDBNull(reader["UpperPanicValue"]);
                            objVO.LowerPanicValue = (double)DALHelper.HandleDBNull(reader["LowerpanicValue"]);
                            objVO.LowReflex = (double)DALHelper.HandleDBNull(reader["LowReflex"]);
                            objVO.HighReflex = (double)DALHelper.HandleDBNull(reader["HighReflex"]);
                            objVO.ReferenceRange = ((double)DALHelper.HandleDBNull(reader["LowReffValue"]) + " - " + (double)DALHelper.HandleDBNull(reader["HighReffValue"])).ToString();
                            objVO.ParameterDefaultValueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterDefaultValueId"]));
                            objVO.VaryingReferences = Convert.ToString(DALHelper.HandleDBNull(reader["VaryingReferences"]));
                            objVO.FormulaID = Convert.ToString(DALHelper.HandleDBNull(reader["FormulaID"]));  //by rohini
                            objVO.Formula = Convert.ToString(DALHelper.HandleDBNull(reader["Formula"]));
                            objVO.HelpValue1 = Convert.ToString(DALHelper.HandleDBNull(reader["HelpValue"]));
                            objVO.IsAbnoramal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAbnoramal"]));
                            objVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));  //by rohini for sample no gruping
                            objVO.TestAndSampleNO = Convert.ToString(DALHelper.HandleDBNull(reader["TestAndSampleNO"]));   //By Rohini

                            BizActionObj.TestList.Add(objVO);
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            BizActionObj.Note = Convert.ToString(DALHelper.HandleDBNull(reader["Note"]));
                            BizActionObj.FootNote = Convert.ToString(DALHelper.HandleDBNull(reader["FootNote"]));
                        }
                    }
                    reader.Close();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            #endregion

            return valueObject;

        }

        public override IValueObject GetHelpValuesFroResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetHelpValuesFroResultEntryBizActionVO BizActionObj = valueObject as clsGetHelpValuesFroResultEntryBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetHelpValuesFroResultEntry");


                dbServer.AddInParameter(command, "ParameterID", DbType.Int64, BizActionObj.ParameterID);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.HelpValueList == null)
                        BizActionObj.HelpValueList = new List<clsPathoTestParameterVO>();


                    while (reader.Read())
                    {
                        clsPathoTestParameterVO objVO = new clsPathoTestParameterVO();


                        objVO.ParameterID = (long)DALHelper.HandleDBNull(reader["ParameterID"]);
                        objVO.HelpValueID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.HelpValue = (string)DALHelper.HandleDBNull(reader["HelpValue"]);
                        objVO.HelpValueDefault = (bool)DALHelper.HandleDBNull(reader["IsDefault"]);
                        objVO.IsAbnormal = (bool)DALHelper.HandleDBNull(reader["IsAbnoramal"]);

                        BizActionObj.HelpValueList.Add(objVO);

                    }


                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPathoProfileServiceIDForPathoTestMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO BizActionObj = valueObject as clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoProfileServiceIDForPathoTestMaster");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceDetails == null)
                        BizActionObj.ServiceDetails = new List<clsPathoTestMasterVO>();

                    while (reader.Read())
                    {
                        clsPathoTestMasterVO objVO = new clsPathoTestMasterVO();
                        objVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        BizActionObj.ServiceDetails.Add(objVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;

        }

        //Added by Saily P for Pathology Result Entry
        public override IValueObject AddPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPathoTestResultEntryBizActionVO BizAction = valueObject as clsPathoTestResultEntryBizActionVO;
            try
            {
                clsPathoResultEntryVO objPathoResult = BizAction.PathoResultEntry;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPathoTestResutEntry");
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPathoResult.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPathoResult.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPathoResult.PatientUnitID);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objPathoResult.CategoryID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, objPathoResult.TestID);
                dbServer.AddInParameter(command, "ParameterID", DbType.Int64, objPathoResult.ParameterID);
                dbServer.AddInParameter(command, "LabID", DbType.Int64, objPathoResult.LabID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objPathoResult.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objPathoResult.Time);
                dbServer.AddInParameter(command, "ResultValue", DbType.String, objPathoResult.ResultValue);
                dbServer.AddInParameter(command, "ResultType", DbType.Int64, objPathoResult.ResultType);
                dbServer.AddInParameter(command, "Note", DbType.String, objPathoResult.Note);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPathoResult.Status);

                dbServer.AddInParameter(command, "AttachmentName", DbType.String, objPathoResult.AttachmentFileName);
                dbServer.AddInParameter(command, "Attachment", DbType.Binary, objPathoResult.Attachment);

                //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objPathoResult.UnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objPathoResult.UnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objPathoResult.UnitID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objPathoResult.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPathoResult.AddedDateTime);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));

                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objPathoResult.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //By Anjali
                dbServer.AddInParameter(command, "ParameterUnitId", DbType.Int64, objPathoResult.ParameterUnitID);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPathoResult.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject DeletePathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeletePathoTestResultEntryBizActionVO BizAction = valueObject as clsDeletePathoTestResultEntryBizActionVO;
            try
            {
                clsPathoResultEntryVO objPAthoResutl = BizAction.PathoResultEntry;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeletePathoTestResutEntry");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPAthoResutl.ID);

                int Status = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestResultEntryBizActionVO BizAction = valueObject as clsGetPathoTestResultEntryBizActionVO;
            try
            {
                TimeSpan ts;
                DateTime CurrDate = DateTime.Now.Date;
                DateTime dDate;
                long days;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestResultEntry");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.PatientID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.PathoResultEntry == null)
                        BizAction.PathoResultEntry = new List<clsPathoResultEntryVO>();
                    while (reader.Read())
                    {
                        clsPathoResultEntryVO objVO = new clsPathoResultEntryVO();
                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        objVO.CategoryID = (long)DALHelper.HandleDBNull(reader["CategoryId"]);
                        objVO.TestID = (long)DALHelper.HandleDBNull(reader["TestId"]);
                        objVO.ParameterID = (long)DALHelper.HandleDBNull(reader["ParameterId"]);

                        //By Anjali
                        objVO.ParameterUnitID = (long)DALHelper.HandleDBNull(reader["ParameterUnitId"]);


                        objVO.LabID = (long)DALHelper.HandleDBNull(reader["LabId"]);
                        dDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        objVO.Date = dDate.Date;
                        objVO.sDate = dDate.Date.ToShortDateString();
                        objVO.Time = (DateTime)DALHelper.HandleDBNull(reader["Time"]);
                        objVO.ResultType = (long)DALHelper.HandleDBNull(reader["ResultType"]);
                        objVO.ResultValue = (string)DALHelper.HandleDBNull(reader["ResultValue"]);
                        objVO.Note = (string)DALHelper.HandleDBNull(reader["Notes"]);

                        objVO.ParameterName = (string)DALHelper.HandleDBNull(reader["ParameterName"]);

                        //By Anjali
                        objVO.ParameterUnitName = (string)(DALHelper.HandleDBNull(reader["ParameterUnitName"]));

                        objVO.LabName = (string)DALHelper.HandleDBNull(reader["LabName"]);
                        objVO.ResultTypeName = (string)DALHelper.HandleDBNull(reader["ResultTypeName"]);
                        objVO.Category = (string)DALHelper.HandleDBNull(reader["CategoryName"]);
                        objVO.Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                        objVO.AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);

                        ts = CurrDate - dDate;
                        days = ts.Days;
                        objVO.EllapsedTime = days + " Days";
                        //objVO.ParameterID = (long)DALHelper.HandleDBNull(reader["ParameterID"]);
                        //objVO.EllapsedTime= 
                        BizAction.PathoResultEntry.Add(objVO);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetPathoTestForresultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestListForResultEntryBizActionVO BizActionObj = valueObject as clsGetPathoTestListForResultEntryBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestListForResultEntry");

                dbServer.AddInParameter(command, "ApplicableTo", DbType.String, BizActionObj.ApplicaleTo);
                dbServer.AddInParameter(command, "CategoryId", DbType.Int64, BizActionObj.Category);
                //dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsPathoTestMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO objPathoTestVO = new clsPathoTestMasterVO();
                        objPathoTestVO.ID = (long)reader["ID"];
                        //objPathoTestVO.UnitID = (long)reader["UnitID"];
                        objPathoTestVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objPathoTestVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        //objPathoTestVO.TurnAroundTime = (string)DALHelper.HandleDBNull(reader["TurnAroundTime"]);
                        //objPathoTestVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        //objPathoTestVO.CategoryID = (long)reader["CategoryID"];
                        //objPathoTestVO.ServiceID = (long)reader["ServiceID"];
                        objPathoTestVO.TestPrintName = (string)DALHelper.HandleDBNull(reader["TestPrintName"]);
                        //objPathoTestVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.TestList.Add(objPathoTestVO);
                    }


                }
                reader.NextResult();

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        // BY BHUSHAN . . . . . . . 
        public override IValueObject GetResultOnParameterSelection(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetResultOnParameterSelectionBizActionVO BizActionObj = valueObject as clsGetResultOnParameterSelectionBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetResultOnParameterSelection");
                dbServer.AddInParameter(command, "ParamID", DbType.Int64, BizActionObj.ParamID);
                dbServer.AddInParameter(command, "Gender", DbType.String, BizActionObj.Gender);
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, BizActionObj.DOB);
                //  dbServer.AddInParameter(command, "resultValue", DbType.Double, BizActionObj.resultValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PathoResultEntry == null)
                        BizActionObj.PathoResultEntry = new List<clsPathoResultEntryVO>();
                    while (reader.Read())
                    {
                        clsPathoResultEntryVO objPathoTestVO = new clsPathoResultEntryVO();

                        objPathoTestVO.MinValue = (double)DALHelper.HandleDBNull(reader["MinValue"]);
                        objPathoTestVO.MaxValue = (double)DALHelper.HandleDBNull(reader["MaxValue"]);
                        objPathoTestVO.DefaultValue = (double)DALHelper.HandleDBNull(reader["DefaultValue"]);
                        BizActionObj.PathoResultEntry.Add(objPathoTestVO);

                    }
                    reader.NextResult();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return BizActionObj;
        }

        //By Anjali.....
        public override IValueObject GetPathoTestResultEntryDateWise(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestResultEntryDateWiseBizActionVO BizAction = valueObject as clsGetPathoTestResultEntryDateWiseBizActionVO;
            try
            {
                TimeSpan ts;
                DateTime CurrDate = DateTime.Now.Date;
                DateTime dDate;
                long days;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTestResultEntryDateWise");
                dbServer.AddInParameter(command, "Date", DbType.Date, BizAction.Date);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "fromform", DbType.Int64, BizAction.fromform);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.PathoResultEntry == null)
                        BizAction.PathoResultEntry = new List<clsPathoResultEntryVO>();
                    while (reader.Read())
                    {
                        clsPathoResultEntryVO objVO = new clsPathoResultEntryVO();
                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        objVO.CategoryID = (long)DALHelper.HandleDBNull(reader["CategoryId"]);
                        objVO.TestID = (long)DALHelper.HandleDBNull(reader["TestId"]);
                        objVO.ParameterID = (long)DALHelper.HandleDBNull(reader["ParameterId"]);
                        objVO.ParameterUnitID = (long)DALHelper.HandleDBNull(reader["ParameterUnitId"]);
                        objVO.LabID = (long)DALHelper.HandleDBNull(reader["LabId"]);
                        dDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        objVO.Date = dDate.Date;
                        objVO.sDate = dDate.Date.ToShortDateString();
                        objVO.Time = (DateTime)DALHelper.HandleDBNull(reader["Time"]);
                        objVO.ResultType = (long)DALHelper.HandleDBNull(reader["ResultType"]);
                        objVO.ResultValue = (string)DALHelper.HandleDBNull(reader["ResultValue"]);
                        objVO.Note = (string)DALHelper.HandleDBNull(reader["Notes"]);
                        objVO.ParameterName = (string)DALHelper.HandleDBNull(reader["ParameterName"]);
                        objVO.ParameterUnitName = (string)(DALHelper.HandleDBNull(reader["ParameterUnitName"]));
                        objVO.LabName = (string)DALHelper.HandleDBNull(reader["LabName"]);
                        objVO.ResultTypeName = (string)DALHelper.HandleDBNull(reader["ResultTypeName"]);
                        objVO.Category = (string)DALHelper.HandleDBNull(reader["CategoryName"]);
                        objVO.Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                        objVO.AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);

                        objVO.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"]));

                        ts = CurrDate - dDate;
                        days = ts.Days;
                        objVO.EllapsedTime = days + " Days";
                        BizAction.PathoResultEntry.Add(objVO);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        #region For Pathology Additions

        public override IValueObject GetAgencyApplicableUnitList(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsGetPathoAgencyApplicableUnitListBizActionVO BizActionObj = valueObject as clsGetPathoAgencyApplicableUnitListBizActionVO;
                BizActionObj.ServiceAgencyMasterDetails = new List<clsServiceAgencyMasterVO>();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceApplicableUnitDetails");

                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceId);
                dbServer.AddInParameter(command, "ApplicableUnitID", DbType.Int64, BizActionObj.ApplicableUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                while (reader.Read())
                {
                    clsServiceAgencyMasterVO ObjItem = new clsServiceAgencyMasterVO();
                    ObjItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    ObjItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    ObjItem.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                    ObjItem.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                    ObjItem.ApplicableUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableUnitID"]));
                    ObjItem.AgencyTestName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyTestName"]));
                    ObjItem.Rate = (float)Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                    ObjItem.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                    ObjItem.AgencyCode = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyCode"]));
                    ObjItem.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                    ObjItem.IsDefaultAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefaultAgency"]));
                    ObjItem.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                    BizActionObj.ServiceAgencyMasterDetails.Add(ObjItem);
                }

                reader.Close();

                return BizActionObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetPathOrderBookingReportDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderBookingDetailReportDetailsBizActionVO BizActionObj = valueObject as clsGetPathOrderBookingDetailReportDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathOrderBookingReportDetail");

                dbServer.AddInParameter(command, "PathOrderBookingDetailsID", DbType.Int64, BizActionObj.OrderBookingDetail.ID);
                dbServer.AddInParameter(command, "PathOrderBookingDetailsUnitID", DbType.Int64, BizActionObj.OrderBookingDetail.UnitId);

                DbDataReader reader;


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.OrderBookingDetailList == null)
                        BizActionObj.OrderBookingDetail = new clsPathOrderBookingDetailVO();
                    while (reader.Read())
                    {
                        BizActionObj.OrderBookingDetail.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                        BizActionObj.OrderBookingDetail.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);

                    }

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject UpdateEmailDeliveryStatusinPathDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePathOrderBookingDetailDeliveryStatusBizActionVO objItem = valueObject as clsUpdatePathOrderBookingDetailDeliveryStatusBizActionVO;
            if (objItem.PathOrderBookList.Count > 0)
            {
                try
                {

                    List<clsPathOrderBookingDetailVO> objItemVO = objItem.PathOrderBookList;
                    int count = objItemVO.Count;
                    DbCommand command = null;

                    for (int i = 0; i < count; i++)
                    {

                        //   clsPathOrderBookingDetailVO objItemVO = objItem.PathOrderBookingDetail;
                        command = dbServer.GetStoredProcCommand("CIMS_UpdateEmailDeliveryStatusinPathDetails");
                        dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objItemVO[i].ID);
                        dbServer.AddInParameter(command, "OrderID", DbType.Int64, objItemVO[i].OrderBookingID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO[i].UnitId);
                        dbServer.AddInParameter(command, "IsDeliverdthroughEmail", DbType.Boolean, objItemVO[i].IsDeliverdthroughEmail);
                        dbServer.AddInParameter(command, "EmailDeliverdDateTime", DbType.DateTime, null);


                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);

                        objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return objItem;
        }

        public override IValueObject AddPathPatientReportToGetEmail(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsAddPathPatientReportDetailsForEmailSendingBizActionVO objItem = valueObject as clsAddPathPatientReportDetailsForEmailSendingBizActionVO;
            clsPathPatientReportVO objVO = objItem.OrderPathPatientReportList;

            if (objItem.PathOrderBookList.Count > 0)
            {
                try
                {
                    try
                    {
                        con = dbServer.CreateConnection();
                        con.Open();
                        trans = con.BeginTransaction();

                        List<clsPathOrderBookingDetailVO> objDetailVO = objItem.PathOrderBookList;
                        int count = objDetailVO.Count;
                        DbCommand command = null;

                        for (int i = 0; i < count; i++)
                        {
                            command = dbServer.GetStoredProcCommand("CIMS_AddPathEmailReportDeliveryDetails");
                            command.Connection = con;
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailVO[i].UnitId);
                            dbServer.AddInParameter(command, "OrderID", DbType.Int64, objDetailVO[i].OrderBookingID);
                            dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objDetailVO[i].ID);
                            dbServer.AddInParameter(command, "PathPatientReportID", DbType.Int64, objDetailVO[i].PathPatientReportID);
                            dbServer.AddInParameter(command, "PatientEmailID", DbType.String, objItem.PatientEmailID);
                            dbServer.AddInParameter(command, "DoctorEmailID", DbType.String, objItem.DoctorEmailID);
                            dbServer.AddInParameter(command, "Status", DbType.Int64, false);
                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command, "PatientID", DbType.Int64, objItem.PatientID);
                            dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objItem.PatientUnitID);
                            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);
                            dbServer.AddParameter(command, "AttachmentID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.AttachmentID);
                            int intStatus = dbServer.ExecuteNonQuery(command, trans);
                            objItem.AttachmentID = (long)dbServer.GetParameterValue(command, "AttachmentID");
                        }
                        trans.Commit();
                        con.Close();
                    }

                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objItem.PathOrderBookingDetailList = null;
                        objItem.OrderPathPatientReportList = null;
                    }

                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con = null;
                    trans = null;
                }
            }
            return objItem;
        }

        #endregion

        #region Newly Added Methods

        public override IValueObject FillTemplateComboBoxInPathoResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            FillTemplateComboBoxInPathoResultEntryBizActionVO BizAction = (FillTemplateComboBoxInPathoResultEntryBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillTemplateComboBoxInPathoResultEntry");

                DbDataReader reader;
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizAction.TestID);
                dbServer.AddInParameter(command, "Pathologist", DbType.Int64, BizAction.Pathologist);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizAction.GenderID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), Convert.ToDouble((int)reader["IsFormTemplate"])));
                        BizAction.IsFormTemplate = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsFormTemplate"]));
                    }
                }
            }


            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return BizAction;
        }

        public override IValueObject GetFinalizedParameter(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoFinalizedEntryBizActionVO BizActionObj = valueObject as clsGetPathoFinalizedEntryBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPAthoResultEntryRushabh");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.String, BizActionObj.DetailID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ResultList == null)
                        BizActionObj.ResultList = new List<clsPathPatientReportVO>();
                    while (reader.Read())
                    {
                        clsPathPatientReportVO Obj = new clsPathPatientReportVO();
                        Obj.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        Obj.PathOrderBookingDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathOrderBookingDetailID"]));
                        Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                        Obj.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Obj.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        Obj.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        Obj.SampleCollectionTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectionTime"]);
                        Obj.PathologistID1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"]));
                        Obj.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"]));
                        Obj.ResultAddedDateTime = (DateTime?)DALHelper.HandleDate(reader["ResultAddedTime"]);
                        Obj.SampleReceiveDateTime = (DateTime?)DALHelper.HandleDate(reader["SampleReceivedDateTime"]);
                        Obj.IsFinalized = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFinalized"]));
                        Obj.BillID = Convert.ToInt64(DALHelper.HandleBoolDBNull(reader["BillId"]));
                        Obj.BillNo = Convert.ToString(DALHelper.HandleBoolDBNull(reader["BillNo"]));
                        BizActionObj.ResultList.Add(Obj);
                        
                    }
                    reader.NextResult();

                    if (BizActionObj.ResultEntryDetails.TestList == null)
                        BizActionObj.ResultEntryDetails.TestList = new List<clsPathoTestParameterVO>();

                    while (reader.Read())
                    {
                        clsPathoTestParameterVO ObjTest = new clsPathoTestParameterVO();
                        ObjTest.PathoTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        ObjTest.PathoSubTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubTestID"]));
                        ObjTest.ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"]));
                        ObjTest.ParameterCode = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterCode"]));  //by rohini 6.1.17
                        ObjTest.ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"]));
                        ObjTest.Category = Convert.ToString(DALHelper.HandleDBNull(reader["Category"]));
                        ObjTest.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                        ObjTest.ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"]));
                        ObjTest.ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"]));
                        ObjTest.ParameterUnit = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterUnit"]));
                        ObjTest.Print = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterPrintName"]));
                        ObjTest.ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ResultValue"]));
                        ObjTest.DefaultValue = Convert.ToString(DALHelper.HandleDBNull(reader["DefaultValue"]));
                        ObjTest.NormalRange = Convert.ToString(DALHelper.HandleDBNull(reader["NormalRange"]));
                        ObjTest.Note = Convert.ToString(DALHelper.HandleDBNull(reader["SuggetionNote"]));
                        ObjTest.FootNote = Convert.ToString(DALHelper.HandleDBNull(reader["FootNote"]));
                        ObjTest.PathoSubTestName = Convert.ToString(DALHelper.HandleDBNull(reader["SubTest"]));
                        ObjTest.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"]));
                        ObjTest.PathoTestName = Convert.ToString(DALHelper.HandleDBNull(reader["Test"]));
                        ObjTest.TestCategory = Convert.ToString(DALHelper.HandleDBNull(reader["TestCategory"]));
                        ObjTest.TestCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestCategoryID"]));
                        ObjTest.IsReflexTesting = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReflexTesting"]));
                        ObjTest.MinImprobable = (double)DALHelper.HandleDBNull(reader["MinImprobable"]);
                        ObjTest.MaxImprobable = (double)DALHelper.HandleDBNull(reader["MaxImprobable"]);
                        ObjTest.DeltaCheckDefaultValue =(double)DALHelper.HandleDBNull(reader["DeltaCheck"]);
                        ObjTest.HighReffValue = (double)DALHelper.HandleDBNull(reader["HighReffValue"]);
                        ObjTest.LowReffValue = (double)DALHelper.HandleDBNull(reader["LowReffValue"]);
                        ObjTest.UpperPanicValue = (double)DALHelper.HandleDBNull(reader["UpperPanicValue"]);
                        ObjTest.LowerPanicValue = (double)DALHelper.HandleDBNull(reader["LowerPanicValue"]);
                        ObjTest.LowReflex = (double)DALHelper.HandleDBNull(reader["LowReflex"]);
                        ObjTest.HighReflex = (double)DALHelper.HandleDBNull(reader["HighReflex"]);
                        ObjTest.ReferenceRange = ((double)DALHelper.HandleDBNull(reader["LowReffValue"]) + " - " + (double)DALHelper.HandleDBNull(reader["HighReffValue"])).ToString();
                        //ObjTest.ReferenceRange = Convert.ToString(DALHelper.HandleDBNull(reader["ReferenceRange"]));
                        ObjTest.IsAbnormal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAbnormal"]));
                        ObjTest.DeltaCheckValue = (double)DALHelper.HandleDBNull(reader["DeltaCheckValue"]);
                        ObjTest.ParameterDefaultValueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterDefaultValueId"]));
                        ObjTest.VaryingReferences = Convert.ToString(DALHelper.HandleDBNull(reader["VaryingReferences"]));
                        ObjTest.Formula = Convert.ToString(DALHelper.HandleDBNull(reader["Formula"]));
                        ObjTest.FormulaID = Convert.ToString(DALHelper.HandleDBNull(reader["FormulaID"])); //By Rohini
                        ObjTest.IsMachine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMachine"]));
                        ObjTest.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"])); //By Rohini
                        ObjTest.TestAndSampleNO = Convert.ToString(DALHelper.HandleDBNull(reader["TestAndSampleNO"]));   //By Rohini

                        ObjTest.IsSecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSecondLevel"]));   //By Rohini
                        
                        BizActionObj.ResultEntryDetails.TestList.Add(ObjTest);

                    }
                    reader.NextResult();

                    if (BizActionObj.ResultEntryDetails.ItemList == null)
                        BizActionObj.ResultEntryDetails.ItemList = new List<clsPathoTestItemDetailsVO>();

                    while (reader.Read())
                    {
                        clsPathoTestItemDetailsVO ObjItem = new clsPathoTestItemDetailsVO();

                        ObjItem.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        ObjItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        ObjItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        ObjItem.Quantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["IdealQuantity"]));
                        ObjItem.ActualQantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["ActualQantity"]));
                        ObjItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        ObjItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        ObjItem.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExpiryDate"]));
                        ObjItem.BalanceQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BalQuantity"]));
                        BizActionObj.ResultEntryDetails.ItemList.Add(ObjItem);
                    }

                    reader.NextResult();

                    if (BizActionObj.ResultEntryDetails.TemplateDetails == null)
                        BizActionObj.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();

                    while (reader.Read())
                    {
                        BizActionObj.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();
                        BizActionObj.ResultEntryDetails.TemplateDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        BizActionObj.ResultEntryDetails.TemplateDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


                    }
                    reader.Close();


                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject ViewPathoTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoViewTemplateBizActionVO BizActionObj = (clsGetPathoViewTemplateBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ViewPathoTemplate");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "Flag", DbType.Int64, BizActionObj.Flag);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    if (BizActionObj.Template == null)
                        BizActionObj.Template = new clsPathoTestTemplateDetailsVO();
                    while (reader.Read())
                    {
                        BizActionObj.Template.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.Template.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        BizActionObj.Template.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject ApprovePathPatientReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApprovePathPatientReortBizActionVO objItem = valueObject as clsApprovePathPatientReortBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateApprovalStatusForTests");
                dbServer.AddInParameter(command, "OrderDetailsID", DbType.String, objItem.OrderDetailsID);
                dbServer.AddInParameter(command, "DoctorUserID", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "IsSecondLevelApproval", DbType.Boolean, objItem.IsSecondLevelApproval);
                dbServer.AddInParameter(command, "IsThirdLevelApproval", DbType.Boolean, objItem.IsThirdLevelApproval);
                dbServer.AddInParameter(command, "IsCheckedResults", DbType.Boolean, objItem.IsForCheckResults);
                dbServer.AddInParameter(command, "ThirdLevelCheckResult", DbType.Boolean, objItem.IsThirdLevelCheckResult);
                dbServer.AddInParameter(command, "CheckResultValueMessage", DbType.String, objItem.checkResultValueMessage);
                dbServer.AddInParameter(command, "IsApprove", DbType.Boolean, true);
                dbServer.AddInParameter(command, "IsDigitalSignatureRequired", DbType.Boolean, objItem.IsDigitalSignatureRequired);
                dbServer.AddInParameter(command, "ApprovedBy", DbType.Int64, UserVo.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.UserName);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                if (objItem.TestList != null && objItem.TestList.Count > 0)
                {
                    foreach (clsPathoTestParameterVO item in objItem.TestList.ToList())
                    {
                        DbCommand command1 = null;
                        command1 = dbServer.GetStoredProcCommand("CIMS_UpdateNotes");
                        dbServer.AddInParameter(command1, "OrderID", DbType.Int64, item.OrderID);
                        dbServer.AddInParameter(command1, "TestID", DbType.Int64, item.PathoTestID);
                        dbServer.AddInParameter(command1, "FootNote", DbType.String, item.FootNote);
                        dbServer.AddInParameter(command1, "SuggestionNote", DbType.String, item.Note);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    }
                }
            }
            catch
            {

            }
            return objItem;
        }

        public override IValueObject GetPathoResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPathoResultEntryPrintDetailsBizActionVO BizActionObj = valueObject as clsPathoResultEntryPrintDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_rpt_PathoTemplateResultEntry1");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IsDelivered", DbType.Int64, BizActionObj.IsDelivered);
                dbServer.AddInParameter(command, "IsOpdIpd", DbType.Int64, BizActionObj.IsOpdIpd);
                dbServer.AddInParameter(command, "OrderUnitID", DbType.Int64, BizActionObj.OrderUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ResultDetails == null)
                        BizActionObj.ResultDetails = new clsPathoResultEntryPrintDetailsVO();
                    while (reader.Read())
                    {
                        BizActionObj.ResultDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ResultDetails.Salutation = Convert.ToString(DALHelper.HandleDBNull(reader["Salutation"]));
                        BizActionObj.ResultDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        BizActionObj.ResultDetails.ResultAddedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ResultAddedTime"]));
                        BizActionObj.ResultDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ResultDetails.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        BizActionObj.ResultDetails.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                        BizActionObj.ResultDetails.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                        BizActionObj.ResultDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        BizActionObj.ResultDetails.Pathologist = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"]));
                        BizActionObj.ResultDetails.TestTemplate = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedTemplate"]));
                        BizActionObj.ResultDetails.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        BizActionObj.ResultDetails.TemplateId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateId"]));
                        BizActionObj.ResultDetails.Test = Convert.ToString(DALHelper.HandleDBNull(reader["Test"]));
                        BizActionObj.ResultDetails.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"]));
                        //  BizActionObj.ResultDetails.ShowinPathoReport = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["ShowinReport"]));
                        BizActionObj.ResultDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        BizActionObj.ResultDetails.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        BizActionObj.ResultDetails.SampleCollectionTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SampleCollectionTime"]));
                        BizActionObj.ResultDetails.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        BizActionObj.ResultDetails.AgeYear = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeYear"]));
                        BizActionObj.ResultDetails.AgeMonth = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeMonth"]));
                        BizActionObj.ResultDetails.AgeDate = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeDate"]));
                        BizActionObj.ResultDetails.PathoCategory = Convert.ToString(DALHelper.HandleDBNull(reader["PathoCategory"]));
                        BizActionObj.ResultDetails.Pathologist1 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist1"]));
                        BizActionObj.ResultDetails.Signature1 = (byte[])(DALHelper.HandleDBNull(reader["DigitalSignature"]));
                        BizActionObj.ResultDetails.PathoDoctorid1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        //   BizActionObj.ResultDetails.Roles = Convert.ToString(DALHelper.HandleDBNull(reader["Roles"]));

                        BizActionObj.ResultDetails.PatientCategory = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategory"]));
                        BizActionObj.ResultDetails.PatientSource = Convert.ToString(DALHelper.HandleDBNull(reader["PatientSource"]));
                        BizActionObj.ResultDetails.Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                        BizActionObj.ResultDetails.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo"]));

                        BizActionObj.ResultDetails.DonarCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonarCode"]));
                        BizActionObj.ResultDetails.ReferenceNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReferenceNo"]));                      
                        
                        //ApprovedDateTime	GeneratedDateTime	SubOptimalRemark	Authorizedby	Disclaimer
                        BizActionObj.ResultDetails.ApprovedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ApprovedDateTime"]));
                        BizActionObj.ResultDetails.GeneratedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["GeneratedDateTime"]));

                        BizActionObj.ResultDetails.IsSubOptimal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSubOptimal"]));
                        

                        BizActionObj.ResultDetails.SubOptimalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SubOptimalRemark"]));
                        BizActionObj.ResultDetails.Authorizedby = Convert.ToString(DALHelper.HandleDBNull(reader["Authorizedby"]));
                        BizActionObj.ResultDetails.Disclaimer = Convert.ToString(DALHelper.HandleDBNull(reader["Disclaimer"]));

                        BizActionObj.ResultDetails.UnitName = Convert.ToString((reader["ClinicName"]));
                        BizActionObj.ResultDetails.AdressLine1 = Convert.ToString((reader["address"]));
                        BizActionObj.ResultDetails.AddressLine2 = Convert.ToString((reader["AddressLine2"]));
                        BizActionObj.ResultDetails.AddressLine3 = Convert.ToString((reader["AddressLine3"]));
                        BizActionObj.ResultDetails.Email = Convert.ToString((reader["Email"]));
                        BizActionObj.ResultDetails.PinCode = Convert.ToString((reader["PinCode"]));
                        BizActionObj.ResultDetails.TinNo = Convert.ToString((reader["TinNo"]));
                        BizActionObj.ResultDetails.RegNo = Convert.ToString((reader["RegNo"]));
                        BizActionObj.ResultDetails.City = Convert.ToString((reader["City"]));
                        BizActionObj.ResultDetails.UnitContactNo = Convert.ToString((reader["MobileNO"]));
                        BizActionObj.ResultDetails.MobileNo = Convert.ToString((reader["MobNo"]));

                        //BizActionObj.ResultDetails.UnitLogo = (Byte[])(DALHelper.HandleDBNull(reader["Logo"]));
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {

                        BizActionObj.ResultDetails.UnitLogo = (Byte[])(DALHelper.HandleDBNull(reader["Logo"]));
                        BizActionObj.ResultDetails.DisclaimerImg = (Byte[])(DALHelper.HandleDBNull(reader["DisImg"]));
                        
                    }
                    int RowCnt = 0;

                    while (reader.Read())
                    {
                        RowCnt = RowCnt + 1;

                        if (RowCnt == 1)
                        {
                            BizActionObj.ResultDetails.Pathologist1 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                            // BizActionObj.ResultDetails.Signature1 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                            BizActionObj.ResultDetails.Education1 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                            BizActionObj.ResultDetails.PathoDoctorid1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                        }

                        if (RowCnt == 2)
                        {
                            BizActionObj.ResultDetails.Pathologist2 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                            //  BizActionObj.ResultDetails.Signature2 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                            BizActionObj.ResultDetails.Education2 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                            BizActionObj.ResultDetails.PathoDoctorid2 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                        }

                        if (RowCnt == 3)
                        {
                            BizActionObj.ResultDetails.Pathologist3 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                            // BizActionObj.ResultDetails.Signature3 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                            BizActionObj.ResultDetails.Education3 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                            BizActionObj.ResultDetails.PathoDoctorid3 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                        }

                        if (RowCnt == 4)
                        {
                            BizActionObj.ResultDetails.Pathologist4 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                            // BizActionObj.ResultDetails.Signature4 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                            BizActionObj.ResultDetails.Education4 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                            BizActionObj.ResultDetails.PathoDoctorid4 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                        }

                    }

                    reader.Close();

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;

        }







        #endregion


        #region Machine Parameters

        public override IValueObject GetMachineParameterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoMachineParameterBizActionVO BizActionObj = valueObject as clsGetPathoMachineParameterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMachineParameterList");

                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                dbServer.AddInParameter(command, "MachineID", DbType.Int64, BizActionObj.MachineID);
                //dbServer.AddInParameter(command, "GenderID", DbType.String, BizActionObj.MachineName);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsMachineParameterMasterVO>();
                    while (reader.Read())
                    {
                        clsMachineParameterMasterVO objDetails = new clsMachineParameterMasterVO();
                        objDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDetails.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objDetails.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objDetails.ParameterDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objDetails.MachineId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineID"]));
                        objDetails.MachineName = Convert.ToString(DALHelper.HandleDBNull(reader["MachineMaster"]));
                        objDetails.Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freeze"]));
                        objDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.DetailsList.Add(objDetails);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {

            }
            return BizActionObj;
        }

        public override IValueObject AddUpdatePathoMachineParameter(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePathoMachineParameterBizActionVO BizActionObj = valueObject as clsAddUpdatePathoMachineParameterBizActionVO;
            try
            {
                clsMachineParameterMasterVO objBizAction = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateMachineParameter");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objBizAction.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objBizAction.ParameterDesc);
                dbServer.AddInParameter(command, "MachineID", DbType.Int64, objBizAction.MachineId);
                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, objBizAction.Freezed);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBizAction.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception)
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetParameterLinkingList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetParameterLinkingBizActionVO BizActionObj = valueObject as clsGetParameterLinkingBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetParameterLinkingList");

                dbServer.AddInParameter(command, "ParameterID", DbType.Int64, BizActionObj.ParameterID);
                dbServer.AddInParameter(command, "MachineParameterID", DbType.Int64, BizActionObj.MachineParaID);
                dbServer.AddInParameter(command, "MachineID", DbType.Int64, BizActionObj.MachineID);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsParameterLinkingVO>();
                    while (reader.Read())
                    {
                        clsParameterLinkingVO objDetails = new clsParameterLinkingVO();
                        objDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDetails.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objDetails.MachineParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineParameterId"]));
                        objDetails.ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"]));
                        objDetails.MachineParameter = Convert.ToString(DALHelper.HandleDBNull(reader["MachineParaDescription"]));
                        objDetails.ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParaDescription"]));
                        objDetails.Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freeze"]));
                        objDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objDetails.MachineName = Convert.ToString(DALHelper.HandleDBNull(reader["MachineName"]));
                        //by rohini dated 18.1.16
                        objDetails.MachineID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineID"]));
                        BizActionObj.DetailsList.Add(objDetails);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }


        //ADDED BY ROHINI DATED 12.2.16

        public override IValueObject GetTestDetailsByTestID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderTestDetailListBizActionVO BizActionObj = valueObject as clsGetPathOrderTestDetailListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailByTestID");

                dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                dbServer.AddInParameter(command, "OrderDetailID", DbType.Int64, BizActionObj.OrderDetailID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();
                        objOrderBookingVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objOrderBookingVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objOrderBookingVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objOrderBookingVO.TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                        objOrderBookingVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        objOrderBookingVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));


                        objOrderBookingVO.SampleCollectedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]);
                        objOrderBookingVO.SampleCollectedTime = ((DateTime?)DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]));
                        objOrderBookingVO.SampleCollectionCenter = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCollectionCenter"]));
                        objOrderBookingVO.CollectionCenter = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionCenter"]));
                        objOrderBookingVO.SampleCollectedBy = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCollectedBy"]));
                        objOrderBookingVO.FastingStatusHrs = Convert.ToInt16(DALHelper.HandleDBNull(reader["FastingStatusHrs"]));
                        objOrderBookingVO.Gestation = Convert.ToString(DALHelper.HandleDBNull(reader["Gestation"]));

                        objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);
                        objOrderBookingVO.SampleReceivedTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);
                        objOrderBookingVO.SampleReceiveBy = Convert.ToString(DALHelper.HandleDBNull(reader["SampleReceiveBy"]));


                        objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);
                        objOrderBookingVO.SampleDispatchTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]);
                        objOrderBookingVO.DispatchBy = Convert.ToString(DALHelper.HandleDBNull(reader["SampleDispatchBy"]));
                        objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                        objOrderBookingVO.DispatchToName = Convert.ToString(DALHelper.HandleDBNull(reader["DispatchToName"]));


                        objOrderBookingVO.SampleRejectionDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);
                        objOrderBookingVO.SampleRejectionTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleRejectDateTime"]);

                        objOrderBookingVO.SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"]));
                        objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);
                        objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);

                        objOrderBookingVO.SampleAcceptanceDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                        objOrderBookingVO.SampleAcceptanceTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]);
                        objOrderBookingVO.AcceptedOrRejectedByName = Convert.ToString(DALHelper.HandleDBNull(reader["AcceptedOrRejectedByName"]));
                        objOrderBookingVO.AcceptedOrRejectedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AcceptedOrRejectedByID"]));
                        objOrderBookingVO.CollectionName = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionName"]));
                        objOrderBookingVO.CollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionID"]));
                        objOrderBookingVO.IsSubOptimal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSubOptimal"]));
                        objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));

                        objOrderBookingVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));

                        objOrderBookingVO.HandDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]);
                        objOrderBookingVO.EmailDeliverdDateTime = (DateTime?)DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]);


                        objOrderBookingVO.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtAgencyID"]));
                        objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));

                        //objOrderBookingVO.ResultEntryBy = Convert.ToString(DALHelper.HandleDBNull(reader["ResultEntryBy"]));                      
                        //objOrderBookingVO.ResultDateTime = (DateTime?)DALHelper.HandleDBNull(reader["ResultDateTime"]);

                        //objOrderBookingVO.ApproveBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveBy"]));
                        //objOrderBookingVO.ADateTime = (DateTime?)DALHelper.HandleDBNull(reader["ADateTime"]);

                        //hISTORY dETAILS
                        //objOrderBookingVO.Reason = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                        //objOrderBookingVO.DateTimeNow = (DateTime)DALHelper.HandleDBNull(reader["DateTimeNow"]);
                        //objOrderBookingVO.UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));

                        BizActionObj.CollectionOrderBookingDetailList.Add(objOrderBookingVO);

                    }
                }
                reader.Close();
            }                
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailOfResultEntryEditByTestID");

                dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();


                        //hISTORY dETAILS
                        objOrderBookingVO.Reason = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                        objOrderBookingVO.DateTimeNow = (DateTime?)DALHelper.HandleDBNull(reader["DateTimeNow"]);
                        objOrderBookingVO.UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));

                        BizActionObj.ReslutEntryEditList.Add(objOrderBookingVO);

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailOfResultByTestID");

                dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();


                        //hISTORY dETAILS

                        objOrderBookingVO.ResultEntryBy = Convert.ToString(DALHelper.HandleDBNull(reader["ResultEntryBy"]));
                        objOrderBookingVO.ResultDateTime = (DateTime?)DALHelper.HandleDBNull(reader["ResultDateTime"]);

                        BizActionObj.ResultOrderBookingDetailList.Add(objOrderBookingVO);

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailOfAuthorizationByTestID");

                dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();


                        objOrderBookingVO.ApproveBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveBy"]));
                        objOrderBookingVO.ADateTime = (DateTime?)DALHelper.HandleDBNull(reader["ADateTime"]);


                        BizActionObj.AuthorizedOrderBookingDetailList.Add(objOrderBookingVO);

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }
        //
        public override IValueObject AddUpdateParameterLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateParameterLinkingBizActionVO BizActionObj = valueObject as clsAddUpdateParameterLinkingBizActionVO;
            try
            {
                clsParameterLinkingVO objBizAction = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateParameterLinking");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);
                //dbServer.AddInParameter(command, "Code", DbType.String, objBizAction.MachineParameter);
                //dbServer.AddInParameter(command, "Code", DbType.String, objBizAction.ParameterName);
                dbServer.AddInParameter(command, "MachineParameterID", DbType.String, objBizAction.MachineParameterID);
                //by rohini dated 18.1.16
                dbServer.AddInParameter(command, "MachineID", DbType.String, objBizAction.MachineID);

                dbServer.AddInParameter(command, "ParameterID", DbType.Int64, objBizAction.ParameterID);
                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, objBizAction.Freezed);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objBizAction.CreatedUnitId); //UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objBizAction.AddedBy);//UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objBizAction.AddedOn); //UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objBizAction.AddedWindowsLoginName); //UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBizAction.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateStatusParameterLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusParameterLinkingBizActionVO BizActionObj = valueObject as clsUpdateStatusParameterLinkingBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("UpdateStatusParameterLinkingStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ApplicationParameterID", DbType.Int64, BizActionObj.AppParameterID);
                dbServer.AddInParameter(command, "MachineParameterID", DbType.Int64, BizActionObj.MachineParameterID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.Status);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateStatusMachineParameterMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusMachineParameterBizActionVO BizActionObj = valueObject as clsUpdateStatusMachineParameterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("UpdateStatusMachineParameter");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "MachineID", DbType.Int64, BizActionObj.MachineID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.Status);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetPathoTemplate(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetPathoTemplateMasterBizActionVO BizActionObj = valueObject as clsGetPathoTemplateMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoTemplateList");
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                dbServer.AddInParameter(command, "Pathologist", DbType.Int64, BizActionObj.Pathologist);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TemplateDetails == null)
                        BizActionObj.TemplateDetails = new List<clsPathoTestTemplateDetailsVO>();
                    while (reader.Read())
                    {
                        clsPathoTestTemplateDetailsVO objRadiologyVO = new clsPathoTestTemplateDetailsVO();
                        objRadiologyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objRadiologyVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objRadiologyVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objRadiologyVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objRadiologyVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objRadiologyVO.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        objRadiologyVO.Pathologist = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"]));
                        objRadiologyVO.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        objRadiologyVO.PathologistName = Convert.ToString(DALHelper.HandleDBNull(reader["PathologistName"]));
                        objRadiologyVO.GenderName = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));

                        BizActionObj.TemplateDetails.Add(objRadiologyVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch
            {

            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetPathoGender(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetPathoTemplateGenderBizActionVO BizActionObj = valueObject as clsGetPathoTemplateGenderBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoGenderTemplateList");
                dbServer.AddInParameter(command, "TemplateID", DbType.String, BizActionObj.TemplateID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.GenderDetails == null)
                        BizActionObj.GenderDetails = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objRadiologyVO = new MasterListItem();
                        objRadiologyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));

                        BizActionObj.GenderDetails.Add(objRadiologyVO);
                    }
                }

                reader.Close();
            }
            catch
            {

            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject AddPathoTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathoTemplateMasterBizActionVO BizActionObj = valueObject as clsAddPathoTemplateMasterBizActionVO;

            if (BizActionObj.TemplateDetails.ID == 0)
                BizActionObj = AddTemplateDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateTemplateDetails(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddPathoTemplateMasterBizActionVO AddTemplateDetails(clsAddPathoTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {

            try
            {
                clsPathoTestTemplateDetailsVO objPathoVO = BizActionObj.TemplateDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPathoTemplateMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, objPathoVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, objPathoVO.Description.Trim());
                dbServer.AddInParameter(command, "MultiplePathoDoctor", DbType.Boolean, objPathoVO.MultiplePathoDoctor);
                dbServer.AddInParameter(command, "Template", DbType.String, objPathoVO.Template);
                dbServer.AddInParameter(command, "Pathologist", DbType.Int64, objPathoVO.Pathologist);
                dbServer.AddInParameter(command, "PathologistName", DbType.String, objPathoVO.PathologistName);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPathoVO.GenderID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPathoVO.ID);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.TemplateDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
                //added by rohini dated 14.4.16
                List<MasterListItem> masterList = BizActionObj.GenderList;
                if (masterList != null || masterList.Count > 0)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteGenderToTemplate");
                    dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.ID);
                    int intStatus88 = dbServer.ExecuteNonQuery(command2);

                    foreach (var item in masterList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPathoGenderToTemplate");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "GenderID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.ID);
                        dbServer.AddInParameter(command1, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    }
                }
                //
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }



        private clsAddPathoTemplateMasterBizActionVO UpdateTemplateDetails(clsAddPathoTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPathoTestTemplateDetailsVO objPathoVO = BizActionObj.TemplateDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePathoTemplateMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPathoVO.ID);
                dbServer.AddInParameter(command, "Code", DbType.String, objPathoVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, objPathoVO.Description.Trim());
                dbServer.AddInParameter(command, "PathologistName", DbType.String, objPathoVO.PathologistName.Trim());
                dbServer.AddInParameter(command, "Template", DbType.String, objPathoVO.Template.Trim());
                dbServer.AddInParameter(command, "Pathologist", DbType.Int64, objPathoVO.Pathologist);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPathoVO.GenderID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");

                //added by rohini dated 14.4.16
                List<MasterListItem> masterList = BizActionObj.TemplateDetails.GenderList;
                if (masterList != null || masterList.Count > 0)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteGenderToTemplate");
                    dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, objPathoVO.ID);
                    int intStatus88 = dbServer.ExecuteNonQuery(command2);

                    foreach (var item in masterList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPathoGenderToTemplate");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "GenderID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, objPathoVO.ID);
                        dbServer.AddInParameter(command1, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    }
                }
                //

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject ChangePathoTemplateStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsAddPathoTemplateMasterBizActionVO BizActionObj = valueObject as clsAddPathoTemplateMasterBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStatusPathoTemplateMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TemplateDetails.ID);
                //dbServer.AddInParameter(command, "Code", DbType.String, objPathoVO.Code.Trim());
                //dbServer.AddInParameter(command, "Description", DbType.String, objPathoVO.Description.Trim());
                //dbServer.AddInParameter(command, "PathologistName", DbType.String, objPathoVO.PathologistName.Trim());
                //dbServer.AddInParameter(command, "Template", DbType.String, objPathoVO.Template.Trim());
                //dbServer.AddInParameter(command, "Pathologist", DbType.Int64, objPathoVO.Pathologist);
                //dbServer.AddInParameter(command, "GenderID", DbType.Int64, objPathoVO.GenderID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.IsStatusChanged);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception EX)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        //by rohini dated 18.1.2016
        public override IValueObject GetParameterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetParaByParaAndMachineBizActionVO BizActionObj = valueObject as clsGetParaByParaAndMachineBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetParameterListbyMachineId");

                //dbServer.AddInParameter(command, "ParameterID", DbType.Int64, BizActionObj.ParameterID);
                dbServer.AddInParameter(command, "MachineParameterID", DbType.Int64, BizActionObj.MachineParaID);
                //dbServer.AddInParameter(command, "GenderID", DbType.String, BizActionObj.MachineName);

                //dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                // MasterListItem list = new MasterListItem();
                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsParameterLinkingVO>();
                    while (reader.Read())
                    {
                        clsParameterLinkingVO objDetails = new clsParameterLinkingVO();
                        objDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDetails.ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        BizActionObj.DetailsList.Add(objDetails);
                    }
                }
                reader.NextResult();
                // BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }


        public override IValueObject AddMachineToTest(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddMachineToTestbizActionVO objItem = valueObject as clsAddMachineToTestbizActionVO;
            try
            {
                DbCommand command = null;
                clsPathoTestMasterVO objItemVO = objItem.ItemSupplier;
                int status = 0;

                command = dbServer.GetStoredProcCommand("CIMS_AddMachineListForTest");

                if (objItem.ItemSupplierList.Count > 0)
                {
                    for (int i = 0; i <= objItem.ItemSupplierList.Count - 1; i++)
                    {
                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitID);
                        dbServer.AddInParameter(command, "TestID", DbType.Int64, objItemVO.TestID);
                        dbServer.AddInParameter(command, "MchineID", DbType.Int64, objItem.ItemSupplierList[i].ID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItem.ItemSupplierList[i].status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        if (intStatus > 0)
                        {
                            status = 1;
                        }
                    }
                    objItem.SuccessStatus = status;//(int)dbServer.GetParameterValue(command, "ResultStatus");   
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;


        }

        public override IValueObject GetMachineToTestList(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetMachineToTestBizActionVO objBizAction = valueObject as clsGetMachineToTestBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetMachijneListForTest");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemSupplier.TestID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemSupplierList == null)
                        objBizAction.ItemSupplierList = new List<clsPathoTestMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO objItemMaster = new clsPathoTestMasterVO();
                        objItemMaster.status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.MachineID = (long)DALHelper.HandleDBNull(reader["MachineID"]);
                        objItemMaster.TestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        objBizAction.ItemSupplierList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }

        public override IValueObject AddMachineToSubTest(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddMachineToSubTestbizActionVO objItem = valueObject as clsAddMachineToSubTestbizActionVO;
            try
            {
                DbCommand command = null;
                clsPathoTestMasterVO objItemVO = objItem.ItemSupplier;
                int status = 0;

                command = dbServer.GetStoredProcCommand("CIMS_AddMachineListForSubTest");

                if (objItem.ItemSupplierList.Count > 0)
                {
                    for (int i = 0; i <= objItem.ItemSupplierList.Count - 1; i++)
                    {
                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitID);
                        dbServer.AddInParameter(command, "SubTestID", DbType.Int64, objItemVO.SubTestID);
                        dbServer.AddInParameter(command, "MchineID", DbType.Int64, objItem.ItemSupplierList[i].ID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItem.ItemSupplierList[i].status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        if (intStatus > 0)
                        {
                            status = 1;
                        }
                    }
                    objItem.SuccessStatus = status;//(int)dbServer.GetParameterValue(command, "ResultStatus");   
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;


        }

        public override IValueObject GetMachineToSubTestList(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetMachineToSubTestBizActionVO objBizAction = valueObject as clsGetMachineToSubTestBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetMachijneListForSubTest");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemSupplier.SubTestID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemSupplierList == null)
                        objBizAction.ItemSupplierList = new List<clsPathoTestMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO objItemMaster = new clsPathoTestMasterVO();
                        objItemMaster.status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.MachineID = (long)DALHelper.HandleDBNull(reader["MachineID"]);
                        objItemMaster.SubTestID = (long)DALHelper.HandleDBNull(reader["SubTestID"]);
                        objBizAction.ItemSupplierList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }

        public override IValueObject AddPathologistToTemp(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddPathologistToTempbizActionVO objItem = valueObject as clsAddPathologistToTempbizActionVO;
            try
            {
                DbCommand command = null;
                clsPathoTestTemplateDetailsVO objItemVO = objItem.ItemSupplier;
                int status = 0;

                command = dbServer.GetStoredProcCommand("CIMS_AddPathologistListForTest");

                if (objItem.ItemSupplierList.Count > 0)
                {
                    for (int i = 0; i <= objItem.ItemSupplierList.Count - 1; i++)
                    {
                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitID);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objItemVO.TemplateID);
                        dbServer.AddInParameter(command, "PathologistID", DbType.Int64, objItem.ItemSupplierList[i].ID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItem.ItemSupplierList[i].Status);
                        //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                        //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        //dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        if (intStatus > 0)
                        {
                            status = 1;
                        }
                    }
                    objItem.SuccessStatus = status;//(int)dbServer.GetParameterValue(command, "ResultStatus");   
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;


        }

        public override IValueObject GetPathologistToTempList(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetPathologistToTempBizActionVO objBizAction = valueObject as clsGetPathologistToTempBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetPathologistListForTest");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemSupplier.TemplateID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemSupplierList == null)
                        objBizAction.ItemSupplierList = new List<clsPathoTestTemplateDetailsVO>();
                    while (reader.Read())
                    {
                        clsPathoTestTemplateDetailsVO objItemMaster = new clsPathoTestTemplateDetailsVO();
                        objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.Pathologist = (long)DALHelper.HandleDBNull(reader["PathologistID"]);
                        objItemMaster.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
                        objBizAction.ItemSupplierList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }
        //
        public override IValueObject GetParameterListForTest(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetParameterOrSubTestSearchBizActionVO objBizAction = valueObject as clsGetParameterOrSubTestSearchBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetParameterByName");
                dbServer.AddInParameter(command, "Description", DbType.String, objBizAction.Description);
                dbServer.AddInParameter(command, "Flag", DbType.Int16, objBizAction.Flag);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ParameterList == null)
                        objBizAction.ParameterList = new List<clsPathoTestMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO objItemMaster = new clsPathoTestMasterVO();
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemMaster.status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemMaster.FormulaID = Convert.ToString(DALHelper.HandleDBNull(reader["FormulaID"]));                        
                        objBizAction.ParameterList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }

        public override IValueObject GetTemplateListForTest(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetWordOrReportTemplateBizActionVO objBizAction = valueObject as clsGetWordOrReportTemplateBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetTemplateByName");
                dbServer.AddInParameter(command, "Description", DbType.String, objBizAction.Description);
                dbServer.AddInParameter(command, "Flag", DbType.Int16, objBizAction.Flag);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.TemplateDetails == null)
                        objBizAction.TemplateDetails = new List<clsPathoTestTemplateDetailsVO>();
                    while (reader.Read())
                    {
                        clsPathoTestTemplateDetailsVO objItemMaster = new clsPathoTestTemplateDetailsVO();
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemMaster.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objBizAction.TemplateDetails.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }
        //
        #endregion

        // Added By Anumani 

        public override IValueObject GetPreviousRecordDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;

            clsGetPreviousParameterValueBizActionVO objItem = valueObject as clsGetPreviousParameterValueBizActionVO;
            objItem.ParameterList = new List<clsGetPreviousParameterValueBizActionVO>();
            try
            {

                con = dbServer.CreateConnection();
                con.Open();

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetPreviousResultForParameterID");
                dbServer.AddInParameter(command, "ParameterId", DbType.Int64, objItem.PathoTestParameter.ParameterID);
                dbServer.AddInParameter(command, "TestId", DbType.Int64, objItem.PathTestId.TestID);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objItem.PathPatientDetail.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objItem.PathPatientDetail.PatientUnitID);
                dbServer.AddInParameter(command, "mainId", DbType.Int64, objItem.PathPatientDetail.ID);
                dbServer.AddInParameter(command, "OrderDate", DbType.DateTime, objItem.PathPatientDetail.OrderDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);




                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                while (reader.Read())
                {
                    clsGetPreviousParameterValueBizActionVO obj = new clsGetPreviousParameterValueBizActionVO();
                    //obj.ParameterList = new List<clsGetPreviousParameterValueBizActionVO>();
                    obj.ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"]));
                    obj.ParameterValue = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"]));
                    obj.ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ResultValue"]));
                    obj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ADate"]));

                    objItem.ParameterList.Add(obj);
                }


            }
            catch (Exception E)
            {
            }
            return objItem;
        }





        public override IValueObject GetDeltaCheckDetails(IValueObject valueObject, clsUserVO UserVo)
        {

            DbConnection con = null;

            clsGetDeltaCheckValueBizActionVO objItem = valueObject as clsGetDeltaCheckValueBizActionVO;
            objItem.List = new List<clsGetDeltaCheckValueBizActionVO>();
            try
            {

                con = dbServer.CreateConnection();
                con.Open();

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetDeltaCheckForParameter");
                dbServer.AddInParameter(command, "ParameterId", DbType.Int64, objItem.PathoTestParameter.ParameterID);
                dbServer.AddInParameter(command, "TestId", DbType.String, objItem.PathTestId.TestID);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, objItem.PathPatientDetail.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objItem.PathPatientDetail.PatientUnitID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "mainId", DbType.Int64, objItem.PathPatientDetail.ID);
                dbServer.AddInParameter(command, "OrderDate", DbType.DateTime, objItem.PathPatientDetail.OrderDate);


                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGetDeltaCheckValueBizActionVO obj = new clsGetDeltaCheckValueBizActionVO();
                        //    obj.List = new List<clsGetDeltaCheckValueBizActionVO>();
                        obj.ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"]));
                        //   obj.ParameterValue = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"]));
                        obj.ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ResultValue"]));
                        //   obj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                        objItem.List.Add(obj);
                    }
                }


            }
            catch (Exception E)
            {
            }
            return objItem;
        }
        public override IValueObject GetReflexTestingDetails(IValueObject valueObject, clsUserVO UserVo)
        {

            DbConnection con = null;

            clsGetReflexTestingServiceParameterBizActionVO objItem = valueObject as clsGetReflexTestingServiceParameterBizActionVO;
            objItem.ServiceList = new List<clsGetReflexTestingServiceParameterBizActionVO>();
            try
            {

                con = dbServer.CreateConnection();
                con.Open();

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetReflexTextingParameters");
                dbServer.AddInParameter(command, "ParameterId", DbType.Int64, objItem.ParameterID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItem.ServiceID);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);



                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                while (reader.Read())
                {
                    clsGetReflexTestingServiceParameterBizActionVO obj = new clsGetReflexTestingServiceParameterBizActionVO();
                    //    obj.List = new List<clsGetDeltaCheckValueBizActionVO>();
                    obj.ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"]));
                    obj.ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"]));
                    obj.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                    obj.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                    //   obj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));

                    objItem.ServiceList.Add(obj);
                }


            }
            catch (Exception E)
            {
            }
            return objItem;
        }

        public override IValueObject GetAssignedAgency(IValueObject valueObject, clsUserVO UserVo)
        {


            DbConnection con = null;

            clsGetAssignedAgencyBizActionVO objItem = valueObject as clsGetAssignedAgencyBizActionVO;
            objItem.AgencyList = new List<clsGetAssignedAgencyBizActionVO>();
            try
            {

                con = dbServer.CreateConnection();
                con.Open();

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetAgencyIForService");

                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItem.ServiceID);


                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                clsGetAssignedAgencyBizActionVO obj1 = new clsGetAssignedAgencyBizActionVO();


                while (reader.Read())
                {
                    clsGetAssignedAgencyBizActionVO obj = new clsGetAssignedAgencyBizActionVO();

                    obj.AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]));
                    //obj.DefaultAgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DefaultAgencyId"]));
                    //obj.DefaultAgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"])); 

                    objItem.AgencyList.Add(obj);
                }


                command = dbServer.GetStoredProcCommand("CIMS_GetDefaultAgencyIForService");

                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItem.ServiceID);
                DbDataReader reader1;
                reader1 = (DbDataReader)dbServer.ExecuteReader(command);
                while (reader1.Read())
                {
                    objItem.DefaultAgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["DefaultAgencyId"]));
                    objItem.DefaultAgencyName = Convert.ToString(DALHelper.HandleDBNull(reader1["AgencyName"]));
                }
            }
            catch (Exception E)
            {
            }
            return objItem;
        }

        public override IValueObject GetBillingStatus(IValueObject valueObject, clsUserVO UserVo)
        {


            DbConnection con = null;

            clsPathoCheckBillingStatusVO objItem = valueObject as clsPathoCheckBillingStatusVO;
           
            try
            {

                con = dbServer.CreateConnection();
                con.Open();

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetBillingStatusForWalkin");

                dbServer.AddInParameter(command, "OrderID", DbType.Int64, objItem.OrderId);
                        
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItem.UnitId);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.ResultStatus);
                      
                int intStatus4 = dbServer.ExecuteNonQuery(command);
                objItem.ResultStatus = Convert.ToBoolean(dbServer.GetParameterValue(command, "ResultStatus"));
                }
                        
            catch (Exception E)
            {
            }
            return objItem;
        }


        //added by rohini dated 15.4.16 for display server date time on controls as per discussion with nilesh sir
        public override IValueObject GetServerDateTime(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServerDateTimeBizActionVO BizActionObj = valueObject as clsGetServerDateTimeBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServerDateTime");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsPathOrderBookingVO objPathoVO = new clsPathOrderBookingVO();
                        BizActionObj.ServerDateTime = (DateTime)reader["ServerDateTime"];
                        BizActionObj.OrderBookingList.Add(objPathoVO);
                    }


                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }
        //by rohini dated 28.4.16
        public override IValueObject GetDispachReceiveDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDispachReciveDetailListBizActionVO BizActionObj = valueObject as clsGetDispachReciveDetailListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDispatchReceiveDetails");

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);



                if (BizActionObj.IsSampleDispatched != false)
                {
                    dbServer.AddInParameter(command, "IsSampleDispatched", DbType.String, BizActionObj.IsSampleDispatched);
                }
                else
                    dbServer.AddInParameter(command, "IsSampleDispatched", DbType.String, 0);
                //added by rohini dated 11.2.16

                if (BizActionObj.OrderDetail.BillNo != null && BizActionObj.OrderDetail.BillNo.Length != 0)
                {
                    dbServer.AddInParameter(command, "BillNo", DbType.String, BizActionObj.OrderDetail.BillNo + "%");
                }
                else
                    dbServer.AddInParameter(command, "BillNo", DbType.String, null);
                if (BizActionObj.BatchNo != null && BizActionObj.BatchNo.Length != 0)
                {
                    dbServer.AddInParameter(command, "BatchNo", DbType.String, BizActionObj.BatchNo + "%");
                }
                else
                    dbServer.AddInParameter(command, "BatchNo", DbType.String, null);

                if (BizActionObj.SampleNo != null && BizActionObj.SampleNo.Length != 0)
                {
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.SampleNo + "%");
                }
                else
                    dbServer.AddInParameter(command, "SampleNo", DbType.String, null);
                if (BizActionObj.MRNo != null && BizActionObj.MRNo.Length != 0)
                {
                    dbServer.AddInParameter(command, "MRNo", DbType.String, "%" + BizActionObj.MRNo + "%");
                }
                else
                    dbServer.AddInParameter(command, "MRNo", DbType.String, null);
                //

                //if (BizActionObj.OrderDetail.FirstName != null && BizActionObj.OrderDetail.FirstName.Length != 0)
                //{
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.OrderDetail.FirstName) + "%");
                //}
                //else
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                if (BizActionObj.OrderDetail.FirstName != null && BizActionObj.OrderDetail.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, "%" + BizActionObj.OrderDetail.FirstName + "%");
                else
                    dbServer.AddInParameter(command, "FirstName", DbType.String, null);


                //if (BizActionObj.OrderDetail.LastName != null && BizActionObj.OrderDetail.LastName.Length != 0)
                //{
                //    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.OrderDetail.LastName) + "%");
                //}
                //else
                //    dbServer.AddInParameter(command, "LastName", DbType.String, null);

                if (BizActionObj.OrderDetail.LastName != null && BizActionObj.OrderDetail.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, "%" + BizActionObj.OrderDetail.LastName + "%");
                else
                    dbServer.AddInParameter(command, "LastName", DbType.String, null);

                dbServer.AddInParameter(command, "IsPending", DbType.Int64, BizActionObj.IsPending);
                dbServer.AddInParameter(command, "IsCollected", DbType.Int64, BizActionObj.IsCollected);
                dbServer.AddInParameter(command, "DispatchUserID", DbType.Int64, BizActionObj.IsDispatchByID);

                dbServer.AddInParameter(command, "SampleStatus", DbType.Int64, BizActionObj.SampleStatus);
                dbServer.AddInParameter(command, "BillType", DbType.Int32, BizActionObj.BillType);
                dbServer.AddInParameter(command, "SampleAcceptRejectBy", DbType.Int64, BizActionObj.SampleAcceptRejectBy);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "DispatchTo", DbType.Int64, BizActionObj.ClinicID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                DbDataReader reader;


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.OrderBookingDetailList == null)
                        BizActionObj.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objOrderBookingVO = new clsPathOrderBookingDetailVO();

                        objOrderBookingVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objOrderBookingVO.SampleNumber = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        objOrderBookingVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objOrderBookingVO.SampleDispatchDateTime = (DateTime?)DALHelper.HandleDBNull(reader["dispatchDate"]);
                        //objOrderBookingVO.SampleDispatchDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["dispatchDate"]));

                        objOrderBookingVO.TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                        objOrderBookingVO.TestCode = Convert.ToString(DALHelper.HandleDBNull(reader["TestCode"]));
                        objOrderBookingVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objOrderBookingVO.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                        objOrderBookingVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        objOrderBookingVO.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                        objOrderBookingVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objOrderBookingVO.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Pre"]));
                        objOrderBookingVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        objOrderBookingVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        objOrderBookingVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        objOrderBookingVO.IsSampleDispatched = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));
                        objOrderBookingVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objOrderBookingVO.IsOutSourced = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutsourced"]));
                        objOrderBookingVO.IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"]));
                        objOrderBookingVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objOrderBookingVO.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));

                        objOrderBookingVO.IsAccepted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAccepted"]));
                        objOrderBookingVO.IsRejected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRejected"]));
                        objOrderBookingVO.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                        objOrderBookingVO.IsSampleDispatched = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"]));

                        objOrderBookingVO.IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"]));
                        objOrderBookingVO.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                        objOrderBookingVO.SampleReceivedDateTime = (DateTime?)DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]);

                        objOrderBookingVO.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                        objOrderBookingVO.OrderDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["OrderDate"]));
                        objOrderBookingVO.TestDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["TestDate"]));

                        BizActionObj.OrderBookingList.Add(objOrderBookingVO);

                    }

                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }


        //by rohini dated 28.4.16
        public override IValueObject GetBatchCode(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBatchCodeBizActionVO BizActionObj = valueObject as clsGetBatchCodeBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBatchCodeDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizActionObj.OrderID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                if (BizActionObj.BatchCode != null && BizActionObj.BatchCode.Length != 0)
                {
                    dbServer.AddInParameter(command, "BatchNo", DbType.String, BizActionObj.BatchCode + "%");
                }
                else
                    dbServer.AddInParameter(command, "BatchNo", DbType.String, null);

                if (BizActionObj.DispatchTo != null && BizActionObj.DispatchTo != 0)
                {
                    dbServer.AddInParameter(command, "DispatchTo", DbType.Int64, BizActionObj.DispatchTo);
                }
                else
                    dbServer.AddInParameter(command, "DispatchTo", DbType.Int64, 0);
                if (BizActionObj.IsFromAccept == true)
                    dbServer.AddInParameter(command, "IsFromAccept", DbType.Boolean, true);
                //dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO objPathoVO = new clsPathOrderBookingDetailVO();
                        //objPathoVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //objPathoVO.SampleDispatchDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["dispatchDate"]));
                        //objPathoVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objPathoVO.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        objPathoVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        //objPathoVO.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        BizActionObj.OrderBookingList.Add(objPathoVO);
                    }

                }

                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }


        //

        //by rohini dated 28.4.16      
        public override IValueObject AddHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsRemarkHistoryBizActionVO BizActionObj = valueObject as clsRemarkHistoryBizActionVO;
            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddRemarkHistory");
            try
            {
                foreach (var item in BizActionObj.RemarkHistory)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPathoRemarkHistory");
                    dbServer.AddInParameter(command1, "UserID", DbType.Int64, BizActionObj.UserID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command1, "OrderID", DbType.Int64, BizActionObj.OrderID);
                    dbServer.AddInParameter(command1, "OrderUnitID", DbType.Int64, BizActionObj.OrderUnitID);
                    dbServer.AddInParameter(command1, "Remark", DbType.String, BizActionObj.Remark);
                    dbServer.AddInParameter(command1, "UserName", DbType.String, BizActionObj.UserName);
                    dbServer.AddInParameter(command1, "OrderDetailID", DbType.Int64, item.ID);//detail id unitid
                    dbServer.AddInParameter(command1, "OrderDetailUnitId", DbType.Int64, item.UnitId);  //detail id unitid
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);             
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    
                    int intStatus1 = dbServer.ExecuteNonQuery(command1);

                    BizActionObj.ID = (long)dbServer.GetParameterValue(command1, "ID");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

    }
}


