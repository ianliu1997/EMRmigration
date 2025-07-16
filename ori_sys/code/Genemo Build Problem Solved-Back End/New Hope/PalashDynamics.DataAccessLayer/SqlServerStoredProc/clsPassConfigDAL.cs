namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using System;
    using System.Data;
    using System.Data.Common;

    public class clsPassConfigDAL : clsBasePassConfigDAL
    {
        private Database dbServer;
        private LogManager logmanager;

        private clsPassConfigDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logmanager == null)
                {
                    this.logmanager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetPassConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPassConfigBizActionVO nvo = valueObject as clsGetPassConfigBizActionVO;
            try
            {
                if (nvo.PassConfig == null)
                {
                    nvo.PassConfig = new clsPassConfigurationVO();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPassConfig");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    reader.Read();
                    nvo.PassConfig.MinPasswordLength = (short) DALHelper.HandleDBNull(reader["MinPasswordLength"]);
                    nvo.PassConfig.MaxPasswordLength = (short) DALHelper.HandleDBNull(reader["MaxPasswordLength"]);
                    nvo.PassConfig.MinPasswordAge = (short) DALHelper.HandleDBNull(reader["MinPasswordAge"]);
                    nvo.PassConfig.MaxPasswordAge = (short) DALHelper.HandleDBNull(reader["MaxPasswordAge"]);
                    nvo.PassConfig.AtLeastOneDigit = (bool) DALHelper.HandleDBNull(reader["AtLeastOneDigit"]);
                    nvo.PassConfig.AtLeastOneLowerCaseChar = (bool) DALHelper.HandleDBNull(reader["AtLeastOneLowerCaseChar"]);
                    nvo.PassConfig.AtLeastOneUpperCaseChar = (bool) DALHelper.HandleDBNull(reader["AtLeastOneUpperCaseChar"]);
                    nvo.PassConfig.AtLeastOneSpecialChar = (bool) DALHelper.HandleDBNull(reader["AtLeastOneSpecialChar"]);
                    nvo.PassConfig.NoOfPasswordsToRemember = (short) DALHelper.HandleDBNull(reader["NoOfPasswordsToRemember"]);
                    nvo.PassConfig.AccountLockThreshold = (short) DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                    nvo.PassConfig.AccountLockDuration = (float) ((double) DALHelper.HandleDBNull(reader["AccountLockDuration"]));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdatePasswordConfig(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPasswordConfigBizActionVO nvo = valueObject as clsAddPasswordConfigBizActionVO;
            try
            {
                clsPassConfigurationVO passwordConfig = nvo.PasswordConfig;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePasswordConfig");
                this.dbServer.AddInParameter(storedProcCommand, "MinPasswordLength", DbType.Int16, passwordConfig.MinPasswordLength);
                this.dbServer.AddInParameter(storedProcCommand, "MaxPasswordLength", DbType.Int16, passwordConfig.MaxPasswordLength);
                this.dbServer.AddInParameter(storedProcCommand, "MinPasswordAge", DbType.Int16, passwordConfig.MinPasswordAge);
                this.dbServer.AddInParameter(storedProcCommand, "MaxPasswordAge", DbType.Int16, passwordConfig.MaxPasswordAge);
                this.dbServer.AddInParameter(storedProcCommand, "AtLeastOneDigit", DbType.Boolean, passwordConfig.AtLeastOneDigit);
                this.dbServer.AddInParameter(storedProcCommand, "AtLeastOneLowerCaseChar", DbType.Boolean, passwordConfig.AtLeastOneLowerCaseChar);
                this.dbServer.AddInParameter(storedProcCommand, "AtleastOneUpperCaseChar", DbType.Boolean, passwordConfig.AtLeastOneUpperCaseChar);
                this.dbServer.AddInParameter(storedProcCommand, "AtleastOneSpecialChar", DbType.Boolean, passwordConfig.AtLeastOneSpecialChar);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfPasswordsToRemember", DbType.Int16, passwordConfig.NoOfPasswordsToRemember);
                this.dbServer.AddInParameter(storedProcCommand, "AccountLockThreshold", DbType.Int16, passwordConfig.AccountLockThreshold);
                this.dbServer.AddInParameter(storedProcCommand, "AccountLockDuration", DbType.Double, passwordConfig.AccountLockDuration);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }
    }
}

