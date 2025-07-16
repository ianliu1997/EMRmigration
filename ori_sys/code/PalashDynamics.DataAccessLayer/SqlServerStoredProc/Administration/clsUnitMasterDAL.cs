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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects.Administration.UnitMaster;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsUnitMasterDAL : clsBaseUnitMasterDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        public clsUnitMasterDAL()
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


        public override IValueObject AddUnitMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUnitMasterBizActionVO BizActionobj = valueObject as clsAddUnitMasterBizActionVO;


            if (BizActionobj.UnitDetails.UnitID == 0)
            {
                BizActionobj = AddUnit(BizActionobj, objUserVO);
            }

            else
            {
                BizActionobj = UpdateUnit(BizActionobj, objUserVO);

            }

            return BizActionobj;
        }

        private clsAddUnitMasterBizActionVO AddUnit(clsAddUnitMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsUnitMasterVO ObjUnitVO = BizActionObj.UnitDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUnitMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, ObjUnitVO.UnitCode.Trim());
                dbServer.AddInParameter(command, "Name", DbType.String, ObjUnitVO.Description.Trim());
                dbServer.AddInParameter(command, "ClusterID", DbType.Int64, ObjUnitVO.ClusterID);
                dbServer.AddInParameter(command, "Description", DbType.String, ObjUnitVO.Description.Trim());
                if (ObjUnitVO.AddressLine1 != null) ObjUnitVO.AddressLine1 = ObjUnitVO.AddressLine1.Trim();
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, ObjUnitVO.AddressLine1);

                if (ObjUnitVO.AddressLine2 != null) ObjUnitVO.AddressLine2 = ObjUnitVO.AddressLine2.Trim();
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, ObjUnitVO.AddressLine2);

                if (ObjUnitVO.AddressLine3 != null) ObjUnitVO.AddressLine3 = ObjUnitVO.AddressLine3.Trim();
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, ObjUnitVO.AddressLine3);

                if (ObjUnitVO.Country != null) ObjUnitVO.Country = ObjUnitVO.Country.Trim();
                dbServer.AddInParameter(command, "Country", DbType.String, ObjUnitVO.Country);
                if (ObjUnitVO.State != null) ObjUnitVO.State = ObjUnitVO.State.Trim();
                dbServer.AddInParameter(command, "State", DbType.String, ObjUnitVO.State);
                if (ObjUnitVO.City != null) ObjUnitVO.City = ObjUnitVO.City.Trim();
                dbServer.AddInParameter(command, "City", DbType.String, ObjUnitVO.City);
                if (ObjUnitVO.Taluka != null) ObjUnitVO.Taluka = ObjUnitVO.Taluka.Trim();
                dbServer.AddInParameter(command, "Taluka", DbType.String, ObjUnitVO.Taluka);
                if (ObjUnitVO.Area != null) ObjUnitVO.Area = ObjUnitVO.Area.Trim();
                dbServer.AddInParameter(command, "Area", DbType.String, ObjUnitVO.Area);
                if (ObjUnitVO.District != null) ObjUnitVO.District = ObjUnitVO.District.Trim();
                dbServer.AddInParameter(command, "District", DbType.String, ObjUnitVO.District);
                if (ObjUnitVO.Pincode != null) ObjUnitVO.Pincode = ObjUnitVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, ObjUnitVO.Pincode);

                if (ObjUnitVO.ContactNo != null) ObjUnitVO.ContactNo = ObjUnitVO.ContactNo.Trim();
                dbServer.AddInParameter(command, "ContactNo", DbType.String, ObjUnitVO.ContactNo);

                //added by neena
                if (ObjUnitVO.ContactNo1 != null) ObjUnitVO.ContactNo1 = ObjUnitVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, ObjUnitVO.ContactNo1);

                if (ObjUnitVO.MobileNO != null) ObjUnitVO.MobileNO = ObjUnitVO.MobileNO.Trim();
                dbServer.AddInParameter(command, "MobileNO", DbType.String, ObjUnitVO.MobileNO);

                dbServer.AddInParameter(command, "MobileCountryCode", DbType.Int32, ObjUnitVO.MobileCountryCode);
                //

                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int32, ObjUnitVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int32, ObjUnitVO.ResiSTDCode);

                if (ObjUnitVO.FaxNo != null) ObjUnitVO.FaxNo = ObjUnitVO.FaxNo.Trim();
                dbServer.AddInParameter(command, "FaxNo", DbType.String, ObjUnitVO.FaxNo);
                if (ObjUnitVO.Email != null) ObjUnitVO.Email = ObjUnitVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, ObjUnitVO.Email);
                if (ObjUnitVO.ServerName != null) ObjUnitVO.ServerName = ObjUnitVO.ServerName.Trim();
                dbServer.AddInParameter(command, "ServerName", DbType.String, ObjUnitVO.ServerName);

                if (ObjUnitVO.DatabaseName != null) ObjUnitVO.DatabaseName = ObjUnitVO.DatabaseName.Trim();
                dbServer.AddInParameter(command, "DatabaseName", DbType.String, ObjUnitVO.DatabaseName);


                dbServer.AddInParameter(command, "PharmacyLicenseNo", DbType.String, ObjUnitVO.PharmacyLicenseNo);
                dbServer.AddInParameter(command, "ClinicRegNo", DbType.String, ObjUnitVO.ClinicRegNo);
                dbServer.AddInParameter(command, "ShopNo", DbType.String, ObjUnitVO.ShopNo);
                dbServer.AddInParameter(command, "TradeNo", DbType.String, ObjUnitVO.TradeNo);
                //added by neena
                dbServer.AddInParameter(command, "GSTNNo", DbType.String, ObjUnitVO.GSTNNo);
                //

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjUnitVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                //add by akshays
                dbServer.AddInParameter(command, "Countryid", DbType.Int64, ObjUnitVO.Countryid);
                dbServer.AddInParameter(command, "Stateid", DbType.Int64, ObjUnitVO.Stateid);
                dbServer.AddInParameter(command, "Areaid", DbType.Int64, ObjUnitVO.Areaid);
                dbServer.AddInParameter(command, "Cityid", DbType.Int64, ObjUnitVO.Cityid);
                //close by akshays

                //added by rohini dated 5.2.16
                dbServer.AddInParameter(command, "IsProcessingUnit", DbType.Boolean, ObjUnitVO.IsProcessingUnit);
                dbServer.AddInParameter(command, "IsCollectionUnit", DbType.Boolean, ObjUnitVO.IsCollectionUnit);
                //
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjUnitVO.UnitID);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.UnitDetails.UnitID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.SuccessStatus != -9)
                {
                    foreach (var ObjDept in ObjUnitVO.DepartmentDetails)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUnitDepartmentDetails");

                        dbServer.AddOutParameter(command1, "ID", DbType.Int64, int.MaxValue);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, ObjUnitVO.UnitID);
                        dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, ObjDept.DepartmentID);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjDept.Status);
                        //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDept.ID);



                        int iStatus = dbServer.ExecuteNonQuery(command1);
                        ObjDept.ID = (long)dbServer.GetParameterValue(command1, "ID");

                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

            return BizActionObj;
        }

        private clsAddUnitMasterBizActionVO UpdateUnit(clsAddUnitMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsUnitMasterVO ObjUnitVO = BizActionObj.UnitDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateUnitMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjUnitVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjUnitVO.UnitCode.Trim());
                dbServer.AddInParameter(command, "Name", DbType.String, ObjUnitVO.Description.Trim());
                dbServer.AddInParameter(command, "ClusterID", DbType.Int64, ObjUnitVO.ClusterID);
                dbServer.AddInParameter(command, "Description", DbType.String, ObjUnitVO.Description.Trim());

                if (ObjUnitVO.AddressLine1 != null) ObjUnitVO.AddressLine1 = ObjUnitVO.AddressLine1.Trim();
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, ObjUnitVO.AddressLine1);

                if (ObjUnitVO.AddressLine2 != null) ObjUnitVO.AddressLine2 = ObjUnitVO.AddressLine2.Trim();
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, ObjUnitVO.AddressLine2);

                if (ObjUnitVO.AddressLine3 != null) ObjUnitVO.AddressLine3 = ObjUnitVO.AddressLine3.Trim();
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, ObjUnitVO.AddressLine3);

                if (ObjUnitVO.Country != null) ObjUnitVO.Country = ObjUnitVO.Country.Trim();
                dbServer.AddInParameter(command, "Country", DbType.String, ObjUnitVO.Country);
                if (ObjUnitVO.State != null) ObjUnitVO.State = ObjUnitVO.State.Trim();
                dbServer.AddInParameter(command, "State", DbType.String, ObjUnitVO.State);
                if (ObjUnitVO.City != null) ObjUnitVO.City = ObjUnitVO.City.Trim();
                dbServer.AddInParameter(command, "City", DbType.String, ObjUnitVO.City);
                if (ObjUnitVO.Taluka != null) ObjUnitVO.Taluka = ObjUnitVO.Taluka.Trim();
                dbServer.AddInParameter(command, "Taluka", DbType.String, ObjUnitVO.Taluka);
                if (ObjUnitVO.Area != null) ObjUnitVO.Area = ObjUnitVO.Area.Trim();
                dbServer.AddInParameter(command, "Area", DbType.String, ObjUnitVO.Area);
                if (ObjUnitVO.District != null) ObjUnitVO.District = ObjUnitVO.District.Trim();
                dbServer.AddInParameter(command, "District", DbType.String, ObjUnitVO.District);
                if (ObjUnitVO.Pincode != null) ObjUnitVO.Pincode = ObjUnitVO.Pincode.Trim();
                dbServer.AddInParameter(command, "Pincode", DbType.String, ObjUnitVO.Pincode);

                if (ObjUnitVO.ContactNo != null) ObjUnitVO.ContactNo = ObjUnitVO.ContactNo.Trim();
                dbServer.AddInParameter(command, "ContactNo", DbType.String, ObjUnitVO.ContactNo);

                //added by neena
                if (ObjUnitVO.ContactNo1 != null) ObjUnitVO.ContactNo1 = ObjUnitVO.ContactNo1.Trim();
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, ObjUnitVO.ContactNo1);

                if (ObjUnitVO.MobileNO!= null) ObjUnitVO.MobileNO = ObjUnitVO.MobileNO.Trim();
                dbServer.AddInParameter(command, "MobileNO", DbType.String, ObjUnitVO.MobileNO);

                dbServer.AddInParameter(command, "MobileCountryCode", DbType.Int32, ObjUnitVO.MobileCountryCode);
                //


                //added by rohini dated 5.2.16
                dbServer.AddInParameter(command, "IsProcessingUnit", DbType.Boolean, ObjUnitVO.IsProcessingUnit);
                dbServer.AddInParameter(command, "IsCollectionUnit", DbType.Boolean, ObjUnitVO.IsCollectionUnit);
                //

                dbServer.AddInParameter(command, "ResiNoCountryCode", DbType.Int32, ObjUnitVO.ResiNoCountryCode);
                dbServer.AddInParameter(command, "ResiSTDCode", DbType.Int32, ObjUnitVO.ResiSTDCode);

                if (ObjUnitVO.FaxNo != null) ObjUnitVO.FaxNo = ObjUnitVO.FaxNo.Trim();
                dbServer.AddInParameter(command, "FaxNo", DbType.String, ObjUnitVO.FaxNo);
                if (ObjUnitVO.Email != null) ObjUnitVO.Email = ObjUnitVO.Email.Trim();
                dbServer.AddInParameter(command, "Email", DbType.String, ObjUnitVO.Email);

                if (ObjUnitVO.ServerName != null) ObjUnitVO.ServerName = ObjUnitVO.ServerName.Trim();
                dbServer.AddInParameter(command, "ServerName", DbType.String, ObjUnitVO.ServerName);

                if (ObjUnitVO.DatabaseName != null) ObjUnitVO.DatabaseName = ObjUnitVO.DatabaseName.Trim();
                dbServer.AddInParameter(command, "DatabaseName", DbType.String, ObjUnitVO.DatabaseName);

                //add by akshays
                dbServer.AddInParameter(command, "Countryid", DbType.Int64, ObjUnitVO.Countryid);
                dbServer.AddInParameter(command, "Stateid", DbType.Int64, ObjUnitVO.Stateid);
                dbServer.AddInParameter(command, "Areaid", DbType.Int64, ObjUnitVO.Areaid);
                dbServer.AddInParameter(command, "Cityid", DbType.Int64, ObjUnitVO.Cityid);
                //close by akshays

                dbServer.AddInParameter(command, "PharmacyLicenseNo", DbType.String, ObjUnitVO.PharmacyLicenseNo);
                dbServer.AddInParameter(command, "ClinicRegNo", DbType.String, ObjUnitVO.ClinicRegNo);
                dbServer.AddInParameter(command, "TinNo", DbType.String, ObjUnitVO.TINNo);
                dbServer.AddInParameter(command, "ShopNo", DbType.String, ObjUnitVO.ShopNo);
                dbServer.AddInParameter(command, "TradeNo", DbType.String, ObjUnitVO.TradeNo);
                //added by neena
                dbServer.AddInParameter(command, "GSTNNo", DbType.String, ObjUnitVO.GSTNNo);
                //

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjUnitVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command);


                if (ObjUnitVO.DepartmentDetails != null && ObjUnitVO.DepartmentDetails.Count > 0)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteUnitDepartmentDetails");

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, ObjUnitVO.UnitID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2);
                }


                foreach (var ObjDept in ObjUnitVO.DepartmentDetails)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUnitDepartmentDetails");

                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, int.MaxValue);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, ObjUnitVO.UnitID);
                    dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, ObjDept.DepartmentID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjDept.Status);
                    //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDept.ID);



                    int iStatus = dbServer.ExecuteNonQuery(command1);
                    ObjDept.ID = (long)dbServer.GetParameterValue(command1, "ID");

                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
            }

            return BizActionObj;
        }


        public override IValueObject GetDepartmentList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListBizActionVO BizActionObj = (clsGetDepartmentListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDepartmentList");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.UnitDetails == null)
                        BizActionObj.UnitDetails = new List<clsUnitMasterVO>();
                    while (reader.Read())
                    {
                        clsUnitMasterVO UnitVO = new clsUnitMasterVO();
                        UnitVO.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        UnitVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        UnitVO.Department = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        UnitVO.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));
                        UnitVO.IsActive = (bool)(DALHelper.HandleDBNull(reader["Status"]));
                        UnitVO.IsClinic = (bool)(DALHelper.HandleDBNull(reader["IsClinical"]));
                        BizActionObj.UnitDetails.Add(UnitVO);
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

        public override IValueObject GetUnitList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitMasterListBizActionVO BizActionObj = (clsGetUnitMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUnitDetailsList");

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.UnitDetails == null)
                        BizActionObj.UnitDetails = new List<clsUnitMasterVO>();
                    while (reader.Read())
                    {
                        clsUnitMasterVO UnitVO = new clsUnitMasterVO();
                        UnitVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        UnitVO.UnitCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        UnitVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        UnitVO.AddressLine1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"]));
                        UnitVO.AddressLine2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"]));
                        UnitVO.AddressLine3 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"]));
                        UnitVO.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo"]));
                        UnitVO.Email = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                        UnitVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.UnitDetails.Add(UnitVO);
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


        public override IValueObject GetUnitDetailsByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitDetailsByIDBizActionVO BizActionObj = (clsGetUnitDetailsByIDBizActionVO)valueObject;

            try
            {
                clsUnitMasterVO ObjUnitVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUnitDetailsByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.Details == null)
                            BizActionObj.Details = new clsUnitMasterVO();

                        BizActionObj.Details.UnitID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.Details.UnitCode = (string)DALHelper.HandleDBNull(reader["Code"]);
                        BizActionObj.Details.Name = (string)DALHelper.HandleDBNull(reader["Name"]);
                        BizActionObj.Details.ClusterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClusterID"]));
                        BizActionObj.Details.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.Details.AddressLine1 = (string)DALHelper.HandleDBNull(reader["AddressLine1"]);
                        BizActionObj.Details.AddressLine2 = (string)DALHelper.HandleDBNull(reader["AddressLine2"]);
                        BizActionObj.Details.AddressLine3 = (string)DALHelper.HandleDBNull(reader["AddressLine3"]);
                        BizActionObj.Details.Country = (string)DALHelper.HandleDBNull(reader["Country"]);
                        BizActionObj.Details.State = (string)DALHelper.HandleDBNull(reader["State"]);
                        BizActionObj.Details.District = (string)DALHelper.HandleDBNull(reader["District"]);
                        BizActionObj.Details.Taluka = (string)DALHelper.HandleDBNull(reader["Taluka"]);
                        BizActionObj.Details.Area = (string)DALHelper.HandleDBNull(reader["Area"]);
                        BizActionObj.Details.Pincode = (string)DALHelper.HandleDBNull(reader["Pincode"]);
                        BizActionObj.Details.Email = (string)DALHelper.HandleDBNull(reader["Email"]);
                        BizActionObj.Details.FaxNo = (string)DALHelper.HandleDBNull(reader["FaxNo"]);
                        BizActionObj.Details.ResiSTDCode = (int)DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        BizActionObj.Details.ResiNoCountryCode = (int)DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        BizActionObj.Details.ContactNo = (string)DALHelper.HandleDBNull(reader["ContactNo"]);
                        BizActionObj.Details.City = (string)DALHelper.HandleDBNull(reader["City"]);
                        BizActionObj.Details.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.Details.ServerName = (string)DALHelper.HandleDBNull(reader["ServerName"]);
                        BizActionObj.Details.DatabaseName = (string)DALHelper.HandleDBNull(reader["DatabaseName"]);
                        BizActionObj.Details.PharmacyLicenseNo = (string)DALHelper.HandleDBNull(reader["PharmacyLicenseNo"]);
                        BizActionObj.Details.ClinicRegNo = (string)DALHelper.HandleDBNull(reader["ClinicRegNo"]);
                        BizActionObj.Details.TINNo = Convert.ToString(DALHelper.HandleDBNull(reader["TinNo"]));
                        BizActionObj.Details.ShopNo = (string)DALHelper.HandleDBNull(reader["ShopNo"]);
                        BizActionObj.Details.TradeNo = (string)DALHelper.HandleDBNull(reader["TradeNo"]);
                        //added by akshays 
                        BizActionObj.Details.Countryid = (long)DALHelper.HandleDBNull(reader["Countryid"]);
                        BizActionObj.Details.Stateid = (long)DALHelper.HandleDBNull(reader["Stateid"]);
                        BizActionObj.Details.Cityid = (long)DALHelper.HandleDBNull(reader["Cityid"]);
                        BizActionObj.Details.Areaid = (long)DALHelper.HandleDBNull(reader["Areaid"]);
                        //closed by akshays
                        //closed by rohini dated 5.2.16 for pathology
                        BizActionObj.Details.IsCollectionUnit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCollectionUnit"]));
                        BizActionObj.Details.IsProcessingUnit =Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsProcessingUnit"]));
                        
                        //added by neena
                        BizActionObj.Details.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        BizActionObj.Details.MobileCountryCode =Convert.ToInt32(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        BizActionObj.Details.MobileNO = (string)DALHelper.HandleDBNull(reader["MobileNO"]);
                        BizActionObj.Details.GSTNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GSTNNo"]));
                        //

                        BizActionObj.SuccessStatus = true;
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    BizActionObj.Details.DepartmentDetails = new List<clsDepartmentDetailsVO>();
                    while (reader.Read())
                    {
                        clsDepartmentDetailsVO objDepartment = new clsDepartmentDetailsVO();
                        objDepartment.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objDepartment.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        objDepartment.Department = (string)DALHelper.HandleDBNull(reader["Department"]);

                        objDepartment.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.Details.DepartmentDetails.Add(objDepartment);
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


        public override IValueObject GetUserWiseUnitList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitDetailsByIDBizActionVO BizActionObj = (clsGetUnitDetailsByIDBizActionVO)valueObject;
            try
            {
               // MasterListItem ObjUnitVO = BizActionObj.ObjMasterList;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUserWiseUnitList");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.UserID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ObjMasterList == null)
                        BizActionObj.ObjMasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem obj = new MasterListItem();
                        obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        obj.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        obj.Description = (string)DALHelper.HandleDBNull(reader["Name"]);
                        BizActionObj.ObjMasterList.Add(obj);
                    }
                    BizActionObj.SuccessStatus = true;
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
    }
}
