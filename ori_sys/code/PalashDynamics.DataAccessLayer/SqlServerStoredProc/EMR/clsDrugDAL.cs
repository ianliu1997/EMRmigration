using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsDrugDAL : clsBaseDrugDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsDrugDAL()
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

        public override IValueObject GetDrugList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDrugListBizActionVO BizActionObj = valueObject as clsGetDrugListBizActionVO;
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDrugListByCategoryID");

                dbServer.AddInParameter(command, "TheraputicID", DbType.Int64, BizActionObj.TheraputicID);
                dbServer.AddInParameter(command, "MoleculeID", DbType.Int64, BizActionObj.MoleculeID);
                dbServer.AddInParameter(command, "GroupID", DbType.Int64, BizActionObj.GroupID);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);
                dbServer.AddInParameter(command, "PregnancyID", DbType.Int64, BizActionObj.PregnancyID);
                // Commented by Harish on 14 April 2011 12:15 PM
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, 0);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objDrugList == null)
                        BizActionObj.objDrugList = new List<clsDrugVO>();

                    while (reader.Read())
                    {
                        clsDrugVO objDrugVO = new clsDrugVO();

                        objDrugVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objDrugVO.DrugName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objDrugVO.CategoryID = (long)DALHelper.HandleDBNull(reader["TherClass"]);


                        BizActionObj.objDrugList.Add(objDrugVO);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;

        }

        public override IValueObject AddPatientPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientPrescriptionBizActionVO BizActionObj = valueObject as clsAddPatientPrescriptionBizActionVO;

            try
            {
                clsPatientPrescriptionVO objPatientPrescriptionVO = BizActionObj.PatientPrescriptionSummary;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescription");


                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientPrescriptionVO.LinkServer);
                if (objPatientPrescriptionVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientPrescriptionVO.LinkServer.Replace(@"\", "_"));


                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objPatientPrescriptionVO.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientPrescriptionVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPatientPrescriptionVO.PatientUnitID);

                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objPatientPrescriptionVO.TemplateID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objPatientPrescriptionVO.DoctorID);
                dbServer.AddInParameter(command, "IsFrom", DbType.Int32, objPatientPrescriptionVO.IsFrom);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPatientPrescriptionVO.UnitID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientPrescriptionVO.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientPrescriptionVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientPrescriptionSummary.ID = (long)dbServer.GetParameterValue(command, "ID");


                if (BizActionObj.PatientPrescriptionSummary.ID != 0)
                {
                    List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetailVO = BizActionObj.PatientPrescriptionDetail;

                    int count = objPatientPrescriptionDetailVO.Count;
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDeatail");

                            dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientPrescriptionDetailVO[i].LinkServer);
                            if (objPatientPrescriptionDetailVO[i].LinkServer != null)
                                dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientPrescriptionDetailVO[i].LinkServer.Replace(@"\", "_"));
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objPatientPrescriptionVO.UnitID);
                            dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PatientPrescriptionSummary.ID);
                            dbServer.AddInParameter(command1, "DrugID", DbType.Int64, objPatientPrescriptionDetailVO[i].DrugID);
                            dbServer.AddInParameter(command1, "Dose", DbType.String, objPatientPrescriptionDetailVO[i].Dose);
                            dbServer.AddInParameter(command1, "Route", DbType.String, objPatientPrescriptionDetailVO[i].Route);
                            dbServer.AddInParameter(command1, "Frequency", DbType.String, objPatientPrescriptionDetailVO[i].Frequency);
                            dbServer.AddInParameter(command1, "Days", DbType.Int64, objPatientPrescriptionDetailVO[i].Days);
                            dbServer.AddInParameter(command1, "Quantity", DbType.Int64, objPatientPrescriptionDetailVO[i].Quantity);
                            dbServer.AddInParameter(command1, "ItemName", DbType.String, objPatientPrescriptionDetailVO[i].DrugName);
                            dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objPatientPrescriptionDetailVO[i].IsOther);
                            dbServer.AddInParameter(command1, "Reason", DbType.String, objPatientPrescriptionDetailVO[i].Reason);
                            dbServer.AddInParameter(command1, "PendingQuantity", DbType.Int64, objPatientPrescriptionDetailVO[i].Quantity);

                            //dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objPatientPrescriptionVO.UnitID);
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientPrescriptionDetailVO[i].ID);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command1);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                            BizActionObj.PatientPrescriptionDetail[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                        }

                        catch (Exception ex)
                        {
                            throw;
                        }
                        finally
                        {

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

            }
            return valueObject;
        }

        public override IValueObject GetPatientPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            DbDataReader reader;
            clsGetPatientPrescriptionBizActionVO BizActionObj = valueObject as clsGetPatientPrescriptionBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescription");

                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.PatientPrescriptionSummary.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientPrescriptionSummary.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientPrescriptionSummary.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.PatientPrescriptionSummary.TemplateID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.PatientPrescriptionSummary.DoctorID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.PatientPrescriptionSummary.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    reader.Read();
                    BizActionObj.PatientPrescriptionSummary.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            reader.Close();
            return valueObject;
        }

        public override IValueObject UpdatePatientPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientPrescriptionBizActionVO BizActionObj = valueObject as clsUpdatePatientPrescriptionBizActionVO;


            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientPrescriptionDeatail");

                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetailVO = BizActionObj.PatientPrescriptionDetail;

                int count = objPatientPrescriptionDetailVO.Count;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDeatail");

                        dbServer.AddInParameter(command, "LinkServer", DbType.String, objPatientPrescriptionDetailVO[i].LinkServer);
                        if (objPatientPrescriptionDetailVO[i].LinkServer != null)
                            dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPatientPrescriptionDetailVO[i].LinkServer.Replace(@"\", "_"));

                        dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                        dbServer.AddInParameter(command1, "DrugID", DbType.Int64, objPatientPrescriptionDetailVO[i].DrugID);
                        dbServer.AddInParameter(command1, "Dose", DbType.String, objPatientPrescriptionDetailVO[i].Dose);
                        dbServer.AddInParameter(command1, "Route", DbType.String, objPatientPrescriptionDetailVO[i].Route);
                        dbServer.AddInParameter(command1, "Frequency", DbType.String, objPatientPrescriptionDetailVO[i].Frequency);
                        dbServer.AddInParameter(command1, "Days", DbType.Int64, objPatientPrescriptionDetailVO[i].Days);
                        dbServer.AddInParameter(command1, "Quantity", DbType.Int64, objPatientPrescriptionDetailVO[i].Quantity);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "ItemName", DbType.String, objPatientPrescriptionDetailVO[i].DrugName);
                        dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objPatientPrescriptionDetailVO[i].IsOther);
                        dbServer.AddInParameter(command1, "Reason", DbType.String, objPatientPrescriptionDetailVO[i].Reason);
                        dbServer.AddInParameter(command1, "PendingQuantity", DbType.Int64, objPatientPrescriptionDetailVO[i].Quantity);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientPrescriptionDetailVO[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.PatientPrescriptionDetail[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }

                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {

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

        public override IValueObject GetPatientPrescriptionDetailByVisitID(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            DbDataReader reader;
            double TotalNewPendingQuantity = 0;
            clsGetPatientPrescriptionDetailByVisitIDBizActionVO BizActionObj = valueObject as clsGetPatientPrescriptionDetailByVisitIDBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionByVisitID");

                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IsFrom", DbType.Int32, BizActionObj.IsFrom);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientPrescriptionDetail == null)
                        BizActionObj.PatientPrescriptionDetail = new List<clsPatientPrescriptionDetailVO>();

                    while (reader.Read())
                    {
                        clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();

                        objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPrescriptionVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                        objPrescriptionVO.DrugID = (long)DALHelper.HandleDBNull(reader["DrugID"]);
                        objPrescriptionVO.DrugName = (string)DALHelper.HandleDBNull(reader["DrugName"]);
                        objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                        objPrescriptionVO.Route = (string)DALHelper.HandleDBNull(reader["Route"]);
                        objPrescriptionVO.Frequency = (string)DALHelper.HandleDBNull(reader["Frequency"]);
                        objPrescriptionVO.Days = (int?)DALHelper.HandleDBNull(reader["Days"]);
                        objPrescriptionVO.Quantity = (double)(Int32)DALHelper.HandleDBNull(reader["Quantity"]);
                        objPrescriptionVO.IsBatchRequired = (bool?)DALHelper.HandleDBNull(reader["BatchesRequired"]);
                        objPrescriptionVO.PendingQuantity = (double)Convert.ToInt32(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objPrescriptionVO.NewPendingQuantity = (double)Convert.ToInt32(DALHelper.HandleDBNull(reader["NewPendingQuantity"]));
                        TotalNewPendingQuantity = TotalNewPendingQuantity + objPrescriptionVO.NewPendingQuantity;

                        //by neena
                        objPrescriptionVO.TotalNewPendingQuantity = objPrescriptionVO.TotalNewPendingQuantity+objPrescriptionVO.NewPendingQuantity;
                        objPrescriptionVO.UOM = (string)DALHelper.HandleDBNull(reader["UOM"]); 

                        //

                        //By Anjali...........................
                        objPrescriptionVO.Manufacture = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"]));
                        objPrescriptionVO.PregnancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"]));

                        objPrescriptionVO.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));

                        objPrescriptionVO.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                        objPrescriptionVO.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));

                        objPrescriptionVO.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));

                        objPrescriptionVO.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                        objPrescriptionVO.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));

                        objPrescriptionVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objPrescriptionVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objPrescriptionVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        objPrescriptionVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                        objPrescriptionVO.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                        objPrescriptionVO.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));

                        objPrescriptionVO.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        objPrescriptionVO.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));


                        objPrescriptionVO.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objPrescriptionVO.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));


                        objPrescriptionVO.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                        objPrescriptionVO.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));

                        objPrescriptionVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objPrescriptionVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objPrescriptionVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        objPrescriptionVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));

                        objPrescriptionVO.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));

                        #region  // Begin Properties for Sale 29062017

                        objPrescriptionVO.SGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTTaxSale"]));
                        objPrescriptionVO.CGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTTaxSale"]));
                        objPrescriptionVO.IGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTTaxSale"]));
                            //objVO.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                            //------------------------------------------

                        objPrescriptionVO.SGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtypeSale"]));
                        objPrescriptionVO.SGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableonSale"]));

                        objPrescriptionVO.CGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtypeSale"]));
                        objPrescriptionVO.CGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableonSale"]));

                        objPrescriptionVO.IGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtypeSale"]));
                        objPrescriptionVO.IGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableonSale"]));

                            // End Properties for Sale 29062017

                            #endregion
                        //.......................................
                        objPrescriptionVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])); //***//
                        objPrescriptionVO.Billed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BillDone"]));                     


                        BizActionObj.PatientPrescriptionDetail.Add(objPrescriptionVO);
                    }
                    BizActionObj.TotalNewPendingQuantity = TotalNewPendingQuantity;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            reader.Close();
            return valueObject;
        }

        //added by neena
        public override IValueObject GetPatientPrescriptionDetailByVisitIDForPrint(IValueObject valueObject, clsUserVO UserVo)
        {

            bool CurrentMethodExecutionStatus = true;
            DbDataReader reader;
            clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO BizActionObj = valueObject as clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionDetailsForPrint");

                dbServer.AddInParameter(command, "PrescriptionDetailsID", DbType.String, BizActionObj.SendPrescriptionID);
                dbServer.AddInParameter(command, "PrescriptionDetailsUnitID", DbType.Int64, BizActionObj.PatientPrescriptionDetailObj.UnitID);
               

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (BizActionObj.PatientPrescriptionDetail == null)
                    BizActionObj.PatientPrescriptionDetail = new List<clsPatientPrescriptionDetailVO>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();

                        objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["PrescriptionDetailsID"]);
                        objPrescriptionVO.UnitID = (long)DALHelper.HandleDBNull(reader["PrescriptionDetailsUnitID"]);                          
                        objPrescriptionVO.SaleQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["SaleQuantity"]));                   

                        BizActionObj.PatientPrescriptionDetail.Add(objPrescriptionVO);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            reader.Close();
            return valueObject;
        }

        public override IValueObject AddPatientPrescriptionResason(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientPrescriptionReasonOnCounterSaleBizActionVO BizActionObj = valueObject as clsAddPatientPrescriptionReasonOnCounterSaleBizActionVO;

            try
            {
                clsPatientPrescriptionReasonOncounterSaleVO objPatientPrescriptionReason = BizActionObj.PatientPrescriptionReason;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePresciptionReasonOnCounterSale");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPatientPrescriptionReason.UnitID);
                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, objPatientPrescriptionReason.PrescriptionID);
                dbServer.AddInParameter(command, "PrescriptionUnitID", DbType.Int64, objPatientPrescriptionReason.PrescriptionUnitID);
                dbServer.AddInParameter(command, "Reason", DbType.String, objPatientPrescriptionReason.Reason);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "Status", DbType.String, objPatientPrescriptionReason.Status);  
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPatientPrescriptionReason.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientPrescriptionReason.ID = (long)dbServer.GetParameterValue(command, "ID");

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


        public override IValueObject GetPatientPrescriptionReason(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            DbDataReader reader;
            clsGetPrescriptionReasonOnCounterSaleBizActionVO BizActionObj = valueObject as clsGetPrescriptionReasonOnCounterSaleBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPresciptionReasonOnCounterSale");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.PatientPrescriptionReason.UnitID);
                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, BizActionObj.PatientPrescriptionReason.PrescriptionID);
                dbServer.AddInParameter(command, "PrescriptionUnitID", DbType.Int64, BizActionObj.PatientPrescriptionReason.PrescriptionUnitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientPrescriptionReasonList == null)
                        BizActionObj.PatientPrescriptionReasonList = new List<clsPatientPrescriptionReasonOncounterSaleVO>();

                    while (reader.Read())
                    {
                        clsPatientPrescriptionReasonOncounterSaleVO objPrescriptionVO = new clsPatientPrescriptionReasonOncounterSaleVO();

                        objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPrescriptionVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                        objPrescriptionVO.PrescriptionUnitID = (long)DALHelper.HandleDBNull(reader["PrescriptionUnitID"]);
                        objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                        objPrescriptionVO.AddedDateTime =(DateTime?) DALHelper.HandleDBNull(reader["AddedDateTime"]);
                        BizActionObj.PatientPrescriptionReasonList.Add(objPrescriptionVO);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            reader.Close();
            return valueObject;
        }


        public override IValueObject GetPatientPrescriptionID(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            DbDataReader reader;
            clsGetPrescriptionIDBizActionVO BizActionObj = valueObject as clsGetPrescriptionIDBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionIDByVisitID");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.PatientPrescriptionReason.UnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.PatientPrescriptionReason.VisitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                        
                        BizActionObj.PatientPrescriptionReason.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                        BizActionObj.PatientPrescriptionReason.PrescriptionUnitID = (long)DALHelper.HandleDBNull(reader["PrescriptionUnitID"]);                      
                        
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            reader.Close();
            return valueObject;
        }
        //



        public override IValueObject AddPCR(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPCRBizActionVO BizActionObj = valueObject as clsAddPCRBizActionVO;

            try
            {
                clsPCRVO objPCRVO = BizActionObj.PCRDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddEMR_PCR");


                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPCRVO.LinkServer);
                if (objPCRVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPCRVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objPCRVO.TemplateID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPCRVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPCRVO.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objPCRVO.VisitID);
                dbServer.AddInParameter(command, "ComplaintReported", DbType.String, objPCRVO.ComplaintReported);
                dbServer.AddInParameter(command, "ChiefComplaints", DbType.String, objPCRVO.ChiefComplaints);
                dbServer.AddInParameter(command, "PastHistory", DbType.String, objPCRVO.PastHistory);
                dbServer.AddInParameter(command, "DrugHistory", DbType.String, objPCRVO.DrugHistory);
                dbServer.AddInParameter(command, "Allergies", DbType.String, objPCRVO.Allergies);
                dbServer.AddInParameter(command, "Investigations", DbType.String, objPCRVO.Investigations);
                dbServer.AddInParameter(command, "PovisionalDiagnosis", DbType.String, objPCRVO.PovisionalDiagnosis);
                dbServer.AddInParameter(command, "FinalDiagnosis", DbType.String, objPCRVO.FinalDiagnosis);
                dbServer.AddInParameter(command, "Hydration", DbType.String, objPCRVO.Hydration);
                dbServer.AddInParameter(command, "IVHydration4", DbType.String, objPCRVO.IVHydration4);
                dbServer.AddInParameter(command, "ZincSupplement", DbType.String, objPCRVO.ZincSupplement);
                dbServer.AddInParameter(command, "Nutritions", DbType.String, objPCRVO.Nutritions);
                dbServer.AddInParameter(command, "AdvisoryAttached", DbType.String, objPCRVO.AdvisoryAttached);
                dbServer.AddInParameter(command, "WhenToVisitHospital", DbType.String, objPCRVO.WhenToVisitHospital);
                dbServer.AddInParameter(command, "SpecificInstructions", DbType.String, objPCRVO.SpecificInstructions);
                dbServer.AddInParameter(command, "FollowUpDate", DbType.DateTime, objPCRVO.FollowUpDate);
                dbServer.AddInParameter(command, "FollowUpAt", DbType.String, objPCRVO.FollowUpAt);
                dbServer.AddInParameter(command, "ReferralTo", DbType.String, objPCRVO.ReferralTo);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPCRVO.UnitID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPCRVO.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPCRVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PCRDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

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

        public override IValueObject UpdatePCR(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePCRBizActionVO BizActionObj = valueObject as clsUpdatePCRBizActionVO;

            try
            {
                clsPCRVO objPCRVO = BizActionObj.PCRDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateEMR_PCR");


                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPCRVO.LinkServer);
                if (objPCRVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPCRVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPCRVO.ID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objPCRVO.VisitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objPCRVO.TemplateID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPCRVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objPCRVO.PatientUnitID);
                dbServer.AddInParameter(command, "ComplaintReported", DbType.String, objPCRVO.ComplaintReported);
                dbServer.AddInParameter(command, "ChiefComplaints", DbType.String, objPCRVO.ChiefComplaints);
                dbServer.AddInParameter(command, "PastHistory", DbType.String, objPCRVO.PastHistory);
                dbServer.AddInParameter(command, "DrugHistory", DbType.String, objPCRVO.DrugHistory);
                dbServer.AddInParameter(command, "Allergies", DbType.String, objPCRVO.Allergies);
                dbServer.AddInParameter(command, "Investigations", DbType.String, objPCRVO.Investigations);
                dbServer.AddInParameter(command, "PovisionalDiagnosis", DbType.String, objPCRVO.PovisionalDiagnosis);
                dbServer.AddInParameter(command, "FinalDiagnosis", DbType.String, objPCRVO.FinalDiagnosis);
                dbServer.AddInParameter(command, "Hydration", DbType.String, objPCRVO.Hydration);
                dbServer.AddInParameter(command, "IVHydration4", DbType.String, objPCRVO.IVHydration4);
                dbServer.AddInParameter(command, "ZincSupplement", DbType.String, objPCRVO.ZincSupplement);
                dbServer.AddInParameter(command, "Nutritions", DbType.String, objPCRVO.Nutritions);
                dbServer.AddInParameter(command, "AdvisoryAttached", DbType.String, objPCRVO.AdvisoryAttached);
                dbServer.AddInParameter(command, "WhenToVisitHospital", DbType.String, objPCRVO.WhenToVisitHospital);
                dbServer.AddInParameter(command, "SpecificInstructions", DbType.String, objPCRVO.SpecificInstructions);
                dbServer.AddInParameter(command, "FollowUpDate", DbType.DateTime, objPCRVO.FollowUpDate);
                dbServer.AddInParameter(command, "FollowUpAt", DbType.String, objPCRVO.FollowUpAt);
                dbServer.AddInParameter(command, "ReferralTo", DbType.String, objPCRVO.ReferralTo);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPCRVO.UnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPCRVO.Status);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

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

        public override IValueObject AddCaseReferral(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCaseReferralBizActionVO BizActionObj = valueObject as clsAddCaseReferralBizActionVO;

            try
            {
                clsCaseReferralVO objCaseReferralVO = BizActionObj.CaseReferralDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddEMR_CaseReferral");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objCaseReferralVO.LinkServer);
                if (objCaseReferralVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objCaseReferralVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objCaseReferralVO.TemplateID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objCaseReferralVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objCaseReferralVO.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objCaseReferralVO.VisitID);
                dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, objCaseReferralVO.ReferredToDoctorID);
                dbServer.AddInParameter(command, "ReferredToClinicID", DbType.Int64, objCaseReferralVO.ReferredToClinicID);
                dbServer.AddInParameter(command, "ReferredToMobile", DbType.String, objCaseReferralVO.ReferredToMobile);
                dbServer.AddInParameter(command, "ProvisionalDiagnosis", DbType.String, objCaseReferralVO.ProvisionalDiagnosis);
                dbServer.AddInParameter(command, "ChiefComplaints", DbType.String, objCaseReferralVO.ChiefComplaints);
                dbServer.AddInParameter(command, "Summary", DbType.String, objCaseReferralVO.Summary);
                dbServer.AddInParameter(command, "Observations", DbType.String, objCaseReferralVO.Observations);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objCaseReferralVO.UnitID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objCaseReferralVO.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCaseReferralVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.CaseReferralDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

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

        public override IValueObject UpdateCaseReferral(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCaseReferralBizActionVO BizActionObj = valueObject as clsUpdateCaseReferralBizActionVO;

            try
            {
                clsCaseReferralVO objCaseReferralVO = BizActionObj.CaseReferralDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateEMR_CaseReferral");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objCaseReferralVO.LinkServer);
                if (objCaseReferralVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objCaseReferralVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "ID", DbType.Int64, objCaseReferralVO.ID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objCaseReferralVO.VisitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objCaseReferralVO.TemplateID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objCaseReferralVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objCaseReferralVO.PatientUnitID);
                dbServer.AddInParameter(command, "ReferredToDoctorID", DbType.Int64, objCaseReferralVO.ReferredToDoctorID);
                dbServer.AddInParameter(command, "ReferredToClinicID", DbType.Int64, objCaseReferralVO.ReferredToClinicID);
                dbServer.AddInParameter(command, "ReferredToMobile", DbType.String, objCaseReferralVO.ReferredToMobile);
                dbServer.AddInParameter(command, "ProvisionalDiagnosis", DbType.String, objCaseReferralVO.ProvisionalDiagnosis);
                dbServer.AddInParameter(command, "ChiefComplaints", DbType.String, objCaseReferralVO.ChiefComplaints);
                dbServer.AddInParameter(command, "Summary", DbType.String, objCaseReferralVO.Summary);
                dbServer.AddInParameter(command, "Observations", DbType.String, objCaseReferralVO.Observations);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objCaseReferralVO.UnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objCaseReferralVO.Status);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

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

        public override IValueObject AddDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDoctorSuggestedServiceDetailBizActionVO BizActionObj = valueObject as clsAddDoctorSuggestedServiceDetailBizActionVO;

            try
            {
                List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceVO = BizActionObj.DoctorSuggestedServiceDetail;

                int count = objDoctorSuggestedServiceVO.Count;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDeatail");

                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDoctorSuggestedServiceVO[i].LinkServer);
                        if (objDoctorSuggestedServiceVO[i].LinkServer != null)
                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDoctorSuggestedServiceVO[i].LinkServer.Replace(@"\", "_"));
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PatientPrescriptionID);
                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, objDoctorSuggestedServiceVO[i].ServiceID);
                        dbServer.AddInParameter(command1, "ServiceName", DbType.String, objDoctorSuggestedServiceVO[i].ServiceName);
                        dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objDoctorSuggestedServiceVO[i].IsOther);
                        dbServer.AddInParameter(command1, "Reason", DbType.String, objDoctorSuggestedServiceVO[i].Reason);

                        //dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objPatientPrescriptionVO.UnitID);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDoctorSuggestedServiceVO[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.DoctorSuggestedServiceDetail[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }

                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {

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

        public override IValueObject UpdateDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateDoctorSuggestedServiceBizActionVO BizActionObj = valueObject as clsUpdateDoctorSuggestedServiceBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorSuggestedServiceDeatail");

                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceVO = BizActionObj.DoctorSuggestedServiceDetail;

                int count = objDoctorSuggestedServiceVO.Count;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDeatail");

                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDoctorSuggestedServiceVO[i].LinkServer);
                        if (objDoctorSuggestedServiceVO[i].LinkServer != null)
                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDoctorSuggestedServiceVO[i].LinkServer.Replace(@"\", "_"));
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, objDoctorSuggestedServiceVO[i].ServiceID);
                        dbServer.AddInParameter(command1, "ServiceName", DbType.String, objDoctorSuggestedServiceVO[i].ServiceName);
                        dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objDoctorSuggestedServiceVO[i].IsOther);
                        dbServer.AddInParameter(command1, "Reason", DbType.String, objDoctorSuggestedServiceVO[i].Reason);

                        //dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objPatientPrescriptionVO.UnitID);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDoctorSuggestedServiceVO[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.DoctorSuggestedServiceDetail[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }

                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {

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

        public override IValueObject GetFrequencyList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRFrequencyBizActionVO BizActionObj = valueObject as clsGetEMRFrequencyBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFrequencyList");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.FrequencyList == null)
                        BizActionObj.FrequencyList = new List<FrequencyMaster>();

                    while (reader.Read())
                    {

                        FrequencyMaster Freq = new FrequencyMaster();
                        Freq.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Freq.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        Freq.Quantity = (double)DALHelper.HandleDBNull(reader["QuntityPerDay"]);
                        Freq.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.FrequencyList.Add(Freq);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddBPControlDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBPControlBizActionVO BizAction = valueObject as clsAddPatientBPControlBizActionVO;
            clsBPControlVO objControl = BizAction.BPControlDetails;
            try
            {
                if (BizAction.IsBPControl == true)
                {
                    if (BizAction.BPControlDetails != null)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateBPControletails");

                        dbServer.AddInParameter(command, "VisitID", DbType.Int64, objControl.VisitID);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, objControl.PatientID);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objControl.PatientUnitID);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objControl.TemplateID);
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objControl.DoctorID);
                        dbServer.AddInParameter(command, "PatientEMRDataID", DbType.Int64, objControl.PatientEMRDataID);
                        dbServer.AddInParameter(command, "BP1", DbType.Int32, objControl.BP1);
                        dbServer.AddInParameter(command, "BP2", DbType.Int32, objControl.BP2);

                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objControl.UnitID);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objControl.Status);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objControl.ID);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        BizAction.BPControlDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject AddVisionControlDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientVisionControlBizActionVO BizAction = valueObject as clsAddPatientVisionControlBizActionVO;
            clsVisionVO objControl = BizAction.VisionControlDetails;
            try
            {
                if (BizAction.IsVisionControl == true)
                {
                    if (BizAction.VisionControlDetails != null)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateVisionDetails");

                        dbServer.AddInParameter(command, "VisitID", DbType.Int64, objControl.VisitID);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, objControl.PatientID);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objControl.PatientUnitID);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objControl.TemplateID);
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objControl.DoctorID);
                        dbServer.AddInParameter(command, "PatientEMRDataID", DbType.Int64, objControl.PatientEMRDataID);
                        dbServer.AddInParameter(command, "SPH1", DbType.Double, objControl.SPH);
                        dbServer.AddInParameter(command, "CYL1", DbType.Double, objControl.CYL);
                        dbServer.AddInParameter(command, "Axis1", DbType.Double, objControl.Axis);

                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objControl.UnitID);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objControl.Status);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objControl.ID);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        BizAction.VisionControlDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject AddGPControlDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientGPControlBizActionVO BizAction = valueObject as clsAddPatientGPControlBizActionVO;
            clsGlassPowerVO objControl = BizAction.GPControlDetails;
            try
            {
                if (BizAction.IsGPControl == true)
                {
                    if (BizAction.GPControlDetails != null)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateGlassPower");

                        dbServer.AddInParameter(command, "VisitID", DbType.Int64, objControl.VisitID);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, objControl.PatientID);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objControl.PatientUnitID);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objControl.TemplateID);
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objControl.DoctorID);
                        dbServer.AddInParameter(command, "PatientEMRDataID", DbType.Int64, objControl.PatientEMRDataID);
                        dbServer.AddInParameter(command, "SPH1", DbType.Double, objControl.SPH1);
                        dbServer.AddInParameter(command, "CYL1", DbType.Double, objControl.CYL1);
                        dbServer.AddInParameter(command, "Axis1", DbType.Double, objControl.Axis1);
                        dbServer.AddInParameter(command, "VA1", DbType.Double, objControl.VA1);
                        dbServer.AddInParameter(command, "SPH2", DbType.Double, objControl.SPH2);
                        dbServer.AddInParameter(command, "CYL2", DbType.Double, objControl.CYL2);
                        dbServer.AddInParameter(command, "Axis2", DbType.Double, objControl.Axis2);
                        dbServer.AddInParameter(command, "VA2", DbType.Double, objControl.VA2);

                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objControl.UnitID);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objControl.Status);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objControl.ID);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        BizAction.GPControlDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetPatientVital(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientVitalBizActionVO BizActionObj = valueObject as clsGetPatientVitalBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientVital");


                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.VitalDetails == null)
                        BizActionObj.VitalDetails = new List<clsEMRVitalsVO>();

                    while (reader.Read())
                    {
                        clsEMRVitalsVO Obj = new clsEMRVitalsVO();
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["VitalID"]);
                        Obj.PatientVitalID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Obj.Description = (string)DALHelper.HandleDBNull(reader["Vital"]);
                        Obj.Unit = (string)DALHelper.HandleDBNull(reader["Unit"]);
                        Obj.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDBNull(reader["Time"]);
                        Obj.Value = (double)DALHelper.HandleDBNull(reader["Value"]);
                        BizActionObj.VitalDetails.Add(Obj);
                    }

                }

                reader.Close();

            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return valueObject;
        }

        public override IValueObject GetItemMoleculeNameList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetItemMoleculeNameBizActionVO objBizAction = valueObject as clsGetItemMoleculeNameBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                if (objBizAction.isOtherDrug == true)
                {
                    command = dbServer.GetStoredProcCommand("[CIMS_GetNonAvailableStockItemsWithMoleculeName]");
                    dbServer.AddInParameter(command, "StoreID", DbType.String, objBizAction.StoreID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);
                }
                else
                    command = dbServer.GetStoredProcCommand("CIMS_GetAllItemsWithMoleculeName");
                dbServer.AddInParameter(command, "ItemsName", DbType.String, objBizAction.ItemName);
                dbServer.AddInParameter(command, "MoleculeID", DbType.Int64, objBizAction.MoleculeID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objBizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    clsItemMoleculeDetails objItemVO;
                    if (objBizAction.ItemMoleculeDetailsList == null)
                        objBizAction.ItemMoleculeDetailsList = new List<clsItemMoleculeDetails>();
                    while (reader.Read())
                    {
                        objItemVO = new clsItemMoleculeDetails();
                        objItemVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemVO.MoleculeName = Convert.ToString(DALHelper.HandleDBNull(reader["Molecule"]));
                        objItemVO.RouteID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RouteID"]));
                        objItemVO.Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]));
                        objBizAction.ItemMoleculeDetailsList.Add(objItemVO);
                    }

                }
                reader.NextResult();
                objBizAction.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return objBizAction;
        }
    }
}
