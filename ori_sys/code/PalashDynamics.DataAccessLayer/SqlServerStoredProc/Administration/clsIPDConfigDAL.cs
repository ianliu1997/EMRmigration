using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Administration.IPD;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsIPDConfigDAL: clsBaseIPDConfigDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public clsIPDConfigDAL()
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

        public override ValueObjects.IValueObject AddUpdateBlockMaster(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsIPDBlockMasterVO objItemVO = new clsIPDBlockMasterVO();
            clsAddUpdateIPDBlockMasterBizActionVO objItem = valueObject as clsAddUpdateIPDBlockMasterBizActionVO;

            try
            {
                DbCommand command;
                objItemVO = objItem.objBlockMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateBlockMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
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
                throw;
            }
            return objItem;
            
        }

        public override ValueObjects.IValueObject GetBlockMasterDetails(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override ValueObjects.IValueObject GetBlockMasterList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetBlockMasterBizActionVO objItem = valueObject as clsIPDGetBlockMasterBizActionVO;
            clsIPDBlockMasterVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBlockMasterDetails");
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
                        objItemVO = new clsIPDBlockMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                       // objItemVO. = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTheatreId"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        objItem.objBlockMasterDetails.Add(objItemVO);
                    }
                }
            }
            catch (Exception ex)
            {                
                throw;
            }
            return objItem;
        }

        public override ValueObjects.IValueObject AddUpdateWardMaster(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsIPDWardMasterVO objItemVO = new clsIPDWardMasterVO();
            clsIPDAddUpdateWardMasterBizActionVO objItem = valueObject as clsIPDAddUpdateWardMasterBizActionVO;

            try
            {
                DbCommand command;
                objItemVO = objItem.objWardMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateWardMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "FloorId", DbType.Int64, objItemVO.FloorID);
                dbServer.AddInParameter(command, "Sex", DbType.Int64, objItemVO.CategoryID); 
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "BlockID", DbType.Int64, objItemVO.BlockID);
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
                throw;
            }
            return objItem;
        }

        public override ValueObjects.IValueObject GetBedMasterList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader1 = null;
            clsIPDGetBedMasterBizActionVO objItem = valueObject as clsIPDGetBedMasterBizActionVO;
            clsIPDBedMasterVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBedMasterDetails");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
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
                        objItemVO = new clsIPDBedMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardId"]));
                        objItemVO.BedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"]));
                        objItemVO.RoomID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RoomId"]));
                        objItemVO.WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                        objItemVO.BedCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["BedCategoryName"]));
                        objItemVO.RoomName = Convert.ToString(DALHelper.HandleDBNull(reader["RoomName"]));
                        objItemVO.IsAmmenity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBedAmme"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.IsNonCensus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNonCensus"]));
                        objItemVO.BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]));
                        DbCommand command1;
                        command1 = dbServer.GetStoredProcCommand("CIMS_GetBedMasterAmmenitiesDetails");
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, objItemVO.ID);
                        reader1 = (DbDataReader)dbServer.ExecuteReader(command1);
                        if (reader1.HasRows)
                        {
                            objItemVO.AmmenityDetails = new List<MasterListItem>();
                            while (reader1.Read())
                            {
                                MasterListItem objUnit = new MasterListItem();
                                //objUnit.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ID"]));
                                objUnit.ID = (long)DALHelper.HandleDBNull(reader1["BedAmmenityID"]);
                                objUnit.Description = Convert.ToString(DALHelper.HandleDBNull(reader1["Ammenity"]));
                                objUnit.Status = Convert.ToBoolean(reader1["AmmenityStatus"]);
                                objItemVO.AmmenityDetails.Add(objUnit);
                            }
                        }
                        reader1.Close();
                        objItem.objBedMasterDetails.Add(objItemVO);
                    }
                    reader.NextResult();
                    objItem.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        public override ValueObjects.IValueObject GetWardMasterList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetWardMasterBizActionVO objItem = valueObject as clsIPDGetWardMasterBizActionVO;
            clsIPDWardMasterVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetWardMasterDetails");
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
                        objItemVO = new clsIPDWardMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.FloorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FloorId"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        objItemVO.FloorName = Convert.ToString(DALHelper.HandleDBNull(reader["FloorName"]));
                        objItemVO.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardType"]));
                        objItemVO.CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["WardTypeName"]));
                        objItemVO.BlockID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlockId"]));
                        objItemVO.BlockName = Convert.ToString(DALHelper.HandleDBNull(reader["BlockName"]));
                        objItem.objWardMasterDetails.Add(objItemVO);
                    }
                    reader.NextResult();
                    objItem.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        public override ValueObjects.IValueObject AddUpdateBedMaster(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsIPDBedMasterVO ObjService = new clsIPDBedMasterVO();
            clsIPDAddUpdateBedMasterBizActionVO objItem = valueObject as clsIPDAddUpdateBedMasterBizActionVO;

            try
            {
                DbCommand command;
                ObjService = objItem.objBedMatserDetails;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateBedMaster");
                //dbServer.AddInParameter(command, "ID", DbType.Int64, ObjService.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ObjService.UnitID);
                dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, ObjService.BedUnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjService.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, ObjService.Description);
                dbServer.AddInParameter(command, "BedCategoryId", DbType.Int64, ObjService.BedCategoryID);
                dbServer.AddInParameter(command, "WardId", DbType.Int64, ObjService.WardID);
                dbServer.AddInParameter(command, "RoomId", DbType.Int64, ObjService.RoomID);
                dbServer.AddInParameter(command, "IsAmmenity", DbType.Boolean, ObjService.IsAmmenity);
                dbServer.AddInParameter(command, "IsNonCensus", DbType.Boolean, ObjService.IsNonCensus);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjService.Status);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjService.ID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, ObjService.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, ObjService.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, ObjService.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, ObjService.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjService.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, ObjService.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, ObjService.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjService.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, ObjService.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, ObjService.UpdateWindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                ObjService.ID = (long)dbServer.GetParameterValue(command, "ID");


                DbCommand command1;

                command1 = dbServer.GetStoredProcCommand("CIMS_DeleteBedAmmenities");
                dbServer.AddInParameter(command1, "BedID", DbType.Int64, ObjService.ID);

                int intdeleteStatus = dbServer.ExecuteNonQuery(command1);

                DbCommand command2;

                command2 = dbServer.GetStoredProcCommand("CIMS_AddBedAmmenitiesDetails");

                foreach (MasterListItem objBedItem in ObjService.AmmenityDetails)
                {
                    command2.Parameters.Clear();
                    dbServer.AddInParameter(command2, "BedID", DbType.Int64, ObjService.ID);
                    dbServer.AddInParameter(command2, "SelUnitID", DbType.Int64, ObjService.UnitID);
                    dbServer.AddInParameter(command2, "SelAmmID", DbType.Int64, objBedItem.ID);
                    dbServer.AddInParameter(command2, "SelAmmStaus", DbType.Boolean, ObjService.Status);

                    int intBedStatus = dbServer.ExecuteNonQuery(command2);
                }
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;


            //clsIPDBedMasterVO objItemVO = new clsIPDBedMasterVO();
            //clsIPDAddUpdateBedMasterBizActionVO objItem = valueObject as clsIPDAddUpdateBedMasterBizActionVO;
            //try
            //{
            //    DbCommand command;
            //    objItemVO = objItem.objBedMatserDetails;
            //    command = dbServer.GetStoredProcCommand("CIMS_AddUpdateBedMaster");
            //    dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
            //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
            //    dbServer.AddInParameter(command, "BedUnitID", DbType.Int64, objItemVO.BedUnitID);
            //    dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
            //    dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
            //    dbServer.AddInParameter(command, "BedCategoryId", DbType.Int64, objItemVO.BedCategoryID);
            //    dbServer.AddInParameter(command, "WardId", DbType.Int64, objItemVO.WardID);
            //    dbServer.AddInParameter(command, "RoomId", DbType.Int64, objItemVO.RoomID);
            //    dbServer.AddInParameter(command, "IsAmmenity", DbType.Boolean, objItemVO.IsAmmenity);
            //    dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
            //    dbServer.AddInParameter(command, "IsNonCensus", DbType.Boolean, objItemVO.IsNonCensus);
            //    if (objItemVO.IsAmmenity == true)
            //    {
            //        StringBuilder AmmenityIdList = new StringBuilder();
            //        StringBuilder AmmenityStatusList = new StringBuilder();
            //        StringBuilder ABedIdList = new StringBuilder();
            //        StringBuilder UnitIdList = new StringBuilder();
            //        for (int UnitCount = 0; UnitCount < objItemVO.AmmenityDetails.Count; UnitCount++)
            //        {
            //            AmmenityIdList.Append(objItemVO.AmmenityDetails[UnitCount].ID);
            //            AmmenityStatusList.Append(objItemVO.AmmenityDetails[UnitCount].Status);
            //            ABedIdList.Append(objItemVO.ID);
            //            UnitIdList.Append(objItemVO.UnitID);
            //            if (UnitCount < (objItemVO.AmmenityDetails.Count - 1))
            //            {
            //                AmmenityIdList.Append(",");
            //                AmmenityStatusList.Append(",");
            //                ABedIdList.Append(",");
            //                UnitIdList.Append(",");
            //            }
            //        }
            //        dbServer.AddInParameter(command, "AmmenityIdList", DbType.String, AmmenityIdList.ToString());
            //        dbServer.AddInParameter(command, "AmmenityStatusList", DbType.String, AmmenityStatusList.ToString());
            //        dbServer.AddInParameter(command, "ARoomIdList", DbType.String, ABedIdList.ToString());
            //        dbServer.AddInParameter(command, "UnitIdList", DbType.String, UnitIdList.ToString());
            //    }
            //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
            //    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
            //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
            //    dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
            //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
            //    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
            //    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
            //    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
            //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.AddedWindowsLoginName);
            //    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
            //    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

            //    int intStatus = dbServer.ExecuteNonQuery(command);
            //    objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //return objItem;
        }

        public override ValueObjects.IValueObject AddUpdateRoomMasterList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsIPDRoomMasterVO objItemVO = new clsIPDRoomMasterVO();
            clsIPDAddUpdateRoomMasterBizActionVO objItem = valueObject as clsIPDAddUpdateRoomMasterBizActionVO;

            try
            {
                DbCommand command;
                objItemVO = objItem.objRoomMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateRoomMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "IsRoomAmmenities", DbType.Boolean, objItemVO.IsAmmenity);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                if (objItemVO.IsAmmenity == true)
                {

                    StringBuilder AmmenityIdList = new StringBuilder();
                    StringBuilder AmmenityStatusList = new StringBuilder();
                    StringBuilder ARoomIdList = new StringBuilder();
                    StringBuilder UnitIdList = new StringBuilder();

                    for (int UnitCount = 0; UnitCount < objItemVO.AmmenityDetails.Count; UnitCount++)
                    {
                        AmmenityIdList.Append(objItemVO.AmmenityDetails[UnitCount].ID);
                        AmmenityStatusList.Append(objItemVO.AmmenityDetails[UnitCount].Status);
                        ARoomIdList.Append(objItemVO.ID);
                        UnitIdList.Append(objItemVO.UnitID);
                        if (UnitCount < (objItemVO.AmmenityDetails.Count - 1))
                        {
                            AmmenityIdList.Append(",");
                            AmmenityStatusList.Append(",");
                            ARoomIdList.Append(",");
                            UnitIdList.Append(",");
                        }
                    }

                    dbServer.AddInParameter(command, "AmmenityIdList", DbType.String, AmmenityIdList.ToString());
                    dbServer.AddInParameter(command, "AmmenityStatusList", DbType.String, AmmenityStatusList.ToString());
                    dbServer.AddInParameter(command, "ARoomIdList", DbType.String, ARoomIdList.ToString());
                    dbServer.AddInParameter(command, "UnitIdList", DbType.String, UnitIdList.ToString());
                }
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
                throw;
            }
            return objItem;
        }

        public override IValueObject UpdateRoomMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateRoomStatusBizActionVO bizObject = valueObject as clsIPDUpdateRoomStatusBizActionVO;
            try
            {
                clsIPDRoomMasterVO objVO = bizObject.RoomStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateRoomMasterStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.RoomStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }
       
        public override ValueObjects.IValueObject GetRoomMasterList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader1 = null;
            clsIPDGetRoomMasterBizActionVO objItem = valueObject as clsIPDGetRoomMasterBizActionVO;
            clsIPDRoomMasterVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetRoomMasterDetails");
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
                        objItemVO = new clsIPDRoomMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.IsAmmenity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRoomAmme"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));

                        DbCommand command1;
                        command1 = dbServer.GetStoredProcCommand("CIMS_GetRoomMasterAmmenitiesDetails");
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, objItemVO.ID);
                        reader1 = (DbDataReader)dbServer.ExecuteReader(command1);

                        if (reader1.HasRows)
                        {
                            //objItem.objRoomMasterAmmenities.AmmenityDetails = new List<MasterListItem>();
                            objItemVO.AmmenityDetails = new List<MasterListItem>();
                            while (reader1.Read())
                            {
                                MasterListItem objUnit = new MasterListItem();

                                objUnit.ID = (long)DALHelper.HandleDBNull(reader1["ID"]);
                                objUnit.Description = (string)DALHelper.HandleDBNull(reader1["Ammenity"]);
                                objUnit.Status = reader1["AmmenityStatus"].HandleDBNull() == null ? false : Convert.ToBoolean(reader1["AmmenityStatus"]);
                                objItemVO.AmmenityDetails.Add(objUnit);
                                //objItem.objRoomMasterAmmenities.AmmenityDetails.Add(objUnit);
                            }
                        }
                        reader1.Close();
                        objItem.objRoomMasterDetails.Add(objItemVO);
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
            return objItem;
        }        

        public override ValueObjects.IValueObject GetRoomMasterDetails(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetRoomMasterBizActionVO objItem = valueObject as clsIPDGetRoomMasterBizActionVO;
            clsIPDRoomMasterVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetRoomMasterAmmenitiesDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItem.ID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsIPDRoomMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItem.objRoomMasterDetails.Add(objItemVO);
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    objItem.objRoomMasterAmmenities.AmmenityDetails = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem objUnit = new MasterListItem();

                        objUnit.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objUnit.Description = (string)DALHelper.HandleDBNull(reader["Ammenity"]);
                        objUnit.Status = reader["AmmenityStatus"].HandleDBNull() == null ? false : Convert.ToBoolean(reader["AmmenityStatus"]);
                        objItem.objRoomMasterAmmenities.AmmenityDetails.Add(objUnit);                        
                    }
                }
            }
            catch (Exception ex)
            {                
                throw;
            }
            return objItem;
        }

        public override IValueObject AddUpdateClassMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDClassMasterVO objItemVO = new clsIPDClassMasterVO();
            clsIPDAddUpdateClassMasterBizActionVO objItem = valueObject as clsIPDAddUpdateClassMasterBizActionVO;

            try
            {
                DbCommand command;
                objItemVO = objItem.objClassMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateClassMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "DepositAdmission", DbType.Decimal, objItemVO.DepositIPD);
                dbServer.AddInParameter(command, "DepositOT", DbType.Decimal, objItemVO.DepositOT);
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
                throw;
            }
            return objItem;
        }

        public override IValueObject GetClassMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetClassMasterBizActionVO objItem = valueObject as clsIPDGetClassMasterBizActionVO;
            clsIPDClassMasterVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetClassMasterDetails");
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
                        objItemVO = new clsIPDClassMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        // objItemVO. = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTheatreId"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objItemVO.DepositIPD = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DepositAdmission"]));
                        objItemVO.DepositOT = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DepositOT"]));
                        objItem.objClassMasterDetails.Add(objItemVO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        public override IValueObject GetBedListByDifferentSearch(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
           
            clsGetIPDBedListBizActionVO BizActionObj = valueObject as clsGetIPDBedListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetIPDBedList");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, BizActionObj.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Description);
                dbServer.AddInParameter(command, "BedCategoryId", DbType.Int64, BizActionObj.BedCategoryID);
                dbServer.AddInParameter(command, "WardID", DbType.Int64, BizActionObj.WardID);
                dbServer.AddInParameter(command, "RoomID", DbType.Int64, BizActionObj.RoomID);
                dbServer.AddInParameter(command, "Occupied", DbType.Boolean, BizActionObj.Occupied);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String,"ID");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                
                reader = (DbDataReader)dbServer.ExecuteReader(command);
               
                if (reader.HasRows)
                {
                    if (BizActionObj.BedDetails == null)
                        BizActionObj.BedDetails = new List<clsIPDBedMasterVO>();

                    while (reader.Read())
                    {
                        clsIPDBedMasterVO BedVO = new clsIPDBedMasterVO();
                        BedVO.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        BedVO.UnitID = (long)(DALHelper.HandleDBNull(reader["UnitID"]));
                        BedVO.Code = (string)(DALHelper.HandleDBNull(reader["Code"]));
                        BedVO.Description = (string)(DALHelper.HandleDBNull(reader["Description"]));
                        BedVO.WardID = (long)(DALHelper.HandleDBNull(reader["WardID"]));
                        BedVO.WardName = (string)(DALHelper.HandleDBNull(reader["Ward"]));
                        BedVO.BedCategoryID = (long)(DALHelper.HandleDBNull(reader["BedCategoryId"]));
                        BedVO.BedCategoryName = (string)(DALHelper.HandleDBNull(reader["BedCategory"]));
                        BedVO.RoomID = (long)(DALHelper.HandleDBNull(reader["RoomID"]));
                        BedVO.RoomName = (string)(DALHelper.HandleDBNull(reader["Room"]));
                        BedVO.Status = (bool)(DALHelper.HandleDBNull(reader["status"]));
                        BedVO.Occupied = (bool)(DALHelper.HandleDBNull(reader["Occupied"]));

                        BizActionObj.BedDetails.Add(BedVO);
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
            return BizActionObj;
        }

        public override IValueObject UpdateBedMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateBedMasterStatusBizActionVO bizObject = valueObject as clsIPDUpdateBedMasterStatusBizActionVO;
            try
            {
                clsIPDBedMasterVO objVO = bizObject.BedStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateBedMasterStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.BedStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        public override IValueObject UpdateBlockMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateBlockMasterStatusBizActionVO bizObject = valueObject as clsIPDUpdateBlockMasterStatusBizActionVO;
            try
            {
                clsIPDBlockMasterVO objVO = bizObject.BlockStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateBlockMasterStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.BlockStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        public override IValueObject UpdateClassMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateClassMasterStatusBizActionVO bizObject = valueObject as clsIPDUpdateClassMasterStatusBizActionVO;
            try
            {
                clsIPDClassMasterVO objVO = bizObject.ClassStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateClassMasterStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.ClassStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        public override IValueObject UpdateWardMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateWardMasterStatusBizActionVO bizObject = valueObject as clsIPDUpdateWardMasterStatusBizActionVO;
            try
            {
                clsIPDWardMasterVO objVO = bizObject.WardStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateWardMasterStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.WardStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        public override IValueObject AddUpdateFoodItemMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDDietNutritionMasterVO objItemVO = new clsIPDDietNutritionMasterVO();
            clsIPDAddUpdateDietNutritionBizActionVO objItem = valueObject as clsIPDAddUpdateDietNutritionBizActionVO;

            try
            {
                DbCommand command;
                objItemVO = objItem.objDietMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateFoodItemMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "ItemId", DbType.Int64, objItemVO.ItemID);
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
                throw;
            }
            return objItem;
        }

        public override IValueObject GetFoodItemMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetDietNutritionBizActionVO objItem = valueObject as clsIPDGetDietNutritionBizActionVO;
            clsIPDDietNutritionMasterVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetFoodItemMasterDetails");
                string sSearch = '%' + objItem.SearchExpression + '%';
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, sSearch);
                dbServer.AddInParameter(command, "SearchCategory", DbType.Int64, objItem.SearchCategory);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsIPDDietNutritionMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));

                        objItem.objDietMasterDetails.Add(objItemVO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        public override IValueObject UpdateStatusFoodItemMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateDietNutritionStatusBizActionVO bizObject = valueObject as clsIPDUpdateDietNutritionStatusBizActionVO;
            try
            {
                clsIPDDietNutritionMasterVO objVO = bizObject.DietStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateFoodItemMasterStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.DietStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }

        private clsIPDAddUpdateDietPlanBizactionVO AddDietPlan(clsIPDAddUpdateDietPlanBizactionVO objItem, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsIPDDietPlanMasterVO objItemVO = new clsIPDDietPlanMasterVO();
                objItemVO = objItem.objDietMatserDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDietPlanMaster");
               // dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "PlanInfo", DbType.String, objItemVO.PlanInst);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objItemVO.CreatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objItemVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objItemVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objItemVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objItemVO.ID);                        
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                objItem.objDietMatserDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                
                if (objItem.SuccessStatus == 1)
                {
                    if (objItemVO.ItemList != null && objItemVO.ItemList.Count > 0)
                    {
                        foreach (var item in objItemVO.ItemList)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDietPlanDetails");

                            dbServer.AddInParameter(command1, "PlanID", DbType.Int64, objItemVO.ID);
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objItemVO.UnitID);
                            dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.FoodItem.ID);
                            dbServer.AddInParameter(command1, "Timing", DbType.String, item.Timing);

                            dbServer.AddInParameter(command1, "ItemQty", DbType.String, item.ItemQty);
                            dbServer.AddInParameter(command1, "ItemUnit", DbType.String, item.ItemUnit);
                            dbServer.AddInParameter(command1, "ItemCal", DbType.String, item.ItemCalories);
                            dbServer.AddInParameter(command1, "ItemCH", DbType.String, item.ItemCh);
                            dbServer.AddInParameter(command1, "ItemFat", DbType.String, item.ItemFat);
                            dbServer.AddInParameter(command1, "ItemExpectedCal", DbType.String, item.ExpectedCal);
                            dbServer.AddInParameter(command1, "ItemInst", DbType.String, item.ItemInst);
                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                          //  objItem.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                            item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                objItem.objDietMatserDetails = null;
            }
            finally
            {
                trans = null;
                con.Close();
                con = null;
            }

            return objItem;
        }

        private clsIPDAddUpdateDietPlanBizactionVO UpdateDietPlan(clsIPDAddUpdateDietPlanBizactionVO objItem, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsIPDDietPlanMasterVO objItemVO = new clsIPDDietPlanMasterVO();
                objItemVO = objItem.objDietMatserDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDietPlanMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objItemVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description);
                dbServer.AddInParameter(command, "PlanInfo", DbType.String, objItemVO.PlanInst);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objItemVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objItemVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objItemVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objItemVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objItemVO.UpdateWindowsLoginName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));

                if (objItem.SuccessStatus == 1)
                {
                    if (objItemVO.ItemList != null && objItemVO.ItemList.Count != 0)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteDietPlanDetails");
                        dbServer.AddInParameter(command2, "PlanId", DbType.Int64, objItemVO.ID);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    }

                    if (objItemVO.ItemList != null && objItemVO.ItemList.Count > 0)
                    {
                        foreach (var item in objItemVO.ItemList)
                        {
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDietPlanDetails");

                            dbServer.AddInParameter(command1, "PlanID", DbType.Int64, objItemVO.ID);
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objItemVO.UnitID);
                            dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.FoodItem.ID);
                            dbServer.AddInParameter(command1, "Timing", DbType.String, item.Timing);

                            dbServer.AddInParameter(command1, "ItemQty", DbType.String, item.ItemQty);
                            dbServer.AddInParameter(command1, "ItemUnit", DbType.String, item.ItemUnit);
                            dbServer.AddInParameter(command1, "ItemCal", DbType.String, item.ItemCalories);
                            dbServer.AddInParameter(command1, "ItemCH", DbType.String, item.ItemCh);
                            dbServer.AddInParameter(command1, "ItemFat", DbType.String, item.ItemFat);
                            dbServer.AddInParameter(command1, "ItemExpectedCal", DbType.String, item.ExpectedCal);
                            dbServer.AddInParameter(command1, "ItemInst", DbType.String, item.ItemInst);
                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                            //item.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                            item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                objItem.objDietMatserDetails = null;
            }
            finally
            {
                trans = null;
                con.Close();
                con = null;
            }

            return objItem;
        }

        public override IValueObject AddUpdateDietPlanMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            
            clsIPDAddUpdateDietPlanBizactionVO objItem = valueObject as clsIPDAddUpdateDietPlanBizactionVO;

            if (objItem.objDietMatserDetails.ID == 0)
                objItem = AddDietPlan(objItem, UserVo);
            else
                objItem = UpdateDietPlan(objItem, UserVo);
            
            return objItem;
        }

        public override IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader1 = null;
            clsIPDGetDietPlanBizActionVO objItem = valueObject as clsIPDGetDietPlanBizActionVO;
            clsIPDDietPlanMasterVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetDietPlan");
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsIPDDietPlanMasterVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItemVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                       //objItemVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItemVO.PlanInst = Convert.ToString(DALHelper.HandleDBNull(reader["PlanInformation"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        DbCommand command1;
                        command1 = dbServer.GetStoredProcCommand("CIMS_GetDietPlanDetails");
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, objItemVO.ID);
                        reader1 = (DbDataReader)dbServer.ExecuteReader(command1);

                        if (reader1.HasRows)
                        {
                            //objItem.objRoomMasterAmmenities.AmmenityDetails = new List<MasterListItem>();
                            objItemVO.ItemList = new List<clsIPDDietPlanItemMasterVO>();
                            while (reader1.Read())
                            {
                                clsIPDDietPlanItemMasterVO objFoodItem = new clsIPDDietPlanItemMasterVO();
                                objFoodItem.ID = (long)DALHelper.HandleDBNull(reader1["ID"]);
                                objFoodItem.ExpectedCal = (string)DALHelper.HandleDBNull(reader1["ItemExpectedCal"]);
                                objFoodItem.FoodItem.ID = (long)DALHelper.HandleDBNull(reader1["ItemID"]);
                                objFoodItem.FoodItem.Description = (string)DALHelper.HandleDBNull(reader1["FoodItemName"]);
                                objFoodItem.FoodItem.ItemName = (string)DALHelper.HandleDBNull(reader1["FoodItemCatName"]);
                                objFoodItem.FoodItem.ItemID = (long)DALHelper.HandleDBNull(reader1["ItemCategoryID"]);
                                objFoodItem.ItemCalories = (string)DALHelper.HandleDBNull(reader1["ItemCal"]);
                                objFoodItem.ItemCh = (string)DALHelper.HandleDBNull(reader1["ItemCH"]);
                                objFoodItem.ItemFat = (string)DALHelper.HandleDBNull(reader1["ItemFat"]);
                                objFoodItem.ItemInst = (string)DALHelper.HandleDBNull(reader1["ItemInstruction"]);
                                objFoodItem.ItemQty = (string)DALHelper.HandleDBNull(reader1["ItemQty"]);
                                objFoodItem.ItemUnit = (string)DALHelper.HandleDBNull(reader1["ItemUnit"]);
                                objFoodItem.Timing = (string)DALHelper.HandleDBNull(reader1["Timing"]);
                                
                               // objFoodItem.Status = reader1["AmmenityStatus"].HandleDBNull() == null ? false : Convert.ToBoolean(reader1["AmmenityStatus"]);
                                objItemVO.ItemList.Add(objFoodItem);
                                //objItem.objRoomMasterAmmenities.AmmenityDetails.Add(objUnit);
                            }
                        }
                        reader1.Close();
                        objItem.objDietPlanMasterDetails.Add(objItemVO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }

        public override IValueObject UpdateDietPlanMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateDietNutritionStatusBizActionVO bizObject = valueObject as clsIPDUpdateDietNutritionStatusBizActionVO;
            try
            {
                clsIPDDietNutritionMasterVO objVO = bizObject.DietStatus;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateDietPlanMasterStatus");
                //  dbServer.AddInParameter(command, "ID", DbType.Int64, bizObject.ID); 
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVO.Status);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizObject.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizObject.DietStatus.ID = (long)dbServer.GetParameterValue(command, "ID");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //  logManager.LogInfo(bizObject.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return bizObject;
        }


        #region Floor Master

        public override IValueObject GetFloorMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsIPDGetFloorMasterBizActionVO objFloor = valueObject as clsIPDGetFloorMasterBizActionVO;
            clsIPDFloorMasterVO objFloorVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetFloorMasterList");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objFloor.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objFloor.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objFloor.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objFloor.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objFloorVO = new clsIPDFloorMasterVO();
                        objFloorVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objFloorVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objFloorVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objFloorVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objFloorVO.BlockID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlockID"]));
                        objFloorVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        objFloorVO.BlockName = Convert.ToString(DALHelper.HandleDBNull(reader["BlockName"]));
                        objFloor.objFloorMasterDetails.Add(objFloorVO);
                    }
                }
                reader.NextResult();
                objFloor.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objFloor;

        }

        public override IValueObject AddUpdateFloorMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDFloorMasterVO objFloorVO = new clsIPDFloorMasterVO();
            clsIPDAddUpdateFloorMasterBizActionVO objFloor = valueObject as clsIPDAddUpdateFloorMasterBizActionVO;
            try
            {
                DbCommand command;
                objFloorVO = objFloor.FlooMasterDetails;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateFloorMaster");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objFloorVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objFloorVO.UnitID);
                dbServer.AddInParameter(command, "BlockID", DbType.Int64, objFloorVO.BlockID);
                dbServer.AddInParameter(command, "Code", DbType.String, objFloorVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objFloorVO.Description);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objFloorVO.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objFloor.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objFloor;

        }

        public override IValueObject UpdateFloorMasterStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDFloorMasterVO objFloorVO = valueObject as clsIPDFloorMasterVO;
            clsIPDAddUpdateFloorMasterBizActionVO objFloor = valueObject as clsIPDAddUpdateFloorMasterBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_UpadateFloorMasterStatus");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objFloor.FlooMasterDetails.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objFloor.FlooMasterDetails.Status);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objFloor.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objFloor;
        }
        #endregion

        #region Admission Type Master

        public override IValueObject GetAdmissionTypeMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsIPDGetAdmissionTypeMasterBizActionVO objFloor = valueObject as clsIPDGetAdmissionTypeMasterBizActionVO;
            clsIPDAdmissionTypeVO objAdmVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeMasterList"); //CIMS_GetFloorMasterList
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objFloor.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objFloor.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objFloor.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objFloor.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objAdmVO = new clsIPDAdmissionTypeVO();
                        objAdmVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdmVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objAdmVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objAdmVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objFloorVO.BlockID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlockID"]));
                        objAdmVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        //objFloorVO.BlockName = Convert.ToString(DALHelper.HandleDBNull(reader["BlockName"]));
                        objFloor.objAdmissionTypeMasterDetails.Add(objAdmVO);
                        //objFloor.objFloorMasterDetails.Add(objAdmVO);
                    }
                }
                reader.NextResult();
                objFloor.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objFloor;

        }

        public override IValueObject AddUpdateAdmissionTypeMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDAdmissionTypeVO objFloorVO = new clsIPDAdmissionTypeVO();
            clsIPDAddUpdateAdmissionTypeMasterBizActionVO objFloor = valueObject as clsIPDAddUpdateAdmissionTypeMasterBizActionVO;
            try
            {
                DbCommand command;
                objFloorVO = objFloor.AdmissionTypeMasterDetails;
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateAdmissionTypeMaster"); //CIMS_AddUpdateFloorMaster
                dbServer.AddInParameter(command, "ID", DbType.Int64, objFloorVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objFloorVO.UnitID);
                //dbServer.AddInParameter(command, "BlockID", DbType.Int64, objFloorVO.BlockID);
                dbServer.AddInParameter(command, "Code", DbType.String, objFloorVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objFloorVO.Description);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objFloorVO.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objFloor.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objFloor;

        }

        public override IValueObject UpdateAdmissionTypeMasterStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDAdmissionTypeVO objFloorVO = valueObject as clsIPDAdmissionTypeVO;
            clsIPDAddUpdateAdmissionTypeMasterBizActionVO objFloor = valueObject as clsIPDAddUpdateAdmissionTypeMasterBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_UpadateAdmissionTypeMasterStatus");           //CIMS_UpadateFloorMasterStatus
                dbServer.AddInParameter(command, "ID", DbType.Int64, objFloor.AdmissionTypeMasterDetails.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objFloor.AdmissionTypeMasterDetails.Status);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objFloor.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objFloor;
        }
       
        #endregion

        #region Admission Type  Service Linked List

        public override IValueObject GetAdmisionTypeServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsIPDGetAdmissionTypeServiceListBizActionVO BizActionObj = (clsIPDGetAdmissionTypeServiceListBizActionVO)valueObject;
            DbCommand command;
            DbDataReader reader;
            try
            {


                command = dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeServiceList"); //CIMS_GetDoctorServiceList

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                if (BizActionObj.ServiceName != null && BizActionObj.ServiceName.Length != 0)
                    dbServer.AddInParameter(command, "ServiceName", DbType.String, BizActionObj.ServiceName);
                dbServer.AddInParameter(command, "SpecializationID", DbType.Int64, BizActionObj.SpecializationID);
                dbServer.AddInParameter(command, "SubSpecializationID", DbType.Int64, BizActionObj.SubSpecializationID);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceID);                
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

                        objServiceMasterVO.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationId"]);
                        objServiceMasterVO.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationId"]);
                        objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);

                        objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"]));
                        objServiceMasterVO.ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objServiceMasterVO.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        
                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }

                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                    reader.Close();
                }
                if (BizActionObj.UnitID > 0)
                {
                    DbCommand command1;
                    DbDataReader reader1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeServiceLinkedList"); //CIMS_GetDoctorServiceLinkedList
                    dbServer.AddInParameter(command1, "AdmissionTypeID", DbType.Int64, BizActionObj.AdmissionTypeID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                    dbServer.AddInParameter(command1, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command1, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command1, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                    dbServer.AddOutParameter(command1, "TotalRows", DbType.Int32, int.MaxValue);
                    reader1 = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader1.HasRows)
                    {
                        if (BizActionObj.SelectedServiceList == null)
                            BizActionObj.SelectedServiceList = new List<clsServiceMasterVO>();
                        while (reader1.Read())
                        {
                            clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                            objServiceMasterVO.ID = (long)reader1["ID"];
                            objServiceMasterVO.AdmissionTypeName = (string)DALHelper.HandleDBNull(reader1["AdmissionTypeName"]);
                            objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader1["ServiceName"]);
                            objServiceMasterVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["UnitID"]));
                            objServiceMasterVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader1["UnitName"]));

                            objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader1["Specialization"]);
                            objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader1["SubSpecialization"]);

                            //objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader1["Rate"]));
                            // objServiceMasterVO.MinRate = (decimal)DALHelper.HandleDBNull(reader["MinRate"]);
                            BizActionObj.SelectedServiceList.Add(objServiceMasterVO);
                        }

                        reader1.NextResult();
                        BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command1, "TotalRows");
                        reader1.Close();
                    }
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

        public override IValueObject AddAdmissionTypeServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsIPDAddAdmissionTypeServiceListBizActionVO BizActionObj = (clsIPDAddAdmissionTypeServiceListBizActionVO)valueObject;
            BizActionObj.DoctorServiceDetails = new clsIPDAdmissionTypeServiceLinkVO();
            try
            {

                //if (BizActionObj.Modify == true)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteAdmissionTypeServiceDetail"); //CIMS_DeleteDoctorServiceDetail
                //    dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, BizActionObj.DoctorServiceList[0].AdmissionTypeID);
                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.DoctorServiceList[0].UnitID);
                //    int intStatus2 = dbServer.ExecuteNonQuery(command);

                //}

                foreach (var item in BizActionObj.DoctorServiceList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAdmissionTypeServiceDetail");  //CIMS_AddDoctorServiceDetail
                    dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, item.AdmissionTypeID);

                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, item.UnitID);
                    //dbServer.AddInParameter(command, "Rate", DbType.Decimal, item.Rate);
                    
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);                    

                    //dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, item.AddedDateTime);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.DoctorServiceDetails.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus2 = dbServer.ExecuteNonQuery(command);
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

        public override IValueObject GetAdmissionTypeServiceLinkedList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO BizActionObj = (clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO)valueObject;
            BizActionObj.ServiceList = new List<clsServiceMasterVO>();
            DbCommand command;
            DbDataReader reader;
            try
            {


                command = dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeServiceLinkedList"); //CIMS_GetDoctorServiceLinkedList
                dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, BizActionObj.AdmissionTypeID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);



                if (reader.HasRows)
                {
                    if (BizActionObj.ServiceList == null)
                        BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                        objServiceMasterVO.ID = (long)reader["ID"];
                        objServiceMasterVO.AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionTypeID"]));
                        objServiceMasterVO.AdmissionTypeName = (string)DALHelper.HandleDBNull(reader["AdmissionTypeName"]);
                        objServiceMasterVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objServiceMasterVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objServiceMasterVO.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));

                        objServiceMasterVO.SpecializationString = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        objServiceMasterVO.SubSpecializationString = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                        //objServiceMasterVO.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        objServiceMasterVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.ServiceList.Add(objServiceMasterVO);
                    }

                    reader.NextResult();
                    BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
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

        public override IValueObject GetAdmissionTypeDetailListForAdmissionTypeMasterByAdmissionTypeID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO BizActionObj = (clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO)valueObject;
            try
            {
                clsIPDAdmissionTypeVO ObjDoctorVo = BizActionObj.DoctorDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeMasterDetailsListByAdmissionTypeID");    //CIMS_GetDoctorMasterDetailsListByDoctorID
                DbDataReader reader;
                dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, BizActionObj.AdmissionTypeID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizActionObj.DoctorDetails == null)
                            BizActionObj.DoctorDetails = new clsIPDAdmissionTypeVO();
                        BizActionObj.DoctorDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.DoctorDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        //BizActionObj.DoctorDetails.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        //BizActionObj.DoctorDetails.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        //BizActionObj.DoctorDetails.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        //BizActionObj.DoctorDetails.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        //BizActionObj.DoctorDetails.Education = (string)DALHelper.HandleDBNull(reader["Education"]);
                        //BizActionObj.DoctorDetails.Experience = (string)DALHelper.HandleDBNull(reader["Experience"]);
                        //BizActionObj.DoctorDetails.Specialization = (long)DALHelper.HandleDBNull(reader["SpecializationID"]);
                        //BizActionObj.DoctorDetails.SpecializationDis = (string)DALHelper.HandleDBNull(reader["Specialization"]);
                        //BizActionObj.DoctorDetails.SubSpecialization = (long)DALHelper.HandleDBNull(reader["SubSpecializationID"]);
                        //BizActionObj.DoctorDetails.SubSpecializationDis = (string)DALHelper.HandleDBNull(reader["SubSpecialization"]);
                        //BizActionObj.DoctorDetails.DoctorType = (long)DALHelper.HandleDBNull(reader["DoctorTypeID"]);
                        BizActionObj.DoctorDetails.Description = (string)DALHelper.HandleDBNull(reader["AdmissionType"]);
                        BizActionObj.DoctorDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //BizActionObj.DoctorDetails.GenderId = (long)DALHelper.HandleDBNull(reader["GenderId"]);
                        //BizActionObj.DoctorDetails.Photo = (byte[])(DALHelper.HandleDBNull(reader["Photo"]));
                        //BizActionObj.DoctorDetails.Signature = (byte[])(DALHelper.HandleDBNull(reader["Signature"]));
                        //BizActionObj.DoctorDetails.MaritalStatusId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        //BizActionObj.DoctorDetails.ProvidentFund = Convert.ToString(DALHelper.HandleDBNull(reader["PFNumber"]));
                        //BizActionObj.DoctorDetails.PermanentAccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PANNumber"]));
                        //BizActionObj.DoctorDetails.AccessCardNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccessCardNumber"]));
                        //BizActionObj.DoctorDetails.RegistrationNumber = Convert.ToString(DALHelper.HandleDBNull(reader["RegestrationNumber"]));
                        //BizActionObj.DoctorDetails.DateofJoining = (DateTime?)DALHelper.HandleDate(reader["DateOfJoining"]);
                        //BizActionObj.DoctorDetails.EmailId = (string)DALHelper.HandleDBNull(reader["EmailId"]);
                        //BizActionObj.DoctorDetails.EmployeeNumber = Convert.ToString(DALHelper.HandleDBNull(reader["EmployeeNumber"]));
                        //BizActionObj.DoctorDetails.DoctorBankInformation.BankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankId"]));
                        //BizActionObj.DoctorDetails.DoctorBankInformation.BranchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BankBrachId"]));
                        //BizActionObj.DoctorDetails.DoctorBankInformation.AccountNumber = Convert.ToString(DALHelper.HandleDBNull(reader["AccountNumber"]));
                        //BizActionObj.DoctorDetails.DoctorBankInformation.AccountTypeId = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AccountType"]));
                        //BizActionObj.DoctorDetails.DoctorBankInformation.BranchAddress = Convert.ToString(DALHelper.HandleDBNull(reader["Address"]));
                        //BizActionObj.DoctorDetails.DoctorBankInformation.MICRNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["MICRNumber"]));
                    }
                }

                //reader.NextResult();
                //if (reader.HasRows)
                //{
                //    if (BizActionObj.DoctorDetails != null)
                //    {
                //        BizActionObj.DoctorDetails.UnitDepartmentDetails = new List<clsUnitDepartmentsDetailsVO>();
                //        while (reader.Read())
                //        {
                //            clsUnitDepartmentsDetailsVO objDepartment = new clsUnitDepartmentsDetailsVO();
                //            objDepartment.DepartmentID = (long)DALHelper.HandleDBNull(reader["DeptMasterId"]);
                //            objDepartment.DepartmentName = Convert.ToString(DALHelper.HandleDBNull(reader["Department"]));
                //            objDepartment.UnitID = (long)DALHelper.HandleDBNull(reader["UnitMasterId"]);
                //            if (DALHelper.HandleDBNull(reader["DoctorDepartmentstatus"]) != null)
                //            {
                //                objDepartment.Status = (bool)DALHelper.HandleDBNull(reader["DoctorDepartmentstatus"]);
                //            }
                //            else
                //                objDepartment.Status = false;
                //            objDepartment.UnitName = (string)DALHelper.HandleDBNull(reader["Unit"]);
                //            BizActionObj.DoctorDetails.UnitDepartmentDetails.Add(objDepartment);
                //        }
                //    }
                //}
                //reader.NextResult();
                //if (reader.HasRows)
                //{
                //    if (BizActionObj.DoctorDetails != null)
                //    {
                //        BizActionObj.DoctorDetails.UnitClassificationDetailsList = new List<clsUnitClassificationsDetailsVO>();
                //        while (reader.Read())
                //        {
                //            clsUnitClassificationsDetailsVO objClassification = new clsUnitClassificationsDetailsVO();
                //            objClassification.ClassificationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClassificationID"]));
                //            objClassification.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                //            objClassification.IsAvailableStr = Convert.ToString(DALHelper.HandleDBNull(reader["IsAvailable"]));
                //            if (objClassification.IsAvailableStr == "true")
                //            {
                //                objClassification.IsAvailable = true;
                //            }
                //            else
                //                objClassification.IsAvailable = false;
                //            BizActionObj.DoctorDetails.UnitClassificationDetailsList.Add(objClassification);
                //        }
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


        public override IValueObject AddUpdateAdmissionTypeServiceList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsServiceMasterVO objFloorVO = valueObject as clsServiceMasterVO;
            clsIPDAddUpdateAdmissionTypeServiceListBizActionVO objFloor = valueObject as clsIPDAddUpdateAdmissionTypeServiceListBizActionVO;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_UpadateAdmissionTypeServiceStatus");            //CIMS_UpadateAdmissionTypeMasterStatus
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objFloor.DoctorServiceDetails.ServiceID);
                dbServer.AddInParameter(command, "AdmissionTypeID", DbType.Int64, objFloor.DoctorServiceDetails.AdmissionTypeID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objFloor.DoctorServiceDetails.Status);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objFloor.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }
            catch (Exception ex)
            {
                throw;
            }
            return objFloor;
        }

        #endregion

        public override ValueObjects.IValueObject GetBedCensusAndNonCensusList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader1 = null;
            clsIPDGetBedCensusAndNonCensusListBizActionVO objItem = valueObject as clsIPDGetBedCensusAndNonCensusListBizActionVO;
            clsIPDBedMasterVO ObjService = null;
            try
            {
                StringBuilder AmmenityList = new StringBuilder();
                DbCommand command;
                if (objItem.IsBedDetails == false)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetBedCensusAndNonCensusList");
                    dbServer.AddInParameter(command, "IsNonCensus", DbType.String, objItem.IsNonCensus);
                    dbServer.AddInParameter(command, "Occupied", DbType.Boolean, false);
                    dbServer.AddInParameter(command, "IsUnderMaintanence", DbType.Boolean, false);
                    dbServer.AddInParameter(command, "IsForReservation", DbType.Boolean, objItem.BedDetails.IsForReservation);
                    dbServer.AddInParameter(command, "ClassID", DbType.Int64, objItem.ClassID);
                    dbServer.AddInParameter(command, "WardID", DbType.Int64, objItem.WardID);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetBedDetailsByIDandBedUnitID");
                    dbServer.AddInParameter(command, "BedID", DbType.Int64, objItem.BedID);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ObjService = new clsIPDBedMasterVO();
                        ObjService.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjService.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        ObjService.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        ObjService.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ObjService.BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"]));
                        ObjService.WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardId"]));
                        ObjService.BedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"]));
                        ObjService.RoomID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RoomId"]));
                        ObjService.WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"]));
                        ObjService.BedCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["BedCategoryName"]));
                        ObjService.RoomName = Convert.ToString(DALHelper.HandleDBNull(reader["RoomName"]));
                        ObjService.IsAmmenity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBedAmme"]));
                        ObjService.IsNonCensus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNonCensus"]));
                        ObjService.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        //ObjService.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        if (objItem.IsBedDetails == true)
                        {
                           ObjService.PatientName =Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                           ObjService.MrNo =Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                           ObjService.AdmissionDate=Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                           ObjService.PatientIPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        }

                        DbCommand command1;
                        command1 = dbServer.GetStoredProcCommand("CIMS_GetBedMasterAmmenitiesDetails");
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, ObjService.ID);
                        reader1 = (DbDataReader)dbServer.ExecuteReader(command1);

                        if (reader1.HasRows)
                        {
                            ObjService.AmmenityDetails = new List<MasterListItem>();
                            AmmenityList = new StringBuilder () ;
                            while (reader1.Read())
                            {
                                MasterListItem objUnit = new MasterListItem();
                                objUnit.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ID"]));
                                //objUnit.ID = (long)DALHelper.HandleDBNull(reader1["BedAmmenityID"]);
                                objUnit.Description = Convert.ToString(DALHelper.HandleDBNull(reader1["Ammenity"]));
                                objUnit.Status = reader1["AmmenityStatus"].HandleDBNull() == null ? false : Convert.ToBoolean(reader1["AmmenityStatus"]);
                                ObjService.AmmenityDetails.Add(objUnit);

                                AmmenityList.Append(objUnit.Description);
                                AmmenityList.Append(",");
                            }
                            ObjService.Facilities = AmmenityList.ToString();
                        }
                        reader1.Close();                        
                        objItem.objBedMasterDetails.Add(ObjService);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = Convert.ToInt64(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return objItem;
        }


    }

}
