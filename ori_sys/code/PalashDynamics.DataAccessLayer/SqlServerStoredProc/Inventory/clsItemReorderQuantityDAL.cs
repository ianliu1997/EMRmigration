using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    class clsItemReorderQuantityDAL:clsBaseItemReorderQuantityDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsItemReorderQuantityDAL()
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

        public override IValueObject GetItemReorderQuantity(IValueObject ValueObject, clsUserVO UserVo)
        {
            clsGetItemReorderQuantityBizActionVO objBizAction = ValueObject as clsGetItemReorderQuantityBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetReorderItemList");
               
                dbServer.AddInParameter(command, "IsPaging", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "NoOfRecordShow", DbType.Int64, objBizAction.NoOfRecords);
                dbServer.AddInParameter(command, "StartIndex", DbType.Int64, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizAction.store);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizAction.SupplierID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.Unit);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objBizAction.UserID);
                dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.ItemName);
                dbServer.AddOutParameter(command, "TotalRow", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "OrderBy", DbType.Int64, objBizAction.IsOrderBy);
               
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemReorderList == null)
                        objBizAction.ItemReorderList = new List<clsItemReorderDetailVO>();
                    while (reader.Read())
                    {
                        clsItemReorderDetailVO objItem = new clsItemReorderDetailVO();
                        objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem.ItemReorderQty = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReorderQnt"]));
                        objItem.AvailableStock = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStock"]));
                    //    objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objItem.PUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                        objItem.SUM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objItem.ConversionFactor=Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItem.StockingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        objBizAction.ItemReorderList.Add(objItem);
                    }
                }
                reader.NextResult();
               objBizAction.TotalRow = (long)dbServer.GetParameterValue(command, "TotalRow");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return ValueObject;
        }
    }
}
