using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Radiology;
using System.Data.Common;
using System.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using System.IO;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsRadiologyDAL : clsBaseRadiologyDAL
    {
        //This Region Contains Variables Which are Used At Form Level

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        public clsRadiologyDAL()
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

        #region Template Master
        public override IValueObject AddTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRadTemplateMasterBizActionVO BizActionObj = valueObject as clsAddRadTemplateMasterBizActionVO;

            if (BizActionObj.TemplateDetails.TemplateID == 0)
                BizActionObj = AddTemplateDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateTemplateDetails(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddRadTemplateMasterBizActionVO AddTemplateDetails(clsAddRadTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsRadiologyVO objRadiologyVO = BizActionObj.TemplateDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRadTemplateMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, objRadiologyVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, objRadiologyVO.Description.Trim());

                dbServer.AddInParameter(command, "Template", DbType.String, objRadiologyVO.TemplateDesign);
                dbServer.AddInParameter(command, "Radiologist", DbType.Int64, objRadiologyVO.Radiologist);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objRadiologyVO.GenderID);
                dbServer.AddInParameter(command, "TemplateResultID", DbType.Int64, objRadiologyVO.TemplateResultID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objRadiologyVO.TemplateID);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.TemplateDetails.TemplateID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");

                List<MasterListItem> masterList = BizActionObj.TemplateDetails.GenderList;
                if (masterList != null || masterList.Count > 0)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteRadioGenderToTemplate");
                    dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.TemplateID);
                    int intStatus88 = dbServer.ExecuteNonQuery(command2);

                    foreach (var item in masterList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadioGenderToTemplate");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "GenderID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.TemplateID);
                        dbServer.AddInParameter(command1, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
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
      // AddRadiologistToTempList
        public override IValueObject AddRadiologistToTempList(IValueObject valueObject, clsUserVO userVO)
        {

            clsAddRadiologistToTempbizActionVO objItem = valueObject as clsAddRadiologistToTempbizActionVO;
            try
            {
                DbCommand command = null;

                clsRadiologyVO objItemVO = objItem.ItemSupplier;

                
                int status = 0;

                command = dbServer.GetStoredProcCommand("CIMS_AddRadiologistListForTest");

                if (objItem.ItemSupplierList.Count > 0)
                {
                    for (int i = 0; i <= objItem.ItemSupplierList.Count - 1; i++)
                    {
                        command.Parameters.Clear();
                        dbServer.AddInParameter(command, "Id", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, objItemVO.UnitID);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int64, objItemVO.TemplateID);
                        dbServer.AddInParameter(command, "RadiologistID", DbType.Int64, objItem.ItemSupplierList[i].ID);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, objItem.ItemSupplierList[i].Status);
                        //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                        //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                        //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                        //dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                        //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                        //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        if (intStatus > 0)
                        {
                            status = 1;
                        }
                    }
                    objItem.SuccessStatus = status;//(int)dbServer.GetParameterValue(command, "ResultStatus");   
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objItem;


        }


        public override IValueObject GetRadiologistToTempList(IValueObject valueObject, clsUserVO userVO)
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetRadiologistToTempBizActionVO objBizAction = valueObject as clsGetRadiologistToTempBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetRadiologistListForTest");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemSupplier.TemplateID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemSupplierList == null)
                        objBizAction.ItemSupplierList = new List<clsRadiologyVO>();
                    while (reader.Read())
                    {
                        clsRadiologyVO objItemMaster = new clsRadiologyVO();
                        objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.Radiologist = (long)DALHelper.HandleDBNull(reader["RadiologistID"]);
                        objItemMaster.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
                        objBizAction.ItemSupplierList.Add(objItemMaster);
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
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }


        public override IValueObject GetRadiologistResultEntry(IValueObject valueObject, clsUserVO userVO) //Added By yogesh K 2 6 16
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;
            clsGetRadiologistToTempBizActionVO objBizAction = valueObject as clsGetRadiologistToTempBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetRadiologistListForTest");
                dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemSupplier.TemplateID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemSupplierList == null)
                        objBizAction.ItemSupplierList = new List<clsRadiologyVO>();
                    while (reader.Read())
                    {
                        clsRadiologyVO objItemMaster = new clsRadiologyVO();
                        objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objItemMaster.Radiologist = (long)DALHelper.HandleDBNull(reader["RadiologistID"]);
                        objItemMaster.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
                        objBizAction.ItemSupplierList.Add(objItemMaster);
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
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }


        public override IValueObject GetRadiologistResultEntryDefined(IValueObject valueObject, clsUserVO userVO) //Added By yogesh K 2 6 16
        {
            //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
            //objItemList.
            DbDataReader reader = null;

          
            clsGetRadiologistResultEntryBizActionVO objBizAction = valueObject as clsGetRadiologistResultEntryBizActionVO;
            DbCommand command;

            try
            {
                command = dbServer.GetStoredProcCommand("[CIMS_GetRadiologistResultEntryDefined]");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizAction.TestIDNew);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemSupplierList == null)
                        objBizAction.ItemSupplierList = new List<clsRadiologyVO>();
                    while (reader.Read())
                    {
                        objBizAction.MasterList.Add(new MasterListItem((long)reader["RadiologistID"], reader["Description"].ToString()));
                        //clsRadiologyVO objItemMaster = new clsRadiologyVO();
                        //objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //objItemMaster.Radiologist = (long)DALHelper.HandleDBNull(reader["RadiologistID"]);
                        //objItemMaster.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
                        //objBizAction.ItemSupplierList.Add(objItemMaster);
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
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objBizAction;

        }


        //Added  By Yogesh K 19 5 16

        //public override IValueObject GetPathologistToTempList(IValueObject valueObject, clsUserVO userVO)
        //{
        //    //clsGetItemListBizActionVO objItemList = new clsGetItemListBizActionVO();
        //    //objItemList.
        //    DbDataReader reader = null;

        //    clsAddRadTestMasterBizActionVO objBizAction = valueObject as clsAddRadTestMasterBizActionVO;
        //    DbCommand command;

        //    try
        //    {
        //        command = dbServer.GetStoredProcCommand("CIMS_GetRadiologistListForTest");
        //        dbServer.AddInParameter(command, "ID", DbType.String, objBizAction.ItemSupplier.TemplateID);

        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (objBizAction.ItemSupplierList == null)
        //                objBizAction.ItemSupplierList = new List<clsGetRadTestDetailsBizActionVO>();
        //            while (reader.Read())
        //            {
        //                clsAddRadTestMasterBizActionVO objItemMaster = new clsAddRadTestMasterBizActionVO();
        //                objItemMaster.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
        //                objItemMaster.Pathologist = (long)DALHelper.HandleDBNull(reader["PathologistID"]);
        //                objItemMaster.TemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
        //                objBizAction.ItemSupplierList.Add(objItemMaster);
        //            }
        //        }
        //        reader.NextResult();

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            if (reader.IsClosed == false)
        //                reader.Close();
        //        }
        //    }
        //    return objBizAction;

        //}

        private clsAddRadTemplateMasterBizActionVO UpdateTemplateDetails(clsAddRadTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsRadiologyVO objRadiologyVO = BizActionObj.TemplateDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRadTemplateMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objRadiologyVO.TemplateID);
                dbServer.AddInParameter(command, "Code", DbType.String, objRadiologyVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, objRadiologyVO.Description.Trim());

                dbServer.AddInParameter(command, "Template", DbType.String, objRadiologyVO.TemplateDesign.Trim());
                dbServer.AddInParameter(command, "Radiologist", DbType.Int64, objRadiologyVO.Radiologist);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, objRadiologyVO.GenderID);
                dbServer.AddInParameter(command, "TemplateResultID", DbType.Int64, objRadiologyVO.TemplateResultID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");

                List<MasterListItem> masterList = BizActionObj.TemplateDetails.GenderList;
                if (masterList != null || masterList.Count > 0)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteRadioGenderToTemplate");
                    dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, objRadiologyVO.TemplateID);
                    int intStatus88 = dbServer.ExecuteNonQuery(command2);

                    foreach (var item in masterList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadioGenderToTemplate");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "GenderID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, objRadiologyVO.TemplateID);
                        dbServer.AddInParameter(command1, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
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

        public override IValueObject GetTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTemplateBizActionVO BizActionObj = valueObject as clsGetRadTemplateBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadTemplateList");


                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                dbServer.AddInParameter(command, "Radiologist", DbType.Int64, BizActionObj.Radiologist);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                dbServer.AddInParameter(command, "TemplateResultID", DbType.Int64, BizActionObj.TemplateResultID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
               // dbServer.AddInParameter(command, "RadiologistName", DbType.String, BizActionObj.RadiologistName);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                
              

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TemplateList == null)
                        BizActionObj.TemplateList = new List<clsRadiologyVO>();
                    while (reader.Read())
                    {
                        clsRadiologyVO objRadiologyVO = new clsRadiologyVO();
                        objRadiologyVO.TemplateID = (long)reader["ID"];
                        objRadiologyVO.UnitID = (long)reader["UnitID"];
                        objRadiologyVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objRadiologyVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        //objRadiologyVO.Template = (byte[])DALHelper.HandleDBNull(reader["Template"]);
                        objRadiologyVO.TemplateDesign = (string)DALHelper.HandleDBNull(reader["Template"]);
                        objRadiologyVO.Radiologist = (long)reader["Radiologist"];
                        objRadiologyVO.GenderID = (long)reader["GenderID"];
                        objRadiologyVO.TemplateResultID = (long)reader["TemplateResultID"];
                        objRadiologyVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objRadiologyVO.RadiologistName = (string)DALHelper.HandleDBNull(reader["RadiologistName"]);

                        BizActionObj.TemplateList.Add(objRadiologyVO);
                    }


                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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
        #endregion

        #region Test Master
        public override IValueObject GetTemplateDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTemplateDetailsBizActionVO BizAction = (clsGetRadTemplateDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillRadiologyTemplate");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.TemplateList == null)
                    {
                        BizAction.TemplateList = new List<clsRadiologyVO>();
                    }

                    while (reader.Read())
                    {
                        clsRadiologyVO objRadiologyVO = new clsRadiologyVO();
                        objRadiologyVO.TemplateID = (long)reader["ID"];
                        objRadiologyVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objRadiologyVO.TemplateDesign = (string)DALHelper.HandleDBNull(reader["Template"]);
                        BizAction.TemplateList.Add(objRadiologyVO);

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
            return BizAction;
        }


        public override IValueObject GetServiceList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetRadServiceBizActionVO BizAction = (clsGetRadServiceBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillRadServiceComboBox");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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
            return BizAction;

        }

        public override IValueObject GetItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadItemBizActionVO BizAction = (clsGetRadItemBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillRadItemComboBox");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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
            return BizAction;

        }



        public override IValueObject AddTestMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRadTestMasterBizActionVO BizActionObj = valueObject as clsAddRadTestMasterBizActionVO;

            if (BizActionObj.TestDetails.TestID == 0)
                BizActionObj = AddTest(BizActionObj, UserVo);
            else
                BizActionObj = UpdateTest(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddRadTestMasterBizActionVO AddTest(clsAddRadTestMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsRadiologyVO objRadiologyVO = BizActionObj.TestDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRadTestMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, objRadiologyVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, objRadiologyVO.Description.Trim());
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objRadiologyVO.CategoryID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objRadiologyVO.ServiceID);
                dbServer.AddInParameter(command, "TurnAroundTime", DbType.String, objRadiologyVO.TurnAroundTime);
                dbServer.AddInParameter(command, "PrintTestName", DbType.String, objRadiologyVO.PrintTestName);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objRadiologyVO.TestID);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.TestDetails.TestID = (long)dbServer.GetParameterValue(command, "ID");

                foreach (var ObjTemplate in objRadiologyVO.TestTemplateList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadTestTemplateDetailMaster");


                    dbServer.AddInParameter(command1, "TestID", DbType.Int64, objRadiologyVO.TestID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, ObjTemplate.TestTemplateID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjTemplate.Status);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTemplate.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjTemplate.ID = (long)dbServer.GetParameterValue(command1, "ID");


                }
                foreach (var ObjItem in objRadiologyVO.TestItemList)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddRadTestItemDetailMaster");


                    dbServer.AddInParameter(command2, "TestID", DbType.Int64, objRadiologyVO.TestID);
                    dbServer.AddInParameter(command2, "ItemID", DbType.Int64, ObjItem.ItemID);
                    dbServer.AddInParameter(command2, "Quantity", DbType.Double, ObjItem.Quantity);

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, ObjItem.Status);
                    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjItem.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                    ObjItem.ID = (long)dbServer.GetParameterValue(command2, "ID");

                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;

            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.TestDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;

            }
            return BizActionObj;
        }


        private clsAddRadTestMasterBizActionVO UpdateTest(clsAddRadTestMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsRadiologyVO objRadiologyVO = BizActionObj.TestDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRadTestMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objRadiologyVO.TestID);
                dbServer.AddInParameter(command, "Code", DbType.String, objRadiologyVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, objRadiologyVO.Description.Trim());
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "CategoryID", DbType.Int64, objRadiologyVO.CategoryID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objRadiologyVO.ServiceID);
                dbServer.AddInParameter(command, "TurnAroundTime", DbType.String, objRadiologyVO.TurnAroundTime);
                dbServer.AddInParameter(command, "PrintTestName", DbType.String, objRadiologyVO.PrintTestName);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (objRadiologyVO.TestTemplateList != null && objRadiologyVO.TestTemplateList.Count > 0)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteTestTemplate");

                    dbServer.AddInParameter(command3, "TestID", DbType.Int64, objRadiologyVO.TestID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
                }

                foreach (var ObjTemplate in objRadiologyVO.TestTemplateList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadTestTemplateDetailMaster");


                    dbServer.AddInParameter(command1, "TestID", DbType.Int64, objRadiologyVO.TestID);
                    dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, ObjTemplate.TestTemplateID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjTemplate.Status);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjTemplate.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                    ObjTemplate.ID = (long)dbServer.GetParameterValue(command1, "ID");


                }


                if (objRadiologyVO.TestItemList != null && objRadiologyVO.TestItemList.Count > 0)
                {
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_DeleteTestItem");

                    dbServer.AddInParameter(command4, "TestID", DbType.Int64, objRadiologyVO.TestID);
                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                }
                foreach (var ObjItem in objRadiologyVO.TestItemList)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddRadTestItemDetailMaster");


                    dbServer.AddInParameter(command2, "TestID", DbType.Int64, objRadiologyVO.TestID);
                    dbServer.AddInParameter(command2, "ItemID", DbType.Int64, ObjItem.ItemID);
                    dbServer.AddInParameter(command2, "Quantity", DbType.Double, ObjItem.Quantity);

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, ObjItem.Status);
                    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjItem.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                    ObjItem.ID = (long)dbServer.GetParameterValue(command2, "ID");

                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;

            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.TestDetails = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;

            }
            return BizActionObj;
        }

      

        public override IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTestDetailsBizActionVO BizActionObj = valueObject as clsGetRadTestDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadTestList");

                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                dbServer.AddInParameter(command, "Category", DbType.Int64, BizActionObj.Category);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsRadiologyVO>();
                    while (reader.Read())
                    {
                        clsRadiologyVO objRadiologyVO = new clsRadiologyVO();
                        objRadiologyVO.TestID = (long)reader["ID"];
                        objRadiologyVO.UnitID = (long)reader["UnitID"];
                        objRadiologyVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objRadiologyVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objRadiologyVO.TurnAroundTime = (string)DALHelper.HandleDBNull(reader["TurnAroundTime"]);
                        objRadiologyVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objRadiologyVO.CategoryID = (long)reader["CategoryID"];
                        objRadiologyVO.ServiceID = (long)reader["ServiceID"];
                        objRadiologyVO.PrintTestName = (string)DALHelper.HandleDBNull(reader["PrintTestName"]);
                        objRadiologyVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


                        BizActionObj.TestList.Add(objRadiologyVO);
                    }


                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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
            //clsGetRadTestDetailsBizActionVO BizActionObj = valueObject as clsGetRadTestDetailsBizActionVO;
            //try
            //{
            //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadTestListDetails");


            //    dbServer.AddInParameter(command, "ROBID", DbType.Int64, BizActionObj.robID);
            //    dbServer.AddInParameter(command, "ROBUnitID", DbType.Int64, BizActionObj.robUnitID);


            //    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            //    if (reader.HasRows)
            //    {
            //        if (BizActionObj.TestList == null)
            //            BizActionObj.TestList = new List<clsRadiologyVO>();
            //        while (reader.Read())
            //        {
            //            clsRadiologyVO objRadiologyVO = new clsRadiologyVO();
            //            objRadiologyVO.TestID = (long)reader["ID"];
            //            objRadiologyVO.ID = (long)reader["ROBDID"];
            //            //objRadiologyVO.UnitID = (long)reader["UnitID"];
            //            //objRadiologyVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
            //            //objRadiologyVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
            //            //objRadiologyVO.TurnAroundTime = (string)DALHelper.HandleDBNull(reader["TurnAroundTime"]);
            //            //objRadiologyVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
            //            //objRadiologyVO.CategoryID = (long)reader["CategoryID"];
            //            //objRadiologyVO.ServiceID = (long)reader["ServiceID"];
            //            //objRadiologyVO.PrintTestName = (string)DALHelper.HandleDBNull(reader["PrintTestName"]);
            //            //objRadiologyVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


            //            BizActionObj.TestList.Add(objRadiologyVO);
            //        }


            //    }
            //    reader.NextResult();
            //  //  BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            //    reader.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //finally
            //{
            //}
            //return BizActionObj;
        }



        //public override IValueObject GetTestListRadiologyOrderDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetRadTestDetailsBizActionVO BizActionObj = valueObject as clsGetRadTestDetailsBizActionVO;
        //    try
        //    {
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadTestListDetails");


        //        dbServer.AddInParameter(command, "ROBID", DbType.Int64, BizActionObj.robID);
        //        dbServer.AddInParameter(command, "ROBUnitID", DbType.Int64, BizActionObj.robUnitID);
             

        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.TestList == null)
        //                BizActionObj.TestList = new List<clsRadiologyVO>();
        //            while (reader.Read())
        //            {
        //                clsRadiologyVO objRadiologyVO = new clsRadiologyVO();
        //                objRadiologyVO.TestID = (long)reader["ID"];
        //                objRadiologyVO.ID = (long)reader["ROBDID"];
        //                //objRadiologyVO.UnitID = (long)reader["UnitID"];
        //                //objRadiologyVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
        //                //objRadiologyVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
        //                //objRadiologyVO.TurnAroundTime = (string)DALHelper.HandleDBNull(reader["TurnAroundTime"]);
        //                //objRadiologyVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
        //                //objRadiologyVO.CategoryID = (long)reader["CategoryID"];
        //                //objRadiologyVO.ServiceID = (long)reader["ServiceID"];
        //                //objRadiologyVO.PrintTestName = (string)DALHelper.HandleDBNull(reader["PrintTestName"]);
        //                //objRadiologyVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


        //                BizActionObj.TestList.Add(objRadiologyVO);
        //            }


        //        }
        //        reader.NextResult();
        //        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
        //        reader.Close();
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


        public override IValueObject GetTemplateAndItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTemplateAndItemByTestIDBizActionVO BizActionObj = (clsGetRadTemplateAndItemByTestIDBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTemplateAndItemDetails");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizActionObj.TestID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    if (BizActionObj.TestList == null)
                        BizActionObj.TestList = new List<clsRadTemplateDetailMasterVO>();

                    while (reader.Read())
                    {
                        clsRadTemplateDetailMasterVO ObjTemp = new clsRadTemplateDetailMasterVO();
                        ObjTemp.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjTemp.TemplateTestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        ObjTemp.TestTemplateID = (long)DALHelper.HandleDBNull(reader["TemplateID"]);
                        ObjTemp.TemplateCode = (string)DALHelper.HandleDBNull(reader["Code"]);
                        ObjTemp.TemplateName = (string)DALHelper.HandleDBNull(reader["Description"]);
                        ObjTemp.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.TestList.Add(ObjTemp);
                    }
                    reader.NextResult();

                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsRadItemDetailMasterVO>();

                    while (reader.Read())
                    {
                        clsRadItemDetailMasterVO ObjItem = new clsRadItemDetailMasterVO();
                        ObjItem.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjItem.ItemTestID = (long)DALHelper.HandleDBNull(reader["TestID"]);
                        ObjItem.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        ObjItem.Quantity = (double)DALHelper.HandleDBNull(reader["Quantity"]);
                        ObjItem.ActualQantity = (double)DALHelper.HandleDBNull(reader["Quantity"]);
                        ObjItem.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        ObjItem.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        ObjItem.BatchesRequired = (bool)DALHelper.HandleDBNull(reader["BatchesRequired"]);

                        BizActionObj.ItemList.Add(ObjItem);
                    }


                }
            }
            catch (Exception ex)
            {
            }

            return BizActionObj;

        }

        public override IValueObject ViewTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadViewTemplateBizActionVO BizActionObj = (clsGetRadViewTemplateBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ViewRadTemplate");
                DbDataReader reader;
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                //dbServer.AddInParameter(command, "RadiologistID", DbType.Int64, BizActionObj.RadiologistID);
                //dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                //dbServer.AddInParameter(command, "ResultID", DbType.Int64, BizActionObj.ResultID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    if (BizActionObj.Template == null)
                        BizActionObj.Template = new clsRadiologyVO();
                    while (reader.Read())
                    {
                        BizActionObj.Template.TemplateID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.Template.TemplateDesign = (string)DALHelper.HandleDBNull(reader["Template"]);
                        BizActionObj.Template.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }


        #endregion

        #region Order Generation
        public override IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo)
        {


            clsAddRadOrderBookingBizActionVO BizActionObj = valueObject as clsAddRadOrderBookingBizActionVO;

            if (BizActionObj.BookingDetails.ID == 0)
                BizActionObj = AddOrder(BizActionObj, UserVo, null, null);
            else
                BizActionObj = UpdateOrder(BizActionObj, UserVo, null, null);
                //BizActionObj = UpdateOrder(BizActionObj, UserVo);


            return valueObject;
        }

        public override IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddRadOrderBookingBizActionVO BizActionObj = valueObject as clsAddRadOrderBookingBizActionVO;

            if (BizActionObj.BookingDetails.ID == 0)
                BizActionObj = AddOrder(BizActionObj, UserVo, pConnection, pTransaction);
               
            else
                BizActionObj = UpdateOrder(BizActionObj, UserVo, null, null);
                //BizActionObj = UpdateOrder(BizActionObj, UserVo);


            return valueObject;
        }

        private clsAddRadOrderBookingBizActionVO AddOrder(clsAddRadOrderBookingBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                clsRadOrderBookingVO objBookingVO = BizActionObj.BookingDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRadOrderBooking");

                dbServer.AddInParameter(command, "BillID", DbType.String, objBookingVO.BillID);
                dbServer.AddInParameter(command, "BillNo", DbType.String, objBookingVO.BillNo);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objBookingVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objBookingVO.Date);
                dbServer.AddInParameter(command, "Opd_Ipd_External_ID", DbType.Int64, objBookingVO.Opd_Ipd_External_ID);
                dbServer.AddInParameter(command, "Opd_Ipd_External_UnitID", DbType.Int64, objBookingVO.Opd_Ipd_External_UnitID);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, objBookingVO.Opd_Ipd_External);
                dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, objBookingVO.IsCancelled);
                dbServer.AddInParameter(command, "TestType", DbType.Int64, objBookingVO.TestType);
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, objBookingVO.IsApproved);
                dbServer.AddInParameter(command, "IsResultEntry", DbType.Boolean, objBookingVO.IsResultEntry);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objBookingVO.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "OrderNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000");
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBookingVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //int intStatus = dbServer.ExecuteNonQuery(command);
                int intStatus = 0;


                intStatus = dbServer.ExecuteNonQuery(command, trans);



                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.BookingDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.BookingDetails.OrderNo = (string)dbServer.GetParameterValue(command, "OrderNo");

                if (objBookingVO.OrderBookingDetails != null && objBookingVO.OrderBookingDetails.Count > 0)
                {
                    foreach (var ObjOrderDetails in objBookingVO.OrderBookingDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadOrderBookingDetails");

                        dbServer.AddInParameter(command1, "RadOrderID", DbType.Int64, objBookingVO.ID);
                        dbServer.AddInParameter(command1, "TestID", DbType.Int64, ObjOrderDetails.TestID);
                        dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, ObjOrderDetails.ChargeID);
                        dbServer.AddInParameter(command1, "TariffServiceId", DbType.Int64, ObjOrderDetails.TariffServiceID);
                        if (ObjOrderDetails.Number != null)
                            dbServer.AddInParameter(command1, "Number", DbType.String, ObjOrderDetails.Number.Trim());
                        dbServer.AddInParameter(command1, "IsEmergency", DbType.Boolean, ObjOrderDetails.IsEmergency);
                        dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjOrderDetails.DoctorID);
                        dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, ObjOrderDetails.IsApproved);
                        dbServer.AddInParameter(command1, "IsCompleted", DbType.Boolean, ObjOrderDetails.IsCompleted);
                        dbServer.AddInParameter(command1, "IsDelivered", DbType.Boolean, ObjOrderDetails.IsDelivered);
                        dbServer.AddInParameter(command1, "IsPrinted", DbType.Boolean, ObjOrderDetails.IsPrinted);
                        dbServer.AddInParameter(command1, "MicrobiologistId", DbType.Int64, ObjOrderDetails.MicrobiologistID);

                        if (ObjOrderDetails.ReferredBy != null)
                            dbServer.AddInParameter(command1, "ReferredBy", DbType.String, ObjOrderDetails.ReferredBy.Trim());
                        dbServer.AddInParameter(command1, "IsResultEntry", DbType.Boolean, ObjOrderDetails.IsResultEntry);
                        dbServer.AddInParameter(command1, "IsFinalized", DbType.Boolean, ObjOrderDetails.IsFinalized);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objBookingVO.Status);
                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjOrderDetails.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        //int intStatus1 = dbServer.ExecuteNonQuery(command1);



                        intStatus = dbServer.ExecuteNonQuery(command1, trans);


                        ObjOrderDetails.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        ObjOrderDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");


                    }
                }
                BizActionObj.SuccessStatus = 0;

                if (pConnection == null) trans.Commit();
            }
            catch (Exception ex)
            {
                //
                string err = ex.Message;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }

            return BizActionObj;
        }

        private clsAddRadOrderBookingBizActionVO UpdateOrder(clsAddRadOrderBookingBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                clsRadOrderBookingVO objBookingVO = BizActionObj.BookingDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRadOrderBooking");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objBookingVO.ID);
                dbServer.AddInParameter(command, "OrderNo", DbType.String, objBookingVO.OrderNo);
                dbServer.AddInParameter(command, "BillNo", DbType.String, objBookingVO.BillNo);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objBookingVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objBookingVO.Date);
                dbServer.AddInParameter(command, "Opd_Ipd_External_ID", DbType.Int64, objBookingVO.Opd_Ipd_External_ID);
                dbServer.AddInParameter(command, "Opd_Ipd_External_UnitID", DbType.Int64, objBookingVO.Opd_Ipd_External_UnitID);
                dbServer.AddInParameter(command, "Opd_Ipd_External", DbType.Int64, objBookingVO.Opd_Ipd_External);
                dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, objBookingVO.IsCancelled);
                dbServer.AddInParameter(command, "TestType", DbType.Int64, objBookingVO.TestType);
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, objBookingVO.IsApproved);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objBookingVO.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = 0;
                intStatus = dbServer.ExecuteNonQuery(command, trans);


                if (objBookingVO.OrderBookingDetails != null && objBookingVO.OrderBookingDetails.Count > 0)
                {
                    foreach (var ObjOrderDetails in objBookingVO.OrderBookingDetails)
                    {
                        //if (objBookingVO.ID> 0)
                      
                        if (ObjOrderDetails.ID > 0)
                          //   obj.BookingDetails.ID = BizActionObj.Details.ID;
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateRadOrderBookingDetails");

                            dbServer.AddInParameter(command1, "ID", DbType.Int64, ObjOrderDetails.ID);
                            dbServer.AddInParameter(command1, "RadOrderID", DbType.Int64, objBookingVO.ID);
                            dbServer.AddInParameter(command1, "TestID", DbType.Int64, ObjOrderDetails.TestID);
                            dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, ObjOrderDetails.ChargeID);
                            dbServer.AddInParameter(command1, "TariffServiceId", DbType.Int64, ObjOrderDetails.TariffServiceID);
                            if (ObjOrderDetails.Number != null)
                                dbServer.AddInParameter(command1, "Number", DbType.String, ObjOrderDetails.Number.Trim());
                            dbServer.AddInParameter(command1, "IsEmergency", DbType.Boolean, ObjOrderDetails.IsEmergency);
                            dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjOrderDetails.DoctorID);
                            dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, ObjOrderDetails.IsApproved);
                            dbServer.AddInParameter(command1, "IsCompleted", DbType.Boolean, ObjOrderDetails.IsCompleted);
                            dbServer.AddInParameter(command1, "IsDelivered", DbType.Boolean, ObjOrderDetails.IsDelivered);
                            dbServer.AddInParameter(command1, "IsPrinted", DbType.Boolean, ObjOrderDetails.IsPrinted);
                            dbServer.AddInParameter(command1, "MicrobiologistId", DbType.Int64, ObjOrderDetails.MicrobiologistID);

                            if (ObjOrderDetails.ReferredBy != null)
                                dbServer.AddInParameter(command1, "ReferredBy", DbType.String, ObjOrderDetails.ReferredBy.Trim());

                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, objBookingVO.Status);
                            dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                            dbServer.AddInParameter(command1, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                            //int intStatus1 = dbServer.ExecuteNonQuery(command1);
                            int intStatus1 = 0;

                            intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        }
                        else if (ObjOrderDetails.ID == 0)  //else if (objBookingVO.ID == 0)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadOrderBookingDetails");

                            dbServer.AddInParameter(command1, "RadOrderID", DbType.Int64, objBookingVO.ID);
                            dbServer.AddInParameter(command1, "TestID", DbType.Int64, ObjOrderDetails.TestID);
                            dbServer.AddInParameter(command1, "ChargeID", DbType.Int64, ObjOrderDetails.ChargeID);
                            dbServer.AddInParameter(command1, "TariffServiceId", DbType.Int64, ObjOrderDetails.TariffServiceID);
                            if (ObjOrderDetails.Number != null)
                                dbServer.AddInParameter(command1, "Number", DbType.String, ObjOrderDetails.Number.Trim());
                            dbServer.AddInParameter(command1, "IsEmergency", DbType.Boolean, ObjOrderDetails.IsEmergency);
                            dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, ObjOrderDetails.DoctorID);
                            dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, ObjOrderDetails.IsApproved);
                            dbServer.AddInParameter(command1, "IsCompleted", DbType.Boolean, ObjOrderDetails.IsCompleted);
                            dbServer.AddInParameter(command1, "IsDelivered", DbType.Boolean, ObjOrderDetails.IsDelivered);
                            dbServer.AddInParameter(command1, "IsPrinted", DbType.Boolean, ObjOrderDetails.IsPrinted);
                            dbServer.AddInParameter(command1, "MicrobiologistId", DbType.Int64, ObjOrderDetails.MicrobiologistID);
                            //dbServer.AddInParameter(command1, "IsOutSourced", DbType.Boolean, ObjOrderDetails.IsOutSourced);
                            //dbServer.AddInParameter(command1, "AgencyID", DbType.Int64, ObjOrderDetails.AgencyID);
                            //dbServer.AddInParameter(command1, "InvestigationDate", DbType.DateTime, ObjOrderDetails.InvestigationDate);
                            //dbServer.AddInParameter(command1, "InvestigationTime", DbType.DateTime, ObjOrderDetails.InvestigationTime);
                            if (ObjOrderDetails.ReferredBy != null)
                                dbServer.AddInParameter(command1, "ReferredBy", DbType.String, ObjOrderDetails.ReferredBy.Trim());
                            dbServer.AddInParameter(command1, "IsResultEntry", DbType.Boolean, ObjOrderDetails.IsResultEntry);
                            dbServer.AddInParameter(command1, "IsFinalized", DbType.Boolean, ObjOrderDetails.IsFinalized);
                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, objBookingVO.Status);
                            dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                            //dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, ObjOrderDetails.ServiceID);
                            //dbServer.AddInParameter(command1, "TariffId", DbType.Int64, ObjOrderDetails.TariffId);

                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjOrderDetails.ID);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                            //int intStatus1 = dbServer.ExecuteNonQuery(command1);
                            int intStatus1 = 0;

                            intStatus1 = dbServer.ExecuteNonQuery(command1, trans);


                            ObjOrderDetails.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                            ObjOrderDetails.ID = (long)dbServer.GetParameterValue(command1, "ID");
                        }

                    }
                }
                BizActionObj.SuccessStatus = 0;

                if (pConnection == null) trans.Commit();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();

            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizActionObj;
        }

       


       
        #endregion

        #region Result Entry
        public override IValueObject FillTestComboBox(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillTestComboBoxBizActionVO BizAction = (clsFillTestComboBoxBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillTestComboBoxInResultEntry");


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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
            return BizAction;
        }


      

   

       

      

    

        public override IValueObject FillTemplateComboBox(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillTemplateComboBoxForResultEntryBizActionVO BizAction = (clsFillTemplateComboBoxForResultEntryBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillTemplateComboBoxInResultEntry");

                DbDataReader reader;
                dbServer.AddInParameter(command, "TestID", DbType.Int64, BizAction.TestID);
                dbServer.AddInParameter(command, "Radiologist", DbType.Int64, BizAction.Radiologist);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizAction.GenderID);
                dbServer.AddInParameter(command, "TemplateResultID", DbType.Int64, BizAction.TemplateResultID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizAction.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
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
            return BizAction;
        }

        #endregion

        #region For Radiology Additions

        public override IValueObject UpdateRadOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateRadOrderBookingDetailListBizActionVO BizAction = valueObject as clsUpdateRadOrderBookingDetailListBizActionVO;
            bool flag = false;
            int count = BizAction.OrderBookingDetailList.Count;
            try
            {

                foreach (var item in BizAction.OrderBookingDetailList)
                {

                    if (item.IsReportCollected)
                    {

                        flag = true;
                        clsRadOrderBookingDetailsVO objVO = BizAction.OrderBookingDetaildetails;
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRadOrderBookingDetailsForAgencyRepSchedule");

                        //dbServer.AddInParameter(command, "LinkServer", DbType.String, objVO.LinkServer);
                        //if (objVO.LinkServer != null)
                        //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objVO.LinkServer.Replace(@"\", "_"));

                        dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                        //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                        //dbServer.AddInParameter(command, "SampleNo", DbType.Int64, item.SampleNo);
                        dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, item.IsOutSourced);
                        dbServer.AddInParameter(command, "ExtAgencyID", DbType.Int64, item.AgencyID);
                        //dbServer.AddInParameter(command, "Quantity", DbType.Double, item.Quantity);
                        dbServer.AddInParameter(command, "ReportCollected", DbType.DateTime, BizAction.ReportCollectionDate);
                        dbServer.AddInParameter(command, "IsReportCollected", DbType.Boolean, item.IsReportCollected);

                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        //dbServer.AddInParameter(command, "SampleCollectedDateTime", DbType.DateTime, item.SampleCollectedDateTime);
                        //dbServer.AddInParameter(command, "SampleReceivedDateTime", DbType.DateTime, item.SampleReceivedDateTime);
                        //dbServer.AddInParameter(command, "SampleCollectionCenter", DbType.String, item.SampleCollectionCenter);

                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                    }

                }
            }
            catch (Exception)
            {

            }

            return valueObject;
        }

        public override IValueObject GetRadologyPringHeaderFooterImage(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadologyPringHeaderFooterImageBizActionVO BizActionObj = valueObject as clsGetRadologyPringHeaderFooterImageBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadiologyHeaderFooterImageForPrint");

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.HeaderImage = (Byte[])DALHelper.HandleDBNull(reader["HeaderImage"]);
                        BizActionObj.FooterImage = (Byte[])DALHelper.HandleDBNull(reader["FooterImage"]);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

    

        public override IValueObject GetRadiologistbySpecia(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadiologistBySpecializationBizActionVO BizActionObj = valueObject as clsGetRadiologistBySpecializationBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillRadiologistinResultEntrySpecia");
                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.SpecializationID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
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
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetRadiologistGenderByitsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadiologistGenderByIDBizActionVO BizActionObj = valueObject as clsGetRadiologistGenderByIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillRadiologistGender");
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
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
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString()));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddRadDetilsForEmail(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsRadPathPatientReportDetailsForEmailSendingBizActionVO BizAction = valueObject as clsRadPathPatientReportDetailsForEmailSendingBizActionVO;

            if (BizAction.RadOrderBookingDetailList.Count > 0)
            {
                try
                {
                    con = dbServer.CreateConnection();
                    con.Open();
                    trans = con.BeginTransaction();
                    DbCommand command = null;
                    foreach (var item in BizAction.RadOrderBookingDetailList)
                    {


                        command = dbServer.GetStoredProcCommand("CIMS_AddRadReportEmailDetailsforSendingEmail");
                        command.Connection = con;
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                        dbServer.AddInParameter(command, "OrderID", DbType.Int64, item.RadOrderID);
                        dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command, "PathPatientReportID", DbType.Int64, item.ResultEntryID);
                        dbServer.AddInParameter(command, "PatientEmailID", DbType.String, BizAction.PatientEmailID);
                        dbServer.AddInParameter(command, "DoctorEmailID", DbType.String, BizAction.DoctorEmailID);
                        dbServer.AddInParameter(command, "Status", DbType.Int64, false);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                        dbServer.AddInParameter(command, "PatientName", DbType.String, BizAction.PatientName);
                        dbServer.AddInParameter(command, "Report", DbType.Binary, item.Report);
                        dbServer.AddInParameter(command, "Body", DbType.String, null);
                        dbServer.AddInParameter(command, "Subject", DbType.String, null);
                        dbServer.AddInParameter(command, "SourceURL", DbType.String, item.SourceURL);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);
                        dbServer.AddParameter(command, "AttachmentID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.AttachmentID);

                        int intStatus = dbServer.ExecuteNonQuery(command, trans);
                        BizAction.AttachmentID = (long)dbServer.GetParameterValue(command, "AttachmentID");
                    }

                    trans.Commit();
                    con.Close();
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    BizAction.RadOrderBookingDetailList = null;

                    throw ex;
                }


                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizAction;
        }

        public override IValueObject UpdateEmailDelivredIntoList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateRadOrderBookingDetailDeliveryStatusBizActionVO objItem = valueObject as clsUpdateRadOrderBookingDetailDeliveryStatusBizActionVO;
            //if (objItem.PathOrderBookList.Count > 0)
            //{
            //    try
            //    {

            //        List<clsRadOrderBookingDetailsVO> objItemVO = objItem.PathOrderBookList;
            //        int count = objItemVO.Count;
            //        DbCommand command = null;

            //        for (int i = 0; i < count; i++)
            //        {

            //            //   clsPathOrderBookingDetailVO objItemVO = objItem.PathOrderBookingDetail;
            //            command = dbServer.GetStoredProcCommand("CIMS_UpdateEmailDeliveryStatusinPathDetails");
            //            dbServer.AddInParameter(command, "PathOrderBookingDetailID", DbType.Int64, objItemVO[i].ID);
            //           // dbServer.AddInParameter(command, "OrderID", DbType.Int64, objItemVO[i].OrderBookingID);
            //     //       dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO[i].UnitId);
            //            dbServer.AddInParameter(command, "IsDeliverdthroughEmail", DbType.Boolean, objItemVO[i].IsDeliverdthroughEmail);
            //            dbServer.AddInParameter(command, "EmailDeliverdDateTime", DbType.DateTime, null);


            //            dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

            //            int intStatus = dbServer.ExecuteNonQuery(command);

            //            objItem.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        throw ex;
            //    }
            //}
            return objItem;
        }
        public override IValueObject GetTechnicianEntryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadTechnicianEntryListBizActionVO BizActionObj = valueObject as clsGetRadTechnicianEntryListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadiologyTechnicianEntryList");


                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "RadOrderID", DbType.Int64, BizActionObj.RadOrderID);
                dbServer.AddInParameter(command, "OrderDetailID", DbType.Int64, BizActionObj.OrderDetailID);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, BizActionObj.BillID);
                dbServer.AddInParameter(command, "ChargeID", DbType.Int64, BizActionObj.ChargeID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.BookingList == null)
                        BizActionObj.BookingList = new List<clsRadOrderBookingVO>();
                    while (reader.Read())
                    {
                        clsRadOrderBookingVO objRadiologyVO = new clsRadOrderBookingVO();
                        objRadiologyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objRadiologyVO.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                        objRadiologyVO.Date = (DateTime)(DALHelper.HandleDBNull(reader["Date"]));
                        objRadiologyVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objRadiologyVO.PatientUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitId"]));


                        objRadiologyVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        objRadiologyVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objRadiologyVO.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        objRadiologyVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));

                        objRadiologyVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objRadiologyVO.Balance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));
                        objRadiologyVO.Gender = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        switch (objRadiologyVO.Gender)
                        {
                            case 1:
                                objRadiologyVO.GenderName = "Male";
                                break;
                            case 2:
                                objRadiologyVO.GenderName = "Female";
                                break;
                        }
                        objRadiologyVO.ReferredDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        objRadiologyVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                        objRadiologyVO.IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]));
                        objRadiologyVO.IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"]));

                        objRadiologyVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objRadiologyVO.Service = Convert.ToString(DALHelper.HandleDBNull(reader["Service"]));

                        /*
                        objRadiologyVO.PaymentModeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PaymentModeID"]);
                        switch (objRadiologyVO.PaymentModeID)
                        {
                            case 1:
                                objRadiologyVO.PaymentMode = "Cash";
                                break;
                            case 2:
                                objRadiologyVO.PaymentMode = "Cheque";
                                break;
                            case 3:
                                objRadiologyVO.PaymentMode = "DD";
                                break;
                            case 4:
                                objRadiologyVO.PaymentMode = "CreditCard";
                                break;
                            case 5:
                                objRadiologyVO.PaymentMode = "DebitCard";
                                break;
                            case 6:
                                objRadiologyVO.PaymentMode = "StaffFree";
                                break;
                            case 7:
                                objRadiologyVO.PaymentMode = "CompanyAdvance";
                                break;
                            case 8:
                                objRadiologyVO.PaymentMode = "PatientAdvance";
                                break;
                            case 9:
                                objRadiologyVO.PaymentMode = "Credit";
                                break;
                        }
                         */

                        objRadiologyVO.Age = Convert.ToInt32(DALHelper.HandleDBNull(reader["Age"]));
                        objRadiologyVO.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        objRadiologyVO.TestID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TestID"]));
                        objRadiologyVO.IsTechnicianEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntry"]));
                        objRadiologyVO.IsTechnicianEntryFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntryFinalized"]));
                        objRadiologyVO.Contrast = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Contrast"]));
                        objRadiologyVO.ContrastDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ContrastDetails"]));
                        objRadiologyVO.FilmWastage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FilmWastage"]));
                        objRadiologyVO.FilmWastageDetails = Convert.ToString(DALHelper.HandleDBNull(reader["WastageDetails"]));
                        objRadiologyVO.NoOfFilms = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NoOfFlims"]));
                        objRadiologyVO.Sedation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Sedation"]));
                        objRadiologyVO.IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"]));
                        objRadiologyVO.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        BizActionObj.BookingList.Add(objRadiologyVO);
                    }
                }
                reader.Close();
            }
            catch
            {

            }
            finally
            {

            }
            return BizActionObj;
        }


        public override IValueObject AddResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddRadResultEntryBizActionVO BizActionObj = valueObject as clsAddRadResultEntryBizActionVO;

            if (BizActionObj.ResultDetails.ID > 0)
                BizActionObj = UpdateResult(BizActionObj, UserVo); 
            else
                BizActionObj = AddResult(BizActionObj, UserVo);

            return valueObject;

        }
        private clsAddRadResultEntryBizActionVO AddResult(clsAddRadResultEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsRadResultEntryVO objResultVO = BizActionObj.ResultDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRadResultEntryMilan");

                dbServer.AddInParameter(command, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, objResultVO.TestID);
                dbServer.AddInParameter(command, "FlimId", DbType.Int64, objResultVO.FilmID);
                //dbServer.AddInParameter(command, "NoOfFlims", DbType.Int64, objResultVO.NoOfFilms);
                dbServer.AddInParameter(command, "RadiologistID1", DbType.Int64, objResultVO.RadiologistID1);
                dbServer.AddInParameter(command, "FirstLevelDescription", DbType.String, objResultVO.FirstLevelDescription);
                dbServer.AddInParameter(command, "TemplateResultID", DbType.Int64, objResultVO.TemplateResultID);
                dbServer.AddInParameter(command, "RadiologistID2", DbType.Int64, objResultVO.RadiologistID2);
                dbServer.AddInParameter(command, "RadiologistID3", DbType.Int64, objResultVO.RadiologistID3);
                dbServer.AddInParameter(command, "SecondLevelDescription", DbType.String, objResultVO.SecondLevelDescription);
                dbServer.AddInParameter(command, "ThirdLevelDescription", DbType.String, objResultVO.ThirdLevelDescription);
                dbServer.AddInParameter(command, "FirstLevelId", DbType.Int64, objResultVO.FirstLevelId);
                dbServer.AddInParameter(command, "SecondLevelId", DbType.Int64, objResultVO.SecondLevelId);
                dbServer.AddInParameter(command, "ThirdLevelId", DbType.Int64, objResultVO.ThirdLevelId);
                dbServer.AddInParameter(command, "TemplateId", DbType.Int64, objResultVO.TemplateID);
                dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, objResultVO.IsOutSourced);
                dbServer.AddInParameter(command, "AgencyId", DbType.Int64, objResultVO.AgencyId);

                dbServer.AddInParameter(command, "ReferredBy", DbType.String, objResultVO.ReferredBy.Trim());

                dbServer.AddInParameter(command, "IsFinalized", DbType.Boolean, objResultVO.IsFinalized);
                dbServer.AddInParameter(command, "IsResultEntry", DbType.Boolean, objResultVO.IsResultEntry);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //Added By Yogita
                dbServer.AddInParameter(command, "SourceURL", DbType.String, objResultVO.SourceURL);
                dbServer.AddInParameter(command, "Report", DbType.Binary, objResultVO.Report);
                dbServer.AddInParameter(command, "Notes", DbType.String, objResultVO.Notes);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objResultVO.Remarks);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objResultVO.Time);
                dbServer.AddInParameter(command, "IsCompleted", DbType.Boolean, objResultVO.IsCompleted);
                dbServer.AddInParameter(command, "IsUpload", DbType.Boolean, BizActionObj.IsUpload);
                if (objResultVO.IsRefDoctorSigniture == true)
                {
                    dbServer.AddInParameter(command, "IsDigitalSignatureRequired", DbType.Boolean, objResultVO.IsRefDoctorSigniture);
                    dbServer.AddInParameter(command, "DoctorUserID", DbType.Int64, objResultVO.RadiologistID1);
                }
                else
                {
                    dbServer.AddInParameter(command, "IsDigitalSignatureRequired", DbType.Boolean, null);
                    dbServer.AddInParameter(command, "DoctorUserID", DbType.Int64, null);
                }
                //End


                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objResultVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ResultDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                //if (objResultVO.TestItemList != null && objResultVO.TestItemList.Count > 0)
                //{
                //    foreach (var ItemList in objResultVO.TestItemList)
                //    {
                //        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadResultItemDetails");

                //        dbServer.AddInParameter(command1, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                //        dbServer.AddInParameter(command1, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                //        dbServer.AddInParameter(command1, "RadTechnicianEntryID", DbType.Int64, objResultVO.ID);
                //        dbServer.AddInParameter(command1, "TestID", DbType.Int64, objResultVO.TestID);
                //        dbServer.AddInParameter(command1, "StoreID", DbType.Int64, ItemList.StoreID);
                //        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, ItemList.ItemID);
                //        dbServer.AddInParameter(command1, "BatchID", DbType.Int64, ItemList.BatchID);
                //        dbServer.AddInParameter(command1, "IdealQuantity", DbType.Double, ItemList.Quantity);
                //        dbServer.AddInParameter(command1, "ActualQantity", DbType.Double, ItemList.ActualQantity);
                //        dbServer.AddInParameter(command1, "BalQantity", DbType.Double, ItemList.BalanceQuantity);
                //        dbServer.AddInParameter(command1, "WastageQuantity", DbType.Double, ItemList.WastageQantity);
                //        dbServer.AddInParameter(command1, "ItemCategoryID", DbType.Int64, ItemList.ItemCategoryID);

                //        dbServer.AddInParameter(command1, "ExpiryDate ", DbType.DateTime, ItemList.ExpiryDate);
                //        dbServer.AddInParameter(command1, "Remarks", DbType.String, ItemList.Remarks);


                //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                //        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                //        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ItemList.ID);
                //        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                //        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //        ItemList.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                //        ItemList.ID = (long)dbServer.GetParameterValue(command1, "ID");

                //        if (objResultVO.IsFinalized == true)
                //        {
                //            if (BizActionObj.AutoDeductStock == true)
                //            {

                //                clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                //                clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
                //                clsItemStockVO StockDetails = new clsItemStockVO();

                //                StockDetails.ItemID = ItemList.ItemID;
                //                StockDetails.BatchID = ItemList.BatchID;
                //                StockDetails.TransactionTypeID = InventoryTransactionType.RadiologyTestConsumption;
                //                StockDetails.TransactionQuantity = ItemList.Quantity;
                //                StockDetails.TransactionID = BizActionObj.ResultDetails.ID;
                //                StockDetails.Date = DateTime.Now;
                //                StockDetails.Time = DateTime.Now;
                //                StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                //                StockDetails.StoreID = ItemList.StoreID;
                //                obj.Details = StockDetails;
                //                obj.Details.ID = 0;

                //                obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                //                if (obj.SuccessStatus == -1)
                //                {
                //                    throw new Exception();
                //                }

                //                StockDetails.ID = obj.Details.ID;

                //            }
                //        }
                //    }
                //}
                trans.Commit();
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.ResultDetails = null;

            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        private clsAddRadResultEntryBizActionVO UpdateResult(clsAddRadResultEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsRadResultEntryVO objResultVO = BizActionObj.ResultDetails;

                DbCommand command = dbServer.GetStoredProcCommand("[CIMS_UpdateRadResultEntryMilan]");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objResultVO.ID);
                dbServer.AddInParameter(command, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, objResultVO.TestID);
                dbServer.AddInParameter(command, "FlimId", DbType.Int64, objResultVO.FilmID);
                dbServer.AddInParameter(command, "NoOfFlims", DbType.Int64, objResultVO.NoOfFilms);
                dbServer.AddInParameter(command, "RadiologistID1", DbType.Int64, objResultVO.RadiologistID1);
                dbServer.AddInParameter(command, "FirstLevelDescription", DbType.String, objResultVO.FirstLevelDescription);
                dbServer.AddInParameter(command, "TemplateResultID", DbType.Int64, objResultVO.TemplateResultID);
                dbServer.AddInParameter(command, "RadiologistID2", DbType.Int64, objResultVO.RadiologistID2);
                dbServer.AddInParameter(command, "RadiologistID3", DbType.Int64, objResultVO.RadiologistID3);
                dbServer.AddInParameter(command, "SecondLevelDescription", DbType.String, objResultVO.SecondLevelDescription);
                dbServer.AddInParameter(command, "ThirdLevelDescription", DbType.String, objResultVO.ThirdLevelDescription);
                dbServer.AddInParameter(command, "FirstLevelId", DbType.Int64, objResultVO.FirstLevelId);
                dbServer.AddInParameter(command, "SecondLevelId", DbType.Int64, objResultVO.SecondLevelId);
                dbServer.AddInParameter(command, "ThirdLevelId", DbType.Int64, objResultVO.ThirdLevelId);
                dbServer.AddInParameter(command, "TemplateId", DbType.Int64, objResultVO.TemplateID);
                dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, objResultVO.IsOutSourced);
                dbServer.AddInParameter(command, "AgencyId", DbType.Int64, objResultVO.AgencyId);

                dbServer.AddInParameter(command, "ReferredBy", DbType.String, objResultVO.ReferredBy.Trim());

                dbServer.AddInParameter(command, "IsFinalized", DbType.Boolean, objResultVO.IsFinalized);

                //Added By Yogita
                dbServer.AddInParameter(command, "SourceURL", DbType.String, objResultVO.SourceURL);
                dbServer.AddInParameter(command, "Report", DbType.Binary, objResultVO.Report);
                dbServer.AddInParameter(command, "Notes", DbType.String, objResultVO.Notes);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objResultVO.Remarks);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objResultVO.Time);
                dbServer.AddInParameter(command, "IsCompleted", DbType.Boolean, objResultVO.IsCompleted);
                dbServer.AddInParameter(command, "IsUpload", DbType.Boolean, BizActionObj.IsUpload);
                if (objResultVO.IsRefDoctorSigniture == true)
                {
                    dbServer.AddInParameter(command, "IsDigitalSignatureRequired", DbType.Boolean, objResultVO.IsRefDoctorSigniture);
                    dbServer.AddInParameter(command, "DoctorUserID", DbType.Int64, objResultVO.RadiologistID1);
                }
                else
                {
                    dbServer.AddInParameter(command, "IsDigitalSignatureRequired", DbType.Boolean, null);
                    dbServer.AddInParameter(command, "DoctorUserID", DbType.Int64, null);
                }
                //End
             //   objResultVO.FinalizedByDoctorTime
                //dbServer.AddInParameter(command, "FinalizedByDoctorTime", DbType.DateTime, objResultVO.FinalizedByDoctorTime);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Parse(System.DateTime.Now.ToString()));
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);



                //if (objResultVO.TestItemList != null && objResultVO.TestItemList.Count > 0)
                //{
                //    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteRadTestResultItemDetails");

                //    //dbServer.AddInParameter(command3, "ID", DbType.Int64, objResultVO.ID);
                //    dbServer.AddInParameter(command3, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                //    dbServer.AddInParameter(command3, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                //    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command3, "RadTechnicianEntryID", DbType.Int64, objResultVO.RadTechnicianEntryID);
                //    int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
                //}


                //if (objResultVO.TestItemList != null && objResultVO.TestItemList.Count > 0)
                //{
                //    foreach (var ItemList in objResultVO.TestItemList)
                //    {
                //        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadResultItemDetails");

                //        dbServer.AddInParameter(command1, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                //        dbServer.AddInParameter(command1, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                //        dbServer.AddInParameter(command1, "RadTechnicianEntryID", DbType.Int64, objResultVO.ID);
                //        dbServer.AddInParameter(command1, "TestID", DbType.Int64, objResultVO.TestID);
                //        dbServer.AddInParameter(command1, "StoreID", DbType.Int64, ItemList.StoreID);
                //        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, ItemList.ItemID);
                //        dbServer.AddInParameter(command1, "BatchID", DbType.Int64, ItemList.BatchID);
                //        dbServer.AddInParameter(command1, "IdealQuantity", DbType.Double, ItemList.Quantity);
                //        dbServer.AddInParameter(command1, "ActualQantity", DbType.Double, ItemList.ActualQantity);
                //        dbServer.AddInParameter(command1, "BalQantity", DbType.Double, ItemList.BalanceQuantity);
                //        dbServer.AddInParameter(command1, "WastageQuantity", DbType.Double, ItemList.WastageQantity);
                //        dbServer.AddInParameter(command1, "ItemCategoryID", DbType.Int64, ItemList.ItemCategoryID);

                //        dbServer.AddInParameter(command1, "ExpiryDate ", DbType.DateTime, ItemList.ExpiryDate);
                //        dbServer.AddInParameter(command1, "Remarks", DbType.String, ItemList.Remarks);


                //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                //        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                //        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ItemList.ID);
                //        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                //        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //        ItemList.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                //        ItemList.ID = (long)dbServer.GetParameterValue(command1, "ID");

                //        if (objResultVO.IsFinalized == true)
                //        {
                //            if (BizActionObj.AutoDeductStock == true)
                //            {

                //                clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                //                clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
                //                clsItemStockVO StockDetails = new clsItemStockVO();

                //                StockDetails.ItemID = ItemList.ItemID;
                //                StockDetails.BatchID = ItemList.BatchID;
                //                StockDetails.TransactionTypeID = InventoryTransactionType.RadiologyTestConsumption;
                //                StockDetails.TransactionQuantity = ItemList.Quantity;
                //                StockDetails.TransactionID = BizActionObj.ResultDetails.ID;
                //                StockDetails.Date = DateTime.Now;
                //                StockDetails.Time = DateTime.Now;
                //                StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                //                StockDetails.StoreID = ItemList.StoreID;
                //                obj.Details = StockDetails;
                //                obj.Details.ID = 0;

                //                obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                //                if (obj.SuccessStatus == -1)
                //                {
                //                    throw new Exception();
                //                }

                //                StockDetails.ID = obj.Details.ID;

                //            }
                //        }
                //    }
                //}
                trans.Commit();

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.ResultDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetOrderDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOrderBookingDetailsListBizActionVO BizActionObj = valueObject as clsGetOrderBookingDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadOrderBookingDetailsList");                
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ModalityID", DbType.Int64, BizActionObj.ModalityID);
                dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);
                dbServer.AddInParameter(command, "DeliveryStatus", DbType.Boolean, BizActionObj.DeliveryStatus);
                dbServer.AddInParameter(command, "CheckResultEntryStatus", DbType.Boolean, BizActionObj.CheckResultEntryStatus);
                dbServer.AddInParameter(command, "ResultEntryStatus", DbType.Boolean, BizActionObj.ResultEntryStatus);
                dbServer.AddInParameter(command, "IsTechnicianEntry", DbType.Boolean, BizActionObj.IsTechnicianEntry);
                dbServer.AddInParameter(command, "IsFinalized", DbType.Boolean, BizActionObj.IsFinalizedByDr);
                dbServer.AddInParameter(command, "NotDone", DbType.Boolean, BizActionObj.NotDone);               
                dbServer.AddInParameter(command, "RadiologistID", DbType.Int64, BizActionObj.RadiologistID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.BookingDetails == null)
                        BizActionObj.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
                    while (reader.Read())
                    {
                        clsRadOrderBookingDetailsVO objRadiologyVO = new clsRadOrderBookingDetailsVO();
                        objRadiologyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objRadiologyVO.RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"]));
                        objRadiologyVO.RadOrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderDetailID"]));
                        objRadiologyVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objRadiologyVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objRadiologyVO.TestCode = Convert.ToString(DALHelper.HandleDBNull(reader["TestCode"]));
                        objRadiologyVO.TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                        objRadiologyVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Service"]));
                        objRadiologyVO.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        objRadiologyVO.IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"]));
                        objRadiologyVO.IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"]));
                        objRadiologyVO.IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]));
                        objRadiologyVO.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                        objRadiologyVO.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                        objRadiologyVO.ReportUploadPath = Convert.ToString(DALHelper.HandleDBNull(reader["ReportPath"]));
                        objRadiologyVO.IsCompleted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCompleted"]));
                        objRadiologyVO.IsUpload = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUpload"]));
                        objRadiologyVO.ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModalityId"]));
                        objRadiologyVO.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"]));
                        objRadiologyVO.ResultEntryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultEntryID"]));
                        objRadiologyVO.IsTechnicianEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntry"]));
                        objRadiologyVO.IsTechnicianEntryFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntryFinalized"]));
                        objRadiologyVO.RadiologistEntryDate = (DateTime?)(DALHelper.HandleDBNull(reader["RadiologistEntryDate"]));//Added By Yogesh K
                        objRadiologyVO.PatientFullName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));//Added By Yogesh K
                        objRadiologyVO.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["PrescribingDr"]));//Added By Yogesh K
                        objRadiologyVO.RadiologistID1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologistID1"]));
                       objRadiologyVO.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));//Added By Yogesh K
                        objRadiologyVO.Radiologist = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));//Added By Yogesh K
                        objRadiologyVO.FinalizedByDoctorTime = (DateTime?)(DALHelper.HandleDBNull(reader["FinalizedByDoctorTime"]));//Added By Yogesh K
                        objRadiologyVO.ReportUploadDateTime = (DateTime?)(DALHelper.HandleDBNull(reader["ReportUploadDateTime"]));
                        objRadiologyVO.DeliveryReportDateTime = (DateTime?)(DALHelper.HandleDBNull(reader["DeliveryReportDateTime"]));
                        objRadiologyVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCancelled"]));
                        objRadiologyVO.CancelledDateTime = (DateTime?)(DALHelper.HandleDBNull(reader["CancelledDate"]));//Added By Yogesh K
                        objRadiologyVO.Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External"]));//Added By Bhushanp
                        BizActionObj.BookingDetails.Add(objRadiologyVO);
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
        
        public override IValueObject GetOrderDetailsForWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOrderBookingDetailsListForWorkOrderBizActionVO BizActionObj = valueObject as clsGetOrderBookingDetailsListForWorkOrderBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadOrderBookingDetailsListforWorkOrderMilan");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ModalityID", DbType.Int64, BizActionObj.ModalityID);
                dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);
                dbServer.AddInParameter(command, "DeliveryStatus", DbType.Boolean, BizActionObj.DeliveryStatus);
                dbServer.AddInParameter(command, "CheckResultEntryStatus", DbType.Boolean, BizActionObj.CheckResultEntryStatus);
                dbServer.AddInParameter(command, "ResultEntryStatus", DbType.Boolean, BizActionObj.ResultEntryStatus);
                dbServer.AddInParameter(command, "IsTechnicianEntry", DbType.Boolean, BizActionObj.IsTechnicianEntry);
                dbServer.AddInParameter(command, "IsFinalized", DbType.Boolean, BizActionObj.IsFinalizedByDr);
                dbServer.AddInParameter(command, "NotDone", DbType.Boolean, BizActionObj.NotDone);

                dbServer.AddInParameter(command, "Modality", DbType.Int64, BizActionObj.ModalityID);
                dbServer.AddInParameter(command, "RadiologistID", DbType.Int64, BizActionObj.RadiologistID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.BookingDetails == null)
                        BizActionObj.BookingDetails = new List<clsRadOrderBookingDetailsVO>();
                    while (reader.Read())
                    {
                        clsRadOrderBookingDetailsVO objRadiologyVO = new clsRadOrderBookingDetailsVO();

                        objRadiologyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objRadiologyVO.RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"]));
                        objRadiologyVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objRadiologyVO.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        objRadiologyVO.ChargeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChargeID"]));
                        objRadiologyVO.TestCode = Convert.ToString(DALHelper.HandleDBNull(reader["TestCode"]));
                        objRadiologyVO.TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"]));
                        objRadiologyVO.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Service"]));
                        objRadiologyVO.LongDescription = Convert.ToString(DALHelper.HandleDBNull(reader["LongDescription"]));
                        objRadiologyVO.IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"]));
                        objRadiologyVO.IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"]));
                        objRadiologyVO.IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]));
                        if (BizActionObj.ReportDelivery == true)
                            objRadiologyVO.ResultEntryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultEntryID"]));

                        objRadiologyVO.IsTechnicianEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntry"]));
                        objRadiologyVO.IsTechnicianEntryFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTechnicianEntryFinalized"]));
                        objRadiologyVO.Contrast = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Contrast"]));
                        objRadiologyVO.ContrastDetails = Convert.ToString(DALHelper.HandleDBNull(reader["ContrastDetails"]));
                        objRadiologyVO.FilmWastage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FilmWastage"]));
                        objRadiologyVO.FilmWastageDetails = Convert.ToString(DALHelper.HandleDBNull(reader["WastageDetails"]));
                        objRadiologyVO.NoOfFilms = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NoOfFlims"]));
                        objRadiologyVO.Sedation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Sedation"]));
                        objRadiologyVO.IsDelivered = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDelivered"]));
                        objRadiologyVO.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"]));
                        objRadiologyVO.ModalityID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ModalityID"]));
                        objRadiologyVO.IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                        objRadiologyVO.PatientFullName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        objRadiologyVO.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"]));//Added By Yogesh K
                        objRadiologyVO.Radiologist = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));//Added By Yogesh K
                        objRadiologyVO.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"]));//Added By Yogesh K
                        objRadiologyVO.PrescribingDr = Convert.ToString(DALHelper.HandleDBNull(reader["PrescribingDr"]));//Added By Yogesh K
                        //objRadiologyVO.ModalityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Modality"]));//Added By Yogesh K
                        //objRadiologyVO.RadiologistID1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["Modality"]));//Added By Yogesh K
                       
                        objRadiologyVO.RadiologistEntryDate = (DateTime?)DALHelper.HandleDBNull(reader["RadiologistEntryDate"]);//Added By Yogesh K
                        objRadiologyVO.FinalizedByDoctorTime = (DateTime?)DALHelper.HandleDBNull(reader["FinalizedByDoctorTime"]);//Added By Yogesh K

                        objRadiologyVO.IsUpload = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUpload"]));
                        objRadiologyVO.ReportUploadDateTime = (DateTime?)(DALHelper.HandleDBNull(reader["ReportUploadDateTime"]));

                        objRadiologyVO.DeliveryReportDateTime = (DateTime?)(DALHelper.HandleDBNull(reader["DeliveryReportDateTime"]));

                        
                        objRadiologyVO.CancelledDateTime = (DateTime?)(DALHelper.HandleDBNull(reader["CancelledDate"]));//Added By Yogesh K
               
                        BizActionObj.BookingDetails.Add(objRadiologyVO);
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

       // Need to create class clsRadPatientReportVO
        public override IValueObject UploadReport(IValueObject valueObject, clsUserVO UserVo)
        {

            clsRadUploadReportBizActionVO BizActionObj = valueObject as clsRadUploadReportBizActionVO;

            try
            {
                clsRadPatientReportVO objVO = BizActionObj.UploadReportDetails;
               // clsPathPatientReportVO objVO = BizActionObj.UploadReportDetails;
                //if (BizActionObj.IsResultEntry == true)
                //{
                //    //If Result entry is completed for patho order
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUploadReportRad");

                //    dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objVO.RadOrderDetailID);
                //    dbServer.AddInParameter(command, "RadPatientReportID", DbType.Int64, objVO.RadPatientReportID);
                //    dbServer.AddInParameter(command, "SourceURL", DbType.String, objVO.SourceURL);
                //    dbServer.AddInParameter(command, "Report", DbType.Binary, objVO.Report);
                //    dbServer.AddInParameter(command, "Notes", DbType.String, objVO.Notes);
                //    dbServer.AddInParameter(command, "Remarks", DbType.String, objVO.Remarks);
                //    dbServer.AddInParameter(command, "Time", DbType.DateTime, objVO.Time);

                //    if (BizActionObj.UnitID > 0)
                //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                //    else
                //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                //    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                //    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //    int intStatus = dbServer.ExecuteNonQuery(command);
                //}
                //else
                //{
                    //if result entry not done for patho order

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRadPatientReportWithOutResultEntry");

                   
                    dbServer.AddInParameter(command, "SourceURL", DbType.String, objVO.SourceURL);
                    dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objVO.RadOrderDetailID);
                    dbServer.AddInParameter(command, "RadOrderID", DbType.Int64, objVO.RadOrderID);
                  //  dbServer.AddInParameter(command, "SampleNo", DbType.String, objVO.SampleNo);
                    dbServer.AddInParameter(command, "Report", DbType.Binary, objVO.Report);
                    dbServer.AddInParameter(command, "ReportPath", DbType.String, objVO.ReportPath);
                    dbServer.AddInParameter(command, "TestID", DbType.Int64, objVO.TestID);//Added By Yogesh K 06 06 16
                    dbServer.AddInParameter(command, "RadiologistID1", DbType.Int64, objVO.RefDoctorID);//Added By Yogesh K 06 06 16
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, objVO.ReferredBy);//Added By Yogesh K 06 06 16
                    dbServer.AddInParameter(command, "Notes", DbType.String, objVO.Notes);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, objVO.Remarks);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, objVO.Time);

                    if (BizActionObj.UnitID > 0)
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    else
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                   // dbServer.AddInParameter(command, "ReportPath", DbType.String, objVO.ReportPath);//Added By Yogesh

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);



                   // File.WriteAllBytes(objVO.ReportPath, objVO.Report);



                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);

                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.UploadReportDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
              //  }



            }
            catch (Exception ex)
            {

                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetUploadReport(IValueObject valueObject, clsUserVO UserVo)
        {

            clsRadUploadReportBizActionVO BizActionObj = valueObject as clsRadUploadReportBizActionVO;

            try
            {
                clsRadPatientReportVO objVO = BizActionObj.UploadReportDetails;
                // clsPathPatientReportVO objVO = BizActionObj.UploadReportDetails;
                if (BizActionObj.IsResultEntry == true)
                {
                    //If Result entry is completed for patho order
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUploadReportRad");

                    dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objVO.RadOrderDetailID);
                    dbServer.AddInParameter(command, "RadPatientReportID", DbType.Int64, objVO.RadPatientReportID);
                    dbServer.AddInParameter(command, "SourceURL", DbType.String, objVO.SourceURL);
                  //  dbServer.AddInParameter(command, "Report", DbType.Binary, objVO.Report);
                    dbServer.AddInParameter(command, "Notes", DbType.String, objVO.Notes);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, objVO.Remarks);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, objVO.Time);

                    if (BizActionObj.UnitID > 0)
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    else
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                }
                else
                {
                    //if result entry not done for patho order

                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRadPatientReportWithOutResultEntry");


                    dbServer.AddInParameter(command, "SourceURL", DbType.String, objVO.SourceURL);
                    dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objVO.RadOrderDetailID);
                    dbServer.AddInParameter(command, "RadOrderID", DbType.Int64, objVO.RadOrderID);
                    //  dbServer.AddInParameter(command, "SampleNo", DbType.String, objVO.SampleNo);
                  // dbServer.AddInParameter(command, "Report", DbType.Binary, objVO.Report);
                    dbServer.AddInParameter(command, "ReportPath", DbType.String, objVO.ReportPath);
                    dbServer.AddInParameter(command, "TestID", DbType.Int64, objVO.TestID);//Added By Yogesh K 06 06 16
                    dbServer.AddInParameter(command, "RadiologistID1", DbType.Int64, objVO.RefDoctorID);//Added By Yogesh K 06 06 16
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, objVO.ReferredBy);//Added By Yogesh K 06 06 16
                    dbServer.AddInParameter(command, "Notes", DbType.String, objVO.Notes);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, objVO.Remarks);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, objVO.Time);

                    if (BizActionObj.UnitID > 0)
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                    else
                        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    // dbServer.AddInParameter(command, "ReportPath", DbType.String, objVO.ReportPath);//Added By Yogesh

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objVO.ID);



                    // File.WriteAllBytes(objVO.ReportPath, objVO.Report);



                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);

                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.UploadReportDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                }



            }
            catch (Exception ex)
            {

                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetRadioGender(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetRadioTemplateGenderBizActionVO BizActionObj = valueObject as clsGetRadioTemplateGenderBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadioGenderTemplateList");
                dbServer.AddInParameter(command, "TemplateID", DbType.String, BizActionObj.TemplateID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.GenderDetails == null)
                        BizActionObj.GenderDetails = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objRadiologyVO = new MasterListItem();
                        objRadiologyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));

                        BizActionObj.GenderDetails.Add(objRadiologyVO);
                    }
                }

                reader.Close();
            }
            catch
            {

            }
            finally
            {
            }
            return BizActionObj;
        }


        public override IValueObject GetOrderList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOrderBookingListBizActionVO BizActionObj = valueObject as clsGetOrderBookingListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadiologyOrderBookingListMilan");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                if (BizActionObj.OPDNO != null && BizActionObj.OPDNO.Length != 0)
                {
                    dbServer.AddInParameter(command, "ScanNo", DbType.String, BizActionObj.OPDNO + "%");
                }
                else
                    dbServer.AddInParameter(command, "ScanNo", DbType.String, null);

                if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                {
                    dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName + "%");
                }
                else
                    dbServer.AddInParameter(command, "FirstName", DbType.String, null);
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                {
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName + "%");
                }
                else
                    dbServer.AddInParameter(command, "LastName", DbType.String, null);



                if (BizActionObj.MRNO != null && BizActionObj.MRNO.Length != 0) //Added By Yogesh 
                {
                    dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO + "%");
                }
                else
                    dbServer.AddInParameter(command, "MRNO", DbType.String, null);


                if (BizActionObj.CategoryID > 0)
                {

                    dbServer.AddInParameter(command, "CategoryID", DbType.Int64, BizActionObj.CategoryID);//Added By Yogesh 
                }
                else
                    dbServer.AddInParameter(command, "CategoryID", DbType.Int64, null);//Added By Yogesh 



                if (BizActionObj.IsFinalizedByDr == true)
                {

                    dbServer.AddInParameter(command, "IsFinalized", DbType.Boolean, BizActionObj.IsFinalizedByDr);//Added By Yogesh 
                }
                else
                    dbServer.AddInParameter(command, "IsFinalized", DbType.Boolean, null);//Added By Yogesh 


                if (BizActionObj.IsTechnicianEntry == true)
                {

                    dbServer.AddInParameter(command, "IsTechnicianEntry", DbType.Boolean, BizActionObj.IsTechnicianEntry);//Added By Yogesh 
                }
                else
                    dbServer.AddInParameter(command, "IsTechnicianEntry", DbType.Boolean, null);//Added By Yogesh 

                if (BizActionObj.NotDone == false)
                {
                    dbServer.AddInParameter(command, "NotDone", DbType.Boolean, null);//Added By Yogesh 
                }
                else
                {
                    dbServer.AddInParameter(command, "NotDone", DbType.Boolean, false);//Added By Yogesh 
                  
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);//Change By Bhushanp 24052017
                dbServer.AddInParameter(command, "CheckDeliveryStatus", DbType.Boolean, BizActionObj.CheckDeliveryStatus);
                dbServer.AddInParameter(command, "DeliveryStatus", DbType.Boolean, BizActionObj.DeliveryStatus);
                dbServer.AddInParameter(command, "CheckResultEntryStatus", DbType.Boolean, BizActionObj.CheckResultEntryStatus);
                dbServer.AddInParameter(command, "ResultEntryStatus", DbType.Boolean, BizActionObj.ResultEntryStatus);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.BookingList == null)
                        BizActionObj.BookingList = new List<clsRadOrderBookingVO>();
                    while (reader.Read())
                    {
                        clsRadOrderBookingVO objRadiologyVO = new clsRadOrderBookingVO();
                        objRadiologyVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objRadiologyVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objRadiologyVO.BillDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BillDate"]));
                        objRadiologyVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objRadiologyVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        objRadiologyVO.OrderNo = Convert.ToString(DALHelper.HandleDBNull(reader["OrderNo"]));
                        objRadiologyVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objRadiologyVO.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        objRadiologyVO.PatientUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitId"]));
                        objRadiologyVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objRadiologyVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        objRadiologyVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        objRadiologyVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalBillAmount"]));
                        objRadiologyVO.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                        objRadiologyVO.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                        objRadiologyVO.PaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientPaidAmount"]));
                        objRadiologyVO.VisitNotes = Convert.ToString(DALHelper.HandleDBNull(reader["VisitNotes"]));
                        objRadiologyVO.Balance = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceAmount"]));
                        objRadiologyVO.Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freezed"]));
                        objRadiologyVO.Gender = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                     
                        //objRadiologyVO.Radiologist = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));

                      //  objRadiologyVO.Modality = Convert.ToString(DALHelper.HandleDBNull(reader["Modality"]));

                        if (objRadiologyVO.InProcess != null)
                        {
                            //objRadiologyVO.InProcess = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InProcess"]));
                        }

                        if (objRadiologyVO.TotalCount != null)
                        {
                            objRadiologyVO.TotalCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalCount"]));
                        }

                        if (objRadiologyVO.ResultEntryCount != null)
                        {
                            objRadiologyVO.ResultEntryCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultEntryCount"]));
                        }

                        if (objRadiologyVO.CompletedTest != null)
                        {
                            objRadiologyVO.CompletedTest = Convert.ToInt64(DALHelper.HandleDBNull(reader["Completed"]));
                        }

                        if (objRadiologyVO.UploadedCount != null)
                        {
                            objRadiologyVO.UploadedCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["UploadedCount"]));
                        }

                        //if (objRadiologyVO.IsResultEntry != null)
                        //{
                        //    objRadiologyVO.IsResultEntry = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResultEntry"]));
                        //}
                        //if (objRadiologyVO.IsFinalized != null)
                        //{
                        //    objRadiologyVO.IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"]));
                        //}

                        switch (objRadiologyVO.Gender)
                        {
                            case 1:
                                objRadiologyVO.GenderName = "Male";
                                break;
                            case 2:
                                objRadiologyVO.GenderName = "Female";
                                break;
                        }
                        objRadiologyVO.AgeINDDMMYYYY = Convert.ToString(DALHelper.HandleDBNull(reader["Age"]));
                        objRadiologyVO.ContactNO = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNO"]));
                        objRadiologyVO.ReferredDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        objRadiologyVO.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        objRadiologyVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        objRadiologyVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        objRadiologyVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        objRadiologyVO.EmailID = Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"]));
                        objRadiologyVO.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"]));
                        objRadiologyVO.AgeInYears = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInYears"]));
                        objRadiologyVO.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmail"]));

                      //  objRadiologyVO.DeliveryStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DeliveryStatus"]));
                       // objRadiologyVO.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        BizActionObj.BookingList.Add(objRadiologyVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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

        public override IValueObject GetResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRadResultEntryBizActionVO BizActionObj = valueObject as clsGetRadResultEntryBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRadResultEntryMilan");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizActionObj.DetailID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ResultDetails == null)
                        BizActionObj.ResultDetails = new clsRadResultEntryVO();
                    while (reader.Read())
                    {
                        BizActionObj.ResultDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ResultDetails.RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"]));

                        BizActionObj.ResultDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        BizActionObj.ResultDetails.FilmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FlimId"]));
                        BizActionObj.ResultDetails.NoOfFilms = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfFlims"]));
                        BizActionObj.ResultDetails.RadiologistID1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadiologistID1"]));
                        BizActionObj.ResultDetails.FirstLevelDescription = Convert.ToString(DALHelper.HandleDBNull(reader["FirstLevelDescription"]));
                        BizActionObj.ResultDetails.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        BizActionObj.ResultDetails.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));
                       
                        BizActionObj.ResultDetails.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                        BizActionObj.ResultDetails.Report = (byte[])DALHelper.HandleDBNull(reader["Report"]);
                        BizActionObj.ResultDetails.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"]));
                        BizActionObj.ResultDetails.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        BizActionObj.ResultDetails.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizActionObj.ResultDetails.IsCompleted = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCompleted"]));
                        BizActionObj.ResultDetails.IsDigitalSignatureRequired = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDigitalSignatureRequired"]));

                        BizActionObj.ResultDetails.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"]));//Added By yogesh K 17May16

                        BizActionObj.ResultDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));//Added By yogesh K 31May16
                    }
                    reader.NextResult();

                    if (BizActionObj.TestItemList == null)
                        BizActionObj.TestItemList = new List<clsRadItemDetailMasterVO>();

                    while (reader.Read())
                    {
                        clsRadItemDetailMasterVO ObjItem = new clsRadItemDetailMasterVO();
                        ObjItem.RadOrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadOrderID"]));
                        ObjItem.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        ObjItem.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        ObjItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        ObjItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        ObjItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        ObjItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        ObjItem.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["IdealQuantity"]));
                        ObjItem.ActualQantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ActualQantity"]));
                        ObjItem.WastageQantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WastageQuantity"]));
                        ObjItem.BalanceQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalQuantity"]));
                        ObjItem.ExpiryDate = (DateTime?)DALHelper.HandleDBNull(reader["ExpiryDate"]);
                        ObjItem.ItemCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        ObjItem.ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"]));

                        ObjItem.IsFinalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFinalized"]));
                        ObjItem.BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));


                        BizActionObj.TestItemList.Add(ObjItem);
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

        public override IValueObject GetResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsRadResultEntryPrintDetailsBizActionVO BizActionObj = valueObject as clsRadResultEntryPrintDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_rpt_RadResultEntry");
                dbServer.AddInParameter(command, "ResultID", DbType.Int64, BizActionObj.ResultID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "OPDIPD", DbType.Int64, BizActionObj.OPDIPD);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ResultDetails == null)
                        BizActionObj.ResultDetails = new clsRadResultEntryPrintDetailsVO();
                    while (reader.Read())
                    {
                        BizActionObj.ResultDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ResultDetails.FirstLevelDescription = Convert.ToString(DALHelper.HandleDBNull(reader["FirstLevelDescription"]));
                        BizActionObj.ResultDetails.TestTemplate = Convert.ToString(DALHelper.HandleDBNull(reader["TestTemplate"]));
                        BizActionObj.ResultDetails.TestNAme = Convert.ToString(DALHelper.HandleDBNull(reader["TestNAme"]));
                        BizActionObj.ResultDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        BizActionObj.ResultDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        BizActionObj.ResultDetails.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        BizActionObj.ResultDetails.OrderDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["OrderDate"]));
                        BizActionObj.ResultDetails.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.ResultDetails.AgeYear = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeYear"]));
                        BizActionObj.ResultDetails.AgeMonth = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeMonth"]));
                        BizActionObj.ResultDetails.AgeDate = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeDate"]));
                        BizActionObj.ResultDetails.ReferredDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        BizActionObj.ResultDetails.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        BizActionObj.ResultDetails.TestDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["TestDate"]));
                        BizActionObj.ResultDetails.PrintTestName = Convert.ToString(DALHelper.HandleDBNull(reader["PrintTestName"]));
                        BizActionObj.ResultDetails.AddedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"]));
                        BizActionObj.ResultDetails.ResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ResultDetails.ResultAddedDateTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ResultAddedDateTime"]));
                        BizActionObj.ResultDetails.RadiologistName = Convert.ToString(DALHelper.HandleDBNull(reader["RadiologistName"]));
                        BizActionObj.ResultDetails.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        BizActionObj.ResultDetails.Radiologist = Convert.ToInt64(DALHelper.HandleDBNull(reader["Radiologist"]));
                        BizActionObj.ResultDetails.Radiologist1 = Convert.ToString(DALHelper.HandleDBNull(reader["RadiologistName"]));
                        BizActionObj.ResultDetails.Education1 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        BizActionObj.ResultDetails.RadioDoctorid1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["Radiologist"]));
                        BizActionObj.ResultDetails.Signature1 = (Byte[])(DALHelper.HandleDBNull(reader["DigitalSignature"]));
                        BizActionObj.ResultDetails.ReportPrepairDate = Convert.ToString(DALHelper.HandleDBNull(reader["ReportPrepairDate"]));


                        BizActionObj.ResultDetails.Signature2 = (Byte[])(DALHelper.HandleDBNull(reader["Logo"]));//Added By YK

                        BizActionObj.ResultDetails.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["Name"]));
                        BizActionObj.ResultDetails.UnitAddress = Convert.ToString(DALHelper.HandleDBNull(reader["address"]));
                        BizActionObj.ResultDetails.UnitContact = Convert.ToString(DALHelper.HandleDBNull(reader["contact"]));
                        BizActionObj.ResultDetails.UnitContact1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        BizActionObj.ResultDetails.UnitMobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["MobileNO"]));

                        BizActionObj.ResultDetails.UnitEmail = Convert.ToString(DALHelper.HandleDBNull(reader["Email"]));
                        BizActionObj.ResultDetails.UnitWebsite = Convert.ToString(DALHelper.HandleDBNull(reader["WebSite"]));




                        
                        
                    }

                    //reader.NextResult();

                    //int RowCnt = 0;

                    //while (reader.Read())
                    //{
                    //    RowCnt = RowCnt + 1;

                    //    if (RowCnt == 1)
                    //    {
                    //        BizActionObj.ResultDetails.Radiologist1 = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));
                    //        BizActionObj.ResultDetails.Signature1 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                    //        BizActionObj.ResultDetails.Education1 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                    //        BizActionObj.ResultDetails.RadioDoctorid1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadioDoctorid"]));
                    //    }

                    //    if (RowCnt == 2)
                    //    {
                    //        BizActionObj.ResultDetails.Radiologist2 = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));
                    //        BizActionObj.ResultDetails.Signature2 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                    //        BizActionObj.ResultDetails.Education2 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                    //        BizActionObj.ResultDetails.RadioDoctorid2 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadioDoctorid"]));
                    //    }

                    //    if (RowCnt == 3)
                    //    {
                    //        BizActionObj.ResultDetails.Radiologist3 = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));
                    //        BizActionObj.ResultDetails.Signature3 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                    //        BizActionObj.ResultDetails.Education3 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                    //        BizActionObj.ResultDetails.RadioDoctorid3 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadioDoctorid"]));
                    //    }

                    //    if (RowCnt == 4)
                    //    {
                    //        BizActionObj.ResultDetails.Radiologist4 = Convert.ToString(DALHelper.HandleDBNull(reader["Radiologist"]));
                    //        BizActionObj.ResultDetails.Signature4 = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                    //        BizActionObj.ResultDetails.Education4 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                    //        BizActionObj.ResultDetails.RadioDoctorid4 = Convert.ToInt64(DALHelper.HandleDBNull(reader["RadioDoctorid"]));
                    //    }

                    //}

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


        public override IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override IValueObject UpdateReportDelivery(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateReportDeliveryBizActionVO BizActionObj = valueObject as clsUpdateReportDeliveryBizActionVO;

            try
            {
                clsRadOrderBookingDetailsVO objOrderVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRadReportDeliveryMilan");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objOrderVO.ID);
                dbServer.AddInParameter(command, "IsDelivered", DbType.Boolean, objOrderVO.IsDelivered);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objOrderVO.UnitID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, objOrderVO.TestID);
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
        #endregion

        public override IValueObject UpdateTechnicianEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsUpdateRadTechnicianEntryBizActionVO BizActionObj = valueObject as clsUpdateRadTechnicianEntryBizActionVO;
                if (BizActionObj.ResultDetails.ID == 0)
                    BizActionObj = AddTechnicianEntry(BizActionObj, UserVo);
                else
                    BizActionObj = UpdateTechnician(BizActionObj, UserVo);
            }
            catch
            {

            }
            return valueObject;
        }

        private clsUpdateRadTechnicianEntryBizActionVO AddTechnicianEntry(clsUpdateRadTechnicianEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                clsRadResultEntryVO objResultVO = BizActionObj.ResultDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRadTechnicianEntry");
                dbServer.AddInParameter(command, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, objResultVO.TestID);
                dbServer.AddInParameter(command, "FlimId", DbType.Int64, objResultVO.FilmID);
                dbServer.AddInParameter(command, "NoOfFlims", DbType.Int64, objResultVO.NoOfFilms);
                dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, objResultVO.IsOutSourced);
                dbServer.AddInParameter(command, "AgencyId", DbType.Int64, objResultVO.AgencyId);
                dbServer.AddInParameter(command, "Contrast", DbType.Boolean, objResultVO.Contrast);
                dbServer.AddInParameter(command, "Sedation", DbType.Boolean, objResultVO.Sedation);
                dbServer.AddInParameter(command, "ContrastDetails", DbType.String, objResultVO.ContrastDetails);
                dbServer.AddInParameter(command, "FilmWastage", DbType.Boolean, objResultVO.FilmWastage);
                dbServer.AddInParameter(command, "FilmWastageDetails", DbType.String, objResultVO.FilmWastageDetails);
                if (objResultVO.ReferredBy != null)
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, objResultVO.ReferredBy.Trim());
                else
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, objResultVO.ReferredBy);


                dbServer.AddInParameter(command, "IsTechnicianEntry", DbType.Boolean, objResultVO.IsTechnicianEntry);
                dbServer.AddInParameter(command, "IsTechnicianEntryFinalized", DbType.Boolean, objResultVO.IsTechnicianEntryFinalized);


                dbServer.AddInParameter(command, "TechnicianUserID", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "RadiologistUserID", DbType.Int64, null);
                dbServer.AddInParameter(command, "TechnicianEntryDate", DbType.DateTime, Convert.ToDateTime(DateTime.Now));


                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objResultVO.UnitID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objResultVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ResultDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (objResultVO.TestItemList != null && objResultVO.TestItemList.Count > 0)
                {
                    foreach (var ItemList in objResultVO.TestItemList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadResultItemDetails");
                        dbServer.AddInParameter(command1, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                        dbServer.AddInParameter(command1, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                        dbServer.AddInParameter(command1, "RadTechnicianEntryID", DbType.Int64, objResultVO.ID);
                        dbServer.AddInParameter(command1, "TestID", DbType.Int64, objResultVO.TestID);
                        dbServer.AddInParameter(command1, "StoreID", DbType.Int64, ItemList.StoreID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, ItemList.ItemID);
                        dbServer.AddInParameter(command1, "BatchID", DbType.Int64, ItemList.BatchID);
                        dbServer.AddInParameter(command1, "IdealQuantity", DbType.Double, ItemList.Quantity);
                        dbServer.AddInParameter(command1, "ActualQantity", DbType.Double, ItemList.ActualQantity);
                        dbServer.AddInParameter(command1, "BalQantity", DbType.Double, ItemList.BalanceQuantity);
                        dbServer.AddInParameter(command1, "WastageQuantity", DbType.Double, ItemList.WastageQantity);
                        dbServer.AddInParameter(command1, "ItemCategoryID", DbType.Int64, ItemList.ItemCategoryID);
                        dbServer.AddInParameter(command1, "ExpiryDate ", DbType.DateTime, ItemList.ExpiryDate);
                        dbServer.AddInParameter(command1, "Remarks", DbType.String, ItemList.Remarks);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ItemList.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                        ItemList.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        ItemList.ID = (long)dbServer.GetParameterValue(command1, "ID");

                        if (objResultVO.IsTechnicianEntryFinalized == true)
                        {
                            if (BizActionObj.AutoDeductStock == true)
                            {
                                if (BizActionObj.ItemCusmption == true)
                                {
                                    #region Add To T_MaterailConsumption And T_MaterialConsumptionDetails
                                    try
                                    {
                                        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddMaterialConsumption");
                                        dbServer.AddInParameter(command5, "BillID", DbType.Int64, objResultVO.BillID);
                                        dbServer.AddInParameter(command5, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        dbServer.AddInParameter(command5, "StoreID", DbType.Int64, ItemList.StoreID);
                                        dbServer.AddInParameter(command5, "Date", DbType.DateTime, DateTime.Now);
                                        dbServer.AddInParameter(command5, "Time", DbType.DateTime, DateTime.Now);
                                        dbServer.AddInParameter(command5, "TotalAmount", DbType.Double, null);
                                        dbServer.AddInParameter(command5, "Remark", DbType.String, "Radiographor Entry Item Consumption");
                                        dbServer.AddInParameter(command5, "TotalItems", DbType.Decimal, objResultVO.TestItemList.Count);
                                        dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                                        dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                        dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now.Date);
                                        dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                        dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);
                                        dbServer.AddOutParameter(command5, "ID5", DbType.Int64, int.MaxValue);
                                        int intStatus5 = dbServer.ExecuteNonQuery(command5);
                                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                                        BizActionObj.ConsumptionID = (long)dbServer.GetParameterValue(command5, "ID5");
                                        DbCommand command6 = dbServer.GetStoredProcCommand("CIMS_AddMaerialConsumptionDetails");
                                        command6.Connection = con;
                                        dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                                        dbServer.AddInParameter(command6, "ConsumptionID", DbType.Int64, BizActionObj.ConsumptionID);
                                        dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        dbServer.AddInParameter(command6, "BatchID", DbType.Int64, ItemList.BatchID);
                                        dbServer.AddInParameter(command6, "ItemId", DbType.Int64, ItemList.ItemID);
                                        dbServer.AddInParameter(command6, "UsedQty", DbType.Decimal, ItemList.WastageQantity + ItemList.ActualQantity);
                                        dbServer.AddInParameter(command6, "Rate", DbType.Decimal, null);
                                        dbServer.AddInParameter(command6, "Amount", DbType.Decimal, null);
                                        dbServer.AddInParameter(command6, "Remark", DbType.String, "Radiographor Entry Item Consumption");
                                        dbServer.AddInParameter(command6, "BatchCode", DbType.String, ItemList.BatchCode);
                                        dbServer.AddInParameter(command6, "ExpiryDate", DbType.Date, ItemList.ExpiryDate);
                                        dbServer.AddInParameter(command6, "ItemName", DbType.String, ItemList.ItemName);
                                        dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, int.MaxValue);
                                        int intStatus6 = dbServer.ExecuteNonQuery(command6);
                                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command6, "ResultStatus");
                                    }
                                    catch
                                    {

                                    }
                                    #endregion

                                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
                                    clsItemStockVO StockDetails = new clsItemStockVO();

                                    StockDetails.ItemID = ItemList.ItemID;
                                    StockDetails.BatchID = ItemList.BatchID;
                                    StockDetails.BatchCode = ItemList.BatchCode;
                                    StockDetails.TransactionTypeID = InventoryTransactionType.RadiologyTestConsumption;
                                    StockDetails.TransactionQuantity = (ItemList.ActualQantity + ItemList.WastageQantity);
                                    StockDetails.TransactionID = BizActionObj.ResultDetails.ID;
                                    StockDetails.Date = DateTime.Now;
                                    StockDetails.Time = DateTime.Now;
                                    StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                                    StockDetails.StoreID = ItemList.StoreID;
                                    obj.Details = StockDetails;
                                    obj.Details.ID = 0;

                                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                                    if (obj.SuccessStatus == -1)
                                    {
                                        throw new Exception();
                                    }
                                    StockDetails.ID = obj.Details.ID;
                                }
                            }
                        }
                    }
                }
                trans.Commit();
            }
            catch
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.ResultDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        private clsUpdateRadTechnicianEntryBizActionVO UpdateTechnician(clsUpdateRadTechnicianEntryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsRadResultEntryVO objResultVO = BizActionObj.ResultDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRadTechnicianEntry");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objResultVO.ID);
                dbServer.AddInParameter(command, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                dbServer.AddInParameter(command, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                dbServer.AddInParameter(command, "TestID", DbType.Int64, objResultVO.TestID);
                dbServer.AddInParameter(command, "FlimId", DbType.Int64, objResultVO.FilmID);
                dbServer.AddInParameter(command, "NoOfFlims", DbType.Int64, objResultVO.NoOfFilms);
                dbServer.AddInParameter(command, "IsOutSourced", DbType.Boolean, objResultVO.IsOutSourced);
                dbServer.AddInParameter(command, "AgencyId", DbType.Int64, objResultVO.AgencyId);

                if (objResultVO.ReferredBy != null)
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, objResultVO.ReferredBy.Trim());
                else
                    dbServer.AddInParameter(command, "ReferredBy", DbType.String, objResultVO.ReferredBy);

                dbServer.AddInParameter(command, "Contrast", DbType.Boolean, objResultVO.Contrast);
                dbServer.AddInParameter(command, "Sedation", DbType.Boolean, objResultVO.Sedation);
                dbServer.AddInParameter(command, "ContrastDetails", DbType.String, objResultVO.ContrastDetails);
                dbServer.AddInParameter(command, "FilmWastage", DbType.Boolean, objResultVO.FilmWastage);
                dbServer.AddInParameter(command, "FilmWastageDetails", DbType.String, objResultVO.FilmWastageDetails);


                dbServer.AddInParameter(command, "IsTechnicianEntry", DbType.Boolean, objResultVO.IsTechnicianEntry);
                dbServer.AddInParameter(command, "IsTechnicianEntryFinalized", DbType.Boolean, objResultVO.IsTechnicianEntryFinalized);


                dbServer.AddInParameter(command, "TechnicianUserID", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "TechnicianEntryDate", DbType.DateTime, Convert.ToDateTime(DateTime.Now));

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objResultVO.UnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);


                if (objResultVO.TestItemList != null && objResultVO.TestItemList.Count > 0)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteRadTestResultItemDetails");

                    dbServer.AddInParameter(command3, "RadTechnicianEntryID", DbType.Int64, objResultVO.ID);
                    dbServer.AddInParameter(command3, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                    dbServer.AddInParameter(command3, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
                }


                if (objResultVO.TestItemList != null && objResultVO.TestItemList.Count > 0)
                {
                    foreach (var ItemList in objResultVO.TestItemList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRadResultItemDetails");

                        dbServer.AddInParameter(command1, "RadOrderID", DbType.Int64, objResultVO.RadOrderID);
                        dbServer.AddInParameter(command1, "RadOrderDetailID", DbType.Int64, objResultVO.BookingDetailsID);
                        dbServer.AddInParameter(command1, "RadTechnicianEntryID", DbType.Int64, objResultVO.ID);
                        dbServer.AddInParameter(command1, "TestID", DbType.Int64, objResultVO.TestID);
                        dbServer.AddInParameter(command1, "StoreID", DbType.Int64, ItemList.StoreID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, ItemList.ItemID);
                        dbServer.AddInParameter(command1, "BatchID", DbType.Int64, ItemList.BatchID);
                        dbServer.AddInParameter(command1, "IdealQuantity", DbType.Double, ItemList.Quantity);
                        dbServer.AddInParameter(command1, "ActualQantity", DbType.Double, ItemList.ActualQantity);
                        dbServer.AddInParameter(command1, "BalQantity", DbType.Double, ItemList.BalanceQuantity);
                        dbServer.AddInParameter(command1, "WastageQuantity", DbType.Double, ItemList.WastageQantity);
                        dbServer.AddInParameter(command1, "ItemCategoryID", DbType.Int64, ItemList.ItemCategoryID);

                        dbServer.AddInParameter(command1, "ExpiryDate ", DbType.DateTime, ItemList.ExpiryDate);
                        dbServer.AddInParameter(command1, "Remarks", DbType.String, ItemList.Remarks);


                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ItemList.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                        ItemList.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        ItemList.ID = (long)dbServer.GetParameterValue(command1, "ID");

                        if (objResultVO.IsTechnicianEntryFinalized == true)
                        {
                            if (BizActionObj.AutoDeductStock == true)
                            {
                                if (BizActionObj.ItemCusmption == true)
                                {
                                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
                                    clsItemStockVO StockDetails = new clsItemStockVO();

                                    StockDetails.ItemID = ItemList.ItemID;
                                    StockDetails.BatchID = ItemList.BatchID;
                                    StockDetails.TransactionTypeID = InventoryTransactionType.RadiologyTestConsumption;
                                    StockDetails.TransactionQuantity = (ItemList.ActualQantity + ItemList.WastageQantity);
                                    StockDetails.BatchCode = ItemList.BatchCode;
                                    StockDetails.TransactionID = BizActionObj.ResultDetails.ID;
                                    StockDetails.Date = DateTime.Now;
                                    StockDetails.Time = DateTime.Now;
                                    StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                                    StockDetails.StoreID = ItemList.StoreID;
                                    obj.Details = StockDetails;
                                    obj.Details.ID = 0;

                                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                                    if (obj.SuccessStatus == -1)
                                    {
                                        throw new Exception();
                                    }

                                    StockDetails.ID = obj.Details.ID;
                                }

                            }
                        }
                    }
                }
                trans.Commit();


            }
            catch
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.ResultDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        //PACS
        public override IValueObject GetPACSTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbCommand command;
            clsGetPACSTestListBizActionVO BizActionObj = valueObject as clsGetPACSTestListBizActionVO;
            try
            {
                if (BizActionObj.IsForStudyComparision == true)
                {
                    command = dbServer.GetStoredProcCommand("PACS_DisplayStudyVisitWise");
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("Pacsdatashow");
                }
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MRNO);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PACSTestList == null)
                        BizActionObj.PACSTestList = new List<clsPACSTestPropertiesVO>();
                    while (reader.Read())
                    {
                        clsPACSTestPropertiesVO obj = new clsPACSTestPropertiesVO();
                        obj.MODALITY = Convert.ToString(DALHelper.HandleDBNull(reader["MODALITY"]));
                        obj.IMAGECOUNT = Convert.ToString(DALHelper.HandleDBNull(reader["IMAGECOUNT"]));
                        obj.STUDYDESC = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC"]));
                        obj.STUDYDATE = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDATE"]));
                        obj.STUDYTIME = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYTIME"]));
                        obj.PHYSICIANNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PHYSICIANNAME"]));
                        obj.PATIENTNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTNAME"]));
                        obj.PATIENTID = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTID"]));
                        BizActionObj.PACSTestList.Add(obj);
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
        public override IValueObject GetPACSTestSeriesList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPACSTestSeriesListBizActionVO BizActionObj = valueObject as clsGetPACSTestSeriesListBizActionVO;
            try
            {
                if (BizActionObj.IsForShowPACS == false)
                {

                    if (BizActionObj.IsForStudyComparision == true)
                    {
                        DbCommand command;
                        command = dbServer.GetStoredProcCommand("PACS_ShowPatientStudyDetailsForVisitWise");
                        dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.PACSTestDetails.MRNO);
                        dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, BizActionObj.PACSTestDetails.PATIENTNAME);
                        dbServer.AddInParameter(command, "MODALITY", DbType.String, BizActionObj.PACSTestDetails.MODALITY);
                        dbServer.AddInParameter(command, "STUDYDATE", DbType.String, BizActionObj.PACSTestDetails.STUDYDATE);
                        dbServer.AddInParameter(command, "STUDYTIME", DbType.String, BizActionObj.PACSTestDetails.STUDYTIME);
                        dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
                        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PACSTestSeriesList == null)
                                BizActionObj.PACSTestSeriesList = new List<clsPACSTestSeriesVO>();
                            while (reader.Read())
                            {
                                clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
                                obj.PATIENTID = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTID"]));
                                obj.PATIENTNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTNAME"]));
                                obj.MODALITY = Convert.ToString(DALHelper.HandleDBNull(reader["MODALITY"]));
                                obj.STUDYDESC = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC"]));
                                obj.STUDYDATE = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDATE"]));
                                obj.STUDYTIME = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYTIME"]));
                                obj.SERIESNUMBER = Convert.ToString(DALHelper.HandleDBNull(reader["SERIESNUMBER"]));
                                obj.STUDYDESC1 = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC1"]));
                                BizActionObj.PACSTestSeriesList.Add(obj);
                            }
                        }
                        reader.Close();
                    }
                    else
                    {
                        DbCommand command;
                        command = dbServer.GetStoredProcCommand("ShowPatientStudyDetails");
                        dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.PACSTestDetails.MRNO);
                        dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, BizActionObj.PACSTestDetails.PATIENTNAME);
                        dbServer.AddInParameter(command, "MODALITY", DbType.String, BizActionObj.PACSTestDetails.MODALITY);
                        dbServer.AddInParameter(command, "STUDYDATE", DbType.String, BizActionObj.PACSTestDetails.STUDYDATE);
                        dbServer.AddInParameter(command, "STUDYTIME", DbType.String, BizActionObj.PACSTestDetails.STUDYTIME);
                        dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
                        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PACSTestSeriesList == null)
                                BizActionObj.PACSTestSeriesList = new List<clsPACSTestSeriesVO>();
                            while (reader.Read())
                            {
                                clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
                                obj.PATIENTID = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTID"]));
                                obj.PATIENTNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTNAME"]));
                                obj.MODALITY = Convert.ToString(DALHelper.HandleDBNull(reader["MODALITY"]));
                                obj.STUDYDESC = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC"]));
                                obj.STUDYDATE = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDATE"]));
                                obj.STUDYTIME = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYTIME"]));
                                obj.SERIESNUMBER = Convert.ToString(DALHelper.HandleDBNull(reader["SERIESNUMBER"]));
                                BizActionObj.PACSTestSeriesList.Add(obj);
                            }
                        }
                        reader.Close();
                    }
                }
                else
                {
                    if (BizActionObj.IsForShowSeriesPACS == true)
                    {

                        DbCommand command = dbServer.GetStoredProcCommand("ShowPatientImages_StudyWise");

                        dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.PACSTestDetails.MRNO);
                        //dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, BizActionObj.PACSTestDetails.PATIENTNAME);
                        //dbServer.AddInParameter(command, "MODALITY", DbType.String, BizActionObj.PACSTestDetails.MODALITY);
                        //dbServer.AddInParameter(command, "STUDYDATE", DbType.String, BizActionObj.PACSTestDetails.STUDYDATE);
                        //dbServer.AddInParameter(command, "STUDYTIME", DbType.String, BizActionObj.PACSTestDetails.STUDYTIME);
                        //dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
                        //dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
                        dbServer.AddInParameter(command, "SERIESNUMBER", DbType.String, BizActionObj.PACSTestDetails.SERIESNUMBER);
                        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PACSTestSeriesImageList == null)
                                BizActionObj.PACSTestSeriesImageList = new List<clsPACSTestSeriesVO>();
                            while (reader.Read())
                            {
                                clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
                                obj.IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]));
                                BizActionObj.PACSTestSeriesImageList.Add(obj);
                            }
                        }
                        reader.Close();
                    }
                    else
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("ShowPatientImages");

                        dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.PACSTestDetails.MRNO);
                        dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, BizActionObj.PACSTestDetails.PATIENTNAME);
                        dbServer.AddInParameter(command, "MODALITY", DbType.String, BizActionObj.PACSTestDetails.MODALITY);
                        dbServer.AddInParameter(command, "STUDYDATE", DbType.String, BizActionObj.PACSTestDetails.STUDYDATE);
                        dbServer.AddInParameter(command, "STUDYTIME", DbType.String, BizActionObj.PACSTestDetails.STUDYTIME);
                        dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
                        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                        if (reader.HasRows)
                        {
                            if (BizActionObj.PACSTestSeriesImageList == null)
                                BizActionObj.PACSTestSeriesImageList = new List<clsPACSTestSeriesVO>();
                            while (reader.Read())
                            {
                                clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
                                obj.IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]));
                                BizActionObj.PACSTestSeriesImageList.Add(obj);
                            }
                        }

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

        public override IValueObject GetVitalDetailsForRadiology(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetVitalDetailsForRadiologyBizActionVO BizActionObj = valueObject as clsGetVitalDetailsForRadiologyBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVitalDetailsForRadiology");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.VitalDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.VitalDetails.PatientUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.VitalDetails == null)
                        BizActionObj.VitalDetails = new clsVitalDetailsForRadiologyVO();
                    while (reader.Read())
                    {
                        BizActionObj.VitalDetails.height = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientHeight"]));
                        BizActionObj.VitalDetails.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientWeight"]));
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

        public override clsPACSTestSeriesVO GetPACSTestSeriesListaspx(string MRNO, string PATIENTNAME, string MODALITY, string STUDYDATE, string STUDYTIME, string STUDYDESC)
        {
            //throw new NotImplementedException();
            clsPACSTestSeriesVO pacs = new clsPACSTestSeriesVO();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("ShowPatientImages");
                dbServer.AddInParameter(command, "MRNO", DbType.String, MRNO);
                dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, PATIENTNAME);
                dbServer.AddInParameter(command, "MODALITY", DbType.String, MODALITY);
                dbServer.AddInParameter(command, "STUDYDATE", DbType.String, STUDYDATE);
                dbServer.AddInParameter(command, "STUDYTIME", DbType.String, STUDYTIME);
                dbServer.AddInParameter(command, "STUDYDESC", DbType.String, STUDYDESC);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                pacs.PacsTestSeriesList = new List<clsPACSTestSeriesVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
                        obj.IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]));
                        pacs.PacsTestSeriesList.Add(obj);
                    }
                }
                reader.Close();
            }
            catch
            {

            }
            return pacs;
            //try
            //{
            //    if (BizActionObj.IsForShowPACS == false)
            //    {
            //        DbCommand command = dbServer.GetStoredProcCommand("ShowPatientStudyDetails");
            //        dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.PACSTestDetails.MRNO);
            //        dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, BizActionObj.PACSTestDetails.PATIENTNAME);
            //        dbServer.AddInParameter(command, "MODALITY", DbType.String, BizActionObj.PACSTestDetails.MODALITY);
            //        dbServer.AddInParameter(command, "STUDYDATE", DbType.String, BizActionObj.PACSTestDetails.STUDYDATE);
            //        dbServer.AddInParameter(command, "STUDYTIME", DbType.String, BizActionObj.PACSTestDetails.STUDYTIME);
            //        dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
            //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            //        if (reader.HasRows)
            //        {
            //            if (BizActionObj.PACSTestSeriesList == null)
            //                BizActionObj.PACSTestSeriesList = new List<clsPACSTestSeriesVO>();
            //            while (reader.Read())
            //            {
            //                clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
            //                obj.PATIENTID = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTID"]));
            //                obj.PATIENTNAME = Convert.ToString(DALHelper.HandleDBNull(reader["PATIENTNAME"]));
            //                obj.MODALITY = Convert.ToString(DALHelper.HandleDBNull(reader["MODALITY"]));
            //                obj.STUDYDESC = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDESC"]));
            //                obj.STUDYDATE = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYDATE"]));
            //                obj.STUDYTIME = Convert.ToString(DALHelper.HandleDBNull(reader["STUDYTIME"]));
            //                obj.SERIESNUMBER = Convert.ToString(DALHelper.HandleDBNull(reader["SERIESNUMBER"]));
            //                BizActionObj.PACSTestSeriesList.Add(obj);
            //            }
            //        }
            //        reader.Close();
            //    }
            //    else
            //    {
            //        if (BizActionObj.IsForShowSeriesPACS == true)
            //        {

            //            DbCommand command = dbServer.GetStoredProcCommand("ShowPatientImages_StudyWise");

            //            dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.PACSTestDetails.MRNO);
            //            //dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, BizActionObj.PACSTestDetails.PATIENTNAME);
            //            //dbServer.AddInParameter(command, "MODALITY", DbType.String, BizActionObj.PACSTestDetails.MODALITY);
            //            //dbServer.AddInParameter(command, "STUDYDATE", DbType.String, BizActionObj.PACSTestDetails.STUDYDATE);
            //            //dbServer.AddInParameter(command, "STUDYTIME", DbType.String, BizActionObj.PACSTestDetails.STUDYTIME);
            //            //dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
            //            //dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
            //            dbServer.AddInParameter(command, "SERIESNUMBER", DbType.String, BizActionObj.PACSTestDetails.SERIESNUMBER);
            //            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            //            if (reader.HasRows)
            //            {
            //                if (BizActionObj.PACSTestSeriesImageList == null)
            //                    BizActionObj.PACSTestSeriesImageList = new List<clsPACSTestSeriesVO>();
            //                while (reader.Read())
            //                {
            //                    clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
            //                    obj.IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]));
            //                    BizActionObj.PACSTestSeriesImageList.Add(obj);
            //                }
            //            }
            //            reader.Close();
            //        }
            //        else
            //        {
            //            DbCommand command = dbServer.GetStoredProcCommand("ShowPatientImages");

            //            dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.PACSTestDetails.MRNO);
            //            dbServer.AddInParameter(command, "PATIENTNAME", DbType.String, BizActionObj.PACSTestDetails.PATIENTNAME);
            //            dbServer.AddInParameter(command, "MODALITY", DbType.String, BizActionObj.PACSTestDetails.MODALITY);
            //            dbServer.AddInParameter(command, "STUDYDATE", DbType.String, BizActionObj.PACSTestDetails.STUDYDATE);
            //            dbServer.AddInParameter(command, "STUDYTIME", DbType.String, BizActionObj.PACSTestDetails.STUDYTIME);
            //            dbServer.AddInParameter(command, "STUDYDESC", DbType.String, BizActionObj.PACSTestDetails.STUDYDESC);
            //            DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
            //            if (reader.HasRows)
            //            {
            //                if (BizActionObj.PACSTestSeriesImageList == null)
            //                    BizActionObj.PACSTestSeriesImageList = new List<clsPACSTestSeriesVO>();
            //                while (reader.Read())
            //                {
            //                    clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
            //                    obj.IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]));
            //                    BizActionObj.PACSTestSeriesImageList.Add(obj);
            //                }
            //            }

            //            reader.Close();
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //finally
            //{
            //}
            //return BizActionObj;
        }

        public override clsPACSTestSeriesVO GetPACSTestseriesListSeriesWise(bool IsForShowSeriesPACS, bool IsForShowPACS, string SERIESNUMBER, string MRNO, bool IsVisitWise, string MODALITY, string STUDYTIME, string STUDYDESC, string STUDYDATE)
        {
            clsPACSTestSeriesVO pacs = new clsPACSTestSeriesVO();
            try
            {
                DbCommand command;
                if (IsVisitWise == true)
                {
                    command = dbServer.GetStoredProcCommand("PACS_VisitWiseImages");
                    dbServer.AddInParameter(command, "STUDYTIME", DbType.String, STUDYTIME);
                    dbServer.AddInParameter(command, "STUDYDATE", DbType.String, STUDYDATE);
                    dbServer.AddInParameter(command, "STUDYDESC", DbType.String, STUDYDESC);
                    dbServer.AddInParameter(command, "MODALITY", DbType.String, MODALITY);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("ShowPatientImages_StudyWise");
                }
                dbServer.AddInParameter(command, "MRNO", DbType.String, MRNO);
                dbServer.AddInParameter(command, "SERIESNUMBER", DbType.String, SERIESNUMBER);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                pacs.PacsTestSeriesList = new List<clsPACSTestSeriesVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPACSTestSeriesVO obj = new clsPACSTestSeriesVO();
                        obj.IMAGEPATH = Convert.ToString(DALHelper.HandleDBNull(reader["Imagepath"]));
                        pacs.PacsTestSeriesList.Add(obj);
                    }
                }
                reader.Close();
            }
            catch
            {

            }
            return pacs;
        }
    }
}
