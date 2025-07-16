using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsOtConfigDAL : clsBaseOtConfigDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// 

        string ImgIP = string.Empty;
        string ImgVirtualDir = string.Empty;
        string ImgSaveLocation = string.Empty;
        private clsOtConfigDAL()
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
                ImgIP = System.Configuration.ConfigurationManager.AppSettings["RegImgIP"];
                ImgVirtualDir = System.Configuration.ConfigurationManager.AppSettings["RegImgVirtualDir"];
                ImgSaveLocation = System.Configuration.ConfigurationManager.AppSettings["ImgSavingLocation"];

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Adds Ot Table Details
        /// </summary>
        /// <param name="valueObject">clsAddUpdateOTTableDetailsBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsAddUpdateOTTableDetailsBizActionVO object</returns>
        public override IValueObject AddUpdateOtTableDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsOTTableVO objItemVO = new clsOTTableVO();
            clsAddUpdateOTTableDetailsBizActionVO objItem = valueObject as clsAddUpdateOTTableDetailsBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.OTTableMasterMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTTable");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "OTTheatreId", DbType.Int64, objItemVO.OTTheatreID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        /// <summary>
        /// Fills Ot Table List
        /// </summary>
        /// <param name="valueObject">clsGetOTTableDetailsBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetOTTableDetailsBizActionVO object</returns>
        public override IValueObject GetOtTableDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetOTTableDetailsBizActionVO objItem = valueObject as clsGetOTTableDetailsBizActionVO;
            clsOTTableVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetOtTableDetails");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsOTTableVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.OTTheatreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTheatreId"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.TheatreName = Convert.ToString(DALHelper.HandleDBNull(reader["TheatreName"]));
                        objItem.OtTableMatserDetails.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = Convert.ToInt64(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;

        }
        /// <summary>
        /// Fills patient config combo with patinet related fields
        /// </summary>
        /// <param name="valueObject">clsGetPatientConfigFieldsBizActionVO</param>
        /// <param name="UserVo"><clsUserVO/param>
        /// <returns>clsGetPatientConfigFieldsBizActionVO object</returns>
        public override IValueObject GetConfig_Patient_Fields(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetPatientConfigFieldsBizActionVO objItem = valueObject as clsGetPatientConfigFieldsBizActionVO;
            clsPatientFieldsConfigVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetConfig_Patient_Fields");

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsPatientFieldsConfigVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.TableName = Convert.ToString(DALHelper.HandleDBNull(reader["TableName"]));
                        objItemVO.FieldName = Convert.ToString(DALHelper.HandleDBNull(reader["FieldName"]));
                        objItemVO.FieldColumn = Convert.ToString(DALHelper.HandleDBNull(reader["FieldColumn"]));
                        objItem.OtPateintConfigFieldsMatserDetails.Add(objItemVO);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;

        }

        /// <summary>
        /// Adds or Updates Consent Master Details
        /// </summary>
        /// <param name="valueObject">clsAddUpdateConsentMasterBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsAddUpdateConsentMasterBizActionVO object</returns>
        public override IValueObject AddUpdateConsentMaster(IValueObject valueObject, clsUserVO UserVo)
        {

            clsConsentMasterVO objItemVO = new clsConsentMasterVO();
            clsAddUpdateConsentMasterBizActionVO objItem = valueObject as clsAddUpdateConsentMasterBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.OTTableMasterMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateConsentMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                //dbServer.AddInParameter(command, "ConsentType", DbType.Int64, objItemVO.ConsentType);//Added By Ashish Thombre
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddInParameter(command, "TemplateName", DbType.String, objItemVO.TemplateName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        /// <summary>
        /// fills consent master list
        /// </summary>
        /// <param name="valueObject"> clsGetConsentMasterBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetConsentMasterBizActionVO object</returns>
        public override IValueObject GetConsentMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetConsentMasterBizActionVO objItem = valueObject as clsGetConsentMasterBizActionVO;
            clsConsentMasterVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetConsentMaster");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsConsentMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objItemVO.ConsentType = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentType"]));//Added By Ashish Thombre
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]));
                        objItem.ConsentMatserDetails.Add(objItemVO);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        /// <summary>
        /// Gets Instruction details
        /// </summary>
        public override IValueObject GetInstructionDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetInstructionDetailsBizActionVO objItem = valueObject as clsGetInstructionDetailsBizActionVO;
            clsInstructionMasterVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetInstructionDetails");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "FilterCriteria", DbType.String, objItem.FilterCriteria);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsInstructionMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.SelectInstruction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstructionType"]));
                        objItemVO.SelectInstruction = objItemVO.Instruction.FirstOrDefault(q => q.ID == objItemVO.SelectInstruction.ID);
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItem.InstructionMasterDetails.Add(objItemVO);
                        // ObjTemp.SelectedPrintName.ID = (long)DALHelper.HandleDBNull(reader["PrintNameType"]);
                        //ObjTemp.SelectedPrintName = ObjTemp.PrintName.FirstOrDefault(q => q.ID == ObjTemp.SelectedPrintName.ID);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }


        /// <summary>
        /// Gets Instruction details by instruction id
        /// </summary>
        public override IValueObject GetInstructionDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetInstructionDetailsByIDBizActionVO BizAction = valueObject as clsGetInstructionDetailsByIDBizActionVO;
            try
            {
                clsInstructionMasterVO objVO = BizAction.InstructionDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetInstructionDetailsByID");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.InstructionDetails == null)
                            BizAction.InstructionDetails = new clsInstructionMasterVO();
                        BizAction.InstructionDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizAction.InstructionDetails.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        BizAction.InstructionDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizAction.InstructionDetails.SelectInstruction.ID = (long)DALHelper.HandleDBNull(reader["InstructionType"]);
                        BizAction.InstructionDetails.SelectInstruction = BizAction.InstructionDetails.Instruction.FirstOrDefault(q => q.ID == BizAction.InstructionDetails.SelectInstruction.ID);
                        //ObjTemp.SelectedPrintName = ObjTemp.PrintName.FirstOrDefault(q => q.ID == ObjTemp.SelectedPrintName.ID);
                        BizAction.InstructionDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizAction.InstructionDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //BizAction.InstructionDetails.Formula = (string)DALHelper.HandleDBNull(reader["Formula"]);
                        //BizAction.InstructionDetails.TextRange = (string)DALHelper.HandleDBNull(reader["TextRange"]);
                        //BizAction.InstructionDetails.stringstrParameterUnitName = (string)DALHelper.HandleDBNull(reader["ParameterUnitName"]);
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


        /// <summary>
        /// Updates instruction status
        /// </summary>
        public override IValueObject UpdateInstructionStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateInstructionStatusBizActionVO bizObject = valueObject as clsUpdateInstructionStatusBizActionVO;

            try
            {
                clsInstructionMasterVO objTemp = new clsInstructionMasterVO();
                objTemp = bizObject.InstructionTempStatus;
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateInstructionStatus");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objTemp.ID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objTemp.Status);
                dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.InstructionTempStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                throw;
            }

            return bizObject;
        }


        /// <summary>
        /// Add Update instruction master
        /// </summary>
        public override IValueObject AddUpdateInstructionDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateInstructionDetailsBizActionVO objInstruction = valueObject as clsAddUpdateInstructionDetailsBizActionVO;
            clsInstructionMasterVO Inst = objInstruction.InstMaster;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateInstructionMaster");

                if (objInstruction.InstMaster.ID == 0)
                {//add new
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {//modify
                    dbServer.AddInParameter(command, "ID", DbType.Int64, Inst.ID);
                    //dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                }

                dbServer.AddInParameter(command, "Code", DbType.String, objInstruction.InstMaster.Code);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Description", DbType.String, objInstruction.InstMaster.Description);
                dbServer.AddInParameter(command, "InstructionType", DbType.Int64, objInstruction.InstMaster.SelectInstruction.ID);
                //dbServer.AddInParameter(command, "Status", DbType.Boolean, "True");
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //AddCommonParametersForAddUpdateParameter(command, Inst, UserVo);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objInstruction.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objInstruction;
        }

        private void AddCommonParametersForAddUpdateParameter(DbCommand command, clsInstructionMasterVO objInstruction, clsUserVO objUserVO)
        {
            try
            {
                dbServer.AddInParameter(command, "Code", DbType.String, objInstruction.Code);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "ParameterUnitId", DbType.Int64, objInstruction.ParamUnit);
                dbServer.AddInParameter(command, "Description", DbType.String, objInstruction.Description);
                //dbServer.AddInParameter(command, "PrintName", DbType.String, objInstruction.PrintName);
                //dbServer.AddInParameter(command, "IsNumeric", DbType.Boolean, objInstruction.IsNumeric);
                dbServer.AddInParameter(command, "InstructionType", DbType.Int64, objInstruction.SelectInstruction.ID);
                //if (objParameter.IsNumeric == false)
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objInstruction.Status);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Add update procedure master
        /// </summary>
        public override IValueObject AddProcedureMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddProcedureMasterBizActionVO BizActionObj = valueObject as clsAddProcedureMasterBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureMaster");
                command.Connection = con;

                dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.ProcDetails.Code);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ProcDetails.ServiceID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.ProcDetails.Description);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ProcDetails.Status);
                dbServer.AddInParameter(command, "Duration", DbType.String, BizActionObj.ProcDetails.Duration);
                dbServer.AddInParameter(command, "ProcedureTypeID", DbType.Int64, BizActionObj.ProcDetails.ProcedureTypeID);
                dbServer.AddInParameter(command, "RecommandedAnesthesiaTypeID", DbType.Int64, BizActionObj.ProcDetails.RecommandedAnesthesiaTypeID);
                dbServer.AddInParameter(command, "OperationTheatreID", DbType.Int64, BizActionObj.ProcDetails.OperationTheatreID);
                dbServer.AddInParameter(command, "OTTableID", DbType.Int64, BizActionObj.ProcDetails.OTTableID);
                dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.ProcDetails.Remark);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ProcDetails.ID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.ProcDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                DbCommand Deletecommand = null;
                Deletecommand = dbServer.GetStoredProcCommand("CIMS_DeleteProcedureDetails");
                dbServer.AddInParameter(Deletecommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(Deletecommand, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                int intstatusDelete = dbServer.ExecuteNonQuery(Deletecommand, trans);

                foreach (var item in BizActionObj.ProcDetails.ConsentList)
                {
                    DbCommand command1 = null;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureConsentDetails");
                    command1.Connection = con;
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                    dbServer.AddInParameter(command1, "ConsentID", DbType.Int64, item.ConsentID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                }

                foreach (var item in BizActionObj.ProcDetails.InstructionList)
                {
                    DbCommand command2 = null;
                    command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureInstructionDetails");
                    command2.Connection = con;

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                    dbServer.AddInParameter(command2, "InstructionID", DbType.Int64, item.InstructionID);
                    dbServer.AddInParameter(command2, "InstructionTypeID", DbType.Int64, item.InstructionTypeID);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                }

                foreach (var item in BizActionObj.ProcDetails.ItemList)
                {
                    DbCommand command3 = null;
                    command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureItemDetails");
                    command3.Connection = con;

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                    dbServer.AddInParameter(command3, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command3, "ItemCode", DbType.String, item.ItemCode);
                    dbServer.AddInParameter(command3, "ItemName", DbType.String, item.ItemName);
                    dbServer.AddInParameter(command3, "Quantity", DbType.Double, item.Quantity);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                }

                foreach (var item in BizActionObj.ProcDetails.ServiceList)
                {
                    DbCommand command4 = null;
                    command4 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureServiceDetails");
                    command4.Connection = con;

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                    dbServer.AddInParameter(command4, "ServiceCode", DbType.String, item.ServiceCode);
                    dbServer.AddInParameter(command4, "ServiceName", DbType.String, item.ServiceName);
                    dbServer.AddInParameter(command4, "ServiceType", DbType.String, item.ServiceType);
                    dbServer.AddInParameter(command4, "GroupName", DbType.String, item.GroupName);
                    dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);

                }

                foreach (var item in BizActionObj.ProcDetails.StaffList)
                {
                    DbCommand command5 = null;
                    command5 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureStaffDetails");
                    command5.Connection = con;

                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                    dbServer.AddInParameter(command5, "IsDoctor", DbType.Boolean, item.IsDoctor);
                    dbServer.AddInParameter(command5, "DocOrStaffCode", DbType.String, item.DocOrStaffCode);
                    dbServer.AddInParameter(command5, "NoofDocOrStaff", DbType.Int64, item.NoofDocOrStaff);
                    dbServer.AddInParameter(command5, "DocSpecialization", DbType.String, item.DocClassification);
                    dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                }

                foreach (var item in BizActionObj.ProcDetails.CheckList)
                {
                    DbCommand command6 = null;
                    command6 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcChecklistUsingProcedureMaster");
                    command6.Connection = con;

                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                    dbServer.AddInParameter(command6, "ChecklistID", DbType.Int64, item.CheckListId);
                    dbServer.AddInParameter(command6, "ChecklistUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "Remark", DbType.String, item.Remark);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command6, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command6, "ResultStatus");

                }

                foreach (var item in BizActionObj.ProcDetails.ProcedureTempalateList)
                {
                    DbCommand command7 = null;
                    command7 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureTempateDetails");
                    command7.Connection = con;

                    dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command7, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
                    dbServer.AddInParameter(command7, "TemplateID", DbType.Int64, item.TemplateID);
                    dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command7, trans);
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        /// <summary>
        /// get procedure master
        /// </summary>
        public override IValueObject GetProcedureMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetProcedureMasterBizActionVO objGetProc = valueObject as clsGetProcedureMasterBizActionVO;
            clsProcedureMasterVO objProcVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetProcedureMasterList");
                dbServer.AddInParameter(command, "Description", DbType.String, objGetProc.Description);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "ProcedureTypeID", DbType.Int64, objGetProc.ProcedureTypeID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objGetProc.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objGetProc.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objGetProc.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objGetProc.SortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objProcVO = new clsProcedureMasterVO();
                        objProcVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objProcVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objProcVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objProcVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objProcVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objProcVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objProcVO.Duration = Convert.ToString(DALHelper.HandleDBNull(reader["Duration"]));
                        objProcVO.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
                        objProcVO.RecommandedAnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"]));
                        objProcVO.OperationTheatreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreID"]));
                        objProcVO.OTTableID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"]));
                        objProcVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        //objProcVO.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        //objProcVO.SubSpecialization = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        objGetProc.ProcDetails.Add(objProcVO);
                    }
                }
                reader.NextResult();
                objGetProc.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return objGetProc;
        }

        /// <summary>
        /// Gets procedure details from procedure id
        /// </summary>
        public override IValueObject GetProcDetailsByProcID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetProcDetailsByProcIDBizActionVO objGetProc = valueObject as clsGetProcDetailsByProcIDBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetProcDetailsByProcID");
                dbServer.AddInParameter(command, "procID", DbType.Int64, objGetProc.ProcID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                while (reader.Read())
                {
                    clsProcedureChecklistDetailsVO checkListObj = new clsProcedureChecklistDetailsVO();
                    checkListObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    checkListObj.CategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                    checkListObj.SubCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubCategoryID"]));
                    checkListObj.CheckListId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChecklistID"]));
                    checkListObj.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                    checkListObj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                    objGetProc.CheckList.Add(checkListObj);
                }


                reader.NextResult();
                while (reader.Read())
                {
                    clsProcedureConsentDetailsVO consentObj = new clsProcedureConsentDetailsVO();
                    consentObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    consentObj.ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentID"]));
                    consentObj.ConsentDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    consentObj.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                    consentObj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                    objGetProc.ConsentList.Add(consentObj);
                }

                reader.NextResult();
                objGetProc.InstructionList = new List<clsProcedureInstructionDetailsVO>();
                objGetProc.PreInstructionList = new List<clsProcedureInstructionDetailsVO>();
                objGetProc.PostInstructionList = new List<clsProcedureInstructionDetailsVO>();

                while (reader.Read())
                {
                    clsProcedureInstructionDetailsVO InstructionObj = new clsProcedureInstructionDetailsVO();
                    InstructionObj.InstructionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstructionTypeID"]));
                    InstructionObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    InstructionObj.InstructionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstructionID"]));
                    InstructionObj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                    if (InstructionObj.InstructionTypeID == 2)
                    {
                        InstructionObj.InstructionDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Pre"]));
                        objGetProc.PreInstructionList.Add(InstructionObj);
                    }
                    else if (InstructionObj.InstructionTypeID == 3)
                    {
                        InstructionObj.InstructionDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Post"]));
                        objGetProc.PostInstructionList.Add(InstructionObj);
                    }
                    else if (InstructionObj.InstructionTypeID == 1)
                    {
                        InstructionObj.InstructionDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Intra"]));
                        objGetProc.InstructionList.Add(InstructionObj);
                    }
                }

                reader.NextResult();

                while (reader.Read())
                {
                    clsProcedureItemDetailsVO ItemObj = new clsProcedureItemDetailsVO();
                    ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    ItemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                    ItemObj.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                    ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                    objGetProc.ItemList.Add(ItemObj);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    clsDoctorSuggestedServiceDetailVO ServiceObj = new clsDoctorSuggestedServiceDetailVO();
                    ServiceObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    ServiceObj.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                    ServiceObj.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                    ServiceObj.GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["GroupName"]));
                    ServiceObj.ServiceType = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceType"]));
                    objGetProc.ServiceList.Add(ServiceObj);
                }

                reader.NextResult();
                while (reader.Read())
                {
                    clsProcedureStaffDetailsVO StaffObj = new clsProcedureStaffDetailsVO();
                    StaffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    StaffObj.DocOrStaffCode = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"]));
                    StaffObj.DocClassification = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"]));
                    StaffObj.NoofDocOrStaff = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"]));
                    StaffObj.IsDoctor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]));

                    objGetProc.StaffList.Add(StaffObj);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    clsProcedureStaffDetailsVO StaffObj = new clsProcedureStaffDetailsVO();
                    StaffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    StaffObj.DocOrStaffCode = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"]));
                    StaffObj.DocClassification = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"]));
                    StaffObj.NoofDocOrStaff = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"]));
                    StaffObj.IsDoctor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]));

                    objGetProc.StaffList.Add(StaffObj);
                }

                reader.NextResult();

                while (reader.Read())
                {
                    clsProcedureTemplateDetailsVO objProcTemplate = new clsProcedureTemplateDetailsVO();
                    objProcTemplate.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    objProcTemplate.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    objProcTemplate.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                    objProcTemplate.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                    objProcTemplate.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                    objProcTemplate.Description = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]));
                    objGetProc.ProcedureTempalateList.Add(objProcTemplate);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return objGetProc;
        }

        /// <summary>
        /// Gets OT for selected procedure
        /// </summary>

        public override IValueObject GetOTForProcedure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTForProcedureBizActionVO bizActionVo = valueObject as clsGetOTForProcedureBizActionVO;
            bizActionVo.DocDetails = new List<MasterListItem>();
            bizActionVo.DesignationDetails = new List<MasterListItem>();
            List<MasterListItem> DocList = new List<MasterListItem>();
            List<MasterListItem> DesignationList = new List<MasterListItem>();
            try
            {
                for (int i = 0; i < bizActionVo.procedureIDList.Count; i++)
                {
                    DbDataReader reader = null;

                    DbCommand command;
                    command = dbServer.GetStoredProcCommand("OT_GetOTForProcedure");
                    //dbServer.AddInParameter(command, "procedureID", DbType.Int64, bizActionVo.procedureID);
                    dbServer.AddInParameter(command, "procedureID", DbType.Int64, bizActionVo.procedureIDList[i]);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MasterListItem ObjSpecialization = new MasterListItem();
                            ObjSpecialization.Code = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"]));
                            ObjSpecialization.Description = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"]));
                            DocList.Add(ObjSpecialization);
                            //bizActionVo.DocDetails.Add(ObjSpecialization);
                        }

                        //reader.NextResult();

                        //while (reader.Read())
                        //{
                        //    MasterListItem designationObj = new MasterListItem();
                        //    designationObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"]));
                        //    designationObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"]));
                        //    DesignationList.Add(designationObj);
                        //    //bizActionVo.DesignationDetails.Add(designationObj);
                        //}

                    }

                    reader.Close();

                    DbDataReader reader1 = null;
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("OT_GetOTStaffForProcedure");
                    dbServer.AddInParameter(command1, "procedureID", DbType.Int64, bizActionVo.procedureIDList[i]);
                    reader1 = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            MasterListItem designationObj = new MasterListItem();
                            designationObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader1["DocStaffCode"]));
                            designationObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader1["DocSpecialization"]));
                            DesignationList.Add(designationObj);
                            //bizActionVo.DesignationDetails.Add(designationObj);
                        }
                    }
                    reader1.Close();


                    foreach (var item in DocList)
                    {
                        if (bizActionVo.DocDetails.Where(checkItem => checkItem.Code == item.Code).Any() == false)
                        {
                            bizActionVo.DocDetails.Add(
                                new MasterListItem()
                                {
                                    Code = item.Code,
                                    Description = item.Description
                                });
                        }
                    }


                    foreach (var item in DesignationList)
                    {
                        if (bizActionVo.DesignationDetails.Where(checkItem => checkItem.Code == item.Code).Any() == false)
                        {
                            bizActionVo.DesignationDetails.Add(
                                new MasterListItem()
                                {
                                    Code = item.Code,
                                    Description = item.Description
                                });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return bizActionVo;

            //clsGetOTForProcedureBizActionVO bizActionVo = valueObject as clsGetOTForProcedureBizActionVO;
            //try
            //{
            //    DbDataReader reader = null;


            //    DbCommand command;
            //    command = dbServer.GetStoredProcCommand("OT_GetOTForProcedure");
            //    dbServer.AddInParameter(command, "procedureID", DbType.Int64, bizActionVo.procedureID);

            //    reader = (DbDataReader)dbServer.ExecuteReader(command);
            //    if (reader.HasRows)
            //    {

            //        while (reader.Read())
            //        {
            //            MasterListItem ObjSpecialization = new MasterListItem();
            //            ObjSpecialization.Code = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"]));
            //            ObjSpecialization.Description = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"]));
            //            bizActionVo.DocDetails.Add(ObjSpecialization);
            //        }

            //        reader.NextResult();

            //        while (reader.Read())
            //        {
            //            MasterListItem designationObj = new MasterListItem();
            //            designationObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"]));
            //            designationObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"]));
            //            bizActionVo.DesignationDetails.Add(designationObj);
            //        }

            //    }
            //    reader.Close();

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //return bizActionVo;
        }


        /// <summary>
        /// Gets doctor for doctor classification
        /// </summary>

        public override IValueObject GetDoctorForDocType(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorListBySpecializationBizActionVO bizActionVo = valueObject as clsGetDoctorListBySpecializationBizActionVO;
            try
            {
                DbDataReader reader = null;


                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorByDocType");
                dbServer.AddInParameter(command, "DocType", DbType.Int64, bizActionVo.SpecializationCode);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        MasterListItem docObj = new MasterListItem();
                        docObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        docObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        //docObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"]));
                        bizActionVo.DocDetails.Add(docObj);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            return bizActionVo;
        }

        /// <summary>
        /// Gets doctor for doctor classification
        /// </summary>

        public override IValueObject GetStaffByDesignation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsStaffByDesignationIDBizActionVO bizActionVo = valueObject as clsStaffByDesignationIDBizActionVO;
            try
            {
                DbDataReader reader = null;


                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetStaffByDesignation");
                dbServer.AddInParameter(command, "DesignationID", DbType.Int64, bizActionVo.DesignationID);
                dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, bizActionVo.ProcedureID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        MasterListItem staffObj = new MasterListItem();
                        staffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        staffObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        bizActionVo.StaffDetails.Add(staffObj);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        bizActionVo.staffQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"]));
                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            return bizActionVo;
        }

        /// <summary>
        ///Add Update Patient proc Schedule Master.
        /// </summary>

        public override IValueObject AddupdatePatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddupdatePatientProcedureSchedulebizActionVO bizActionVo = valueObject as clsAddupdatePatientProcedureSchedulebizActionVO;


            DbTransaction trans = null;
            DbConnection con = null;
            DbCommand command = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcedureScheduleMaster");
                command.Connection = con;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.patientProcScheduleDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, bizActionVo.patientProcScheduleDetails.PatientUnitID);
                dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, bizActionVo.patientProcScheduleDetails.VisitAdmID);
                dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, bizActionVo.patientProcScheduleDetails.VisitAdmUnitID);
                dbServer.AddInParameter(command, "Opd_Ipd", DbType.Int64, bizActionVo.patientProcScheduleDetails.Opd_Ipd);
                dbServer.AddInParameter(command, "OTTableID", DbType.Int64, bizActionVo.patientProcScheduleDetails.OTTableID);
                dbServer.AddInParameter(command, "OperationTheatreID", DbType.Int64, bizActionVo.patientProcScheduleDetails.OTID);
                dbServer.AddInParameter(command, "OperationTheatreUnitID", DbType.Int64, bizActionVo.patientProcScheduleDetails.OTUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, bizActionVo.patientProcScheduleDetails.Date);
                dbServer.AddInParameter(command, "StartTime", DbType.DateTime, bizActionVo.patientProcScheduleDetails.StartTime);
                dbServer.AddInParameter(command, "EndTime", DbType.DateTime, bizActionVo.patientProcScheduleDetails.EndTime);
                dbServer.AddInParameter(command, "SpecialRequirement", DbType.String, bizActionVo.patientProcScheduleDetails.SpecialRequirement);
                dbServer.AddInParameter(command, "Remarks", DbType.String, bizActionVo.patientProcScheduleDetails.Remarks);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, bizActionVo.patientProcScheduleDetails.Status);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, bizActionVo.patientProcScheduleDetails.IsEmergency);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.patientProcScheduleDetails.ID);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.patientProcScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                bizActionVo.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                foreach (var item in bizActionVo.patientProcScheduleDetails.PatientProcList)
                {
                    DbCommand command1 = null;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcdure");
                    command1.Connection = con;
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ProcedureID", DbType.Int64, item.ProcedureID);
                    dbServer.AddInParameter(command1, "PatientProcedureScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
                    dbServer.AddInParameter(command1, "PatientProcedureScheduleUnitID", DbType.Int64, bizActionVo.patientProcScheduleDetails.UnitID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                }

                foreach (var item in bizActionVo.patientProcScheduleDetails.DocScheduleList)
                {
                    DbCommand command2 = null;
                    command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcDocSchedule");
                    command2.Connection = con;

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientProcedureScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
                    dbServer.AddInParameter(command2, "PatientProcedureScheduleUnitID", DbType.Int64, bizActionVo.patientProcScheduleDetails.UnitID);
                    dbServer.AddInParameter(command2, "DocTypeID", DbType.Int64, item.DocTypeID);
                    dbServer.AddInParameter(command2, "ApplyToAllDay", DbType.Boolean, item.ApplyToAllDay);
                    dbServer.AddInParameter(command2, "ScheduleID", DbType.Int64, item.ScheduleID);
                    dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, item.ProcedureID);
                    dbServer.AddInParameter(command2, "DocID", DbType.Int64, item.DocID);
                    dbServer.AddInParameter(command2, "DoctorCode", DbType.String, item.DoctorCode);
                    dbServer.AddInParameter(command2, "DayID", DbType.Int64, item.DayID);
                    dbServer.AddInParameter(command2, "StartTime", DbType.DateTime, item.StartTime);
                    dbServer.AddInParameter(command2, "EndTime", DbType.DateTime, item.EndTime);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, bizActionVo.patientProcScheduleDetails.Date);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                }

                foreach (var item in bizActionVo.patientProcScheduleDetails.StaffList)
                {
                    DbCommand command2 = null;
                    command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcStaffDetails");
                    command2.Connection = con;

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientProcedureScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
                    dbServer.AddInParameter(command2, "PatientProcedureScheduleUnitID", DbType.Int64, bizActionVo.patientProcScheduleDetails.UnitID);
                    dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, item.ProcedureID);
                    dbServer.AddInParameter(command2, "DesignationID", DbType.Int64, item.DesignationID);
                    dbServer.AddInParameter(command2, "StaffID", DbType.Int64, item.StaffID);
                    dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                }

                foreach (var item in bizActionVo.patientProcScheduleDetails.PatientProcCheckList)
                {
                    DbCommand command4 = null;
                    command4 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcedureChecklistDetails");
                    command4.Connection = con;

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientProcScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
                    dbServer.AddInParameter(command4, "PatientProcScheduleUnitID", DbType.Int64, bizActionVo.patientProcScheduleDetails.UnitID);
                    dbServer.AddInParameter(command4, "Category", DbType.String, item.Category);
                    dbServer.AddInParameter(command4, "SubCategory1", DbType.String, item.SubCategory1);
                    dbServer.AddInParameter(command4, "SubCategory2", DbType.String, item.SubCategory2);
                    dbServer.AddInParameter(command4, "Remarks", DbType.String, item.Remarks);
                    dbServer.AddInParameter(command4, "Name", DbType.String, item.Name);
                    dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);

                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            return bizActionVo;
        }

        public override IValueObject GetPatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientProcedureScheduleBizActionVO bizActionVo = valueObject as clsGetPatientProcedureScheduleBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetPatientProcedureScheduleList");

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizActionVo.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, bizActionVo.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, bizActionVo.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, bizActionVo.SortExpression);
                dbServer.AddInParameter(command, "OTID", DbType.Int64, bizActionVo.OTID);
                dbServer.AddInParameter(command, "OTUnitID", DbType.Int64, bizActionVo.OTUnitID);
                dbServer.AddInParameter(command, "OTTableID", DbType.Int64, bizActionVo.OTTableID);
                dbServer.AddInParameter(command, "OTBookingDate", DbType.DateTime, bizActionVo.OTBookingDate);
                dbServer.AddInParameter(command, "OTToDate", DbType.DateTime, bizActionVo.OTTODate);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, bizActionVo.PatientUnitID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, bizActionVo.IsEmergency);
                dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, bizActionVo.IsCancelled);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                if (bizActionVo.FirstName != null && bizActionVo.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, bizActionVo.FirstName + "%");
                if (bizActionVo.LastName != null && bizActionVo.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, bizActionVo.LastName+ "%");
                if (bizActionVo.MRNo != null && bizActionVo.MRNo.Length > 0)
                    dbServer.AddInParameter(command, "MRNo", DbType.String, bizActionVo.MRNo + "%");

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientProcedureScheduleVO patientProcScheduleObj = new clsPatientProcedureScheduleVO();
                        patientProcScheduleObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        patientProcScheduleObj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        patientProcScheduleObj.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        patientProcScheduleObj.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        patientProcScheduleObj.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DateOfBirth"]));
                        patientProcScheduleObj.MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        //patientProcScheduleObj.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        patientProcScheduleObj.Religion = Convert.ToString(DALHelper.HandleDBNull(reader["Religion"]));
                        patientProcScheduleObj.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        patientProcScheduleObj.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        patientProcScheduleObj.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        //patientProcScheduleObj.VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmID"]));
                        //patientProcScheduleObj.VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmUnitID"]));
                        //patientProcScheduleObj.Opd_Ipd = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Opd_Ipd"]));
                        patientProcScheduleObj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        patientProcScheduleObj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        patientProcScheduleObj.StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"]));
                        patientProcScheduleObj.EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"]));
                        patientProcScheduleObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        patientProcScheduleObj.SpecialRequirement = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRequirement"]));
                        patientProcScheduleObj.OT = Convert.ToString(DALHelper.HandleDBNull(reader["OTName"]));
                        patientProcScheduleObj.OTTable = Convert.ToString(DALHelper.HandleDBNull(reader["OTTableName"]));
                        patientProcScheduleObj.OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"]));
                        patientProcScheduleObj.OTUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTUnitID"]));
                        patientProcScheduleObj.OTTableID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"]));
                        patientProcScheduleObj.OTDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTDetailsID"]));
                        patientProcScheduleObj.OTDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTDetailsUnitID"]));
                        patientProcScheduleObj.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        patientProcScheduleObj.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        patientProcScheduleObj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        patientProcScheduleObj.ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]));
                        //patientProcScheduleObj.IsPACDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPACDone"]));
                        patientProcScheduleObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
                        patientProcScheduleObj.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        patientProcScheduleObj.AnesthetistName = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthetistName"]));

                        //***//
                        patientProcScheduleObj.BillClearanceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillClearanceID"]));
                        patientProcScheduleObj.BillClearanceUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillClearanceUnitID"]));
                        patientProcScheduleObj.BillClearanceIsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BillClearanceIsFreezed"]));
                        //---------//
                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));
                        patientProcScheduleObj.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;
                        bizActionVo.patientProcScheduleDetails.Add(patientProcScheduleObj);

                    }
                }
                reader.NextResult();
                bizActionVo.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

                reader.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }

        public override IValueObject GetProcScheduleDetailsByProcScheduleID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProcScheduleDetailsByProcScheduleIDBizActionVO bizActionVo = valueObject as clsGetProcScheduleDetailsByProcScheduleIDBizActionVO;
            DbDataReader reader = null;
            try
            {

                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetProcScheduleDetailsByProcScheduleID");

                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleID);
                long ID = 1;
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId); //bizActionVo.ScheduleUnitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsPatientProcedureVO patientProcObj = new clsPatientProcedureVO();
                        patientProcObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        patientProcObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        patientProcObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["procID"]));
                        patientProcObj.AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"]));
                        patientProcObj.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
                        patientProcObj.ProcedureUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureUnitID"]));
                        patientProcObj.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        bizActionVo.PatientProcList.Add(patientProcObj);
                    }

                    reader.NextResult();
                    bizActionVo.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientProcDocScheduleDetailsVO docScheduleVO = new clsPatientProcDocScheduleDetailsVO();
                        docScheduleVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        docScheduleVO.SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationCode"]));
                        docScheduleVO.DocID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocID"]));
                        docScheduleVO.DocTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocTypeID"]));
                        docScheduleVO.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"]));
                        docScheduleVO.docTypeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocTypeDesc"]));
                        docScheduleVO.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        docScheduleVO.ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]));
                        docScheduleVO.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                        docScheduleVO.docDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocDesc"]));
                        docScheduleVO.DayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"]));
                        docScheduleVO.StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"]));
                        docScheduleVO.EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"]));
                        docScheduleVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        docScheduleVO.StrStartTime = docScheduleVO.StartTime.ToShortTimeString();
                        docScheduleVO.StrEndTime = docScheduleVO.EndTime.ToShortTimeString();
                        //docScheduleVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        bizActionVo.DocScheduleDetails.Add(docScheduleVO);
                    }

                    reader.NextResult();
                    bizActionVo.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientProcStaffDetailsVO staffDetailObj = new clsPatientProcStaffDetailsVO();
                        staffDetailObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        staffDetailObj.DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DesignationID"]));
                        staffDetailObj.designationDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DesignationDesc"]));
                        staffDetailObj.StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffID"]));
                        staffDetailObj.ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]));
                        staffDetailObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                        staffDetailObj.stffDesc = Convert.ToString(DALHelper.HandleDBNull(reader["staffDesc"]));
                        staffDetailObj.Quantity = (float)Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        bizActionVo.StaffDetailList.Add(staffDetailObj);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        clsPatientProcedureScheduleVO OTScheduleObj = new clsPatientProcedureScheduleVO();
                        OTScheduleObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        OTScheduleObj.OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreID"]));
                        OTScheduleObj.OTUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreUnitID"]));
                        OTScheduleObj.OTTableID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"]));
                        OTScheduleObj.StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"]));
                        OTScheduleObj.Opd_Ipd = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Opd_Ipd"]));
                        OTScheduleObj.VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmID"]));
                        OTScheduleObj.VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmUnitID"]));
                        OTScheduleObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        OTScheduleObj.SpecialRequirement = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRequirement"]));
                        OTScheduleObj.EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"]));
                        OTScheduleObj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        OTScheduleObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));

                        bizActionVo.OTScheduleList.Add(OTScheduleObj);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        bizActionVo.patientInfoObject.pateintID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])); ;
                        bizActionVo.patientInfoObject.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        bizActionVo.patientInfoObject.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        //bizActionVo.patientInfoObject.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        bizActionVo.patientInfoObject.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        bizActionVo.patientInfoObject.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DateOfBirth"]));
                        bizActionVo.patientInfoObject.patientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    }

                    reader.NextResult();
                    bizActionVo.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientProcedureChecklistDetailsVO checklistObj = new clsPatientProcedureChecklistDetailsVO();
                        checklistObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        checklistObj.Category = Convert.ToString(DALHelper.HandleDBNull(reader["Category"]));
                        checklistObj.SubCategory1 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory1"]));
                        checklistObj.SubCategory2 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory2"]));
                        checklistObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        checklistObj.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        checklistObj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        bizActionVo.CheckList.Add(checklistObj);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        bizActionVo.AnesthesiaNotes = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));
                        bizActionVo.detailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailsID"]));
                    }

                    reader.NextResult();
                    bizActionVo.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO ServiceListObj = new clsDoctorSuggestedServiceDetailVO();
                        ServiceListObj.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        ServiceListObj.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        ServiceListObj.ServiceType = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceType"]));
                        ServiceListObj.GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["GroupName"]));
                        bizActionVo.ServiceList.Add(ServiceListObj);
                    }

                    reader.NextResult();
                    bizActionVo.PreOperativeInstructionList = new List<string>();
                    bizActionVo.InstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
                    while (reader.Read())
                    {
                        clsOTDetailsInstructionListDetailsVO ObjInstructionList = new clsOTDetailsInstructionListDetailsVO();
                        ObjInstructionList.GroupName = "Pre Operative Instruction Notes";
                        ObjInstructionList.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        bizActionVo.InstructionList.Add(ObjInstructionList);
                        bizActionVo.PreOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
                    }

                    reader.NextResult();
                    bizActionVo.IntraOperativeInstructionList = new List<string>();
                    while (reader.Read())
                    {
                        clsOTDetailsInstructionListDetailsVO objInstructionList = new clsOTDetailsInstructionListDetailsVO();
                        objInstructionList.GroupName = "Intra Operative Instruction Notes";
                        objInstructionList.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        bizActionVo.InstructionList.Add(objInstructionList);
                        bizActionVo.IntraOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
                    }

                    reader.NextResult();
                    bizActionVo.PostOperativeInstructionList = new List<string>();
                    while (reader.Read())
                    {
                        clsOTDetailsInstructionListDetailsVO objInstructionList = new clsOTDetailsInstructionListDetailsVO();
                        objInstructionList.GroupName = "Post Operative Instruction Notes";
                        objInstructionList.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        bizActionVo.InstructionList.Add(objInstructionList);
                        bizActionVo.PostOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
                    }

                    reader.NextResult();

                    bizActionVo.AddedPatientProcList = new List<clsPatientProcedureVO>();
                    while (reader.Read())
                    {

                        clsPatientProcedureVO patientProcObj = new clsPatientProcedureVO();
                        patientProcObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                        patientProcObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        bizActionVo.AddedPatientProcList.Add(patientProcObj);
                    }


                    reader.NextResult();
                    bizActionVo.ItemList = new List<clsProcedureItemDetailsVO>();
                    while (reader.Read())
                    {
                        clsProcedureItemDetailsVO ItemObj = new clsProcedureItemDetailsVO();
                        //ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ItemObj.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        ItemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        ItemObj.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        bizActionVo.ItemList.Add(ItemObj);
                    }

                    //bizActionVo.PostInstructionList = new List<clsOTDetailsPostInstructionDetailsVO>();
                    //bizActionVo.PostInstruction = new List<string>();

                    //while (reader.Read())
                    //{
                    //    clsOTDetailsPostInstructionDetailsVO PostInstructionListObj = new clsOTDetailsPostInstructionDetailsVO();
                    //    PostInstructionListObj.PostInstructionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Code"]));
                    //    PostInstructionListObj.PostInstruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    //    bizActionVo.PostInstruction.Add(PostInstructionListObj.PostInstruction);
                    //    bizActionVo.PostInstructionList.Add(PostInstructionListObj);
                    //}




                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }

        /// <summary>
        /// Check existing OT schedule
        /// </summary>
        public override IValueObject CheckOTScheduleExistance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckTimeForOTScheduleExistanceBizActionVO BizAction = valueObject as clsCheckTimeForOTScheduleExistanceBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_CheckOTScheduleExistance");

                dbServer.AddInParameter(command, "OTTableID", DbType.Int64, BizAction.OTTableID);
                dbServer.AddInParameter(command, "OTID", DbType.Int64, BizAction.OTID);
                dbServer.AddInParameter(command, "StartTime", DbType.DateTime, BizAction.StartTime);
                dbServer.AddInParameter(command, "EndTime", DbType.DateTime, BizAction.EndTime);
                dbServer.AddInParameter(command, "DayID", DbType.Int64, BizAction.DayID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.IsSchedulePresent = (bool)DALHelper.HandleDBNull(reader["IsSchedulePresent"]);
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

        public override IValueObject AddOTScheduleMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddOTScheduleMasterBizActionVO BizAction = valueObject as clsAddOTScheduleMasterBizActionVO;
            BizAction = AddOtSchedule(BizAction, objUserVO);
            return BizAction;
        }

        private clsAddOTScheduleMasterBizActionVO AddOtSchedule(clsAddOTScheduleMasterBizActionVO BizAction, clsUserVO UserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                clsOTScheduleVO objOtScheduleVO = BizAction.OTScheduleDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddOTSchedule");
                dbServer.AddInParameter(command, "OTID", DbType.Int64, objOtScheduleVO.OTID);
                dbServer.AddInParameter(command, "OTTableID", DbType.Int64, objOtScheduleVO.OTTableID);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objOtScheduleVO.ID);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizAction.OTScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteOTScheduleDetails");
                dbServer.AddInParameter(command3, "OTScheduleID", DbType.Int64, objOtScheduleVO.ID);
                int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);

                foreach (var ObjDetails in objOtScheduleVO.OTScheduleDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddOTScheduleDetails");

                    dbServer.AddInParameter(command1, "OTScheduleID", DbType.Int64, objOtScheduleVO.ID);
                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
                    dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
                    dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
                    dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
                    dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");


                }
                trans.Commit();
                BizAction.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //   throw;
                BizAction.SuccessStatus = -1;
                trans.Rollback();
                BizAction.OTScheduleDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;

            }
            return BizAction;
        }

        /// <summary>
        /// Gets OT schedule
        /// </summary>
        public override IValueObject GetOTScheduleMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTScheduleMasterListBizActionVO BizActionObj = (clsGetOTScheduleMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetOTScheduleBySearchCriteria");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "OTTableId", DbType.Int64, BizActionObj.OTTableID);
                dbServer.AddInParameter(command, "OTID", DbType.Int64, BizActionObj.OTID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.OTScheduleList == null)
                        BizActionObj.OTScheduleList = new List<clsOTScheduleVO>();
                    while (reader.Read())
                    {
                        clsOTScheduleVO objOTScheduleVO = new clsOTScheduleVO();
                        objOTScheduleVO.ID = (long)reader["ID"];
                        objOTScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objOTScheduleVO.OTID = (long)DALHelper.HandleDBNull(reader["OTID"]);
                        objOTScheduleVO.OTName = (string)DALHelper.HandleDBNull(reader["OTName"]);
                        objOTScheduleVO.OTTableID = (long)DALHelper.HandleDBNull(reader["OTTableID"]);
                        objOTScheduleVO.OTTableName = (string)DALHelper.HandleDBNull(reader["OTTableName"]);
                        objOTScheduleVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.OTScheduleList.Add(objOTScheduleVO);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    BizActionObj.TotalRows = Convert.ToInt32(reader["TotalRows"]);
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
        /// Gets OT schedule Details
        /// </summary>
        public override IValueObject GetOTScheduleDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTScheduleListBizActionVO BizActionObj = (clsGetOTScheduleListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetOTScheduleList");
                dbServer.AddInParameter(command, "OTScheduleID ", DbType.Int64, BizActionObj.OTScheduleID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.OTScheduleList == null)
                        BizActionObj.OTScheduleList = new List<clsOTScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsOTScheduleDetailsVO objOTScheduleVO = new clsOTScheduleDetailsVO();
                        objOTScheduleVO.ID = (long)reader["ID"];
                        objOTScheduleVO.OTScheduleID = (long)reader["OTScheduleID"];
                        objOTScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        if (objOTScheduleVO.DayID == 1)
                            objOTScheduleVO.Day = "Sunday";
                        else if (objOTScheduleVO.DayID == 2)
                            objOTScheduleVO.Day = "Monday";
                        else if (objOTScheduleVO.DayID == 3)
                            objOTScheduleVO.Day = "Tuesday";
                        else if (objOTScheduleVO.DayID == 4)
                            objOTScheduleVO.Day = "Wednesday";
                        else if (objOTScheduleVO.DayID == 5)
                            objOTScheduleVO.Day = "Thursday";
                        else if (objOTScheduleVO.DayID == 6)
                            objOTScheduleVO.Day = "Friday";
                        else if (objOTScheduleVO.DayID == 7)
                            objOTScheduleVO.Day = "Saturday";
                        objOTScheduleVO.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
                        objOTScheduleVO.Schedule = Convert.ToString(DALHelper.HandleDBNull(reader["Schedule"]));
                        objOTScheduleVO.StartTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["StartTime"]));
                        objOTScheduleVO.EndTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EndTime"]));
                        BizActionObj.OTScheduleList.Add(objOTScheduleVO);
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


        /// <summary>
        /// Grets Ot Scehdule
        /// </summary>
        public override IValueObject GetOTScheduleTime(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTScheduleTimeVO BizAction = (clsGetOTScheduleTimeVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetOTScheduleTime");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "OTID", DbType.Int64, BizAction.OTID);
                dbServer.AddInParameter(command, "OTTableID", DbType.Int64, BizAction.OTTabelID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.OTScheduleDetailsList == null)
                        BizAction.OTScheduleDetailsList = new List<clsOTScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsOTScheduleDetailsVO OTScheduleVO = new clsOTScheduleDetailsVO();
                        OTScheduleVO.OTTableID = (long)DALHelper.HandleDBNull(reader["OTTableID"]);
                        OTScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
                        OTScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
                        OTScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
                        OTScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        if (OTScheduleVO.DayID == 1)
                            OTScheduleVO.Day = "Sunday";
                        else if (OTScheduleVO.DayID == 2)
                            OTScheduleVO.Day = "Monday";
                        else if (OTScheduleVO.DayID == 3)
                            OTScheduleVO.Day = "Tuesday";
                        else if (OTScheduleVO.DayID == 4)
                            OTScheduleVO.Day = "Wednesday";
                        else if (OTScheduleVO.DayID == 5)
                            OTScheduleVO.Day = "Thursday";
                        else if (OTScheduleVO.DayID == 6)
                            OTScheduleVO.Day = "Friday";
                        else if (OTScheduleVO.DayID == 7)
                            OTScheduleVO.Day = "Saturday";
                        BizAction.OTScheduleDetailsList.Add(OTScheduleVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizAction;
        }

        /// <summary>
        /// Gets Procedure wise OT Scedule
        /// </summary>
        public override IValueObject GetProcOTSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProcOTScheduleBizActionVO BizActionObj = (clsGetProcOTScheduleBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetProcOTScedule");
                dbServer.AddInParameter(command, "OTTheaterID", DbType.Int64, BizActionObj.OTID);
                dbServer.AddInParameter(command, "OTTable", DbType.Int64, BizActionObj.OTTableID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.OTScheduleDetailsList == null)
                        BizActionObj.OTScheduleDetailsList = new List<clsPatientProcOTScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsPatientProcOTScheduleDetailsVO objOTScheduleVO = new clsPatientProcOTScheduleDetailsVO();
                        objOTScheduleVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objOTScheduleVO.OTID = (long)DALHelper.HandleDBNull(reader["OTID"]);
                        objOTScheduleVO.OTDesc = (string)DALHelper.HandleDBNull(reader["OTDesc"]);
                        objOTScheduleVO.OTTableID = (long)DALHelper.HandleDBNull(reader["OTTableID"]);
                        objOTScheduleVO.OTTableDesc = (string)DALHelper.HandleDBNull(reader["OTTableDesc"]);
                        objOTScheduleVO.StartTime = (DateTime)DALHelper.HandleDate(reader["StartTime"]);
                        objOTScheduleVO.EndTime = (DateTime)DALHelper.HandleDate(reader["EndTime"]);
                        objOTScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
                        BizActionObj.OTScheduleDetailsList.Add(objOTScheduleVO);
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

        /// <summary>
        /// Gets checklist by procedure id
        /// </summary>
        public override IValueObject GetCheckListByProcedureID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCheckListByProcedureIDBizActionVO BizActionObj = (clsGetCheckListByProcedureIDBizActionVO)valueObject;
            List<clsPatientProcedureChecklistDetailsVO> checkList = new List<clsPatientProcedureChecklistDetailsVO>();
            try
            {
                BizActionObj.ChecklistDetails = new List<clsPatientProcedureChecklistDetailsVO>();
                for (int i = 0; i < BizActionObj.procedureIDList.Count; i++)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCheckListByProcedureID");
                    //dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, BizActionObj.ProcedureID);
                    dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, BizActionObj.procedureIDList[i]);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsPatientProcedureChecklistDetailsVO checkListObj = new clsPatientProcedureChecklistDetailsVO();
                            checkListObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            checkListObj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            checkListObj.CategoryID = (long)DALHelper.HandleDBNull(reader["CategoryID"]);
                            checkListObj.ChecklistID = (long)DALHelper.HandleDBNull(reader["ChecklistID"]);
                            checkListObj.SubCategoryID = (long)DALHelper.HandleDBNull(reader["SubCategoryID"]);
                            checkListObj.Remarks = (string)DALHelper.HandleDBNull(reader["Remark"]);
                            checkListObj.Name = (string)DALHelper.HandleDBNull(reader["Name"]);
                            checkListObj.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                            checkListObj.ProcedureID = (long)DALHelper.HandleDBNull(reader["ProcedureID"]);
                            checkList.Add(checkListObj);

                            //BizActionObj.ChecklistDetails.Add(checkListObj);
                        }
                    }

                    foreach (var item in checkList)
                    {
                        if (BizActionObj.ChecklistDetails.Where(checkItem => checkItem.ChecklistID == item.ChecklistID).Any() == false)
                        {
                            BizActionObj.ChecklistDetails.Add(
                                new clsPatientProcedureChecklistDetailsVO()
                                {
                                    ID = item.ID,
                                    UnitID = item.UnitID,
                                    CategoryID = item.CategoryID,
                                    ChecklistID = item.ChecklistID,
                                    SubCategoryID = item.SubCategoryID,
                                    Remarks = item.Remarks,
                                    Name = item.Name,
                                    Status = item.Status,
                                    ProcedureID = item.ProcedureID
                                });

                        }
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

        /// <summary>
        /// Gets checklist by procedure id
        /// </summary>
        public override IValueObject GetCheckListByScheduleID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCheckListByScheduleIDBizActionVO BizActionObj = (clsGetCheckListByScheduleIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCheckListByScheduleID");
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.ScheduleID);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, BizActionObj.ScheduleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ChecklistDetails == null)
                        BizActionObj.ChecklistDetails = new List<clsPatientProcedureChecklistDetailsVO>();

                    while (reader.Read())
                    {
                        clsPatientProcedureChecklistDetailsVO checkListObj = new clsPatientProcedureChecklistDetailsVO();
                        checkListObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        checkListObj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        checkListObj.Category = (string)DALHelper.HandleDBNull(reader["Category"]);
                        checkListObj.PatientProcScheduleID = (long)DALHelper.HandleDBNull(reader["PatientProcScheduleID"]);
                        checkListObj.PatientProcScheduleUnitID = (long)DALHelper.HandleDBNull(reader["PatientProcScheduleUnitID"]);
                        checkListObj.SubCategory1 = (string)DALHelper.HandleDBNull(reader["SubCategory1"]);
                        checkListObj.SubCategory2 = (string)DALHelper.HandleDBNull(reader["SubCategory2"]);
                        checkListObj.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        checkListObj.Name = (string)DALHelper.HandleDBNull(reader["Name"]);
                        BizActionObj.ChecklistDetails.Add(checkListObj);
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

        /// <summary>
        /// Gets checklist by procedure id
        /// </summary>
        public override IValueObject GetDocScheduleList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDocScheduleByDocIDBizActionVO BizActionObj = (clsGetDocScheduleByDocIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDocScheduleListByDocIDIDAndDate");
                dbServer.AddInParameter(command, "DocID", DbType.Int64, BizActionObj.DocID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "DocTypeID", DbType.Int64, BizActionObj.DocTableID);
                dbServer.AddInParameter(command, "ProcDate", DbType.DateTime, BizActionObj.procDate);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DocScheduleListDetails == null)
                        BizActionObj.DocScheduleListDetails = new List<clsPatientProcDocScheduleDetailsVO>();

                    while (reader.Read())
                    {
                        clsPatientProcDocScheduleDetailsVO docSceduleListObj = new clsPatientProcDocScheduleDetailsVO();
                        docSceduleListObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        docSceduleListObj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        docSceduleListObj.DocID = (long)DALHelper.HandleDBNull(reader["DocID"]);
                        docSceduleListObj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        docSceduleListObj.StartTime = (DateTime)DALHelper.HandleDate(reader["FromTime"]);
                        docSceduleListObj.EndTime = (DateTime)DALHelper.HandleDate(reader["ToTime"]);
                        docSceduleListObj.DocTypeID = (long)DALHelper.HandleDBNull(reader["DocTypeID"]);
                        BizActionObj.DocScheduleListDetails.Add(docSceduleListObj);
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

        /// <summary>
        /// Cancel OT Booking
        /// </summary>
        public override IValueObject CancelOTBooking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCancelOTBookingBizActionVO BizActionObj = (clsCancelOTBookingBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CancelOTBooking1");
                dbServer.AddInParameter(command, "PatientProcScheduleID", DbType.Int64, BizActionObj.patientProcScheduleID);
                dbServer.AddInParameter(command, "CancelledReason", DbType.String, BizActionObj.CancelledReason);
                dbServer.AddInParameter(command, "CancelledBy", DbType.Int64, BizActionObj.CancelledBy);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CancelledDate", DbType.DateTime, DateTime.Now);
                int intStatus = dbServer.ExecuteNonQuery(command);
            }

            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        /// <summary>
        /// Gets staff list
        /// </summary>
        public override IValueObject GetStaffForOTScheduling(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStaffForOTSchedulingBizActionVO BizActionObj = (clsGetStaffForOTSchedulingBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffForOTScheduling");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.staffList == null)
                        BizActionObj.staffList = new List<MasterListItem>();

                    while (reader.Read())
                    {
                        MasterListItem staffObj = new MasterListItem();
                        staffObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        staffObj.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.staffList.Add(staffObj);
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

        /// <summary>
        /// Gets procedure id for OT booking id 
        /// </summary>
        public override IValueObject GetProceduresForOTBookingID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProceduresForOTBookingIDBizActionVO BizActionObj = (clsGetProceduresForOTBookingIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetProcedureIDForOTBookingID");
                dbServer.AddInParameter(command, "OTBookingID", DbType.Int64, BizActionObj.OTBokingID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.procedureList == null)
                        BizActionObj.procedureList = new List<MasterListItem>();

                    while (reader.Read())
                    {
                        MasterListItem procObj = new MasterListItem();
                        procObj.ID = (long)DALHelper.HandleDBNull(reader["ProcedureID"]);
                        BizActionObj.procedureList.Add(procObj);
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

        /// <summary>
        /// Gets procedure id for OT booking id 
        /// </summary>
        public override IValueObject GetConsentForProcedureID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsentForProcedureIDBizActionVO BizActionObj = (clsGetConsentForProcedureIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetConsentForProcedureID");
                dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, BizActionObj.ProcedureID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ConsentList == null)
                        BizActionObj.ConsentList = new List<MasterListItem>();

                    while (reader.Read())
                    {
                        MasterListItem ConsentObj = new MasterListItem();
                        ConsentObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ConsentObj.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.ConsentList.Add(ConsentObj);
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

        public override IValueObject GetPatientDetailsForConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsentDetailsBizActionVO BizActionObj = (clsGetConsentDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsForConsent");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ConsentDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ConsentDetails.PatientUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ConsentDetails == null)
                        BizActionObj.ConsentDetails = new clsConsentDetailsVO();
                    while (reader.Read())
                    {
                        clsConsentDetailsVO objListVO = new clsConsentDetailsVO();

                        BizActionObj.ConsentDetails.ID = Convert.ToInt64(reader["Id"]);
                        BizActionObj.ConsentDetails.UnitID = Convert.ToInt64(reader["UnitID"]);

                        BizActionObj.ConsentDetails.MRNo = Convert.ToString(reader["MRNo"]);

                        //string fullname = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        //fullname += " " + Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));

                        //BizActionObj.ConsentDetails.PatientName = fullname;

                        BizActionObj.ConsentDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        BizActionObj.ConsentDetails.PatientAddress = Security.base64Decode((string)DALHelper.HandleDBNull(reader["ResAddress"]));
                        BizActionObj.ConsentDetails.PatientContachNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileNo"]));

                        BizActionObj.ConsentDetails.KinName = Convert.ToString(DALHelper.HandleDBNull(reader["KinName"]));
                        BizActionObj.ConsentDetails.KinAddress = Convert.ToString(DALHelper.HandleDBNull(reader["KinAddress"]));
                        BizActionObj.ConsentDetails.KinMobileNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["KinMobileNo"]));

                        break;
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

        public override IValueObject SaveConsentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSaveConsentDetailsBizActionVO BizActionObj = (clsSaveConsentDetailsBizActionVO)valueObject;
            try
            {
                clsConsentDetailsVO objEmergencyVO = BizActionObj.ConsentDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddConsentDetails");
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.ConsentDetails.ScheduleID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ConsentDetails.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ConsentDetails.UnitID);
                dbServer.AddInParameter(command, "Date", DbType.Date, BizActionObj.ConsentDetails.Date);
                dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.ConsentDetails.VisitAdmID);
                dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.ConsentDetails.VisitAdmUnitID);
                dbServer.AddInParameter(command, "Opd_Ipd", DbType.Int32, BizActionObj.ConsentDetails.Opd_Ipd);
                dbServer.AddInParameter(command, "ConsentId", DbType.Int64, BizActionObj.ConsentDetails.ConsentID);
                dbServer.AddInParameter(command, "Consent", DbType.String, BizActionObj.ConsentDetails.Consent);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "ConsentSummaryID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ConsentDetails.ConsentSummaryID);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizActionObj;
        }

        //public override IValueObject SaveConsentDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsSaveConsentDetailsBizActionVO BizActionObj = (clsSaveConsentDetailsBizActionVO)valueObject;
        //    try
        //    {
        //        clsConsentDetailsVO objEmergencyVO = BizActionObj.ConsentDetails;

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddConsentDetails");

        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ConsentDetails.UnitID);
        //        dbServer.AddInParameter(command, "Date", DbType.Date, BizActionObj.ConsentDetails.Date);

        //        dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.ConsentDetails.VisitAdmID);
        //        dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.ConsentDetails.VisitAdmUnitID);
        //        dbServer.AddInParameter(command, "Opd_Ipd", DbType.Int32, BizActionObj.ConsentDetails.Opd_Ipd);

        //        dbServer.AddInParameter(command, "ConsentId", DbType.Int64, BizActionObj.ConsentDetails.ConsentID);
        //        dbServer.AddInParameter(command, "Consent", DbType.String, BizActionObj.ConsentDetails.Consent);

        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return BizActionObj;
        //}

        public override IValueObject GetOTBookingByDateID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTBookingByOTTablebookingDateBizActionVO BizActionObj = (clsGetOTBookingByOTTablebookingDateBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetProOTScheduleListByOTTableIDAndDate");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "OTTableID", DbType.Int64, BizActionObj.OTTableID);
                dbServer.AddInParameter(command, "OTID", DbType.Int64, BizActionObj.OTID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "OTDate", DbType.DateTime, BizActionObj.OTDate);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.bookingDetailsList == null)
                        BizActionObj.bookingDetailsList = new List<clsPatientProcedureScheduleVO>();
                    while (reader.Read())
                    {
                        clsPatientProcedureScheduleVO OTScheduleVO = new clsPatientProcedureScheduleVO();
                        OTScheduleVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        OTScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        OTScheduleVO.OTID = (long)DALHelper.HandleDBNull(reader["OperationTheatreID"]);
                        OTScheduleVO.OTTableID = (long)DALHelper.HandleDBNull(reader["OTTableID"]);
                        OTScheduleVO.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        OTScheduleVO.StartTime = (DateTime)reader["StartTime"];
                        OTScheduleVO.EndTime = (DateTime)reader["EndTime"];                        
                        OTScheduleVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["Patientname"]));
                        BizActionObj.bookingDetailsList.Add(OTScheduleVO);
                    }
                    reader.NextResult();
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetConsentByConsentType(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsentByConsentTypeBizActionVO BizActionObj = (clsGetConsentByConsentTypeBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetConsentByConsentTypeID");
                dbServer.AddInParameter(command, "ConsentTypeID", DbType.Int64, BizActionObj.ConsentTypeID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ConsentList == null)
                        BizActionObj.ConsentList = new List<clsConsentDetailsVO>();
                    while (reader.Read())
                    {
                        clsConsentDetailsVO Consent = new clsConsentDetailsVO();
                        Consent.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Consent.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        //Consent.ConsentType = (long)DALHelper.HandleDBNull(reader["ConsentType"]);
                        Consent.TemplateName = (string)DALHelper.HandleDBNull(reader["Template"]);
                        BizActionObj.ConsentList.Add(Consent);
                    }
                    reader.NextResult();
                }
                reader.Close();

            }

            catch
            {
                throw;
            }

            finally
            {
            }

            return BizActionObj;
        }

        //public override IValueObject GetPatientConsents(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetPatientConsentsBizActionVO BizActionObj = (clsGetPatientConsentsBizActionVO)valueObject;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientConsents");
        //        dbServer.AddInParameter(command, "ConsentType", DbType.Int64, BizActionObj.ConsentTypeID);
        //        dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.VisitAdmID);
        //        dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.VisitAdmUnitID);
        //        dbServer.AddInParameter(command, "OPD_IPD", DbType.Boolean, BizActionObj.OPD_IPD);
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ConsentList == null)
        //                BizActionObj.ConsentList = new List<clsConsentDetailsVO>();
        //            while (reader.Read())
        //            {
        //                clsConsentDetailsVO Consent = new clsConsentDetailsVO();
        //                Consent.ID = (long)DALHelper.HandleDBNull(reader["Id"]);
        //                Consent.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                Consent.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
        //                Consent.Description = (string)DALHelper.HandleDBNull(reader["TemplateName"]);
        //                Consent.Consent = (string)DALHelper.HandleDBNull(reader["Consent"]);
        //                Consent.PatientName = (string)DALHelper.HandleDBNull(reader["PatientName"]);
        //                Consent.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
        //                Consent.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
        //                Consent.Age = (int)DALHelper.HandleDBNull(reader["Age"]);
        //                BizActionObj.ConsentList.Add(Consent);
        //            }
        //            reader.NextResult();
        //        }
        //        reader.Close();

        //    }

        //    catch
        //    {
        //        throw;
        //    }

        //    finally
        //    {
        //    }

        //    return BizActionObj;
        //}

        public override IValueObject GetDeleteConsents(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteConsentBizActionVO BizActionObj = (clsDeleteConsentBizActionVO)valueObject;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_DeleteConsentDetails");

                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.Consent.ScheduleID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Consent.PatientID);
                dbServer.AddInParameter(command, "ConsentId", DbType.Int64, BizActionObj.Consent.ConsentID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientConsents(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientConsentsBizActionVO BizActionObj = (clsGetPatientConsentsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientConsentsOT");
                //dbServer.AddInParameter(command, "ConsentType", DbType.Int64, BizActionObj.ConsentTypeID);
                //dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.VisitAdmID);
                //dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.VisitAdmUnitID);
                //dbServer.AddInParameter(command, "OPD_IPD", DbType.Boolean, BizActionObj.OPD_IPD);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.ScheduleID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ConsentList == null)
                        BizActionObj.ConsentList = new List<clsConsentDetailsVO>();
                    while (reader.Read())
                    {
                        clsConsentDetailsVO Consent = new clsConsentDetailsVO();
                        Consent.ID = (long)DALHelper.HandleDBNull(reader["Id"]);
                        Consent.ConsentID = (long)DALHelper.HandleDBNull(reader["ConsentId"]);
                        Consent.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Consent.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        Consent.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        Consent.Consent = (string)DALHelper.HandleDBNull(reader["Consent"]);
                        Consent.PatientName = (string)DALHelper.HandleDBNull(reader["PatientName"]);
                        Consent.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        Consent.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        Consent.Age = (int)DALHelper.HandleDBNull(reader["Age"]);
                        BizActionObj.ConsentList.Add(Consent);
                    }
                    reader.NextResult();
                }
                reader.Close();
            }

            catch
            {
                throw;
            }

            finally
            {
            }

            return BizActionObj;
        }

        public override IValueObject GetDoctorOrderForOTScheduling(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorForDoctorTypeBizActionVO BizAction = valueObject as clsGetDoctorForDoctorTypeBizActionVO;
            try
            {
                DbCommand command = null;
                DbDataReader reader = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorOrderForOTScheduling");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizAction.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizAction.ToDate);
                dbServer.AddInParameter(command, "Priority", DbType.Int32, BizAction.PriorityID);
                dbServer.AddInParameter(command, "OPD_IPD", DbType.Int32, BizAction.Opd_Ipd);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientProcedureScheduleVO objDoc = new clsPatientProcedureScheduleVO();
                        objDoc.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDoc.TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                        objDoc.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        objDoc.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objDoc.Group = Convert.ToString(DALHelper.HandleDBNull(reader["GroupName"]));
                        objDoc.PatientType = Convert.ToString(DALHelper.HandleDBNull(reader["PatientType"]));
                        objDoc.PriorityName = Convert.ToString(DALHelper.HandleDBNull(reader["Priority"]));
                        objDoc.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDate"]));
                        objDoc.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        objDoc.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objDoc.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objDoc.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        BizAction.DoctorOrderList.Add(objDoc);
                    }
                }
                reader.NextResult();
                BizAction.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
                return BizAction;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddUpdateProcedureSubCategory(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            clsProcedureSubCategoryVO ObjSubVO = new clsProcedureSubCategoryVO();
            clsAddUpdateProcedureSubCategoryBizActionVO objItem = valueObject as clsAddUpdateProcedureSubCategoryBizActionVO;

            try
            {
                ObjSubVO = objItem.SubCategoryList[0];

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcSubCategory");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjSubVO.ID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, ObjSubVO.UnitId);
                dbServer.AddInParameter(command, "ProcedureCategoryID", DbType.Int64, ObjSubVO.CategoryId);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjSubVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjSubVO.Description.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjSubVO.Status);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, ObjSubVO.CreatedUnitID);                
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, ObjSubVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, ObjSubVO.AddedOn);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, ObjSubVO.AddedWindowsLoginName);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, ObjSubVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, ObjSubVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, ObjSubVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, ObjSubVO.UpdateWindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SubCategoryDetails.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;

        }

        public override IValueObject GetProcedureSubCategoryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetProcedureSubCategoryListBizActionVO BizActionObj = (clsGetProcedureSubCategoryListBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("[CIMS_GetSubCategoryMasterList]");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "TotalRows", DbType.Int64, BizActionObj.TotalRows);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.SubCategoryList == null)
                        BizActionObj.SubCategoryList = new List<clsProcedureSubCategoryVO>();
                    while (reader.Read())
                    {
                        clsProcedureSubCategoryVO ComptVO = new clsProcedureSubCategoryVO();
                        ComptVO.ID = Convert.ToInt64(reader["ID"]);
                        ComptVO.Code = Convert.ToString(reader["Code"]);
                        ComptVO.Description = Convert.ToString(reader["Description"]);
                        ComptVO.CategoryId = Convert.ToInt64(reader["ProcedureCategoryID"]);
                        ComptVO.Category = Convert.ToString(reader["Category"]);
                        ComptVO.UnitId = Convert.ToInt64(reader["UnitID"]);
                        ComptVO.Status = Convert.ToBoolean(reader["Status"]);
                        BizActionObj.SubCategoryList.Add(ComptVO);
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

        public override IValueObject UpdateStatusProcedureSubCategory(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusProcedureSubCategoryBizActionVO BizActionObj = (clsUpdateStatusProcedureSubCategoryBizActionVO)valueObject;
            try
            {
                clsProcedureSubCategoryVO objSDeptVO = BizActionObj.SubCategoryDetails;


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStatusProcedureSubCategory");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objSDeptVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objSDeptVO.Status);
                int intstatus = dbServer.ExecuteNonQuery(command);
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

        public override IValueObject AddUpdateProcedureCheckList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            clsProcedureCheckListVO ObjCheckListVO = new clsProcedureCheckListVO();
            clsAddUpdateProcedureCheckListBizActionVO objItem = valueObject as clsAddUpdateProcedureCheckListBizActionVO;

            try
            {
                ObjCheckListVO = objItem.CheckList[0];

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureCheckList");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjCheckListVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, ObjCheckListVO.CategoryId);
                dbServer.AddInParameter(command, "SubCategoryID", DbType.Int64, ObjCheckListVO.SubCategoryId);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjCheckListVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjCheckListVO.Description.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjCheckListVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.UserGeneralDetailVO.EmployeeID);
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserGeneralDetailVO.AddedOn);
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.CheckListDetails.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;

        }

        public override IValueObject GetProcedureCheckList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetProcedureCheckListBizActionVO BizActionObj = (clsGetProcedureCheckListBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetProcedureCheckList");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "TotalRows", DbType.Int64, BizActionObj.TotalRows);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.CheckList == null)
                        BizActionObj.CheckList = new List<clsProcedureCheckListVO>();
                    while (reader.Read())
                    {
                        clsProcedureCheckListVO ComptVO = new clsProcedureCheckListVO();
                        ComptVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ComptVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        ComptVO.CategoryId = (long)DALHelper.HandleDBNull(reader["CategoryID"]);
                        ComptVO.SubCategoryId = (long)DALHelper.HandleDBNull(reader["SubCategoryID"]);
                        ComptVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        ComptVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        ComptVO.Category = (string)DALHelper.HandleDBNull(reader["Category"]);
                        ComptVO.SubCategory = (string)DALHelper.HandleDBNull(reader["SubCategory"]);
                        ComptVO.Status = (bool)reader["Status"];
                        BizActionObj.CheckList.Add(ComptVO);
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

        public override IValueObject UpdateStatusProcedureCheckList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusProcedureCheckListBizActionVO BizActionObj = (clsUpdateStatusProcedureCheckListBizActionVO)valueObject;
            try
            {
                clsProcedureCheckListVO objSDeptVO = BizActionObj.CheckListDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStatusProcedureCheckList");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objSDeptVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objSDeptVO.Status);
                int intstatus = dbServer.ExecuteNonQuery(command);
            }

            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetServicesForProcedure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServicesForProcedureBizActionVO BizActionObj = (clsGetServicesForProcedureBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServicesForProcedure");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem Service = new MasterListItem();
                        Service.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Service.Description = (string)DALHelper.HandleDBNull(reader["DESCRIPTION"]);
                        BizActionObj.MasterList.Add(Service);
                    }
                    reader.NextResult();
                }
                reader.Close();

            }

            catch
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject ProcedureUpdtStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdStatusProcedureMasterBizActionVO BizActionObj = (clsUpdStatusProcedureMasterBizActionVO)valueObject;
            try
            {
                clsProcedureMasterVO objproceVO = BizActionObj.ProcedureObj;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStatusProcedureMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objproceVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objproceVO.Status);
                int intstatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {

                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorListBySpecialization(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorListBySpecializationBizActionVO bizActionVo = valueObject as clsGetDoctorListBySpecializationBizActionVO;
            try
            {
                DbDataReader reader = null;


                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorListBySpecialization");
                dbServer.AddInParameter(command, "SepcializationCode", DbType.String, bizActionVo.SpecializationCode.Trim());

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        MasterListItem docObj = new MasterListItem();
                        docObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        docObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        docObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"]));
                        bizActionVo.DocDetails.Add(docObj);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            return bizActionVo;
        }






        //#region Variables Declaration
        ////Declare the database object
        //private Database dbServer = null;
        ////Declare the LogManager object
        //private LogManager logManager = null;
        //#endregion

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //private clsOtConfigDAL()
        //{
        //    try
        //    {
        //        #region Create Instance of database,LogManager object and BaseSql object
        //        //Create Instance of the database object and BaseSql object
        //        if (dbServer == null)
        //        {
        //            dbServer = HMSConfigurationManager.GetDatabaseReference();
        //        }

        //        //Create Instance of the LogManager object 
        //        if (logManager == null)
        //        {
        //            logManager = LogManager.GetInstance();
        //        }
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Adds Ot Table Details
        ///// </summary>
        ///// <param name="valueObject">clsAddUpdateOTTableDetailsBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns>clsAddUpdateOTTableDetailsBizActionVO object</returns>
        //public override IValueObject AddUpdateOtTableDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsOTTableVO objItemVO = new clsOTTableVO();
        //    clsAddUpdateOTTableDetailsBizActionVO objItem = valueObject as clsAddUpdateOTTableDetailsBizActionVO;
        //    try
        //    {
        //        DbCommand command;
        //        objItemVO = objItem.OTTableMasterMatserDetails[0];
        //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTTable");
        //        dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
        //        dbServer.AddInParameter(command, "OTTheatreId", DbType.Int64, objItemVO.OTTheatreID);
        //        dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
        //        dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);

        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);


        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objItem;
        //}

        ///// <summary>
        ///// Fills Ot Table List
        ///// </summary>
        ///// <param name="valueObject">clsGetOTTableDetailsBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns>clsGetOTTableDetailsBizActionVO object</returns>
        //public override IValueObject GetOtTableDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetOTTableDetailsBizActionVO objItem = valueObject as clsGetOTTableDetailsBizActionVO;
        //    clsOTTableVO objItemVO = null;
        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetOtTableDetails");
        //        dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
        //        dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objItemVO = new clsOTTableVO();
        //                objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                objItemVO.OTTheatreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTheatreId"]));
        //                objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
        //                objItemVO.TheatreName = Convert.ToString(DALHelper.HandleDBNull(reader["TheatreName"]));
        //                objItem.OtTableMatserDetails.Add(objItemVO);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objItem;

        //}
        ///// <summary>
        ///// Fills patient config combo with patinet related fields
        ///// </summary>
        ///// <param name="valueObject">clsGetPatientConfigFieldsBizActionVO</param>
        ///// <param name="UserVo"><clsUserVO/param>
        ///// <returns>clsGetPatientConfigFieldsBizActionVO object</returns>
        //public override IValueObject GetConfig_Patient_Fields(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetPatientConfigFieldsBizActionVO objItem = valueObject as clsGetPatientConfigFieldsBizActionVO;
        //    clsPatientFieldsConfigVO objItemVO = null;
        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetConfig_Patient_Fields");

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objItemVO = new clsPatientFieldsConfigVO();
        //                objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                objItemVO.TableName = Convert.ToString(DALHelper.HandleDBNull(reader["TableName"]));
        //                objItemVO.FieldName = Convert.ToString(DALHelper.HandleDBNull(reader["FieldName"]));
        //                objItem.OtPateintConfigFieldsMatserDetails.Add(objItemVO);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objItem;

        //}

        ///// <summary>
        ///// Adds or Updates Consent Master Details
        ///// </summary>
        ///// <param name="valueObject">clsAddUpdateConsentMasterBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns>clsAddUpdateConsentMasterBizActionVO object</returns>
        //public override IValueObject AddUpdateConsentMaster(IValueObject valueObject, clsUserVO UserVo)
        //{

        //    clsConsentMasterVO objItemVO = new clsConsentMasterVO();
        //    clsAddUpdateConsentMasterBizActionVO objItem = valueObject as clsAddUpdateConsentMasterBizActionVO;
        //    try
        //    {
        //        DbCommand command;
        //        objItemVO = objItem.OTTableMasterMatserDetails[0];
        //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdateConsentMaster");
        //        dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
        //        dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
        //        dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);

        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
        //        dbServer.AddInParameter(command, "TemplateName", DbType.String, objItemVO.TemplateName);


        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objItem;
        //}

        ///// <summary>
        ///// fills consent master list
        ///// </summary>
        ///// <param name="valueObject"> clsGetConsentMasterBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns>clsGetConsentMasterBizActionVO object</returns>
        //public override IValueObject GetConsentMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetConsentMasterBizActionVO objItem = valueObject as clsGetConsentMasterBizActionVO;
        //    clsConsentMasterVO objItemVO = null;
        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetConsentMaster");
        //        dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
        //        dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objItemVO = new clsConsentMasterVO();
        //                objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
        //                objItemVO.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]));
        //                objItem.ConsentMatserDetails.Add(objItemVO);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objItem;
        //}

        ///// <summary>
        ///// Gets Instruction details
        ///// </summary>
        //public override IValueObject GetInstructionDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetInstructionDetailsBizActionVO objItem = valueObject as clsGetInstructionDetailsBizActionVO;
        //    clsInstructionMasterVO objItemVO = null;
        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetInstructionDetails");
        //        dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
        //        dbServer.AddInParameter(command, "FilterCriteria", DbType.String, objItem.FilterCriteria);
        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objItemVO = new clsInstructionMasterVO();
        //                objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                objItemVO.SelectInstruction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstructionType"]));
        //                objItemVO.SelectInstruction = objItemVO.Instruction.FirstOrDefault(q => q.ID == objItemVO.SelectInstruction.ID);
        //                objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
        //                objItem.InstructionMasterDetails.Add(objItemVO);
        //                // ObjTemp.SelectedPrintName.ID = (long)DALHelper.HandleDBNull(reader["PrintNameType"]);
        //                //ObjTemp.SelectedPrintName = ObjTemp.PrintName.FirstOrDefault(q => q.ID == ObjTemp.SelectedPrintName.ID);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objItem;
        //}


        ///// <summary>
        ///// Gets Instruction details by instruction id
        ///// </summary>
        //public override IValueObject GetInstructionDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetInstructionDetailsByIDBizActionVO BizAction = valueObject as clsGetInstructionDetailsByIDBizActionVO;
        //    try
        //    {
        //        clsInstructionMasterVO objVO = BizAction.InstructionDetails;
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetInstructionDetailsByID");
        //        DbDataReader reader;

        //        dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                if (BizAction.InstructionDetails == null)
        //                    BizAction.InstructionDetails = new clsInstructionMasterVO();
        //                BizAction.InstructionDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //                BizAction.InstructionDetails.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //                BizAction.InstructionDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
        //                BizAction.InstructionDetails.SelectInstruction.ID = (long)DALHelper.HandleDBNull(reader["InstructionType"]);
        //                BizAction.InstructionDetails.SelectInstruction = BizAction.InstructionDetails.Instruction.FirstOrDefault(q => q.ID == BizAction.InstructionDetails.SelectInstruction.ID);
        //                //ObjTemp.SelectedPrintName = ObjTemp.PrintName.FirstOrDefault(q => q.ID == ObjTemp.SelectedPrintName.ID);
        //                BizAction.InstructionDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                BizAction.InstructionDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                //BizAction.InstructionDetails.Formula = (string)DALHelper.HandleDBNull(reader["Formula"]);
        //                //BizAction.InstructionDetails.TextRange = (string)DALHelper.HandleDBNull(reader["TextRange"]);
        //                //BizAction.InstructionDetails.stringstrParameterUnitName = (string)DALHelper.HandleDBNull(reader["ParameterUnitName"]);
        //            }
        //        }
        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return BizAction;

        //}


        ///// <summary>
        ///// Updates instruction status
        ///// </summary>
        //public override IValueObject UpdateInstructionStatus(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    bool CurrentMethodExecutionStatus = true;
        //    clsUpdateInstructionStatusBizActionVO bizObject = valueObject as clsUpdateInstructionStatusBizActionVO;

        //    try
        //    {
        //        clsInstructionMasterVO objTemp = new clsInstructionMasterVO();
        //        objTemp = bizObject.InstructionTempStatus;
        //        DbCommand command = null;
        //        command = dbServer.GetStoredProcCommand("CIMS_UpdateInstructionStatus");

        //        dbServer.AddInParameter(command, "Id", DbType.Int64, objTemp.ID);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objTemp.Status);
        //        dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        bizObject.InstructionTempStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return bizObject;
        //}


        ///// <summary>
        ///// Add Update instruction master
        ///// </summary>
        //public override IValueObject AddUpdateInstructionDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddUpdateInstructionDetailsBizActionVO objInstruction = valueObject as clsAddUpdateInstructionDetailsBizActionVO;
        //    clsInstructionMasterVO Inst = objInstruction.InstMaster;
        //    try
        //    {
        //        DbCommand command = null;
        //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdateInstructionMaster");

        //        if (objInstruction.InstMaster.ID == 0)
        //        {//add new
        //            dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
        //            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        }
        //        else
        //        {//modify
        //            dbServer.AddInParameter(command, "ID", DbType.Int64, Inst.ID);
        //            dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
        //        }

        //        dbServer.AddInParameter(command, "Code", DbType.String, objInstruction.InstMaster.Code);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "Description", DbType.String, objInstruction.InstMaster.Description);
        //        dbServer.AddInParameter(command, "InstructionType", DbType.Int64, objInstruction.InstMaster.SelectInstruction.ID);
        //        //dbServer.AddInParameter(command, "Status", DbType.Boolean, "True");
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        //AddCommonParametersForAddUpdateParameter(command, Inst, UserVo);

        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        objInstruction.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objInstruction;
        //}

        //private void AddCommonParametersForAddUpdateParameter(DbCommand command, clsInstructionMasterVO objInstruction, clsUserVO objUserVO)
        //{
        //    try
        //    {
        //        dbServer.AddInParameter(command, "Code", DbType.String, objInstruction.Code);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        //dbServer.AddInParameter(command, "ParameterUnitId", DbType.Int64, objInstruction.ParamUnit);
        //        dbServer.AddInParameter(command, "Description", DbType.String, objInstruction.Description);
        //        //dbServer.AddInParameter(command, "PrintName", DbType.String, objInstruction.PrintName);
        //        //dbServer.AddInParameter(command, "IsNumeric", DbType.Boolean, objInstruction.IsNumeric);
        //        dbServer.AddInParameter(command, "InstructionType", DbType.Int64, objInstruction.SelectInstruction.ID);
        //        //if (objParameter.IsNumeric == false)
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objInstruction.Status);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Add update procedure master
        ///// </summary>
        //public override IValueObject AddProcedureMaster(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddProcedureMasterBizActionVO BizActionObj = valueObject as clsAddProcedureMasterBizActionVO;
        //    DbTransaction trans = null;
        //    DbConnection con = null;
        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        DbCommand command = null;
        //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureMaster");
        //        command.Connection = con;

        //        dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.ProcDetails.Code);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.ProcDetails.Description);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ProcDetails.Status);
        //        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ProcDetails.ServiceID);
        //        dbServer.AddInParameter(command, "Duration", DbType.String, BizActionObj.ProcDetails.Duration);
        //        dbServer.AddInParameter(command, "ProcedureTypeID", DbType.Int64, BizActionObj.ProcDetails.ProcedureTypeID);
        //        dbServer.AddInParameter(command, "RecommandedAnesthesiaTypeID", DbType.Int64, BizActionObj.ProcDetails.RecommandedAnesthesiaTypeID);
        //        dbServer.AddInParameter(command, "OperationTheatreID", DbType.Int64, BizActionObj.ProcDetails.OperationTheatreID);
        //        dbServer.AddInParameter(command, "OTTableID", DbType.Int64, BizActionObj.ProcDetails.OTTableID);
        //        dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.ProcDetails.Remark);
        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ProcDetails.ID);


        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        BizActionObj.ProcDetails.ID = (long)dbServer.GetParameterValue(command, "ID");


        //        foreach (var item in BizActionObj.ProcDetails.ConsentList)
        //        {
        //            DbCommand command1 = null;
        //            command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureConsentDetails");
        //            command1.Connection = con;


        //            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command1, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
        //            dbServer.AddInParameter(command1, "ConsentID", DbType.Int64, item.ConsentID);
        //            dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
        //            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //            dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

        //            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
        //            int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

        //        }

        //        foreach (var item in BizActionObj.ProcDetails.InstructionList)
        //        {
        //            DbCommand command2 = null;
        //            command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureInstructionDetails");
        //            command2.Connection = con;


        //            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
        //            dbServer.AddInParameter(command2, "InstructionID", DbType.Int64, item.InstructionID);
        //            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
        //            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //            dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command2, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

        //            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
        //            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);

        //        }



        //        foreach (var item in BizActionObj.ProcDetails.ItemList)
        //        {
        //            DbCommand command3 = null;
        //            command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureItemDetails");
        //            command3.Connection = con;


        //            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
        //            dbServer.AddInParameter(command3, "ItemID", DbType.Int64, item.ItemID);
        //            dbServer.AddInParameter(command3, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
        //            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //            dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command3, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

        //            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);
        //            int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);

        //        }

        //        foreach (var item in BizActionObj.ProcDetails.ServiceList)
        //        {
        //            DbCommand command4 = null;
        //            command4 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureServiceDetails");
        //            command4.Connection = con;


        //            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command4, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
        //            dbServer.AddInParameter(command4, "ServiceID", DbType.Int64, item.ServiceID);
        //            dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
        //            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command4, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //            dbServer.AddInParameter(command4, "UpdatedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command4, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command4, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command4, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

        //            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
        //            int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);

        //        }

        //        foreach (var item in BizActionObj.ProcDetails.StaffList)
        //        {
        //            DbCommand command5 = null;
        //            command5 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureStaffDetails");
        //            command5.Connection = con;


        //            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command5, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
        //            dbServer.AddInParameter(command5, "IsDoctor", DbType.Boolean, item.IsDoctor);
        //            dbServer.AddInParameter(command5, "DocOrStaffID", DbType.Int64, item.DocOrStaffID);
        //            dbServer.AddInParameter(command5, "NoofDocOrStaff", DbType.Int64, item.NoofDocOrStaff);
        //            dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
        //            dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command5, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //            dbServer.AddInParameter(command5, "UpdatedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command5, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command5, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command5, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

        //            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);
        //            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);

        //        }

        //        foreach (var item in BizActionObj.ProcDetails.CheckList)
        //        {
        //            DbCommand command6 = null;
        //            command6 = dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureChecklistDetails");
        //            command6.Connection = con;


        //            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command6, "ProcedureID", DbType.Int64, BizActionObj.ProcDetails.ID);
        //            dbServer.AddInParameter(command6, "Category", DbType.String, item.Category);
        //            dbServer.AddInParameter(command6, "SubCategory1", DbType.String, item.SubCategory1);
        //            dbServer.AddInParameter(command6, "SubCategory2", DbType.String, item.SubCategory2);
        //            dbServer.AddInParameter(command6, "Remarks", DbType.String, item.Remarks);
        //            dbServer.AddInParameter(command6, "Name", DbType.String, item.Name);
        //            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
        //            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command6, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //            dbServer.AddInParameter(command6, "UpdatedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command6, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command6, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command6, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

        //            dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, int.MaxValue);
        //            int intStatus1 = dbServer.ExecuteNonQuery(command6, trans);
        //            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

        //        }
        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        trans = null;
        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// get procedure master
        ///// </summary>
        //public override IValueObject GetProcedureMaster(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetProcedureMasterBizActionVO objGetProc = valueObject as clsGetProcedureMasterBizActionVO;
        //    clsProcedureMasterVO objProcVO = null;

        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetProcedureMasterList");
        //        dbServer.AddInParameter(command, "Description", DbType.String, objGetProc.Description);
        //        dbServer.AddInParameter(command, "ProcedureTypeID", DbType.Int64, objGetProc.ProcedureTypeID);
        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objGetProc.IsPagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objGetProc.StartIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objGetProc.MaximumRows);
        //        dbServer.AddInParameter(command, "sortExpression", DbType.String, objGetProc.SortExpression);

        //        dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                objProcVO = new clsProcedureMasterVO();
        //                objProcVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                objProcVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                objProcVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                objProcVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objProcVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
        //                objProcVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
        //                objProcVO.Duration = Convert.ToString(DALHelper.HandleDBNull(reader["Duration"]));
        //                objProcVO.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
        //                objProcVO.RecommandedAnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"]));
        //                objProcVO.OperationTheatreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreID"]));
        //                objProcVO.OTTableID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"]));
        //                objProcVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
        //                objGetProc.ProcDetails.Add(objProcVO);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objGetProc;
        //}

        ///// <summary>
        ///// Gets procedure details from procedure id
        ///// </summary>
        //public override IValueObject GetProcDetailsByProcID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetProcDetailsByProcIDBizActionVO objGetProc = valueObject as clsGetProcDetailsByProcIDBizActionVO;


        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetProcDetailsByProcID");
        //        dbServer.AddInParameter(command, "procID", DbType.Int64, objGetProc.ProcID);



        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                clsProcedureChecklistDetailsVO checkListObj = new clsProcedureChecklistDetailsVO();
        //                checkListObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                checkListObj.Category = Convert.ToString(DALHelper.HandleDBNull(reader["Category"]));
        //                checkListObj.SubCategory1 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory1"]));
        //                checkListObj.SubCategory2 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory2"]));
        //                checkListObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
        //                checkListObj.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
        //                checkListObj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

        //                objGetProc.CheckList.Add(checkListObj);
        //            }


        //            reader.NextResult();
        //            while (reader.Read())
        //            {
        //                clsProcedureConsentDetailsVO consentObj = new clsProcedureConsentDetailsVO();
        //                consentObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                consentObj.ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentID"]));
        //                consentObj.ConsentDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objGetProc.ConsentList.Add(consentObj);
        //            }

        //            reader.NextResult();
        //            while (reader.Read())
        //            {
        //                clsProcedureInstructionDetailsVO InstructionObj = new clsProcedureInstructionDetailsVO();
        //                InstructionObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                InstructionObj.InstructionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstuctionID"]));
        //                InstructionObj.InstructionDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objGetProc.InstructionList.Add(InstructionObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsProcedureItemDetailsVO ItemObj = new clsProcedureItemDetailsVO();
        //                ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ItemObj.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
        //                ItemObj.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));

        //                objGetProc.ItemList.Add(ItemObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsProcedureServiceDetailsVO ServiceObj = new clsProcedureServiceDetailsVO();
        //                ServiceObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ServiceObj.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
        //                ServiceObj.ServiceDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                objGetProc.ServiceList.Add(ServiceObj);
        //            }

        //            reader.NextResult();


        //            while (reader.Read())
        //            {
        //                clsProcedureStaffDetailsVO StaffObj = new clsProcedureStaffDetailsVO();
        //                StaffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                StaffObj.DocOrStaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocOrStaffID"]));
        //                StaffObj.DocClassification = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                StaffObj.NoofDocOrStaff = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"]));
        //                StaffObj.IsDoctor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]));

        //                objGetProc.StaffList.Add(StaffObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsProcedureStaffDetailsVO StaffObj = new clsProcedureStaffDetailsVO();
        //                StaffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                StaffObj.DocOrStaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocOrStaffID"]));
        //                StaffObj.DocClassification = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                StaffObj.NoofDocOrStaff = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"]));
        //                StaffObj.IsDoctor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]));

        //                objGetProc.StaffList.Add(StaffObj);
        //            }

        //            reader.NextResult();


        //        }
        //        reader.Close();



        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return objGetProc;
        //}

        ///// <summary>
        ///// Gets OT for selected procedure
        ///// </summary>

        //public override IValueObject GetOTForProcedure(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetOTForProcedureBizActionVO bizActionVo = valueObject as clsGetOTForProcedureBizActionVO;
        //    try
        //    {
        //        DbDataReader reader = null;


        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("GetGetOTForProcedure");
        //        dbServer.AddInParameter(command, "procedureID", DbType.Int64, bizActionVo.procedureID);

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                MasterListItem OtObj = new MasterListItem();
        //                OtObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                OtObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.OTDetails.Add(OtObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                MasterListItem docTypeObj = new MasterListItem();
        //                docTypeObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                docTypeObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.DocDetails.Add(docTypeObj);

        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                MasterListItem designationObj = new MasterListItem();
        //                designationObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                designationObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

        //                bizActionVo.DesignationDetails.Add(designationObj);

        //            }

        //        }
        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return bizActionVo;
        //}


        ///// <summary>
        ///// Gets doctor for doctor classification
        ///// </summary>

        //public override IValueObject GetDoctorForDocType(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetDoctorForDoctorTypeBizActionVO bizActionVo = valueObject as clsGetDoctorForDoctorTypeBizActionVO;
        //    try
        //    {
        //        DbDataReader reader = null;


        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetDoctorByDocType");
        //        dbServer.AddInParameter(command, "DocType", DbType.Int64, bizActionVo.docTypeID);

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                MasterListItem docObj = new MasterListItem();
        //                docObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                docObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.DocDetails.Add(docObj);
        //            }
        //        }
        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return bizActionVo;
        //}

        ///// <summary>
        ///// Gets doctor for doctor classification
        ///// </summary>

        //public override IValueObject GetStaffByDesignation(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsStaffByDesignationIDBizActionVO bizActionVo = valueObject as clsStaffByDesignationIDBizActionVO;
        //    try
        //    {
        //        DbDataReader reader = null;


        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetStaffByDesignation");
        //        dbServer.AddInParameter(command, "DesignationID", DbType.Int64, bizActionVo.DesignationID);
        //        dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, bizActionVo.ProcedureID);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                MasterListItem staffObj = new MasterListItem();
        //                staffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                staffObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.StaffDetails.Add(staffObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                bizActionVo.staffQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"]));
        //            }
        //        }


        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return bizActionVo;
        //}

        ///// <summary>
        /////Add Update Patient proc Schedule Master.
        ///// </summary>

        //public override IValueObject AddupdatePatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddupdatePatientProcedureSchedulebizActionVO bizActionVo = valueObject as clsAddupdatePatientProcedureSchedulebizActionVO;


        //    DbTransaction trans = null;
        //    DbConnection con = null;
        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        DbCommand command = null;
        //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcedureScheduleMaster");
        //        command.Connection = con;


        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.patientProcScheduleDetails.PatientID);
        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, bizActionVo.patientProcScheduleDetails.Date);
        //        dbServer.AddInParameter(command, "StartTime", DbType.DateTime, bizActionVo.patientProcScheduleDetails.StartTime);
        //        dbServer.AddInParameter(command, "EndTime", DbType.DateTime, bizActionVo.patientProcScheduleDetails.EndTime);
        //        dbServer.AddInParameter(command, "EstimatedProcedureTime", DbType.Int64, bizActionVo.patientProcScheduleDetails.EstimatedProcedureTime);
        //        dbServer.AddInParameter(command, "Remarks", DbType.String, bizActionVo.patientProcScheduleDetails.Remarks);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, bizActionVo.patientProcScheduleDetails.Status);


        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.patientProcScheduleDetails.ID);


        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        bizActionVo.patientProcScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
        //        bizActionVo.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

        //        foreach (var item in bizActionVo.patientProcScheduleDetails.PatientProcList)
        //        {
        //            DbCommand command1 = null;
        //            command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcdure");
        //            command1.Connection = con;


        //            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command1, "PatientID", DbType.Int64, bizActionVo.patientProcScheduleDetails.PatientID);
        //            dbServer.AddInParameter(command1, "ProcedureID", DbType.Int64, item.ProcedureID);
        //            dbServer.AddInParameter(command1, "PatientProcedureScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);


        //            dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
        //            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);

        //            dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

        //            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
        //            int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);


        //        }

        //        foreach (var item in bizActionVo.patientProcScheduleDetails.DocScheduleList)
        //        {
        //            DbCommand command2 = null;
        //            command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcDocSchedule");
        //            command2.Connection = con;


        //            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "PatientProcedureScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
        //            dbServer.AddInParameter(command2, "DocTypeID", DbType.Int64, item.DocTypeID);
        //            dbServer.AddInParameter(command2, "DocID", DbType.Int64, item.DocID);
        //            dbServer.AddInParameter(command2, "DayID", DbType.Int64, item.DayID);
        //            dbServer.AddInParameter(command2, "StartTime", DbType.DateTime, item.StartTime);
        //            dbServer.AddInParameter(command2, "EndTime", DbType.DateTime, item.EndTime);
        //            dbServer.AddInParameter(command2, "Date", DbType.DateTime, bizActionVo.patientProcScheduleDetails.Date);
        //            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

        //            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
        //            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

        //            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);


        //        }

        //        foreach (var item in bizActionVo.patientProcScheduleDetails.StaffList)
        //        {
        //            DbCommand command2 = null;
        //            command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcStaffDetails");
        //            command2.Connection = con;


        //            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "PatientProcedureScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
        //            dbServer.AddInParameter(command2, "DesignationID", DbType.Int64, item.DesignationID);
        //            dbServer.AddInParameter(command2, "StaffID", DbType.Int64, item.StaffID);
        //            dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);

        //            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

        //            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
        //            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

        //            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);


        //        }

        //        foreach (var item in bizActionVo.patientProcScheduleDetails.OTScedhuleList)
        //        {
        //            DbCommand command3 = null;
        //            command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcOTSchedule");
        //            command3.Connection = con;


        //            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command3, "PatientProcedureScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
        //            dbServer.AddInParameter(command3, "OTID", DbType.Int64, item.OTID);
        //            dbServer.AddInParameter(command3, "OTTableID", DbType.Int64, item.OTTableID);
        //            dbServer.AddInParameter(command3, "Date", DbType.DateTime, item.Date);
        //            dbServer.AddInParameter(command3, "DayID", DbType.Double, item.DayID);
        //            dbServer.AddInParameter(command3, "StartTime", DbType.DateTime, item.StartTime);
        //            dbServer.AddInParameter(command3, "EndTime", DbType.DateTime, item.EndTime);

        //            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);

        //            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
        //            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

        //            int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);


        //        }



        //        foreach (var item in bizActionVo.patientProcScheduleDetails.PatientProcCheckList)
        //        {
        //            DbCommand command4 = null;
        //            command4 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcedureChecklistDetails");
        //            command4.Connection = con;


        //            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command4, "PatientProcScheduleID", DbType.Int64, bizActionVo.patientProcScheduleDetails.ID);
        //            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, item.PatientID);
        //            dbServer.AddInParameter(command4, "Category", DbType.String, item.Category);
        //            dbServer.AddInParameter(command4, "SubCategory1", DbType.String, item.SubCategory1);
        //            dbServer.AddInParameter(command4, "SubCategory2", DbType.String, item.SubCategory2);
        //            dbServer.AddInParameter(command4, "Remarks", DbType.String, item.Remarks);
        //            dbServer.AddInParameter(command4, "Name", DbType.String, item.Name);

        //            dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);

        //            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
        //            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

        //            int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);


        //        }




        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        throw;
        //    }
        //    return bizActionVo;
        //}

        //public override IValueObject GetPatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetPatientProcedureScheduleBizActionVO bizActionVo = valueObject as clsGetPatientProcedureScheduleBizActionVO;
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetPatientProcedureScheduleList");

        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizActionVo.IsPagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, bizActionVo.StartIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int64, bizActionVo.MaximumRows);
        //        dbServer.AddInParameter(command, "sortExpression", DbType.String, bizActionVo.SortExpression);
        //        dbServer.AddInParameter(command, "OTID", DbType.Int64, bizActionVo.OTID);
        //        dbServer.AddInParameter(command, "OTTableID", DbType.Int64, bizActionVo.OTTableID);
        //        dbServer.AddInParameter(command, "DocID", DbType.Int64, bizActionVo.DocID);
        //        dbServer.AddInParameter(command, "StaffID", DbType.Int64, bizActionVo.StaffID);

        //        dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                clsPatientProcedureScheduleVO patientProcScheduleObj = new clsPatientProcedureScheduleVO();
        //                patientProcScheduleObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                patientProcScheduleObj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                patientProcScheduleObj.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
        //                patientProcScheduleObj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
        //                patientProcScheduleObj.StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"]));
        //                patientProcScheduleObj.EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"]));
        //                patientProcScheduleObj.EstimatedProcedureTime = Convert.ToInt64(DALHelper.HandleDBNull(reader["EstimatedProcedureTime"]));

        //                patientProcScheduleObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
        //                patientProcScheduleObj.OT = Convert.ToString(DALHelper.HandleDBNull(reader["OTName"]));
        //                patientProcScheduleObj.OTTable = Convert.ToString(DALHelper.HandleDBNull(reader["OTTableName"]));

        //                bizActionVo.patientProcScheduleDetails.Add(patientProcScheduleObj);

        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}

        //public override IValueObject GetProcScheduleDetailsByProcScheduleID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetProcScheduleDetailsByProcScheduleIDBizActionVO bizActionVo = valueObject as clsGetProcScheduleDetailsByProcScheduleIDBizActionVO;
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetProcScheduleDetailsByProcScheduleID");

        //        dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleID);
        //        long ID = 1;
        //        dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, ID); //bizActionVo.ScheduleUnitID);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                clsPatientProcedureVO patientProcObj = new clsPatientProcedureVO();
        //                patientProcObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                patientProcObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                patientProcObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["procID"]));
        //                patientProcObj.AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"]));
        //                patientProcObj.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
        //                patientProcObj.ProcedureUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureUnitID"]));
        //                patientProcObj.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
        //                bizActionVo.PatientProcList.Add(patientProcObj);
        //            }

        //            reader.NextResult();
        //            bizActionVo.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
        //            while (reader.Read())
        //            {
        //                clsPatientProcDocScheduleDetailsVO docScheduleVO = new clsPatientProcDocScheduleDetailsVO();
        //                docScheduleVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                docScheduleVO.SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationCode"]));
        //                docScheduleVO.DocID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocID"]));
        //                docScheduleVO.DocTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocTypeID"]));
        //                docScheduleVO.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"]));
        //                docScheduleVO.docTypeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocTypeDesc"]));
        //                docScheduleVO.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["DocTypeDesc"]));
        //                docScheduleVO.ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]));
        //                docScheduleVO.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                docScheduleVO.docDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocDesc"]));
        //                docScheduleVO.DayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"]));
        //                docScheduleVO.StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"]));
        //                docScheduleVO.EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"]));
        //                docScheduleVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
        //                docScheduleVO.StrStartTime = docScheduleVO.StartTime.ToShortTimeString();
        //                docScheduleVO.StrEndTime = docScheduleVO.EndTime.ToShortTimeString();
        //                //docScheduleVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
        //                bizActionVo.DocScheduleDetails.Add(docScheduleVO);
        //            }

        //            reader.NextResult();
        //            bizActionVo.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
        //            while (reader.Read())
        //            {
        //                clsPatientProcStaffDetailsVO staffDetailObj = new clsPatientProcStaffDetailsVO();
        //                staffDetailObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                staffDetailObj.DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DesignationID"]));
        //                staffDetailObj.designationDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DesignationDesc"]));
        //                staffDetailObj.StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffID"]));
        //                staffDetailObj.ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]));
        //                staffDetailObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                staffDetailObj.stffDesc = Convert.ToString(DALHelper.HandleDBNull(reader["staffDesc"]));
        //                staffDetailObj.Quantity = (float)Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                bizActionVo.StaffDetailList.Add(staffDetailObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsPatientProcedureScheduleVO OTScheduleObj = new clsPatientProcedureScheduleVO();
        //                OTScheduleObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                OTScheduleObj.OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreID"]));
        //                OTScheduleObj.OTUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreUnitID"]));
        //                OTScheduleObj.OTTableID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"]));
        //                OTScheduleObj.StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"]));
        //                OTScheduleObj.Opd_Ipd = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Opd_Ipd"]));
        //                OTScheduleObj.VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmID"]));
        //                OTScheduleObj.VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmUnitID"]));
        //                OTScheduleObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
        //                OTScheduleObj.SpecialRequirement = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRequirement"]));
        //                OTScheduleObj.EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"]));
        //                OTScheduleObj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
        //                OTScheduleObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));

        //                bizActionVo.OTScheduleList.Add(OTScheduleObj);
        //            }

        //            reader.NextResult();
        //            while (reader.Read())
        //            {
        //                bizActionVo.patientInfoObject.pateintID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])); ;
        //                bizActionVo.patientInfoObject.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
        //                bizActionVo.patientInfoObject.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
        //                //bizActionVo.patientInfoObject.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
        //                bizActionVo.patientInfoObject.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
        //                bizActionVo.patientInfoObject.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DateOfBirth"]));
        //                bizActionVo.patientInfoObject.patientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //            }

        //            reader.NextResult();
        //            bizActionVo.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        //            while (reader.Read())
        //            {
        //                clsPatientProcedureChecklistDetailsVO checklistObj = new clsPatientProcedureChecklistDetailsVO();
        //                checklistObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                checklistObj.Category = Convert.ToString(DALHelper.HandleDBNull(reader["Category"]));
        //                checklistObj.SubCategory1 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory1"]));
        //                checklistObj.SubCategory2 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory2"]));
        //                checklistObj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
        //                checklistObj.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
        //                checklistObj.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
        //                bizActionVo.CheckList.Add(checklistObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                bizActionVo.AnesthesiaNotes = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));
        //                bizActionVo.detailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailsID"]));
        //            }

        //            reader.NextResult();
        //            bizActionVo.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
        //            while (reader.Read())
        //            {
        //                clsDoctorSuggestedServiceDetailVO ServiceListObj = new clsDoctorSuggestedServiceDetailVO();
        //                ServiceListObj.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
        //                ServiceListObj.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
        //                ServiceListObj.ServiceType = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceType"]));
        //                ServiceListObj.GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["GroupName"]));
        //                bizActionVo.ServiceList.Add(ServiceListObj);
        //            }

        //            reader.NextResult();
        //            bizActionVo.PreOperativeInstructionList = new List<string>();
        //            bizActionVo.InstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
        //            while (reader.Read())
        //            {
        //                clsOTDetailsInstructionListDetailsVO ObjInstructionList = new clsOTDetailsInstructionListDetailsVO();
        //                ObjInstructionList.GroupName = "Pre Operative Instruction Notes";
        //                ObjInstructionList.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.InstructionList.Add(ObjInstructionList);
        //                bizActionVo.PreOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
        //            }

        //            reader.NextResult();
        //            bizActionVo.IntraOperativeInstructionList = new List<string>();
        //            while (reader.Read())
        //            {
        //                clsOTDetailsInstructionListDetailsVO objInstructionList = new clsOTDetailsInstructionListDetailsVO();
        //                objInstructionList.GroupName = "Intra Operative Instruction Notes";
        //                objInstructionList.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.InstructionList.Add(objInstructionList);
        //                bizActionVo.IntraOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
        //            }

        //            reader.NextResult();
        //            bizActionVo.PostOperativeInstructionList = new List<string>();
        //            while (reader.Read())
        //            {
        //                clsOTDetailsInstructionListDetailsVO objInstructionList = new clsOTDetailsInstructionListDetailsVO();
        //                objInstructionList.GroupName = "Post Operative Instruction Notes";
        //                objInstructionList.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.InstructionList.Add(objInstructionList);
        //                bizActionVo.PostOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
        //            }

        //            reader.NextResult();

        //            bizActionVo.AddedPatientProcList = new List<clsPatientProcedureVO>();
        //            while (reader.Read())
        //            {

        //                clsPatientProcedureVO patientProcObj = new clsPatientProcedureVO();
        //                patientProcObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                patientProcObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.AddedPatientProcList.Add(patientProcObj);
        //            }


        //            reader.NextResult();
        //            bizActionVo.ItemList = new List<clsProcedureItemDetailsVO>();
        //            while (reader.Read())
        //            {
        //                clsProcedureItemDetailsVO ItemObj = new clsProcedureItemDetailsVO();
        //                //ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ItemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
        //                ItemObj.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                bizActionVo.ItemList.Add(ItemObj);
        //            }

        //            //bizActionVo.PostInstructionList = new List<clsOTDetailsPostInstructionDetailsVO>();
        //            //bizActionVo.PostInstruction = new List<string>();

        //            //while (reader.Read())
        //            //{
        //            //    clsOTDetailsPostInstructionDetailsVO PostInstructionListObj = new clsOTDetailsPostInstructionDetailsVO();
        //            //    PostInstructionListObj.PostInstructionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Code"]));
        //            //    PostInstructionListObj.PostInstruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //            //    bizActionVo.PostInstruction.Add(PostInstructionListObj.PostInstruction);
        //            //    bizActionVo.PostInstructionList.Add(PostInstructionListObj);
        //            //}




        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}
        ///// <summary>
        ///// Check existing OT schedule
        ///// </summary>

        //public override IValueObject CheckOTScheduleExistance(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsCheckTimeForOTScheduleExistanceBizActionVO bizActionVo = valueObject as clsCheckTimeForOTScheduleExistanceBizActionVO;
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_CheckOTScheduleExistance");

        //        dbServer.AddInParameter(command, "OTTableID", DbType.Int64, bizActionVo.OTTableID);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                if (bizActionVo.Details == null)
        //                    bizActionVo.Details = new List<clsOTScheduleDetailsVO>();

        //                while (reader.Read())
        //                {
        //                    clsOTScheduleDetailsVO objOTScheduleVO = new clsOTScheduleDetailsVO();
        //                    objOTScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                    objOTScheduleVO.OTTableID = (long)DALHelper.HandleDBNull(reader["OTTableID"]);
        //                    objOTScheduleVO.OTID = (long)DALHelper.HandleDBNull(reader["OTID"]);
        //                    objOTScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
        //                    objOTScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
        //                    objOTScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
        //                    objOTScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);

        //                    bizActionVo.SuccessStatus = true;

        //                    bizActionVo.Details.Add(objOTScheduleVO);



        //                }

        //                reader.Close();
        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}

        ///// <summary>
        ///// Check existing OT schedule
        ///// </summary>

        //public override IValueObject AddOTScheduleMaster(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddOTScheduleMasterBizActionVO bizActionVO = valueObject as clsAddOTScheduleMasterBizActionVO;
        //    DbConnection con = null;
        //    DbTransaction trans = null;
        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        clsOTScheduleVO objOTScheduleVO = bizActionVO.OTScheduleDetails;
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddOTSchedule");
        //        command.Connection = con;

        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "OTID", DbType.Int64, objOTScheduleVO.OTID);
        //        dbServer.AddInParameter(command, "OTTableID", DbType.Int64, objOTScheduleVO.OTTableID);

        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objOTScheduleVO.ID);
        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        bizActionVO.OTScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

        //        foreach (var ObjDetails in objOTScheduleVO.OTScheduleDetailsList)
        //        {
        //            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddOTScheduleDetails");
        //            command1.Connection = con;

        //            dbServer.AddInParameter(command1, "OTScheduleID", DbType.Int64, objOTScheduleVO.ID);
        //            dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
        //            dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
        //            dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
        //            dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
        //            dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
        //            dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

        //            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

        //            int iStatus = dbServer.ExecuteNonQuery(command1, trans);
        //            ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");


        //        }
        //        trans.Commit();


        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        throw;

        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //        trans = null;
        //    }
        //    return bizActionVO;
        //}

        ///// <summary>
        ///// Gets OT schedule
        ///// </summary>
        //public override IValueObject GetOTScheduleMasterList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetOTScheduleMasterListBizActionVO BizActionObj = (clsGetOTScheduleMasterListBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetOTScheduleBySearchCriteria");
        //        dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");


        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
        //        dbServer.AddInParameter(command, "OTTableId", DbType.Int64, BizActionObj.OTTableID);

        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
        //        dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
        //        dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.OTScheduleList == null)
        //                BizActionObj.OTScheduleList = new List<clsOTScheduleVO>();

        //            while (reader.Read())
        //            {
        //                clsOTScheduleVO objOTScheduleVO = new clsOTScheduleVO();
        //                objOTScheduleVO.ID = (long)reader["ID"];
        //                objOTScheduleVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                objOTScheduleVO.OTID = (long)DALHelper.HandleDBNull(reader["OTID"]);
        //                objOTScheduleVO.OTName = (string)DALHelper.HandleDBNull(reader["OTName"]);
        //                objOTScheduleVO.OTTableID = (long)DALHelper.HandleDBNull(reader["OTTableID"]);
        //                objOTScheduleVO.OTTableName = (string)DALHelper.HandleDBNull(reader["OTTableName"]);
        //                objOTScheduleVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                BizActionObj.OTScheduleList.Add(objOTScheduleVO);
        //            }

        //        }

        //        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Gets OT schedule Details
        ///// </summary>
        //public override IValueObject GetOTScheduleDetailList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetOTScheduleListBizActionVO BizActionObj = (clsGetOTScheduleListBizActionVO)valueObject;

        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetOTScheduleList");

        //        dbServer.AddInParameter(command, "OTScheduleID ", DbType.Int64, BizActionObj.OTScheduleID);


        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.OTScheduleList == null)
        //                BizActionObj.OTScheduleList = new List<clsOTScheduleDetailsVO>();

        //            while (reader.Read())
        //            {
        //                clsOTScheduleDetailsVO objOTScheduleVO = new clsOTScheduleDetailsVO();
        //                objOTScheduleVO.ID = (long)reader["ID"];
        //                objOTScheduleVO.OTScheduleID = (long)reader["OTScheduleID"];
        //                objOTScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);
        //                if (objOTScheduleVO.DayID == 1)
        //                    objOTScheduleVO.Day = "Sunday";
        //                else if (objOTScheduleVO.DayID == 2)
        //                    objOTScheduleVO.Day = "Monday";
        //                else if (objOTScheduleVO.DayID == 3)
        //                    objOTScheduleVO.Day = "Tuesday";
        //                else if (objOTScheduleVO.DayID == 4)
        //                    objOTScheduleVO.Day = "Wednesday";
        //                else if (objOTScheduleVO.DayID == 5)
        //                    objOTScheduleVO.Day = "Thursday";
        //                else if (objOTScheduleVO.DayID == 6)
        //                    objOTScheduleVO.Day = "Friday";
        //                else if (objOTScheduleVO.DayID == 7)
        //                    objOTScheduleVO.Day = "Saturday";

        //                objOTScheduleVO.ScheduleID = (long)DALHelper.HandleDBNull(reader["ScheduleID"]);
        //                objOTScheduleVO.Schedule = (string)DALHelper.HandleDBNull(reader["Schedule"]);
        //                objOTScheduleVO.StartTime = (DateTime)DALHelper.HandleDBNull(reader["StartTime"]);
        //                objOTScheduleVO.EndTime = (DateTime)DALHelper.HandleDBNull(reader["EndTime"]);
        //                BizActionObj.OTScheduleList.Add(objOTScheduleVO);
        //            }
        //        }
        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Check existing OT schedule
        ///// </summary>

        public override IValueObject UpdateOTSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            //clsAddOTScheduleMasterBizActionVO bizActionVO = valueObject as clsAddOTScheduleMasterBizActionVO;
            //DbConnection con = null;
            //DbTransaction trans = null;
            //try
            //{
            //    con = dbServer.CreateConnection();
            //    con.Open();
            //    trans = con.BeginTransaction();

            //    clsOTScheduleVO objOTScheduleVO = bizActionVO.OTScheduleDetails;
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddOTSchedule");
            //    command.Connection = con;

            //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            //    dbServer.AddInParameter(command, "OTID", DbType.Int64, objOTScheduleVO.OTID);
            //    dbServer.AddInParameter(command, "OTTableID", DbType.Int64, objOTScheduleVO.OTTableID);

            //    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
            //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

            //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
            //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

            //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objOTScheduleVO.ID);
            //    int intStatus = dbServer.ExecuteNonQuery(command, trans);
            //    bizActionVO.OTScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

            //    foreach (var ObjDetails in objOTScheduleVO.OTScheduleDetailsList)
            //    {
            //        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddOTScheduleDetails");
            //        command1.Connection = con;

            //        dbServer.AddInParameter(command1, "OTScheduleID", DbType.Int64, objOTScheduleVO.ID);
            //        dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
            //        dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
            //        dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
            //        dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
            //        dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
            //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

            //        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

            //        int iStatus = dbServer.ExecuteNonQuery(command1, trans);
            //        ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");


            //    }
            //    trans.Commit();


            //}
            //catch (Exception ex)
            //{
            //    trans.Rollback();
            //    throw;

            //}
            //finally
            //{
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //    }
            //    trans = null;
            //}
            return valueObject;
        }

        ///// <summary>
        ///// Grets Ot Scehdule
        ///// </summary>
        //public override IValueObject GetOTScheduleTime(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    //clsAddOTScheduleMasterBizActionVO bizActionVO = valueObject as clsAddOTScheduleMasterBizActionVO;
        //    //DbConnection con = null;
        //    //DbTransaction trans = null;
        //    //try
        //    //{
        //    //    con = dbServer.CreateConnection();
        //    //    con.Open();
        //    //    trans = con.BeginTransaction();

        //    //    clsOTScheduleVO objOTScheduleVO = bizActionVO.OTScheduleDetails;
        //    //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddOTSchedule");
        //    //    command.Connection = con;

        //    //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //    //    dbServer.AddInParameter(command, "OTID", DbType.Int64, objOTScheduleVO.OTID);
        //    //    dbServer.AddInParameter(command, "OTTableID", DbType.Int64, objOTScheduleVO.OTTableID);

        //    //    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
        //    //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //    //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //    //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //    //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
        //    //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //    //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objOTScheduleVO.ID);
        //    //    int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //    //    bizActionVO.OTScheduleDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

        //    //    foreach (var ObjDetails in objOTScheduleVO.OTScheduleDetailsList)
        //    //    {
        //    //        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddOTScheduleDetails");
        //    //        command1.Connection = con;

        //    //        dbServer.AddInParameter(command1, "OTScheduleID", DbType.Int64, objOTScheduleVO.ID);
        //    //        dbServer.AddInParameter(command1, "DayID", DbType.Int64, ObjDetails.DayID);
        //    //        dbServer.AddInParameter(command1, "ScheduleID", DbType.Int64, ObjDetails.ScheduleID);
        //    //        dbServer.AddInParameter(command1, "StartTime", DbType.DateTime, ObjDetails.StartTime);
        //    //        dbServer.AddInParameter(command1, "EndTime", DbType.DateTime, ObjDetails.EndTime);
        //    //        dbServer.AddInParameter(command1, "ApplyToAllDay", DbType.Boolean, ObjDetails.ApplyToAllDay);
        //    //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

        //    //        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ID);

        //    //        int iStatus = dbServer.ExecuteNonQuery(command1, trans);
        //    //        ObjDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");


        //    //    }
        //    //    trans.Commit();


        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    trans.Rollback();
        //    //    throw;

        //    //}
        //    //finally
        //    //{
        //    //    if (con.State == ConnectionState.Open)
        //    //    {
        //    //        con.Close();
        //    //    }
        //    //    trans = null;
        //    //}
        //    return valueObject;
        //}

        ///// <summary>
        ///// Gets Procedure wise OT Scedule
        ///// </summary>
        //public override IValueObject GetProcOTSchedule(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetProcOTScheduleBizActionVO BizActionObj = (clsGetProcOTScheduleBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetProcOTScedule");


        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.OTScheduleDetailsList == null)
        //                BizActionObj.OTScheduleDetailsList = new List<clsPatientProcOTScheduleDetailsVO>();

        //            while (reader.Read())
        //            {
        //                clsPatientProcOTScheduleDetailsVO objOTScheduleVO = new clsPatientProcOTScheduleDetailsVO();
        //                objOTScheduleVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

        //                objOTScheduleVO.OTID = (long)DALHelper.HandleDBNull(reader["OTID"]);
        //                objOTScheduleVO.OTDesc = (string)DALHelper.HandleDBNull(reader["OTDesc"]);
        //                objOTScheduleVO.OTTableID = (long)DALHelper.HandleDBNull(reader["OTTableID"]);
        //                objOTScheduleVO.OTTableDesc = (string)DALHelper.HandleDBNull(reader["OTTableDesc"]);
        //                objOTScheduleVO.StartTime = (DateTime)DALHelper.HandleDate(reader["StartTime"]);
        //                objOTScheduleVO.EndTime = (DateTime)DALHelper.HandleDate(reader["EndTime"]);
        //                objOTScheduleVO.DayID = (long)DALHelper.HandleDBNull(reader["DayID"]);


        //                BizActionObj.OTScheduleDetailsList.Add(objOTScheduleVO);
        //            }

        //        }


        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Gets checklist by procedure id
        ///// </summary>
        //public override IValueObject GetCheckListByProcedureID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetCheckListByProcedureIDBizActionVO BizActionObj = (clsGetCheckListByProcedureIDBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCheckListByProcedureID");
        //        dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, BizActionObj.ProcedureID);


        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ChecklistDetails == null)
        //                BizActionObj.ChecklistDetails = new List<clsPatientProcedureChecklistDetailsVO>();

        //            while (reader.Read())
        //            {
        //                clsPatientProcedureChecklistDetailsVO checkListObj = new clsPatientProcedureChecklistDetailsVO();
        //                checkListObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

        //                checkListObj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                checkListObj.Category = (string)DALHelper.HandleDBNull(reader["Category"]);
        //                checkListObj.SubCategory1 = (string)DALHelper.HandleDBNull(reader["SubCategory1"]);
        //                checkListObj.SubCategory2 = (string)DALHelper.HandleDBNull(reader["SubCategory2"]);
        //                checkListObj.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
        //                checkListObj.Name = (string)DALHelper.HandleDBNull(reader["Name"]);


        //                BizActionObj.ChecklistDetails.Add(checkListObj);
        //            }

        //        }


        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Gets checklist by procedure id
        ///// </summary>
        //public override IValueObject GetCheckListByScheduleID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetCheckListByScheduleIDBizActionVO BizActionObj = (clsGetCheckListByScheduleIDBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCheckListByScheduleID");
        //        dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.ScheduleID);


        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ChecklistDetails == null)
        //                BizActionObj.ChecklistDetails = new List<clsPatientProcedureChecklistDetailsVO>();

        //            while (reader.Read())
        //            {
        //                clsPatientProcedureChecklistDetailsVO checkListObj = new clsPatientProcedureChecklistDetailsVO();
        //                checkListObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

        //                checkListObj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                checkListObj.Category = (string)DALHelper.HandleDBNull(reader["Category"]);
        //                checkListObj.SubCategory1 = (string)DALHelper.HandleDBNull(reader["SubCategory1"]);
        //                checkListObj.SubCategory2 = (string)DALHelper.HandleDBNull(reader["SubCategory2"]);
        //                checkListObj.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
        //                checkListObj.Name = (string)DALHelper.HandleDBNull(reader["Name"]);


        //                BizActionObj.ChecklistDetails.Add(checkListObj);
        //            }

        //        }


        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Gets checklist by procedure id
        ///// </summary>
        //public override IValueObject GetDocScheduleList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetDocScheduleByDocIDBizActionVO BizActionObj = (clsGetDocScheduleByDocIDBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDocScheduleListByDocIDIDAndDate");
        //        dbServer.AddInParameter(command, "DocID", DbType.Int64, BizActionObj.DocID);
        //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "DocTypeID", DbType.Int64, BizActionObj.DocTableID);
        //        dbServer.AddInParameter(command, "ProcDate", DbType.DateTime, BizActionObj.procDate);


        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.DocScheduleListDetails == null)
        //                BizActionObj.DocScheduleListDetails = new List<clsPatientProcDocScheduleDetailsVO>();

        //            while (reader.Read())
        //            {
        //                clsPatientProcDocScheduleDetailsVO docSceduleListObj = new clsPatientProcDocScheduleDetailsVO();
        //                docSceduleListObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);

        //                docSceduleListObj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                docSceduleListObj.DocID = (long)DALHelper.HandleDBNull(reader["DocID"]);
        //                docSceduleListObj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
        //                docSceduleListObj.StartTime = (DateTime)DALHelper.HandleDate(reader["FromTime"]);
        //                docSceduleListObj.EndTime = (DateTime)DALHelper.HandleDate(reader["ToTime"]);
        //                docSceduleListObj.DocTypeID = (long)DALHelper.HandleDBNull(reader["DocTypeID"]);


        //                BizActionObj.DocScheduleListDetails.Add(docSceduleListObj);
        //            }

        //        }


        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Cancel OT Booking
        ///// </summary>
        //public override IValueObject CancelOTBooking(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsCancelOTBookingBizActionVO BizActionObj = (clsCancelOTBookingBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_CancelOTBooking");
        //        dbServer.AddInParameter(command, "PatientProcScheduleID", DbType.Int64, BizActionObj.patientProcScheduleID);
        //        dbServer.AddInParameter(command, "CancelledReason", DbType.String, BizActionObj.CancelledReason);
        //        dbServer.AddInParameter(command, "CancelledBy", DbType.String, BizActionObj.CancelledBy);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "CancelledDate", DbType.DateTime, DateTime.Now);
        //        int intStatus = dbServer.ExecuteNonQuery(command);




        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Gets staff list
        ///// </summary>
        //public override IValueObject GetStaffForOTScheduling(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetStaffForOTSchedulingBizActionVO BizActionObj = (clsGetStaffForOTSchedulingBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStaffForOTScheduling");



        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.staffList == null)
        //                BizActionObj.staffList = new List<MasterListItem>();

        //            while (reader.Read())
        //            {
        //                MasterListItem staffObj = new MasterListItem();
        //                staffObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //                staffObj.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
        //                BizActionObj.staffList.Add(staffObj);
        //            }

        //        }


        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Gets procedure id for OT booking id 
        ///// </summary>
        //public override IValueObject GetProceduresForOTBookingID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetProceduresForOTBookingIDBizActionVO BizActionObj = (clsGetProceduresForOTBookingIDBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetProcedureIDForOTBookingID");
        //        dbServer.AddInParameter(command, "OTBookingID", DbType.Int64, BizActionObj.OTBokingID);


        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.procedureList == null)
        //                BizActionObj.procedureList = new List<MasterListItem>();

        //            while (reader.Read())
        //            {
        //                MasterListItem procObj = new MasterListItem();
        //                procObj.ID = (long)DALHelper.HandleDBNull(reader["ProcedureID"]);

        //                BizActionObj.procedureList.Add(procObj);
        //            }

        //        }


        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        ///// <summary>
        ///// Gets procedure id for OT booking id 
        ///// </summary>
        //public override IValueObject GetConsentForProcedureID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetConsentForProcedureIDBizActionVO BizActionObj = (clsGetConsentForProcedureIDBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetConsentForProcedureID");
        //        dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, BizActionObj.ProcedureID);


        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ConsentList == null)
        //                BizActionObj.ConsentList = new List<MasterListItem>();

        //            while (reader.Read())
        //            {
        //                MasterListItem ConsentObj = new MasterListItem();
        //                ConsentObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //                ConsentObj.Description = (string)DALHelper.HandleDBNull(reader["Description"]);

        //                BizActionObj.ConsentList.Add(ConsentObj);
        //            }

        //        }


        //        reader.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        //#region For IPD Module

        //public override IValueObject GetPatientDetailsForConsent(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetConsentDetailsBizActionVO BizActionObj = (clsGetConsentDetailsBizActionVO)valueObject;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsForConsent");
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ConsentDetails.PatientID);
        //        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ConsentDetails.PatientUnitID);

        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ConsentDetails == null)
        //                BizActionObj.ConsentDetails = new clsConsentDetailsVO();
        //            while (reader.Read())
        //            {
        //                clsConsentDetailsVO objListVO = new clsConsentDetailsVO();

        //                //BizActionObj.ConsentDetails.ID = Convert.ToInt64(reader["Id"]);
        //                //BizActionObj.ConsentDetails.UnitID = Convert.ToInt64(reader["UnitID"]);

        //                //BizActionObj.ConsentDetails.MRNo = Convert.ToString(reader["MRNo"]);

        //                //string fullname = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
        //                //fullname += " " + Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));

        //                //BizActionObj.ConsentDetails.PatientName = fullname;

        //                //BizActionObj.ConsentDetails.PatientAddress = Convert.ToString(DALHelper.HandleDBNull(reader["ResAddress"]));
        //                //BizActionObj.ConsentDetails.PatientContachNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileNo"]));

        //                //BizActionObj.ConsentDetails.KinName = Convert.ToString(DALHelper.HandleDBNull(reader["KinName"]));
        //                //BizActionObj.ConsentDetails.KinAddress = Convert.ToString(DALHelper.HandleDBNull(reader["KinAddress"]));
        //                //BizActionObj.ConsentDetails.KinMobileNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["KinMobileNo"]));
        //                BizActionObj.ConsentDetails.ID = Convert.ToInt64(reader["Id"]);
        //                BizActionObj.ConsentDetails.UnitID = Convert.ToInt64(reader["UnitID"]);
        //                BizActionObj.ConsentDetails.MRNo = Convert.ToString(reader["MRNo"]);
        //                BizActionObj.ConsentDetails.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
        //                BizActionObj.ConsentDetails.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
        //                BizActionObj.ConsentDetails.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
        //                BizActionObj.ConsentDetails.DOB = Convert.ToString(DALHelper.HandleDBNull(reader["DOB"]));
        //                BizActionObj.ConsentDetails.PatientAddress = Convert.ToString(DALHelper.HandleDBNull(reader["ResAddress"]));
        //                BizActionObj.ConsentDetails.Location = Convert.ToString(DALHelper.HandleDBNull(reader["Location"]));
        //                BizActionObj.ConsentDetails.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
        //                BizActionObj.ConsentDetails.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
        //                BizActionObj.ConsentDetails.VisitorFirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["VisitorFirstName"]));
        //                BizActionObj.ConsentDetails.VisitorMiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["VisitorMiddleName"]));
        //                BizActionObj.ConsentDetails.VisitorLastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["VisitorLastName"]));
        //                BizActionObj.ConsentDetails.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AppoinmentDate"]));
        //                BizActionObj.ConsentDetails.DoctorFirstName = (string)DALHelper.HandleDBNull(reader["DoctorFirstName"]);
        //                BizActionObj.ConsentDetails.DoctoLastName = (string)DALHelper.HandleDBNull(reader["DoctoLastName"]);
        //                BizActionObj.ConsentDetails.DoctorMiddleName = (string)DALHelper.HandleDBNull(reader["DoctorMiddleName"]);




        //                break;
        //            }
        //        }
        //        reader.NextResult();
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return BizActionObj;
        //}

        //public override IValueObject GetPatientConsents(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetPatientConsentsBizActionVO BizActionObj = (clsGetPatientConsentsBizActionVO)valueObject;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientConsents");
        //        dbServer.AddInParameter(command, "ConsentType", DbType.Int64, BizActionObj.ConsentTypeID);
        //        dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.VisitAdmID);
        //        dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.VisitAdmUnitID);
        //        dbServer.AddInParameter(command, "OPD_IPD", DbType.Boolean, BizActionObj.OPD_IPD);
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ConsentList == null)
        //                BizActionObj.ConsentList = new List<clsConsentDetailsVO>();
        //            while (reader.Read())
        //            {
        //                clsConsentDetailsVO Consent = new clsConsentDetailsVO();
        //                Consent.ID = (long)DALHelper.HandleDBNull(reader["Id"]);
        //                Consent.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
        //                Consent.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
        //                Consent.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
        //                Consent.Consent = (string)DALHelper.HandleDBNull(reader["Consent"]);
        //                Consent.PatientName = (string)DALHelper.HandleDBNull(reader["PatientName"]);
        //                Consent.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
        //                Consent.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
        //                Consent.Age = (int)DALHelper.HandleDBNull(reader["Age"]);
        //                BizActionObj.ConsentList.Add(Consent);
        //            }

        //            reader.NextResult();
        //        }
        //        reader.Close();

        //    }

        //    catch
        //    {
        //        throw;
        //    }

        //    finally
        //    {
        //    }

        //    return BizActionObj;
        //}

        public override IValueObject GetPatientConsentsDetailsInHTML(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientConsentsDetailsInHTMLBizActionVO BizActionObj = (clsGetPatientConsentsDetailsInHTMLBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientConsentsDetailsInHTML");
                dbServer.AddInParameter(command, "ConsentType", DbType.Int64, BizActionObj.ConsentTypeID);
                dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.VisitAdmID);
                dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.VisitAdmUnitID);
                dbServer.AddInParameter(command, "PrintID", DbType.Int64, BizActionObj.ConsentTypeID);
                //dbServer.AddInParameter(command, "OPD_IPD", DbType.Boolean, BizActionObj.OPD_IPD);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ResultDetails == null)
                        BizActionObj.ResultDetails = new clsConsentDetailsVO();
                    while (reader.Read())
                    {
                        BizActionObj.ResultDetails.ID = (long)DALHelper.HandleDBNull(reader["Id"]);
                        BizActionObj.ResultDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizActionObj.ResultDetails.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        BizActionObj.ResultDetails.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.ResultDetails.Consent = (string)DALHelper.HandleDBNull(reader["Consent"]);
                        BizActionObj.ResultDetails.PatientName = (string)DALHelper.HandleDBNull(reader["PatientName"]);
                        BizActionObj.ResultDetails.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        BizActionObj.ResultDetails.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        BizActionObj.ResultDetails.Age = (int)DALHelper.HandleDBNull(reader["Age"]);
                    }

                    reader.NextResult();
                }
                reader.Close();

            }

            catch
            {
                throw;
            }

            finally
            {
            }

            return BizActionObj;
        }



        //public override IValueObject SaveConsentDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsSaveConsentDetailsBizActionVO BizActionObj = (clsSaveConsentDetailsBizActionVO)valueObject;
        //    try
        //    {
        //        clsConsentDetailsVO objEmergencyVO = BizActionObj.ConsentDetails;

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddConsentDetails");

        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ConsentDetails.UnitID);
        //        dbServer.AddInParameter(command, "Date", DbType.Date, BizActionObj.ConsentDetails.Date);

        //        dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.ConsentDetails.VisitAdmID);
        //        dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.ConsentDetails.VisitAdmUnitID);
        //        dbServer.AddInParameter(command, "Opd_Ipd", DbType.Int32, BizActionObj.ConsentDetails.Opd_Ipd);

        //        dbServer.AddInParameter(command, "ConsentId", DbType.Int64, BizActionObj.ConsentDetails.ConsentID);
        //        dbServer.AddInParameter(command, "Consent", DbType.String, BizActionObj.ConsentDetails.Consent);

        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
        //        dbServer.AddOutParameter(command, "ConsentSummaryID", DbType.Int64, int.MaxValue);
        //        int intStatus = dbServer.ExecuteNonQuery(command);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        BizActionObj.ConsentDetails.ConsentSummaryID = Convert.ToInt64(dbServer.GetParameterValue(command, "ConsentSummaryID"));

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return BizActionObj;
        //}

        //public override IValueObject GetConsentByConsentType(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetConsentByConsentTypeBizActionVO BizActionObj = (clsGetConsentByConsentTypeBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetConsentByConsentTypeID");
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        //Check whether the reader contains the records
        //        if (reader.HasRows)
        //        {
        //            //if masterlist instance is null then creates new instance
        //            if (BizActionObj.MasterList == null)
        //            {
        //                BizActionObj.MasterList = new List<MasterListItem>();
        //            }
        //            //Reading the record from reader and stores in list
        //            while (reader.Read())
        //            {
        //                //Add the object value in list
        //                BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), reader["Template"].ToString()));//HandleDBNull(reader["Date"])));

        //            }
        //        }



        //        //DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetConsentByConsentTypeID");
        //        //dbServer.AddInParameter(command, "ConsentTypeID", DbType.Int64, BizActionObj.ConsentTypeID);
        //        //DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        //if (reader.HasRows)
        //        //{
        //        //    if (BizActionObj.ConsentList == null)
        //        //        BizActionObj.ConsentList = new List<clsConsentDetailsVO>();
        //        //    while (reader.Read())
        //        //    {
        //        //        clsConsentDetailsVO Consent = new clsConsentDetailsVO();
        //        //        Consent.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //        //        Consent.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //        //        Consent.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //        //        Consent.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //        //        //Consent.ConsentType = (long)DALHelper.HandleDBNull(reader["ConsentType"]);
        //        //        Consent.Consent = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
        //        //        BizActionObj.ConsentList.Add(Consent);
        //        //    }
        //        //    reader.NextResult();
        //        //}
        //        //reader.Close();

        //    }

        //    catch
        //    {
        //        throw;
        //    }

        //    finally
        //    {
        //    }

        //    return BizActionObj;
        //}

        //#endregion


        public override IValueObject GetEMRTemplateByProcID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProcDetailsByGetEMRTemplateBizactionVO bizActionVo = valueObject as clsGetProcDetailsByGetEMRTemplateBizactionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetProcBYScheduleID");
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleID);
                long ID = 1;
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId); //bizActionVo.ScheduleUnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    bizActionVo.PatientProcList = new List<clsPatientProcedureVO>();
                    while (reader.Read())
                    {
                        clsPatientProcedureVO patientProcObj = new clsPatientProcedureVO();
                        patientProcObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        patientProcObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        patientProcObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["procID"]));
                        patientProcObj.AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"]));
                        patientProcObj.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
                        patientProcObj.ProcedureUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureUnitID"]));
                        patientProcObj.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        bizActionVo.PatientProcList.Add(patientProcObj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return bizActionVo;

        }

    }
}
