using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using PalashDynamics.ValueObjects.Inventory.MaterialConsumption;
using System.Data;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsMaterialConsumptionDAL : clsBaseMaterialConsumptionDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsMaterialConsumptionDAL()
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


        public override IValueObject Add(IValueObject valueObject, clsUserVO userVO)
        {

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            clsAddMaterialConsumptionBizActionVO BizActionObj = valueObject as clsAddMaterialConsumptionBizActionVO;

            try
            {
                con.Open();
                trans = con.BeginTransaction();


                clsMaterialConsumptionVO objDetailsVO = BizActionObj.ConsumptionDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddMaterialConsumption");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, userVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, objDetailsVO.PackageID);  //Added by AJ Date 18/1/2017
                //  dbServer.AddInParameter(command, "ScrapSaleNo", DbType.String, objDetailsVO.ScrapSaleNo);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                dbServer.AddInParameter(command, "IsAgainstPatient", DbType.Int64, objDetailsVO.IsAgainstPatient);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);

                dbServer.AddInParameter(command, "TotalItems", DbType.Decimal, objDetailsVO.TotalItems);

                //Added bu AJ Date 2/1/2017
                dbServer.AddInParameter(command, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, objDetailsVO.Opd_Ipd_External);
                dbServer.AddInParameter(command, "TotalMRPAmount", DbType.Double, objDetailsVO.TotalMRPAmount);//Added bu AJ Date 1/3/2017

                dbServer.AddInParameter(command, "LinkPatientID", DbType.Int64, objDetailsVO.LinkPatientID);
                dbServer.AddInParameter(command, "LinkPatientUnitID", DbType.Int64, objDetailsVO.LinkPatientUnitID);
                //***//-----------------

                dbServer.AddInParameter(command, "IsAgainstPatientIndent", DbType.Boolean, objDetailsVO.IsAgainstPatientIndent);

                //For Package New Changes Added on 30042018
                dbServer.AddInParameter(command, "IsPackageConsumable", DbType.Boolean, objDetailsVO.IsPackageConsumable);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);


                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, userVO.ID);

                dbServer.AddInParameter(command, "AddedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now.Date);

                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ScrapID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ConsumptionDetails.ID = (long)dbServer.GetParameterValue(command, "ID");


                bool IsGetPreviousConsumptionAmount = false;
                foreach (var item in BizActionObj.ConsumptionDetails.ConsumptionItemDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddMaerialConsumptionDetails");
                    command1.Connection = con;
                    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "ConsumptionID", DbType.Int64, BizActionObj.ConsumptionDetails.ID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "BatchID", DbType.Int64, item.BatchId);
                    dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command1, "UsedQty", DbType.Decimal, item.UsedQty);
                    dbServer.AddInParameter(command1, "Rate", DbType.Decimal, item.Rate);
                    dbServer.AddInParameter(command1, "Amount", DbType.Decimal, item.Amount);
                    dbServer.AddInParameter(command1, "Remark", DbType.String, item.Remark);
                    dbServer.AddInParameter(command1, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command1, "ExpiryDate", DbType.Date, item.ExpiryDate);
                    dbServer.AddInParameter(command1, "ItemName", DbType.String, item.ItemName);
                    dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MRP);
                    //Added bu AJ Date 2/2/2017                                        
                    dbServer.AddInParameter(command1, "Opd_Ipd_External_Id", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                    dbServer.AddInParameter(command1, "Opd_Ipd_External_UnitId", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                    dbServer.AddInParameter(command1, "PackageID", DbType.Int64, objDetailsVO.PackageID);  //Added by AJ Date 18/1/2017
                    //Added by AJ Date 18/4/2017
                    dbServer.AddInParameter(command1, "PackageBillID", DbType.Int64, objDetailsVO.PackageBillID);
                    dbServer.AddInParameter(command1, "PackageBillUnitID", DbType.Int64, objDetailsVO.PackageBillUnitID);
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);

                    dbServer.AddInParameter(command1, "TotalPatientIndentReceiveQty", DbType.Decimal, item.TotalPatientIndentReceiveQty - item.TotalPatientIndentConsumptionQty);

                    if (objDetailsVO.IsPackageConsumable == true)   //For Package New Changes Added on 30042018
                    {
                        dbServer.AddInParameter(command1, "PackageConsumptionTypeID", DbType.Int64, 1);     // 1 : Package Consumable 
                    }
                    else if (objDetailsVO.IsAgainstPatientIndent == true)
                    {
                        dbServer.AddInParameter(command1, "PackageConsumptionTypeID", DbType.Int64, 2);     // 2 : Package Pharmacy Consumable
                    }


                    // For Package New Changes Commented on 02052018
                    //dbServer.AddOutParameter(command1, "ConsumptionAmount", DbType.Int64, int.MaxValue);
                    //dbServer.AddOutParameter(command1, "PreviousConsumptionAmount", DbType.Int64, int.MaxValue);
                    //***//-----------------

                    // For Package New Changes modified on 02052018
                    dbServer.AddOutParameter(command1, "ConsumptionAmount", DbType.Double, int.MaxValue);
                    dbServer.AddOutParameter(command1, "PreviousConsumptionAmount", DbType.Double, int.MaxValue);

                    # region For Conversion Factor
                    dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Decimal, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                    //    dbServer.AddInParameter(command1, "ConversionFactor", DbType.Decimal, item.ConversionFactor);
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command1, "StockingOty", DbType.Decimal, item.StockOty);
                    dbServer.AddInParameter(command1, "StockCF", DbType.Decimal, item.ConversionFactor);
                    # endregion
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.ConsumptionDetails.ConsumptionAmount = Convert.ToDouble(dbServer.GetParameterValue(command1, "ConsumptionAmount"));  //Added bu AJ Date 2/2/2017
                    if (IsGetPreviousConsumptionAmount == false)
                    {
                        IsGetPreviousConsumptionAmount = true;
                        BizActionObj.ConsumptionDetails.PreviousConsumptionAmount = Convert.ToDouble(dbServer.GetParameterValue(command1, "PreviousConsumptionAmount"));
                    }
                    item.StockDetails.BatchID = item.BatchId;
                    item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    item.StockDetails.ItemID = item.ItemId;
                    item.StockDetails.TransactionTypeID = InventoryTransactionType.MaterialConsumption;
                    item.StockDetails.TransactionID = BizActionObj.ConsumptionDetails.ID;
                    item.StockDetails.TransactionQuantity = (double)(item.BaseOty);

                    # region For Conversion Factor

                    item.StockDetails.BaseUOMID = item.BaseUOMID;
                    item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                    item.StockDetails.SUOMID = item.SUOMID;
                    item.StockDetails.ConversionFactor = item.ConversionFactor;
                    item.StockDetails.SelectedUOM = item.SelectedUOM;
                    item.StockDetails.StockingQuantity = (double)item.StockOty;
                    item.StockDetails.InputTransactionQuantity = (float)item.UsedQty;
                    # endregion

                    if (DALHelper.HandleDBNull(objDetailsVO.Date) == null)
                        item.StockDetails.Date = DateTime.Now;
                    else
                        item.StockDetails.Date = Convert.ToDateTime(objDetailsVO.Date);
                    if (DALHelper.HandleDBNull(objDetailsVO.Date) == null)
                        item.StockDetails.Time = DateTime.Now;
                    else
                        item.StockDetails.Time = Convert.ToDateTime(objDetailsVO.Time);
                    item.StockDetails.StoreID = objDetailsVO.StoreID;

                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                    obj.Details = item.StockDetails;
                    obj.Details.ID = 0;
                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, userVO, con, trans);

                    if (obj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    item.StockDetails.ID = obj.Details.ID;
                }

                # region For Save in IPD Patient Data in Bill
                //if (objDetailsVO.Opd_Ipd_External == 1)                                       // For Package New Changes Commented on 02052018
                //if (objDetailsVO != null && (objDetailsVO.IsPackageConsumable == true || objDetailsVO.IsAgainstPatientIndent == true))
                if (objDetailsVO != null && ((objDetailsVO.Opd_Ipd_External == 0 && (objDetailsVO.IsPackageConsumable == true || objDetailsVO.IsAgainstPatientIndent == true)) || (objDetailsVO.Opd_Ipd_External == 1)))      // Modified on 19Feb2019 for Package Flow in IPD
                {
                    if (objDetailsVO.Opd_Ipd_External == 1 || objDetailsVO.Opd_Ipd_External == 0)   // For Package New Changes Added on 02052018
                    {
                        clsBaseBillDAL BaseBiil = clsBaseBillDAL.GetInstance();
                        clsAddBillBizActionVO objBill = new clsAddBillBizActionVO();
                        if (objDetailsVO.PackageID > 0)
                        {
                            double PackageLimit = 0;

                            if (objDetailsVO.Opd_Ipd_External == 0)         // For Package New Changes Added on 02052018
                            {
                                if (objDetailsVO.IsAgainstPatientIndent == true)
                                {
                                    PackageLimit = objDetailsVO.PharmacyFixedRate;
                                }
                                else if (objDetailsVO.IsPackageConsumable == true)
                                {
                                    PackageLimit = objDetailsVO.PackageConsumableLimit;
                                }
                            }
                            else if (objDetailsVO.Opd_Ipd_External == 1)    // For Package New Changes Added on 02052018
                            {
                                PackageLimit = objDetailsVO.PharmacyFixedRate;
                            }

                            if ((objDetailsVO.Opd_Ipd_External == 0 && objDetailsVO.IsAgainstPatientIndent == true) || (objDetailsVO.Opd_Ipd_External == 1))    // For Package New Changes Added on 03052018
                            {
                                //if (BizActionObj.ConsumptionDetails.ConsumptionAmount > objDetailsVO.PharmacyFixedRate)     // For Package New Changes Commented on 30042018
                                //if (BizActionObj.ConsumptionDetails.ConsumptionAmount > PackageLimit)                         // For Package New Changes Added on 30042018 & Commented on 19062018
                                if (BizActionObj.ConsumptionDetails.ConsumptionAmount > 0 && PackageLimit > 0)                         // For Package New Changes Added on 30042018
                                {

                                    if (BizActionObj.ConsumptionDetails.PreviousConsumptionAmount == 0)
                                    {
                                        objBill = BizActionObj.ObjBillDetails;
                                        double ConcessionAmount = 0;
                                        double ItemMRP = 0;

                                        //double PharmacyFixedRate = objDetailsVO.PharmacyFixedRate;      // For Package New Changes Commented on 30042018
                                        double PharmacyFixedRate = PackageLimit;                          // For Package New Changes Added on 30042018

                                        foreach (var item in objBill.Details.PharmacyItems.Items)
                                        {
                                            double TotalItemMRP = item.MRP * item.Quantity;
                                            if (PharmacyFixedRate > 0)
                                            {

                                                if (BizActionObj.ObjBillDetails.Details.PharmacyItems.Items.Count() > 1)
                                                {
                                                    if (TotalItemMRP > PharmacyFixedRate)
                                                    {
                                                        double PackageDiscount = 0;
                                                        //item.ConcessionAmount = PharmacyFixedRate;        // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = PharmacyFixedRate;   // For Package New Changes Added on 18062018
                                                        PackageDiscount = ((TotalItemMRP - PharmacyFixedRate) / 100) * item.DiscountOnPackageItem;
                                                        //item.ConcessionAmount = item.ConcessionAmount + PackageDiscount;              // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = item.PackageConcessionAmount + PackageDiscount;  // For Package New Changes Added on 18062018
                                                        PharmacyFixedRate = 0;
                                                        //ConcessionAmount += item.ConcessionAmount;    // For Package New Changes Commented on 18062018
                                                    }
                                                    else
                                                    {
                                                        //item.ConcessionAmount = TotalItemMRP;         // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = TotalItemMRP;    // For Package New Changes Added on 18062018
                                                        ItemMRP = PharmacyFixedRate - TotalItemMRP;
                                                        PharmacyFixedRate = ItemMRP;
                                                        //ConcessionAmount += item.ConcessionAmount;    // For Package New Changes Commented on 18062018
                                                    }
                                                }
                                                else
                                                {
                                                    if (TotalItemMRP > PharmacyFixedRate)
                                                    {
                                                        double PackageDiscount = 0;
                                                        //item.ConcessionAmount = PharmacyFixedRate;        // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = PharmacyFixedRate;   // For Package New Changes Added on 18062018
                                                        PackageDiscount = ((TotalItemMRP - PharmacyFixedRate) / 100) * item.DiscountOnPackageItem;
                                                        //item.ConcessionAmount = item.ConcessionAmount + PackageDiscount;                 // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = item.PackageConcessionAmount + PackageDiscount;     // For Package New Changes Added on 18062018
                                                        //ConcessionAmount += item.ConcessionAmount;        // For Package New Changes Commented on 18062018
                                                    }
                                                    else
                                                    {
                                                        //item.ConcessionAmount = TotalItemMRP;         // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = TotalItemMRP;    // For Package New Changes Added on 18062018
                                                        ItemMRP = PharmacyFixedRate - TotalItemMRP;
                                                        PharmacyFixedRate = ItemMRP;
                                                        //ConcessionAmount += item.ConcessionAmount;    // For Package New Changes Commented on 18062018
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                double PackageDiscount = 0;
                                                PackageDiscount = ((TotalItemMRP) / 100) * item.DiscountOnPackageItem;

                                                //item.ConcessionAmount = PackageDiscount;                      // For Package New Changes Commented on 18062018
                                                item.PackageConcessionAmount = PackageDiscount;                 // For Package New Changes Added on 18062018
                                                //item.ConcessionPercentage = item.DiscountOnPackageItem;       // For Package New Changes Commented on 18062018
                                                item.PackageConcessionPercentage = item.DiscountOnPackageItem;  // For Package New Changes Added on 18062018
                                                //ConcessionAmount += item.ConcessionAmount;                    // For Package New Changes Commented on 18062018
                                            }
                                        }
                                        double NetBillAmt = BizActionObj.ObjBillDetails.Details.NetBillAmount;
                                        double PayNetbillAmt = NetBillAmt - ConcessionAmount;//objDetailsVO.PharmacyFixedRate; Change By Bhushanp For New Percentage Package Flow 31082017                               
                                        objBill.Details.NetBillAmount = PayNetbillAmt;
                                        objBill.Details.SelfAmount = PayNetbillAmt;
                                        objBill.Details.BalanceAmountSelf = PayNetbillAmt;
                                        objBill.Details.TotalConcessionAmount = ConcessionAmount;//objDetailsVO.PharmacyFixedRate;
                                    }   //else if (BizActionObj.ConsumptionDetails.PreviousConsumptionAmount < objDetailsVO.PharmacyFixedRate)    // For Package New Changes Commented on 30042018
                                    else if (BizActionObj.ConsumptionDetails.PreviousConsumptionAmount < PackageLimit)                            // For Package New Changes Added on 30042018
                                    {
                                        objBill = BizActionObj.ObjBillDetails;
                                        double ConcessionAmount = 0;
                                        double ItemMRP = 0;

                                        //double TotalConcessionAmount = objDetailsVO.PharmacyFixedRate - BizActionObj.ConsumptionDetails.PreviousConsumptionAmount;      // For Package New Changes Commented on 30042018
                                        double TotalConcessionAmount = PackageLimit - BizActionObj.ConsumptionDetails.PreviousConsumptionAmount;                          // For Package New Changes Added on 30042018

                                        foreach (var item in objBill.Details.PharmacyItems.Items)
                                        {
                                            double TotalItemMRP = item.MRP * item.Quantity;
                                            if (TotalConcessionAmount > 0)
                                            {
                                                if (BizActionObj.ObjBillDetails.Details.PharmacyItems.Items.Count() > 1)
                                                {
                                                    if (TotalItemMRP > TotalConcessionAmount)
                                                    {
                                                        double PackageDiscount = 0;
                                                        //item.ConcessionAmount = TotalConcessionAmount;        // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = TotalConcessionAmount;   // For Package New Changes Added on 18062018
                                                        PackageDiscount = ((TotalItemMRP - TotalConcessionAmount) / 100) * item.DiscountOnPackageItem;
                                                        //item.ConcessionAmount = item.ConcessionAmount + PackageDiscount;                  // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = item.PackageConcessionAmount + PackageDiscount;      // For Package New Changes Added on 18062018
                                                        TotalConcessionAmount = 0;
                                                        //ConcessionAmount += item.ConcessionAmount;            // For Package New Changes Commented on 18062018
                                                    }
                                                    else
                                                    {
                                                        //item.ConcessionAmount = TotalItemMRP;         // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = TotalItemMRP;    // For Package New Changes Added on 18062018
                                                        ItemMRP = TotalConcessionAmount - TotalItemMRP;
                                                        TotalConcessionAmount = ItemMRP; //Change By Bhushanp
                                                        //ConcessionAmount += item.ConcessionAmount;    // For Package New Changes Commented on 18062018
                                                    }
                                                }
                                                else
                                                {
                                                    if (TotalItemMRP > TotalConcessionAmount)
                                                    {
                                                        double PackageDiscount = 0;
                                                        //item.ConcessionAmount = TotalConcessionAmount;        // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = TotalConcessionAmount;   // For Package New Changes Added on 18062018
                                                        PackageDiscount = ((TotalItemMRP - TotalConcessionAmount) / 100) * item.DiscountOnPackageItem;
                                                        //item.ConcessionAmount = item.ConcessionAmount + PackageDiscount;              // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = item.PackageConcessionAmount + PackageDiscount;  // For Package New Changes Added on 18062018
                                                        //ConcessionAmount += item.ConcessionAmount;            // For Package New Changes Commented on 18062018
                                                    }
                                                    else
                                                    {
                                                        //item.ConcessionAmount = TotalItemMRP;         // For Package New Changes Commented on 18062018
                                                        item.PackageConcessionAmount = TotalItemMRP;    // For Package New Changes Added on 18062018
                                                        ItemMRP = TotalConcessionAmount - TotalItemMRP;
                                                        TotalConcessionAmount = ItemMRP; //Change By Bhushanp
                                                        //ConcessionAmount += item.ConcessionAmount;    // For Package New Changes Commented on 18062018
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                double PackageDiscount = 0;
                                                PackageDiscount = ((TotalItemMRP) / 100) * item.DiscountOnPackageItem;

                                                //item.ConcessionAmount = PackageDiscount;          // For Package New Changes Commented on 18062018
                                                item.PackageConcessionAmount = PackageDiscount;     // For Package New Changes Added on 18062018
                                                //item.ConcessionPercentage = item.DiscountOnPackageItem;       // For Package New Changes Commented on 18062018
                                                item.PackageConcessionPercentage = item.DiscountOnPackageItem;  // For Package New Changes Added on 18062018
                                                //ConcessionAmount += item.ConcessionAmount;        // For Package New Changes Commented on 18062018
                                            }
                                        }
                                        objBill.Details.TotalConcessionAmount = ConcessionAmount;
                                        double NetBillAmt = BizActionObj.ObjBillDetails.Details.NetBillAmount;
                                        double PayNetbillAmt = NetBillAmt - ConcessionAmount;
                                        objBill.Details.NetBillAmount = PayNetbillAmt;
                                        objBill.Details.SelfAmount = PayNetbillAmt;
                                        objBill.Details.BalanceAmountSelf = PayNetbillAmt;
                                    }
                                    else
                                    {
                                        objBill = BizActionObj.ObjBillDetails;
                                        /////BEGIN New
                                        double ConcessionAmount = 0;
                                        foreach (var item in objBill.Details.PharmacyItems.Items)
                                        {
                                            double TotalItemMRP = item.MRP * item.Quantity;
                                            double PackageDiscount = 0;
                                            PackageDiscount = ((TotalItemMRP) / 100) * item.DiscountOnPackageItem;

                                            item.ConcessionAmount = PackageDiscount;
                                            item.ConcessionPercentage = item.DiscountOnPackageItem;
                                            ConcessionAmount += item.ConcessionAmount;
                                        }
                                        objBill.Details.TotalConcessionAmount = objBill.Details.TotalConcessionAmount + ConcessionAmount;

                                        objBill.Details.PharmacyItems.ConcessionAmount = objBill.Details.TotalConcessionAmount + ConcessionAmount;      // For Package New Changes Added on 02052018      

                                        double NetBillAmt = BizActionObj.ObjBillDetails.Details.TotalBillAmount;
                                        double PayNetbillAmt = NetBillAmt - objBill.Details.TotalConcessionAmount;
                                        objBill.Details.NetBillAmount = PayNetbillAmt;
                                        objBill.Details.SelfAmount = PayNetbillAmt;
                                        objBill.Details.BalanceAmountSelf = PayNetbillAmt;
                                        //////END NEW
                                    }

                                    objBill = (clsAddBillBizActionVO)BaseBiil.Add(objBill, userVO, con, trans);

                                    if (objBill.SuccessStatus == -1)
                                    {
                                        throw new Exception();
                                    }

                                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateMaterialConsumptionBillID");
                                    command2.Connection = con;

                                    dbServer.AddInParameter(command2, "MaterialConsumptionID", DbType.Int64, BizActionObj.ConsumptionDetails.ID);
                                    dbServer.AddInParameter(command2, "MaterialConsumptionUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command2, "BillID", DbType.Int64, objBill.Details.ID);
                                    dbServer.AddInParameter(command2, "BillUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);

                                }   // if (BizActionObj.ConsumptionDetails.ConsumptionAmount > PackageLimit)

                            }
                        }
                        else
                        {
                            #region commented on 29062018 (while selecting Patient Indent Stock with no package selection)

                            if (objDetailsVO.Opd_Ipd_External == 1)     // Added on 20Feb2019 for Package Flow in IPD
                            {
                                objBill = BizActionObj.ObjBillDetails;
                                objBill = (clsAddBillBizActionVO)BaseBiil.Add(objBill, userVO, con, trans);

                                if (objBill.SuccessStatus == -1)
                                {
                                    throw new Exception();
                                }
                            }

                            //DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateMaterialConsumptionBillID");
                            //command3.Connection = con;

                            //dbServer.AddInParameter(command3, "MaterialConsumptionID", DbType.Int64, BizActionObj.ConsumptionDetails.ID);
                            //dbServer.AddInParameter(command3, "MaterialConsumptionUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command3, "BillID", DbType.Int64, objBill.Details.ID);
                            //dbServer.AddInParameter(command3, "BillUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                            //int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            #endregion
                        }

                    }
                }
                # endregion

                trans.Commit();

            }


            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.ConsumptionDetails = null;
                BizActionObj.SuccessStatus = -1;
                throw;

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;

        }
        public override IValueObject GetMaterialConsumptionList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetMatarialConsumptionListBizActionVO objItem = valueObject as clsGetMatarialConsumptionListBizActionVO;
            clsMaterialConsumptionVO objItemVO = null;


            try
            {

                if (!objItem.IsPatientAgainstMaterialConsumption)
                {
                    DbCommand command;
                    command = dbServer.GetStoredProcCommand("CIMS_GetMaterialConsumption");

                    dbServer.AddInParameter(command, "StoreId", DbType.Int64, objItem.StoreId);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);//UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objItem.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objItem.ToDate);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItemVO = new clsMaterialConsumptionVO();
                            objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                            objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            //objItemVO.Date = objItemVO.Date.Value.ToString("dd-MM-yyyy");
                            objItemVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                            objItemVO.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                            //objItemVO.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalMRPAmount"])); //Added by AJ Date 1/3/2017
                            objItemVO.ConsumptionNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConsumptionNo"]));
                            objItemVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                            objItemVO.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                            objItemVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"]));
                            objItemVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                            objItemVO.BillDate = (DateTime?)(DALHelper.HandleDate(reader["BillDate"]));
                            objItemVO.TotalBillAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                            objItemVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                            objItemVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));  //Added by AJ Date 9/1/2017
                            objItemVO.PackageName = Convert.ToString(DALHelper.HandleDBNull(reader["PackageName"]));
                            objItem.ConsumptionList.Add(objItemVO);
                        }
                    }

                    reader.NextResult();
                    objItem.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                }
                else //Added by AJ Date 20/1/2017
                {
                    DbCommand command;
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientAgainstMaterialConsumption");

                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objItem.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objItem.PatientUnitID);
                    dbServer.AddInParameter(command, "AdmID", DbType.Int64, objItem.AdmID);
                    dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, objItem.AdmissionUnitID);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItemVO = new clsMaterialConsumptionVO();
                            objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                            objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            objItemVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                            objItemVO.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                            objItemVO.ConsumptionNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConsumptionNo"]));
                            objItemVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                            objItemVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"]));
                            objItemVO.TotalBillAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBillAmount"]));

                            objItemVO.ItemId = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                            objItemVO.BatchId = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                            objItemVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                            objItemVO.UsedQty = (decimal)DALHelper.HandleDBNull(reader["UsedQty"]);
                            objItemVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                            objItemVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                            objItemVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                            objItemVO.TransactionUOM = (string)DALHelper.HandleDBNull(reader["Description"]);
                            objItemVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                            objItemVO.PackageName = (string)DALHelper.HandleDBNull(reader["PackageName"]);

                            objItem.ConsumptionList.Add(objItemVO);


                        }
                    }
                    reader.NextResult();
                    objItem.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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
        public override IValueObject GetMaterialConsumptionItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMatarialConsumptionItemListBizActionVO BizActionObj = valueObject as clsGetMatarialConsumptionItemListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ GetMaterialConsumptionItemList");
                DbDataReader reader;


                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ConsumptionID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsMaterialConsumptionItemDetailsVO>();
                    while (reader.Read())
                    {
                        clsMaterialConsumptionItemDetailsVO objVO = new clsMaterialConsumptionItemDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.ConsumptionID = (long)DALHelper.HandleDBNull(reader["ConsumptionID"]);
                        objVO.ItemId = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objVO.BatchId = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.UsedQty = (decimal)DALHelper.HandleDBNull(reader["UsedQty"]);
                        objVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);  
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.IsAgainstPatientIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgainstPatientIndent"]));
                        if (objVO.UsedQty == 1)
                        {
                            if (objVO.IsAgainstPatientIndent == true)
                            {
                                objVO.Amount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                            }
                            else
                            {
                                objVO.Amount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            }
                        }
                        else
                        {
                            if (objVO.IsAgainstPatientIndent == true)
                            {
                                objVO.Amount = Convert.ToDecimal(objVO.MRP) * objVO.UsedQty;
                            }
                            else
                            {
                                objVO.Amount = objVO.Rate * objVO.UsedQty;
                            }
                            //objVO.Amount = objVO.Rate * objVO.UsedQty;
                        }
                        // objVO.Amount = (decimal)DALHelper.HandleDBNull(reader["Amount"]);
                        objVO.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.TransactionUOM = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objVO.Flag = true;



                        //objVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);

                        BizActionObj.ItemList.Add(objVO);

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

        public override IValueObject GetPatientIndentReceiveStock(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMatarialConsumptionItemListBizActionVO BizActionObj = valueObject as clsGetMatarialConsumptionItemListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientIndentReceiveStockForMaterialConsm");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.objConsumptionVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.objConsumptionVO.PatientUnitID);
                dbServer.AddInParameter(command, "ItemIDs", DbType.String, BizActionObj.objConsumptionVO.ItemIDs);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.objConsumptionVO.StoreID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsMaterialConsumptionItemDetailsVO>();
                    while (reader.Read())
                    {
                        clsMaterialConsumptionItemDetailsVO objVO = new clsMaterialConsumptionItemDetailsVO();

                        objVO.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.TotalPatientIndentReceiveQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalPatientIndentReceiveQty"]));
                        objVO.TotalPatientIndentConsumptionQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalPatientIndentConsumptionQty"]));
                        BizActionObj.ItemList.Add(objVO);

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



    }


}

