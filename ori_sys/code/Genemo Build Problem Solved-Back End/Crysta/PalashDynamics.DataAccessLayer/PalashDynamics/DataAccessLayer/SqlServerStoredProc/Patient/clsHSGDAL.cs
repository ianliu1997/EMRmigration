namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Patient
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IVFPlanTherapy;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsHSGDAL : clsBaseHSGDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsHSGDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
        }

        public override IValueObject AddUpdateHSG(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateHSGBizActionVO nvo = valueObject as clsAddUpdateHSGBizActionVO;
            clsCoupleVO coupleDetail = new clsCoupleVO();
            coupleDetail = nvo.HSGDetails.CoupleDetail;
            try
            {
                clsHSGVO hSGDetails = nvo.HSGDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateHSG");
                if (nvo.HSGDetails.ID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.HSGDetails.ID);
                }
                else
                {
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, coupleDetail.CoupleId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, coupleDetail.CoupleUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, hSGDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientTypeID", DbType.Int64, coupleDetail.MalePatient.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Uterus", DbType.String, hSGDetails.Uterus);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, hSGDetails.HSGDate);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, hSGDetails.HSGTime);
                this.dbServer.AddInParameter(storedProcCommand, "Image", DbType.Binary, hSGDetails.Image);
                this.dbServer.AddInParameter(storedProcCommand, "cavity", DbType.Boolean, hSGDetails.cavity);
                this.dbServer.AddInParameter(storedProcCommand, "Patent_Tube", DbType.Boolean, hSGDetails.Patent_Tube);
                this.dbServer.AddInParameter(storedProcCommand, "Blocked_tube", DbType.Boolean, hSGDetails.Blocked_tube);
                this.dbServer.AddInParameter(storedProcCommand, "Cornul_blockage", DbType.Boolean, hSGDetails.Cornul_blockage);
                this.dbServer.AddInParameter(storedProcCommand, "Isthmic_Blockage", DbType.Boolean, hSGDetails.Isthmic_Blockage);
                this.dbServer.AddInParameter(storedProcCommand, "Ampullary_Blockage", DbType.Boolean, hSGDetails.Ampullary_Blockage);
                this.dbServer.AddInParameter(storedProcCommand, "Fimbrial_Blockage", DbType.Boolean, hSGDetails.Fimbrial_Blockage);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, hSGDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "Hydrosalplnx", DbType.Boolean, hSGDetails.Hydrosalplnx);
                this.dbServer.AddInParameter(storedProcCommand, "Other_Patho", DbType.String, hSGDetails.Other_Patho);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, hSGDetails.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy ", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, hSGDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, hSGDetails.Title);
                this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, hSGDetails.AttachedFileName);
                this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, hSGDetails.AttachedFileContent);
                this.dbServer.AddInParameter(storedProcCommand, "IsDeleted", DbType.Boolean, hSGDetails.IsDeleted);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.HSGDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetHSGDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetHSGBizActionVO nvo = new clsGetHSGBizActionVO();
            nvo = valueObject as clsGetHSGBizActionVO;
            clsHSGVO hSGDetails = nvo.HSGDetails;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVE_GetHSG");
                nvo.HSGList = new List<clsHSGVO>();
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, hSGDetails.PatientID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsHSGVO shsgvo1 = new clsHSGVO();
                        nvo.HSGDetails.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        nvo.HSGDetails.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        nvo.HSGDetails.Uterus = Convert.ToString(DALHelper.HandleDBNull(reader["Uterus"]));
                        nvo.HSGDetails.HSGDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.HSGDetails.HSGTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])));
                        nvo.HSGDetails.Image = (byte[]) DALHelper.HandleDBNull(reader["Image"]);
                        nvo.HSGDetails.cavity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["cavity"]));
                        nvo.HSGDetails.Patent_Tube = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Patent_Tube"]));
                        nvo.HSGDetails.Blocked_tube = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Blocked_tube"]));
                        nvo.HSGDetails.Cornul_blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Cornul_blockage"]));
                        nvo.HSGDetails.Isthmic_Blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isthmic_Blockage"]));
                        nvo.HSGDetails.Ampullary_Blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ampullary_Blockage"]));
                        nvo.HSGDetails.Fimbrial_Blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Fimbrial_Blockage"]));
                        nvo.HSGDetails.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        nvo.HSGDetails.Hydrosalplnx = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hydrosalplnx"]));
                        nvo.HSGDetails.Other_Patho = Convert.ToString(DALHelper.HandleDBNull(reader["Other_Patho"]));
                        nvo.HSGDetails.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.HSGDetails.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        nvo.HSGDetails.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        nvo.HSGDetails.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        nvo.HSGDetails.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        nvo.HSGDetails.IsDeleted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDeleted"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }
    }
}

