namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Data;
    using System.Data.Common;

    public class clsBedUnReservationDAL : clsBaseUnReservationDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsBedUnReservationDAL()
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
            catch (Exception)
            {
            }
        }

        private clsAddIPDBedUnReservationBizActionVO AddBedUnReservationDetails(clsAddIPDBedUnReservationBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                foreach (clsIPDBedReservationVO nvo2 in BizActionObj.BedUnResDetails.BedList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddBedUnReservation");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "BedReservationID", DbType.Int64, nvo2.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "BedReservationUnitID", DbType.Int64, nvo2.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo2.UnResRemark);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    BizActionObj.BedUnResDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddIPDBedUnReservation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIPDBedUnReservationBizActionVO bizActionObj = valueObject as clsAddIPDBedUnReservationBizActionVO;
            bizActionObj = this.AddBedUnReservationDetails(bizActionObj, UserVo);
            return valueObject;
        }
    }
}

