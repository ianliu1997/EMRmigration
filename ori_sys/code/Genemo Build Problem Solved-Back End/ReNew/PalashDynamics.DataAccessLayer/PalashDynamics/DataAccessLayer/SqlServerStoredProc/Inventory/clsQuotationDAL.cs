namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Quotation;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Inventory.Quotation;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsQuotationDAL : clsBaseQuotationDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsQuotationDAL()
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

        public override IValueObject AddQuotation(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddQuotationBizActionVO nvo = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                transaction = null;
                connection.Open();
                transaction = connection.BeginTransaction();
                nvo = valueObject as clsAddQuotationBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddQuotation");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.Quotation.UnitId);
                this.dbServer.AddParameter(storedProcCommand, "QuotationNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.Quotation.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.Quotation.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Quotation.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Quotation.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EnquiryID", DbType.Int64, nvo.Quotation.EnquiryID);
                this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, nvo.Quotation.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, nvo.Quotation.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Other", DbType.Double, nvo.Quotation.Other);
                this.dbServer.AddInParameter(storedProcCommand, "TotalConcession", DbType.Int64, nvo.Quotation.TotalConcession);
                this.dbServer.AddInParameter(storedProcCommand, "TotalNetAmount", DbType.Double, nvo.Quotation.TotalNet);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Quotation.Status);
                this.dbServer.AddInParameter(storedProcCommand, "LeadTime", DbType.Int64, nvo.Quotation.LeadTime);
                this.dbServer.AddInParameter(storedProcCommand, "ValidityDate", DbType.DateTime, nvo.Quotation.ValidityDate);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, nvo.Quotation.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, nvo.Quotation.CreatedUnitId);
                if (nvo.Quotation.AddedOn != null)
                {
                    nvo.Quotation.AddedOn = nvo.Quotation.AddedOn.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, nvo.Quotation.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.Quotation.AddedDateTime);
                if (nvo.Quotation.AddedWindowsLoginName != null)
                {
                    nvo.Quotation.AddedWindowsLoginName = nvo.Quotation.AddedWindowsLoginName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, nvo.Quotation.AddedWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Quotation.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.Quotation.QuotationNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "QuotationNo");
                foreach (clsQuotationDetailsVO svo in nvo.Quotation.Items)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddQuotationItems");
                    command2.Connection = connection;
                    command2.Parameters.Clear();
                    svo.QuotationID = nvo.Quotation.ID;
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.Quotation.UnitId);
                    this.dbServer.AddInParameter(command2, "QuotationID", DbType.Int64, svo.QuotationID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                    this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, svo.Quantity);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.Rate);
                    this.dbServer.AddInParameter(command2, "Amount", DbType.Double, svo.Amount);
                    this.dbServer.AddInParameter(command2, "ExcisePercent", DbType.Double, svo.ExcisePercent);
                    this.dbServer.AddInParameter(command2, "ExciseAmount", DbType.Double, svo.ExciseAmount);
                    this.dbServer.AddInParameter(command2, "TaxPercent", DbType.Double, svo.TAXPercent);
                    this.dbServer.AddInParameter(command2, "TaxAmount", DbType.Double, svo.TAXAmount);
                    this.dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Double, svo.ConcessionPercent);
                    this.dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, svo.ConcessionAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(command2, "Remarks", DbType.String, svo.Specification);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    long parameterValue = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                foreach (clsItemsEnquiryTermConditionVO nvo2 in nvo.TermsAndConditions)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddQuotationTermsAndConditions");
                    command3.Connection = connection;
                    command3.Parameters.Clear();
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, nvo.Quotation.UnitId);
                    this.dbServer.AddInParameter(command3, "QuotationID", DbType.Int64, nvo.Quotation.ID);
                    this.dbServer.AddInParameter(command3, "TermsConditionID", DbType.Int64, nvo2.TermsConditionID);
                    this.dbServer.AddInParameter(command3, "Remarks", DbType.String, nvo2.Remarks);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0);
                    this.dbServer.AddOutParameter(command3, "ID", DbType.Int64, 0);
                    if (nvo2.IsCheckedStatus)
                    {
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        long parameterValue = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.Quotation = null;
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

        public override IValueObject AddQuotationAttachments(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddQuotationAttachmentBizActionVO nvo = valueObject as clsAddQuotationAttachmentBizActionVO;
            try
            {
                foreach (clsQuotationAttachmentsVO svo in nvo.AttachmentList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddQuotationAttachments");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "QuotationID", DbType.Int64, svo.QuotationID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.String, svo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "ValidityDate", DbType.DateTime, svo.ValidityDate);
                    this.dbServer.AddInParameter(storedProcCommand, "LeadTime", DbType.Int64, svo.LeadTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, svo.AttachedFileName);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, svo.AttachedFileContent);
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, svo.Description);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDelete", DbType.Boolean, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
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
            clsDeleteQuotationAttachmentBizActionVO nvo = valueObject as clsDeleteQuotationAttachmentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddQuotationAttachments");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "QuotationID", DbType.Int64, nvo.QuotationID);
                this.dbServer.AddInParameter(storedProcCommand, "IsDelete", DbType.Boolean, 1);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetQuotation(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsGetQuotationBizActionVO nvo = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                transaction = null;
                connection.Open();
                transaction = connection.BeginTransaction();
                nvo = valueObject as clsGetQuotationBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetQuotations");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.SearchStoreID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SearchSupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierIDs", DbType.String, nvo.SupplierIDs);
                this.dbServer.AddInParameter(storedProcCommand, "ItemIDs", DbType.String, nvo.ItemIDs);
                if (nvo.SearchFromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.SearchFromDate);
                }
                if (nvo.SearchToDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.SearchToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaxRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.QuotaionList == null)
                    {
                        nvo.QuotaionList = new List<clsQuotaionVO>();
                    }
                    while (reader.Read())
                    {
                        clsQuotaionVO item = new clsQuotaionVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            QuotationNo = Convert.ToString(DALHelper.HandleDBNull(reader["QuotationNO"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"])),
                            SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            ValidityDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ValidityDate"])),
                            QuotationFrom = Convert.ToString(DALHelper.HandleDBNull(reader["QuotationFrom"]))
                        };
                        nvo.QuotaionList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.QuotaionList = null;
            }
            return nvo;
        }

        public override IValueObject GetQuotationAttachments(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetQuotationAttachmentBizActionVO nvo = valueObject as clsGetQuotationAttachmentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetQuotationAttachments");
                this.dbServer.AddInParameter(storedProcCommand, "QuotationID", DbType.Int64, nvo.QuotationID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.AttachmentList = new List<clsQuotationAttachmentsVO>();
                    while (reader.Read())
                    {
                        clsQuotationAttachmentsVO item = new clsQuotationAttachmentsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            QuotationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["QuotationID"])),
                            LeadTime = Convert.ToInt64(DALHelper.HandleDBNull(reader["LeadTime"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ValidityDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ValidityDate"])),
                            AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"])),
                            AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"])
                        };
                        nvo.AttachmentList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetQuotationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsGetQuotationDetailsBizActionVO nvo = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                transaction = null;
                connection.Open();
                transaction = connection.BeginTransaction();
                nvo = valueObject as clsGetQuotationDetailsBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetQuotationDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SearchQuotationID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MinRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.QuotaionList == null)
                    {
                        nvo.QuotaionList = new List<clsQuotationDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsQuotationDetailsVO item = new clsQuotationDetailsVO {
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            TAXPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercent"])),
                            TAXAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXAmount"])),
                            ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"])),
                            ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"])),
                            NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            Specification = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"])),
                            PUM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ConPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"])),
                            TAXPer = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercent"])),
                            ExcisePercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ExcisePercent"])),
                            ExciseAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ExciseAmount"]))
                        };
                        nvo.QuotaionList.Add(item);
                    }
                }
                reader.Close();
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.QuotaionList = null;
            }
            return nvo;
        }
    }
}

