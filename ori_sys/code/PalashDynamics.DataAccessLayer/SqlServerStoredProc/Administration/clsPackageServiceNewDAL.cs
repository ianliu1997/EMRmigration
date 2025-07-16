using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using System.Data;
using System.Data.Common;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.ValueObjects.Administration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsPackageServiceNewDAL : clsBasePackageServiceNewDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsPackageServiceNewDAL()
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

        public override IValueObject AddPackageServiceMaster(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsAddServiceMasterNewBizActionVO objItem = valueObject as clsAddServiceMasterNewBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = null;
                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;


                #region OLD ServiceMaster
                if (objItemVO.IsNewMaster == false)
                {
                    if (objItemVO.EditMode == true)
                    {

                        command = dbServer.GetStoredProcCommand("CIMS_UpdateServiceMasterForPackageNew");   //CIMS_UpdateServiceMaster
                        command.Connection = con;
                        dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, userVO.ID);
                        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    }
                    else
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_AddServiceMasterForPackageNew");  //CIMS_AddServiceMaster
                        command.Connection = con;
                        dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "ServiceCode", DbType.String, objItemVO.ServiceCode);
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItemVO.ServiceID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, userVO.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);

                    }
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CodeType", DbType.Int64, objItemVO.CodeType);
                    dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                    dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItemVO.Specialization);
                    dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItemVO.SubSpecialization);
                    dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.ServiceName);
                    dbServer.AddInParameter(command, "ShortDescription", DbType.String, objItemVO.ShortDescription);
                    dbServer.AddInParameter(command, "LongDescription", DbType.String, objItemVO.LongDescription);

                    dbServer.AddInParameter(command, "StaffDiscount", DbType.Boolean, objItemVO.StaffDiscount);
                    dbServer.AddInParameter(command, "StaffDiscountAmount", DbType.Decimal, objItemVO.StaffDiscountAmount);
                    dbServer.AddInParameter(command, "StaffDiscountPercent", DbType.Decimal, objItemVO.StaffDiscountPercent);

                    dbServer.AddInParameter(command, "StaffDependantDiscount", DbType.Boolean, objItemVO.StaffDependantDiscount);
                    dbServer.AddInParameter(command, "StaffDependantDiscountAmount", DbType.Decimal, objItemVO.StaffDependantDiscountAmount);
                    dbServer.AddInParameter(command, "StaffDependantDiscountPercent", DbType.Decimal, objItemVO.StaffDependantDiscountPercent);


                    dbServer.AddInParameter(command, "Concession", DbType.Boolean, objItemVO.Concession);
                    dbServer.AddInParameter(command, "ConcessionAmount", DbType.Decimal, objItemVO.ConcessionAmount);
                    dbServer.AddInParameter(command, "ConcessionPercent", DbType.Decimal, objItemVO.ConcessionPercent);

                    dbServer.AddInParameter(command, "ServiceTax", DbType.Boolean, objItemVO.ServiceTax);
                    dbServer.AddInParameter(command, "ServiceTaxAmount", DbType.Decimal, objItemVO.ServiceTaxAmount);
                    dbServer.AddInParameter(command, "ServiceTaxPercent", DbType.Decimal, objItemVO.ServiceTaxPercent);

                    dbServer.AddInParameter(command, "SeniorCitizen", DbType.Boolean, objItemVO.SeniorCitizen);
                    dbServer.AddInParameter(command, "SeniorCitizenConAmount", DbType.Decimal, objItemVO.SeniorCitizenConAmount);
                    dbServer.AddInParameter(command, "SeniorCitizenConPercent", DbType.Decimal, objItemVO.SeniorCitizenConPercent);
                    dbServer.AddInParameter(command, "SeniorCitizenAge", DbType.Int16, objItemVO.SeniorCitizenAge);

                    dbServer.AddInParameter(command, "IsFamily", DbType.Boolean, objItemVO.IsFamily);
                    dbServer.AddInParameter(command, "FamilyMemberCount", DbType.Int32, objItemVO.FamilyMemberCount);
                    dbServer.AddInParameter(command, "IsPackage", DbType.Boolean, objItemVO.IsPackage);
                    dbServer.AddInParameter(command, "IsFavorite", DbType.Boolean, objItemVO.IsFavorite);
                    dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objItemVO.ExpiryDate);
                    dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objItemVO.EffectiveDate);
                    dbServer.AddInParameter(command, "InHouse", DbType.Boolean, objItemVO.InHouse);
                    dbServer.AddInParameter(command, "DoctorShare", DbType.Boolean, objItemVO.DoctorShare);
                    dbServer.AddInParameter(command, "DoctorSharePercentage", DbType.Decimal, objItemVO.DoctorSharePercentage);
                    dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Decimal, objItemVO.DoctorShareAmount);
                    dbServer.AddInParameter(command, "RateEditable", DbType.Boolean, objItemVO.RateEditable);
                    dbServer.AddInParameter(command, "MaxRate", DbType.Decimal, objItemVO.MaxRate);
                    dbServer.AddInParameter(command, "MinRate", DbType.Decimal, objItemVO.MinRate);
                    dbServer.AddInParameter(command, "Rate", DbType.Decimal, objItemVO.Rate);

                    dbServer.AddInParameter(command, "CheckedAllTariffs", DbType.Boolean, objItemVO.CheckedAllTariffs);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);

                    dbServer.AddInParameter(command, "IsMarkUp", DbType.Boolean, objItemVO.IsMarkUp);
                    dbServer.AddInParameter(command, "PercentageOnMrp", DbType.Decimal, objItemVO.PercentageOnMrp);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "ID")));

                    objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    if (objItem.FamilyMemberMasterDetails != null && objItem.FamilyMemberMasterDetails.Count > 0)
                    {
                        if (objItemVO.EditMode == true)
                        {
                            DbCommand command6 = dbServer.GetStoredProcCommand("CIMS_DeletePackageMember");        //CIMS_AddServiceItemMasterDetails
                            command6.Connection = con;

                            dbServer.AddInParameter(command6, "PackageServiceID", DbType.Int64, objItemVO.ID);
                            dbServer.AddInParameter(command6, "PackageServiceUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);

                        }

                        foreach (clsPackageRelationsVO ObjItem in objItem.FamilyMemberMasterDetails)
                        {
                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddPackageMemberRelationMasterDetails");  //CIMS_AddServiceItemMasterDetails
                            command5.Connection = con;

                            if (ObjItem.IsSetAll == true)
                            {
                                dbServer.AddInParameter(command5, "PackageServiceId", DbType.Int64, objItemVO.ID);
                                dbServer.AddInParameter(command5, "PackageServiceUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "RelationID", DbType.Int64, ObjItem.RelationID);
                                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "Status", DbType.Boolean, ObjItem.Status);

                                dbServer.AddInParameter(command5, "IsSetAll", DbType.Boolean, ObjItem.IsSetAll);

                                dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjItem.ID);
                                dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, 0);

                                int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                                ObjItem.ID = (long)dbServer.GetParameterValue(command5, "ID");
                                break;
                            }
                            else
                            {
                                dbServer.AddInParameter(command5, "PackageServiceId", DbType.Int64, objItemVO.ID);
                                dbServer.AddInParameter(command5, "PackageServiceUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "RelationID", DbType.Int64, ObjItem.RelationID);
                                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "Status", DbType.Boolean, ObjItem.Status);

                                dbServer.AddInParameter(command5, "IsSetAll", DbType.Boolean, false);

                                dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjItem.ID);
                                dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, 0);

                                int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                                ObjItem.ID = (long)dbServer.GetParameterValue(command5, "ID");
                            }

                        }
                    }



                }
                #endregion
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                objItem.ServiceMasterDetails = null;
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }
            return objItem;
        }

        public override IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListNewBizActionVO BizActionObj = (clsGetServiceMasterListNewBizActionVO)valueObject;
            try
            {
                //DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetails");
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetailsForPackage");   //CIMS_GetAllServiceDetails

                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "FromPackage", DbType.Boolean, BizActionObj.FromPackage);

                DbDataReader reader;

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");
                // dbServer.AddInParameter(command, "SearchExpression", DbType.String, Security.base64Encode(BizActionObj.SearchExpression));

                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.IsStatus);

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = (long)reader["ID"];
                        objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);

                        objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDesc"]);
                        objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDesc"]);
                        objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);

                        #  region Coomment

                        // objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        // objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);

                        // objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
                        // objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        // objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);

                        // objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
                        // objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                        // objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                        //// objServiceMasterVO.GeneralDiscount = (bool)DALHelper.HandleDBNull(reader["GeneralDiscount"]);

                        // objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
                        // objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        // objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
                        // objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
                        //// objServiceMasterVO.OutSource = (bool)DALHelper.HandleDBNull(reader["OutSource"]);
                        // //objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        // objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);

                        // objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
                        // objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                        // objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);

                        // objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        // objServiceMasterVO.Specialization = (Int64)DALHelper.HandleDBNull(reader["SpecializationId"]);

                        // objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                        // objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
                        // objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);

                        // objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
                        // objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                        // objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);

                        //  objServiceMasterVO.CheckedAllTariffs = (bool)DALHelper.HandleDBNull(reader["CheckedAllTariffs"]);
                        //objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //objServiceMasterVO.SubSpecialization = (Int64)DALHelper.HandleDBNull(reader["SubSpecializationId"]);

                        # endregion

                        objServiceMasterVO.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);

                        objServiceMasterVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));

                        objServiceMasterVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));

                        objServiceMasterVO.PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"]));
                        objServiceMasterVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objServiceMasterVO.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));

                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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

        public override IValueObject UpdatePackageServiceMasterStatus(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddServiceMasterBizActionVO objItem = valueObject as clsAddServiceMasterBizActionVO;

            try
            {
                DbCommand command = null;

                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;
                command = dbServer.GetStoredProcCommand("CIMS_UpdateServiceMasterStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);

                int intStatus = dbServer.ExecuteNonQuery(command);
                if (intStatus > 0)
                {
                    objItem.SuccessStatus = 1;

                }
                else
                {
                    objItem.SuccessStatus = 0;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;
        }

        public override IValueObject GetPackageServiceMasterDetailsForId(IValueObject valueObject, clsUserVO objUserVO)
        {
            try
            {
                clsGetServiceMasterListNewBizActionVO BizActionObj = valueObject as clsGetServiceMasterListNewBizActionVO;
                BizActionObj.ServiceMaster = BizActionObj.ServiceMaster;

                # region Comment
                //if (BizActionObj.IsNewServiceMaster == true)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceMasterDetails_New");
                //    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ID);
                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    DbDataReader reader;
                //    reader = (DbDataReader)dbServer.ExecuteReader(command);
                //    if (reader.HasRows)
                //    {
                //        while (reader.Read())
                //        {
                //            BizActionObj.ServiceMaster.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                //            BizActionObj.ServiceMaster.CodeType = (long)DALHelper.HandleDBNull(reader["CodeType"]);
                //            BizActionObj.ServiceMaster.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                //            BizActionObj.ServiceMaster.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                //            BizActionObj.ServiceMaster.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                //            BizActionObj.ServiceMaster.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                //            BizActionObj.ServiceMaster.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                //            BizActionObj.ServiceMaster.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                //            BizActionObj.ServiceMaster.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
                //            BizActionObj.ServiceMaster.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                //            BizActionObj.ServiceMaster.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                //            BizActionObj.ServiceMaster.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                //            BizActionObj.ServiceMaster.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
                //            BizActionObj.ServiceMaster.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
                //            BizActionObj.ServiceMaster.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
                //            BizActionObj.ServiceMaster.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                //            BizActionObj.ServiceMaster.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                //            BizActionObj.ServiceMaster.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
                //            BizActionObj.ServiceMaster.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                //            BizActionObj.ServiceMaster.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                //            BizActionObj.ServiceMaster.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
                //            BizActionObj.ServiceMaster.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
                //            BizActionObj.ServiceMaster.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                //            BizActionObj.ServiceMaster.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                //            BizActionObj.ServiceMaster.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
                //            BizActionObj.ServiceMaster.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
                //            BizActionObj.ServiceMaster.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
                //            BizActionObj.ServiceMaster.CheckedAllTariffs = (bool)DALHelper.HandleDBNull(reader["CheckedAllTariffs"]);
                //            BizActionObj.ServiceMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                //            BizActionObj.ServiceMaster.ServiceName = BizActionObj.ServiceMaster.Description;
                //            BizActionObj.ServiceMaster.IsFamily = (bool)DALHelper.HandleBoolDBNull(reader["IsFamily"]);
                //            BizActionObj.ServiceMaster.IsPackage = (bool)DALHelper.HandleDBNull(reader["IsPackage"]);
                //            BizActionObj.ServiceMaster.EffectiveDate = (DateTime?)DALHelper.HandleDate(reader["PackageEffectiveDate"]);
                //            BizActionObj.ServiceMaster.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["PackageExpiryDate"]);
                //            BizActionObj.ServiceMaster.SeniorCitizen = (bool)DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]);
                //            BizActionObj.ServiceMaster.SeniorCitizenConAmount = (decimal)DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]);
                //            BizActionObj.ServiceMaster.SeniorCitizenConPercent = (decimal)DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]);
                //            BizActionObj.ServiceMaster.SeniorCitizenAge = (int)DALHelper.HandleDBNull(reader["SeniorCitizenAge"]);

                //            BizActionObj.ServiceMaster.IsMarkUp = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMarkUp"]));
                //            BizActionObj.ServiceMaster.PercentageOnMrp = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PercentageOnMrp"]));

                //            BizActionObj.ServiceMaster.IsFavourite = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFavorite"]));
                //            BizActionObj.ServiceMaster.IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IslinkWithInventory"]));
                //            BizActionObj.ServiceMaster.CodeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["CodeDetails"]));
                //            BizActionObj.ServiceMaster.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseServiceRate"]));
                //        }
                //        reader.NextResult();
                //        reader.Close();
                //    }
                //}
                //else
                # endregion

                if (BizActionObj.IsNewServiceMaster == false)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceMasterDetailsForPackage");   //CIMS_GetAllServiceMasterDetails
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    DbDataReader reader;
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BizActionObj.ServiceMaster.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                            BizActionObj.ServiceMaster.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                            BizActionObj.ServiceMaster.CodeType = Convert.ToInt64(DALHelper.HandleDBNull(reader["CodeType"]));
                            BizActionObj.ServiceMaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            BizActionObj.ServiceMaster.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                            BizActionObj.ServiceMaster.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                            BizActionObj.ServiceMaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            BizActionObj.ServiceMaster.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                            BizActionObj.ServiceMaster.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                            BizActionObj.ServiceMaster.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                            BizActionObj.ServiceMaster.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                            BizActionObj.ServiceMaster.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                            BizActionObj.ServiceMaster.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                            BizActionObj.ServiceMaster.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                            BizActionObj.ServiceMaster.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                            BizActionObj.ServiceMaster.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                            BizActionObj.ServiceMaster.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            BizActionObj.ServiceMaster.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                            BizActionObj.ServiceMaster.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                            BizActionObj.ServiceMaster.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                            BizActionObj.ServiceMaster.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                            BizActionObj.ServiceMaster.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                            BizActionObj.ServiceMaster.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                            BizActionObj.ServiceMaster.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                            BizActionObj.ServiceMaster.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                            BizActionObj.ServiceMaster.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                            BizActionObj.ServiceMaster.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                            BizActionObj.ServiceMaster.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                            BizActionObj.ServiceMaster.CheckedAllTariffs = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CheckedAllTariffs"]));
                            BizActionObj.ServiceMaster.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            BizActionObj.ServiceMaster.ServiceName = BizActionObj.ServiceMaster.Description;

                            BizActionObj.ServiceMaster.IsFamily = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFamily"]));
                            BizActionObj.ServiceMaster.FamilyMemberCount = Convert.ToInt32(DALHelper.HandleDBNull(reader["FamilyMemberCount"]));

                            BizActionObj.ServiceMaster.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                            BizActionObj.ServiceMaster.EffectiveDate = (DateTime?)DALHelper.HandleDate(reader["PackageEffectiveDate"]);
                            BizActionObj.ServiceMaster.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["PackageExpiryDate"]);

                            BizActionObj.ServiceMaster.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                            BizActionObj.ServiceMaster.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                            BizActionObj.ServiceMaster.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                            BizActionObj.ServiceMaster.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));

                            BizActionObj.ServiceMaster.IsMarkUp = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMarkUp"]));
                            BizActionObj.ServiceMaster.PercentageOnMrp = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PercentageOnMrp"]));

                            BizActionObj.ServiceMaster.IsFavorite = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFavorite"]));
                            BizActionObj.ServiceMaster.IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IslinkWithInventory"]));
                            BizActionObj.ServiceMaster.CodeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["CodeDetails"]));
                            BizActionObj.ServiceMaster.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        }
                    }
                    reader.NextResult();



                    if (BizActionObj.FamilyMemberMasterDetails == null)
                        BizActionObj.FamilyMemberMasterDetails = new List<clsPackageRelationsVO>();

                    while (reader.Read())
                    {
                        clsPackageRelationsVO ObjItem = new clsPackageRelationsVO();
                        ObjItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ObjItem.PackageServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceID"]));
                        ObjItem.PackageServiceUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceUnitID"]));
                        ObjItem.RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RelationID"]));
                        ObjItem.Relation = Convert.ToString(DALHelper.HandleDBNull(reader["Relation"]));
                        ObjItem.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        //by Anjali............................
                        ObjItem.IsSetAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetAll"]));

                        BizActionObj.FamilyMemberMasterDetails.Add(ObjItem);
                    }



                    reader.Close();
                }
                return BizActionObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetPackageRelationsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            try
            {

                clsGetPackageRelationsBizActionVO BizActionObj = valueObject as clsGetPackageRelationsBizActionVO;
                BizActionObj.PackageRelationsList = BizActionObj.PackageRelationsList;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageMemberRelation");
                dbServer.AddInParameter(command, "PackageServiceID", DbType.Int64, BizActionObj.PackageServiceID);
                dbServer.AddInParameter(command, "PackageServiceUnitID", DbType.Int64, BizActionObj.PackageServiceUnitID);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MasterListItem objRelation = new MasterListItem();

                        objRelation.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objRelation.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        BizActionObj.PackageRelationsList.Add(objRelation);
                    }

                    reader.Close();
                }

                return BizActionObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddPackageServicesNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPackageServiceNewBizActionVO BizActionObj = valueObject as clsAddPackageServiceNewBizActionVO;

            if (BizActionObj.Details.PackageDetails[0].ID == 0)   //if (BizActionObj.Details.ID == 0)
                BizActionObj = AddPackageNew(BizActionObj, UserVo);
            else if (BizActionObj.Details.PackageDetails[0].ID > 0)
                BizActionObj = UpdatePackage(BizActionObj, UserVo);


            return valueObject;
        }

        private clsAddPackageServiceNewBizActionVO AddPackageNew(clsAddPackageServiceNewBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsPackageServiceVO objPackageVO = BizActionObj.Details;

                if (BizActionObj.Details.ID == 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPackageServiceForPackage");  //M_PackageServices

                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objPackageVO.ServiceID);
                    dbServer.AddInParameter(command, "Validity", DbType.String, objPackageVO.Validity);
                    dbServer.AddInParameter(command, "ValidityUnit", DbType.String, objPackageVO.ValidityUnit);
                    dbServer.AddInParameter(command, "PackageAmount", DbType.Double, objPackageVO.PackageAmount);
                    //dbServer.AddInParameter(command, "NoOfFollowUp", DbType.String, objPackageVO.NoOfFollowUp);

                    dbServer.AddInParameter(command, "ApplicableToAll", DbType.Boolean, objPackageVO.ApplicableToAll);
                    dbServer.AddInParameter(command, "ApplicableToAllDiscount", DbType.Double, objPackageVO.ApplicableToAllDiscount);

                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objPackageVO.IsFreezed);

                    // Added By CDS On 19/01/2017
                    //dbServer.AddInParameter(command, "IsFixed", DbType.Boolean, BizActionObj.IsFixedRate);
                    //dbServer.AddInParameter(command, "ServiceFixedRate", DbType.Double, BizActionObj.ServiceFixedRate);
                    //dbServer.AddInParameter(command, "PharmacyFixedRate", DbType.Double, BizActionObj.PharmacyFixedRate);

                    //dbServer.AddInParameter(command, "ServicePercentage", DbType.Double, BizActionObj.ServicePercentage);
                    //dbServer.AddInParameter(command, "PharmacyPercentage", DbType.Double, BizActionObj.PharmacyPercentage);

                    dbServer.AddInParameter(command, "IsFixed", DbType.Boolean, objPackageVO.IsFixedRate);
                    dbServer.AddInParameter(command, "ServiceFixedRate", DbType.Double, objPackageVO.ServiceFixedRate);
                    dbServer.AddInParameter(command, "PharmacyFixedRate", DbType.Double, objPackageVO.PharmacyFixedRate);

                    dbServer.AddInParameter(command, "ServicePercentage", DbType.Double, objPackageVO.ServicePercentage);
                    dbServer.AddInParameter(command, "PharmacyPercentage", DbType.Double, objPackageVO.PharmacyPercentage);
                    // END
                    //dbServer.AddInParameter(command, "FamilyMemberCount", DbType.Int32, objPackageVO.FamilyMemberCount);

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPackageVO.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
                    objPackageVO.ID = (long)dbServer.GetParameterValue(command, "ID");
                }


                if (BizActionObj.Details.ID > 0)
                {
                    DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceForPackage");   //M_PackageServices

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, objPackageVO.ID);
                    dbServer.AddInParameter(command5, "ServiceID", DbType.Int64, objPackageVO.ServiceID);
                    dbServer.AddInParameter(command5, "Validity", DbType.String, objPackageVO.Validity);
                    dbServer.AddInParameter(command5, "PackageAmount", DbType.Double, objPackageVO.PackageAmount);
                    dbServer.AddInParameter(command5, "NoOfFollowUp", DbType.String, objPackageVO.NoOfFollowUp);

                    dbServer.AddInParameter(command5, "ApplicableToAll", DbType.Boolean, objPackageVO.ApplicableToAll);
                    dbServer.AddInParameter(command5, "ApplicableToAllDiscount", DbType.Double, objPackageVO.ApplicableToAllDiscount);

                    dbServer.AddInParameter(command5, "IsFreezed", DbType.Boolean, objPackageVO.IsFreezed);

                    // Added By CDS On 19/01/2017


                    //dbServer.AddInParameter(command5, "IsFixed", DbType.Boolean, BizActionObj.IsFixedRate);
                    //dbServer.AddInParameter(command5, "ServiceFixedRate", DbType.Double, BizActionObj.ServiceFixedRate);
                    //dbServer.AddInParameter(command5, "PharmacyFixedRate", DbType.Double, BizActionObj.PharmacyFixedRate);
                    //dbServer.AddInParameter(command5, "ServicePercentage", DbType.Double, BizActionObj.ServicePercentage);
                    //dbServer.AddInParameter(command5, "PharmacyPercentage", DbType.Double, BizActionObj.PharmacyPercentage);

                    dbServer.AddInParameter(command5, "IsFixed", DbType.Boolean, objPackageVO.IsFixedRate);
                    dbServer.AddInParameter(command5, "ServiceFixedRate", DbType.Double, objPackageVO.ServiceFixedRate);
                    dbServer.AddInParameter(command5, "PharmacyFixedRate", DbType.Double, objPackageVO.PharmacyFixedRate);

                    dbServer.AddInParameter(command5, "ServicePercentage", DbType.Double, objPackageVO.ServicePercentage);
                    dbServer.AddInParameter(command5, "PharmacyPercentage", DbType.Double, objPackageVO.PharmacyPercentage);
                    // END

                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "Status  ", DbType.Boolean, true);

                    dbServer.AddInParameter(command5, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command5, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command5, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command5, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command5, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                }

                decimal ConcessionAmt = 0;
                long ServiceId = 0;
                int Cnt1 = 0;

                if (objPackageVO.PackageDetails != null && objPackageVO.PackageDetails.Count != 0)
                {
                    foreach (var item in objPackageVO.PackageDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPackageServiceDetailsForPackage");  //M_PackageServiceDetails

                        dbServer.AddInParameter(command1, "PackageID", DbType.Int64, objPackageVO.ID);
                        dbServer.AddInParameter(command1, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "SpecilizationID", DbType.Int64, item.DepartmentID);  //DepartmentID
                        dbServer.AddInParameter(command1, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);

                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, item.ServiceID);
                        //dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);

                        dbServer.AddInParameter(command1, "Amount", DbType.Double, item.Amount);
                        dbServer.AddInParameter(command1, "Discount", DbType.Double, item.Discount);

                        dbServer.AddInParameter(command1, "IsDiscountOnQuantity", DbType.Boolean, item.IsDiscountOnQuantity);
                        dbServer.AddInParameter(command1, "AgeLimit", DbType.Int64, item.AgeLimit);
                        dbServer.AddInParameter(command1, "IsFollowupNotRequired", DbType.Boolean, item.IsFollowupNotRequired);

                        dbServer.AddInParameter(command1, "ApplicableTo", DbType.Int64, item.SelectedGender.ID);

                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
                        dbServer.AddInParameter(command1, "Infinite", DbType.Boolean, item.Infinite);

                        //dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
                        //dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
                        //dbServer.AddInParameter(command1, "FreeAtFollowUp", DbType.Boolean, item.FreeAtFollowUp);
                        dbServer.AddInParameter(command1, "IsActive", DbType.Boolean, item.IsActive);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                        dbServer.AddInParameter(command1, "Month", DbType.String, item.Month);
                        dbServer.AddInParameter(command1, "MonthStatus", DbType.Boolean, item.MonthStatus);

                        //dbServer.AddInParameter(command1, "One", DbType.Boolean, item.One);
                        //dbServer.AddInParameter(command1, "Two", DbType.Boolean, item.Two);
                        //dbServer.AddInParameter(command1, "Three", DbType.Boolean, item.Three);
                        //dbServer.AddInParameter(command1, "Four", DbType.Boolean, item.Four);
                        //dbServer.AddInParameter(command1, "Five", DbType.Boolean, item.Five);
                        //dbServer.AddInParameter(command1, "Six", DbType.Boolean, item.Six);
                        //dbServer.AddInParameter(command1, "Seven", DbType.Boolean, item.Seven);
                        //dbServer.AddInParameter(command1, "Eight", DbType.Boolean, item.Eight);
                        //dbServer.AddInParameter(command1, "Nine", DbType.Boolean, item.Nine);
                        //dbServer.AddInParameter(command1, "Ten", DbType.Boolean, item.Ten);
                        //dbServer.AddInParameter(command1, "Eleven", DbType.Boolean, item.Eleven);
                        //dbServer.AddInParameter(command1, "Twelve", DbType.Boolean, item.Twelve);


                        
                        // Added By CDS On 19/01/2017
                        dbServer.AddInParameter(command1, "AdjustableHead", DbType.Boolean, item.AdjustableHead);

                        dbServer.AddInParameter(command1, "IsFixed", DbType.Boolean, item.IsFixed);
                        dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);
                        //dbServer.AddInParameter(command1, "FixedRate", DbType.Double, item.Rate);

                        dbServer.AddInParameter(command1, "Percentage", DbType.Double, item.RatePercentage);
                        dbServer.AddInParameter(command1, "IsDoctorSharePercentage", DbType.Boolean, item.IsDoctorSharePercentage);

                        dbServer.AddInParameter(command1, "ConsiderAdjustable", DbType.Boolean, item.ConsiderAdjustable);
                        // END

                        dbServer.AddInParameter(command1, "ProcessID", DbType.Int64, item.SelectedProcess.ID);           // Package New Changes for Procedure Added on 16042018
                        dbServer.AddInParameter(command1, "AdjustableHeadType", DbType.Int32, item.AdjustableHeadType);  // Package New Changes for Procedure Added on 16042018  // 1 = Clinical , 2 = Pharmacy

                        dbServer.AddInParameter(command1, "IsConsumables", DbType.Boolean, item.IsConsumables);          // Package New Changes Added on 25042018 for Procedure

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");


                        if (ServiceId != item.ServiceID)
                        {
                            ServiceId = item.ServiceID;
                            Cnt1 = 0;
                        }

                        if (ServiceId == item.ServiceID && Cnt1 == 0)
                        {

                            #region Commented By CDS TO RECORDS ARE NOT ENTERED INTO THSES TWO TABLES T_TariffServiceMaster,T_TariffServiceClassRateDetail In CASE OF PACKAGE CREATION
                            ////-------------------Begin Of Add Package Specialization & Services to T_TariffServiceMaster,T_TariffServiceClassRateDetail --------------------------------
                            //DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddTariffServieLinkForPackageNew");   // T_TariffServiceMaster And  T_TariffServiceClassRateDetail


                            //if (item.IsSpecilizationGroup == false)
                            //{
                            //    dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, item.ServiceID);
                            //}
                            //else if (item.IsSpecilizationGroup == true)
                            //{
                            //    dbServer.AddInParameter(command3, "Specialization", DbType.Int64, item.DepartmentID);                                
                            //}

                            //dbServer.AddInParameter(command3, "PackageID", DbType.Int64, BizActionObj.Details.ID);

                            //ConcessionAmt = (Convert.ToDecimal(item.Discount) * Convert.ToDecimal(item.Rate)) / 100;

                            //dbServer.AddInParameter(command3, "Concession", DbType.Boolean, true);
                            //dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Decimal, ConcessionAmt);
                            //dbServer.AddInParameter(command3, "ConcessionPercent", DbType.Decimal, Convert.ToDecimal(item.Discount));                          

                            //dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                            //dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            //dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);                            

                            //dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0);
                            //dbServer.AddOutParameter(command3, "Id", DbType.Int64, 0);

                            //int intTariffStatus = dbServer.ExecuteNonQuery(command3, trans);


                            //-------------------End Of Add Package Specialization & Services to T_TariffServiceMaster,T_TariffServiceClassRateDetail --------------------------------
                            #endregion


                            ////-------------------Begin Of Add Package Service Relation Detils For M_PackageServiceRelations --------------------------------

                            if (objPackageVO.PackageServiceRelationDetails != null && objPackageVO.PackageServiceRelationDetails.Count != 0)
                            {
                                foreach (clsPackageServiceRelationsVO item2 in objPackageVO.PackageServiceRelationDetails)
                                {
                                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_AddPackageServiceRelationForPackage");

                                    dbServer.AddInParameter(command4, "RelationID", DbType.Int64, item2.RelationID);
                                    dbServer.AddInParameter(command4, "PackageID", DbType.Int64, BizActionObj.Details.ID);  //item2.PackageID
                                    dbServer.AddInParameter(command4, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command4, "ServiceID", DbType.Int64, item2.ServiceID);
                                    dbServer.AddInParameter(command4, "SpecilizationID", DbType.Int64, item.DepartmentID);  //item2.SpecilizationID
                                    dbServer.AddInParameter(command4, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);  //item.IsSpecilizationGroup

                                    dbServer.AddInParameter(command4, "IsSetAllRelations", DbType.Boolean, item2.IsSetAllRelations);

                                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command4, "Status", DbType.Boolean, item.Status);
                                    //dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    //dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                                    //dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    //dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    //dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    dbServer.AddInParameter(command4, "ProcessID", DbType.Int64, item2.ProcessID);      // Package New Changes Added on 25042018 for Procedure

                                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, 0);

                                    int iStatus = dbServer.ExecuteNonQuery(command4, trans);
                                    item2.ID = (long)dbServer.GetParameterValue(command4, "ID");

                                }
                            }

                            ////-------------------End Of Add Package Service Relation Detils For M_PackageServiceRelations --------------------------------

                            ////-------------------Begin Of Add Package Services Conditions in M_PackageServiceConditions --------------------------------


                            if (objPackageVO.ServiceConditionDetails != null && objPackageVO.ServiceConditionDetails.Count != 0)
                            {
                                foreach (clsPackageServiceConditionsVO item3 in objPackageVO.ServiceConditionDetails)
                                {
                                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddPackageServiceConditionForPackage");


                                    dbServer.AddInParameter(command2, "PackageID", DbType.Int64, BizActionObj.Details.ID);  //item3.PackageID
                                    dbServer.AddInParameter(command2, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command2, "PackageServiceId", DbType.Int64, item.ServiceID); //item3.PackageServiceDetailID
                                    dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, item3.SelectedService.ID);  //item3.ServiceID
                                    dbServer.AddInParameter(command2, "Rate", DbType.Double, item3.Rate);
                                    dbServer.AddInParameter(command2, "Quantity", DbType.Double, item3.Quantity);
                                    dbServer.AddInParameter(command2, "Discount", DbType.Double, item3.Discount);
                                    dbServer.AddInParameter(command2, "ConditionType", DbType.String, item3.SelectedCondition.Description);  //item3.ConditionType
                                    dbServer.AddInParameter(command2, "ConditionTypeID", DbType.String, item3.SelectedCondition.ID);  //item3.ConditionTypeID
                                    dbServer.AddInParameter(command2, "SpecilizationID", DbType.Int64, item.DepartmentID);  //item3.SpecilizationID
                                    dbServer.AddInParameter(command2, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);  //item3.IsSpecilizationGroup

                                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, item3.Status);
                                    //dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    //dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                                    //dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    //dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    //dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0);

                                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                                    item3.ID = (long)dbServer.GetParameterValue(command2, "ID");

                                }
                            }

                            ////------------ End of Add Package Services Conditions in M_PackageServiceConditions -------------------



                            Cnt1++;

                        }
                    }

                }

                long ServiceId2 = 0;
                int Cnt2 = 0;

                ////------------ Begin of Add Patient Package Details after Package Freezed for particular Service for already Registered Patient  -------------------

                if (BizActionObj.IsSavePatientData == true && objPackageVO.PackageDetails != null && objPackageVO.PackageDetails.Count != 0)
                {
                    foreach (var item in objPackageVO.PackageDetails)
                    {

                        if (ServiceId2 != item.ServiceID)
                        {
                            ServiceId2 = item.ServiceID;
                            Cnt2 = 0;
                        }

                        if (ServiceId2 == item.ServiceID && Cnt2 == 0)
                        {
                            if (BizActionObj.IsSavePatientData == true)
                            {
                                DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_AddPatientPackageDetailsForPackageAllAddAfterFreeze2"); //CIMS_AddPatientPackageDetailsForPackageAllAddAfterFreeze

                                dbServer.AddInParameter(command7, "PackageID", DbType.Int64, objPackageVO.ID);
                                dbServer.AddInParameter(command7, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command7, "ServiceID", DbType.Int64, item.ServiceID);
                                dbServer.AddInParameter(command7, "SpecilizationID", DbType.Int64, item.DepartmentID);  //DepartmentID
                                dbServer.AddInParameter(command7, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);
                                dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                                //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                //dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatusPatient = dbServer.ExecuteNonQuery(command7, trans);
                                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                                //item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                            }

                            if (BizActionObj.IsSavePatientData == true && item.IsSpecilizationGroup == false)
                            {
                                DbCommand command8 = dbServer.GetStoredProcCommand("CIMS_AddPatientFollowUpHealthSpringForPackageAllAddAfterFreeze"); //  //CIMS_AddPackageServiceDetails

                                dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command8, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                //dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddInParameter(command8, "PackageID", DbType.Int64, objPackageVO.ID);
                                dbServer.AddInParameter(command8, "PackageUnitID", DbType.Int64, objPackageVO.UnitID);

                                dbServer.AddInParameter(command8, "ServiceID", DbType.Int64, item.ServiceID);
                                dbServer.AddInParameter(command8, "SpecilizationID", DbType.Int64, item.DepartmentID);
                                dbServer.AddInParameter(command8, "IsSpecilizationGroup", DbType.Int64, item.IsSpecilizationGroup);

                                dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatusFollow = dbServer.ExecuteNonQuery(command8, trans);
                                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                                //item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                            }

                            Cnt2++;
                        }

                    }
                }

                ////------------ Begin of Add Patient Package Details after Package Freezed for particular Service for already Registered Patient  -------------------


                //if (objPackageVO.ItemDetails != null && objPackageVO.ItemDetails.Count != 0)
                //{
                //    foreach (var item in objPackageVO.ItemDetails)
                //    {
                //        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddPackageItemDetails");

                //        dbServer.AddInParameter(command5, "PackageID", DbType.Int64, BizActionObj.Details.ID);

                //        dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item.ItemID);
                //        dbServer.AddInParameter(command5, "Discount", DbType.Double, item.Discount);

                //        dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command5, "Status", DbType.Boolean, item.Status);
                //        dbServer.AddInParameter(command5, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                //        dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //        dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                //        dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //        dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                //        dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, 0);

                //        int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                //        item.ID = (long)dbServer.GetParameterValue(command5, "ID");

                //    }
                //}




                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;

        }

        private clsAddPackageServiceNewBizActionVO UpdatePackage(clsAddPackageServiceNewBizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsPackageServiceVO objPackageVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceForPackage");    // M_PackageServices

                dbServer.AddInParameter(command, "ID", DbType.Int64, objPackageVO.ID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objPackageVO.ServiceID);
                dbServer.AddInParameter(command, "Validity", DbType.String, objPackageVO.Validity);
                dbServer.AddInParameter(command, "PackageAmount", DbType.Double, objPackageVO.PackageAmount);
                dbServer.AddInParameter(command, "NoOfFollowUp", DbType.String, objPackageVO.NoOfFollowUp);

                dbServer.AddInParameter(command, "ApplicableToAll", DbType.Boolean, objPackageVO.ApplicableToAll);
                dbServer.AddInParameter(command, "ApplicableToAllDiscount", DbType.Double, objPackageVO.ApplicableToAllDiscount);

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objPackageVO.IsFreezed);
                // Added By CDS On 19/01/2017
                //dbServer.AddInParameter(command, "IsFixed", DbType.Boolean, BizActionObj.IsFixedRate);
                //dbServer.AddInParameter(command, "ServiceFixedRate", DbType.Double, BizActionObj.ServiceFixedRate);
                //dbServer.AddInParameter(command, "PharmacyFixedRate", DbType.Double, BizActionObj.PharmacyFixedRate);
                //dbServer.AddInParameter(command, "ServicePercentage", DbType.Double, BizActionObj.ServicePercentage);
                //dbServer.AddInParameter(command, "PharmacyPercentage", DbType.Double, BizActionObj.PharmacyPercentage);


                dbServer.AddInParameter(command, "IsFixed", DbType.Boolean, objPackageVO.IsFixedRate);
                dbServer.AddInParameter(command, "ServiceFixedRate", DbType.Double, objPackageVO.ServiceFixedRate);
                dbServer.AddInParameter(command, "PharmacyFixedRate", DbType.Double, objPackageVO.PharmacyFixedRate);
                dbServer.AddInParameter(command, "ServicePercentage", DbType.Double, objPackageVO.ServicePercentage);
                dbServer.AddInParameter(command, "PharmacyPercentage", DbType.Double, objPackageVO.PharmacyPercentage);
                // END

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");



                decimal ConcessionAmt = 0;
                long ServiceId = 0;
                int Cnt1 = 0;

                if (objPackageVO.PackageDetails != null && objPackageVO.PackageDetails.Count != 0)
                {
                    foreach (var item in objPackageVO.PackageDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceDetailsForPackage");  // M_PackageServiceDetails

                        dbServer.AddInParameter(command1, "PackageID", DbType.Int64, objPackageVO.ID);
                        dbServer.AddInParameter(command1, "PackageUnitID", DbType.Int64, objPackageVO.UnitID);
                        dbServer.AddInParameter(command1, "SpecilizationID", DbType.Int64, item.DepartmentID);
                        dbServer.AddInParameter(command1, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);

                        dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, item.ServiceID);
                        //dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);

                        dbServer.AddInParameter(command1, "Amount", DbType.Double, item.Amount);
                        dbServer.AddInParameter(command1, "Discount", DbType.Double, item.Discount);

                        dbServer.AddInParameter(command1, "IsDiscountOnQuantity", DbType.Boolean, item.IsDiscountOnQuantity);
                        dbServer.AddInParameter(command1, "AgeLimit", DbType.Int64, item.AgeLimit);
                        dbServer.AddInParameter(command1, "IsFollowupNotRequired", DbType.Boolean, item.IsFollowupNotRequired);

                        dbServer.AddInParameter(command1, "ApplicableTo", DbType.Int64, item.SelectedGender.ID);

                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
                        dbServer.AddInParameter(command1, "Infinite", DbType.Boolean, item.Infinite);

                        //dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
                        //dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
                        //dbServer.AddInParameter(command1, "FreeAtFollowUp", DbType.Boolean, item.FreeAtFollowUp);
                        dbServer.AddInParameter(command1, "IsActive", DbType.Boolean, item.IsActive);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                        dbServer.AddInParameter(command1, "Month", DbType.String, item.Month);
                        dbServer.AddInParameter(command1, "MonthStatus", DbType.Boolean, item.MonthStatus);

                        //dbServer.AddInParameter(command1, "One", DbType.Boolean, item.One);
                        //dbServer.AddInParameter(command1, "Two", DbType.Boolean, item.Two);
                        //dbServer.AddInParameter(command1, "Three", DbType.Boolean, item.Three);
                        //dbServer.AddInParameter(command1, "Four", DbType.Boolean, item.Four);
                        //dbServer.AddInParameter(command1, "Five", DbType.Boolean, item.Five);
                        //dbServer.AddInParameter(command1, "Six", DbType.Boolean, item.Six);
                        //dbServer.AddInParameter(command1, "Seven", DbType.Boolean, item.Seven);
                        //dbServer.AddInParameter(command1, "Eight", DbType.Boolean, item.Eight);
                        //dbServer.AddInParameter(command1, "Nine", DbType.Boolean, item.Nine);
                        //dbServer.AddInParameter(command1, "Ten", DbType.Boolean, item.Ten);
                        //dbServer.AddInParameter(command1, "Eleven", DbType.Boolean, item.Eleven);
                        //dbServer.AddInParameter(command1, "Twelve", DbType.Boolean, item.Twelve);

                        // Added By CDS On 19/01/2017
                        dbServer.AddInParameter(command1, "AdjustableHead", DbType.Boolean, item.AdjustableHead);

                        dbServer.AddInParameter(command1, "IsFixed", DbType.Boolean, item.IsFixed);
                        dbServer.AddInParameter(command1, "Rate", DbType.Double, item.Rate);
                        //dbServer.AddInParameter(command1, "FixedRate", DbType.Double, item.Rate);
                        dbServer.AddInParameter(command1, "Percentage", DbType.Double, item.RatePercentage);
                        dbServer.AddInParameter(command1, "IsDoctorSharePercentage", DbType.Boolean, item.IsDoctorSharePercentage);

                        dbServer.AddInParameter(command1, "ConsiderAdjustable", DbType.Boolean, item.ConsiderAdjustable);
                        // END

                        dbServer.AddInParameter(command1, "ProcessID", DbType.Int64, item.SelectedProcess.ID);           // Package New Changes for Procedure Added on 16042018
                        dbServer.AddInParameter(command1, "AdjustableHeadType", DbType.Int32, item.AdjustableHeadType);  // Package New Changes for Procedure Added on 18042018  // 1 = Clinical , 2 = Pharmacy

                        dbServer.AddInParameter(command1, "IsConsumables", DbType.Boolean, item.IsConsumables);          // Package New Changes Added on 25042018 for Procedure

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");



                        if (ServiceId != item.ServiceID)
                        {
                            ServiceId = item.ServiceID;
                            Cnt1 = 0;
                        }

                        if (ServiceId == item.ServiceID && Cnt1 == 0)
                        {
                            #region Commented By CDS TO RECORDS ARE NOT ENTERED INTO THSES TWO TABLES T_TariffServiceMaster,T_TariffServiceClassRateDetail In CASE OF PACKAGE CREATION
                            ////-------------------Begin Of Add Package Specialization & Services to T_TariffServiceMaster,T_TariffServiceClassRateDetail --------------------------------
                            //DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateTariffServieLinkForPackage");  // T_TariffServiceMaster,T_TariffServiceClassRateDetail


                            //if (item.IsSpecilizationGroup == false)
                            //{
                            //    dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, item.ServiceID);
                            //}
                            //else if (item.IsSpecilizationGroup == true)
                            //{
                            //    dbServer.AddInParameter(command3, "Specialization", DbType.Int64, item.DepartmentID);
                            //}


                            //dbServer.AddInParameter(command3, "PackageID", DbType.Int64, BizActionObj.Details.ID);

                            //ConcessionAmt = (Convert.ToDecimal(item.Discount) * Convert.ToDecimal(item.Rate)) / 100;

                            //dbServer.AddInParameter(command3, "Concession", DbType.Boolean, true);
                            //dbServer.AddInParameter(command3, "ConcessionAmount", DbType.Decimal, ConcessionAmt);
                            //dbServer.AddInParameter(command3, "ConcessionPercent", DbType.Decimal, Convert.ToDecimal(item.Discount));

                            //dbServer.AddInParameter(command3, "Rate", DbType.Double, item.Rate);

                            //dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                            //dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            //dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                            //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objPackageVO.ID);
                            //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                            //dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0);
                            //dbServer.AddOutParameter(command3, "Id", DbType.Int64, 0);


                            //int intTariffStatus = dbServer.ExecuteNonQuery(command3, trans);


                            ////-------------------End Of Add Package Specialization & Services to T_tariffservicemaster,T_tariffserviceClassratedetail --------------------------------
                            #endregion

                            ////-------------------Begin Of Delete Package Service Relation Detils For M_PackageServiceRelations --------------------------------

                            if (objPackageVO.PackageServiceRelationDetailsDelete != null && objPackageVO.PackageServiceRelationDetailsDelete.Count != 0)
                            {
                                foreach (clsPackageServiceRelationsVO item2 in objPackageVO.PackageServiceRelationDetailsDelete)
                                {
                                    DbCommand command44 = dbServer.GetStoredProcCommand("CIMS_DeletePackageServiceRelationForPackage");

                                    dbServer.AddInParameter(command44, "RelationID", DbType.Int64, item2.RelationID);
                                    dbServer.AddInParameter(command44, "PackageID", DbType.Int64, BizActionObj.Details.ID);  //item2.PackageID
                                    dbServer.AddInParameter(command44, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command44, "ServiceID", DbType.Int64, item2.ServiceID);
                                    dbServer.AddInParameter(command44, "SpecilizationID", DbType.Int64, item.DepartmentID);  //item2.SpecilizationID
                                    dbServer.AddInParameter(command44, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);  //item.IsSpecilizationGroup

                                    dbServer.AddInParameter(command44, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command44, "Status", DbType.Boolean, item.Status);

                                    dbServer.AddInParameter(command44, "ID", DbType.Int64, item2.ID);

                                    int iStatus = dbServer.ExecuteNonQuery(command44, trans);

                                }
                            }

                            ////-------------------End Of Delete Package Service Relation Detils For M_PackageServiceRelations --------------------------------


                            ////-------------------Begin Of Update Package Service Relation Detils For M_PackageServiceRelations --------------------------------

                            if (objPackageVO.PackageServiceRelationDetails != null && objPackageVO.PackageServiceRelationDetails.Count != 0)
                            {
                                foreach (clsPackageServiceRelationsVO item2 in objPackageVO.PackageServiceRelationDetails)
                                {
                                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceRelationForPackage");   //CIMS_AddPackageServiceRelationForPackage

                                    dbServer.AddInParameter(command4, "RelationID", DbType.Int64, item2.RelationID);
                                    dbServer.AddInParameter(command4, "PackageID", DbType.Int64, BizActionObj.Details.ID);  //item2.PackageID
                                    dbServer.AddInParameter(command4, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command4, "ServiceID", DbType.Int64, item2.ServiceID);
                                    dbServer.AddInParameter(command4, "SpecilizationID", DbType.Int64, item.DepartmentID);  //item2.SpecilizationID
                                    dbServer.AddInParameter(command4, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);  //item.IsSpecilizationGroup

                                    dbServer.AddInParameter(command4, "IsSetAllRelations", DbType.Boolean, item2.IsSetAllRelations);

                                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command4, "Status", DbType.Boolean, item.Status);
                                    //dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    //dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                                    //dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    //dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    //dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    dbServer.AddInParameter(command4, "ProcessID", DbType.Int64, item2.ProcessID);      // Package New Changes Added on 25042018 for Procedure

                                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, 0);

                                    int iStatus = dbServer.ExecuteNonQuery(command4, trans);
                                    item2.ID = (long)dbServer.GetParameterValue(command4, "ID");

                                }
                            }

                            ////-------------------End Of Update Package Service Relation Detils For M_PackageServiceRelations --------------------------------

                            ////-------------------Begin Of Delete Package Services Conditions in M_PackageServiceConditions --------------------------------

                            if (objPackageVO.ServiceConditionDetailsDelete != null && objPackageVO.ServiceConditionDetailsDelete.Count != 0)
                            {
                                foreach (clsPackageServiceConditionsVO item3 in objPackageVO.ServiceConditionDetailsDelete)
                                {
                                    DbCommand command22 = dbServer.GetStoredProcCommand("CIMS_DeletePackageServiceConditionForPackage");


                                    dbServer.AddInParameter(command22, "PackageID", DbType.Int64, BizActionObj.Details.ID);  // item3.PackageID
                                    dbServer.AddInParameter(command22, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command22, "PackageServiceId", DbType.Int64, item.ServiceID); //item3.PackageServiceDetailID
                                    dbServer.AddInParameter(command22, "ServiceID", DbType.Int64, item3.SelectedService.ID);  //item3.ServiceID
                                    dbServer.AddInParameter(command22, "SpecilizationID", DbType.Int64, item.DepartmentID);  //item3.SpecilizationID
                                    dbServer.AddInParameter(command22, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);  //item3.IsSpecilizationGroup

                                    dbServer.AddInParameter(command22, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command22, "Status", DbType.Boolean, item3.Status);

                                    dbServer.AddInParameter(command22, "ID", DbType.Int64, item3.ID);

                                    int iStatus = dbServer.ExecuteNonQuery(command22, trans);

                                }
                            }

                            ////-------------------End Of Delete Package Services Conditions in M_PackageServiceConditions --------------------------------


                            ////-------------------Begin Of Update Package Services Conditions in M_PackageServiceConditions --------------------------------


                            if (objPackageVO.ServiceConditionDetails != null && objPackageVO.ServiceConditionDetails.Count != 0)
                            {
                                foreach (clsPackageServiceConditionsVO item3 in objPackageVO.ServiceConditionDetails)
                                {
                                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdatePackageServiceConditionForPackage");  //CIMS_AddPackageServiceConditionForPackage


                                    dbServer.AddInParameter(command2, "PackageID", DbType.Int64, BizActionObj.Details.ID);  // item3.PackageID
                                    dbServer.AddInParameter(command2, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command2, "PackageServiceId", DbType.Int64, item.ServiceID); //item3.PackageServiceDetailID
                                    dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, item3.SelectedService.ID);  //item3.ServiceID
                                    dbServer.AddInParameter(command2, "Rate", DbType.Double, item3.Rate);
                                    dbServer.AddInParameter(command2, "Quantity", DbType.Double, item3.Quantity);
                                    dbServer.AddInParameter(command2, "Discount", DbType.Double, item3.Discount);
                                    dbServer.AddInParameter(command2, "ConditionType", DbType.String, item3.SelectedCondition.Description);  //item3.ConditionType
                                    dbServer.AddInParameter(command2, "ConditionTypeID", DbType.String, item3.SelectedCondition.ID);  //item3.ConditionTypeID
                                    dbServer.AddInParameter(command2, "SpecilizationID", DbType.Int64, item.DepartmentID);  //item3.SpecilizationID
                                    dbServer.AddInParameter(command2, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);  //item3.IsSpecilizationGroup

                                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, item3.Status);
                                    //dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    //dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                                    //dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    //dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    //dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0);

                                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                                    item3.ID = (long)dbServer.GetParameterValue(command2, "ID");

                                }
                            }

                            ////------------ End of Update Package Services Conditions in M_PackageServiceConditions -------------------

                            Cnt1++;
                        }

                    }
                }


                long ServiceId2 = 0;
                int Cnt2 = 0;

                ////------------ Begin of Add Patient Package Details after Package Freezed for particular Service for already Registered Patient  -------------------

                if (BizActionObj.IsSavePatientData == true && objPackageVO.PackageDetails != null && objPackageVO.PackageDetails.Count != 0)
                {
                    foreach (var item in objPackageVO.PackageDetails)
                    {

                        if (ServiceId2 != item.ServiceID)
                        {
                            ServiceId2 = item.ServiceID;
                            Cnt2 = 0;
                        }

                        if (ServiceId2 == item.ServiceID && Cnt2 == 0)
                        {
                            if (BizActionObj.IsSavePatientData == true)
                            {
                                DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_UpdatePatientPackageDetailsForPackageAllAddAfterFreeze2");  //CIMS_UpdatePatientPackageDetailsForPackageAllAddAfterFreeze

                                dbServer.AddInParameter(command7, "PackageID", DbType.Int64, objPackageVO.ID);
                                dbServer.AddInParameter(command7, "PackageUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command7, "ServiceID", DbType.Int64, item.ServiceID);
                                dbServer.AddInParameter(command7, "SpecilizationID", DbType.Int64, item.DepartmentID);  //DepartmentID
                                dbServer.AddInParameter(command7, "IsSpecilizationGroup", DbType.Boolean, item.IsSpecilizationGroup);
                                dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                                //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                                //dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatusPatient = dbServer.ExecuteNonQuery(command7, trans);
                                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                                //item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                            }

                            Cnt2++;
                        }

                    }
                }

                ////------------ Begin of Add Patient Package Details after Package Freezed for particular Service for already Registered Patient  -------------------


                //if (objPackageVO.ItemDetails != null)  // && objPackageVO.ItemDetails.Count != 0)
                //{
                //    DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_DeletePackageItemDetails");
                //    dbServer.AddInParameter(command7, "PackageID", DbType.Int64, objPackageVO.ID);
                //    dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    int intStatus3 = dbServer.ExecuteNonQuery(command7, trans);
                //}

                //if (objPackageVO.ItemDetails != null && objPackageVO.ItemDetails.Count != 0)
                //{
                //    foreach (var item in objPackageVO.ItemDetails)
                //    {
                //        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddPackageItemDetails");

                //        dbServer.AddInParameter(command5, "PackageID", DbType.Int64, BizActionObj.Details.ID);

                //        dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item.ItemID);
                //        dbServer.AddInParameter(command5, "Discount", DbType.Double, item.Discount);

                //        dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command5, "Status", DbType.Boolean, item.Status);
                //        dbServer.AddInParameter(command5, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                //        dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //        dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                //        dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //        dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                //        dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, 0);

                //        int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                //        item.ID = (long)dbServer.GetParameterValue(command5, "ID");

                //    }
                //}

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;

        }

        public override IValueObject GetPackageServiceDetailListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServiceDetailsListNewBizActionVO BizActionObj = valueObject as clsGetPackageServiceDetailsListNewBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageServiceDetailsForPackage");  //CIMS_GetPackageServiceDetails
                DbDataReader reader;


                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageMasterList == null)
                        BizActionObj.PackageMasterList = new clsPackageServiceVO();

                    while (reader.Read())
                    {
                        clsPackageServiceVO PackageVO = new clsPackageServiceVO();
                        PackageVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        PackageVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        PackageVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        PackageVO.Service = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        PackageVO.Validity = (string)DALHelper.HandleDBNull(reader["Validity"]);
                        PackageVO.ValidityUnit = (long)DALHelper.HandleIntegerNull(reader["ValidityUnit"]);
                        PackageVO.PackageAmount = (double)DALHelper.HandleDBNull(reader["PackageAmount"]);
                        PackageVO.NoOfFollowUp = (string)DALHelper.HandleDBNull(reader["NoOfFollowUp"]);
                        PackageVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        PackageVO.ApplicableToAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ApplicableToAll"]));
                        PackageVO.ApplicableToAllDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ApplicableToAllDiscount"]));

                        //By Anjali...........................
                        PackageVO.TotalBudget = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBudget"]));
                        //......................................

                        PackageVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));

                        PackageVO.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));

                        //,IsFixed,ServiceFixedRate,PharmacyFixedRate,ServicePercentage,PharmacyPercentage

                        PackageVO.IsFixedRate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFixed"]));
                        PackageVO.ServiceFixedRate = (double)DALHelper.HandleDBNull(reader["ServiceFixedRate"]);
                        PackageVO.PharmacyFixedRate = (double)DALHelper.HandleDBNull(reader["PharmacyFixedRate"]);
                        PackageVO.ServicePercentage = (double)DALHelper.HandleDBNull(reader["ServicePercentage"]);
                        PackageVO.PharmacyPercentage = (double)DALHelper.HandleDBNull(reader["PharmacyPercentage"]);
                    
                        BizActionObj.PackageMasterList = PackageVO;
                    }
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageDetailList == null)
                        BizActionObj.PackageDetailList = new List<clsPackageServiceDetailsVO>();

                    while (reader.Read())
                    {
                        clsPackageServiceDetailsVO PackageVO = new clsPackageServiceDetailsVO();
                        PackageVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        PackageVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        PackageVO.PackageID = (long)DALHelper.HandleDBNull(reader["PackageID"]);
                        PackageVO.DepartmentID = (long)DALHelper.HandleDBNull(reader["DepartmentID"]);
                        PackageVO.Department = (string)DALHelper.HandleDBNull(reader["Department"]);
                        PackageVO.IsSpecilizationGroup = (bool)DALHelper.HandleBoolDBNull(reader["IsSpecilizationGroup"]);

                        PackageVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        PackageVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        if (PackageVO.IsSpecilizationGroup == true)
                        {
                            PackageVO.ServiceID = PackageVO.DepartmentID;
                            PackageVO.ServiceName = PackageVO.Department;

                        }

                        PackageVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        PackageVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);

                        PackageVO.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                        PackageVO.Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));

                        PackageVO.IsDiscountOnQuantity = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDiscountOnQuantity"]));

                        PackageVO.ApplicableTo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableTo"]));
                        //PackageVO.SelectedGender.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableTo"]));

                        PackageVO.AgeLimit = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeLimit"]));
                        PackageVO.IsFollowupNotRequired = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFollowupNotRequired"]));

                        PackageVO.Quantity = (double)DALHelper.HandleDBNull(reader["Quantity"]);
                        //PackageVO.ConcessionPercentage = (double)DALHelper.HandleDBNull(reader["ConcessionPercentage"]);
                        //PackageVO.ConcessionAmount = (double)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        PackageVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        //PackageVO.FreeAtFollowUp = (bool)DALHelper.HandleDBNull(reader["FreeAtFollowUp"]);
                        PackageVO.IsActive = (bool)DALHelper.HandleDBNull(reader["IsActive"]);
                        PackageVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        PackageVO.Infinite = (bool)DALHelper.HandleBoolDBNull(reader["Infinite"]);
                        PackageVO.Month = (string)DALHelper.HandleBoolDBNull(reader["Month"]);
                        PackageVO.MonthStatus = (bool)DALHelper.HandleBoolDBNull(reader["MonthStatus"]);

                        PackageVO.Validity = Convert.ToString(DALHelper.HandleBoolDBNull(reader["Validity"]));
                        PackageVO.ValidityUnit = Convert.ToInt64(DALHelper.HandleBoolDBNull(reader["ValidityUnit"]));

                        //BY Anjali/CDS.....................
                        PackageVO.DisplayQuantity = Convert.ToString(DALHelper.HandleDBNull(reader["DisplayQuantity"]));
                        //....................................

                        //
                        PackageVO.AdjustableHead = (bool)DALHelper.HandleDBNull(reader["AdjustableHead"]);
                        PackageVO.IsFixed = (bool)DALHelper.HandleDBNull(reader["IsFixed"]);

                        PackageVO.RatePercentage = (double)DALHelper.HandleDBNull(reader["Percentage"]);
                        PackageVO.IsDoctorSharePercentage = (bool)DALHelper.HandleDBNull(reader["IsDoctorSharePercentage"]);
                        
                        PackageVO.ConsiderAdjustable =Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsiderAdjustable"]));
                        
                        //,AdjustableHead,M_PackageServiceDetails.IsFixed,Percentage,IsDoctorSharePercentage
                        //

                        PackageVO.ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]));                       //Package New Changes for Procedure Added on 16042018
                        PackageVO.AdjustableHeadType = Convert.ToInt32(DALHelper.HandleDBNull(reader["AdjustableHeadType"]));     //Package New Changes for Procedure Added on 18042018 // 1 = Clinical , 2 = Pharmacy

                        PackageVO.IsConsumables = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsumables"]));             //Package New Changes Added on 25042018 for Procedure

                        BizActionObj.PackageDetailList.Add(PackageVO);
                    }
                }

                //reader.NextResult();

                //if (BizActionObj.ItemDetails == null)
                //    BizActionObj.ItemDetails = new List<clsPackageItemMasterVO>();

                //while (reader.Read())
                //{
                //    clsPackageItemMasterVO ObjItem = new clsPackageItemMasterVO();
                //    ObjItem.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                //    ObjItem.PackageID = (long)DALHelper.HandleDBNull(reader["PackageID"]);
                //    ObjItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                //    ObjItem.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                //    ObjItem.Discount = (float)(double)DALHelper.HandleDBNull(reader["Discount"]);
                //    ObjItem.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                //    ObjItem.ItemCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"]));
                //    ObjItem.ItemGroupName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"]));
                //    ObjItem.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                //    BizActionObj.ItemDetails.Add(ObjItem);
                //}


                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;

        }

        public override IValueObject GetPackageServiceRelationsListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageServicesAndRelationsNewBizActionVO BizActionObj = valueObject as clsGetPackageServicesAndRelationsNewBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageServiceRelationsListForPackage");
                DbDataReader reader;


                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceConditionList == null)
                        BizActionObj.ServiceConditionList = new List<clsPackageServiceConditionsVO>();

                    while (reader.Read())
                    {
                        clsPackageServiceConditionsVO PackageVO = new clsPackageServiceConditionsVO();
                        PackageVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        PackageVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        PackageVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        PackageVO.PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"]));

                        PackageVO.PackageServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceID"]));
                        PackageVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        PackageVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        PackageVO.SpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecilizationID"]));
                        PackageVO.IsSpecilizationGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSpecilizationGroup"]));

                        PackageVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        PackageVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        PackageVO.Discount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Discount"]));
                        PackageVO.ConditionTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ConditionTypeID"]));
                        PackageVO.ConditionType = Convert.ToString(DALHelper.HandleDBNull(reader["ConditionType"]));
                        PackageVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.ServiceConditionList.Add(PackageVO);
                    }
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageServiceRelationList == null)
                        BizActionObj.PackageServiceRelationList = new List<clsPackageServiceRelationsVO>();

                    while (reader.Read())
                    {
                        clsPackageServiceRelationsVO PackageVO2 = new clsPackageServiceRelationsVO();
                        PackageVO2.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        PackageVO2.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        PackageVO2.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        PackageVO2.PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"]));

                        PackageVO2.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        PackageVO2.SpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecilizationID"]));
                        PackageVO2.IsSpecilizationGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSpecilizationGroup"]));

                        PackageVO2.IsSetAllRelations = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetAllRelations"]));

                        PackageVO2.RelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RelationID"]));

                        PackageVO2.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.PackageServiceRelationList.Add(PackageVO2);
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

        public override IValueObject AddPackagePharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddPackagePharmacyItemsNewBizActionVO BizActionObj = valueObject as clsAddPackagePharmacyItemsNewBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                List<clsPackageItemMasterVO> ItemDetails = BizActionObj.ItemDetails;

                if (ItemDetails != null && ItemDetails.Count != 0)
                {
                    foreach (var item in ItemDetails)
                    {
                        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddPackageItemDetailsForPackage");  //CIMS_AddPackageItemDetails

                        dbServer.AddInParameter(command5, "PackageID", DbType.Int64, BizActionObj.PackageID);
                        dbServer.AddInParameter(command5, "PackageUnitID", DbType.Int64, BizActionObj.PackageUnitID);

                        dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item.ItemID);
                        dbServer.AddInParameter(command5, "Discount", DbType.Double, item.Discount);
                        dbServer.AddInParameter(command5, "Quantity", DbType.Double, item.Quantity);

                        dbServer.AddInParameter(command5, "ItemCategory", DbType.Int64, item.ItemCategory);
                        dbServer.AddInParameter(command5, "IsCategory", DbType.Int64, item.IsCategory);

                        dbServer.AddInParameter(command5, "ItemGroup", DbType.Int64, item.ItemGroup);
                        dbServer.AddInParameter(command5, "IsGroup", DbType.Int64, item.IsGroup);
                        //By Anjali.........................
                        dbServer.AddInParameter(command5, "Budget", DbType.Double, item.Budget);
                        //.....................................

                        dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command5, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command5, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, 0);

                        int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        item.ID = (long)dbServer.GetParameterValue(command5, "ID");

                    }
                    //By Anjali..................
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdatePackagePharmacyItemsForPackage");
                    dbServer.AddInParameter(command1, "PackageID", DbType.Int64, BizActionObj.PackageID);
                    dbServer.AddInParameter(command1, "PackageUnitID", DbType.Int64, BizActionObj.PackageUnitID);
                    dbServer.AddInParameter(command1, "TotalBudget", DbType.Double, BizActionObj.TotalBudget);
                    int intStatus = dbServer.ExecuteNonQuery(command1, trans);
                    //...........................

                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.ItemDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;

        }

        public override IValueObject GetPackagePharmacyItemListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackagePharmacyItemListNewBizActionVO BizActionObj = valueObject as clsGetPackagePharmacyItemListNewBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackagePharmacyItemDetailsForPackage");  //CIMS_GetPackageServiceDetails
                DbDataReader reader;


                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemDetails == null)
                        BizActionObj.ItemDetails = new List<clsPackageItemMasterVO>();

                    while (reader.Read())
                    {
                        clsPackageItemMasterVO ObjItem = new clsPackageItemMasterVO();
                        ObjItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ObjItem.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));
                        ObjItem.PackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageUnitID"]));
                        ObjItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        ObjItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        ObjItem.Discount = (float)(double)DALHelper.HandleDBNull(reader["Discount"]);

                        //by Anjali..............................
                        ObjItem.Budget = (float)Convert.ToDouble(DALHelper.HandleDBNull(reader["Budget"]));
                        //....................................

                        ObjItem.Quantity = (float)(double)DALHelper.HandleDBNull(reader["Quantity"]);
                        ObjItem.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        ObjItem.ItemCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"]));
                        ObjItem.ItemGroupName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"]));
                        ObjItem.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        ObjItem.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryId"]));
                        ObjItem.IsCategory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCategory"]));

                        ObjItem.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["GroupId"]));
                        ObjItem.IsGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGroup"]));

                        if (ObjItem.IsCategory == true)
                        {
                            ObjItem.ItemID = ObjItem.ItemCategory;
                            ObjItem.ItemName = ObjItem.ItemCategoryName;

                        }

                        if (ObjItem.IsGroup == true)
                        {
                            ObjItem.ItemID = ObjItem.ItemGroup;
                            ObjItem.ItemName = ObjItem.ItemGroupName;

                        }

                        BizActionObj.ItemDetails.Add(ObjItem);
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

        public override IValueObject DeletePackageServicesDetilsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsDeletePackageServiceDetilsListNewBizActionVO BizActionObj = valueObject as clsDeletePackageServiceDetilsListNewBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();


                DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_DeletePackageServiceDetailsForPackage");

                dbServer.AddInParameter(command5, "PackageID", DbType.Int64, BizActionObj.PackageID);
                dbServer.AddInParameter(command5, "PackageUnitID", DbType.Int64, BizActionObj.PackageUnitID);
                dbServer.AddInParameter(command5, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command5, "SpecilizationID", DbType.Int64, BizActionObj.SpecilizationID);
                dbServer.AddInParameter(command5, "IsSpecilizationGroup", DbType.Boolean, BizActionObj.IsSpecilizationGroup);
                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command5, "Status", DbType.Boolean, BizActionObj.Status);

                dbServer.AddInParameter(command5, "IsDeletePatientData", DbType.Boolean, BizActionObj.IsDeletePatientData);

                int iStatus = dbServer.ExecuteNonQuery(command5, trans);



                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                //BizActionObj.DeleteItemDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;

        }

        public override IValueObject AddPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddPackageSourceTariffCompanyRelationsBizActionVO BizActionObj = valueObject as clsAddPackageSourceTariffCompanyRelationsBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsPackageSourceRelationVO ObjSource = BizActionObj.PackageSourceRelation;

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePackageRelation");   //CIMS_AddPatientSourceTariffDetails

                dbServer.AddInParameter(command1, "PatientCatagoryID", DbType.Int64, ObjSource.PatientCategoryL1ID);
                dbServer.AddInParameter(command1, "PatientCategoryL2ID", DbType.Int64, ObjSource.PatientCategoryL2ID);
                dbServer.AddInParameter(command1, "CompanyID", DbType.Int64, ObjSource.CompanyID);
                dbServer.AddInParameter(command1, "PatientCategoryL3ID", DbType.Int64, ObjSource.PatientCategoryL3ID);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjSource.Status);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, ObjSource.UnitID);

                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjSource.ID);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);

                int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                ObjSource.ID = (long)dbServer.GetParameterValue(command1, "ID");
                BizActionObj.ResultSuccessStatus = (long)dbServer.GetParameterValue(command1, "ResultStatus");

                //if (ObjSource.IsSaveForL2 == true)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientSourceTariffDetailsForPackage");   //CIMS_AddPatientSourceTariffDetails

                //    dbServer.AddInParameter(command1, "PatientCatagoryID", DbType.Int64, ObjSource.PatientCategoryL1ID);

                //    dbServer.AddInParameter(command1, "PatientSourceID", DbType.Int64, ObjSource.PatientCategoryL2ID);
                //    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjSource.PatientCategoryL3ID);
                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjSource.Status);
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, ObjSource.UnitID);

                //    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjSource.ID);
                //    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);

                //    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                //    ObjSource.ID = (long)dbServer.GetParameterValue(command1, "ID");
                //    BizActionObj.ResultSuccessStatus = (long)dbServer.GetParameterValue(command1, "ResultStatus");
                //}
                //else
                //{
                //    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCompanyTariffDetailsForPackage");   //CIMS_AddCompanyTariffDetails

                //    dbServer.AddInParameter(command2, "CompanyID", DbType.Int64, ObjSource.CompanyID);
                //    dbServer.AddInParameter(command2, "TariffID", DbType.Int64, ObjSource.PatientCategoryL3ID);
                //    dbServer.AddInParameter(command2, "Status", DbType.Boolean, ObjSource.Status);
                //    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, ObjSource.UnitID);

                //    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjSource.ID);
                //    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);

                //    int iStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                //    ObjSource.ID = (long)dbServer.GetParameterValue(command2, "ID");
                //    BizActionObj.ResultSuccessStatus = (long)dbServer.GetParameterValue(command2, "ResultStatus");
                //}

                trans.Commit();
                //     BizActionObj.ResultSuccessStatus = 0;

            }
            catch (Exception)
            {
                //throw;
                BizActionObj.ResultSuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PackageSourceRelation = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        public override IValueObject UpdatePackageFreezeStatusNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddPackageServiceNewBizActionVO objItem = valueObject as clsAddPackageServiceNewBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                DbCommand command = null;

                clsPackageServiceVO objItemVO = objItem.Details;
                command = dbServer.GetStoredProcCommand("CIMS_UpdatePackageMasterFreezeStatusForPackage");  //CIMS_UpdatePackageMasterStatus

                dbServer.AddInParameter(command, "PackageID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "PackageUnitID", DbType.Int64, objItemVO.UnitID);

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objItemVO.IsFreezed);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                trans.Commit();

                if (intStatus > 0)
                {
                    objItem.SuccessStatus = 1;
                }
                else
                {
                    objItem.SuccessStatus = 0;
                }

            }
            catch (Exception ex)
            {
                //throw;
                objItem.SuccessStatus = -1;
                trans.Rollback();
                objItem.Details = null;
            }
            return objItem;
        }

        public override IValueObject UpdatePackgeApproveStatusNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddPackageServiceNewBizActionVO objItem = valueObject as clsAddPackageServiceNewBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                DbCommand command = null;

                clsPackageServiceVO objItemVO = objItem.Details;
                command = dbServer.GetStoredProcCommand("CIMS_UpdatePackgeApproveStatusForPackage");    //CIMS_UpdatePackgeApproveStatus

                dbServer.AddInParameter(command, "PackageID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "PackageUnitID", DbType.Int64, objItemVO.UnitID);

                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, objItemVO.IsApproved);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                trans.Commit();

                if (intStatus > 0)
                {
                    objItem.SuccessStatus = 1;

                }
                else
                {
                    objItem.SuccessStatus = 0;
                }


            }
            catch (Exception ex)
            {
                //throw;
                objItem.SuccessStatus = -1;
                trans.Rollback();
                objItem.Details = null;
            }
            return objItem;
        }

        public override IValueObject GetPackageConditionalServiceListNew(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageConditionalServicesNewBizActionVO BizActionObj = valueObject as clsGetPackageConditionalServicesNewBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageConditionalServiceListForPackage4");  //CIMS_GetPackageConditionalServiceListForPackage3 // CIMS_GetPackageConditionalServiceListForPackage2 //CIMS_GetPackageConditionalServiceListForPackage
                DbDataReader reader;


                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);

                //Added BY CDS
                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);
                //END
                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);

                dbServer.AddInParameter(command, "PatientDateOfBirth", DbType.DateTime, BizActionObj.PatientDateOfBirth);
                dbServer.AddInParameter(command, "MemberRelationID", DbType.Int64, BizActionObj.MemberRelationID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceConditionList == null)
                        BizActionObj.ServiceConditionList = new List<clsServiceMasterVO>();

                    while (reader.Read())
                    {
                        clsServiceMasterVO PackageVO = new clsServiceMasterVO();

                        PackageVO.ID = Convert.ToInt64(reader["ServiceID"]);
                        PackageVO.TariffServiceMasterID = Convert.ToInt64(reader["ID"]);
                        PackageVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        PackageVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        PackageVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        PackageVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        PackageVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        PackageVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));

                        //if (BizActionObj.PatientSourceType == 2) // Camp
                        //{
                        //objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        //objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        //objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        //}
                        //else if (BizActionObj.PatientSourceType == 1)   //Loyalty
                        //{
                        //    objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        //    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        //    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        //}
                        //else
                        //{

                        PackageVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));


                        PackageVO.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                        PackageVO.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                        PackageVO.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                        PackageVO.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));


                        //if (PackageVO.SeniorCitizen == true && BizActionObj.Age >= objServiceMasterVO.SeniorCitizenAge)
                        //{
                        //    PackageVO.ConcessionAmount = PackageVO.SeniorCitizenConAmount;
                        //    PackageVO.ConcessionPercent = PackageVO.SeniorCitizenConPercent;
                        //}
                        //else
                        //{
                        PackageVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        PackageVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));

                        //}





                        //}

                        PackageVO.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        PackageVO.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        PackageVO.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        PackageVO.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        PackageVO.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        PackageVO.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        PackageVO.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        PackageVO.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        PackageVO.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        PackageVO.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        PackageVO.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        PackageVO.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                        PackageVO.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        PackageVO.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        PackageVO.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        PackageVO.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        PackageVO.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        PackageVO.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        PackageVO.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));
                        PackageVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        PackageVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        PackageVO.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                        PackageVO.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        PackageVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        PackageVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        PackageVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        PackageVO.PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"]));
                        PackageVO.IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"]));


                        //if (BizActionObj.UsePackageSubsql == true)
                        //{
                        //    objServiceMasterVO.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                        //    objServiceMasterVO.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));

                        //    // to set service background color to identify that this service is having Package Conditional Services
                        //    objServiceMasterVO.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageServiceConditionID"]));
                        //}




                        PackageVO.ConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionID"]));
                        PackageVO.ConditionUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionUnitID"]));
                        PackageVO.MainServicePackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainServicePackageID"]));
                        PackageVO.MainServicePackageUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainServicePackageUnitID"]));

                        PackageVO.PackageServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageServiceID"]));
                        PackageVO.ConditionServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionServiceID"]));
                        PackageVO.ConditionServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ConditionServiceName"]));
                        PackageVO.MainServiceSpecilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainServiceSpecilizationID"]));
                        PackageVO.MainSerivceIsSpecilizationGroup = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MainSerivceIsSpecilizationGroup"]));

                        PackageVO.ConditionalRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConditionalRate"]));
                        PackageVO.ConditionalQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConditionalQuantity"]));
                        PackageVO.ConditionalDiscount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConditionalDiscount"]));
                        PackageVO.ConditionTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ConditionTypeID"]));
                        PackageVO.ConditionType = Convert.ToString(DALHelper.HandleDBNull(reader["ConditionType"]));
                        PackageVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        PackageVO.ConditionalUsedQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConditionalUsedQuantity"]));
                        PackageVO.MainServiceUsedQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["MainServiceUsedQuantity"]));

                        PackageVO.ServiceMemberRelationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ServiceMemberRelationID"]));
                        PackageVO.IsAgeApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgeApplicable"]));
                        //Added BY CDS 
                        PackageVO.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                        PackageVO.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));
                        //Added BY CDS 
                        BizActionObj.ServiceConditionList.Add(PackageVO);
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

        public override IValueObject GetPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageSourceTariffCompanyListBizActionVO BizActionObj = valueObject as clsGetPackageSourceTariffCompanyListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageRelationDetails");
                DbDataReader reader;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.tariffID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageLinkingDetails == null)
                        BizActionObj.PackageLinkingDetails = new List<clsPackageSourceRelationVO>();

                    while (reader.Read())
                    {
                        clsPackageSourceRelationVO PackageVO = new clsPackageSourceRelationVO();

                        PackageVO.ID = Convert.ToInt64(reader["ID"]);
                        PackageVO.PatientCategoryL1ID = Convert.ToInt64(reader["PatientCategoryL1"]);
                        PackageVO.PatientCategoryL1 = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategory"]));

                        PackageVO.PatientCategoryL2ID = Convert.ToInt64(reader["PatientCategoryL2"]);
                        PackageVO.PatientCategoryL2 = Convert.ToString(DALHelper.HandleDBNull(reader["PatientSource"]));

                        PackageVO.PatientCategoryL3ID = Convert.ToInt64(reader["PatientCategoryL3"]);
                        PackageVO.PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["Tariff"]));

                        PackageVO.CompanyID = Convert.ToInt64(reader["CompanyId"]);
                        PackageVO.Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                        PackageVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.PackageLinkingDetails.Add(PackageVO);
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

        public override IValueObject AddUpdatePackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddPackageRateClinicWiseBizActionVO BizActionObj = valueObject as clsAddPackageRateClinicWiseBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();


                foreach (var item in BizActionObj.PackageRateClinicWiseList)
                {
                    DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePackageRateClinicWise");

                    //dbServer.AddInParameter(command5, "TariffID", DbType.Int64, BizActionObj.tariffID);
                    dbServer.AddInParameter(command5, "TariffID", DbType.Int64, item.PatientCategoryL3);

                    dbServer.AddInParameter(command5, "PackageID", DbType.Int64, BizActionObj.PackageID);
                    dbServer.AddInParameter(command5, "PackageServiceID", DbType.Int64, BizActionObj.PackageServiceID);

                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, item.UnitID);
                    dbServer.AddInParameter(command5, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command5, "Rate", DbType.Decimal, item.Rate);
                    dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, int.MaxValue);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                    item.ID = (long)dbServer.GetParameterValue(command5, "ID");
                    BizActionObj.ResultSuccessStatus = (long)dbServer.GetParameterValue(command5, "ResultStatus");

                }
                trans.Commit();


            }
            catch (Exception)
            {
                //throw;
                BizActionObj.ResultSuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PackageRateClinicWise = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        public override IValueObject GetPackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPackageRateClinicWiseBizActionVO BizActionObj = valueObject as clsGetPackageRateClinicWiseBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageRateClinicWise");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.PackageID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PackageRateClinicWiseList == null)
                        BizActionObj.PackageRateClinicWiseList = new List<clsPackageRateClinicWiseVO>();

                    while (reader.Read())
                    {
                        clsPackageRateClinicWiseVO PackageVO = new clsPackageRateClinicWiseVO();
                        PackageVO.ID = Convert.ToInt64(reader["ID"]);
                        PackageVO.UnitID = Convert.ToInt64(reader["UnitID"]);
                        PackageVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        PackageVO.TariffName = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));
                        PackageVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        PackageVO.PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["TariffId"]));
                        PackageVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.PackageRateClinicWiseList.Add(PackageVO);
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

        public override IValueObject GetPackageRelationsListForPackageOnly(IValueObject valueObject, clsUserVO objUserVO)
        {
            try
            {

                clsGetPackageRelationListBizActionVO BizActionObj = valueObject as clsGetPackageRelationListBizActionVO;
                BizActionObj.PackageRelationsList = BizActionObj.PackageRelationsList;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageMemberRelationList");
                dbServer.AddInParameter(command, "PackageTariffID", DbType.Int64, BizActionObj.PackageTariffID);
                //dbServer.AddInParameter(command, "PackageServiceUnitID", DbType.Int64, BizActionObj.PackageServiceUnitID);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MasterListItem objRelation = new MasterListItem();

                        objRelation.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RelationID"]));
                        objRelation.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Relation"]));

                        BizActionObj.PackageRelationsList.Add(objRelation);
                    }

                    reader.Close();
                }

                return BizActionObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject DeletePackageItemsDetilsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsDeletePackageItemDetilsListNewBizActionVO BizActionObj = valueObject as clsDeletePackageItemDetilsListNewBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();


                DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_DeletePackageItemDetailsForPackage");

                dbServer.AddInParameter(command5, "PackageID", DbType.Int64, BizActionObj.PackageID);
                dbServer.AddInParameter(command5, "PackageUnitID", DbType.Int64, BizActionObj.PackageUnitID);
                dbServer.AddInParameter(command5, "ItemID", DbType.Int64, BizActionObj.ItemID);
                dbServer.AddInParameter(command5, "ItemGroupID", DbType.Int64, BizActionObj.ItemGroupID);
                dbServer.AddInParameter(command5, "IsGroup", DbType.Boolean, BizActionObj.IsGroup);
                dbServer.AddInParameter(command5, "ItemCategoryID", DbType.Int64, BizActionObj.ItemCategoryID);
                dbServer.AddInParameter(command5, "IsCategory", DbType.Boolean, BizActionObj.IsCategory);
                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command5, "Status", DbType.Boolean, BizActionObj.Status);

                dbServer.AddInParameter(command5, "IsDeletePatientData", DbType.Boolean, BizActionObj.IsDeletePatientData);

                int iStatus = dbServer.ExecuteNonQuery(command5, trans);



                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                //BizActionObj.DeleteItemDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }

        public override IValueObject UpdatePackageApplicableToAllPharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddPackagePharmacyItemsNewBizActionVO objItem = valueObject as clsAddPackagePharmacyItemsNewBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                DbCommand command = null;

                //clsPackageServiceVO objItemVO = objItem.Details;
                command = dbServer.GetStoredProcCommand("CIMS_UpdatePackageApplicableToAllPharmacyItemsForPackage");

                dbServer.AddInParameter(command, "PackageID", DbType.Int64, objItem.PackageID);
                dbServer.AddInParameter(command, "PackageUnitID", DbType.Int64, objItem.PackageUnitID);

                dbServer.AddInParameter(command, "ApplicableToAll", DbType.Boolean, objItem.ApplicableToAll);
                dbServer.AddInParameter(command, "ApplicableToAllDiscount", DbType.Double, objItem.ApplicableToAllDiscount);
                //By Anjali..................
                dbServer.AddInParameter(command, "TotalBudget", DbType.Double, objItem.TotalBudget);
                //...........................

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (objItem.ItemDetails != null)  // && objPackageVO.ItemDetails.Count != 0)
                {
                    DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_DeletePackageItemDetailsAllForPackage");  //CIMS_DeletePackageItemDetails
                    dbServer.AddInParameter(command7, "PackageID", DbType.Int64, objItem.PackageID);
                    dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);  //objItem.PackageUnitID
                    int intStatus3 = dbServer.ExecuteNonQuery(command7, trans);
                }

                trans.Commit();

                if (intStatus > 0)
                {
                    objItem.SuccessStatus = 1;

                }
                else
                {
                    objItem.SuccessStatus = 0;
                }


            }
            catch (Exception ex)
            {
                //throw;
                objItem.SuccessStatus = -1;
                trans.Rollback();
                objItem.ItemDetails = null;
            }
            return objItem;
        }

        //added by neena
        public override IValueObject AddPackageConsentLink(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddPackageConsentLinkBizActionVO BizActionObj = valueObject as clsAddPackageConsentLinkBizActionVO;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                 DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateStatusPackageConsentDetails");
                 dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, BizActionObj.ServiceID);                   
                 dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                 int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                 BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                   


                foreach (var item in BizActionObj.ServiceItemMasterDetails)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePackageConsentDetails");

                    //dbServer.AddInParameter(command5, "TariffID", DbType.Int64, BizActionObj.tariffID);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceID);
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, item.TemplateID);
                    dbServer.AddInParameter(command, "Description", DbType.String, item.Description);
                    dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, item.DepartmentID);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int iStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    

                }
                trans.Commit();


            }
            catch (Exception)
            {
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

        public override IValueObject GetPackageConsentLink(IValueObject valueObject, clsUserVO objUserVO)
        {
            try
            {

                clsGetPackageConsentLinkBizActionVO BizActionObj = valueObject as clsGetPackageConsentLinkBizActionVO;
                BizActionObj.ServiceItemMasterDetails = new List<clsServiceMasterVO>();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPackageConsentDetails");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                //dbServer.AddInParameter(command, "PackageServiceUnitID", DbType.Int64, BizActionObj.PackageServiceUnitID);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsServiceMasterVO objRelation = new clsServiceMasterVO();

                        objRelation.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objRelation.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objRelation.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        objRelation.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objRelation.DepartmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentID"]));
                        BizActionObj.ServiceItemMasterDetails.Add(objRelation);
                    }

                    reader.Close();
                }

                return BizActionObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //

    }
}


