namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.RateContract
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RateContract;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.RateContract;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsRateContractDAL : clsBaseRateContractDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsRateContractDAL()
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

        public override IValueObject GetRateContractAgainstSupplierAndItem(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRateContractAgainstSupplierAndItemBizActionVO nvo = valueObject as clsGetRateContractAgainstSupplierAndItemBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            DbDataReader reader = null;
            connection.Open();
            transaction = connection.BeginTransaction();
            nvo.RateContractMasterList = new List<clsRateContractMasterVO>();
            nvo.RateContractItemDetailsList = new List<clsRateContractItemDetailsVO>();
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRateContractAgainstSupplierAndName");
            try
            {
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.String, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.String, nvo.ItemIDs);
                this.dbServer.AddInParameter(storedProcCommand, "LoginUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand, transaction);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsRateContractDetailsVO svo = new clsRateContractDetailsVO {
                            ContractID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractID"])),
                            ContractUnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractUnitID"])),
                            ContractCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ContractDescription = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]))
                        };
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])),
                            Description = svo.TransUOM
                        };
                        svo.SelectedUOM = item;
                        svo.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        svo.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        svo.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        svo.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        svo.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseRate"]));
                        svo.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseMRP"]));
                        svo.DiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DiscountPercent"]));
                        svo.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        svo.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        svo.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        nvo.RateContractListNew.Add(svo);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsRateContractDetailsVO item = new clsRateContractDetailsVO {
                            ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"])),
                            BestBaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BestBaseRate"])),
                            BestBaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BestBaseMRP"])),
                            IsItemInRateContract = false
                        };
                        nvo.POBestPriceList.Add(item);
                    }
                }
                if ((nvo.RateContractListNew != null) && (nvo.POBestPriceList != null))
                {
                    using (List<clsRateContractDetailsVO>.Enumerator enumerator = nvo.RateContractListNew.ToList<clsRateContractDetailsVO>().GetEnumerator())
                    {
                        Func<clsRateContractDetailsVO, bool> predicate = null;
                        while (enumerator.MoveNext())
                        {
                            clsRateContractDetailsVO itemRateContract = enumerator.Current;
                            if (predicate == null)
                            {
                                predicate = z => z.ItemID == itemRateContract.ItemID;
                            }
                            nvo.POBestPriceList.Where<clsRateContractDetailsVO>(predicate).Select<clsRateContractDetailsVO, bool>(delegate (clsRateContractDetailsVO z) {
                                bool flag;
                                z.IsItemInRateContract = flag = true;
                                return flag;
                            }).ToList<bool>();
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
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

