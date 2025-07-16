using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using com.seedhealthcare.hms.CustomExceptions;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsMasterDAL : clsBaseMasterDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        public bool chkFlag = true;
        #endregion

        private clsMasterDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetMasterListBizActionVO BizActionObj = (clsGetMasterListBizActionVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.IsActive.HasValue)
                    FilterExpression.Append("Status = '" + BizActionObj.IsActive.Value + "'");

                if (BizActionObj.Parent != null)
                {
                    if (FilterExpression.Length > 0)
                        FilterExpression.Append(" And ");
                    FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
                }

                DbCommand command;
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                if (BizActionObj.MasterTable.ToString() == "M_MenuMaster")
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetMasterMenu");
                }
                else {

                      command = dbServer.GetStoredProcCommand("CIMS_GetMasterList");
                
                }
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

              

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        if (BizActionObj.MasterTable.ToString() == "M_MenuMaster")
                        {
                            BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"],  reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));
                        
                        }
                        else
                        {

                            BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));
                        

                        }
                      
                          //  BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Title"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));
                        
                        ////Added By CDS 22/2/16
                        //BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));

                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;
        }


        public override IValueObject GetCodeMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetMasterListBizActionVO BizActionObj = (clsGetMasterListBizActionVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.IsActive.HasValue)
                    FilterExpression.Append("Status = '" + BizActionObj.IsActive.Value + "'");

                if (BizActionObj.Parent != null)
                {
                    if (FilterExpression.Length > 0)
                        FilterExpression.Append(" And ");
                    FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
                }


                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterList_New");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        //BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));

                        ////Added By CDS 22/2/16
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (bool)reader["Status"]));//HandleDBNull(reader["Date"])));

                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;
        }
        //Addded By Bhushanp 21062017 For GST
        public override IValueObject GETSupplierList(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMasterListBizActionVO BizActionObj = (clsGetMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSupplierList");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (bool)reader["Status"], (long)reader["StateID"]));//HandleDBNull(reader["Date"])));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
            }
            finally
            {
            }
            return BizActionObj;
        }

        //public override IValueObject AddUserRole(IValueObject valueObject, clsUserVO UserVO)
        //{

        //    clsAddUserRoleBizActionVO BizActionObj = valueObject as clsAddUserRoleBizActionVO;
        //    try
        //    {
        //        clsUserRoleVO objDetailsVO = BizActionObj.Details;
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUserRole");


        //        dbServer.AddInParameter(command, "Code", DbType.String, objDetailsVO.Code);
        //        dbServer.AddInParameter(command, "Description", DbType.String, objDetailsVO.Description);


        //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objDetailsVO.AddedBy);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, objDetailsVO.AddedOn);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objDetailsVO.AddedWindowsLoginName);

        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objDetailsVO.UpdatedBy);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objDetailsVO.UpdatedOn);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.UpdatedDateTime);
        //        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objDetailsVO.UpdatedWindowsLoginName);

        //        //   dbServer.AddOutParameter(command, "VisitId", DbType.Int64, int.MaxValue);
        //        dbServer.AddParameter(command, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "Id");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {

        //    }

          //  return valueObject;
       // }


        public override IValueObject UpdateEmailTemplateStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateEmailTemplateStatusBizActionVO bizObject = valueObject as clsUpdateEmailTemplateStatusBizActionVO;

            try
            {
                clsEmailTemplateVO objEmailTemp = new clsEmailTemplateVO();
                objEmailTemp = bizObject.EmailTempStatus;
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateEmailTemplateStatus");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objEmailTemp.ID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objEmailTemp.Status);
                dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.EmailTempStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                throw;
            }

            return bizObject;
        }

        public override IValueObject UpdateSMSTemplateStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateTemplatestatusBizActionVO bizObject = valueObject as clsUpdateTemplatestatusBizActionVO;

            try
            {
                clsSMSTemplateVO objSMSTemp = new clsSMSTemplateVO();
                objSMSTemp = bizObject.SMSTempStatus;
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateSMSTemplateStatus");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objSMSTemp.ID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objSMSTemp.Status);
                dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.SMSTempStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                throw;
            }
            return bizObject;
        }

        public override IValueObject UpdateStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsUpdateRoleStatusBizActionVO bizObject = valueObject as clsUpdateRoleStatusBizActionVO;
            try
            {
                clsUserRoleVO objRoleVO = new clsUserRoleVO();

                objRoleVO = bizObject.RoleStatus;
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateRoleStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objRoleVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objRoleVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.RoleStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
            //throw new NotImplementedException();
        }

        public override IValueObject AddUserRole(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddUserRoleBizActionVO bizObject = valueObject as clsAddUserRoleBizActionVO;
            try
            {
                clsUserRoleVO objRoleVO = bizObject.Details;
                DbCommand command = null;
                if (objRoleVO.ID == 0)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddUserRole");
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateUserRole");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objRoleVO.ID);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                }

                AddCommonParametersForAddUpdateMethods(command, objRoleVO, UserVo);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.Details.ID = (long)dbServer.GetParameterValue(command, "ID"); 
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        private void AddCommonParametersForAddUpdateMethods(DbCommand command, clsUserRoleVO objRoleVO, clsUserVO objUserVO)
        {

            dbServer.AddInParameter(command, "Code", DbType.String, objRoleVO.Code);
            dbServer.AddInParameter(command, "Description", DbType.String, objRoleVO.Description);
            dbServer.AddInParameter(command, "Status", DbType.Boolean, objRoleVO.Status);
            dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
            //dbServer.AddOutParameter(command, "RoleId", DbType.Int64, int.MaxValue);

            StringBuilder MenuRightsIdList = new StringBuilder();
            StringBuilder MenuRightsStatusList = new StringBuilder();
            //StringBuilder CreateList = new StringBuilder();
            //StringBuilder UpdateList = new StringBuilder();
            //StringBuilder DeleteList = new StringBuilder();
            //StringBuilder ReadList = new StringBuilder();
            //StringBuilder PrintList = new StringBuilder();

            for (int MenuCount = 0; MenuCount < objRoleVO.MenuList.Count; MenuCount++)
            {
                clsMenuVO varMenu = objRoleVO.MenuList[MenuCount];
                MenuRightsIdList.Append(varMenu.ID);
                MenuRightsStatusList.Append(varMenu.Status);
                //CreateList.Append(varMenu.IsCreate);
                //UpdateList.Append(varMenu.IsUpdate);
                //DeleteList.Append(varMenu.IsDelete);
                //ReadList.Append(varMenu.IsRead);
                //PrintList.Append(varMenu.IsPrint);

                if (MenuCount < (objRoleVO.MenuList.Count - 1))
                {
                    MenuRightsIdList.Append(",");
                    MenuRightsStatusList.Append(",");
                    //CreateList.Append(",");
                    //UpdateList.Append(",");
                    //DeleteList.Append(",");
                    //ReadList.Append(",");
                    //PrintList.Append(",");
                }

            }

            dbServer.AddInParameter(command, "MenuIdList", DbType.String, MenuRightsIdList.ToString());
            dbServer.AddInParameter(command, "MenuStatusList", DbType.String, MenuRightsStatusList.ToString());
            //dbServer.AddInParameter(command, "CreateList", DbType.String, CreateList.ToString());
            //dbServer.AddInParameter(command, "UpdateList", DbType.String, UpdateList.ToString());
            //dbServer.AddInParameter(command, "DeleteList", DbType.String, DeleteList.ToString());
            //dbServer.AddInParameter(command, "ReadList", DbType.String, ReadList.ToString());
            //dbServer.AddInParameter(command, "PrintList", DbType.String, PrintList.ToString());


            StringBuilder DashboardIdList = new StringBuilder();
            StringBuilder DashboardStatusList = new StringBuilder();

            for (int DashboardCnt = 0; DashboardCnt < objRoleVO.DashBoardList.Count; DashboardCnt++)
            {
                clsDashBoardVO varDashboard = objRoleVO.DashBoardList[DashboardCnt];
                DashboardIdList.Append(varDashboard.ID);
                DashboardStatusList.Append(varDashboard.Status);

                if (DashboardCnt < (objRoleVO.DashBoardList.Count - 1))
                {
                    DashboardIdList.Append(",");
                    DashboardStatusList.Append(",");
                }
            }
            dbServer.AddInParameter(command, "DashboardIdList", DbType.String, DashboardIdList.ToString());
            dbServer.AddInParameter(command, "DashboardStatusList", DbType.String, DashboardStatusList.ToString());

        }

        private void AddCommonParametersForAddUpdateSMSTemplate(DbCommand command, clsSMSTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            dbServer.AddInParameter(command, "Code", DbType.String, objTemplateVO.Code);
            dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "Description", DbType.String, objTemplateVO.Description);
            dbServer.AddInParameter(command, "EnglishText", DbType.String, objTemplateVO.EnglishText);
            dbServer.AddInParameter(command, "LocalText", DbType.String, objTemplateVO.LocalText);
            // dbServer.AddInParameter(command, "Status", DbType.Boolean, objTemplateVO.Status);

            // dbServer.AddInParameter(command, "

            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
        }

        public override IValueObject UpdateParameterStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdatePathoParameterStatusBizActionVO bizObject = valueObject as clsUpdatePathoParameterStatusBizActionVO;

            try
            {
                clsPathoParameterMasterVO objTemp = new clsPathoParameterMasterVO();
                objTemp = bizObject.PathoParameterStatus;
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdatePathologyParameterStatus");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objTemp.ID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objTemp.Status);
                dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.PathoParameterStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                throw;
            }

            return bizObject;
        }
        public override IValueObject AddUpdateParameter(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatePathoParameterBizActionVO BizActionObj = valueObject as clsAddUpdatePathoParameterBizActionVO;
            clsPathoParameterMasterVO objParameter = BizActionObj.PathologyParameter;
            //DbConnection con = dbServer.CreateConnection();
             //DbTransaction trans = null;
            bool IsAdd = true;
            try
            {
                 DbCommand command = null;
                //trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoParameterMaster");
                if (BizActionObj.PathologyParameter.ID == 0)
                {
                    //add
                    //command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoParameterMaster");
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    IsAdd = true;
                }
                else
                {
                    //Update
                    //command = dbServer.GetStoredProcCommand("CIMS_");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objParameter.ID);
                    dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    IsAdd = false;
                }

                AddCommonParametersForAddUpdateParameter(command, objParameter, objUserVO);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                if (BizActionObj.SuccessStatus == 1)
                {
                    BizActionObj.PathologyParameter.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                    objParameter.ID = BizActionObj.PathologyParameter.ID;

                    if (IsAdd == false)
                    {
                        // Delete the respective record and Add The Details
                        DeleteHelpAndDefaultValueDetails(objParameter, objUserVO);
                    }

                    //Add Default values if numeric
                    if (objParameter.IsNumeric == true)
                        AddParameterDefaultDetails(objParameter, objUserVO, IsAdd);
                    //Add Help Values if text
                    else
                        AddParameterHelpValueDetails(objParameter, objUserVO, IsAdd);
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

        private void DeleteHelpAndDefaultValueDetails(clsPathoParameterMasterVO objParameter, clsUserVO objUserVO)
        {
            try
            {
                //Help Values.
                //foreach (var item in objParameter.Items)
                //{
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_DeletePathoParameterHelpValueDetails");

                dbServer.AddInParameter(command, "ParameterID", DbType.Int64, objParameter.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                //}

                //Default Values.
                //foreach (var item in objParameter.DefaultValues)
                //{
                DbCommand command1 = null;
                command1 = dbServer.GetStoredProcCommand("CIMS_DeletePathoParameterDefaultValueDetails");

                dbServer.AddInParameter(command1, "ParameterID", DbType.Int64, objParameter.ID);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus1 = dbServer.ExecuteNonQuery(command1);

                //foreach (var item in objParameter.DefaultValues)
                //{
                    DbCommand command2 = null;
                    command2 = dbServer.GetStoredProcCommand("CIMS_DeleteServiceParameterDefaultValueDetails");

                    dbServer.AddInParameter(command2, "ParameterID", DbType.Int64, objParameter.ID);
                    //dbServer.AddInParameter(command2, "ParameterDefaultID", DbType.Int64, item.ID);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2);

                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AddParameterHelpValueDetails(clsPathoParameterMasterVO objParameter, clsUserVO objUserVO, bool NewAdd)
        {
            try
            {
                if (objParameter.Items.Count > 0)
                {
                    foreach (var item in objParameter.Items)
                    {
                        DbCommand command = null;
                        command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoParameterHelpValueMaster");

                        dbServer.AddInParameter(command, "ParameterID", DbType.Int64, objParameter.ID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "HelpValue", DbType.String, item.strHelp);
                        dbServer.AddInParameter(command, "IsDefault", DbType.Boolean, item.IsDefault);
                        //added by rohini dated 3.1.16
                        dbServer.AddInParameter(command, "IsAbnoramal", DbType.Boolean, item.IsAbnoramal);
                        //

                        //if (NewAdd == true)
                        AddParameterUserDetails(command, objUserVO);
                        //else
                        //  UpdateParameterUserDetails(command, objParameter.ID, objUserVO);

                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        //BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This Function Is Called When We Are Inserting Or Updating Default Values Of Parameter
        /// </summary>
        /// <param name="objParameter"></param>
        /// <param name="objUserVO"></param>
        /// <param name="NewAdd"></param>
        private void AddParameterDefaultDetails(clsPathoParameterMasterVO objParameter, clsUserVO objUserVO, bool NewAdd)
        {
            //DbConnection con = dbServer.CreateConnection();
           // DbTransaction trans = null;
            try
            {
               // trans = con.BeginTransaction();
                if (objParameter.DefaultValues.Count > 0)
                {
                    foreach (var item in objParameter.DefaultValues)
                    {
                        DbCommand command = null;
                        command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoParameterDefaultValueMaster");

                        dbServer.AddInParameter(command, "ParameterID", DbType.Int64, objParameter.ID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "CategoryID", DbType.Int64, item.CategoryID);
                        dbServer.AddInParameter(command, "Category", DbType.String, item.Category);
                        dbServer.AddInParameter(command, "IsAgeApplicable", DbType.Boolean, item.IsAge);
                        dbServer.AddInParameter(command, "AgeFrom", DbType.Double, item.AgeFrom);
                        dbServer.AddInParameter(command, "AgeTo", DbType.Double, item.AgeTo);
                        dbServer.AddInParameter(command, "MinValue", DbType.Double, item.MinValue);
                        dbServer.AddInParameter(command, "MaxValue", DbType.Double, item.MaxValue);
                        //newly added as per disscussion with dr. gautam
                        dbServer.AddInParameter(command, "HighReffValue", DbType.Double, item.HighReffValue);
                        dbServer.AddInParameter(command, "LowReffValue", DbType.Double, item.LowReffValue);
                        dbServer.AddInParameter(command, "UpperPanicValue", DbType.Double, item.UpperPanicValue);
                        dbServer.AddInParameter(command, "LowerPanicValue", DbType.Double, item.LowerPanicValue);

                        dbServer.AddInParameter(command, "LowReflex", DbType.Double, item.LowReflexValue);
                        dbServer.AddInParameter(command, "HighReflex", DbType.Double, item.HighReflexValue);
                        //
                        dbServer.AddInParameter(command, "DefaultValue", DbType.Double, item.DefaultValue);

                        //added by rohini dated 15.1.2016
                        dbServer.AddInParameter(command, "AgeValue", DbType.String, item.AgeValue);

                        dbServer.AddInParameter(command, "MachineID", DbType.Int64, item.MachineID);
                        dbServer.AddInParameter(command, "Machine", DbType.String, item.Machine);
                        
                        dbServer.AddInParameter(command, "MinImprobable", DbType.Double, item.MinImprobable);
                        dbServer.AddInParameter(command, "MaxImprobable", DbType.Double, item.MaxImprobable);
                        
                        dbServer.AddInParameter(command, "Note", DbType.String, item.Note);
                         dbServer.AddInParameter(command, "IsReflexTesting", DbType.Boolean, item.IsReflexTesting);
                        //

                        // Varying References
                         dbServer.AddInParameter(command, "VaryingReferences", DbType.String, item.VaryingReferences);

                        // if (NewAdd == true)
                        AddParameterUserDetails(command, objUserVO);
                        //added by rohini 17.2.16
                       

                        //else
                        //  UpdateParameterUserDetails(command, objParameter.ID, objUserVO);

                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        long getid = Convert.ToInt16(dbServer.GetParameterValue(command, "ID"));
                        if (item.List != null)
                        {
                            foreach (var item1 in item.List)
                            {
                                DbCommand command2 = null;
                                command2 = dbServer.GetStoredProcCommand("CIMS_AddServiceListForParameter");
                                command2.Parameters.Clear();
                                dbServer.AddInParameter(command2, "Id", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "ParameterDefaultID", DbType.Int64, getid);
                                dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command2, "ParameterID", DbType.Int64, objParameter.ID);
                                dbServer.AddInParameter(command2, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "Status", DbType.Boolean, 1);
                                //dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                                //dbServer.AddInParameter(command2, "AddedOn", DbType.String, objItemVO.AddedOn);
                                //dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                                //dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0);
                                int intStatus2 = dbServer.ExecuteNonQuery(command2);
                            }
                        }
                       // AddServicesToParameter( getid);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //added by rohini dated 17.2.16
        //CIMS_DeleteServiceParameterDefaultValueDetails

        //public override IValueObject AddServicesToParameter(IValueObject valueObject, clsUserVO userVO)
        //{
        ////    clsAddServiceToParameterbizActionVO objItem = valueObject as clsAddServiceToParameterbizActionVO;
        ////    try
        ////    {
        ////        DbCommand command = null;
        ////        clsPathoParameterDefaultValueMasterVO objItemVO = objItem.ItemSupplier;
        ////        int status = 0;

        ////        command = dbServer.GetStoredProcCommand("CIMS_AddServiceListForParameter");

        ////        if (objItem.ItemSupplierList.Count > 0)
        ////        {
        ////            for (int i = 0; i <= objItem.ItemSupplierList.Count - 1; i++)
        ////            {
        ////                command.Parameters.Clear();
        ////                dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
        ////                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitID);
        ////                dbServer.AddInParameter(command, "ParameterDefaultID", DbType.Int64, objItemVO.ParameterDefaultID);
        ////                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItem.ItemSupplierList[i].ID);
        ////                dbServer.AddInParameter(command, "Status", DbType.Boolean, 1);
        ////                //  dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
        ////                //   dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitId);
        ////                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
        ////                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
        ////                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
        ////                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
        ////                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
        ////                int intStatus = dbServer.ExecuteNonQuery(command);
        ////                if (intStatus > 0)
        ////                {
        ////                    status = 1;
        ////                }
        ////            }
        ////            objItem.SuccessStatus = status;//(int)dbServer.GetParameterValue(command, "ResultStatus");   
        ////        }

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        throw ex;
        ////    }
        //    return valueObject;


        //}

        public override IValueObject GetServicesByID(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetPathoServicesByIDBizActionVO objBizAction = valueObject as clsGetPathoServicesByIDBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetServiceListForParaDefaultvalue");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ID);
              //  dbServer.AddInParameter(command, "ParameterID", DbType.String, objBizAction.ParaID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    //if (objBizAction.ID == null)
                        objBizAction.ServiceDetailsList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objItemMaster = new MasterListItem();
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["description"]));
                        objItemMaster.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objBizAction.ServiceDetailsList.Add(objItemMaster);
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

        /// <summary>
        /// This Function Is Called When We are Inserting User Related Values In M_PathoParameterDefaultValueDetails
        /// Or HelpValueDetail Table.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="objUserVO"></param>
        private void AddParameterUserDetails(DbCommand command, clsUserVO objUserVO)
        {
            dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
        }

        /// <summary>
        /// This Function Is Called When We Are Updating User Related Values In M_PathoParameterDefaultValueDetails
        /// Or HelpValueDetail Table.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="objUserVO"></param>
        private void UpdateParameterUserDetails(DbCommand command, long Id, clsUserVO objUserVO)
        {
            dbServer.AddInParameter(command, "ID", DbType.Int64, Id);
            dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
        }


        private void AddCommonParametersForAddUpdateParameter(DbCommand command, clsPathoParameterMasterVO objParameter, clsUserVO objUserVO)
        {
            try
            {
                dbServer.AddInParameter(command, "Code", DbType.String, objParameter.Code);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ParameterUnitId", DbType.Int64, objParameter.ParamUnit);
                dbServer.AddInParameter(command, "Description", DbType.String, objParameter.ParameterDesc);
                dbServer.AddInParameter(command, "PrintName", DbType.String, objParameter.PrintName);
                dbServer.AddInParameter(command, "IsNumeric", DbType.Boolean, objParameter.IsNumeric);
                dbServer.AddInParameter(command, "Formula", DbType.String, objParameter.Formula);
                //BY ROHINEE DATED 22/11/16
                dbServer.AddInParameter(command, "FormulaID", DbType.String, objParameter.FormulaID);
                //
                // added on 25/07/2016
                // Techniques Used parameter Wise
                // added by Anumani
                dbServer.AddInParameter(command, "TechniqueUsed", DbType.String, objParameter.Technique);

                //ends
                //by rohini dated 15/1/2016
                dbServer.AddInParameter(command, "DeltaCheckPer", DbType.Double, objParameter.DeltaCheckPer);  
               // dbServer.AddInParameter(command, "IsFlagReflex", DbType.Boolean, objParameter.IsFlagReflex);  
                //
                //if (objParameter.IsNumeric == false)

                //added by neena for pathology linking
                dbServer.AddInParameter(command, "ExcutionCalenderParameterID", DbType.String, objParameter.ExcutionCalenderParameterID);
                //
                dbServer.AddInParameter(command, "TextValue", DbType.String, objParameter.TextRange);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject AddSMSTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddSMSTemplateBizActionVO BizActionObj = valueObject as clsAddSMSTemplateBizActionVO;
            clsSMSTemplateVO objTemplate = BizActionObj.SMSTemplate;
            try
            {
                DbCommand command = null;

                if (objTemplate.ID == 0)
                {  // add
                    command = dbServer.GetStoredProcCommand("CIMS_AddSMSTemplate");
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {  // Update
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateSMSTemplate");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objTemplate.ID);
                    dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    //  dbServer.AddInParameter(command, "Status", DbType.Boolean, objTemplate.Status);
                }

                AddCommonParametersForAddUpdateSMSTemplate(command, objTemplate, UserVo);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.SMSTemplate.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return BizActionObj;
        }

        private void AddCommonParametersForAddUpdateEmailTemplate(DbCommand command, clsEmailTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            dbServer.AddInParameter(command, "Code", DbType.String, objTemplateVO.Code);
            dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "Description", DbType.String, objTemplateVO.Description);
            dbServer.AddInParameter(command, "Subject", DbType.String, objTemplateVO.Subject);
            dbServer.AddInParameter(command, "Text", DbType.String, objTemplateVO.Text);
            dbServer.AddInParameter(command, "EmailFormat", DbType.Boolean, objTemplateVO.EmailFormat);
            dbServer.AddInParameter(command, "AttachmentNos", DbType.Int64, objTemplateVO.AttachmentNos);


            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
        }

        public override IValueObject AddEmailTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddEmailTemplateBizActionVO objBizEmail = valueObject as clsAddEmailTemplateBizActionVO;
            try
            {
                clsEmailTemplateVO objEmail = objBizEmail.EmailTemplate;
                DbCommand command = null;

                if (objEmail.ID == 0)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddEmailTemplate");
                    dbServer.AddOutParameter(command, "Id", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    chkFlag = true;
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateEmailTemplate");
                    dbServer.AddInParameter(command, "Id", DbType.Int64, objEmail.ID);
                    dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    chkFlag = false;
                }

                AddCommonParametersForAddUpdateEmailTemplate(command, objEmail, UserVo);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objBizEmail.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                objBizEmail.EmailTemplate.ID = (long)dbServer.GetParameterValue(command, "ID");
                objEmail = objBizEmail.EmailTemplate;

                if (chkFlag == true && objEmail.AttachmentNos > 0)
                    AddAttachmentDetails(objEmail, UserVo);
                else if (chkFlag == false && objEmail.AttachmentNos > 0)
                {
                    DeleteAttachmentDetails(objEmail, UserVo);
                    AddAttachmentDetails(objEmail, UserVo);
                }

                //if (objEmail.AttachmentNos > 0)
                //    AddAttachmentDetails(objEmail, UserVo);
                //else
                //    DeleteAttachmentDetails(objEmail, UserVo);

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return objBizEmail;
        }

        private void DeleteAttachmentDetails(clsEmailTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            try
            {
                if (chkFlag == false)
                {
                    DbCommand commandImage = null;

                    commandImage = dbServer.GetStoredProcCommand("CIMS_UpdateEmailAttachmentDetailsStatus");
                    dbServer.AddInParameter(commandImage, "ID", DbType.Int64, objTemplateVO.ID);
                    // dbServer.AddInParameter(commandImage, "TemplateID", DbType.Int64, objTemplateVO.ID);

                    dbServer.AddInParameter(commandImage, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandImage, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandImage, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(commandImage, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(commandImage, "UpdatedBy", DbType.Int64, objUserVO.ID);

                    int intStatus = dbServer.ExecuteNonQuery(commandImage);
                }
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }

        private void AddAttachmentDetails(clsEmailTemplateVO objTemplateVO, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            try
            {
                // if (chkFlag == true)
                // {
                DbCommand commandImage = null;

                for (int i = 0; i < objTemplateVO.AttachmentNos; i++)
                {
                    commandImage = dbServer.GetStoredProcCommand("CIMS_AddEmailAttachmentDetails");
                    dbServer.AddOutParameter(commandImage, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(commandImage, "TemplateID", DbType.Int64, objTemplateVO.ID);
                    dbServer.AddInParameter(commandImage, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandImage, "Attachment", DbType.Binary, objTemplateVO.AttachmentDetails[i].Attachment);
                    dbServer.AddInParameter(commandImage, "AttachmentFileName", DbType.String, objTemplateVO.AttachmentDetails[i].AttachmentFileName);
                    dbServer.AddInParameter(commandImage, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(commandImage, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandImage, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(commandImage, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandImage, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                    int intStatus = dbServer.ExecuteNonQuery(commandImage);
                    //objTemplateVO.AttachmentDetails[0].ID = (long)dbServer.GetParameterValue(commandImage, "Id");
                    objTemplateVO.AttachmentDetails[i].ID = (long)dbServer.GetParameterValue(commandImage, "ID");
                }
                // }
                //else
                //{
                //    //for (int ACount = 0; ACount < objTemplateVO.AttachmentNos; ACount++)
                //    //{
                //    DbCommand commandImage = null;

                //    commandImage = dbServer.GetStoredProcCommand("CIMS_UpdateEmailAttachmentDetails");
                //    dbServer.AddInParameter(commandImage, "ID", DbType.Int64, objTemplateVO.AttachmentDetails.ID);
                //    dbServer.AddInParameter(commandImage, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(commandImage, "TemplateID", DbType.Int64, objTemplateVO.ID);
                //    dbServer.AddInParameter(commandImage, "Attachment", DbType.Binary, objTemplateVO.AttachmentDetails.Attachment);
                //    dbServer.AddInParameter(commandImage, "AttachmentFileName", DbType.String, objTemplateVO.AttachmentDetails.AttachmentFileName);

                //    dbServer.AddInParameter(commandImage, "UpdatedUnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(commandImage, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(commandImage, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                //    dbServer.AddInParameter(commandImage, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //    dbServer.AddInParameter(commandImage, "UpdatedBy", DbType.Int64, objUserVO.ID);

                //    int intStatus = dbServer.ExecuteNonQuery(commandImage);
                //    //  objTemplateVO.ID = (long)dbServer.GetParameterValue(commandImage, "Id");

                //    //  dbServer.AddOutParameter(commandImage, "ResultStatus", DbType.Int32, int.MaxValue);
                //    //}
                //}

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

        }

        public override IValueObject GetParametersForList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoParameterMasterBizActionVO BizActionParamList = (clsGetPathoParameterMasterBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathologyParameterListForSearch");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionParamList.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionParamList.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionParamList.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "Code");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionParamList.SearchExpression);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //added by rohinee
                dbServer.AddInParameter(command, "AllParameter", DbType.Boolean, BizActionParamList.AllParameter);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionParamList.TotalRows);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizActionParamList.ParameterList == null)
                        BizActionParamList.ParameterList = new List<clsPathoParameterMasterVO>();

                    while (reader.Read())
                    {
                        clsPathoParameterMasterVO objParameterVO = new clsPathoParameterMasterVO();

                        //clsEmailTemplateVO objSMSVO = new clsEmailTemplateVO();
                        objParameterVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        //objSMSVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

                        objParameterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objParameterVO.ParameterDesc = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objParameterVO.PrintName = (string)DALHelper.HandleDBNull(reader["PrintName"]);
                        //objParameterVO.ParamUnit = (long)DALHelper.HandleDBNull(reader["ParameterUnitId"]);
                        //objParameterVO.EmailFormat = (bool)DALHelper.HandleDBNull(reader["EmailFormat"]);

                        objParameterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objParameterVO.stringstrParameterUnitName = (string)DALHelper.HandleDBNull(reader["ParameterUnitName"]);
                       // objParameterVO.FormulaID = (string)DALHelper.HandleDBNull(reader["FormulaID"]);
                        //BizActionETemp.EmailList.Add(objSMSVO);
                        BizActionParamList.ParameterList.Add(objParameterVO);
                    }
                }
                reader.NextResult();
                BizActionParamList.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }

            return BizActionParamList;
        }

        public override IValueObject GetEmailTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEmailTemplateListBizActionVO BizActionETemp = (clsGetEmailTemplateListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEmailTemplateListForSearch");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionETemp.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionETemp.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionETemp.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "Code");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionETemp.SearchExpression);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionETemp.TotalRows);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionETemp.EmailList == null)
                        BizActionETemp.EmailList = new List<clsEmailTemplateVO>();

                    while (reader.Read())
                    {
                        clsEmailTemplateVO objSMSVO = new clsEmailTemplateVO();

                        objSMSVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

                        objSMSVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objSMSVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objSMSVO.Subject = (string)DALHelper.HandleDBNull(reader["Subject"]);
                        objSMSVO.Text = (string)DALHelper.HandleDBNull(reader["Text"]);
                        objSMSVO.EmailFormat = (bool)DALHelper.HandleDBNull(reader["EmailFormat"]);
                        if (objSMSVO.EmailFormat == true)
                            objSMSVO.EmailFormatDisp = "Text";
                        else
                            objSMSVO.EmailFormatDisp = "HTML";
                        // objSMSVO.AttachmentNos = (long)DALHelper.HandleDBNull(reader["AttachmentNos"]);
                        objSMSVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        //BizActionETemp.EmailList.Add(objSMSVO);
                        BizActionETemp.EmailList.Add(objSMSVO);

                    }

                }
                reader.NextResult();
                BizActionETemp.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionETemp;
        }

        public override IValueObject GetSMSTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetListSMSTemplateListBizActionVO BizActionTemp = (clsGetListSMSTemplateListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSMSTemplateListForSearch");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionTemp.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionTemp.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionTemp.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "Code");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionTemp.SearchExpression);


                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionTemp.TotalRows);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionTemp.SMSList == null)
                        BizActionTemp.SMSList = new List<clsSMSTemplateVO>();

                    while (reader.Read())
                    {
                        clsSMSTemplateVO objSMSVO = new clsSMSTemplateVO();

                        objSMSVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

                        //objPatientVO.UserGeneralDetailVO.UserName = (string)DALHelper.HandleDBNull(reader["UserName"]);

                        //objPatientVO.LoginName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LoginName"]));
                        //objPatientVO.UserLoginInfo.Name = objPatientVO.LoginName;
                        ////  objPatientVO.UserType = (int)DALHelper.HandleDBNull(reader["UserType"]);
                        //objPatientVO.UserGeneralDetailVO.Locked = (bool)DALHelper.HandleDBNull(reader["Locked"]);
                        //objPatientVO.UserGeneralDetailVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //objPatientVO.UserTypeName = (string)DALHelper.HandleDBNull(reader["UserType"]);
                        //objPatientVO.RoleName = (string)DALHelper.HandleDBNull(reader["Role"]);
                        objSMSVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objSMSVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objSMSVO.EnglishText = (string)DALHelper.HandleDBNull(reader["EnglishText"]);
                        objSMSVO.LocalText = (string)DALHelper.HandleDBNull(reader["LocalText"]);
                        objSMSVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionTemp.SMSList.Add(objSMSVO);

                    }

                }
                reader.NextResult();
                BizActionTemp.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionTemp;
        }

        public override IValueObject GetEmailTemplateDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEmailTemplateBizActionVO objBizActionEmail = valueObject as clsGetEmailTemplateBizActionVO;

            try
            {
                clsEmailTemplateVO objEmailVO = objBizActionEmail.EmailDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEmailTemplateDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "Id", DbType.Int64, objBizActionEmail.ID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (objBizActionEmail.EmailDetails == null)
                            objBizActionEmail.EmailDetails = new clsEmailTemplateVO();

                        objBizActionEmail.EmailDetails.AttachmentNos = (long)DALHelper.HandleDBNull(reader["AttachmentNos"]);

                        // if (objBizActionEmail.EmailDetails.AttachmentNos > 0 && i==0)
                        if (i == 0)
                        {
                            objBizActionEmail.EmailDetails.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                            objBizActionEmail.EmailDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                            objBizActionEmail.EmailDetails.Subject = (string)DALHelper.HandleDBNull(reader["Subject"]);
                            objBizActionEmail.EmailDetails.Text = (string)DALHelper.HandleDBNull(reader["Text"]);
                            objBizActionEmail.EmailDetails.EmailFormat = (bool)DALHelper.HandleDBNull(reader["EmailFormat"]);
                            //       objEmailVO.AttachmentNos = (long)DALHelper.HandleDBNull(reader["AttachmentNos"]);
                            objBizActionEmail.EmailDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


                            objBizActionEmail.EmailDetails.AttachmentDetails = new List<clsEmailAttachmentVO>();
                        }
                        if (objBizActionEmail.EmailDetails.AttachmentNos > 0)
                        {
                            objBizActionEmail.EmailDetails.AttachmentDetails.Add(new clsEmailAttachmentVO());
                            objBizActionEmail.EmailDetails.AttachmentDetails[i].ID = (long)DALHelper.HandleDBNull(reader["DetailsID"]);
                            objBizActionEmail.EmailDetails.AttachmentDetails[i].Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                            objBizActionEmail.EmailDetails.AttachmentDetails[i].AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                            i++;
                        }
                    }
                    //else
                    //{
                    //    objBizActionEmail.EmailDetails.AttachmentDetails.ID = 0;// (Int64)DALHelper.HandleDBNull(reader["DetailsID"]);
                    //    //objBizActionEmail.EmailDetails.AttachmentDetails.Attachment = ;// (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                    //    //objBizActionEmail.EmailDetails.AttachmentDetails.AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                    //}


                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return objBizActionEmail;
        }

        public override IValueObject GetSMSTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetSMSTemplateDetailsBizActionVO objBizAction = valueObject as clsGetSMSTemplateDetailsBizActionVO;

            try
            {
                clsSMSTemplateVO ObjectVO = objBizAction.SMSDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSMSTemplateDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizAction.ID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (objBizAction.SMSDetails == null)
                            objBizAction.SMSDetails = new clsSMSTemplateVO();

                        objBizAction.ID = (long)DALHelper.HandleDBNull(reader["Id"]);
                        objBizAction.SMSDetails.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objBizAction.SMSDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objBizAction.SMSDetails.EnglishText = (string)DALHelper.HandleDBNull(reader["EnglishText"]);
                        objBizAction.SMSDetails.LocalText = (string)DALHelper.HandleDBNull(reader["LocalText"]);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw;
            }

            return objBizAction;

        }

        public override IValueObject GetRoleGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRoleGeneralDetailsBizActionVO BizActionObj = (clsGetRoleGeneralDetailsBizActionVO)valueObject;
            try
            {

                StringBuilder FilterExpression = new StringBuilder();
                if (BizActionObj.Status.HasValue && BizActionObj.Status.Value == true)
                    FilterExpression.Append("Status = 'True'");
                else if (BizActionObj.Status.HasValue && BizActionObj.Status.Value == false)
                    FilterExpression.Append("Status = 'False'");

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterGeneralDetailsList");
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, "T_UserRoleMaster");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "Id");
                dbServer.AddInParameter(command, "ExtraParameterList", DbType.String, "");
                dbServer.AddInParameter(command, "ExtraParameterDeclarationList", DbType.String, "");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.InputSearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.InputPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.InputStartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.InputMaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.InputSortExpression);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "FilterExpression", DbType.String, "");
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.RoleGeneralDetailsList == null)
                        BizActionObj.RoleGeneralDetailsList = new List<clsUserRoleVO>();
                    while (reader.Read())
                    {
                        clsUserRoleVO RoleVO = new clsUserRoleVO();
                        RoleVO.ID = (long)reader["Id"];
                        RoleVO.Code = (string)reader["Code"];
                        RoleVO.Description = reader["Description"].ToString();
                        RoleVO.Status = (bool)reader["Status"];
                        RoleVO.IsActive = RoleVO.Status;
                        BizActionObj.RoleGeneralDetailsList.Add(RoleVO);
                    }
                }
                reader.NextResult();
                //ioObj.Clear();

                BizActionObj.OutputTotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logManager.LogError(new Guid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return BizActionObj;
        }

        public override IValueObject GetSelectedRoleMenuId(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetSelectedRoleMenuIdBizActionVO obj = (clsGetSelectedRoleMenuIdBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSelectedRoleDetails");
                dbServer.AddInParameter(command, "RoleId", DbType.Int64, (long)obj.RoleId);

                using (DbDataReader ParamReader = (DbDataReader)dbServer.ExecuteReader(command))
                {
                    if (ParamReader.HasRows)
                    {
                        obj.MenuList = new List<clsMenuVO>();
                        while (ParamReader.Read())
                        {
                            //obj.RoleDetails.RoleMenuRights.Add(new RoleMenuRightsItem(Convert.ToInt64(ParamReader["MenuId"]), Convert.ToBoolean(ParamReader["Status"])));
                            obj.MenuList.Add(new clsMenuVO()
                            {
                                ID = Convert.ToInt64(ParamReader["MenuId"]),
                                Status = Convert.ToBoolean(ParamReader["Status"])
                                //IsPrint = (bool)DALHelper.HandleDBNull(ParamReader["IsPrint"]), IsUpdate =(bool)DALHelper.HandleDBNull(ParamReader["IsUpdate"]),
                                //IsRead = (bool)DALHelper.HandleDBNull(ParamReader["IsRead"]),
                                //IsDelete = (bool)DALHelper.HandleDBNull(ParamReader["IsDelete"]),
                                //IsCreate = (bool)DALHelper.HandleDBNull(ParamReader["IsCreate"])
                            });
                        }
                    }

                    ParamReader.NextResult();
                    if (ParamReader.HasRows)
                    {
                        obj.DashBoardList = new List<clsDashBoardVO>();
                        while (ParamReader.Read())
                        {
                            obj.DashBoardList.Add(new clsDashBoardVO()
                            {
                                ID = (long)DALHelper.HandleDBNull(ParamReader["DashBoardID"]),
                                Status = (bool)DALHelper.HandleDBNull(ParamReader["Status"])
                            });
                        }
                    }

                    ParamReader.Close();
                }


            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logManager.LogError(obj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(obj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

        public override IValueObject GetRoleList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRoleListBizActionVO BizActionObj = (clsGetRoleListBizActionVO)valueObject;
            try
            {
                StringBuilder FilterExpression = new StringBuilder();
                if (BizActionObj.Status.HasValue && BizActionObj.Status.Value == true)
                    FilterExpression.Append("Status = 'True'");
                else if (BizActionObj.Status.HasValue && BizActionObj.Status.Value == false)
                    FilterExpression.Append("Status = 'False'");

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterRoleList");

                dbServer.AddInParameter(command, "MasterTableName", DbType.String, "T_UserRoleMaster");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "Id");
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.RoleList.Add(new MasterListItem((long)reader["ID"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return BizActionObj;
        }

        public override IValueObject GetAutoCompleteList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAutoCompleteListVO BizActionObj = (clsGetAutoCompleteListVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.Parent != null)
                {
                    if (BizActionObj.IsDecode == true)
                    {

                        FilterExpression.Append(BizActionObj.Parent.Key.ToString() + " like '%" + Security.base64Encode(BizActionObj.Parent.Value) + "%'");
                    }
                    else
                        FilterExpression.Append(BizActionObj.Parent.Key.ToString() + " like '%" + (BizActionObj.Parent.Value) + "%'");

                }


                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetListForAutoCompleteBox");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "TableName", DbType.String, BizActionObj.TableName);
                dbServer.AddInParameter(command, "ColumnName", DbType.String, BizActionObj.ColumnName);
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<string>();
                    }
                    //Reading the record from reader and stores in list
                    switch (BizActionObj.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                //Add the object value in list
                                BizActionObj.List.Add((string)(reader[BizActionObj.ColumnName].ToString()));
                            }
                            break;

                        case true:
                            while (reader.Read())
                            {
                                //Add the object value in list
                                BizActionObj.List.Add(Security.base64Decode((string)(reader[BizActionObj.ColumnName].ToString())));
                            }
                            break;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return BizActionObj;
        }
        public override IValueObject GetAutoCompleteList_2colums(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAutoCompleteListVO_2Colums BizActionObj = (clsGetAutoCompleteListVO_2Colums)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.Parent != null)
                {
                    if (BizActionObj.IsDecode == true)
                    {
                        FilterExpression.Append(BizActionObj.Parent.Key.ToString() + " like '%" + Security.base64Encode(BizActionObj.Parent.Value) + "%'");
                    }
                    else
                        FilterExpression.Append(BizActionObj.Parent.Key.ToString() + " like '%" + (BizActionObj.Parent.Value) + "%'");
                }
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetListForAutoCompleteBox_2coloums");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "TableName", DbType.String, BizActionObj.TableName);
                dbServer.AddInParameter(command, "ColumnName", DbType.String, BizActionObj.ColumnName);
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<string>();
                    }
                    //Reading the record from reader and stores in list
                    switch (BizActionObj.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                //Add the object value in list
                                BizActionObj.List.Add((string)(reader[BizActionObj.ColumnName].ToString()));
                            }
                            break;

                        case true:
                            while (reader.Read())
                            {
                                //Add the object value in list
                                BizActionObj.List.Add(Security.base64Decode((string)(reader[BizActionObj.ColumnName].ToString())));
                            }
                            break;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetDashBoardList(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            clsGetDashBoardListVO BizActionObj = valueObject as clsGetDashBoardListVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDashBoard");
                DbDataReader reader;

                // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);

                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsDashBoardVO>();
                    while (reader.Read())
                    {
                        clsDashBoardVO objVO = new clsDashBoardVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.List.Add(objVO);
                    }
                }
                reader.NextResult();
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

        public override IValueObject GetPatientMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientMasterListBizActionVO BizActionObj = valueObject as clsGetPatientMasterListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientMaterList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ComboList == null)
                        BizActionObj.ComboList = new List<clsComboMasterBizActionVO>();


                    switch (BizActionObj.IsDecode)
                    {
                        case false:

                            //Add the object value in list

                            while (reader.Read())
                            {
                                clsComboMasterBizActionVO ObjComboList = new clsComboMasterBizActionVO();
                                ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                                ObjComboList.Value = (string)DALHelper.HandleDBNull(reader["Value"]);

                                BizActionObj.ComboList.Add(ObjComboList);
                            }

                            break;

                        case true:



                            while (reader.Read())
                            {
                                clsComboMasterBizActionVO ObjComboList = new clsComboMasterBizActionVO();
                                ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["PatientID"]);

                                string tempstr = (string)DALHelper.HandleDBNull(reader["Value"]);

                                if (!string.IsNullOrEmpty(tempstr))
                                {
                                    string[] words = tempstr.Split(' ');
                                    tempstr = "";
                                    foreach (string word in words)
                                    {
                                        tempstr += Security.base64Decode(word);
                                        tempstr += " ";
                                    }
                                }
                                else
                                    tempstr = "";

                                ObjComboList.Value = tempstr.Trim();


                                BizActionObj.ComboList.Add(ObjComboList);
                            }

                            // BizActionObj.List.Add(Security.base64Decode((string)(reader[BizActionObj.ColumnName].ToString())));

                            break;
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

        public override IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBdMasterBizActionVO BizActionObj = valueObject as clsGetBdMasterBizActionVO;

            
            try
            {
                

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillBdMasterCombobox");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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
            return valueObject;
        }




        public override IValueObject GetDoctorMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorMasterListBizActionVO BizActionObj = valueObject as clsGetDoctorMasterListBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorMaterList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.String, BizActionObj.ID);
                // dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ComboList == null)
                        BizActionObj.ComboList = new List<clsComboMasterBizActionVO>();

                    switch (BizActionObj.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {

                                clsComboMasterBizActionVO ObjComboList = new clsComboMasterBizActionVO();
                                ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                ObjComboList.Value = (string)DALHelper.HandleDBNull(reader["Value"]);
                                ObjComboList.EmailId = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                                BizActionObj.ComboList.Add(ObjComboList);

                            }
                            break;

                        case true:

                            while (reader.Read())
                            {
                                clsComboMasterBizActionVO ObjComboList = new clsComboMasterBizActionVO();
                                ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                ObjComboList.EmailId = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                                string tempstr = (string)DALHelper.HandleDBNull(reader["Value"]);

                                if (!string.IsNullOrEmpty(tempstr))
                                {
                                    string[] words = tempstr.Split(' ');
                                    tempstr = "";
                                    foreach (string word in words)
                                    {
                                        tempstr += Security.base64Decode(word);
                                        tempstr += " ";
                                    }
                                }

                                ObjComboList.Value = tempstr.Trim();
                                BizActionObj.ComboList.Add(ObjComboList);
                            }

                            break;
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

        public override IValueObject GetUserMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetUserMasterListBizActionVO BizActionObj = valueObject as clsGetUserMasterListBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserMaterList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.String, BizActionObj.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();

                    switch (BizActionObj.IsDecode)
                    {
                        case false:
                            while (reader.Read())
                            {
                                //while (reader.Read())     // Commented on 26062018 as first user skipped due to this extra : while (reader.Read()) 
                                //{
                                    MasterListItem ObjComboList = new MasterListItem();
                                    ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                    ObjComboList.Description = (string)DALHelper.HandleDBNull(reader["Value"]);

                                    BizActionObj.MasterList.Add(ObjComboList);
                                //}
                            }
                            break;

                        case true:

                            while (reader.Read())
                            {
                                MasterListItem ObjComboList = new MasterListItem();
                                ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

                                string tempstr = (string)DALHelper.HandleDBNull(reader["Value"]);

                                if (!string.IsNullOrEmpty(tempstr))
                                {
                                    string[] words = tempstr.Split(' ');
                                    tempstr = "";
                                    foreach (string word in words)
                                    {
                                        tempstr += Security.base64Decode(word);
                                        tempstr += " ";
                                    }
                                }

                                ObjComboList.Description = tempstr.Trim();
                                BizActionObj.MasterList.Add(ObjComboList);
                            }

                            break;
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
        #region Added by Shikha
        public override IValueObject AddUpdateMasterList(IValueObject valueObject, clsUserVO userVO)
        {
            clsMasterListVO objItemVO = new clsMasterListVO();
            clsAddUpdateMasterListBizActionVO objItem = valueObject as clsAddUpdateMasterListBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateMasterList");

                dbServer.AddInParameter(command, "MasterTableName", DbType.String, objItemVO.MasterTableName);
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitId);

                //if (objItemVO.Description.Contains("'"))
                //    objItemVO.Description = objItemVO.Description.Replace("'", "''");

                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, objItemVO.AddUnitID);

                dbServer.AddInParameter(command, "By", DbType.Int64, objItemVO.By);
                dbServer.AddInParameter(command, "On", DbType.String, objItemVO.On);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objItemVO.DateTime);
                dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, objItemVO.WindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }

        public override IValueObject GetMasterListDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetMasterListDetailsBizActionVO objItem = valueObject as clsGetMasterListDetailsBizActionVO;
            clsMasterListVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetMasterListDetails");
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, objItem.MasterTableName);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExperssion);

                //For preprocessive & post processive instructions in OT configuration

                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsMasterListVO();
                        objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objItem.ItemMatserDetails.Add(objItemVO);
                    }

                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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
            return objItem;
        }

        #endregion Added by Shikha

        //public override IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsGetDoctorDepartmentDetailsBizActionVO BizAction = (clsGetDoctorDepartmentDetailsBizActionVO)valueObject;
        //    try
        //    {
        //        DbCommand command;

        //        if (BizAction.IsServiceWiseDoctorList == true)
        //        {
        //            command = dbServer.GetStoredProcCommand("CIMS_FillServiceWiseDoctorCombobox");

        //            dbServer.AddInParameter(command, "ServiceId", DbType.Int64, BizAction.ServiceId);
        //            dbServer.AddInParameter(command, "AllRecord", DbType.Boolean, BizAction.AllRecord);
        //        }
        //        else if (BizAction.FromRoster == true)
        //        {
        //            command = dbServer.GetStoredProcCommand("CIMS_FillDoctorForRosterCombobox");

        //            dbServer.AddInParameter(command, "Date", DbType.DateTime, BizAction.Date);

        //        }
        //        else
        //        {
        //            command = dbServer.GetStoredProcCommand("CIMS_FillDoctorCombobox");
        //        }


        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitId);


        //        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentId);

        //        dbServer.AddInParameter(command, "DoctorTypeID", DbType.Int64, BizAction.DoctorTypeID);

        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {

        //            if (BizAction.MasterList == null)
        //            {
        //                BizAction.MasterList = new List<MasterListItem>();
        //            }
        //            //Reading the record from reader and stores in list
        //            while (reader.Read())
        //            {
        //                if (BizAction.FromBill == false && BizAction.AllRecord == false)
        //                {
        //                    if (BizAction.IsServiceWiseDoctorList == false)
        //                    {
        //                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true));
        //                    }
        //                    else
        //                    {
        //                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true, Convert.ToDouble(DALHelper.HandleDBNull((reader["Rate"])))));

        //                    }
        //                }
        //                else if (BizAction.AllRecord == true)
        //                {
        //                    BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (long)reader["ServiceID"], Convert.ToDouble(Convert.ToDecimal((reader["Rate"])))));
        //                }
        //                else
        //                {
        //                    BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (long)reader["SpecializationID"]));

        //                }
        //            }
        //        }
        //    }


        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //    return BizAction;
        //}



        public override IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = (clsGetDoctorDepartmentDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command;

                if (BizAction.IsServiceWiseDoctorList == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_FillServiceWiseDoctorCombobox");

                    dbServer.AddInParameter(command, "ServiceId", DbType.Int64, BizAction.ServiceId);
                    dbServer.AddInParameter(command, "AllRecord", DbType.Boolean, BizAction.AllRecord);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_FillDoctorCombobox");
                    dbServer.AddInParameter(command, "StrClinicID", DbType.String, BizAction.StrClinicID);
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "DoctorTypeID", DbType.Int64, BizAction.DoctorTypeID);

                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentId);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                        if (BizAction.IsServiceWiseDoctorList == false)
                        {
                            BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true));
                        }
                        else
                        {
                            BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true, Convert.ToDouble(DALHelper.HandleDBNull((reader["Rate"])))));
                        }
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
        //by Anjali..................................



        public override IValueObject GetDoctorListBySpecializationID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = (clsGetDoctorDepartmentDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorListBySpecializationID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizAction.SpecializationID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.MasterList == null)
                        BizAction.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Name"].ToString(), true));                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }
        //by Anjali..................................

        public override IValueObject GetOtherThanReferralDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = (clsGetDoctorDepartmentDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command;

              
                 command = dbServer.GetStoredProcCommand("CIMS_FillDoctorComboboxOtherThanReferral");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "ReferralID", DbType.Int64, BizAction.ReferralID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizAction.DepartmentId);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                        if (BizAction.IsServiceWiseDoctorList == false)
                        {
                            BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true));
                        }
                        else
                        {
                            BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true, Convert.ToDouble(DALHelper.HandleDBNull((reader["Rate"])))));
                        }
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
            //.........................................
        public override IValueObject GetRefDoctor(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRefernceDoctorBizActionVO BizAction = (clsGetRefernceDoctorBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillRefDoctor");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "IsFromVisit", DbType.Boolean, BizAction.IsFromVisit);

                DbDataReader reader;

                

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.ComboList == null)
                        BizAction.ComboList = new List<clsComboMasterBizActionVO>();


                    while (reader.Read())
                    {

                        clsComboMasterBizActionVO ObjComboList = new clsComboMasterBizActionVO();
                        ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjComboList.Value = (string)DALHelper.HandleDBNull(reader["Value"]);

                        BizAction.ComboList.Add(ObjComboList);

                    }
                    reader.Close();
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


        //rohinee dated 28/12//2015 to get all doctors
        public override IValueObject GetAllDoctorList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorListForComboBizActionVO BizAction = (clsGetDoctorListForComboBizActionVO)valueObject;
            try
            {   
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillAllDoctors");
                DbDataReader reader; 
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.MasterList == null)
                        BizAction.MasterList = new List<MasterListItem>();


                    while (reader.Read())
                    {
                        MasterListItem msaterlist = new MasterListItem();
                        msaterlist.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        msaterlist.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        msaterlist.SpecializationID = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);// UOMID is a Specialization ID
                        BizAction.MasterList.Add(msaterlist);

                    }
                    reader.Close();
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

        public override IValueObject AddUpdateSurrogactAgencyDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsAddUpdateSurrogactAgencyMasterBizActionVO BizActionObj = valueObject as clsAddUpdateSurrogactAgencyMasterBizActionVO;


            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSurrogactAgency");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Affilatedyear", DbType.DateTime, BizActionObj.AgencyDetails.Affilatedyear);
                dbServer.AddInParameter(command, "ReferralTypeID", DbType.Int64, BizActionObj.AgencyDetails.ReferralTypeID);
                dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.AgencyDetails.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.AgencyDetails.Description);
                dbServer.AddInParameter(command, "SourceEmail", DbType.String, BizActionObj.AgencyDetails.SourceEmail);
                dbServer.AddInParameter(command, "SourceContactNo", DbType.String, BizActionObj.AgencyDetails.SourceContactNo);
                dbServer.AddInParameter(command, "SourceAddress", DbType.String, BizActionObj.AgencyDetails.SourceAddress);
                dbServer.AddInParameter(command, "RegistrationNo", DbType.String, BizActionObj.AgencyDetails.RegistrationNo);
                dbServer.AddInParameter(command, "AffilatedBy", DbType.String, BizActionObj.AgencyDetails.AffilatedBy);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.AgencyDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.AgencyDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.AgencyDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;




        }
        public override IValueObject GetSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            //throw new NotImplementedException();
            clsGetSurrogactAgencyMasterBizActionVO BizActionObj = valueObject as clsGetSurrogactAgencyMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSurrogactAgency");
                DbDataReader reader;

                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.AgencyDetails.Description);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.AgencyDetailsList == null)
                        BizActionObj.AgencyDetailsList = new List<clsSurrogateAgencyMasterVO>();

                    while (reader.Read())
                    {
                        clsSurrogateAgencyMasterVO Details = new clsSurrogateAgencyMasterVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.ReferralTypeID = (long)DALHelper.HandleDBNull(reader["ReferralTypeID"]);
                        Details.ReferralName = (string)DALHelper.HandleDBNull(reader["ReferralName"]);
                        Details.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        Details.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        Details.RegistrationNo = (string)DALHelper.HandleDBNull(reader["RegistrationNo"]);
                        Details.SourceEmail = (string)DALHelper.HandleDBNull(reader["SourceEmail"]);
                        Details.SourceContactNo = (string)DALHelper.HandleDBNull(reader["SourceContactNo"]);
                        Details.SourceAddress = (string)DALHelper.HandleDBNull(reader["SourceAddress"]);
                        Details.AffilatedBy = (string)DALHelper.HandleDBNull(reader["AffilatedBy"]);
                        Details.Affilatedyear = (DateTime?)DALHelper.HandleDate(reader["Affilatedyear"]);
                        Details.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.AgencyDetailsList.Add(Details);
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
        public override IValueObject UpdateStatusSurrogactAgencyDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsSurrogateAgencyMasterVO objItemVO = new clsSurrogateAgencyMasterVO();
          
            clsUpdateStatusSurrogactAgencyMasterBizActionVO BizActionObj = valueObject as clsUpdateStatusSurrogactAgencyMasterBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();

                objItemVO = BizActionObj.AgencyDetailsList[0];
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStatusSurrogactAgency");


                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.AgencyDetails.UnitID);
                //dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.AgencyDetails.ID);
                //dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.AgencyDetails.Status);


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.AgencyDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;

        }

        public override IValueObject GetRadiologist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRadiologistBizActionVO BizAction = (clsGetRadiologistBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillRadiologistCombobox");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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
        public override IValueObject GetEmbryologist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEmbryologistBizActionVO BizAction = (clsGetEmbryologistBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillEmbryologistCombobox");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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

        public override IValueObject GetAnesthetist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAnesthetistBizActionVO BizAction = (clsGetAnesthetistBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillAnesthetistCombobox");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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

        public override IValueObject GetPathologist(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathologistBizActionVO BizAction = (clsGetPathologistBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillPathologistCombobox");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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


        public override IValueObject GetPathoUsers(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoUsersBizActionVO BizAction = (clsGetPathoUsersBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoUsers");
                dbServer.AddInParameter(command, "MenuID", DbType.Int64, BizAction.MenuID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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

        public override IValueObject GetMasterSearchList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetSearchMasterListBizActionVO BizActionObj = valueObject as clsGetSearchMasterListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterSearchList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable);
                dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);


                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objVO = new MasterListItem();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);

                        BizActionObj.MasterList.Add(objVO);

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

        public override IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetMasterListByTableNameBizActionVO BizActionObj = (clsGetMasterListByTableNameBizActionVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                //if (BizActionObj.IsActive.HasValue)
                //      FilterExpression.Append("IsActive = '" + BizActionObj.IsActive.Value.ToString().ToUpper() + "'");

                if (BizActionObj.Parent != null)
                {
                    if (FilterExpression.Length > 0)
                        FilterExpression.Append(" And ");
                    FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
                }


                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterList");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;
        }

        // Added By Harish
        // Date 1 Aug 2011
        // For Dynamic get Master list from Any Master Table with Dynamic Column Names
        public override IValueObject GetMasterListByTableNameAndColumnName(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetMasterListByTableNameAndColumnNameBizActionVO BizActionObj = (clsGetMasterListByTableNameAndColumnNameBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterListByTableNameAndColumnName");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "ColumnName", DbType.String, BizActionObj.ColumnName.ToString());
                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return BizActionObj;
        }

        public override IValueObject GetColumnListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetColumnListByTableNameBizActionVO BizActionObj = (clsGetColumnListByTableNameBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetColumnListByTableName");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.ColumnList == null)
                    {
                        BizActionObj.ColumnList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.ColumnList.Add(new MasterListItem(Convert.ToInt64(reader["Id"]), reader["ColumnName"].ToString()));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return BizActionObj;
        }

        //by rohini dated 4.2.16

        public override IValueObject GetPathoFasting(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetPathoFastingBizActionVO BizActionObj = valueObject as clsGetPathoFastingBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoFastingDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.id);
                //dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable);
                //dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                //dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);     

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objVO = new MasterListItem();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objVO.IsHrs = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsHrs"]));
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.MasterList.Add(objVO);
                    }

                }
                reader.NextResult();
             //   BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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
        //----------by rohini
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="userVO"></param>
        /// <returns></returns>
        public override IValueObject UpdateStausMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsMasterListVO objItemVO = new clsMasterListVO();
            clsUpdateStatusMasterBizActionVO objItem = valueObject as clsUpdateStatusMasterBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateStatusMaster");

                dbServer.AddInParameter(command, "MasterTableName", DbType.String, objItemVO.MasterTableName);
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, objItemVO.AddUnitID);

                dbServer.AddInParameter(command, "By", DbType.Int64, objItemVO.By);
                dbServer.AddInParameter(command, "On", DbType.String, objItemVO.On);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objItemVO.DateTime);
                dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, objItemVO.WindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }

        //public override IValueObject AddUpdateParameter(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    throw new NotImplementedException();
        //}

        public override IValueObject GetParameterByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoParameterByIDBizActionVO BizAction = valueObject as clsGetPathoParameterByIDBizActionVO;
            try
            {
                clsPathoParameterMasterVO objVO = BizAction.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoParameterByID");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.Details.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.Details == null)
                            BizAction.Details = new clsPathoParameterMasterVO();
                        BizAction.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizAction.Details.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        BizAction.Details.ParameterDesc = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizAction.Details.PrintName = (string)DALHelper.HandleDBNull(reader["PrintName"]);
                        BizAction.Details.ParamUnit = (long)DALHelper.HandleDBNull(reader["ParameterUnitId"]);
                        BizAction.Details.IsNumeric = (bool)DALHelper.HandleDBNull(reader["IsNumeric"]);
                        BizAction.Details.Formula = (string)DALHelper.HandleDBNull(reader["Formula"]);
                        //by rohinee dated 22/11/16 FOR ID OF FORMULA
                        BizAction.Details.FormulaID = (string)DALHelper.HandleDBNull(reader["FormulaID"]);
                        //
                        BizAction.Details.TextRange = (string)DALHelper.HandleDBNull(reader["TextRange"]);
                        BizAction.Details.stringstrParameterUnitName = (string)DALHelper.HandleDBNull(reader["ParameterUnitName"]);
                        //by rohini dated 15/1/2016                      
                       // BizAction.Details.IsFlagReflex = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFlagReflex"]));
                        BizAction.Details.DeltaCheckPer = Convert.ToDouble(DALHelper.HandleDBNull(reader["DeltaCheckPer"]));
                        BizAction.Details.Technique = (string)DALHelper.HandleDBNull(reader["TechniqueUsed"]);

                        //added by neena 19 sep 2017 for pathology linking]
                        BizAction.Details.ExcutionCalenderParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExcutionCalenderParameterID"]));
                        //
                    
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    BizAction.Details.DefaultValues = new List<clsPathoParameterDefaultValueMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoParameterDefaultValueMasterVO obj = new clsPathoParameterDefaultValueMasterVO();
                        obj.ID = (long)DALHelper.HandleDBNull(reader["DefaultValueID"]);
                        obj.Category = (string)DALHelper.HandleDBNull(reader["Category"]);
                        obj.CategoryID = (long)DALHelper.HandleDBNull(reader["CategoryID"]);
                        obj.IsAge = (bool)DALHelper.HandleDBNull(reader["IsAgeApplicable"]);
                        //if (obj.IsAge == true)
                        //{
                        obj.AgeFrom = (Double)DALHelper.HandleDBNull(reader["AgeFrom"]);
                        obj.AgeTo = (Double)DALHelper.HandleDBNull(reader["AgeTo"]);
                        //}
                        //else
                        //{
                        //    obj.AgeFrom = null;

                        //}
                        obj.MinValue = (double)DALHelper.HandleDBNull(reader["MinValue"]);
                        obj.MaxValue = (double)DALHelper.HandleDBNull(reader["MaxValue"]);
                        obj.DefaultValue = (double)DALHelper.HandleDBNull(reader["DefaultValue"]);
                        //added by rohini dated 15.1.2016
                        obj.MinImprobable  = Convert.ToDouble(DALHelper.HandleDBNull(reader["MinImprobable"]));
                        obj.MaxImprobable = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaxImprobable"]));
                        obj.MachineID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineID"]));
                        obj.Note  = Convert.ToString(DALHelper.HandleDBNull(reader["Note"]));
                        obj.Machine = Convert.ToString(DALHelper.HandleDBNull(reader["Machine"]));
                        obj.IsReflexTesting = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReflexTesting"]));
                        obj.AgeValue = Convert.ToString(DALHelper.HandleDBNull(reader["AgeValue"]));
                        obj.HighReffValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["HighReffValue"]));
                        obj.LowReffValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["LowReffValue"]));
                        obj.UpperPanicValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["UpperPanicValue"]));
                        obj.LowerPanicValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["LowerPanicValue"]));

                        obj.LowReflexValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["LowReflex"]));
                        obj.HighReflexValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["HighReflex"]));

                        obj.VaryingReferences = Convert.ToString(DALHelper.HandleDBNull(reader["VaryingReferences"]));

                        BizAction.Details.DefaultValues.Add(obj);
                       
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    BizAction.Details.Items = new List<clsPathoParameterHelpValueMasterVO>();
                    while (reader.Read())
                    {
                        clsPathoParameterHelpValueMasterVO obj = new clsPathoParameterHelpValueMasterVO();
                        obj.ID = (long)DALHelper.HandleDBNull(reader["HelpID"]);
                        obj.strHelp = (string)DALHelper.HandleDBNull(reader["HelpValue"]);
                        obj.IsDefault = (bool)DALHelper.HandleDBNull(reader["IsDefault"]);
                        obj.IsAbnoramal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAbnoramal"]));                        
                        BizAction.Details.Items.Add(obj);
                    }
                }
                reader.NextResult();

                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }

            return BizAction;
        }

        // BY BHUSHAN . . . . . . .
        public override IValueObject GetPathoParameter(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoParameterUnitBizActionVO BizAction = (clsGetPathoParameterUnitBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPathoParameterUnit");
                dbServer.AddInParameter(command, "ParamID", DbType.Int64, BizAction.ParamID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["ID"], reader["Description"].ToString()));
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return BizAction;
        }

        public override IValueObject GetCurrencyMasterListDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetCurrencyMasterListDetailsBizActionVO objItem = valueObject as clsGetCurrencyMasterListDetailsBizActionVO;
            clsCurrencyMasterListVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetCurrencyMasterListDetails");

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExperssion);

                //For preprocessive & post processive instructions in OT configuration

                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsCurrencyMasterListVO();
                        objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Symbol = Convert.ToString(DALHelper.HandleDBNull(reader["Symbol"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objItem.ItemMatserDetails.Add(objItemVO);
                    }

                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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
            return objItem;
        }

        public override IValueObject AddUpdateCurrencyMasterList(IValueObject valueObject, clsUserVO userVO)
        {
            clsCurrencyMasterListVO objItemVO = new clsCurrencyMasterListVO();
            clsAddUpdateCurrencyMasterListBizActionVO objItem = valueObject as clsAddUpdateCurrencyMasterListBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCurrencyMasterList");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitId);

                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Symbol", DbType.String, objItemVO.Symbol);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, objItemVO.AddUnitID);

                dbServer.AddInParameter(command, "By", DbType.Int64, objItemVO.By);
                dbServer.AddInParameter(command, "On", DbType.String, objItemVO.On);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objItemVO.DateTime);
                dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, objItemVO.WindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;
        }

        #region For IPD Module

        //By Anjali on 03/04/2014
        public override IValueObject GetMasterListForConsent(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetMasterListConsentBizActionVO BizActionObj = (clsGetMasterListConsentBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterList_New");
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, BizActionObj.FilterExpression);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if (BizActionObj.MasterTable.ToString() == "M_PathoTestMaster" || BizActionObj.MasterTable.ToString() == "M_RadTestMaster")
                        {
                            BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (long)reader["CategoryID"]));
                        }
                        else
                        {
                            BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), reader["TemplateName"].ToString()));//HandleDBNull(reader["Date"])));
                        }
                        if (BizActionObj.MasterTable.ToString() == "M_PathoTestMaster" && BizActionObj.IsSubTest)
                        {
                            BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (bool)reader["IsSubTest"]));
                        }
                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;
            }

            return BizActionObj;
        }

        public override IValueObject DeptFromSubSpecilization(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsFillDepartmentBizActionVO BizAction = (clsFillDepartmentBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("FillDeptFromSubSp");


                // dbServer.AddInParameter(command, "SubSpecilization", DbType.Int64, BizAction.SubSpecilizationID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        MasterListItem Obj = new MasterListItem();
                        Obj.ID = (long)DALHelper.HandleIntegerNull(reader["ID"]);
                        Obj.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        Obj.FilterID = (long)DALHelper.HandleDBNull(reader["SubSpecializationID"]);
                        BizAction.MasterList.Add(Obj);

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

        #endregion

        #region For Pathology Additions

        public override IValueObject GetUnitContactNo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitContactNoBizActionVO objBizAction = valueObject as clsGetUnitContactNoBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUnitContactNo");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID ", DbType.Int64, objBizAction.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objBizAction.ContactNo = (string)DALHelper.HandleDBNull(reader["ContactNo"]);
                        objBizAction.SuccessStatus = true;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return objBizAction;

        }

        #endregion

        //rohinee
        //public override IValueObject GetAuthorisedUserMasterList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetAuthorisedUserMasterListBizActionVO BizActionObj = valueObject as clsGetAuthorisedUserMasterListBizActionVO;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAuthorisedUserMaterList");
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
        //        DbDataReader reader;
        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        //if (reader.HasRows)
        //        //{
        //        //    BizActionObj.CompAdvAuthMasterList = new List<MasterListItem>();
        //        //    while (reader.Read())
        //        //    {
        //        //        MasterListItem ObjComboList = new MasterListItem();
        //        //        ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //        //        ObjComboList.Description = (string)DALHelper.HandleDBNull(reader["Value"]);

        //        //        BizActionObj.CompAdvAuthMasterList.Add(ObjComboList);
        //        //    }
        //        //}
        //        if (reader.HasRows)
        //        {
        //            BizActionObj.CreditAuthMasterList = new List<MasterListItem>();
        //            while (reader.Read())
        //            {
        //                MasterListItem ObjComboList = new MasterListItem();
        //                ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //                ObjComboList.Description = (string)DALHelper.HandleDBNull(reader["Value"]);
        //                BizActionObj.CreditAuthMasterList.Add(ObjComboList);
        //            }
        //        }
        //        if (reader.NextResult() && reader.HasRows)
        //        {
        //            BizActionObj.ConcessionAuthMasterList = new List<MasterListItem>();
        //            while (reader.Read())
        //            {
        //                MasterListItem ObjComboList = new MasterListItem();
        //                ObjComboList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //                ObjComboList.Description = (string)DALHelper.HandleDBNull(reader["Value"]);
        //                BizActionObj.ConcessionAuthMasterList.Add(ObjComboList);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return valueObject;
        //}
        //rohinee
        public override IValueObject GetMasterNames(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMasterNamesBizActionVO BizActionObj = valueObject as clsGetMasterNamesBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetMasterNames");
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.objPrintMasterList == null)
                        BizActionObj.objPrintMasterList = new List<clsPrintMasterVO>();
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        clsPrintMasterVO objVO = new clsPrintMasterVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        objVO.ViewName = Convert.ToString(DALHelper.HandleDBNull(reader["ViewName"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.MasterList.Add(new MasterListItem() { ID = objVO.ID, Code = objVO.ViewName, Description = objVO.Name, Status = objVO.Status });
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
        //rohinee
        public override IValueObject GetDataToPrint(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDatatoPrintMasterBizActionVO BizAction = valueObject as clsGetDatatoPrintMasterBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetDatatoPrintMasterBox");
                DbDataReader reader;
                dbServer.AddInParameter(command, "id", DbType.Int64, BizAction.id);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.MasterList == null)
                        BizAction.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        clsMasterDataVO objVO = new clsMasterDataVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.column0 = Convert.ToString(DALHelper.HandleDBNull(reader["Test"]));
                        objVO.column1 = Convert.ToString(DALHelper.HandleDBNull(reader["Test1"]));
                        objVO.column2 = Convert.ToString(DALHelper.HandleDBNull(reader["Test2"]));
                        BizAction.MasterList.Add(new MasterListItem() { ID = objVO.ID, Code = objVO.Code, Description = objVO.Description, Status = true, column0 = objVO.column0, column1 = objVO.column1, column2 = objVO.column2, PurchaseRate = objVO.Rate });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        //By Anjali.................................................................................................
        public override IValueObject GetStoreForComboBox(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillStoreMasterListBizActionVO BizAction = valueObject as clsFillStoreMasterListBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetStorListForComboBox");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "StoreType", DbType.Int32, BizAction.StoreType);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.StoreMasterDetails == null)
                        BizAction.StoreMasterDetails = new List<clsStoreVO>();
                    while (reader.Read())
                    {
                        clsStoreVO objVO = new clsStoreVO();
                        objVO.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));
                        objVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objVO.IsQuarantineStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsQuarantineStore"]));
                        objVO.ParentStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParentStoreID"]));
                        BizAction.StoreMasterDetails.Add(objVO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }
        //...........................................................................................................

        public override IValueObject CheckDuplicasy(IValueObject valueObject, clsUserVO objUserVO)  // By Umesh
        {
            clsCheckDuplicasyBizActionVO BizAction = valueObject as clsCheckDuplicasyBizActionVO;
            try
            {
                DbCommand command = null;
                         //command = dbServer.GetStoredProcCommand("CIMS_CheckDuplicasy"); Commnted By CDS
              //  DbDataReader reader;
                foreach (var item in BizAction.lstDuplicasy)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_CheckDuplicasy");

                    dbServer.AddInParameter(command, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command, "StoreID", DbType.Int64, item.StoreID);
                    dbServer.AddInParameter(command, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    dbServer.AddInParameter(command, "CostPrice", DbType.Double, item.CostPrice);
                    dbServer.AddInParameter(command, "MRP", DbType.Double, item.MRP);
                    dbServer.AddInParameter(command, "TransactionTypeID", DbType.Int16, (Int16)item.TransactionTypeID);
                    dbServer.AddInParameter(command, "IsBatchRequired", DbType.Boolean, item.IsBatchRequired);
             //       dbServer.AddInParameter(command, "IsFromOpeningBalance", DbType.Boolean, item.IsFromOpeningBalance);

                    dbServer.AddInParameter(command, "IsFree", DbType.Boolean, item.IsFree);  // For Free Items

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                    BizAction.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                    if (BizAction.SuccessStatus == 1)
                    {
                        BizAction.ItemName = item.ItemName;
                        BizAction.BatchCode = item.BatchCode;
                        BizAction.IsBatchRequired = item.IsBatchRequired;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }


        //by neena

        public override IValueObject AddUpdateCleavageGradeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsAddUpdateCleavageGradeMasterBizActionVO BizActionObj = valueObject as clsAddUpdateCleavageGradeMasterBizActionVO;
           
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCleavageGrade");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);  
                dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.CleavageDetails.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.CleavageDetails.Description);
                dbServer.AddInParameter(command, "Name", DbType.String, BizActionObj.CleavageDetails.Name);
                dbServer.AddInParameter(command, "ApplyTo", DbType.String, BizActionObj.CleavageDetails.ApplyTo);
                dbServer.AddInParameter(command, "Flag", DbType.String, BizActionObj.CleavageDetails.Flag);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.CleavageDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.CleavageDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.CleavageDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;




        }


        public override IValueObject GetMarketingExecutivesList(IValueObject valueObject, clsUserVO userVO)
        {           
            DbDataReader reader = null;
            clsGetMarketingExecutivesListVO objBizAction = valueObject as clsGetMarketingExecutivesListVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetMarketingExecutivesList");
                dbServer.AddInParameter(command, "IsMarketingExecutives", DbType.String, objBizAction.IsMarketingExecutives);
                dbServer.AddInParameter(command, "UnitID", DbType.String, objBizAction.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {               
                    objBizAction.MarketingExecutivesList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objItemMaster = new MasterListItem();
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["MarketingExecutivesName"]));
                        objItemMaster.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objBizAction.MarketingExecutivesList.Add(objItemMaster);
                    }
                }              

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
    }
}
