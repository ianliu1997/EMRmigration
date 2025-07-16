namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
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

    internal class clsItemReorderQuantityDAL : clsBaseItemReorderQuantityDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsItemReorderQuantityDAL()
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

        public override IValueObject GetItemReorderQuantity(IValueObject ValueObject, clsUserVO UserVo)
        {
            clsGetItemReorderQuantityBizActionVO nvo = ValueObject as clsGetItemReorderQuantityBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReorderItemList");
                this.dbServer.AddInParameter(storedProcCommand, "IsPaging", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfRecordShow", DbType.Int64, nvo.NoOfRecords);
                this.dbServer.AddInParameter(storedProcCommand, "StartIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.store);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.Unit);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemName);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRow", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "OrderBy", DbType.Int64, nvo.IsOrderBy);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemReorderList == null)
                    {
                        nvo.ItemReorderList = new List<clsItemReorderDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemReorderDetailVO item = new clsItemReorderDetailVO {
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                            ItemReorderQty = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ReorderQnt"]))),
                            AvailableStock = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            PUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"])),
                            SUM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            StockingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]))
                        };
                        nvo.ItemReorderList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRow = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRow");
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
            return ValueObject;
        }
    }
}

