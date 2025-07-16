namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsAutoChargesDAL : clsBaseAutoChargesDAL
    {
        private Database dbServer;

        private clsAutoChargesDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddAutoChargesServiceList(IValueObject valueObject, clsUserVO userVO)
        {
            clsIPDAutoChargesVO svo = new clsIPDAutoChargesVO();
            clsAddIPDAutoChargesServiceListBizActionVO nvo = valueObject as clsAddIPDAutoChargesServiceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteAutoCharges");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                foreach (clsIPDAutoChargesVO svo2 in nvo.ChargesMasterList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddAutoChargesServiceList");
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitId);
                    this.dbServer.AddInParameter(command2, "ServiceId", DbType.Int64, svo2.ServiceId);
                    this.dbServer.ExecuteNonQuery(command2);
                }
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    svo.PrimaryKeyViolationError = true;
                }
                else
                {
                    svo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject DeleteService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteServiceBizActionVO nvo = valueObject as clsDeleteServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteService");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.Id);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAutoChargesServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDAutoChargesServiceListBizActionVO nvo = (clsGetIPDAutoChargesServiceListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetAutoChargesServiceList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GetChargesMasterList == null)
                    {
                        nvo.GetChargesMasterList = new List<clsIPDAutoChargesVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDAutoChargesVO item = new clsIPDAutoChargesVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.GetChargesMasterList.Add(item);
                    }
                }
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

