using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RateContract;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.RateContract;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.RateContract
{
    public class clsRateContractDAL : clsBaseRateContractDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsRateContractDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override ValueObjects.IValueObject GetRateContractAgainstSupplierAndItem(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsGetRateContractAgainstSupplierAndItemBizActionVO BizActionObj = valueObject as clsGetRateContractAgainstSupplierAndItemBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            DbDataReader reader = null;
            con.Open();
            trans = con.BeginTransaction();
            BizActionObj.RateContractMasterList = new List<clsRateContractMasterVO>();
            BizActionObj.RateContractItemDetailsList = new List<clsRateContractItemDetailsVO>();
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRateContractAgainstSupplierAndName");
            try
            {
                dbServer.AddInParameter(command, "SupplierID", DbType.String, BizActionObj.SupplierID);
                dbServer.AddInParameter(command, "ItemID", DbType.String, BizActionObj.ItemIDs);
                dbServer.AddInParameter(command, "LoginUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command, trans);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsRateContractDetailsVO objRateContractList = new clsRateContractDetailsVO();
                        objRateContractList.ContractID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractID"]));
                        objRateContractList.ContractUnitId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractUnitID"]));
                        objRateContractList.ContractCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objRateContractList.ContractDescription = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objRateContractList.Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"]));
                        objRateContractList.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        objRateContractList.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        objRateContractList.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        objRateContractList.SelectedUOM = new ValueObjects.MasterListItem { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"])), Description = objRateContractList.TransUOM };
                        objRateContractList.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));  //stock CF
                        objRateContractList.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objRateContractList.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objRateContractList.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objRateContractList.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseRate"]));
                        objRateContractList.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseMRP"]));
                        objRateContractList.DiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DiscountPercent"]));
                        objRateContractList.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        objRateContractList.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objRateContractList.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        BizActionObj.RateContractListNew.Add(objRateContractList);


                        #region Old Code
                        //clsRateContractMasterVO objRateContractMaster = new clsRateContractMasterVO();
                        //objRateContractMaster.RateContractID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractID"]));
                        //objRateContractMaster.RateContractUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractUnitID"]));
                        //objRateContractMaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        //objRateContractMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        //objRateContractMaster.FromDate = Convert.ToDateTime(DALHelper.HandleDate(reader["FromDate"]));
                        //objRateContractMaster.ToDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ToDate"]));
                        //objRateContractMaster.ContractValue = Convert.ToDouble(DALHelper.HandleDBNull(reader["ContractValue"]));
                        //BizActionObj.RateContractMasterList.Add(objRateContractMaster);
                        //clsRateContractItemDetailsVO objRateContractItemDetails = new clsRateContractItemDetailsVO();
                        //clsRateContractItemDetailsVO objDetails = new clsRateContractItemDetailsVO();
                        //objDetails.MinQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["MinQuantity"]));
                        //objDetails.MaxQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["MaxQuantity"]));
                        //objDetails.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        //objDetails.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        //objDetails.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        //objDetails.UnlimitedQuantity = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["UnlimitedQuantity"]));
                        //objDetails.DiscountPercent = Convert.ToDouble(reader["DiscountPercent"]);
                        //objDetails.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));
                        //objDetails.Condition = Convert.ToString(DALHelper.HandleDBNull(reader["Condition"]));
                        //objDetails.RateContractID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractID"]));
                        //objDetails.RateContractUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContractUnitID"]));
                        //BizActionObj.RateContractItemDetailsList.Add(objDetails);
                        #endregion
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsRateContractDetailsVO objPOBestPriceList = new clsRateContractDetailsVO();
                        objPOBestPriceList.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));
                        objPOBestPriceList.BestBaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BestBaseRate"]));
                        objPOBestPriceList.BestBaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BestBaseMRP"]));
                        objPOBestPriceList.IsItemInRateContract = false;
                        BizActionObj.POBestPriceList.Add(objPOBestPriceList);
                    }
                }               

                if (BizActionObj.RateContractListNew != null && BizActionObj.POBestPriceList != null)
                {
                    foreach (var itemRateContract in BizActionObj.RateContractListNew.ToList())
                    {
                        BizActionObj.POBestPriceList.Where(z => z.ItemID == itemRateContract.ItemID).Select(z => z.IsItemInRateContract = true).ToList();
                    }
                }

            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return BizActionObj;
        }
    }
}
