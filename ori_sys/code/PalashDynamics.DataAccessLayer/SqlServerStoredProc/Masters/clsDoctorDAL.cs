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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.ValueObjects.Billing;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsDoctorDAL : clsBaseDoctorDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsDoctorDAL()
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
            catch (Exception ex)
            {
                throw;
            }
        }



        public override IValueObject GetDoctorList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorListBizActionVO BizActionObj = (clsGetDoctorListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorList");
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizActionObj.DepartmentId);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetailsList == null)
                        BizActionObj.DoctorDetailsList = new List<clsDoctorVO>();
                    while (reader.Read())
                    {
                        clsDoctorVO DoctorVO = new clsDoctorVO();
                        DoctorVO.DoctorId = (long)reader["DoctorId"];
                        DoctorVO.DoctorName = reader["DoctorName"].ToString();
                        BizActionObj.DoctorDetailsList.Add(DoctorVO);
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

        public override IValueObject GetDoctorSchedule(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDoctorScheduleBizActionVO BizAction = (valueObject) as clsGetDoctorScheduleBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorScheduleList");

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizAction.UnitId);
                dbServer.AddInParameter(command, "DepartmentId", DbType.Int64, BizAction.DepartmentId);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    //if (BizAction.DoctorDetailsList == null)
                    //    BizAction.DoctorDetailsList = new List<clsDoctorVO>();

                    while (reader.Read())
                    {



                        clsDoctorVO ObjDoctorVO = BizAction.DoctorDetails;
                        BizAction.DoctorDetails.DoctorId = (long)reader["DoctorId"];
                        BizAction.DoctorDetails.DoctorName = reader["DoctorName"].ToString();
                        BizAction.DoctorDetails.DepartmentName = reader["DepartmentName"].ToString();
                        BizAction.DoctorDetails.UnitName = reader["Unit"].ToString();
                        BizAction.DoctorDetails.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        BizAction.DoctorDetails.FromTime = TimeSpan.Parse((string)DALHelper.HandleDBNull(reader["FromTime"]) == null ? "00:00:00" : (string)DALHelper.HandleDBNull(reader["FromTime"]));
                        BizAction.DoctorDetails.ToTime = TimeSpan.Parse((string)DALHelper.HandleDBNull(reader["ToTime"]) == null ? "00:00:00" : (string)DALHelper.HandleDBNull(reader["ToTime"]));
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
            return BizAction;
        }

        public override IValueObject AddDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddDoctorMasterBizActionVO BizActionobj = valueObject as clsAddDoctorMasterBizActionVO;


            if (BizActionobj.DoctorDetails.DoctorId == 0)
            {
                BizActionobj = AddDcotorMaster(BizActionobj, objUserVO);
            }

            else
            {
                BizActionobj = UpdateDcotorMaster(BizActionobj, objUserVO);

            }

            return BizActionobj;


        }

        private clsAddDoctorMasterBizActionVO AddDcotorMaster(clsAddDoctorMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsDoctorVO ObjDoctorVO = BizActionObj.DoctorDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorMaster");
                if (ObjDoctorVO.FirstName != null) ObjDoctorVO.FirstName = ObjDoctorVO.FirstName.Trim();
                dbServer.AddInParameter(command, "FirstName", DbType.String, ObjDoctorVO.FirstName);
                if (ObjDoctorVO.MiddleName != null) ObjDoctorVO.MiddleName = ObjDoctorVO.MiddleName.Trim();
                dbServer.AddInParameter(command, "MiddleName", DbType.String, ObjDoctorVO.MiddleName);
                if (ObjDoctorVO.LastName != null) ObjDoctorVO.LastName = ObjDoctorVO.LastName.Trim();
                dbServer.AddInParameter(command, "LastName", DbType.String, ObjDoctorVO.LastName);
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, ObjDoctorVO.DOB);

                if (ObjDoctorVO.Education != null) ObjDoctorVO.Education = ObjDoctorVO.Education.Trim();
                dbServer.AddInParameter(command, "Education", DbType.String, ObjDoctorVO.Education);

                if (ObjDoctorVO.Experience != null) ObjDoctorVO.Experience = ObjDoctorVO.Experience.Trim();
                dbServer.AddInParameter(command, "Experience", DbType.String, ObjDoctorVO.Experience);
                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, ObjDoctorVO.Specialization);
                dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, ObjDoctorVO.SubSpecialization);
                dbServer.AddInParameter(command, "DoctorTypeID", DbType.Int64, ObjDoctorVO.DoctorType);
                dbServer.AddInParameter(command, "EmailId", DbType.String, ObjDoctorVO.EmailId);
                dbServer.AddInParameter(command, "GenderId", DbType.Int64, ObjDoctorVO.GenderId);
                dbServer.AddInParameter(command, "Photo", DbType.Binary, ObjDoctorVO.Photo);
                //ROHINEE
                // dbServer.AddInParameter(command, "DigitalSignature", DbType.Binary, ObjDoctorVO.Signature);
                dbServer.AddInParameter(command, "DigitalSignature", DbType.Binary, ObjDoctorVO.Signature);
                //Added By Somnath
                dbServer.AddInParameter(command, "MaritalStatusId", DbType.Int64, ObjDoctorVO.MaritalStatusId);
                dbServer.AddInParameter(command, "ProvidentFund", DbType.String, ObjDoctorVO.ProvidentFund);
                dbServer.AddInParameter(command, "PermanentAccountNumber", DbType.String, ObjDoctorVO.PermanentAccountNumber);
                dbServer.AddInParameter(command, "AccessCardNumber", DbType.String, ObjDoctorVO.AccessCardNumber);
                dbServer.AddInParameter(command, "RegistrationNumber", DbType.String, ObjDoctorVO.RegistrationNumber);
                dbServer.AddInParameter(command, "DateofJoining", DbType.DateTime, ObjDoctorVO.DateofJoining);
                dbServer.AddInParameter(command, "EmployeeNumber", DbType.String, ObjDoctorVO.EmployeeNumber);
                //End

                //Added by Ashish Z.
                dbServer.AddInParameter(command, "DocCategoryId", DbType.Int64, ObjDoctorVO.DoctorCategoryId);
                //

                //***//
                dbServer.AddInParameter(command, "MarketingExecutivesID", DbType.Int64, ObjDoctorVO.MarketingExecutivesID);
                //----
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.DoctorId);

                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                int intStatus2 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.DoctorDetails.DoctorId = (long)dbServer.GetParameterValue(command, "ID");


                //foreach (var ObjDept in ObjDoctorVO.DepartmentDetails)
                //{

                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorDepartmentDetails");


                //    dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                //    dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, ObjDept.DepartmentID);
                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjDept.Status);
                //    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDept.ID);

                //    int iStatus = dbServer.ExecuteNonQuery(command1);
                //    ObjDept.ID = (long)dbServer.GetParameterValue(command1, "ID");

                //}


                //By Anjali.......................
                if (ObjDoctorVO.IsFromReferralDoctorChildWindow == true)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddDoctorAddressDetail");
                    dbServer.AddInParameter(command3, "DoctorId", DbType.Int64, BizActionObj.DoctorDetails.DoctorId);
                    dbServer.AddInParameter(command3, "AddressTypeID", DbType.Int64, BizActionObj.DoctorDetails.AddressTypeID);
                    //dbServer.AddInParameter(command, "BranchId", DbType.Int64, BizActionObj.objDoctorBankDetail.BranchId);
                    dbServer.AddInParameter(command3, "Name", DbType.String, BizActionObj.DoctorDetails.Name);
                    dbServer.AddInParameter(command3, "Address", DbType.String, BizActionObj.DoctorDetails.Address);
                    dbServer.AddInParameter(command3, "Contact1", DbType.String, BizActionObj.DoctorDetails.Contact1);
                    dbServer.AddInParameter(command3, "Contact2", DbType.String, BizActionObj.DoctorDetails.Contact2);
                    //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objDoctorBankDetail.ID);
                    dbServer.AddOutParameter(command3, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddParameter(command3, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, 0);
                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                }

                //....................................

                if (BizActionObj.SuccessStatus == 0)
                {
                    foreach (var ObjDept in ObjDoctorVO.UnitDepartmentDetails)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorDepartmentDetails");


                        dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                        dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, ObjDept.DepartmentID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, ObjDept.UnitID);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjDept.Status);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDept.ID);



                        int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                        ObjDept.ID = (long)dbServer.GetParameterValue(command1, "ID");

                    }

                    foreach (var ObjDept in ObjDoctorVO.UnitClassificationDetailsList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorClassificationDetails");


                        dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                        dbServer.AddInParameter(command1, "ClassificationID", DbType.Int64, ObjDept.ClassificationID);

                        int iStatus = dbServer.ExecuteNonQuery(command1, trans);


                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                throw;

            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;






        }

        private clsAddDoctorMasterBizActionVO UpdateDcotorMaster(clsAddDoctorMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsDoctorVO ObjDoctorVO = BizActionObj.DoctorDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorMaster");
                dbServer.AddInParameter(command, "ID", DbType.String, ObjDoctorVO.DoctorId);
                dbServer.AddInParameter(command, "FirstName", DbType.String, ObjDoctorVO.FirstName.Trim());
                dbServer.AddInParameter(command, "MiddleName", DbType.String, ObjDoctorVO.MiddleName.Trim());
                dbServer.AddInParameter(command, "LastName", DbType.String, ObjDoctorVO.LastName.Trim());
                dbServer.AddInParameter(command, "DOB", DbType.DateTime, ObjDoctorVO.DOB);
                dbServer.AddInParameter(command, "Education", DbType.String, ObjDoctorVO.Education.Trim());
                dbServer.AddInParameter(command, "Experience", DbType.String, ObjDoctorVO.Experience.Trim());
                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, ObjDoctorVO.Specialization);
                dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, ObjDoctorVO.SubSpecialization);
                dbServer.AddInParameter(command, "DoctorTypeID", DbType.Int64, ObjDoctorVO.DoctorType);
                dbServer.AddInParameter(command, "EmailId", DbType.String, ObjDoctorVO.EmailId);
                dbServer.AddInParameter(command, "GenderId", DbType.Int64, ObjDoctorVO.GenderId);
                dbServer.AddInParameter(command, "Photo", DbType.Binary, ObjDoctorVO.Photo);
                //ROHINEE
                dbServer.AddInParameter(command, "DigitalSignature", DbType.Binary, ObjDoctorVO.Signature);
                //Added By Somnath
                dbServer.AddInParameter(command, "MaritalStatusId", DbType.Int64, ObjDoctorVO.MaritalStatusId);
                dbServer.AddInParameter(command, "ProvidentFund", DbType.String, ObjDoctorVO.ProvidentFund);
                dbServer.AddInParameter(command, "PermanentAccountNumber", DbType.String, ObjDoctorVO.PermanentAccountNumber);
                dbServer.AddInParameter(command, "AccessCardNumber", DbType.String, ObjDoctorVO.AccessCardNumber);
                dbServer.AddInParameter(command, "RegistrationNumber", DbType.String, ObjDoctorVO.RegistrationNumber);
                dbServer.AddInParameter(command, "DateofJoining", DbType.DateTime, ObjDoctorVO.DateofJoining);
                dbServer.AddInParameter(command, "EmployeeNumber", DbType.String, ObjDoctorVO.EmployeeNumber);
                //End

                //Added by Ashish Z.
                dbServer.AddInParameter(command, "DocCategoryId", DbType.Int64, ObjDoctorVO.DoctorCategoryId);
                //
                //***//
                dbServer.AddInParameter(command, "MarketingExecutivesID", DbType.Int64, ObjDoctorVO.MarketingExecutivesID); 
                //----

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjDoctorVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.SuccessStatus == 0)
                {

                    if (ObjDoctorVO.UnitDepartmentDetails != null && ObjDoctorVO.UnitDepartmentDetails.Count > 0)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorDepartmentDetails");

                        dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2);
                    }



                    foreach (var ObjDept in ObjDoctorVO.UnitDepartmentDetails)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorDepartmentDetails");


                        dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                        dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, ObjDept.DepartmentID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, ObjDept.UnitID);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjDept.Status);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDept.ID);



                        int iStatus = dbServer.ExecuteNonQuery(command1);
                        ObjDept.ID = (long)dbServer.GetParameterValue(command1, "ID");

                    }

                    if (ObjDoctorVO.UnitClassificationDetailsList != null && ObjDoctorVO.UnitClassificationDetailsList.Count > 0)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorClassificationDetails");

                        dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2);
                    }

                    foreach (var ObjDept in ObjDoctorVO.UnitClassificationDetailsList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorClassificationDetails");


                        dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                        dbServer.AddInParameter(command1, "ClassificationID", DbType.Int64, ObjDept.ClassificationID);

                        int iStatus = dbServer.ExecuteNonQuery(command1);


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
            return BizActionObj;





        }

        public override IValueObject AddDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddDoctorDepartmentDetailsBizActionVO BizActionobj = valueObject as clsAddDoctorDepartmentDetailsBizActionVO;
            if (BizActionobj.DepartmentDetails.DoctorDepartmentDetailID == 0)
            {
                BizActionobj = AddDoctorDepartmentDetails(BizActionobj, objUserVO);
            }

            else
            {
                BizActionobj = UpdateDoctorDepartmentDetails(BizActionobj, objUserVO);

            }
            return BizActionobj;



        }

        private clsAddDoctorDepartmentDetailsBizActionVO AddDoctorDepartmentDetails(clsAddDoctorDepartmentDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsDoctorVO DoctorVO = BizActionObj.DepartmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorDepartmentDetails");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, DoctorVO.DoctorId);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, DoctorVO.DepartmentID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, DoctorVO.Status);


                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, DoctorVO.DoctorDepartmentDetailID);
                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.DepartmentDetails.DepartmentID = (long)dbServer.GetParameterValue(command, "ID");





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

        private clsAddDoctorDepartmentDetailsBizActionVO UpdateDoctorDepartmentDetails(clsAddDoctorDepartmentDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsDoctorVO ObjDoctorVO = BizActionObj.DepartmentDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorDepartmentDetails");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDoctorVO.DoctorDepartmentDetailID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorId);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, ObjDoctorVO.DepartmentID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);



                int intStatus = dbServer.ExecuteNonQuery(command);


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

        public override IValueObject GetDepartmentListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListForDoctorMasterBizActionVO BizActionObj = (clsGetDepartmentListForDoctorMasterBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentListForDoctorMaster");
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorVO>();
                    while (reader.Read())
                    {
                        clsDoctorVO DoctorVO = new clsDoctorVO();
                        DoctorVO.DoctorDepartmentDetailID = (long)reader["ID"];
                        DoctorVO.UnitName = reader["Unit"].ToString();
                        DoctorVO.UnitID = (long)reader["UnitID"];
                        DoctorVO.DepartmentName = reader["Department"].ToString();
                        DoctorVO.DepartmentID = (long)reader["DepartmentID"];
                        DoctorVO.DepartmentStatus = (bool)reader["Status"];

                        BizActionObj.DoctorDetails.Add(DoctorVO);
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

        public override IValueObject GetDoctorDetailListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDetailListForDoctorMasterBizActionVO BizActionObj = (clsGetDoctorDetailListForDoctorMasterBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorMasterDetailsList");

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");

                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");

                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");

                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorVO>();
                    while (reader.Read())
                    {
                        clsDoctorVO DoctorVO = new clsDoctorVO();
                        DoctorVO.DoctorId = (long)DALHelper.HandleDBNull(reader["ID"]);

                        DoctorVO.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);
                        DoctorVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        DoctorVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        DoctorVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        DoctorVO.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        DoctorVO.Education = (string)DALHelper.HandleDBNull(reader["Education"]);
                        DoctorVO.Experience = (string)DALHelper.HandleDBNull(reader["Experience"]);
                        DoctorVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
                        DoctorVO.SpecializationDis = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        DoctorVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationID"]);
                        DoctorVO.SubSpecializationDis = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                        DoctorVO.DoctorType = (long)DALHelper.HandleDBNull(reader["DoctorTypeID"]);
                        DoctorVO.DoctorTypeDis = (string)DALHelper.HandleDBNull(reader["DoctorType"]);
                        DoctorVO.DepartmentStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        DoctorVO.DoctorCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocCategoryId"])); //added by Ashish Z.
                        DoctorVO.IsDocAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDocAttached"])); //***//
                        BizActionObj.DoctorDetails.Add(DoctorVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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

        public override IValueObject GetDoctorDetailListForDoctorMasterByDoctorID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorDetailListForDoctorMasterByIDBizActionVO BizActionObj = (clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)valueObject;

            try
            {
                clsDoctorVO ObjDoctorVo = BizActionObj.DoctorDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorMasterDetailsListByDoctorID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.DoctorDetails == null)
                            BizActionObj.DoctorDetails = new clsDoctorVO();

                        BizActionObj.DoctorDetails.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        BizActionObj.DoctorDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.DoctorDetails.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        BizActionObj.DoctorDetails.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        BizActionObj.DoctorDetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        BizActionObj.DoctorDetails.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        BizActionObj.DoctorDetails.DateofJoining = (DateTime?)DALHelper.HandleDate(reader["DateOfJoining"]);
                        BizActionObj.DoctorDetails.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        BizActionObj.DoctorDetails.Experience = Convert.ToString(DALHelper.HandleDBNull(reader["Experience"]));
                        BizActionObj.DoctorDetails.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        BizActionObj.DoctorDetails.SpecializationDis = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        BizActionObj.DoctorDetails.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationID"]));
                        BizActionObj.DoctorDetails.SubSpecializationDis = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        BizActionObj.DoctorDetails.DoctorType = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorTypeID"]));
                        BizActionObj.DoctorDetails.DoctorTypeDis = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorType"]));
                        BizActionObj.DoctorDetails.DoctorCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocCategoryId"]));
                        BizActionObj.DoctorDetails.DoctorCategoryDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCategory"]));
                        BizActionObj.DoctorDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.DoctorDetails.GenderId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderId"]));
                        BizActionObj.DoctorDetails.Photo = (byte[])(DALHelper.HandleDBNull(reader["Photo"]));
                        //ROHINEE
                        if (DALHelper.HandleDBNull(reader["DigitalSignature"]) != null)
                            // BizActionObj.DoctorDetails.Signature = (byte[])(DALHelper.HandleDBNull(reader["DigitalSignature"]));
                            BizActionObj.DoctorDetails.Signature = (byte[])DALHelper.HandleDBNull(reader["DigitalSignature"]);
                        else
                            BizActionObj.DoctorDetails.Signature = null;



                        BizActionObj.DoctorDetails.MaritalStatusId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        BizActionObj.DoctorDetails.ProvidentFund = Convert.ToString(DALHelper.HandleDBNull(reader["PFNumber"]));
                        BizActionObj.DoctorDetails.PermanentAccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PANNumber"]));
                        BizActionObj.DoctorDetails.AccessCardNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccessCardNumber"]));
                        BizActionObj.DoctorDetails.RegistrationNumber = Convert.ToString(DALHelper.HandleDBNull(reader["RegestrationNumber"]));
                        BizActionObj.DoctorDetails.DateofJoining = (DateTime?)DALHelper.HandleDate(reader["DateOfJoining"]);
                        BizActionObj.DoctorDetails.EmailId = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                        BizActionObj.DoctorDetails.EmployeeNumber = Convert.ToString(DALHelper.HandleDBNull(reader["EmployeeNumber"]));

                        //Added by Somnath
                        BizActionObj.DoctorDetails.DoctorBankInformation.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        BizActionObj.DoctorDetails.DoctorBankInformation.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        BizActionObj.DoctorDetails.DoctorBankInformation.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        BizActionObj.DoctorDetails.DoctorBankInformation.AccountTypeId = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AccountType"]));
                        BizActionObj.DoctorDetails.DoctorBankInformation.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        BizActionObj.DoctorDetails.DoctorBankInformation.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        //End

                        //***//
                        BizActionObj.DoctorDetails.MarketingExecutivesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MarketingExecutivesID"]));
                    }
                }

                reader.NextResult();

                //if (reader.HasRows)
                //{
                //    BizActionObj.DoctorDetails.DepartmentDetails = new List<clsDepartmentsDetailsVO>();
                //    while (reader.Read())
                //    {
                //        clsDepartmentsDetailsVO objDepartment = new clsDepartmentsDetailsVO();

                //        //objDepartment.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                //        objDepartment.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                //        objDepartment.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                //       objDepartment.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                //        BizActionObj.DoctorDetails.DepartmentDetails.Add(objDepartment);
                //    }
                //}


                if (reader.HasRows)
                {
                    BizActionObj.DoctorDetails.UnitDepartmentDetails = new List<clsUnitDepartmentsDetailsVO>();
                    while (reader.Read())
                    {
                        clsUnitDepartmentsDetailsVO objDepartment = new clsUnitDepartmentsDetailsVO();

                        //objDepartment.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        //  objDepartment.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        objDepartment.DepartmentID = (long)DALHelper.HandleDBNull(reader["DeptMasterId"]);
                        objDepartment.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]));
                        objDepartment.UnitID = (long)DALHelper.HandleDBNull(reader["UnitMasterId"]);

                        if (DALHelper.HandleDBNull(reader["DoctorDepartmentstatus"]) != null)
                        {
                            objDepartment.Status = (bool)DALHelper.HandleDBNull(reader["DoctorDepartmentstatus"]);
                        }
                        else
                            objDepartment.Status = false;
                        objDepartment.UnitName = (string)DALHelper.HandleDBNull(reader["Unit"]);
                        BizActionObj.DoctorDetails.UnitDepartmentDetails.Add(objDepartment);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    BizActionObj.DoctorDetails.UnitClassificationDetailsList = new List<clsUnitClassificationsDetailsVO>();
                    while (reader.Read())
                    {
                        clsUnitClassificationsDetailsVO objClassification = new clsUnitClassificationsDetailsVO();

                        //objDepartment.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objClassification.ClassificationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassificationID"]));
                        objClassification.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objClassification.IsAvailableStr = Convert.ToString(DALHelper.HandleDBNull(reader["IsAvailable"]));
                        if (objClassification.IsAvailableStr == "true")
                        {
                            objClassification.IsAvailable = true;
                        }
                        else
                            objClassification.IsAvailable = false;


                        BizActionObj.DoctorDetails.UnitClassificationDetailsList.Add(objClassification);
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

        public override IValueObject GetDepartmentListForDoctorMasterByDoctorID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionObj = (clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentListByDoctorID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorVO>();
                    while (reader.Read())
                    {
                        clsDoctorVO objclsDoctorVO = new clsDoctorVO();

                        objclsDoctorVO.DoctorId = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        objclsDoctorVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        objclsDoctorVO.DepartmentCode = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objclsDoctorVO.DepartmentName = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objclsDoctorVO.DepartmentStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.DoctorDetails.Add(objclsDoctorVO);
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


        public override IValueObject GetDepartmentListByUnitID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionObj = (clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentListByUnitID");
                DbDataReader reader;

                if (BizActionObj.IsClinical == true && BizActionObj.UnitID > 0)     // if Condition add, to filter Departments unitwise  02032017
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                else
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "IsClinical", DbType.Boolean, BizActionObj.IsClinical);// flag use to Show/not Clinical Departments  02032017

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterListItem == null)
                        BizActionObj.MasterListItem = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objList = new MasterListItem();
                        objList.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objList.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.MasterListItem.Add(objList);
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

        public override IValueObject DeleteDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsDeleteDoctorDepartmentDetailsBizActionVO BizActionobj = valueObject as clsDeleteDoctorDepartmentDetailsBizActionVO;

            try
            {
                clsDoctorVO objDoctorVO = BizActionobj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorDepartmentDetails");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDoctorVO.DoctorId);
                int intStatus = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return BizActionobj;
        }

        public override IValueObject AddDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddDoctorBankInfoBizActionVO BizActionObj = (clsAddDoctorBankInfoBizActionVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorBankDetail");
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.objDoctorBankDetail.DoctorId);
                dbServer.AddInParameter(command, "BankId", DbType.Int64, BizActionObj.objDoctorBankDetail.BankId);
                dbServer.AddInParameter(command, "BranchId", DbType.Int64, BizActionObj.objDoctorBankDetail.BranchId);
                dbServer.AddInParameter(command, "AccountNumber", DbType.String, BizActionObj.objDoctorBankDetail.AccountNumber);
                dbServer.AddInParameter(command, "AccountType", DbType.Boolean, BizActionObj.objDoctorBankDetail.AccountType);
                dbServer.AddInParameter(command, "BranchAddress", DbType.String, BizActionObj.objDoctorBankDetail.BranchAddress);
                dbServer.AddInParameter(command, "MICRNumber", DbType.Int64, BizActionObj.objDoctorBankDetail.MICRNumber);
                dbServer.AddInParameter(command, "IFSCCode", DbType.String, BizActionObj.objDoctorBankDetail.IFSCCode);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                BizActionObj.objDoctorBankDetail.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject UpdateDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsUpdateDoctorBankInfoVO BizActionObj = (clsUpdateDoctorBankInfoVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorBankDetail");
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.objDoctorBankDetail.DoctorId);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.objDoctorBankDetail.ID);
                dbServer.AddInParameter(command, "BankId", DbType.Int64, BizActionObj.objDoctorBankDetail.BankId);
                dbServer.AddInParameter(command, "BranchId", DbType.Int64, BizActionObj.objDoctorBankDetail.BranchId);
                dbServer.AddInParameter(command, "AccountNumber", DbType.String, BizActionObj.objDoctorBankDetail.AccountNumber);
                dbServer.AddInParameter(command, "AccountType", DbType.Boolean, BizActionObj.objDoctorBankDetail.AccountType);
                dbServer.AddInParameter(command, "BranchAddress", DbType.String, BizActionObj.objDoctorBankDetail.BranchAddress);
                dbServer.AddInParameter(command, "MICRNumber", DbType.Int64, BizActionObj.objDoctorBankDetail.MICRNumber);
                dbServer.AddInParameter(command, "IFSCCode", DbType.String, BizActionObj.objDoctorBankDetail.IFSCCode);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objDoctorBankDetail.ID);
                //dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                //BizActionObj.objDoctorBankDetail.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject GetDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorBankInfoBizActionVO BizActionObj = (clsGetDoctorBankInfoBizActionVO)valueObject;

            try
            {
                clsDoctorBankInfoVO ObjDoctorBankVo = BizActionObj.objDoctorBankDetail;
                //clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorBankInfo");
                DbDataReader reader;
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorID);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorBankDetailList == null)
                        BizActionObj.DoctorBankDetailList = new List<clsDoctorBankInfoVO>();
                    while (reader.Read())
                    {

                        clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                        objdoctoebankinfo.DoctorId = (long)DALHelper.HandleDBNull(reader["DoctorId"]);
                        objdoctoebankinfo.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objdoctoebankinfo.BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"]));
                        objdoctoebankinfo.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        objdoctoebankinfo.BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"]));
                        objdoctoebankinfo.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        objdoctoebankinfo.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        objdoctoebankinfo.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        objdoctoebankinfo.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        objdoctoebankinfo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objdoctoebankinfo.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        objdoctoebankinfo.IFSCCode = Convert.ToString(DALHelper.HandleDBNull(reader["IFSCCode"])); //***//
                        BizActionObj.DoctorBankDetailList.Add(objdoctoebankinfo);

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

        public override IValueObject GetDoctorBankInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorBankInfoByIdVO BizActionObj = (clsGetDoctorBankInfoByIdVO)valueObject;

            try
            {
                //clsDoctorBankInfoVO ObjDoctorBankVo = BizActionObj.objDoctorBankDetail;
                //clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorBankInfoById");
                DbDataReader reader;

                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    //if (BizActionObj.DoctorBankDetailList == null)
                    //    BizActionObj.DoctorBankDetailList = new List<clsDoctorBankInfoVO>();
                    while (reader.Read())
                    {
                        BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
                        //clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                        BizActionObj.objDoctorBankDetail.DoctorId = (long)DALHelper.HandleDBNull(reader["DoctorId"]);
                        BizActionObj.objDoctorBankDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.objDoctorBankDetail.BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"]));
                        BizActionObj.objDoctorBankDetail.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        BizActionObj.objDoctorBankDetail.BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["BankBranchName"]));
                        BizActionObj.objDoctorBankDetail.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        BizActionObj.objDoctorBankDetail.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        BizActionObj.objDoctorBankDetail.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        BizActionObj.objDoctorBankDetail.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        BizActionObj.objDoctorBankDetail.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        BizActionObj.objDoctorBankDetail.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        BizActionObj.objDoctorBankDetail.IFSCCode = Convert.ToString(DALHelper.HandleDBNull(reader["IFSCCode"])); //***//
                        //BizActionObj.objDoctorBankDetail.Add(objdoctoebankinfo);

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

        public override IValueObject AddDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddDoctorAddressInfoBizActionVO BizActionObj = (clsAddDoctorAddressInfoBizActionVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorAddressDetail");
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.objDoctorBankDetail.DoctorId);
                dbServer.AddInParameter(command, "AddressTypeID", DbType.Int64, BizActionObj.objDoctorBankDetail.AddressTypeID);
                //dbServer.AddInParameter(command, "BranchId", DbType.Int64, BizActionObj.objDoctorBankDetail.BranchId);
                dbServer.AddInParameter(command, "Name", DbType.String, BizActionObj.objDoctorBankDetail.Name);
                dbServer.AddInParameter(command, "Address", DbType.String, BizActionObj.objDoctorBankDetail.Address);
                dbServer.AddInParameter(command, "Contact1", DbType.String, BizActionObj.objDoctorBankDetail.Contact1);
                dbServer.AddInParameter(command, "Contact2", DbType.String, BizActionObj.objDoctorBankDetail.Contact2);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //  BizActionObj.objDoctorBankDetail.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.objDoctorBankDetail.ID = Convert.ToInt64(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "ID")));
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

        public override IValueObject GetDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorAddressInfoBizActionVO BizActionObj = (clsGetDoctorAddressInfoBizActionVO)valueObject;

            try
            {
                clsDoctorAddressInfoVO ObjDoctorBankVo = BizActionObj.objDoctorAddressDetail;
                //clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorAddressInfo");
                DbDataReader reader;
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorID);
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorAddressDetailList == null)
                        BizActionObj.DoctorAddressDetailList = new List<clsDoctorAddressInfoVO>();
                    while (reader.Read())
                    {


                        clsDoctorAddressInfoVO objdoctoebankinfo = new clsDoctorAddressInfoVO();
                        objdoctoebankinfo.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objdoctoebankinfo.DoctorId = (long)DALHelper.HandleDBNull(reader["DoctorId"]);
                        objdoctoebankinfo.AddressType = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objdoctoebankinfo.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"]));
                        objdoctoebankinfo.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        objdoctoebankinfo.Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        objdoctoebankinfo.Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"]));
                        objdoctoebankinfo.Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]));
                        //objdoctoebankinfo.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        //objdoctoebankinfo.AccountTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["AccountType"]));
                        //objdoctoebankinfo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        //objdoctoebankinfo.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        BizActionObj.DoctorAddressDetailList.Add(objdoctoebankinfo);

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

        public override IValueObject GetDoctorAddressInfoById(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorAddressInfoByIdVO BizActionObj = (clsGetDoctorAddressInfoByIdVO)valueObject;

            try
            {
                //clsDoctorBankInfoVO ObjDoctorBankVo = BizActionObj.objDoctorBankDetail;
                //clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorAddressInfoById");
                DbDataReader reader;

                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "AddressTypeID", DbType.Int64, BizActionObj.AddressTypeId);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    //if (BizActionObj.DoctorBankDetailList == null)
                    //    BizActionObj.DoctorBankDetailList = new List<clsDoctorBankInfoVO>();
                    while (reader.Read())
                    {
                        BizActionObj.objDoctorAddressDetail = new clsDoctorAddressInfoVO();
                        ////clsDoctorBankInfoVO objdoctoebankinfo = new clsDoctorBankInfoVO();
                        BizActionObj.objDoctorAddressDetail.DoctorId = (long)DALHelper.HandleDBNull(reader["DoctorId"]);

                        BizActionObj.objDoctorAddressDetail.AddressTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressType"]));
                        BizActionObj.objDoctorAddressDetail.Address = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        BizActionObj.objDoctorAddressDetail.Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        BizActionObj.objDoctorAddressDetail.Contact1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"]));
                        BizActionObj.objDoctorAddressDetail.Contact2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"]));

                        // BizActionObj.DoctorAddressDetailList.Add(objdoctoebankinfo);

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

        public override IValueObject UpdateDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsUpdateDoctorAddressInfoVO BizActionObj = (clsUpdateDoctorAddressInfoVO)valueObject;
            //BizActionObj.objDoctorBankDetail = new clsDoctorBankInfoVO();
            try
            {


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorAddressDetail");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.objDoctorAddressDetail.ID);
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.objDoctorAddressDetail.DoctorId);
                dbServer.AddInParameter(command, "AddressTypeID", DbType.Int64, BizActionObj.objDoctorAddressDetail.AddressTypeID);
                dbServer.AddInParameter(command, "Name", DbType.String, BizActionObj.objDoctorAddressDetail.Name);
                dbServer.AddInParameter(command, "Address", DbType.String, BizActionObj.objDoctorAddressDetail.Address);
                dbServer.AddInParameter(command, "Contact1", DbType.String, BizActionObj.objDoctorAddressDetail.Contact1);
                dbServer.AddInParameter(command, "Contact2", DbType.String, BizActionObj.objDoctorAddressDetail.Contact2);

                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                //dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                //BizActionObj.objDoctorAddressDetail.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        public override IValueObject GetClassificationListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetClassificationListForDoctorMasterBizActionVO BizActionObj = (clsGetClassificationListForDoctorMasterBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetClassificationListForDoctorMaster");
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorVO>();
                    while (reader.Read())
                    {
                        clsDoctorVO DoctorVO = new clsDoctorVO();
                        // DoctorVO.UnitClassificationDetails.ID = (long)reader["ID"];
                        //DoctorVO.UnitClassificationDetails.UnitID = reader["Unit"].ToString();
                        //DoctorVO.UnitID = (long)reader["UnitID"];
                        DoctorVO.UnitClassificationName = reader["Description"].ToString();
                        DoctorVO.ClassificationID = (long)reader["ID"];
                        DoctorVO.Status = (bool)reader["Status"];

                        BizActionObj.DoctorDetails.Add(DoctorVO);
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

        public override IValueObject FillDoctorCombo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorListBizActionVO BizActionObj = (clsGetDoctorListBizActionVO)valueObject;

            try
            {
                DbCommand command;
                if (BizActionObj.IsInternal)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetAllInternalDoctorList");
                    dbServer.AddInParameter(command, "IsInternal", DbType.Boolean,true);
                }
                else if (BizActionObj.IsExternal)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetAllInternalDoctorList");
                    dbServer.AddInParameter(command, "IsInternal", DbType.Boolean, false);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetAllDoctorList");
                }
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["ID"], reader["DoctorName"].ToString()));
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

        public override IValueObject GetDoctorWaiverDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorWaiverDetailListBizActionVO BizActionObj = (clsGetDoctorWaiverDetailListBizActionVO)valueObject;
            if (BizActionObj.PageName == "Doctor Waiver")
            {
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorWaiverDetailsList");

                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    if (BizActionObj.DepartmentID > 0)
                    {
                        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                    }
                    if (BizActionObj.DoctorID > 0)
                    {
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                    }
                    if (BizActionObj.TariffID > 0)
                    {
                        dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                    }
                    if (BizActionObj.ServiceID > 0)
                    {
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                    }
                    //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    //    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");

                    //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    //    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");

                    //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    //    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorWaiverDetails == null)
                            BizActionObj.DoctorWaiverDetails = new List<clsDoctorWaiverDetailVO>();
                        while (reader.Read())
                        {
                            clsDoctorWaiverDetailVO DoctorWaiverInformation = new clsDoctorWaiverDetailVO();
                            DoctorWaiverInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            DoctorWaiverInformation.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            DoctorWaiverInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            DoctorWaiverInformation.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));

                            DoctorWaiverInformation.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            DoctorWaiverInformation.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                            DoctorWaiverInformation.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            DoctorWaiverInformation.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            DoctorWaiverInformation.EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorShareAmount"]));
                            DoctorWaiverInformation.EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorSharePercentage"]));
                            DoctorWaiverInformation.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                            DoctorWaiverInformation.TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                            DoctorWaiverInformation.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            DoctorWaiverInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                            DoctorWaiverInformation.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            DoctorWaiverInformation.EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyServiceRate"]));

                            DoctorWaiverInformation.WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["WaiverDays"]));
                            //DoctorVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationID"]);
                            //DoctorVO.SubSpecializationDis = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                            //DoctorVO.DoctorType = (long)DALHelper.HandleDBNull(reader["DoctorTypeID"]);
                            //DoctorVO.DoctorTypeDis = (string)DALHelper.HandleDBNull(reader["DoctorType"]);
                            //DoctorVO.DepartmentStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);

                            BizActionObj.DoctorWaiverDetails.Add(DoctorWaiverInformation);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();

                }


                catch (Exception ex)
                {

                    throw;
                }

                finally
                {

                }
            }
            else if (BizActionObj.PageName == "Department Waiver")
            {
                try
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentWaiverDetailsList");



                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    if (BizActionObj.DepartmentID > 0)
                    {
                        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                    }
                    if (BizActionObj.DoctorID > 0)
                    {
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                    }
                    if (BizActionObj.TariffID > 0)
                    {
                        dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                    }
                    if (BizActionObj.ServiceID > 0)
                    {
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                    }
                    //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    //    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");

                    //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    //    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");

                    //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    //    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorWaiverDetails == null)
                            BizActionObj.DoctorWaiverDetails = new List<clsDoctorWaiverDetailVO>();
                        while (reader.Read())
                        {
                            clsDoctorWaiverDetailVO DoctorWaiverInformation = new clsDoctorWaiverDetailVO();
                            DoctorWaiverInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                            DoctorWaiverInformation.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));

                            DoctorWaiverInformation.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            DoctorWaiverInformation.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                            DoctorWaiverInformation.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            DoctorWaiverInformation.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            DoctorWaiverInformation.EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorShareAmount"]));
                            DoctorWaiverInformation.EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorSharePercentage"]));
                            DoctorWaiverInformation.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                            DoctorWaiverInformation.TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                            DoctorWaiverInformation.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            DoctorWaiverInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                            DoctorWaiverInformation.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            DoctorWaiverInformation.EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyServiceRate"]));

                            DoctorWaiverInformation.WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["WaiverDays"]));
                            //DoctorVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationID"]);
                            //DoctorVO.SubSpecializationDis = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                            //DoctorVO.DoctorType = (long)DALHelper.HandleDBNull(reader["DoctorTypeID"]);
                            //DoctorVO.DoctorTypeDis = (string)DALHelper.HandleDBNull(reader["DoctorType"]);
                            //DoctorVO.DepartmentStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);

                            BizActionObj.DoctorWaiverDetails.Add(DoctorWaiverInformation);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }

                catch (Exception ex)
                {

                    throw;
                }

                finally
                {

                }
            }

            else
            {
                try
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCenterWaiverDetailsList");



                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    if (BizActionObj.DepartmentID > 0)
                    {
                        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                    }
                    if (BizActionObj.DoctorID > 0)
                    {
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                    }
                    if (BizActionObj.TariffID > 0)
                    {
                        dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                    }
                    if (BizActionObj.ServiceID > 0)
                    {
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                    }

                    //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    //    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");

                    //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    //    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");

                    //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    //    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorWaiverDetails == null)
                            BizActionObj.DoctorWaiverDetails = new List<clsDoctorWaiverDetailVO>();
                        while (reader.Read())
                        {
                            clsDoctorWaiverDetailVO DoctorWaiverInformation = new clsDoctorWaiverDetailVO();
                            DoctorWaiverInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));



                            DoctorWaiverInformation.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            DoctorWaiverInformation.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                            DoctorWaiverInformation.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            DoctorWaiverInformation.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            DoctorWaiverInformation.EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorShareAmount"]));
                            DoctorWaiverInformation.EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorSharePercentage"]));
                            DoctorWaiverInformation.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                            DoctorWaiverInformation.TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                            DoctorWaiverInformation.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            DoctorWaiverInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                            DoctorWaiverInformation.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            DoctorWaiverInformation.EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyServiceRate"]));

                            DoctorWaiverInformation.WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["WaiverDays"]));
                            //DoctorVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationID"]);
                            //DoctorVO.SubSpecializationDis = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                            //DoctorVO.DoctorType = (long)DALHelper.HandleDBNull(reader["DoctorTypeID"]);
                            //DoctorVO.DoctorTypeDis = (string)DALHelper.HandleDBNull(reader["DoctorType"]);
                            //DoctorVO.DepartmentStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);

                            BizActionObj.DoctorWaiverDetails.Add(DoctorWaiverInformation);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }

                catch (Exception ex)
                {

                    throw;
                }

                finally
                {

                }
            }


            return BizActionObj;

        }

        public override IValueObject GetDoctorTariffServiceDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorTariffServiceListBizActionVO BizActionObj = (clsGetDoctorTariffServiceListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceMasterList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TeriffServiceDetail.TariffID);
                if (BizActionObj.TeriffServiceDetail.TariffID > 0)
                {
                    //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.TeriffServiceDetailList == null)
                            BizActionObj.TeriffServiceDetailList = new List<clsDoctorWaiverDetailVO>();
                        while (reader.Read())
                        {
                            clsDoctorWaiverDetailVO objclsDoctorTAriffServiceVO = new clsDoctorWaiverDetailVO();

                            objclsDoctorTAriffServiceVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                            objclsDoctorTAriffServiceVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceId"]);
                            objclsDoctorTAriffServiceVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                            objclsDoctorTAriffServiceVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));

                            BizActionObj.TeriffServiceDetailList.Add(objclsDoctorTAriffServiceVO);
                        }
                    }
                    reader.Close();
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

        public override IValueObject AddDoctorWaiverDetails(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddDoctorWaiverDetailsBizActionVO BizActionObj = valueObject as clsAddDoctorWaiverDetailsBizActionVO;

            try
            {
                clsDoctorWaiverDetailVO ObjDoctorVO = BizActionObj.DoctorWaiverDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorWaiverDetails");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);

                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, ObjDoctorVO.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, ObjDoctorVO.TariffID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, ObjDoctorVO.ServiceID);


                dbServer.AddInParameter(command, "WaiverDays", DbType.Int64, ObjDoctorVO.WaiverDays);
                dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, ObjDoctorVO.Rate);
                dbServer.AddInParameter(command, "EmergencyServiceRate", DbType.Decimal, ObjDoctorVO.EmergencyRate);
                dbServer.AddInParameter(command, "DoctorSharePercentage", DbType.Decimal, ObjDoctorVO.DoctorSharePercentage);
                dbServer.AddInParameter(command, "EmergencyDoctorSharePercentage", DbType.Decimal, ObjDoctorVO.EmergencyDoctorSharePercentage);
                dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Decimal, ObjDoctorVO.DoctorShareAmount);
                dbServer.AddInParameter(command, "EmergencyDoctorShareAmount", DbType.Decimal, ObjDoctorVO.EmergencyDoctorShareAmount);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.ID);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.DoctorWaiverDetails.ID = (long)dbServer.GetParameterValue(command, "ID");




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

        public override IValueObject GetDoctorWaiverDetailListByID(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorWaiverDetailListByIDBizActionVO BizActionObj = (clsGetDoctorWaiverDetailListByIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorWaiverDetailByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.DoctorWaiverDetails.ID);
                //dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorWaiverDetails.DoctorID);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DoctorWaiverDetails.DepartmentID);
                //dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.DoctorWaiverDetails.TariffID);
                //dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.DoctorWaiverDetails.ServiceID);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorWaiverDetailsList == null)
                        BizActionObj.DoctorWaiverDetailsList = new List<clsDoctorWaiverDetailVO>();
                    while (reader.Read())
                    {
                        //clsDoctorWaiverDetailVO objclsDoctorVO = new clsDoctorWaiverDetailVO();
                        //BizActionObj.DoctorWaiverDetails
                        BizActionObj.DoctorWaiverDetails.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        BizActionObj.DoctorWaiverDetails.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        BizActionObj.DoctorWaiverDetails.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        BizActionObj.DoctorWaiverDetails.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        BizActionObj.DoctorWaiverDetails.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"]));
                        BizActionObj.DoctorWaiverDetails.WaiverDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["WaiverDays"]));
                        BizActionObj.DoctorWaiverDetails.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        BizActionObj.DoctorWaiverDetails.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        BizActionObj.DoctorWaiverDetails.EmergencyDoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorShareAmount"]));
                        BizActionObj.DoctorWaiverDetails.EmergencyDoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyDoctorSharePerCentage"]));
                        BizActionObj.DoctorWaiverDetails.EmergencyRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EmergencyServiceRate"]));
                        //BizActionObj.DoctorWaiverDetailsList.Add(objclsDoctorVO);
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

        public override IValueObject UpdateDoctorWaiverInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsUpdateDoctorWaiverDetailsBizActionVO BizActionObj = valueObject as clsUpdateDoctorWaiverDetailsBizActionVO;

            try
            {
                clsDoctorWaiverDetailVO ObjDoctorVO = BizActionObj.objDoctorWaiverDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorWaiverDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDoctorVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);

                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, ObjDoctorVO.DepartmentID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, ObjDoctorVO.TariffID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, ObjDoctorVO.ServiceID);
                dbServer.AddInParameter(command, "WaiverDays", DbType.Int64, ObjDoctorVO.WaiverDays);
                dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, ObjDoctorVO.Rate);
                dbServer.AddInParameter(command, "EmergencyServiceRate", DbType.Decimal, ObjDoctorVO.EmergencyRate);
                dbServer.AddInParameter(command, "DoctorSharePercentage", DbType.Decimal, ObjDoctorVO.DoctorSharePercentage);
                dbServer.AddInParameter(command, "EmergencyDoctorSharePercentage", DbType.Decimal, ObjDoctorVO.EmergencyDoctorSharePercentage);
                dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Decimal, ObjDoctorVO.DoctorShareAmount);
                dbServer.AddInParameter(command, "EmergencyDoctorShareAmount", DbType.Decimal, ObjDoctorVO.EmergencyDoctorShareAmount);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjDoctorVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.ID);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                //BizActionObj.objDoctorWaiverDetail.ID = (long)dbServer.GetParameterValue(command, "ID");




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
        public override IValueObject GetPendingDoctorDetail(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPendingDoctorDetails BizActionObj = (clsGetPendingDoctorDetails)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPendingDoctorMasterDetailsList");

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");

                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName + "%");

                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");

                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                if (BizActionObj.SpecializationID != null && BizActionObj.SpecializationID.Length != 0)
                {
                    dbServer.AddInParameter(command, "SpecializationID", DbType.String, BizActionObj.SpecializationID);
                }
                else
                {
                    dbServer.AddInParameter(command, "SpecializationID", DbType.String, null);
                }
                if (BizActionObj.SubSpecializationID != null && BizActionObj.SubSpecializationID.Length != 0)
                {
                    dbServer.AddInParameter(command, "SubSpecializationID", DbType.String, BizActionObj.SubSpecializationID);
                }
                else
                {
                    dbServer.AddInParameter(command, "SubSpecializationID", DbType.String, null);
                }
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorVO>();
                    while (reader.Read())
                    {
                        clsDoctorVO DoctorVO = new clsDoctorVO();
                        DoctorVO.DoctorId = (long)DALHelper.HandleDBNull(reader["ID"]);

                        DoctorVO.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);
                        DoctorVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        DoctorVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        DoctorVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        DoctorVO.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        DoctorVO.Education = (string)DALHelper.HandleDBNull(reader["Education"]);
                        DoctorVO.Experience = (string)DALHelper.HandleDBNull(reader["Experience"]);
                        DoctorVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
                        DoctorVO.SpecializationDis = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        DoctorVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationID"]);
                        DoctorVO.SubSpecializationDis = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                        DoctorVO.DoctorType = (long)DALHelper.HandleDBNull(reader["DoctorTypeID"]);
                        DoctorVO.DoctorTypeDis = (string)DALHelper.HandleDBNull(reader["DoctorType"]);
                        DoctorVO.DepartmentStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.DoctorDetails.Add(DoctorVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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
        public override IValueObject GetExistingDoctorList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetExistingDoctorShareDetails BizActionObj = (clsGetExistingDoctorShareDetails)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetExistingDoctorMasterList");

                dbServer.AddInParameter(command, "DOCTORID", DbType.String, BizActionObj.DoctorIDs);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorList == null)
                        BizActionObj.DoctorList = new List<clsDoctorShareServicesDetailsVO>();
                    while (reader.Read())
                    {
                        clsDoctorShareServicesDetailsVO DoctorVO = new clsDoctorShareServicesDetailsVO();
                        DoctorVO.DoctorId = (long)DALHelper.HandleIntegerNull(reader["DoctorID"]);
                        DoctorVO.ModalityID = (long)DALHelper.HandleIntegerNull(reader["ModalityID"]);
                        DoctorVO.TariffID = (long)DALHelper.HandleIntegerNull(reader["TariffID"]);
                        BizActionObj.DoctorList.Add(DoctorVO);
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
        public override IValueObject UpdateDoctorShareInfo(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateDoctorShareServiceBizActionVO BizAction = valueObject as clsUpdateDoctorShareServiceBizActionVO;
            try
            {

                foreach (var item in BizAction.objServiceList)
                {

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateShareDetails");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceId);
                    dbServer.AddInParameter(command, "ModalityID", DbType.Int64, item.ModalityID);
                    dbServer.AddInParameter(command, "TariffID", DbType.Int64, item.TariffID);
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorId);
                    dbServer.AddInParameter(command, "SharePercentage", DbType.Decimal, item.DoctorSharePercentage);
                    dbServer.AddInParameter(command, "ShareAmount", DbType.Decimal, item.DoctorShareAmount);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;


        }
        public override IValueObject AddDoctorShareDetails(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddDoctorShareDetailsBizActionVO BizActionObj = valueObject as clsAddDoctorShareDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            if (BizActionObj.IsCompanyForm == true)
            {
                //============================Save the data of Company Share Details===============================================
                try
                {
                    clsDoctorShareServicesDetailsVO ObjDoctorVO = BizActionObj.DoctorShareDetails;


                    foreach (var item in BizActionObj.DoctorShareInfoList)
                    {

                        if (item.IsSelected == true)
                        {
                            con = dbServer.CreateConnection();
                            con.Open();
                            trans = con.BeginTransaction();
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCompanyandAssCompShareDetails");

                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);
                            dbServer.AddInParameter(command, "CompanyID", DbType.Int64, item.CompanyID);
                            dbServer.AddInParameter(command, "AssCompanyID", DbType.Int64, item.AssCompanyID);

                            dbServer.AddInParameter(command, "TariffID", DbType.Int64, item.TariffID);
                            dbServer.AddInParameter(command, "TariffServiceID", DbType.Int64, item.TariffServiceID);
                            dbServer.AddInParameter(command, "ServiceID", DbType.String, item.ServiceId);
                            dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, item.SpecializationID);

                            dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, item.ServiceRate);
                            dbServer.AddInParameter(command, "SharePercentage", DbType.Decimal, item.DoctorSharePercentage);
                            dbServer.AddInParameter(command, "ShareAmount", DbType.Decimal, item.DoctorShareAmount);

                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                            dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                            //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.ID);

                            int intStatus2 = dbServer.ExecuteNonQuery(command);
                            BizActionObj.DoctorShareDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                //============================End of the Company Share Details===============================================
            }
            else
            {
                //============================Save the data of Doctor Share Details===============================================
                //try
                //{
                //    clsDoctorShareServicesDetailsVO ObjDoctorVO = BizActionObj.DoctorShareDetails;


                //    if (BizActionObj.DoctorShareInfoList != null && BizActionObj.DoctorShareInfoList.Count > 0)
                //    {

                //        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorShareServices");

                //        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);
                //        dbServer.AddInParameter(command3, "DepartmentID", DbType.Int64, ObjDoctorVO.DepartmentId);
                //        dbServer.AddInParameter(command3, "SpecializationID", DbType.Int64, ObjDoctorVO.SpecializationID);
                //        dbServer.AddInParameter(command3, "TariffID", DbType.Int64, ObjDoctorVO.TariffID);

                //        int intStatus2 = dbServer.ExecuteNonQuery(command3);
                //    }

                //    foreach (var item in BizActionObj.DoctorShareInfoList)
                //    {

                //        if (item.IsSelected == true)
                //        {
                //            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorShareDetails");

                //            dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);
                //            dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, ObjDoctorVO.DepartmentId);
                //            dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorId);
                //            dbServer.AddInParameter(command, "TariffID", DbType.Int64, item.TariffID);
                //            dbServer.AddInParameter(command, "ServiceID", DbType.String, item.ServiceId);
                //            dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, item.SpecializationID);


                //            dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, item.ServiceRate);
                //            dbServer.AddInParameter(command, "DoctorSharePercentage", DbType.Decimal, item.DoctorSharePercentage);
                //            dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Decimal, item.DoctorShareAmount);

                //            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                //            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                //            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                //            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                //            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //            dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);

                //            dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.ID);
                //            int intStatus2 = dbServer.ExecuteNonQuery(command);
                //            BizActionObj.DoctorShareDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                //        }
                //    }


                //}
                //catch (Exception)
                //{
                //    throw;
                //}
                try
                {
                    // clsDoctorShareServicesDetailsVO ObjDoctorVO = BizActionObj.DoctorShareDetails;
                    clsDoctorShareServicesDetailsVO ObjDoctorVO = BizActionObj.DoctorShareDetails;

                    //if (BizActionObj.DoctorShareInfoList.Count() > 0)
                    //{


                    // ModalityWise
                    if (BizActionObj.ISShareModalityWise == true)
                    {
                        //For All Modality
                        if (BizActionObj.IsApplyToallDoctorWithAllTariffAndAllModality == true) //origionally !=
                        {
                            foreach (var item in BizActionObj.DoctorShareInfoList)
                            {

                                if (item.IsSelected == true)
                                {
                                    con = dbServer.CreateConnection();
                                    con.Open();
                                    trans = con.BeginTransaction();

                                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDoctorShareDetails");

                                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorId);
                                    dbServer.AddInParameter(command, "IsApplyToallDoctorWithAllTariffAndAllModality", DbType.Boolean, BizActionObj.IsApplyToallDoctorWithAllTariffAndAllModality);
                                    dbServer.AddInParameter(command, "IsApplyToallDoctor", DbType.Boolean, BizActionObj.IsAllDoctorShate);
                                    dbServer.AddInParameter(command, "TariffID", DbType.Int64, item.TariffID);
                                    dbServer.AddInParameter(command, "TariffServiceID", DbType.Int64, item.TariffServiceID);
                                    dbServer.AddInParameter(command, "ServiceID", DbType.String, item.ServiceId);
                                    dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, item.SpecializationID);
                                    dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, item.SubSpecializationId);
                                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.DepartmentId);
                                    dbServer.AddInParameter(command, "ModalityID", DbType.Int64, item.ModalityID);
                                    dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, item.ServiceRate);
                                    dbServer.AddInParameter(command, "SharePercentage", DbType.Decimal, item.DoctorSharePercentage);
                                    dbServer.AddInParameter(command, "ShareAmount", DbType.Decimal, item.DoctorShareAmount);
                                    dbServer.AddInParameter(command, "SpecializationModalityWise", DbType.Boolean, BizActionObj.ISShareModalityWise);
                                    dbServer.AddInParameter(command, "DontChangeTheExistingDoctor", DbType.Boolean, BizActionObj.DontChangeTheExistingDoctor);
                                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, item.AddedDateTime);
                                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command, "ISFORALLDOCTOR", DbType.Boolean, BizActionObj.ISFORALLDOCTOR);

                                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, item.AddedDateTime);
                                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                                    dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                                    int intStatus2 = dbServer.ExecuteNonQuery(command, trans);
                                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                                    trans.Commit();
                                    command.Connection.Close();
                                    // BizActionObj.DoctorShareDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                                }
                            }
                        }
                        else
                        {
                            //For Single Modality
                            con = dbServer.CreateConnection();
                            con.Open();
                            trans = con.BeginTransaction();


                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDoctorShareDetails");

                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.DoctorShareDetails.UnitID);
                            dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorShareDetails.DoctorId);
                            dbServer.AddInParameter(command, "IsApplyToallDoctor", DbType.Boolean, BizActionObj.IsAllDoctorShate);
                            dbServer.AddInParameter(command, "IsApplyToallDoctorWithAllTariffAndAllModality", DbType.Boolean, BizActionObj.IsApplyToallDoctorWithAllTariffAndAllModality);
                            dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.DoctorShareDetails.TariffID);
                            dbServer.AddInParameter(command, "TariffServiceID", DbType.Int64, BizActionObj.DoctorShareDetails.TariffServiceID);
                            dbServer.AddInParameter(command, "ServiceID", DbType.String, BizActionObj.DoctorShareDetails.ServiceId);
                            dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.DoctorShareDetails.SpecializationID);
                            dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, BizActionObj.DoctorShareDetails.SubSpecializationId);
                            dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DoctorShareDetails.DepartmentId);
                            dbServer.AddInParameter(command, "ModalityID", DbType.Int64, BizActionObj.DoctorShareDetails.ModalityID);
                            dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, BizActionObj.DoctorShareDetails.ServiceRate);
                            dbServer.AddInParameter(command, "SharePercentage", DbType.Decimal, BizActionObj.DoctorShareDetails.DoctorSharePercentage);
                            dbServer.AddInParameter(command, "ShareAmount", DbType.Decimal, BizActionObj.DoctorShareDetails.DoctorShareAmount);
                            dbServer.AddInParameter(command, "SpecializationModalityWise", DbType.Boolean, BizActionObj.ISShareModalityWise);
                            dbServer.AddInParameter(command, "DontChangeTheExistingDoctor", DbType.Boolean, BizActionObj.DontChangeTheExistingDoctor);

                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.DoctorShareDetails.AddedDateTime);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command, "ISFORALLDOCTOR", DbType.Boolean, BizActionObj.ISFORALLDOCTOR);
                            dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, BizActionObj.DoctorShareDetails.AddedDateTime);
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.DoctorShareDetails.Status);
                            dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.DoctorShareDetails.ID);
                            dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                            int intStatus2 = dbServer.ExecuteNonQuery(command, trans);

                            BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                            trans.Commit();
                            command.Connection.Close();
                        }
                    }
                    //SpecializationWise
                    else
                    {
                        //For All
                        foreach (var item in BizActionObj.DoctorShareInfoList)
                        {
                            con = dbServer.CreateConnection();
                            con.Open();
                            trans = con.BeginTransaction();

                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDoctorShareDetails");

                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                            dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorId);
                            dbServer.AddInParameter(command, "IsApplyToallDoctorWithAllTariffAndAllModality", DbType.Boolean, BizActionObj.IsApplyToallDoctorWithAllTariffAndAllModality);
                            dbServer.AddInParameter(command, "IsApplyToallDoctor", DbType.Boolean, BizActionObj.IsAllDoctorShate);
                            dbServer.AddInParameter(command, "TariffID", DbType.Int64, item.TariffID);
                            dbServer.AddInParameter(command, "TariffServiceID", DbType.Int64, item.TariffServiceID);
                            dbServer.AddInParameter(command, "ServiceID", DbType.String, item.ServiceId);
                            dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, item.SpecializationID);
                            dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, item.SubSpecializationId);
                            dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.DepartmentId);
                            dbServer.AddInParameter(command, "ModalityID", DbType.Int64, item.ModalityID);
                            dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, item.ServiceRate);
                            dbServer.AddInParameter(command, "SharePercentage", DbType.Decimal, item.DoctorSharePercentage);
                            dbServer.AddInParameter(command, "ShareAmount", DbType.Decimal, item.DoctorShareAmount);
                            dbServer.AddInParameter(command, "SpecializationModalityWise", DbType.Boolean, BizActionObj.ISShareModalityWise);
                            dbServer.AddInParameter(command, "DontChangeTheExistingDoctor", DbType.Boolean, BizActionObj.DontChangeTheExistingDoctor);
                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, item.AddedDateTime);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command, "ISFORALLDOCTOR", DbType.Boolean, BizActionObj.ISFORALLDOCTOR);

                            dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, item.AddedDateTime);
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                            dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                            dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                            int intStatus2 = dbServer.ExecuteNonQuery(command, trans);
                            BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                            trans.Commit();
                            command.Connection.Close();
                        }
                    }

                    //}
                    //else
                    //{
                    //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDoctorShareDetails");

                    //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.DoctorShareDetails.UnitID);
                    //    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorShareDetails.DoctorId);
                    //    dbServer.AddInParameter(command, "IsApplyToallDoctor", DbType.Boolean, BizActionObj.IsAllDoctorShate);
                    //    dbServer.AddInParameter(command, "IsApplyToallDoctorWithAllTariffAndAllModality", DbType.Boolean, BizActionObj.IsApplyToallDoctorWithAllTariffAndAllModality);
                    //    dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.DoctorShareDetails.TariffID);
                    //    dbServer.AddInParameter(command, "TariffServiceID", DbType.Int64, BizActionObj.DoctorShareDetails.TariffServiceID);
                    //    dbServer.AddInParameter(command, "ServiceID", DbType.String, BizActionObj.DoctorShareDetails.ServiceId);
                    //    dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.DoctorShareDetails.SpecializationID);
                    //    dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, BizActionObj.DoctorShareDetails.SubSpecializationId);
                    //    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DoctorShareDetails.DepartmentId);
                    //    dbServer.AddInParameter(command, "ModalityID", DbType.Int64, BizActionObj.DoctorShareDetails.ModalityID);
                    //    dbServer.AddInParameter(command, "ServiceRate", DbType.Decimal, BizActionObj.DoctorShareDetails.ServiceRate);
                    //    dbServer.AddInParameter(command, "SharePercentage", DbType.Decimal, BizActionObj.DoctorShareDetails.DoctorSharePercentage);
                    //    dbServer.AddInParameter(command, "ShareAmount", DbType.Decimal, BizActionObj.DoctorShareDetails.DoctorShareAmount);

                    //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                    //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    //    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.DoctorShareDetails.AddedDateTime);
                    //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);


                    //    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    //    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    //    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    //    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, BizActionObj.DoctorShareDetails.AddedDateTime);
                    //    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                    //    dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.DoctorShareDetails.Status);
                    //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.DoctorShareDetails.ID);
                    //    int intStatus2 = dbServer.ExecuteNonQuery(command);
                    //}
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                    trans = null;
                    con = null;
                }
            }
            //============================End DorShare Details===============================================
            return BizActionObj;

        }
        public override IValueObject GetDoctorShares1DetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorShare1DetailsBizActionVO BizActionObj = valueObject as clsGetDoctorShare1DetailsBizActionVO;
            try
            {
                if (BizActionObj.FromDoctorShareChildWindow == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_NewGetDoctorShares1DetailsListForChildWindow");
                    DbDataReader reader;

                    if (BizActionObj.SpecID != null)
                    {
                        dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.SpecID);
                    }
                    if (BizActionObj.SubSpecID != null)
                    {
                        dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, BizActionObj.SubSpecID);
                    }
                    if (BizActionObj.TariffID != null)
                    {
                        dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                    }
                    if (BizActionObj.ModalityID != null)
                    {
                        dbServer.AddInParameter(command, "ModalityID", DbType.Int64, BizActionObj.ModalityID);
                    }
                    //if (BizActionObj.ServiceId != null)
                    //{
                    //    dbServer.AddInParameter(command, "ServiceId", DbType.Int64, BizActionObj.ServiceId);
                    //}
                    if (BizActionObj.DoctorId != null)
                    {
                        dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorId);
                    }
                    dbServer.AddInParameter(command, "Service", DbType.String, BizActionObj.ServiceName + "%");

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorShareInfoGetList == null)
                            BizActionObj.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
                        while (reader.Read())
                        {
                            clsDoctorShareServicesDetailsVO objclsDoctorShareVO = new clsDoctorShareServicesDetailsVO();
                            objclsDoctorShareVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objclsDoctorShareVO.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objclsDoctorShareVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            objclsDoctorShareVO.ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModalityID"]));
                            objclsDoctorShareVO.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["modality"]));
                            objclsDoctorShareVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            objclsDoctorShareVO.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objclsDoctorShareVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Service"]));
                            objclsDoctorShareVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                            objclsDoctorShareVO.TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["Tariff"]));
                            objclsDoctorShareVO.SpecializationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                            objclsDoctorShareVO.SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                            objclsDoctorShareVO.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                            objclsDoctorShareVO.SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                            objclsDoctorShareVO.DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));
                            objclsDoctorShareVO.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));
                            objclsDoctorShareVO.ServiceRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            BizActionObj.DoctorShareInfoGetList.Add(objclsDoctorShareVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                else if (BizActionObj.ForAllDoctorShareRecord == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllDoctorShares1DetailsList");
                    DbDataReader reader;
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorShareInfoGetList == null)
                            BizActionObj.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
                        while (reader.Read())
                        {
                            clsDoctorShareServicesDetailsVO objclsDoctorShareVO = new clsDoctorShareServicesDetailsVO();
                            //  objclsDoctorShareVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objclsDoctorShareVO.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));

                            objclsDoctorShareVO.ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModalityID"]));
                            objclsDoctorShareVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));

                            objclsDoctorShareVO.SpecializationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                            objclsDoctorShareVO.SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));

                            BizActionObj.DoctorShareInfoGetList.Add(objclsDoctorShareVO);
                        }
                    }
                    reader.NextResult();
                    //BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_NewGetDoctorShares1DetailsList");
                    DbDataReader reader;

                    if (BizActionObj.SpecID != null)
                    {
                        dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.SpecID);
                    }
                    if (BizActionObj.SubSpecID != null)
                    {
                        dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, BizActionObj.SubSpecID);
                    }
                    if (BizActionObj.TariffID != null)
                    {
                        dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);

                    }
                    if (BizActionObj.ModalityID != null)
                    {
                        dbServer.AddInParameter(command, "ModalityID", DbType.Int64, BizActionObj.ModalityID);
                    }
                    //if (BizActionObj.ServiceId != null)
                    //{
                    //    dbServer.AddInParameter(command, "ServiceId", DbType.Int64, BizActionObj.ServiceId);
                    //}
                    if (BizActionObj.DocIds != null)
                    {
                        //  dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizActionObj.DoctorId);
                        dbServer.AddInParameter(command, "DocIds", DbType.String, BizActionObj.DocIds);
                    }
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorShareInfoGetList == null)
                            BizActionObj.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
                        while (reader.Read())
                        {
                            clsDoctorShareServicesDetailsVO objclsDoctorShareVO = new clsDoctorShareServicesDetailsVO();
                            objclsDoctorShareVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objclsDoctorShareVO.DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"]));
                            objclsDoctorShareVO.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"]));
                            objclsDoctorShareVO.ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModalityID"]));
                            objclsDoctorShareVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            //  objclsDoctorShareVO.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                            //  objclsDoctorShareVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                            objclsDoctorShareVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                            objclsDoctorShareVO.TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                            objclsDoctorShareVO.SpecializationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                            objclsDoctorShareVO.SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                            objclsDoctorShareVO.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                            objclsDoctorShareVO.SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                            //objclsDoctorShareVO.DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            // objclsDoctorShareVO.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            //  objclsDoctorShareVO.ServiceRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            BizActionObj.DoctorShareInfoGetList.Add(objclsDoctorShareVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return BizActionObj;
        }
        public override IValueObject DeleteExistingDoctorShareInfo(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsDeleteDoctorShareForOverRideExistingShareVO BizActionobj = valueObject as clsDeleteDoctorShareForOverRideExistingShareVO;

            try
            {
                List<clsDoctorShareServicesDetailsVO> objDoctorVO = BizActionobj.ExistingDoctorShareInfoList;

                foreach (var item in objDoctorVO)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteExistingDoctorShareServices");

                    //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorId);
                    dbServer.AddInParameter(command, "ModalityID", DbType.Int64, item.ModalityID);
                    dbServer.AddInParameter(command, "TariffID", DbType.Int64, item.TariffID);
                    int intStatus = dbServer.ExecuteNonQuery(command);
                }
                // DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteExistingDoctorShareServices");

                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDoctorVO.UnitID);
                //dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDoctorVO.DepartmentId);
                //dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, objDoctorVO.SpecializationID);
                //dbServer.AddInParameter(command, "TariffID", DbType.Int64, objDoctorVO.TariffID);
                // int intStatus = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return BizActionobj;
        }

        #region Added by Ashish Z.
        //======================================================for Doctor Service Linking=============================================================//
        public override IValueObject GetDoctorServiceLinkingByCategoryId(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDoctorServiceLinkingByCategoryBizActionVO objItem = valueObject as clsGetDoctorServiceLinkingByCategoryBizActionVO;
            clsServiceMasterVO objItemVO = new clsServiceMasterVO();
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceLinkingByCategoryId");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objItem.ServiceMasterDetails.ID);  // DoctorId
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objItem.ServiceMasterDetails.CategoryID);
                dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItem.ServiceMasterDetails.Specialization);
                dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItem.ServiceMasterDetails.SubSpecialization);
                if (objItem.ServiceMasterDetails.ServiceName != null && objItem.ServiceMasterDetails.ServiceName.Length > 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, objItem.ServiceMasterDetails.ServiceName);

                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objItem.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objItem.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, null);
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.TotalRows);

                objItem.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsServiceMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objItemVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])) + " " + Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        objItemVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        objItemVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objItemVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        objItemVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objItemVO.Rate = Convert.ToDecimal((DALHelper.HandleDBNull(reader["Rate"])));
                        objItemVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objItemVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                        objItemVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        objItemVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));
                        objItem.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        objItemVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        objItemVO.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        objItemVO.Status = Convert.ToBoolean((DALHelper.HandleDBNull(reader["Status"])));
                        objItem.ServiceMasterDetailsList.Add(objItemVO);
                    }
                }
                //reader.NextResult();
                //objItem.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
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

        public override IValueObject GetDoctorServiceLinkingByClinic(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDoctorServiceLinkingByCategoryBizActionVO objItem = valueObject as clsGetDoctorServiceLinkingByCategoryBizActionVO;
            clsServiceMasterVO objItemVO = new clsServiceMasterVO();
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceLinkingByClinic");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objItem.ServiceMasterDetails.ID);  // DoctorId
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
                dbServer.AddInParameter(command, "ClassID", DbType.Int64, objItem.ServiceMasterDetails.ClassID);

                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objItem.ServiceMasterDetails.CategoryID);
                dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItem.ServiceMasterDetails.Specialization);
                dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItem.ServiceMasterDetails.SubSpecialization);
                if (objItem.ServiceMasterDetails.ServiceName != null && objItem.ServiceMasterDetails.ServiceName.Length > 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, objItem.ServiceMasterDetails.ServiceName);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objItem.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.TotalRows);

                objItem.ServiceMasterDetailsList = new List<clsServiceMasterVO>();
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsServiceMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"]));
                        objItemVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objItemVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])) + " " + Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        objItemVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        objItemVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objItemVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        objItemVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objItemVO.Rate = Convert.ToDecimal((DALHelper.HandleDBNull(reader["Rate"])));
                        objItemVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objItemVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                        objItemVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        objItemVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));
                        objItem.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        objItemVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        objItemVO.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        objItemVO.Status = Convert.ToBoolean((DALHelper.HandleDBNull(reader["Status"])));
                        objItem.ServiceMasterDetailsList.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
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

        public override IValueObject AddUpdateDoctorServiceLinkingByCategory(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO BizActionObj = valueObject as clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO;

            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                if (BizActionObj.DeletedServiceList == null)
                    BizActionObj.DeletedServiceList = new List<clsServiceMasterVO>();

                if (BizActionObj.IsModify == true)
                {
                    if (BizActionObj.DeletedServiceList.Count > 0)
                    {
                        foreach (var item in BizActionObj.DeletedServiceList.ToList())
                        {
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorServiceLinkingByCategory");
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorID);
                            dbServer.AddInParameter(command, "CategoryID", DbType.Int64, item.CategoryID);
                            dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceID); //serviceID
                            dbServer.AddInParameter(command, "ClassId", DbType.Int64, item.ClassID);

                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                            int intStatus1 = dbServer.ExecuteNonQuery(command);
                        }
                    }

                    //DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorServiceLinkingByCategory");
                    //dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                    //dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    //int intStatus1 = dbServer.ExecuteNonQuery(command);
                }

                if (BizActionObj.ServiceMasterDetailsList == null)
                    BizActionObj.ServiceMasterDetailsList = new List<clsServiceMasterVO>();

                if (BizActionObj.ServiceMasterDetailsList != null && BizActionObj.ServiceMasterDetailsList.Count > 0)
                {
                    foreach (var item in BizActionObj.ServiceMasterDetailsList)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorServiceLinkingByCategory");
                        //DbCommand command = dbServer.GetStoredProcCommand("temp_CIMS_AddDoctorServiceLinkingByCategory");

                        dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                        dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceID); //serviceID
                        dbServer.AddInParameter(command, "ClassId", DbType.Int64, item.ClassID);
                        dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, item.Specialization);
                        dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, item.SubSpecialization);
                        dbServer.AddInParameter(command, "Rate", DbType.Decimal, item.Rate);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    }
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                //throw;
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

        //======================================================For Bulk Rate Change======================================================//

        public override IValueObject AddupdateBulkRateChangeDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateBulkRateChangeDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateBulkRateChangeDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateBulkRateChange");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IsApplicable", DbType.Boolean, BizActionObj.BulkRateChangeDetailsVO.IsApplicable);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, BizActionObj.BulkRateChangeDetailsVO.EffectiveDate);
                dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.BulkRateChangeDetailsVO.Remark);
                dbServer.AddInParameter(command, "IsFreeze", DbType.Boolean, BizActionObj.BulkRateChangeDetailsVO.IsFreeze);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, 1);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.BulkRateChangeDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.BulkRateChangeDetailsVO.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                if (BizActionObj.IsModify) //when Modify then IsModify flag will become true
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteBulkRateChangeForSpecialization");
                    command3.Connection = con;
                    dbServer.AddInParameter(command3, "BulkRateChangeID", DbType.Int64, BizActionObj.BulkRateChangeDetailsVO.ID);
                    dbServer.AddInParameter(command3, "BulkRateChangeUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                }

                if (BizActionObj.TariffDetailsList == null)
                    BizActionObj.TariffDetailsList = new List<ValueObjects.Billing.clsTariffMasterBizActionVO>();
                if (BizActionObj.TariffDetailsList != null && BizActionObj.TariffDetailsList.Count > 0)
                {
                    foreach (var item in BizActionObj.TariffDetailsList)
                    {
                        DbCommand command1;
                        command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBulkRateChangeForTariff");
                        command1.Connection = con;
                        dbServer.AddInParameter(command1, "BulkRateChangeID", DbType.Int64, BizActionObj.BulkRateChangeDetailsVO.ID);
                        dbServer.AddInParameter(command1, "BulkRateChangeUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "TariffID", DbType.Int64, item.TariffID);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, 1);

                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objUserVO.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    }
                }

                //if (BizActionObj.IsModify) //when Modify then IsModify flag is true                 
                //{
                //    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteBulkRateChangeForSpecialization");
                //    command3.Connection = con;
                //    dbServer.AddInParameter(command3, "BulkRateChangeID", DbType.Int64, BizActionObj.BulkRateChangeDetailsVO.ID);
                //    dbServer.AddInParameter(command3, "BulkRateChangeUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                //}

                if (BizActionObj.SubSpecializationList == null)
                    BizActionObj.SubSpecializationList = new List<clsSubSpecializationVO>();
                if (BizActionObj.SubSpecializationList != null && BizActionObj.SubSpecializationList.Count > 0)
                {
                    foreach (var item in BizActionObj.SubSpecializationList)
                    {
                        DbCommand command2;
                        command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBulkRateChangeForSpecialization");
                        command2.Connection = con;
                        dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "BulkRateChangeID", DbType.Int64, BizActionObj.BulkRateChangeDetailsVO.ID);
                        dbServer.AddInParameter(command2, "BulkRateChangeUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                        //dbServer.AddInParameter(command2, "IsSetRateForAll", DbType.Boolean, 1);
                        dbServer.AddInParameter(command2, "SpecilizationID", DbType.Int64, item.SpecializationId);
                        dbServer.AddInParameter(command2, "SubSpecializationID", DbType.Int64, item.SubSpecializationId);
                        dbServer.AddInParameter(command2, "IsPercentageRate", DbType.Boolean, item.IsPercentageRate);
                        if (item.IsPercentageRate) dbServer.AddInParameter(command2, "PercentageRate", DbType.Decimal, item.SharePercentage);
                        if (item.IsAmountRate) dbServer.AddInParameter(command2, "AmountRate", DbType.Decimal, item.SharePercentage);//item.ShareAmount);
                        if (item.IsAddition) item.intOperationType = 1;
                        else if (item.IsSubtaction) item.intOperationType = 2;
                        dbServer.AddInParameter(command2, "OperationType", DbType.Int16, item.intOperationType);
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, 1);

                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);
                        int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                    }
                }

                trans.Commit();
            }

            catch (Exception ex)
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
            return BizActionObj;

        }

        public override IValueObject GetBulkRateChangeDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            //StringBuilder sbTariffList = new StringBuilder();
            DbDataReader reader = null;
            clsGetBulkRateChangeDetailsListBizActionVO objItem = valueObject as clsGetBulkRateChangeDetailsListBizActionVO;
            clsTariffMasterBizActionVO objItemVO = new clsTariffMasterBizActionVO();
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBulkRateChangeDetailsList");

                dbServer.AddInParameter(command, "FromEffectiveDate", DbType.Date, objItem.BulkRateChangeDetailsVO.FromEffectiveDate);
                dbServer.AddInParameter(command, "ToEffectiveDate", DbType.Date, objItem.BulkRateChangeDetailsVO.ToEffectiveDate);
                dbServer.AddInParameter(command, "TariffName", DbType.String, objItem.BulkRateChangeDetailsVO.TariffName);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objItem.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.TotalRows);

                objItem.TariffDetailsList = new List<clsTariffMasterBizActionVO>();
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsTariffMasterBizActionVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.IsApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApplicable"]));
                        objItemVO.EffectiveDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EffectiveDate"]));
                        objItemVO.IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"]));
                        objItemVO.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objItemVO.Status = Convert.ToBoolean((DALHelper.HandleDBNull(reader["Status"])));
                        objItemVO.AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddedBy"]));
                        objItemVO.strAddedBy = Convert.ToString(DALHelper.HandleDBNull(reader["strAddedBy"]));
                        //objItemVO.TariffID = Convert.ToInt64((DALHelper.HandleDBNull(reader["TariffID"])));
                        objItemVO.BulkRateChangeID = Convert.ToInt64((DALHelper.HandleDBNull(reader["BulkRateChangeID"])));
                        objItemVO.TariffName = Convert.ToString((DALHelper.HandleDBNull(reader["TariffName"])));
                        objItem.TariffDetailsList.Add(objItemVO);
                    }
                }
                //reader.NextResult();
                //if (reader.HasRows)
                //{
                //    clsSubSpecializationVO objItemVO1 = new clsSubSpecializationVO();
                //    while (reader.Read())
                //    {
                //        objItemVO1 = new clsSubSpecializationVO();
                //        objItemVO1.BulkRateChangeID = Convert.ToInt64((DALHelper.HandleDBNull(reader["BulkRateChangeID"])));
                //        objItemVO1.SpecializationId = Convert.ToInt64((DALHelper.HandleDBNull(reader["TariffID"])));
                //        objItemVO1.SpecializationName = Convert.ToString((DALHelper.HandleDBNull(reader["TariffName"])));
                //        objItem.SubSpecializationList.Add(objItemVO1);

                //        //sbTariffList.Append(objItemVO.TariffName);
                //        //sbTariffList.Append(",");
                //    }
                //    //objItem.TariffList = sbTariffList.ToString();
                //}
                reader.NextResult();
                objItem.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
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

        public override IValueObject GetBulkRateChangeDetailsListByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;

            clsGetBulkRateChangeDetailsListBizActionVO objItem = valueObject as clsGetBulkRateChangeDetailsListBizActionVO;
            clsTariffMasterBizActionVO objItemVO = new clsTariffMasterBizActionVO();
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBulkRateChangeDetailsListByID");
                dbServer.AddInParameter(command, "BulkRateChangeID", DbType.Int64, objItem.BulkRateChangeDetailsVO.BulkRateChangeID);
                dbServer.AddInParameter(command, "BulkRateChangeUnitID", DbType.Int64, objItem.BulkRateChangeDetailsVO.BulkRateChangeUnitID);
                objItem.TariffDetailsList = new List<clsTariffMasterBizActionVO>();
                objItem.SubSpecializationList = new List<clsSubSpecializationVO>();
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsTariffMasterBizActionVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.BulkRateChangeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeID"]));
                        objItemVO.BulkRateChangeUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeUnitID"]));
                        objItemVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                        objItem.TariffDetailsList.Add(objItemVO);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    clsSubSpecializationVO objItemVO1 = new clsSubSpecializationVO();
                    while (reader.Read())
                    {
                        objItemVO1 = new clsSubSpecializationVO();
                        objItemVO1.BulkRateChangeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeID"]));
                        objItemVO1.BulkRateChangeUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BulkRateChangeUnitID"]));
                        objItemVO1.IsSetRateForAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetRateForAll"]));
                        objItemVO1.SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecilizationID"]));
                        objItemVO1.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                        objItemVO1.SubSpecializationId = Convert.ToInt64((DALHelper.HandleDBNull(reader["SubSpecializationID"])));
                        objItemVO1.SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));
                        objItemVO1.IsPercentageRate = Convert.ToBoolean((DALHelper.HandleDBNull(reader["IsPercentageRate"])));
                        if (objItemVO1.IsPercentageRate) { objItemVO1.SharePercentage = objItemVO1.ShareAmount = Convert.ToDouble((DALHelper.HandleDBNull(reader["PercentageRate"]))); }
                        if (!objItemVO1.IsPercentageRate) { objItemVO1.SharePercentage = objItemVO1.ShareAmount = Convert.ToDouble((DALHelper.HandleDBNull(reader["AmountRate"]))); }
                        objItemVO1.intOperationType = Convert.ToInt16((DALHelper.HandleDBNull(reader["OperationType"])));
                        objItemVO1.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItem.SubSpecializationList.Add(objItemVO1);
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
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }

        #endregion

        #region Added BY CDS
        //======================================================for Doctor Service Linking On Frm Bill =============================================================//
        public override IValueObject GetDoctorServiceLinkingByDoctorId(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO objItem = valueObject as clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO;
            clsServiceMasterVO objItemVO = new clsServiceMasterVO();
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceLinkingRateByDoctorID");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objItem.ServiceMasterDetails.DoctorID);  // DoctorId
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItem.ServiceMasterDetails.ServiceID);
                //dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItem.ServiceMasterDetails.Specialization);
                //dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItem.ServiceMasterDetails.SubSpecialization);
                //if (objItem.ServiceMasterDetails.ServiceName != null && objItem.ServiceMasterDetails.ServiceName.Length > 0)
                //    dbServer.AddInParameter(command, "ServiceName", DbType.String, objItem.ServiceMasterDetails.ServiceName);

                //objItem.ServiceMasterDetails = new clsServiceMasterVO();
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsServiceMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objItemVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])) + " " + Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        objItemVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        objItemVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objItemVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        objItemVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));

                        objItemVO.Rate = Convert.ToDecimal((DALHelper.HandleDBNull(reader["Rate"])));

                        objItemVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objItemVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                        objItemVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        objItemVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));
                        objItem.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        objItemVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        objItemVO.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        objItemVO.Status = Convert.ToBoolean((DALHelper.HandleDBNull(reader["Status"])));
                        //objItem.ServiceMasterDetailsList.Add(objItemVO);
                        objItem.ServiceMasterDetails = objItemVO;
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
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }
        //==================================================================================================================================
        #endregion


        #region //By Umesh For Doctor Payment
        public override IValueObject AddDoctorBillPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorBillPaymentDetailsBizActionVO BizActionObj = (clsAddDoctorBillPaymentDetailsBizActionVO)valueObject;

            try
            {
                clsDoctorPaymentVO ObjDoctorVO = BizActionObj.DoctorInfo;


                //if (BizActionObj.DoctorDetails != null && BizActionObj.DoctorDetails.Count > 0)
                //{

                //    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorBillPaymentDetails");

                //    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);
                //    dbServer.AddInParameter(command3, "DoctorPaymentID", DbType.Int64, ObjDoctorVO.DoctorPaymentID);
                //    dbServer.AddInParameter(command3, "DoctorPaymentUnitID", DbType.Int64, ObjDoctorVO.DoctorPaymentUnitID);

                //    int intStatus2 = dbServer.ExecuteNonQuery(command3);
                //}

                foreach (var item in BizActionObj.DoctorDetails)
                {


                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorBillPaymentDetails");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "DoctorPaymentID", DbType.Int64, item.DoctorPaymentID);
                    dbServer.AddInParameter(command, "DoctorPaymentUnitID", DbType.Int64, item.DoctorPaymentUnitID);
                    dbServer.AddInParameter(command, "BillID", DbType.Int64, item.BillID);
                    dbServer.AddInParameter(command, "BillUnitID", DbType.Int64, item.BillUnitID);
                    dbServer.AddInParameter(command, "DoctorSharePercentage", DbType.Double, item.DoctorSharePercentage);
                    dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Double, item.DoctorShareAmount);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.ID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command);
                    BizActionObj.DoctorInfo.ID = (long)dbServer.GetParameterValue(command, "ID");

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

        public override IValueObject AddDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDoctorPaymentDetailsBizActionVO BizActionObj = (clsAddDoctorPaymentDetailsBizActionVO)valueObject;

            try
            {
                clsDoctorPaymentVO ObjDoctorVO = BizActionObj.DoctorInfo;


                //if (BizActionObj.DoctorDetails != null && BizActionObj.DoctorDetails.Count > 0)
                //{

                //    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorPayment");

                //    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, ObjDoctorVO.UnitID);
                //    dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, ObjDoctorVO.DoctorID);
                //    dbServer.AddInParameter(command3, "Doctor_UnitID", DbType.Int64, ObjDoctorVO.DoctorUnitID);

                //    int intStatus2 = dbServer.ExecuteNonQuery(command3);
                //}

                foreach (var item in BizActionObj.DoctorDetails)
                {


                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorPayment");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int64, item.DoctorID);
                    dbServer.AddInParameter(command, "Doctor_UnitID", DbType.Int64, item.DoctorUnitID);

                    if (ObjDoctorVO.PaymentNo != null) ObjDoctorVO.PaymentNo = item.PaymentNo.Trim();
                    dbServer.AddParameter(command, "PaymentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000");
                    dbServer.AddInParameter(command, "PaymentDate", DbType.DateTime, item.PaymentDate);
                    dbServer.AddInParameter(command, "TotalAmount", DbType.Double, item.TotalBillAmount);
                    dbServer.AddInParameter(command, "DoctorPayAmount", DbType.Double, item.DoctorPayAmount);
                    dbServer.AddInParameter(command, "PaidAmount", DbType.Double, item.PaidAmount);
                    dbServer.AddInParameter(command, "BalanceAmount", DbType.Double, item.BalanceAmount);
                    dbServer.AddInParameter(command, "IsOnBill", DbType.Boolean, item.IsOnBill);
                    dbServer.AddInParameter(command, "IsFix", DbType.Boolean, item.IsFix);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDoctorVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjDoctorVO.Status);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDoctorVO.ID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command);
                    BizActionObj.DoctorInfo.ID = (long)dbServer.GetParameterValue(command, "ID");

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



        public override IValueObject GetDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentAllBizActionVO BizActionObj = (clsGetDoctorPaymentAllBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorPayment");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorPaymentVO>();
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO DoctorShareInformation = new clsDoctorPaymentVO();
                        DoctorShareInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        DoctorShareInformation.PaymentDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PaymentDate"]));
                        DoctorShareInformation.PaymentNo = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentNo"]));
                        DoctorShareInformation.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        DoctorShareInformation.PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"]));
                        DoctorShareInformation.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));

                        BizActionObj.DoctorDetails.Add(DoctorShareInformation);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorPaymentBillsDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentListBizActionVO BizActionObj = (clsGetDoctorPaymentListBizActionVO)valueObject;


            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentBills");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorPaymentVO>();
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO DoctorShareInformation = new clsDoctorPaymentVO();

                        DoctorShareInformation.BillID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BillID"]));
                        DoctorShareInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        DoctorShareInformation.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                        DoctorShareInformation.ScanNo = Convert.ToString(DALHelper.HandleDBNull(reader["ScanNO"]));
                        DoctorShareInformation.PatientName = (Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])));
                        DoctorShareInformation.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        DoctorShareInformation.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                        DoctorShareInformation.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));


                        BizActionObj.DoctorDetails.Add(DoctorShareInformation);

                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }




            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorPaymentList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentBizActionVO BizActionObj = (clsGetDoctorPaymentBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentList");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorId);


                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorPaymentVO>();
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO DoctorShareInformation = new clsDoctorPaymentVO();

                        DoctorShareInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        DoctorShareInformation.PaymentDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PaymentDate"]));
                        DoctorShareInformation.PaymentNo = Convert.ToString(DALHelper.HandleDBNull(reader["PaymentNo"]));
                        DoctorShareInformation.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        DoctorShareInformation.PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"]));
                        DoctorShareInformation.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));
                        DoctorShareInformation.BillID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BillID"]));

                        BizActionObj.DoctorDetails.Add(DoctorShareInformation);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorBillPaymentDetailListByBillID(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorPaymentDetailListByBillIDBizActionVO BizActionObj1 = (clsGetDoctorPaymentDetailListByBillIDBizActionVO)valueObject;
            try
            {
                foreach (var item in BizActionObj1.BillIdList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorBillPaymentDetailListByBillID");
                    DbDataReader reader;
                    dbServer.AddInParameter(command, "BillID", DbType.Int32, item);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int32, BizActionObj1.UnitID);
                    dbServer.AddInParameter(command, "DoctorID", DbType.Int32, BizActionObj1.DoctorID);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                    dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj1.TotalRows);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj1.DoctorPaymentDetailsList == null)
                            BizActionObj1.DoctorPaymentDetailsList = new List<clsDoctorPaymentVO>();
                        while (reader.Read())
                        {

                            clsDoctorPaymentVO objclsDoctorVO = new clsDoctorPaymentVO();
                            //BizActionObj.DoctorWaiverDetails
                            //BizActionObj.DoctorPaymentDetails.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            objclsDoctorVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                            objclsDoctorVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                            objclsDoctorVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            objclsDoctorVO.DoctorShareAmount = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            BizActionObj1.DoctorPaymentDetailsList.Add(objclsDoctorVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj1.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return BizActionObj1;
        }

        public override IValueObject GetDoctorShareAmount(IValueObject valueObject, clsUserVO objUserVO)
        {



            clsGetDoctorShareAmountBizActionVO BizActionObj = (clsGetDoctorShareAmountBizActionVO)valueObject;

            try
            {
                foreach (var item in BizActionObj.BillIdList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDocShare");
                    dbServer.AddInParameter(command, "DoctorID", DbType.Double, BizActionObj.DoctorId);
                    dbServer.AddInParameter(command, "BillID", DbType.Double, item);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorDetails == null)
                            BizActionObj.DoctorDetails = new List<clsDoctorPaymentVO>();
                        while (reader.Read())
                        {
                            clsDoctorPaymentVO DoctorShareInformation = new clsDoctorPaymentVO();
                            DoctorShareInformation.DoctorShareAmount = DoctorShareInformation.DoctorShareAmount + Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            DoctorShareInformation.Rate = DoctorShareInformation.Rate + Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            DoctorShareInformation.DoctorSharePercentage = DoctorShareInformation.DoctorSharePercentage + Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            BizActionObj.DoctorDetails.Add(DoctorShareInformation);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;

        }

        public override IValueObject GetPaidDoctorPaymentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPaidDoctorPaymentDetailsBizActionVO BizActionObj = (clsGetPaidDoctorPaymentDetailsBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPaidDoctorPaymentDetails");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorId);
                dbServer.AddInParameter(command, "DoctorPaymentID", DbType.Int64, BizActionObj.DoctorPaymentId);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorPaymentVO>();
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO PaidDoctorInformation = new clsDoctorPaymentVO();
                        PaidDoctorInformation.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        PaidDoctorInformation.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        PaidDoctorInformation.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        PaidDoctorInformation.BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillUnitID"]));
                        PaidDoctorInformation.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        PaidDoctorInformation.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        PaidDoctorInformation.TotalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalShareAmount"]));
                        PaidDoctorInformation.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        PaidDoctorInformation.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        PaidDoctorInformation.DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));

                        BizActionObj.DoctorDetails.Add(PaidDoctorInformation);
                        //BizActionObj.DoctorInfo = PaidDoctorInformation;
                    }
                }
                reader.NextResult();
                //BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject SaveDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsSaveDoctorPaymentDetailsBizActionVO BizActionObj = (clsSaveDoctorPaymentDetailsBizActionVO)valueObject;


            DbCommand command = dbServer.GetStoredProcCommand("CIMS_InsertDoctorPayment");

            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
            dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
            dbServer.AddInParameter(command, "DoctorServiceLinkingTypeID", DbType.Int64, BizActionObj.DoctorServiceLinkingTypeID);
            //dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.BillID);
            //dbServer.AddInParameter(command, "BillUnitID", DbType.Int64, BizActionObj.BillUnitID);
            dbServer.AddInParameter(command, "PaymentDateTime", DbType.DateTime, BizActionObj.PaymentDate);
            dbServer.AddInParameter(command, "TotalAmount", DbType.Double, BizActionObj.TotalAmount);
            dbServer.AddInParameter(command, "DoctorPaidAmount", DbType.Double, BizActionObj.DoctorPaidAmount);
            dbServer.AddInParameter(command, "DoctorBalanceAmount", DbType.Double, BizActionObj.BalanceAmount);
            dbServer.AddInParameter(command, "IsPaymentdone", DbType.Byte, 1);

            dbServer.AddInParameter(command, "BillIDs", DbType.String, BizActionObj.DoctorInfo.BillIDs);
            dbServer.AddInParameter(command, "BillUnitIDs", DbType.String, BizActionObj.DoctorInfo.BillUnitIDs);

            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

            dbServer.AddParameter(command, "VoucherNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????????");
            dbServer.AddParameter(command, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 1);
            int intStatus2 = dbServer.ExecuteNonQuery(command);
            BizActionObj.DoctorInfo.ID = (long)dbServer.GetParameterValue(command, "Id");
            BizActionObj.DoctorInfo.PaymentNo = (string)dbServer.GetParameterValue(command, "VoucherNo");
            if (BizActionObj.PaymentDetailDetails.Count > 0)
            {
                foreach (var item in BizActionObj.PaymentDetailDetails)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_InsertDoctorPaymentModeDetail");

                    dbServer.AddInParameter(command1, "DoctorPaymentID", DbType.Int64, BizActionObj.DoctorInfo.ID);
                    //dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                    dbServer.AddInParameter(command1, "DoctorPaymentMode", DbType.Int64, item.PaymentModeID);
                    dbServer.AddInParameter(command1, "BankID", DbType.Int64, item.BankID);
                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, item.Date);
                    dbServer.AddInParameter(command1, "AccountNo", DbType.String, item.Number);
                    dbServer.AddInParameter(command1, "PaidAmount", DbType.Double, item.PaidAmount);

                    int intStatus3 = dbServer.ExecuteNonQuery(command1);

                }

            }


            return BizActionObj;
        }

        public override IValueObject GetDoctorDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorBizActionVO BizActionObj = (clsGetDoctorBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoc");
                if (BizActionObj.UnitId > 0)
                {
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                }
                if (BizActionObj.DepartmentId > 0)
                {
                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentId);
                }
                if (BizActionObj.ClassificationId > 0)
                {
                    dbServer.AddInParameter(command, "ClassificationID", DbType.Int64, BizActionObj.ClassificationId);
                }
                if (BizActionObj.DoctorTypeId > 0)
                {
                    dbServer.AddInParameter(command, "DoctorTypeID", DbType.Int64, BizActionObj.DoctorTypeId);
                }

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorPaymentVO>();
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO DoctorShareInformation = new clsDoctorPaymentVO();
                        //DoctorShareInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        DoctorShareInformation.DoctorID = Convert.ToInt32(DALHelper.HandleDBNull(reader["DoctorID"]));
                        DoctorShareInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        DoctorShareInformation.ClassificationID = Convert.ToInt32(DALHelper.HandleDBNull(reader["ClassificationID"]));
                        DoctorShareInformation.ClassificationName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassificationName"]));

                        DoctorShareInformation.DepartmentID = Convert.ToInt32(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        DoctorShareInformation.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"]));
                        DoctorShareInformation.DoctorTypeID = Convert.ToInt32(DALHelper.HandleDBNull(reader["DoctorTypeID"]));
                        DoctorShareInformation.DoctorTypeName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorTypeName"]));

                        BizActionObj.DoctorDetails.Add(DoctorShareInformation);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorPaymentChildDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorPaymentChildBizActionVO BizActionObj = (clsGetDoctorPaymentChildBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorBillDetails");


                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorId);
                dbServer.AddInParameter(command, "TariffServiceID", DbType.Int64, BizActionObj.DoctorInfo.TariffServiceID);
                dbServer.AddInParameter(command, "BillNumber", DbType.String, BizActionObj.DoctorInfo.BillNo);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                // dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorInfoList == null)
                        BizActionObj.DoctorInfoList = new List<clsDoctorPaymentVO>();
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO DoctorInfo = new clsDoctorPaymentVO();

                        DoctorInfo.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        DoctorInfo.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        DoctorInfo.TariffID = Convert.ToInt32(DALHelper.HandleDBNull(reader["TariffId"]));

                        DoctorInfo.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        DoctorInfo.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));

                        DoctorInfo.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        DoctorInfo.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));
                        DoctorInfo.DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));

                        DoctorInfo.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNumber"]));

                        DoctorInfo.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        DoctorInfo.BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));



                        BizActionObj.DoctorInfoList.Add(DoctorInfo);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;


        }

        public override IValueObject GetDoctorPaymentDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorPaymentShareDetailsBizActionVO BizActionObj = (clsGetDoctorPaymentShareDetailsBizActionVO)valueObject;

            try
            {
                if (BizActionObj.IsForAllData == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentShareDetailsForAllBill");


                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "DoctorID", DbType.String, BizActionObj.DoctorIds);
                    dbServer.AddInParameter(command, "IsForBoth", DbType.Boolean, BizActionObj.IsForBoth);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorInfoList == null)
                            BizActionObj.DoctorInfoList = new List<clsDoctorPaymentVO>();
                        while (reader.Read())
                        {
                            clsDoctorPaymentVO DoctorInfo = new clsDoctorPaymentVO();

                            //DoctorInfo.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                            //DoctorInfo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                            //DoctorInfo.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                            //DoctorInfo.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));

                            //DoctorInfo.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                            //DoctorInfo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));

                            //DoctorInfo.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));



                            //DoctorInfo.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceRate"]));
                            //DoctorInfo.DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));
                            //DoctorInfo.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));

                            DoctorInfo.BillUnitIDs = Convert.ToString(DALHelper.HandleDBNull(reader["BIllUnitID"]));
                            DoctorInfo.BillIDs = Convert.ToString(DALHelper.HandleDBNull(reader["BIllID"]));
                            DoctorInfo.TotalBillShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillShareAmount"]));

                            BizActionObj.DoctorInfo = DoctorInfo;



                            //BizActionObj.DoctorInfoList.Add(DoctorInfo);
                        }
                    }
                    reader.NextResult();
                    //BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                else
                {


                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_NewGetDoctorPaymentShareDetails");


                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                    dbServer.AddInParameter(command, "DoctorID", DbType.String, BizActionObj.DoctorIds);
                    dbServer.AddInParameter(command, "IsSettleBill", DbType.Boolean, BizActionObj.IsSettleBill);
                    dbServer.AddInParameter(command, "IsCreditBill", DbType.Boolean, BizActionObj.IsCreditBill);
                    dbServer.AddInParameter(command, "IsForBoth", DbType.Boolean, BizActionObj.IsForBoth);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);



                    // dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        if (BizActionObj.DoctorInfoList == null)
                            BizActionObj.DoctorInfoList = new List<clsDoctorPaymentVO>();
                        while (reader.Read())
                        {
                            clsDoctorPaymentVO DoctorInfo = new clsDoctorPaymentVO();

                            DoctorInfo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                            DoctorInfo.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                            DoctorInfo.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            //DoctorInfo.TariffServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceID"]));
                            DoctorInfo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                            DoctorInfo.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            DoctorInfo.BillUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillUnitID"]));
                            DoctorInfo.VisitDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VisitDate"]));
                            DoctorInfo.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));


                            DoctorInfo.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                            DoctorInfo.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));

                            DoctorInfo.TotalConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalConcessionAmount"]));
                            DoctorInfo.TotalShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalShareAmount"]));
                            DoctorInfo.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                            DoctorInfo.SelfAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SelfAmount"]));

                            //DoctorInfo.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"])); 
                            //DoctorInfo.TariffID  = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffServiceId"]));
                            //DoctorInfo.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));

                            DoctorInfo.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));

                            //int creditbill = Convert.ToInt16(DALHelper.HandleDBNull(reader["IsCreditBill"]));

                            //if (creditbill == 1)
                            //    DoctorInfo.IsCreditBill = true;
                            //else
                            //    DoctorInfo.IsCreditBill = false;

                            //DoctorInfo.DoctorSharePercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["SharePercentage"]));

                            //DoctorInfo.DoctorShareAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ShareAmount"]));


                            BizActionObj.DoctorInfoList.Add(DoctorInfo);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetDoctorPaymentFrontDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorPaymentDetailListBizActionVO BizActionObj = (clsGetDoctorPaymentDetailListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorPaymentDetail");

                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorInfo.DoctorID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.DoctorInfo.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.DoctorInfo.ToDate);
                dbServer.AddInParameter(command, "PaidUnpaid", DbType.Boolean, BizActionObj.DoctorInfo.IsPaymentDone);


                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, false);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, 0);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorDetails == null)
                        BizActionObj.DoctorDetails = new List<clsDoctorPaymentVO>();
                    while (reader.Read())
                    {
                        clsDoctorPaymentVO DoctorShareInformation = new clsDoctorPaymentVO();

                        DoctorShareInformation.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])); ;
                        DoctorShareInformation.DoctorPaymentID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["Id"]));
                        DoctorShareInformation.DoctorPaymentUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        DoctorShareInformation.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        DoctorShareInformation.PaymentDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PaymentDateTime"]));
                        DoctorShareInformation.PaymentNo = Convert.ToString(DALHelper.HandleDBNull(reader["VoucherNo"]));
                        DoctorShareInformation.TotalBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        DoctorShareInformation.PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorPaidAmount"]));
                        DoctorShareInformation.BalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["DoctorBalanceAmount"]));
                        // DoctorShareInformation.BillID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BillID"]));

                        BizActionObj.DoctorDetails.Add(DoctorShareInformation);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;

        }
        public override IValueObject SaveDoctorSettlePaymentDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            return valueObject;
        }
        public override IValueObject GetDoctorShareRangeList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDoctorShareRangeList BizActionObj = (clsGetDoctorShareRangeList)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorShareRangeDetailList");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorShareRangeList == null)
                        BizActionObj.DoctorShareRangeList = new List<clsDoctorShareRangeList>();
                    while (reader.Read())
                    {
                        clsDoctorShareRangeList DoctorShareInformation = new clsDoctorShareRangeList();

                        DoctorShareInformation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])); ;
                        DoctorShareInformation.LowerLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LowerLimit"]));
                        DoctorShareInformation.UpperLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["UpperLimit"]));
                        DoctorShareInformation.SharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SharePercentage"]));
                        DoctorShareInformation.ShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ShareAmount"]));
                        DoctorShareInformation.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));


                        BizActionObj.DoctorShareRangeList.Add(DoctorShareInformation);
                    }
                }

                reader.Close();
            }

            catch (Exception e)
            {
                throw;
            }

            finally
            {
            }
            return BizActionObj;

        }

        # endregion
    }
}