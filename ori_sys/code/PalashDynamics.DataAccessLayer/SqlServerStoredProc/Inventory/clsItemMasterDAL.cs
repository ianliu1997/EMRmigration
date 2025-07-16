using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.Data.Common;
using System.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Inventory.BarCode;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;
using System.Reflection;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    public class clsItemMasterDAL : clsBaseItemMasterDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        string IsApprovedDirect = string.Empty;  //for Direct Approve criteria
        #endregion


        private clsItemMasterDAL()
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

                IsApprovedDirect = System.Configuration.ConfigurationManager.AppSettings["IsApprovedDirect"];
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public override IValueObject AddItemMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemMasterBizActionVO objItem = valueObject as clsAddItemMasterBizActionVO;
            try
            {
                DbCommand command;
                clsItemMasterVO objItemVO = objItem.ItemMatserDetails;
                if (objItemVO.EditMode == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateItemMaster");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_InsertItemMaster");
                    dbServer.AddOutParameter(command, "Id", DbType.Int64, 0);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "BrandName", DbType.String, objItemVO.BrandName);
                dbServer.AddInParameter(command, "Strength", DbType.String, objItemVO.Strength);
                dbServer.AddInParameter(command, "ItemName", DbType.String, objItemVO.ItemName);
                dbServer.AddInParameter(command, "MoleculeName", DbType.Int64, objItemVO.MoleculeName);
                dbServer.AddInParameter(command, "ItemGroup", DbType.Int64, objItemVO.ItemGroup);
                dbServer.AddInParameter(command, "ItemCategory", DbType.Int64, objItemVO.ItemCategory);
                dbServer.AddInParameter(command, "DispencingType", DbType.Int64, objItemVO.DispencingType);
                dbServer.AddInParameter(command, "StoreageType", DbType.Int64, objItemVO.StoreageType);
                dbServer.AddInParameter(command, "PregClass", DbType.Int64, objItemVO.PregClass);
                dbServer.AddInParameter(command, "TherClass", DbType.Int64, objItemVO.TherClass);
                dbServer.AddInParameter(command, "MfgBy", DbType.Int64, objItemVO.MfgBy);
                dbServer.AddInParameter(command, "MrkBy", DbType.Int64, objItemVO.MrkBy);
                dbServer.AddInParameter(command, "PUM", DbType.Int64, objItemVO.PUM);
                dbServer.AddInParameter(command, "SUM", DbType.Int64, objItemVO.SUM);
                dbServer.AddInParameter(command, "ConversionFactor", DbType.String, objItemVO.ConversionFactor);

                dbServer.AddInParameter(command, "BaseUM", DbType.Int64, objItemVO.BaseUM);
                dbServer.AddInParameter(command, "SellingUM", DbType.Int64, objItemVO.SellingUM);
                dbServer.AddInParameter(command, "ConvFactStockBase", DbType.String, objItemVO.ConvFactStockBase);
                dbServer.AddInParameter(command, "ConvFactBaseSell", DbType.String, objItemVO.ConvFactBaseSale);
                dbServer.AddInParameter(command, "ItemExpiredInDays", DbType.Int64, objItemVO.ItemExpiredInDays);

                dbServer.AddInParameter(command, "Route", DbType.Int64, objItemVO.Route);
                dbServer.AddInParameter(command, "PurchaseRate", DbType.Decimal, objItemVO.PurchaseRate);
                dbServer.AddInParameter(command, "MRP", DbType.Decimal, objItemVO.MRP);
                dbServer.AddInParameter(command, "VatPer", DbType.Decimal, objItemVO.VatPer);
                dbServer.AddInParameter(command, "ReorderQnt", DbType.Int32, objItemVO.ReorderQnt);
                dbServer.AddInParameter(command, "BatchesRequired", DbType.Boolean, objItemVO.BatchesRequired);
                dbServer.AddInParameter(command, "InclusiveOfTax", DbType.Boolean, objItemVO.InclusiveOfTax);
                dbServer.AddInParameter(command, "DiscountOnSale", DbType.Double, objItemVO.DiscountOnSale);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdateddOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddInParameter(command, "DispencingTypeString", DbType.String, objItemVO.DispencingTypeString);
                dbServer.AddInParameter(command, "StorageDegree", DbType.Decimal, objItemVO.StorageDegree);
                dbServer.AddInParameter(command, "MinStock", DbType.Decimal, objItemVO.MinStock);
                dbServer.AddInParameter(command, "ItemMarginID", DbType.Int64, objItemVO.ItemMarginID);
                dbServer.AddInParameter(command, "ItemMovementID", DbType.Int64, objItemVO.ItemMovementID);
                dbServer.AddInParameter(command, "Margin", DbType.Decimal, objItemVO.Margin);
                dbServer.AddInParameter(command, "HighestRetailPrice", DbType.Decimal, objItemVO.HighestRetailPrice);
                dbServer.AddInParameter(command, "BarCode", DbType.String, objItemVO.BarCode);
                dbServer.AddInParameter(command, "IsABC", DbType.Boolean, objItemVO.IsABC);
                dbServer.AddInParameter(command, "IsFNS", DbType.Boolean, objItemVO.IsFNS);
                dbServer.AddInParameter(command, "IsVED", DbType.Boolean, objItemVO.IsVED);
                dbServer.AddInParameter(command, "StrengthUnitTypeID", DbType.Int64, objItemVO.StrengthUnitTypeID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                dbServer.AddInParameter(command, "HSNCodesID", DbType.Int64, objItemVO.HSNCodesID);//Added By Bhushanp For GST 19062017
                //-----***------//
                dbServer.AddInParameter(command, "StaffDiscount", DbType.Double, objItemVO.StaffDiscount);
                dbServer.AddInParameter(command, "WalkinDiscount", DbType.Double, objItemVO.WalkinDiscount);
                dbServer.AddInParameter(command, "RegisteredPatientsDiscount", DbType.Double, objItemVO.RegisteredPatientsDiscount);
                //-------
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                objItem.ItemID = (long)dbServer.GetParameterValue(command, "Id");
                objItem.ItemMatserDetails = GetItem(objItem.ItemID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objItem;
        }

        public clsItemMasterVO GetItem(long itemId)
        {
            DbDataReader reader = null;
            try
            {
                DbCommand command;

                clsItemMasterVO objItemMaster = new clsItemMasterVO();
                command = dbServer.GetStoredProcCommand("GetItemById");
                dbServer.AddInParameter(command, "ItemId", DbType.Int64, itemId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                while (reader.Read())
                {
                    objItemMaster.ItemCode = (string)(reader["ItemCode"].HandleDBNull() == null ? "" : reader["ItemCode"]);
                    objItemMaster.ItemName = (string)(reader["ItemName"].HandleDBNull() == null ? "" : reader["ItemName"]);
                    objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                    objItemMaster.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    objItemMaster.UnitID = (long)(reader["UnitID"].HandleDBNull() == null ? 0 : reader["UnitID"]);
                    objItemMaster.BrandName = (string)DALHelper.HandleDBNull(reader["BrandName"]);
                    objItemMaster.Strength = (string)DALHelper.HandleDBNull(reader["Strength"]);
                    objItemMaster.MoleculeName = (long)DALHelper.HandleDBNull(reader["MoleculeName"]);
                    objItemMaster.MoleculeNameString = (string)DALHelper.HandleDBNull(reader["MoleculeNameString"]);
                    objItemMaster.ItemGroup = (long)DALHelper.HandleDBNull(reader["ItemGroup"]);
                    objItemMaster.ItemGroupString = (string)DALHelper.HandleDBNull(reader["ItemGroupString"]);
                    objItemMaster.ItemGroupString = (string)(reader["ItemGroupString"].HandleDBNull() == null ? "" : reader["ItemGroupString"]);

                    objItemMaster.ItemCategory = (long)DALHelper.HandleDBNull(reader["ItemCategory"]);
                    objItemMaster.ItemCategoryString = (string)(reader["ItemCategoryString"].HandleDBNull() == null ? "" : reader["ItemCategoryString"]);
                    objItemMaster.DispencingType = reader["DispencingType"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["DispencingType"]);
                    objItemMaster.StoreageType = reader["StoreageType"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["StoreageType"]); //(long)(DALHelper.HandleDBNull(reader["StoreageType"])==null?0:;
                    objItemMaster.PregClass = reader["PregClass"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["PregClass"]);//(long)DALHelper.HandleDBNull(reader["PregClass"]);
                    objItemMaster.TherClass = reader["TherClass"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["TherClass"]); //(long)DALHelper.HandleDBNull(reader["TherClass"]);
                    objItemMaster.MfgBy = reader["MfgBy"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["MfgBy"]);//(long)DALHelper.HandleDBNull(reader["MfgBy"]);
                    objItemMaster.MfgByString = (string)(reader["MfgByString"].HandleDBNull() == null ? "" : reader["MfgByString"]);

                    objItemMaster.MrkBy = reader["MrkBy"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["MrkBy"]);//(long)DALHelper.HandleDBNull(reader["MrkBy"]);
                    objItemMaster.MrkByString = (string)(reader["MrkByString"].HandleDBNull() == null ? "" : reader["MrkByString"]);

                    objItemMaster.PUM = reader["PUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["PUM"]); //(long)DALHelper.HandleDBNull(reader["PUM"]);

                    objItemMaster.PUMString = (string)(reader["PUMString"].HandleDBNull() == null ? "" : reader["PUMString"]);

                    objItemMaster.SUM = reader["SUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["SUM"]);//(long)DALHelper.HandleDBNull(reader["SUM"]);
                    objItemMaster.ConversionFactor = reader["ConversionFactor"].HandleDBNull() == null ? "" : Convert.ToString(reader["ConversionFactor"]); //(string)DALHelper.HandleDBNull(reader["ConversionFactor"]);

                    objItemMaster.BaseUM = reader["BaseUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["BaseUM"]);
                    objItemMaster.SellingUM = reader["SellingUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["SellingUM"]);
                    objItemMaster.ConvFactStockBase = reader["ConvFactStockBase"].HandleDBNull() == null ? "" : Convert.ToString(reader["ConvFactStockBase"]);
                    objItemMaster.ConvFactBaseSale = reader["ConvFactBaseSale"].HandleDBNull() == null ? "" : Convert.ToString(reader["ConvFactBaseSale"]);

                    objItemMaster.Route = reader["Route"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["Route"]);//(long)DALHelper.HandleDBNull(reader["Route"]);
                    objItemMaster.PurchaseRate = reader["PurchaseRate"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["PurchaseRate"]);//(decimal)DALHelper.HandleDBNull(reader["PurchaseRate"]);
                    objItemMaster.MRP = reader["MRP"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["MRP"]);//(decimal)DALHelper.HandleDBNull(reader["MRP"]);
                    objItemMaster.VatPer = reader["VatPer"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["VatPer"]); //(decimal)DALHelper.HandleDBNull(reader["VatPer"]);
                    objItemMaster.ReorderQnt = reader["ReorderQnt"].HandleDBNull() == null ? 0 : Convert.ToInt32(reader["ReorderQnt"]); //(int)DALHelper.HandleDBNull(reader["ReorderQnt"]);
                    objItemMaster.BatchesRequired = reader["BatchesRequired"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["BatchesRequired"]); //(bool)DALHelper.HandleDBNull(reader["BatchesRequired"]);
                    objItemMaster.InclusiveOfTax = reader["InclusiveOfTax"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["InclusiveOfTax"]);//(bool)DALHelper.HandleDBNull(reader["InclusiveOfTax"]);
                    objItemMaster.DiscountOnSale = reader["DiscountOnSale"].HandleDBNull() == null ? 0 : Convert.ToDouble(reader["DiscountOnSale"]);

                    objItemMaster.ItemMarginID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemMarginID"]));
                    objItemMaster.ItemMargin = (string)DALHelper.HandleDBNull(reader["ItemMargin"]);
                    objItemMaster.ItemMovementID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemMovementID"]));
                    objItemMaster.ItemMovement = (string)DALHelper.HandleDBNull(reader["ItemMovement"]);
                    objItemMaster.MinStock = reader["MinStock"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["MinStock"]);
                    objItemMaster.Margin = reader["Margin"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["Margin"]);
                    objItemMaster.HighestRetailPrice = reader["HighestRetailPrice"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["HighestRetailPrice"]);
                    objItemMaster.StorageDegree = reader["StorageDegree"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["StorageDegree"]);
                    objItemMaster.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                }

                reader.Close();

                return objItemMaster;
            }
            catch (Exception ex)
            {
                if (reader != null && reader.IsClosed == false)
                    reader.Close();
                throw;
            }

        }

        public override IValueObject AddItemTax(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemTaxBizActionVO objItem = valueObject as clsAddItemTaxBizActionVO;
            try
            {
                DbCommand command = null;
                clsItemTaxVO objItemVO = objItem.ItemTax;
                int status = 0;




                command = dbServer.GetStoredProcCommand("CIMS_AddItemTax");

                clsUserVO User = new clsUserVO();

                if (objItem.ItemTaxList.Count > 0)
                {
                    for (int i = 0; i <= objItem.ItemTaxList.Count - 1; i++)
                    {

                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.ID);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitId);
                        dbServer.AddInParameter(command, "ItemID", DbType.Int64, objItemVO.ItemID);
                        dbServer.AddInParameter(command, "TaxID", DbType.Int64, objItem.ItemTaxList[i].ID);
                        dbServer.AddInParameter(command, "Percentage", DbType.Decimal, objItem.ItemTaxList[i].Percentage);
                        dbServer.AddInParameter(command, "ApplicableFor", DbType.Int32, objItem.ItemTaxList[i].ApplicableFor.ID);
                        dbServer.AddInParameter(command, "ApplicableOn", DbType.Int32, objItem.ItemTaxList[i].ApplicableOn.ID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItem.ItemTaxList[i].status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);

                        //commented By Pallavi
                        //if (objItem.ItemTaxList[i].status == true)
                        //{
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        //if (intStatus > 0)
                        //{
                        status = 1;
                        //}
                        //}



                    }
                    objItem.SuccessStatus = status;//(int)dbServer.GetParameterValue(command, "ResultStatus");   

                }






            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;

        }

        public override IValueObject AddItemSupplier(IValueObject valueObject, clsUserVO userVO)
        {

            clsAddItemSupplierbizActionVO objItem = valueObject as clsAddItemSupplierbizActionVO;
            try
            {
                DbCommand command = null;
                clsItemSupllierVO objItemVO = objItem.ItemSupplier;
                int status = 0;

                command = dbServer.GetStoredProcCommand("CIMS_AddItemSupplier");



                if (objItem.ItemSupplierList.Count > 0)
                {
                    for (int i = 0; i <= objItem.ItemSupplierList.Count - 1; i++)
                    {

                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitId);

                        dbServer.AddInParameter(command, "ItemID", DbType.Int64, objItemVO.ItemID);
                        dbServer.AddInParameter(command, "SuplierID", DbType.Int64, objItem.ItemSupplierList[i].ID);

                        dbServer.AddInParameter(command, "HPLavel", DbType.Int32, objItem.ItemSupplierList[i].SelectedHPLevel.ID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItem.ItemSupplierList[i].status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                        //if (objItem.ItemSupplierList[i].status == true)
                        //{
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        if (intStatus > 0)
                        {
                            status = 1;
                        }
                        //}



                    }
                    objItem.SuccessStatus = status;//(int)dbServer.GetParameterValue(command, "ResultStatus");   

                }






            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;

            //clsItemMasterVO objItemVO = null;
            //clsAddItemSupplierbizActionVO objItem = valueObject as clsAddItemSupplierbizActionVO;
            //try
            //{
            //    DbCommand command;
            //     objItemVO = objItem.ItemMatserDetails;
            //    if (objItemVO.EditMode == true)
            //    {
            //        command = dbServer.GetStoredProcCommand("CIMS_UpdateItemSupplier");
            //        dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
            //    }
            //    else
            //    {
            //        command = dbServer.GetStoredProcCommand("CIMS_AddItemSupplier");


            //    }


            //    //dbServer.AddInParameter(command, "SuplierID", DbType.Int64, objItemVO.SupplierID);
            //    //dbServer.AddInParameter(command, "ItemID", DbType.Int64, objItemVO.ItemID);
            //    //dbServer.AddInParameter(command, "HPLavel", DbType.Int32, objItemVO.HPLevel);
            //    //dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
            //    //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
            //    //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
            //    //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
            //    //dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
            //    //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
            //    //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objItemVO.UpdatedBy);
            //    //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdateddOn);
            //    //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
            //    //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
            //    //dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
            //    //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
            //    int intStatus = dbServer.ExecuteNonQuery(command);
            //    objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.Contains("duplicate"))
            //    {
            //        objItemVO.PrimaryKeyViolationError = true;
            //    }
            //    else
            //    {
            //        objItemVO.GeneralError = true;
            //    }


            //}
            //return objItem;
        }

        public override IValueObject CheckForTaxExistance(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemTaxBizActionVO objBizAction = valueObject as clsGetItemTaxBizActionVO;
            Boolean objectbool = false;
            DbCommand command;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_CheckForTaxExistance");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemDetails.ItemTaxID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);


                object obj = dbServer.ExecuteScalar(command);

                if (obj != null)

                    objectbool = true;
                else
                    objectbool = false;
                objBizAction.IsTaxAdded = objectbool;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
            return valueObject;

        }

        public override IValueObject GetItemList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListBizActionVO objBizAction = valueObject as clsGetItemListBizActionVO;
            DbCommand command;
            DbDataReader reader = null;
            try
            {
                if (objBizAction.FromEmr == false)
                {
                    if (objBizAction.ItemDetails.RetrieveDataFlag == true)
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_GetItemListByItemName");
                        if (objBizAction.ItemDetails != null)
                        {
                            dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.ItemDetails.ItemName);
                            dbServer.AddInParameter(command, "ItemCode", DbType.String, objBizAction.ItemDetails.ItemCode);
                            dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                            dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartRowIndex);
                            dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                            dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                        }
                    }
                    else
                    {
                        if (objBizAction.FilterCriteria == 0)
                        {
                            command = dbServer.GetStoredProcCommand("CIMS_GetItemList");
                        }
                        else
                        {
                            command = dbServer.GetStoredProcCommand("CIMS_GetItemListMultipleFiltered");
                            dbServer.AddInParameter(command, "FilterGroupId", DbType.Int64, objBizAction.FilterIGroupID);
                            dbServer.AddInParameter(command, "FilterCategoryId", DbType.Int64, objBizAction.FilterICatId);
                            dbServer.AddInParameter(command, "FilterDispensingId", DbType.Int64, objBizAction.FilterIDispensingId);
                            dbServer.AddInParameter(command, "FilterTherClassId", DbType.Int64, objBizAction.FilterITherClassId);
                            dbServer.AddInParameter(command, "FilterMolecule", DbType.Int64, objBizAction.FilterIMoleculeNameId);
                            dbServer.AddInParameter(command, "FilterClinicId", DbType.Int64, objBizAction.FilterClinicId);
                            dbServer.AddInParameter(command, "FilterStoreId", DbType.Int64, objBizAction.FilterStoreId);
                            dbServer.AddInParameter(command, "ForReportFilter", DbType.Boolean, objBizAction.ForReportFilter);
                            dbServer.AddInParameter(command, "BrandName", DbType.String, objBizAction.BrandName);
                            dbServer.AddInParameter(command, "IsQtyShow", DbType.Boolean, objBizAction.IsQtyShow);
                            dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                        }
                    }
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemListMultipleFiltered");
                    dbServer.AddInParameter(command, "FilterGroupId", DbType.Int64, objBizAction.FilterIGroupID);
                    dbServer.AddInParameter(command, "FilterCategoryId", DbType.Int64, objBizAction.FilterICatId);
                    dbServer.AddInParameter(command, "FilterDispensingId", DbType.Int64, objBizAction.FilterIDispensingId);
                    dbServer.AddInParameter(command, "FilterTherClassId", DbType.Int64, objBizAction.FilterITherClassId);
                    dbServer.AddInParameter(command, "FilterMolecule", DbType.Int64, objBizAction.FilterIMoleculeNameId);
                    dbServer.AddInParameter(command, "BrandName", DbType.String, objBizAction.BrandName);
                    #region Added BY Pallavi
                    dbServer.AddInParameter(command, "FilterClinicId", DbType.Int64, objBizAction.FilterClinicId);
                    dbServer.AddInParameter(command, "FilterStoreId", DbType.Int64, objBizAction.FilterStoreId);
                    dbServer.AddInParameter(command, "ForReportFilter", DbType.Boolean, objBizAction.ForReportFilter);
                    #endregion
                    dbServer.AddInParameter(command, "IsQtyShow", DbType.Boolean, objBizAction.IsQtyShow);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                }
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsItemMasterVO>();
                    if (objBizAction.MasterList == null && objBizAction.ForReportFilter == true)
                        objBizAction.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        if (objBizAction.ForReportFilter == false)
                        {
                            clsItemMasterVO objItemMaster = new clsItemMasterVO();
                            objItemMaster.ItemCode = (string)(reader["ItemCode"].HandleDBNull() == null ? "" : reader["ItemCode"]);
                            objItemMaster.ItemName = (string)(reader["ItemName"].HandleDBNull() == null ? "" : reader["ItemName"]);
                            objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                            objItemMaster.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objItemMaster.UnitID = (long)(reader["UnitID"].HandleDBNull() == null ? 0 : reader["UnitID"]);
                            objItemMaster.BrandName = (string)DALHelper.HandleDBNull(reader["BrandName"]);
                            objItemMaster.Strength = (string)DALHelper.HandleDBNull(reader["Strength"]);
                            objItemMaster.MoleculeName = (long)DALHelper.HandleDBNull(reader["MoleculeName"]);
                            objItemMaster.MoleculeNameString = (string)DALHelper.HandleDBNull(reader["MoleculeNameString"]);
                            objItemMaster.ItemGroup = (long)DALHelper.HandleDBNull(reader["ItemGroup"]);
                            objItemMaster.ItemGroupString = (string)DALHelper.HandleDBNull(reader["ItemGroupString"]);
                            objItemMaster.ItemGroupString = (string)(reader["ItemGroupString"].HandleDBNull() == null ? "" : reader["ItemGroupString"]);
                            objItemMaster.ItemCategory = (long)DALHelper.HandleDBNull(reader["ItemCategory"]);
                            objItemMaster.ItemCategoryString = (string)(reader["ItemCategoryString"].HandleDBNull() == null ? "" : reader["ItemCategoryString"]);
                            objItemMaster.DispencingType = reader["DispencingType"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["DispencingType"]);
                            objItemMaster.StoreageType = reader["StoreageType"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["StoreageType"]); //(long)(DALHelper.HandleDBNull(reader["StoreageType"])==null?0:;
                            objItemMaster.PregClass = reader["PregClass"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["PregClass"]);//(long)DALHelper.HandleDBNull(reader["PregClass"]);
                            objItemMaster.TherClass = reader["TherClass"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["TherClass"]); //(long)DALHelper.HandleDBNull(reader["TherClass"]);
                            objItemMaster.MfgBy = reader["MfgBy"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["MfgBy"]);//(long)DALHelper.HandleDBNull(reader["MfgBy"]);
                            objItemMaster.MfgByString = (string)(reader["MfgByString"].HandleDBNull() == null ? "" : reader["MfgByString"]);
                            objItemMaster.MrkBy = reader["MrkBy"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["MrkBy"]);//(long)DALHelper.HandleDBNull(reader["MrkBy"]);
                            objItemMaster.MrkByString = (string)(reader["MrkByString"].HandleDBNull() == null ? "" : reader["MrkByString"]);
                            objItemMaster.PUM = reader["PUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["PUM"]); //(long)DALHelper.HandleDBNull(reader["PUM"]);
                            objItemMaster.PUMString = (string)(reader["PUMString"].HandleDBNull() == null ? "" : reader["PUMString"]);
                            objItemMaster.SUM = reader["SUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["SUM"]);//(long)DALHelper.HandleDBNull(reader["SUM"]);
                            objItemMaster.ConversionFactor = reader["ConversionFactor"].HandleDBNull() == null ? "" : Convert.ToString(reader["ConversionFactor"]); //(string)DALHelper.HandleDBNull(reader["ConversionFactor"]);

                            objItemMaster.BaseUM = reader["BaseUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["BaseUM"]);
                            objItemMaster.SellingUM = reader["SellingUM"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["SellingUM"]);
                            objItemMaster.ConvFactStockBase = reader["ConvFactStockBase"].HandleDBNull() == null ? "" : Convert.ToString(reader["ConvFactStockBase"]);
                            objItemMaster.ConvFactBaseSale = reader["ItemExpiredInDays"].HandleDBNull() == null ? "" : Convert.ToString(reader["ConvFactBaseSale"]);

                            objItemMaster.Route = reader["Route"].HandleDBNull() == null ? 0 : Convert.ToInt64(reader["Route"]);//(long)DALHelper.HandleDBNull(reader["Route"]);
                            objItemMaster.PurchaseRate = reader["PurchaseRate"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["PurchaseRate"]);//(decimal)DALHelper.HandleDBNull(reader["PurchaseRate"]);
                            objItemMaster.MRP = reader["MRP"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["MRP"]);//(decimal)DALHelper.HandleDBNull(reader["MRP"]);
                            objItemMaster.VatPer = reader["VatPer"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["VatPer"]); //(decimal)DALHelper.HandleDBNull(reader["VatPer"]);
                            objItemMaster.ReorderQnt = reader["ReorderQnt"].HandleDBNull() == null ? 0 : Convert.ToInt32(reader["ReorderQnt"]); //(int)DALHelper.HandleDBNull(reader["ReorderQnt"]);
                            objItemMaster.BatchesRequired = reader["BatchesRequired"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["BatchesRequired"]); //(bool)DALHelper.HandleDBNull(reader["BatchesRequired"]);
                            objItemMaster.InclusiveOfTax = reader["InclusiveOfTax"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["InclusiveOfTax"]);//(bool)DALHelper.HandleDBNull(reader["InclusiveOfTax"]);
                            objItemMaster.DiscountOnSale = reader["DiscountOnSale"].HandleDBNull() == null ? 0 : Convert.ToDouble(reader["DiscountOnSale"]);
                            objItemMaster.ItemMarginID = (long)DALHelper.HandleIntegerNull(reader["ItemMarginID"]);
                            objItemMaster.ItemMargin = (string)DALHelper.HandleDBNull(reader["ItemMargin"]);
                            objItemMaster.ItemMovementID = (long)DALHelper.HandleIntegerNull(reader["ItemMovementID"]);
                            objItemMaster.ItemMovement = (string)DALHelper.HandleDBNull(reader["ItemMovement"]);
                            objItemMaster.MinStock = reader["MinStock"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["MinStock"]);
                            objItemMaster.Margin = reader["Margin"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["Margin"]);
                            objItemMaster.HighestRetailPrice = reader["HighestRetailPrice"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["HighestRetailPrice"]);
                            objItemMaster.StorageDegree = reader["StorageDegree"].HandleDBNull() == null ? 0 : Convert.ToDecimal(reader["StorageDegree"]);
                            objItemMaster.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                            objItemMaster.IsABC = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsABC"]));
                            objItemMaster.IsFNS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFNS"]));
                            objItemMaster.IsVED = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsVED"]));
                            objItemMaster.StrengthUnitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrengthUnitTypeID"]));
                            objItemMaster.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                            if (objBizAction.ItemDetails.RetrieveDataFlag == true)
                            {
                                objItemMaster.HSNCodesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HSNCodesID"])); //Added By Bhushanp For GST 19062017
                                //***//
                                objItemMaster.StaffDiscount = reader["StaffDiscount"].HandleDBNull() == null ? 0 : Convert.ToDouble(reader["StaffDiscount"]);
                                objItemMaster.RegisteredPatientsDiscount = reader["RegisteredPatientsDiscount"].HandleDBNull() == null ? 0 : Convert.ToDouble(reader["RegisteredPatientsDiscount"]);
                                objItemMaster.WalkinDiscount = reader["WalkinDiscount"].HandleDBNull() == null ? 0 : Convert.ToDouble(reader["WalkinDiscount"]);

                            }
                            objBizAction.ItemList.Add(objItemMaster);
                        }
                        else
                        {
                            MasterListItem objList = new MasterListItem();
                            objList.Code = (string)(reader["Strength"].HandleDBNull() == null ? "" : reader["Strength"]);
                            objList.Description = (string)(reader["BrandName"].HandleDBNull() == null ? "" : reader["BrandName"]);
                            objList.Name = (string)(reader["ItemName"].HandleDBNull() == null ? "" : reader["ItemName"]);
                            objList.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            objList.FilterID = (long)DALHelper.HandleDBNull(reader["Route"]);
                            objList.Value = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));//(decimal)DALHelper.HandleDBNull(reader["MRP"]);
                            objList.PrintDescription = (string)(reader["RouteName"].HandleDBNull() == null ? "" : reader["RouteName"]);
                            objList.StockQty = Convert.ToDouble(DALHelper.HandleDBNull((reader["AvailableStock"])));
                            objList.UOM = (string)(reader["UOM"].HandleDBNull() == null ? "" : reader["UOM"]);
                            objList.UOMID = Convert.ToInt64((reader["UOMID"].HandleDBNull() == null ? "" : reader["UOMID"]));
                            objList.Route = reader["RouteName"].HandleDBNull() == null ? "" : Convert.ToString(reader["RouteName"]);
                            objBizAction.MasterList.Add(objList);
                        }
                    }
                }
                reader.NextResult();
                if (objBizAction.ForReportFilter == true)
                {
                    objBizAction.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
                }

                if (objBizAction.FromEmr == false)
                {
                    if (objBizAction.ItemDetails.RetrieveDataFlag == true)
                    {
                        objBizAction.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
                    }
                }
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
            return valueObject;
        }

        public override IValueObject DeleteItemStore(IValueObject valueObject, clsUserVO userVO)
        {
            clsDeleteItemStoresBizActionVO objItem = valueObject as clsDeleteItemStoresBizActionVO;

            try
            {
                DbCommand command = null;

                clsItemMasterVO objItemVO = objItem.ItemMatserDetails;
                command = dbServer.GetStoredProcCommand("CIMS_DeleteItemStore");
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objItemVO.ItemID);
                dbServer.AddInParameter(command, "ClinicID", DbType.Int64, objItemVO.ClinicID);

                int intStatus = dbServer.ExecuteNonQuery(command);
                if (intStatus > 0)
                {
                    objItem.SuccessStatus = 1;

                }
                else
                {
                    objItem.SuccessStatus = 0;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;
        }

        public override IValueObject GetItemTaxList(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetItemTaxBizActionVO objBizAction = valueObject as clsGetItemTaxBizActionVO;
            //objBizAction.ItemDetails = new clsItemMasterVO();
            DbCommand command;
            //
            try
            {
                //if (objBizAction.ItemDetails.RetrieveDataFlag == true)
                //{
                //    command = dbServer.GetStoredProcCommand("CIMS_GetItemTaxListByItemName");
                //    dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.ItemDetails.ItemName);
                //}
                //else
                //{
                command = dbServer.GetStoredProcCommand("CIMS_GetItemTaxList");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemTaxDetails.ItemID);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                //}


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemTaxList == null)
                        objBizAction.ItemTaxList = new List<clsItemTaxVO>();
                    while (reader.Read())
                    {
                        clsItemTaxVO objItemMaster = new clsItemTaxVO();

                        objItemMaster.status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objItemMaster.ApplicableOn.ID = (Int32)DALHelper.HandleDBNull(reader["AppOn"]);
                        objItemMaster.ApplicableFor.ID = (Int32)DALHelper.HandleDBNull(reader["AppFor"]);
                        objItemMaster.TaxID = (long)DALHelper.HandleDBNull(reader["TaxID"]);
                        objItemMaster.Percentage = (decimal)DALHelper.HandleDBNull(reader["Percentage"]);
                        objItemMaster.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objItemMaster.TaxName = (string)DALHelper.HandleDBNull(reader["Description"]);





                        objBizAction.ItemTaxList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }

        public override IValueObject GetItemSupplierList(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetItemSupplierBizActionVO objBizAction = valueObject as clsGetItemSupplierBizActionVO;

            //objBizAction.ItemDetails = new clsItemMasterVO();
            DbCommand command;
            //
            try
            {
                //if (objBizAction.ItemDetails.RetrieveDataFlag == true)
                //{
                //    command = dbServer.GetStoredProcCommand("CIMS_GetItemTaxListByItemName");
                //    dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.ItemDetails.ItemName);
                //}
                //else
                //{
                command = dbServer.GetStoredProcCommand("CIMS_GetItemSupplierList");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemSupplier.ItemID);
                //dbServer.AddInParameter(command, "UnitID", DbType.String, userVO.UserLoginInfo.UnitId);
                //}


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemSupplierList == null)
                        objBizAction.ItemSupplierList = new List<clsItemSupllierVO>();
                    while (reader.Read())
                    {

                        clsItemSupllierVO objItemMaster = new clsItemSupllierVO();

                        objItemMaster.status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.ID = (long)DALHelper.HandleDBNull(reader["SuplierID"]);
                        objItemMaster.SelectedHPLevel.ID = (Int32)DALHelper.HandleDBNull(reader["HPLavel"]);

                        //objItemMaster.HPLevel = (Int32)DALHelper.HandleDBNull(reader["HPLavel"]);
                        objItemMaster.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);

                        objBizAction.ItemSupplierList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }

        #region OLD COde Commented By CDS
        //public override IValueObject AddItemClinic(IValueObject valueObject, clsUserVO userVO)
        //{

        //    clsAddItemClinicBizActionVO objItem = valueObject as clsAddItemClinicBizActionVO;

        //    DbConnection con = dbServer.CreateConnection();
        //    DbTransaction trans = null;

        //    try
        //    {
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        DbCommand command = null;
        //        DbCommand command1 = null;
        //        DbCommand command3 = null;



        //        clsItemStoreVO objItemVO = objItem.ItemStore;
        //        int status = 0;

        //        if (objItemVO.StoreList.Count > 0)
        //        {
        //            StringBuilder sbTaxListIDs = new StringBuilder();
        //            StringBuilder sbStoreListIDs = new StringBuilder();

        //            for (int i = 0; i <= objItemVO.StoreList.Count - 1; i++)
        //            {

        //                sbTaxListIDs = new StringBuilder();

        //                if (objItemVO.StoreList != null && objItemVO.StoreList[i].objStoreTaxList != null)
        //                {
        //                    foreach (clsItemTaxVO item in objItemVO.StoreList[i].objStoreTaxList)
        //                    {
        //                        sbTaxListIDs.Append("," + Convert.ToString(item.ID));
        //                    }

        //                    if (sbTaxListIDs.Length > 0)
        //                        sbTaxListIDs.Replace(",", "", 0, 1);

        //                    command3 = dbServer.GetStoredProcCommand("CIMS_DeleteItemClinicDetail");
        //                    command3.Connection = con;
        //                    dbServer.AddInParameter(command3, "UnitId", DbType.Int64, objItemVO.UnitID);
        //                    dbServer.AddInParameter(command3, "ItemClinicId", DbType.Int64, objItemVO.StoreList[i].ID);
        //                    dbServer.AddInParameter(command3, "StoreId", DbType.Int64, objItemVO.StoreList[i].StoreID);
        //                    dbServer.AddInParameter(command3, "ItemClinicDetailIdList", DbType.String, Convert.ToString(sbTaxListIDs));
        //                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
        //                }

        //                sbStoreListIDs.Append("," + Convert.ToString(objItemVO.StoreList[i].ID));

        //            }

        //            sbStoreListIDs.Replace(",", "", 0, 1);

        //            command1 = dbServer.GetStoredProcCommand("CIMS_DeleteItemClinic");
        //            command1.Connection = con;
        //            dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objItemVO.UnitID);
        //            dbServer.AddInParameter(command1, "ItemID", DbType.Int64, objItemVO.ItemID);
        //            dbServer.AddInParameter(command1, "StoreId", DbType.Int64, objItemVO.StoreList[0].StoreID);
        //            dbServer.AddInParameter(command1, "ItemClinicIdList", DbType.String, Convert.ToString(sbStoreListIDs));
        //            int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

        //        }

        //        command = dbServer.GetStoredProcCommand("CIMS_AddItemClinic");
        //        command.Connection = con;

        //        if (objItemVO.StoreList.Count > 0)
        //        {
        //            for (int i = 0; i <= objItemVO.StoreList.Count - 1; i++)
        //            {
        //                command.Parameters.Clear();
        //                dbServer.AddParameter(command, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.StoreList[i].ID);
        //                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitID);
        //                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objItemVO.ItemID);
        //                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objItemVO.StoreList[i].StoreID);
        //                dbServer.AddInParameter(command, "Min", DbType.Decimal, objItemVO.StoreList[i].Min);
        //                dbServer.AddInParameter(command, "Max", DbType.Decimal, objItemVO.StoreList[i].Max);
        //                dbServer.AddInParameter(command, "Re_Order", DbType.Decimal, objItemVO.StoreList[i].Re_Order);
        //                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.StoreList[i].status);
        //                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
        //                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
        //                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
        //                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
        //                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
        //                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);

        //                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //                int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //                if (intStatus > 0)
        //                {
        //                    status = 1;
        //                }
        //                objItem.ItemStoreDetails = new clsItemStoreVO();
        //                objItem.ItemStoreDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

        //                #region SudhirTest

        //                DbCommand command2 = null;


        //                if (objItemVO.StoreList != null && objItemVO.StoreList[i].objStoreTaxList != null) //if (objItem.ItemTaxList != null && objItem.ItemTaxList.Count > 0)
        //                {
        //                    foreach (clsItemTaxVO objItemTax in objItemVO.StoreList[i].objStoreTaxList)  //foreach (clsItemTaxVO objItemTax in objItem.ItemTaxList)
        //                    {
        //                        command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemClinicDetail");
        //                        command2.Connection = con;

        //                        dbServer.AddInParameter(command2, "Id", DbType.Int64, objItemTax.ID);
        //                        dbServer.AddInParameter(command2, "ItemClinicId", DbType.Int64, objItem.ItemStoreDetails.ID);
        //                        dbServer.AddInParameter(command2, "TaxID", DbType.Int64, objItemTax.TaxID);
        //                        dbServer.AddInParameter(command2, "VATPercentage", DbType.Decimal, objItemTax.Percentage);
        //                        dbServer.AddInParameter(command2, "ApplicableFor", DbType.Int32, objItemTax.ApplicableForId);
        //                        dbServer.AddInParameter(command2, "VATApplicableOn", DbType.Int32, objItemTax.ApplicableOnId);
        //                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objItemTax.status);
        //                        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
        //                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objItemVO.AddedBy);
        //                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, objItemVO.AddedOn);
        //                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
        //                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
        //                        int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
        //                    }
        //                    objItem.SuccessStatus = 1;
        //                }
        //                #endregion SudhirTest
        //            }
        //            objItem.SuccessStatus = status;

        //            trans.Commit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        throw;
        //        //logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //        con = null;
        //        trans = null;
        //    }
        //    return objItem;
        //}
        #endregion


        public override IValueObject AddItemClinic(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemClinicBizActionVO objItem = valueObject as clsAddItemClinicBizActionVO;
            try
            {
                DbCommand command = null;
                DbCommand command1 = null;
                clsItemStoreVO objItemVO = objItem.ItemStore;
                int status = 0;
                if (objItemVO.DeletedStoreList.Count > 0)
                {
                    foreach (var item in objItemVO.DeletedStoreList)
                    {
                        command1 = dbServer.GetStoredProcCommand("CIMS_DeleteItemClinic");
                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, item.UnitId);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                        dbServer.AddInParameter(command1, "StoreId", DbType.Int64, item.StoreID);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    }


                    //for (int i = 0; i <= objItemVO.StoreList.Count - 1; i++)
                    //{
                    //    command1 = dbServer.GetStoredProcCommand("CIMS_DeleteItemClinic");
                    //    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objItemVO.UnitID);
                    //    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, objItemVO.ItemID);
                    //    dbServer.AddInParameter(command1, "StoreId", DbType.Int64, objItemVO.StoreList[i].StoreID);
                    //    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    //}
                }
                command = dbServer.GetStoredProcCommand("CIMS_AddItemClinic");
                if (objItemVO.StoreList.Count > 0)//&& objItem.ItemTaxList.Count > 0
                {
                    for (int i = 0; i <= objItemVO.StoreList.Count - 1; i++)
                    {
                        command.Parameters.Clear();
                        dbServer.AddParameter(command, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.StoreList[i].ID);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitID);
                        dbServer.AddInParameter(command, "ItemID", DbType.Int64, objItemVO.ItemID);
                        dbServer.AddInParameter(command, "StoreId", DbType.Int64, objItemVO.StoreList[i].StoreID);
                        dbServer.AddInParameter(command, "Min", DbType.Decimal, objItemVO.StoreList[i].Min);
                        dbServer.AddInParameter(command, "Max", DbType.Decimal, objItemVO.StoreList[i].Max);
                        dbServer.AddInParameter(command, "Re_Order", DbType.Decimal, objItemVO.StoreList[i].Re_Order);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.StoreList[i].status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        if (intStatus > 0)
                        {
                            status = 1;
                        }
                        objItem.ItemStoreDetails = new clsItemStoreVO();
                        objItem.ItemStoreDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                        #region SudhirTest
                        //DbCommand command2 = null;
                        //if (objItem.ItemTaxList != null && objItem.ItemTaxList.Count > 0)
                        //{
                        //    foreach (clsItemTaxVO objItemTax in objItem.ItemTaxList)
                        //    {
                        //        command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemClinicDetail");
                        //        dbServer.AddInParameter(command2, "Id", DbType.Int64, objItemTax.ID);
                        //        dbServer.AddInParameter(command2, "ItemClinicId", DbType.Int64, objItem.ItemStoreDetails.ID);
                        //        dbServer.AddInParameter(command2, "TaxID", DbType.Int64, objItemTax.TaxID);
                        //        dbServer.AddInParameter(command2, "VATPercentage", DbType.Decimal, objItemTax.Percentage);
                        //        dbServer.AddInParameter(command2, "taxtype", DbType.Int32, objItemTax.TaxType);
                        //        dbServer.AddInParameter(command2, "ApplicableFor", DbType.Int32, objItemTax.ApplicableForId);
                        //        dbServer.AddInParameter(command2, "VATApplicableOn", DbType.Int32, objItemTax.ApplicableOnId);
                        //        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objItemTax.status);
                        //        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        //        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        //        dbServer.AddInParameter(command2, "AddedOn", DbType.String, objItemVO.AddedOn);
                        //        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        //        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        //        int intStatus1 = dbServer.ExecuteNonQuery(command2);
                        //    }
                        //    objItem.SuccessStatus = 1;
                        //}
                        #endregion SudhirTest
                    }
                    objItem.SuccessStatus = status;
                }
                else if (objItem.ItemTaxList != null && objItem.ItemTaxList.Count > 0)
                {
                    #region SudhirTest
                    DbCommand command2 = null;
                    if (objItem.ItemTaxList != null && objItem.ItemTaxList.Count > 0)
                    {
                        foreach (clsItemTaxVO objItemTax in objItem.ItemTaxList)
                        {
                            command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemClinicDetail");
                            dbServer.AddInParameter(command2, "Id", DbType.Int64, objItemTax.ID);
                            //    dbServer.AddInParameter(command2, "ItemClinicId", DbType.Int64, objItem.ItemStoreDetails.ID);
                            dbServer.AddInParameter(command2, "TaxID", DbType.Int64, objItemTax.TaxID);
                            dbServer.AddInParameter(command2, "StoreID", DbType.Int64, objItemTax.StoreID);
                            dbServer.AddInParameter(command2, "ItemID", DbType.Int64, objItemVO.ItemID);
                            dbServer.AddInParameter(command2, "VATPercentage", DbType.Decimal, objItemTax.Percentage);
                            dbServer.AddInParameter(command2, "taxtype", DbType.Int32, objItemTax.TaxType);
                            dbServer.AddInParameter(command2, "ApplicableFor", DbType.Int32, objItemTax.ApplicableForId);
                            dbServer.AddInParameter(command2, "VATApplicableOn", DbType.Int32, objItemTax.ApplicableOnId);
                            dbServer.AddInParameter(command2, "ApplicableFromDate", DbType.DateTime, objItemTax.ApplicableFrom); //Added By Bhushanp For GST 20062017
                            dbServer.AddInParameter(command2, "ApplicableToDate", DbType.DateTime, objItemTax.ApplicableTo); //Added By Bhushanp For GST 20062017
                            dbServer.AddInParameter(command2, "IsGST", DbType.Boolean, objItemTax.IsGST);//Added By Bhushanp For GST 20062017
                            dbServer.AddInParameter(command2, "Status", DbType.Boolean, objItemTax.status);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, objItemVO.AddedOn);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command2);
                        }
                        objItem.SuccessStatus = 1;
                        objItem.ResultStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    }
                    #endregion SudhirTest
                }
                else
                {
                    objItem.SuccessStatus = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objItem;
        }

        public override IValueObject DeleteTax(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemClinicBizActionVO objItem = valueObject as clsAddItemClinicBizActionVO;
            DbCommand command = null;
            DbCommand command1 = null;
            command = dbServer.GetStoredProcCommand("CIMS_DeleteItemClinicDetail");
            dbServer.AddInParameter(command, "TaxID", DbType.Int64, objItem.DeleteTaxID);
            int intStatus = dbServer.ExecuteNonQuery(command);
            return valueObject;
        }

        //***//19 Add Update Tax Same Item But multiple Store  
 
        public override IValueObject AddMultipleStoreTax(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemClinicBizActionVO objItem = valueObject as clsAddItemClinicBizActionVO;
            try
            {
                DbCommand command = null;

                clsItemStoreVO objItemVO = objItem.ItemStore;
                int status = 0;

                if (objItem.ISAddMultipleStoreTax)
                {                   
                        foreach (clsItemTaxVO objItemTax in objItem.ItemTaxList)
                        {
                            command = dbServer.GetStoredProcCommand("CIMS_AddMultipleStoreTax");
                            dbServer.AddOutParameter(command, "Id", DbType.Int64, 0);
                            dbServer.AddInParameter(command, "TaxID", DbType.Int64, objItemTax.TaxID);
                            //dbServer.AddInParameter(command, "StoreID", DbType.Int64, objStoreList.StoreID);
                            dbServer.AddInParameter(command, "ItemID", DbType.Int64, objItemVO.ItemID);
                            dbServer.AddInParameter(command, "VATPercentage", DbType.Decimal, objItemTax.Percentage);
                            dbServer.AddInParameter(command, "taxtype", DbType.Int32, objItemTax.TaxType);
                            dbServer.AddInParameter(command, "ApplicableFor", DbType.Int32, objItemTax.ApplicableForId);
                            dbServer.AddInParameter(command, "VATApplicableOn", DbType.Int32, objItemTax.ApplicableOnId);
                            //dbServer.AddInParameter(command, "ApplicableFromDate", DbType.DateTime, objItemTax.ApplicableFrom);
                            //dbServer.AddInParameter(command, "ApplicableToDate", DbType.DateTime, objItemTax.ApplicableTo);
                            //dbServer.AddInParameter(command, "IsGST", DbType.Boolean, objItemTax.IsGST);
                            dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemTax.status);
                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command);

                            objItem.SuccessStatus = 1;
                            objItem.ResultStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                            objItem.MultipleStoreTaxID = (long)dbServer.GetParameterValue(command, "ID");
                        }

                        if (objItem.ResultStatus == 1)
                        {
                            DbCommand command1 = null;

                            foreach (clsItemMasterVO objStoreList in objItem.ItemLinkStoreList)
                            {
                                foreach (clsItemTaxVO objItemTax in objItem.ItemTaxList)
                                {
                                    command1 = dbServer.GetStoredProcCommand("CIMS_AddMultipleItemClinic");
                                    dbServer.AddInParameter(command1, "Id", DbType.Int64, objItemTax.ID);
                                    dbServer.AddInParameter(command1, "MultipleStoreTaxDetailsID", DbType.Int64, objItem.MultipleStoreTaxID);
                                    dbServer.AddInParameter(command1, "TaxID", DbType.Int64, objItemTax.TaxID);
                                    dbServer.AddInParameter(command1, "StoreID", DbType.Int64, objStoreList.StoreID);
                                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, objItemVO.ItemID);
                                    dbServer.AddInParameter(command1, "VATPercentage", DbType.Decimal, objItemTax.Percentage);
                                    dbServer.AddInParameter(command1, "taxtype", DbType.Int32, objItemTax.TaxType);
                                    dbServer.AddInParameter(command1, "ApplicableFor", DbType.Int32, objItemTax.ApplicableForId);
                                    dbServer.AddInParameter(command1, "VATApplicableOn", DbType.Int32, objItemTax.ApplicableOnId);
                                    dbServer.AddInParameter(command1, "ApplicableFromDate", DbType.DateTime, objItemTax.ApplicableFrom);
                                    dbServer.AddInParameter(command1, "ApplicableToDate", DbType.DateTime, objItemTax.ApplicableTo);
                                    dbServer.AddInParameter(command1, "IsGST", DbType.Boolean, objItemTax.IsGST);
                                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objItemTax.status);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, objItemVO.AddedOn);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                                    objItem.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                                }
                            }
                        }
                }

                else
                {
                    DbCommand command2 = null;
                    foreach (clsItemTaxVO objItemTax in objItem.ItemTaxList)
                    {
                        command2 = dbServer.GetStoredProcCommand("CIMS_UpdateMultipleStoreTax");
                        dbServer.AddInParameter(command2, "Id", DbType.Int64, objItemTax.ID);
                        dbServer.AddInParameter(command2, "TaxID", DbType.Int64, objItemTax.TaxID);
                        dbServer.AddInParameter(command2, "StoreID", DbType.Int64, objItemTax.StoreID);
                        dbServer.AddInParameter(command2, "ItemID", DbType.Int64, objItemVO.ItemID);
                        dbServer.AddInParameter(command2, "VATPercentage", DbType.Decimal, objItemTax.Percentage);
                        dbServer.AddInParameter(command2, "taxtype", DbType.Int32, objItemTax.TaxType);
                        dbServer.AddInParameter(command2, "ApplicableFor", DbType.Int32, objItemTax.ApplicableForId);
                        dbServer.AddInParameter(command2, "VATApplicableOn", DbType.Int32, objItemTax.ApplicableOnId);
                        dbServer.AddInParameter(command2, "ApplicableFromDate", DbType.DateTime, objItemTax.ApplicableFrom);
                        dbServer.AddInParameter(command2, "ApplicableToDate", DbType.DateTime, objItemTax.ApplicableTo);
                        dbServer.AddInParameter(command2, "IsGST", DbType.Boolean, objItemTax.IsGST);
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objItemTax.status);
                        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus1 = dbServer.ExecuteNonQuery(command2);

                        objItem.SuccessStatus = 1;
                        objItem.ResultStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objItem;
        }

        public override IValueObject GetItemMapStoreList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemClinicBizActionVO objBizAction = valueObject as clsGetItemClinicBizActionVO;
            DbCommand command = null;
           int ISItemLinkStore = 0;
            try
            {                 
                command = dbServer.GetStoredProcCommand("CIMS_GetItemMapStoreList");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemDetails.ItemID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.ItemDetails.UnitID);             
                dbServer.AddOutParameter(command, "ISItemLinkStore", DbType.Int32, int.MaxValue);
           
                reader = (DbDataReader)dbServer.ExecuteReader(command);       

                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objItemMaster = new clsItemMasterVO();                      
                        objItemMaster.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));                      
                        objItemMaster.ClinicID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));                     
                        objBizAction.ItemList.Add(objItemMaster);
                    }
                }

                reader.NextResult();
                ISItemLinkStore = (int)dbServer.GetParameterValue(command, "ISItemLinkStore");
                
                if (ISItemLinkStore == 0)
                {
                    DbCommand command1 = null;
                 
                    foreach (var item in objBizAction.ItemList)
                    {                       
                        command1 = dbServer.GetStoredProcCommand("CIMS_MapItemClinic");  //Map Item To Multiple Store
                        dbServer.AddInParameter(command1, "ID", DbType.Int64,0);
                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, item.ClinicID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, objBizAction.ItemDetails.ItemID);
                        dbServer.AddInParameter(command1, "StoreId", DbType.Int64, item.StoreID);
                        dbServer.AddInParameter(command1, "Min", DbType.Decimal,0);
                        dbServer.AddInParameter(command1, "Max", DbType.Decimal,0);
                        dbServer.AddInParameter(command1, "Re_Order", DbType.Decimal,0);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, 1);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, objBizAction.ItemDetails.CreatedUnitID);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objBizAction.ItemDetails.AddedBy);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, objBizAction.ItemDetails.AddedOn);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objBizAction.ItemDetails.AddedDateTime);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objBizAction.ItemDetails.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        reader = (DbDataReader)dbServer.ExecuteReader(command1);
                       long ResultStatus = Convert.ToInt64(dbServer.GetParameterValue(command1, "ResultStatus"));
                    }                   
                }

                if (ISItemLinkStore == 0)
                {
                    DbCommand command2 = null;
                    command2 = dbServer.GetStoredProcCommand("CIMS_GetItemMapStoreList");
                    dbServer.AddInParameter(command2, "ID", DbType.Int64, objBizAction.ItemDetails.ItemID);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objBizAction.ItemDetails.UnitID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command2);

                    if (reader.HasRows)
                    {                      
                            objBizAction.ItemList = new List<clsItemMasterVO>();

                        while (reader.Read())
                        {
                            clsItemMasterVO objItemMaster = new clsItemMasterVO();
                            objItemMaster.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));
                            objItemMaster.ClinicID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));

                            objBizAction.ItemList.Add(objItemMaster);
                        }
                    }
                }             
                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return valueObject;

        }

        public override IValueObject GetMultipleStoreItemTaxList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemStoreTaxListBizActionVO BizActionObj = valueObject as clsGetItemStoreTaxListBizActionVO;
            DbCommand command;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetMultipleStoreItemTaxList");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreItemTaxDetails.ID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.StoreItemTaxDetails.ItemID);
               
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.StoreItemTaxList == null)
                        BizActionObj.StoreItemTaxList = new List<clsItemTaxVO>();
                    while (reader.Read())
                    {
                        clsItemTaxVO objItemMaster = new clsItemTaxVO();
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"]));
                        objItemMaster.TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"]));
                        objItemMaster.ApplicableOnId = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATApplicableOn"]));
                        objItemMaster.ApplicableForId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableFor"]));
                        objItemMaster.ApplicableOnDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableOnDesc"]));
                        objItemMaster.ApplicableForDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableForDesc"]));                      
                        objItemMaster.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemMaster.TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        objItemMaster.status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));                       
                    
                        if (objItemMaster.TaxType == 1)
                        {
                            objItemMaster.TaxTypeName = "Inclusive";
                        }
                        else
                        {
                            objItemMaster.TaxTypeName = "Exclusive";
                        }

                        BizActionObj.StoreItemTaxList.Add(objItemMaster);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return BizActionObj;
        }

        public override IValueObject DeleteMultipleStoreTax(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemClinicBizActionVO objItem = valueObject as clsAddItemClinicBizActionVO;
            DbCommand command = null;
            DbCommand command1 = null;
            command = dbServer.GetStoredProcCommand("CIMS_DleteMultipleStoreTaxDetail");
            dbServer.AddInParameter(command, "TaxID", DbType.Int64, objItem.DeleteTaxID);
            int intStatus = dbServer.ExecuteNonQuery(command);
            return valueObject;
        }


        //----------------------------------------------//----------------------------------------------------------------//


        public override IValueObject GetItemClinicList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemClinicBizActionVO objBizAction = valueObject as clsGetItemClinicBizActionVO;
            DbCommand command;
            try
            {
                if (objBizAction.ItemDetails.RetrieveDataFlag == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemClinicListByItemName");
                    dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.ItemDetails.ItemName);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemClinicList");
                    dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemDetails.ItemID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.ItemDetails.UnitID);
                }
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objItemMaster = new clsItemMasterVO();
                        objItemMaster.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemMaster.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));
                        //objItemMaster.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objItemMaster.Min = Convert.ToDouble(DALHelper.HandleDBNull(reader["Min"]));
                        objItemMaster.Max = Convert.ToDouble(DALHelper.HandleDBNull(reader["Max"]));
                        objItemMaster.Re_Order = Convert.ToDouble(DALHelper.HandleDBNull(reader["Re_Order"]));
                        objItemMaster.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.ClinicID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objBizAction.ItemList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return valueObject;

        }

        public override IValueObject GetStoreStatus(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            clsGetStoreStatusBizActionVO objBizAction = valueObject as clsGetStoreStatusBizActionVO;
            //objBizAction.ItemDetails = new clsItemMasterVO();
            DbCommand command;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetStoreStatus");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizAction.ItemDetails.StoreID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objBizAction.ItemDetails.ItemID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, userVO.UserLoginInfo.UnitId);



                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        objBizAction.ItemDetails = new clsItemMasterVO();

                        objBizAction.ItemDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);





                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }

        public override IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO userVO)
        {
            clsItemMasterVO objItemVO = null;
            clsAddOpeningBalanceBizActionVO objItem = valueObject as clsAddOpeningBalanceBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails;

                command = dbServer.GetStoredProcCommand("CIMS_AddOpeningBalanceMaster");






                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "LinkServerName", DbType.String, objItemVO.LinkServerName);
                dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objItemVO.LinkServerAlias);
                dbServer.AddInParameter(command, "LinkServerDBName", DbType.String, objItemVO.LinkServerDBName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;
        }


        #region Added by sarang
        public override IValueObject GetStores(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            clsGetStoresBizActionVO objBizAction = valueObject as clsGetStoresBizActionVO;
            //objBizAction.ItemDetails = new clsItemMasterVO();
            DbCommand command;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetStoreByClinicID");
                dbServer.AddInParameter(command, "ClinicID", DbType.Int64, objBizAction.ItemDetails.ClinicID);
                //dbServer.AddInParameter(command, "DefaultClinicID", DbType.Int64, objBizAction.ItemDetails.DefaultClinicID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objBizAction.ItemDetails.ItemID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objItemMaster = new clsItemMasterVO();
                        // objItemMaster.StoreID = (long)DALHelper.HandleDBNull(reader["Id"]);
                        objItemMaster.StoreName = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        // objItemMaster.ClinicID = (long)DALHelper.HandleDBNull(reader["ClinicID"]);
                        // objItemMaster.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        // objItemMaster.ClinicName = (string)DALHelper.HandleDBNull(reader["ClinicName"]);




                        objBizAction.ItemList.Add(objItemMaster);
                    }
                }
                reader.NextResult();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return valueObject;

        }
        #endregion


        public override IValueObject AddOpeningBalanceDetail(IValueObject valueObject, clsUserVO userVO)
        {
            //clsItemMasterVO objItemVO = null;
            clsAddOpeningBalanceDetailBizActionVO objItem = valueObject as clsAddOpeningBalanceDetailBizActionVO;
            //try
            //{
            //    DbCommand command;
            //    objItemVO = objItem.ItemMatserDetails;

            //    command = dbServer.GetStoredProcCommand("CIMS_AddOpeningBalanceMaster");






            //    dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
            //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
            //    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
            //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
            //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
            //    dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
            //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
            //    dbServer.AddInParameter(command, "LinkServerName", DbType.String, objItemVO.LinkServerName);
            //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objItemVO.LinkServerAlias);
            //    dbServer.AddInParameter(command, "LinkServerDBName", DbType.String, objItemVO.LinkServerDBName);
            //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
            //    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
            //    dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
            //    int intStatus = dbServer.ExecuteNonQuery(command);
            //    objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.Contains("duplicate"))
            //    {
            //        objItemVO.PrimaryKeyViolationError = true;
            //    }
            //    else
            //    {
            //        objItemVO.GeneralError = true;
            //    }


            //}
            return objItem;
        }

        #region Created by shikha
        public override IValueObject AddUpdateSupplier(IValueObject valueObject, clsUserVO userVO)
        {
            SupplierVO objItemVO = new SupplierVO();
            clsAddUpdateSupplierBizActionVO objItem = valueObject as clsAddUpdateSupplierBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];


                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSupplier");
                dbServer.AddInParameter(command, "SupplierId", DbType.Int64, objItemVO.SupplierId);
                dbServer.AddInParameter(command, "MFlag", DbType.Boolean, objItemVO.MFlag);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitId);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);



                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.SupplierName);
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, objItemVO.Address1);
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, objItemVO.Address2);
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, objItemVO.Address3);


                dbServer.AddInParameter(command, "Country", DbType.String, objItemVO.Country);
                dbServer.AddInParameter(command, "State", DbType.String, objItemVO.State);
                dbServer.AddInParameter(command, "District", DbType.String, objItemVO.District);
                dbServer.AddInParameter(command, "City", DbType.String, objItemVO.City);
                //dbServer.AddInParameter(command, "Zone", DbType.String, objItemVO.Area);
                //dbServer.AddInParameter(command, "Area", DbType.String, objItemVO.AddressLocation6ID);
                dbServer.AddInParameter(command, "Area", DbType.String, objItemVO.Area);
                dbServer.AddInParameter(command, "Pincode", DbType.String, objItemVO.Pincode);
                dbServer.AddInParameter(command, "ContactPerson1Name", DbType.String, objItemVO.ContactPerson1Name);
                dbServer.AddInParameter(command, "ContactPerson1MobileNo", DbType.String, objItemVO.ContactPerson1MobileNo);
                dbServer.AddInParameter(command, "ContactPerson1EmailId", DbType.String, objItemVO.ContactPerson1Email);
                dbServer.AddInParameter(command, "ContactPerson2Name", DbType.String, objItemVO.ContactPerson2Name);
                dbServer.AddInParameter(command, "ContactPerson2MobileNo", DbType.String, objItemVO.ContactPerson2MobileNo);
                dbServer.AddInParameter(command, "ContactPerson2EmailId", DbType.String, objItemVO.ContactPerson2Email);
                dbServer.AddInParameter(command, "PhoneNo", DbType.String, objItemVO.PhoneNo);


                dbServer.AddInParameter(command, "Fax", DbType.String, objItemVO.Fax);
                dbServer.AddInParameter(command, "ModeofPayment", DbType.String, objItemVO.ModeOfPayment);
                dbServer.AddInParameter(command, "TaxNature", DbType.String, objItemVO.TaxNature);
                dbServer.AddInParameter(command, "TermofPayment", DbType.String, objItemVO.TermofPayment);
                dbServer.AddInParameter(command, "Currency", DbType.String, objItemVO.Currency);
                dbServer.AddInParameter(command, "MSTNumber", DbType.String, objItemVO.MSTNumber);
                dbServer.AddInParameter(command, "VATNumber", DbType.String, objItemVO.VAT);
                dbServer.AddInParameter(command, "CSTNumber", DbType.String, objItemVO.CSTNumber);
                dbServer.AddInParameter(command, "DrugLicenceNumber", DbType.String, objItemVO.DRUGLicence);

                dbServer.AddInParameter(command, "ServiceTaxNumber", DbType.String, objItemVO.ServiceTaxNumber);
                dbServer.AddInParameter(command, "PANNumber", DbType.String, objItemVO.PANNumber);

                dbServer.AddInParameter(command, "Depreciation", DbType.String, objItemVO.Depreciation);
                dbServer.AddInParameter(command, "RatingSystem", DbType.String, objItemVO.RatingSystem);
                dbServer.AddInParameter(command, "SupplierCategoryId", DbType.Int64, objItemVO.SupplierCategoryId);

                dbServer.AddInParameter(command, "POAutoCloseDays", DbType.Int32, objItemVO.POAutoCloseDays);     //added on 11Oct2018 for parameter to set PO Auto Close Duration for Supplier 

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);

                dbServer.AddInParameter(command, "GSTINNo", DbType.String, objItemVO.GSTINNo);//Added By Bhushanp For GST 19062017

                dbServer.AddInParameter(command, "IsFertilityPoint", DbType.Boolean, objItemVO.IsFertilityPoint); //***//19

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);

                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }

            }

            return objItem;

        }




        public override IValueObject GetSupplierDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            ClsGetSupplierDetailsBizActionVO objItem = valueObject as ClsGetSupplierDetailsBizActionVO;
            SupplierVO objItemVO = null;
            try
            {

                if (objItem.SupplierPaymentMode == false)
                {
                    DbCommand command;
                    command = dbServer.GetStoredProcCommand("CIMS_GetSupplierDetails");
                    dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItemVO = new SupplierVO();
                            objItemVO.SupplierId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            objItemVO.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            objItemVO.Address1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"]));
                            objItemVO.Address2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"]));
                            objItemVO.Address3 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"]));
                            objItemVO.City = Convert.ToInt64(DALHelper.HandleDBNull(reader["City"]));
                            objItemVO.State = Convert.ToInt64(DALHelper.HandleDBNull(reader["State"]));
                            objItemVO.Country = Convert.ToInt64(DALHelper.HandleDBNull(reader["Country"]));
                            objItemVO.City = Convert.ToInt64(DALHelper.HandleDBNull(reader["City"]));
                            objItemVO.District = Convert.ToInt64(DALHelper.HandleDBNull(reader["District"]));
                            objItemVO.Zone = Convert.ToInt64(DALHelper.HandleDBNull(reader["Area"]));
                            //objItemVO.Area = Convert.ToInt64(DALHelper.HandleDBNull(reader["Zone"]));
                            objItemVO.Area = Convert.ToInt64(DALHelper.HandleDBNull(reader["Area"]));


                            objItemVO.Pincode = Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"]));
                            objItemVO.ContactPerson1Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1Name"]));
                            objItemVO.ContactPerson1MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1MobileNo"]));
                            objItemVO.ContactPerson1Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1EmailId"]));
                            objItemVO.ContactPerson2Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2Name"]));
                            objItemVO.ContactPerson2MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2MobileNo"]));
                            objItemVO.ContactPerson2Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2EmailId"]));
                            objItemVO.PhoneNo = Convert.ToString(DALHelper.HandleDBNull(reader["PhoneNo"]));
                            objItemVO.Fax = Convert.ToString(DALHelper.HandleDBNull(reader["Fax"]));
                            objItemVO.ModeOfPayment = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModeofPayment"]));
                            objItemVO.TermofPayment = Convert.ToInt64(DALHelper.HandleDBNull(reader["TermofPayment"]));
                            objItemVO.TaxNature = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxNature"]));
                            objItemVO.Currency = Convert.ToInt64(DALHelper.HandleDBNull(reader["Currency"]));
                            objItemVO.MSTNumber = Convert.ToString(DALHelper.HandleDBNull(reader["MSTNumber"]));
                            objItemVO.CSTNumber = Convert.ToString(DALHelper.HandleDBNull(reader["CSTNumber"]));
                            objItemVO.VAT = Convert.ToString(DALHelper.HandleDBNull(reader["VATNumber"]));
                            objItemVO.DRUGLicence = Convert.ToString(DALHelper.HandleDBNull(reader["DrugLicenceNumber"]));
                            objItemVO.ServiceTaxNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceTaxNumber"]));
                            objItemVO.PANNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PANNumber"]));
                            objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            objItemVO.GSTINNo = Convert.ToString(DALHelper.HandleDBNull(reader["GSTINNo"]));
                            objItemVO.Depreciation = Convert.ToString(DALHelper.HandleDBNull(reader["Depreciation"]));
                            objItemVO.RatingSystem = Convert.ToString(DALHelper.HandleDBNull(reader["RatingSystem"]));
                            objItemVO.SupplierCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierCategoryID"]));
                            objItemVO.POAutoCloseDays = Convert.ToInt32(DALHelper.HandleDBNull(reader["POAutoCloseDays"]));        //added on 11Oct2018 for parameter to get PO Auto Close Duration for Supplier

                            objItem.ItemMatserDetails.Add(objItemVO);
                        }
                    }
                    reader.NextResult();
                    objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
                }
                else
                {
                    DbCommand command;
                    command = dbServer.GetStoredProcCommand("CIMS_GetSupplierPaymentMode");
                    dbServer.AddInParameter(command, "SupplierId", DbType.String, objItem.SupplierId);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItem.ModeOfPayment = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModeofPayment"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }

        public override IValueObject clsUpdateCentralStoreDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsStoreVO objStoreVO = new clsStoreVO();
            clsUpdateCentralStoreDetailsBizActionVO objStore = valueObject as clsUpdateCentralStoreDetailsBizActionVO;
            try
            {
                DbCommand command;
                objStoreVO = objStore.StoreMasterDetails;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateCentralStoreDetails");
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objStoreVO.StoreId);
                dbServer.AddInParameter(command, "Description", DbType.String, objStoreVO.StoreName);
                dbServer.AddInParameter(command, "ClinicId", DbType.Int64, objStoreVO.ClinicId);
                dbServer.AddInParameter(command, "Code", DbType.String, objStoreVO.Code);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objStoreVO.Status);
                dbServer.AddInParameter(command, "IsCentralStore", DbType.Boolean, objStoreVO.isCentralStore);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objStoreVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objStoreVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objStoreVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objStoreVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objStoreVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objStore.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }

            return objStore;
        }

        public override IValueObject clsAddUpdateStoreDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsStoreVO objItemVO = new clsStoreVO();
            clsAddUpdateStoreDetailsBizActionVO objItem = valueObject as clsAddUpdateStoreDetailsBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateStore");
                dbServer.AddParameter(command, "StoreId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.StoreId);
                //dbServer.AddParameter(command, "StoreId", DbType.Int64, objItemVO.StoreId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.StoreName);
                dbServer.AddInParameter(command, "CostCenterCode", DbType.Int64, objItemVO.CostCenterCodeID);
                dbServer.AddInParameter(command, "ClinicId", DbType.Int64, objItemVO.ClinicId);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "OpeningBalance", DbType.Boolean, objItemVO.OpeningBalance);
                dbServer.AddInParameter(command, "Indent", DbType.Boolean, objItemVO.Indent);
                dbServer.AddInParameter(command, "Issue", DbType.Boolean, objItemVO.Issue);
                dbServer.AddInParameter(command, "IsQuarantineStore", DbType.Boolean, objItemVO.IsQuarantineStore);
                dbServer.AddInParameter(command, "ItemReturn", DbType.Boolean, objItemVO.ItemReturn);
                dbServer.AddInParameter(command, "GoodsReceivedNote", DbType.Boolean, objItemVO.GoodsReceivedNote);
                dbServer.AddInParameter(command, "GRNReturn", DbType.Boolean, objItemVO.GRNReturn);
                dbServer.AddInParameter(command, "ItemsSale", DbType.Boolean, objItemVO.ItemsSale);
                dbServer.AddInParameter(command, "ItemsSaleReturn", DbType.Boolean, objItemVO.ItemSaleReturn);
                dbServer.AddInParameter(command, "ExpiryItemReturn", DbType.Boolean, objItemVO.ExpiryItemReturn);
                dbServer.AddInParameter(command, "ReceiveIssue", DbType.Boolean, objItemVO.ReceiveIssue);
                dbServer.AddInParameter(command, "ReceiveIssueReturn", DbType.Boolean, objItemVO.ReceiveItemReturn);
                dbServer.AddInParameter(command, "Isparent", DbType.Int64, objItemVO.Parent);
                dbServer.AddInParameter(command, "ItemCatagoryID", DbType.String, objItemVO.ItemCatagoryID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "IsCentralStore", DbType.Boolean, objItemVO.isCentralStore);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);

                dbServer.AddInParameter(command, "ApplyAllItems", DbType.Boolean, objItem.ApplyallItems);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                //dbServer.AddOutParameter(command, "StoreId", DbType.Int64, 0);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                objItem.StoreID = Convert.ToInt64(dbServer.GetParameterValue(command, "StoreId"));

                DbCommand command1;
                if (objItem.ApplyallItems != null && objItem.SuccessStatus == 1)
                {
                    if (objItem.ApplyallItems == true)
                    {
                        command1 = dbServer.GetStoredProcCommand("CIMS_DeleteStoreAndCategoriesRelation");
                        // dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                        //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.ID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, 1);
                        dbServer.AddInParameter(command1, "StoreId", DbType.Int64, objItem.StoreID);
                        dbServer.AddInParameter(command1, "IsForCategories", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "IsForAll", DbType.Boolean, false);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1);

                        command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreAndCategoriesRelation");
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, 1);
                        dbServer.AddInParameter(command1, "StoreId", DbType.Int64, objItem.StoreID);
                        dbServer.AddInParameter(command1, "IsForAll", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "IsForCategories", DbType.Boolean, false);
                        dbServer.AddInParameter(command1, "CategotyID", DbType.Int64, 0);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                        int intStatus3 = dbServer.ExecuteNonQuery(command1);
                        objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command1, "ResultStatus"));
                    }
                    else
                    {

                        command1 = dbServer.GetStoredProcCommand("CIMS_DeleteStoreAndCategoriesRelation");
                        // dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                        //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.ID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, 1);
                        dbServer.AddInParameter(command1, "StoreId", DbType.Int64, objItem.StoreID);
                        dbServer.AddInParameter(command1, "IsForCategories", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "IsForAll", DbType.Boolean, false);
                        int intStatus4 = dbServer.ExecuteNonQuery(command1);

                        foreach (Int64 item in objItem.ItemCategoryID)
                        {
                            command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreAndCategoriesRelation");
                            dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                            //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.ID);
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, 1);
                            dbServer.AddInParameter(command1, "StoreId", DbType.Int64, objItem.StoreID);
                            dbServer.AddInParameter(command1, "IsForCategories", DbType.Boolean, true);
                            dbServer.AddInParameter(command1, "IsForAll", DbType.Boolean, false);
                            dbServer.AddInParameter(command1, "CategotyID", DbType.Int64, item);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                            int intStatus2 = dbServer.ExecuteNonQuery(command1);
                            objItem.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                            objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command1, "ResultStatus"));


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }

        public override IValueObject clsUpdateStoreStatus(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddUpdateStoreDetailsBizActionVO objItem = valueObject as clsAddUpdateStoreDetailsBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateStoreStatus");
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objItem.StoreID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItem.Status);

                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {

            }
            return objItem;

        }


        public override IValueObject clsGetStoreDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoreDetailsBizActionVO objItem = valueObject as clsGetStoreDetailsBizActionVO;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetStoreDetails");

                dbServer.AddInParameter(command, "StoreType", DbType.Int32, objItem.StoreType);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    clsStoreVO objItemVO = null;
                    while (reader.Read())
                    {
                        objItemVO = new clsStoreVO();
                        objItemVO.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        objItemVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        objItemVO.IsQuarantineStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsQuarantineStore"]));
                        if (objItemVO.IsQuarantineStore == true)
                            objItemVO.QuarantineDescription = "YES";
                        else
                            objItemVO.QuarantineDescription = "NO";

                        objItemVO.OpeningBalance = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OpeningBalance"]));
                        objItemVO.Indent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Indent"]));
                        objItemVO.Issue = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Issue"]));
                        objItemVO.ItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ItemReturn"]));
                        objItemVO.Parent = Convert.ToInt64(DALHelper.HandleDBNull(reader["Isparent"]));
                        if (Convert.ToString(DALHelper.HandleDBNull(reader["ParentStore"])) != "")
                            objItemVO.ParentName = Convert.ToString(DALHelper.HandleDBNull(reader["ParentStore"]));
                        else
                            objItemVO.ParentName = "NA";
                        objItemVO.GoodsReceivedNote = Convert.ToBoolean(DALHelper.HandleDBNull(reader["GoodsReceivedNote"]));
                        objItemVO.GRNReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["GRNReturn"]));
                        objItemVO.ItemsSale = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ItemsSale"]));
                        objItemVO.ItemSaleReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ItemsSaleReturn"]));
                        objItemVO.ExpiryItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ExpiryItemReturn"]));
                        objItemVO.ReceiveIssue = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ReceiveIssue"]));
                        objItemVO.ReceiveItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ReceiveIssueReturn"]));

                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.isCentralStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCentralStore"]));
                        objItemVO.StateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StateID"]));//Added By Bhushanp For GST 21062017

                        objItem.ItemMatserDetails.Add(objItemVO);
                    }


                }
                if (objItem.IsUserwiseStores)
                {
                    DbDataReader reader1 = null;
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetUserWiseStoreDetails");
                    dbServer.AddInParameter(command1, "UserID", DbType.Int64, objItem.UserID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objItem.UnitID);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, objItem.ItemID);

                    reader1 = (DbDataReader)dbServer.ExecuteReader(command1);
                    clsStoreVO objStoreVO = null;
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            objStoreVO = new clsStoreVO();
                            objStoreVO.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader1["Id"]));
                            objStoreVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader1["Code"]));
                            objStoreVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader1["Description"]));
                            objStoreVO.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ClinicId"]));
                            objStoreVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader1["ClinicName"]));
                            objStoreVO.IsQuarantineStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["IsQuarantineStore"]));
                            if (objStoreVO.IsQuarantineStore == true)
                                objStoreVO.QuarantineDescription = "YES";
                            else
                                objStoreVO.QuarantineDescription = "NO";
                            objStoreVO.OpeningBalance = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["OpeningBalance"]));
                            objStoreVO.Indent = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["Indent"]));
                            objStoreVO.Issue = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["Issue"]));
                            objStoreVO.ItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["ItemReturn"]));
                            objStoreVO.Parent = Convert.ToInt64(DALHelper.HandleDBNull(reader1["Isparent"]));
                            if (Convert.ToString(DALHelper.HandleDBNull(reader1["ParentStore"])) != "")
                                objStoreVO.ParentName = Convert.ToString(DALHelper.HandleDBNull(reader1["ParentStore"]));
                            else
                                objStoreVO.ParentName = "NA";
                            objStoreVO.GoodsReceivedNote = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["GoodsReceivedNote"]));
                            objStoreVO.GRNReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["GRNReturn"]));
                            objStoreVO.ItemsSale = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["ItemsSale"]));
                            objStoreVO.ItemSaleReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["ItemsSaleReturn"]));
                            objStoreVO.ExpiryItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["ExpiryItemReturn"]));
                            objStoreVO.ReceiveIssue = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["ReceiveIssue"]));
                            objStoreVO.ReceiveItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader1["ReceiveIssueReturn"]));
                            objStoreVO.StateID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["StateID"]));//Added By Bhushanp For GST 21062017
                            objItem.ToStoreList.Add(objStoreVO);

                            //objStoreVO = new clsStoreVO();
                            //objStoreVO.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader1["StoreID"]));
                            //objStoreVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader1["Code"]));
                            //objStoreVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader1["Description"]));
                            //objStoreVO.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ClinicId"]));
                            //objStoreVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader1["ClinicName"]));
                            //objItem.ToStoreList.Add(objStoreVO);
                        }
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }


        public override IValueObject clsGetStoreWithCategoryDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoreWithCategoryDetailsBizActionVO objItem = valueObject as clsGetStoreWithCategoryDetailsBizActionVO;
            clsStoreVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetStoreCategoryDetailsByStoreID");

                dbServer.AddInParameter(command, "StoreID", DbType.String, objItem.StoreId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsStoreVO();
                        // objItemVO. = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItem.IsForAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForAll"]));
                        objItemVO.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItem.IsForCategories = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForCategories"]));
                        objItemVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategotyID"]));
                        //objItem.ob
                        objItem.ItemMatserCategoryDetails.Add(objItemVO.CategoryID);
                    }
                }
                reader.NextResult();
                //objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }

        public override IValueObject clsGetValuesforScrapSalesbyItemID(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoreDetailsBizActionVO objItem = valueObject as clsGetStoreDetailsBizActionVO;
            clsStoreVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetStoreDetails");
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objItem.StoreId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsStoreVO();
                        objItemVO.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"]));
                        objItemVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.isCentralStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCentralStore"]));
                        objItem.ItemMatserDetails.Add(objItemVO);
                    }
                }
                reader.NextResult();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }

        public override IValueObject AddScrapSalesDetails(IValueObject valueObject, clsUserVO userVO)
        {

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            clsAddScrapSalesDetailsBizActionVO BizActionObj = valueObject as clsAddScrapSalesDetailsBizActionVO;

            try
            {
                con.Open();
                trans = con.BeginTransaction();


                clsSrcapVO objDetailsVO = BizActionObj.ItemMatserDetail;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddtemScrapSale");

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                userVO.UserLoginInfo.UnitId = objDetailsVO.UnitId;

                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objDetailsVO.SupplierID);
                //dbServer.AddInParameter(command, "ScrapSaleNo", DbType.String, objDetailsVO.ScrapSaleNo);
                dbServer.AddParameter(command, "ScrapSaleNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, objDetailsVO.IsApproved);
                dbServer.AddInParameter(command, "SupplierName", DbType.String, objDetailsVO.SupplierName);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);

                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                dbServer.AddInParameter(command, "TotalVatAmount", DbType.Double, objDetailsVO.TotalVatAmount);
                dbServer.AddInParameter(command, "TotalTaxAmount", DbType.Double, objDetailsVO.TotalTaxAmount);
                dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.TotalNetAmount);
                dbServer.AddInParameter(command, "ModeOfTransport", DbType.String, objDetailsVO.ModeOfTransport);
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, objDetailsVO.AddUnitID);


                dbServer.AddInParameter(command, "By", DbType.Int64, objDetailsVO.By);

                dbServer.AddInParameter(command, "On", DbType.String, objDetailsVO.On);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objDetailsVO.DateTime);

                dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, objDetailsVO.WindowsLoginName);
                dbServer.AddInParameter(command, "PaymentModeID", DbType.Int16, objDetailsVO.PaymentModeID);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ScrapID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ItemScrapSaleId = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.ItemMatserDetail.ScrapSaleNo = (string)dbServer.GetParameterValue(command, "ScrapSaleNo");



                foreach (var item in BizActionObj.ItemsDetail)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemScrapSaleDetails");

                    dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ScrapSalesItemID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                    dbServer.AddInParameter(command1, "ItemScrapSaleId", DbType.Int64, BizActionObj.ItemScrapSaleId);
                    dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command1, "BatchID", DbType.Int64, item.BatchID);
                    dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, item.BatchExpiryDate);
                    dbServer.AddInParameter(command1, "SaleQty", DbType.Double, item.SaleQty);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);
                    dbServer.AddInParameter(command1, "TotalPurchaseRate", DbType.Single, item.TotalPurchaseRate);
                    dbServer.AddInParameter(command1, "MRP", DbType.Single, item.MRP);
                    dbServer.AddInParameter(command1, "TotalAmount", DbType.Single, item.MRPAmount);
                    dbServer.AddInParameter(command1, "TaxPercentage", DbType.Double, item.ItemTax);
                    dbServer.AddInParameter(command1, "TaxAmount", DbType.Double, item.TaxAmount);
                    dbServer.AddInParameter(command1, "VATPercentage", DbType.Double, item.VATPerc);
                    dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Single, item.SaleQty * item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Single, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "BaseUMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                    dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.ConversionFactor);
                    dbServer.AddInParameter(command1, "othertaxApplicableon", DbType.Int16, item.OtherGRNItemTaxApplicationOn);
                    dbServer.AddInParameter(command1, "otherTaxType", DbType.Int16, item.OtherGRNItemTaxType);
                    dbServer.AddInParameter(command1, "VatApplicableon", DbType.Int16, item.GRNItemVatApplicationOn);
                    dbServer.AddInParameter(command1, "Vattype", DbType.Int16, item.GRNItemVatType);
                    dbServer.AddInParameter(command1, "ReceivedID", DbType.Int64, item.ReceivedID);
                    dbServer.AddInParameter(command1, "ReceivedUnitID", DbType.Int64, item.ReceivedUnitID);
                    dbServer.AddInParameter(command1, "ReceivedDetailID", DbType.Int64, item.ReceivedDetailID);
                    dbServer.AddInParameter(command1, "ReceivedDetailUnitID", DbType.Int64, item.ReceivedDetailUnitID);
                    dbServer.AddInParameter(command1, "Remark", DbType.String, item.Remark);
                    dbServer.AddInParameter(command1, "Amount", DbType.Double, item.TotalAmount);
                    dbServer.AddInParameter(command1, "ScrapRate", DbType.String, item.ScrapRate);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    if (item.ReceivedDetailID != null && item.ReceivedDetailID > 0)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                        command2.Connection = con;

                        dbServer.AddInParameter(command2, "ReceiveItemDetailsID", DbType.Int64, item.ReceivedDetailID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, item.ReceivedDetailUnitID);
                        dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.SaleQty * item.BaseConversionFactor);//(item.ReturnQty));
                        int status2 = dbServer.ExecuteNonQuery(command2, trans);
                    }

                    //item.StockDetails.BatchID = item.BatchID;
                    //item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    //item.StockDetails.ItemID = item.ItemId;
                    //item.StockDetails.TransactionTypeID = InventoryTransactionType.ScrapSale;
                    //item.StockDetails.TransactionID = BizActionObj.ItemScrapSaleId;
                    ////item.StockDetails.TransactionQuantity = (item.SaleQty);
                    //if (DALHelper.HandleDBNull(objDetailsVO.Date) == null)
                    //    item.StockDetails.Date = DateTime.Now;
                    //else
                    //    item.StockDetails.Date = Convert.ToDateTime(objDetailsVO.Date);
                    //if (DALHelper.HandleDBNull(objDetailsVO.Date) == null)
                    //    item.StockDetails.Time = DateTime.Now;
                    //else
                    //    item.StockDetails.Time = Convert.ToDateTime(objDetailsVO.Time);
                    //item.StockDetails.StoreID = objDetailsVO.StoreID;

                    //clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    //clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                    //obj.Details = item.StockDetails;
                    //obj.Details.InputTransactionQuantity = Convert.ToSingle(item.SaleQty);  // InputTransactionQuantity // For Conversion Factor   
                    //obj.Details.BaseUOMID = item.BaseUOMID;                        // Base  UOM   // For Conversion Factor
                    //obj.Details.BaseConversionFactor = item.BaseConversionFactor;  // Base Conversion Factor   // For Conversion Factor
                    //obj.Details.TransactionQuantity = item.BaseQuantity;         // Base Quantity            // For Conversion Factor
                    //obj.Details.SUOMID = item.SUOMID;                           // SUOM UOM                     // For Conversion Factor
                    //obj.Details.ConversionFactor = item.ConversionFactor;       // Stocking ConversionFactor     // For Conversion Factor
                    //obj.Details.StockingQuantity = item.SaleQty * item.ConversionFactor;  // StockingQuantity // For Conversion Factor
                    //obj.Details.SelectedUOM.ID = item.SelectedUOM.ID;          // Transaction UOM      // For Conversion Factor 

                    //obj.Details.ID = 0;
                    //obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, userVO);

                    //if (obj.SuccessStatus == -1)
                    //{
                    //    throw new Exception();
                    //}
                    //item.StockDetails.ID = obj.Details.ID;
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.ItemMatserDetail = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }

            return BizActionObj;

        }

        public override IValueObject ApproveScrapSalesDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddScrapSalesDetailsBizActionVO BizActionObj = valueObject as clsAddScrapSalesDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateScrapSaleForApprove");
                command.Connection = con;
                dbServer.AddInParameter(command, "ItemScrapSaleId", DbType.Int64, BizActionObj.ItemMatserDetail.ScrapID);
                dbServer.AddInParameter(command, "ItemScrapSaleUnitId", DbType.Int64, BizActionObj.ItemMatserDetail.UnitId);

                dbServer.AddInParameter(command, "ApproveOrRejectedBy", DbType.Int64, UserVo.ID);
                dbServer.AddParameter(command, "ReasultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ReasultStatus"));

                if (BizActionObj.SuccessStatus == -10)
                {
                    throw new Exception();
                }

                List<clsSrcapDetailsVO> objDetailsList = BizActionObj.ItemsDetail;
                if (objDetailsList.Count > 0)
                {
                    foreach (var item in objDetailsList.ToList())
                    {
                        item.StockDetails.PurchaseRate = item.Rate;
                        item.StockDetails.BatchID = Convert.ToInt64(item.BatchID);
                        item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        item.StockDetails.ItemID = item.ItemId;
                        item.StockDetails.TransactionTypeID = InventoryTransactionType.ScrapSale;
                        item.StockDetails.TransactionID = Convert.ToInt64(item.ItemScrapSaleId);
                        item.StockDetails.TransactionQuantity = item.BaseQuantity;
                        item.StockDetails.Date = DateTime.Now;
                        item.StockDetails.Time = DateTime.Now;
                        item.StockDetails.StoreID = item.StoreID;
                        item.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.SaleQty);
                        item.StockDetails.BaseUOMID = item.BaseUOMID;
                        item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                        item.StockDetails.SUOMID = item.SUOMID;
                        item.StockDetails.ConversionFactor = item.ConversionFactor;
                        item.StockDetails.StockingQuantity = item.SaleQty * item.ConversionFactor;
                        item.StockDetails.SelectedUOM.ID = item.TransactionUOMID;
                        item.StockDetails.ExpiryDate = item.BatchExpiryDate;
                        item.StockDetails.UnitId = item.UnitId;

                        clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                        obj.Details = item.StockDetails;
                        obj.Details.ID = 0;
                        obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                        if (obj.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        item.StockDetails.ID = obj.Details.ID;
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;

        }

        public override IValueObject GetScrapSalesDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetScrapSalesDetailsBizActionVO objItem = valueObject as clsGetScrapSalesDetailsBizActionVO;
            clsSrcapVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetScrapSaleDetails");

                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objItem.StoreID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objItem.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objItem.ToDate);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, userVO.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsSrcapVO();
                        objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objItemVO.ScrapID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objItemVO.Date = objItemVO.Date.Value.ToString("dd-MM-yyyy");
                        objItemVO.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                        objItemVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objItemVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.TotalTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalTaxAmount"]));
                        objItemVO.ScrapSaleNo = Convert.ToString(DALHelper.HandleDBNull(reader["ScrapSaleNo"]));
                        objItemVO.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                        if (objItemVO.IsApproved == true)
                        {
                            objItemVO.IsApprovedStatus = "Yes";
                        }
                        else
                        {
                            objItemVO.IsApprovedStatus = "No";
                        }

                        objItemVO.ModeOfTransport = Convert.ToString(DALHelper.HandleDBNull(reader["ModeOfTransport"]));

                        objItem.MasterDetail.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");


            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }

        public override IValueObject GetScrapSalesItemsDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetScrapSalesItemsDetailsBizActionVO bizActionVO = valueObject as clsGetScrapSalesItemsDetailsBizActionVO;
            clsSrcapDetailsVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetScrapSaleItemsDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, bizActionVO.ItemScrapSaleId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizActionVO.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsSrcapDetailsVO();
                        objItemVO.ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        objItemVO.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ReceivedDate"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemVO.BatchExpiryDate = (DateTime?)DALHelper.HandleDBNull(reader["ExpiryDate"]);
                        objItemVO.ReceivedQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        objItemVO.ReceivedQtyUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedQtyUOM"]));
                        objItemVO.SaleQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["SaleQty"]));
                        objItemVO.TransactionUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SaleQtyUOM"]));
                        objItemVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objItemVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objItemVO.ItemTax = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxPercentage"]));
                        objItemVO.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objItemVO.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objItemVO.VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemVO.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objItemVO.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objItemVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.ItemScrapSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemScrapSaleId"]));
                        objItemVO.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objItemVO.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        objItemVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItemVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objItemVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItemVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItemVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItemVO.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        bizActionVO.MasterDetail.Add(objItemVO);
                    }
                }
                reader.NextResult();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return bizActionVO;
        }

        public override IValueObject GetDOSReturnDetails(IValueObject valueObject, clsUserVO uservo)
        {
            return valueObject;

            //clsGetDOSReturnDetailsBizActionVO objBizAction = valueObject as clsGetDOSReturnDetailsBizActionVO;
            //DbCommand command;
            //DbDataReader reader = null;
            //try
            //{
            //    command = dbServer.GetStoredProcCommand("CIMS_GetDOSReturnDetails");  //CIMS_GetExpiredReturnDetails
            //    dbServer.AddInParameter(command, "ID", DbType.Int64, objBizAction.DosID);
            //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);


            //    reader = (DbDataReader)dbServer.ExecuteReader(command);

            //    if (reader.HasRows)
            //    {
            //        if (objBizAction.DOSMainList == null)
            //            objBizAction.DOSMainList = new clsSrcapVO();

            //        while (reader.Read())
            //        {
            //            clsSrcapVO objItem = new clsSrcapVO();
            //            objItem.ScrapID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            //            objItem.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
            //            objItem.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
            //            objItem.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));
            //            objItem.ScrapSaleNo = Convert.ToString(DALHelper.HandleDBNull(reader["ScrapSaleNo"]));

            //            objItem.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
            //            objItem.Time = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Time"]));

            //            objItem.TotalAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalAmount"]));
            //            objItem.TotalVATAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalVATAmount"]));
            //            objItem.TotalTaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalTaxAmount"]));
            //            objItem.TotalOctriAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalOctriAmount"]));
            //            objItem.OtherDeducution = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["OtherDeducution"]));
            //            objItem.NetAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["NetAmount"]));

            //            objItem.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
            //            objItem.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

            //            objItem.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
            //            objItem.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));

            //            objItem.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
            //            if (objItem.IsApproved == true)
            //            {
            //                objItem.IsApprovedStatus = "Yes";
            //            }
            //            else
            //            {
            //                objItem.IsApprovedStatus = "No";
            //            }

            //            objItem.ModeOfTransport = Convert.ToString(DALHelper.HandleDBNull(reader["ModeOfTransport"]));

            //            objBizAction.DOSMainList = objItem;

            //        }

            //    }
            //    reader.NextResult();

            //    if (reader.HasRows)
            //    {
            //        if (objBizAction.DOSItemList == null)
            //            objBizAction.DOSItemList = new List<clsSrcapDetailsVO>();

            //        while (reader.Read())
            //        {
            //            clsSrcapDetailsVO objItem = new clsSrcapDetailsVO();

            //            //objItemVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemId"]));
            //            //objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
            //            //objItemVO.SaleQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["SaleQty"]));
            //            //objItemVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
            //            //objItemVO.TaxPercentage = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxPercentage"]));
            //            //objItemVO.TaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TaxAmount"]));
            //            //objItemVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
            //            //objItemVO.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
            //            objItemVO.Currency = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
            //            //objItemVO.ScrapRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["ScrapRate"]));

            //            objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            //            objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
            //            objItem.ItemExpiryReturnID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiryReturnID"]));
            //            objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));

            //            objItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
            //            objItem.BatchExpiryDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BatchExpiryDate"]));
            //            objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));

            //            objItem.Conversion = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["Conversion"]));
            //            objItem.SaleQty = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["ReturnQty"]));
            //            objItem.PurchaseRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["PurchaseRate"]));
            //            objItem.ScrapRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["ScrapRate"]));

            //            objItem.Amount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalAmount"]));
            //            objItem.DiscountPercentage = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["DiscountPercentage"]));
            //            objItem.DiscountAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["DiscountAmount"]));
            //            objItem.OctroiAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["OctroiAmount"]));

            //            objItem.VATPercentage = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["VATPercentage"]));
            //            objItem.VATAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["VATAmount"]));
            //            objItem.TaxPercentage = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxPercentage"]));  //
            //            objItem.TaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TaxAmount"]));
            //            objItem.TotalTaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalTaxAmount"]));
            //            objItem.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));

            //            objItem.BaseQuantity = Convert.ToSingle((double)DALHelper.HandleDBNull(reader["BaseQuantity"]));      // Base Quantity            // For Conversion Factor
            //            objItem.BaseConversionFactor = Convert.ToSingle((double)DALHelper.HandleDBNull(reader["BaseCF"]));
            //            objItem.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
            //            objItem.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));         // Transaction UOM      // For Conversion Factor
            //            objItem.SelectedUOM.Description = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
            //            objItem.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));                      // Base  UOM            // For Conversion Factor
            //            objItem.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));                       // SUOM UOM                     // For Conversion Factor
            //            objItem.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));               // SUOM CF                     // For Conversion Factor
            //            objItem.BaseMRP = Convert.ToSingle((double)DALHelper.HandleDBNull(reader["MRP"]));
            //            objItem.BaseRate = Convert.ToSingle((decimal)DALHelper.HandleDBNull(reader["PurchaseRate"]));

            //            objItem.TotalPurchaseRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalPurchaseRate"]));

            //            objItem.AvailableStock = Convert.ToDouble((double)DALHelper.HandleDBNull(reader["AvailableStock"]));
            //            objItem.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));
            //            objItem.AvailableStockInBase = Convert.ToDouble((double)DALHelper.HandleDBNull(reader["AvailableStockInBase"]));

            //            objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
            //            objItem.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
            //            objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));


            //            #region For Quarantine Items (Expired, DOS)

            //            objItem.OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
            //            objItem.OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"]));
            //            objItem.GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"]));
            //            objItem.GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"]));

            //            #endregion

            //            objBizAction.ExpiredItemList.Add(objItem);
            //        }

            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //finally
            //{
            //    if (reader.IsClosed == false)
            //    {
            //        reader.Close();
            //    }


            //}
            //return objBizAction;
        }

        public override IValueObject clsAddItemsEnquiry(IValueObject valueObject, clsUserVO userVO)
        {


            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            clsAddItemsEnquiryBizActionVO BizActionObj = valueObject as clsAddItemsEnquiryBizActionVO;

            try
            {
                con.Open();
                trans = con.BeginTransaction();
                clsItemEnquiryVO objDetailsVO = BizActionObj.ItemMatserDetail;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddtemEnquiry");


                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                // dbServer.AddInParameter(command, "EnquiryNO", DbType.String, objDetailsVO.EnquiryNO);

                dbServer.AddParameter(command, "EnquiryNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "Header", DbType.String, objDetailsVO.Header);
                dbServer.AddInParameter(command, "Notes", DbType.String, objDetailsVO.Notes);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, objDetailsVO.AddUnitID);
                dbServer.AddInParameter(command, "By", DbType.Int64, objDetailsVO.By);
                dbServer.AddInParameter(command, "On", DbType.String, objDetailsVO.On);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objDetailsVO.DateTime);
                dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, objDetailsVO.WindowsLoginName);

                // dbServer.AddParameter(command, "EquryId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.EnquiryId);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ItemEnquiryId = (long)dbServer.GetParameterValue(command, "ID");

                BizActionObj.ItemMatserDetail.EnquiryNO = (string)dbServer.GetParameterValue(command, "EnquiryNO");

                foreach (var item in BizActionObj.ItemsSupplierDetail)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemsEnquirySupplier");

                    dbServer.AddInParameter(command1, "EnquiryID", DbType.Int64, BizActionObj.ItemEnquiryId);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, item.UnitId);
                    dbServer.AddInParameter(command1, "SupplierID", DbType.Int64, item.SupplierID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                }

                foreach (var item in BizActionObj.ItemsTermsConditionDetail)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemsEnquiryTermsCondtn");

                    dbServer.AddInParameter(command1, "EnquiryID", DbType.Int64, BizActionObj.ItemEnquiryId);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, item.UnitId);
                    dbServer.AddInParameter(command1, "TermsConditionID", DbType.Int64, item.TermsConditionID);
                    dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                }

                foreach (var item in BizActionObj.ItemsDetail)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemsEnquiryDetails");

                    dbServer.AddInParameter(command1, "EnquiryID", DbType.Int64, BizActionObj.ItemEnquiryId);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
                    dbServer.AddInParameter(command1, "PackSize", DbType.Double, item.PackSize);
                    dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);


                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                }

                trans.Commit();


            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.ItemMatserDetail = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }

            return BizActionObj;

        }
        public override IValueObject GetItemEnquiry(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemEnquiryBizActionVO objItem = valueObject as clsGetItemEnquiryBizActionVO;
            clsItemEnquiryVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemsEnquiry");
                dbServer.AddInParameter(command, "SupplierId", DbType.Int64, objItem.SupplierId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitId);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objItem.UserID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objItem.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objItem.ToDate);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsItemEnquiryVO();
                        objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objItemVO.EnquiryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objItemVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objItemVO.EnquiryNO = Convert.ToString(DALHelper.HandleDBNull(reader["EnquiryNO"]));
                        objItemVO.Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                        objItemVO.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objItemVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItem.ItemMatserDetail.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }
        public override IValueObject GetItemEnquiryDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemEnquiryDetailsBizActionVO bizActionVO = valueObject as clsGetItemEnquiryDetailsBizActionVO;
            clsEnquiryDetailsVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemsEnquiryDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, bizActionVO.ItemEnquiryId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizActionVO.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsEnquiryDetailsVO();

                        objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objItemVO.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        objItemVO.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        objItemVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        objItemVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        bizActionVO.ItemMatserDetail.Add(objItemVO);
                    }
                }
                reader.NextResult();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return bizActionVO;
        }
        #endregion

        public override IValueObject GetItemSearchListForWorkOrder(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetItemListForWorkOrderBizActionVO BizActionObj = valueObject as clsGetItemListForWorkOrderBizActionVO;
            try
            {
                DbCommand command;
                //if (BizActionObj.IsFromOpeningBalance)
                //    command = dbServer.GetStoredProcCommand("CIMS_GetItemListForOpeningBalanceSearch");
                //else
                command = dbServer.GetStoredProcCommand("CIMS_GetItemListForWorkOrder");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                if (BizActionObj.ItemCode != null && BizActionObj.ItemCode.Length != 0)
                    dbServer.AddInParameter(command, "ItemCode", DbType.String, BizActionObj.ItemCode);
                if (BizActionObj.ItemName != null && BizActionObj.ItemName.Length != 0)
                    dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemName);
                ;

                //if (BizActionObj.MoleculeName != null && BizActionObj.MoleculeName.Length != 0)

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //     dbServer.AddInParameter(command, "UnitID", DbType.Int64, uservo.UserLoginInfo.UnitId);



                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objVO = new clsItemMasterVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));

                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));


                        BizActionObj.ItemList.Add(objVO);

                    }

                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject GetItemSearchList(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetItemListForSearchBizActionVO BizActionObj = valueObject as clsGetItemListForSearchBizActionVO;
            try
            {
                DbCommand command;
                if (BizActionObj.IsFromOpeningBalance)
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemListForOpeningBalanceSearch");
                else
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemListForSearch");  //
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                if (BizActionObj.ItemCode != null && BizActionObj.ItemCode.Length != 0)
                    dbServer.AddInParameter(command, "ItemCode", DbType.String, BizActionObj.ItemCode);
                if (BizActionObj.ItemName != null && BizActionObj.ItemName.Length != 0)
                    dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemName);
                if (BizActionObj.BrandName != null && BizActionObj.BrandName.Length != 0)
                    dbServer.AddInParameter(command, "BrandName", DbType.String, BizActionObj.BrandName);

                //if (BizActionObj.MoleculeName != null && BizActionObj.MoleculeName.Length != 0)
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.ItemCategoryId);

                dbServer.AddInParameter(command, "ManufactureCompanyID", DbType.Int64, BizActionObj.ManufactureCompanyID);

                dbServer.AddInParameter(command, "GroupId", DbType.Int64, BizActionObj.ItemGroupId);

                dbServer.AddInParameter(command, "MoleculeName", DbType.Int64, BizActionObj.MoleculeName);

                dbServer.AddInParameter(command, "ScrapCategoryItems", DbType.Boolean, BizActionObj.ShowScrapItems);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, uservo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "ShowNonZeroStockBatches", DbType.Boolean, BizActionObj.ShowZeroStockBatches);    //to show items with > 0 AvailableStock

                dbServer.AddInParameter(command, "PlusThreeMonthFlag", DbType.Boolean, BizActionObj.ShowNotShowPlusThreeMonthExp);

                //Added by AJ Date 2/1/2017 search for package Item Only
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);
                //***//----------
                if (!BizActionObj.IsFromOpeningBalance)
                {
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                }

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objVO = new clsItemMasterVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        objVO.Status = false;
                        objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objVO.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                        objVO.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));

                        objVO.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        objVO.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));

                        objVO.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objVO.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));

                        objVO.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                        objVO.Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"]));
                        objVO.PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"]));
                        objVO.TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"]));
                        objVO.TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"]));
                        objVO.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                        objVO.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objVO.VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));

                        objVO.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));



                        objVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objVO.ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"]));

                        objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVO.ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"]));

                        if (BizActionObj.ShowZeroStockBatches == true)
                            objVO.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));

                        //objVO.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        //objVO.MoleculeName = Convert.ToInt64(DALHelper.HandleDBNull(reader["MoleculeName"]));
                        //if (BizActionObj.IsFromOpeningBalance)
                        objVO.ConversionFactor = Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        if (BizActionObj.IsFromOpeningBalance == true)
                            objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));

                        //by Anjali.............................
                        if (BizActionObj.StoreID != 0)//&& BizActionObj.ShowZeroStockBatches == false
                            objVO.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        //.......................................



                        if (BizActionObj.IsFromOpeningBalance == false && BizActionObj.StoreID != 0)
                        {
                            #region Added By Bhushanp For GST 21062017

                            objVO.SGSTPercent = (decimal)DALHelper.HandleDBNull(reader["SGSTTax"]);
                            objVO.CGSTPercent = (decimal)DALHelper.HandleDBNull(reader["CGSTTax"]);
                            objVO.IGSTPercent = (decimal)DALHelper.HandleDBNull(reader["IGSTTax"]);
                            objVO.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                            //------------------------------------------

                            objVO.SGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtype"]));
                            objVO.SGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableon"]));

                            objVO.CGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtype"]));
                            objVO.CGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableon"]));

                            objVO.IGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtype"]));
                            objVO.IGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableon"]));

                            // Begin Properties for Sale 29062017

                            objVO.SGSTPercentSale = (decimal)DALHelper.HandleDBNull(reader["SGSTTaxSale"]);
                            objVO.CGSTPercentSale = (decimal)DALHelper.HandleDBNull(reader["CGSTTaxSale"]);
                            objVO.IGSTPercentSale = (decimal)DALHelper.HandleDBNull(reader["IGSTTaxSale"]);
                            //objVO.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                            //------------------------------------------

                            objVO.SGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtypeSale"]));
                            objVO.SGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableonSale"]));

                            objVO.CGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtypeSale"]));
                            objVO.CGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableonSale"]));

                            objVO.IGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtypeSale"]));
                            objVO.IGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableonSale"]));

                            // End Properties for Sale 29062017
                         
                                objVO.TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"]));
                                objVO.StaffDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                                objVO.WalkinDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["WalkinDiscount"]));
                                objVO.RegisteredPatientsDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RegisteredPatientsDiscount"]));
                                                   

                            #endregion
                        }
                        if (BizActionObj.IsFromOpeningBalance == false && BizActionObj.StoreID != 0 && BizActionObj.IsFromStockAdjustment == false)
                        {
                            objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                            objVO.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));
                            objVO.ItemVatPer = (decimal)DALHelper.HandleDBNull(reader["othertax"]);
                            objVO.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                            objVO.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                            objVO.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                            objVO.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                            if (BizActionObj.StoreID > 0)
                            {
                                objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                                objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            }
                            //By Anjali...........................
                            objVO.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));
                            objVO.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                            objVO.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));
                            objVO.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));
                            objVO.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                            objVO.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));
                            objVO.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                            objVO.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                            if (BizActionObj.ShowZeroStockBatches != true)
                            {
                                objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                                objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                                objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                            }
                            if (BizActionObj.IsFromOpeningBalance == false && BizActionObj.PackageID > 0) //ADDED By Bhushanp For New Package Flow 1092017
                            {
                                objVO.DiscountPerc = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Discount"])); //Added By Bhushanp For New Package Flow 31082017
                            }
                            //.......................................
                        }


                        if (BizActionObj.IsFromOpeningBalance)
                        {
                            objVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                            objVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));

                            objVO.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                            objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));

                            objVO.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                            objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));

                            objVO.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));
                            objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        }
                        else
                        {
                            objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            objVO.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                        }

                        if (BizActionObj.IsFromOpeningBalance == true && BizActionObj.ShowZeroStockBatches == false)
                        {
                            objVO.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                            objVO.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));

                            objVO.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                            objVO.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));

                            objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                            objVO.InclusiveForOB = Convert.ToInt32(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                            objVO.ApplicableOnForOB = Convert.ToInt32(DALHelper.HandleDBNull(reader["VATApplicableOn"]));
                            objVO.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                        }

                        if (IsApprovedDirect.Equals("Yes"))
                            objVO.IsApprovedDirect = true;
                        else if (IsApprovedDirect.Equals("No"))
                            objVO.IsApprovedDirect = false;

                        objVO.AssignSupplier = CheckItemSupplier(objVO.ID, BizActionObj.SupplierID);
                        BizActionObj.ItemList.Add(objVO);

                    }

                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject GetPackageItemListForCounterSaleSearch(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetItemListForSearchBizActionVO BizActionObj = valueObject as clsGetItemListForSearchBizActionVO;
            try
            {
                DbCommand command;

                command = dbServer.GetStoredProcCommand("CIMS_GetPackageItemListForCounterSaleSearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                if (BizActionObj.ItemCode != null && BizActionObj.ItemCode.Length != 0)
                    dbServer.AddInParameter(command, "ItemCode", DbType.String, BizActionObj.ItemCode);
                if (BizActionObj.ItemName != null && BizActionObj.ItemName.Length != 0)
                    dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemName);
                if (BizActionObj.BrandName != null && BizActionObj.BrandName.Length != 0)
                    dbServer.AddInParameter(command, "BrandName", DbType.String, BizActionObj.BrandName);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.ItemCategoryId);
                dbServer.AddInParameter(command, "GroupId", DbType.Int64, BizActionObj.ItemGroupId);
                dbServer.AddInParameter(command, "MoleculeName", DbType.Int64, BizActionObj.MoleculeName);
                dbServer.AddInParameter(command, "ScrapCategoryItems", DbType.Boolean, BizActionObj.ShowScrapItems);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "ShowNonZeroStockBatches", DbType.Boolean, BizActionObj.ShowZeroStockBatches);    //to show items with > 0 AvailableStock

                dbServer.AddInParameter(command, "PlusThreeMonthFlag", DbType.Boolean, BizActionObj.ShowNotShowPlusThreeMonthExp);

                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objVO = new clsItemMasterVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        objVO.Status = false;
                        objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objVO.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                        objVO.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));

                        objVO.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        objVO.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));

                        objVO.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objVO.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));

                        objVO.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                        objVO.Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"]));
                        objVO.PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"]));
                        objVO.TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"]));
                        objVO.TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"]));
                        objVO.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                        objVO.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objVO.VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));

                        objVO.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objVO.ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"]));

                        objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVO.ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"]));

                        if (BizActionObj.ShowZeroStockBatches == true)
                            objVO.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.ConversionFactor = Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        if (BizActionObj.IsFromOpeningBalance == true)
                            objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        if (BizActionObj.StoreID != 0)//&& BizActionObj.ShowZeroStockBatches == false
                            objVO.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        if (BizActionObj.IsFromOpeningBalance == false && BizActionObj.StoreID != 0 && BizActionObj.IsFromStockAdjustment == false)
                        {
                            objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                            objVO.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));
                            objVO.ItemVatPer = (decimal)DALHelper.HandleDBNull(reader["othertax"]);
                            objVO.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                            objVO.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                            objVO.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                            objVO.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                            if (BizActionObj.StoreID > 0)
                            {
                                objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                                objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            }
                            objVO.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));
                            objVO.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                            objVO.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));
                            objVO.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));
                            objVO.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                            objVO.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));
                            objVO.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                            objVO.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                            if (BizActionObj.ShowZeroStockBatches != true)
                            {
                                objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                                objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                                objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                            }
                        }
                        objVO.AssignSupplier = CheckItemSupplier(objVO.ID, BizActionObj.SupplierID);
                        objVO.Budget = Convert.ToSingle(DALHelper.HandleDBNull(reader["Budget"]));
                        objVO.TotalBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalBudget"]));


                        BizActionObj.ItemList.Add(objVO);
                    }

                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }


        public override IValueObject GetItemOtherDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;

            clsGetItemMasterOtherDetailsBizActionVO objBizAction = valueObject as clsGetItemMasterOtherDetailsBizActionVO;

            DbCommand command;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetItemOtherDetails");
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objBizAction.ItemID);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemOtherDetails == null)
                        objBizAction.ItemOtherDetails = new clsItemMasterOtherDetailsVO();
                    while (reader.Read())
                    {
                        objBizAction.ItemOtherDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objBizAction.ItemOtherDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objBizAction.ItemOtherDetails.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objBizAction.ItemOtherDetails.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objBizAction.ItemOtherDetails.Contradiction = (string)DALHelper.HandleDBNull(reader["Contradiction"]);
                        objBizAction.ItemOtherDetails.SideEffect = (string)DALHelper.HandleDBNull(reader["SideEffect"]);
                        objBizAction.ItemOtherDetails.URL = (string)DALHelper.HandleDBNull(reader["URL"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return valueObject;
        }
        public override IValueObject AddUpdateItemOtherDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddUpdateItemMasterOtherDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateItemMasterOtherDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemOtherDetails");

                dbServer.AddInParameter(command1, "ID", DbType.Int64, BizActionObj.ItemOtherDetails.ID);
                dbServer.AddInParameter(command1, "UnitId", DbType.Int64, userVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "ItemID", DbType.Int64, BizActionObj.ItemOtherDetails.ItemID);
                dbServer.AddInParameter(command1, "Contradiction", DbType.String, BizActionObj.ItemOtherDetails.Contradiction);
                dbServer.AddInParameter(command1, "SideEffect", DbType.String, BizActionObj.ItemOtherDetails.SideEffect);
                dbServer.AddInParameter(command1, "URL", DbType.String, BizActionObj.ItemOtherDetails.URL);

                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
            }

            return BizActionObj;

        }
        public override IValueObject GetStockForStockAdjustment(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;

            clsGetStockDetailsForStockAdjustmentBizActionVO objBizAction = valueObject as clsGetStockDetailsForStockAdjustmentBizActionVO;
            objBizAction.StockList = new List<clsAdjustmentStockVO>();
            DbCommand command;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetAvailableStockForStockAdjustment");
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objBizAction.StoreID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                //SELECT		dbo.M_ItemMaster.ItemCode,
                //                 dbo.M_ItemMaster.ID AS ItemId,
                //                 dbo.M_ItemMaster.ItemName,
                //                 dbo.M_ItemBatchMaster.ID AS BatchaId, 
                //                 dbo.M_ItemBatchMaster.BatchCode, dbo.M_ItemBatchMaster.ExpiryDate,
                //                 (SELECT     TOP (1) AvailableStock FROM  dbo.T_ItemStock WHERE (StoreID =@StoreId ) AND (ItemID = dbo.M_ItemMaster.ID) AND (BatchID = dbo.M_ItemBatchMaster.ID) ORDER BY ID DESC) AS AvailableStock, 
                //                 (SELECT     TOP (1) ID FROM  dbo.T_ItemStock WHERE (StoreID =@StoreId ) AND (ItemID = dbo.M_ItemMaster.ID) AND (BatchID = dbo.M_ItemBatchMaster.ID) ORDER BY ID DESC) AS StockId,
                //                 dbo.T_ItemClinic.StoreId,
                //                 dbo.M_Store.Description As Store

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsAdjustmentStockVO stockItem = new clsAdjustmentStockVO();
                        stockItem.StockId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockId"]));
                        stockItem.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        stockItem.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        stockItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        stockItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        stockItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        stockItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        stockItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        stockItem.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));
                        stockItem.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));

                        objBizAction.StockList.Add(stockItem);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }
        public override IValueObject AddStockAdjustment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddStockAdjustmentBizActionVO BizActionObj = valueObject as clsAddStockAdjustmentBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();


                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddItemStockAdjustmentMain");
                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objMainStock.ID);
                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "StoreID", DbType.Int64, BizActionObj.objMainStock.StoreID);
                dbServer.AddInParameter(command2, "RequestDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "Remark", DbType.String, BizActionObj.objMainStock.Remark);
                dbServer.AddInParameter(command2, "RequestedBy", DbType.Int64, BizActionObj.objMainStock.RequestedBy);
                dbServer.AddInParameter(command2, "IsApproved", DbType.Boolean, BizActionObj.objMainStock.IsApproved);
                dbServer.AddInParameter(command2, "IsFromPST", DbType.Boolean, BizActionObj.objMainStock.IsFromPST);
                dbServer.AddInParameter(command2, "PhysicalItemID", DbType.Int64, BizActionObj.objMainStock.PhysicalItemID);
                dbServer.AddInParameter(command2, "PhysicalItemUnitID", DbType.Int64, BizActionObj.objMainStock.PhysicalItemUnitID);
                dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                dbServer.AddParameter(command2, "StockAdjustmentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                BizActionObj.objMainStock.ID = (long)dbServer.GetParameterValue(command2, "ID");
                BizActionObj.objMainStock.UnitID = UserVo.UserLoginInfo.UnitId;
                BizActionObj.objMainStock.StockAdjustmentNo = (string)dbServer.GetParameterValue(command2, "StockAdjustmentNo");



                foreach (var item in BizActionObj.StockAdustmentItems)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemStockAdjustmentDetails");
                    if (item.RadioStatusNo == true)
                    {
                        item.OperationType = InventoryStockOperationType.Subtraction;
                    }
                    if (item.RadioStatusYes == true)
                    {
                        item.OperationType = InventoryStockOperationType.Addition;
                    }
                    //dbServer.AddParameter(command1, "StockAdjustmentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command1, "BatchId", DbType.Int64, item.BatchId);

                    dbServer.AddInParameter(command1, "StockAdjustmentID", DbType.Int64, BizActionObj.objMainStock.ID);
                    dbServer.AddInParameter(command1, "StockAdjustmentUnitID", DbType.Int64, BizActionObj.objMainStock.UnitID);

                    dbServer.AddInParameter(command1, "AdjustmentQuantity", DbType.Double, item.AdjustmentQunatitiy);
                    dbServer.AddInParameter(command1, "CurrentBalance", DbType.Double, item.AvailableStock);
                    dbServer.AddInParameter(command1, "CurrentBalanceInBase", DbType.Double, item.AvailableStockInBase);
                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, BizActionObj.DateTime);
                    dbServer.AddInParameter(command1, "Time", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command1, "StoreID", DbType.Int64, item.StoreID);
                    dbServer.AddInParameter(command1, "OperationType", DbType.Int16, (Int16)item.OperationType);
                    dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);

                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, BizActionObj.DateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    // For Conversion Factor By Umesh
                    dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUMID);
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.StockingUMID);
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, item.AdjustmentQunatitiy * item.ConversionFactor); //dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, item.Quantity);
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Double, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Double, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "StockCF", DbType.Double, item.ConversionFactor);

                    //  END
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    BizActionObj.StockAdjustmentId = (long)dbServer.GetParameterValue(command1, "ID");
                    //item.StockAdjustmentNo = (string)dbServer.GetParameterValue(command1, "StockAdjustmentNo");

                    //BizActionObj.StockAdustmentItem.StockDetails.ItemID = item.ItemId;
                    //BizActionObj.StockAdustmentItem.StockDetails.BatchID = Convert.ToInt64(item.BatchId);
                    //BizActionObj.StockAdustmentItem.StockDetails.TransactionTypeID = InventoryTransactionType.StockAdujustment;
                    //BizActionObj.StockAdustmentItem.StockDetails.TransactionQuantity = item.AdjustmentQunatitiy;
                    //BizActionObj.StockAdustmentItem.StockDetails.TransactionID = BizActionObj.StockAdjustmentId;
                    //BizActionObj.StockAdustmentItem.StockDetails.Date = BizActionObj.DateTime;
                    //BizActionObj.StockAdustmentItem.StockDetails.Time = System.DateTime.Now;
                    //BizActionObj.StockAdustmentItem.StockDetails.OperationType = item.OperationType;
                    //BizActionObj.StockAdustmentItem.StockDetails.StoreID = BizActionObj.StoreId;
                    //BizActionObj.StockAdustmentItem.StockDetails.UnitId = BizActionObj.UnitID;

                    //clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    //clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();


                    //obj.Details = BizActionObj.StockAdustmentItem.StockDetails;
                    //obj.Details.ID = 0;
                    //obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo);

                    //if (obj.SuccessStatus == -1)
                    //{
                    //    throw new Exception();
                    //}

                    //BizActionObj.StockAdustmentItem.StockDetails.ID = obj.Details.ID;

                }


                if (BizActionObj.objMainStock.IsFromPST == true)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdatePhysicalStockItem");
                    dbServer.AddInParameter(command3, "PhysicalItemID", DbType.Int64, BizActionObj.objMainStock.PhysicalItemID);
                    dbServer.AddInParameter(command3, "PhysicalItemUnitID", DbType.Int64, BizActionObj.objMainStock.PhysicalItemUnitID);
                    dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command3, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                }


                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.StockAdustmentItem = null;
                BizActionObj.StockAdustmentItems = null;
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
            }

            return BizActionObj;
        }
        public override IValueObject GetStockAdjustmentList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;

            clsGetStockAdustmentListBizActionVO objBizAction = valueObject as clsGetStockAdustmentListBizActionVO;
            objBizAction.AdjustStock = new List<clsAdjustmentStockVO>();
            DbCommand command;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetStockAdjustmentDetails");
                //dbServer.AddInParameter(command, "StoreId", DbType.Int64, objBizAction.StoreID);
                //dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                //dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objBizAction.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objBizAction.MaximumRows);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                dbServer.AddInParameter(command, "StockAdjustmentID", DbType.Int64, objBizAction.StockAdjustmentID);
                dbServer.AddInParameter(command, "StockAdjustmentUnitID", DbType.Int64, objBizAction.StockAdjustmentUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {


                    while (reader.Read())
                    {
                        clsAdjustmentStockVO stockItem = new clsAdjustmentStockVO();
                        stockItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        stockItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        stockItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        stockItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        stockItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchiD"]));
                        stockItem.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        stockItem.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        stockItem.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentBalance"]));
                        //stockItem.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        stockItem.AdjustmentQunatitiy = Convert.ToDouble(DALHelper.HandleDBNull(reader["AdjustmentQuantity"]));
                        stockItem.DateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        stockItem.Time = String.Format("{0:T}", stockItem.DateTime);
                        stockItem.DateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        stockItem.StockAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["StockAdjustmentNo"]));
                        stockItem.intOperationType = Convert.ToInt16(DALHelper.HandleDBNull((reader["OperationType"])));
                        stockItem.StockingQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingQuantity"]));
                        stockItem.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        stockItem.BaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        stockItem.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        stockItem.StockingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        stockItem.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        stockItem.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        stockItem.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));

                        if (stockItem.intOperationType == 1)
                        {
                            stockItem.stOperationType = InventoryStockOperationType.Addition.ToString();
                        }
                        else if (stockItem.intOperationType == 2)
                        {
                            stockItem.stOperationType = InventoryStockOperationType.Subtraction.ToString();
                        }
                        else
                        {
                            stockItem.stOperationType = InventoryStockOperationType.None.ToString();
                        }



                        objBizAction.AdjustStock.Add(stockItem);
                    }
                    reader.NextResult();
                    // objBizAction.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }
        public bool CheckItemSupplier(long ItemID, long SupplierID)
        {
            //clsCheckItemSupplierFromGRNBizActionVO BizActionObj = valueObject as clsCheckItemSupplierFromGRNBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckItemSupplier");
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, ItemID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, SupplierID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                int check;
                check = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (check == 1)
                    return true;
                else
                    return false;

            }
            catch (Exception Ex)
            {
                throw;
            }

        }

        // From City Clinic

        //public override IValueObject GetBarCodeCounterSale(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsCounterSaleBarCodeBizActionVO BizActionobj = valueObject as clsCounterSaleBarCodeBizActionVO;
        //    DbCommand command;
        //    DbDataReader reader = null;
        //    //
        //    try
        //    {

        //        command = dbServer.GetStoredProcCommand("CIMS_GetItemListForBarCode_CounterSale");
        //        dbServer.AddInParameter(command, "ItemCode", DbType.String, BizActionobj.ItemCode);
        //        dbServer.AddInParameter(command, "BatchCode", DbType.String, BizActionobj.BatchCode);
        //        dbServer.AddInParameter(command, "BarCode", DbType.String, BizActionobj.BarCode);
        //        dbServer.AddInParameter(command, "BatchID", DbType.Int64, BizActionobj.BatchID);
        //        dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionobj.ItemID);
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionobj.UnitID);
        //        dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionobj.StoreId);
        //        reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                clsItemSalesDetailsVO objItemBarCode = new clsItemSalesDetailsVO();
        //                objItemBarCode.ItemID = Convert.ToInt32(DALHelper.HandleDBNull(reader["ID"]));
        //                objItemBarCode.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
        //                objItemBarCode.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
        //                objItemBarCode.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                objItemBarCode.ExpiryDate = (DateTime?)(DALHelper.HandleDBNull(reader["ExpiryDate"]));
        //                objItemBarCode.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableQuantity"]));
        //                objItemBarCode.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                objItemBarCode.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
        //                //objItemBarCode. = Convert.ToInt64(DALHelper.HandleDoubleNull(reader["StoreID"]));
        //                objItemBarCode.VATPercent = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["VatPercentage"]));
        //                objItemBarCode.VATAmount = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["VatAmount"]));
        //                objItemBarCode.PurchaseRate = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["PurchaseRate"]));
        //                // objItemBarCode.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CostPrize"]));
        //                objItemBarCode.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
        //                // objItemBarCode.StockDetails = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
        //                //   objItemBarCode.MRP = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostPrize"]));
        //                // objItemBarCode.PurchaseRate = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostPrize"]));
        //                objItemBarCode.PregnancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PregnancyClass"]));
        //                objItemBarCode.Manufacture = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"]));

        //                BizActionobj.IssueList.Add(objItemBarCode);
        //                //BizActionobj.IssueList.Add(objItemBarCode);

        //            }
        //            reader.NextResult();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader.IsClosed == false)
        //        {
        //            reader.Close();
        //        }
        //    }
        //    return BizActionobj;

        //}

        ///// New From Health Spring  ///// 






        public override IValueObject AddMRPAdjustment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMRPAdjustmentBizActionVO BizActionObj = valueObject as clsAddMRPAdjustmentBizActionVO;
            if (BizActionObj.MRPAdjustmentMainVO == null) BizActionObj.MRPAdjustmentMainVO = new clsMRPAdjustmentMainVO();
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddMRPAdjustmentMain");
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.MRPAdjustmentMainVO.StoreID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "MRPAdjustmentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.MRPAdjustmentMainVO.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.MRPAdjustmentMainVO.MRPAdjustmentNo = (string)dbServer.GetParameterValue(command, "MRPAdjustmentNo");

                foreach (var item in BizActionObj.MRPAdjustmentItems)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddMRPAdjustmentDetails");
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "MRPAdjustmentMainID", DbType.Int64, BizActionObj.MRPAdjustmentMainVO.ID);


                    //dbServer.AddParameter(command1, "MRPAdjustmentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command1, "BatchId", DbType.Int64, item.BatchId);
                    dbServer.AddInParameter(command1, "PreviousMRP", DbType.Double, item.MRP);
                    dbServer.AddInParameter(command1, "UpdatedMRP", DbType.Double, item.UpdatedMRP);
                    dbServer.AddInParameter(command1, "UpdatedPurchaseRate", DbType.Double, item.UpdatedPurchaseRate);
                    dbServer.AddInParameter(command1, "PreviousPurchaseRate", DbType.Double, item.PurchaseRate);

                    dbServer.AddInParameter(command1, "PreviousBatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command1, "UpdatedBatchCode", DbType.String, item.UpdatedBatchCode);

                    dbServer.AddInParameter(command1, "PreExpiryDate", DbType.DateTime, item.ExpiryDate);
                    dbServer.AddInParameter(command1, "UpdatedExpiryDate", DbType.DateTime, item.UpdatedExpiryDate);

                    dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                    if (BizActionObj.SuccessStatus == -2)
                    {
                        BizActionObj.ItemName = item.ItemName;
                        throw new Exception();
                    }
                }
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                if (BizActionObj.SuccessStatus != -2)
                    BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject ApproveMRPAdjustment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMRPAdjustmentBizActionVO BizActionObj = valueObject as clsAddMRPAdjustmentBizActionVO;
            if (BizActionObj.MRPAdjustmentMainVO == null) BizActionObj.MRPAdjustmentMainVO = new clsMRPAdjustmentMainVO();
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ApproveMRPAdjustment");
                dbServer.AddInParameter(command, "MRPAdjustmentID", DbType.Int64, BizActionObj.MRPAdjustmentMainVO.ID);
                dbServer.AddInParameter(command, "MRPAdjustmentUnitId", DbType.Int64, BizActionObj.MRPAdjustmentMainVO.UnitID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);


                foreach (var item in BizActionObj.MRPAdjustmentItems)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_ApproveMRPAdjustmentDetails");
                    dbServer.AddInParameter(command1, "MRPAdjustmentID", DbType.Int64, BizActionObj.MRPAdjustmentMainVO.ID);
                    dbServer.AddInParameter(command1, "MRPAdjustmentUnitId", DbType.Int64, BizActionObj.MRPAdjustmentMainVO.UnitID);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command1, "BatchId", DbType.Int64, item.BatchId);
                    dbServer.AddInParameter(command1, "StoreID", DbType.Int64, BizActionObj.MRPAdjustmentMainVO.StoreID);

                    dbServer.AddInParameter(command1, "PreExpiryDate", DbType.DateTime, item.ExpiryDate);
                    dbServer.AddInParameter(command1, "PreviousBatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command1, "PreviousPurchaseRate", DbType.Double, item.PurchaseRate);
                    dbServer.AddInParameter(command1, "PreviousMRP", DbType.Double, item.MRP);

                    dbServer.AddInParameter(command1, "UpdatedExpiryDate", DbType.DateTime, item.UpdatedExpiryDate);
                    dbServer.AddInParameter(command1, "UpdatedBatchCode", DbType.String, item.UpdatedBatchCode);
                    dbServer.AddInParameter(command1, "UpdatedPurchaseRate", DbType.Double, item.UpdatedPurchaseRate);
                    dbServer.AddInParameter(command1, "UpdatedMRP", DbType.Double, item.UpdatedMRP);

                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                    //if (BizActionObj.SuccessStatus == -1)
                    //{
                    //    BizActionObj.ItemName = item.ItemName;
                    //    //throw new Exception();
                    //}
                }
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetMRPAdjustmentMainList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;

            clsGetMRPAdustmentListBizActionVO objBizAction = valueObject as clsGetMRPAdustmentListBizActionVO;
            objBizAction.AdjustmentList = new List<clsMRPAdjustmentVO>();
            DbCommand command;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetMRPAdjustmentMain");
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objBizAction.StoreID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objBizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {


                    while (reader.Read())
                    {
                        clsMRPAdjustmentMainVO objMRPAdjustment = new clsMRPAdjustmentMainVO();
                        objMRPAdjustment.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objMRPAdjustment.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objMRPAdjustment.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objMRPAdjustment.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objMRPAdjustment.MRPAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRPAdjustmentNo"]));
                        objMRPAdjustment.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objMRPAdjustment.IsApprove = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApprove"]));
                        objMRPAdjustment.IsReject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReject"]));
                        objMRPAdjustment.stApproveRejectBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveOrRejecteBy"]));
                        objMRPAdjustment.ApproveRejectDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ApproveRejectDate"]));
                        objBizAction.AdjustmentMainList.Add(objMRPAdjustment);
                    }
                    reader.NextResult();
                    objBizAction.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }

        public override IValueObject GetMRPAdjustmentList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;

            clsGetMRPAdustmentListBizActionVO objBizAction = valueObject as clsGetMRPAdustmentListBizActionVO;
            objBizAction.AdjustmentList = new List<clsMRPAdjustmentVO>();
            DbCommand command;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetMRPAdjustmentDetails");
                dbServer.AddInParameter(command, "MRPAdjustmentID", DbType.Int64, objBizAction.MRPAdjustmentMainVO.ID);
                dbServer.AddInParameter(command, "MRPAdjustmentUnitID", DbType.Int64, objBizAction.MRPAdjustmentMainVO.UnitID);

                //dbServer.AddInParameter(command, "StoreId", DbType.Int64, objBizAction.StoreID);
                //dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                //dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objBizAction.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objBizAction.MaximumRows);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {


                    while (reader.Read())
                    {
                        clsMRPAdjustmentVO objMRPAdjustment = new clsMRPAdjustmentVO();
                        objMRPAdjustment.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objMRPAdjustment.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objMRPAdjustment.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objMRPAdjustment.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objMRPAdjustment.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objMRPAdjustment.UpdatedBatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedBatchCode"]));
                        objMRPAdjustment.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objMRPAdjustment.AdjustmentDate = Convert.ToDateTime(DALHelper.HandleDate(reader["AdjustmentDate"]));
                        objMRPAdjustment.MRPAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRPAdjustmentNo"]));
                        objMRPAdjustment.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["PreviousMRP"]));
                        objMRPAdjustment.UpdatedMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["UpdatedMRP"]));
                        objMRPAdjustment.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PreviousPurchaseRate"]));
                        objMRPAdjustment.UpdatedPurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["UpdatedPurchaseRate"]));

                        objMRPAdjustment.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["PreExpiryDate"]);
                        objMRPAdjustment.ExpiryDateString = objMRPAdjustment.ExpiryDate == null ? "" : objMRPAdjustment.ExpiryDate.Value.ToString("MMM-yyyy");
                        objMRPAdjustment.UpdatedExpiryDate = (DateTime?)DALHelper.HandleDate(reader["UpdatedExpiryDate"]);

                        objMRPAdjustment.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objMRPAdjustment.stBatchUpdateStatus = Convert.ToString(DALHelper.HandleDBNull(reader["BatchUpdateStatus"]));
                        objBizAction.AdjustmentList.Add(objMRPAdjustment);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }
        public override IValueObject GetAllItemTaxDetail(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetAllItemTaxDetailBizActionVO objBizAction = valueObject as clsGetAllItemTaxDetailBizActionVO;
            DbCommand command;
            try
            {
                if (objBizAction.ItemTaxList == null)
                    objBizAction.ItemTaxList = new List<clsItemTaxVO>();

                objBizAction.ItemTaxList = GetItemTaxes(0, objBizAction.StoreID, objBizAction.ApplicableFor);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }
        private List<clsItemTaxVO> GetItemTaxes(long ItemID, long StoreID, long ApplicableFor)
        {
            DbDataReader reader = null;
            List<clsItemTaxVO> TaxList = new List<clsItemTaxVO>();
            DbCommand command;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetItemTaxDetail");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, StoreID);
                dbServer.AddInParameter(command, "ApplicableFor", DbType.Int64, ApplicableFor);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, ItemID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsItemTaxVO objItemMaster = new clsItemTaxVO();
                        objItemMaster.status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItemMaster.TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"]));

                        objItemMaster.ApplicableOn.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATApplicableOn"]));
                        if (objItemMaster.ApplicableOnList != null)
                        {
                            objItemMaster.ApplicableOn = objItemMaster.ApplicableOnList.SingleOrDefault(S => S.ID.Equals(objItemMaster.ApplicableOn.ID));
                        }

                        objItemMaster.ApplicableFor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableFor"]));
                        if (objItemMaster.ApplicableForList != null)
                        {
                            objItemMaster.ApplicableFor = objItemMaster.ApplicableForList.SingleOrDefault(S => S.ID.Equals(objItemMaster.ApplicableFor.ID));
                        }

                        // objItemMaster.ItemClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemClinicId"]));
                        objItemMaster.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        TaxList.Add(objItemMaster);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }

            return TaxList;
        }
        public override IValueObject GetAllItemList(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetAllItemListBizActionVO BizActionObj = valueObject as clsGetAllItemListBizActionVO;
            try
            {
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_GetItemsforAutocompleteSearch_New");  // CIMS_GetItemsforAutocompleteSearch on 26July2018
                DbDataReader reader;

                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "IsFromSale", DbType.Boolean, BizActionObj.IsFromSale);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsItemMasterVO>();

                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();

                    while (reader.Read())
                    {
                        MasterListItem objMasterList = new MasterListItem();
                        objMasterList.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objMasterList.Description = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objMasterList.Code = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objMasterList.isChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        objMasterList.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        objMasterList.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objMasterList.VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));
                        objMasterList.NonBatchItemBarcode = Convert.ToString(DALHelper.HandleDBNull(reader["NonBatchItemBarcode"]));
                        objMasterList.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUM"]));
                        objMasterList.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));   // For Item Selection Control on Counter Sale
                        BizActionObj.MasterList.Add(objMasterList);

                        //clsItemMasterVO objVO = new clsItemMasterVO();
                        //objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        //objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        //objVO.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        //objVO.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        //objVO.VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));
                        //objVO.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        //objVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["NonBatchItemBarcode"]));
                        //objVO.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUM"]));
                        //objVO.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));   // For Item Selection Control on Counter Sale
                        //BizActionObj.MasterList.Add(new MasterListItem() { ID = objVO.ID, Description = objVO.ItemName, Code = objVO.ItemCode, isChecked = objVO.BatchesRequired, MRP = objVO.MRP, 
                        //    PurchaseRate = objVO.PurchaseRate, VatPer = objVO.VatPer, NonBatchItemBarcode = objVO.BarCode, BaseUOMID = objVO.BaseUM, IsItemBlock = objVO.IsItemBlock });
                    }

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }
        public override IValueObject GetItemsByItemCategoryStore(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetAllItemListBizActionVO BizActionObj = valueObject as clsGetAllItemListBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemsByItemCategoryStore");
                DbDataReader reader;
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "ItemCategory", DbType.Int64, BizActionObj.ItemCategory);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    //if(BizActionObj.ItemList == null)
                    //    BizActionObj.ItemList = new List<clsItemMasterVO>();
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objVO = new clsItemMasterVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        BizActionObj.MasterList.Add(new MasterListItem() { ID = objVO.ID, Description = objVO.ItemName, Code = objVO.ItemCode, isChecked = objVO.BatchesRequired });
                        // BizActionObj.MasterList.Add(new MasterListItem() { ID = objVO.ID, Description = objVO.ItemName, Code = objVO.ItemCode });
                        // objVO.BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"]));
                        //objVO.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        //objVO.Status = false;
                        //objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        //objVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                        //objVO.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                        //objVO.Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"]));
                        //objVO.PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"]));
                        //objVO.TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"]));
                        //objVO.TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"]));
                        //objVO.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                        //objVO.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        //objVO.VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));
                        //objVO.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        //objVO.ConversionFactor = Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        //objVO.VatApplicableOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatAppOn"]));
                        //objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        //objVO.AssignSupplier = CheckItemSupplier(objVO.ID, BizActionObj.SupplierID);
                        // BizActionObj.ItemList.Add(objVO);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }


        public override IValueObject GetItemClinicDetailList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemClinicDetailBizActionVO objBizAction = valueObject as clsGetItemClinicDetailBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetItemClinicDetail");
                dbServer.AddInParameter(command, "ItemClinicId", DbType.String, objBizAction.ItemClinicId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemTaxList == null)
                        objBizAction.ItemTaxList = new List<clsItemTaxVO>();
                    while (reader.Read())
                    {
                        clsItemTaxVO objItemMaster = new clsItemTaxVO();
                        objItemMaster.ItemClinicDetailId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"]));
                        objItemMaster.ApplicableOn.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATApplicableOn"]));
                        if (objItemMaster.ApplicableOnList != null)
                        {
                            objItemMaster.ApplicableOn = objItemMaster.ApplicableOnList.SingleOrDefault(S => S.ID.Equals(objItemMaster.ApplicableOn.ID));
                        }
                        objItemMaster.ApplicableFor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableFor"]));
                        if (objItemMaster.ApplicableForList != null)
                        {
                            objItemMaster.ApplicableFor = objItemMaster.ApplicableForList.SingleOrDefault(S => S.ID.Equals(objItemMaster.ApplicableFor.ID));
                        }
                        objItemMaster.ItemClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemClinicId"]));
                        objItemMaster.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objBizAction.ItemTaxList.Add(objItemMaster);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }

        public override IValueObject AddUpdateItemClinicDetail(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddUpdateItemClinicDetailBizActionVO BizActionObj = valueObject as clsAddUpdateItemClinicDetailBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemClinicDetail");
                if (BizActionObj.ItemTaxList.Count > 0)
                {
                    for (int i = 0; i <= BizActionObj.ItemTaxList.Count - 1; i++)
                    {
                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.ItemTaxList[i].ItemClinicDetailId);
                        dbServer.AddInParameter(command, "ItemClinicId", DbType.Int64, BizActionObj.StoreClinicID);
                        dbServer.AddInParameter(command, "TaxID", DbType.Int64, BizActionObj.ItemTaxList[i].ID);
                        dbServer.AddInParameter(command, "VATPercentage", DbType.Decimal, BizActionObj.ItemTaxList[i].Percentage);
                        dbServer.AddInParameter(command, "ApplicableFor", DbType.Int32, BizActionObj.ItemTaxList[i].ApplicableForId);
                        dbServer.AddInParameter(command, "VATApplicableOn", DbType.Int32, BizActionObj.ItemTaxList[i].ApplicableOnId);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ItemTaxList[i].status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, BizActionObj.ItemTax.CreatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, BizActionObj.ItemTax.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, BizActionObj.ItemTax.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.ItemTax.AddedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, BizActionObj.ItemTax.AddedWindowsLoginName);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                    }
                    BizActionObj.SuccessStatus = 1;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetStoreItemTaxList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemStoreTaxListBizActionVO BizActionObj = valueObject as clsGetItemStoreTaxListBizActionVO;
            DbCommand command;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetItemStoreTaxList");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreItemTaxDetails.ID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.StoreItemTaxDetails.ItemID);
                dbServer.AddInParameter(command, "IsForAllStore", DbType.Boolean, BizActionObj.IsForAllStore);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.StoreItemTaxList == null)
                        BizActionObj.StoreItemTaxList = new List<clsItemTaxVO>();
                    while (reader.Read())
                    {
                        clsItemTaxVO objItemMaster = new clsItemTaxVO();
                        objItemMaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemMaster.TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"]));
                        objItemMaster.TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"]));
                        objItemMaster.ApplicableOnId = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATApplicableOn"]));
                        objItemMaster.ApplicableForId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableFor"]));
                        objItemMaster.ApplicableOnDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableOnDesc"]));
                        objItemMaster.ApplicableForDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableForDesc"]));
                        objItemMaster.ItemClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemClinicId"]));
                        objItemMaster.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemMaster.status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemMaster.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItemMaster.TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        objItemMaster.ApplicableFrom = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ApplicableFromDate"]));
                        objItemMaster.ApplicableTo = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ApplicableToDate"]));
                        objItemMaster.IsGST = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGST"]));
                        if (objItemMaster.TaxType == 1)
                        {
                            objItemMaster.TaxTypeName = "Inclusive";
                        }
                        else
                        {
                            objItemMaster.TaxTypeName = "Exclusive";
                        }

                        BizActionObj.StoreItemTaxList.Add(objItemMaster);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return BizActionObj;
        }




        public override IValueObject clsAddUpdateItemLocation(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddUpdateItemLocationBizActionVO BizActionObj = valueObject as clsAddUpdateItemLocationBizActionVO;
            try
            {
                DbCommand command1 = null;
                command1 = dbServer.GetStoredProcCommand("CIMS_DeleteItemLocationDetails");
                if (BizActionObj.StoreDetails.Count > 0)
                {
                    foreach (long item in BizActionObj.StoreDetails)
                    {
                        command1.Parameters.Clear();
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.ItemLocationDetails.UnitID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, BizActionObj.ItemLocationDetails.ItemID);
                        //dbServer.AddInParameter(command1, "StoreID", DbType.Int64, item);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    }
                }

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemLocationDetails");
                if (BizActionObj.StoreDetails.Count > 0)
                {
                    foreach (long item in BizActionObj.StoreDetails)
                    {
                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                        //dbServer.AddParameter(command, "StoreId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.StoreId);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ItemLocationDetails.UnitID);
                        dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemLocationDetails.ItemID);
                        dbServer.AddInParameter(command, "StoreID", DbType.Int64, item);
                        dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ItemLocationDetails.RackID);
                        dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ItemLocationDetails.ShelfID);
                        dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ItemLocationDetails.ContainerID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ItemLocationDetails.Status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, BizActionObj.ItemLocationDetails.CreatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, BizActionObj.ItemLocationDetails.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, BizActionObj.ItemLocationDetails.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.ItemLocationDetails.AddedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, BizActionObj.ItemLocationDetails.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizActionObj.ItemLocationDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    }
                }
                BizActionObj.SuccessStatus = 1;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;

            throw new NotImplementedException();
        }

        public override IValueObject clsGetItemLocation(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetItemLocationBizActionVO BizActionObj = valueObject as clsGetItemLocationBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_GetItemLocationDetails");
                dbServer.AddInParameter(command, "itemID", DbType.Int64, BizActionObj.ItemLocationDetails.ItemID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ItemLocationDetails.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    BizActionObj.ItemLocationDetails = new clsItemLocationVO();
                    BizActionObj.StoreDetails = new List<long>();
                    while (reader.Read())
                    {

                        BizActionObj.ItemLocationDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ItemLocationDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ItemLocationDetails.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        BizActionObj.ItemLocationDetails.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));
                        BizActionObj.ItemLocationDetails.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        BizActionObj.ItemLocationDetails.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                        BizActionObj.ItemLocationDetails.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                        BizActionObj.ItemLocationDetails.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));
                        BizActionObj.ItemLocationDetails.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                        BizActionObj.ItemLocationDetails.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                        BizActionObj.ItemLocationDetails.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        BizActionObj.StoreDetails.Add(BizActionObj.ItemLocationDetails.StoreID);
                    }
                }
                reader.NextResult();
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;

            throw new NotImplementedException();
        }

        public override IValueObject clsGetGRNNumberBetweenDateRange(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetAllGRNNumberBetweenDateRangeBizActionVO BizActionObj = valueObject as clsGetAllGRNNumberBetweenDateRangeBizActionVO;
            try
            {
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_GetGRNNumberBetweenDateRange");
                DbDataReader reader;

                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionObj.SupplierID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsItemMasterVO>();

                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();

                    while (reader.Read())
                    {
                        clsGRNVO objVO = new clsGRNVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.GRNNO = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"]));

                        BizActionObj.MasterList.Add(new MasterListItem() { ID = objVO.ID, Description = objVO.GRNNO });


                    }

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

        #region tariffItemLinking

        public override IValueObject GetItemListForTariffItemLink(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListBizActionVO objBizAction = valueObject as clsGetItemListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemListForTariffLinking");
                if (objBizAction.ItemDetails.ItemName != null && objBizAction.ItemDetails.ItemName.Length != 0)
                    dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.ItemDetails.ItemName);
                if (objBizAction.ItemDetails.ItemCode != null && objBizAction.ItemDetails.ItemCode.Length != null)
                    dbServer.AddInParameter(command, "ItemCode", DbType.String, objBizAction.ItemDetails.ItemCode);
                if (objBizAction.ItemDetails.ItemCategory != null)
                    if (objBizAction.ItemDetails.ItemCategory != 0)
                        dbServer.AddInParameter(command, "ItemCategory", DbType.String, objBizAction.ItemDetails.ItemCategory);
                    else
                        dbServer.AddInParameter(command, "ItemCategory", DbType.String, null);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objBizAction.TariffID);
                dbServer.AddInParameter(command, "operationType", DbType.Int64, objBizAction.OperationType);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsItemMasterVO>();
                    while (reader.Read())
                    {
                        clsItemMasterVO objServiceMasterVO = new clsItemMasterVO();
                        objServiceMasterVO.ID = Convert.ToInt64(reader["ID"]);
                        objServiceMasterVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objServiceMasterVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objServiceMasterVO.ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryString"]));
                        objServiceMasterVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objServiceMasterVO.BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["brandname"]));
                        objServiceMasterVO.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objServiceMasterVO.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        objServiceMasterVO.DeductiblePerc = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DeductablePer"]));
                        objServiceMasterVO.DiscountPerc = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DiscountPer"]));
                        objBizAction.ItemList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
                    objBizAction.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
                }
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
            return valueObject;
        }

        public override IValueObject AddTariffItem(IValueObject valueObject, clsUserVO userVO)
        {

            DbTransaction trans = null;
            clsAddTariffItemBizActionVO objItem = valueObject as clsAddTariffItemBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_AddTariffLinkingWithItems");
                command.Parameters.Clear();
                dbServer.AddInParameter(command, "ApplyForAll", DbType.Boolean, objItem.IsAllItemsSelected);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objItem.TariffID);
                dbServer.AddInParameter(command, "ItemIDList", DbType.String, objItem.ItemIDList);
                dbServer.AddInParameter(command, "ItemDisList", DbType.String, objItem.ItemDisList);
                dbServer.AddInParameter(command, "ItemDedList", DbType.String, objItem.ItemDedList);
                dbServer.AddInParameter(command, "DiscountPer", DbType.Decimal, objItem.DiscountPer);
                dbServer.AddInParameter(command, "DeductablePer", DbType.Decimal, objItem.DeductablePer);

                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItem.ItemMasterDetails.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objItem.ItemMasterDetails.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItem.ItemMasterDetails.UpdateddOn);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItem.ItemMasterDetails.AddedBy);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItem.ItemMasterDetails.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItem.ItemMasterDetails.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "SucessStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "SucessStatus");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans = null;
            }
            return objItem;
        }

        public override IValueObject UpdateTariffItem(IValueObject valueObject, clsUserVO userVO)
        {

            DbTransaction trans = null;
            clsAddTariffItemBizActionVO objItemForUpdate = valueObject as clsAddTariffItemBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateTariffLinkingWithItems");
                command.Parameters.Clear();
                dbServer.AddInParameter(command, "ApplyForAll", DbType.Boolean, objItemForUpdate.IsAllItemsSelected);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemForUpdate.UnitID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objItemForUpdate.TariffID);
                dbServer.AddInParameter(command, "ItemIDList", DbType.String, objItemForUpdate.ItemIDList);
                dbServer.AddInParameter(command, "ItemDisList", DbType.String, objItemForUpdate.ItemDisList);
                dbServer.AddInParameter(command, "ItemDedList", DbType.String, objItemForUpdate.ItemDedList);
                //dbServer.AddInParameter(command, "DiscountPer", DbType.Decimal, objItemForUpdate.DiscountPer);
                //dbServer.AddInParameter(command, "DeductablePer", DbType.Decimal, objItemForUpdate.DeductablePer);

                //dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemForUpdate.ItemMasterDetails.AddedOn);
                // dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objItemForUpdate.ItemMasterDetails.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemForUpdate.ItemMasterDetails.UpdateddOn);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemForUpdate.ItemMasterDetails.AddedBy);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                //  dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemForUpdate.ItemMasterDetails.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemForUpdate.ItemMasterDetails.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "SucessStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItemForUpdate.SuccessStatus = (int)dbServer.GetParameterValue(command, "SucessStatus");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans = null;
            }
            return objItemForUpdate;

        }

        #endregion



        public override IValueObject GetItemBatchList(IValueObject valueObject, clsUserVO uservo)
        {
            DbDataReader reader = null;
            clsGetItemListForNewItemBatchMasterBizActionVO BizActionObj = valueObject as clsGetItemListForNewItemBatchMasterBizActionVO;
            BizActionObj.ItemList = new List<clsItemMasterVO>();
            DbCommand command;
            try
            {
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemListForSearchForNewform");
                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    if (BizActionObj.ItemName != null && BizActionObj.ItemName.Length != 0)
                        dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemName);
                    if (BizActionObj.BatchCode != null && BizActionObj.BatchCode.Length != 0)
                        dbServer.AddInParameter(command, "BatchCode", DbType.String, BizActionObj.BatchCode);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {


                        while (reader.Read())
                        {
                            clsItemMasterVO ItemListnew = new clsItemMasterVO();
                            ItemListnew.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                            ItemListnew.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                            ItemListnew.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                            ItemListnew.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                            //By Anjali............
                            ItemListnew.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                            //...........................
                            BizActionObj.ItemList.Add(ItemListnew);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject AddItemBarCodefromAssigned(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddAssignedItemBarcodeBizActionVO objItem = valueObject as clsAddAssignedItemBarcodeBizActionVO;
            objItem.ItemList = new List<clsItemMasterVO>();
            try
            {
                DbCommand command;
                clsItemMasterVO ItemListnew = new clsItemMasterVO();
                command = dbServer.GetStoredProcCommand("CIMS_AddItemBarCodefromAssignedFrm");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
                dbServer.AddInParameter(command, "ItemName", DbType.String, objItem.ItemName);
                dbServer.AddInParameter(command, "BatchCode", DbType.String, objItem.BatchCode);
                dbServer.AddInParameter(command, "BarCode", DbType.String, objItem.BarCode);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


            }
            catch (Exception ex)
            {

            }
            return objItem;
        }

        //added by akshays
        public override IValueObject clsAddUpdateItemStoreLocationDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            ItemStoreLocationDetailsBizActionVo BizActionObj = valueObject as ItemStoreLocationDetailsBizActionVo;
            try
            {
                //DbCommand command1 = null;
                //command1 = dbServer.GetStoredProcCommand("CIMS_DeleteItemLocationDetails");
                //if (BizActionObj.StoreDetails.Count > 0)
                //{
                //    foreach (long item in BizActionObj.StoreDetails)
                //    {
                //        command1.Parameters.Clear();
                //        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.UnitID);
                //        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ItemID);
                //        //dbServer.AddInParameter(command1, "StoreID", DbType.Int64, item);
                //        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                //        int intStatus = dbServer.ExecuteNonQuery(command1);
                //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                //    }
                //}

                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemStoreLocationDetails");
                if (BizActionObj.StoreDetails.Count > 0)
                {
                    foreach (long item in BizActionObj.StoreDetails)
                    {
                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ID);
                        //dbServer.AddParameter(command, "StoreId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.StoreId);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.UnitID);
                        dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ItemID);
                        dbServer.AddInParameter(command, "StoreID", DbType.Int64, item);
                        dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.RackID);
                        dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ShelfID);
                        dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ContainerID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ItemStoreLocationDetail.Status);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.CreatedUnitID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, BizActionObj.ItemStoreLocationDetail.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, BizActionObj.ItemStoreLocationDetail.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.ItemStoreLocationDetail.AddedDateTime);
                        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.UpdatedUnitID);
                        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, BizActionObj.ItemStoreLocationDetail.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizActionObj.ItemStoreLocationDetail.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    }

                    BizActionObj.SuccessStatus = 1;
                }

                else
                {


                    //if (BizActionObj.StoreDetails.Count > 0)
                    //{
                    //    foreach (long item in BizActionObj.StoreDetails)
                    //    {
                    command.Parameters.Clear();
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ID);
                    //dbServer.AddParameter(command, "StoreId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.StoreId);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.UnitID);
                    dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ItemID);
                    dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.StoreID);
                    dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.RackID);
                    dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ShelfID);
                    dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.ContainerID);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ItemStoreLocationDetail.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.CreatedUnitID);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, BizActionObj.ItemStoreLocationDetail.AddedBy);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, BizActionObj.ItemStoreLocationDetail.AddedOn);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.ItemStoreLocationDetail.AddedDateTime);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, BizActionObj.ItemStoreLocationDetail.UpdatedUnitID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, BizActionObj.ItemStoreLocationDetail.AddedWindowsLoginName);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                    BizActionObj.ItemStoreLocationDetail.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    //    }
                    //}
                    BizActionObj.SuccessStatus = 1;
                }
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizActionObj;

            throw new NotImplementedException();
        }

        public override IValueObject clsGetItemStoreLocationDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            GetItemStoreLocationDetailsBizActionVO BizActionObj = valueObject as GetItemStoreLocationDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemStoreLocationDetails");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ObjItemStoreLocationDetails.UnitID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ObjItemStoreLocationDetails.ItemID);
                //dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                //dbServer.AddInParameter(command, "ApplicableGender", DbType.Int64, BizActionObj.ApplicableGender);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ItemStoreLocationDetailslist == null)
                        BizActionObj.ItemStoreLocationDetailslist = new List<ItemStoreLocationDetailsVO>();
                    while (reader.Read())
                    {
                        ItemStoreLocationDetailsVO objStateVO = new ItemStoreLocationDetailsVO();
                        objStateVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objStateVO.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));
                        objStateVO.ItemUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemUnitID"]));
                        objStateVO.Barcode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        objStateVO.StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"]));
                        objStateVO.RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RackID"]));
                        objStateVO.ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ShelfID"]));
                        objStateVO.ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerID"]));
                        objStateVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                        objStateVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                        objStateVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        objStateVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objStateVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));

                        //objStateVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        //objStateVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        //objStateVO.GetApplicableGender = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableGender"]));
                        //objStateVO.ApplicableGender = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableGenderID"]));
                        objStateVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.ItemStoreLocationDetailslist.Add(objStateVO);
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

            throw new NotImplementedException();
        }
        //closed by akshays

        //added by akshays on 5-11-2015
        public override IValueObject clsAddUpdateStoreLocationLock(IValueObject valueObject, clsUserVO UserVO)
        {
            AddUpdateStoreLocationLockBizActionVO BizActionObj = valueObject as AddUpdateStoreLocationLockBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command = null;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreLocationLock");
                command.Connection = con;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ObjStoreLocationLock.StoreID);
                dbServer.AddInParameter(command, "date", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.ObjStoreLocationLock.Remark);
                dbServer.AddInParameter(command, "IsFreeze", DbType.Boolean, BizActionObj.ObjStoreLocationLock.IsFreeze);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ObjStoreLocationLock.ID);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ObjStoreLocationLock.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                BizActionObj.ObjStoreLocationLock.UnitID = Convert.ToInt64(dbServer.GetParameterValue(command, "UnitID"));
                BizActionObj.SuccessStatus = 1;

                if (BizActionObj.IsModify)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteStoreLocationLock");
                    command3.Connection = con;
                    dbServer.AddInParameter(command3, "StoreLocationID", DbType.Int64, BizActionObj.ObjStoreLocationLock.ID);
                    dbServer.AddInParameter(command3, "StoreLocationUnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                    int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                }

                if (BizActionObj.StoreLocationLockDetails.Count > 0)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreLocationLockDetails");
                    command1.Connection = con;
                    foreach (StoreLocationLockVO item in BizActionObj.StoreLocationLockDetails)
                    {
                        command1.Parameters.Clear();
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, item.UnitID);
                        dbServer.AddInParameter(command1, "StoreID", DbType.Int64, item.StoreID);
                        dbServer.AddInParameter(command1, "RackID", DbType.Int64, item.RackID);
                        dbServer.AddInParameter(command1, "ShelfID", DbType.Int64, item.ShelfID);
                        dbServer.AddInParameter(command1, "ContainerID", DbType.Int64, item.ContainerID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                        dbServer.AddInParameter(command1, "BlockType", DbType.String, item.BlockType);
                        dbServer.AddInParameter(command1, "BlockTypeID", DbType.Int64, item.BlockTypeID);
                        if (BizActionObj.ObjStoreLocationLock.IsFreeze)
                            dbServer.AddInParameter(command1, "IsBlock", DbType.Boolean, BizActionObj.ObjStoreLocationLock.IsFreeze);
                        dbServer.AddInParameter(command1, "LockDate", DbType.DateTime, item.LockDate);
                        //dbServer.AddInParameter(command1, "ReleaseDate", DbType.DateTime, item.LockDate);
                        dbServer.AddInParameter(command1, "status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, item.CreatedUnitID);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, item.AddedBy);
                        dbServer.AddInParameter(command1, "addedOn", DbType.String, item.AddOn);
                        dbServer.AddInParameter(command1, "addedDateTime", DbType.DateTime, item.AddedDateTime);
                        dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVO.ID);
                        dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, item.UpdatedUnitID);
                        dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command1, "StoreLocationID", DbType.Int64, BizActionObj.ObjStoreLocationLock.ID);
                        dbServer.AddInParameter(command1, "StoreLocationUnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    }
                    BizActionObj.SuccessStatus = 1;
                }
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                con.Close();
                trans = null;
                throw;
            }
            finally
            {
                con.Close();
                //trans.Commit(); 
                trans = null;
            }
            return valueObject;
            throw new NotImplementedException();
        }

        public override IValueObject clsGetStoreLocationLock(IValueObject valueObject, clsUserVO UserVO)
        {

            GetStoreLocationLockBizActionVO BizActionObj = valueObject as GetStoreLocationLockBizActionVO;

            try
            {
                if (BizActionObj.Flag == 1) // For Main List
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStoreLocaionLock");
                    if (BizActionObj.ObjStoreLocationLock.StoreID > 0)
                    {
                        dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ObjStoreLocationLock.StoreID);
                    }
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                    if (BizActionObj.ObjStoreLocationLock.Remark != null && BizActionObj.ObjStoreLocationLock.Remark != "")
                    {
                        dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.ObjStoreLocationLock.Remark);
                    }
                    dbServer.AddInParameter(command, "UserID", DbType.Int64, UserVO.ID);
                    dbServer.AddInParameter(command, "FromDate", DbType.Date, BizActionObj.ObjStoreLocationLock.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.Date, BizActionObj.ObjStoreLocationLock.ToDate);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.StoreLocationLock == null)
                            BizActionObj.StoreLocationLock = new List<StoreLocationLockVO>();
                        while (reader.Read())
                        {
                            StoreLocationLockVO objStateVO = new StoreLocationLockVO();
                            objStateVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                            objStateVO.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                            objStateVO.RequestNo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RequestNo"]));
                            objStateVO.AddedDateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                            objStateVO.StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"]));
                            objStateVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                            objStateVO.AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddedBy"]));
                            objStateVO.IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"]));
                            objStateVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                            objStateVO.AddedByname = Convert.ToString(DALHelper.HandleDBNull(reader["AddedbyName"]));
                            objStateVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            objStateVO.LockDate = Convert.ToDateTime(DALHelper.HandleDate(reader["LockDate"]));
                            objStateVO.IsBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBlock"]));
                            //objStateVO.ReleaseDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReleaseDate"]));
                            objStateVO.ReleaseDate = (DateTime?)DALHelper.HandleDate(reader["ReleaseDate"]);

                            BizActionObj.StoreLocationLock.Add(objStateVO);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                        // reader.Close();
                    }
                }
                if (BizActionObj.Flag == 2)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStoreLocaionLockDetails");
                    dbServer.AddInParameter(command, "StoreLocationID", DbType.Int64, BizActionObj.ObjStoreLocationLock.ID);
                    dbServer.AddInParameter(command, "StoreLocationUnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.StoreLocationLock == null)
                            BizActionObj.StoreLocationLock = new List<StoreLocationLockVO>();
                        while (reader.Read())
                        {
                            StoreLocationLockVO objStateVO = new StoreLocationLockVO();
                            objStateVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                            objStateVO.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                            objStateVO.StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"]));
                            objStateVO.ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ShelfID"]));
                            objStateVO.ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerID"]));
                            objStateVO.RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RackID"]));
                            objStateVO.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));
                            objStateVO.LockDate = Convert.ToDateTime(DALHelper.HandleDate(reader["LockDate"]));
                            objStateVO.ReleaseDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReleaseDate"]));
                            //DateTime RevokeDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReleaseDate"]));
                            //if (RevokeDate == DateTime.MinValue)
                            //{
                            //    objStateVO.strReleaseDate = "N/A";//objStateVO.ReleaseDate = DateTime.Parse("N/A".ToString());
                            //}
                            //else
                            //    objStateVO.strReleaseDate = Convert.ToString(DALHelper.HandleDate(reader["ReleaseDate"]));  
                            objStateVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                            objStateVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                            objStateVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                            objStateVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                            objStateVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                            objStateVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                            objStateVO.IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"]));
                            objStateVO.IsBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                            objStateVO.BlockType = Convert.ToString(DALHelper.HandleDBNull(reader["BlockType"]));
                            objStateVO.BlockTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BlockTypeID"]));
                            BizActionObj.StoreLocationLock.Add(objStateVO);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

            throw new NotImplementedException();
        }
        //closed by akshays on 5-11-2015

        //***Added by Ashish Z. on 18-Dec-2015 for Getting the Items.
        public override IValueObject clsGetStoreLocationLockForView(IValueObject valueObject, clsUserVO UserVO)
        {
            GetStoreLocationLockBizActionVO BizActionObj = valueObject as GetStoreLocationLockBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStoreLocationLockForView");
                dbServer.AddInParameter(command, "StoreLocationID", DbType.Int64, BizActionObj.ObjStoreLocationLock.ID);
                dbServer.AddInParameter(command, "StoreLocationUnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.ObjStoreLocationLock.StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"]));
                        BizActionObj.ObjStoreLocationLock.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        BizActionObj.ObjStoreLocationLock.IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"]));

                    }
                    reader.NextResult();

                    if (BizActionObj.StoreLocationLock == null)
                        BizActionObj.StoreLocationLockDetailsList = new List<ItemStoreLocationDetailsVO>();
                    while (reader.Read())
                    {
                        ItemStoreLocationDetailsVO objStateVO = new ItemStoreLocationDetailsVO();
                        objStateVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        objStateVO.StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"]));
                        objStateVO.ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ShelfID"]));
                        objStateVO.ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerID"]));
                        objStateVO.RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RackID"]));
                        objStateVO.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemId"]));
                        objStateVO.LockDate = Convert.ToDateTime(DALHelper.HandleDate(reader["LockDate"]));
                        objStateVO.ReleaseDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReleaseDate"]));
                        objStateVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objStateVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                        objStateVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                        objStateVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        objStateVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objStateVO.BlockType = Convert.ToString(DALHelper.HandleDBNull(reader["BlockType"]));
                        objStateVO.BlockTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BlockTypeID"]));
                        BizActionObj.StoreLocationLockDetailsList.Add(objStateVO);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

            throw new NotImplementedException();
        }

        public override IValueObject GetItemsStoreLocationLock(IValueObject valueObject, clsUserVO UserVO)
        {
            GetItemStoreLocationLockBizActionVO BizActionObj = valueObject as GetItemStoreLocationLockBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemsStoreLocaionLockDetails");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ObjStoreLocationLockVO.StoreID);
                dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ObjStoreLocationLockVO.RackID);
                dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ObjStoreLocationLockVO.ShelfID);
                dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ObjStoreLocationLockVO.ContainerID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.StoreLocationLockDetailsList == null)
                        BizActionObj.StoreLocationLockDetailsList = new List<ItemStoreLocationDetailsVO>();
                    while (reader.Read())
                    {
                        ItemStoreLocationDetailsVO objStateVO = new ItemStoreLocationDetailsVO();
                        objStateVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objStateVO.StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"]));
                        objStateVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objStateVO.RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RackID"]));
                        objStateVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                        objStateVO.ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ShelfID"]));
                        objStateVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                        objStateVO.ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerID"]));
                        objStateVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        objStateVO.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));
                        objStateVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));

                        BizActionObj.StoreLocationLockDetailsList.Add(objStateVO);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

            throw new NotImplementedException();
        }


        public override IValueObject SetUnBlockRecords(IValueObject valueObject, clsUserVO UserVO)
        {
            GetItemStoreLocationLockBizActionVO BizActionObj = valueObject as GetItemStoreLocationLockBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {

                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_SetUnBlockRecords");

                dbServer.AddInParameter(command, "IsForMainRecord", DbType.Boolean, BizActionObj.IsForMainRecord);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.UnitID);
                dbServer.AddInParameter(command, "BlockTypeID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.BlockTypeID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.StoreID);
                dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.RackID);
                dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.ShelfID);
                dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.ContainerID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ObjStoreLocationLockDetailsVO.ItemID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                if (BizActionObj.SuccessStatus == -1)  // Added by Ashish Z. on dated 07102016 for Unrevoke the request while UnApprove Stock Adjustment..
                {
                    //throw new Exception();
                    trans.Rollback();
                }
                else if (BizActionObj.SuccessStatus == 1)
                {
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            return BizActionObj;

            throw new NotImplementedException();
        }

        public override IValueObject GetRackMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            GetRackMasterBizActionVO BizActionObj = valueObject as GetRackMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRackMaster");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objRackMasterVO = new MasterListItem();
                        objRackMasterVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objRackMasterVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.MasterList.Add(objRackMasterVO);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
            throw new NotImplementedException();
        }

        public override IValueObject GetShelfMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            GetShelfMasterBizActionVO BizActionObj = valueObject as GetShelfMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetShelfMaster");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.RackID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objRackMasterVO = new MasterListItem();
                        objRackMasterVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objRackMasterVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.MasterList.Add(objRackMasterVO);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
            throw new NotImplementedException();
        }

        public override IValueObject GetBinMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            GetBinMasterBizActionVO BizActionObj = valueObject as GetBinMasterBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBinMaster");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.RackID);
                dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ShelfID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objRackMasterVO = new MasterListItem();
                        objRackMasterVO.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objRackMasterVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.MasterList.Add(objRackMasterVO);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
            throw new NotImplementedException();
        }


        //***

        //By Anjali...............................................
        public override IValueObject clsGetBlockItemList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetBlockItemsListBizActionVO BizActionObj = valueObject as clsGetBlockItemsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBlockItemList");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ItemDetails.StoreID);
                dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ItemDetails.RackID);
                dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ItemDetails.ShelfID);
                dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ItemDetails.ContainerID);
                dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemDetails.ItemName);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ItemDetailsList == null)
                        BizActionObj.ItemDetailsList = new List<clsBlockItemsVO>();
                    while (reader.Read())
                    {


                        clsBlockItemsVO objVO = new clsBlockItemsVO();
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objVO.Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objVO.Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objVO.Container = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        objVO.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));

                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDBNull(reader["ExpiryDate"]);
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                        objVO.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                        objVO.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));

                        objVO.SellingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        objVO.SellingUM = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));
                        objVO.BaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objVO.BaseUM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                        objVO.PurchaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PurchaseUMID"]));
                        objVO.PurchaseUM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUM"]));
                        objVO.StockingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockingUMID"]));
                        objVO.StockingUM = Convert.ToString(DALHelper.HandleDBNull(reader["StockingUM"]));


                        objVO.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                        objVO.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                        objVO.AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));

                        BizActionObj.ItemDetailsList.Add(objVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
            //throw new NotImplementedException();
        }


        public override IValueObject AddUpdatePhysicalItemStock(IValueObject valueObject, clsUserVO UserVO)
        {
            // throw new NotImplementedException();
            clsAddUpdatePhysicalItemStockBizActionVO BizActionObj = valueObject as clsAddUpdatePhysicalItemStockBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPhysicalStockItem");
                command1.Connection = con;
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ItemDetails.ID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "StoreID", DbType.Int64, BizActionObj.ItemDetails.StoreID);
                dbServer.AddInParameter(command1, "RequestDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "RequestedBy", DbType.Int64, BizActionObj.ItemDetails.RequestedBy);
                dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                BizActionObj.ItemDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");
                BizActionObj.ItemDetails.UnitID = UserVO.UserLoginInfo.UnitId;

                foreach (var item in BizActionObj.ItemDetailsList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPhysicalStockItemDetails");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PhysicalItemID", DbType.Int64, BizActionObj.ItemDetails.ID);
                    dbServer.AddInParameter(command, "PhysicalItemUnitID", DbType.Int64, BizActionObj.ItemDetails.UnitID);
                    dbServer.AddInParameter(command, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command, "BatchID", DbType.Int64, item.BatchID);
                    dbServer.AddInParameter(command, "AvailableStock", DbType.Single, item.AvailableStock);
                    dbServer.AddInParameter(command, "OperationTypeID", DbType.Int32, item.intOperationType);
                    dbServer.AddInParameter(command, "AdjustmentQunatity", DbType.Single, item.AdjustmentQunatity);
                    dbServer.AddInParameter(command, "PhysicalQuantity", DbType.Single, item.PhysicalQuantity);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "Remark", DbType.String, item.Remark);
                    dbServer.AddInParameter(command, "ShelfID", DbType.Int64, item.ShelfID);
                    dbServer.AddInParameter(command, "ContainerID", DbType.Int64, item.ContainerID);
                    dbServer.AddInParameter(command, "RackID", DbType.Int64, item.RackID);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    //For Conversion factor.......................
                    dbServer.AddInParameter(command, "UOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command, "BaseUMID", DbType.Int64, item.BaseUMID);
                    dbServer.AddInParameter(command, "StockingUMID", DbType.Int64, item.StockingUMID);
                    dbServer.AddInParameter(command, "StockingCF", DbType.Single, item.ConversionFactor);
                    dbServer.AddInParameter(command, "BaseCF", DbType.Single, item.BaseConversionFactor);
                    dbServer.AddInParameter(command, "BaseQuantity", DbType.Single, item.BaseQuantity);
                    //............................................ 

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));


                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
            }
            finally
            {

                con.Close();
                con = null;
                trans = null;

            }

            return BizActionObj;


        }
        public override IValueObject GetPhysicalItemStock(IValueObject valueObject, clsUserVO UserVO)
        {
            //throw new NotImplementedException();


            clsGetPhysicalItemStockBizActionVO BizActionObj = valueObject as clsGetPhysicalItemStockBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPhysicalStockItem");
                DbDataReader reader;


                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, UserVO.ID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemDetailsList == null)
                        BizActionObj.ItemDetailsList = new List<clsPhysicalItemsMainVO>();
                    while (reader.Read())
                    {
                        clsPhysicalItemsMainVO objVO = new clsPhysicalItemsMainVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objVO.RequestDateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["RequestDateTime"]));
                        objVO.RequestedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestedBy"]));
                        objVO.RequestedByName = Convert.ToString(DALHelper.HandleDBNull(reader["RequestedByName"]));
                        objVO.IsConvertedToSA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConvertedToSA"]));
                        objVO.PhysicalItemStockNo = Convert.ToString(DALHelper.HandleDBNull(reader["PhysicalItemStockNo"]));

                        BizActionObj.ItemDetailsList.Add(objVO);
                    }
                }

                reader.NextResult();
                BizActionObj.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject GetPhysicalItemStockDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            //throw new NotImplementedException();
            clsGetPhysicalItemStockDetailsBizActionVO BizActionObj = valueObject as clsGetPhysicalItemStockDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPhysicalStockItemDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PhysicalItemID", DbType.Int64, BizActionObj.PhysicalItemID);
                dbServer.AddInParameter(command, "PhysicalItemUnitID", DbType.Int64, BizActionObj.PhysicalItemUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemDetailsList == null)
                        BizActionObj.ItemDetailsList = new List<clsPhysicalItemsVO>();
                    while (reader.Read())
                    {
                        clsPhysicalItemsVO objVO = new clsPhysicalItemsVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objVO.AdjustmentQunatity = Convert.ToSingle(DALHelper.HandleDBNull(reader["AdjustmentQunatity"]));
                        objVO.AvailableStock = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.Container = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        objVO.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));
                        objVO.intOperationType = Convert.ToInt32(DALHelper.HandleDBNull(reader["OperationType"]));
                        objVO.PhysicalItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicalItemID"]));
                        objVO.PhysicalItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicalItemUnitID"]));
                        objVO.PhysicalQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["PhysicalQuantity"]));
                        objVO.Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objVO.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                        objVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objVO.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                        objVO.Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        if (objVO.intOperationType == 1)
                        {
                            objVO.intOperationTypeName = InventoryStockOperationType.Addition.ToString();
                        }
                        else if (objVO.intOperationType == 2)
                        {
                            objVO.intOperationTypeName = InventoryStockOperationType.Subtraction.ToString();
                        }
                        else
                        {
                            objVO.intOperationTypeName = InventoryStockOperationType.None.ToString();
                        }
                        //For Conversion factor.......................
                        objVO.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOMID"]));
                        objVO.BaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objVO.StockingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockingUMID"]));
                        objVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                        objVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objVO.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        objVO.BaseUM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                        objVO.StockingUM = Convert.ToString(DALHelper.HandleDBNull(reader["StockingUM"]));
                        objVO.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        objVO.AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));

                        objVO.SelectedUOM.Description = objVO.UOM;
                        objVO.SelectedUOM.ID = objVO.UOMID;
                        //..................................................
                        BizActionObj.ItemDetailsList.Add(objVO);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;

        }
        public override IValueObject GetStockAdjustmentListMain(IValueObject valueObject, clsUserVO UserVO)
        {
            //throw new NotImplementedException();

            DbDataReader reader = null;

            clsGetStockAdustmentListMainBizActionVO objBizAction = valueObject as clsGetStockAdustmentListMainBizActionVO;
            objBizAction.AdjustStock = new List<clsAdjustmentStockMainVO>();
            DbCommand command;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetStockAdjustmentDetailsMain");
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, objBizAction.StoreID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objBizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {


                    while (reader.Read())
                    {


                        clsAdjustmentStockMainVO stockItem = new clsAdjustmentStockMainVO();
                        stockItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        stockItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        stockItem.RequestDateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["RequestDateTime"]));
                        stockItem.RequestedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestedBy"]));
                        stockItem.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                        stockItem.ApprovedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovedBy"]));
                        stockItem.ApprovedDateTime = (DateTime?)(DALHelper.HandleDate(reader["ApprovedDateTime"]));
                        stockItem.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        stockItem.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        stockItem.StockAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["StockAdjustmentNo"]));
                        stockItem.IsFromPST = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromPST"]));
                        stockItem.PhysicalItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicalItemID"]));
                        stockItem.PhysicalItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull((reader["PhysicalItemUnitID"])));
                        stockItem.RequestedByName = Convert.ToString(DALHelper.HandleDBNull((reader["RequestedByName"])));
                        stockItem.ApprovedByName = Convert.ToString(DALHelper.HandleDBNull((reader["ApprovedByName"])));
                        stockItem.IsRejected = Convert.ToBoolean(DALHelper.HandleDBNull((reader["IsRejected"])));
                        stockItem.Remark = Convert.ToString(DALHelper.HandleDBNull((reader["Remark"])));




                        objBizAction.AdjustStock.Add(stockItem);
                    }
                    reader.NextResult();
                    objBizAction.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;
        }
        public override IValueObject UpdateStockAdjustment(IValueObject valueObject, clsUserVO UserVO)
        {

            clsUpdateStockAdjustmentBizActionVO BizActionObj = valueObject as clsUpdateStockAdjustmentBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStockAdjustment");
                command.Connection = con;


                dbServer.AddInParameter(command, "StockAdjustmentID", DbType.Int64, BizActionObj.objMainStock.ID);
                dbServer.AddInParameter(command, "StockAdjustmentUnitID", DbType.Int64, BizActionObj.objMainStock.UnitID);
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, BizActionObj.objMainStock.IsApproved);
                dbServer.AddInParameter(command, "ApprovedBy", DbType.Int64, BizActionObj.objMainStock.ApprovedBy);
                dbServer.AddInParameter(command, "ApprovedDateTime", DbType.DateTime, BizActionObj.objMainStock.ApprovedDateTime);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                //if (BizActionObj.objMainStock.IsFromPST == true)
                //{
                //    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdatePhysicalStockItem");
                //    dbServer.AddInParameter(command2, "PhysicalItemID", DbType.Int64, BizActionObj.objMainStock.PhysicalItemID);
                //    dbServer.AddInParameter(command2, "PhysicalItemUnitID", DbType.Int64, BizActionObj.objMainStock.PhysicalItemUnitID);
                //    dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVO.ID);
                //    dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //    dbServer.AddInParameter(command2, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                //    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                //    int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                //}

                foreach (var item in BizActionObj.StockAdustmentItems)
                {
                    item.StockAdjustmentNo = BizActionObj.objMainStock.StockAdjustmentNo;

                    BizActionObj.StockAdustmentItem.StockDetails.ItemID = item.ItemId;
                    BizActionObj.StockAdustmentItem.StockDetails.BatchID = Convert.ToInt64(item.BatchId);
                    BizActionObj.StockAdustmentItem.StockDetails.TransactionTypeID = InventoryTransactionType.StockAdujustment;
                    BizActionObj.StockAdustmentItem.StockDetails.TransactionQuantity = item.BaseQuantity; //item.AdjustmentQunatitiy;
                    BizActionObj.StockAdustmentItem.StockDetails.TransactionID = BizActionObj.objMainStock.ID; //BizActionObj.StockAdjustmentId;
                    BizActionObj.StockAdustmentItem.StockDetails.Date = Convert.ToDateTime(BizActionObj.objMainStock.ApprovedDateTime);
                    BizActionObj.StockAdustmentItem.StockDetails.Time = System.DateTime.Now;
                    BizActionObj.StockAdustmentItem.StockDetails.OperationType = (InventoryStockOperationType)item.intOperationType;//item.OperationType;
                    BizActionObj.StockAdustmentItem.StockDetails.StoreID = BizActionObj.objMainStock.StoreID;
                    BizActionObj.StockAdustmentItem.StockDetails.UnitId = BizActionObj.objMainStock.UnitID;
                    //By Umesh
                    BizActionObj.StockAdustmentItem.StockDetails.BaseUOMID = item.BaseUMID;
                    BizActionObj.StockAdustmentItem.StockDetails.SUOMID = item.StockingUMID;
                    BizActionObj.StockAdustmentItem.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                    BizActionObj.StockAdustmentItem.StockDetails.ConversionFactor = item.ConversionFactor;
                    BizActionObj.StockAdustmentItem.StockDetails.StockingQuantity = item.AdjustmentQunatitiy * item.ConversionFactor; //item.StockingQuantity;
                    BizActionObj.StockAdustmentItem.StockDetails.SelectedUOM.ID = item.TransactionUOMID;
                    BizActionObj.StockAdustmentItem.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.AdjustmentQunatitiy);

                    //END
                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();


                    obj.Details = BizActionObj.StockAdustmentItem.StockDetails;
                    obj.Details.ID = 0;

                    //obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVO);                     // transaction was not maintained previously
                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVO, con, trans);           // transaction maintained on 27022017   

                    if (obj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }

                    BizActionObj.StockAdustmentItem.StockDetails.ID = obj.Details.ID;
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.objMainStock = null;
                BizActionObj.StockAdustmentItems = null;
                BizActionObj.StockAdustmentItem = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;

        }

        public override IValueObject RejectStockAdjustment(IValueObject valueObject, clsUserVO UserVO)
        {

            clsUpdateStockAdjustmentBizActionVO BizActionObj = valueObject as clsUpdateStockAdjustmentBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_RejectStockAdjustment");
                command.Connection = con;


                dbServer.AddInParameter(command, "StockAdjustmentID", DbType.Int64, BizActionObj.objMainStock.ID);
                dbServer.AddInParameter(command, "StockAdjustmentUnitID", DbType.Int64, BizActionObj.objMainStock.UnitID);
                dbServer.AddInParameter(command, "IsRejected", DbType.Boolean, BizActionObj.objMainStock.IsRejected);
                dbServer.AddInParameter(command, "RejectedBy", DbType.Int64, BizActionObj.objMainStock.RejectedBy);
                dbServer.AddInParameter(command, "RejectedDateTime", DbType.DateTime, BizActionObj.objMainStock.RejectedDateTime);
                dbServer.AddInParameter(command, "ResonForRejection", DbType.String, BizActionObj.objMainStock.ResonForRejection);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.objMainStock = null;

            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;

        }


        public override IValueObject clsItemListForSuspendStockSearch(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetBlockItemsListBizActionVO BizActionObj = valueObject as clsGetBlockItemsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemListForSuspendStockSearch");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ItemDetails.UnitID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ItemDetails.StoreID);
                dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ItemDetails.RackID);
                dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ItemDetails.ShelfID);
                dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ItemDetails.ContainerID);
                dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemDetails.ItemName);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ItemDetailsList == null)
                        BizActionObj.ItemDetailsList = new List<clsBlockItemsVO>();
                    while (reader.Read())
                    {
                        clsBlockItemsVO objVO = new clsBlockItemsVO();
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                        objVO.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                        objVO.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));
                        objVO.Store = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objVO.Rack = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                        objVO.Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                        objVO.Container = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));


                        BizActionObj.ItemDetailsList.Add(objVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
            //throw new NotImplementedException();
        }

        public override IValueObject IsForCheckInTransitItems(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetBlockItemsListBizActionVO BizActionObj = valueObject as clsGetBlockItemsListBizActionVO;
            DbCommand objCommand;
            DbDataReader objReader;
            try
            {
                objCommand = dbServer.GetStoredProcCommand("CIMS_IsForCheckInTransitItemsForSuspendStock");
                dbServer.AddInParameter(objCommand, "StoreID", DbType.Int64, BizActionObj.ItemDetails.StoreID);
                dbServer.AddInParameter(objCommand, "ItemID", DbType.Int64, BizActionObj.ItemDetails.ItemID);
                dbServer.AddInParameter(objCommand, "UnitID", DbType.Int64, BizActionObj.ItemDetails.UnitID);
                dbServer.AddOutParameter(objCommand, "ResultStatus", DbType.Int32, 0);
                objReader = (DbDataReader)dbServer.ExecuteReader(objCommand);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(objCommand, "ResultStatus");

            }
            catch (Exception Ex)
            {
                throw;
            }
            return BizActionObj;
        }


        public override IValueObject ValidationForDuplicateRecords(IValueObject valueObject, clsUserVO UserVO)
        {
            GetStoreLocationLockBizActionVO BizActionObj = valueObject as GetStoreLocationLockBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ValidationForDuplicateRecordsForSuspendStock");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ObjStoreLocationLock.UnitID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.ObjStoreLocationLock.StoreID);
                dbServer.AddInParameter(command, "RackID", DbType.Int64, BizActionObj.ObjStoreLocationLock.RackID);
                dbServer.AddInParameter(command, "ShelfID", DbType.Int64, BizActionObj.ObjStoreLocationLock.ShelfID);
                dbServer.AddInParameter(command, "ContainerID", DbType.Int64, BizActionObj.ObjStoreLocationLock.ContainerID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

            throw new NotImplementedException();



        }

        //...........................................................

        #region For Item Selection Control

        public override IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemDetailsByIDBizActionVO BizActionObj = valueObject as clsGetItemDetailsByIDBizActionVO;
            try
            {
                DbCommand command;
                //if (BizActionObj.IsFromOpeningBalance)
                //    command = dbServer.GetStoredProcCommand("CIMS_GetItemListForOpeningBalanceSearch");
                //else
                command = dbServer.GetStoredProcCommand("GetItemDetailsByID");      //CIMS_GetItemListForSearch
                DbDataReader reader;

                //dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                //dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);

                //dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                //if (BizActionObj.ItemCode != null && BizActionObj.ItemCode.Length != 0)
                //    dbServer.AddInParameter(command, "ItemCode", DbType.String, BizActionObj.ItemCode);
                //if (BizActionObj.ItemName != null && BizActionObj.ItemName.Length != 0)
                //    dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemName);
                //if (BizActionObj.BrandName != null && BizActionObj.BrandName.Length != 0)
                //    dbServer.AddInParameter(command, "BrandName", DbType.String, BizActionObj.BrandName);

                ////if (BizActionObj.MoleculeName != null && BizActionObj.MoleculeName.Length != 0)
                //dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.ItemCategoryId);
                //dbServer.AddInParameter(command, "GroupId", DbType.Int64, BizActionObj.ItemGroupId);

                //dbServer.AddInParameter(command, "MoleculeName", DbType.Int64, BizActionObj.MoleculeName);

                //dbServer.AddInParameter(command, "ScrapCategoryItems", DbType.Boolean, BizActionObj.ShowScrapItems);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                ////dbServer.AddInParameter(command, "UnitID", DbType.Int64, uservo.UserLoginInfo.UnitId);

                //dbServer.AddInParameter(command, "ShowNonZeroStockBatches", DbType.Boolean, BizActionObj.ShowZeroStockBatches);    //to show items with > 0 AvailableStock

                //dbServer.AddInParameter(command, "PlusThreeMonthFlag", DbType.Boolean, BizActionObj.ShowNotShowPlusThreeMonthExp);

                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    //if (BizActionObj.ItemList == null)
                    //    BizActionObj.ItemList = new List<clsItemMasterVO>();

                    while (reader.Read())
                    {
                        clsItemMasterVO objVO = new clsItemMasterVO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        objVO.Status = false;
                        objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objVO.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                        objVO.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));

                        objVO.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        objVO.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));

                        objVO.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objVO.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));

                        objVO.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                        objVO.Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"]));
                        objVO.PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"]));
                        objVO.TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"]));
                        objVO.TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"]));
                        objVO.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                        objVO.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objVO.VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));

                        objVO.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));



                        objVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objVO.ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"]));

                        objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVO.ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"]));

                        //if (BizActionObj.ShowZeroStockBatches == true)
                        //    objVO.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));

                        ////objVO.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        ////objVO.MoleculeName = Convert.ToInt64(DALHelper.HandleDBNull(reader["MoleculeName"]));
                        ////if (BizActionObj.IsFromOpeningBalance)

                        objVO.ConversionFactor = Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"]));

                        //if (BizActionObj.IsFromOpeningBalance == true)
                        objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));

                        ////by Anjali.............................
                        //if (BizActionObj.StoreID != 0)//&& BizActionObj.ShowZeroStockBatches == false
                        //    objVO.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        ////.......................................

                        //if (BizActionObj.IsFromOpeningBalance == false && BizActionObj.StoreID != 0 && BizActionObj.IsFromStockAdjustment == false)
                        //{

                        objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        objVO.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));
                        objVO.ItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["othertax"]));
                        objVO.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        objVO.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                        objVO.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                        objVO.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));

                        if (BizActionObj.StoreID > 0)
                        {
                            objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        }

                        //By Anjali...........................
                        objVO.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));
                        objVO.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                        objVO.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));
                        objVO.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));
                        objVO.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                        objVO.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));
                        objVO.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                        objVO.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));

                        //if (BizActionObj.ShowZeroStockBatches != true)
                        //{
                        //    objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        //    objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        //    objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        //}

                        //.......................................
                        //}


                        //if (BizActionObj.IsFromOpeningBalance)
                        //{
                        //    objVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        //    objVO.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));

                        //    objVO.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                        //    objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));

                        //    objVO.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                        //    objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));

                        //    objVO.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));
                        //    objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        //}

                        //if (BizActionObj.IsFromOpeningBalance == true && BizActionObj.ShowZeroStockBatches == false)
                        //{
                        //    objVO.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        //    objVO.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));

                        //    objVO.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        //    objVO.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));

                        //    objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                        //    objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        //    objVO.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        //    objVO.InclusiveForOB = Convert.ToInt32(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                        //    objVO.ApplicableOnForOB = Convert.ToInt32(DALHelper.HandleDBNull(reader["VATApplicableOn"]));
                        //    objVO.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                        //}

                        //objVO.AssignSupplier = CheckItemSupplier(objVO.ID, BizActionObj.SupplierID);
                        //BizActionObj.ItemList.Add(objVO);

                        BizActionObj.objItem = new clsItemMasterVO();
                        BizActionObj.objItem = objVO;

                    }

                }

                //reader.NextResult();
                //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        #endregion

        public override IValueObject GetBarCodeCounterSale(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsCounterSaleBarCodeBizActionVO BizActionobj = valueObject as clsCounterSaleBarCodeBizActionVO;
            BizActionobj.ItemBatchListForBarCode = new List<clsItembatchSearchVO>();
            DbCommand command;
            DbDataReader reader = null;
            try
            {
                // The new Stored Procedure is Created.The existing is "CIMS_GetItemListForBarCode_CounterSale".
                command = dbServer.GetStoredProcCommand("CIMS_GetItemBatchListForSearchForBarCode");  //CIMS_GetItemListForBarCode_CounterSale_1
                dbServer.AddInParameter(command, "BarCode", DbType.String, BizActionobj.BarCode);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionobj.StoreId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionobj.UnitID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsItembatchSearchVO objItem = new clsItembatchSearchVO();
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        objItem.Status = false;
                        objItem.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objItem.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                        objItem.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                        objItem.BUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objItem.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"])); ;
                        objItem.Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"])); ;
                        objItem.PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"])); ;
                        objItem.TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"])); ;
                        objItem.TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"])); ;
                        objItem.AssignSupplier = false; //Convert.ToBoolean(DALHelper.HandleDBNull(reader["AssignSupplier"]));;
                        objItem.CategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])); ;
                        objItem.GroupId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"])); ;
                        objItem.ItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"])); ;
                        objItem.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"])); ;
                        objItem.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"])); ;
                        objItem.ConversionFactor = Convert.ToSingle(Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"])));
                        objItem.SGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTTax"])); ;
                        objItem.CGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTTax"]));
                        objItem.IGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTTax"]));
                        objItem.SGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtype"]));
                        objItem.SGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableon"]));
                        objItem.CGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtype"]));
                        objItem.CGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableon"]));
                        objItem.IGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtype"]));
                        objItem.IGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableon"]));
                        objItem.SGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"]));
                        objItem.CGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"]));
                        objItem.IGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"]));
                        objItem.SGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtypeSale"]));
                        objItem.SGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableonSale"]));
                        objItem.CGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtypeSale"]));
                        objItem.CGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableonSale"]));
                        objItem.IGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtypeSale"]));
                        objItem.IGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableonSale"]));
                        objItem.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                        objItem.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                        objItem.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objItem.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                        objItem.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        objItem.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));
                        objItem.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                        objItem.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                        objItem.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                        objItem.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        objItem.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objItem.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        objItem.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objItem.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                        objItem.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                        objItem.IssueVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));
                        objItem.IssueItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["othertax"]));
                        objItem.IssueItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        objItem.IssueItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                        objItem.DiscountOnPackageItem = 0;// ((clsItemMasterVO)(dataGrid2.SelectedItem)).DiscountPerc; 
                        objItem.StaffDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        objItem.WalkinDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["WalkinDiscount"]));
                        objItem.RegisteredPatientsDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RegisteredPatientsDiscount"]));

                        //***Batches
                        objItem.ID = Convert.ToInt64((DALExtension.HandleDBNull(reader["ID"])));
                        objItem.Status = false;
                        objItem.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objItem.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItem.CurrentStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentStock"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objItem.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objItem.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objItem.VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem.VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
                        objItem.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                        objItem.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                        objItem.AvailableStockInBase = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        objItem.PurchaseToBaseCFForStkAdj = 0; //((clsItemStockVO)(dgItemBatches.SelectedItem)).PurchaseToBaseCF;   //By Umesh
                        objItem.StockingToBaseCFForStkAdj = 0; //((clsItemStockVO)(dgItemBatches.SelectedItem)).StockingToBaseCF;   //By Umesh


                        BizActionobj.ItemBatchListForBarCode.Add(objItem);
                    }
                }
                reader.NextResult();
                int TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();

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
            return BizActionobj;

        }
    }
}

