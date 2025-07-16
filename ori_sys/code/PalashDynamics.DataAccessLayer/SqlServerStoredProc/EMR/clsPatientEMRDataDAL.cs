using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.EMR.EMR_Field_Values;
using System.IO;
using PalashDynamics.ValueObjects.CompoundDrug;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Drawing;
using System.Drawing.Imaging;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsPatientEMRDataDAL : clsBasePatientEMRDataDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;

        //added by neena
        string ImgIP = string.Empty;
        string ImgVirtualDir = string.Empty;
        string ImgSaveLocation = string.Empty;
        #endregion

        private clsPatientEMRDataDAL()
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

                //added by neena
                ImgIP = System.Configuration.ConfigurationManager.AppSettings["EMRImgIP"];
                ImgVirtualDir = System.Configuration.ConfigurationManager.AppSettings["EMRImgVirtualDir"];
                ImgSaveLocation = System.Configuration.ConfigurationManager.AppSettings["EMRImgSavingLocation"];
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject AddPatientEMRData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientEMRDataBizActionVO BizActionObj = valueObject as clsAddPatientEMRDataBizActionVO;
            try
            {
                clsPatientEMRDataVO objPatientEMRDataVO = BizActionObj.PatientEMRDataDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientEMRData");
                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientEMRDataVO.LinkServer);
                if (objPatientEMRDataVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientEMRDataVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientEMRDataVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPatientEMRDataVO.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objPatientEMRDataVO.TemplateID);
                dbServer.AddInParameter(command, "TemplateByNurse", DbType.String, objPatientEMRDataVO.TemplateByNurse);
                dbServer.AddInParameter(command, "TemplateByDoctor", DbType.String, objPatientEMRDataVO.TemplateByDoctor);
                dbServer.AddInParameter(command, "HistoryTemplate", DbType.String, objPatientEMRDataVO.HistoryTemplate);

                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objPatientEMRDataVO.VisitID);
                dbServer.AddInParameter(command, "SavedBy", DbType.String, objPatientEMRDataVO.SavedBy);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientEMRDataVO.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientEMRDataVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientEMRDataVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientEMRDataDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        //// To save Template Data FieldWise in Table-T_PatientEMRDetails
        // Author- Harish Kirnani , Date:22July2011
        public override IValueObject AddUpdatePatientEMRDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            DateTime dCurrDate = DateTime.Now.Date;
            clsAddUpdatePatientEMRDetailBizActionVO BizActionObj = valueObject as clsAddUpdatePatientEMRDetailBizActionVO;
            try
            {
                DbCommand command;
                if (BizActionObj.FalgForAddUpdate == 1)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientEMRDetail");
                    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.TempVariance.VisitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.TempVariance.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.TempVariance.PatientUnitID);
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TempVariance.TemplateID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.TempVariance.UnitId);
                    dbServer.AddInParameter(command, "EMRTemplateDataId", DbType.Int64, BizActionObj.TemplateDataId);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                List<clsPatientEMRDetailsVO> objPatEMRDetailsVO = BizActionObj.PatientEMRDataDetails;
                int count = objPatEMRDetailsVO.Count;
                for (int i = 0; i < count; i++)
                {
                    //  try
                    // {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientEMRDetails");
                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, BizActionObj.TempVariance.LinkServer);
                    if (BizActionObj.TempVariance.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, BizActionObj.TempVariance.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.TempVariance.VisitID);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.TempVariance.PatientID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.TempVariance.PatientUnitID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TempVariance.TemplateID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, BizActionObj.TempVariance.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, BizActionObj.TempVariance.Status);
                    dbServer.AddInParameter(command1, "ControlCaption", DbType.String, objPatEMRDetailsVO[i].ControlCaption);
                    dbServer.AddInParameter(command1, "ControlName", DbType.String, objPatEMRDetailsVO[i].ControlName);
                    dbServer.AddInParameter(command1, "ControlType", DbType.String, objPatEMRDetailsVO[i].ControlType);
                    dbServer.AddInParameter(command1, "ControlUnit", DbType.String, objPatEMRDetailsVO[i].ControlUnit);
                    dbServer.AddInParameter(command1, "CurrDate", DbType.DateTime, dCurrDate);
                    dbServer.AddInParameter(command1, "Value", DbType.String, objPatEMRDetailsVO[i].Value);
                    dbServer.AddInParameter(command1, "EMRTemplateDataId", DbType.Int64, BizActionObj.TemplateDataId);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatEMRDetailsVO[i].ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command1);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    BizActionObj.PatientEMRDataDetails[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw;
                    //}
                    //finally
                    //{
                    //}
                }


                if (BizActionObj.TempVariance.TemplateID == 41)
                {// Template 41 is Sperm Freezing, so insert the freezing data in thawing table also - Added by Saily P
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddSpremThawingFromVitrifiaction");

                    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, BizActionObj.TempVariance.UnitId);
                    dbServer.AddInParameter(command2, "IsDonor", DbType.Boolean, BizActionObj.IsDonor);

                    int intStatus = dbServer.ExecuteNonQuery(command2);

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
        public override IValueObject AddUpdatePatientEMRUploadedFiles(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientEMRUploadedFilesBizActionVO BizActionObj = valueObject as clsAddUpdatePatientEMRUploadedFilesBizActionVO;
            try
            {
                DbCommand command;
                if (BizActionObj.FalgForAddUpdate == 1)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientEMRUploadedFiles");
                    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.TempVariance.VisitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.TempVariance.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.TempVariance.PatientUnitID);
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TempVariance.TemplateID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.TempVariance.UnitId);

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                List<clsPatientEMRUploadedFilesVO> objPatEMRDetailsVO = BizActionObj.PatientEMRUploadedFiles;
                int count = objPatEMRDetailsVO.Count;
                for (int i = 0; i < count; i++)
                {
                    //try
                    //{
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientEMRUploadedFiles");
                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, BizActionObj.TempVariance.LinkServer);
                    if (BizActionObj.TempVariance.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, BizActionObj.TempVariance.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.TempVariance.VisitID);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.TempVariance.PatientID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.TempVariance.PatientUnitID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TempVariance.TemplateID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, BizActionObj.TempVariance.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, BizActionObj.TempVariance.Status);
                    dbServer.AddInParameter(command1, "IsIvfId", DbType.Int32, BizActionObj.IsivfID);
                    dbServer.AddInParameter(command1, "ControlCaption", DbType.String, objPatEMRDetailsVO[i].ControlCaption);
                    dbServer.AddInParameter(command1, "ControlName", DbType.String, objPatEMRDetailsVO[i].ControlName);
                    dbServer.AddInParameter(command1, "ControlType", DbType.String, objPatEMRDetailsVO[i].ControlType);
                    dbServer.AddInParameter(command1, "ControlIndex", DbType.Int32, objPatEMRDetailsVO[i].ControlIndex);
                    dbServer.AddInParameter(command1, "Value", DbType.Binary, objPatEMRDetailsVO[i].Value);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatEMRDetailsVO[i].ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command1);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    BizActionObj.PatientEMRUploadedFiles[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw;
                    //}
                    //finally
                    //{

                    //}
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

        public override IValueObject GetPatientEMRUploadedFiles(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientEMRUploadedFilesBizActionVO BizActionObj = valueObject as clsGetPatientEMRUploadedFilesBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRUploadFiles");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ControlName", DbType.String, BizActionObj.ControlName);
                dbServer.AddInParameter(command, "ControlIndex", DbType.Int32, BizActionObj.ControlIndex);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.objPatientEMRUploadedFiles == null)
                        {
                            BizActionObj.objPatientEMRUploadedFiles = new List<clsPatientEMRUploadedFilesVO>();
                        }
                        clsPatientEMRUploadedFilesVO objUF = new clsPatientEMRUploadedFilesVO();
                        objUF.ControlCaption = (string)DALHelper.HandleDBNull(reader["ControlCaption"]);
                        objUF.ControlName = (string)DALHelper.HandleDBNull(reader["ControlName"]);
                        objUF.ControlType = (string)DALHelper.HandleDBNull(reader["ControlType"]);
                        objUF.ControlIndex = (int)DALHelper.HandleDBNull(reader["ControlIndex"]);
                        objUF.Value = (byte[])DALHelper.HandleDBNull(reader["Value"]);
                        BizActionObj.objPatientEMRUploadedFiles.Add(objUF);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }

        public override IValueObject UpdatePatientEMRData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientEMRDataBizActionVO BizActionObj = valueObject as clsUpdatePatientEMRDataBizActionVO;
            try
            {
                clsPatientEMRDataVO objPatientEMRDataVO = BizActionObj.PatientEMRDataDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientEMRData");
                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientEMRDataVO.LinkServer);
                if (objPatientEMRDataVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientEMRDataVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientEMRDataVO.ID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientEMRDataVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPatientEMRDataVO.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objPatientEMRDataVO.TemplateID);
                dbServer.AddInParameter(command, "TemplateByNurse", DbType.String, objPatientEMRDataVO.TemplateByNurse);
                dbServer.AddInParameter(command, "TemplateByDoctor", DbType.String, objPatientEMRDataVO.TemplateByDoctor);
                dbServer.AddInParameter(command, "HistoryTemplate", DbType.String, objPatientEMRDataVO.HistoryTemplate);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objPatientEMRDataVO.VisitID);
                dbServer.AddInParameter(command, "SavedBy", DbType.String, objPatientEMRDataVO.SavedBy);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientEMRDataVO.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientEMRDataVO.Status);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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

        public override IValueObject GetPatientEMRData(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientEMRDataBizActionVO BizActionObj = valueObject as clsGetPatientEMRDataBizActionVO;
            try
            {
                DbCommand command;
                if (BizActionObj.IsPrevious == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPreviousVisitPatientEMRData");
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                }
                else if (BizActionObj.IsHistory == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientHistoryEMRData");
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRData");
                }
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.objPatientEMRData = new clsPatientEMRDataVO();

                        BizActionObj.objPatientEMRData.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.objPatientEMRData.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        BizActionObj.objPatientEMRData.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
                        BizActionObj.objPatientEMRData.TemplateByNurse = (string)DALHelper.HandleDBNull(reader["TemplateByNurse"]);
                        BizActionObj.objPatientEMRData.TemplateByDoctor = (string)DALHelper.HandleDBNull(reader["TemplateByDoctor"]);
                        BizActionObj.objPatientEMRData.HistoryTemplate = (string)DALHelper.HandleDBNull(reader["HistoryTemplate"]);
                        BizActionObj.objPatientEMRData.VisitID = (Int64)DALHelper.HandleDBNull(reader["VisitID"]);
                        BizActionObj.objPatientEMRData.SavedBy = (string)DALHelper.HandleDBNull(reader["SavedBy"]);
                        BizActionObj.objPatientEMRData.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.objPatientEMRData.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.objPatientEMRData.CreatedUnitID = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitID"]);
                        BizActionObj.objPatientEMRData.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.objPatientEMRData.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.objPatientEMRData.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        BizActionObj.objPatientEMRData.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        BizActionObj.objPatientEMRData.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.objPatientEMRData.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.objPatientEMRData.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        BizActionObj.objPatientEMRData.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                    }
                }
                else if (BizActionObj.IsPrevious == true)
                {
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }

        public override IValueObject GetPatientIVFEMR(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRSummaryDataBizActionVO BizActionObj = valueObject as clsGetPatientEMRSummaryDataBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetPatientIVFEMRData");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.SummaryList == null)
                            BizActionObj.SummaryList = new List<clsPatientEMRDataVO>();

                        clsPatientEMRDataVO objSummary = new clsPatientEMRDataVO();
                        objSummary.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objSummary.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        objSummary.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
                        objSummary.TemplateByNurse = (string)DALHelper.HandleDBNull(reader["TemplateByNurse"]);
                        objSummary.TemplateByDoctor = (string)DALHelper.HandleDBNull(reader["TemplateByDoctor"]);
                        objSummary.VisitID = (Int64)DALHelper.HandleDBNull(reader["VisitID"]);
                        objSummary.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        objSummary.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        objSummary.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        // objSummary.StaffName = (string)DALHelper.HandleDBNull(reader["StaffName"]);
                        // objSummary.DocName = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                        BizActionObj.SummaryList.Add(objSummary);
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

        public override IValueObject AddPatientFeedback(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientFeedbackBizActionVO BizActionObj = valueObject as clsAddPatientFeedbackBizActionVO;
            try
            {
                clsPatientFeedbackVO objPatientFeedbackVO = BizActionObj.PatientFeedbackDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientFeedback");


                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientFeedbackVO.LinkServer);
                if (objPatientFeedbackVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientFeedbackVO.LinkServer.Replace(@"\", "_"));


                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientFeedbackVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPatientFeedbackVO.PatientUnitID);
                dbServer.AddInParameter(command, "Feedback", DbType.String, objPatientFeedbackVO.Feedback);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objPatientFeedbackVO.VisitID);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientFeedbackVO.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientFeedbackVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientFeedbackVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientFeedbackDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject UpdatePatientFeedback(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientFeedbackBizActionVO BizActionObj = valueObject as clsUpdatePatientFeedbackBizActionVO;
            try
            {
                clsPatientFeedbackVO objPatientFeedbackVO = BizActionObj.PatientFeedbackDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientFeedback");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientFeedbackVO.LinkServer);
                if (objPatientFeedbackVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientFeedbackVO.LinkServer.Replace(@"\", "_"));


                dbServer.AddInParameter(command, "ID", DbType.Int64, objPatientFeedbackVO.ID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientFeedbackVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPatientFeedbackVO.PatientUnitID);
                dbServer.AddInParameter(command, "Feedback", DbType.String, objPatientFeedbackVO.Feedback);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objPatientFeedbackVO.VisitID);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientFeedbackVO.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientFeedbackVO.Status);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

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

        public override IValueObject GetPatientFeedback(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientFeedbackBizActionVO BizActionObj = valueObject as clsGetPatientFeedbackBizActionVO;
            try
            {
                DbCommand command;

                command = dbServer.GetStoredProcCommand("CIMS_GetPatientFeedback");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                //if (BizActionObj.IsPrevious == true)
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.objPatientFeedback = new clsPatientFeedbackVO();
                        BizActionObj.objPatientFeedback.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.objPatientFeedback.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        BizActionObj.objPatientFeedback.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        BizActionObj.objPatientFeedback.Feedback = (string)DALHelper.HandleDBNull(reader["Feedback"]);
                        BizActionObj.objPatientFeedback.VisitID = (Int64)DALHelper.HandleDBNull(reader["VisitID"]);

                        BizActionObj.objPatientFeedback.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);
                        BizActionObj.objPatientFeedback.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.objPatientFeedback.CreatedUnitID = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitID"]);

                        BizActionObj.objPatientFeedback.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.objPatientFeedback.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.objPatientFeedback.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        BizActionObj.objPatientFeedback.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        BizActionObj.objPatientFeedback.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.objPatientFeedback.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.objPatientFeedback.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        BizActionObj.objPatientFeedback.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;

        }

        // Added by Saily P

        public override IValueObject AddUpdateEMRFieldValue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateFieldValueMasterBizActionVO objItem = valueObject as clsAddUpdateFieldValueMasterBizActionVO;

            if (objItem.objFieldMatserDetails.ID == 0)
                objItem = AddEMRFieldValue(objItem, UserVo);
            else
                objItem = UpdateEMRFieldValue(objItem, UserVo);

            return objItem;
        }

        private clsAddUpdateFieldValueMasterBizActionVO AddEMRFieldValue(clsAddUpdateFieldValueMasterBizActionVO objItem, clsUserVO UserVo)
        {

            try
            {


                clsFieldValuesMasterVO objItemVO = new clsFieldValuesMasterVO();
                objItemVO = objItem.objFieldMatserDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddEMRValuesMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, 0);//objItemVO.ID);
                //  dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "UsedFor", DbType.String, objItemVO.UsedFor);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime,  objItemVO.AddedDateTime);
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String,objItemVO.AddedWindowsLoginName);

                // dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return objItem;
        }

        private clsAddUpdateFieldValueMasterBizActionVO UpdateEMRFieldValue(clsAddUpdateFieldValueMasterBizActionVO objItem, clsUserVO UserVo)
        {

            try
            {


                clsFieldValuesMasterVO objItemVO = new clsFieldValuesMasterVO();
                objItemVO = objItem.objFieldMatserDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateEMRValueMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "UsedFor", DbType.String, objItemVO.UsedFor);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                //dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return objItem;
        }

        public override IValueObject GetEMRFieldValue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFieldValueMasterBizActionVO BizActionObj = valueObject as clsGetFieldValueMasterBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetFeildValue_EMR");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objFieldMasterDetails == null)
                        BizActionObj.objFieldMasterDetails = new List<clsFieldValuesMasterVO>();
                    while (reader.Read())
                    {
                        clsFieldValuesMasterVO objItemVO = new clsFieldValuesMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        //    objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.UsedFor = Convert.ToString(DALHelper.HandleDBNull(reader["UsedFor"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.objFieldMasterDetails.Add(objItemVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");

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
            return BizActionObj;
        }

        public override IValueObject UpdateStatusEMRFieldValue(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateStatusFieldValueMasterBizActionVO bizObject = valueObject as clsUpdateStatusFieldValueMasterBizActionVO;
            try
            {
                clsFieldValuesMasterVO objVO = bizObject.FieldStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateEMRFieldStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        public override IValueObject GetUsedForValue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetUsedForMasterBizActionVO BizAction = new clsGetUsedForMasterBizActionVO();
            DbDataReader reader = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetUsedForValue_EMR");

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.objFieldMasterDetails == null)
                        BizAction.objFieldMasterDetails = new List<clsFieldValuesMasterVO>();
                    while (reader.Read())
                    {
                        clsFieldValuesMasterVO objItemVO = new clsFieldValuesMasterVO();
                        //    objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.UsedFor = Convert.ToString(DALHelper.HandleDBNull(reader["UsedFor"]));
                        BizAction.objFieldMasterDetails.Add(objItemVO);
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
        //GetPatientPrescriptionAndVisitDetails_IPD


        /// <summary>
        ///  Devidas 6/3/2017
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public override IValueObject GetPatientPrescriptionAndVisitDetails_IPD(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPrescriptionandVisitDetailsBizActionVO BizActionObj = valueObject as clsGetPatientPrescriptionandVisitDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionListForEMR_IPD");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                //dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                //dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                //dbServer.AddInParameter(command, "IsFromPresc", DbType.Boolean, BizActionObj.IsFromPresc);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientVisitEMRDetailsIPD == null)
                        BizActionObj.PatientVisitEMRDetailsIPD = new List<clsVisitEMRDetails>();
                    while (reader.Read())
                    {
                        clsVisitEMRDetails objVisitVO = new clsVisitEMRDetails();
                        //  objVisitVO.VisitId = (long)DALHelper.HandleIntegerNull(reader["VisitID"]);
                        // objVisitVO.PatientId = (long)DALHelper.HandleIntegerNull(reader["PatientId"]);
                        // objVisitVO.PatientUnitId = (long)DALHelper.HandleIntegerNull(reader["PatientUnitId"]);
                        // objVisitVO.UnitId = (Int64)DALHelper.HandleIntegerNull(reader["UnitId"]);
                        objVisitVO.PrescriptionID = (long)DALHelper.HandleIntegerNull(reader["PrescriptionID"]);
                        //  objVisitVO.DoctorCode = Convert.ToString(reader["DoctorID"]);
                        objVisitVO.VisitDate = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        // objVisitVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        //objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                        //  objVisitVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                        objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                        // objVisitVO.VisitCenter = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objVisitVO.Specialization = (string)DALHelper.HandleDBNull(reader["Specialization"]);//   -----Commented----------- 08062017
                        ////BizActionObj.PatientVisitEMRDetailsIPD.Add(objVisitVO);     -----Commented----------- 08062017
                        BizActionObj.PatientVisitEMRDetailsIPD.Add(objVisitVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                //WriteErrorLog("GetPatientPrescriptionAndVisitDetails", "CIMS_GetPatientPrescriptionListForEMR", 0, BizActionObj.PatientID, BizActionObj.PatientUnitID, 0, BizActionObj.UnitID, "Patient Prescription", ex.Message);
                throw ex;
            }

            return valueObject;

        }

        public override IValueObject GetPatientPrescriptionAndVisitDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPrescriptionandVisitDetailsBizActionVO BizActionObj = valueObject as clsGetPatientPrescriptionandVisitDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionListForEMR");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command, "IsFromPresc", DbType.Boolean, BizActionObj.IsFromPresc);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientVisitEMRDetails == null)
                        BizActionObj.PatientVisitEMRDetails = new List<clsVisitEMRDetails>();
                    while (reader.Read())
                    {
                        clsVisitEMRDetails objVisitVO = new clsVisitEMRDetails();
                        objVisitVO.VisitId = (long)DALHelper.HandleIntegerNull(reader["VisitID"]);
                        objVisitVO.PatientId = (long)DALHelper.HandleIntegerNull(reader["PatientId"]);
                        objVisitVO.PatientUnitId = (long)DALHelper.HandleIntegerNull(reader["PatientUnitId"]);
                        objVisitVO.UnitId = (Int64)DALHelper.HandleIntegerNull(reader["UnitId"]);
                        objVisitVO.PrescriptionID = (long)DALHelper.HandleIntegerNull(reader["PrescriptionID"]);
                        objVisitVO.DoctorCode = Convert.ToString(reader["DoctorID"]);
                        objVisitVO.VisitDate = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objVisitVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                        objVisitVO.VisitType = (string)DALHelper.HandleDBNull(reader["VisitType"]);
                        objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                        objVisitVO.VisitCenter = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        BizActionObj.PatientVisitEMRDetails.Add(objVisitVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                //WriteErrorLog("GetPatientPrescriptionAndVisitDetails", "CIMS_GetPatientPrescriptionListForEMR", 0, BizActionObj.PatientID, BizActionObj.PatientUnitID, 0, BizActionObj.UnitID, "Patient Prescription", ex.Message);
                throw ex;
            }

            return valueObject;

        }


        public override IValueObject GetPatientCurrentMedicationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCurrentMedicationDetailsBizActionVO BizActionObj = valueObject as clsGetPatientCurrentMedicationDetailsBizActionVO;
            try
            {
                DbDataReader reader;
                if (BizActionObj.IsPrevious == true)
                {
                    if (BizActionObj.IsFromPresc == true)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionByPrescriptionIDDetails");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                        dbServer.AddInParameter(command1, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                        dbServer.AddInParameter(command1, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                        dbServer.AddInParameter(command1, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                        dbServer.AddOutParameter(command1, "TotalRows", DbType.Int32, int.MaxValue);

                        reader = (DbDataReader)dbServer.ExecuteReader(command1);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PatientCurrentMedicationDetailList == null)
                                BizActionObj.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                            while (reader.Read())
                            {
                                clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                                objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                                //objPrescriptionVO.SelectedItem.Code = Convert.ToString(reader["DrugCode"]);
                                objPrescriptionVO.SelectedItem.ID = (long)(DALHelper.HandleDBNull(reader["DrugID"]));
                                objPrescriptionVO.SelectedItem.Description = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["DrugID"]));
                                //objPrescriptionVO.DrugCode = Convert.ToString(reader["DrugCode"]);
                                objPrescriptionVO.SelectedFrequency.Description = (string)(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.SelectedInstruction.Description = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                                objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                                objPrescriptionVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                                objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
                                objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                                objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                                objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                                objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                                objPrescriptionVO.FromHistory = false;
                                objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                                objPrescriptionVO.Instruction = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                                BizActionObj.PatientCurrentMedicationDetailList.Add(objPrescriptionVO);
                            }
                        }
                    }
                    else
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentMedicationByPrescriptionIDDetais");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                        dbServer.AddInParameter(command1, "IsFromPresc", DbType.Boolean, BizActionObj.IsFromPresc);
                        dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                        dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                        reader = (DbDataReader)dbServer.ExecuteReader(command1);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PatientCurrentMedicationDetailList == null)
                                BizActionObj.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                            while (reader.Read())
                            {
                                clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                                objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                                objPrescriptionVO.SelectedItem.ID = (long)(DALHelper.HandleDBNull(reader["DrugID"]));
                                objPrescriptionVO.SelectedItem.Description = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["DrugID"]));
                                objPrescriptionVO.SelectedFrequency.Description = (string)(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.SelectedInstruction.Description = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                                objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                                objPrescriptionVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                                objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
                                objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                                objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                                objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                                objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                                objPrescriptionVO.FromHistory = false;
                                objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                                objPrescriptionVO.Instruction = (string)(DALHelper.HandleDBNull(reader["Reason"]));

                                objPrescriptionVO.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                                objPrescriptionVO.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOMID"]));
                                BizActionObj.PatientCurrentMedicationDetailList.Add(objPrescriptionVO);
                            }
                        }
                    }
                }
                else
                {
                    if (BizActionObj.IsFromPresc == true)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionDetailsForCPOE");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                        reader = (DbDataReader)dbServer.ExecuteReader(command1);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PatientCurrentMedicationDetailList == null)
                                BizActionObj.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                            while (reader.Read())
                            {
                                clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                                objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                                objPrescriptionVO.SelectedItem.ID = (long)(DALHelper.HandleDBNull(reader["DrugID"]));
                                objPrescriptionVO.SelectedItem.Description = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["DrugID"]));
                                objPrescriptionVO.SelectedFrequency.Description = (string)(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.SelectedInstruction.Description = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                                objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                                objPrescriptionVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                                objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
                                objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                                objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                                objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                                objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                                objPrescriptionVO.FromHistory = false;
                                objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                                objPrescriptionVO.Instruction = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                                BizActionObj.PatientCurrentMedicationDetailList.Add(objPrescriptionVO);
                            }
                        }
                    }
                    else
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentMedicationDetais");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                        dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                        reader = (DbDataReader)dbServer.ExecuteReader(command1);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PatientCurrentMedicationDetailList == null)
                                BizActionObj.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                            while (reader.Read())
                            {
                                clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                                objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                                objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                                objPrescriptionVO.SelectedItem.ID = (long)(DALHelper.HandleDBNull(reader["DrugID"]));
                                objPrescriptionVO.SelectedItem.Description = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                                objPrescriptionVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["DrugID"]));
                                objPrescriptionVO.SelectedFrequency.Description = (string)(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.SelectedInstruction.Description = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                                objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                                objPrescriptionVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                                objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
                                objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                                objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                                objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                                objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                                objPrescriptionVO.FromHistory = false;
                                objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                                objPrescriptionVO.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                                objPrescriptionVO.Instruction = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                                BizActionObj.PatientCurrentMedicationDetailList.Add(objPrescriptionVO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // WriteErrorLog("GetPatientCurrentMedicationDetails", "CIMS_GetPatientPrescriptionByPrescriptionIDDetails + CIMS_GetPatientCurrentMedicationByPrescriptionIDDetais + CIMS_GetPatientPrescriptionDetailsForCPOE + CIMS_GetPatientCurrentMedicationDetais", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "Get Current Detail", ex.Message);
                throw ex;
            }

            return valueObject;
        }

        public override IValueObject GetPatientPastMedicationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastMedicationDetailsBizActionVO BizActionObj = valueObject as clsGetPatientPastMedicationDetailsBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = null;
                if (BizActionObj.IsForCompound == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientCompoundMedicationDetails");
                }
                else
                {
                    if (BizActionObj.IsFromPresc == false)
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_GetPatientPastMedicationDetails");
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    else
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_GetPatientPastPrescriptionDetails");
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    }
                }
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "DoctorId", DbType.String, BizActionObj.DoctorId);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "@startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "@maximumRows", DbType.Int64, BizActionObj.MaximumRows);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientMedicationDetailList == null)
                        BizActionObj.PatientMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();

                    while (reader.Read())
                    {
                        clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                        objPrescriptionVO.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
                        objPrescriptionVO.SelectedItem.Description = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objPrescriptionVO.DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));

                        objPrescriptionVO.SelectedFrequency.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                        objPrescriptionVO.SelectedInstruction.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                        objPrescriptionVO.SelectedRoute.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));

                        if (BizActionObj.IsFromPresc == true)
                        {
                            objPrescriptionVO.Instruction = Convert.ToString(reader["Instruction"]);
                            //added by neena
                            objPrescriptionVO.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                            objPrescriptionVO.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                            objPrescriptionVO.ArtEnabled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["ARTEnables"]));
                            objPrescriptionVO.DrugSource = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugSourceId"]));
                            objPrescriptionVO.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
                            objPrescriptionVO.PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]));
                            //

                        }
                        objPrescriptionVO.Frequency = Convert.ToString(reader["Frequency"]);
                        objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                        if (BizActionObj.IsFromPresc == false)
                        {
                            objPrescriptionVO.Year = Convert.ToString(reader["year"]);
                        }
                        if (BizActionObj.IsFromPresc && BizActionObj.IsOPDIPD)
                        {
                            objPrescriptionVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            objPrescriptionVO.DoctorSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecilization"]));
                        }
                        //if (BizActionObj.IsFromPresc && !BizActionObj.IsForCompound)
                        //{
                        //    objPrescriptionVO.DrugCode = Convert.ToString(DALHelper.HandleDBNull(reader["DrugCode"]));
                        //}
                        if (BizActionObj.IsForCompound)
                        {
                            objPrescriptionVO.sComponentQuantity = Convert.ToString(reader["Quantity"]);
                            objPrescriptionVO.sCompoundQuantity = Convert.ToString(reader["CompdQuantity"]);
                            objPrescriptionVO.CompoundDrug = Convert.ToString(reader["CompoundDrug"]);
                        }
                        else
                        {
                            objPrescriptionVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                            objPrescriptionVO.PastPrescriptionQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        }

                        objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                        objPrescriptionVO.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));



                        BizActionObj.PatientMedicationDetailList.Add(objPrescriptionVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }
            return BizActionObj;
        }

        //public override IValueObject GetPatientCurrentMedicationDetailList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetPatientCurrentMedicationDetailListBizActionVO BizAction = valueObject as clsGetPatientCurrentMedicationDetailListBizActionVO;
        //    DbDataReader reader;
        //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentMedicationDetailList");
        //    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizAction.VisitID);
        //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizAction.PatientID);
        //    dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
        //    reader = (DbDataReader)dbServer.ExecuteReader(command1);
        //    if (reader.HasRows)
        //    {
        //        if (BizAction.PatientCurrentMedicationDetailList == null)
        //            BizAction.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
        //        while (reader.Read())
        //        {
        //            clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
        //            objPrescriptionVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //            objPrescriptionVO.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
        //            objPrescriptionVO.SelectedItem.Code = Convert.ToString(reader["DrugCode"]);
        //            objPrescriptionVO.SelectedItem.Description = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //            objPrescriptionVO.DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //            objPrescriptionVO.DrugCode = Convert.ToString(reader["DrugCode"]);
        //            objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
        //            objPrescriptionVO.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
        //            objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
        //            objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
        //            BizAction.PatientCurrentMedicationDetailList.Add(objPrescriptionVO);
        //        }
        //    }
        //    return BizAction;
        //}

        //public override IValueObject AddUpdatePatientCurrentMedicationDetail(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddUpdatePatientCurrentMedicationDetailBizActionVO BizActionObj = valueObject as clsAddUpdatePatientCurrentMedicationDetailBizActionVO;
        //    DbTransaction trans = null;
        //    DbConnection con = null;
        //    try
        //    {
        //        long PrescriptionID = 0;
        //        con = dbServer.CreateConnection();
        //        if (con.State != ConnectionState.Open) con.Open();
        //        trans = con.BeginTransaction();

        //        if (BizActionObj.PatientCurrentMedicationDetail != null)
        //        {
        //            DbDataReader reader;
        //            DbCommand mygetcommand = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionID");
        //            dbServer.AddInParameter(mygetcommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
        //            dbServer.AddInParameter(mygetcommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
        //            dbServer.AddInParameter(mygetcommand, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
        //            //mygetcommand.Connection = con;
        //            //mygetcommand.Transaction = trans;
        //            //PrescriptionID =(long)mygetcommand.ExecuteScalar();
        //            reader = (DbDataReader)dbServer.ExecuteReader(mygetcommand, trans);
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    PrescriptionID = (long)DALHelper.HandleDBNull(reader["ID"]);
        //                }
        //                reader.Close();
        //            }
        //            reader.Dispose();
        //            mygetcommand.Dispose();

        //            if (PrescriptionID == 0)
        //            {
        //                DbCommand prescriptionCommand = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionFromEMR");

        //                dbServer.AddInParameter(prescriptionCommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
        //                dbServer.AddInParameter(prescriptionCommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
        //                //dbServer.AddInParameter(prescriptionCommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
        //                dbServer.AddInParameter(prescriptionCommand, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
        //                dbServer.AddInParameter(prescriptionCommand, "Status", DbType.Boolean, true);
        //                //dbServer.AddInParameter(prescriptionCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(prescriptionCommand, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(prescriptionCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                //dbServer.AddInParameter(prescriptionCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
        //                dbServer.AddInParameter(prescriptionCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //                dbServer.AddParameter(prescriptionCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, PrescriptionID);
        //                dbServer.AddOutParameter(prescriptionCommand, "ResultStatus", DbType.Int32, int.MaxValue);

        //                int intStatus1 = dbServer.ExecuteNonQuery(prescriptionCommand, trans);
        //                int mSuccessStatus = (int)dbServer.GetParameterValue(prescriptionCommand, "ResultStatus");
        //                PrescriptionID = (long)dbServer.GetParameterValue(prescriptionCommand, "ID");
        //                prescriptionCommand.Dispose();
        //            }

        //            DbCommand mycommand = dbServer.GetStoredProcCommand("CIMS_DeletePatientCurrentMedication");
        //            dbServer.AddInParameter(mycommand, "PrescriptionID", DbType.Int64, PrescriptionID);
        //            int intStatus2 = dbServer.ExecuteNonQuery(mycommand, trans);
        //            mycommand.Dispose();

        //            foreach (clsPatientPrescriptionDetailVO objCurMedVo in BizActionObj.PatientCurrentMedicationDetail)
        //            {
        //                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDetailCurrentMedication");

        //                dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, PrescriptionID);
        //                dbServer.AddInParameter(command1, "DrugCode", DbType.String, objCurMedVo.DrugCode);

        //                dbServer.AddInParameter(command1, "Instruction", DbType.String, objCurMedVo.Instruction);
        //                dbServer.AddInParameter(command1, "Frequency", DbType.String, objCurMedVo.Frequency);

        //                dbServer.AddInParameter(command1, "ItemName", DbType.String, objCurMedVo.DrugName);
        //                dbServer.AddInParameter(command1, "Quantity", DbType.Double, objCurMedVo.Quantity);
        //                dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);

        //                dbServer.AddInParameter(command1, "FromHistory", DbType.Boolean, objCurMedVo.FromHistory);
        //                dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objCurMedVo.IsOther);
        //                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
        //                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

        //                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
        //                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
        //            }
        //        }
        //        BizActionObj.SuccessStatus = 1;
        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        BizActionObj.SuccessStatus = -1;
        //        trans.Rollback();
        //      //  WriteErrorLog("AddUpdatePatientCurrentMedicationDetail", "CIMS_GetPatientPrescriptionID + CIMS_AddPatientPrescriptionFromEMR + CIMS_DeletePatientCurrentMedication + CIMS_DeletePatientCurrentMedication", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "Current Medication", ex.Message);
        //        throw ex;

        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed) con.Close();
        //        con.Dispose();
        //        trans.Dispose();
        //    }
        //    return valueObject;
        //}
        public override IValueObject GetPatientCurrentMedicationDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCurrentMedicationDetailListBizActionVO BizAction = valueObject as clsGetPatientCurrentMedicationDetailListBizActionVO;
            DbDataReader reader;
            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentMedicationDetailList");
            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
            dbServer.AddInParameter(command1, "ISOPDIPD", DbType.Int64, BizAction.IsOPDIPD);
            reader = (DbDataReader)dbServer.ExecuteReader(command1);
            if (reader.HasRows)
            {
                if (BizAction.PatientCurrentMedicationDetailList == null)
                    BizAction.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                while (reader.Read())
                {
                    clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                    objPrescriptionVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    objPrescriptionVO.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
                    objPrescriptionVO.SelectedItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugID"]));
                    objPrescriptionVO.SelectedItem.Description = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                    objPrescriptionVO.DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                    objPrescriptionVO.DrugID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DrugID"]));
                    objPrescriptionVO.SelectedFrequency.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                    objPrescriptionVO.SelectedInstruction.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                    objPrescriptionVO.Dose = Convert.ToString(DALHelper.HandleDBNull(reader["Dose"]));
                    objPrescriptionVO.SelectedRoute.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                    objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
                    objPrescriptionVO.Reason = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                    objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                    objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                    objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                    objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                    objPrescriptionVO.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                    objPrescriptionVO.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                    objPrescriptionVO.Year = Convert.ToString(DALHelper.HandleDBNull(reader["Pyear"]));
                    BizAction.PatientCurrentMedicationDetailList.Add(objPrescriptionVO);
                }
            }

            return BizAction;
        }
        public override IValueObject AddUpdatePatientCurrentMedicationDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientCurrentMedicationDetailBizActionVO BizActionObj = valueObject as clsAddUpdatePatientCurrentMedicationDetailBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                long PrescriptionID = 0;
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                if (BizActionObj.PatientCurrentMedicationDetail != null)
                {
                    DbDataReader reader;
                    DbCommand mygetcommand = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionID");
                    dbServer.AddInParameter(mygetcommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(mygetcommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(mygetcommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(mygetcommand, "DoctorId", DbType.Int64, BizActionObj.DoctorID);
                    dbServer.AddInParameter(mygetcommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(mygetcommand, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    dbServer.AddInParameter(mygetcommand, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                    reader = (DbDataReader)dbServer.ExecuteReader(mygetcommand, trans);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PrescriptionID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        }
                        reader.Close();
                    }
                    reader.Dispose();
                    mygetcommand.Dispose();

                    if (PrescriptionID == 0)
                    {
                        DbCommand prescriptionCommand = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionFromEMR");
                        dbServer.AddInParameter(prescriptionCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(prescriptionCommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                        dbServer.AddInParameter(prescriptionCommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                        dbServer.AddInParameter(prescriptionCommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                        dbServer.AddInParameter(prescriptionCommand, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                        dbServer.AddInParameter(prescriptionCommand, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                        dbServer.AddInParameter(prescriptionCommand, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                        dbServer.AddInParameter(prescriptionCommand, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(prescriptionCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(prescriptionCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(prescriptionCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(prescriptionCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(prescriptionCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(prescriptionCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, PrescriptionID);
                        dbServer.AddOutParameter(prescriptionCommand, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(prescriptionCommand, trans);
                        int mSuccessStatus = (int)dbServer.GetParameterValue(prescriptionCommand, "ResultStatus");
                        PrescriptionID = (long)dbServer.GetParameterValue(prescriptionCommand, "ID");
                        prescriptionCommand.Dispose();
                    }
                    DbCommand mycommand = dbServer.GetStoredProcCommand("CIMS_DeletePatientCurrentMedication");
                    dbServer.AddInParameter(mycommand, "PrescriptionID", DbType.Int64, PrescriptionID);
                    dbServer.AddInParameter(mycommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(mycommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(mycommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(mycommand, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);

                    int intStatus2 = dbServer.ExecuteNonQuery(mycommand, trans);
                    mycommand.Dispose();
                    foreach (clsPatientPrescriptionDetailVO objCurMedVo in BizActionObj.PatientCurrentMedicationDetail)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDetailCurrentMedication");

                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, PrescriptionID);
                        dbServer.AddInParameter(command1, "DrugID", DbType.Int64, objCurMedVo.DrugID);

                        dbServer.AddInParameter(command1, "Dose", DbType.String, objCurMedVo.Dose);

                        if (objCurMedVo.SelectedRoute != null && objCurMedVo.SelectedRoute.Description != "--Select--")
                            dbServer.AddInParameter(command1, "Route", DbType.String, objCurMedVo.SelectedRoute.Description);
                        else
                            dbServer.AddInParameter(command1, "Route", DbType.String, null);

                        if (objCurMedVo.SelectedInstruction != null && objCurMedVo.SelectedInstruction.Description != "--Select--")
                            dbServer.AddInParameter(command1, "Instruction", DbType.String, objCurMedVo.SelectedInstruction.Description);
                        else
                            dbServer.AddInParameter(command1, "Instruction", DbType.String, null);

                        if (objCurMedVo.SelectedFrequency.Description != "--Select--")
                            dbServer.AddInParameter(command1, "Frequency", DbType.String, objCurMedVo.SelectedFrequency.Description);
                        else
                            dbServer.AddInParameter(command1, "Frequency", DbType.String, null);

                        // dbServer.AddInParameter(command1, "Frequency", DbType.String, objCurMedVo.Frequency);
                        //dbServer.AddInParameter(command1, "Instruction", DbType.String, objCurMedVo.Instruction);
                        dbServer.AddInParameter(command1, "ItemName", DbType.String, objCurMedVo.DrugName);
                        dbServer.AddInParameter(command1, "Days", DbType.Int64, objCurMedVo.Days);
                        dbServer.AddInParameter(command1, "year", DbType.String, objCurMedVo.Year);
                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, objCurMedVo.Quantity);
                        dbServer.AddInParameter(command1, "Rate", DbType.Int64, objCurMedVo.Rate);
                        dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                        dbServer.AddInParameter(command1, "FromHistory", DbType.Boolean, objCurMedVo.FromHistory);
                        dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objCurMedVo.IsOther);
                        dbServer.AddInParameter(command1, "Reason", DbType.String, objCurMedVo.Reason);
                        dbServer.AddInParameter(command1, "Timing", DbType.String, null);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    }
                }
                BizActionObj.SuccessStatus = 1;
                trans.Commit();
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                // WriteErrorLog("AddUpdatePatientCurrentMedicationDetail", "CIMS_GetPatientPrescriptionID + CIMS_AddPatientPrescriptionFromEMR + CIMS_DeletePatientCurrentMedication + CIMS_DeletePatientCurrentMedication", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "Current Medication", ex.Message);
                throw ex;

            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return valueObject;
        }
        public override IValueObject GetPatientEMRDataDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRDetailsBizActionVO BizActionObj = valueObject as clsGetPatientEMRDetailsBizActionVO;
            try
            {
                //bool isBPControl = false;
                //bool isVisionControl = false;
                //bool isGPControl = false;
                DbDataReader reader;
                //DbDataReader reader1;
                //DbDataReader reader2;
                //DbDataReader reader3;

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRDetails");
                dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command1, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
                //dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command1, "Tab", DbType.String, BizActionObj.Tab);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                reader = (DbDataReader)dbServer.ExecuteReader(command1);
                if (reader.HasRows)
                {
                    if (BizActionObj.EMRDetailsList == null)
                        BizActionObj.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                        EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                        EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        EmrDetails.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        //EmrDetails.Value1 = (byte[])DALHelper.HandleDBNull(reader["Value"]);
                        EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
                        BizActionObj.EMRDetailsList.Add(EmrDetails);
                    }
                    reader.NextResult();
                    if (BizActionObj.EMRImgList == null)
                        BizActionObj.EMRImgList = new List<clsPatientEMRDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                        EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                        EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
                        EmrDetails.Value1 = (byte[])DALHelper.HandleDBNull(reader["Value"]);
                        BizActionObj.EMRImgList.Add(EmrDetails);
                    }
                }
                reader.Close();
                command1.Dispose();
            }
            catch (Exception ex)
            {
                // WriteErrorLog("GetPatientEMRDataDetails", "CIMS_GetPatientEMRDetails", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, BizActionObj.Tab, ex.Message);
                throw ex;
            }
            return BizActionObj;
        }
        public override IValueObject GetPatientEMRPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRPhysicalExamDetailsBizActionVO BizActionObj = valueObject as clsGetPatientEMRPhysicalExamDetailsBizActionVO;
            try
            {
                DbDataReader reader;
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRPhysicalExamDetails");
                dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command1, "Doctorid", DbType.Int64, BizActionObj.DoctorId);
                dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command1, "Tab", DbType.String, BizActionObj.Tab);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command1, "ISFromOTDetails", DbType.Boolean, BizActionObj.ISFromOTDetails);
                reader = (DbDataReader)dbServer.ExecuteReader(command1);
                if (reader.HasRows)
                {
                    if (BizActionObj.EMRDetailsList == null)
                        BizActionObj.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                        EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                        EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        EmrDetails.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
                        BizActionObj.EMRDetailsList.Add(EmrDetails);
                    }
                    //reader.NextResult();
                    //if (BizActionObj.EMRImgList == null)
                    //    BizActionObj.EMRImgList = new List<clsPatientEMRDetailsVO>();
                    //while (reader.Read())
                    //{
                    //    clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                    //    EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                    //    EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                    //    EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
                    //    EmrDetails.Value1 = (byte[])DALHelper.HandleDBNull(reader["Value"]);
                    //    BizActionObj.EMRImgList.Add(EmrDetails);
                    //}
                }
                reader.Close();
                command1.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPhysicalExamDetailsBizActionVO BizActionObj = valueObject as clsGetPatientPhysicalExamDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPhysicalExamDetails");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.TemplateByNurse = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateByNurse"]));
                        BizActionObj.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        BizActionObj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        BizActionObj.IsUpdated = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUpdated"]));
                    }
                    reader.NextResult();
                    BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
                //  WriteErrorLog("GetPatientPhysicalExamDetails", "CIMS_GetPatientPhysicalExamDetails", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "Get Consultation", ex.Message);
                throw ex;

            }
            return valueObject;

        }
        public override IValueObject AddUpdateCompoundMedicationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateCompoundPrescriptionBizActionVO BizActionObj = valueObject as clsAddUpdateCompoundPrescriptionBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            long PrescriptionID = 0;
            con = dbServer.CreateConnection();
            if (con.State != ConnectionState.Open) con.Open();
            trans = con.BeginTransaction();
            try
            {
                if (BizActionObj.CoumpoundDrugList != null && BizActionObj.CoumpoundDrugList.Count > 0)
                {
                    DbCommand mycommand1 = dbServer.GetStoredProcCommand("CIMS_DeletePatientCompoundPrescription");//CIMS_DeletePatientCompoundPrescriptionDetailFromCPOE
                    dbServer.AddInParameter(mycommand1, "PatientID", DbType.String, BizActionObj.PatientID);
                    dbServer.AddInParameter(mycommand1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(mycommand1, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
                    dbServer.AddInParameter(mycommand1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(mycommand1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(mycommand1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(mycommand1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(mycommand1, "PrescriptionID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);

                    int intStatus2 = dbServer.ExecuteNonQuery(mycommand1, trans);
                    PrescriptionID = (long)dbServer.GetParameterValue(mycommand1, "PrescriptionID");
                    mycommand1.Dispose();

                    foreach (var item2 in BizActionObj.CompoundDrugMaster)
                    {
                        clsCompoundDrugMasterVO objCompoundDrug = item2;
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCompoundDrug");
                        dbServer.AddInParameter(command1, "Code", DbType.String, objCompoundDrug.Code);
                        dbServer.AddInParameter(command1, "Description", DbType.String, objCompoundDrug.Description);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objCompoundDrug.Status);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCompoundDrug.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                        objCompoundDrug.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                        foreach (var item in BizActionObj.CoumpoundDrugList)
                        {
                            if (item.CompoundDrug == objCompoundDrug.Description)
                            {
                                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientCompoundPrescription");
                                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, PrescriptionID);

                                if (item.CompoundDrugID > 0)
                                {
                                    dbServer.AddInParameter(command, "CompoundDrugID", DbType.Int64, item.CompoundDrugID);
                                }
                                else
                                {
                                    dbServer.AddInParameter(command, "CompoundDrugID", DbType.Int64, objCompoundDrug.ID);
                                }

                                dbServer.AddInParameter(command, "ItemCode", DbType.String, item.ItemCode);
                                dbServer.AddInParameter(command, "ItemName", DbType.String, item.ItemName);
                                dbServer.AddInParameter(command, "ComponentQuantity", DbType.String, item.sComponentQuantity);
                                dbServer.AddInParameter(command, "CompoundQuantity", DbType.String, item.sCompoundQuantity);
                                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                                //dbServer.AddInParameter(command, "Dose", DbType.String, item.Dose);
                                //if (item.SelectedRoute != null && item.SelectedRoute.Description != "--Select--")
                                //    dbServer.AddInParameter(command, "Route", DbType.String, item.SelectedRoute.Description);
                                //else
                                //    dbServer.AddInParameter(command, "Route", DbType.String, null);

                                //if (item.SelectedInstruction != null && item.SelectedInstruction.Description != "--Select--")
                                dbServer.AddInParameter(command, "Instruction", DbType.String, item.Instruction);

                                //if (item.SelectedFrequency.Description != "--Select--")
                                dbServer.AddInParameter(command, "Frequency", DbType.String, item.Frequency);
                                //else
                                //    dbServer.AddInParameter(command, "Frequency", DbType.String, null);
                                //dbServer.AddInParameter(command, "Days", DbType.Int64, item.Days);
                                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                                int intStatus3 = dbServer.ExecuteNonQuery(command, trans);
                                if (intStatus3 < 1)
                                {
                                    InvalidOperationException ex = new InvalidOperationException();
                                    throw ex;
                                }

                                if (BizActionObj.SuccessStatus == 1)
                                {
                                    DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_AddCompoundDrugItems");
                                    dbServer.AddInParameter(command12, "CompoundDrugId", DbType.Int64, objCompoundDrug.ID);
                                    dbServer.AddInParameter(command12, "ItemCode", DbType.String, item.ItemCode);
                                    dbServer.AddInParameter(command12, "ItemName", DbType.String, item.ItemName);
                                    dbServer.AddInParameter(command12, "Quantity", DbType.String, item.sComponentQuantity);
                                    dbServer.AddParameter(command12, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                                    dbServer.AddOutParameter(command12, "ResultStatus", DbType.Int32, int.MaxValue);
                                    int intStatus4 = dbServer.ExecuteNonQuery(command12, trans);
                                }
                            }
                        }
                    }
                    trans.Commit();

                }
            }

            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                // WriteErrorLog("CIMS_AddCompoundDrug", "CIMS_AddCompoundDrugItems", BizActionObj.VisitID, BizActionObj.PatientID, 1, 0, 1, "Compound Medication", ex.Message);
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return BizActionObj;
        }
        public override IValueObject GetPatientCompoundPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompoundPrescriptionBizActionVO BizAction = valueObject as clsGetCompoundPrescriptionBizActionVO;
            DbDataReader reader;
            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientCompoundPrescription");
            dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(command1, "DoctorCode", DbType.String, BizAction.DoctorCode);
            dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
            reader = (DbDataReader)dbServer.ExecuteReader(command1);
            if (reader.HasRows)
            {
                if (BizAction.CoumpoundDrugList == null)
                    BizAction.CoumpoundDrugList = new List<clsPatientPrescriptionDetailVO>();
                while (reader.Read())
                {
                    clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                    objPrescriptionVO.DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                    objPrescriptionVO.DrugCode = Convert.ToString(reader["ItemCode"]);
                    //objPrescriptionVO.SelectedFrequency.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                    //objPrescriptionVO.Dose = Convert.ToString(DALHelper.HandleDBNull(reader["Dose"]));
                    //objPrescriptionVO.SelectedRoute.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                    objPrescriptionVO.Frequency = Convert.ToString(reader["Frequency"]);
                    objPrescriptionVO.sCompoundQuantity = Convert.ToString(reader["CompoundQty"]);
                    objPrescriptionVO.sComponentQuantity = Convert.ToString(reader["ComponentQty"]);
                    objPrescriptionVO.CompoundDrugID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CompoundDrugID"]));
                    objPrescriptionVO.CompoundDrug = Convert.ToString(reader["CompoundDrug"]);
                    objPrescriptionVO.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                    //objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                    objPrescriptionVO.AvailableStock = Convert.ToDouble(reader["AvailableStock"]);
                    BizAction.CoumpoundDrugList.Add(objPrescriptionVO);
                }
            }

            return BizAction;
        }
        public override IValueObject AddUpdatePatientCPOEDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientCPOEDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatePatientCPOEDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                long PrescriptionID = 0;
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();

                DbCommand prescriptionCommand = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientPrescriptionFromCPOE");

                dbServer.AddInParameter(prescriptionCommand, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(prescriptionCommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(prescriptionCommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(prescriptionCommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(prescriptionCommand, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(prescriptionCommand, "DoctorID", DbType.Int64, BizActionObj.DoctorID);

                dbServer.AddInParameter(prescriptionCommand, "Advice", DbType.String, BizActionObj.Advice);
                dbServer.AddInParameter(prescriptionCommand, "FollowUpDate", DbType.DateTime, BizActionObj.FollowupDate);
                dbServer.AddInParameter(prescriptionCommand, "FollowUpRemark", DbType.String, BizActionObj.FollowUpRemark);

                dbServer.AddInParameter(prescriptionCommand, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(prescriptionCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(prescriptionCommand, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(prescriptionCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(prescriptionCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(prescriptionCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(prescriptionCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, PrescriptionID);
                dbServer.AddOutParameter(prescriptionCommand, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus1 = dbServer.ExecuteNonQuery(prescriptionCommand, trans);
                int mSuccessStatus = (int)dbServer.GetParameterValue(prescriptionCommand, "ResultStatus");
                PrescriptionID = (long)dbServer.GetParameterValue(prescriptionCommand, "ID");
                prescriptionCommand.Dispose();

                if (BizActionObj.FollowupDate != null)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientFollowUpFromCPOE");

                    //dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "FollowUpNo", DbType.Int32, 0);
                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                    dbServer.AddInParameter(command, "ServiceId", DbType.Int64, 0);
                    dbServer.AddInParameter(command, "FollowUpDate", DbType.DateTime, BizActionObj.FollowupDate);
                    dbServer.AddInParameter(command, "FollowUpTime", DbType.DateTime, BizActionObj.FollowupDate);
                    dbServer.AddInParameter(command, "FollowUpFor", DbType.String, "");
                    dbServer.AddInParameter(command, "FollowUpRemarks", DbType.String, BizActionObj.FollowUpRemark);
                    dbServer.AddInParameter(command, "FollowUpFrom", DbType.String, FollowUpFrom.EMR.ToString());
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "TariffID", DbType.Int64, 0);
                    dbServer.AddInParameter(command, "PackageServiceID", DbType.Int64, 0);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.FollowUpID);

                    dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    command.Dispose();
                }
                else
                {

                    /////////////////////////////////////////////////////////////
                    DbCommand followUpCommand1 = dbServer.GetStoredProcCommand("CIMS_DeletePatientFollowUpFromCPOE");
                    dbServer.AddInParameter(followUpCommand1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(followUpCommand1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(followUpCommand1, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                    dbServer.AddInParameter(followUpCommand1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    int intfollowUpstatus = dbServer.ExecuteNonQuery(followUpCommand1, trans);
                    followUpCommand1.Dispose();
                }

                /////////////////////////////////////////////////////////////
                DbCommand mycommand1 = dbServer.GetStoredProcCommand("CIMS_DeletePatientPrescriptionDetailFromCPOE");
                dbServer.AddInParameter(mycommand1, "PrescriptionID", DbType.Int64, PrescriptionID);
                dbServer.AddInParameter(mycommand1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                int intStatus0 = dbServer.ExecuteNonQuery(mycommand1, trans);
                mycommand1.Dispose();

                if (BizActionObj.PatientPrescriptionDetailList != null)
                {
                    if (BizActionObj.PatientPrescriptionDetailList.Count > 0)
                    {
                        foreach (clsPatientPrescriptionDetailVO objCurMedVo in BizActionObj.PatientPrescriptionDetailList)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDetailFromCPOE");
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, PrescriptionID);
                            dbServer.AddInParameter(command1, "DrugID", DbType.Int64, objCurMedVo.DrugID);
                            dbServer.AddInParameter(command1, "Dose", DbType.String, objCurMedVo.Dose);
                            if (objCurMedVo.SelectedRoute != null && objCurMedVo.SelectedRoute.Description != "--Select--")
                                dbServer.AddInParameter(command1, "Route", DbType.String, objCurMedVo.SelectedRoute.Description);
                            else
                                dbServer.AddInParameter(command1, "Route", DbType.String, null);
                            if (objCurMedVo.SelectedInstruction != null && objCurMedVo.SelectedInstruction.Description != "--Select--")
                                dbServer.AddInParameter(command1, "Instruction", DbType.String, objCurMedVo.SelectedInstruction.Description);
                            else
                                dbServer.AddInParameter(command1, "Instruction", DbType.String, null);
                            if (objCurMedVo.FromHistory == true)
                            {
                                objCurMedVo.SelectedFrequency.Description = objCurMedVo.Frequency;
                            }
                            if (objCurMedVo.SelectedFrequency.Description != "--Select--")
                                dbServer.AddInParameter(command1, "Frequency", DbType.String, objCurMedVo.SelectedFrequency.Description);
                            else
                                dbServer.AddInParameter(command1, "Frequency", DbType.String, null);
                            dbServer.AddInParameter(command1, "ItemName", DbType.String, objCurMedVo.DrugName);
                            dbServer.AddInParameter(command1, "Days", DbType.Int64, objCurMedVo.Days);
                            dbServer.AddInParameter(command1, "Quantity", DbType.Double, objCurMedVo.Quantity);
                            dbServer.AddInParameter(command1, "Rate", DbType.Int64, objCurMedVo.Rate);
                            dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objCurMedVo.IsOther);
                            dbServer.AddInParameter(command1, "Reason", DbType.String, objCurMedVo.Reason);
                            dbServer.AddInParameter(command1, "Timing", DbType.String, null);
                            dbServer.AddInParameter(command1, "ID", DbType.Int64, objCurMedVo.ID);
                            // dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        }
                    }
                }
                DbCommand mycommand = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorSuggestedServiceDeatailFromCPOE");
                dbServer.AddInParameter(mycommand, "PrescriptionID", DbType.Int64, PrescriptionID);
                dbServer.AddInParameter(mycommand, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddOutParameter(mycommand, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(mycommand, trans);
                int SuccessStatus0 = (int)dbServer.GetParameterValue(mycommand, "ResultStatus");
                mycommand.Dispose();
                if (BizActionObj.PatientServiceDetailDetailList != null)
                {
                    if (BizActionObj.PatientServiceDetailDetailList.Count > 0)
                    {

                        foreach (clsDoctorSuggestedServiceDetailVO objCurMedVo in BizActionObj.PatientServiceDetailDetailList)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDeatailFromCPOE");
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, PrescriptionID);
                            dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, objCurMedVo.ServiceID);
                            dbServer.AddInParameter(command1, "ServiceName", DbType.String, objCurMedVo.ServiceName);
                            dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objCurMedVo.IsOther);
                            dbServer.AddInParameter(command1, "Reason", DbType.String, objCurMedVo.Reason);
                            dbServer.AddInParameter(command1, "Rate", DbType.Double, 0);
                            if (objCurMedVo.SelectedDoctor != null)
                                dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, objCurMedVo.SelectedDoctor.ID);
                            else
                                dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, 0);
                            dbServer.AddInParameter(command1, "ID", DbType.Int64, objCurMedVo.ID);
                            // dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus3 = dbServer.ExecuteNonQuery(command1, trans);
                            int SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        }
                    }
                }
                BizActionObj.SuccessStatus = 1;
                trans.Commit();
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                //WriteErrorLog("AddUpdatePatientCPOEDetail", "CIMS_AddUpdatePatientPrescriptionFromCPOE + CIMS_AddUpdatePatientFollowUpFromCPOE + CIMS_AddPatientPrescriptionDetailFromCPOE + CIMS_DeleteDoctorSuggestedServiceDeatailFromCPOE + CIMS_AddDoctorSuggestedServiceDeatailFromCPOE", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "CPOE Detail", ex.Message);
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return valueObject;
        }
        public override IValueObject GetPatientCPOEDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCPOEDetailsBizActionVO BizActionObj = valueObject as clsGetPatientCPOEDetailsBizActionVO;
            try
            {
                DbDataReader reader;
                DbCommand prescriptionCommand = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionForCPOE");

                dbServer.AddInParameter(prescriptionCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(prescriptionCommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(prescriptionCommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(prescriptionCommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(prescriptionCommand, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(prescriptionCommand, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                reader = (DbDataReader)dbServer.ExecuteReader(prescriptionCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PrescriptionID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        //  BizActionObj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        BizActionObj.Advice = (string)DALHelper.HandleDBNull(reader["Advice"]);
                        BizActionObj.FollowupDate = (DateTime?)DALHelper.HandleDate(reader["FollowUpDate"]);
                        BizActionObj.FollowUpRemark = (string)DALHelper.HandleDBNull(reader["FollowUpRemark"]);
                    }
                }

                if (BizActionObj.PrescriptionID >= 0)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionForCPOEDetails");
                    dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(command1, "Doctorid", DbType.Int64, BizActionObj.DoctorID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientPrescriptionDetailList == null)
                            BizActionObj.PatientPrescriptionDetailList = new List<clsPatientPrescriptionDetailVO>();
                        while (reader.Read())
                        {
                            clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                            objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                            objPrescriptionVO.SelectedItem.ID = (long)(DALHelper.HandleDBNull(reader["DrugID"]));
                            objPrescriptionVO.SelectedItem.Description = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                            objPrescriptionVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                            objPrescriptionVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["DrugID"]));
                            objPrescriptionVO.SelectedFrequency.Description = (string)(DALHelper.HandleDBNull(reader["Frequency"]));
                            objPrescriptionVO.SelectedInstruction.Description = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                            objPrescriptionVO.Instruction = (string)DALHelper.HandleDBNull(reader["Reason"]);
                            objPrescriptionVO.NewInstruction = (string)DALHelper.HandleDBNull(reader["Instruction"]);
                            //objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                            objPrescriptionVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                            objPrescriptionVO.Quantity = (int)(DALHelper.HandleDBNull(reader["Quantity"]));
                            objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                            objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                            //objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                            objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                            objPrescriptionVO.FromHistory = false;
                            objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                            objPrescriptionVO.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                            objPrescriptionVO.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                            //added by neena
                            objPrescriptionVO.ArtEnabled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["ARTEnables"]));
                            objPrescriptionVO.DrugSource = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugSourceId"]));
                            objPrescriptionVO.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
                            objPrescriptionVO.PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]));
                            //
                            BizActionObj.PatientPrescriptionDetailList.Add(objPrescriptionVO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // WriteErrorLog("AddUpdatePatientCPOEDetail", "CIMS_GetPatientPrescriptionForCPOE + CIMS_GetPatientPrescriptionForCPOEDetails + CIMS_GetPatientCPOEServiceList", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "Get CPOE Detail", ex.Message);
                throw ex;
            }

            return valueObject;
        }
        public override IValueObject GetPatientCPOEPrescriptionDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCPOEPrescriptionDetailsBizActionVO BizActionObj = valueObject as clsGetPatientCPOEPrescriptionDetailsBizActionVO;
            try
            {
                DbDataReader reader;

                DbCommand mygetcommand = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionID");
                dbServer.AddInParameter(mygetcommand, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(mygetcommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(mygetcommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(mygetcommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(mygetcommand, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                reader = (DbDataReader)dbServer.ExecuteReader(mygetcommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PrescriptionID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    }

                }
                mygetcommand.Dispose();

                if (BizActionObj.PrescriptionID > 0)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionForCPOEDetails");
                    dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientPrescriptionDetailList == null)
                            BizActionObj.PatientPrescriptionDetailList = new List<clsPatientPrescriptionDetailVO>();
                        while (reader.Read())
                        {
                            clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                            objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                            objPrescriptionVO.SelectedItem.ID = (long)(DALHelper.HandleDBNull(reader["DrugID"]));
                            objPrescriptionVO.SelectedItem.Description = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                            objPrescriptionVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                            objPrescriptionVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["DrugID"]));
                            objPrescriptionVO.SelectedFrequency.Description = (string)(DALHelper.HandleDBNull(reader["Frequency"]));
                            objPrescriptionVO.SelectedInstruction.Description = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                            objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                            objPrescriptionVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                            objPrescriptionVO.Quantity = (double)DALHelper.HandleDoubleNull(reader["Quantity"]);
                            objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                            objPrescriptionVO.NewInstruction = (string)DALHelper.HandleDBNull(reader["Instruction"]);
                            objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                            objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                            objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                            objPrescriptionVO.FromHistory = false;
                            objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                            BizActionObj.PatientPrescriptionDetailList.Add(objPrescriptionVO);
                        }
                    }
                    reader.Close();
                    command1.Dispose();
                }
            }
            catch (Exception ex)
            {
                //  WriteErrorLog("GetPatientCPOEPrescriptionDetail", "CIMS_GetPatientPrescriptionID + CIMS_GetPatientPrescriptionForCPOEDetails", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "Get CPOE Prescription Detail", ex.Message);
                throw ex;
            }

            return valueObject;
        }
        public override IValueObject GetItemsNServiceBySelectedDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCPOEServicItemDiagnosisWiseBizActionVO BizActionObj = valueObject as clsGetCPOEServicItemDiagnosisWiseBizActionVO;
            try
            {
                DbDataReader reader;
                if (BizActionObj.DiagnosisIds != "")
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetItemDetailsByDiagnosisID");
                    dbServer.AddInParameter(command1, "DiagnosisId", DbType.String, BizActionObj.DiagnosisIds);

                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.ItemBySelectedDiagnosisDetailList == null)
                            BizActionObj.ItemBySelectedDiagnosisDetailList = new List<clsGetItemBySelectedDiagnosisVO>();
                        while (reader.Read())
                        {
                            clsGetItemBySelectedDiagnosisVO objMappedItemsVO = new clsGetItemBySelectedDiagnosisVO();
                            objMappedItemsVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                            objMappedItemsVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["ItemID"]));
                            objMappedItemsVO.RouteID = (long)(DALHelper.HandleDBNull(reader["RouteID"]));
                            objMappedItemsVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                            objMappedItemsVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                            BizActionObj.ItemBySelectedDiagnosisDetailList.Add(objMappedItemsVO);
                        }
                    }
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetServiceDetailsByDiagnosisID");
                    dbServer.AddInParameter(command1, "DiagnosisId", DbType.String, BizActionObj.DiagnosisIds);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.ServiceBySelectedDiagnosisDetailList == null)
                            BizActionObj.ServiceBySelectedDiagnosisDetailList = new List<clsGetServiceBySelectedDiagnosisVO>();
                        while (reader.Read())
                        {
                            clsGetServiceBySelectedDiagnosisVO objMappedServiceVO = new clsGetServiceBySelectedDiagnosisVO();

                            objMappedServiceVO.ServiceID = (long)(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objMappedServiceVO.ServiceName = (string)(DALHelper.HandleDBNull(reader["ServiceName"]));
                            objMappedServiceVO.Status = (bool)DALHelper.HandleDBNull(reader["status"]);
                            objMappedServiceVO.ServiceRate = (decimal)DALHelper.HandleDBNull(reader["ServiceRate"]);
                            BizActionObj.ServiceBySelectedDiagnosisDetailList.Add(objMappedServiceVO);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valueObject;
        }
        public override IValueObject DeleteCPOEService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteCPOEServiceBizActionVO BizActionObj = valueObject as clsDeleteCPOEServiceBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteCPOEService");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                command.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;

            }
            return valueObject;

        }
        public override IValueObject DeleteCPOEMedicine(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteCPOEMedicineBizActionVO BizActionObj = valueObject as clsDeleteCPOEMedicineBizActionVO;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteCPOEDrug");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                command.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;

            }
            return valueObject;

        }
        public override IValueObject AddUpdateFollowUpDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateFollowUpDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateFollowUpDetailsBizActionVO;
            DbCommand command;
            if (BizActionObj.ISFollowUpNewQueueList == false)
                {
                     command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientFollowUp");
                }
            else
                {
                     command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientFollowUpQueue");
                }
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command, "FollowUpDateTime", DbType.DateTime, BizActionObj.FollowupDate);
                dbServer.AddInParameter(command, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
                dbServer.AddInParameter(command, "NoFollowReq", DbType.Boolean, BizActionObj.FolloWUPRequired);
                dbServer.AddInParameter(command, "AppoinmentReson", DbType.Int64, BizActionObj.AppoinmentReson);
                dbServer.AddInParameter(command, "DepartmentCode", DbType.String, BizActionObj.DepartmentCode);
                dbServer.AddInParameter(command, "FollowUpRemarks", DbType.String, BizActionObj.FollowUpRemark);
                dbServer.AddInParameter(command, "Advice", DbType.String, BizActionObj.Advice);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter   (command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FollowUpID);
               
            int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt64(dbServer.GetParameterValue(command, "ResultStatus"));
                command.Dispose();              
                return BizActionObj;
        }
        public override IValueObject GetPatientFollowUpDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFollowUpDetailsBizActionVO BizActionObj = valueObject as clsGetPatientFollowUpDetailsBizActionVO;

            DbDataReader reader;
            DbCommand Command = dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUpDetails");
            dbServer.AddInParameter(Command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(Command, "VisitID", DbType.Int64, BizActionObj.VisitID);
            dbServer.AddInParameter(Command, "PatientID", DbType.Int64, BizActionObj.PatientID);
            dbServer.AddInParameter(Command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
            dbServer.AddInParameter(Command, "IsOpdIpd", DbType.Int64, BizActionObj.Isopdipd);
            reader = (DbDataReader)dbServer.ExecuteReader(Command);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BizActionObj.PrescriptionID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    BizActionObj.FollowUpAdvice = (string)DALHelper.HandleDBNull(reader["Advice"]);
                    BizActionObj.FollowupDate = (DateTime?)DALHelper.HandleDate(reader["FollowUpDate"]);
                    BizActionObj.FollowUpRemark = (string)DALHelper.HandleDBNull(reader["FollowUpRemark"]);
                    BizActionObj.NoFollowReq = (Boolean)DALHelper.HandleDBNull(reader["NoFollowReq"]);
                    BizActionObj.AppoinmentReson = (long)DALHelper.HandleDBNull(reader["AppoinmentReson"]);
                }
            }
            reader.Close();
            Command.Dispose();

            return BizActionObj;
        }
        public override IValueObject GetPatientPastChiefComplaints(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastChiefComplaintsBizActionVO BizAction = valueObject as clsGetPatientPastChiefComplaintsBizActionVO;
            DbDataReader reader;
            DbCommand Command = dbServer.GetStoredProcCommand("CIMS_GetPatientPastChiefComplaints");
            dbServer.AddInParameter(Command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(Command, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(Command, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(Command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
            dbServer.AddInParameter(Command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
            dbServer.AddInParameter(Command, "Doctorid", DbType.Int32, BizAction.DoctorID);
            reader = (DbDataReader)dbServer.ExecuteReader(Command);
            if (reader.HasRows)
            {
                BizAction.ChiefComplaintList = new List<clsChiefComplaintsVO>();
                while (reader.Read())
                {
                    clsChiefComplaintsVO objChiefcomplaint = new clsChiefComplaintsVO();
                    objChiefcomplaint.Description = Convert.ToString(reader["Description"]);
                    //objChiefcomplaint.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                    //objChiefcomplaint.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"]));
                    objChiefcomplaint.DoctorName = Convert.ToString(reader["DoctorName"]);
                    objChiefcomplaint.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                    objChiefcomplaint.DoctorSpec = Convert.ToString(reader["SpecializationName"]);
                    //objChiefcomplaint.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                    //objChiefcomplaint.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    BizAction.ChiefComplaintList.Add(objChiefcomplaint);
                }
            }
            reader.Close();
            Command.Dispose();
            return BizAction;
        }
        public override IValueObject GetServicesCPOEDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCurrentServicesBizActionVO BizAction = valueObject as clsGetPatientCurrentServicesBizActionVO;

            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCurrentServicesDetails");
            dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
            dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(command, "Doctorid", DbType.Int32, BizAction.DoctorID);
            dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            BizAction.ServiceDetails = new List<clsDoctorSuggestedServiceDetailVO>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    clsDoctorSuggestedServiceDetailVO objServices = new clsDoctorSuggestedServiceDetailVO();
                    objServices.PrescriptionID = (long)DALHelper.HandleIntegerNull(reader["PrescriptionID"]);
                    objServices.ServiceID = Convert.ToInt64(reader["ServiceID"]);
                    objServices.ServiceName = Convert.ToString(reader["ServiceName"]);
                    objServices.ServiceRate = Convert.ToDouble(reader["serviceRate"]);
                    objServices.ServiceCode = Convert.ToString(reader["ServiceCode"]);
                    objServices.PriorityIndex = Convert.ToInt32(reader["PriorityID"]);
                    objServices.GroupName = Convert.ToString(reader["GroupName"]);
                    objServices.SpecializationId = Convert.ToInt64(reader["SpecializationId"]);
                    BizAction.ServiceDetails.Add(objServices);
                }
                reader.Close();
            }
            return BizAction;
        }
        public override IValueObject AddUpdatePatientHistoryData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientHistoryDataBizActionVO BizActionObj = valueObject as clsAddUpdatePatientHistoryDataBizActionVO;
            try
            {
                clsPatientEMRDataVO item = BizActionObj.PatientEMRData;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientHistoryData");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, item.LinkServer);
                if (item.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, item.LinkServer.Replace(@"\", "_"));
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, item.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, item.TemplateID);
                dbServer.AddInParameter(command, "TemplateByNurse", DbType.String, item.TemplateByNurse);
                dbServer.AddInParameter(command, "TemplateByDoctor", DbType.String, item.TemplateByDoctor);
                dbServer.AddInParameter(command, "HistoryTemplate", DbType.String, item.HistoryTemplate);

                dbServer.AddInParameter(command, "VisitID", DbType.Int64, item.VisitID);
                dbServer.AddInParameter(command, "SavedBy", DbType.String, item.SavedBy);
                dbServer.AddInParameter(command, "Freeze", DbType.Boolean, item.Freeze);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, item.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                item.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
        public override IValueObject AddUpdatePatientHistoryDataAndDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientHistoryDataAndDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatePatientHistoryDataAndDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                clsPatientEMRDataVO item = BizActionObj.PatientEMRData;
                if (item != null)
                {
                    DbCommand commandData = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRData");
                    dbServer.AddInParameter(commandData, "PatientID", DbType.Int64, item.PatientID);
                    dbServer.AddInParameter(commandData, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(commandData, "VisitID", DbType.Int64, item.VisitID);
                    dbServer.AddInParameter(commandData, "Doctorid", DbType.Int64, item.DoctorID);
                    dbServer.AddInParameter(commandData, "TemplateID", DbType.String, BizActionObj.TemplateID);
                    dbServer.AddInParameter(commandData, "SavedBy", DbType.String, item.SavedBy);
                    dbServer.AddInParameter(commandData, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(commandData, "TakenBy", DbType.Int64, BizActionObj.Takenby);
                    dbServer.AddInParameter(commandData, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //dbServer.AddInParameter(commandData, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandData, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(commandData, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandData, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(commandData, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(commandData, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(commandData, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(commandData, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(commandData, "ID");
                }
                DbCommand commandUpdate = dbServer.GetStoredProcCommand("CIMS_UpdatePatientHistoryDetails");
                dbServer.AddInParameter(commandUpdate, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(commandUpdate, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(commandUpdate, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(commandUpdate, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(commandUpdate, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(commandUpdate, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddOutParameter(commandUpdate, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(commandUpdate, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(commandUpdate, "ResultStatus");
                if (BizActionObj.SuccessStatus == -1)
                    throw new Exception();
                foreach (clsPatientEMRDetailsVO objPatEMRDetailsVO in BizActionObj.PatientHistoryDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientHistoryDetails");

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, BizActionObj.LinkServer);
                    if (BizActionObj.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command1, "PatientEMRdata", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, BizActionObj.Status);
                    dbServer.AddInParameter(command1, "ControlCaption", DbType.String, objPatEMRDetailsVO.ControlCaption);
                    dbServer.AddInParameter(command1, "ControlName", DbType.String, objPatEMRDetailsVO.ControlName);
                    dbServer.AddInParameter(command1, "ControlType", DbType.String, objPatEMRDetailsVO.ControlType);
                    dbServer.AddInParameter(command1, "ControlUnit", DbType.String, objPatEMRDetailsVO.ControlUnit);
                    dbServer.AddInParameter(command1, "ControlHeader", DbType.String, objPatEMRDetailsVO.Header);
                    dbServer.AddInParameter(command1, "ControlSection", DbType.String, objPatEMRDetailsVO.Section);
                    dbServer.AddInParameter(command1, "Doctorid", DbType.Int64, item.DoctorID);
                    dbServer.AddInParameter(command1, "Value", DbType.String, objPatEMRDetailsVO.Value);
                    dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatEMRDetailsVO.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    if (BizActionObj.SuccessStatus != 1 && BizActionObj.SuccessStatus != 2)
                        throw new Exception();
                }
                BizActionObj.SuccessStatus = 1;
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                // WriteErrorLog("AddUpdatePatientHistoryDataAndDetail", "CIMS_UpdatePatientHistoryDetails+CIMS_AddUpdatePatientHistoryDetails", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "History", ex.Message);
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return valueObject;
        }
        public override IValueObject GetPatientHistoryData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPatientHistoryDataBizActionVO BizActionObj = valueObject as clsGetPatientPatientHistoryDataBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientHistoryData");
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.TemplateByNurse = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateByNurse"]));
                        BizActionObj.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        BizActionObj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        BizActionObj.IsUpdated = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUpdated"]));
                    }
                    reader.NextResult();
                    BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                //WriteErrorLog("GetPatientHistoryData", "CIMS_GetPatientHistoryData", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "History", ex.Message);
                throw ex;
            }
            return valueObject;

        }
        public override IValueObject AddUpdatePatientPhysicalExamDataAndDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientPhysicalExamDataAndDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatePatientPhysicalExamDataAndDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                clsPatientEMRDataVO item = BizActionObj.PatientPhysicalExamData;
                if (item != null)
                {
                    DbCommand commandData = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRData");
                    dbServer.AddInParameter(commandData, "PatientID", DbType.Int64, item.PatientID);
                    dbServer.AddInParameter(commandData, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    dbServer.AddInParameter(commandData, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(commandData, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandData, "DoctorId", DbType.String, item.DoctorID);
                    dbServer.AddInParameter(commandData, "VisitID", DbType.Int64, item.VisitID);
                    dbServer.AddInParameter(commandData, "SavedBy", DbType.String, item.SavedBy);
                    dbServer.AddInParameter(commandData, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    //dbServer.AddInParameter(commandData, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandData, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(commandData, "TakenBy", DbType.Int64, BizActionObj.TakenBy);
                    dbServer.AddInParameter(commandData, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandData, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(commandData, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(commandData, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(commandData, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(commandData, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(commandData, "ID");
                }
                //if (BizActionObj.FalgForAddUpdate == true)
                //{
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientPhysicalExamDetails");
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "DoctorId", DbType.String, BizActionObj.DoctorId);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //}
                foreach (clsPatientEMRDetailsVO objPatEMRDetailsVO in BizActionObj.PatientPhysicalExamDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientPhysicalExamDetails");
                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, BizActionObj.LinkServer);
                    if (BizActionObj.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(command1, "Doctorid", DbType.String, BizActionObj.DoctorId);
                    dbServer.AddInParameter(command1, "PatientEmrDataId", DbType.String, item.ID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, objPatEMRDetailsVO.TemplateID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatEMRDetailsVO.Status);
                    dbServer.AddInParameter(command1, "Header", DbType.String, objPatEMRDetailsVO.Header);
                    dbServer.AddInParameter(command1, "ControlUnit", DbType.String, objPatEMRDetailsVO.ControlUnit);
                    dbServer.AddInParameter(command1, "Section", DbType.String, objPatEMRDetailsVO.Section);
                    dbServer.AddInParameter(command1, "ControlCaption", DbType.String, objPatEMRDetailsVO.ControlCaption);
                    dbServer.AddInParameter(command1, "ControlName", DbType.String, objPatEMRDetailsVO.ControlName);
                    dbServer.AddInParameter(command1, "ControlType", DbType.String, objPatEMRDetailsVO.ControlType);
                    dbServer.AddInParameter(command1, "Value", DbType.String, objPatEMRDetailsVO.Value);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatEMRDetailsVO.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    // BizActionObj.PatientHistoryDetailsList.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    if (BizActionObj.SuccessStatus != 1 && BizActionObj.SuccessStatus != 2)
                        throw new Exception();
                }
                // added by Saily P on 03.12.13 Purpose - New control
                //if (BizActionObj.IsBPControl == true)
                //{
                //    clsBaseDrugDAL objBaseDAL = clsBaseDrugDAL.GetInstance();
                //    clsAddPatientBPControlBizActionVO objBP = new clsAddPatientBPControlBizActionVO();
                //    objBP.IsBPControl = true;
                //    objBP.BPControlDetails = new clsBPControlVO();
                //    objBP.BPControlDetails.PatientID = BizActionObj.PatientPhysicalExamData.PatientID;
                //    objBP.BPControlDetails.PatientUnitID = BizActionObj.PatientPhysicalExamData.PatientUnitID;
                //    objBP.BPControlDetails.UnitID = BizActionObj.PatientPhysicalExamData.UnitId;
                //    objBP.BPControlDetails.VisitID = BizActionObj.PatientPhysicalExamData.VisitID;
                //    objBP.BPControlDetails.TemplateID = BizActionObj.TemplateID;
                //    objBP.BPControlDetails.DoctorID = BizActionObj.PatientPhysicalExamData.DoctorID;
                //    objBP.BPControlDetails.BP1 = BizActionObj.objBPDetails.BP1;
                //    objBP.BPControlDetails.BP2 = BizActionObj.objBPDetails.BP2;
                //    objBP = (clsAddPatientBPControlBizActionVO)objBaseDAL.AddBPControlDetails(objBP, UserVo);
                //    if (objBP.SuccessStatus == -1) throw new Exception();
                //}
                //if (BizActionObj.IsVisionControl == true)
                //{
                //    clsBaseDrugDAL objBaseDAL = clsBaseDrugDAL.GetInstance();
                //    clsAddPatientVisionControlBizActionVO objVision = new clsAddPatientVisionControlBizActionVO();
                //    objVision.IsVisionControl = true;
                //    objVision.VisionControlDetails = new clsVisionVO();
                //    objVision.VisionControlDetails.PatientID = BizActionObj.PatientPhysicalExamData.PatientID;
                //    objVision.VisionControlDetails.PatientUnitID = BizActionObj.PatientPhysicalExamData.PatientUnitID;
                //    objVision.VisionControlDetails.UnitID = BizActionObj.PatientPhysicalExamData.UnitId;
                //    objVision.VisionControlDetails.VisitID = BizActionObj.PatientPhysicalExamData.VisitID;
                //    objVision.VisionControlDetails.TemplateID = BizActionObj.TemplateID;
                //    objVision.VisionControlDetails.DoctorID = BizActionObj.PatientPhysicalExamData.DoctorID;
                //    objVision.VisionControlDetails.SPH = BizActionObj.objVisionDetails.SPH;
                //    objVision.VisionControlDetails.CYL = BizActionObj.objVisionDetails.CYL;
                //    objVision.VisionControlDetails.Axis = BizActionObj.objVisionDetails.Axis;
                //    objVision = (clsAddPatientVisionControlBizActionVO)objBaseDAL.AddVisionControlDetails(objVision, UserVo);
                //    if (objVision.SuccessStatus == -1) throw new Exception();
                //}
                //if (BizActionObj.IsGPControl == true)
                //{
                //    clsBaseDrugDAL objBaseDAL = clsBaseDrugDAL.GetInstance();
                //    clsAddPatientGPControlBizActionVO objGP = new clsAddPatientGPControlBizActionVO();
                //    objGP.IsGPControl = true;
                //    objGP.GPControlDetails = new clsGlassPowerVO();
                //    objGP.GPControlDetails.PatientID = BizActionObj.PatientPhysicalExamData.PatientID;
                //    objGP.GPControlDetails.PatientUnitID = BizActionObj.PatientPhysicalExamData.PatientUnitID;
                //    objGP.GPControlDetails.UnitID = BizActionObj.PatientPhysicalExamData.UnitId;
                //    objGP.GPControlDetails.VisitID = BizActionObj.PatientPhysicalExamData.VisitID;
                //    objGP.GPControlDetails.TemplateID = BizActionObj.TemplateID;
                //    objGP.GPControlDetails.DoctorID = BizActionObj.PatientPhysicalExamData.DoctorID;
                //    objGP.GPControlDetails.SPH1 = BizActionObj.objGPDetails.SPH1;
                //    objGP.GPControlDetails.CYL1 = BizActionObj.objGPDetails.CYL1;
                //    objGP.GPControlDetails.Axis1 = BizActionObj.objGPDetails.Axis1;
                //    objGP.GPControlDetails.VA1 = BizActionObj.objGPDetails.VA1;
                //    objGP.GPControlDetails.SPH2 = BizActionObj.objGPDetails.SPH2;
                //    objGP.GPControlDetails.CYL2 = BizActionObj.objGPDetails.CYL2;
                //    objGP.GPControlDetails.Axis2 = BizActionObj.objGPDetails.Axis2;
                //    objGP.GPControlDetails.VA2 = BizActionObj.objGPDetails.VA2;
                //    objGP = (clsAddPatientGPControlBizActionVO)objBaseDAL.AddGPControlDetails(objGP, UserVo);
                //    if (objGP.SuccessStatus == -1) throw new Exception();
                //}

                BizActionObj.SuccessStatus = 1;
                trans.Commit();
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                //WriteErrorLog("AddUpdatePatientPhysicalExamDataAndDetail", "CIMS_UpdatePatientPhysicalExamDetails + CIMS_AddUpdatePatientPhysicalExamDetails", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "Consultation", ex.Message);
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return valueObject;
        }
        public override IValueObject AddUpdatePatientOTDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientOTDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatePatientOTDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                clsPatientEMRDataVO item = BizActionObj.PatientPhysicalExamData;
                if (item != null)
                {
                    DbCommand commandData = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTData");
                    dbServer.AddInParameter(commandData, "SceduleID", DbType.Int64, item.ScheduleID);
                    dbServer.AddInParameter(commandData, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    dbServer.AddInParameter(commandData, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandData, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(commandData, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandData, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(commandData, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(commandData, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(commandData, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(commandData, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(commandData, "ID");
                }
                DbCommand commandUpdate = dbServer.GetStoredProcCommand("CIMS_DeleteOTTemplatedetails");
                dbServer.AddInParameter(commandUpdate, "SecduleID", DbType.Int64, item.ScheduleID);
                dbServer.AddInParameter(commandUpdate, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(commandUpdate, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(commandUpdate, "EMrID", DbType.Int64, item.ID);
                int intStatus1 = dbServer.ExecuteNonQuery(commandUpdate, trans);

                foreach (clsPatientEMRDetailsVO objPatEMRDetailsVO in BizActionObj.PatientPhysicalExamDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTTemplateDetails");
                    dbServer.AddInParameter(command1, "PatientEmrDataId", DbType.String, item.ID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, objPatEMRDetailsVO.TemplateID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objPatEMRDetailsVO.Status);
                    dbServer.AddInParameter(command1, "Header", DbType.String, objPatEMRDetailsVO.Header);
                    dbServer.AddInParameter(command1, "ControlUnit", DbType.String, objPatEMRDetailsVO.ControlUnit);
                    dbServer.AddInParameter(command1, "Section", DbType.String, objPatEMRDetailsVO.Section);
                    dbServer.AddInParameter(command1, "ControlCaption", DbType.String, objPatEMRDetailsVO.ControlCaption);
                    dbServer.AddInParameter(command1, "ControlName", DbType.String, objPatEMRDetailsVO.ControlName);
                    dbServer.AddInParameter(command1, "ControlType", DbType.String, objPatEMRDetailsVO.ControlType);
                    dbServer.AddInParameter(command1, "Value", DbType.String, objPatEMRDetailsVO.Value);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatEMRDetailsVO.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    if (BizActionObj.SuccessStatus != 1 && BizActionObj.SuccessStatus != 2)
                        throw new Exception();
                }
                foreach (ListItems2 Img in BizActionObj.UploadImg)
                {
                    string ImgName = GetImageName(BizActionObj.TemplateID, item.ScheduleID, UserVo.UserLoginInfo.UnitId, con, trans);
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddImageForOT");
                    dbServer.AddInParameter(command, "SceduleID", DbType.Int64, item.ScheduleID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int32, BizActionObj.TemplateID);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "ImgPath", DbType.String, ImgName);
                    dbServer.AddInParameter(command, "EMRID", DbType.Int64, item.ID);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    MemoryStream ms = new MemoryStream(Img.Photo);
                    Image image = Image.FromStream(ms);
                    Image img = (Image)(new Bitmap(image, new Size(150, 120)));
                    ImageConverter converter = new ImageConverter();
                    MemoryStream mem = new MemoryStream();
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    img.Save(ImgSaveLocation + ImgName, jpgEncoder, myEncoderParameters);
                }


                BizActionObj.SuccessStatus = 1;
                trans.Commit();
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return valueObject;
        }
        public override IValueObject GetPatientVisitSummaryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientVisitSummaryListForEMRBizActionVO BizActionObj = valueObject as clsGetPatientVisitSummaryListForEMRBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSummaryListForEMR");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "unitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.VisitEMRDetailsList == null)
                        BizActionObj.VisitEMRDetailsList = new List<clsVisitEMRDetails>();
                    while (reader.Read())
                    {
                        clsVisitEMRDetails objVisitVO = new clsVisitEMRDetails();
                        objVisitVO.VisitId = (long)DALHelper.HandleDBNull(reader["Id"]);
                        objVisitVO.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        objVisitVO.Department = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));
                        objVisitVO.VisitTypeID = (long)DALHelper.HandleDBNull(reader["VisitTypeID"]);
                        objVisitVO.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objVisitVO.VisitDate = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        objVisitVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OPDNO"]);
                        objVisitVO.Doctor = (string)DALHelper.HandleDBNull(reader["Doctor"]);
                        BizActionObj.VisitEMRDetailsList.Add(objVisitVO);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // WriteErrorLog("GetPatientVisitSummaryDetails", "CIMS_GetPatientSummaryListForEMR", 0, BizActionObj.PatientID, BizActionObj.PatientUnitID, 0, 0, "Get EMR Visit Details", ex.Message);
                throw ex;
            }
            return valueObject;
        }
        public override IValueObject GetVisitAdmission(IValueObject valueObject, clsUserVO UserVo)
        {
            int ItemCount = 3;
            ClsGetVisitAdmissionBizActionVO BizActionObj = valueObject as ClsGetVisitAdmissionBizActionVO;
            if (BizActionObj.DMSViewerVisitAdmissionList == null)
                BizActionObj.DMSViewerVisitAdmissionList = new List<clsDMSViewerVisitAdmissionVO>();

            DbCommand command1 = null;
            command1 = dbServer.GetStoredProcCommand("DMS_GETFilesDetails");
            DbDataReader reader1;
            dbServer.AddInParameter(command1, "MRNO", DbType.String, BizActionObj.MRNO);
            dbServer.AddInParameter(command1, "OPD_IPD", DbType.Int16, 0);
            reader1 = (DbDataReader)dbServer.ExecuteReader(command1);

            clsDMSViewerVisitAdmissionVO OPD_objDMSViewerVisitAdmissionDetails = new clsDMSViewerVisitAdmissionVO();

            OPD_objDMSViewerVisitAdmissionDetails.ID = 1; ;
            OPD_objDMSViewerVisitAdmissionDetails.Date = DateTime.Now;
            OPD_objDMSViewerVisitAdmissionDetails.DoctorName = "OPD";
            OPD_objDMSViewerVisitAdmissionDetails.NoOfFiles = 0;
            OPD_objDMSViewerVisitAdmissionDetails.DepartmentName = "";
            OPD_objDMSViewerVisitAdmissionDetails.PatientID = 0;
            OPD_objDMSViewerVisitAdmissionDetails.PatientUnitID = 0;
            OPD_objDMSViewerVisitAdmissionDetails.OPD_IPD_External = 0;

            BizActionObj.DMSViewerVisitAdmissionList.Add(OPD_objDMSViewerVisitAdmissionDetails);

            clsDMSViewerVisitAdmissionVO IPD_objDMSViewerVisitAdmissionDetails = new clsDMSViewerVisitAdmissionVO();

            IPD_objDMSViewerVisitAdmissionDetails.ID = 2;
            IPD_objDMSViewerVisitAdmissionDetails.Date = DateTime.Now;
            IPD_objDMSViewerVisitAdmissionDetails.DoctorName = "IPD";
            IPD_objDMSViewerVisitAdmissionDetails.NoOfFiles = 0;
            IPD_objDMSViewerVisitAdmissionDetails.DepartmentName = "";
            IPD_objDMSViewerVisitAdmissionDetails.PatientID = 0;
            IPD_objDMSViewerVisitAdmissionDetails.PatientUnitID = 0;
            IPD_objDMSViewerVisitAdmissionDetails.OPD_IPD_External = 1;
            BizActionObj.DMSViewerVisitAdmissionList.Add(IPD_objDMSViewerVisitAdmissionDetails);
            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    clsDMSViewerVisitAdmissionVO objDMSViewerVisitAdmissionDetails = new clsDMSViewerVisitAdmissionVO();

                    objDMSViewerVisitAdmissionDetails.ID = ItemCount;
                    objDMSViewerVisitAdmissionDetails.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader1["Date"]));
                    objDMSViewerVisitAdmissionDetails.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader1["DoctorName"]));
                    objDMSViewerVisitAdmissionDetails.NoOfFiles = Convert.ToInt32(DALHelper.HandleDBNull(reader1["NoOfFiles"]));
                    objDMSViewerVisitAdmissionDetails.PatientVisitId = Convert.ToInt32(DALHelper.HandleDBNull(reader1["OpdIpdId"]));
                    objDMSViewerVisitAdmissionDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["PatientID"]));
                    objDMSViewerVisitAdmissionDetails.ParentID = 1;
                    objDMSViewerVisitAdmissionDetails.OPD_IPD_External = 0;
                    ItemCount++;
                    BizActionObj.DMSViewerVisitAdmissionList.Add(objDMSViewerVisitAdmissionDetails);

                    DbCommand command4 = null;
                    command4 = dbServer.GetStoredProcCommand("DMS_GetFilePathAndName");
                    DbDataReader reader4;
                    dbServer.AddInParameter(command4, "VisitId", DbType.Int64, objDMSViewerVisitAdmissionDetails.PatientVisitId);
                    dbServer.AddInParameter(command4, "OPD_IPD", DbType.Int16, 0);

                    reader4 = (DbDataReader)dbServer.ExecuteReader(command4);
                    if (reader4.HasRows)
                    {
                        while (reader4.Read())
                        {
                            clsDMSViewerVisitAdmissionVO objDMSViewerVisitAdmissionDetailsForFileOPD = new clsDMSViewerVisitAdmissionVO();
                            objDMSViewerVisitAdmissionDetailsForFileOPD.ID = ItemCount;
                            objDMSViewerVisitAdmissionDetailsForFileOPD.ImgName = Convert.ToString(DALHelper.HandleDBNull(reader4["Filename"]));
                            objDMSViewerVisitAdmissionDetailsForFileOPD.ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader4["ServerPathRegNo"]));
                            objDMSViewerVisitAdmissionDetailsForFileOPD.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader4["Filename"]));
                            objDMSViewerVisitAdmissionDetailsForFileOPD.ParentID = objDMSViewerVisitAdmissionDetails.ID;
                            objDMSViewerVisitAdmissionDetailsForFileOPD.OPD_IPD_External = 0;
                            ItemCount++;
                            BizActionObj.DMSViewerVisitAdmissionList.Add(objDMSViewerVisitAdmissionDetailsForFileOPD);
                        }
                    }
                    reader4.Close();
                }
            }
            reader1.Close();
            DbCommand command2 = null;
            command2 = dbServer.GetStoredProcCommand("DMS_GETFilesDetails");
            DbDataReader reader2;
            dbServer.AddInParameter(command2, "MRNO", DbType.String, BizActionObj.MRNO);
            dbServer.AddInParameter(command2, "OPD_IPD", DbType.Boolean, true);
            reader2 = (DbDataReader)dbServer.ExecuteReader(command2);

            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    clsDMSViewerVisitAdmissionVO objDMSViewerVisitAdmissionDetails = new clsDMSViewerVisitAdmissionVO();

                    objDMSViewerVisitAdmissionDetails.ID = ItemCount;
                    objDMSViewerVisitAdmissionDetails.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader2["Date"]));
                    objDMSViewerVisitAdmissionDetails.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader2["DoctorName"]));
                    objDMSViewerVisitAdmissionDetails.NoOfFiles = Convert.ToInt32(DALHelper.HandleDBNull(reader2["NoOfFiles"]));
                    //objDMSViewerVisitAdmissionDetails.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader2["DepartmentName"]));
                    objDMSViewerVisitAdmissionDetails.PatientVisitId = Convert.ToInt32(DALHelper.HandleDBNull(reader2["OpdIpdId"]));
                    objDMSViewerVisitAdmissionDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PatientID"]));
                    objDMSViewerVisitAdmissionDetails.OPD_IPD_External = 1;
                    objDMSViewerVisitAdmissionDetails.ParentID = 2;
                    ItemCount++;
                    //BizActionObj.DMSViewerVisitAdmissionList.Add(objDMSViewerVisitAdmissionDetails);
                    DbCommand command3 = null;
                    command3 = dbServer.GetStoredProcCommand("DMS_GetFolderMaster");
                    DbDataReader reader3;
                    reader3 = (DbDataReader)dbServer.ExecuteReader(command3);
                    if (reader3.HasRows)
                    {
                        while (reader3.Read())
                        {
                            clsDMSViewerVisitAdmissionVO objDMSViewerVisitAdmissionFolderDetails = new clsDMSViewerVisitAdmissionVO();
                            objDMSViewerVisitAdmissionFolderDetails.ID = ItemCount;
                            objDMSViewerVisitAdmissionFolderDetails.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader3["Description"]));
                            objDMSViewerVisitAdmissionFolderDetails.PatientID = BizActionObj.PatientID;
                            objDMSViewerVisitAdmissionFolderDetails.FolderID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ID"]));
                            objDMSViewerVisitAdmissionFolderDetails.OPD_IPD_External = 1;
                            objDMSViewerVisitAdmissionFolderDetails.ParentID = objDMSViewerVisitAdmissionDetails.ID;
                            ItemCount++;
                            BizActionObj.DMSViewerVisitAdmissionList.Add(objDMSViewerVisitAdmissionFolderDetails);

                            //Code for Get Images for IPD patient 
                            DbCommand command5 = null;
                            command5 = dbServer.GetStoredProcCommand("DMS_GetFilePathForIPD");
                            DbDataReader reader5;
                            dbServer.AddInParameter(command5, "FolderID", DbType.Int32, objDMSViewerVisitAdmissionFolderDetails.FolderID);
                            dbServer.AddInParameter(command5, "AdmissionID", DbType.Int32, objDMSViewerVisitAdmissionDetails.PatientVisitId);
                            reader5 = (DbDataReader)dbServer.ExecuteReader(command5);
                            if (reader5.HasRows)
                            {
                                while (reader5.Read())
                                {
                                    clsDMSViewerVisitAdmissionVO objDMSViewerVisitAdmissionDetailsForFileIPD = new clsDMSViewerVisitAdmissionVO();
                                    objDMSViewerVisitAdmissionDetailsForFileIPD.ID = ItemCount;
                                    objDMSViewerVisitAdmissionDetailsForFileIPD.ImgName = Convert.ToString(DALHelper.HandleDBNull(reader5["Filename"]));
                                    objDMSViewerVisitAdmissionDetailsForFileIPD.ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader5["ServerPathFolder"]));
                                    objDMSViewerVisitAdmissionDetailsForFileIPD.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader5["Filename"]));
                                    objDMSViewerVisitAdmissionDetailsForFileIPD.ParentID = objDMSViewerVisitAdmissionFolderDetails.ID;
                                    objDMSViewerVisitAdmissionDetailsForFileIPD.OPD_IPD_External = 1;
                                    ItemCount++;
                                    BizActionObj.DMSViewerVisitAdmissionList.Add(objDMSViewerVisitAdmissionDetailsForFileIPD);
                                }
                            }
                            reader5.Close();
                        }

                    }
                    reader3.Close();

                    BizActionObj.DMSViewerVisitAdmissionList.Add(objDMSViewerVisitAdmissionDetails);
                }
            }
            reader2.Close();

            return BizActionObj;
        }
        public override IValueObject GetImage(IValueObject valueObject, clsUserVO UserVo)
        {
            ClsGetImageBizActionVO BizAction = ((ClsGetImageBizActionVO)valueObject);
            try
            {
                if (File.Exists(BizAction.ImagePath))
                {
                    BizAction.ImageByte = File.ReadAllBytes(BizAction.ImagePath);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;
        }
        public override IValueObject GetPatientAllVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientAllVisitBizActionVO BizActon = valueObject as clsGetPatientAllVisitBizActionVO;
            try
            {
                DbDataReader reader;
                DbCommand mygetcommand = dbServer.GetStoredProcCommand("CIMS_GetPatientAllVisit");
                dbServer.AddInParameter(mygetcommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(mygetcommand, "PatientID", DbType.Int64, BizActon.PatientID);
                dbServer.AddInParameter(mygetcommand, "IsfemaleTemplate", DbType.Boolean, BizActon.IsFemaleTemplate);
                dbServer.AddInParameter(mygetcommand, "TemplateID", DbType.Int64, BizActon.TemplateID);
                dbServer.AddInParameter(mygetcommand, "ISFromNursing", DbType.Boolean, BizActon.ISFromNursing);
                dbServer.AddInParameter(mygetcommand, "VisitID", DbType.Int64, BizActon.VisitID1);
                dbServer.AddInParameter(mygetcommand, "IsPhysicalExamination", DbType.Int64, BizActon.IsPhysicalExamination);
                dbServer.AddInParameter(mygetcommand, "AllTemplateID", DbType.String, BizActon.AllTemplateID);

                reader = (DbDataReader)dbServer.ExecuteReader(mygetcommand);
                if (reader.HasRows)
                {
                    if (BizActon.VisitList == null)
                        BizActon.VisitList = new List<clsVisitVO>();
                    while (reader.Read())
                    {
                        clsVisitVO objVisitVO = new clsVisitVO();
                        objVisitVO.ArtDashboardDate = Convert.ToString(DALHelper.HandleDBNull(reader["Date"]));
                        objVisitVO.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        objVisitVO.HistoryFlag = Convert.ToString(DALHelper.HandleDBNull(reader["histoyFlag"]));
                        objVisitVO.IsOPDIPDFlag = Convert.ToString(DALHelper.HandleDBNull(reader["ISOPDIPDFLAG"]));
                        objVisitVO.TakenBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["TakenBy"]));
                        objVisitVO.LoginNm = Convert.ToString(DALHelper.HandleDBNull(reader["loginnm"]));
                        if (BizActon.TemplateID == 63)
                            objVisitVO.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        if (BizActon.ISFromNursing)
                        {
                            objVisitVO.EMRID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EMRID"]));
                            objVisitVO.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        }

                        BizActon.VisitList.Add(objVisitVO);
                    }
                }
            }
            catch (Exception ee)
            {
            }
            return valueObject;
        }
        public override IValueObject AddUpdatePatientIVFHistoryDataAndDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientIVFHistoryDataAndDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatePatientIVFHistoryDataAndDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                clsPatientEMRDataVO item = BizActionObj.PatientEMRData;
                if (item != null)
                {
                    DbCommand commandData = dbServer.GetStoredProcCommand("CIMS_AddUpdateIVFPatientEMRData");
                    dbServer.AddInParameter(commandData, "PatientID", DbType.Int64, item.PatientID);
                    dbServer.AddInParameter(commandData, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                    dbServer.AddInParameter(commandData, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(commandData, "VisitID", DbType.Int64, item.VisitID);
                    dbServer.AddInParameter(commandData, "Doctorid", DbType.String, item.DoctorID);
                    dbServer.AddInParameter(commandData, "TemplateID", DbType.String, BizActionObj.TemplateID);
                    dbServer.AddInParameter(commandData, "SavedBy", DbType.String, item.SavedBy);
                    dbServer.AddInParameter(commandData, "IsIvfHistory", DbType.Boolean, BizActionObj.ISIvfhistory);
                    dbServer.AddInParameter(commandData, "UnitId", DbType.Int64, item.UnitId);
                    dbServer.AddInParameter(commandData, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandData, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(commandData, "IvfID", DbType.Int64, BizActionObj.SaveIvfID);
                    dbServer.AddInParameter(commandData, "TakenBy", DbType.Int64, BizActionObj.TakenBy);
                    dbServer.AddInParameter(commandData, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandData, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(commandData, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(commandData, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(commandData, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(commandData, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(commandData, "ID");
                }
                DbCommand commandUpdate = dbServer.GetStoredProcCommand("CIMS_UpdateIVFPatientHistoryDetails");
                dbServer.AddInParameter(commandUpdate, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(commandUpdate, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(commandUpdate, "UnitId", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(commandUpdate, "IvfID", DbType.Int64, BizActionObj.SaveIvfID);
                dbServer.AddInParameter(commandUpdate, "Isopdipd", DbType.Int64, BizActionObj.IsOPDIPD);
                //dbServer.AddInParameter(commandUpdate, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(commandUpdate, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(commandUpdate, "IsIvfHistory", DbType.Boolean, BizActionObj.ISIvfhistory);
                dbServer.AddOutParameter(commandUpdate, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(commandUpdate, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(commandUpdate, "ResultStatus");
                if (BizActionObj.SuccessStatus == -1)
                    throw new Exception();
                foreach (clsPatientEMRDetailsVO objPatEMRDetailsVO in BizActionObj.PatientHistoryDetailsList)
                {
                    //@IsOPDIPD
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateIVFPatientHistoryDetails");
                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, BizActionObj.LinkServer);
                    if (BizActionObj.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command1, "PatientEMRdata", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "IvfID", DbType.Int64, BizActionObj.SaveIvfID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, BizActionObj.Status);
                    dbServer.AddInParameter(command1, "ControlCaption", DbType.String, objPatEMRDetailsVO.ControlCaption);
                    dbServer.AddInParameter(command1, "ControlName", DbType.String, objPatEMRDetailsVO.ControlName);
                    dbServer.AddInParameter(command1, "ControlType", DbType.String, objPatEMRDetailsVO.ControlType);
                    dbServer.AddInParameter(command1, "Tab", DbType.String, BizActionObj.Tab);
                    dbServer.AddInParameter(command1, "Value", DbType.String, objPatEMRDetailsVO.Value);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatEMRDetailsVO.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    if (BizActionObj.SuccessStatus != 1 && BizActionObj.SuccessStatus != 2)
                        throw new Exception();
                }
                BizActionObj.SuccessStatus = 1;
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                // WriteErrorLog("AddUpdatePatientHistoryDataAndDetail", "CIMS_UpdatePatientHistoryDetails+CIMS_AddUpdatePatientHistoryDetails", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, "History", ex.Message);
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return valueObject;
        }
        public override IValueObject GetPatientIvfID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientIvfIDBizActionVO BizActionObj = valueObject as clsGetPatientIvfIDBizActionVO;
            DbDataReader reader;
            DbCommand mygetcommand = dbServer.GetStoredProcCommand("CIMS_GetPatientIvfID");
            reader = (DbDataReader)dbServer.ExecuteReader(mygetcommand);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BizActionObj.IvfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IvfID"]));
                }
            }
            return valueObject;
        }
        public override IValueObject GetPatientEMRIvfHistoryDataDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRIvfhistoryDetailsBizActionVO BizActionObj = valueObject as clsGetPatientEMRIvfhistoryDetailsBizActionVO;
            try
            {
                //bool isBPControl = false;
                //bool isVisionControl = false;
                //bool isGPControl = false;
                DbDataReader reader;
                //DbDataReader reader1;
                //DbDataReader reader2;
                //DbDataReader reader3;
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRIvfHistoryDetails");
                dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command1, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
                //dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command1, "Tab", DbType.String, BizActionObj.Tab);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command1, "EMRID", DbType.Int64, BizActionObj.EMRID);
                dbServer.AddInParameter(command1, "ISFromNursing", DbType.Boolean, BizActionObj.ISFromNursing);
                reader = (DbDataReader)dbServer.ExecuteReader(command1);
                if (reader.HasRows)
                {
                    if (BizActionObj.EMRDetailsList == null)
                        BizActionObj.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                        EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                        EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        EmrDetails.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
                        BizActionObj.EMRDetailsList.Add(EmrDetails);
                    }
                    reader.NextResult();
                    if (BizActionObj.EMRImgList == null)
                        BizActionObj.EMRImgList = new List<clsPatientEMRDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                        EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                        EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
                        EmrDetails.Value1 = (byte[])DALHelper.HandleDBNull(reader["Value"]);
                        BizActionObj.EMRImgList.Add(EmrDetails);
                    }
                }
                reader.Close();
                command1.Dispose();
            }
            catch (Exception ex)
            {
                // WriteErrorLog("GetPatientEMRDataDetails", "CIMS_GetPatientEMRDetails", BizActionObj.VisitID, BizActionObj.PatientID, BizActionObj.PatientUnitID, BizActionObj.TemplateID, BizActionObj.UnitID, BizActionObj.Tab, ex.Message);
                throw ex;
            }
            return BizActionObj;
        }
        public override IValueObject GetPatientPastFollowUPNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastFollowUPNotesBizActionVO BizAction = valueObject as clsGetPatientPastFollowUPNotesBizActionVO;
            DbDataReader reader;
            DbCommand Command = dbServer.GetStoredProcCommand("CIMS_GetPatientPastfollowupnotes");
            dbServer.AddInParameter(Command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(Command, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(Command, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(Command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
            dbServer.AddInParameter(Command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
            dbServer.AddInParameter(Command, "Doctorid", DbType.Int32, BizAction.DoctorID);
            dbServer.AddOutParameter(Command, "TotalRows", DbType.Int32, int.MaxValue);
            dbServer.AddInParameter(Command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
            dbServer.AddInParameter(Command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
            reader = (DbDataReader)dbServer.ExecuteReader(Command);
            if (reader.HasRows)
            {
                BizAction.PastFollowUPList = new List<clsPastFollowUpnoteVO>();
                while (reader.Read())
                {
                    clsPastFollowUpnoteVO objfollowUP = new clsPastFollowUpnoteVO();
                    objfollowUP.ID = Convert.ToInt64(reader["ID"]);
                    objfollowUP.Notes = Convert.ToString(reader["notes"]);
                    objfollowUP.Description = Convert.ToString(reader["Description"]);
                    objfollowUP.DoctorName = Convert.ToString(reader["DoctorName"]);
                    objfollowUP.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                    objfollowUP.DoctorSpec = Convert.ToString(reader["SpecializationName"]);
                    BizAction.PastFollowUPList.Add(objfollowUP);
                }
                reader.NextResult();
                BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(Command, "TotalRows"));
            }
            reader.Close();
            Command.Dispose();
            return BizAction;
        }


        public override IValueObject GetPatientPastcostNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastcostBizActionVO BizAction = valueObject as clsGetPatientPastcostBizActionVO;
            DbDataReader reader;
            DbCommand Command = dbServer.GetStoredProcCommand("CIMS_GetPatientPastcostnotes");  //CIMS_GetPatientPastfollowupnotes
            dbServer.AddInParameter(Command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(Command, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(Command, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(Command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
            dbServer.AddInParameter(Command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
            dbServer.AddInParameter(Command, "Doctorid", DbType.Int32, BizAction.DoctorID);
            dbServer.AddOutParameter(Command, "TotalRows", DbType.Int32, int.MaxValue);
            dbServer.AddInParameter(Command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
            dbServer.AddInParameter(Command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
            reader = (DbDataReader)dbServer.ExecuteReader(Command);
            if (reader.HasRows)
            {
                BizAction.PastFollowUPList = new List<clsPastFollowUpnoteVO>();
                while (reader.Read())
                {
                    clsPastFollowUpnoteVO objfollowUP = new clsPastFollowUpnoteVO();
                    objfollowUP.ID = Convert.ToInt64(reader["ID"]);
                    objfollowUP.Notes = Convert.ToString(reader["notes"]);
                    objfollowUP.Description = Convert.ToString(reader["Description"]);
                    objfollowUP.DoctorName = Convert.ToString(reader["DoctorName"]);
                    objfollowUP.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                    objfollowUP.DoctorSpec = Convert.ToString(reader["SpecializationName"]);
                    BizAction.PastFollowUPList.Add(objfollowUP);
                }
                reader.NextResult();
                BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(Command, "TotalRows"));
            }
            reader.Close();
            Command.Dispose();
            return BizAction;
        }

        public override IValueObject AddUpdatePatientForIPDLAPAndHistro(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientHistoryForIPDLapAndHistroBizActionVO BizActionObj = valueObject as clsAddUpdatePatientHistoryForIPDLapAndHistroBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                clsPatientEMRDataVO item = BizActionObj.PatientEMRData;
                if (item != null)
                {
                    DbCommand commandData = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRDataForIPD");
                    dbServer.AddInParameter(commandData, "PatientID", DbType.Int64, item.PatientID);
                    dbServer.AddInParameter(commandData, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(commandData, "VisitID", DbType.Int64, item.VisitID);
                    dbServer.AddInParameter(commandData, "Doctorid", DbType.Int64, item.DoctorID);
                    dbServer.AddInParameter(commandData, "TemplateID", DbType.String, BizActionObj.TemplateID);
                    dbServer.AddInParameter(commandData, "SavedBy", DbType.String, item.SavedBy);
                    dbServer.AddInParameter(commandData, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(commandData, "TakenBy", DbType.Int64, BizActionObj.Takenby);
                    dbServer.AddInParameter(commandData, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //dbServer.AddInParameter(commandData, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandData, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(commandData, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(commandData, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(commandData, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(commandData, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(commandData, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(commandData, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(commandData, "ID");
                }
                foreach (clsPatientEMRDetailsVO objPatEMRDetailsVO in BizActionObj.PatientHistoryDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientHistoryDetailsForIPD");
                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, BizActionObj.LinkServer);
                    if (BizActionObj.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command1, "PatientEMRdata", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, BizActionObj.Status);
                    dbServer.AddInParameter(command1, "ControlCaption", DbType.String, objPatEMRDetailsVO.ControlCaption);
                    dbServer.AddInParameter(command1, "ControlName", DbType.String, objPatEMRDetailsVO.ControlName);
                    dbServer.AddInParameter(command1, "ControlType", DbType.String, objPatEMRDetailsVO.ControlType);
                    dbServer.AddInParameter(command1, "ControlUnit", DbType.String, objPatEMRDetailsVO.ControlUnit);
                    dbServer.AddInParameter(command1, "ControlHeader", DbType.String, objPatEMRDetailsVO.Header);
                    dbServer.AddInParameter(command1, "ControlSection", DbType.String, objPatEMRDetailsVO.Section);
                    dbServer.AddInParameter(command1, "Doctorid", DbType.Int64, item.DoctorID);
                    dbServer.AddInParameter(command1, "Value", DbType.String, objPatEMRDetailsVO.Value);
                    dbServer.AddInParameter(command1, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatEMRDetailsVO.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    if (BizActionObj.SuccessStatus != 1 && BizActionObj.SuccessStatus != 2)
                        throw new Exception();
                }
                if (BizActionObj.HystroLaproImg != null && BizActionObj.HystroLaproImg.Count > 0)
                {
                    foreach (ListItems2 Img in BizActionObj.HystroLaproImg)
                    {
                        string ImgName = GetImageName(BizActionObj.TemplateID, item.PatientID, item.VisitID, UserVo.UserLoginInfo.UnitId, BizActionObj.IsOPDIPD);
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddImageForHistroScopyAndLaproScopy");
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command, "VisitID", DbType.Int64, item.VisitID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int32, BizActionObj.TemplateID);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command, "ImgPath", DbType.String, ImgName);
                        dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                        dbServer.AddInParameter(command, "EMRID", DbType.Int64, item.ID);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        MemoryStream ms = new MemoryStream(Img.Photo);
                        Image image = Image.FromStream(ms);
                        Image img = (Image)(new Bitmap(image, new Size(150, 120)));
                        ImageConverter converter = new ImageConverter();
                        MemoryStream mem = new MemoryStream();
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        img.Save(ImgSaveLocation + ImgName, jpgEncoder, myEncoderParameters);
                    }
                }
                BizActionObj.SuccessStatus = 1;
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return valueObject;
        }
        public string GetImageName(long TemplateID, long PatientID, long VisitID, long UnitID, Boolean OPDIPD)
        {
            string ImgName = null;
            Random random = new Random();
            long RandomNumber = random.Next(111111, 666666);
            if (TemplateID == 23)
            {
                ImgName = "HystroScopy-" + Convert.ToString(PatientID) + "-" + Convert.ToString(VisitID) + "-" + Convert.ToString(UnitID) + "-" + Convert.ToString(RandomNumber) + "-" + Convert.ToString(OPDIPD) + ".png";
            }
            else
            {
                ImgName = "LaproScopy-" + Convert.ToString(PatientID) + "-" + Convert.ToString(VisitID) + "-" + Convert.ToString(UnitID) + "-" + Convert.ToString(RandomNumber) + "-" + Convert.ToString(OPDIPD) + ".png";
            }
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckImageDupliaction");
                dbServer.AddInParameter(command, "ImgName", DbType.String, ImgName);
                int Status = Convert.ToInt32(dbServer.ExecuteScalar(command));
                if (Status == 0)
                {
                    GetImageName(TemplateID, PatientID, VisitID, UnitID, OPDIPD);
                }
                else
                {
                    return ImgName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            return "";
        }
        public string GetImageName(long TemplateID, long PatientID, long UnitID, DbConnection myConnection, DbTransaction myTransaction)
        {
            string ImgName = null;
            Random random = new Random();
            long RandomNumber = random.Next(111111, 666666);

            ImgName = "OTOtherDetails-" + Convert.ToString(PatientID) + "-" + Convert.ToString(UnitID) + "-" + Convert.ToString(RandomNumber) + ".png";

            DbConnection con = null;
            DbTransaction trans = null;

            if (myConnection != null)
                con = myConnection;
            else
                con = dbServer.CreateConnection();


            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            if (myTransaction != null)
                trans = myTransaction;
            else
                trans = con.BeginTransaction();

            try
            {
                //con = dbServer.CreateConnection();
                //if (con.State != ConnectionState.Open) con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckImageDupliactionForOT");
                dbServer.AddInParameter(command, "ImgName", DbType.String, ImgName);
                int Status = Convert.ToInt32(dbServer.ExecuteScalar(command, trans));
                if (Status == 0)
                {
                    GetImageName(TemplateID, PatientID, UnitID, con, trans);
                }
                else
                {
                    return ImgName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //if (con.State == ConnectionState.Open) con.Close();
                //con.Dispose();
            }
            return "";
        }
        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public override IValueObject GetPatientFollowUpList(IValueObject valueObject, clsUserVO UserVo)     // To Get FollowUpList on Dashboard 08032017
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientFollowUpDetailsBizActionVO BizActionObj = (clsGetPatientFollowUpDetailsBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUpList");           // CIMS_GetAppointmentMrNoList

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentId);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                //dbServer.AddInParameter(command, "UnRegistered", DbType.Boolean, BizActionObj.UnRegistered);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.InputPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.InputStartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.InputMaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);
                //dbServer.AddInParameter(command, "AppointStatus", DbType.Int32, BizActionObj.AppintmentStatusID);
                //dbServer.AddInParameter(command, "VisitMark", DbType.Int32, BizActionObj.VisitMark);
                //dbServer.AddInParameter(command, "AppType", DbType.Int64, BizActionObj.AppType);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo);
                //dbServer.AddInParameter(command, "SpecialRegistrationId", DbType.Int64, BizActionObj.SpecialRegistrationId);
                dbServer.AddInParameter(command, "@Status", DbType.Boolean, true);

                //long AppTypeID = 0;
                //AppTypeID = BizActionObj.AppintmentStatusID;

                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                ////  dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizActionObj.MrNo);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);




                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.FollowUpDetailsList == null)
                        BizActionObj.FollowUpDetailsList = new List<clsFollowUpVO>();
                    while (reader.Read())
                    {
                        clsFollowUpVO FollowUpVO = new clsFollowUpVO();

                        FollowUpVO.FollowUpID = Convert.ToInt64(reader["ID"]);
                        FollowUpVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        FollowUpVO.PatientUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));

                        //FollowUpVO.VisitId = (long)DALHelper.HandleDBNull(reader["VisitID"]);


                        FollowUpVO.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        FollowUpVO.MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])));
                        FollowUpVO.LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])));
                        FollowUpVO.FamilyName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"])));
                        //FollowUpVO.GenderId = (long)DALHelper.HandleDBNull(reader["GenderID"]);
                        //// FollowUpVO.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        //FollowUpVO.BloodId = (long)reader["BloodGroupID"];
                        //FollowUpVO.MaritalStatusId = (long)reader["MaritalStatusID"];
                        FollowUpVO.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"]));
                        FollowUpVO.ContactNo2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]));

                        FollowUpVO.MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        FollowUpVO.ResiNoCountryCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResiNoCountryCode"]));
                        FollowUpVO.ResiSTDCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResiSTDCode"]));

                        FollowUpVO.FaxNo = Convert.ToString(DALHelper.HandleDBNull(reader["FaxNo"]));
                        FollowUpVO.Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])));
                        FollowUpVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        FollowUpVO.DepartmentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentId"]));

                        FollowUpVO.AppointmentReasonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["AppointmentReasonID"]));
                        FollowUpVO.AppointmentReason = Convert.ToString(DALHelper.HandleDBNull(reader["AppointmentReason"]));
                        FollowUpVO.FollowUpDate = (DateTime?)DALHelper.HandleDate(reader["FollowUpDate"]);
                        //FollowUpVO.FromTime = (DateTime)reader["FromTime"];
                        //FollowUpVO.ToTime = (DateTime)reader["ToTime"];
                        FollowUpVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        //FollowUpVO.RegRemark = Security.base64Decode((string)DALHelper.HandleDBNull(reader["RegRemark"]));
                        FollowUpVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        FollowUpVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        FollowUpVO.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                        FollowUpVO.MrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        //FollowUpVO.VisitMark = (bool)DALHelper.HandleDBNull(reader["VisitMark"]);
                        //FollowUpVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        //FollowUpVO.SpecialRegistrationID = (Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecialRegistrationID"])));
                        //FollowUpVO.SpecialRegistration = (string)DALHelper.HandleDBNull(reader["SpecialRegistration"]);
                        FollowUpVO.AddedDateTime = (Convert.ToDateTime(DALHelper.HandleDate(reader["AddedDateTime"])));
                        //FollowUpVO.UpdatedDateTime = (Convert.ToDateTime(DALHelper.HandleDate(reader["UpdatedDateTime"])));
                        FollowUpVO.createdByName = (Convert.ToString(DALHelper.HandleDBNull(reader["createdByName"])));
                        FollowUpVO.ModifiedByName = (Convert.ToString(DALHelper.HandleDBNull(reader["ModifiedByName"])));
                        //FollowUpVO.MarkVisitStatus = (Convert.ToString(DALHelper.HandleDBNull(reader["MarkVisitStatus"])));
                        //FollowUpVO.AppointmentStatusNew = (Convert.ToString(DALHelper.HandleDBNull(reader["AppointmentStatusNew"])));
                        FollowUpVO.IsAge = (Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"])));
                        //FollowUpVO.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]));
                        if (FollowUpVO.IsAge == true)
                        {
                            FollowUpVO.DateOfBirthFromAge = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        else
                        {
                            FollowUpVO.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        }
                        ////* Added by - Ajit Jadhav  //* Added Date - 24/9/2016   //* Comments - get ReScheduling Reason
                        //FollowUpVO.Reschedule = Security.base64Decode((string)DALHelper.HandleDBNull(reader["ReSchedulingReason"]));
                        //FollowUpVO.Cancelschedule = Security.base64Decode((string)DALHelper.HandleDBNull(reader["AppCancelReason"]));   //* Added by - Ajit Jadhav  //* Added Date - 4/10/2016 

                        //// Added by ajit jadhav Date 12/10/2016 
                        //FollowUpVO.NationalityId = (long)DALHelper.HandleDBNull(reader["NationalityID"]);
                        ////***//---------------------

                       FollowUpVO.VisitDate = (Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"])));
                       FollowUpVO.OPDNo = (Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"])));
                       FollowUpVO.VisitID = (Convert.ToInt32(DALHelper.HandleDBNull(reader["VisitID"])));

                       FollowUpVO.ISAppointment = (Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISAppointment"]))); //***//
                       BizActionObj.FollowUpDetailsList.Add(FollowUpVO);
                    }


                }
                reader.NextResult();
                BizActionObj.OutputTotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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
    }
}
