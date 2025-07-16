namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsItemConversionsDAL : clsBaseItemConversionsDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsItemConversionsDAL()
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
                throw;
            }
        }

        public override IValueObject AddUpdateConversionFactorMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddUpdateItemConversionFactorListBizActionVO nvo = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                transaction = null;
                connection.Open();
                transaction = connection.BeginTransaction();
                nvo = valueObject as clsAddUpdateItemConversionFactorListBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateConversionFactorMaster");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.UOMConversionVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsModify", DbType.Boolean, nvo.IsModify);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.UOMConversionVO.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "FromUOMID", DbType.Int64, nvo.UOMConversionVO.FromUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "ToUOMID", DbType.Int64, nvo.UOMConversionVO.ToUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "ConversionFactor", DbType.Decimal, nvo.UOMConversionVO.ConversionFactor);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject DeleteConversionFactorMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            clsAddUpdateItemConversionFactorListBizActionVO nvo = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                nvo = valueObject as clsAddUpdateItemConversionFactorListBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteConversionFactorMaster");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.UOMConversionVO.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "FromUOMID", DbType.Int64, nvo.UOMConversionVO.FromUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "ToUOMID", DbType.Int64, nvo.UOMConversionVO.ToUOMID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection = null;
            }
            return nvo;
        }

        public override IValueObject GetItemConversionsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemConversionFactorListBizActionVO nvo = valueObject as clsGetItemConversionFactorListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemConversionsListByItemID");
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.UOMConversionList == null)
                    {
                        nvo.UOMConversionList = new List<clsConversionsVO>();
                    }
                    while (reader.Read())
                    {
                        clsConversionsVO item = new clsConversionsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            FromUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromUOMID"])),
                            ToUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToUOMID"])),
                            FromUOM = Convert.ToString(DALHelper.HandleDBNull(reader["FromUOM"])),
                            ToUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ToUOM"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            MainConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.UOMConversionList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    if (nvo.UOMConvertList == null)
                    {
                        nvo.UOMConvertList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.UOMConvertList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }
    }
}

