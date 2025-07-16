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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsServiceMasterDAL : clsBaseServiceMasterDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsServiceMasterDAL()
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

        public override IValueObject GetServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceDetail");

                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.IsStatus);

                //By Anjali...................................
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");


                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                //..........................................

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
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
                        objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        objServiceMasterVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));

                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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


        #region Commented Becouse Of Package by CDS
        //public override IValueObject GetAllServiceList(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;

        //    #region  OLD Code Commented by CDS
        //    //try
        //    //{
        //    //    //DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetails");
        //    //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetails");

        //    //    if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
        //    //        dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.ServiceName);
        //    //    dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
        //    //    //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //    //    //dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);

        //    //    DbDataReader reader;

        //    //    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
        //    //    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
        //    //    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
        //    //    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");
        //    //    // dbServer.AddInParameter(command, "SearchExpression", DbType.String, Security.base64Encode(BizActionObj.SearchExpression));

        //    //    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

        //    //    reader = (DbDataReader)dbServer.ExecuteReader(command);

        //    //    if (reader.HasRows)
        //    //    {
        //    //        if (BizActionObj.ServiceList == null)
        //    //            BizActionObj.ServiceList = new List<clsServiceMasterVO>();
        //    //        while (reader.Read())
        //    //        {
        //    //            clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
        //    //            objServiceMasterVO.ID = (long)reader["ID"];
        //    //            objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
        //    //            objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //    //            objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
        //    //            objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
        //    //            objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
        //    //            objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);

        //    //            objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDesc"]);
        //    //            objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDesc"]);
        //    //            objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
        //    //            objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //    //            // objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //    //            // objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);

        //    //            // objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
        //    //            // objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
        //    //            // objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);

        //    //            // objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
        //    //            // objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
        //    //            // objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
        //    //            //// objServiceMasterVO.GeneralDiscount = (bool)DALHelper.HandleDBNull(reader["GeneralDiscount"]);

        //    //            // objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
        //    //            // objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
        //    //            // objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
        //    //            // objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
        //    //            //// objServiceMasterVO.OutSource = (bool)DALHelper.HandleDBNull(reader["OutSource"]);
        //    //            // //objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
        //    //            // objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);

        //    //            // objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
        //    //            // objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
        //    //            // objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);

        //    //            // objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
        //    //            // objServiceMasterVO.Specialization = (Int64)DALHelper.HandleDBNull(reader["SpecializationId"]);

        //    //            // objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
        //    //            // objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
        //    //            // objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);

        //    //            // objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
        //    //            // objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
        //    //            // objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);

        //    //            //  objServiceMasterVO.CheckedAllTariffs = (bool)DALHelper.HandleDBNull(reader["CheckedAllTariffs"]);
        //    //            //objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //    //            //objServiceMasterVO.SubSpecialization = (Int64)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //    //            objServiceMasterVO.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);

        //    //            BizActionObj.ServiceList.Add(objServiceMasterVO);
        //    //        }
        //    //        reader.NextResult();
        //    //        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
        //    //        reader.Close();
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw;
        //    //}


        //    //finally
        //    //{

        //    //}
        //    //return BizActionObj;

        //    #endregion

        //    // Added for IPD by CDS

        //    try
        //    {
        //        if (BizActionObj.IsOLDServiceMaster == true)
        //        {
        //            #region OLD Code
        //            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetails");
        //            //if (BizActionObj.ServiceCode != null)
        //            //{
        //            //    dbServer.AddInParameter(command, "ServiceCode", DbType.Int64, BizActionObj.ServiceCode);
        //            //}
        //            if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
        //                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.ServiceName);
        //            dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
        //            //dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
        //            dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
        //            dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
        //            dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
        //            dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");

        //            dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

        //            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //            if (reader.HasRows)
        //            {
        //                if (BizActionObj.ServiceList == null)
        //                    BizActionObj.ServiceList = new List<clsServiceMasterVO>();
        //                while (reader.Read())
        //                {
        //                    clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
        //                    objServiceMasterVO.ID = (long)reader["ID"];
        //                    objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
        //                    objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                    objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
        //                    // objServiceMasterVO.CodeType = (long)DALHelper.HandleDBNull(reader["CodeType"]);
        //                    objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
        //                    objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
        //                    objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);

        //                    objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDesc"]);
        //                    objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDesc"]);
        //                    objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
        //                    objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //                    // objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //                    // objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);

        //                    // objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
        //                    // objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
        //                    // objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);

        //                    // objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
        //                    // objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
        //                    // objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
        //                    //// objServiceMasterVO.GeneralDiscount = (bool)DALHelper.HandleDBNull(reader["GeneralDiscount"]);

        //                    // objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
        //                    // objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
        //                    // objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
        //                    // objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
        //                    //// objServiceMasterVO.OutSource = (bool)DALHelper.HandleDBNull(reader["OutSource"]);
        //                    // //objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
        //                    // objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);

        //                    // objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
        //                    // objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
        //                    // objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);

        //                    // objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
        //                    // objServiceMasterVO.Specialization = (Int64)DALHelper.HandleDBNull(reader["SpecializationId"]);

        //                    // objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
        //                    // objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
        //                    // objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);

        //                    // objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
        //                    // objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
        //                    // objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);

        //                    //  objServiceMasterVO.CheckedAllTariffs = (bool)DALHelper.HandleDBNull(reader["CheckedAllTariffs"]);
        //                    //objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                    //objServiceMasterVO.SubSpecialization = (Int64)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //                    objServiceMasterVO.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);

        //                    BizActionObj.ServiceList.Add(objServiceMasterVO);
        //                }
        //                reader.NextResult();
        //                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            DbDataReader reader;
        //            DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceList_New");
        //            if (BizActionObj.ServiceCode != null && BizActionObj.ServiceCode != 0)
        //                dbServer.AddInParameter(command, "ServiceCode", DbType.String, BizActionObj.ServiceCode);
        //            if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
        //                dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
        //            dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
        //            dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);

        //            dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
        //            dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
        //            dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
        //            dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");


        //            dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

        //            reader = (DbDataReader)dbServer.ExecuteReader(command);
        //            if (reader.HasRows)
        //            {
        //                if (BizActionObj.ServiceList == null)
        //                    BizActionObj.ServiceList = new List<clsServiceMasterVO>();
        //                while (reader.Read())
        //                {
        //                    clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
        //                    objServiceMasterVO.ID = (long)reader["ID"];
        //                    objServiceMasterVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                    objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
        //                    objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                    objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
        //                    objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
        //                    objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //                    objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
        //                    objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
        //                    objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseServiceRate"]));
        //                    objServiceMasterVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
        //                    objServiceMasterVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
        //                    objServiceMasterVO.IsFavourite = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFavourite"]));
        //                    objServiceMasterVO.IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IslinkWithInventory"]));
        //                    BizActionObj.ServiceList.Add(objServiceMasterVO);
        //                }
        //                reader.NextResult();
        //                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
        //            }
        //            reader.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //    return BizActionObj;

        //}
        #endregion Commented Becouse Of Package by CDS

        #region Added by CDS for Of Package
        public override IValueObject GetAllServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;
            try
            {
                if (BizActionObj.IsOLDServiceMaster == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceDetails");

                    if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                        dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.ServiceName);
                    dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                    dbServer.AddInParameter(command, "ServiceCode", DbType.String, BizActionObj.ServiceCode);
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
                            objServiceMasterVO.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);

                            objServiceMasterVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                            objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PackageID"]));

                            BizActionObj.ServiceList.Add(objServiceMasterVO);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                        reader.Close();
                    }
                }
                else
                {
                    DbDataReader reader;
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceList_New");
                    if (BizActionObj.ServiceCode != null)
                        dbServer.AddInParameter(command, "ServiceCode", DbType.String, BizActionObj.ServiceCode);
                    if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                        dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                    dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                    dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                    dbServer.AddInParameter(command, "IsFromPackage", DbType.Boolean, BizActionObj.IsFromPackage);

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");


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
                            objServiceMasterVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                            objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                            objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                            objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                            objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                            objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                            objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                            objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseServiceRate"]));
                            objServiceMasterVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                            objServiceMasterVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                            objServiceMasterVO.IsFavourite = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFavourite"]));
                            objServiceMasterVO.IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IslinkWithInventory"]));
                            objServiceMasterVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));

                            // Added by CDS
                            objServiceMasterVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                            objServiceMasterVO.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                            BizActionObj.ServiceList.Add(objServiceMasterVO);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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
        #endregion Added by CDS for Of Package
        public override IValueObject GetServiceBySpecialization(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceBySpecializationBizActionVO BizActionObj = (clsGetServiceBySpecializationBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceBySpecialization");


                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.ServiceMaster.Specialization);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.ServiceMaster.Description);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = (long)reader["ID"];
                        objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
                        objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
                        objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                        objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                        //objServiceMasterVO.GeneralDiscount = (bool)DALHelper.HandleDBNull(reader["GeneralDiscount"]);

                        objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
                        objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
                        objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
                        //objServiceMasterVO.OutSource = (bool)DALHelper.HandleDBNull(reader["OutSource"]);
                        //objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);

                        objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
                        objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objServiceMasterVO.Specialization = (Int64)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                        objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
                        objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objServiceMasterVO.SubSpecialization = (Int64)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        objServiceMasterVO.UnitID = (Int64)DALHelper.HandleDBNull(reader["UnitID"]);






                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
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
        public override IValueObject GetAllTariffApplicableList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllTariffsApplicable");

                //if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                //    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                //dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ServiceID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = (long)reader["TariffID"];
                        objServiceMasterVO.ServiceTariffMasterStatus = (bool)DALHelper.HandleDBNull(reader["ServiceTariffMasterStatus"]);
                        objServiceMasterVO.Description = (string)DALHelper.HandleDBNull(reader["Description"].ToString());
                        objServiceMasterVO.TariffCode = (string)DALHelper.HandleDBNull(reader["Description"].ToString());





                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
                }
                if (!reader.IsClosed)
                {
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


        public override IValueObject GetTariffServiceMasterID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceMasterID");

                //if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                //    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                //dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ServiceID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.TariffServiceMasterID = (long)reader["ID"];






                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
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

        public override IValueObject GetAllServiceClassRateDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ServiceClassRateDetails");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceMaster == null)
                        BizActionObj.ServiceMaster = new clsServiceMasterVO();
                    while (reader.Read())
                    {

                        BizActionObj.ServiceMaster.Rate = (decimal)reader["Rate"];

                    }
                    reader.NextResult();
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


        public override IValueObject GetTariffServiceClassRate(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceClassRateBizActionVO BizActionObj = (clsGetTariffServiceClassRateBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_TariffServiceClassRateDetails");
                dbServer.AddInParameter(command, "TariffServiceID", DbType.Int64, BizActionObj.ServiceMaster.TariffServiceMasterID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceMaster == null)
                        BizActionObj.ServiceMaster = new clsServiceMasterVO();
                    while (reader.Read())
                    {

                        BizActionObj.ServiceMaster.Rate = (decimal)reader["Rate"];

                    }
                    reader.NextResult();
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

        public override IValueObject GetServiceTariff(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceTariffBizActionVO BizActionObj = valueObject as clsGetServiceTariffBizActionVO;
            BizActionObj.ServiceList = new List<clsServiceMasterVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ServiceTariffDetails");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ServiceID);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {


                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();

                        objServiceMasterVO.TariffID = (long)reader["TariffID"];





                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
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

        private List<long> GetServiceTarrifIds(long serviceID, long unitId)
        {
            List<long> tariffIds = new List<long>();
            long tariffId;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ServiceTariffDetails");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, serviceID);
                // dbServer.AddInParameter(command, "UnitId", DbType.Int64, unitId);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {


                    while (reader.Read())
                    {


                        tariffId = (long)reader["Id"];
                        tariffIds.Add(tariffId);


                    }
                    reader.Close();
                }
                return tariffIds;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public override IValueObject GetTariffService(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceBizActionVO BizActionObj = valueObject as clsGetTariffServiceBizActionVO;

            BizActionObj.ServiceList = new List<clsServiceMasterVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIDsFromTariffServiceMaster");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ServiceID);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    //if (BizActionObj.ServiceMaster == null)

                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();

                        //BizActionObj.ServiceMaster.TariffID = (long)reader["TariffID"];

                        objServiceMasterVO.TariffServiceMasterID = (long)reader["ID"];





                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
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

        // Added Only for IPD by CDS

        public override IValueObject AddUpdateServiceClassRates(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            clsAddUpdateServiceMasterTariffBizActionVO bizAction = valueObject as clsAddUpdateServiceMasterTariffBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = null;
                if (bizAction.IsApplyToAllTariff == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_ApllyServiceClassRateToAllTariff");
                    dbServer.AddInParameter(command, "IsupdatePreviousRate", DbType.Boolean, bizAction.IsupdatePreviousRate);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_ApllyServiceClassRateToSelectedTariff");
                    dbServer.AddInParameter(command, "TariffID", DbType.String, bizAction.TariffIDList);
                }
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, bizAction.ServiceID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizAction.UnitID);
                dbServer.AddInParameter(command, "IsApplyToAllTariff", DbType.Boolean, bizAction.IsApplyToAllTariff);
                dbServer.AddInParameter(command, "ClassID", DbType.String, bizAction.ClassIDList);
                dbServer.AddInParameter(command, "Rate", DbType.String, bizAction.ClassRateList);
                dbServer.AddOutParameter(command, "SucessStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizAction.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "SucessStatus")));
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }
            return bizAction;
        }

        public override IValueObject ModifyServiceClassRates(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateServiceMasterTariffBizActionVO bizAction = valueObject as clsAddUpdateServiceMasterTariffBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                int intStatus = 0;
                DbCommand command = null;
                if (bizAction.SelectedTariffClassList != null && bizAction.SelectedTariffClassList.Count > 0)
                {
                    foreach (clsServiceTarrifClassRateDetailsNewVO objTariffClassRates in bizAction.SelectedTariffClassList)
                    {
                        if (bizAction.IsRemoveTariffClassRatesLink == true)
                        {
                            command = dbServer.GetStoredProcCommand("CIMS_RemoveTariffClassRatesLink_New");
                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                        }
                        else
                        {
                            command = dbServer.GetStoredProcCommand("CIMS_ModifyTariffClassRates");
                        }
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, bizAction.ServiceID);
                        dbServer.AddInParameter(command, "TariffID", DbType.String, objTariffClassRates.TariffID);
                        dbServer.AddInParameter(command, "ClassID", DbType.String, objTariffClassRates.ClassID);
                        dbServer.AddInParameter(command, "Rate", DbType.String, objTariffClassRates.Rate);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizAction.UnitID);
                        dbServer.AddOutParameter(command, "SucessStatus", DbType.Int32, int.MaxValue);
                        intStatus = dbServer.ExecuteNonQuery(command, trans);
                        bizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "SucessStatus");
                    }
                }
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return bizAction;
        }

        public override IValueObject GetTariffServiceClassRateNew(IValueObject valueObject, clsUserVO objUserVo)
        {
            clsGetTariffServiceClassRateNewBizActionVO BizActionObj = (clsGetTariffServiceClassRateNewBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceTariffClassRateDetails_New");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "TariffName", DbType.String, BizActionObj.TariffName);
                dbServer.AddInParameter(command, "OperationType", DbType.Int32, BizActionObj.OperationType);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceTarrifClassRateDetailsNewVO>();
                    while (reader.Read())
                    {
                        clsServiceTarrifClassRateDetailsNewVO ObjService = new clsServiceTarrifClassRateDetailsNewVO();
                        ObjService.TSMID = Convert.ToInt64(reader["TSM"]);
                        ObjService.ServiceID = Convert.ToInt64(reader["ServiceId"]);
                        ObjService.TariffID = Convert.ToInt64(reader["TariffId"]);
                        ObjService.TariffName = Convert.ToString(reader["TariffDescription"]);
                        ObjService.ClassID = Convert.ToInt64(reader["ClassId"]);
                        ObjService.Rate = Convert.ToDecimal(reader["Rate"]);
                        if (BizActionObj.OperationType == 1)
                        {
                            ObjService.ClassName = Convert.ToString(reader["ClassName"]);
                        }
                        BizActionObj.ServiceList.Add(ObjService);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                    reader.Close();
                }
                if (BizActionObj.ServiceList != null)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetExistClassANDRate");
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                    DbDataReader reader1 = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader1.HasRows)
                    {
                        if (BizActionObj.ExistingClassRates == null)
                            BizActionObj.ExistingClassRates = new List<clsServiceTarrifClassRateDetailsNewVO>();
                        while (reader1.Read())
                        {
                            clsServiceTarrifClassRateDetailsNewVO ObjServiceClass = new clsServiceTarrifClassRateDetailsNewVO();
                            ObjServiceClass.ServiceID = Convert.ToInt64(reader1["ServiceId"]);
                            ObjServiceClass.TariffID = Convert.ToInt64(reader1["TariffId"]);
                            ObjServiceClass.ClassID = Convert.ToInt64(reader1["ClassId"]);
                            ObjServiceClass.ClassName = Convert.ToString(reader1["ClassName"]);
                            ObjServiceClass.Rate = Convert.ToDecimal(reader1["Rate"]);
                            BizActionObj.ExistingClassRates.Add(ObjServiceClass);
                        }
                        reader1.NextResult();
                        reader1.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizActionObj;
        }

        #region Added Only for IPD Service Master

        //public override IValueObject AddServiceMaster(IValueObject valueObject, clsUserVO userVO)
        //{
        //    DbConnection con = null;
        //    DbTransaction trans = null;
        //    clsAddServiceMasterBizActionVO objItem = valueObject as clsAddServiceMasterBizActionVO;
        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        DbCommand command = null;
        //        clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;
        //        if (objItemVO.EditMode == true)
        //        {
        //            command = dbServer.GetStoredProcCommand("CIMS_UpdateServiceMaster");
        //            command.Connection = con;
        //            dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
        //            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, userVO.ID); //objItemVO.UpdatedBy);
        //            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName); //objItemVO.UpdatedOn);
        //            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now); //objItemVO.UpdatedDateTime);

        //            dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);// objItemVO.UpdateWindowsLoginName);
        //        }
        //        else
        //        {
        //            command = dbServer.GetStoredProcCommand("CIMS_AddServiceMaster");
        //            command.Connection = con;
        //            dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
        //            dbServer.AddInParameter(command, "ServiceCode", DbType.String, objItemVO.ServiceCode);
        //            dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItemVO.ServiceID);
        //            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, userVO.ID); //objItemVO.AddedBy);
        //            dbServer.AddInParameter(command, "AddedOn", DbType.String, userVO.UserLoginInfo.MachineName); //objItemVO.AddedOn);
        //            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now); //objItemVO.AddedDateTime);
        //            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);// objItemVO.AddedWindowsLoginName);

        //        }


        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId); //objItemVO.UnitID);
        //        dbServer.AddInParameter(command, "CodeType", DbType.Int64, objItemVO.CodeType);
        //        dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
        //        dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItemVO.Specialization);
        //        dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItemVO.SubSpecialization);
        //        dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.ServiceName);
        //        dbServer.AddInParameter(command, "ShortDescription", DbType.String, objItemVO.ShortDescription);
        //        dbServer.AddInParameter(command, "LongDescription", DbType.String, objItemVO.LongDescription);

        //        dbServer.AddInParameter(command, "StaffDiscount", DbType.Boolean, objItemVO.StaffDiscount);
        //        dbServer.AddInParameter(command, "StaffDiscountAmount", DbType.Decimal, objItemVO.StaffDiscountAmount);
        //        dbServer.AddInParameter(command, "StaffDiscountPercent", DbType.Decimal, objItemVO.StaffDiscountPercent);

        //        dbServer.AddInParameter(command, "StaffDependantDiscount", DbType.Boolean, objItemVO.StaffDependantDiscount);
        //        dbServer.AddInParameter(command, "StaffDependantDiscountAmount", DbType.Decimal, objItemVO.StaffDependantDiscountAmount);
        //        dbServer.AddInParameter(command, "StaffDependantDiscountPercent", DbType.Decimal, objItemVO.StaffDependantDiscountPercent);


        //        dbServer.AddInParameter(command, "Concession", DbType.Boolean, objItemVO.Concession);
        //        dbServer.AddInParameter(command, "ConcessionAmount", DbType.Decimal, objItemVO.ConcessionAmount);
        //        dbServer.AddInParameter(command, "ConcessionPercent", DbType.Decimal, objItemVO.ConcessionPercent);

        //        dbServer.AddInParameter(command, "ServiceTax", DbType.Boolean, objItemVO.ServiceTax);
        //        dbServer.AddInParameter(command, "ServiceTaxAmount", DbType.Decimal, objItemVO.ServiceTaxAmount);
        //        dbServer.AddInParameter(command, "ServiceTaxPercent", DbType.Decimal, objItemVO.ServiceTaxPercent);

        //        dbServer.AddInParameter(command, "IsPackage", DbType.Boolean, objItemVO.IsPackage);
        //        dbServer.AddInParameter(command, "InHouse", DbType.Boolean, objItemVO.InHouse);
        //        dbServer.AddInParameter(command, "DoctorShare", DbType.Boolean, objItemVO.DoctorShare);
        //        dbServer.AddInParameter(command, "DoctorSharePercentage", DbType.Decimal, objItemVO.DoctorSharePercentage);
        //        dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Decimal, objItemVO.DoctorShareAmount);
        //        dbServer.AddInParameter(command, "RateEditable", DbType.Boolean, objItemVO.RateEditable);
        //        dbServer.AddInParameter(command, "MaxRate", DbType.Decimal, objItemVO.MaxRate);
        //        dbServer.AddInParameter(command, "MinRate", DbType.Decimal, objItemVO.MinRate);
        //        dbServer.AddInParameter(command, "Rate", DbType.Decimal, objItemVO.Rate);

        //        dbServer.AddInParameter(command, "CheckedAllTariffs", DbType.Boolean, objItemVO.CheckedAllTariffs);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);


        //        //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.ID );

        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);

        //        objItemVO.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

        //        objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        if (objItemVO.EditMode != true)
        //        {

        //            objItem.ServiceID = objItemVO.ID;
        //            objItem.ServiceMasterDetails.ServiceCode = objItemVO.ID.ToString();
        //        }

        //        //For ServiceClassRate Details
        //        DbCommand command1 = null;
        //        command1 = dbServer.GetStoredProcCommand("CIMS_AddServiceClassRateDetails");
        //        command1.Connection = con;

        //        dbServer.AddInParameter(command1, "Id", DbType.Int64, 0);
        //        if (objItemVO.EditMode != true)
        //            dbServer.AddInParameter(command1, "ServiceId", DbType.Int64, objItem.ServiceID);
        //        else
        //            dbServer.AddInParameter(command1, "ServiceId", DbType.Int64, objItemVO.ID);
        //        dbServer.AddInParameter(command1, "ClassId", DbType.Int64, 1);
        //        dbServer.AddInParameter(command1, "Rate", DbType.Decimal, objItemVO.Rate);
        //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objItemVO.Status);
        //        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
        //        int queryStatus = dbServer.ExecuteNonQuery(command1, trans);



        //        //For TariffServiceMaster
        //        clsAddTariffServiceBizActionVO _objBizActionVO = new clsAddTariffServiceBizActionVO();
        //        _objBizActionVO.ServiceMasterDetails = objItemVO;
        //        _objBizActionVO.TariffList = objItemVO.TariffIDList;
        //        _objBizActionVO.ServiceMasterDetails.ServiceCode = objItemVO.ServiceCode;
        //        if (objItemVO.EditMode != true)
        //            _objBizActionVO.ServiceMasterDetails.ServiceID = objItem.ServiceID;
        //        else
        //            _objBizActionVO.ServiceMasterDetails.ServiceID = objItemVO.ID;
        //        _objBizActionVO.TariffServiceForm = false;
        //        long ServiceID = _objBizActionVO.ServiceMasterDetails.ServiceID;

        //        List<long> tarrifIds = GetServiceTarrifIds(ServiceID, objItemVO.UnitID);

        //        if (_objBizActionVO.TariffServiceForm == false)
        //        {
        //            UpdateTariffServiceMaster(tarrifIds, con, trans);
        //            UpdateTariffServiceClassRateDetails(tarrifIds, con, trans);
        //        }

        //        DbCommand command2 = null;
        //        command2 = dbServer.GetStoredProcCommand("CIMS_AddTariffService");
        //        command2.Connection = con;

        //        if (_objBizActionVO.TariffList != null && _objBizActionVO.TariffList.Count > 0)
        //        {
        //            for (int i = 0; i <= _objBizActionVO.TariffList.Count - 1; i++)
        //            {
        //                command2.Parameters.Clear();

        //                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objItemVO.UnitID);
        //                dbServer.AddInParameter(command2, "TariffID", DbType.Int64, Convert.ToInt64(objItemVO.TariffIDList[i]));
        //                dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, objItemVO.ID);
        //                dbServer.AddInParameter(command2, "ServiceCode", DbType.String, objItemVO.ServiceCode);
        //                dbServer.AddInParameter(command2, "SpecializationId", DbType.Int64, objItemVO.Specialization);
        //                dbServer.AddInParameter(command2, "SubSpecializationId", DbType.Int64, objItemVO.SubSpecialization);
        //                dbServer.AddInParameter(command2, "ShortDescription", DbType.String, objItemVO.ShortDescription);
        //                dbServer.AddInParameter(command2, "LongDescription", DbType.String, objItemVO.LongDescription);
        //                dbServer.AddInParameter(command2, "Description", DbType.String, objItemVO.ServiceName);
        //                dbServer.AddInParameter(command2, "CodeType", DbType.Int64, objItemVO.CodeType);
        //                dbServer.AddInParameter(command2, "Code", DbType.String, objItemVO.Code);
        //                dbServer.AddInParameter(command2, "StaffDiscount", DbType.Boolean, objItemVO.StaffDiscount);
        //                dbServer.AddInParameter(command2, "StaffDiscountAmount", DbType.Decimal, objItemVO.StaffDiscountAmount);
        //                dbServer.AddInParameter(command2, "StaffDiscountPercent", DbType.Decimal, objItemVO.StaffDiscountPercent);

        //                dbServer.AddInParameter(command2, "StaffDependantDiscount", DbType.Boolean, objItemVO.StaffDependantDiscount);
        //                dbServer.AddInParameter(command2, "StaffDependantDiscountAmount", DbType.Decimal, objItemVO.StaffDependantDiscountAmount);
        //                dbServer.AddInParameter(command2, "StaffDependantDiscountPercent", DbType.Decimal, objItemVO.StaffDependantDiscountPercent);


        //                dbServer.AddInParameter(command2, "Concession", DbType.Boolean, objItemVO.Concession);
        //                dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Decimal, objItemVO.ConcessionAmount);
        //                dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Decimal, objItemVO.ConcessionPercent);

        //                dbServer.AddInParameter(command2, "ServiceTax", DbType.Boolean, objItemVO.ServiceTax);
        //                dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Decimal, objItemVO.ServiceTaxAmount);
        //                dbServer.AddInParameter(command2, "ServiceTaxPercent", DbType.Decimal, objItemVO.ServiceTaxPercent);


        //                dbServer.AddInParameter(command2, "InHouse", DbType.Boolean, objItemVO.InHouse);
        //                dbServer.AddInParameter(command2, "DoctorShare", DbType.Boolean, objItemVO.DoctorShare);
        //                dbServer.AddInParameter(command2, "DoctorSharePercentage", DbType.Decimal, objItemVO.DoctorSharePercentage);
        //                dbServer.AddInParameter(command2, "DoctorShareAmount", DbType.Decimal, objItemVO.DoctorShareAmount);
        //                dbServer.AddInParameter(command2, "RateEditable", DbType.Boolean, objItemVO.RateEditable);
        //                dbServer.AddInParameter(command2, "MaxRate", DbType.Decimal, objItemVO.MaxRate);
        //                dbServer.AddInParameter(command2, "MinRate", DbType.Decimal, objItemVO.MinRate);
        //                dbServer.AddInParameter(command2, "Rate", DbType.Decimal, objItemVO.Rate);
        //                dbServer.AddInParameter(command2, "CheckedAllTariffs", DbType.Boolean, objItemVO.CheckedAllTariffs);
        //                dbServer.AddInParameter(command2, "Status", DbType.Boolean, objItemVO.Status);
        //                dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
        //                dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
        //                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objItemVO.AddedBy);
        //                dbServer.AddInParameter(command2, "AddedOn", DbType.String, objItemVO.AddedOn);
        //                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);

        //                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);

        //                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0);
        //                dbServer.AddOutParameter(command2, "Id", DbType.Int64, 0);

        //                int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
        //                _objBizActionVO.TariffServiceID = (Int64)dbServer.GetParameterValue(command2, "Id");


        //                //For TarrifServiceClassRateDetail
        //                DbCommand command3 = null;
        //                command3 = dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
        //                command3.Connection = con;

        //                dbServer.AddInParameter(command3, "TariffServiceId", DbType.Int64, _objBizActionVO.TariffServiceID);
        //                dbServer.AddInParameter(command3, "ClassId", DbType.Int64, 1);
        //                dbServer.AddInParameter(command3, "Rate", DbType.Int64, objItemVO.Rate);
        //                dbServer.AddInParameter(command3, "Status", DbType.Boolean, objItemVO.Status);
        //                dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objItemVO.UnitID);

        //                int TarrifServiceClassRateDetailStatus = dbServer.ExecuteNonQuery(command3, trans);
        //            }
        //        }

        //        trans.Commit();

        //    }




        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        objItem.ServiceMasterDetails = null;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //        con = null;
        //        trans = null;
        //    }
        //    return objItem;
        //}

        public override IValueObject AddServiceMaster(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            clsAddServiceMasterBizActionVO bizAction = valueObject as clsAddServiceMasterBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = null;
                clsServiceMasterVO objItemVO = bizAction.ServiceMasterDetails;
                if (bizAction.IsModify == true)
                {
                    if (bizAction.IsOLDServiceMaster == false)
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_UpdateServiceMaster");
                        dbServer.AddInParameter(command, "LuxuryTaxAmount", DbType.Decimal, objItemVO.LuxuryTaxAmount);
                        dbServer.AddInParameter(command, "LuxuryTaxPercent", DbType.Decimal, objItemVO.LuxuryTaxPercent);

                    }
                    else
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_UpdateServiceMaster_OLD");
                    }
                    command.Connection = con;
                    dbServer.AddInParameter(command, "ID", DbType.Int64, bizAction.ServiceMasterDetails.ID);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, userVO.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);// objItemVO.UpdateWindowsLoginName);
                }
                else
                {
                    if (bizAction.IsOLDServiceMaster == false)
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_AddServiceMaster");
                        dbServer.AddInParameter(command, "LuxuryTaxAmount", DbType.Decimal, objItemVO.LuxuryTaxAmount);
                        dbServer.AddInParameter(command, "LuxuryTaxPercent", DbType.Decimal, objItemVO.LuxuryTaxPercent);
                    }
                    else
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_AddServiceMaster_OLD");
                    }
                    command.Connection = con;
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command, "ServiceCode", DbType.String, objItemVO.ServiceCode);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItemVO.ServiceID);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, userVO.ID); //objItemVO.AddedBy);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, userVO.UserLoginInfo.MachineName); //objItemVO.AddedOn);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now); //objItemVO.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);// objItemVO.AddedWindowsLoginName);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID); //objItemVO.UnitID);
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
                dbServer.AddInParameter(command, "IsPackage", DbType.Boolean, objItemVO.IsPackage);
                dbServer.AddInParameter(command, "InHouse", DbType.Boolean, objItemVO.InHouse);
                dbServer.AddInParameter(command, "DoctorShare", DbType.Boolean, objItemVO.DoctorShare);
                dbServer.AddInParameter(command, "DoctorSharePercentage", DbType.Decimal, objItemVO.DoctorSharePercentage);
                dbServer.AddInParameter(command, "DoctorShareAmount", DbType.Decimal, objItemVO.DoctorShareAmount);
                dbServer.AddInParameter(command, "RateEditable", DbType.Boolean, objItemVO.RateEditable);
                dbServer.AddInParameter(command, "MaxRate", DbType.Decimal, objItemVO.MaxRate);
                dbServer.AddInParameter(command, "MinRate", DbType.Decimal, objItemVO.MinRate);
                dbServer.AddInParameter(command, "SACCodeID", DbType.Int64, objItemVO.SACCodeID);  //For GST Details 27062017..
                if (bizAction.IsOLDServiceMaster == false)
                {
                    dbServer.AddInParameter(command, "BaseServiceRate", DbType.Decimal, objItemVO.Rate);
                    dbServer.AddInParameter(command, "IsFavourite", DbType.Boolean, objItemVO.IsFavourite);
                    dbServer.AddInParameter(command, "IslinkWithInventory", DbType.Boolean, objItemVO.IsLinkWithInventory);
                    dbServer.AddInParameter(command, "CodeDetails", DbType.String, objItemVO.CodeDetails);
                }
                else
                {
                    dbServer.AddInParameter(command, "Rate", DbType.Decimal, objItemVO.Rate);
                }
                dbServer.AddInParameter(command, "CheckedAllTariffs", DbType.Boolean, objItemVO.CheckedAllTariffs);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                objItemVO.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                bizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (bizAction.IsOLDServiceMaster == false)
                {
                    if (objItemVO.ID != 0)
                    {
                        if (bizAction.IsModify == true)
                        {
                            bizAction.ServiceID = objItemVO.ID;
                            bizAction.ServiceMasterDetails.ServiceCode = objItemVO.ID.ToString();
                        }
                        foreach (var item in bizAction.ServiceClassList)
                        {
                            command = dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceClassRateDetails");
                            command.Connection = con;
                            dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command, "ServiceId", DbType.Int64, objItemVO.ID);
                            dbServer.AddInParameter(command, "ClassId", DbType.Int64, item.ClassID);
                            dbServer.AddInParameter(command, "Rate", DbType.Decimal, item.Rate);
                            dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                            dbServer.ExecuteNonQuery(command, trans);
                        }
                    }
                }
                else
                {
                    #region VitaLife OLD Add ServiceMastwer

                    ////////For ServiceClassRate Details
                    DbCommand command1 = null;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddServiceClassRateDetails");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "Id", DbType.Int64, 0);
                    if (objItemVO.EditMode == true)
                        dbServer.AddInParameter(command1, "ServiceId", DbType.Int64, bizAction.ServiceMasterDetails.ID);
                    else
                        dbServer.AddInParameter(command1, "ServiceId", DbType.Int64, objItemVO.ID);
                    dbServer.AddInParameter(command1, "ClassId", DbType.Int64, 1);
                    dbServer.AddInParameter(command1, "Rate", DbType.Decimal, objItemVO.Rate);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objItemVO.Status);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                    int queryStatus = dbServer.ExecuteNonQuery(command1, trans);

                    //For TariffServiceMaster
                    clsAddTariffServiceBizActionVO _objBizActionVO = new clsAddTariffServiceBizActionVO();
                    _objBizActionVO.ServiceMasterDetails = objItemVO;
                    _objBizActionVO.TariffList = objItemVO.TariffIDList;
                    _objBizActionVO.ServiceMasterDetails.ServiceCode = objItemVO.ServiceCode;
                    if (objItemVO.EditMode != true)
                        _objBizActionVO.ServiceMasterDetails.ServiceID = bizAction.ServiceID;
                    else
                        _objBizActionVO.ServiceMasterDetails.ServiceID = objItemVO.ID;
                    _objBizActionVO.TariffServiceForm = false;
                    long ServiceID = _objBizActionVO.ServiceMasterDetails.ServiceID;

                    List<long> tarrifIds = GetServiceTarrifIds(ServiceID, objItemVO.UnitID);

                    if (_objBizActionVO.TariffServiceForm == false)
                    {
                        UpdateTariffServiceMaster(tarrifIds, con, trans);
                        UpdateTariffServiceClassRateDetails(tarrifIds, con, trans);
                    }

                    DbCommand command2 = null;
                    command2 = dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                    command2.Connection = con;

                    if (_objBizActionVO.TariffList != null && _objBizActionVO.TariffList.Count > 0)
                    {
                        for (int i = 0; i <= _objBizActionVO.TariffList.Count - 1; i++)
                        {
                            command2.Parameters.Clear();

                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objItemVO.UnitID);
                            dbServer.AddInParameter(command2, "TariffID", DbType.Int64, Convert.ToInt64(objItemVO.TariffIDList[i]));
                            dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, objItemVO.ID);
                            dbServer.AddInParameter(command2, "ServiceCode", DbType.String, objItemVO.ServiceCode);
                            dbServer.AddInParameter(command2, "SpecializationId", DbType.Int64, objItemVO.Specialization);
                            dbServer.AddInParameter(command2, "SubSpecializationId", DbType.Int64, objItemVO.SubSpecialization);
                            dbServer.AddInParameter(command2, "ShortDescription", DbType.String, objItemVO.ShortDescription);
                            dbServer.AddInParameter(command2, "LongDescription", DbType.String, objItemVO.LongDescription);
                            dbServer.AddInParameter(command2, "Description", DbType.String, objItemVO.ServiceName);
                            dbServer.AddInParameter(command2, "CodeType", DbType.Int64, objItemVO.CodeType);
                            dbServer.AddInParameter(command2, "Code", DbType.String, objItemVO.Code);
                            dbServer.AddInParameter(command2, "StaffDiscount", DbType.Boolean, objItemVO.StaffDiscount);
                            dbServer.AddInParameter(command2, "StaffDiscountAmount", DbType.Decimal, objItemVO.StaffDiscountAmount);
                            dbServer.AddInParameter(command2, "StaffDiscountPercent", DbType.Decimal, objItemVO.StaffDiscountPercent);

                            dbServer.AddInParameter(command2, "StaffDependantDiscount", DbType.Boolean, objItemVO.StaffDependantDiscount);
                            dbServer.AddInParameter(command2, "StaffDependantDiscountAmount", DbType.Decimal, objItemVO.StaffDependantDiscountAmount);
                            dbServer.AddInParameter(command2, "StaffDependantDiscountPercent", DbType.Decimal, objItemVO.StaffDependantDiscountPercent);


                            dbServer.AddInParameter(command2, "Concession", DbType.Boolean, objItemVO.Concession);
                            dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Decimal, objItemVO.ConcessionAmount);
                            dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Decimal, objItemVO.ConcessionPercent);

                            dbServer.AddInParameter(command2, "ServiceTax", DbType.Boolean, objItemVO.ServiceTax);
                            dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Decimal, objItemVO.ServiceTaxAmount);
                            dbServer.AddInParameter(command2, "ServiceTaxPercent", DbType.Decimal, objItemVO.ServiceTaxPercent);

                            dbServer.AddInParameter(command2, "SeniorCitizen", DbType.Boolean, objItemVO.SeniorCitizen);
                            dbServer.AddInParameter(command2, "SeniorCitizenConAmount", DbType.Decimal, objItemVO.SeniorCitizenConAmount);
                            dbServer.AddInParameter(command2, "SeniorCitizenConPercent", DbType.Decimal, objItemVO.SeniorCitizenConPercent);
                            dbServer.AddInParameter(command2, "SeniorCitizenAge", DbType.Int16, objItemVO.SeniorCitizenAge);

                            dbServer.AddInParameter(command2, "InHouse", DbType.Boolean, objItemVO.InHouse);
                            dbServer.AddInParameter(command2, "DoctorShare", DbType.Boolean, objItemVO.DoctorShare);
                            dbServer.AddInParameter(command2, "DoctorSharePercentage", DbType.Decimal, objItemVO.DoctorSharePercentage);
                            dbServer.AddInParameter(command2, "DoctorShareAmount", DbType.Decimal, objItemVO.DoctorShareAmount);
                            dbServer.AddInParameter(command2, "RateEditable", DbType.Boolean, objItemVO.RateEditable);
                            dbServer.AddInParameter(command2, "MaxRate", DbType.Decimal, objItemVO.MaxRate);
                            dbServer.AddInParameter(command2, "MinRate", DbType.Decimal, objItemVO.MinRate);
                            dbServer.AddInParameter(command2, "Rate", DbType.Decimal, objItemVO.Rate);
                            dbServer.AddInParameter(command2, "CheckedAllTariffs", DbType.Boolean, objItemVO.CheckedAllTariffs);
                            dbServer.AddInParameter(command2, "Status", DbType.Boolean, objItemVO.Status);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                            dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, objItemVO.AddedOn);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);

                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);

                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0);
                            dbServer.AddOutParameter(command2, "Id", DbType.Int64, 0);

                            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                            _objBizActionVO.TariffServiceID = (Int64)dbServer.GetParameterValue(command2, "Id");


                            //For TarrifServiceClassRateDetail
                            DbCommand command3 = null;
                            command3 = dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                            command3.Connection = con;

                            dbServer.AddInParameter(command3, "TariffServiceId", DbType.Int64, _objBizActionVO.TariffServiceID);
                            dbServer.AddInParameter(command3, "ClassId", DbType.Int64, 1);
                            dbServer.AddInParameter(command3, "Rate", DbType.Int64, objItemVO.Rate);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, objItemVO.Status);
                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objItemVO.UnitID);

                            int TarrifServiceClassRateDetailStatus = dbServer.ExecuteNonQuery(command3, trans);
                        }
                    }
                    #endregion
                }
                trans.Commit();



            }
            catch (Exception ex)
            {
                trans.Rollback();
                bizAction.ServiceMasterDetails = null;
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }
            return bizAction;
        }


        #endregion




        public override IValueObject DeletetariffServiceAndServiceTariffMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsDeleteTariffServiceAndServiceTariffBizActionVO objItem = valueObject as clsDeleteTariffServiceAndServiceTariffBizActionVO;

            try
            {
                DbCommand command = null;
                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;

                objItemVO.Query = "Delete from T_ServiceTariffMaster where ServiceId=" + objItemVO.ServiceID;
                objItemVO.Query = objItemVO.Query + " Delete from T_TariffServiceMaster where ServiceId=" + objItemVO.ServiceID;

                //string Query1 = "";


                command = dbServer.GetSqlStringCommand(objItemVO.Query);


                //}



                int intStatus = dbServer.ExecuteNonQuery(command);
                if (intStatus > 0)
                {
                    objItem.SuccessStatus = 1;
                    objItemVO.Query = "";
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

        public override IValueObject DeleteTariffServiceClassRateDetail(IValueObject valueObject, clsUserVO userVO)
        {
            clsDeletetTriffServiceClassRateDetailsBizActionVO objItem = valueObject as clsDeletetTriffServiceClassRateDetailsBizActionVO;

            try
            {
                DbCommand command = null;
                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;

                objItemVO.Query = "Delete from T_TariffServiceClassRateDetail where TariffServiceId in (" + objItemVO.TariffIDs + ")";
                // objItemVO.Query = "";



                command = dbServer.GetSqlStringCommand(objItemVO.Query);


                //}



                int intStatus = dbServer.ExecuteNonQuery(command);
                if (intStatus > 0)
                {
                    objItem.SuccessStatus = 1;
                    objItemVO.Query = "";
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

        public override IValueObject CheckForTariffExistanceInServiceTariffMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddServiceTariffBizActionVO objItem = valueObject as clsAddServiceTariffBizActionVO;
            try
            {
                DbCommand command = null;
                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;


                command = dbServer.GetSqlStringCommand(objItemVO.Query);






                int intStatus = (int)dbServer.ExecuteScalar(command);
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

        public void UpdateTariffServiceMaster(List<long> ids, DbConnection con, DbTransaction trans)
        {
            try
            {
                DbCommand command = null;
                for (int i = 0; i < ids.Count; i++)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_ChangeTariffServiceStatus");
                    command.Connection = con;
                    command.Parameters.Clear();
                    dbServer.AddInParameter(command, "ID", DbType.Int64, ids[i]);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, 0);
                    int result = dbServer.ExecuteNonQuery(command, trans);

                }
            }
            finally
            {
            }

        }

        public void UpdateTariffServiceClassRateDetails(List<long> ids, DbConnection con, DbTransaction trans)
        {
            try
            {
                DbCommand command = null;
                for (int i = 0; i < ids.Count; i++)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_ChangeTariffServiceClassRateDetailsStatus");
                    command.Connection = con;
                    command.Parameters.Clear();
                    dbServer.AddInParameter(command, "ID", DbType.Int64, ids[i]);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, 0);
                    int result = dbServer.ExecuteNonQuery(command, trans);

                }
            }
            finally { }


        }

        public override IValueObject AddTariffService(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsAddTariffServiceBizActionVO objItem = valueObject as clsAddTariffServiceBizActionVO;
            try
            {
                DbCommand command = null;
                DbCommand command1 = null;
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;
                List<long> tarrifIds = GetServiceTarrifIds(objItemVO.ID, objItem.ServiceMasterDetails.UnitID);


                if (objItem.TariffServiceForm == false)
                {
                    UpdateTariffServiceMaster(tarrifIds, con, trans);
                    UpdateTariffServiceClassRateDetails(tarrifIds, con, trans);
                }


                //if (objItemVO.EditMode == true)
                //{
                //    command = dbServer.GetStoredProcCommand("CIMS_UpdateTariffService");
                //    dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);

                //}
                //else
                //{
                command = dbServer.GetStoredProcCommand("CIMS_AddTariffService");
                command.Connection = con;


                //}



                if (objItem.TariffList.Count > 0)
                {
                    for (int i = 0; i <= objItem.TariffList.Count - 1; i++)
                    {
                        command.Parameters.Clear();


                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                        dbServer.AddInParameter(command, "TariffID", DbType.Int64, Convert.ToInt64(objItem.TariffList[i]));
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItemVO.ID);
                        dbServer.AddInParameter(command, "ServiceCode", DbType.String, objItemVO.ServiceCode);
                        dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItemVO.Specialization);
                        dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItemVO.SubSpecialization);
                        dbServer.AddInParameter(command, "ShortDescription", DbType.String, objItemVO.ShortDescription);
                        dbServer.AddInParameter(command, "LongDescription", DbType.String, objItemVO.LongDescription);
                        dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.ServiceName);
                        dbServer.AddInParameter(command, "CodeType", DbType.Int64, objItemVO.CodeType);
                        dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
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
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objItemVO.UpdatedBy);
                        //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                        //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        //dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                        dbServer.AddOutParameter(command, "Id", DbType.Int64, 0);
                        //dbServer.AddOutParameter(command, "TariffIServiceID", DbType.Int64, 0);
                        //dbServer.AddOutParameter(command, "ServiceID", DbType.Int32, 0);
                        int intStatus = dbServer.ExecuteNonQuery(command, trans);
                        //objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        //objItem.ServiceID = (int)dbServer.GetParameterValue(command, "ServiceID");
                        objItem.TariffServiceID = (Int64)dbServer.GetParameterValue(command, "Id");

                        foreach (var item in objItem.ClassList)
                        {
                            command1 = dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                            command1.Connection = con;
                            //For TarrifServiceClassRateDetail
                            dbServer.AddInParameter(command1, "TariffServiceId", DbType.Int64, objItem.TariffServiceID);
                            dbServer.AddInParameter(command1, "ClassId", DbType.Int64, item.ClassID);
                            dbServer.AddInParameter(command1, "Rate", DbType.Int64, item.Rate);
                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objItemVO.UnitID);

                            int TarrifServiceClassRateDetailStatus = dbServer.ExecuteNonQuery(command1, trans);


                        }



                    }



                }


                trans.Commit();
                objItem.SuccessStatus = 0;

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

        public override IValueObject UpdateTariffService(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddTariffServiceBizActionVO objItem = valueObject as clsAddTariffServiceBizActionVO;
            try
            {
                DbCommand command = null;
                DbCommand command1 = null;
                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;
                //List<long> tarrifIds = GetServiceTarrifIds(objItemVO.ID, objItem.ServiceMasterDetails.UnitID);


                //UpdateTariffServiceMaster(tarrifIds);
                //UpdateTariffServiceClassRateDetails(tarrifIds);


                //if (objItemVO.EditMode == true)
                //{
                //    command = dbServer.GetStoredProcCommand("CIMS_UpdateTariffService");
                //    dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);

                //}
                //else
                //{
                command = dbServer.GetStoredProcCommand("CIMS_AddTariffService");


                //}



                if (objItem.TariffList.Count > 0)
                {
                    for (int i = 0; i <= objItem.TariffList.Count - 1; i++)
                    {
                        command.Parameters.Clear();

                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                        dbServer.AddInParameter(command, "TariffID", DbType.Int64, Convert.ToInt64(objItem.TariffList[i]));
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objItemVO.ID);
                        dbServer.AddInParameter(command, "ServiceCode", DbType.String, objItemVO.ServiceCode);
                        dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItemVO.Specialization);
                        dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItemVO.SubSpecialization);
                        dbServer.AddInParameter(command, "ShortDescription", DbType.String, objItemVO.ShortDescription);
                        dbServer.AddInParameter(command, "LongDescription", DbType.String, objItemVO.LongDescription);
                        dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.ServiceName);
                        dbServer.AddInParameter(command, "CodeType", DbType.Int64, objItemVO.CodeType);
                        dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
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
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        //dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objItemVO.UpdatedBy);
                        //dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                        //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        //dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                        dbServer.AddOutParameter(command, "Id", DbType.Int64, 0);
                        //dbServer.AddOutParameter(command, "TariffIServiceID", DbType.Int64, 0);
                        //dbServer.AddOutParameter(command, "ServiceID", DbType.Int32, 0);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        //objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        //objItem.ServiceID = (int)dbServer.GetParameterValue(command, "ServiceID");
                        objItem.TariffServiceID = (Int64)dbServer.GetParameterValue(command, "Id");



                        command1 = dbServer.GetStoredProcCommand("CIMS_AddTariffServiceClassRateDetail");
                        //For TarrifServiceClassRateDetail
                        dbServer.AddInParameter(command1, "TariffServiceId", DbType.Int64, objItem.TariffServiceID);
                        dbServer.AddInParameter(command1, "ClassId", DbType.Int64, 1);
                        dbServer.AddInParameter(command1, "Rate", DbType.Int64, objItemVO.Rate);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objItemVO.Status);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objItemVO.UnitID);

                        int TarrifServiceClassRateDetailStatus = dbServer.ExecuteNonQuery(command1);


                    }



                }




            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;
        }

        //public override IValueObject GetTariffServiceList(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsGetTariffServiceListBizActionVO BizActionObj = (clsGetTariffServiceListBizActionVO)valueObject;
        //    DbCommand command;
        //     DbDataReader reader;
        //    try
        //    {

        //            command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceList");
        //             if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
        //            dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
        //            dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
        //            dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
        //            dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
        //            dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
        //            dbServer.AddInParameter(command, "PatientSourceType", DbType.Int16, BizActionObj.PatientSourceType);
        //            dbServer.AddInParameter(command, "PatientSourceTypeID", DbType.Int64, BizActionObj.PatientSourceTypeID);
        //            dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
        //            dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
        //            dbServer.AddInParameter(command, "GetSuggestedServices", DbType.Boolean, BizActionObj.GetSuggestedServices);
        //            dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
        //            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
        //            if (BizActionObj.ClassID > 0)
        //                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ClassID); 


        //        reader= (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.ServiceList == null)
        //                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
        //            while (reader.Read())
        //            {
        //                clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
        //                objServiceMasterVO.ID = (long)reader["ServiceID"];
        //                objServiceMasterVO.TariffServiceMasterID = (long)reader["ID"];
        //                objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
        //                objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
        //                objServiceMasterVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
        //                objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
        //                objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //                if (BizActionObj.PatientSourceType == 2) // Camp
        //                {
        //                    objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
        //                    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                }
        //                else if (BizActionObj.PatientSourceType == 1)   //Loyalty
        //                {
        //                    objServiceMasterVO.Rate =  Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
        //                    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                }
        //                else
        //                {
        //                    objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
        //                    objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
        //                    objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
        //                }

        //                objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
        //                objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
        //                objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
        //                objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
        //                objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
        //                objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
        //                objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);                       
        //                objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
        //                objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
        //                objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
        //                objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
        //                objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
        //                objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
        //                objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
        //                objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
        //                objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
        //                objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
        //                objServiceMasterVO.TarrifCode = (string)DALHelper.HandleDBNull(reader["TarrifServiceCode"]);
        //                objServiceMasterVO.TarrifName = (string)DALHelper.HandleDBNull(reader["TarrifServiceName"]);
        //                objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //                objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
        //                objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
        //                objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
        //                objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
        //                objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);



        //                BizActionObj.ServiceList.Add(objServiceMasterVO);
        //            }
        //            reader.NextResult();


        //        }
        //        if (reader.IsClosed == false)
        //        {
        //            reader.Close();

        //        }

        //    }


        //    catch (Exception ex)
        //    {
        //        throw;
        //    }


        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}

        #region  Commented By CDS this is OLD
        //public override IValueObject GetTariffServiceList(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsGetTariffServiceListBizActionVO BizActionObj = (clsGetTariffServiceListBizActionVO)valueObject;
        //    DbCommand command;
        //    DbDataReader reader;
        //    try
        //    {
        //        if (BizActionObj.PrescribedService == true)
        //        {
        //            command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceList");  //CIMS_GetTariffServiceList

        //            if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
        //                dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
        //            dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
        //            dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
        //            dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
        //            dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
        //            dbServer.AddInParameter(command, "PatientSourceType", DbType.Int16, BizActionObj.PatientSourceType);
        //            dbServer.AddInParameter(command, "PatientSourceTypeID", DbType.Int64, BizActionObj.PatientSourceTypeID);
        //            dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
        //            dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
        //            dbServer.AddInParameter(command, "GetSuggestedServices", DbType.Boolean, BizActionObj.GetSuggestedServices);
        //            dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
        //            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
        //            if (BizActionObj.ClassID > 0)
        //                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ClassID);


        //            reader = (DbDataReader)dbServer.ExecuteReader(command);
        //            if (reader.HasRows)
        //            {
        //                if (BizActionObj.ServiceList == null)
        //                    BizActionObj.ServiceList = new List<clsServiceMasterVO>();
        //                while (reader.Read())
        //                {
        //                    clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
        //                    objServiceMasterVO.ID = (long)reader["ServiceID"];
        //                    objServiceMasterVO.TariffServiceMasterID = (long)reader["ID"];
        //                    objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
        //                    objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                    objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
        //                    objServiceMasterVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
        //                    objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
        //                    objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //                    if (BizActionObj.PatientSourceType == 2) // Camp
        //                    {
        //                        objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
        //                        objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                        objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                    }
        //                    else if (BizActionObj.PatientSourceType == 1)   //Loyalty
        //                    {
        //                        objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
        //                        objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                        objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                    }
        //                    else
        //                    {
        //                        objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
        //                        objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
        //                        objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
        //                    }

        //                    objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
        //                    objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
        //                    objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
        //                    objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
        //                    objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
        //                    objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
        //                    objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
        //                    objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
        //                    objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
        //                    objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
        //                    objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
        //                    objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
        //                    objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
        //                    objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
        //                    objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
        //                    objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
        //                    objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
        //                    objServiceMasterVO.TarrifCode = (string)DALHelper.HandleDBNull(reader["TarrifServiceCode"]);
        //                    objServiceMasterVO.TarrifName = (string)DALHelper.HandleDBNull(reader["TarrifServiceName"]);
        //                    objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //                    objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
        //                    objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
        //                    objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
        //                    objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
        //                    objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);



        //                    BizActionObj.ServiceList.Add(objServiceMasterVO);
        //                }
        //                reader.NextResult();


        //            }
        //            if (reader.IsClosed == false)
        //            {
        //                reader.Close();

        //            }

        //        }
        //        else
        //        {

        //            command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceListNew");
        //            if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
        //                dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
        //            dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
        //            dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
        //            dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
        //            dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
        //            dbServer.AddInParameter(command, "PatientSourceType", DbType.Int16, BizActionObj.PatientSourceType);
        //            dbServer.AddInParameter(command, "PatientSourceTypeID", DbType.Int64, BizActionObj.PatientSourceTypeID);
        //            dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
        //            dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
        //            dbServer.AddInParameter(command, "GetSuggestedServices", DbType.Boolean, BizActionObj.GetSuggestedServices);
        //            dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
        //            dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

        //            dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
        //            dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
        //            dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

        //            dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

        //            if (BizActionObj.ClassID > 0)
        //                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ClassID);


        //            reader = (DbDataReader)dbServer.ExecuteReader(command);



        //            if (reader.HasRows)
        //            {
        //                if (BizActionObj.ServiceList == null)
        //                    BizActionObj.ServiceList = new List<clsServiceMasterVO>();
        //                while (reader.Read())
        //                {
        //                    clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
        //                    objServiceMasterVO.ID = (long)reader["ServiceID"];
        //                    objServiceMasterVO.TariffServiceMasterID = (long)reader["ID"];
        //                    objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
        //                    objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                    objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
        //                    objServiceMasterVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
        //                    objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
        //                    objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
        //                    if (BizActionObj.PatientSourceType == 2) // Camp
        //                    {
        //                        objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
        //                        objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                        objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                    }
        //                    else if (BizActionObj.PatientSourceType == 1)   //Loyalty
        //                    {
        //                        objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
        //                        objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                        objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                    }
        //                    else
        //                    {

        //                        objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
        //                        objServiceMasterVO.SeniorCitizen = (bool)DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]);
        //                        objServiceMasterVO.SeniorCitizenConAmount = (decimal)DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]);
        //                        objServiceMasterVO.SeniorCitizenConPercent = (decimal)DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]);
        //                        objServiceMasterVO.SeniorCitizenAge = (int)DALHelper.HandleDBNull(reader["SeniorCitizenAge"]);

        //                        if (objServiceMasterVO.SeniorCitizen == true && BizActionObj.Age >= objServiceMasterVO.SeniorCitizenAge)
        //                        {
        //                            objServiceMasterVO.ConcessionAmount = objServiceMasterVO.SeniorCitizenConAmount;
        //                            objServiceMasterVO.ConcessionPercent = objServiceMasterVO.SeniorCitizenConPercent;
        //                        }
        //                        else
        //                        {
        //                            objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
        //                            objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
        //                        }

        //                    }

        //                    objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
        //                    objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
        //                    objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
        //                    objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
        //                    objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
        //                    objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
        //                    objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
        //                    objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
        //                    objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
        //                    objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
        //                    objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
        //                    objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
        //                    objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
        //                    objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
        //                    objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
        //                    objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
        //                    objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
        //                    objServiceMasterVO.TarrifCode = (string)DALHelper.HandleDBNull(reader["TarrifServiceCode"]);
        //                    objServiceMasterVO.TarrifName = (string)DALHelper.HandleDBNull(reader["TarrifServiceName"]);
        //                    objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //                    objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
        //                    objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
        //                    objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
        //                    objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
        //                    objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
        //                    objServiceMasterVO.IsPackage = (bool)DALHelper.HandleDBNull(reader["IsPackage"]);
        //                    objServiceMasterVO.PackageID = (long)DALHelper.HandleIntegerNull(reader["PackageID"]);
        //                    objServiceMasterVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
        //                    objServiceMasterVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));

        //                    BizActionObj.ServiceList.Add(objServiceMasterVO);
        //                }
        //                reader.NextResult();
        //                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

        //                reader.Close();


        //            }
        //            if (reader.IsClosed == false)
        //            {
        //                reader.Close();

        //            }
        //        }

        //    }


        //    catch (Exception ex)
        //    {
        //        throw;
        //    }


        //    finally
        //    {

        //    }
        //    return BizActionObj;
        //}
        #endregion Commented By CDS this is OLD

        # region Added By CDS for package Only
        public override IValueObject GetTariffServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceListBizActionVO BizActionObj = (clsGetTariffServiceListBizActionVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {

                if (BizActionObj.PrescribedService == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceListForPackage");  //CIMS_GetTariffServiceList

                    if (BizActionObj.UsePackageSubsql == true)
                        dbServer.AddInParameter(command, "UsePackageSubsql", DbType.Boolean, BizActionObj.UsePackageSubsql);
                    dbServer.AddInParameter(command, "ISOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);  //added by vikrant 
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows1);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                    if (BizActionObj.IsPackage == true)     // Package New Changes for Process Added on 24042018
                    {
                        //dbServer.AddInParameter(command, "IsPackage", DbType.Boolean, BizActionObj.IsPackage); 
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.ForFilterPackageID);
                    }
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceListNewForPackage3"); // CIMS_GetTariffServiceListNewForPackage2 //CIMS_GetTariffServiceListNewForPackage  //CIMS_GetTariffServiceListNew

                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                    if (BizActionObj.IsPackage == true)
                        dbServer.AddInParameter(command, "IsPackage", DbType.Boolean, BizActionObj.IsPackage);
                    else
                        dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.ForFilterPackageID);

                    //dbServer.AddOutParameter(command, "SetTariffIsPackage", DbType.Int64, int.MaxValue);

                    if (BizActionObj.UsePackageSubsql == true)
                        dbServer.AddInParameter(command, "UsePackageSubsql", DbType.Boolean, BizActionObj.UsePackageSubsql);

                    dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                    dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);
                }
                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "PatientSourceType", DbType.Int16, BizActionObj.PatientSourceType);
                dbServer.AddInParameter(command, "PatientSourceTypeID", DbType.Int64, BizActionObj.PatientSourceTypeID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "GetSuggestedServices", DbType.Boolean, BizActionObj.GetSuggestedServices);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ChargeID", DbType.Int64, BizActionObj.ChargeID);

                if (BizActionObj.SearchExpression != null && BizActionObj.SearchExpression.Length > 0)
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SearchExpression);

                if (BizActionObj.ClassID > 0)
                    dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ClassID);




                reader = (DbDataReader)dbServer.ExecuteReader(command);

                //long ServiceSetForPackage = 0;


                if (reader.HasRows)
                {

                    //ServiceSetForPackage = Convert.ToInt64(dbServer.GetParameterValue(command, "SetTariffIsPackage"));

                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();

                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = Convert.ToInt64(reader["ServiceID"]);
                        objServiceMasterVO.TariffServiceMasterID = Convert.ToInt64(reader["ID"]);
                        objServiceMasterVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objServiceMasterVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objServiceMasterVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        objServiceMasterVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objServiceMasterVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        objServiceMasterVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        if (BizActionObj.PatientSourceType == 2) // Camp
                        {
                            objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else if (BizActionObj.PatientSourceType == 1)   //Loyalty
                        {
                            objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else
                        {

                            objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));


                            objServiceMasterVO.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                            objServiceMasterVO.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                            objServiceMasterVO.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                            objServiceMasterVO.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));


                            if (objServiceMasterVO.SeniorCitizen == true && BizActionObj.Age >= objServiceMasterVO.SeniorCitizenAge)
                            {
                                objServiceMasterVO.ConcessionAmount = objServiceMasterVO.SeniorCitizenConAmount;
                                objServiceMasterVO.ConcessionPercent = objServiceMasterVO.SeniorCitizenConPercent;
                            }
                            else
                            {
                                objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));

                            }

                            //if (BizActionObj.PrescribedService == false)      // Package New Changes for Process Commented on 24042018
                            //{
                            objServiceMasterVO.PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));

                            //}

                            //if (BizActionObj.PrescribedService == false)        // Package New Changes for Process Added on 20042018
                            //{
                            objServiceMasterVO.ProcessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcessID"]));        // Package New Changes for Process Added on 23042018
                            objServiceMasterVO.ProcessName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcessName"]));   // Package New Changes for Process Added on 23042018
                            //}

                            objServiceMasterVO.AdjustableHeadType = Convert.ToInt32(DALHelper.HandleDBNull(reader["AdjustableHeadType"]));  // For Package New Changes Added on 19062018
                        }

                        objServiceMasterVO.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        objServiceMasterVO.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        objServiceMasterVO.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        objServiceMasterVO.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        objServiceMasterVO.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        objServiceMasterVO.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        objServiceMasterVO.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        objServiceMasterVO.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        objServiceMasterVO.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        objServiceMasterVO.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        objServiceMasterVO.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        objServiceMasterVO.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                        objServiceMasterVO.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        objServiceMasterVO.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        objServiceMasterVO.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        objServiceMasterVO.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        objServiceMasterVO.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        objServiceMasterVO.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        objServiceMasterVO.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));
                        objServiceMasterVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        objServiceMasterVO.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                        objServiceMasterVO.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        objServiceMasterVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        objServiceMasterVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        objServiceMasterVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"]));
                        objServiceMasterVO.IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"]));



                        if (BizActionObj.PrescribedService == true && BizActionObj.ForFilterPackageID == 0)
                        {
                            objServiceMasterVO.Billed = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Billed"])); 
                            objServiceMasterVO.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                            objServiceMasterVO.PrescriptionDetailsID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PreDetailsID"]));
                            objServiceMasterVO.InvestigationDetailsID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["InvDetailsID"]));
                            objServiceMasterVO.InvestigationBilled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["InvBilled"]));

                        }



                        //if (BizActionObj.PrescribedService == false && BizActionObj.ForFilterPackageID > 0)   // Package New Changes for Process Commented on 24042018
                        if (BizActionObj.ForFilterPackageID > 0)    // Package New Changes for Process Added on 24042018
                        {
                            objServiceMasterVO.ServiceComponentRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceComponentRate"]));
                            objServiceMasterVO.IsAdjustableHead = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAdjustableHead"]));
                            objServiceMasterVO.IsConsiderAdjustable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsiderAdjustable"]));

                            objServiceMasterVO.SumOfExludedServices = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SumOfExludedServices"]));
                        }

                        //objServiceMasterVO.TariffIsPackage = ServiceSetForPackage;  // Convert.ToInt64(dbServer.GetParameterValue(command, "SetTariffIsPackage"));

                        //if (objServiceMasterVO.TariffIsPackage > 0 )  //== "Package")
                        //{
                        if (BizActionObj.UsePackageSubsql == true)
                        {
                            objServiceMasterVO.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                            objServiceMasterVO.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));

                            // to set service background color to identify that this service is having Package Conditional Services
                            objServiceMasterVO.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageServiceConditionID"]));
                        }
                        //}
                        if (BizActionObj.PrescribedService == true)
                        {
                            if (BizActionObj.IsOPDIPD)
                            {
                                if (BizActionObj.GetSuggestedServices == true)
                                {
                                    objServiceMasterVO.RoundDate = Convert.ToDateTime(DALHelper.HandleDate(reader["RoundDate"]));
                                    objServiceMasterVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["drName"]));
                                    objServiceMasterVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["RoundSpecialization"]));
                                }
                            }
                            else
                            {
                                objServiceMasterVO.RoundDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));

                            }
                            if (Convert.ToInt64(DALHelper.HandleDBNull(reader["Isbilled"])) == 0)
                            {
                                objServiceMasterVO.IsBilledEMR = false;
                            }
                            else
                            {
                                objServiceMasterVO.IsBilledEMR = true;
                            }
                        }
                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }

                    if (BizActionObj.PrescribedService == true)
                    {
                        //if (!BizActionObj.IsOPDIPD)
                        //{
                        reader.NextResult();
                        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                        //}
                    }

                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            long i = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PatientId"]));
                        }
                    }


                    if (BizActionObj.PrescribedService == false)
                    {
                        reader.NextResult();
                        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    }

                    reader.Close();

                }
                if (reader.IsClosed == false)
                {
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
        #endregion  Added By CDS for package Only

        public override IValueObject GetAdmissionTypeTariffServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAdmissionTypeTariffServiceListBizActionVO BizActionObj = (clsGetAdmissionTypeTariffServiceListBizActionVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceAdmissionTypeList");   //CIMS_GetTariffServiceListNew
                //if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                //    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                //dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                //dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, BizActionObj.AdmissionTypeID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ClassID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = (long)reader["ServiceID"];
                        objServiceMasterVO.TariffServiceMasterID = (long)reader["ID"];
                        objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        objServiceMasterVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
                        objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        if (BizActionObj.PatientSourceType == 2) // Camp
                        {
                            objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
                            objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else if (BizActionObj.PatientSourceType == 1)   //Loyalty
                        {
                            objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
                            objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else
                        {

                            objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                            objServiceMasterVO.SeniorCitizen = (bool)DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]);
                            objServiceMasterVO.SeniorCitizenConAmount = (decimal)DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]);
                            objServiceMasterVO.SeniorCitizenConPercent = (decimal)DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]);
                            objServiceMasterVO.SeniorCitizenAge = (int)DALHelper.HandleDBNull(reader["SeniorCitizenAge"]);

                            if (objServiceMasterVO.SeniorCitizen == true && BizActionObj.Age >= objServiceMasterVO.SeniorCitizenAge)
                            {
                                objServiceMasterVO.ConcessionAmount = objServiceMasterVO.SeniorCitizenConAmount;
                                objServiceMasterVO.ConcessionPercent = objServiceMasterVO.SeniorCitizenConPercent;
                            }
                            else
                            {
                                objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                                objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                            }
                        }
                        objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
                        objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                        objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                        objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                        objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
                        objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
                        objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
                        objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
                        objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                        objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                        objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
                        objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
                        objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                        objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                        objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
                        objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
                        objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
                        objServiceMasterVO.TarrifCode = (string)DALHelper.HandleDBNull(reader["TarrifServiceCode"]);
                        objServiceMasterVO.TarrifName = (string)DALHelper.HandleDBNull(reader["TarrifServiceName"]);
                        objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                        objServiceMasterVO.IsPackage = (bool)DALHelper.HandleDBNull(reader["IsPackage"]);
                        objServiceMasterVO.PackageID = (long)DALHelper.HandleIntegerNull(reader["PackageID"]);
                        objServiceMasterVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                        objServiceMasterVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));

                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    //reader.NextResult();
                    //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                    reader.Close();
                }
                //if (reader.IsClosed == false)
                //{
                //    reader.Close();
                //}
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
        // End Block 

        public override IValueObject GetTariffServiceMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceMasterListBizActionVO BizActionObj = (clsGetTariffServiceMasterListBizActionVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceMasterList");
                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                //add by akshays
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, BizActionObj.TotalRows);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //closed by akshays
                //dbServer.AddInParameter(command, "PatientSourceType", DbType.Int16, BizActionObj.PatientSourceType);
                //dbServer.AddInParameter(command, "PatientSourceTypeID", DbType.Int64, BizActionObj.PatientSourceTypeID);
                //dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = (long)reader["ServiceID"];
                        objServiceMasterVO.TariffServiceMasterID = (long)reader["ID"];
                        objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        objServiceMasterVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
                        objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        //if (BizActionObj.PatientSourceType == 2) // Camp
                        //{
                        //    objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
                        //    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        //    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        //}
                        //else if (BizActionObj.PatientSourceType == 1)   //Loyalty
                        //{
                        //    objServiceMasterVO.Rate = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["Rate"]));
                        //    objServiceMasterVO.ConcessionAmount = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        //    objServiceMasterVO.ConcessionPercent = Convert.ToDecimal((double)DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        //}
                        //else
                        //{
                        objServiceMasterVO.BaseServiceRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseServiceRate"]));
                        objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        objServiceMasterVO.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                        objServiceMasterVO.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                        //}

                        objServiceMasterVO.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
                        objServiceMasterVO.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                        objServiceMasterVO.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                        objServiceMasterVO.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                        objServiceMasterVO.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
                        objServiceMasterVO.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
                        objServiceMasterVO.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
                        objServiceMasterVO.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
                        objServiceMasterVO.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                        objServiceMasterVO.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                        objServiceMasterVO.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
                        objServiceMasterVO.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
                        objServiceMasterVO.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                        objServiceMasterVO.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                        objServiceMasterVO.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
                        objServiceMasterVO.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
                        objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
                        objServiceMasterVO.TarrifCode = (string)DALHelper.HandleDBNull(reader["TarrifServiceCode"]);
                        objServiceMasterVO.TarrifName = (string)DALHelper.HandleDBNull(reader["TariffName"]);
                        objServiceMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                        //objServiceMasterVO.TariffName = (string)DALHelper.HandleDBNull(reader["TariffName"]);
                        objServiceMasterVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));



                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
                    //BizActionObj.TotalRows = (Int32)dbServer.GetParameterValue(command, "TotalRows");
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");



                }
                if (reader.IsClosed == false)
                {
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

        public override IValueObject UpdateServiceMasterStatus(IValueObject valueObject, clsUserVO userVO)
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

        public override IValueObject ChangeTariffServiceStatus(IValueObject valueObject, clsUserVO userVO)
        {
            clsChangeTariffServiceStatusBizActionVO objItem = valueObject as clsChangeTariffServiceStatusBizActionVO;

            try
            {
                DbCommand command = null;

                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;
                command = dbServer.GetStoredProcCommand("CIMS_ChangeTariffServiceStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Int32, objItem.SuccessStatus);

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

        #region Added byShikha

        public override IValueObject AddUpdateSpecialization(IValueObject valueObject, clsUserVO userVO)
        {
            clsSpecializationVO objItemVO = new clsSpecializationVO();
            clsAddUpdateSpecializationBizActionVO objItem = valueObject as clsAddUpdateSpecializationBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpecialization");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.SpecializationId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.ClinicId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.SpecializationName);
                dbServer.AddInParameter(command, "IsGenerateToken", DbType.Boolean, objItemVO.IsGenerateToken);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }

        public override IValueObject GetSpecializationDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            //DbDataReader reader = null;
            //clsGetSpecializationDetailsBizActionVO objItem = valueObject as clsGetSpecializationDetailsBizActionVO;
            //clsSpecializationVO objItemVO = null;
            //try
            //{
            //    DbCommand command;
            //    command = dbServer.GetStoredProcCommand("CIMS_GetSpecializationDetails");
            //    dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SerachExpression);
            //    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
            //    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
            //    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
            //    dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

            //    reader = (DbDataReader)dbServer.ExecuteReader(command);
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            objItemVO = new clsSpecializationVO();
            //            objItemVO.SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
            //            objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
            //            objItemVO.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
            //            objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
            //            objItem.ItemMatserDetails.Add(objItemVO);
            //        }
            //    }
            //    reader.NextResult();
            //    objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //finally
            //{
            //    if (reader != null)
            //    {
            //        if (reader.IsClosed == false)
            //            reader.Close();
            //    }
            //}
            //return objItem;


            DbDataReader reader = null;
            clsGetSpecializationDetailsBizActionVO objItem = valueObject as clsGetSpecializationDetailsBizActionVO;
            clsSpecializationVO objItemVO = null;
            if (objItem.IsFromAgency == true)
            {
                #region Fill ComboBox For Agency Service Link
                try
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillSpecializationComboForAgencyServiceLink");
                    dbServer.AddInParameter(command, "AgencyID", DbType.Int64, objItem.AgencyID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (objItem.MasterList == null)
                        {
                            objItem.MasterList = new List<MasterListItem>();
                        }
                        while (reader.Read())
                        {
                            objItem.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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
                #endregion
            }
            else
            {
                #region FillOn Master
                try
                {
                    DbCommand command;
                    command = dbServer.GetStoredProcCommand("CIMS_GetSpecializationDetails");
                    dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SerachExpression);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItemVO = new clsSpecializationVO();
                            objItemVO.SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                            objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            objItemVO.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            objItemVO.IsGenerateToken = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGenerateToken"]));
                            objItemVO.SubSpeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpeID"]));
                            objItem.ItemMatserDetails.Add(objItemVO);
                        }
                    }
                    reader.NextResult();
                    objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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
                #endregion
            }
            return objItem;
        }




        public override IValueObject AddUpdateSubSpecialization(IValueObject valueObject, clsUserVO userVO)
        {
            clsSubSpecializationVO objItemVO = new clsSubSpecializationVO();
            clsAddUpadateSubSpecializationBizActionVO objItem = valueObject as clsAddUpadateSubSpecializationBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSubSpecialization");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.SubSpecializationId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.ClinicId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.SubSpecializationName);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, objItemVO.SpecializationId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }

        public override IValueObject GetSubSpecializationDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetSubSpecializationDetailsBizActionVO objItem = valueObject as clsGetSubSpecializationDetailsBizActionVO;
            clsSubSpecializationVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetSubSpecializationDetails");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsSubSpecializationVO();
                        objItemVO.SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objItemVO.SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["fkSpecializationID"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specilaization"]));
                        objItemVO.SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItem.ItemMatserDetails.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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


        public override IValueObject AddUpdateCompanyDetails(IValueObject valueObject, clsUserVO userVO)
        {


            clsAddUpdateCompanyDetailsBizActionVO BizActionobj = valueObject as clsAddUpdateCompanyDetailsBizActionVO;


            if (BizActionobj.ItemMatserDetails.Id == 0)
            {
                BizActionobj = AddCompanyMaster(BizActionobj, userVO);
            }

            else
            {
                BizActionobj = UpdateCompanyMaster(BizActionobj, userVO);

            }

            return BizActionobj;



        }


        private clsAddUpdateCompanyDetailsBizActionVO AddCompanyMaster(clsAddUpdateCompanyDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsCompanyVO objItemVO = BizActionObj.ItemMatserDetails;

                //command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCompanyDetails");
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCompanyMaster");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "CompanyTypeId", DbType.Int64, objItemVO.CompanyTypeId);
                //ROHINEE
                dbServer.AddInParameter(command, "PatientCatagoryID", DbType.Int64, objItemVO.PatientCatagoryID);
                //
                dbServer.AddInParameter(command, "ContactPerson", DbType.String, objItemVO.ContactPerson);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, objItemVO.ContactNo);
                dbServer.AddInParameter(command, "EmailId", DbType.String, objItemVO.CompanyEmail);
                dbServer.AddInParameter(command, "Address", DbType.String, objItemVO.CompanyAddress);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);


                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.Id);
                int intStatus2 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.ItemMatserDetails.Id = (long)dbServer.GetParameterValue(command, "ID");



                foreach (var ObjTariff in objItemVO.TariffDetails)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCompanyTariffDetails");

                    dbServer.AddInParameter(command1, "CompanyID", DbType.Int64, objItemVO.Id);
                    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjTariff.TariffID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true); //ObjTariff.Status);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTariff.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjTariff.ID = (long)dbServer.GetParameterValue(command1, "ID");
                }
                // Added by Ashish Z. for Company Logo and other
                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddCompanyLogoDetails");
                dbServer.AddInParameter(command2, "CompanyID", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command2, "CompanyLogoFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFileContent);
                dbServer.AddInParameter(command2, "LogoFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFileName);
                dbServer.AddInParameter(command2, "Title", DbType.String, BizActionObj.ItemMatserDetails.Title);

                dbServer.AddInParameter(command2, "CompHeadImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedHeadImgFileContent);
                dbServer.AddInParameter(command2, "HeadImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedHeadImgFileName);
                dbServer.AddInParameter(command2, "TitleHeadImg", DbType.String, BizActionObj.ItemMatserDetails.TitleHeaderImage);

                dbServer.AddInParameter(command2, "CompFootImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFootImgFileContent);
                dbServer.AddInParameter(command2, "FootImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFootImgFileName);
                dbServer.AddInParameter(command2, "TitleFootImg", DbType.String, BizActionObj.ItemMatserDetails.TitleFooterImage);

                dbServer.AddInParameter(command2, "HeaderText", DbType.String, BizActionObj.ItemMatserDetails.HeaderText);
                dbServer.AddInParameter(command2, "FooterText", DbType.String, BizActionObj.ItemMatserDetails.FooterText);

                dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                int intStatus = dbServer.ExecuteNonQuery(command2, trans);
                //

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.ItemMatserDetails = null;



            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }


        private clsAddUpdateCompanyDetailsBizActionVO UpdateCompanyMaster(clsAddUpdateCompanyDetailsBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsCompanyVO objItemVO = BizActionObj.ItemMatserDetails;

                //command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCompanyDetails");
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCompanyMaster");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);

                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "CompanyTypeId", DbType.Int64, objItemVO.CompanyTypeId);

                //ROHINEE
                dbServer.AddInParameter(command, "PatientCatagoryID", DbType.Int64, objItemVO.PatientCatagoryID);
                //

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "ContactPerson", DbType.String, objItemVO.ContactPerson);
                dbServer.AddInParameter(command, "ContactNo", DbType.String, objItemVO.ContactNo);
                dbServer.AddInParameter(command, "EmailId", DbType.String, objItemVO.CompanyEmail);
                dbServer.AddInParameter(command, "Address", DbType.String, objItemVO.CompanyAddress);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (objItemVO.TariffDetails != null && objItemVO.TariffDetails.Count > 0)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteCompanyTariffDetails");

                    dbServer.AddInParameter(command2, "ID", DbType.Int64, objItemVO.Id);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                }


                foreach (var ObjTariff in objItemVO.TariffDetails)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCompanyTariffDetails");

                    dbServer.AddInParameter(command1, "CompanyID", DbType.Int64, objItemVO.Id);
                    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjTariff.TariffID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);// ObjTariff.Status);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTariff.ID);
                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjTariff.ID = (long)dbServer.GetParameterValue(command1, "ID");


                }

                // Added by Ashish Z. for Company Logo
                DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateCompanyLogoDetails");
                dbServer.AddInParameter(command3, "CompanyID", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command3, "CompanyLogoFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFileContent);
                dbServer.AddInParameter(command3, "LogoFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFileName);
                dbServer.AddInParameter(command3, "Title", DbType.String, BizActionObj.ItemMatserDetails.Title);

                dbServer.AddInParameter(command3, "CompHeadImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedHeadImgFileContent);
                dbServer.AddInParameter(command3, "HeadImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedHeadImgFileName);
                dbServer.AddInParameter(command3, "TitleHeadImg", DbType.String, BizActionObj.ItemMatserDetails.TitleHeaderImage);

                dbServer.AddInParameter(command3, "CompFootImgFile", DbType.Binary, BizActionObj.ItemMatserDetails.AttachedFootImgFileContent);
                dbServer.AddInParameter(command3, "FootImgFileName", DbType.String, BizActionObj.ItemMatserDetails.AttachedFootImgFileName);
                dbServer.AddInParameter(command3, "TitleFootImg", DbType.String, BizActionObj.ItemMatserDetails.TitleFooterImage);

                dbServer.AddInParameter(command3, "HeaderText", DbType.String, BizActionObj.ItemMatserDetails.HeaderText);
                dbServer.AddInParameter(command3, "FooterText", DbType.String, BizActionObj.ItemMatserDetails.FooterText);

                dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command3, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                //



                trans.Commit();
                BizActionObj.SuccessStatus = 0;




            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.ItemMatserDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }




        public override IValueObject GetCompanyDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetCompanyDetailsBizActionVO objItem = valueObject as clsGetCompanyDetailsBizActionVO;
            clsCompanyVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetCompanyDetails");
                //command = dbServer.GetStoredProcCommand("temp_CIMS_GetCompanyDetails"); // for testing Purpose..

                dbServer.AddInParameter(command, "Id", DbType.Int64, objItem.CompanyId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
                if (objItem.Description != null && objItem.Description.Length > 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, objItem.Description);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objItem.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.TotalRows);

                if (objItem.CompanyId == 0) // for getting the Company List.
                {
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItemVO = new clsCompanyVO();
                            objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objItemVO.CompanyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyTypeId"]));

                            objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            objItemVO.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["CompanyType"]));
                            objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                            //added by Ashish Z. for company logo.
                            objItemVO.ContactPerson = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson"]));
                            objItemVO.ContactNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContactNo"]));
                            objItemVO.CompanyEmail = Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"]));
                            objItemVO.CompanyAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));

                            objItem.ItemMatserDetails.Add(objItemVO);
                        }
                    }

                    reader.NextResult();
                    objItem.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                else if (objItem.CompanyId > 0) //for getting the Company Details by CompanyId.
                {
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItemVO = new clsCompanyVO();
                            objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objItemVO.CompanyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyTypeId"]));

                            objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            objItemVO.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["CompanyType"]));
                            objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                            //ROHINEE
                            objItemVO.PatientCatagoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceID"]));
                            objItemVO.CompanyCategory = Convert.ToString(DALHelper.HandleDBNull(reader["CompanyCategory"]));
                            //
                            //added by Ashish Z. for company logo and other.
                            objItemVO.ContactPerson = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson"]));
                            objItemVO.ContactNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContactNo"]));
                            objItemVO.CompanyEmail = Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"]));
                            objItemVO.CompanyAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));

                            objItemVO.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                            //objItemVO.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["LogoFileName"]));
                            objItemVO.AttachedFileContent = (Byte[])DALHelper.HandleDBNull(reader["CompanyLogoFile"]);

                            objItemVO.TitleHeaderImage = Convert.ToString(DALHelper.HandleDBNull(reader["TitleHeadImg"]));
                            //objItemVO.AttachedHeadImgFileName = Convert.ToString(DALHelper.HandleDBNull(reader["HeadImgFileName"]));
                            objItemVO.AttachedHeadImgFileContent = (Byte[])DALHelper.HandleDBNull(reader["CompHeadImgFile"]);

                            objItemVO.TitleFooterImage = Convert.ToString(DALHelper.HandleDBNull(reader["TitleFootImg"]));
                            //objItemVO.AttachedFootImgFileName = Convert.ToString(DALHelper.HandleDBNull(reader["FootImgFileName"]));
                            objItemVO.AttachedFootImgFileContent = (Byte[])DALHelper.HandleDBNull(reader["CompFootImgFile"]);

                            objItemVO.HeaderText = Convert.ToString(DALHelper.HandleDBNull(reader["HeaderText"]));
                            objItemVO.FooterText = Convert.ToString(DALHelper.HandleDBNull(reader["FooterText"]));
                            //

                            objItem.ItemMatserDetails.Add(objItemVO);
                        }
                    }
                    reader.NextResult();

                    if (reader.HasRows)
                    {
                        objItem.TariffDetails = new List<clsTariffDetailsVO>();
                        while (reader.Read())
                        {
                            clsTariffDetailsVO objTariffDetailsVO = new clsTariffDetailsVO();
                            objTariffDetailsVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                            objTariffDetailsVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                            objTariffDetailsVO.TariffDescription = (string)DALHelper.HandleDBNull(reader["Description"]);
                            objTariffDetailsVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                            objItem.TariffDetails.Add(objTariffDetailsVO);
                        }
                    }
                    reader.NextResult();
                    objItem.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }

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

        public override IValueObject AddUpdateCompanyAssociate(IValueObject valueObject, clsUserVO userVO)
        {




            clsCompanyAssociateVO objItemVO = new clsCompanyAssociateVO();
            clsAddUpdateCompanyAssociateBizActionVO objItem = valueObject as clsAddUpdateCompanyAssociateBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateCompanyAssociateDetails");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objItemVO.CompanyId);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objItemVO.TariffId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }


        public override IValueObject GetCompanyAssociateDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetCompanyAssociateDetailsBizActionVO objItem = valueObject as clsGetCompanyAssociateDetailsBizActionVO;
            clsCompanyAssociateVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetCompanyAssociateDetails");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsCompanyAssociateVO();
                        objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.CompanyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CompanyID"]));
                        objItemVO.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.CompanyName = Convert.ToString(DALHelper.HandleDBNull(reader["Compnay"]));
                        objItemVO.Tariff = Convert.ToString(DALHelper.HandleDBNull(reader["Tariff"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItem.ItemMatserDetails.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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

        public override IValueObject AddUpdateTariff(IValueObject valueObject, clsUserVO userVO)
        {
            clsTariffVO objItemVO = new clsTariffVO();
            clsAddUpdateTariffBizActionVO objItem = valueObject as clsAddUpdateTariffBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateTariffDetails");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }

        public override IValueObject GetTariffDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetTariffDetailsBizActionVO objItem = valueObject as clsGetTariffDetailsBizActionVO;
            clsTariffVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetTariffDetails");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsTariffVO();
                        objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItem.ItemMatserDetails.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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

        public override IValueObject AddUpdateDepartmentMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsDepartmentVO objItemVO = new clsDepartmentVO();
            clsAddUpdateDepartmentMasterBizActionVO objItem = valueObject as clsAddUpdateDepartmentMasterBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDepartmentMaster");


                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);
                dbServer.AddInParameter(command, "IsClinical", DbType.Boolean, objItemVO.IsClinical);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, objItemVO.AddUnitID);

                dbServer.AddInParameter(command, "By", DbType.Int64, objItemVO.By);
                dbServer.AddInParameter(command, "On", DbType.String, objItemVO.On);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objItemVO.DateTime);
                dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, objItemVO.WindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddOutParameter(command, "OutPutID", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

                //Added Only For IPD 

                if (objItem.IsUpdate == true && objItem.SuccessStatus == 1)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteDepartmentSpecializationDetails");
                    dbServer.AddInParameter(command2, "DeptID", DbType.Int64, objItemVO.Id);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2);
                }

                if (objItem.SuccessStatus == 1)
                {
                    foreach (var item in objItemVO.SpecilizationList)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateDeptSpecList");
                        dbServer.AddInParameter(command3, "Id", DbType.Int64, 0);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objItemVO.UnitId);
                        dbServer.AddInParameter(command3, "DeptID", DbType.Int64, objItemVO.Id);
                        dbServer.AddInParameter(command3, "SpecializationID", DbType.Int64, item.SpecializationId);
                        dbServer.AddInParameter(command3, "SubSpecializationId", DbType.Int64, item.SubSpecializationId);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, item.Status);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, int.MaxValue);
                        intStatus = dbServer.ExecuteNonQuery(command3);
                        objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command3, "ResultStatus"));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }

        public override IValueObject GetDepartmentMasterDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetDepartmentMasterDetailsBizActionVO objItem = valueObject as clsGetDepartmentMasterDetailsBizActionVO;
            clsDepartmentVO objItemVO = null;
            clsSubSpecializationVO SpecializationDetails = null;
            objItem.DeptSpecializationDetails.SpecilizationList = new List<clsSubSpecializationVO>();
            try
            {
                DbCommand command;
                if (objItem.DeptSpecializationDetails.Id > 0)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetDepatmentMasterDetailsByID");
                    dbServer.AddInParameter(command, "DeptID", DbType.Int64, objItem.DeptSpecializationDetails.Id);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            SpecializationDetails = new clsSubSpecializationVO();
                            SpecializationDetails.SpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                            SpecializationDetails.SubSpecializationId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                            SpecializationDetails.SpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                            SpecializationDetails.SubSpecializationName = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                            SpecializationDetails.Status = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Status"]));
                            objItem.DeptSpecializationDetails.SpecilizationList.Add(SpecializationDetails);
                        }
                    }

                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetDepatmentMasterDetails");
                    dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objItemVO = new clsDepartmentVO();
                            objItemVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            objItemVO.IsClinical = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClinical"]));
                            if (objItemVO.IsClinical == true)
                            {
                                objItemVO.ClinicalStatus = "Yes";
                            }
                            else
                            {
                                objItemVO.ClinicalStatus = "No";
                            }
                            objItem.ItemMatserDetails.Add(objItemVO);
                        }
                    }
                    reader.NextResult();
                    objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
                }
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

        #endregion Added byShikha

        /// <summary>
        /// Adds ServiceClassRateDetails
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="userVO"></param>
        /// <returns></returns>
        public override IValueObject AddServiceTariff(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddServiceTariffBizActionVO objItem = valueObject as clsAddServiceTariffBizActionVO;
            try
            {
                //    DbCommand command = null;
                DbCommand command1 = null;
                clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;

                objItemVO.Query = "";
                //string Query1 = "";
                //if (objItem.ServiceMasterDetails.EditMode == false)
                //{
                //List<long> tariffIds;
                //tariffIds = GetServiceTarrifIds(objItem.ServiceMasterDetails.ServiceID);
                int queryStatus = 0;
                //command = dbServer.GetStoredProcCommand("CIMS_AddServiceTariffMaster");





                //objItemVO.Query = objItemVO.Query + " Insert Into T_ServiceTariffMaster (ServiceId,TariffId,Status) values ( " + objItem.ServiceMasterDetails.ServiceID + "," + objItem.ServiceMasterDetails.TariffIDList[i] + "," + status + " )  ";
                //command.Parameters.Clear();

                //dbServer.AddInParameter(command, "ServiceId", DbType.Int64, objItem.ServiceMasterDetails.ServiceID);
                //dbServer.AddInParameter(command, "TariffId", DbType.Int64, objItem.ServiceMasterDetails.TariffIDList[i]);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "Status", DbType.Boolean, status);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                ////dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.ServiceMasterDetails.ID);
                //int intStatus2 = dbServer.ExecuteNonQuery(command);
                //objItem.ServiceMasterDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                command1 = dbServer.GetStoredProcCommand("CIMS_AddServiceClassRateDetails");
                command1.Parameters.Clear();
                dbServer.AddInParameter(command1, "Id", DbType.Int64, 0);
                dbServer.AddInParameter(command1, "ServiceId", DbType.Int64, objItem.ServiceMasterDetails.ServiceID);
                dbServer.AddInParameter(command1, "ClassId", DbType.Int64, 1);
                dbServer.AddInParameter(command1, "Rate", DbType.Decimal, objItem.ServiceMasterDetails.Rate);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, objItem.ServiceMasterDetails.Status);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                queryStatus = dbServer.ExecuteNonQuery(command1);


                objItem.SuccessStatus = queryStatus;



            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;
        }

        public override IValueObject UpdateServiceTariff(IValueObject valueObject, clsUserVO userVO)
        {
            //clsUpdateServiceTariffMasterBizActionVO objItem = valueObject as clsUpdateServiceTariffMasterBizActionVO;
            //try
            //{
            //    DbCommand command = null;
            //    DbCommand command1 = null;
            //    clsServiceMasterVO objItemVO = objItem.ServiceMasterDetails;

            //    objItemVO.Query = "";
            //    //string Query1 = "";
            //    //if (objItem.ServiceMasterDetails.EditMode == false)
            //    //{
            //    List<long> tariffIds;
            //    tariffIds = GetServiceTarrifIds(objItem.ServiceMasterDetails.ServiceID);
            //    int queryStatus = 0;


            //   // command = dbServer.GetStoredProcCommand("CIMS_AddServiceTariffMaster");

            //    int status;



            //        //command.Parameters.Clear();

            //        //dbServer.AddInParameter(command, "ServiceId", DbType.Int64, objItem.ServiceMasterDetails.ServiceID);
            //        //dbServer.AddInParameter(command, "TariffId", DbType.Int64, tariffIds[i]);
            //        //dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
            //        //dbServer.AddInParameter(command, "Status", DbType.Boolean, status);
            //        //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
            //        ////dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItem.ServiceMasterDetails.ID);
            //        //int intStatus2 = dbServer.ExecuteNonQuery(command);

            //        command1 = dbServer.GetStoredProcCommand("CIMS_AddServiceClassRateDetails");
            //        command1.Parameters.Clear();
            //        dbServer.AddInParameter(command1, "ServiceId", DbType.Int64, objItem.ServiceMasterDetails.ServiceID);
            //        dbServer.AddInParameter(command1, "ClassId", DbType.Int64, 1);
            //        dbServer.AddInParameter(command1, "Rate", DbType.Decimal, objItem.ServiceMasterDetails.Rate);
            //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objItem.ServiceMasterDetails.Status);
            //        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
            //        queryStatus = dbServer.ExecuteNonQuery(command1);

            //        objItem.SuccessStatus =  UpdateTariffService(objItem, userVO);










            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            return valueObject;
        }

        //Added By Pallavi
        /// <summary>
        /// Required when view is clicked
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="objUserVO"></param>
        /// <returns></returns>
        public override IValueObject GetServiceMasterDetailsForId(IValueObject valueObject, clsUserVO objUserVO)
        {
            #region OLD Code Commented by CDS
            //try
            //{
            //    clsGetServiceMasterListBizActionVO BizActionObj = valueObject as clsGetServiceMasterListBizActionVO;
            //    BizActionObj.ServiceMaster = BizActionObj.ServiceMaster;
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceMasterDetails");
            //    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ID);
            //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
            //    //dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
            //    DbDataReader reader;
            //    // dbServer.AddInParameter(command, "SearchExpression", DbType.String, Security.base64Encode(BizActionObj.SearchExpression));
            //    reader = (DbDataReader)dbServer.ExecuteReader(command);
            //    while (reader.Read())
            //    {
            //        BizActionObj.ServiceMaster.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
            //        BizActionObj.ServiceMaster.CodeType = (long)DALHelper.HandleDBNull(reader["CodeType"]);
            //        BizActionObj.ServiceMaster.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
            //        BizActionObj.ServiceMaster.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
            //        BizActionObj.ServiceMaster.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
            //        BizActionObj.ServiceMaster.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
            //        BizActionObj.ServiceMaster.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
            //        BizActionObj.ServiceMaster.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
            //        BizActionObj.ServiceMaster.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
            //        BizActionObj.ServiceMaster.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
            //        BizActionObj.ServiceMaster.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
            //        BizActionObj.ServiceMaster.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
            //        BizActionObj.ServiceMaster.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
            //        BizActionObj.ServiceMaster.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
            //        //BizActionObj.ServiceMaster.GeneralDiscount = (bool)DALHelper.HandleDBNull(reader["GeneralDiscount"]);
            //        BizActionObj.ServiceMaster.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
            //        BizActionObj.ServiceMaster.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
            //        BizActionObj.ServiceMaster.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
            //        BizActionObj.ServiceMaster.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
            //        BizActionObj.ServiceMaster.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
            //        BizActionObj.ServiceMaster.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
            //        //BizActionObj.ServiceMaster.OutSource = (bool)DALHelper.HandleDBNull(reader["OutSource"]);
            //        BizActionObj.ServiceMaster.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
            //        BizActionObj.ServiceMaster.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
            //        BizActionObj.ServiceMaster.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
            //        BizActionObj.ServiceMaster.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
            //        BizActionObj.ServiceMaster.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
            //        BizActionObj.ServiceMaster.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
            //        BizActionObj.ServiceMaster.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
            //        BizActionObj.ServiceMaster.CheckedAllTariffs = (bool)DALHelper.HandleDBNull(reader["CheckedAllTariffs"]);
            //        BizActionObj.ServiceMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
            //        BizActionObj.ServiceMaster.ServiceName = BizActionObj.ServiceMaster.Description;

            //        BizActionObj.ServiceMaster.IsPackage = (bool)DALHelper.HandleDBNull(reader["IsPackage"]);

            //    }
            //    reader.Close();

            //    return BizActionObj;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            #endregion

            // Added for IPD  by CDS 
            try
            {
                clsGetServiceMasterListBizActionVO BizActionObj = valueObject as clsGetServiceMasterListBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAllServiceMasterDetails");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceMaster.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ServiceMaster.UnitID);//objUserVO.UserLoginInfo.UnitId);
                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                while (reader.Read())
                {
                    BizActionObj.ServiceMaster.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                    BizActionObj.ServiceMaster.CodeType = (long)DALHelper.HandleDBNull(reader["CodeType"]);
                    BizActionObj.ServiceMaster.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                    BizActionObj.ServiceMaster.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                    BizActionObj.ServiceMaster.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                    BizActionObj.ServiceMaster.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                    BizActionObj.ServiceMaster.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                    BizActionObj.ServiceMaster.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                    BizActionObj.ServiceMaster.StaffDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDiscount"]);
                    BizActionObj.ServiceMaster.StaffDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountAmount"]);
                    BizActionObj.ServiceMaster.StaffDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDiscountPercent"]);
                    BizActionObj.ServiceMaster.StaffDependantDiscount = (bool)DALHelper.HandleDBNull(reader["StaffDependantDiscount"]);
                    BizActionObj.ServiceMaster.StaffDependantDiscountAmount = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]);
                    BizActionObj.ServiceMaster.StaffDependantDiscountPercent = (decimal)DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]);
                    BizActionObj.ServiceMaster.Concession = (bool)DALHelper.HandleDBNull(reader["Concession"]);
                    BizActionObj.ServiceMaster.ConcessionAmount = (decimal)DALHelper.HandleDBNull(reader["ConcessionAmount"]);
                    BizActionObj.ServiceMaster.ConcessionPercent = (decimal)DALHelper.HandleDBNull(reader["ConcessionPercent"]);
                    BizActionObj.ServiceMaster.ServiceTax = (bool)DALHelper.HandleDBNull(reader["ServiceTax"]);
                    BizActionObj.ServiceMaster.ServiceTaxAmount = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxAmount"]);
                    BizActionObj.ServiceMaster.ServiceTaxPercent = (decimal)DALHelper.HandleDBNull(reader["ServiceTaxPercent"]);
                    BizActionObj.ServiceMaster.InHouse = (bool)DALHelper.HandleDBNull(reader["InHouse"]);
                    BizActionObj.ServiceMaster.DoctorShare = (bool)DALHelper.HandleDBNull(reader["DoctorShare"]);
                    BizActionObj.ServiceMaster.DoctorSharePercentage = (decimal)DALHelper.HandleDBNull(reader["DoctorSharePercentage"]);
                    BizActionObj.ServiceMaster.DoctorShareAmount = (decimal)DALHelper.HandleDBNull(reader["DoctorShareAmount"]);
                    BizActionObj.ServiceMaster.RateEditable = (bool)DALHelper.HandleDBNull(reader["RateEditable"]);
                    BizActionObj.ServiceMaster.MaxRate = (decimal)DALHelper.HandleDBNull(reader["MaxRate"]);
                    BizActionObj.ServiceMaster.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
                    BizActionObj.ServiceMaster.CheckedAllTariffs = (bool)DALHelper.HandleDBNull(reader["CheckedAllTariffs"]);
                    BizActionObj.ServiceMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                    BizActionObj.ServiceMaster.ServiceName = BizActionObj.ServiceMaster.Description;
                    BizActionObj.ServiceMaster.IsPackage = (bool)DALHelper.HandleDBNull(reader["IsPackage"]);
                    BizActionObj.ServiceMaster.IsFavourite = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFavourite"]));
                    BizActionObj.ServiceMaster.IsLinkWithInventory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IslinkWithInventory"]));
                    BizActionObj.ServiceMaster.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                    BizActionObj.ServiceMaster.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                    BizActionObj.ServiceMaster.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                    BizActionObj.ServiceMaster.SeniorCitizenAge = Convert.ToInt16(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));
                    //by Anjali...................
                    BizActionObj.ServiceMaster.LuxuryTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LuxuryTaxAmount"]));
                    BizActionObj.ServiceMaster.LuxuryTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["LuxuryTaxPercent"]));

                    BizActionObj.ServiceMaster.SACCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SACCodeID"]));  // for GST Details 27062017

                    //.........................
                    if (BizActionObj.IsOLDServiceMaster == false)
                    {
                        BizActionObj.ServiceMaster.CodeDetails = Convert.ToString(DALHelper.HandleDBNull(reader["CodeDetails"]));
                        BizActionObj.ServiceMaster.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseServiceRate"]));
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsServiceMasterVO objService = new clsServiceMasterVO();
                                objService.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                                objService.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                                objService.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                                objService.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                                objService.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                                BizActionObj.ServiceList.Add(objService);
                            }
                        }
                    }
                }
                reader.Close();
                return BizActionObj;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public override IValueObject GetServiceTariffClassList(IValueObject valueObject, clsUserVO objUserVO)
        {
            //throw new NotImplementedException();

            clsGetTariffServiceClassBizActionVO BizActionObj = (clsGetTariffServiceClassBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceClassList");


                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "TariffID ", DbType.Int64, BizActionObj.TariffID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ClassList == null)
                        BizActionObj.ClassList = new List<clsServiceTarrifClassRateDetailsVO>();
                    while (reader.Read())
                    {
                        clsServiceTarrifClassRateDetailsVO objServiceMasterVO = new clsServiceTarrifClassRateDetailsVO();
                        objServiceMasterVO.ClassID = (long)reader["ID"];
                        objServiceMasterVO.ClassName = (string)DALHelper.HandleDBNull(reader["ClassName"]);
                        objServiceMasterVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //objServiceMasterVO.ServiceCode = (string)DALHelper.HandleDBNull(reader["ServiceCode"]);
                        //objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        //objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        //objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        //objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objServiceMasterVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);


                        BizActionObj.ClassList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
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

        //Added By Somnath
        public override IValueObject GetBankBranchDetails(IValueObject valueObject, clsUserVO objUserVO)
        {

            DbDataReader reader = null;
            clsGetBankBranchDetailsBizActionVO objItem = valueObject as clsGetBankBranchDetailsBizActionVO;
            clsBankBranchVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBankBranchDetails");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsBankBranchVO();
                        objItemVO.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objItemVO.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankID"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                        objItemVO.BankName = Convert.ToString(DALHelper.HandleDBNull(reader["BankName"]));
                        objItemVO.BranchName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItem.ItemMatserDetails.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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

        public override IValueObject AddUpdateBankBranchDetails(IValueObject valueObject, clsUserVO userVO)
        {

            clsBankBranchVO objItemVO = new clsBankBranchVO();
            clsAddUpadateBAnkBranchDetailsBizActionVO objItem = valueObject as clsAddUpadateBAnkBranchDetailsBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateBankBranchDetails");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.BranchId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.ClinicId);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.BranchName);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "BankId", DbType.Int64, objItemVO.BankId);
                dbServer.AddInParameter(command, "MICRNumber", DbType.Int64, objItemVO.MICRNumber);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }


            }
            return objItem;

        }
        //End

        // Added Only For IPD by CDS

        public override IValueObject GetServiceClassRateList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceWiseClassRateBizActionVO BizActionObj = valueObject as clsGetServiceWiseClassRateBizActionVO;

            BizActionObj.ServiceClassList = new List<clsServiceMasterVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceClassRateList");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsServiceMasterVO objService = new clsServiceMasterVO();
                        objService.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        objService.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        objService.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objService.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objService.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        BizActionObj.ServiceClassList.Add(objService);
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

        public override IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetMasterForServiceBizActionVO BizActionObj = (clsGetMasterForServiceBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceMasterDetailsList");

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");


                if (BizActionObj.ServiceCode != null && BizActionObj.ServiceCode != "" && BizActionObj.ServiceCode.Length != 0)
                    dbServer.AddInParameter(command, "ServiceCode", DbType.String, "%" + BizActionObj.ServiceCode + "%");


                if (BizActionObj.Description != null && BizActionObj.Description != "" && BizActionObj.Description.Length != 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, "%" + BizActionObj.Description + "%");

                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.SpecializationID);
                dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, BizActionObj.SubSpecializationID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceDetails == null)
                        BizActionObj.ServiceDetails = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO serviceVO = new clsServiceMasterVO();
                        serviceVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        serviceVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        serviceVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        serviceVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceRate"]));
                        serviceVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        serviceVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        serviceVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        serviceVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.ServiceDetails.Add(serviceVO);
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

            #region Commented By Yogita
            //bool CurrentMethodExecutionStatus = true;

            //clsGetMasterForServiceBizActionVO BizActionObj = (clsGetMasterForServiceBizActionVO)valueObject;

            //try
            //{
            //    StringBuilder FilterExpression = new StringBuilder();

            //    //if (BizActionObj.IsActive.HasValue)
            //    //      FilterExpression.Append("IsActive = '" + BizActionObj.IsActive.Value.ToString().ToUpper() + "'");

            //    if (BizActionObj.Parent != null)
            //    {
            //        if (FilterExpression.Length > 0)
            //            FilterExpression.Append(" And ");
            //        FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
            //    }


            //    //Take storeprocedure name as input parameter and creates DbCommand Object.
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMasterList");
            //    //Adding MasterTableName as Input Parameter to filter record
            //    dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
            //    dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
            //    //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

            //    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            //    //Check whether the reader contains the records
            //    if (reader.HasRows)
            //    {
            //        //if masterlist instance is null then creates new instance
            //        if (BizActionObj.MasterList == null)
            //        {
            //            BizActionObj.MasterList = new List<MasterListItem>();
            //        }
            //        //Reading the record from reader and stores in list
            //        while (reader.Read())
            //        {
            //            //Add the object value in list
            //            BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));
            //        }
            //    }
            //    //if (!reader.IsClosed)
            //    //{
            //    //    reader.Close();
            //    //}
            //    reader.Close();

            //}
            //catch (Exception ex)
            //{
            //    CurrentMethodExecutionStatus = false;
            //    BizActionObj.Error = ex.Message;  //"Error Occured";
            //    //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
            //    //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            //}
            //finally
            //{
            //    //log error  
            //    //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            //}


            //return BizActionObj;
            #endregion
        }

        #region Added by Ashish Z. for DoctorServiceRateCategoryWise
        public override IValueObject GetServiceListForDocSerRateCat(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceDetailForDocSerRateCat");

                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);    // Get Services by Application Configuration Seting.
                //dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ServiceMaster.ClassID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
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
                        objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        objServiceMasterVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                        objServiceMasterVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));

                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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

        public override IValueObject AddUpdateDoctorServiceRateCategory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDoctorServiceRateCategoryBizActionVO BizActionObj = valueObject as clsAddUpdateDoctorServiceRateCategoryBizActionVO;

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
                            DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorServiceRateCategoryWise");
                            dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                            dbServer.AddInParameter(command, "CategoryID", DbType.Int64, item.CategoryID);
                            dbServer.AddInParameter(command, "ServiceId", DbType.Int64, item.ServiceID);
                            dbServer.AddInParameter(command, "ClassId", DbType.Int64, item.ClassID);

                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            int intStatus1 = dbServer.ExecuteNonQuery(command);
                        }
                    }


                    //DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorServiceRateCategoryWise");
                    //dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);
                    //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //int intStatus1 = dbServer.ExecuteNonQuery(command);
                }

                if (BizActionObj.ServiceMasterDetailsList == null)
                    BizActionObj.ServiceMasterDetailsList = new List<clsServiceMasterVO>();

                if (BizActionObj.ServiceMasterDetailsList != null && BizActionObj.ServiceMasterDetailsList.Count > 0)
                {
                    foreach (var item in BizActionObj.ServiceMasterDetailsList)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorServiceRateCategoryWise");
                        //DbCommand command = dbServer.GetStoredProcCommand("temp_CIMS_AddDoctorServiceRateCategoryWise");

                        dbServer.AddInParameter(command, "ID", DbType.Int64, 0); //0
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);
                        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceID); //serviceID
                        dbServer.AddInParameter(command, "ClassId", DbType.Int64, item.ClassID);
                        dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, item.Specialization);
                        dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, item.SubSpecialization);
                        dbServer.AddInParameter(command, "Rate", DbType.Decimal, item.Rate);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
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

        public override IValueObject GetFrontPannelDataGridList(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetFrontPannelDataGridListBizActionVO objItem = valueObject as clsGetFrontPannelDataGridListBizActionVO;
            clsServiceMasterVO objItemVO = new clsServiceMasterVO();
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceRateCategoryList");

                dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.ServiceMasterDetails.UnitID);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objItem.ServiceMasterDetails.CategoryID);
                dbServer.AddInParameter(command, "ClassID", DbType.Int64, objItem.ServiceMasterDetails.ClassID);
                dbServer.AddInParameter(command, "ServiceName", DbType.String, objItem.ServiceMasterDetails.ServiceName);
                //dbServer.AddInParameter(command, "SpecializationId", DbType.Int64, objItem.ServiceMasterDetails.Specialization);
                //dbServer.AddInParameter(command, "SubSpecializationId", DbType.Int64, objItem.ServiceMasterDetails.SubSpecialization);

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
                        objItemVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                        objItemVO.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        objItemVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        objItemVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                        objItemVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objItemVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objItemVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objItemVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                        objItemVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        objItemVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));
                        objItemVO.Rate = Convert.ToDecimal((DALHelper.HandleDBNull(reader["Rate"])));
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

        public override IValueObject GetFrontPannelDataGridByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetFrontPannelDataGridListBizActionVO objItem = valueObject as clsGetFrontPannelDataGridListBizActionVO;
            clsServiceMasterVO objItemVO = new clsServiceMasterVO();
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDoctorServiceRateCategoryList");

                dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.ServiceMasterDetails.UnitID);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objItem.CategoryID);
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
                        objItemVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                        objItemVO.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"]));
                        objItemVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));
                        objItemVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objItemVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassId"]));
                        objItemVO.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objItemVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationId"]));
                        objItemVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationName"]));
                        objItemVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        objItemVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecializationName"]));
                        objItemVO.Rate = Convert.ToDecimal((DALHelper.HandleDBNull(reader["Rate"])));
                        objItemVO.Status = Convert.ToBoolean((DALHelper.HandleDBNull(reader["Status"])));
                        objItem.ServiceMasterDetailsList.Add(objItemVO);
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
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }

        public override IValueObject GetUnSelectedRecordForCategoryComboBox(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO objItem = valueObject as clsGetUnSelectedRecordForCategoryComboBoxBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetUnSelectedRecordForComboBox");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItem.UnitID);
                dbServer.AddInParameter(command, "IsFromDocSerLinling", DbType.Boolean, objItem.IsFromDocSerLinling);
                dbServer.AddOutParameter(command, "Status", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                objItem.SuccessStatus = Convert.ToInt64(dbServer.GetParameterValue(command, "Status"));
                //if masterlist instance is null then creates new instance
                //if (objItem.MasterList == null)
                //{
                //    objItem.MasterList = new List<MasterListItem>();
                //}
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //Reading the record from reader and stores in list
                        //Add the object value in list
                        //objItem.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (Boolean)reader["Status"]));//HandleDBNull(reader["Date"])));
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
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }


        public override IValueObject GetUnSelectedRecordForClinicComboBox(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO objItem = valueObject as clsGetUnSelectedRecordForCategoryComboBoxBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetUnSelectedRecordForComboBox");
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objItem.DoctorID);
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItem.UnitID);
                dbServer.AddInParameter(command, "IsFromDocSerLinling", DbType.Boolean, objItem.IsFromDocSerLinling);
                dbServer.AddOutParameter(command, "Status", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                objItem.SuccessStatus = Convert.ToInt64(dbServer.GetParameterValue(command, "Status"));
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




        //By Anjali.......................................
        public override IValueObject AddApplyLevelToService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateApplyLevelsToServiceBizActionVO objItem = valueObject as clsAddUpdateApplyLevelsToServiceBizActionVO;
            try
            {

                DbCommand command1 = null;
                clsServiceLevelsVO objItemVO = objItem.Obj;
                command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceLevels");

                dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, objItem.Obj.ServiceID);
                dbServer.AddInParameter(command1, "ServiceUnitID", DbType.Int64, objItem.Obj.ServiceUnitID);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "L1ID", DbType.Int64, objItem.Obj.L1ID);
                dbServer.AddInParameter(command1, "L2ID", DbType.Int64, objItem.Obj.L2ID);
                dbServer.AddInParameter(command1, "L3ID", DbType.Int64, objItem.Obj.L3ID);
                dbServer.AddInParameter(command1, "L4ID", DbType.Int64, objItem.Obj.L4ID);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command1, "ID", DbType.Int64, objItem.Obj.ID);
                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command1);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command1, "ResultStatus"));


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;
        }
        public override IValueObject GetApplyLevelToService(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetApplyLevelsToServiceBizActionVO BizAction = valueObject as clsGetApplyLevelsToServiceBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceLevels");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizAction.Obj.ServiceID);
                dbServer.AddInParameter(command, "ServiceUnitID", DbType.Int64, BizAction.Obj.ServiceUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Obj.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        BizAction.Obj.ServiceUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceUnitID"]));
                        BizAction.Obj.L1ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L1ID"]));
                        BizAction.Obj.L2ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L2ID"]));
                        BizAction.Obj.L3ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L3ID"]));
                        BizAction.Obj.L4ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["L4ID"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }
        //..................................................


        public override IValueObject GetTariffServiceListForPathology(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffServiceListBizActionForPathologyVO BizActionObj = (clsGetTariffServiceListBizActionForPathologyVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetTariffServiceListNewForPackage3"); // CIMS_GetTariffServiceListNewForPackage2 //CIMS_GetTariffServiceListNewForPackage  //CIMS_GetTariffServiceListNew

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                if (BizActionObj.IsPackage == true)
                    dbServer.AddInParameter(command, "IsPackage", DbType.Boolean, BizActionObj.IsPackage);
                else
                    dbServer.AddInParameter(command, "PackageID", DbType.Int64, BizActionObj.ForFilterPackageID);

                //dbServer.AddOutParameter(command, "SetTariffIsPackage", DbType.Int64, int.MaxValue);

                if (BizActionObj.UsePackageSubsql == true)
                    dbServer.AddInParameter(command, "UsePackageSubsql", DbType.Boolean, BizActionObj.UsePackageSubsql);

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);

                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "PatientSourceType", DbType.Int16, BizActionObj.PatientSourceType);
                dbServer.AddInParameter(command, "PatientSourceTypeID", DbType.Int64, BizActionObj.PatientSourceTypeID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "GetSuggestedServices", DbType.Boolean, BizActionObj.GetSuggestedServices);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                if (BizActionObj.SearchExpression != null && BizActionObj.SearchExpression.Length > 0)
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SearchExpression);

                if (BizActionObj.ClassID > 0)
                    dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ClassID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                //long ServiceSetForPackage = 0;


                if (reader.HasRows)
                {

                    //ServiceSetForPackage = Convert.ToInt64(dbServer.GetParameterValue(command, "SetTariffIsPackage"));

                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();

                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = Convert.ToInt64(reader["ServiceID"]);
                        objServiceMasterVO.TariffServiceMasterID = Convert.ToInt64(reader["ID"]);
                        objServiceMasterVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objServiceMasterVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objServiceMasterVO.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        objServiceMasterVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objServiceMasterVO.Specialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecializationID"]));
                        objServiceMasterVO.SubSpecialization = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubSpecializationId"]));
                        if (BizActionObj.PatientSourceType == 2) // Camp
                        {
                            objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else if (BizActionObj.PatientSourceType == 1)   //Loyalty
                        {
                            objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                            objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                            objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        }
                        else
                        {

                            objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));


                            objServiceMasterVO.SeniorCitizen = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["SeniorCitizen"]));
                            objServiceMasterVO.SeniorCitizenConAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConAmount"]));
                            objServiceMasterVO.SeniorCitizenConPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SeniorCitizenConPercent"]));
                            objServiceMasterVO.SeniorCitizenAge = Convert.ToInt32(DALHelper.HandleDBNull(reader["SeniorCitizenAge"]));


                            if (objServiceMasterVO.SeniorCitizen == true && BizActionObj.Age >= objServiceMasterVO.SeniorCitizenAge)
                            {
                                objServiceMasterVO.ConcessionAmount = objServiceMasterVO.SeniorCitizenConAmount;
                                objServiceMasterVO.ConcessionPercent = objServiceMasterVO.SeniorCitizenConPercent;
                            }
                            else
                            {
                                objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                                objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));

                            }

                            if (BizActionObj.PrescribedService == false)
                            {
                                objServiceMasterVO.PatientCategoryL3 = Convert.ToString(DALHelper.HandleDBNull(reader["TariffName"]));

                            }



                        }

                        objServiceMasterVO.StaffDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        objServiceMasterVO.StaffDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountAmount"]));
                        objServiceMasterVO.StaffDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDiscountPercent"]));
                        objServiceMasterVO.StaffDependantDiscount = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StaffDependantDiscount"]));
                        objServiceMasterVO.StaffDependantDiscountAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountAmount"]));
                        objServiceMasterVO.StaffDependantDiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["StaffDependantDiscountPercent"]));
                        objServiceMasterVO.Concession = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Concession"]));
                        objServiceMasterVO.ServiceTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ServiceTax"]));
                        objServiceMasterVO.ServiceTaxAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
                        objServiceMasterVO.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        objServiceMasterVO.InHouse = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InHouse"]));
                        objServiceMasterVO.DoctorShare = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DoctorShare"]));
                        objServiceMasterVO.DoctorSharePercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorSharePercentage"]));
                        objServiceMasterVO.DoctorShareAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DoctorShareAmount"]));
                        objServiceMasterVO.RateEditable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["RateEditable"]));
                        objServiceMasterVO.MaxRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxRate"]));
                        objServiceMasterVO.MinRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        objServiceMasterVO.TarrifCode = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceCode"]));
                        objServiceMasterVO.TarrifName = Convert.ToString(DALHelper.HandleDBNull(reader["TarrifServiceName"]));
                        objServiceMasterVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objServiceMasterVO.CodeType = (Int64)DALHelper.HandleDBNull(reader["CodeType"]);
                        objServiceMasterVO.ShortDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ShortDescription"]));
                        objServiceMasterVO.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        objServiceMasterVO.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        objServiceMasterVO.SubSpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SubSpecialization"]));
                        objServiceMasterVO.IsPackage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackage"]));
                        objServiceMasterVO.PackageID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageID"]));
                        objServiceMasterVO.IsMarkUp = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsMarkUp"]));

                        //objServiceMasterVO.TariffIsPackage = ServiceSetForPackage;  // Convert.ToInt64(dbServer.GetParameterValue(command, "SetTariffIsPackage"));

                        //if (objServiceMasterVO.TariffIsPackage > 0 )  //== "Package")
                        //{
                        if (BizActionObj.UsePackageSubsql == true)
                        {
                            objServiceMasterVO.ApplicableTo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ApplicableTo"]));
                            objServiceMasterVO.ApplicableToString = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableToString"]));

                            // to set service background color to identify that this service is having Package Conditional Services
                            objServiceMasterVO.PackageServiceConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PackageServiceConditionID"]));
                        }
                        //}

                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }




                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            long i = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PatientId"]));
                        }
                    }


                    if (BizActionObj.PrescribedService == false)
                    {
                        reader.NextResult();
                        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    }



                    reader.Close();


                }
                if (reader.IsClosed == false)
                {
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


        public override IValueObject GetServiceListForPathology(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO BizActionObj = (clsGetServiceMasterListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceListForPathology");

                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "Specialization", DbType.Int64, BizActionObj.Specialization);
                dbServer.AddInParameter(command, "SubSpecialization", DbType.Int64, BizActionObj.SubSpecialization);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, 1);

                //By Anjali...................................
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ServiceName");


                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                //..........................................

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
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
                        objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        objServiceMasterVO.LongDescription = (string)DALHelper.HandleDBNull(reader["LongDescription"]);
                        objServiceMasterVO.ShortDescription = (string)DALHelper.HandleDBNull(reader["ShortDescription"]);
                        objServiceMasterVO.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        objServiceMasterVO.ClassID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));

                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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

        #region GST Details added by Ashish Z. on dated 24062017
        public override IValueObject AddUpdateSeviceTaxDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            clsAddUpdateServiceTaxBizActionVO BizAction = valueObject as clsAddUpdateServiceTaxBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = null;

                if (BizAction.OperationType == 1) //Save
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceTaxDetails");
                    dbServer.AddInParameter(command, "OperationType", DbType.Int32, BizAction.OperationType);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.ServiceTaxDetailsVO.UnitId);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizAction.ServiceTaxDetailsVO.ServiceId);
                    dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizAction.ServiceTaxDetailsVO.TariffId);
                    dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizAction.ServiceTaxDetailsVO.ClassId);
                    dbServer.AddInParameter(command, "TaxID", DbType.Int64, BizAction.ServiceTaxDetailsVO.TaxID);
                    dbServer.AddInParameter(command, "TaxPercentage", DbType.Decimal, BizAction.ServiceTaxDetailsVO.Percentage);
                    dbServer.AddInParameter(command, "TaxType", DbType.Int32, BizAction.ServiceTaxDetailsVO.TaxType);
                    dbServer.AddInParameter(command, "IsTaxLimitApplicable", DbType.Boolean, BizAction.ServiceTaxDetailsVO.IsTaxLimitApplicable);
                    dbServer.AddInParameter(command, "TaxLimit", DbType.Decimal, BizAction.ServiceTaxDetailsVO.TaxLimit);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, BizAction.ServiceTaxDetailsVO.status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ReasultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.ServiceTaxDetailsVO.ID);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizAction.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "ReasultStatus")));
                    BizAction.ServiceTaxDetailsVO.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                    //if (BizAction.ServiceTaxDetailsVOList != null && BizAction.ServiceTaxDetailsVOList.Count() > 0)
                    //{
                    //    foreach (var item in BizAction.ServiceTaxDetailsVOList)
                    //    {
                    //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceTaxDetails");
                    //        dbServer.AddInParameter(command, "OperationType", DbType.Int32, BizAction.OperationType);
                    //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitId);
                    //        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ServiceId);
                    //        dbServer.AddInParameter(command, "TariffID", DbType.Int64, item.TariffId);
                    //        dbServer.AddInParameter(command, "ClassID", DbType.Int64, item.ClassId);
                    //        dbServer.AddInParameter(command, "TaxID", DbType.Int64, item.TaxID);
                    //        dbServer.AddInParameter(command, "TaxPercentage", DbType.Decimal, item.Percentage);
                    //        dbServer.AddInParameter(command, "TaxType", DbType.Int32, item.TaxType);
                    //        dbServer.AddInParameter(command, "IsTaxLimitApplicable", DbType.Boolean, item.IsTaxLimitApplicable);
                    //        dbServer.AddInParameter(command, "TaxLimit", DbType.Decimal, item.TaxLimit);
                    //        dbServer.AddInParameter(command, "Status", DbType.Boolean, item.status);
                    //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    //        dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    //        dbServer.AddOutParameter(command, "ReasultStatus", DbType.Int32, int.MaxValue);
                    //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    //        BizAction.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "ReasultStatus")));
                    //        item.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                    //    }
                    //}
                }
                else if (BizAction.OperationType == 2) //Modify
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceTaxDetails");
                    dbServer.AddInParameter(command, "OperationType", DbType.Int32, BizAction.OperationType);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.ServiceTaxDetailsVO.UnitId);
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizAction.ServiceTaxDetailsVO.ServiceId);
                    dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizAction.ServiceTaxDetailsVO.TariffId);
                    dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizAction.ServiceTaxDetailsVO.ClassId);
                    dbServer.AddInParameter(command, "TaxID", DbType.Int64, BizAction.ServiceTaxDetailsVO.TaxID);
                    dbServer.AddInParameter(command, "TaxPercentage", DbType.Decimal, BizAction.ServiceTaxDetailsVO.Percentage);
                    dbServer.AddInParameter(command, "TaxType", DbType.Int32, BizAction.ServiceTaxDetailsVO.TaxType);
                    dbServer.AddInParameter(command, "IsTaxLimitApplicable", DbType.Boolean, BizAction.ServiceTaxDetailsVO.IsTaxLimitApplicable);
                    dbServer.AddInParameter(command, "TaxLimit", DbType.Decimal, BizAction.ServiceTaxDetailsVO.TaxLimit);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, BizAction.ServiceTaxDetailsVO.status);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ReasultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.ServiceTaxDetailsVO.ID);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizAction.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "ReasultStatus")));
                    long ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                }
                else if (BizAction.OperationType == 3) //Delete
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddUpdateServiceTaxDetails");
                    dbServer.AddInParameter(command, "OperationType", DbType.Int32, BizAction.OperationType);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, BizAction.ServiceTaxDetailsVO.status);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ReasultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.ServiceTaxDetailsVO.ID);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizAction.SuccessStatus = Convert.ToInt32(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "ReasultStatus")));
                    long ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }
            return BizAction;
        }

        public override IValueObject GetServiceTaxDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceTaxDetailsBizActionVO BizActionObj = (clsGetServiceTaxDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetServiceTaxDetails");
                dbServer.AddInParameter(command, "ServiceId", DbType.Int64, BizActionObj.ServiceTaxDetailsVO.ServiceId);
                dbServer.AddInParameter(command, "ClassID", DbType.Int64, BizActionObj.ServiceTaxDetailsVO.ClassId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceTaxDetailsVOList == null)
                        BizActionObj.ServiceTaxDetailsVOList = new List<clsServiceTaxVO>();
                    while (reader.Read())
                    {
                        clsServiceTaxVO objServiceTax = new clsServiceTaxVO();
                        objServiceTax.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objServiceTax.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objServiceTax.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objServiceTax.TariffId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        objServiceTax.ClassId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassID"]));
                        objServiceTax.TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"]));
                        objServiceTax.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxPercentage"]));
                        objServiceTax.TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["TaxType"]));
                        objServiceTax.IsTaxLimitApplicable = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTaxLimitApplicable"]));
                        objServiceTax.TaxLimit = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TaxLimit"]));
                        objServiceTax.ClassName = Convert.ToString(DALHelper.HandleDBNull(reader["ClassName"]));
                        objServiceTax.TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"]));
                        objServiceTax.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        objServiceTax.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        objServiceTax.status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.ServiceTaxDetailsVOList.Add(objServiceTax);
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
        #endregion

    }
}
