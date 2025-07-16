namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.UnitMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsUnitMasterDAL : clsBaseUnitMasterDAL
    {
        private Database dbServer;

        public clsUnitMasterDAL()
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

        private clsAddUnitMasterBizActionVO AddUnit(clsAddUnitMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsUnitMasterVO unitDetails = BizActionObj.UnitDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUnitMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, unitDetails.UnitCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, unitDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "ClusterID", DbType.Int64, unitDetails.ClusterID);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, unitDetails.Description.Trim());
                if (unitDetails.AddressLine1 != null)
                {
                    unitDetails.AddressLine1 = unitDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, unitDetails.AddressLine1);
                if (unitDetails.AddressLine2 != null)
                {
                    unitDetails.AddressLine2 = unitDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, unitDetails.AddressLine2);
                if (unitDetails.AddressLine3 != null)
                {
                    unitDetails.AddressLine3 = unitDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, unitDetails.AddressLine3);
                if (unitDetails.Country != null)
                {
                    unitDetails.Country = unitDetails.Country.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, unitDetails.Country);
                if (unitDetails.State != null)
                {
                    unitDetails.State = unitDetails.State.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, unitDetails.State);
                if (unitDetails.City != null)
                {
                    unitDetails.City = unitDetails.City.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, unitDetails.City);
                if (unitDetails.Taluka != null)
                {
                    unitDetails.Taluka = unitDetails.Taluka.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, unitDetails.Taluka);
                if (unitDetails.Area != null)
                {
                    unitDetails.Area = unitDetails.Area.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, unitDetails.Area);
                if (unitDetails.District != null)
                {
                    unitDetails.District = unitDetails.District.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, unitDetails.District);
                if (unitDetails.Pincode != null)
                {
                    unitDetails.Pincode = unitDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, unitDetails.Pincode);
                if (unitDetails.ContactNo != null)
                {
                    unitDetails.ContactNo = unitDetails.ContactNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, unitDetails.ContactNo);
                if (unitDetails.ContactNo1 != null)
                {
                    unitDetails.ContactNo1 = unitDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, unitDetails.ContactNo1);
                if (unitDetails.MobileNO != null)
                {
                    unitDetails.MobileNO = unitDetails.MobileNO.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MobileNO", DbType.String, unitDetails.MobileNO);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.Int32, unitDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int32, unitDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int32, unitDetails.ResiSTDCode);
                if (unitDetails.FaxNo != null)
                {
                    unitDetails.FaxNo = unitDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, unitDetails.FaxNo);
                if (unitDetails.Email != null)
                {
                    unitDetails.Email = unitDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, unitDetails.Email);
                if (unitDetails.ServerName != null)
                {
                    unitDetails.ServerName = unitDetails.ServerName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ServerName", DbType.String, unitDetails.ServerName);
                if (unitDetails.DatabaseName != null)
                {
                    unitDetails.DatabaseName = unitDetails.DatabaseName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "DatabaseName", DbType.String, unitDetails.DatabaseName);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyLicenseNo", DbType.String, unitDetails.PharmacyLicenseNo);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicRegNo", DbType.String, unitDetails.ClinicRegNo);
                this.dbServer.AddInParameter(storedProcCommand, "ShopNo", DbType.String, unitDetails.ShopNo);
                this.dbServer.AddInParameter(storedProcCommand, "TradeNo", DbType.String, unitDetails.TradeNo);
                this.dbServer.AddInParameter(storedProcCommand, "GSTNNo", DbType.String, unitDetails.GSTNNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, unitDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Countryid", DbType.Int64, unitDetails.Countryid);
                this.dbServer.AddInParameter(storedProcCommand, "Stateid", DbType.Int64, unitDetails.Stateid);
                this.dbServer.AddInParameter(storedProcCommand, "Areaid", DbType.Int64, unitDetails.Areaid);
                this.dbServer.AddInParameter(storedProcCommand, "Cityid", DbType.Int64, unitDetails.Cityid);
                this.dbServer.AddInParameter(storedProcCommand, "IsProcessingUnit", DbType.Boolean, unitDetails.IsProcessingUnit);
                this.dbServer.AddInParameter(storedProcCommand, "IsCollectionUnit", DbType.Boolean, unitDetails.IsCollectionUnit);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, unitDetails.UnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.UnitDetails.UnitID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (BizActionObj.SuccessStatus != -9)
                {
                    foreach (clsDepartmentDetailsVO svo in unitDetails.DepartmentDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUnitDepartmentDetails");
                        this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0x7fffffff);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, unitDetails.UnitID);
                        this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, svo.Status);
                        this.dbServer.ExecuteNonQuery(command2);
                        svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddUnitMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUnitMasterBizActionVO bizActionObj = valueObject as clsAddUnitMasterBizActionVO;
            return ((bizActionObj.UnitDetails.UnitID != 0L) ? this.UpdateUnit(bizActionObj, objUserVO) : this.AddUnit(bizActionObj, objUserVO));
        }

        public override IValueObject GetDepartmentList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDepartmentListBizActionVO nvo = (clsGetDepartmentListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDepartmentList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.UnitDetails == null)
                    {
                        nvo.UnitDetails = new List<clsUnitMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsUnitMasterVO item = new clsUnitMasterVO {
                            DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Department = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            IsActive = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            IsClinic = (bool) DALHelper.HandleDBNull(reader["IsClinical"])
                        };
                        nvo.UnitDetails.Add(item);
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

        public override IValueObject GetUnitDetailsByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitDetailsByIDBizActionVO nvo = (clsGetUnitDetailsByIDBizActionVO) valueObject;
            try
            {
                clsUnitMasterVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnitDetailsByID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.Details == null)
                        {
                            nvo.Details = new clsUnitMasterVO();
                        }
                        nvo.Details.UnitID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.Details.UnitCode = (string) DALHelper.HandleDBNull(reader["Code"]);
                        nvo.Details.Name = (string) DALHelper.HandleDBNull(reader["Name"]);
                        nvo.Details.ClusterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClusterID"]));
                        nvo.Details.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.Details.AddressLine1 = (string) DALHelper.HandleDBNull(reader["AddressLine1"]);
                        nvo.Details.AddressLine2 = (string) DALHelper.HandleDBNull(reader["AddressLine2"]);
                        nvo.Details.AddressLine3 = (string) DALHelper.HandleDBNull(reader["AddressLine3"]);
                        nvo.Details.Country = (string) DALHelper.HandleDBNull(reader["Country"]);
                        nvo.Details.State = (string) DALHelper.HandleDBNull(reader["State"]);
                        nvo.Details.District = (string) DALHelper.HandleDBNull(reader["District"]);
                        nvo.Details.Taluka = (string) DALHelper.HandleDBNull(reader["Taluka"]);
                        nvo.Details.Area = (string) DALHelper.HandleDBNull(reader["Area"]);
                        nvo.Details.Pincode = (string) DALHelper.HandleDBNull(reader["Pincode"]);
                        nvo.Details.Email = (string) DALHelper.HandleDBNull(reader["Email"]);
                        nvo.Details.FaxNo = (string) DALHelper.HandleDBNull(reader["FaxNo"]);
                        nvo.Details.ResiSTDCode = (int) DALHelper.HandleDBNull(reader["ResiSTDCode"]);
                        nvo.Details.ResiNoCountryCode = (int) DALHelper.HandleDBNull(reader["ResiNoCountryCode"]);
                        nvo.Details.ContactNo = (string) DALHelper.HandleDBNull(reader["ContactNo"]);
                        nvo.Details.City = (string) DALHelper.HandleDBNull(reader["City"]);
                        nvo.Details.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.Details.ServerName = (string) DALHelper.HandleDBNull(reader["ServerName"]);
                        nvo.Details.DatabaseName = (string) DALHelper.HandleDBNull(reader["DatabaseName"]);
                        nvo.Details.PharmacyLicenseNo = (string) DALHelper.HandleDBNull(reader["PharmacyLicenseNo"]);
                        nvo.Details.ClinicRegNo = (string) DALHelper.HandleDBNull(reader["ClinicRegNo"]);
                        nvo.Details.TINNo = Convert.ToString(DALHelper.HandleDBNull(reader["TinNo"]));
                        nvo.Details.ShopNo = (string) DALHelper.HandleDBNull(reader["ShopNo"]);
                        nvo.Details.TradeNo = (string) DALHelper.HandleDBNull(reader["TradeNo"]);
                        nvo.Details.Countryid = (long) DALHelper.HandleDBNull(reader["Countryid"]);
                        nvo.Details.Stateid = (long) DALHelper.HandleDBNull(reader["Stateid"]);
                        nvo.Details.Cityid = (long) DALHelper.HandleDBNull(reader["Cityid"]);
                        nvo.Details.Areaid = (long) DALHelper.HandleDBNull(reader["Areaid"]);
                        nvo.Details.IsCollectionUnit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCollectionUnit"]));
                        nvo.Details.IsProcessingUnit = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsProcessingUnit"]));
                        nvo.Details.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        nvo.Details.MobileCountryCode = Convert.ToInt32(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        nvo.Details.MobileNO = (string) DALHelper.HandleDBNull(reader["MobileNO"]);
                        nvo.Details.GSTNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GSTNNo"]));
                        nvo.SuccessStatus = true;
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.Details.DepartmentDetails = new List<clsDepartmentDetailsVO>();
                    while (reader.Read())
                    {
                        clsDepartmentDetailsVO item = new clsDepartmentDetailsVO {
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            DepartmentID = (long) DALHelper.HandleDBNull(reader["DepartmentID"]),
                            Department = (string) DALHelper.HandleDBNull(reader["Department"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.Details.DepartmentDetails.Add(item);
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

        public override IValueObject GetUnitList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitMasterListBizActionVO nvo = (clsGetUnitMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUnitDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.UnitDetails == null)
                    {
                        nvo.UnitDetails = new List<clsUnitMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsUnitMasterVO item = new clsUnitMasterVO {
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            AddressLine1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"])),
                            AddressLine2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"])),
                            AddressLine3 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"])),
                            ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo"])),
                            Email = Convert.ToString(DALHelper.HandleDBNull(reader["Email"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.UnitDetails.Add(item);
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

        public override IValueObject GetUserWiseUnitList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitDetailsByIDBizActionVO nvo = (clsGetUnitDetailsByIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUserWiseUnitList");
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ObjMasterList == null)
                    {
                        nvo.ObjMasterList = new List<MasterListItem>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            nvo.SuccessStatus = true;
                            break;
                        }
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Name"])
                        };
                        nvo.ObjMasterList.Add(item);
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

        private clsAddUnitMasterBizActionVO UpdateUnit(clsAddUnitMasterBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            try
            {
                clsUnitMasterVO unitDetails = BizActionObj.UnitDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateUnitMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, unitDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, unitDetails.UnitCode.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, unitDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "ClusterID", DbType.Int64, unitDetails.ClusterID);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, unitDetails.Description.Trim());
                if (unitDetails.AddressLine1 != null)
                {
                    unitDetails.AddressLine1 = unitDetails.AddressLine1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, unitDetails.AddressLine1);
                if (unitDetails.AddressLine2 != null)
                {
                    unitDetails.AddressLine2 = unitDetails.AddressLine2.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, unitDetails.AddressLine2);
                if (unitDetails.AddressLine3 != null)
                {
                    unitDetails.AddressLine3 = unitDetails.AddressLine3.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, unitDetails.AddressLine3);
                if (unitDetails.Country != null)
                {
                    unitDetails.Country = unitDetails.Country.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, unitDetails.Country);
                if (unitDetails.State != null)
                {
                    unitDetails.State = unitDetails.State.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, unitDetails.State);
                if (unitDetails.City != null)
                {
                    unitDetails.City = unitDetails.City.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, unitDetails.City);
                if (unitDetails.Taluka != null)
                {
                    unitDetails.Taluka = unitDetails.Taluka.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Taluka", DbType.String, unitDetails.Taluka);
                if (unitDetails.Area != null)
                {
                    unitDetails.Area = unitDetails.Area.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, unitDetails.Area);
                if (unitDetails.District != null)
                {
                    unitDetails.District = unitDetails.District.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, unitDetails.District);
                if (unitDetails.Pincode != null)
                {
                    unitDetails.Pincode = unitDetails.Pincode.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, unitDetails.Pincode);
                if (unitDetails.ContactNo != null)
                {
                    unitDetails.ContactNo = unitDetails.ContactNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, unitDetails.ContactNo);
                if (unitDetails.ContactNo1 != null)
                {
                    unitDetails.ContactNo1 = unitDetails.ContactNo1.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, unitDetails.ContactNo1);
                if (unitDetails.MobileNO != null)
                {
                    unitDetails.MobileNO = unitDetails.MobileNO.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "MobileNO", DbType.String, unitDetails.MobileNO);
                this.dbServer.AddInParameter(storedProcCommand, "MobileCountryCode", DbType.Int32, unitDetails.MobileCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "IsProcessingUnit", DbType.Boolean, unitDetails.IsProcessingUnit);
                this.dbServer.AddInParameter(storedProcCommand, "IsCollectionUnit", DbType.Boolean, unitDetails.IsCollectionUnit);
                this.dbServer.AddInParameter(storedProcCommand, "ResiNoCountryCode", DbType.Int32, unitDetails.ResiNoCountryCode);
                this.dbServer.AddInParameter(storedProcCommand, "ResiSTDCode", DbType.Int32, unitDetails.ResiSTDCode);
                if (unitDetails.FaxNo != null)
                {
                    unitDetails.FaxNo = unitDetails.FaxNo.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "FaxNo", DbType.String, unitDetails.FaxNo);
                if (unitDetails.Email != null)
                {
                    unitDetails.Email = unitDetails.Email.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "Email", DbType.String, unitDetails.Email);
                if (unitDetails.ServerName != null)
                {
                    unitDetails.ServerName = unitDetails.ServerName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ServerName", DbType.String, unitDetails.ServerName);
                if (unitDetails.DatabaseName != null)
                {
                    unitDetails.DatabaseName = unitDetails.DatabaseName.Trim();
                }
                this.dbServer.AddInParameter(storedProcCommand, "DatabaseName", DbType.String, unitDetails.DatabaseName);
                this.dbServer.AddInParameter(storedProcCommand, "Countryid", DbType.Int64, unitDetails.Countryid);
                this.dbServer.AddInParameter(storedProcCommand, "Stateid", DbType.Int64, unitDetails.Stateid);
                this.dbServer.AddInParameter(storedProcCommand, "Areaid", DbType.Int64, unitDetails.Areaid);
                this.dbServer.AddInParameter(storedProcCommand, "Cityid", DbType.Int64, unitDetails.Cityid);
                this.dbServer.AddInParameter(storedProcCommand, "PharmacyLicenseNo", DbType.String, unitDetails.PharmacyLicenseNo);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicRegNo", DbType.String, unitDetails.ClinicRegNo);
                this.dbServer.AddInParameter(storedProcCommand, "TinNo", DbType.String, unitDetails.TINNo);
                this.dbServer.AddInParameter(storedProcCommand, "ShopNo", DbType.String, unitDetails.ShopNo);
                this.dbServer.AddInParameter(storedProcCommand, "TradeNo", DbType.String, unitDetails.TradeNo);
                this.dbServer.AddInParameter(storedProcCommand, "GSTNNo", DbType.String, unitDetails.GSTNNo);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, unitDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                if ((unitDetails.DepartmentDetails != null) && (unitDetails.DepartmentDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteUnitDepartmentDetails");
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, unitDetails.UnitID);
                    this.dbServer.ExecuteNonQuery(command2);
                }
                foreach (clsDepartmentDetailsVO svo in unitDetails.DepartmentDetails)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUnitDepartmentDetails");
                    this.dbServer.AddOutParameter(command3, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, unitDetails.UnitID);
                    this.dbServer.AddInParameter(command3, "DepartmentID", DbType.Int64, svo.DepartmentID);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, svo.Status);
                    this.dbServer.ExecuteNonQuery(command3);
                    svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
            }
            return BizActionObj;
        }
    }
}

