namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsBedStatusDAL : clsBaseBedStatusDAL
    {
        private LogManager logManager;
        private Database dbServer;

        private clsBedStatusDAL()
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

        public override IValueObject GetWardByFloor(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWardByFloorBizActionVO nvo = valueObject as clsGetWardByFloorBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_WardByFloor");
                this.dbServer.AddInParameter(storedProcCommand, "FloorID", DbType.Int64, nvo.Floor.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.Floor.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.Floor.ClassID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.WardList == null)
                    {
                        nvo.WardList = new List<clsIPDBedStatusVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedStatusVO item = new clsIPDBedStatusVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            Description = Convert.ToString(reader["Description"])
                        };
                        nvo.WardList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject ViewBedStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDBedStatusBizActionVO nvo = valueObject as clsIPDBedStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBedStatus");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.BedStatus.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, nvo.BedStatus.WardID);
                if (!nvo.BedStatus.IsOccupiedBoth)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsOccupied", DbType.Boolean, nvo.BedStatus.Occupied);
                }
                if (!nvo.BedStatus.IsUnderMaintanence)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsUnderMaintanance", DbType.Boolean, nvo.BedStatus.IsUnderMaintanence);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.BedStatus.ClassID);
                if (nvo.BedStatus.IsNonCensus != "")
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsNonCensus", DbType.String, nvo.BedStatus.IsNonCensus);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedStatusList == null)
                    {
                        nvo.BedStatusList = new List<clsIPDBedStatusVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedStatusVO item = new clsIPDBedStatusVO {
                            BedID = Convert.ToInt64(reader["ID"]),
                            BedUnitID = Convert.ToInt64(reader["UnitID"]),
                            BedDescription = Convert.ToString(reader["Description"]),
                            WardID = Convert.ToInt64(reader["WardId"]),
                            IsUnderMaintanence = Convert.ToBoolean(reader["IsUnderMaintanence"]),
                            IsDischarged = Convert.ToBoolean(reader["IsDischarged"]),
                            BedCategoryId = Convert.ToInt64(reader["BedCategoryId"]),
                            FromBed = Convert.ToInt64(reader["FromBedID"]),
                            IsReserved = Convert.ToBoolean(reader["IsReserved"]),
                            IsSecondaryBed = Convert.ToBoolean(reader["IsSecondary"]),
                            IsClosed = Convert.ToBoolean(reader["IsClosed"])
                        };
                        nvo.BedStatusList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

