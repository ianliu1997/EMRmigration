using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using System.Data.Common;
using System.Data;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    public class clsBedStatusDAL : clsBaseBedStatusDAL
    {
        private LogManager logManager = null;
        private Database dbServer = null;
        private clsBedStatusDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion
            }
            catch (Exception ex)
            {
            }
        }

        public override IValueObject ViewBedStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDBedStatusBizActionVO BizActionObj = valueObject as clsIPDBedStatusBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBedStatus");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.BedStatus.UnitID);
                dbServer.AddInParameter(command, "WardID", DbType.Int64, BizActionObj.BedStatus.WardID);
                if (!BizActionObj.BedStatus.IsOccupiedBoth)
                {
                    dbServer.AddInParameter(command, "IsOccupied", DbType.Boolean, BizActionObj.BedStatus.Occupied);
                }
                if (!BizActionObj.BedStatus.IsUnderMaintanence)
                {
                    dbServer.AddInParameter(command, "IsUnderMaintanance", DbType.Boolean, BizActionObj.BedStatus.IsUnderMaintanence);
                }

                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.BedStatus.ClassID);
                if (BizActionObj.BedStatus.IsNonCensus != "")
                    dbServer.AddInParameter(command, "IsNonCensus", DbType.String, BizActionObj.BedStatus.IsNonCensus);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.BedStatusList == null)
                        BizActionObj.BedStatusList = new List<clsIPDBedStatusVO>();
                    while (reader.Read())
                    {
                        clsIPDBedStatusVO objListVO = new clsIPDBedStatusVO();

                        objListVO.BedID = Convert.ToInt64(reader["ID"]);
                        objListVO.BedUnitID = Convert.ToInt64(reader["UnitID"]);
                        objListVO.BedDescription = Convert.ToString(reader["Description"]);
                        objListVO.WardID = Convert.ToInt64(reader["WardId"]);
                        //objListVO.Occupied = Convert.ToBoolean(reader["Occupied"]);
                        objListVO.IsUnderMaintanence = Convert.ToBoolean(reader["IsUnderMaintanence"]);
                        objListVO.IsDischarged = Convert.ToBoolean(reader["IsDischarged"]);
                        objListVO.BedCategoryId = Convert.ToInt64(reader["BedCategoryId"]);

                        objListVO.WardID = Convert.ToInt64(reader["WardId"]);
                        // objListVO.IsSecondaryBed = Convert.ToBoolean(reader["IsSecondaryBed"]);
                        objListVO.FromBed = Convert.ToInt64(reader["FromBedID"]);
                        objListVO.IsReserved = Convert.ToBoolean(reader["IsReserved"]);
                        objListVO.IsSecondaryBed = Convert.ToBoolean(reader["IsSecondary"]);
                        objListVO.IsClosed = Convert.ToBoolean(reader["IsClosed"]);

                        //  objListVO.PatientID = Convert.ToInt64(reader["PatientID"]);
                        // objListVO.PatientUnitID = Convert.ToInt64(reader["PatientUnitID"]);
                        // objListVO.MRNO = Convert.ToString(reader["MRNO"]);


                        BizActionObj.BedStatusList.Add(objListVO);
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

        public override IValueObject GetWardByFloor(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWardByFloorBizActionVO BizActionObj = valueObject as clsGetWardByFloorBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_WardByFloor");

                dbServer.AddInParameter(command, "FloorID", DbType.Int64, BizActionObj.Floor.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.Floor.UnitID);
                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.Floor.ClassID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.WardList == null)
                        BizActionObj.WardList = new List<clsIPDBedStatusVO>();
                    while (reader.Read())
                    {
                        clsIPDBedStatusVO objListVO = new clsIPDBedStatusVO();
                        objListVO.ID = Convert.ToInt64(reader["ID"]);
                        objListVO.Description = Convert.ToString(reader["Description"]);
                        BizActionObj.WardList.Add(objListVO);
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
    }
}
