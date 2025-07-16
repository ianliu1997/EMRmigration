using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsCompanyDAL : clsBaseCompanyPaymentDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsCompanyDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }
        public override IValueObject GetCompanyPaymentDetail(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCompanyPaymentDetailsBizActionVO BizActionObj = (clsGetCompanyPaymentDetailsBizActionVO)valueObject;
            if (BizActionObj.IsFromNewForm == true)
            {
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentGroupDetails");
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command, "CompanyID", DbType.String, BizActionObj.CompanyID);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.CompanyPaymentDetails == null)
                            BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                        while (reader.Read())
                        {
                            clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                            CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            CompanyPaymentInformation.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                            CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                            CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                            CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                            CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                            CompanyPaymentInformation.PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"]));
                            CompanyPaymentInformation.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                            CompanyPaymentInformation.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRno"]));
                            CompanyPaymentInformation.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            CompanyPaymentInformation.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                            CompanyPaymentInformation.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                            CompanyPaymentInformation.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                            CompanyPaymentInformation.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                            CompanyPaymentInformation.BalAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmountSelf"]));
                            BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                #region "Get Doctor Share Payment Details"
                if (BizActionObj.ServiceWise == false && BizActionObj.IsCompanyForm == false)
                {
                    #region "View Doctor Share Payment Details"
                    if (BizActionObj.IsPaidBill == false && BizActionObj.IsChild == false)
                    {
                        try
                        {
                            //DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaymentDetails");
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaymentGroupDetails");
                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "@ServiceRate", DbType.Boolean, false);
                            //dbServer.AddInParameter(command, "PaidBill", DbType.Boolean, false);  
                            //if (BizActionObj.DoctorID != "")
                            dbServer.AddInParameter(command, "DoctorID", DbType.String, BizActionObj.DoctorID);
                            //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                            //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                            //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));
                                    //CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ServiceID"]));
                                    //CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));

                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));

                                    //CompanyPaymentInformation.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));
                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));

                                    //CompanyPaymentInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                                    //CompanyPaymentInformation.DoctorId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferredDoctorID"]));
                                    CompanyPaymentInformation.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                                    CompanyPaymentInformation.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));

                                    CompanyPaymentInformation.IsSelected = false;
                                    CompanyPaymentInformation.IsEnabled = true;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();
                            // BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                            reader.Close();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                        }
                    }
                    #endregion
                    #region "BillWise Doctor Share Payment Details"
                    else if (BizActionObj.IsPaidBill == false && BizActionObj.IsChild == true)
                    {
                        try
                        {
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaymentDetails");
                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "PaidBill", DbType.Boolean, false);
                            dbServer.AddInParameter(command, "DoctorID", DbType.String, BizActionObj.DoctorID);
                            dbServer.AddInParameter(command, "@ServiceRate", DbType.Boolean, false);
                            if (BizActionObj.BillNo > 0)
                                dbServer.AddInParameter(command, "BillNo", DbType.Int64, BizActionObj.BillNo);
                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));
                                    CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ServiceID"]));
                                    CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));

                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));

                                    CompanyPaymentInformation.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));
                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));
                                    CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));
                                    CompanyPaymentInformation.PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));

                                    CompanyPaymentInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                                    CompanyPaymentInformation.DoctorId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferredDoctorID"]));

                                    CompanyPaymentInformation.IsSelected = false;
                                    CompanyPaymentInformation.IsEnabled = true;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();
                            // BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                            reader.Close();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    #endregion
                    #region "Bill Wise Paid Doctor Share Payment Details"
                    else if (BizActionObj.IsPaidBill == true && BizActionObj.IsChild == true)
                    {
                        try
                        {
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaidPaymentDetails");
                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "DoctorID", DbType.String, BizActionObj.DoctorID);
                            dbServer.AddInParameter(command, "BillNo", DbType.Double, BizActionObj.BillNo);
                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));
                                    CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ServiceID"]));
                                    CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));

                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));

                                    CompanyPaymentInformation.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalSharePer"]));
                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalShareAmount"]));

                                    CompanyPaymentInformation.PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaySharePer"]));
                                    CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PayShareAmount"]));

                                    //CompanyPaymentInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                                    //CompanyPaymentInformation.DoctorId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferredDoctorID"]));
                                    //CompanyPaymentInformation.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                                    //CompanyPaymentInformation.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));

                                    CompanyPaymentInformation.IsSelected = true;
                                    CompanyPaymentInformation.IsEnabled = false;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();
                            reader.Close();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    #endregion
                    #region "View Saved Doctor Payment Details"
                    else
                    {
                        try
                        {
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorSharePaymentDetails");
                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "PaidBill", DbType.Boolean, true);
                            dbServer.AddInParameter(command, "@ServiceRate", DbType.Boolean, false);
                            if (BizActionObj.DoctorID != "")
                                dbServer.AddInParameter(command, "DoctorID", DbType.String, BizActionObj.DoctorID);
                            //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                            //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                            //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                            //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));
                                    //CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ServiceID"]));
                                    //CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));

                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));

                                    CompanyPaymentInformation.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalSharePer"]));
                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalShareAmount"]));

                                    CompanyPaymentInformation.PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaySharePer"]));
                                    CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PayShareAmount"]));

                                    //CompanyPaymentInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                                    //CompanyPaymentInformation.DoctorId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ReferredDoctorID"]));
                                    CompanyPaymentInformation.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                                    CompanyPaymentInformation.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));

                                    CompanyPaymentInformation.IsSelected = true;
                                    CompanyPaymentInformation.IsEnabled = false;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();
                            // BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                            reader.Close();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                        }

                    }
                    #endregion
                }
                #endregion
                #region "Get Company/Ass Share Payment Details"
                else
                {

                    if (BizActionObj.IsPaidBill == false && BizActionObj.IsChild == false)
                    {

                        try
                        {

                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentGroupDetails");


                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "PatientTypeID", DbType.Int64, BizActionObj.PatientSouceID);
                            dbServer.AddInParameter(command, "@ServiceRate", DbType.Boolean, false);
                            // string comp = "Company";

                            // dbServer.AddInParameter(command, "@CompanyDes", DbType.String, comp);

                            dbServer.AddInParameter(command, "CompanyID", DbType.String, BizActionObj.CompanyID);

                            if (BizActionObj.AssCompanyID != "")
                                dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, BizActionObj.AssCompanyID);
                            // dbServer.AddInParameter(command, "PaidBill", DbType.Boolean, false);
                            // dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                            //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                            //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                            //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                            //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));
                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                    // CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                                    CompanyPaymentInformation.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                                    //CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])); 
                                    CompanyPaymentInformation.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));

                                    CompanyPaymentInformation.AssCompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["AssociateCompany"]));
                                    CompanyPaymentInformation.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                                    CompanyPaymentInformation.AssCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssCompanyID"]));
                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["FinalShareAmount"]));

                                    CompanyPaymentInformation.IsSelected = false;
                                    CompanyPaymentInformation.IsEnabled = true;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();

                            reader.Close();
                        }

                        catch (Exception)
                        {
                        }

                    }
                    else if (BizActionObj.IsPaidBill == false && BizActionObj.IsChild == true)
                    {

                        try
                        {

                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentDetails");


                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "PaidBill", DbType.Boolean, false);
                            dbServer.AddInParameter(command, "@ServiceRate", DbType.Boolean, false);
                            dbServer.AddInParameter(command, "PatientTypeID", DbType.Int64, BizActionObj.PatientSouceID);
                            //string comp = "Company";
                            //dbServer.AddInParameter(command, "@CompanyDes", DbType.String, comp);

                            dbServer.AddInParameter(command, "CompanyID", DbType.String, BizActionObj.CompanyID);
                            try
                            {
                                if (BizActionObj.AssCompanyID != "" && Convert.ToInt64(BizActionObj.AssCompanyID) > 0)
                                    dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, BizActionObj.AssCompanyID);
                                else
                                    dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, null);
                            }
                            catch
                            {
                                dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, null);
                            }
                            if (BizActionObj.BillNo > 0)
                                dbServer.AddInParameter(command, "BillNo", DbType.Int64, BizActionObj.BillNo);


                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));
                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                    CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                                    CompanyPaymentInformation.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                                    CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                                    CompanyPaymentInformation.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                                    CompanyPaymentInformation.AssCompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["AssociateCompany"]));
                                    CompanyPaymentInformation.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                                    CompanyPaymentInformation.AssCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssCompanyID"]));

                                    CompanyPaymentInformation.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));

                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["FinalShareAmount"]));
                                    CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["FinalShareAmount"]));
                                    CompanyPaymentInformation.PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));
                                    CompanyPaymentInformation.IsSelected = false;
                                    CompanyPaymentInformation.IsEnabled = true;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();

                            reader.Close();
                        }

                        catch (Exception)
                        {
                        }


                    }
                    else if (BizActionObj.IsPaidBill == true && BizActionObj.IsChild == true)
                    {

                        try
                        {

                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyPaidPaymentDetails");


                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            //dbServer.AddInParameter(command, "PaidBill", DbType.Boolean, true);
                            //dbServer.AddInParameter(command, "@ServiceRate", DbType.Boolean, false);

                            dbServer.AddInParameter(command, "CompanyID", DbType.String, BizActionObj.CompanyID);

                            try
                            {
                                if (BizActionObj.AssCompanyID != "" && Convert.ToInt64(BizActionObj.AssCompanyID) > 0)
                                    dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, BizActionObj.AssCompanyID);
                                else
                                    dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, null);
                            }
                            catch
                            {
                                dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, null);
                            }
                            if (BizActionObj.BillNo > 0)
                                dbServer.AddInParameter(command, "BillNo", DbType.Int64, BizActionObj.BillNo);

                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));

                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));

                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                    CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                                    // CompanyPaymentInformation.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                                    CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                                    CompanyPaymentInformation.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                                    //CompanyPaymentInformation.AssCompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["AssociateCompany"]));
                                    //CompanyPaymentInformation.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                                    //CompanyPaymentInformation.AssCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssCompanyID"]));

                                    CompanyPaymentInformation.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalSharePer"]));

                                    //CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalShareAmount"]));

                                    CompanyPaymentInformation.PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaySharePer"]));
                                    CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PayShareAmount"]));

                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["FinalShareAmount"]));


                                    //CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["FinalShareAmount"]));
                                    CompanyPaymentInformation.IsSelected = true;
                                    CompanyPaymentInformation.IsEnabled = false;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();

                            reader.Close();
                        }
                        catch (Exception)
                        {
                        }



                    }
                    else
                    {
                        try
                        {

                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentDetails");


                            dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                            dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "PaidBill", DbType.Boolean, true);
                            dbServer.AddInParameter(command, "@ServiceRate", DbType.Boolean, false);
                            dbServer.AddInParameter(command, "PatientTypeID", DbType.Int64, BizActionObj.PatientSouceID);
                            //string comp = "Company";
                            //dbServer.AddInParameter(command, "@CompanyDes", DbType.String, comp);

                            dbServer.AddInParameter(command, "CompanyID", DbType.String, BizActionObj.CompanyID);

                            if (BizActionObj.AssCompanyID != "")
                                dbServer.AddInParameter(command, "AssoCompanyID", DbType.String, BizActionObj.AssCompanyID);


                            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                            if (reader.HasRows)
                            {
                                if (BizActionObj.CompanyPaymentDetails == null)
                                    BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                                while (reader.Read())
                                {
                                    clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                                    CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                                    CompanyPaymentInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));

                                    CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["GridDate"]));

                                    CompanyPaymentInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                                    CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                                    CompanyPaymentInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                                    CompanyPaymentInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                    //CompanyPaymentInformation.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                                    CompanyPaymentInformation.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                                    //CompanyPaymentInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                                    CompanyPaymentInformation.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                                    CompanyPaymentInformation.AssCompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["AssociateCompany"]));
                                    CompanyPaymentInformation.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                                    CompanyPaymentInformation.AssCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssCompanyID"]));

                                    CompanyPaymentInformation.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalSharePer"]));

                                    //CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CalShareAmount"]));

                                    CompanyPaymentInformation.PaySharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaySharePer"]));
                                    CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PayShareAmount"]));

                                    CompanyPaymentInformation.FinalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["FinalShareAmount"]));


                                    //CompanyPaymentInformation.PayShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["FinalShareAmount"]));
                                    CompanyPaymentInformation.IsSelected = true;
                                    CompanyPaymentInformation.IsEnabled = false;
                                    CompanyPaymentInformation.IsReadOnly = true;

                                    BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                                }
                            }
                            reader.NextResult();

                            reader.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                #endregion
            }
            return BizActionObj;
        }
        public override IValueObject AddCompanyPaymentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddCompanyPaymentDetailsBizActionVO BizActionObj = valueObject as clsAddCompanyPaymentDetailsBizActionVO;
            #region "Save Company Share Payment
            if (BizActionObj.IsCompanyForm == true)
            {
                try
                {
                    clsCompanyPaymentDetailsVO ObjDoctorVO = BizActionObj.CompanyPaymentDetails;


                    foreach (var item in BizActionObj.CompanyPaymentInfoList)
                    {

                        if (item.IsSelected == true)
                        {

                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCompanyPaymentDetails");

                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);
                            dbServer.AddInParameter(command, "Billno", DbType.Int64, item.BillNo);
                            dbServer.AddInParameter(command, "AssCompanyID", DbType.Int64, item.AssCompanyID);
                            dbServer.AddInParameter(command, "BillDate", DbType.DateTime, item.BillDate);

                            dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceId);

                            dbServer.AddInParameter(command, "CompanyID", DbType.String, item.CompanyID);
                            //dbServer.AddInParameter(command, "PayAmount", DbType.Decimal, item.FinalShareAmount);
                            dbServer.AddInParameter(command, "CalSharePer", DbType.Decimal, item.DoctorSharePercentage);
                            dbServer.AddInParameter(command, "CalShareAmt", DbType.Decimal, item.FinalShareAmount);
                            dbServer.AddInParameter(command, "PaySharePer", DbType.Decimal, item.PaySharePercentage);
                            dbServer.AddInParameter(command, "PayShareAmt", DbType.Decimal, item.PayShareAmount);




                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);


                            dbServer.AddInParameter(command, "Total", DbType.Decimal, item.TotalAmount);
                            dbServer.AddInParameter(command, "NetAmount", DbType.Decimal, item.NetBillAmount);
                            dbServer.AddInParameter(command, "Discount", DbType.Decimal, item.TotalConcessionAmount);

                            //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);


                            //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                            //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                            //dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                            //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                            //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.ID);
                            int intStatus2 = dbServer.ExecuteNonQuery(command);
                            //BizActionObj.DoctorShareDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
            #endregion
            #region "Save Doctor Share Payment
            else
            {
                //============================Save the data of Doctor Share Details===============================================
                try
                {
                    clsCompanyPaymentDetailsVO ObjDoctorVO = BizActionObj.CompanyPaymentDetails;


                    foreach (var item in BizActionObj.CompanyPaymentInfoList)
                    {

                        if (item.IsSelected == true)
                        {
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorSharePaymentDetails");

                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);
                            dbServer.AddInParameter(command, "Billno", DbType.Int64, item.BillNo);
                            dbServer.AddInParameter(command, "BillDate", DbType.DateTime, item.BillDate);

                            dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceId);
                            dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorId);
                            dbServer.AddInParameter(command, "CalSharePer", DbType.Decimal, item.DoctorSharePercentage);
                            dbServer.AddInParameter(command, "CalShareAmount", DbType.Decimal, item.FinalShareAmount);
                            dbServer.AddInParameter(command, "PaySharePer", DbType.Decimal, item.PaySharePercentage);
                            dbServer.AddInParameter(command, "PayShareAmount", DbType.Decimal, item.PayShareAmount);


                            dbServer.AddInParameter(command, "Total", DbType.Decimal, item.TotalAmount);
                            dbServer.AddInParameter(command, "NetAmount", DbType.Decimal, item.NetBillAmount);
                            dbServer.AddInParameter(command, "Discount", DbType.Decimal, item.TotalConcessionAmount);
                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);


                            //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                            //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                            //dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command, "Status", DbType.Boolean, true);// item.Status);
                            dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                            int intStatus2 = dbServer.ExecuteNonQuery(command);
                            //BizActionObj.DoctorShareDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion
            return BizActionObj;
        }

        public override IValueObject GetCompanyInvoice(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetCompanyInvoiceDetailsBizActionVO BizActionObj = valueObject as clsGetCompanyInvoiceDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCompanyPaymentDetails");

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "CompanyID", DbType.String, BizActionObj.CompanyID);
                dbServer.AddInParameter(command, "InvoiceNo", DbType.String, BizActionObj.InvoiceNo);
                dbServer.AddInParameter(command, "InvoicePrint", DbType.Boolean, BizActionObj.InvoicePrint);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, true);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.CompanyPaymentDetails == null)
                        BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                    while (reader.Read())
                    {
                        clsCompanyPaymentDetailsVO CompanyPaymentInformation = new clsCompanyPaymentDetailsVO();
                        CompanyPaymentInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        CompanyPaymentInformation.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        CompanyPaymentInformation.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        CompanyPaymentInformation.Date = Convert.ToString(DALHelper.HandleDBNull(reader["Date"]));
                        CompanyPaymentInformation.AddedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"]));
                        CompanyPaymentInformation.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["InvoiceAmmount"]));
                        CompanyPaymentInformation.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                        CompanyPaymentInformation.BalAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["InvoioceBalanceAmount"]));
                        CompanyPaymentInformation.PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"]));
                        CompanyPaymentInformation.TDSAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TDSAmount"]));
                        CompanyPaymentInformation.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                        if (CompanyPaymentInformation.BalAmt == 0)
                        {
                            CompanyPaymentInformation.IsEnabled = true;
                        }
                        CompanyPaymentInformation.UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
                        CompanyPaymentInformation.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizActionObj.CompanyPaymentDetails.Add(CompanyPaymentInformation);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return BizActionObj;
        }


        public override IValueObject AddInvoice(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddCompanyInvoiceBizActionVO BizActionObj = valueObject as clsAddCompanyInvoiceBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCompanyInvoice");
                dbServer.AddInParameter(command, "InvoiceNo", DbType.String, BizActionObj.CompanyDetails.InvoiceNo);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, BizActionObj.CompanyDetails.CompanyID);
                dbServer.AddInParameter(command, "InvoiceAmmount", DbType.Double, BizActionObj.CompanyDetails.TotalAmount);
                dbServer.AddInParameter(command, "BalAmt", DbType.Double, BizActionObj.CompanyDetails.BalAmt);
                dbServer.AddInParameter(command, "Isfreezed ", DbType.Boolean, BizActionObj.CompanyDetails.IsFreezed);
                // dbServer.AddInParameter(command, "PaidAmount", DbType.Double, BizActionObj.CompanyDetails.PaidAmount);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.CompanyDetails.InvoiceID);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.CompanyDetails.InvoiceID = (long)dbServer.GetParameterValue(command, "ID");

                foreach (var item in BizActionObj.BillDetails)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCompanyInvoiceDetails");
                    dbServer.AddInParameter(command2, "InvoiceID", DbType.Int64, BizActionObj.CompanyDetails.InvoiceID);
                    dbServer.AddInParameter(command2, "BillUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command2, "BillID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.TotalAmount);
                    dbServer.AddInParameter(command2, "TotalConcesion", DbType.Double, item.TotalConcessionAmount);
                    dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetBillAmount);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "Isfreezed", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.CompanyDetails.IsFreezed);
                 
                    dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    int intStatus3 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.CompanyDetails.IsFreezed = (bool)dbServer.GetParameterValue(command, "Isfreezed");
                }

                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                BizActionObj.CompanyDetails = null;
                throw;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;

        }

        public override IValueObject GetInvoiceSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBillAgainstInvoiceBizActionVO BizActionObj = (clsGetBillAgainstInvoiceBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBillDetailsAgainstInvoice");

                dbServer.AddInParameter(command, "InvoiceID", DbType.String, BizActionObj.InvoiceID);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsCompanyPaymentDetailsVO>();
                    while (reader.Read())
                    {
                        clsCompanyPaymentDetailsVO obj = new clsCompanyPaymentDetailsVO();
                        obj.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        obj.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        obj.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        obj.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        obj.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        obj.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        obj.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        obj.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        obj.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        obj.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        obj.BalanceAmountSelf = (double)DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                        obj.Opd_Ipd_External_Id = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_Id"]);
                        obj.Opd_Ipd_External_UnitId = (long)DALHelper.HandleDBNull(reader["Opd_Ipd_External_UnitId"]);
                        obj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])).ToShortDateString();
                        obj.TDSAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TDSAmount"]));
                        obj.IsFreezed = (bool)DALHelper.HandleDBNull(reader["IsFreezed"]);

                        BizActionObj.List.Add(obj);
                    }
                }

                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return BizActionObj;
        }


        public override IValueObject GetCompanyInvoiceDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompanyInvoiceForModifyVO BizActionObj = (clsGetCompanyInvoiceForModifyVO)valueObject;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBillDetailsAgainstInvoiceID");

                dbServer.AddInParameter(command, "InvoiceID", DbType.String, BizActionObj.InvoiceID);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, true);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.CompanyPaymentDetails == null)
                        BizActionObj.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                    while (reader.Read())
                    {
                        clsCompanyPaymentDetailsVO obj = new clsCompanyPaymentDetailsVO();
                        obj.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        obj.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        obj.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        obj.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        obj.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        obj.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        obj.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        obj.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        obj.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        obj.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        obj.BalAmt = (double)DALHelper.HandleDBNull(reader["BalanceAmountSelf"]);
                        obj.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        obj.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        obj.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        obj.InvoiceDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["InvoiceDate"]));
                        obj.CompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["companyID"]));
                        obj.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                        BizActionObj.CompanyPaymentDetails.Add(obj);

                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();


                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }






        public override IValueObject ModifyInvoice(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsModifyCompanyInvoiceBizActionVO BizActionObj = valueObject as clsModifyCompanyInvoiceBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
            
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteCompanyInvoicebill");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command3, "InvoiceID", DbType.Int64, BizActionObj.InvoiceID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
              
                foreach (var item in BizActionObj.BillDetails)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_ModifyCompanyInvoiceDetails");
                    dbServer.AddParameter(command2, "Isfreezed", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.CompanyDetails.IsFreezed);
                    dbServer.AddInParameter(command2, "InvoiceID", DbType.Int64, BizActionObj.InvoiceID);
                    dbServer.AddInParameter(command2, "BillUnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command2, "BillID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command2, "TotalBillAmount", DbType.Int64, BizActionObj.CompanyDetails.TotalAmount);
                    dbServer.AddInParameter(command2, "BalanceAmount", DbType.Int64, BizActionObj.CompanyDetails.BalAmt);
                    dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.TotalAmount);
                    dbServer.AddInParameter(command2, "TotalConcesion", DbType.Double, item.TotalConcessionAmount);
                    dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetBillAmount);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    int intStatus3 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.CompanyDetails.IsFreezed = (bool)dbServer.GetParameterValue(command2, "Isfreezed");
                }

                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                BizActionObj.CompanyDetails = null;
                throw;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;

        }


        public override IValueObject DeleteInvoiceBill(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsDeleteCompanyInvoiceBillBizActionVO BizActionObj = valueObject as clsDeleteCompanyInvoiceBillBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateInvoiceFlagForBill");

                dbServer.AddInParameter(command3, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command3, "BillID", DbType.Int64, BizActionObj.BillID);
                int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);

            
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                BizActionObj.CompanyDetails = null;
                throw;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;

        }



    }
}
