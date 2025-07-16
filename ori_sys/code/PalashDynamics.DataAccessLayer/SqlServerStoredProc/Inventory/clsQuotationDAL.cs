using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Quotation;
using PalashDynamics.ValueObjects.Inventory.Quotation;
namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    public class clsQuotationDAL : clsBaseQuotationDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion
        private clsQuotationDAL()
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

        public override IValueObject AddQuotation(IValueObject valueObject, clsUserVO UserVo)
        {

            DbConnection con = null;
            DbTransaction trans = null;
            clsAddQuotationBizActionVO BizActionobj = null;
            try
            {

                con = dbServer.CreateConnection();
                trans = null;

                con.Open();
                trans = con.BeginTransaction();

                BizActionobj = valueObject as clsAddQuotationBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddQuotation");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionobj.Quotation.UnitId);
                dbServer.AddParameter(command, "QuotationNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionobj.Quotation.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionobj.Quotation.SupplierID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionobj.Quotation.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionobj.Quotation.Time);
                dbServer.AddInParameter(command, "EnquiryID", DbType.Int64, BizActionobj.Quotation.EnquiryID);
                dbServer.AddInParameter(command, "Notes", DbType.String, BizActionobj.Quotation.Notes);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, BizActionobj.Quotation.TotalAmount);
                dbServer.AddInParameter(command, "Other", DbType.Double, BizActionobj.Quotation.Other);
                dbServer.AddInParameter(command, "TotalConcession", DbType.Int64, BizActionobj.Quotation.TotalConcession);
                dbServer.AddInParameter(command, "TotalNetAmount", DbType.Double, BizActionobj.Quotation.TotalNet);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionobj.Quotation.Status);
                dbServer.AddInParameter(command, "LeadTime", DbType.Int64, BizActionobj.Quotation.LeadTime);  //By Umesh
                dbServer.AddInParameter(command, "ValidityDate", DbType.DateTime, BizActionobj.Quotation.ValidityDate);  //By Umesh
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, BizActionobj.Quotation.AddedBy);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, BizActionobj.Quotation.CreatedUnitId);
                if (BizActionobj.Quotation.AddedOn != null) BizActionobj.Quotation.AddedOn = BizActionobj.Quotation.AddedOn.Trim();
                dbServer.AddInParameter(command, "AddedOn", DbType.String, BizActionobj.Quotation.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionobj.Quotation.AddedDateTime);
                if (BizActionobj.Quotation.AddedWindowsLoginName != null) BizActionobj.Quotation.AddedWindowsLoginName = BizActionobj.Quotation.AddedWindowsLoginName.Trim();
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, BizActionobj.Quotation.AddedWindowsLoginName);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionobj.Quotation.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionobj.Quotation.QuotationNo = (string)dbServer.GetParameterValue(command, "QuotationNo");


                foreach (var item in BizActionobj.Quotation.Items)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddQuotationItems");
                    command1.Connection = con;
                    command1.Parameters.Clear();
                    item.QuotationID = BizActionobj.Quotation.ID;
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionobj.Quotation.UnitId);
                    dbServer.AddInParameter(command1, "QuotationID", DbType.Int64, item.QuotationID);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);
                    dbServer.AddInParameter(command1, "Amount", DbType.Double, item.Amount);
                    dbServer.AddInParameter(command1, "ExcisePercent", DbType.Double, item.ExcisePercent);
                    dbServer.AddInParameter(command1, "ExciseAmount", DbType.Double, item.ExciseAmount);
                    dbServer.AddInParameter(command1, "TaxPercent", DbType.Double, item.TAXPercent);
                    dbServer.AddInParameter(command1, "TaxAmount", DbType.Double, item.TAXAmount);
                    dbServer.AddInParameter(command1, "ConcessionPercent", DbType.Double, item.ConcessionPercent);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
                    dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Specification);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, 0);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);


                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    long id = (long)dbServer.GetParameterValue(command, "ID");
                   // BizActionobj.Quotation.ID = (long)dbServer.GetParameterValue(command, "ID");


                }
                foreach (var item in BizActionobj.TermsAndConditions)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddQuotationTermsAndConditions");
                    command2.Connection = con;
                    command2.Parameters.Clear();

                  
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionobj.Quotation.UnitId);
                    dbServer.AddInParameter(command2, "QuotationID", DbType.Int64, BizActionobj.Quotation.ID);
                    dbServer.AddInParameter(command2, "TermsConditionID", DbType.Int64, item.TermsConditionID);
                    dbServer.AddInParameter(command2, "Remarks", DbType.String, item.Remarks);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0);
                    dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);

                    if (item.IsCheckedStatus == true)
                    {
                        int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                        BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        long id = (long)dbServer.GetParameterValue(command, "ID");
                    }
                    
                 


                }
                trans.Commit();

            }
            catch (Exception ex)
            {

                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionobj.SuccessStatus = -1;
                BizActionobj.Quotation = null;
            }
            finally
            {
                if(con.State == ConnectionState.Open)
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionobj;
        }

        public override IValueObject GetQuotation(IValueObject valueObject, clsUserVO UserVo)
        {
              DbConnection con = null;
            DbTransaction trans = null;
            clsGetQuotationBizActionVO BizActionobj = null;
            try
            {
                  con = dbServer.CreateConnection();
                trans = null;
                con.Open();
                trans = con.BeginTransaction();
                BizActionobj = valueObject as clsGetQuotationBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetQuotations");
                DbDataReader reader;
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionobj.SearchStoreID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionobj.UserID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionobj.SearchSupplierID);
                dbServer.AddInParameter(command, "SupplierIDs", DbType.String, BizActionobj.SupplierIDs);
                dbServer.AddInParameter(command, "ItemIDs", DbType.String, BizActionobj.ItemIDs);
                if (BizActionobj.SearchFromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionobj.SearchFromDate);
                if (BizActionobj.SearchToDate != null)
                {
                    //if (BizActionobj.SearchFromDate != null)
                    //{
                    //    if (BizActionobj.SearchFromDate.Equals(BizActionobj.SearchToDate))
                    //        BizActionobj.SearchToDate = BizActionobj.SearchToDate.Date.AddDays(1);
                    //}
                    //if (BizActionobj.SearchFromDate == BizActionobj.SearchToDate)
                    //{
                    //    BizActionobj.SearchToDate = BizActionobj.SearchToDate.Date.AddDays(1);
                    //}
                    //else
                    //    BizActionobj.SearchToDate = BizActionobj.SearchToDate.Date.AddDays(1);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionobj.SearchToDate);

                }
               
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionobj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionobj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionobj.MaxRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionobj.QuotaionList == null)
                        BizActionobj.QuotaionList = new List<clsQuotaionVO>();

                    while (reader.Read())
                    {
                        clsQuotaionVO objVO = new clsQuotaionVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.QuotationNo = Convert.ToString(DALHelper.HandleDBNull(reader["QuotationNO"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]));
                        objVO.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.ValidityDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ValidityDate"]));
                        objVO.QuotationFrom = Convert.ToString(DALHelper.HandleDBNull(reader["QuotationFrom"]));
                        BizActionobj.QuotaionList.Add(objVO);
                    }
                }
                reader.NextResult();
                 BizActionobj.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
                trans.Commit();
            }
            catch (Exception ex)
            {

                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionobj.SuccessStatus = -1;
                BizActionobj.QuotaionList = null;
            }
            return BizActionobj;

        }

        public override IValueObject GetQuotationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsGetQuotationDetailsBizActionVO BizActionobj = null;
            try
            {
                con = dbServer.CreateConnection();
                trans = null;

                con.Open();
                trans = con.BeginTransaction();

                BizActionobj = valueObject as clsGetQuotationDetailsBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetQuotationDetails");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionobj.SearchQuotationID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionobj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionobj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionobj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionobj.MinRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionobj.QuotaionList == null)
                        BizActionobj.QuotaionList = new List<clsQuotationDetailsVO>();

                    while (reader.Read())
                    {
                        //clsQuotaionVO objVO = new clsQuotaionVO();
                        clsQuotationDetailsVO objVO = new clsQuotationDetailsVO();

                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.TAXPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercent"]));
                        objVO.TAXAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXAmount"]));  
                        objVO.ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        objVO.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objVO.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        objVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objVO.Specification = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        objVO.PUM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.ConPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        objVO.TAXPer = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercent"]));
                        objVO.ExcisePercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ExcisePercent"]));
                        objVO.ExciseAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ExciseAmount"]));



                        BizActionobj.QuotaionList.Add(objVO);

                    }


                }
                //reader.NextResult();
                // BizActionObj.OpeningBalance.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");


                reader.Close();
                trans.Commit();


            }
            catch (Exception ex)
            {

                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionobj.SuccessStatus = -1;
                BizActionobj.QuotaionList = null;
            }
            return BizActionobj;

        }

        public override IValueObject AddQuotationAttachments(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsAddQuotationAttachmentBizActionVO BizActionObj = valueObject as clsAddQuotationAttachmentBizActionVO;
            try
            {
                foreach (clsQuotationAttachmentsVO item in BizActionObj.AttachmentList)
                {
                    DbCommand commandscandocMale = dbServer.GetStoredProcCommand("CIMS_AddQuotationAttachments");
                    dbServer.AddInParameter(commandscandocMale, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(commandscandocMale, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandscandocMale, "QuotationID", DbType.Int64, item.QuotationID);
                    dbServer.AddInParameter(commandscandocMale, "Status", DbType.String, item.Status);
                    dbServer.AddInParameter(commandscandocMale, "ValidityDate", DbType.DateTime, item.ValidityDate);
                    dbServer.AddInParameter(commandscandocMale, "LeadTime", DbType.Int64, item.LeadTime);
                    dbServer.AddInParameter(commandscandocMale, "AttachedFileName", DbType.String, item.AttachedFileName);
                    dbServer.AddInParameter(commandscandocMale, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                    dbServer.AddInParameter(commandscandocMale, "Description", DbType.String, item.Description);
                //    dbServer.AddInParameter(commandscandocMale, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(commandscandocMale, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(commandscandocMale, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
               //     dbServer.AddInParameter(commandscandocMale, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(commandscandocMale, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(commandscandocMale, "IsDelete", DbType.Boolean, 0);
                    int intscandocMale = dbServer.ExecuteNonQuery(commandscandocMale);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
            //..........................................................
        }

        public override IValueObject GetQuotationAttachments(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsGetQuotationAttachmentBizActionVO BizActionObj = valueObject as clsGetQuotationAttachmentBizActionVO;
            try
            {

                DbCommand commandScanDoc = dbServer.GetStoredProcCommand("CIMS_GetQuotationAttachments");
                dbServer.AddInParameter(commandScanDoc, "QuotationID", DbType.Int64, BizActionObj.QuotationID);
            //    dbServer.AddInParameter(commandScanDoc, "PatientUnitID", DbType.Int64, BizActionObj.PatientScanDoc.PatientUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(commandScanDoc);
                if (reader.HasRows)
                {
                    BizActionObj.AttachmentList = new List<clsQuotationAttachmentsVO>();
                    while (reader.Read())
                    {
                        clsQuotationAttachmentsVO AttachmentDetail = new clsQuotationAttachmentsVO();
                        AttachmentDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                      //  AttachmentDetail.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        AttachmentDetail.QuotationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["QuotationID"]));
                        AttachmentDetail.LeadTime = Convert.ToInt64(DALHelper.HandleDBNull(reader["LeadTime"]));
                        //AttachmentDetail.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                        AttachmentDetail.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        AttachmentDetail.ValidityDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ValidityDate"]));
                        AttachmentDetail.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        AttachmentDetail.AttachedFileContent = (byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        BizActionObj.AttachmentList.Add(AttachmentDetail);

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;

        }

        public override IValueObject DeleteQuotationAttachments(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteQuotationAttachmentBizActionVO BizActionObj = valueObject as clsDeleteQuotationAttachmentBizActionVO;
            try
            {
              //  clsPatientSignConsentVO objSignResutl = BizActionObj.DeleteVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddQuotationAttachments");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "QuotationID", DbType.Int64, BizActionObj.QuotationID);
                dbServer.AddInParameter(command, "IsDelete", DbType.Boolean, 1);
                int Status = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valueObject;
        }
    }
}
