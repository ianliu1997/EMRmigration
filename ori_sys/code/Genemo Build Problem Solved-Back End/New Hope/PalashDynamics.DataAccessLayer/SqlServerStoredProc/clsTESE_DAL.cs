namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
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

    public class clsTESE_DAL : clsBaseTESEDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsTESE_DAL()
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

        public override IValueObject AddUpdateTESE(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateTESEBizActionVO nvo = valueObject as clsAddUpdateTESEBizActionVO;
            List<clsTESEDetailsVO> list1 = new List<clsTESEDetailsVO>();
            List<clsTESEDetailsVO> tESEDetailsList = nvo.TESEDetailsList;
            clsCoupleVO coupleDetail = new clsCoupleVO();
            coupleDetail = nvo.TESE.CoupleDetail;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateTESE");
                if (nvo.TESE.ID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.TESE.ID);
                }
                else
                {
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, coupleDetail.CoupleId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, coupleDetail.CoupleUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientTypeID", DbType.Int64, coupleDetail.MalePatient.PatientTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationDate", DbType.DateTime, coupleDetail.CoupleRegDate);
                this.dbServer.AddInParameter(storedProcCommand, "CryoDate", DbType.Date, nvo.TESE.CryoDate);
                this.dbServer.AddInParameter(storedProcCommand, "CryoTime", DbType.Time, nvo.TESE.CryoTime);
                this.dbServer.AddInParameter(storedProcCommand, "EmbroLogistID", DbType.Int64, nvo.TESE.EmbroLogistID);
                this.dbServer.AddInParameter(storedProcCommand, "LabInchargeID", DbType.Int64, nvo.TESE.LabInchargeID);
                this.dbServer.AddInParameter(storedProcCommand, "Tissue", DbType.String, nvo.TESE.Tissue);
                this.dbServer.AddInParameter(storedProcCommand, "TissueSideID", DbType.Int64, nvo.TESE.TissueSideID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.TESE.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Synchronized", DbType.Boolean, false);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.TESE.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.TESE.UnitID = (long) this.dbServer.GetParameterValue(storedProcCommand, "UnitID");
                foreach (clsTESEDetailsVO svo in nvo.TESEDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateTESEDetails");
                    if (svo.ID > 0L)
                    {
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, svo.ID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    }
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "TESE_ID", DbType.Int64, nvo.TESE.ID);
                    this.dbServer.AddInParameter(command2, "TESE_UnitID", DbType.Int64, nvo.TESE.UnitID);
                    this.dbServer.AddInParameter(command2, "Tissue", DbType.String, svo.Tissue);
                    this.dbServer.AddInParameter(command2, "NoofVailsFrozen", DbType.Int64, svo.NoofVailsFrozen);
                    this.dbServer.AddInParameter(command2, "ContainerNumber", DbType.Int64, svo.ContainerNumber);
                    this.dbServer.AddInParameter(command2, "HolderNumber", DbType.Int64, svo.HolderNumber);
                    this.dbServer.AddInParameter(command2, "status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "No_of_VailsUsed", DbType.Int64, svo.No_of_VailsUsed);
                    this.dbServer.AddInParameter(command2, "No_of_VailsRemain", DbType.Int64, svo.No_of_VailsRemain);
                    this.dbServer.AddInParameter(command2, "RenewalDate", DbType.DateTime, svo.RenewalDate);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, nvo.TESE.AddedDateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetTESEDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTESEBizActionVO nvo = new clsGetTESEBizActionVO();
            nvo = valueObject as clsGetTESEBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetTESEDetails");
                nvo.TESEDeatailsList = new List<clsTESEDetailsVO>();
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.TESE.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.TESE.CoupleID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsTESEDetailsVO item = new clsTESEDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Tissue = Convert.ToString(DALHelper.HandleDBNull(reader["Tissue"])),
                            EmbroLogistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbroLogistID"])),
                            LabInchargeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabInchargeID"])),
                            EmbroLogist = Convert.ToString(DALHelper.HandleDBNull(reader["Embrologist"])),
                            LabIncharge = Convert.ToString(DALHelper.HandleDBNull(reader["labincharge"])),
                            No_of_VailsRemain = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["No_of_VailsRemain"])),
                            No_of_VailsUsed = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["No_of_VailsUsed"])),
                            NoofVailsFrozen = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NoofVailsFrozen"])),
                            TESE_ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TESE_ID"])),
                            TESE_UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TESE_UnitID"])),
                            CryoDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["CryoDate"]))),
                            CryoTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["CryoTime"]))),
                            ContainerNumber = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerNumber"])),
                            HolderNumber = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["HolderNumber"])),
                            RenewalDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["RenewalDate"]))),
                            TissueSideID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TissueSideID"]))
                        };
                        nvo.TESEDeatailsList.Add(item);
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

