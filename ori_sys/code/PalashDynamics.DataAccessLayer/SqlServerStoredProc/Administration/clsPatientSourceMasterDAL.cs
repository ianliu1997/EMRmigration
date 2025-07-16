using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsPatientSourceMasterDAL : clsBasePatientSourceMasterMasterDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        public clsPatientSourceMasterDAL()
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

        public override IValueObject AddPatientSourceMaster(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddPatientSourceBizActionVO BizActionobj = valueObject as clsAddPatientSourceBizActionVO;

            if (BizActionobj.IsFromItemGroupMaster)
            {
                if (BizActionobj.PatientDetails.ID == 0)
                {
                    BizActionobj = AddItemGroupMaster(BizActionobj, objUserVO);
                }
                else
                {
                    BizActionobj = UpdateItemGroupMaster(BizActionobj, objUserVO);

                }
            }
            else
            {
                if (BizActionobj.PatientDetails.ID == 0)
                {
                    BizActionobj = AddPatientSourceMaster(BizActionobj, objUserVO);
                }
                else
                {
                    BizActionobj = UpdatePatientSourceMaster(BizActionobj, objUserVO);

                }
            }
            return BizActionobj;
        }

        private clsAddPatientSourceBizActionVO AddItemGroupMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsPatientSourceVO ObjPatientSourceVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddItemGroupMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, ObjPatientSourceVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjPatientSourceVO.Description.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                //ROHINEE
                dbServer.AddInParameter(command, "GeneralLedgerID", DbType.Int64, ObjPatientSourceVO.PatientCatagoryID);
                //
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjPatientSourceVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjPatientSourceVO.ID);
                int intStatus2 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.PatientDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                trans.Commit();
            }
            catch (Exception)
            {
                //throw;
                BizActionObj.ResultSuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        private clsAddPatientSourceBizActionVO UpdateItemGroupMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsPatientSourceVO ObjPatientSourceVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateItemGroupMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjPatientSourceVO.ID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjPatientSourceVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjPatientSourceVO.Description.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjPatientSourceVO.Status);
                //ROHINEE
                dbServer.AddInParameter(command, "@GeneralLedgerID", DbType.Int64, ObjPatientSourceVO.PatientCatagoryID);
                //
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjPatientSourceVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                trans.Commit();
                //BizActionObj.SuccessStatus = 0;


            }
            catch (Exception)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        private clsAddPatientSourceBizActionVO AddPatientSourceMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsPatientSourceVO ObjPatientSourceVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientSource");

                dbServer.AddInParameter(command, "Code", DbType.String, ObjPatientSourceVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjPatientSourceVO.Description.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                //ROHINEE
                dbServer.AddInParameter(command, "PatientCatagoryID", DbType.Int64, ObjPatientSourceVO.PatientCatagoryID);
                //
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjPatientSourceVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjPatientSourceVO.ID);
                int intStatus2 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.PatientDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //BizActionObj.ResultSuccessStatus = BizActionObj.SuccessStatus;


                //foreach (var ObjTariff in ObjPatientSourceVO.TariffDetails)
                //{

                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientSourceTariffDetails");


                //    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "PatientSourceID", DbType.Int64, ObjPatientSourceVO.ID);
                //    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjTariff.TariffID);
                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjTariff.Status);
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTariff.ID);



                //    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                //    ObjTariff.ID = (long)dbServer.GetParameterValue(command1, "ID");

                //}

                trans.Commit();
                //BizActionObj.ResultSuccessStatus = 0;
            }
            catch (Exception)
            {
                //throw;
                BizActionObj.ResultSuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        private clsAddPatientSourceBizActionVO UpdatePatientSourceMaster(clsAddPatientSourceBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsPatientSourceVO ObjPatientSourceVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientSource");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjPatientSourceVO.ID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjPatientSourceVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjPatientSourceVO.Description.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjPatientSourceVO.Status);
                //ROHINEE
                dbServer.AddInParameter(command, "PatientCatagoryID", DbType.Int64, ObjPatientSourceVO.PatientCatagoryID);
                //
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjPatientSourceVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                //Commented By Pallavi
                //if (ObjPatientSourceVO.TariffDetails != null && ObjPatientSourceVO.TariffDetails.Count > 0)
                //{
                //    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeletePatientSourceTariffDetails");

                //    dbServer.AddInParameter(command2, "PatientSourceID", DbType.Int64, ObjPatientSourceVO.ID);
                //    int intStatus2 = dbServer.ExecuteNonQuery(command2);
                //}


                //commented by rohini
                //foreach (var ObjTariff in ObjPatientSourceVO.TariffDetails)
                //{

                //    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientSourceTariffDetails");
                //    dbServer.AddOutParameter(command1, "ID", DbType.Int64, int.MaxValue);
                //    dbServer.AddInParameter(command1, "PatientSourceID", DbType.Int64, ObjPatientSourceVO.ID);
                //    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjTariff.TariffID);
                //    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjTariff.Status);
                //    //Added By Pallavi
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                //    //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTariff.ID);

                //    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                //    ObjTariff.ID = (long)dbServer.GetParameterValue(command1, "ID");

                //}
                trans.Commit();
                //BizActionObj.SuccessStatus = 0;


            }
            catch (Exception)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        public override IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffDetailsListBizActionVO BizActionObj = (clsGetTariffDetailsListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTariffList");

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientSourceDetails == null)
                        BizActionObj.PatientSourceDetails = new List<clsTariffDetailsVO>();
                    while (reader.Read())
                    {
                        clsTariffDetailsVO TariffDetailsVO = new clsTariffDetailsVO();
                        TariffDetailsVO.TariffID = (long)reader["ID"];
                        TariffDetailsVO.TariffCode = reader["Code"].ToString();
                        TariffDetailsVO.TariffDescription = reader["Description"].ToString();
                        TariffDetailsVO.Status = (bool)reader["Status"];

                        TariffDetailsVO.IsFamily = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFamily"]));

                        BizActionObj.PatientSourceDetails.Add(TariffDetailsVO);
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

        //Added by Ashish Z. for CompanyMaster
        public override IValueObject GetTariffListForCompMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffDetailsListBizActionVO BizActionObj = (clsGetTariffDetailsListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTariffListForCompanyMaster");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, null);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientSourceDetails == null)
                        BizActionObj.PatientSourceDetails = new List<clsTariffDetailsVO>();
                    while (reader.Read())
                    {
                        clsTariffDetailsVO TariffDetailsVO = new clsTariffDetailsVO();
                        TariffDetailsVO.TariffID = (long)reader["ID"];
                        TariffDetailsVO.TariffCode = reader["Code"].ToString();
                        TariffDetailsVO.TariffDescription = reader["Description"].ToString();
                        TariffDetailsVO.Status = (bool)reader["Status"];

                        TariffDetailsVO.IsFamily = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFamily"]));

                        BizActionObj.PatientSourceDetails.Add(TariffDetailsVO);
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
        //

        public override IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceListBizActionVO BizActionObj = (clsGetPatientSourceListBizActionVO)valueObject;

            try
            {
                if (!BizActionObj.ValidPatientMasterSourceList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSourceList");


                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                    dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

                    dbServer.AddInParameter(command, "PatientSourceType", DbType.String, BizActionObj.FilterPatientSourceType);


                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientSourceDetails == null)
                            BizActionObj.PatientSourceDetails = new List<clsPatientSourceVO>();
                        while (reader.Read())
                        {
                            //clsPatientSourceVO PatientSourceVO = new clsPatientSourceVO();
                            //PatientSourceVO.ID = (long)reader["ID"];
                            //PatientSourceVO.Code = reader["Code"].ToString();
                            //PatientSourceVO.Description = reader["Description"].ToString();
                            //PatientSourceVO.Status = (bool)reader["Status"];

                            //BizActionObj.PatientSourceDetails.Add(PatientSourceVO);



                            clsPatientSourceVO PatientSourceVO = new clsPatientSourceVO();

                            PatientSourceVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            PatientSourceVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            PatientSourceVO.PatientCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategoryName"]));

                            PatientSourceVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            PatientSourceVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            PatientSourceVO.PatientCatagoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                            PatientSourceVO.PatientSourceType = Convert.ToInt16(DALHelper.HandleDBNull(reader["PatientSourceType"]));
                            PatientSourceVO.PatientSourceTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceTypeID"]));
                            //PatientSourceVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                            //PatientSourceVO.EffectiveDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EffectiveDate"]));
                            //PatientSourceVO.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExpiryDate"]));


                            BizActionObj.PatientSourceDetails.Add(PatientSourceVO);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                        reader.Close();
                    }
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetValidPatientSourceList");


                    dbServer.AddInParameter(command, "ID", DbType.String, BizActionObj.ID);

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.MasterList == null)
                            BizActionObj.MasterList = new List<MasterListItem>();
                        while (reader.Read())
                        {
                            MasterListItem PatientSourceVO = new MasterListItem();
                            PatientSourceVO.ID = (long)reader["ID"];
                            PatientSourceVO.Code = reader["Code"].ToString();
                            PatientSourceVO.Description = reader["Description"].ToString();
                            PatientSourceVO.Status = (bool)reader["Status"];
                            //PatientSourceVO.FromDate = (DateTime?)DALHelper.HandleDate(reader["FromDate"]);
                            //PatientSourceVO.ToDate = (DateTime?)DALHelper.HandleDate(reader["ToDate"]);
                            //PatientSourceVO.PatientSourceType = (short)DALHelper.HandleDBNull(reader["PatientSourceType"]);
                            //PatientSourceVO.PatientSourceTypeID = (long)DALHelper.HandleDBNull(reader["PatientSourceTypeID"]);
                            BizActionObj.MasterList.Add(PatientSourceVO);
                        }

                        //reader.NextResult();
                        //BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                        reader.Close();
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


        public override IValueObject GetItemGroupMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceListBizActionVO BizActionObj = (clsGetPatientSourceListBizActionVO)valueObject;

            try
            {
                if (!BizActionObj.ValidPatientMasterSourceList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemGroupMaster");


                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                    dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientSourceDetails == null)
                            BizActionObj.PatientSourceDetails = new List<clsPatientSourceVO>();
                        while (reader.Read())
                        {

                            clsPatientSourceVO PatientSourceVO = new clsPatientSourceVO();

                            PatientSourceVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            PatientSourceVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            PatientSourceVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            PatientSourceVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            PatientSourceVO.PatientCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["GeneralLedgerName"]));
                            PatientSourceVO.PatientCatagoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GeneralLedgerID"]));
                            BizActionObj.PatientSourceDetails.Add(PatientSourceVO);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                        reader.Close();
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

        public override IValueObject GetPatientSourceByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceDetailsByIDBizActionVO BizActionObj = (clsGetPatientSourceDetailsByIDBizActionVO)valueObject;

            try
            {
                clsPatientSourceVO ObjPatient = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSourceDetailsByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, BizActionObj.ID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.Details == null)
                            BizActionObj.Details = new clsPatientSourceVO();
                        BizActionObj.Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.Details.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        BizActionObj.Details.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.Details.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.Details.PatientCatagoryID = (long)DALHelper.HandleDBNull(reader["PatientCategoryId"]);
                        BizActionObj.Details.FromDate = (DateTime?)DALHelper.HandleDate(reader["FromDate"]);
                        BizActionObj.Details.ToDate = (DateTime?)DALHelper.HandleDate(reader["ToDate"]);
                        BizActionObj.Details.PatientSourceType = (short)DALHelper.HandleDBNull(reader["PatientSourceType"]);
                        BizActionObj.Details.PatientSourceTypeID = (long)DALHelper.HandleDBNull(reader["PatientSourceTypeID"]);
                    }
                }

                reader.NextResult();

                //if (reader.HasRows)
                //{
                //    BizActionObj.Details.TariffDetails = new List<clsTariffDetailsVO>();
                //    while (reader.Read())
                //    {
                //        clsTariffDetailsVO objTariffDetailsVO = new clsTariffDetailsVO();
                //        objTariffDetailsVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                //        objTariffDetailsVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                //        objTariffDetailsVO.TariffDescription = (string)DALHelper.HandleDBNull(reader["Description"]);
                //        objTariffDetailsVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                //        BizActionObj.Details.TariffDetails.Add(objTariffDetailsVO);
                //    }
                //}

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
        //public override IValueObject GetPatientSourceListByPatientCategoryId(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsGetPatientSourceListByPatientCategoryIdBizActionVO BizActionObj = (clsGetPatientSourceListByPatientCategoryIdBizActionVO)valueObject;

        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSourceListByPatientCategoryId");


        //        dbServer.AddInParameter(command, "PatientCategoryId", DbType.Int64, BizActionObj.SelectedPatientCategoryIdID);
        //        //  dbServer.AddInParameter(command, "PatientSourceType", DbType.String, BizActionObj.FilterPatientSourceType);
        //        //   dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.PatientSourceDetails == null)
        //                BizActionObj.PatientSourceDetails = new List<clsPatientSourceVO>();
        //            while (reader.Read())
        //            {
        //                clsPatientSourceVO PatientSourceVO = new clsPatientSourceVO();

        //                PatientSourceVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                PatientSourceVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                PatientSourceVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                PatientSourceVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

        //                PatientSourceVO.PatientSourceType = Convert.ToInt16(DALHelper.HandleDBNull(reader["PatientSourceType"]));
        //                PatientSourceVO.PatientSourceTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceTypeID"]));
        //                //PatientSourceVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
        //                //PatientSourceVO.EffectiveDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EffectiveDate"]));
        //                //PatientSourceVO.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExpiryDate"]));


        //                BizActionObj.PatientSourceDetails.Add(PatientSourceVO);
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


        public override IValueObject GetPatientSourceListByPatientCategoryId(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientSourceListByPatientCategoryIdBizActionVO BizActionObj = (clsGetPatientSourceListByPatientCategoryIdBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSourceListByPatientCategoryId");


                dbServer.AddInParameter(command, "PatientCategoryId", DbType.Int64, BizActionObj.SelectedPatientCategoryIdID);
                //  dbServer.AddInParameter(command, "PatientSourceType", DbType.String, BizActionObj.FilterPatientSourceType);
                //   dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientSourceDetails == null)
                        BizActionObj.PatientSourceDetails = new List<clsPatientSourceVO>();
                    while (reader.Read())
                    {
                        clsPatientSourceVO PatientSourceVO = new clsPatientSourceVO();

                        PatientSourceVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        PatientSourceVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        PatientSourceVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        PatientSourceVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        PatientSourceVO.PatientSourceType = Convert.ToInt16(DALHelper.HandleDBNull(reader["PatientSourceType"]));
                        PatientSourceVO.PatientSourceTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceTypeID"]));
                        //PatientSourceVO.TariffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TariffID"]));
                        //PatientSourceVO.EffectiveDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EffectiveDate"]));
                        //PatientSourceVO.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExpiryDate"]));


                        BizActionObj.PatientSourceDetails.Add(PatientSourceVO);
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
    }
    //rohinee
    public class clsRegistrationChargesMasterDAL : clsBaseRegistrationChargesMasterMasterDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        public clsRegistrationChargesMasterDAL()
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

        public override IValueObject AddRegistartionChargesMaster(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddRegistrationChargesBizActionVO BizActionobj = valueObject as clsAddRegistrationChargesBizActionVO;


            if (BizActionobj.PatientDetails.ID == 0)
            {
                BizActionobj = AddPatientSourceMaster(BizActionobj, objUserVO);
            }

            else
            {
                BizActionobj = UpdatePatientSourceMaster(BizActionobj, objUserVO);

            }

            return BizActionobj;
        }


        private clsAddRegistrationChargesBizActionVO AddPatientSourceMaster(clsAddRegistrationChargesBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsRegistrationChargesVO ObjPatientSourceVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRegistrationCharges");

                dbServer.AddInParameter(command, "PatientTypeId", DbType.Int64, ObjPatientSourceVO.PatientTypeId);
                dbServer.AddInParameter(command, "PatientServiceId", DbType.Int64, ObjPatientSourceVO.PatientServiceId);
                dbServer.AddInParameter(command, "Rate", DbType.Double, ObjPatientSourceVO.Rate);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjPatientSourceVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "Synchronized", DbType.Boolean, false);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);

                int intStatus2 = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.PatientDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ResultSuccessStatus = BizActionObj.SuccessStatus;

                trans.Commit();
                BizActionObj.ResultSuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.ResultSuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        private clsAddRegistrationChargesBizActionVO UpdatePatientSourceMaster(clsAddRegistrationChargesBizActionVO BizActionObj, clsUserVO objUserVO)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsRegistrationChargesVO ObjPatientSourceVO = BizActionObj.PatientDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRagistrationCharges");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjPatientSourceVO.ID);
                dbServer.AddInParameter(command, "PatientTypeId", DbType.Int64, ObjPatientSourceVO.PatientTypeId);
                dbServer.AddInParameter(command, "PatientServiceId", DbType.Int64, ObjPatientSourceVO.PatientServiceId);
                dbServer.AddInParameter(command, "Rate", DbType.Double, ObjPatientSourceVO.Rate);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjPatientSourceVO.Status);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjPatientSourceVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");


                trans.Commit();
                BizActionObj.SuccessStatus = 0;


            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.PatientDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }

            return BizActionObj;
        }

        public override IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRegistrationChargesListBizActionVO BizActionObj = (clsGetRegistrationChargesListBizActionVO)valueObject;

            try
            {
                if (!BizActionObj.ValidPatientMasterSourceList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRegistrationChargesList");


                    dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                    dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

                    //dbServer.AddInParameter(command, "PatientSourceType", DbType.String, BizActionObj.FilterPatientSourceType);


                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.PatientSourceDetails == null)
                            BizActionObj.PatientSourceDetails = new List<clsRegistrationChargesVO>();
                        while (reader.Read())
                        {
                            clsRegistrationChargesVO PatientSourceVO = new clsRegistrationChargesVO();

                            PatientSourceVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            //PatientSourceVO.PatientCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategoryName"]));                          
                            PatientSourceVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                            PatientSourceVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                            PatientSourceVO.PatientTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientTypeId"]));
                            PatientSourceVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                            PatientSourceVO.PatientServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientServiceId"]));
                            PatientSourceVO.PatientServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                            PatientSourceVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));

                            BizActionObj.PatientSourceDetails.Add(PatientSourceVO);
                        }
                        reader.NextResult();
                        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                        reader.Close();
                    }
                }
                //else
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetValidPatientSourceList");


                //    dbServer.AddInParameter(command, "ID", DbType.String, BizActionObj.ID);

                //    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                //    if (reader.HasRows)
                //    {
                //        if (BizActionObj.MasterList == null)
                //            BizActionObj.MasterList = new List<MasterListItem>();
                //        while (reader.Read())
                //        {
                //            MasterListItem PatientSourceVO = new MasterListItem();
                //            PatientSourceVO.ID = (long)reader["ID"];
                //            PatientSourceVO.Code = reader["Code"].ToString();
                //            PatientSourceVO.Description = reader["Description"].ToString();
                //            PatientSourceVO.Status = (bool)reader["Status"];

                //            BizActionObj.MasterList.Add(PatientSourceVO);
                //        }


                //        reader.Close();
                //    }
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


        public override IValueObject GetRegistrationChargesByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRegistrationChargesDetailsByIDBizActionVO BizActionObj = (clsGetRegistrationChargesDetailsByIDBizActionVO)valueObject;

            try
            {
                clsRegistrationChargesVO ObjPatient = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRegistrationChargesDetailsByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.Details == null)
                            BizActionObj.Details = new clsRegistrationChargesVO();

                        BizActionObj.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.Details.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                        BizActionObj.Details.PatientServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientServiceId"]));
                        BizActionObj.Details.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        BizActionObj.Details.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.Details.FromDate = Convert.ToDateTime(DALHelper.HandleDate(reader["FromDate"]));
                        BizActionObj.Details.ToDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ToDate"]));

                    }
                }

                reader.NextResult();



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

        public override IValueObject GetRegistrationChargesByPatientTypeID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO BizActionObj = (clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRegistartionTypeChargesByPatientType");  //clsRegistrationChargesVO

                dbServer.AddInParameter(command, "PatientTypeID", DbType.Int64, BizActionObj.PatientTypeID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<clsRegistrationChargesVO>();
                    }
                    while (reader.Read())
                    {
                        //Add the object value in list
                        clsRegistrationChargesVO objVO = new clsRegistrationChargesVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        //objVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objVO.PatientServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientServiceId"]));
                        //objVO.IsClinical = (bool)DALHelper.HandleDBNull(reader["IsClinical"]);
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.List.Add(objVO);
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




    }
}
