namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Master.CompanyPayment;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsCompanyDAL : clsBaseCompanyPaymentDAL
    {
        private Database dbServer;

        private clsCompanyDAL()
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

        public override IValueObject AddCompanyPaymentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddCompanyPaymentDetailsBizActionVO nvo = valueObject as clsAddCompanyPaymentDetailsBizActionVO;
            if (nvo.IsCompanyForm)
            {
                try
                {
                    clsCompanyPaymentDetailsVO companyPaymentDetails = nvo.CompanyPaymentDetails;
                    foreach (clsCompanyPaymentDetailsVO svo2 in nvo.CompanyPaymentInfoList)
                    {
                        if (svo2.IsSelected)
                        {
                            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyPaymentDetails");
                            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, companyPaymentDetails.UnitID);
                            this.dbServer.AddInParameter(storedProcCommand, "Billno", DbType.Int64, svo2.BillNo);
                            this.dbServer.AddInParameter(storedProcCommand, "AssCompanyID", DbType.Int64, svo2.AssCompanyID);
                            this.dbServer.AddInParameter(storedProcCommand, "BillDate", DbType.DateTime, svo2.BillDate);
                            this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, svo2.ServiceId);
                            this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.String, svo2.CompanyID);
                            this.dbServer.AddInParameter(storedProcCommand, "CalSharePer", DbType.Decimal, svo2.DoctorSharePercentage);
                            this.dbServer.AddInParameter(storedProcCommand, "CalShareAmt", DbType.Decimal, svo2.FinalShareAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "PaySharePer", DbType.Decimal, svo2.PaySharePercentage);
                            this.dbServer.AddInParameter(storedProcCommand, "PayShareAmt", DbType.Decimal, svo2.PayShareAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "Total", DbType.Decimal, svo2.TotalAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Decimal, svo2.NetBillAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "Discount", DbType.Decimal, svo2.TotalConcessionAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, companyPaymentDetails.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo2.Status);
                            this.dbServer.ExecuteNonQuery(storedProcCommand);
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
                    clsCompanyPaymentDetailsVO companyPaymentDetails = nvo.CompanyPaymentDetails;
                    foreach (clsCompanyPaymentDetailsVO svo4 in nvo.CompanyPaymentInfoList)
                    {
                        if (svo4.IsSelected)
                        {
                            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorSharePaymentDetails");
                            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, companyPaymentDetails.UnitID);
                            this.dbServer.AddInParameter(storedProcCommand, "Billno", DbType.Int64, svo4.BillNo);
                            this.dbServer.AddInParameter(storedProcCommand, "BillDate", DbType.DateTime, svo4.BillDate);
                            this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, svo4.ServiceId);
                            this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, svo4.DoctorId);
                            this.dbServer.AddInParameter(storedProcCommand, "CalSharePer", DbType.Decimal, svo4.DoctorSharePercentage);
                            this.dbServer.AddInParameter(storedProcCommand, "CalShareAmount", DbType.Decimal, svo4.FinalShareAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "PaySharePer", DbType.Decimal, svo4.PaySharePercentage);
                            this.dbServer.AddInParameter(storedProcCommand, "PayShareAmount", DbType.Decimal, svo4.PayShareAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "Total", DbType.Decimal, svo4.TotalAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Decimal, svo4.NetBillAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "Discount", DbType.Decimal, svo4.TotalConcessionAmount);
                            this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, companyPaymentDetails.AddedDateTime);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                            this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo4.ID);
                            this.dbServer.ExecuteNonQuery(storedProcCommand);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return nvo;
        }

        public override IValueObject AddInvoice(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddCompanyInvoiceBizActionVO nvo = valueObject as clsAddCompanyInvoiceBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyInvoice");
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceNo", DbType.String, nvo.CompanyDetails.InvoiceNo);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, nvo.CompanyDetails.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceAmmount", DbType.Double, nvo.CompanyDetails.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "BalAmt", DbType.Double, nvo.CompanyDetails.BalAmt);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed ", DbType.Boolean, nvo.CompanyDetails.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.CompanyDetails.InvoiceID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.CompanyDetails.InvoiceID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                foreach (clsCompanyPaymentDetailsVO svo in nvo.BillDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddCompanyInvoiceDetails");
                    this.dbServer.AddInParameter(command2, "InvoiceID", DbType.Int64, nvo.CompanyDetails.InvoiceID);
                    this.dbServer.AddInParameter(command2, "BillUnitID", DbType.Int64, svo.UnitID);
                    this.dbServer.AddInParameter(command2, "BillID", DbType.Int64, svo.ID);
                    this.dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, svo.TotalAmount);
                    this.dbServer.AddInParameter(command2, "TotalConcesion", DbType.Double, svo.TotalConcessionAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetBillAmount);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "Isfreezed", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.CompanyDetails.IsFreezed);
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.CompanyDetails.IsFreezed = (bool) this.dbServer.GetParameterValue(storedProcCommand, "Isfreezed");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.CompanyDetails = null;
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject DeleteInvoiceBill(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsDeleteCompanyInvoiceBillBizActionVO nvo = valueObject as clsDeleteCompanyInvoiceBillBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateInvoiceFlagForBill");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.Int64, nvo.BillID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.CompanyDetails = null;
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetCompanyInvoice(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCompanyInvoiceDetailsBizActionVO nvo = valueObject as clsGetCompanyInvoiceDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.String, nvo.CompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceNo", DbType.String, nvo.InvoiceNo);
                this.dbServer.AddInParameter(storedProcCommand, "InvoicePrint", DbType.Boolean, nvo.InvoicePrint);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CompanyPaymentDetails == null)
                    {
                        nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"])),
                            Date = Convert.ToString(DALHelper.HandleDBNull(reader["Date"])),
                            AddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"]))),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["InvoiceAmmount"])),
                            CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"])),
                            BalAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["InvoioceBalanceAmount"])),
                            PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"])),
                            TDSAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TDSAmount"])),
                            CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]))
                        };
                        if (item.BalAmt == 0.0)
                        {
                            item.IsEnabled = true;
                        }
                        item.UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
                        item.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.CompanyPaymentDetails.Add(item);
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

        public override IValueObject GetCompanyInvoiceDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompanyInvoiceForModifyVO yvo = (clsGetCompanyInvoiceForModifyVO) valueObject;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillDetailsAgainstInvoiceID");
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceID", DbType.String, yvo.InvoiceID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, yvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, yvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, yvo.MaximumRows);
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, yvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (yvo.CompanyPaymentDetails == null)
                    {
                        yvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            yvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            BalAmt = (double) DALHelper.HandleDBNull(reader["BalanceAmountSelf"]),
                            BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"])),
                            InvoiceDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["InvoiceDate"])),
                            CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["companyID"])),
                            CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]))
                        };
                        yvo.CompanyPaymentDetails.Add(item);
                    }
                }
                reader.Close();
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
                connection = null;
                transaction = null;
            }
            return yvo;
        }

        public override IValueObject GetCompanyPaymentDetail(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCompanyPaymentDetailsBizActionVO nvo = (clsGetCompanyPaymentDetailsBizActionVO) valueObject;
            if (nvo.IsFromNewForm)
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentGroupDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.String, nvo.CompanyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.CompanyPaymentDetails == null)
                        {
                            nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                        }
                        while (reader.Read())
                        {
                            clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                                BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                                TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                                NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                                TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                                PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"])),
                                CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"])),
                                MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRno"])),
                                Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"])),
                                FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                                MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                                LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                                CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"])),
                                BalAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]))
                            };
                            nvo.CompanyPaymentDetails.Add(item);
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
            else if (!nvo.ServiceWise && !nvo.IsCompanyForm)
            {
                if (!nvo.IsPaidBill && !nvo.IsChild)
                {
                    try
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaymentGroupDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "@ServiceRate", DbType.Boolean, false);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.String, nvo.DoctorID);
                        DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader2.HasRows)
                        {
                            if (nvo.CompanyPaymentDetails == null)
                            {
                                nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                            }
                            while (reader2.Read())
                            {
                                clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                    ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["BillID"])),
                                    BillNo = Convert.ToString(DALHelper.HandleDBNull(reader2["BillNo"])),
                                    BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader2["BillDate"])),
                                    Date = Convert.ToString(DALHelper.HandleDBNull(reader2["GridDate"])),
                                    TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["TotalAmount"])),
                                    NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["NetAmount"])),
                                    TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["ConcessionAmount"])),
                                    FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["ShareAmount"])),
                                    MRNo = Convert.ToString(DALHelper.HandleDBNull(reader2["MRNo"])),
                                    PatientName = Convert.ToString(DALHelper.HandleDBNull(reader2["PatientName"])),
                                    IsSelected = false,
                                    IsEnabled = true,
                                    IsReadOnly = true
                                };
                                nvo.CompanyPaymentDetails.Add(item);
                            }
                        }
                        reader2.NextResult();
                        reader2.Close();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else if (!nvo.IsPaidBill && nvo.IsChild)
                {
                    try
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaymentDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PaidBill", DbType.Boolean, false);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.String, nvo.DoctorID);
                        this.dbServer.AddInParameter(storedProcCommand, "@ServiceRate", DbType.Boolean, false);
                        if (nvo.BillNo > 0L)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.Int64, nvo.BillNo);
                        }
                        DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader3.HasRows)
                        {
                            if (nvo.CompanyPaymentDetails == null)
                            {
                                nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                            }
                            while (reader3.Read())
                            {
                                clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                    ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader3["BillID"])),
                                    BillNo = Convert.ToString(DALHelper.HandleDBNull(reader3["BillNo"])),
                                    BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader3["BillDate"])),
                                    Date = Convert.ToString(DALHelper.HandleDBNull(reader3["GridDate"])),
                                    ServiceId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader3["ServiceID"])),
                                    ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader3["ServiceName"])),
                                    TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader3["TotalAmount"])),
                                    NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader3["NetAmount"])),
                                    TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader3["ConcessionAmount"])),
                                    DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader3["SharePercentage"])),
                                    FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader3["ShareAmount"])),
                                    PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader3["ShareAmount"])),
                                    PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader3["SharePercentage"])),
                                    DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader3["ReferredDoctor"])),
                                    DoctorId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader3["ReferredDoctorID"])),
                                    IsSelected = false,
                                    IsEnabled = true,
                                    IsReadOnly = true
                                };
                                nvo.CompanyPaymentDetails.Add(item);
                            }
                        }
                        reader3.NextResult();
                        reader3.Close();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else if (nvo.IsPaidBill && nvo.IsChild)
                {
                    try
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaidPaymentDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.String, nvo.DoctorID);
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.Double, nvo.BillNo);
                        DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader4.HasRows)
                        {
                            if (nvo.CompanyPaymentDetails == null)
                            {
                                nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                            }
                            while (reader4.Read())
                            {
                                clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                    ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader4["BillID"])),
                                    BillNo = Convert.ToString(DALHelper.HandleDBNull(reader4["BillNo"])),
                                    BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader4["BillDate"])),
                                    Date = Convert.ToString(DALHelper.HandleDBNull(reader4["GridDate"])),
                                    ServiceId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader4["ServiceID"])),
                                    ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader4["ServiceName"])),
                                    TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader4["TotalAmount"])),
                                    NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader4["NetAmount"])),
                                    TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader4["ConcessionAmount"])),
                                    DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader4["CalSharePer"])),
                                    FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader4["CalShareAmount"])),
                                    PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader4["PaySharePer"])),
                                    PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader4["PayShareAmount"])),
                                    IsSelected = true,
                                    IsEnabled = false,
                                    IsReadOnly = true
                                };
                                nvo.CompanyPaymentDetails.Add(item);
                            }
                        }
                        reader4.NextResult();
                        reader4.Close();
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
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaymentDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PaidBill", DbType.Boolean, true);
                        this.dbServer.AddInParameter(storedProcCommand, "@ServiceRate", DbType.Boolean, false);
                        if (nvo.DoctorID != "")
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.String, nvo.DoctorID);
                        }
                        DbDataReader reader5 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader5.HasRows)
                        {
                            if (nvo.CompanyPaymentDetails == null)
                            {
                                nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                            }
                            while (reader5.Read())
                            {
                                clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                    ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader5["BillID"])),
                                    BillNo = Convert.ToString(DALHelper.HandleDBNull(reader5["BillNo"])),
                                    BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader5["BillDate"])),
                                    Date = Convert.ToString(DALHelper.HandleDBNull(reader5["GridDate"])),
                                    TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader5["TotalAmount"])),
                                    NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader5["NetAmount"])),
                                    TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader5["ConcessionAmount"])),
                                    DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader5["CalSharePer"])),
                                    FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader5["CalShareAmount"])),
                                    PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader5["PaySharePer"])),
                                    PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader5["PayShareAmount"])),
                                    MRNo = Convert.ToString(DALHelper.HandleDBNull(reader5["MRNo"])),
                                    PatientName = Convert.ToString(DALHelper.HandleDBNull(reader5["PatientName"])),
                                    IsSelected = true,
                                    IsEnabled = false,
                                    IsReadOnly = true
                                };
                                nvo.CompanyPaymentDetails.Add(item);
                            }
                        }
                        reader5.NextResult();
                        reader5.Close();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else if (!nvo.IsPaidBill && !nvo.IsChild)
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentGroupDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientTypeID", DbType.Int64, nvo.PatientSouceID);
                    this.dbServer.AddInParameter(storedProcCommand, "@ServiceRate", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.String, nvo.CompanyID);
                    if (nvo.AssCompanyID != "")
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, nvo.AssCompanyID);
                    }
                    DbDataReader reader6 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader6.HasRows)
                    {
                        if (nvo.CompanyPaymentDetails == null)
                        {
                            nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                        }
                        while (reader6.Read())
                        {
                            clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["BillID"])),
                                BillNo = Convert.ToString(DALHelper.HandleDBNull(reader6["BillNo"])),
                                BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader6["BillDate"])),
                                Date = Convert.ToString(DALHelper.HandleDBNull(reader6["GridDate"])),
                                TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader6["TotalAmount"])),
                                NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader6["NetAmount"])),
                                TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader6["ConcessionAmount"])),
                                CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader6["Company"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader6["PatientName"])),
                                AssCompanyName = Convert.ToString(DALHelper.HandleDBNull(reader6["AssociateCompany"])),
                                CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["CompanyID"])),
                                AssCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["AssCompanyID"])),
                                FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader6["FinalShareAmount"])),
                                IsSelected = false,
                                IsEnabled = true,
                                IsReadOnly = true
                            };
                            nvo.CompanyPaymentDetails.Add(item);
                        }
                    }
                    reader6.NextResult();
                    reader6.Close();
                }
                catch (Exception)
                {
                }
            }
            else if (!nvo.IsPaidBill && nvo.IsChild)
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PaidBill", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "@ServiceRate", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientTypeID", DbType.Int64, nvo.PatientSouceID);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.String, nvo.CompanyID);
                    try
                    {
                        if ((nvo.AssCompanyID != "") && (Convert.ToInt64(nvo.AssCompanyID) > 0L))
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, nvo.AssCompanyID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, null);
                        }
                    }
                    catch
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, null);
                    }
                    if (nvo.BillNo > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.Int64, nvo.BillNo);
                    }
                    DbDataReader reader7 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader7.HasRows)
                    {
                        if (nvo.CompanyPaymentDetails == null)
                        {
                            nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                        }
                        while (reader7.Read())
                        {
                            clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader7["BillID"])),
                                BillNo = Convert.ToString(DALHelper.HandleDBNull(reader7["BillNo"])),
                                BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader7["BillDate"])),
                                Date = Convert.ToString(DALHelper.HandleDBNull(reader7["GridDate"])),
                                TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader7["TotalAmount"])),
                                NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader7["NetAmount"])),
                                TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader7["ConcessionAmount"])),
                                ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader7["ServiceID"])),
                                CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader7["Company"])),
                                ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader7["ServiceName"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader7["PatientName"])),
                                AssCompanyName = Convert.ToString(DALHelper.HandleDBNull(reader7["AssociateCompany"])),
                                CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader7["CompanyID"])),
                                AssCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader7["AssCompanyID"])),
                                DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader7["SharePercentage"])),
                                FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader7["FinalShareAmount"])),
                                PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader7["FinalShareAmount"])),
                                PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader7["SharePercentage"])),
                                IsSelected = false,
                                IsEnabled = true,
                                IsReadOnly = true
                            };
                            nvo.CompanyPaymentDetails.Add(item);
                        }
                    }
                    reader7.NextResult();
                    reader7.Close();
                }
                catch (Exception)
                {
                }
            }
            else if (!nvo.IsPaidBill || !nvo.IsChild)
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PaidBill", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "@ServiceRate", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientTypeID", DbType.Int64, nvo.PatientSouceID);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.String, nvo.CompanyID);
                    if (nvo.AssCompanyID != "")
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, nvo.AssCompanyID);
                    }
                    DbDataReader reader9 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader9.HasRows)
                    {
                        if (nvo.CompanyPaymentDetails == null)
                        {
                            nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                        }
                        while (reader9.Read())
                        {
                            clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader9["BillID"])),
                                BillNo = Convert.ToString(DALHelper.HandleDBNull(reader9["BillNo"])),
                                Date = Convert.ToString(DALHelper.HandleDBNull(reader9["GridDate"])),
                                BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader9["BillDate"])),
                                TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader9["TotalAmount"])),
                                NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader9["NetAmount"])),
                                TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader9["ConcessionAmount"])),
                                CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader9["Company"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader9["PatientName"])),
                                AssCompanyName = Convert.ToString(DALHelper.HandleDBNull(reader9["AssociateCompany"])),
                                CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader9["CompanyID"])),
                                AssCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader9["AssCompanyID"])),
                                DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader9["CalSharePer"])),
                                PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader9["PaySharePer"])),
                                PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader9["PayShareAmount"])),
                                FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader9["FinalShareAmount"])),
                                IsSelected = true,
                                IsEnabled = false,
                                IsReadOnly = true
                            };
                            nvo.CompanyPaymentDetails.Add(item);
                        }
                    }
                    reader9.NextResult();
                    reader9.Close();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyPaidPaymentDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.String, nvo.CompanyID);
                    try
                    {
                        if ((nvo.AssCompanyID != "") && (Convert.ToInt64(nvo.AssCompanyID) > 0L))
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, nvo.AssCompanyID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, null);
                        }
                    }
                    catch
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AssoCompanyID", DbType.String, null);
                    }
                    if (nvo.BillNo > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.Int64, nvo.BillNo);
                    }
                    DbDataReader reader8 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader8.HasRows)
                    {
                        if (nvo.CompanyPaymentDetails == null)
                        {
                            nvo.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                        }
                        while (reader8.Read())
                        {
                            clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader8["BillID"])),
                                BillNo = Convert.ToString(DALHelper.HandleDBNull(reader8["BillNo"])),
                                Date = Convert.ToString(DALHelper.HandleDBNull(reader8["GridDate"])),
                                BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader8["BillDate"])),
                                TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader8["TotalAmount"])),
                                NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader8["NetAmount"])),
                                TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader8["ConcessionAmount"])),
                                ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader8["ServiceID"])),
                                ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader8["ServiceName"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader8["PatientName"])),
                                DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader8["CalSharePer"])),
                                PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader8["PaySharePer"])),
                                PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader8["PayShareAmount"])),
                                FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader8["FinalShareAmount"])),
                                IsSelected = true,
                                IsEnabled = false,
                                IsReadOnly = true
                            };
                            nvo.CompanyPaymentDetails.Add(item);
                        }
                    }
                    reader8.NextResult();
                    reader8.Close();
                }
                catch (Exception)
                {
                }
            }
            return nvo;
        }

        public override IValueObject GetInvoiceSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillAgainstInvoiceBizActionVO nvo = (clsGetBillAgainstInvoiceBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillDetailsAgainstInvoice");
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceID", DbType.String, nvo.InvoiceID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsCompanyPaymentDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsCompanyPaymentDetailsVO item = new clsCompanyPaymentDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"])),
                            TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            BalanceAmountSelf = (double) DALHelper.HandleDBNull(reader["BalanceAmountSelf"]),
                            Opd_Ipd_External_Id = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]),
                            Opd_Ipd_External_UnitId = (long) DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"])
                        };
                        item.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])).ToShortDateString();
                        item.TDSAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TDSAmount"]));
                        item.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        nvo.List.Add(item);
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

        public override IValueObject ModifyInvoice(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsModifyCompanyInvoiceBizActionVO nvo = valueObject as clsModifyCompanyInvoiceBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteCompanyInvoicebill");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceID", DbType.Int64, nvo.InvoiceID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                foreach (clsCompanyPaymentDetailsVO svo in nvo.BillDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_ModifyCompanyInvoiceDetails");
                    this.dbServer.AddParameter(command2, "Isfreezed", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.CompanyDetails.IsFreezed);
                    this.dbServer.AddInParameter(command2, "InvoiceID", DbType.Int64, nvo.InvoiceID);
                    this.dbServer.AddInParameter(command2, "BillUnitID", DbType.Int64, svo.UnitID);
                    this.dbServer.AddInParameter(command2, "BillID", DbType.Int64, svo.ID);
                    this.dbServer.AddInParameter(command2, "TotalBillAmount", DbType.Int64, nvo.CompanyDetails.TotalAmount);
                    this.dbServer.AddInParameter(command2, "BalanceAmount", DbType.Int64, nvo.CompanyDetails.BalAmt);
                    this.dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, svo.TotalAmount);
                    this.dbServer.AddInParameter(command2, "TotalConcesion", DbType.Double, svo.TotalConcessionAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetBillAmount);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.CompanyDetails.IsFreezed = (bool) this.dbServer.GetParameterValue(command2, "Isfreezed");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.CompanyDetails = null;
                throw;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }
    }
}

