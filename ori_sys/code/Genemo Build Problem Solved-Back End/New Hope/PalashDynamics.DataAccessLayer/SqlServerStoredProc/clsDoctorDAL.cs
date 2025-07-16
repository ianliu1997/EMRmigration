namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Billing;
    using PalashDynamics.ValueObjects.Master;
    using PalashDynamics.ValueObjects.Master.DoctorMaster;
    using PalashDynamics.ValueObjects.Master.DoctorPayment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsDoctorDAL : clsBaseDoctorDAL
    {
        private Database dbServer;

        private clsDoctorDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private clsAddDoctorMasterBizActionVO AddDcotorMaster(clsAddDoctorMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsDoctorVO doctorDetails = BizActionObj.DoctorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorMaster");
                if (doctorDetails.FirstName != null)
                {
                    doctorDetails.FirstName = doctorDetails.FirstName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, doctorDetails.FirstName);
                if (doctorDetails.MiddleName != null)
                {
                    doctorDetails.MiddleName = doctorDetails.MiddleName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, doctorDetails.MiddleName);
                if (doctorDetails.LastName != null)
                {
                    doctorDetails.LastName = doctorDetails.LastName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, doctorDetails.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, doctorDetails.DOB);
                if (doctorDetails.Education != null)
                {
                    doctorDetails.Education = doctorDetails.Education.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Education", DbType.String, doctorDetails.Education);
                if (doctorDetails.Experience != null)
                {
                    doctorDetails.Experience = doctorDetails.Experience.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Experience", DbType.String, doctorDetails.Experience);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, doctorDetails.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, doctorDetails.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorTypeID", DbType.Int64, doctorDetails.DoctorType);
                this.dbServer.AddInParameter(storedProcCommand, "EmailId", DbType.String, doctorDetails.EmailId);
                this.dbServer.AddInParameter(storedProcCommand, "GenderId", DbType.Int64, doctorDetails.GenderId);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, doctorDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "DigitalSignature", DbType.Binary, doctorDetails.Signature);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusId", DbType.Int64, doctorDetails.MaritalStatusId);
                this.dbServer.AddInParameter(storedProcCommand, "ProvidentFund", DbType.String, doctorDetails.ProvidentFund);
                this.dbServer.AddInParameter(storedProcCommand, "PermanentAccountNumber", DbType.String, doctorDetails.PermanentAccountNumber);
                this.dbServer.AddInParameter(storedProcCommand, "AccessCardNumber", DbType.String, doctorDetails.AccessCardNumber);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationNumber", DbType.String, doctorDetails.RegistrationNumber);
                this.dbServer.AddInParameter(storedProcCommand, "DateofJoining", DbType.DateTime, doctorDetails.DateofJoining);
                this.dbServer.AddInParameter(storedProcCommand, "EmployeeNumber", DbType.String, doctorDetails.EmployeeNumber);
                this.dbServer.AddInParameter(storedProcCommand, "DocCategoryId", DbType.Int64, doctorDetails.DoctorCategoryId);
                this.dbServer.AddInParameter(storedProcCommand, "MarketingExecutivesID", DbType.Int64, doctorDetails.MarketingExecutivesID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, doctorDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorDetails.DoctorId);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.DoctorDetails.DoctorId = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (doctorDetails.IsFromReferralDoctorChildWindow)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorAddressDetail");
                    this.dbServer.AddInParameter(command2, "DoctorId", DbType.Int64, BizActionObj.DoctorDetails.DoctorId);
                    this.dbServer.AddInParameter(command2, "AddressTypeID", DbType.Int64, BizActionObj.DoctorDetails.AddressTypeID);
                    this.dbServer.AddInParameter(command2, "Name", DbType.String, BizActionObj.DoctorDetails.Name);
                    this.dbServer.AddInParameter(command2, "Address", DbType.String, BizActionObj.DoctorDetails.Address);
                    this.dbServer.AddInParameter(command2, "Contact1", DbType.String, BizActionObj.DoctorDetails.Contact1);
                    this.dbServer.AddInParameter(command2, "Contact2", DbType.String, BizActionObj.DoctorDetails.Contact2);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddParameter(command2, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if (BizActionObj.SuccessStatus == 0)
                {
                    foreach (clsUnitDepartmentsDetailsVO svo in doctorDetails.UnitDepartmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorDepartmentDetails");
                        this.dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, doctorDetails.DoctorId);
                        this.dbServer.AddInParameter(command3, "DepartmentID", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, svo.UnitID);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, svo.Status);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                    }
                    foreach (clsUnitClassificationsDetailsVO svo2 in doctorDetails.UnitClassificationDetailsList)
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorClassificationDetails");
                        this.dbServer.AddInParameter(command4, "DoctorID", DbType.Int64, doctorDetails.DoctorId);
                        this.dbServer.AddInParameter(command4, "ClassificationID", DbType.Int64, svo2.ClassificationID);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorAddressInfoBizActionVO nvo = (clsAddDoctorAddressInfoBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorAddressDetail");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.objDoctorBankDetail.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "AddressTypeID", DbType.Int64, nvo.objDoctorBankDetail.AddressTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, nvo.objDoctorBankDetail.Name);
                this.dbServer.AddInParameter(storedProcCommand, "Address", DbType.String, nvo.objDoctorBankDetail.Address);
                this.dbServer.AddInParameter(storedProcCommand, "Contact1", DbType.String, nvo.objDoctorBankDetail.Contact1);
                this.dbServer.AddInParameter(storedProcCommand, "Contact2", DbType.String, nvo.objDoctorBankDetail.Contact2);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.objDoctorBankDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(this.dbServer.GetParameterValue(storedProcCommand, "ID")));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorBankInfoBizActionVO nvo = (clsAddDoctorBankInfoBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorBankDetail");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.objDoctorBankDetail.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "BankId", DbType.Int64, nvo.objDoctorBankDetail.BankId);
                this.dbServer.AddInParameter(storedProcCommand, "BranchId", DbType.Int64, nvo.objDoctorBankDetail.BranchId);
                this.dbServer.AddInParameter(storedProcCommand, "AccountNumber", DbType.String, nvo.objDoctorBankDetail.AccountNumber);
                this.dbServer.AddInParameter(storedProcCommand, "AccountType", DbType.Boolean, nvo.objDoctorBankDetail.AccountType);
                this.dbServer.AddInParameter(storedProcCommand, "BranchAddress", DbType.String, nvo.objDoctorBankDetail.BranchAddress);
                this.dbServer.AddInParameter(storedProcCommand, "MICRNumber", DbType.Int64, nvo.objDoctorBankDetail.MICRNumber);
                this.dbServer.AddInParameter(storedProcCommand, "IFSCCode", DbType.String, nvo.objDoctorBankDetail.IFSCCode);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.objDoctorBankDetail.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddDoctorBillPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorBillPaymentDetailsBizActionVO nvo = (clsAddDoctorBillPaymentDetailsBizActionVO) valueObject;
            try
            {
                clsDoctorPaymentVO doctorInfo = nvo.DoctorInfo;
                foreach (clsDoctorPaymentVO tvo2 in nvo.DoctorDetails)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorBillPaymentDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo2.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorPaymentID", DbType.Int64, tvo2.DoctorPaymentID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorPaymentUnitID", DbType.Int64, tvo2.DoctorPaymentUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, tvo2.BillID);
                    this.dbServer.AddInParameter(storedProcCommand, "BillUnitID", DbType.Int64, tvo2.BillUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorSharePercentage", DbType.Double, tvo2.DoctorSharePercentage);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorShareAmount", DbType.Double, tvo2.DoctorShareAmount);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorInfo.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.DoctorInfo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorDepartmentDetailsBizActionVO bizActionObj = valueObject as clsAddDoctorDepartmentDetailsBizActionVO;
            return ((bizActionObj.DepartmentDetails.DoctorDepartmentDetailID != 0L) ? this.UpdateDoctorDepartmentDetails(bizActionObj, objUserVO) : this.AddDoctorDepartmentDetails(bizActionObj, objUserVO));
        }

        private clsAddDoctorDepartmentDetailsBizActionVO AddDoctorDepartmentDetails(clsAddDoctorDepartmentDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsDoctorVO departmentDetails = BizActionObj.DepartmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorDepartmentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, departmentDetails.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, departmentDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, departmentDetails.Status);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, departmentDetails.DoctorDepartmentDetailID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.DepartmentDetails.DepartmentID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorMasterBizActionVO bizActionObj = valueObject as clsAddDoctorMasterBizActionVO;
            return ((bizActionObj.DoctorDetails.DoctorId != 0L) ? this.UpdateDcotorMaster(bizActionObj, objUserVO) : this.AddDcotorMaster(bizActionObj, objUserVO));
        }

        public override IValueObject AddDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorPaymentDetailsBizActionVO nvo = (clsAddDoctorPaymentDetailsBizActionVO) valueObject;
            try
            {
                clsDoctorPaymentVO doctorInfo = nvo.DoctorInfo;
                foreach (clsDoctorPaymentVO tvo2 in nvo.DoctorDetails)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorPayment");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo2.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, tvo2.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctor_UnitID", DbType.Int64, tvo2.DoctorUnitID);
                    if (doctorInfo.PaymentNo != null)
                    {
                        doctorInfo.PaymentNo = tvo2.PaymentNo.Trim();
                    }
                    this.dbServer.AddParameter(storedProcCommand, "PaymentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000");
                    this.dbServer.AddInParameter(storedProcCommand, "PaymentDate", DbType.DateTime, tvo2.PaymentDate);
                    this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, tvo2.TotalBillAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorPayAmount", DbType.Double, tvo2.DoctorPayAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "PaidAmount", DbType.Double, tvo2.PaidAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "BalanceAmount", DbType.Double, tvo2.BalanceAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOnBill", DbType.Boolean, tvo2.IsOnBill);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFix", DbType.Boolean, tvo2.IsFix);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, doctorInfo.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, doctorInfo.Status);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorInfo.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.DoctorInfo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddDoctorShareDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorShareDetailsBizActionVO nvo = valueObject as clsAddDoctorShareDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            if (nvo.IsCompanyForm)
            {
                try
                {
                    clsDoctorShareServicesDetailsVO doctorShareDetails = nvo.DoctorShareDetails;
                    foreach (clsDoctorShareServicesDetailsVO svo2 in nvo.DoctorShareInfoList)
                    {
                        if (svo2.IsSelected)
                        {
                            connection = this.dbServer.CreateConnection();
                            connection.Open();
                            transaction = connection.BeginTransaction();
                            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyandAssCompShareDetails");
                            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, doctorShareDetails.UnitID);
                            this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, svo2.CompanyID);
                            this.dbServer.AddInParameter(storedProcCommand, "AssCompanyID", DbType.Int64, svo2.AssCompanyID);
                            this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, svo2.TariffID);
                            this.dbServer.AddInParameter(storedProcCommand, "TariffServiceID", DbType.Int64, svo2.TariffServiceID);
                            this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.String, svo2.ServiceId);
                            this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, svo2.SpecializationID);
                            this.dbServer.AddInParameter(storedProcCommand, "ServiceRate", DbType.Decimal, svo2.ServiceRate);
                            this.dbServer.AddInParameter(storedProcCommand, "SharePercentage", DbType.Decimal, svo2.DoctorSharePercentage);
                            this.dbServer.AddInParameter(storedProcCommand, "ShareAmount", DbType.Decimal, svo2.DoctorShareAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, doctorShareDetails.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, doctorShareDetails.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo2.Status);
                            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.ExecuteNonQuery(storedProcCommand);
                            nvo.DoctorShareDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    clsDoctorShareServicesDetailsVO doctorShareDetails = nvo.DoctorShareDetails;
                    if (!nvo.ISShareModalityWise)
                    {
                        foreach (clsDoctorShareServicesDetailsVO svo4 in nvo.DoctorShareInfoList)
                        {
                            connection = this.dbServer.CreateConnection();
                            connection.Open();
                            transaction = connection.BeginTransaction();
                            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDoctorShareDetails");
                            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, svo4.UnitID);
                            this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, svo4.DoctorId);
                            this.dbServer.AddInParameter(storedProcCommand, "IsApplyToallDoctorWithAllTariffAndAllModality", DbType.Boolean, nvo.IsApplyToallDoctorWithAllTariffAndAllModality);
                            this.dbServer.AddInParameter(storedProcCommand, "IsApplyToallDoctor", DbType.Boolean, nvo.IsAllDoctorShate);
                            this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, svo4.TariffID);
                            this.dbServer.AddInParameter(storedProcCommand, "TariffServiceID", DbType.Int64, svo4.TariffServiceID);
                            this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.String, svo4.ServiceId);
                            this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, svo4.SpecializationID);
                            this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, svo4.SubSpecializationId);
                            this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, svo4.DepartmentId);
                            this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, svo4.ModalityID);
                            this.dbServer.AddInParameter(storedProcCommand, "ServiceRate", DbType.Decimal, svo4.ServiceRate);
                            this.dbServer.AddInParameter(storedProcCommand, "SharePercentage", DbType.Decimal, svo4.DoctorSharePercentage);
                            this.dbServer.AddInParameter(storedProcCommand, "ShareAmount", DbType.Decimal, svo4.DoctorShareAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "SpecializationModalityWise", DbType.Boolean, nvo.ISShareModalityWise);
                            this.dbServer.AddInParameter(storedProcCommand, "DontChangeTheExistingDoctor", DbType.Boolean, nvo.DontChangeTheExistingDoctor);
                            this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, svo4.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "ISFORALLDOCTOR", DbType.Boolean, nvo.ISFORALLDOCTOR);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, svo4.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo4.Status);
                            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo4.ID);
                            this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                            nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            transaction.Commit();
                            storedProcCommand.Connection.Close();
                        }
                    }
                    else if (!nvo.IsApplyToallDoctorWithAllTariffAndAllModality)
                    {
                        connection = this.dbServer.CreateConnection();
                        connection.Open();
                        transaction = connection.BeginTransaction();
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDoctorShareDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.DoctorShareDetails.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorShareDetails.DoctorId);
                        this.dbServer.AddInParameter(storedProcCommand, "IsApplyToallDoctor", DbType.Boolean, nvo.IsAllDoctorShate);
                        this.dbServer.AddInParameter(storedProcCommand, "IsApplyToallDoctorWithAllTariffAndAllModality", DbType.Boolean, nvo.IsApplyToallDoctorWithAllTariffAndAllModality);
                        this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.DoctorShareDetails.TariffID);
                        this.dbServer.AddInParameter(storedProcCommand, "TariffServiceID", DbType.Int64, nvo.DoctorShareDetails.TariffServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.String, nvo.DoctorShareDetails.ServiceId);
                        this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.DoctorShareDetails.SpecializationID);
                        this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, nvo.DoctorShareDetails.SubSpecializationId);
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DoctorShareDetails.DepartmentId);
                        this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, nvo.DoctorShareDetails.ModalityID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceRate", DbType.Decimal, nvo.DoctorShareDetails.ServiceRate);
                        this.dbServer.AddInParameter(storedProcCommand, "SharePercentage", DbType.Decimal, nvo.DoctorShareDetails.DoctorSharePercentage);
                        this.dbServer.AddInParameter(storedProcCommand, "ShareAmount", DbType.Decimal, nvo.DoctorShareDetails.DoctorShareAmount);
                        this.dbServer.AddInParameter(storedProcCommand, "SpecializationModalityWise", DbType.Boolean, nvo.ISShareModalityWise);
                        this.dbServer.AddInParameter(storedProcCommand, "DontChangeTheExistingDoctor", DbType.Boolean, nvo.DontChangeTheExistingDoctor);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.DoctorShareDetails.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "ISFORALLDOCTOR", DbType.Boolean, nvo.ISFORALLDOCTOR);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, nvo.DoctorShareDetails.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.DoctorShareDetails.Status);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.DoctorShareDetails.ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                        transaction.Commit();
                        storedProcCommand.Connection.Close();
                    }
                    else
                    {
                        foreach (clsDoctorShareServicesDetailsVO svo3 in nvo.DoctorShareInfoList)
                        {
                            if (svo3.IsSelected)
                            {
                                connection = this.dbServer.CreateConnection();
                                connection.Open();
                                transaction = connection.BeginTransaction();
                                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDoctorShareDetails");
                                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, svo3.UnitID);
                                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, svo3.DoctorId);
                                this.dbServer.AddInParameter(storedProcCommand, "IsApplyToallDoctorWithAllTariffAndAllModality", DbType.Boolean, nvo.IsApplyToallDoctorWithAllTariffAndAllModality);
                                this.dbServer.AddInParameter(storedProcCommand, "IsApplyToallDoctor", DbType.Boolean, nvo.IsAllDoctorShate);
                                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, svo3.TariffID);
                                this.dbServer.AddInParameter(storedProcCommand, "TariffServiceID", DbType.Int64, svo3.TariffServiceID);
                                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.String, svo3.ServiceId);
                                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, svo3.SpecializationID);
                                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, svo3.SubSpecializationId);
                                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, svo3.DepartmentId);
                                this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, svo3.ModalityID);
                                this.dbServer.AddInParameter(storedProcCommand, "ServiceRate", DbType.Decimal, svo3.ServiceRate);
                                this.dbServer.AddInParameter(storedProcCommand, "SharePercentage", DbType.Decimal, svo3.DoctorSharePercentage);
                                this.dbServer.AddInParameter(storedProcCommand, "ShareAmount", DbType.Decimal, svo3.DoctorShareAmount);
                                this.dbServer.AddInParameter(storedProcCommand, "SpecializationModalityWise", DbType.Boolean, nvo.ISShareModalityWise);
                                this.dbServer.AddInParameter(storedProcCommand, "DontChangeTheExistingDoctor", DbType.Boolean, nvo.DontChangeTheExistingDoctor);
                                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, svo3.AddedDateTime);
                                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(storedProcCommand, "ISFORALLDOCTOR", DbType.Boolean, nvo.ISFORALLDOCTOR);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, svo3.AddedDateTime);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo3.Status);
                                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo3.ID);
                                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                                transaction.Commit();
                                storedProcCommand.Connection.Close();
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
                    connection.Close();
                    transaction = null;
                    connection = null;
                }
            }
            return nvo;
        }

        public override IValueObject AddDoctorWaiverDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorWaiverDetailsBizActionVO nvo = valueObject as clsAddDoctorWaiverDetailsBizActionVO;
            try
            {
                clsDoctorWaiverDetailVO doctorWaiverDetails = nvo.DoctorWaiverDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorWaiverDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, doctorWaiverDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, doctorWaiverDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, doctorWaiverDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, doctorWaiverDetails.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, doctorWaiverDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "WaiverDays", DbType.Int64, doctorWaiverDetails.WaiverDays);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceRate", DbType.Decimal, doctorWaiverDetails.Rate);
                this.dbServer.AddInParameter(storedProcCommand, "EmergencyServiceRate", DbType.Decimal, doctorWaiverDetails.EmergencyRate);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorSharePercentage", DbType.Decimal, doctorWaiverDetails.DoctorSharePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "EmergencyDoctorSharePercentage", DbType.Decimal, doctorWaiverDetails.EmergencyDoctorSharePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorShareAmount", DbType.Decimal, doctorWaiverDetails.DoctorShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "EmergencyDoctorShareAmount", DbType.Decimal, doctorWaiverDetails.EmergencyDoctorShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, doctorWaiverDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorWaiverDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.DoctorWaiverDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddupdateBulkRateChangeDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateBulkRateChangeDetailsBizActionVO nvo = valueObject as clsAddUpdateBulkRateChangeDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBulkRateChange");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsApplicable", DbType.Boolean, nvo.BulkRateChangeDetailsVO.IsApplicable);
                this.dbServer.AddInParameter(storedProcCommand, "EffectiveDate", DbType.DateTime, nvo.BulkRateChangeDetailsVO.EffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.BulkRateChangeDetailsVO.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreeze", DbType.Boolean, nvo.BulkRateChangeDetailsVO.IsFreeze);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, 1);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.BulkRateChangeDetailsVO.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.BulkRateChangeDetailsVO.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.IsModify)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteBulkRateChangeForSpecialization");
                    command2.Connection = connection;
                    this.dbServer.AddInParameter(command2, "BulkRateChangeID", DbType.Int64, nvo.BulkRateChangeDetailsVO.ID);
                    this.dbServer.AddInParameter(command2, "BulkRateChangeUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if (nvo.TariffDetailsList == null)
                {
                    nvo.TariffDetailsList = new List<clsTariffMasterBizActionVO>();
                }
                if ((nvo.TariffDetailsList != null) && (nvo.TariffDetailsList.Count > 0))
                {
                    foreach (clsTariffMasterBizActionVO nvo2 in nvo.TariffDetailsList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBulkRateChangeForTariff");
                        command3.Connection = connection;
                        this.dbServer.AddInParameter(command3, "BulkRateChangeID", DbType.Int64, nvo.BulkRateChangeDetailsVO.ID);
                        this.dbServer.AddInParameter(command3, "BulkRateChangeUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "TariffID", DbType.Int64, nvo2.TariffID);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, 1);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, objUserVO.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo2.ID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                if (nvo.SubSpecializationList == null)
                {
                    nvo.SubSpecializationList = new List<clsSubSpecializationVO>();
                }
                if ((nvo.SubSpecializationList != null) && (nvo.SubSpecializationList.Count > 0))
                {
                    foreach (clsSubSpecializationVO nvo3 in nvo.SubSpecializationList)
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBulkRateChangeForSpecialization");
                        command4.Connection = connection;
                        this.dbServer.AddInParameter(command4, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "BulkRateChangeID", DbType.Int64, nvo.BulkRateChangeDetailsVO.ID);
                        this.dbServer.AddInParameter(command4, "BulkRateChangeUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "SpecilizationID", DbType.Int64, nvo3.SpecializationId);
                        this.dbServer.AddInParameter(command4, "SubSpecializationID", DbType.Int64, nvo3.SubSpecializationId);
                        this.dbServer.AddInParameter(command4, "IsPercentageRate", DbType.Boolean, nvo3.IsPercentageRate);
                        if (nvo3.IsPercentageRate)
                        {
                            this.dbServer.AddInParameter(command4, "PercentageRate", DbType.Decimal, nvo3.SharePercentage);
                        }
                        if (nvo3.IsAmountRate)
                        {
                            this.dbServer.AddInParameter(command4, "AmountRate", DbType.Decimal, nvo3.SharePercentage);
                        }
                        if (nvo3.IsAddition)
                        {
                            nvo3.intOperationType = 1;
                        }
                        else if (nvo3.IsSubtaction)
                        {
                            nvo3.intOperationType = 2;
                        }
                        this.dbServer.AddInParameter(command4, "OperationType", DbType.Int16, nvo3.intOperationType);
                        this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, 1);
                        this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDoctorServiceLinkingByCategory(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO nvo = valueObject as clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                if (nvo.DeletedServiceList == null)
                {
                    nvo.DeletedServiceList = new List<clsServiceMasterVO>();
                }
                if (nvo.IsModify && (nvo.DeletedServiceList.Count > 0))
                {
                    foreach (clsServiceMasterVO rvo in nvo.DeletedServiceList.ToList<clsServiceMasterVO>())
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorServiceLinkingByCategory");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, rvo.DoctorID);
                        this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, rvo.CategoryID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, rvo.ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, rvo.ClassID);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                    }
                }
                if (nvo.ServiceMasterDetailsList == null)
                {
                    nvo.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                }
                if ((nvo.ServiceMasterDetailsList != null) && (nvo.ServiceMasterDetailsList.Count > 0))
                {
                    foreach (clsServiceMasterVO rvo2 in nvo.ServiceMasterDetailsList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorServiceLinkingByCategory");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                        this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.CategoryID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, rvo2.ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "ClassId", DbType.Int64, rvo2.ClassID);
                        this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, rvo2.Specialization);
                        this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, rvo2.SubSpecialization);
                        this.dbServer.AddInParameter(storedProcCommand, "Rate", DbType.Decimal, rvo2.Rate);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                transaction.Commit();
                nvo.SuccessStatus = 0L;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1L;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject DeleteDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsDeleteDoctorDepartmentDetailsBizActionVO nvo = valueObject as clsDeleteDoctorDepartmentDetailsBizActionVO;
            try
            {
                clsDoctorVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorDepartmentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, details.DoctorId);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject DeleteExistingDoctorShareInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsDeleteDoctorShareForOverRideExistingShareVO evo = valueObject as clsDeleteDoctorShareForOverRideExistingShareVO;
            try
            {
                foreach (clsDoctorShareServicesDetailsVO svo in evo.ExistingDoctorShareInfoList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteExistingDoctorShareServices");
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, svo.DoctorId);
                    this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, svo.ModalityID);
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, svo.TariffID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
            }
            catch (Exception)
            {
            }
            return evo;
        }

        public override IValueObject FillDoctorCombo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorListBizActionVO nvo = (clsGetDoctorListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand;
                if (nvo.IsInternal)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllInternalDoctorList");
                    this.dbServer.AddInParameter(storedProcCommand, "IsInternal", DbType.Boolean, true);
                }
                else if (!nvo.IsExternal)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllDoctorList");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllInternalDoctorList");
                    this.dbServer.AddInParameter(storedProcCommand, "IsInternal", DbType.Boolean, false);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["DoctorName"].ToString()));
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

        public override IValueObject GetBulkRateChangeDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetBulkRateChangeDetailsListBizActionVO nvo = valueObject as clsGetBulkRateChangeDetailsListBizActionVO;
            clsTariffMasterBizActionVO item = new clsTariffMasterBizActionVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBulkRateChangeDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "FromEffectiveDate", DbType.Date, nvo.BulkRateChangeDetailsVO.FromEffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToEffectiveDate", DbType.Date, nvo.BulkRateChangeDetailsVO.ToEffectiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "TariffName", DbType.String, nvo.BulkRateChangeDetailsVO.TariffName);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                nvo.TariffDetailsList = new List<clsTariffMasterBizActionVO>();
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsTariffMasterBizActionVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IsApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApplicable"])),
                            EffectiveDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["EffectiveDate"]))),
                            IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddedBy"])),
                            strAddedBy = Convert.ToString(DALHelper.HandleDBNull(reader["strAddedBy"])),
                            BulkRateChangeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeID"])),
                            TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]))
                        };
                        nvo.TariffDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
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

        public override IValueObject GetBulkRateChangeDetailsListByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetBulkRateChangeDetailsListBizActionVO nvo = valueObject as clsGetBulkRateChangeDetailsListBizActionVO;
            clsTariffMasterBizActionVO item = new clsTariffMasterBizActionVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBulkRateChangeDetailsListByID");
                this.dbServer.AddInParameter(storedProcCommand, "BulkRateChangeID", DbType.Int64, nvo.BulkRateChangeDetailsVO.BulkRateChangeID);
                this.dbServer.AddInParameter(storedProcCommand, "BulkRateChangeUnitID", DbType.Int64, nvo.BulkRateChangeDetailsVO.BulkRateChangeUnitID);
                nvo.TariffDetailsList = new List<clsTariffMasterBizActionVO>();
                nvo.SubSpecializationList = new List<clsSubSpecializationVO>();
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsTariffMasterBizActionVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            BulkRateChangeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeID"])),
                            BulkRateChangeUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeUnitID"])),
                            TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]))
                        };
                        nvo.TariffDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    clsSubSpecializationVO nvo3 = new clsSubSpecializationVO();
                    while (reader.Read())
                    {
                        nvo3 = new clsSubSpecializationVO {
                            BulkRateChangeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeID"])),
                            BulkRateChangeUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeUnitID"])),
                            IsSetRateForAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetRateForAll"])),
                            SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecilizationID"])),
                            SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"])),
                            SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationID"])),
                            SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"])),
                            IsPercentageRate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPercentageRate"]))
                        };
                        if (nvo3.IsPercentageRate)
                        {
                            nvo3.SharePercentage = nvo3.ShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PercentageRate"]));
                        }
                        if (!nvo3.IsPercentageRate)
                        {
                            nvo3.SharePercentage = nvo3.ShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["AmountRate"]));
                        }
                        nvo3.intOperationType = Convert.ToInt16(DALHelper.HandleDBNull(reader["OperationType"]));
                        nvo3.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        nvo.SubSpecializationList.Add(nvo3);
                    }
                }
                reader.Close();
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

        public override IValueObject GetClassificationListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetClassificationListForDoctorMasterBizActionVO nvo = (clsGetClassificationListForDoctorMasterBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetClassificationListForDoctorMaster");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorVO item = new clsDoctorVO {
                            UnitClassificationName = reader["Description"].ToString(),
                            ClassificationID = (long) reader["ID"],
                            Status = (bool) reader["Status"]
                        };
                        nvo.DoctorDetails.Add(item);
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

        public override IValueObject GetDepartmentListByUnitID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO nvo = (clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentListByUnitID");
                if (nvo.IsClinical && (nvo.UnitID > 0L))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsClinical", DbType.Boolean, nvo.IsClinical);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterListItem == null)
                    {
                        nvo.MasterListItem = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.MasterListItem.Add(item);
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

        public override IValueObject GetDepartmentListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListForDoctorMasterBizActionVO nvo = (clsGetDepartmentListForDoctorMasterBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentListForDoctorMaster");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorVO item = new clsDoctorVO {
                            DoctorDepartmentDetailID = (long) reader["ID"],
                            UnitName = reader["Unit"].ToString(),
                            UnitID = (long) reader["UnitID"],
                            DepartmentName = reader["Department"].ToString(),
                            DepartmentID = (long) reader["DepartmentID"],
                            DepartmentStatus = (bool) reader["Status"]
                        };
                        nvo.DoctorDetails.Add(item);
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

        public override IValueObject GetDepartmentListForDoctorMasterByDoctorID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO nvo = (clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentListByDoctorID");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorVO item = new clsDoctorVO {
                            DoctorId = (long) DALHelper.HandleDBNull(reader["DoctorID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            DepartmentCode = (string) DALHelper.HandleDBNull(reader["Code"]),
                            DepartmentName = (string) DALHelper.HandleDBNull(reader["Description"]),
                            DepartmentStatus = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.DoctorDetails.Add(item);
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

        public override IValueObject GetDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorAddressInfoBizActionVO nvo = (clsGetDoctorAddressInfoBizActionVO) valueObject;
            try
            {
                clsDoctorAddressInfoVO objDoctorAddressDetail = nvo.objDoctorAddressDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorAddressInfo");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorAddressDetailList == null)
                    {
                        nvo.DoctorAddressDetailList = new List<clsDoctorAddressInfoVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorAddressInfoVO item = new clsDoctorAddressInfoVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            DoctorId = (long) DALHelper.HandleDBNull(reader["DoctorId"]),
                            AddressType = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"])),
                            Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"])),
                            Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"])),
                            Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"])),
                            Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]))
                        };
                        nvo.DoctorAddressDetailList.Add(item);
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

        public override IValueObject GetDoctorAddressInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorAddressInfoByIdVO dvo = (clsGetDoctorAddressInfoByIdVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorAddressInfoById");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, dvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "AddressTypeID", DbType.Int64, dvo.AddressTypeId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dvo.objDoctorAddressDetail = new clsDoctorAddressInfoVO();
                        dvo.objDoctorAddressDetail.DoctorId = (long) DALHelper.HandleDBNull(reader["DoctorId"]);
                        dvo.objDoctorAddressDetail.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"]));
                        dvo.objDoctorAddressDetail.Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        dvo.objDoctorAddressDetail.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        dvo.objDoctorAddressDetail.Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"]));
                        dvo.objDoctorAddressDetail.Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return dvo;
        }

        public override IValueObject GetDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorBankInfoBizActionVO nvo = (clsGetDoctorBankInfoBizActionVO) valueObject;
            try
            {
                clsDoctorBankInfoVO objDoctorBankDetail = nvo.objDoctorBankDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorBankInfo");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorBankDetailList == null)
                    {
                        nvo.DoctorBankDetailList = new List<clsDoctorBankInfoVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorBankInfoVO item = new clsDoctorBankInfoVO {
                            DoctorId = (long) DALHelper.HandleDBNull(reader["DoctorId"]),
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"])),
                            BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"])),
                            BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"])),
                            BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"])),
                            AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"])),
                            BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"])),
                            AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"])),
                            IFSCCode = Convert.ToString(DALHelper.HandleDBNull(reader["IFSCCode"]))
                        };
                        nvo.DoctorBankDetailList.Add(item);
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

        public override IValueObject GetDoctorBankInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorBankInfoByIdVO dvo = (clsGetDoctorBankInfoByIdVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorBankInfoById");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, dvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dvo.objDoctorBankDetail = new clsDoctorBankInfoVO();
                        dvo.objDoctorBankDetail.DoctorId = (long) DALHelper.HandleDBNull(reader["DoctorId"]);
                        dvo.objDoctorBankDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        dvo.objDoctorBankDetail.BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"]));
                        dvo.objDoctorBankDetail.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        dvo.objDoctorBankDetail.BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"]));
                        dvo.objDoctorBankDetail.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        dvo.objDoctorBankDetail.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        dvo.objDoctorBankDetail.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        dvo.objDoctorBankDetail.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        dvo.objDoctorBankDetail.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        dvo.objDoctorBankDetail.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        dvo.objDoctorBankDetail.IFSCCode = Convert.ToString(DALHelper.HandleDBNull(reader["IFSCCode"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return dvo;
        }

        public override IValueObject GetDoctorBillPaymentDetailListByBillID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentDetailListByBillIDBizActionVO nvo = (clsGetDoctorPaymentDetailListByBillIDBizActionVO) valueObject;
            try
            {
                foreach (long num in nvo.BillIdList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorBillPaymentDetailListByBillID");
                    this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int32, num);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int32, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int32, nvo.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, null);
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.DoctorPaymentDetailsList == null)
                        {
                            nvo.DoctorPaymentDetailsList = new List<clsDoctorPaymentVO>();
                        }
                        while (reader.Read())
                        {
                            clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                                TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                                DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                                DoctorShareAmount = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorShareAmount"]))
                            };
                            nvo.DoctorPaymentDetailsList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorBizActionVO nvo = (clsGetDoctorBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoc");
                if (nvo.UnitId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                }
                if (nvo.DepartmentId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentId);
                }
                if (nvo.ClassificationId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ClassificationID", DbType.Int64, nvo.ClassificationId);
                }
                if (nvo.DoctorTypeId > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorTypeID", DbType.Int64, nvo.DoctorTypeId);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                            DoctorID = Convert.ToInt32(DALHelper.HandleDBNull(reader["DoctorID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            ClassificationID = Convert.ToInt32(DALHelper.HandleDBNull(reader["ClassificationID"])),
                            ClassificationName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassificationName"])),
                            DepartmentID = Convert.ToInt32(DALHelper.HandleDBNull(reader["DepartmentID"])),
                            DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"])),
                            DoctorTypeID = Convert.ToInt32(DALHelper.HandleDBNull(reader["DoctorTypeID"])),
                            DoctorTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorTypeName"]))
                        };
                        nvo.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorDetailListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDetailListForDoctorMasterBizActionVO nvo = (clsGetDoctorDetailListForDoctorMasterBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorMasterDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                }
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorVO item = new clsDoctorVO {
                            DoctorId = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            DOB = DALHelper.HandleDate(reader["DOB"]),
                            Education = (string) DALHelper.HandleDBNull(reader["Education"]),
                            Experience = (string) DALHelper.HandleDBNull(reader["Experience"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationID"]),
                            SpecializationDis = (string) DALHelper.HandleDBNull(reader["Specialization"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationID"]),
                            SubSpecializationDis = (string) DALHelper.HandleDBNull(reader["SubSpecialization"]),
                            DoctorType = (long) DALHelper.HandleDBNull(reader["DoctorTypeID"]),
                            DoctorTypeDis = (string) DALHelper.HandleDBNull(reader["DoctorType"]),
                            DepartmentStatus = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            DoctorCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocCategoryId"])),
                            IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"]))
                        };
                        nvo.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorDetailListForDoctorMasterByDoctorID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDetailListForDoctorMasterByIDBizActionVO nvo = (clsGetDoctorDetailListForDoctorMasterByIDBizActionVO) valueObject;
            try
            {
                clsDoctorVO doctorDetails = nvo.DoctorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorMasterDetailsListByDoctorID");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.DoctorDetails == null)
                        {
                            nvo.DoctorDetails = new clsDoctorVO();
                        }
                        nvo.DoctorDetails.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.DoctorDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.DoctorDetails.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        nvo.DoctorDetails.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        nvo.DoctorDetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        nvo.DoctorDetails.DOB = DALHelper.HandleDate(reader["DOB"]);
                        nvo.DoctorDetails.DateofJoining = DALHelper.HandleDate(reader["DateOfJoining"]);
                        nvo.DoctorDetails.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        nvo.DoctorDetails.Experience = Convert.ToString(DALHelper.HandleDBNull(reader["Experience"]));
                        nvo.DoctorDetails.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        nvo.DoctorDetails.SpecializationDis = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        nvo.DoctorDetails.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationID"]));
                        nvo.DoctorDetails.SubSpecializationDis = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        nvo.DoctorDetails.DoctorType = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorTypeID"]));
                        nvo.DoctorDetails.DoctorTypeDis = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorType"]));
                        nvo.DoctorDetails.DoctorCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocCategoryId"]));
                        nvo.DoctorDetails.DoctorCategoryDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCategory"]));
                        nvo.DoctorDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.DoctorDetails.GenderId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderId"]));
                        nvo.DoctorDetails.Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]);
                        nvo.DoctorDetails.Signature = (DALHelper.HandleDBNull(reader["DigitalSignature"]) == null) ? null : ((byte[]) DALHelper.HandleDBNull(reader["DigitalSignature"]));
                        nvo.DoctorDetails.MaritalStatusId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        nvo.DoctorDetails.ProvidentFund = Convert.ToString(DALHelper.HandleDBNull(reader["PFNumber"]));
                        nvo.DoctorDetails.PermanentAccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PANNumber"]));
                        nvo.DoctorDetails.AccessCardNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccessCardNumber"]));
                        nvo.DoctorDetails.RegistrationNumber = Convert.ToString(DALHelper.HandleDBNull(reader["RegestrationNumber"]));
                        nvo.DoctorDetails.DateofJoining = DALHelper.HandleDate(reader["DateOfJoining"]);
                        nvo.DoctorDetails.EmailId = (string) DALHelper.HandleDBNull(reader["EmailId"]);
                        nvo.DoctorDetails.EmployeeNumber = Convert.ToString(DALHelper.HandleDBNull(reader["EmployeeNumber"]));
                        nvo.DoctorDetails.DoctorBankInformation.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        nvo.DoctorDetails.DoctorBankInformation.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        nvo.DoctorDetails.DoctorBankInformation.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        nvo.DoctorDetails.DoctorBankInformation.AccountTypeId = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AccountType"]));
                        nvo.DoctorDetails.DoctorBankInformation.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        nvo.DoctorDetails.DoctorBankInformation.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        nvo.DoctorDetails.MarketingExecutivesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MarketingExecutivesID"]));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.DoctorDetails.UnitDepartmentDetails = new List<clsUnitDepartmentsDetailsVO>();
                    while (reader.Read())
                    {
                        clsUnitDepartmentsDetailsVO item = new clsUnitDepartmentsDetailsVO {
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DeptMasterId"]),
                            DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["Department"])),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitMasterId"]),
                            Status = (DALHelper.HandleDBNull(reader["DoctorDepartmentstatus"]) != null) && ((bool) DALHelper.HandleDBNull(reader["DoctorDepartmentstatus"])),
                            UnitName = (string) DALHelper.HandleDBNull(reader["Unit"])
                        };
                        nvo.DoctorDetails.UnitDepartmentDetails.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.DoctorDetails.UnitClassificationDetailsList = new List<clsUnitClassificationsDetailsVO>();
                    while (reader.Read())
                    {
                        clsUnitClassificationsDetailsVO item = new clsUnitClassificationsDetailsVO {
                            ClassificationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassificationID"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            IsAvailableStr = Convert.ToString(DALHelper.HandleDBNull(reader["IsAvailable"]))
                        };
                        item.IsAvailable = item.IsAvailableStr == "true";
                        nvo.DoctorDetails.UnitClassificationDetailsList.Add(item);
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

        public override IValueObject GetDoctorList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorListBizActionVO nvo = (clsGetDoctorListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentId", DbType.Int64, nvo.DepartmentId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetailsList == null)
                    {
                        nvo.DoctorDetailsList = new List<clsDoctorVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorVO item = new clsDoctorVO {
                            DoctorId = (long) reader["DoctorId"],
                            DoctorName = reader["DoctorName"].ToString()
                        };
                        nvo.DoctorDetailsList.Add(item);
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

        public override IValueObject GetDoctorPaymentBillsDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentListBizActionVO nvo = (clsGetDoctorPaymentListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentBills");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                            BillID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BillID"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"])),
                            ScanNo = Convert.ToString(DALHelper.HandleDBNull(reader["ScanNO"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]))
                        };
                        nvo.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorPaymentChildDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentChildBizActionVO nvo = (clsGetDoctorPaymentChildBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorBillDetails");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "TariffServiceID", DbType.Int64, nvo.DoctorInfo.TariffServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "BillNumber", DbType.String, nvo.DoctorInfo.BillNo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorInfoList == null)
                    {
                        nvo.DoctorInfoList = new List<clsDoctorPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                            BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            TariffID = Convert.ToInt32(DALHelper.HandleDBNull(reader["TariffId"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"])),
                            DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNumber"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))
                        };
                        nvo.DoctorInfoList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentAllBizActionVO nvo = (clsGetDoctorPaymentAllBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorPayment");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            PaymentDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["PaymentDate"]))),
                            PaymentNo = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentNo"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"])),
                            BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]))
                        };
                        nvo.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorPaymentDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentShareDetailsBizActionVO nvo = (clsGetDoctorPaymentShareDetailsBizActionVO) valueObject;
            try
            {
                if (nvo.IsForAllData)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentShareDetailsForAllBill");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.String, nvo.DoctorIds);
                    this.dbServer.AddInParameter(storedProcCommand, "IsForBoth", DbType.Boolean, nvo.IsForBoth);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.DoctorInfoList == null)
                        {
                            nvo.DoctorInfoList = new List<clsDoctorPaymentVO>();
                        }
                        while (reader.Read())
                        {
                            clsDoctorPaymentVO tvo = new clsDoctorPaymentVO {
                                BillUnitIDs = Convert.ToString(DALHelper.HandleDBNull(reader["BIllUnitID"])),
                                BillIDs = Convert.ToString(DALHelper.HandleDBNull(reader["BIllID"])),
                                TotalBillShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillShareAmount"]))
                            };
                            nvo.DoctorInfo = tvo;
                        }
                    }
                    reader.NextResult();
                    reader.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_NewGetDoctorPaymentShareDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.String, nvo.DoctorIds);
                    this.dbServer.AddInParameter(storedProcCommand, "IsSettleBill", DbType.Boolean, nvo.IsSettleBill);
                    this.dbServer.AddInParameter(storedProcCommand, "IsCreditBill", DbType.Boolean, nvo.IsCreditBill);
                    this.dbServer.AddInParameter(storedProcCommand, "IsForBoth", DbType.Boolean, nvo.IsForBoth);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.DoctorInfoList == null)
                        {
                            nvo.DoctorInfoList = new List<clsDoctorPaymentVO>();
                        }
                        while (reader2.Read())
                        {
                            clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                                DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferredDoctor"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader2["PatientName"])),
                                DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DoctorID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitId"])),
                                BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["BillID"])),
                                BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["BillUnitID"])),
                                VisitDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader2["VisitDate"])),
                                BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader2["BillDate"])),
                                BillNo = Convert.ToString(DALHelper.HandleDBNull(reader2["BillNo"])),
                                TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["TotalBillAmount"])),
                                TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["TotalConcessionAmount"])),
                                TotalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["TotalShareAmount"])),
                                NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["NetBillAmount"])),
                                SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["SelfAmount"])),
                                UnitName = Convert.ToString(DALHelper.HandleDBNull(reader2["UnitName"]))
                            };
                            nvo.DoctorInfoList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader2.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorPaymentFrontDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentDetailListBizActionVO nvo = (clsGetDoctorPaymentDetailListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentDetail");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorInfo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.DoctorInfo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.DoctorInfo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PaidUnpaid", DbType.Boolean, nvo.DoctorInfo.IsPaymentDone);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            DoctorPaymentID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["Id"])),
                            DoctorPaymentUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            PaymentDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["PaymentDateTime"]))),
                            PaymentNo = Convert.ToString(DALHelper.HandleDBNull(reader["VoucherNo"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorPaidAmount"])),
                            BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorBalanceAmount"]))
                        };
                        nvo.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorPaymentList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentBizActionVO nvo = (clsGetDoctorPaymentBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentList");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            PaymentDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["PaymentDate"]))),
                            PaymentNo = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentNo"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"])),
                            PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"])),
                            BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"])),
                            BillID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BillID"]))
                        };
                        nvo.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorSchedule(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorScheduleBizActionVO nvo = valueObject as clsGetDoctorScheduleBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentId", DbType.Int64, nvo.DepartmentId);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsDoctorVO doctorDetails = nvo.DoctorDetails;
                        nvo.DoctorDetails.DoctorId = (long) reader["DoctorId"];
                        nvo.DoctorDetails.DoctorName = reader["DoctorName"].ToString();
                        nvo.DoctorDetails.DepartmentName = reader["DepartmentName"].ToString();
                        nvo.DoctorDetails.UnitName = reader["Unit"].ToString();
                        nvo.DoctorDetails.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.DoctorDetails.FromTime = TimeSpan.Parse((((string) DALHelper.HandleDBNull(reader["FromTime"])) == null) ? "00:00:00" : ((string) DALHelper.HandleDBNull(reader["FromTime"])));
                        nvo.DoctorDetails.ToTime = TimeSpan.Parse((((string) DALHelper.HandleDBNull(reader["ToTime"])) == null) ? "00:00:00" : ((string) DALHelper.HandleDBNull(reader["ToTime"])));
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

        public override IValueObject GetDoctorServiceLinkingByCategoryId(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDoctorServiceLinkingByCategoryBizActionVO nvo = valueObject as clsGetDoctorServiceLinkingByCategoryBizActionVO;
            clsServiceMasterVO item = new clsServiceMasterVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceLinkingByCategoryId");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.ServiceMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.ServiceMasterDetails.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationId", DbType.Int64, nvo.ServiceMasterDetails.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, nvo.ServiceMasterDetails.SubSpecialization);
                if ((nvo.ServiceMasterDetails.ServiceName != null) && (nvo.ServiceMasterDetails.ServiceName.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceMasterDetails.ServiceName);
                }
                nvo.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])) + " " + Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                            SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]))
                        };
                        nvo.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        item.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.ServiceMasterDetailsList.Add(item);
                    }
                }
                reader.Close();
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

        public override IValueObject GetDoctorServiceLinkingByClinic(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDoctorServiceLinkingByCategoryBizActionVO nvo = valueObject as clsGetDoctorServiceLinkingByCategoryBizActionVO;
            clsServiceMasterVO item = new clsServiceMasterVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceLinkingByClinic");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.ServiceMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ServiceMasterDetails.ClassID);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.ServiceMasterDetails.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationId", DbType.Int64, nvo.ServiceMasterDetails.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationId", DbType.Int64, nvo.ServiceMasterDetails.SubSpecialization);
                if ((nvo.ServiceMasterDetails.ServiceName != null) && (nvo.ServiceMasterDetails.ServiceName.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceMasterDetails.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, null);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                nvo.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))
                        };
                        nvo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        item.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])) + " " + Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        item.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        item.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        item.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        item.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        item.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        item.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        item.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                        item.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        item.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));
                        nvo.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        item.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.ServiceMasterDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
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

        public override IValueObject GetDoctorServiceLinkingByDoctorId(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO nvo = valueObject as clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO;
            clsServiceMasterVO rvo = new clsServiceMasterVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceLinkingRateByDoctorID");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.ServiceMasterDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceMasterDetails.ServiceID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsServiceMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])) + " " + Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"])),
                            ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])),
                            Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                            SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"])),
                            SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                            SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]))
                        };
                        nvo.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        rvo.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        rvo.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        rvo.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.ServiceMasterDetails = rvo;
                    }
                }
                reader.Close();
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

        public override IValueObject GetDoctorShareAmount(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorShareAmountBizActionVO nvo = (clsGetDoctorShareAmountBizActionVO) valueObject;
            try
            {
                foreach (long num in nvo.BillIdList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDocShare");
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Double, nvo.DoctorId);
                    this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Double, num);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, 0);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, null);
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.DoctorDetails == null)
                        {
                            nvo.DoctorDetails = new List<clsDoctorPaymentVO>();
                        }
                        while (reader.Read())
                        {
                            clsDoctorPaymentVO item = new clsDoctorPaymentVO();
                            item.DoctorShareAmount += Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            item.Rate += Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            item.DoctorSharePercentage += Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            nvo.DoctorDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorShareRangeList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorShareRangeList list = (clsGetDoctorShareRangeList) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorShareRangeDetailList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (list.DoctorShareRangeList == null)
                    {
                        list.DoctorShareRangeList = new List<clsDoctorShareRangeList>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorShareRangeList item = new clsDoctorShareRangeList {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            LowerLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LowerLimit"])),
                            UpperLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["UpperLimit"])),
                            SharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SharePercentage"])),
                            ShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ShareAmount"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        list.DoctorShareRangeList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }

        public override IValueObject GetDoctorShares1DetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorShare1DetailsBizActionVO nvo = valueObject as clsGetDoctorShare1DetailsBizActionVO;
            try
            {
                if (nvo.FromDoctorShareChildWindow)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_NewGetDoctorShares1DetailsListForChildWindow");
                    long specID = nvo.SpecID;
                    this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecID);
                    long subSpecID = nvo.SubSpecID;
                    this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, nvo.SubSpecID);
                    long tariffID = nvo.TariffID;
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                    long modalityID = nvo.ModalityID;
                    this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, nvo.ModalityID);
                    long doctorId = nvo.DoctorId;
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorId);
                    this.dbServer.AddInParameter(storedProcCommand, "Service", DbType.String, nvo.ServiceName + "%");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.DoctorShareInfoGetList == null)
                        {
                            nvo.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
                        }
                        while (reader.Read())
                        {
                            clsDoctorShareServicesDetailsVO item = new clsDoctorShareServicesDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModalityID"])),
                                Modality = Convert.ToString(DALHelper.HandleDBNull(reader["modality"])),
                                DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                                ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                                ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Service"])),
                                TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                                TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["Tariff"])),
                                SpecializationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"])),
                                SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"])),
                                SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"])),
                                SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"])),
                                DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"])),
                                DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"])),
                                ServiceRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceRate"]))
                            };
                            nvo.DoctorShareInfoGetList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader.Close();
                }
                else if (nvo.ForAllDoctorShareRecord)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllDoctorShares1DetailsList");
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.DoctorShareInfoGetList == null)
                        {
                            nvo.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
                        }
                        while (reader2.Read())
                        {
                            clsDoctorShareServicesDetailsVO item = new clsDoctorShareServicesDetailsVO {
                                DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DoctorId"])),
                                ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ModalityID"])),
                                TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["TariffID"])),
                                SpecializationID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["SpecializationId"])),
                                SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader2["SubSpecializationId"]))
                            };
                            nvo.DoctorShareInfoGetList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    reader2.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_NewGetDoctorShares1DetailsList");
                    long specID = nvo.SpecID;
                    this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecID);
                    long subSpecID = nvo.SubSpecID;
                    this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, nvo.SubSpecID);
                    long tariffID = nvo.TariffID;
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                    long modalityID = nvo.ModalityID;
                    this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, nvo.ModalityID);
                    if (nvo.DocIds != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DocIds", DbType.String, nvo.DocIds);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader3.HasRows)
                    {
                        if (nvo.DoctorShareInfoGetList == null)
                        {
                            nvo.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
                        }
                        while (reader3.Read())
                        {
                            clsDoctorShareServicesDetailsVO item = new clsDoctorShareServicesDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ID"])),
                                DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader3["DoctorId"])),
                                Modality = Convert.ToString(DALHelper.HandleDBNull(reader3["Modality"])),
                                ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ModalityID"])),
                                DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader3["DoctorName"])),
                                TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["TariffID"])),
                                TariffName = Convert.ToString(DALHelper.HandleDBNull(reader3["TariffName"])),
                                SpecializationID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["SpecializationID"])),
                                SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader3["SubSpecializationId"])),
                                SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader3["Specialization"])),
                                SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader3["SubSpecialization"]))
                            };
                            nvo.DoctorShareInfoGetList.Add(item);
                        }
                    }
                    reader3.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader3.Close();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetDoctorTariffServiceDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorTariffServiceListBizActionVO nvo = (clsGetDoctorTariffServiceListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffServiceMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TeriffServiceDetail.TariffID);
                if (nvo.TeriffServiceDetail.TariffID > 0L)
                {
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.TeriffServiceDetailList == null)
                        {
                            nvo.TeriffServiceDetailList = new List<clsDoctorWaiverDetailVO>();
                        }
                        while (reader.Read())
                        {
                            clsDoctorWaiverDetailVO item = new clsDoctorWaiverDetailVO {
                                TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]),
                                ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceId"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                                Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]))
                            };
                            nvo.TeriffServiceDetailList.Add(item);
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

        public override IValueObject GetDoctorWaiverDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorWaiverDetailListBizActionVO nvo = (clsGetDoctorWaiverDetailListBizActionVO) valueObject;
            if (nvo.PageName == "Doctor Waiver")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorWaiverDetailsList");
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    if (nvo.DepartmentID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                    }
                    if (nvo.DoctorID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                    }
                    if (nvo.TariffID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                    }
                    if (nvo.ServiceID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.DoctorWaiverDetails == null)
                        {
                            nvo.DoctorWaiverDetails = new List<clsDoctorWaiverDetailVO>();
                        }
                        while (reader.Read())
                        {
                            clsDoctorWaiverDetailVO item = new clsDoctorWaiverDetailVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                                DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                                DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"])),
                                DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"])),
                                DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"])),
                                EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorShareAmount"])),
                                EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorSharePercentage"])),
                                TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"])),
                                TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"])),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                                ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                                Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"])),
                                EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyServiceRate"])),
                                WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["WaiverDays"]))
                            };
                            nvo.DoctorWaiverDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.PageName != "Department Waiver")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCenterWaiverDetailsList");
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    if (nvo.DepartmentID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                    }
                    if (nvo.DoctorID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                    }
                    if (nvo.TariffID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                    }
                    if (nvo.ServiceID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader3.HasRows)
                    {
                        if (nvo.DoctorWaiverDetails == null)
                        {
                            nvo.DoctorWaiverDetails = new List<clsDoctorWaiverDetailVO>();
                        }
                        while (reader3.Read())
                        {
                            clsDoctorWaiverDetailVO item = new clsDoctorWaiverDetailVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["UnitID"])),
                                UnitName = Convert.ToString(DALHelper.HandleDBNull(reader3["UnitName"])),
                                DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["DoctorShareAmount"])),
                                DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["DoctorSharePercentage"])),
                                EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["EmergencyDoctorShareAmount"])),
                                EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["EmergencyDoctorSharePercentage"])),
                                TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["TariffID"])),
                                TariffName = Convert.ToString(DALHelper.HandleDBNull(reader3["TariffName"])),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ServiceID"])),
                                ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader3["ServiceName"])),
                                Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["ServiceRate"])),
                                EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["EmergencyServiceRate"])),
                                WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader3["WaiverDays"]))
                            };
                            nvo.DoctorWaiverDetails.Add(item);
                        }
                    }
                    reader3.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader3.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentWaiverDetailsList");
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    if (nvo.DepartmentID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                    }
                    if (nvo.DoctorID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                    }
                    if (nvo.TariffID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                    }
                    if (nvo.ServiceID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.DoctorWaiverDetails == null)
                        {
                            nvo.DoctorWaiverDetails = new List<clsDoctorWaiverDetailVO>();
                        }
                        while (reader2.Read())
                        {
                            clsDoctorWaiverDetailVO item = new clsDoctorWaiverDetailVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ID"])),
                                DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader2["DepartmentName"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"])),
                                DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DepartmentID"])),
                                DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["DoctorShareAmount"])),
                                DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["DoctorSharePercentage"])),
                                EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["EmergencyDoctorShareAmount"])),
                                EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["EmergencyDoctorSharePercentage"])),
                                TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["TariffID"])),
                                TariffName = Convert.ToString(DALHelper.HandleDBNull(reader2["TariffName"])),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ServiceID"])),
                                ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader2["ServiceName"])),
                                Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["ServiceRate"])),
                                EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["EmergencyServiceRate"])),
                                WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader2["WaiverDays"]))
                            };
                            nvo.DoctorWaiverDetails.Add(item);
                        }
                    }
                    reader2.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    reader2.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return nvo;
        }

        public override IValueObject GetDoctorWaiverDetailListByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorWaiverDetailListByIDBizActionVO nvo = (clsGetDoctorWaiverDetailListByIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorWaiverDetailByID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.DoctorWaiverDetails.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorWaiverDetailsList == null)
                    {
                        nvo.DoctorWaiverDetailsList = new List<clsDoctorWaiverDetailVO>();
                    }
                    while (reader.Read())
                    {
                        nvo.DoctorWaiverDetails.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        nvo.DoctorWaiverDetails.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        nvo.DoctorWaiverDetails.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        nvo.DoctorWaiverDetails.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        nvo.DoctorWaiverDetails.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"]));
                        nvo.DoctorWaiverDetails.WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["WaiverDays"]));
                        nvo.DoctorWaiverDetails.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        nvo.DoctorWaiverDetails.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        nvo.DoctorWaiverDetails.EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorShareAmount"]));
                        nvo.DoctorWaiverDetails.EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorSharePerCentage"]));
                        nvo.DoctorWaiverDetails.EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyServiceRate"]));
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

        public override IValueObject GetExistingDoctorList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetExistingDoctorShareDetails details = (clsGetExistingDoctorShareDetails) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetExistingDoctorMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "DOCTORID", DbType.String, details.DoctorIDs);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (details.DoctorList == null)
                    {
                        details.DoctorList = new List<clsDoctorShareServicesDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorShareServicesDetailsVO item = new clsDoctorShareServicesDetailsVO {
                            DoctorId = DALHelper.HandleIntegerNull(reader["DoctorID"]),
                            ModalityID = DALHelper.HandleIntegerNull(reader["ModalityID"]),
                            TariffID = DALHelper.HandleIntegerNull(reader["TariffID"])
                        };
                        details.DoctorList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return details;
        }

        public override IValueObject GetPaidDoctorPaymentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPaidDoctorPaymentDetailsBizActionVO nvo = (clsGetPaidDoctorPaymentDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPaidDoctorPaymentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorPaymentID", DbType.Int64, nvo.DoctorPaymentId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorDetails == null)
                    {
                        nvo.DoctorDetails = new List<clsDoctorPaymentVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO item = new clsDoctorPaymentVO {
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillUnitID"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            TotalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalShareAmount"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]))
                        };
                        nvo.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPendingDoctorDetail(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPendingDoctorDetails details = (clsGetPendingDoctorDetails) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPendingDoctorMasterDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if ((details.FirstName != null) && (details.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, details.FirstName + "%");
                }
                if ((details.MiddleName != null) && (details.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, details.MiddleName + "%");
                }
                if ((details.LastName != null) && (details.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, details.LastName + "%");
                }
                if ((details.SpecializationID != null) && (details.SpecializationID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.String, details.SpecializationID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.String, null);
                }
                if ((details.SubSpecializationID != null) && (details.SubSpecializationID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.String, details.SubSpecializationID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, details.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, details.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, details.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (details.DoctorDetails == null)
                    {
                        details.DoctorDetails = new List<clsDoctorVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorVO item = new clsDoctorVO {
                            DoctorId = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            DOB = DALHelper.HandleDate(reader["DOB"]),
                            Education = (string) DALHelper.HandleDBNull(reader["Education"]),
                            Experience = (string) DALHelper.HandleDBNull(reader["Experience"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationID"]),
                            SpecializationDis = (string) DALHelper.HandleDBNull(reader["Specialization"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationID"]),
                            SubSpecializationDis = (string) DALHelper.HandleDBNull(reader["SubSpecialization"]),
                            DoctorType = (long) DALHelper.HandleDBNull(reader["DoctorTypeID"]),
                            DoctorTypeDis = (string) DALHelper.HandleDBNull(reader["DoctorType"]),
                            DepartmentStatus = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        details.DoctorDetails.Add(item);
                    }
                }
                reader.NextResult();
                details.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return details;
        }

        public override IValueObject SaveDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsSaveDoctorPaymentDetailsBizActionVO nvo = (clsSaveDoctorPaymentDetailsBizActionVO) valueObject;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_InsertDoctorPayment");
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
            this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
            this.dbServer.AddInParameter(storedProcCommand, "DoctorServiceLinkingTypeID", DbType.Int64, nvo.DoctorServiceLinkingTypeID);
            this.dbServer.AddInParameter(storedProcCommand, "PaymentDateTime", DbType.DateTime, nvo.PaymentDate);
            this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, nvo.TotalAmount);
            this.dbServer.AddInParameter(storedProcCommand, "DoctorPaidAmount", DbType.Double, nvo.DoctorPaidAmount);
            this.dbServer.AddInParameter(storedProcCommand, "DoctorBalanceAmount", DbType.Double, nvo.BalanceAmount);
            this.dbServer.AddInParameter(storedProcCommand, "IsPaymentdone", DbType.Byte, 1);
            this.dbServer.AddInParameter(storedProcCommand, "BillIDs", DbType.String, nvo.DoctorInfo.BillIDs);
            this.dbServer.AddInParameter(storedProcCommand, "BillUnitIDs", DbType.String, nvo.DoctorInfo.BillUnitIDs);
            this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
            this.dbServer.AddParameter(storedProcCommand, "VoucherNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
            this.dbServer.AddParameter(storedProcCommand, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 1);
            this.dbServer.ExecuteNonQuery(storedProcCommand);
            nvo.DoctorInfo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "Id");
            nvo.DoctorInfo.PaymentNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "VoucherNo");
            if (nvo.PaymentDetailDetails.Count > 0)
            {
                foreach (clsPaymentDetailsVO svo in nvo.PaymentDetailDetails)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_InsertDoctorPaymentModeDetail");
                    this.dbServer.AddInParameter(command, "DoctorPaymentID", DbType.Int64, nvo.DoctorInfo.ID);
                    this.dbServer.AddInParameter(command, "DoctorPaymentMode", DbType.Int64, svo.PaymentModeID);
                    this.dbServer.AddInParameter(command, "BankID", DbType.Int64, svo.BankID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, svo.Date);
                    this.dbServer.AddInParameter(command, "AccountNo", DbType.String, svo.Number);
                    this.dbServer.AddInParameter(command, "PaidAmount", DbType.Double, svo.PaidAmount);
                    this.dbServer.ExecuteNonQuery(command);
                }
            }
            return nvo;
        }

        public override IValueObject SaveDoctorSettlePaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            return valueObject;
        }

        private clsAddDoctorMasterBizActionVO UpdateDcotorMaster(clsAddDoctorMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsDoctorVO doctorDetails = BizActionObj.DoctorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, doctorDetails.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, doctorDetails.FirstName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, doctorDetails.MiddleName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, doctorDetails.LastName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, doctorDetails.DOB);
                this.dbServer.AddInParameter(storedProcCommand, "Education", DbType.String, doctorDetails.Education.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Experience", DbType.String, doctorDetails.Experience.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, doctorDetails.Specialization);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, doctorDetails.SubSpecialization);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorTypeID", DbType.Int64, doctorDetails.DoctorType);
                this.dbServer.AddInParameter(storedProcCommand, "EmailId", DbType.String, doctorDetails.EmailId);
                this.dbServer.AddInParameter(storedProcCommand, "GenderId", DbType.Int64, doctorDetails.GenderId);
                this.dbServer.AddInParameter(storedProcCommand, "Photo", DbType.Binary, doctorDetails.Photo);
                this.dbServer.AddInParameter(storedProcCommand, "DigitalSignature", DbType.Binary, doctorDetails.Signature);
                this.dbServer.AddInParameter(storedProcCommand, "MaritalStatusId", DbType.Int64, doctorDetails.MaritalStatusId);
                this.dbServer.AddInParameter(storedProcCommand, "ProvidentFund", DbType.String, doctorDetails.ProvidentFund);
                this.dbServer.AddInParameter(storedProcCommand, "PermanentAccountNumber", DbType.String, doctorDetails.PermanentAccountNumber);
                this.dbServer.AddInParameter(storedProcCommand, "AccessCardNumber", DbType.String, doctorDetails.AccessCardNumber);
                this.dbServer.AddInParameter(storedProcCommand, "RegistrationNumber", DbType.String, doctorDetails.RegistrationNumber);
                this.dbServer.AddInParameter(storedProcCommand, "DateofJoining", DbType.DateTime, doctorDetails.DateofJoining);
                this.dbServer.AddInParameter(storedProcCommand, "EmployeeNumber", DbType.String, doctorDetails.EmployeeNumber);
                this.dbServer.AddInParameter(storedProcCommand, "DocCategoryId", DbType.Int64, doctorDetails.DoctorCategoryId);
                this.dbServer.AddInParameter(storedProcCommand, "MarketingExecutivesID", DbType.Int64, doctorDetails.MarketingExecutivesID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, doctorDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (BizActionObj.SuccessStatus == 0)
                {
                    if ((doctorDetails.UnitDepartmentDetails != null) && (doctorDetails.UnitDepartmentDetails.Count > 0))
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorDepartmentDetails");
                        this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, doctorDetails.DoctorId);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                    foreach (clsUnitDepartmentsDetailsVO svo in doctorDetails.UnitDepartmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorDepartmentDetails");
                        this.dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, doctorDetails.DoctorId);
                        this.dbServer.AddInParameter(command3, "DepartmentID", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, svo.UnitID);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, svo.Status);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.ExecuteNonQuery(command3);
                        svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                    }
                    if ((doctorDetails.UnitClassificationDetailsList != null) && (doctorDetails.UnitClassificationDetailsList.Count > 0))
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorClassificationDetails");
                        this.dbServer.AddInParameter(command4, "DoctorID", DbType.Int64, doctorDetails.DoctorId);
                        this.dbServer.ExecuteNonQuery(command4);
                    }
                    foreach (clsUnitClassificationsDetailsVO svo2 in doctorDetails.UnitClassificationDetailsList)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorClassificationDetails");
                        this.dbServer.AddInParameter(command5, "DoctorID", DbType.Int64, doctorDetails.DoctorId);
                        this.dbServer.AddInParameter(command5, "ClassificationID", DbType.Int64, svo2.ClassificationID);
                        this.dbServer.ExecuteNonQuery(command5);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateDoctorAddressInfoVO ovo = (clsUpdateDoctorAddressInfoVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorAddressDetail");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, ovo.objDoctorAddressDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, ovo.objDoctorAddressDetail.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "AddressTypeID", DbType.Int64, ovo.objDoctorAddressDetail.AddressTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, ovo.objDoctorAddressDetail.Name);
                this.dbServer.AddInParameter(storedProcCommand, "Address", DbType.String, ovo.objDoctorAddressDetail.Address);
                this.dbServer.AddInParameter(storedProcCommand, "Contact1", DbType.String, ovo.objDoctorAddressDetail.Contact1);
                this.dbServer.AddInParameter(storedProcCommand, "Contact2", DbType.String, ovo.objDoctorAddressDetail.Contact2);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, ovo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                ovo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return ovo;
        }

        public override IValueObject UpdateDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateDoctorBankInfoVO ovo = (clsUpdateDoctorBankInfoVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorBankDetail");
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, ovo.objDoctorBankDetail.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, ovo.objDoctorBankDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "BankId", DbType.Int64, ovo.objDoctorBankDetail.BankId);
                this.dbServer.AddInParameter(storedProcCommand, "BranchId", DbType.Int64, ovo.objDoctorBankDetail.BranchId);
                this.dbServer.AddInParameter(storedProcCommand, "AccountNumber", DbType.String, ovo.objDoctorBankDetail.AccountNumber);
                this.dbServer.AddInParameter(storedProcCommand, "AccountType", DbType.Boolean, ovo.objDoctorBankDetail.AccountType);
                this.dbServer.AddInParameter(storedProcCommand, "BranchAddress", DbType.String, ovo.objDoctorBankDetail.BranchAddress);
                this.dbServer.AddInParameter(storedProcCommand, "MICRNumber", DbType.Int64, ovo.objDoctorBankDetail.MICRNumber);
                this.dbServer.AddInParameter(storedProcCommand, "IFSCCode", DbType.String, ovo.objDoctorBankDetail.IFSCCode);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return ovo;
        }

        private clsAddDoctorDepartmentDetailsBizActionVO UpdateDoctorDepartmentDetails(clsAddDoctorDepartmentDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsDoctorVO departmentDetails = BizActionObj.DepartmentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorDepartmentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, departmentDetails.DoctorDepartmentDetailID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, departmentDetails.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, departmentDetails.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject UpdateDoctorShareInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateDoctorShareServiceBizActionVO nvo = valueObject as clsUpdateDoctorShareServiceBizActionVO;
            try
            {
                foreach (clsDoctorShareServicesDetailsVO svo in nvo.objServiceList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateShareDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, svo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, svo.ServiceId);
                    this.dbServer.AddInParameter(storedProcCommand, "ModalityID", DbType.Int64, svo.ModalityID);
                    this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, svo.TariffID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, svo.DoctorId);
                    this.dbServer.AddInParameter(storedProcCommand, "SharePercentage", DbType.Decimal, svo.DoctorSharePercentage);
                    this.dbServer.AddInParameter(storedProcCommand, "ShareAmount", DbType.Decimal, svo.DoctorShareAmount);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateDoctorWaiverInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateDoctorWaiverDetailsBizActionVO nvo = valueObject as clsUpdateDoctorWaiverDetailsBizActionVO;
            try
            {
                clsDoctorWaiverDetailVO objDoctorWaiverDetail = nvo.objDoctorWaiverDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorWaiverDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objDoctorWaiverDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objDoctorWaiverDetail.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, objDoctorWaiverDetail.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, objDoctorWaiverDetail.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, objDoctorWaiverDetail.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, objDoctorWaiverDetail.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "WaiverDays", DbType.Int64, objDoctorWaiverDetail.WaiverDays);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceRate", DbType.Decimal, objDoctorWaiverDetail.Rate);
                this.dbServer.AddInParameter(storedProcCommand, "EmergencyServiceRate", DbType.Decimal, objDoctorWaiverDetail.EmergencyRate);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorSharePercentage", DbType.Decimal, objDoctorWaiverDetail.DoctorSharePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "EmergencyDoctorSharePercentage", DbType.Decimal, objDoctorWaiverDetail.EmergencyDoctorSharePercentage);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorShareAmount", DbType.Decimal, objDoctorWaiverDetail.DoctorShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "EmergencyDoctorShareAmount", DbType.Decimal, objDoctorWaiverDetail.EmergencyDoctorShareAmount);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, objDoctorWaiverDetail.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

