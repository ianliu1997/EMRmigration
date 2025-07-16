namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Radiology;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsRadiologyDAL : clsBaseRadiologyDAL
    {
        private Database dbServer;
        private LogManager logManager;

        public clsRadiologyDAL()
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

        private clsAddRadOrderBookingBizActionVO AddOrder(clsAddRadOrderBookingBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (pConnection == null) ? this.dbServer.CreateConnection() : pConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (pTransaction == null) ? connection.BeginTransaction() : pTransaction;
                clsRadOrderBookingVO bookingDetails = BizActionObj.BookingDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadOrderBooking");
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.String, bookingDetails.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, bookingDetails.BillNo);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, bookingDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, bookingDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_ID", DbType.Int64, bookingDetails.Opd_Ipd_External_ID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitID", DbType.Int64, bookingDetails.Opd_Ipd_External_UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, bookingDetails.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, bookingDetails.IsCancelled);
                this.dbServer.AddInParameter(storedProcCommand, "TestType", DbType.Int64, bookingDetails.TestType);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, bookingDetails.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry", DbType.Boolean, bookingDetails.IsResultEntry);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, bookingDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "OrderNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bookingDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.BookingDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.BookingDetails.OrderNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "OrderNo");
                if ((bookingDetails.OrderBookingDetails != null) && (bookingDetails.OrderBookingDetails.Count > 0))
                {
                    foreach (clsRadOrderBookingDetailsVO svo in bookingDetails.OrderBookingDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddRadOrderBookingDetails");
                        this.dbServer.AddInParameter(command2, "RadOrderID", DbType.Int64, bookingDetails.ID);
                        this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, svo.TestID);
                        this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, svo.ChargeID);
                        this.dbServer.AddInParameter(command2, "TariffServiceId", DbType.Int64, svo.TariffServiceID);
                        if (svo.Number != null)
                        {
                            this.dbServer.AddInParameter(command2, "Number", DbType.String, svo.Number.Trim());
                        }
                        this.dbServer.AddInParameter(command2, "IsEmergency", DbType.Boolean, svo.IsEmergency);
                        this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, svo.DoctorID);
                        this.dbServer.AddInParameter(command2, "IsApproved", DbType.Boolean, svo.IsApproved);
                        this.dbServer.AddInParameter(command2, "IsCompleted", DbType.Boolean, svo.IsCompleted);
                        this.dbServer.AddInParameter(command2, "IsDelivered", DbType.Boolean, svo.IsDelivered);
                        this.dbServer.AddInParameter(command2, "IsPrinted", DbType.Boolean, svo.IsPrinted);
                        this.dbServer.AddInParameter(command2, "MicrobiologistId", DbType.Int64, svo.MicrobiologistID);
                        if (svo.ReferredBy != null)
                        {
                            this.dbServer.AddInParameter(command2, "ReferredBy", DbType.String, svo.ReferredBy.Trim());
                        }
                        this.dbServer.AddInParameter(command2, "IsResultEntry", DbType.Boolean, svo.IsResultEntry);
                        this.dbServer.AddInParameter(command2, "IsFinalized", DbType.Boolean, svo.IsFinalized);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, bookingDetails.Status);
                        this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        svo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
                BizActionObj.SuccessStatus = 0;
                if (pConnection == null)
                {
                    transaction.Commit();
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (pConnection == null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRadOrderBookingBizActionVO bizActionObj = valueObject as clsAddRadOrderBookingBizActionVO;
            bizActionObj = (bizActionObj.BookingDetails.ID != 0L) ? this.UpdateOrder(bizActionObj, UserVo, null, null) : this.AddOrder(bizActionObj, UserVo, null, null);
            return valueObject;
        }

        public override IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddRadOrderBookingBizActionVO bizActionObj = valueObject as clsAddRadOrderBookingBizActionVO;
            bizActionObj = (bizActionObj.BookingDetails.ID != 0L) ? this.UpdateOrder(bizActionObj, UserVo, null, null) : this.AddOrder(bizActionObj, UserVo, pConnection, pTransaction);
            return valueObject;
        }

        public override IValueObject AddRadDetilsForEmail(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsRadPathPatientReportDetailsForEmailSendingBizActionVO nvo = valueObject as clsRadPathPatientReportDetailsForEmailSendingBizActionVO;
            if (nvo.RadOrderBookingDetailList.Count > 0)
            {
                try
                {
                    connection = this.dbServer.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    DbCommand storedProcCommand = null;
                    foreach (clsRadOrderBookingDetailsVO svo in nvo.RadOrderBookingDetailList)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadReportEmailDetailsforSendingEmail");
                        storedProcCommand.Connection = connection;
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, svo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, svo.RadOrderID);
                        this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, svo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "PathPatientReportID", DbType.Int64, svo.ResultEntryID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientEmailID", DbType.String, nvo.PatientEmailID);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorEmailID", DbType.String, nvo.DoctorEmailID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Int64, false);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientName", DbType.String, nvo.PatientName);
                        this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, svo.Report);
                        this.dbServer.AddInParameter(storedProcCommand, "Body", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "Subject", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, svo.SourceURL);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                        this.dbServer.AddParameter(storedProcCommand, "AttachmentID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.AttachmentID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.AttachmentID = (long) this.dbServer.GetParameterValue(storedProcCommand, "AttachmentID");
                    }
                    transaction.Commit();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    nvo.RadOrderBookingDetailList = null;
                    throw exception;
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
            }
            return nvo;
        }

        public override IValueObject AddRadiologistToTempList(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddRadiologistToTempbizActionVO nvo = valueObject as clsAddRadiologistToTempbizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsRadiologyVO itemSupplier = nvo.ItemSupplier;
                int num = 0;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadiologistListForTest");
                if (nvo.ItemSupplierList.Count > 0)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 > (nvo.ItemSupplierList.Count - 1))
                        {
                            nvo.SuccessStatus = num;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemSupplier.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, itemSupplier.TemplateID);
                        this.dbServer.AddInParameter(storedProcCommand, "RadiologistID", DbType.Int64, nvo.ItemSupplierList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ItemSupplierList[num2].Status);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        int num3 = this.dbServer.ExecuteNonQuery(storedProcCommand);
                        if (num3 > 0)
                        {
                            num = 1;
                        }
                        num2++;
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        private clsAddRadResultEntryBizActionVO AddResult(clsAddRadResultEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsRadResultEntryVO resultDetails = BizActionObj.ResultDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadResultEntryMilan");
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderID", DbType.Int64, resultDetails.RadOrderID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderDetailID", DbType.Int64, resultDetails.BookingDetailsID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, resultDetails.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "FlimId", DbType.Int64, resultDetails.FilmID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID1", DbType.Int64, resultDetails.RadiologistID1);
                this.dbServer.AddInParameter(storedProcCommand, "FirstLevelDescription", DbType.String, resultDetails.FirstLevelDescription);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateResultID", DbType.Int64, resultDetails.TemplateResultID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID2", DbType.Int64, resultDetails.RadiologistID2);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID3", DbType.Int64, resultDetails.RadiologistID3);
                this.dbServer.AddInParameter(storedProcCommand, "SecondLevelDescription", DbType.String, resultDetails.SecondLevelDescription);
                this.dbServer.AddInParameter(storedProcCommand, "ThirdLevelDescription", DbType.String, resultDetails.ThirdLevelDescription);
                this.dbServer.AddInParameter(storedProcCommand, "FirstLevelId", DbType.Int64, resultDetails.FirstLevelId);
                this.dbServer.AddInParameter(storedProcCommand, "SecondLevelId", DbType.Int64, resultDetails.SecondLevelId);
                this.dbServer.AddInParameter(storedProcCommand, "ThirdLevelId", DbType.Int64, resultDetails.ThirdLevelId);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateId", DbType.Int64, resultDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, resultDetails.IsOutSourced);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyId", DbType.Int64, resultDetails.AgencyId);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, resultDetails.ReferredBy.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "IsFinalized", DbType.Boolean, resultDetails.IsFinalized);
                this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry", DbType.Boolean, resultDetails.IsResultEntry);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, resultDetails.SourceURL);
                this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, resultDetails.Report);
                this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, resultDetails.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, resultDetails.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, resultDetails.Time);
                this.dbServer.AddInParameter(storedProcCommand, "IsCompleted", DbType.Boolean, resultDetails.IsCompleted);
                this.dbServer.AddInParameter(storedProcCommand, "IsUpload", DbType.Boolean, BizActionObj.IsUpload);
                if (resultDetails.IsRefDoctorSigniture)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsDigitalSignatureRequired", DbType.Boolean, resultDetails.IsRefDoctorSigniture);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorUserID", DbType.Int64, resultDetails.RadiologistID1);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsDigitalSignatureRequired", DbType.Boolean, null);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorUserID", DbType.Int64, null);
                }
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, resultDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.ResultDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.ResultDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRadResultEntryBizActionVO bizActionObj = valueObject as clsAddRadResultEntryBizActionVO;
            bizActionObj = (bizActionObj.ResultDetails.ID <= 0L) ? this.AddResult(bizActionObj, UserVo) : this.UpdateResult(bizActionObj, UserVo);
            return valueObject;
        }

        private clsUpdateRadTechnicianEntryBizActionVO AddTechnicianEntry(clsUpdateRadTechnicianEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection myConnection = null;
            try
            {
                myConnection = this.dbServer.CreateConnection();
                if (myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                transaction = myConnection.BeginTransaction();
                clsRadResultEntryVO resultDetails = BizActionObj.ResultDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadTechnicianEntry");
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderID", DbType.Int64, resultDetails.RadOrderID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderDetailID", DbType.Int64, resultDetails.BookingDetailsID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, resultDetails.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "FlimId", DbType.Int64, resultDetails.FilmID);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfFlims", DbType.Int64, resultDetails.NoOfFilms);
                this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, resultDetails.IsOutSourced);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyId", DbType.Int64, resultDetails.AgencyId);
                this.dbServer.AddInParameter(storedProcCommand, "Contrast", DbType.Boolean, resultDetails.Contrast);
                this.dbServer.AddInParameter(storedProcCommand, "Sedation", DbType.Boolean, resultDetails.Sedation);
                this.dbServer.AddInParameter(storedProcCommand, "ContrastDetails", DbType.String, resultDetails.ContrastDetails);
                this.dbServer.AddInParameter(storedProcCommand, "FilmWastage", DbType.Boolean, resultDetails.FilmWastage);
                this.dbServer.AddInParameter(storedProcCommand, "FilmWastageDetails", DbType.String, resultDetails.FilmWastageDetails);
                if (resultDetails.ReferredBy != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, resultDetails.ReferredBy.Trim());
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, resultDetails.ReferredBy);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntry", DbType.Boolean, resultDetails.IsTechnicianEntry);
                this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntryFinalized", DbType.Boolean, resultDetails.IsTechnicianEntryFinalized);
                this.dbServer.AddInParameter(storedProcCommand, "TechnicianUserID", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistUserID", DbType.Int64, null);
                this.dbServer.AddInParameter(storedProcCommand, "TechnicianEntryDate", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, resultDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, resultDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.ResultDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((resultDetails.TestItemList != null) && (resultDetails.TestItemList.Count > 0))
                {
                    foreach (clsRadItemDetailMasterVO rvo in resultDetails.TestItemList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddRadResultItemDetails");
                        this.dbServer.AddInParameter(command2, "RadOrderID", DbType.Int64, resultDetails.RadOrderID);
                        this.dbServer.AddInParameter(command2, "RadOrderDetailID", DbType.Int64, resultDetails.BookingDetailsID);
                        this.dbServer.AddInParameter(command2, "RadTechnicianEntryID", DbType.Int64, resultDetails.ID);
                        this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, resultDetails.TestID);
                        this.dbServer.AddInParameter(command2, "StoreID", DbType.Int64, rvo.StoreID);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, rvo.ItemID);
                        this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, rvo.BatchID);
                        this.dbServer.AddInParameter(command2, "IdealQuantity", DbType.Double, rvo.Quantity);
                        this.dbServer.AddInParameter(command2, "ActualQantity", DbType.Double, rvo.ActualQantity);
                        this.dbServer.AddInParameter(command2, "BalQantity", DbType.Double, rvo.BalanceQuantity);
                        this.dbServer.AddInParameter(command2, "WastageQuantity", DbType.Double, rvo.WastageQantity);
                        this.dbServer.AddInParameter(command2, "ItemCategoryID", DbType.Int64, rvo.ItemCategoryID);
                        this.dbServer.AddInParameter(command2, "ExpiryDate ", DbType.DateTime, rvo.ExpiryDate);
                        this.dbServer.AddInParameter(command2, "Remarks", DbType.String, rvo.Remarks);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        rvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        rvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if (resultDetails.IsTechnicianEntryFinalized && (BizActionObj.AutoDeductStock && BizActionObj.ItemCusmption))
                        {
                            try
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddMaterialConsumption");
                                this.dbServer.AddInParameter(command3, "BillID", DbType.Int64, resultDetails.BillID);
                                this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, rvo.StoreID);
                                this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command3, "TotalAmount", DbType.Double, null);
                                this.dbServer.AddInParameter(command3, "Remark", DbType.String, "Radiographor Entry Item Consumption");
                                this.dbServer.AddInParameter(command3, "TotalItems", DbType.Decimal, resultDetails.TestItemList.Count);
                                this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now.Date);
                                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.AddOutParameter(command3, "ID5", DbType.Int64, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                BizActionObj.ConsumptionID = (long) this.dbServer.GetParameterValue(command3, "ID5");
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddMaerialConsumptionDetails");
                                command4.Connection = myConnection;
                                this.dbServer.AddInParameter(command4, "ID", DbType.Int64, 0);
                                this.dbServer.AddInParameter(command4, "ConsumptionID", DbType.Int64, BizActionObj.ConsumptionID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, rvo.BatchID);
                                this.dbServer.AddInParameter(command4, "ItemId", DbType.Int64, rvo.ItemID);
                                this.dbServer.AddInParameter(command4, "UsedQty", DbType.Decimal, rvo.WastageQantity + rvo.ActualQantity);
                                this.dbServer.AddInParameter(command4, "Rate", DbType.Decimal, null);
                                this.dbServer.AddInParameter(command4, "Amount", DbType.Decimal, null);
                                this.dbServer.AddInParameter(command4, "Remark", DbType.String, "Radiographor Entry Item Consumption");
                                this.dbServer.AddInParameter(command4, "BatchCode", DbType.String, rvo.BatchCode);
                                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.Date, rvo.ExpiryDate);
                                this.dbServer.AddInParameter(command4, "ItemName", DbType.String, rvo.ItemName);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                            }
                            catch
                            {
                            }
                            clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                            clsAddItemStockBizActionVO valueObject = new clsAddItemStockBizActionVO();
                            clsItemStockVO kvo = new clsItemStockVO {
                                ItemID = rvo.ItemID,
                                BatchID = rvo.BatchID,
                                BatchCode = rvo.BatchCode,
                                TransactionTypeID = InventoryTransactionType.RadiologyTestConsumption,
                                TransactionQuantity = rvo.ActualQantity + rvo.WastageQantity,
                                TransactionID = BizActionObj.ResultDetails.ID,
                                Date = DateTime.Now,
                                Time = DateTime.Now,
                                OperationType = InventoryStockOperationType.Subtraction,
                                StoreID = new long?(rvo.StoreID)
                            };
                            valueObject.Details = kvo;
                            valueObject.Details.ID = 0L;
                            valueObject = (clsAddItemStockBizActionVO) instance.Add(valueObject, UserVo, myConnection, transaction);
                            if (valueObject.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            kvo.ID = valueObject.Details.ID;
                        }
                    }
                }
                transaction.Commit();
            }
            catch
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.ResultDetails = null;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRadTemplateMasterBizActionVO bizActionObj = valueObject as clsAddRadTemplateMasterBizActionVO;
            bizActionObj = (bizActionObj.TemplateDetails.TemplateID != 0L) ? this.UpdateTemplateDetails(bizActionObj, UserVo) : this.AddTemplateDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddRadTemplateMasterBizActionVO AddTemplateDetails(clsAddRadTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsRadiologyVO templateDetails = BizActionObj.TemplateDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadTemplateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, templateDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, templateDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, templateDetails.TemplateDesign);
                this.dbServer.AddInParameter(storedProcCommand, "Radiologist", DbType.Int64, templateDetails.Radiologist);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, templateDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateResultID", DbType.Int64, templateDetails.TemplateResultID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, templateDetails.TemplateID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.TemplateDetails.TemplateID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                List<MasterListItem> genderList = BizActionObj.TemplateDetails.GenderList;
                if ((genderList != null) || (genderList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteRadioGenderToTemplate");
                    this.dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.TemplateID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (MasterListItem item in genderList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddRadioGenderToTemplate");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "GenderID", DbType.Int64, item.ID);
                        this.dbServer.AddInParameter(command3, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.TemplateID);
                        this.dbServer.AddInParameter(command3, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, item.Status);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        this.dbServer.ExecuteNonQuery(command3);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        private clsAddRadTestMasterBizActionVO AddTest(clsAddRadTestMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsRadiologyVO testDetails = BizActionObj.TestDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadTestMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, testDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, testDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, testDetails.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, testDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "TurnAroundTime", DbType.String, testDetails.TurnAroundTime);
                this.dbServer.AddInParameter(storedProcCommand, "PrintTestName", DbType.String, testDetails.PrintTestName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, testDetails.TestID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.TestDetails.TestID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsRadTemplateDetailMasterVO rvo in testDetails.TestTemplateList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddRadTestTemplateDetailMaster");
                    this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, testDetails.TestID);
                    this.dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, rvo.TestTemplateID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, rvo.Status);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    rvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                foreach (clsRadItemDetailMasterVO rvo2 in testDetails.TestItemList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddRadTestItemDetailMaster");
                    this.dbServer.AddInParameter(command3, "TestID", DbType.Int64, testDetails.TestID);
                    this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, rvo2.ItemID);
                    this.dbServer.AddInParameter(command3, "Quantity", DbType.Double, rvo2.Quantity);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, rvo2.Status);
                    this.dbServer.AddInParameter(command3, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo2.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    rvo2.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.TestDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddTestMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRadTestMasterBizActionVO bizActionObj = valueObject as clsAddRadTestMasterBizActionVO;
            bizActionObj = (bizActionObj.TestDetails.TestID != 0L) ? this.UpdateTest(bizActionObj, UserVo) : this.AddTest(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject FillTemplateComboBox(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillTemplateComboBoxForResultEntryBizActionVO nvo = (clsFillTemplateComboBoxForResultEntryBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillTemplateComboBoxInResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "Radiologist", DbType.Int64, nvo.Radiologist);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateResultID", DbType.Int64, nvo.TemplateResultID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject FillTestComboBox(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillTestComboBoxBizActionVO nvo = (clsFillTestComboBoxBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillTestComboBoxInResultEntry");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadItemBizActionVO nvo = (clsGetRadItemBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillRadItemComboBox");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOrderDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOrderBookingDetailsListBizActionVO nvo = valueObject as clsGetOrderBookingDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadOrderBookingDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, nvo.ModalityID);
                this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "DeliveryStatus", DbType.Boolean, nvo.DeliveryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "CheckResultEntryStatus", DbType.Boolean, nvo.CheckResultEntryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "ResultEntryStatus", DbType.Boolean, nvo.ResultEntryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntry", DbType.Boolean, nvo.IsTechnicianEntry);
                this.dbServer.AddInParameter(storedProcCommand, "IsFinalized", DbType.Boolean, nvo.IsFinalizedByDr);
                this.dbServer.AddInParameter(storedProcCommand, "NotDone", DbType.Boolean, nvo.NotDone);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID", DbType.Int64, nvo.RadiologistID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BookingDetails == null)
                    {
                        nvo.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadOrderBookingDetailsVO item = new clsRadOrderBookingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"])),
                            RadOrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderDetailID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                            TestCode = Convert.ToString(DALHelper.HandleDBNull(reader["TestCode"])),
                            TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Service"])),
                            LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"])),
                            IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"])),
                            IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"])),
                            IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"])),
                            SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"])),
                            Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]),
                            ReportUploadPath = Convert.ToString(DALHelper.HandleDBNull(reader["ReportPath"])),
                            IsCompleted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompleted"])),
                            IsUpload = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUpload"])),
                            ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModalityId"])),
                            Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"])),
                            ResultEntryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultEntryID"])),
                            IsTechnicianEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntry"])),
                            IsTechnicianEntryFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntryFinalized"])),
                            RadiologistEntryDate = (DateTime?) DALHelper.HandleDBNull(reader["RadiologistEntryDate"]),
                            PatientFullName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["PrescribingDr"])),
                            RadiologistID1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologistID1"])),
                            TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])),
                            Radiologist = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"])),
                            FinalizedByDoctorTime = (DateTime?) DALHelper.HandleDBNull(reader["FinalizedByDoctorTime"]),
                            ReportUploadDateTime = (DateTime?) DALHelper.HandleDBNull(reader["ReportUploadDateTime"]),
                            DeliveryReportDateTime = (DateTime?) DALHelper.HandleDBNull(reader["DeliveryReportDateTime"]),
                            IsCancelled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCancelled"])),
                            CancelledDateTime = (DateTime?) DALHelper.HandleDBNull(reader["CancelledDate"]),
                            Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]))
                        };
                        nvo.BookingDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOrderDetailsForWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOrderBookingDetailsListForWorkOrderBizActionVO nvo = valueObject as clsGetOrderBookingDetailsListForWorkOrderBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadOrderBookingDetailsListforWorkOrderMilan");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, nvo.ModalityID);
                this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "DeliveryStatus", DbType.Boolean, nvo.DeliveryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "CheckResultEntryStatus", DbType.Boolean, nvo.CheckResultEntryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "ResultEntryStatus", DbType.Boolean, nvo.ResultEntryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntry", DbType.Boolean, nvo.IsTechnicianEntry);
                this.dbServer.AddInParameter(storedProcCommand, "IsFinalized", DbType.Boolean, nvo.IsFinalizedByDr);
                this.dbServer.AddInParameter(storedProcCommand, "NotDone", DbType.Boolean, nvo.NotDone);
                this.dbServer.AddInParameter(storedProcCommand, "Modality", DbType.Int64, nvo.ModalityID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID", DbType.Int64, nvo.RadiologistID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BookingDetails == null)
                    {
                        nvo.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadOrderBookingDetailsVO item = new clsRadOrderBookingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                            ChargeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChargeID"])),
                            TestCode = Convert.ToString(DALHelper.HandleDBNull(reader["TestCode"])),
                            TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Service"])),
                            LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"])),
                            IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"])),
                            IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"])),
                            IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]))
                        };
                        if (nvo.ReportDelivery)
                        {
                            item.ResultEntryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultEntryID"]));
                        }
                        item.IsTechnicianEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntry"]));
                        item.IsTechnicianEntryFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntryFinalized"]));
                        item.Contrast = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Contrast"]));
                        item.ContrastDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ContrastDetails"]));
                        item.FilmWastage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FilmWastage"]));
                        item.FilmWastageDetails = Convert.ToString(DALHelper.HandleDBNull(reader["WastageDetails"]));
                        item.NoOfFilms = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NoOfFlims"]));
                        item.Sedation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Sedation"]));
                        item.IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"]));
                        item.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"]));
                        item.ModalityID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ModalityID"]));
                        item.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        item.PatientFullName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        item.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"]));
                        item.Radiologist = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));
                        item.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"]));
                        item.PrescribingDr = Convert.ToString(DALHelper.HandleDBNull(reader["PrescribingDr"]));
                        item.RadiologistEntryDate = (DateTime?) DALHelper.HandleDBNull(reader["RadiologistEntryDate"]);
                        item.FinalizedByDoctorTime = (DateTime?) DALHelper.HandleDBNull(reader["FinalizedByDoctorTime"]);
                        item.IsUpload = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUpload"]));
                        item.ReportUploadDateTime = (DateTime?) DALHelper.HandleDBNull(reader["ReportUploadDateTime"]);
                        item.DeliveryReportDateTime = (DateTime?) DALHelper.HandleDBNull(reader["DeliveryReportDateTime"]);
                        item.CancelledDateTime = (DateTime?) DALHelper.HandleDBNull(reader["CancelledDate"]);
                        nvo.BookingDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOrderList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOrderBookingListBizActionVO nvo = valueObject as clsGetOrderBookingListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadiologyOrderBookingListMilan");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                if ((nvo.OPDNO != null) && (nvo.OPDNO.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ScanNo", DbType.String, nvo.OPDNO + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ScanNo", DbType.String, null);
                }
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                }
                if ((nvo.MRNO != null) && (nvo.MRNO.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, null);
                }
                long? categoryID = nvo.CategoryID;
                if ((categoryID.GetValueOrDefault() > 0L) && (categoryID != null))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.CategoryID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, null);
                }
                if (nvo.IsFinalizedByDr)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsFinalized", DbType.Boolean, nvo.IsFinalizedByDr);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsFinalized", DbType.Boolean, null);
                }
                if (nvo.IsTechnicianEntry)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntry", DbType.Boolean, nvo.IsTechnicianEntry);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntry", DbType.Boolean, null);
                }
                if (!nvo.NotDone)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "NotDone", DbType.Boolean, null);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "NotDone", DbType.Boolean, false);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "DeliveryStatus", DbType.Boolean, nvo.DeliveryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "CheckResultEntryStatus", DbType.Boolean, nvo.CheckResultEntryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "ResultEntryStatus", DbType.Boolean, nvo.ResultEntryStatus);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BookingList == null)
                    {
                        nvo.BookingList = new List<clsRadOrderBookingVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadOrderBookingVO item = new clsRadOrderBookingVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            BillDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]))),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            OrderNo = Convert.ToString(DALHelper.HandleDBNull(reader["OrderNo"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            PatientUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitId"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"])),
                            PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"])),
                            PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientPaidAmount"])),
                            VisitNotes = Convert.ToString(DALHelper.HandleDBNull(reader["VisitNotes"])),
                            Balance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"])),
                            Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freezed"])),
                            Gender = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]))
                        };
                        bool inProcess = item.InProcess;
                        long totalCount = item.TotalCount;
                        item.TotalCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalCount"]));
                        long resultEntryCount = item.ResultEntryCount;
                        item.ResultEntryCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultEntryCount"]));
                        long completedTest = item.CompletedTest;
                        item.CompletedTest = Convert.ToInt64(DALHelper.HandleDBNull(reader["Completed"]));
                        long uploadedCount = item.UploadedCount;
                        item.UploadedCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["UploadedCount"]));
                        long gender = item.Gender;
                        if ((gender <= 2L) && (gender >= 1L))
                        {
                            switch (((int) (gender - 1L)))
                            {
                                case 0:
                                    item.GenderName = "Male";
                                    break;

                                case 1:
                                    item.GenderName = "Female";
                                    break;

                                default:
                                    break;
                            }
                        }
                        item.AgeINDDMMYYYY = Convert.ToString(DALHelper.HandleDBNull(reader["Age"]));
                        item.ContactNO = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO"]));
                        item.ReferredDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        item.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        item.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        item.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        item.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        item.EmailID = Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"]));
                        item.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                        item.AgeInYears = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInYears"]));
                        item.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmail"]));
                        nvo.BookingList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPACSTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPACSTestListBizActionVO nvo = valueObject as clsGetPACSTestListBizActionVO;
            try
            {
                DbCommand command = !nvo.IsForStudyComparision ? this.dbServer.GetStoredProcCommand("Pacsdatashow") : this.dbServer.GetStoredProcCommand("PACS_DisplayStudyVisitWise");
                this.dbServer.AddInParameter(command, "MRNO", DbType.String, nvo.MRNO);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (nvo.PACSTestList == null)
                    {
                        nvo.PACSTestList = new List<clsPACSTestPropertiesVO>();
                    }
                    while (reader.Read())
                    {
                        clsPACSTestPropertiesVO item = new clsPACSTestPropertiesVO {
                            MODALITY = Convert.ToString(DALHelper.HandleDBNull(reader["MODALITY"])),
                            IMAGECOUNT = Convert.ToString(DALHelper.HandleDBNull(reader["IMAGECOUNT"])),
                            STUDYDESC = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC"])),
                            STUDYDATE = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDATE"])),
                            STUDYTIME = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYTIME"])),
                            PHYSICIANNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PHYSICIANNAME"])),
                            PATIENTNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTNAME"])),
                            PATIENTID = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTID"]))
                        };
                        nvo.PACSTestList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPACSTestSeriesList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPACSTestSeriesListBizActionVO nvo = valueObject as clsGetPACSTestSeriesListBizActionVO;
            try
            {
                if (nvo.IsForShowPACS)
                {
                    if (nvo.IsForShowSeriesPACS)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("ShowPatientImages_StudyWise");
                        this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.PACSTestDetails.MRNO);
                        this.dbServer.AddInParameter(storedProcCommand, "SERIESNUMBER", DbType.String, nvo.PACSTestDetails.SERIESNUMBER);
                        DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader3.HasRows)
                        {
                            if (nvo.PACSTestSeriesImageList == null)
                            {
                                nvo.PACSTestSeriesImageList = new List<clsPACSTestSeriesVO>();
                            }
                            while (reader3.Read())
                            {
                                clsPACSTestSeriesVO item = new clsPACSTestSeriesVO {
                                    IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader3["Imagepath"]))
                                };
                                nvo.PACSTestSeriesImageList.Add(item);
                            }
                        }
                        reader3.Close();
                    }
                    else
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("ShowPatientImages");
                        this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.PACSTestDetails.MRNO);
                        this.dbServer.AddInParameter(storedProcCommand, "PATIENTNAME", DbType.String, nvo.PACSTestDetails.PATIENTNAME);
                        this.dbServer.AddInParameter(storedProcCommand, "MODALITY", DbType.String, nvo.PACSTestDetails.MODALITY);
                        this.dbServer.AddInParameter(storedProcCommand, "STUDYDATE", DbType.String, nvo.PACSTestDetails.STUDYDATE);
                        this.dbServer.AddInParameter(storedProcCommand, "STUDYTIME", DbType.String, nvo.PACSTestDetails.STUDYTIME);
                        this.dbServer.AddInParameter(storedProcCommand, "STUDYDESC", DbType.String, nvo.PACSTestDetails.STUDYDESC);
                        DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader4.HasRows)
                        {
                            if (nvo.PACSTestSeriesImageList == null)
                            {
                                nvo.PACSTestSeriesImageList = new List<clsPACSTestSeriesVO>();
                            }
                            while (reader4.Read())
                            {
                                clsPACSTestSeriesVO item = new clsPACSTestSeriesVO {
                                    IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader4["Imagepath"]))
                                };
                                nvo.PACSTestSeriesImageList.Add(item);
                            }
                        }
                        reader4.Close();
                    }
                }
                else if (!nvo.IsForStudyComparision)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("ShowPatientStudyDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.PACSTestDetails.MRNO);
                    this.dbServer.AddInParameter(storedProcCommand, "PATIENTNAME", DbType.String, nvo.PACSTestDetails.PATIENTNAME);
                    this.dbServer.AddInParameter(storedProcCommand, "MODALITY", DbType.String, nvo.PACSTestDetails.MODALITY);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYDATE", DbType.String, nvo.PACSTestDetails.STUDYDATE);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYTIME", DbType.String, nvo.PACSTestDetails.STUDYTIME);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYDESC", DbType.String, nvo.PACSTestDetails.STUDYDESC);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.PACSTestSeriesList == null)
                        {
                            nvo.PACSTestSeriesList = new List<clsPACSTestSeriesVO>();
                        }
                        while (reader2.Read())
                        {
                            clsPACSTestSeriesVO item = new clsPACSTestSeriesVO {
                                PATIENTID = Convert.ToString(DALHelper.HandleDBNull(reader2["PATIENTID"])),
                                PATIENTNAME = Convert.ToString(DALHelper.HandleDBNull(reader2["PATIENTNAME"])),
                                MODALITY = Convert.ToString(DALHelper.HandleDBNull(reader2["MODALITY"])),
                                STUDYDESC = Convert.ToString(DALHelper.HandleDBNull(reader2["STUDYDESC"])),
                                STUDYDATE = Convert.ToString(DALHelper.HandleDBNull(reader2["STUDYDATE"])),
                                STUDYTIME = Convert.ToString(DALHelper.HandleDBNull(reader2["STUDYTIME"])),
                                SERIESNUMBER = Convert.ToString(DALHelper.HandleDBNull(reader2["SERIESNUMBER"]))
                            };
                            nvo.PACSTestSeriesList.Add(item);
                        }
                    }
                    reader2.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("PACS_ShowPatientStudyDetailsForVisitWise");
                    this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.PACSTestDetails.MRNO);
                    this.dbServer.AddInParameter(storedProcCommand, "PATIENTNAME", DbType.String, nvo.PACSTestDetails.PATIENTNAME);
                    this.dbServer.AddInParameter(storedProcCommand, "MODALITY", DbType.String, nvo.PACSTestDetails.MODALITY);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYDATE", DbType.String, nvo.PACSTestDetails.STUDYDATE);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYTIME", DbType.String, nvo.PACSTestDetails.STUDYTIME);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYDESC", DbType.String, nvo.PACSTestDetails.STUDYDESC);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PACSTestSeriesList == null)
                        {
                            nvo.PACSTestSeriesList = new List<clsPACSTestSeriesVO>();
                        }
                        while (reader.Read())
                        {
                            clsPACSTestSeriesVO item = new clsPACSTestSeriesVO {
                                PATIENTID = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTID"])),
                                PATIENTNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTNAME"])),
                                MODALITY = Convert.ToString(DALHelper.HandleDBNull(reader["MODALITY"])),
                                STUDYDESC = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC"])),
                                STUDYDATE = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDATE"])),
                                STUDYTIME = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYTIME"])),
                                SERIESNUMBER = Convert.ToString(DALHelper.HandleDBNull(reader["SERIESNUMBER"])),
                                STUDYDESC1 = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC1"]))
                            };
                            nvo.PACSTestSeriesList.Add(item);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override clsPACSTestSeriesVO GetPACSTestSeriesListaspx(string MRNO, string PATIENTNAME, string MODALITY, string STUDYDATE, string STUDYTIME, string STUDYDESC)
        {
            clsPACSTestSeriesVO svo = new clsPACSTestSeriesVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("ShowPatientImages");
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "PATIENTNAME", DbType.String, PATIENTNAME);
                this.dbServer.AddInParameter(storedProcCommand, "MODALITY", DbType.String, MODALITY);
                this.dbServer.AddInParameter(storedProcCommand, "STUDYDATE", DbType.String, STUDYDATE);
                this.dbServer.AddInParameter(storedProcCommand, "STUDYTIME", DbType.String, STUDYTIME);
                this.dbServer.AddInParameter(storedProcCommand, "STUDYDESC", DbType.String, STUDYDESC);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                svo.PacsTestSeriesList = new List<clsPACSTestSeriesVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPACSTestSeriesVO item = new clsPACSTestSeriesVO {
                            IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]))
                        };
                        svo.PacsTestSeriesList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
            }
            return svo;
        }

        public override clsPACSTestSeriesVO GetPACSTestseriesListSeriesWise(bool IsForShowSeriesPACS, bool IsForShowPACS, string SERIESNUMBER, string MRNO, bool IsVisitWise, string MODALITY, string STUDYTIME, string STUDYDESC, string STUDYDATE)
        {
            clsPACSTestSeriesVO svo = new clsPACSTestSeriesVO();
            try
            {
                DbCommand storedProcCommand;
                if (!IsVisitWise)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("ShowPatientImages_StudyWise");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("PACS_VisitWiseImages");
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYTIME", DbType.String, STUDYTIME);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYDATE", DbType.String, STUDYDATE);
                    this.dbServer.AddInParameter(storedProcCommand, "STUDYDESC", DbType.String, STUDYDESC);
                    this.dbServer.AddInParameter(storedProcCommand, "MODALITY", DbType.String, MODALITY);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, MRNO);
                this.dbServer.AddInParameter(storedProcCommand, "SERIESNUMBER", DbType.String, SERIESNUMBER);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                svo.PacsTestSeriesList = new List<clsPACSTestSeriesVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPACSTestSeriesVO item = new clsPACSTestSeriesVO {
                            IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]))
                        };
                        svo.PacsTestSeriesList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
            }
            return svo;
        }

        public override IValueObject GetRadioGender(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadioTemplateGenderBizActionVO nvo = valueObject as clsGetRadioTemplateGenderBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadioGenderTemplateList");
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.String, nvo.TemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GenderDetails == null)
                    {
                        nvo.GenderDetails = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]))
                        };
                        nvo.GenderDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
            }
            return nvo;
        }

        public override IValueObject GetRadiologistbySpecia(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadiologistBySpecializationBizActionVO nvo = valueObject as clsGetRadiologistBySpecializationBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillRadiologistinResultEntrySpecia");
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRadiologistGenderByitsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadiologistGenderByIDBizActionVO nvo = valueObject as clsGetRadiologistGenderByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillRadiologistGender");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRadiologistResultEntry(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetRadiologistToTempBizActionVO nvo = valueObject as clsGetRadiologistToTempBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadiologistListForTest");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemSupplier.TemplateID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemSupplierList == null)
                    {
                        nvo.ItemSupplierList = new List<clsRadiologyVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadiologyVO item = new clsRadiologyVO {
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            Radiologist = (long) DALHelper.HandleDBNull(reader["RadiologistID"]),
                            TemplateID = (long) DALHelper.HandleDBNull(reader["TemplateID"])
                        };
                        nvo.ItemSupplierList.Add(item);
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
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetRadiologistResultEntryDefined(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetRadiologistResultEntryBizActionVO nvo = valueObject as clsGetRadiologistResultEntryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_GetRadiologistResultEntryDefined]");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.TestIDNew);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemSupplierList == null)
                    {
                        nvo.ItemSupplierList = new List<clsRadiologyVO>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["RadiologistID"], reader["Description"].ToString()));
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
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetRadiologistToTempList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetRadiologistToTempBizActionVO nvo = valueObject as clsGetRadiologistToTempBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadiologistListForTest");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemSupplier.TemplateID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemSupplierList == null)
                    {
                        nvo.ItemSupplierList = new List<clsRadiologyVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadiologyVO item = new clsRadiologyVO {
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            Radiologist = (long) DALHelper.HandleDBNull(reader["RadiologistID"]),
                            TemplateID = (long) DALHelper.HandleDBNull(reader["TemplateID"])
                        };
                        nvo.ItemSupplierList.Add(item);
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
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetRadologyPringHeaderFooterImage(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadologyPringHeaderFooterImageBizActionVO nvo = valueObject as clsGetRadologyPringHeaderFooterImageBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadiologyHeaderFooterImageForPrint");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        nvo.HeaderImage = (byte[]) DALHelper.HandleDBNull(reader["HeaderImage"]);
                        nvo.FooterImage = (byte[]) DALHelper.HandleDBNull(reader["FooterImage"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadResultEntryBizActionVO nvo = valueObject as clsGetRadResultEntryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadResultEntryMilan");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ResultDetails == null)
                    {
                        nvo.ResultDetails = new clsRadResultEntryVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.TestItemList == null)
                            {
                                nvo.TestItemList = new List<clsRadItemDetailMasterVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.Close();
                                    break;
                                }
                                clsRadItemDetailMasterVO item = new clsRadItemDetailMasterVO {
                                    RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"])),
                                    TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                                    StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                                    ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                                    ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                    BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                                    BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                                    Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["IdealQuantity"])),
                                    ActualQantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ActualQantity"])),
                                    WastageQantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WastageQuantity"])),
                                    BalanceQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalQuantity"])),
                                    ExpiryDate = (DateTime?) DALHelper.HandleDBNull(reader["ExpiryDate"]),
                                    ItemCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])),
                                    ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"])),
                                    IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"])),
                                    BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]))
                                };
                                nvo.TestItemList.Add(item);
                            }
                            break;
                        }
                        nvo.ResultDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ResultDetails.RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"]));
                        nvo.ResultDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        nvo.ResultDetails.FilmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FlimId"]));
                        nvo.ResultDetails.NoOfFilms = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfFlims"]));
                        nvo.ResultDetails.RadiologistID1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologistID1"]));
                        nvo.ResultDetails.FirstLevelDescription = Convert.ToString(DALHelper.HandleDBNull(reader["FirstLevelDescription"]));
                        nvo.ResultDetails.TemplateID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])));
                        nvo.ResultDetails.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));
                        nvo.ResultDetails.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                        nvo.ResultDetails.Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]);
                        nvo.ResultDetails.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"]));
                        nvo.ResultDetails.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        nvo.ResultDetails.Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.ResultDetails.IsCompleted = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCompleted"]));
                        nvo.ResultDetails.IsDigitalSignatureRequired = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDigitalSignatureRequired"]));
                        nvo.ResultDetails.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"]));
                        nvo.ResultDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsRadResultEntryPrintDetailsBizActionVO nvo = valueObject as clsRadResultEntryPrintDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_rpt_RadResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "ResultID", DbType.Int64, nvo.ResultID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "OPDIPD", DbType.Int64, nvo.OPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ResultDetails == null)
                    {
                        nvo.ResultDetails = new clsRadResultEntryPrintDetailsVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        nvo.ResultDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.ResultDetails.FirstLevelDescription = Convert.ToString(DALHelper.HandleDBNull(reader["FirstLevelDescription"]));
                        nvo.ResultDetails.TestTemplate = Convert.ToString(DALHelper.HandleDBNull(reader["TestTemplate"]));
                        nvo.ResultDetails.TestNAme = Convert.ToString(DALHelper.HandleDBNull(reader["TestNAme"]));
                        nvo.ResultDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        nvo.ResultDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        nvo.ResultDetails.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        nvo.ResultDetails.OrderDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["OrderDate"])));
                        nvo.ResultDetails.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        nvo.ResultDetails.AgeYear = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeYear"]));
                        nvo.ResultDetails.AgeMonth = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeMonth"]));
                        nvo.ResultDetails.AgeDate = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeDate"]));
                        nvo.ResultDetails.ReferredDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        nvo.ResultDetails.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        nvo.ResultDetails.TestDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["TestDate"])));
                        nvo.ResultDetails.PrintTestName = Convert.ToString(DALHelper.HandleDBNull(reader["PrintTestName"]));
                        nvo.ResultDetails.AddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"])));
                        nvo.ResultDetails.ResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ResultDetails.ResultAddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ResultAddedDateTime"])));
                        nvo.ResultDetails.RadiologistName = Convert.ToString(DALHelper.HandleDBNull(reader["RadiologistName"]));
                        nvo.ResultDetails.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        nvo.ResultDetails.Radiologist = Convert.ToInt64(DALHelper.HandleDBNull(reader["Radiologist"]));
                        nvo.ResultDetails.Radiologist1 = Convert.ToString(DALHelper.HandleDBNull(reader["RadiologistName"]));
                        nvo.ResultDetails.Education1 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        nvo.ResultDetails.RadioDoctorid1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["Radiologist"]));
                        nvo.ResultDetails.Signature1 = (byte[]) DALHelper.HandleDBNull(reader["DigitalSignature"]);
                        nvo.ResultDetails.ReportPrepairDate = Convert.ToString(DALHelper.HandleDBNull(reader["ReportPrepairDate"]));
                        nvo.ResultDetails.Signature2 = (byte[]) DALHelper.HandleDBNull(reader["Logo"]);
                        nvo.ResultDetails.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        nvo.ResultDetails.UnitAddress = Convert.ToString(DALHelper.HandleDBNull(reader["address"]));
                        nvo.ResultDetails.UnitContact = Convert.ToString(DALHelper.HandleDBNull(reader["contact"]));
                        nvo.ResultDetails.UnitContact1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        nvo.ResultDetails.UnitMobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["MobileNO"]));
                        nvo.ResultDetails.UnitEmail = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                        nvo.ResultDetails.UnitWebsite = Convert.ToString(DALHelper.HandleDBNull(reader["WebSite"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServiceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadServiceBizActionVO nvo = (clsGetRadServiceBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillRadServiceComboBox");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTechnicianEntryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTechnicianEntryListBizActionVO nvo = valueObject as clsGetRadTechnicianEntryListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadiologyTechnicianEntryList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderID", DbType.Int64, nvo.RadOrderID);
                this.dbServer.AddInParameter(storedProcCommand, "OrderDetailID", DbType.Int64, nvo.OrderDetailID);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "ChargeID", DbType.Int64, nvo.ChargeID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BookingList == null)
                    {
                        nvo.BookingList = new List<clsRadOrderBookingVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadOrderBookingVO item = new clsRadOrderBookingVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"])),
                            Date = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["Date"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitId"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            Balance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"])),
                            Gender = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]))
                        };
                        long gender = item.Gender;
                        if ((gender <= 2L) && (gender >= 1L))
                        {
                            switch (((int) (gender - 1L)))
                            {
                                case 0:
                                    item.GenderName = "Male";
                                    break;

                                case 1:
                                    item.GenderName = "Female";
                                    break;

                                default:
                                    break;
                            }
                        }
                        item.ReferredDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        item.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                        item.IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]));
                        item.IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"]));
                        item.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        item.Service = Convert.ToString(DALHelper.HandleDBNull(reader["Service"]));
                        item.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        item.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        item.TestID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TestID"]));
                        item.IsTechnicianEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntry"]));
                        item.IsTechnicianEntryFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntryFinalized"]));
                        item.Contrast = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Contrast"]));
                        item.ContrastDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ContrastDetails"]));
                        item.FilmWastage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FilmWastage"]));
                        item.FilmWastageDetails = Convert.ToString(DALHelper.HandleDBNull(reader["WastageDetails"]));
                        item.NoOfFilms = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NoOfFlims"]));
                        item.Sedation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Sedation"]));
                        item.IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        nvo.BookingList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
            }
            return nvo;
        }

        public override IValueObject GetTemplateAndItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTemplateAndItemByTestIDBizActionVO nvo = (clsGetRadTemplateAndItemByTestIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTemplateAndItemDetails");
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsRadTemplateDetailMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.ItemList == null)
                            {
                                nvo.ItemList = new List<clsRadItemDetailMasterVO>();
                            }
                            while (reader.Read())
                            {
                                clsRadItemDetailMasterVO rvo2 = new clsRadItemDetailMasterVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    ItemTestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                                    ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                                    Quantity = (double) DALHelper.HandleDBNull(reader["Quantity"]),
                                    ActualQantity = (double) DALHelper.HandleDBNull(reader["Quantity"]),
                                    ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                    Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                                    BatchesRequired = (bool) DALHelper.HandleDBNull(reader["BatchesRequired"])
                                };
                                nvo.ItemList.Add(rvo2);
                            }
                            break;
                        }
                        clsRadTemplateDetailMasterVO item = new clsRadTemplateDetailMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            TemplateTestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                            TestTemplateID = (long) DALHelper.HandleDBNull(reader["TemplateID"]),
                            TemplateCode = (string) DALHelper.HandleDBNull(reader["Code"]),
                            TemplateName = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.TestList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetTemplateDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTemplateDetailsBizActionVO nvo = (clsGetRadTemplateDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillRadiologyTemplate");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TemplateList == null)
                    {
                        nvo.TemplateList = new List<clsRadiologyVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadiologyVO item = new clsRadiologyVO {
                            TemplateID = (long) reader["ID"],
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            TemplateDesign = (string) DALHelper.HandleDBNull(reader["Template"])
                        };
                        nvo.TemplateList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTemplateBizActionVO nvo = valueObject as clsGetRadTemplateBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadTemplateList");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Radiologist", DbType.Int64, nvo.Radiologist);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateResultID", DbType.Int64, nvo.TemplateResultID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TemplateList == null)
                    {
                        nvo.TemplateList = new List<clsRadiologyVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadiologyVO item = new clsRadiologyVO {
                            TemplateID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            TemplateDesign = (string) DALHelper.HandleDBNull(reader["Template"]),
                            Radiologist = (long) reader["Radiologist"],
                            GenderID = (long) reader["GenderID"],
                            TemplateResultID = (long) reader["TemplateResultID"],
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            RadiologistName = (string) DALHelper.HandleDBNull(reader["RadiologistName"])
                        };
                        nvo.TemplateList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTestDetailsBizActionVO nvo = valueObject as clsGetRadTestDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRadTestList");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Category", DbType.Int64, nvo.Category);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsRadiologyVO>();
                    }
                    while (reader.Read())
                    {
                        clsRadiologyVO item = new clsRadiologyVO {
                            TestID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            TurnAroundTime = (string) DALHelper.HandleDBNull(reader["TurnAroundTime"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = new DateTime?(nullable.Value);
                        item.CategoryID = (long) reader["CategoryID"];
                        item.ServiceID = (long) reader["ServiceID"];
                        item.PrintTestName = (string) DALHelper.HandleDBNull(reader["PrintTestName"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.TestList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override IValueObject GetUploadReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsRadUploadReportBizActionVO nvo = valueObject as clsRadUploadReportBizActionVO;
            try
            {
                clsRadPatientReportVO uploadReportDetails = nvo.UploadReportDetails;
                if (nvo.IsResultEntry)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUploadReportRad");
                    this.dbServer.AddInParameter(storedProcCommand, "RadOrderDetailID", DbType.Int64, uploadReportDetails.RadOrderDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "RadPatientReportID", DbType.Int64, uploadReportDetails.RadPatientReportID);
                    this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, uploadReportDetails.SourceURL);
                    this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, uploadReportDetails.Notes);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, uploadReportDetails.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, uploadReportDetails.Time);
                    if (nvo.UnitID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, uploadReportDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadPatientReportWithOutResultEntry");
                    this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, uploadReportDetails.SourceURL);
                    this.dbServer.AddInParameter(storedProcCommand, "RadOrderDetailID", DbType.Int64, uploadReportDetails.RadOrderDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "RadOrderID", DbType.Int64, uploadReportDetails.RadOrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReportPath", DbType.String, uploadReportDetails.ReportPath);
                    this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, uploadReportDetails.TestID);
                    this.dbServer.AddInParameter(storedProcCommand, "RadiologistID1", DbType.Int64, uploadReportDetails.RefDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, uploadReportDetails.ReferredBy);
                    this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, uploadReportDetails.Notes);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, uploadReportDetails.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, uploadReportDetails.Time);
                    if (nvo.UnitID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, uploadReportDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, uploadReportDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.UploadReportDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetVitalDetailsForRadiology(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetVitalDetailsForRadiologyBizActionVO nvo = valueObject as clsGetVitalDetailsForRadiologyBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetVitalDetailsForRadiology");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.VitalDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.VitalDetails.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VitalDetails == null)
                    {
                        nvo.VitalDetails = new clsVitalDetailsForRadiologyVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        nvo.VitalDetails.height = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientHeight"]));
                        nvo.VitalDetails.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientWeight"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateEmailDelivredIntoList(IValueObject valueObject, clsUserVO UserVo)
        {
            return (valueObject as clsUpdateRadOrderBookingDetailDeliveryStatusBizActionVO);
        }

        private clsAddRadOrderBookingBizActionVO UpdateOrder(clsAddRadOrderBookingBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (pConnection == null) ? this.dbServer.CreateConnection() : pConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (pTransaction == null) ? connection.BeginTransaction() : pTransaction;
                clsRadOrderBookingVO bookingDetails = BizActionObj.BookingDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRadOrderBooking");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, bookingDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "OrderNo", DbType.String, bookingDetails.OrderNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, bookingDetails.BillNo);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, bookingDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, bookingDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_ID", DbType.Int64, bookingDetails.Opd_Ipd_External_ID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitID", DbType.Int64, bookingDetails.Opd_Ipd_External_UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, bookingDetails.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, bookingDetails.IsCancelled);
                this.dbServer.AddInParameter(storedProcCommand, "TestType", DbType.Int64, bookingDetails.TestType);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, bookingDetails.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, bookingDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((bookingDetails.OrderBookingDetails != null) && (bookingDetails.OrderBookingDetails.Count > 0))
                {
                    foreach (clsRadOrderBookingDetailsVO svo in bookingDetails.OrderBookingDetails)
                    {
                        if (svo.ID > 0L)
                        {
                            DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateRadOrderBookingDetails");
                            this.dbServer.AddInParameter(command2, "ID", DbType.Int64, svo.ID);
                            this.dbServer.AddInParameter(command2, "RadOrderID", DbType.Int64, bookingDetails.ID);
                            this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, svo.TestID);
                            this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, svo.ChargeID);
                            this.dbServer.AddInParameter(command2, "TariffServiceId", DbType.Int64, svo.TariffServiceID);
                            if (svo.Number != null)
                            {
                                this.dbServer.AddInParameter(command2, "Number", DbType.String, svo.Number.Trim());
                            }
                            this.dbServer.AddInParameter(command2, "IsEmergency", DbType.Boolean, svo.IsEmergency);
                            this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, svo.DoctorID);
                            this.dbServer.AddInParameter(command2, "IsApproved", DbType.Boolean, svo.IsApproved);
                            this.dbServer.AddInParameter(command2, "IsCompleted", DbType.Boolean, svo.IsCompleted);
                            this.dbServer.AddInParameter(command2, "IsDelivered", DbType.Boolean, svo.IsDelivered);
                            this.dbServer.AddInParameter(command2, "IsPrinted", DbType.Boolean, svo.IsPrinted);
                            this.dbServer.AddInParameter(command2, "MicrobiologistId", DbType.Int64, svo.MicrobiologistID);
                            if (svo.ReferredBy != null)
                            {
                                this.dbServer.AddInParameter(command2, "ReferredBy", DbType.String, svo.ReferredBy.Trim());
                            }
                            this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, bookingDetails.Status);
                            this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                            this.dbServer.AddInParameter(command2, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.ExecuteNonQuery(command2, transaction);
                            continue;
                        }
                        if (svo.ID == 0L)
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddRadOrderBookingDetails");
                            this.dbServer.AddInParameter(command3, "RadOrderID", DbType.Int64, bookingDetails.ID);
                            this.dbServer.AddInParameter(command3, "TestID", DbType.Int64, svo.TestID);
                            this.dbServer.AddInParameter(command3, "ChargeID", DbType.Int64, svo.ChargeID);
                            this.dbServer.AddInParameter(command3, "TariffServiceId", DbType.Int64, svo.TariffServiceID);
                            if (svo.Number != null)
                            {
                                this.dbServer.AddInParameter(command3, "Number", DbType.String, svo.Number.Trim());
                            }
                            this.dbServer.AddInParameter(command3, "IsEmergency", DbType.Boolean, svo.IsEmergency);
                            this.dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, svo.DoctorID);
                            this.dbServer.AddInParameter(command3, "IsApproved", DbType.Boolean, svo.IsApproved);
                            this.dbServer.AddInParameter(command3, "IsCompleted", DbType.Boolean, svo.IsCompleted);
                            this.dbServer.AddInParameter(command3, "IsDelivered", DbType.Boolean, svo.IsDelivered);
                            this.dbServer.AddInParameter(command3, "IsPrinted", DbType.Boolean, svo.IsPrinted);
                            this.dbServer.AddInParameter(command3, "MicrobiologistId", DbType.Int64, svo.MicrobiologistID);
                            if (svo.ReferredBy != null)
                            {
                                this.dbServer.AddInParameter(command3, "ReferredBy", DbType.String, svo.ReferredBy.Trim());
                            }
                            this.dbServer.AddInParameter(command3, "IsResultEntry", DbType.Boolean, svo.IsResultEntry);
                            this.dbServer.AddInParameter(command3, "IsFinalized", DbType.Boolean, svo.IsFinalized);
                            this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, bookingDetails.Status);
                            this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                            this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                            this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                            svo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                            svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        }
                    }
                }
                BizActionObj.SuccessStatus = 0;
                if (pConnection == null)
                {
                    transaction.Commit();
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (pConnection == null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject UpdateRadOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateRadOrderBookingDetailListBizActionVO nvo = valueObject as clsUpdateRadOrderBookingDetailListBizActionVO;
            int count = nvo.OrderBookingDetailList.Count;
            try
            {
                foreach (clsRadOrderBookingDetailsVO svo in nvo.OrderBookingDetailList)
                {
                    if (svo.IsReportCollected)
                    {
                        clsRadOrderBookingDetailsVO orderBookingDetaildetails = nvo.OrderBookingDetaildetails;
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRadOrderBookingDetailsForAgencyRepSchedule");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, svo.IsOutSourced);
                        this.dbServer.AddInParameter(storedProcCommand, "ExtAgencyID", DbType.Int64, svo.AgencyID);
                        this.dbServer.AddInParameter(storedProcCommand, "ReportCollected", DbType.DateTime, nvo.ReportCollectionDate);
                        this.dbServer.AddInParameter(storedProcCommand, "IsReportCollected", DbType.Boolean, svo.IsReportCollected);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject UpdateReportDelivery(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateReportDeliveryBizActionVO nvo = valueObject as clsUpdateReportDeliveryBizActionVO;
            try
            {
                clsRadOrderBookingDetailsVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRadReportDeliveryMilan");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Boolean, details.IsDelivered);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, details.TestID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddRadResultEntryBizActionVO UpdateResult(clsAddRadResultEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsRadResultEntryVO resultDetails = BizActionObj.ResultDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_UpdateRadResultEntryMilan]");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, resultDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderID", DbType.Int64, resultDetails.RadOrderID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderDetailID", DbType.Int64, resultDetails.BookingDetailsID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, resultDetails.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "FlimId", DbType.Int64, resultDetails.FilmID);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfFlims", DbType.Int64, resultDetails.NoOfFilms);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID1", DbType.Int64, resultDetails.RadiologistID1);
                this.dbServer.AddInParameter(storedProcCommand, "FirstLevelDescription", DbType.String, resultDetails.FirstLevelDescription);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateResultID", DbType.Int64, resultDetails.TemplateResultID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID2", DbType.Int64, resultDetails.RadiologistID2);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID3", DbType.Int64, resultDetails.RadiologistID3);
                this.dbServer.AddInParameter(storedProcCommand, "SecondLevelDescription", DbType.String, resultDetails.SecondLevelDescription);
                this.dbServer.AddInParameter(storedProcCommand, "ThirdLevelDescription", DbType.String, resultDetails.ThirdLevelDescription);
                this.dbServer.AddInParameter(storedProcCommand, "FirstLevelId", DbType.Int64, resultDetails.FirstLevelId);
                this.dbServer.AddInParameter(storedProcCommand, "SecondLevelId", DbType.Int64, resultDetails.SecondLevelId);
                this.dbServer.AddInParameter(storedProcCommand, "ThirdLevelId", DbType.Int64, resultDetails.ThirdLevelId);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateId", DbType.Int64, resultDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, resultDetails.IsOutSourced);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyId", DbType.Int64, resultDetails.AgencyId);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, resultDetails.ReferredBy.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "IsFinalized", DbType.Boolean, resultDetails.IsFinalized);
                this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, resultDetails.SourceURL);
                this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, resultDetails.Report);
                this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, resultDetails.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, resultDetails.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, resultDetails.Time);
                this.dbServer.AddInParameter(storedProcCommand, "IsCompleted", DbType.Boolean, resultDetails.IsCompleted);
                this.dbServer.AddInParameter(storedProcCommand, "IsUpload", DbType.Boolean, BizActionObj.IsUpload);
                if (resultDetails.IsRefDoctorSigniture)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsDigitalSignatureRequired", DbType.Boolean, resultDetails.IsRefDoctorSigniture);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorUserID", DbType.Int64, resultDetails.RadiologistID1);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsDigitalSignatureRequired", DbType.Boolean, null);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorUserID", DbType.Int64, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.ResultDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return BizActionObj;
        }

        private clsUpdateRadTechnicianEntryBizActionVO UpdateTechnician(clsUpdateRadTechnicianEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection myConnection = null;
            try
            {
                myConnection = this.dbServer.CreateConnection();
                if (myConnection.State == ConnectionState.Closed)
                {
                    myConnection.Open();
                }
                transaction = myConnection.BeginTransaction();
                clsRadResultEntryVO resultDetails = BizActionObj.ResultDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRadTechnicianEntry");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, resultDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderID", DbType.Int64, resultDetails.RadOrderID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderDetailID", DbType.Int64, resultDetails.BookingDetailsID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, resultDetails.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "FlimId", DbType.Int64, resultDetails.FilmID);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfFlims", DbType.Int64, resultDetails.NoOfFilms);
                this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, resultDetails.IsOutSourced);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyId", DbType.Int64, resultDetails.AgencyId);
                if (resultDetails.ReferredBy != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, resultDetails.ReferredBy.Trim());
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, resultDetails.ReferredBy);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Contrast", DbType.Boolean, resultDetails.Contrast);
                this.dbServer.AddInParameter(storedProcCommand, "Sedation", DbType.Boolean, resultDetails.Sedation);
                this.dbServer.AddInParameter(storedProcCommand, "ContrastDetails", DbType.String, resultDetails.ContrastDetails);
                this.dbServer.AddInParameter(storedProcCommand, "FilmWastage", DbType.Boolean, resultDetails.FilmWastage);
                this.dbServer.AddInParameter(storedProcCommand, "FilmWastageDetails", DbType.String, resultDetails.FilmWastageDetails);
                this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntry", DbType.Boolean, resultDetails.IsTechnicianEntry);
                this.dbServer.AddInParameter(storedProcCommand, "IsTechnicianEntryFinalized", DbType.Boolean, resultDetails.IsTechnicianEntryFinalized);
                this.dbServer.AddInParameter(storedProcCommand, "TechnicianUserID", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TechnicianEntryDate", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, resultDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((resultDetails.TestItemList != null) && (resultDetails.TestItemList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteRadTestResultItemDetails");
                    this.dbServer.AddInParameter(command2, "RadTechnicianEntryID", DbType.Int64, resultDetails.ID);
                    this.dbServer.AddInParameter(command2, "RadOrderID", DbType.Int64, resultDetails.RadOrderID);
                    this.dbServer.AddInParameter(command2, "RadOrderDetailID", DbType.Int64, resultDetails.BookingDetailsID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((resultDetails.TestItemList != null) && (resultDetails.TestItemList.Count > 0))
                {
                    foreach (clsRadItemDetailMasterVO rvo in resultDetails.TestItemList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddRadResultItemDetails");
                        this.dbServer.AddInParameter(command3, "RadOrderID", DbType.Int64, resultDetails.RadOrderID);
                        this.dbServer.AddInParameter(command3, "RadOrderDetailID", DbType.Int64, resultDetails.BookingDetailsID);
                        this.dbServer.AddInParameter(command3, "RadTechnicianEntryID", DbType.Int64, resultDetails.ID);
                        this.dbServer.AddInParameter(command3, "TestID", DbType.Int64, resultDetails.TestID);
                        this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, rvo.StoreID);
                        this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, rvo.ItemID);
                        this.dbServer.AddInParameter(command3, "BatchID", DbType.Int64, rvo.BatchID);
                        this.dbServer.AddInParameter(command3, "IdealQuantity", DbType.Double, rvo.Quantity);
                        this.dbServer.AddInParameter(command3, "ActualQantity", DbType.Double, rvo.ActualQantity);
                        this.dbServer.AddInParameter(command3, "BalQantity", DbType.Double, rvo.BalanceQuantity);
                        this.dbServer.AddInParameter(command3, "WastageQuantity", DbType.Double, rvo.WastageQantity);
                        this.dbServer.AddInParameter(command3, "ItemCategoryID", DbType.Int64, rvo.ItemCategoryID);
                        this.dbServer.AddInParameter(command3, "ExpiryDate ", DbType.DateTime, rvo.ExpiryDate);
                        this.dbServer.AddInParameter(command3, "Remarks", DbType.String, rvo.Remarks);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        rvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        rvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        if (resultDetails.IsTechnicianEntryFinalized && (BizActionObj.AutoDeductStock && BizActionObj.ItemCusmption))
                        {
                            clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                            clsAddItemStockBizActionVO valueObject = new clsAddItemStockBizActionVO();
                            clsItemStockVO kvo = new clsItemStockVO {
                                ItemID = rvo.ItemID,
                                BatchID = rvo.BatchID,
                                TransactionTypeID = InventoryTransactionType.RadiologyTestConsumption,
                                TransactionQuantity = rvo.ActualQantity + rvo.WastageQantity,
                                BatchCode = rvo.BatchCode,
                                TransactionID = BizActionObj.ResultDetails.ID,
                                Date = DateTime.Now,
                                Time = DateTime.Now,
                                OperationType = InventoryStockOperationType.Subtraction,
                                StoreID = new long?(rvo.StoreID)
                            };
                            valueObject.Details = kvo;
                            valueObject.Details.ID = 0L;
                            valueObject = (clsAddItemStockBizActionVO) instance.Add(valueObject, UserVo, myConnection, transaction);
                            if (valueObject.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            kvo.ID = valueObject.Details.ID;
                        }
                    }
                }
                transaction.Commit();
            }
            catch
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.ResultDetails = null;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateTechnicianEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsUpdateRadTechnicianEntryBizActionVO bizActionObj = valueObject as clsUpdateRadTechnicianEntryBizActionVO;
                bizActionObj = (bizActionObj.ResultDetails.ID != 0L) ? this.UpdateTechnician(bizActionObj, UserVo) : this.AddTechnicianEntry(bizActionObj, UserVo);
            }
            catch
            {
            }
            return valueObject;
        }

        private clsAddRadTemplateMasterBizActionVO UpdateTemplateDetails(clsAddRadTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsRadiologyVO templateDetails = BizActionObj.TemplateDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRadTemplateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, templateDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, templateDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, templateDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, templateDetails.TemplateDesign.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Radiologist", DbType.Int64, templateDetails.Radiologist);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, templateDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateResultID", DbType.Int64, templateDetails.TemplateResultID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                List<MasterListItem> genderList = BizActionObj.TemplateDetails.GenderList;
                if ((genderList != null) || (genderList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteRadioGenderToTemplate");
                    this.dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, templateDetails.TemplateID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (MasterListItem item in genderList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddRadioGenderToTemplate");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "GenderID", DbType.Int64, item.ID);
                        this.dbServer.AddInParameter(command3, "TemplateID", DbType.Int64, templateDetails.TemplateID);
                        this.dbServer.AddInParameter(command3, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, item.Status);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        this.dbServer.ExecuteNonQuery(command3);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        private clsAddRadTestMasterBizActionVO UpdateTest(clsAddRadTestMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsRadiologyVO testDetails = BizActionObj.TestDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRadTestMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, testDetails.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, testDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, testDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, testDetails.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, testDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "TurnAroundTime", DbType.String, testDetails.TurnAroundTime);
                this.dbServer.AddInParameter(storedProcCommand, "PrintTestName", DbType.String, testDetails.PrintTestName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((testDetails.TestTemplateList != null) && (testDetails.TestTemplateList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteTestTemplate");
                    this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, testDetails.TestID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                foreach (clsRadTemplateDetailMasterVO rvo in testDetails.TestTemplateList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddRadTestTemplateDetailMaster");
                    this.dbServer.AddInParameter(command3, "TestID", DbType.Int64, testDetails.TestID);
                    this.dbServer.AddInParameter(command3, "TemplateID", DbType.Int64, rvo.TestTemplateID);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, rvo.Status);
                    this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    rvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                if ((testDetails.TestItemList != null) && (testDetails.TestItemList.Count > 0))
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_DeleteTestItem");
                    this.dbServer.AddInParameter(command4, "TestID", DbType.Int64, testDetails.TestID);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                }
                foreach (clsRadItemDetailMasterVO rvo2 in testDetails.TestItemList)
                {
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddRadTestItemDetailMaster");
                    this.dbServer.AddInParameter(command5, "TestID", DbType.Int64, testDetails.TestID);
                    this.dbServer.AddInParameter(command5, "ItemID", DbType.Int64, rvo2.ItemID);
                    this.dbServer.AddInParameter(command5, "Quantity", DbType.Double, rvo2.Quantity);
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, rvo2.Status);
                    this.dbServer.AddInParameter(command5, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo2.ID);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                    rvo2.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.TestDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizActionObj;
        }

        public override IValueObject UploadReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsRadUploadReportBizActionVO nvo = valueObject as clsRadUploadReportBizActionVO;
            try
            {
                clsRadPatientReportVO uploadReportDetails = nvo.UploadReportDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddRadPatientReportWithOutResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, uploadReportDetails.SourceURL);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderDetailID", DbType.Int64, uploadReportDetails.RadOrderDetailID);
                this.dbServer.AddInParameter(storedProcCommand, "RadOrderID", DbType.Int64, uploadReportDetails.RadOrderID);
                this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, uploadReportDetails.Report);
                this.dbServer.AddInParameter(storedProcCommand, "ReportPath", DbType.String, uploadReportDetails.ReportPath);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, uploadReportDetails.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "RadiologistID1", DbType.Int64, uploadReportDetails.RefDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, uploadReportDetails.ReferredBy);
                this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, uploadReportDetails.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, uploadReportDetails.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, uploadReportDetails.Time);
                if (nvo.UnitID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, uploadReportDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, uploadReportDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.UploadReportDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject ViewTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadViewTemplateBizActionVO nvo = (clsGetRadViewTemplateBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ViewRadTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Template == null)
                    {
                        nvo.Template = new clsRadiologyVO();
                    }
                    while (reader.Read())
                    {
                        nvo.Template.TemplateID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.Template.TemplateDesign = (string) DALHelper.HandleDBNull(reader["Template"]);
                        nvo.Template.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

