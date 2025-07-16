using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.OpeartionTheatre;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using System.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsOTDetailsDAL : clsBaseOTDetailsDAL
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
        private clsOTDetailsDAL()
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


        public override IValueObject GetPatientUnitIDForOtDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //DbDataReader reader = null;
            //clsGetPatientConfigFieldsBizActionVO objItem = valueObject as clsGetPatientConfigFieldsBizActionVO;
            //clsPatientFieldsConfigVO objItemVO = null;
            //try
            //{
            //    DbCommand command;
            //    command = dbServer.GetStoredProcCommand("CIMS_GetConfig_Patient_Fields");

            //    reader = (DbDataReader)dbServer.ExecuteReader(command);
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            objItemVO = new clsPatientFieldsConfigVO();
            //            objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            //            objItemVO.TableName = Convert.ToString(DALHelper.HandleDBNull(reader["TableName"]));
            //            objItemVO.FieldName = Convert.ToString(DALHelper.HandleDBNull(reader["FieldName"]));
            //            objItem.OtPateintConfigFieldsMatserDetails.Add(objItemVO);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //return objItem;

            return valueObject;
        }

        /// <summary>
        /// Gets Procedures For Service ID
        /// </summary>
        public override IValueObject GetProceduresForServiceID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetProceduresForServiceIdBizActionVO bizActionVo = valueObject as clsGetProceduresForServiceIdBizActionVO;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetProceduresForServiceID");
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, bizActionVo.ServiceID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsProcedureMasterVO procObj = new clsProcedureMasterVO();

                        procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                        procObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        procObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));

                        bizActionVo.procedureList.Add(procObj);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }


            return bizActionVo;
        }


        /// <summary>
        /// Gets Services For Procedure ID
        /// </summary>
        public override IValueObject GetServicesForProcedureID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetServicesForProcedureIDBizActionVO bizActionVo = valueObject as clsGetServicesForProcedureIDBizActionVO;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetServicesForProcedureID");
                dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, bizActionVo.ProcedureID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsServiceMasterVO serviceObj = new clsServiceMasterVO();
                        serviceObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        serviceObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        serviceObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        serviceObj.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
                        serviceObj.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
                        serviceObj.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        bizActionVo.serviceList.Add(serviceObj);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        clsOTDetailsItemDetailsVO itemObj = new clsOTDetailsItemDetailsVO();
                        itemObj.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        itemObj.ItemDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        itemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        itemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        bizActionVo.ItemList.Add(itemObj);
                    }

                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }


            return bizActionVo;
        }


        public override IValueObject GetDoctorForOTDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorForOTDetailsBizActionVO BizActionObj = (clsGetDoctorForOTDetailsBizActionVO)valueObject;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorForOTDetails");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
                dbServer.AddInParameter(command, "DocTypeID", DbType.Int64, BizActionObj.DocTypeID);


                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DocList == null)
                        BizActionObj.DocList = new List<clsOTDetailsDocDetailsVO>();

                    while (reader.Read())
                    {
                        clsOTDetailsDocDetailsVO objDoc = new clsOTDetailsDocDetailsVO();

                        objDoc.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
                        objDoc.docDesc = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
                        objDoc.DesignationID = (long)DALHelper.HandleDBNull(reader["ClassificationID"]);
                        objDoc.designationDesc = (string)DALHelper.HandleDBNull(reader["Designation"]);

                        BizActionObj.DocList.Add(objDoc);
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

        /// <summary>
        /// Get OT Sheet Details
        /// </summary>
        public override IValueObject GetOTSheetDetailsByOTID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetOTSheetDetailsBizActionVO BizActionObj = valueObject as clsGetOTSheetDetailsBizActionVO;
            DbDataReader reader = null;
            DbConnection con = null;
            //DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                //trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_GetOTSheetDetailsByOTID");
                command.Connection = con;

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.objOTDetails.objOtSheetDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.FromTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.ToTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.TotalHours = Convert.ToString(DALHelper.HandleDBNull(reader["TotalHours"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaTypeID"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.OTResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTResultID"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.OTStatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTStatusID"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaStartTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AnesthesiaStartTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaEndTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AnesthesiaEndTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.WheelInTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WheelInTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.WheelOutTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WheelOutTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.SurgeryStartTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SurgeryStartTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.SurgeryEndTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SurgeryEndTime"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        BizActionObj.objOTDetails.objOtSheetDetails.ManPower = Convert.ToInt64(DALHelper.HandleDBNull(reader["ManPower"]));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw;
            }

            return BizActionObj;
        }


        /// <summary>
        /// Add Update OT Details
        /// </summary>
        public override IValueObject AddupdatOtDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddupdatOtDetailsBizActionVO BizActionObj = valueObject as clsAddupdatOtDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                DbCommand command1;
                command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSheetDetails");
                command1.Connection = con;
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                dbServer.AddInParameter(command1, "FromTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.FromTime);
                dbServer.AddInParameter(command1, "ToTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.ToTime);
                dbServer.AddInParameter(command1, "Date", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.Date);
                dbServer.AddInParameter(command1, "OTID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTID);
                dbServer.AddInParameter(command1, "TotalHours", DbType.String, BizActionObj.objOTDetails.objOtSheetDetails.TotalHours);
                dbServer.AddInParameter(command1, "AnesthesiaTypeID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaTypeID);
                dbServer.AddInParameter(command1, "ProcedureTypeID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.ProcedureTypeID);
                dbServer.AddInParameter(command1, "OTResultID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTResultID);
                dbServer.AddInParameter(command1, "OTStatusID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTStatusID);
                dbServer.AddInParameter(command1, "ManPower", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.ManPower);
                dbServer.AddInParameter(command1, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.objOtSheetDetails.IsEmergency);
                dbServer.AddInParameter(command1, "Remark", DbType.String, BizActionObj.objOTDetails.objOtSheetDetails.Remark);
                dbServer.AddInParameter(command1, "SpecialRequirements", DbType.String, BizActionObj.objOTDetails.objOtSheetDetails.SpecialRequirement);
                dbServer.AddInParameter(command1, "AnesthesiaStartTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaStartTime);
                dbServer.AddInParameter(command1, "AnesthesiaEndTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaEndTime);
                dbServer.AddInParameter(command1, "WheelInTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.WheelInTime);
                dbServer.AddInParameter(command1, "WheelOutTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.WheelOutTime);
                dbServer.AddInParameter(command1, "SurgeryStartTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.SurgeryStartTime);
                dbServer.AddInParameter(command1, "SurgeryEndTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.SurgeryEndTime);
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objOtSheetDetails.ID);

                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                #region Commented Part

                //foreach (var item in BizActionObj.objOTDetails.ProcedureList)
                //{

                //    DbCommand command2;
                //    command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSurgeryDetails");
                //    command2.Connection = con;
                //    //dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                //    dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, item.ProcedureID);
                //    dbServer.AddInParameter(command2, "Rate", DbType.Double, item.Rate);
                //    dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Double, item.ConcessionPercent);
                //    dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                //    dbServer.AddInParameter(command2, "ServiceTaxPercent", DbType.Double, item.ServiceTaxPercent);
                //    dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);
                //    dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
                //    dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.TotalAmount);
                //    dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);

                //    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                //    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


                //    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);
                //    int intStatus3 = dbServer.ExecuteNonQuery(command2, trans);

                //}

                //foreach (var item in BizActionObj.objOTDetails.DocList)
                //{

                //    DbCommand command3;
                //    command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsDocDetails");
                //    command3.Connection = con;
                //    //dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                //    dbServer.AddInParameter(command3, "DesignationID", DbType.Int64, item.DesignationID);
                //    dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, item.DoctorID);
                //    dbServer.AddInParameter(command3, "DocFees", DbType.Double, item.DocFees);
                //    dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, item.ProcedureID);                 

                //    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                //    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


                //    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, int.MaxValue);
                //    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);

                //}

                //foreach (var item in BizActionObj.objOTDetails.StaffList)
                //{

                //    DbCommand command4;
                //    command4 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsStaffDetails");
                //    command4.Connection = con;
                //    //dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command4, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                //    dbServer.AddInParameter(command4, "StaffID", DbType.Int64, item.StaffID);
                //    dbServer.AddInParameter(command4, "ProcedureID", DbType.Int64, item.ProcedureID);

                //    dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                //    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


                //    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, int.MaxValue);
                //    int intStatus3 = dbServer.ExecuteNonQuery(command4, trans);

                //}



                //foreach (var item in BizActionObj.objOTDetails.InstrumentList)
                //{

                //    DbCommand command6;
                //    command6 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsInstrumentDetails");
                //    command6.Connection = con;
                //    //dbServer.AddInParameter(command6, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command6, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                //    dbServer.AddInParameter(command6, "InstrumentID", DbType.Int64, item.InstrumentID);

                //    dbServer.AddInParameter(command6, "Quantity", DbType.Double, item.Quantity);
                //    dbServer.AddInParameter(command6, "Rate", DbType.Double, item.Rate);
                //    dbServer.AddInParameter(command6, "Amount", DbType.Double, item.Amount);


                //    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                //    dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


                //    dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int64, int.MaxValue);
                //    int intStatus3 = dbServer.ExecuteNonQuery(command6, trans);

                //}

                //DbCommand command7;
                //command7 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsAnesthesiaNotesDetails");
                //command7.Connection = con;
                ////dbServer.AddInParameter(command7, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command7, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                //dbServer.AddInParameter(command7, "AnesthesiaNotes", DbType.String, BizActionObj.objOTDetails.objAnesthesiaNotes.AnesthesiaNotes);

                //dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                //dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objAnesthesiaNotes.ID);


                //dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int64, int.MaxValue);
                //int intStatus4 = dbServer.ExecuteNonQuery(command7, trans);

                //DbCommand command8;
                //command8 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsSurgeryNotesDetails");
                //command8.Connection = con;
                ////dbServer.AddInParameter(command8, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command8, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                //dbServer.AddInParameter(command8, "SurgeyNotes", DbType.String, BizActionObj.objOTDetails.objSurgeryNotes.SurgeyNotes);

                //dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
                //dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objSurgeryNotes.ID);


                //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int64, int.MaxValue);
                //int intStatus5 = dbServer.ExecuteNonQuery(command8, trans);


                //foreach (var item in BizActionObj.objOTDetails.ItemList)
                //{

                //    DbCommand command5;
                //    command5 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsItemDetails");
                //    command5.Connection = con;
                //    //dbServer.AddInParameter(command5, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command5, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                //    dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item.ItemID);
                //    dbServer.AddInParameter(command5, "BatchID", DbType.Int64, item.BatchID);
                //    dbServer.AddInParameter(command5, "BatchCode", DbType.String, item.BatchCode);
                //    dbServer.AddInParameter(command5, "Quantity", DbType.Double, item.Quantity);
                //    dbServer.AddInParameter(command5, "Rate", DbType.Double, item.Rate);
                //    dbServer.AddInParameter(command5, "Amount", DbType.Double, item.Amount);
                //    dbServer.AddInParameter(command5, "VatPer", DbType.Double, item.VatPer);
                //    dbServer.AddInParameter(command5, "VatAmount", DbType.Double, item.VatAmt);
                //    dbServer.AddInParameter(command5, "NetAmount", DbType.Double, item.NetAmount);
                //    dbServer.AddInParameter(command5, "IsPackeged", DbType.Boolean, item.IsPackeged);
                //    dbServer.AddInParameter(command5, "IsConsumed", DbType.Boolean, item.IsConsumed);

                //    dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                //    dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


                //    dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, int.MaxValue);
                //    int intStatus3 = dbServer.ExecuteNonQuery(command5, trans);

                //    item.StockDetails.BatchID = (long)item.BatchID;
                //    item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                //    item.StockDetails.ItemID = (long)item.ItemID;
                //    item.StockDetails.TransactionTypeID = InventoryTransactionType.OTDetails;
                //    item.StockDetails.TransactionID = BizActionObj.objOTDetails.ID;
                //    item.StockDetails.TransactionQuantity = (double)item.Quantity;



                //        item.StockDetails.Time = DateTime.Now;

                //    item.StockDetails.StoreID = item.StoreID;

                //    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                //    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                //    obj.Details = item.StockDetails;
                //    obj.Details.ID = 0;
                //    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, objUserVO, con, trans);

                //    if (obj.SuccessStatus == -1)
                //    {
                //        throw new Exception();
                //    }

                //    item.StockDetails.ID = obj.Details.ID;

                //}

                #endregion

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


        /// <summary>
        /// Add Update OT Surgery Details
        /// </summary>
        public override IValueObject AddUpdateOtSurgeryDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtSurgeryDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatOtSurgeryDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddOutParameter(command, "OTDetailsMainID", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                DbCommand command2;
                command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTStatusDetails");
                command2.Connection = con;
                dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                int status = dbServer.ExecuteNonQuery(command2, trans);

                foreach (var item in BizActionObj.objOTDetails.ProcedureList)
                {
                    DbCommand command1;
                    //con = dbServer.CreateConnection();
                    //con.Open();
                    //trans = con.BeginTransaction();
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSurgeryDetails");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64,objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                    dbServer.AddInParameter(command1, "ProcedureID", DbType.Int64, item.ProcedureID);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
                    dbServer.AddInParameter(command1, "IsEmergency", DbType.Boolean, item.IsEmergency);
                    dbServer.AddInParameter(command1, "IsHighRisk", DbType.Boolean, item.IsHighRisk);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command1, "ResultStatus"));

                }

                //trans.Commit();
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                con.Close();
                trans = null;
                throw;
            }
            finally
            {
                trans.Commit();
                //trans = null;
                con.Close();
            }
            return BizActionObj;
        }


        /// <summary>
        /// Add Update OT DocEmp Details
        /// </summary>
        public override IValueObject AddUpdateOtItemDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateOtItemDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateOtItemDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                DbCommand command3;
                command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTItemStatusDetails");
                command3.Connection = con;
                dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                int status = dbServer.ExecuteNonQuery(command3, trans);


                foreach (var item in BizActionObj.objOTDetails.ItemList1)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsItemDetails");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                    dbServer.AddInParameter(command1, "ItemCode", DbType.String, item.ItemCode);
                    dbServer.AddInParameter(command1, "ItemName", DbType.String, item.ItemName);
                    dbServer.AddInParameter(command1, "Quantity", DbType.String, item.Quantity);
                    dbServer.AddInParameter(command1, "ItemID", DbType.String, item.ItemID);                                      
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    //trans.Commit();
                }
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                con.Close();
                trans = null;
                throw;
            }
            finally
            {
                trans.Commit();
                trans = null;
                con.Close();
            }
            return BizActionObj;
        }


        /// <summary>
        /// Add Update OT DocEmp Details
        /// </summary>
        public override IValueObject AddUpdateOtDocEmpDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtDocEmpDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatOtDocEmpDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                DbCommand command3;
                command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDocEmpStatusDetails");
                command3.Connection = con;
                dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                int status = dbServer.ExecuteNonQuery(command3, trans);


                foreach (var item in BizActionObj.objOTDetails.DocList)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsDocDetails");
                    command1.Connection = con;

                    
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                    dbServer.AddInParameter(command1, "DesignationID", DbType.Int64, item.DesignationID);
                    dbServer.AddInParameter(command1, "DoctorID", DbType.Int64, item.DoctorID);
                    //if (item.DoctorCode == null)
                    //{
                    //    item.DoctorCode = item.Code;
                    //}
                    dbServer.AddInParameter(command1, "DoctorCode", DbType.String, item.DoctorCode);
                    dbServer.AddInParameter(command1, "DocFees", DbType.Double, item.DocFees);
                    dbServer.AddInParameter(command1, "ProcedureID", DbType.Int64, item.ProcedureID);
                    dbServer.AddInParameter(command1, "StartTime", DbType.String, item.StrStartTime);
                    dbServer.AddInParameter(command1, "EndTime", DbType.String, item.StrStartTime);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    //trans.Commit();
                }

                foreach (var item in BizActionObj.objOTDetails.StaffList)
                {

                    DbCommand command2;
                    //con = dbServer.CreateConnection();
                    //con.Open();
                    //trans = con.BeginTransaction();
                    command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsStaffDetails");
                    command2.Connection = con;
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                    dbServer.AddInParameter(command2, "StaffID", DbType.Int64, item.StaffID);
                    dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, item.ProcedureID);

                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command2, "ResultStatus"));
                    //trans.Commit();
                }
            }
            catch (Exception Ex)
            {
                trans.Rollback();
                con.Close();
                trans = null;
                throw;
            }
            finally
            {
                trans.Commit();
                trans = null;
                con.Close();
            }
            return BizActionObj;
        }


        public override IValueObject AddUpdateOtServicesDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtServicesDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatOtServicesDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {

                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                DbCommand command3;
                command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTServicesStatusDetails");
                command3.Connection = con;
                dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                int status = dbServer.ExecuteNonQuery(command3, trans);


                List<clsDoctorSuggestedServiceDetailVO> objOTServiceVO = BizActionObj.objOTDetails.OTServicesList;
                for (int i = 0; i < objOTServiceVO.Count; i++)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsServiceDetails");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, objOTServiceVO[i].ServiceID);
                    dbServer.AddInParameter(command1, "ServiceCode", DbType.String, objOTServiceVO[i].ServiceCode);
                    dbServer.AddInParameter(command1, "ServiceName", DbType.String, objOTServiceVO[i].ServiceName);
                    dbServer.AddInParameter(command1, "ServiceType", DbType.String, objOTServiceVO[i].ServiceType);
                    dbServer.AddInParameter(command1, "GroupName", DbType.String, objOTServiceVO[i].GroupName);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, objOTServiceVO[i].Quantity);
                    dbServer.AddInParameter(command1, "Priority", DbType.String, objOTServiceVO[i].SelectedPriority.ID);

                    //dbServer.AddInParameter(command1, "Priority", DbType.String, objOTServiceVO[i].Priority);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objOTServiceVO[i].ID);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command1, "ResultStatus"));
                    //trans.Commit();


                }

            }
            catch (Exception Ex)
            {
                throw;
            }
            finally
            {
                trans.Commit();
                if (con != null && con.State == ConnectionState.Open) con.Close();

            }

            return BizActionObj;
        }

        /// <summary>
        /// Add Update OT Notes Details
        /// </summary>
        public override IValueObject AddUpdateOtNotesDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtNotesDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatOtNotesDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                DbCommand command3;
                command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTNotesStatusDetails");
                command3.Connection = con;
                dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                int status = dbServer.ExecuteNonQuery(command3, trans);


                List<clsOTDetailsInstructionListDetailsVO> objSurgeryList = BizActionObj.SurgeryInstructionList;
                List<clsOTDetailsInstructionListDetailsVO> objAnesthesiaList = BizActionObj.AnesthesiaInstructionList;

                for (int i = 0; i < objAnesthesiaList.Count; i++)
                {

                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsAnesthesiaNotesDetails");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                    dbServer.AddInParameter(command1, "AnesthesiaNotes", DbType.String, objAnesthesiaList[i].Instruction);
                    dbServer.AddInParameter(command1, "GroupName", DbType.String, objAnesthesiaList[i].GroupName);
                    //dbServer.AddInParameter(command1, "AnesthesiaNotes", DbType.String, BizActionObj.objOTDetails.objAnesthesiaNotes.AnesthesiaNotes);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objAnesthesiaNotes.ID);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                }

                for (int i = 0; i < objSurgeryList.Count; i++)
                {
                    DbCommand command2;
                    command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsSurgeryNotesDetails");
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);                    
                    dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                    dbServer.AddInParameter(command2, "SurgeyNotes", DbType.String, objSurgeryList[i].Instruction);
                    dbServer.AddInParameter(command2, "GroupName", DbType.String, objSurgeryList[i].GroupName);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objSurgeryNotes.ID);

                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command2, "ResultStatus"));
                    //trans.Commit();
                }

            }
            catch (Exception Ex)
            {
                trans.Rollback();
                trans = null;
                throw;
            }
            finally
            {
                trans.Commit();
                trans = null;
                con.Close();
            }
            return BizActionObj;
        }


        public override IValueObject AddUpdatePostInstructionDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtPostInstructionDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatOtPostInstructionDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));


                DbCommand command1;
                //con = dbServer.CreateConnection();
                //con.Open();
                //trans = con.BeginTransaction();
                command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsPostInstruction");
                command1.Connection = con;

                dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                dbServer.AddInParameter(command1, "PostInsID", DbType.Double, BizActionObj.objOTDetails.PostInstructionList.PostInstructionID);
                dbServer.AddInParameter(command1, "PostInstructions", DbType.String, BizActionObj.objOTDetails.PostInstructionList.PostInstruction);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);

                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command1, "ResultStatus"));
                //trans.Commit();

            }
            catch (Exception Ex)
            {
                trans.Rollback();
                con.Close();
                trans = null;
                throw;
            }
            finally
            {
                trans.Commit();
                trans = null;
                con.Close();
            }
            return BizActionObj;
        }


        public override IValueObject AddUpdateDoctorNotesDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateOTDoctorNotesDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateOTDoctorNotesDetailsBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
                dbServer.AddInParameter(command, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.IsEmergency);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));


                DbCommand command1;
                command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsDoctorNotes");
                command1.Connection = con;
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command1, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId); 
                dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
                dbServer.AddInParameter(command1, "DoctorNotesID", DbType.Double, BizActionObj.objOTDetails.DoctorNotesList.DoctorNotesID);
                dbServer.AddInParameter(command1, "DoctorNotes", DbType.String, BizActionObj.objOTDetails.DoctorNotesList.DoctorNotes);
                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);

                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command1, "ResultStatus"));
                //trans.Commit();

            }
            catch (Exception Ex)
            {
                trans.Rollback();
                con.Close();
                trans = null;
                throw;
            }
            finally
            {
                trans.Commit();
                trans = null;
                con.Close();
            }
            return BizActionObj;

        }


        ///// <summary>
        ///// Add Update OT Details
        ///// </summary>
        //public override IValueObject AddupdatOtDetails(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsAddupdatOtDetailsBizActionVO BizActionObj = valueObject as clsAddupdatOtDetailsBizActionVO;
        //    DbConnection con = null;
        //    DbTransaction trans = null;
        //    try
        //    {             
        //        DbCommand command;
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
        //        command.Connection = con;

        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
        //        dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
        //        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.objOTDetails.DoctorID);
        //        dbServer.AddInParameter(command, "BedCategory", DbType.String, BizActionObj.objOTDetails.BedCategory);
        //        dbServer.AddInParameter(command, "Bed", DbType.String, BizActionObj.objOTDetails.Bed);
        //        dbServer.AddInParameter(command, "Ward", DbType.String, BizActionObj.objOTDetails.Ward);
        //        dbServer.AddInParameter(command, "AdmissionDate", DbType.DateTime, BizActionObj.objOTDetails.AdmissionDate);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "DetailsID", DbType.Int64, BizActionObj.objOTDetails.detailsID);


        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objUserVO.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);


        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


        //        int intStatus = dbServer.ExecuteNonQuery(command,trans);
        //        BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
        //        BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

        //        DbCommand command1;
        //        command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSheetDetails");
        //        command1.Connection = con;
        //        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //        dbServer.AddInParameter(command1, "FromTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.FromTime);
        //        dbServer.AddInParameter(command1, "ToTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.ToTime);
        //        dbServer.AddInParameter(command1, "Date", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.Date);
        //        dbServer.AddInParameter(command1, "OTID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTID);
        //        dbServer.AddInParameter(command1, "TotalHours", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.TotalHours);
        //        dbServer.AddInParameter(command1, "AnesthesiaTypeID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaTypeID);
        //        dbServer.AddInParameter(command1, "ProcedureTypeID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.ProcedureTypeID);
        //        dbServer.AddInParameter(command1, "OTResultID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTResultID);
        //        dbServer.AddInParameter(command1, "OTStatusID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTStatusID);
        //        dbServer.AddInParameter(command1, "ManPower", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.ManPower);
        //        dbServer.AddInParameter(command1, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.objOtSheetDetails.IsEmergency);
        //        dbServer.AddInParameter(command1, "Remark", DbType.String, BizActionObj.objOTDetails.objOtSheetDetails.Remark);
        //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
        //        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objOtSheetDetails.ID);


        //        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
        //        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

        //        foreach (var item in BizActionObj.objOTDetails.ProcedureList)
        //        {

        //            DbCommand command2;
        //            command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSurgeryDetails");
        //            command2.Connection = con;
        //            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, item.ProcedureID);
        //            dbServer.AddInParameter(command2, "Rate", DbType.Double, item.Rate);
        //            dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Double, item.ConcessionPercent);
        //            dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
        //            dbServer.AddInParameter(command2, "ServiceTaxPercent", DbType.Double, item.ServiceTaxPercent);
        //            dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);
        //            dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.TotalAmount);
        //            dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);

        //            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command2, trans);

        //        }

        //        foreach (var item in BizActionObj.objOTDetails.DocList)
        //        {

        //            DbCommand command3;
        //            command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsDocDetails");
        //            command3.Connection = con;
        //            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command3, "DesignationID", DbType.Int64, item.DesignationID);
        //            dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, item.DoctorID);
        //            dbServer.AddInParameter(command3, "DocFees", DbType.Double, item.DocFees);
        //            dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, item.ProcedureID);                 

        //            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);

        //        }

        //        foreach (var item in BizActionObj.objOTDetails.StaffList)
        //        {

        //            DbCommand command4;
        //            command4 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsStaffDetails");
        //            command4.Connection = con;
        //            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command4, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command4, "StaffID", DbType.Int64, item.StaffID);
        //            dbServer.AddInParameter(command4, "ProcedureID", DbType.Int64, item.ProcedureID);

        //            dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command4, trans);

        //        }



        //        foreach (var item in BizActionObj.objOTDetails.InstrumentList)
        //        {

        //            DbCommand command6;
        //            command6 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsInstrumentDetails");
        //            command6.Connection = con;
        //            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command6, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command6, "InstrumentID", DbType.Int64, item.InstrumentID);

        //            dbServer.AddInParameter(command6, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command6, "Rate", DbType.Double, item.Rate);
        //            dbServer.AddInParameter(command6, "Amount", DbType.Double, item.Amount);


        //            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command6, trans);

        //        }

        //        DbCommand command7;
        //        command7 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsAnesthesiaNotesDetails");
        //        command7.Connection = con;
        //        dbServer.AddInParameter(command7, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command7, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //        dbServer.AddInParameter(command7, "AnesthesiaNotes", DbType.String, BizActionObj.objOTDetails.objAnesthesiaNotes.AnesthesiaNotes);




        //        dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
        //        dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objAnesthesiaNotes.ID);


        //        dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int64, int.MaxValue);
        //        int intStatus4 = dbServer.ExecuteNonQuery(command7, trans);

        //        DbCommand command8;
        //        command8 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsSurgeryNotesDetails");
        //        command8.Connection = con;
        //        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command8, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //        dbServer.AddInParameter(command8, "SurgeyNotes", DbType.String, BizActionObj.objOTDetails.objSurgeryNotes.SurgeyNotes);




        //        dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
        //        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objSurgeryNotes.ID);


        //        dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int64, int.MaxValue);
        //        int intStatus5 = dbServer.ExecuteNonQuery(command8, trans);


        //        foreach (var item in BizActionObj.objOTDetails.ItemList)
        //        {

        //            DbCommand command5;
        //            command5 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsItemDetails");
        //            command5.Connection = con;
        //            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command5, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item.ItemID);
        //            dbServer.AddInParameter(command5, "BatchID", DbType.Int64, item.BatchID);
        //            dbServer.AddInParameter(command5, "BatchCode", DbType.String, item.BatchCode);
        //            dbServer.AddInParameter(command5, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command5, "Rate", DbType.Double, item.Rate);
        //            dbServer.AddInParameter(command5, "Amount", DbType.Double, item.Amount);
        //            dbServer.AddInParameter(command5, "VatPer", DbType.Double, item.VatPer);
        //            dbServer.AddInParameter(command5, "VatAmount", DbType.Double, item.VatAmt);
        //            dbServer.AddInParameter(command5, "NetAmount", DbType.Double, item.NetAmount);
        //            dbServer.AddInParameter(command5, "IsPackeged", DbType.Boolean, item.IsPackeged);
        //            dbServer.AddInParameter(command5, "IsConsumed", DbType.Boolean, item.IsConsumed);

        //            dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command5, trans);

        //            item.StockDetails.BatchID = (long)item.BatchID;
        //            item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
        //            item.StockDetails.ItemID = (long)item.ItemID;
        //            item.StockDetails.TransactionTypeID = InventoryTransactionType.OTDetails;
        //            item.StockDetails.TransactionID = BizActionObj.objOTDetails.ID;
        //            item.StockDetails.TransactionQuantity = (double)item.Quantity;



        //                item.StockDetails.Time = DateTime.Now;

        //            item.StockDetails.StoreID = item.StoreID;

        //            clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
        //            clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

        //            obj.Details = item.StockDetails;
        //            obj.Details.ID = 0;
        //            obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, objUserVO, con, trans);

        //            if (obj.SuccessStatus == -1)
        //            {
        //                throw new Exception();
        //            }

        //            item.StockDetails.ID = obj.Details.ID;

        //        }


        //        trans.Commit();

        //    }



        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        con.Close();
        //        trans = null;
        //        throw;

        //    }
        //    finally
        //    {
        //        con.Close();
        //        trans = null;
        //    }
        //    return BizActionObj;

        //}


        /// <summary>
        /// Gets OT Details
        /// </summary>

        public override IValueObject GetOTDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTDetailsListizActionVO bizActionVo = valueObject as clsGetOTDetailsListizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetOtDetailsList");

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizActionVo.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, bizActionVo.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, bizActionVo.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, bizActionVo.SortExpression);
                dbServer.AddInParameter(command, "OTID", DbType.Int64, bizActionVo.OTID);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, bizActionVo.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, bizActionVo.ToDate);
                if (bizActionVo.FirstName != null && bizActionVo.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, bizActionVo.FirstName + "%");
                if (bizActionVo.LastName != null && bizActionVo.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, bizActionVo.LastName+ "%");
                if (bizActionVo.MRNo != null && bizActionVo.MRNo.Length > 0)
                    dbServer.AddInParameter(command, "MRNo", DbType.String, bizActionVo.MRNo + "%");

                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.TotalRows);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsOTDetailsVO otDetailsObj = new clsOTDetailsVO();
                        otDetailsObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        otDetailsObj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        otDetailsObj.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        otDetailsObj.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
                        otDetailsObj.MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"]));
                        //otDetailsObj.Education = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                        otDetailsObj.Religion = Convert.ToString(DALHelper.HandleDBNull(reader["Religion"]));
                        otDetailsObj.ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"]));
                        otDetailsObj.Photo = (Byte[])DALHelper.HandleDBNull(reader["Photo"]);
                        //otDetailsObj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        //otDetailsObj.DocName = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"]));
                        otDetailsObj.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        otDetailsObj.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        otDetailsObj.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DateOfBirth"]));
                        otDetailsObj.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        otDetailsObj.FromTime = Convert.ToDateTime(DALHelper.HandleDate(reader["FromTime"]));
                        otDetailsObj.ToTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ToTime"]));
                        otDetailsObj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        //otDetailsObj.Remnark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        otDetailsObj.OTName = Convert.ToString(DALHelper.HandleDBNull(reader["OTName"]));
                        otDetailsObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
                        bizActionVo.objOTDetails.Add(otDetailsObj);
                    }
                }
                reader.NextResult();
                bizActionVo.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }

        ///// <summary>
        ///// Gets OT Details
        ///// </summary>

        //public override IValueObject GetOTDetailsList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetOTDetailsListizActionVO bizActionVo = valueObject as clsGetOTDetailsListizActionVO;
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetOtDetailsList");

        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizActionVo.IsPagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, bizActionVo.StartIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int64, bizActionVo.MaximumRows);
        //        dbServer.AddInParameter(command, "sortExpression", DbType.String, bizActionVo.SortExpression);
        //        dbServer.AddInParameter(command, "OTID", DbType.Int64, bizActionVo.OTID);

        //        dbServer.AddInParameter(command, "DocID", DbType.Int64, bizActionVo.DocID);
        //        dbServer.AddInParameter(command, "StaffID", DbType.Int64, bizActionVo.StaffID);

        //        dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                clsOTDetailsVO otDetailsObj = new clsOTDetailsVO();
        //                otDetailsObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                otDetailsObj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                otDetailsObj.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
        //                otDetailsObj.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
        //                otDetailsObj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
        //                otDetailsObj.DocName = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"]));

        //                otDetailsObj.FromTime = Convert.ToDateTime(DALHelper.HandleDate(reader["FromTime"]));
        //                otDetailsObj.ToTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ToTime"]));
        //                otDetailsObj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));


        //                otDetailsObj.Remnark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
        //                otDetailsObj.OTName= Convert.ToString(DALHelper.HandleDBNull(reader["OTName"]));
        //                otDetailsObj.BedCategory = Convert.ToString(DALHelper.HandleDBNull(reader["BedCategory"]));
        //                otDetailsObj.Bed = Convert.ToString(DALHelper.HandleDBNull(reader["Bed"]));
        //                otDetailsObj.AdmissionDate = Convert.ToDateTime(DALHelper.HandleDate(reader["AdmissionDate"]));
        //                otDetailsObj.Ward = Convert.ToString(DALHelper.HandleDBNull(reader["Ward"]));
        //                otDetailsObj.detailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailsID"]));


        //                bizActionVo.objOTDetails.Add(otDetailsObj);

        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}

        /// <summary>
        /// Gets detail tables of OT details by OT ID
        /// </summary>
        /// 

        public override IValueObject GetDetailTablesOfOTDetailsByOTDetailID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO bizActionVo = valueObject as clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.OTDetailsID);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.OTDetailsID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));


                if (bizActionVo.OTDetailsID > 0)
                {

                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsForOTDetailsID");
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
                    //dbServer.AddInParameter(command, "OTDetailsID", DbType.Int64, 1);                   
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bizActionVo.OTSheetDetailsObj = new clsOTDetailsOTSheetDetailsVO();
                            bizActionVo.OTSheetDetailsObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            bizActionVo.OTSheetDetailsObj.FromTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"]));
                            bizActionVo.OTSheetDetailsObj.ToTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"]));
                            bizActionVo.OTSheetDetailsObj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                            bizActionVo.OTSheetDetailsObj.OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"]));
                            bizActionVo.OTSheetDetailsObj.TotalHours = Convert.ToString(DALHelper.HandleDBNull(reader["TotalHours"]));
                            bizActionVo.OTSheetDetailsObj.AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaTypeID"]));
                            bizActionVo.OTSheetDetailsObj.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
                            bizActionVo.OTSheetDetailsObj.OTResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTResultID"]));
                            bizActionVo.OTSheetDetailsObj.OTStatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTStatusID"]));
                            bizActionVo.OTSheetDetailsObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
                            bizActionVo.OTSheetDetailsObj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                            bizActionVo.OTSheetDetailsObj.SpecialRequirement = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRequirements"]));
                            bizActionVo.OTSheetDetailsObj.ManPower = Convert.ToInt64(DALHelper.HandleDBNull(reader["ManPower"]));

                            object AStartTime = reader["AnesthesiaStartTime"];
                            //bizActionVo.OTSheetDetailsObj.AnesthesiaStartTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AnesthesiaStartTime"]));
                            bizActionVo.OTSheetDetailsObj.AnesthesiaStartTime = (AStartTime == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(AStartTime);

                            object AEndTime = reader["AnesthesiaEndTime"];
                            bizActionVo.OTSheetDetailsObj.AnesthesiaEndTime = (AEndTime == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(AEndTime);
                            //bizActionVo.OTSheetDetailsObj.AnesthesiaEndTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AnesthesiaEndTime"]));

                            object WInTime = reader["WheelInTime"];
                            bizActionVo.OTSheetDetailsObj.WheelInTime = (WInTime == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(WInTime);
                            //bizActionVo.OTSheetDetailsObj.WheelInTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WheelInTime"]));

                            object WOutTime = reader["WheelOutTime"];
                            bizActionVo.OTSheetDetailsObj.WheelOutTime = (WOutTime == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(WOutTime);
                            //bizActionVo.OTSheetDetailsObj.WheelOutTime = Convert.ToDateTime(DALHelper.HandleDate(reader["WheelOutTime"]));

                            object SStartTime = reader["SurgeryStartTime"];
                            bizActionVo.OTSheetDetailsObj.SurgeryStartTime = (SStartTime == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(SStartTime);
                            //bizActionVo.OTSheetDetailsObj.SurgeryStartTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SurgeryStartTime"]));

                            object SEndTime = reader["SurgeryEndTime"];
                            bizActionVo.OTSheetDetailsObj.SurgeryEndTime = (SEndTime == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(SEndTime);
                            //bizActionVo.OTSheetDetailsObj.SurgeryEndTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SurgeryEndTime"]));

                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }

        public override IValueObject GetSurgeryDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSurgeryDetailsByOTDetailsIDBizActionVO bizActionVo = valueObject as clsGetSurgeryDetailsByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.OTDetailsID);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.OTDetailsID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));


                if (bizActionVo.OTDetailsID > 0)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetSurgeryDetailsByOTDetailsID");
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    bizActionVo.ProcedureList = new List<clsPatientProcedureVO>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsPatientProcedureVO procObj = new clsPatientProcedureVO();
                            procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            procObj.Quantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["Quantity"]));
                            procObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                            procObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            procObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
                            procObj.IsHighRisk = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsHighRisk"]));
                            bizActionVo.ProcedureList.Add(procObj);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }

        public override IValueObject GetItemDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemDetailsByOTDetailsIDBizActionVO bizActionVo = valueObject as clsGetItemDetailsByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.OTDetailsID);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.OTDetailsID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));


                if (bizActionVo.OTDetailsID > 0)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetItemDetailsByOTDetailsID");
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsProcedureItemDetailsVO ItemObj = new clsProcedureItemDetailsVO();
                            ItemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                            ItemObj.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                            ItemObj.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                            ItemObj.Quantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["Quantity"]));
                            bizActionVo.ItemList1.Add(ItemObj);
                        }
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }

        public override IValueObject GetDocEmpDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDocEmpDetailsByOTDetailsIDBizActionVO bizActionVo = valueObject as clsGetDocEmpDetailsByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.OTDetailsID);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.OTDetailsID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));


                if (bizActionVo.OTDetailsID > 0)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetDocEmpDetailsByOTDetailsID");
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MasterListItem defaultObj = new MasterListItem();
                            defaultObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            defaultObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                            bizActionVo.ProcedureList.Add(defaultObj);

                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsOTDetailsDocDetailsVO docObj = new clsOTDetailsDocDetailsVO();
                            docObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            docObj.DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DesignationID"]));
                            docObj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                            docObj.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"]));
                            docObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                            docObj.docDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"]));
                            docObj.StrStartTime = Convert.ToString(DALHelper.HandleDBNull(reader["StartTime"]));
                            docObj.StrEndTime = Convert.ToString(DALHelper.HandleDBNull(reader["EndTime"]));
                            docObj.designationDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Designation"]));
                            docObj.ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]));
                            MasterListItem defaultObj = new MasterListItem();
                            defaultObj.ID = Convert.ToInt64(docObj.ProcedureID);
                            defaultObj.Description = docObj.ProcedureName;
                            //defaultObj.ID = 0;
                            //defaultObj.Description = "--Select--";
                            docObj.SelectedProcedure.ID = defaultObj.ID;
                            docObj.SelectedProcedure.Description = defaultObj.Description;
                            docObj.ProcedureList.Add(defaultObj);
                            bizActionVo.DoctorList.Add(docObj);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsOTDetailsStaffDetailsVO StaffObj = new clsOTDetailsStaffDetailsVO();
                            StaffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            StaffObj.StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["staffID"]));
                            StaffObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                            StaffObj.StaffDesc = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]));
                            StaffObj.ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]));
                            MasterListItem defaultObj = new MasterListItem();
                            //defaultObj.ID = 0;
                            //defaultObj.Description = "--Select--";
                            defaultObj.ID = Convert.ToInt64(StaffObj.ProcedureID);
                            defaultObj.Description = StaffObj.ProcedureName;
                            StaffObj.SelectedProcedure.ID = defaultObj.ID;
                            StaffObj.SelectedProcedure.Description = defaultObj.Description;
                            StaffObj.ProcedureList.Add(defaultObj);
                            bizActionVo.StaffList.Add(StaffObj);

                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return bizActionVo;
        }

        public override IValueObject GetOTNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTNotesByOTDetailsIDBizActionVO bizActionVo = valueObject as clsGetOTNotesByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.OTDetailsID);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.OTDetailsID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                if (bizActionVo.OTDetailsID > 0)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetOTNotesDetailsByOTDetailsID");
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        bizActionVo.AnesthesiaInstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
                        bizActionVo.SurgeryInstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
                        while (reader.Read())
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));
                            obj.GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["groupName"]));
                            bizActionVo.AnesthesiaInstructionList.Add(obj);
                            bizActionVo.AnesthesiaNotesObj.AnesthesiaNotes = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsOTDetailsInstructionListDetailsVO obj = new clsOTDetailsInstructionListDetailsVO();
                            obj.Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["SurgeyNotes"]));
                            obj.GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["groupName"]));
                            bizActionVo.SurgeryInstructionList.Add(obj);
                            bizActionVo.SurgeryNotesObj.SurgeyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["SurgeyNotes"]));
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;

        }

        public override IValueObject GetDoctorNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorNotesByOTDetailsIDBizActionVO bizActionVo = valueObject as clsGetDoctorNotesByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.OTDetailsID);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.OTDetailsID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                if (bizActionVo.OTDetailsID > 0)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetDoctorNotesDetailsByOTDetailsID");
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bizActionVo.DoctorNotes.DoctorNotes = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorNotes"]));
                            bizActionVo.DoctorNotes.DoctorNotesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorNotesID"]));
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;

        }


        public override IValueObject GetServicesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServicesByOTDetailsIDBizActionVO bizActionVo = valueObject as clsGetServicesByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizActionVo.PatientID);
                dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, bizActionVo.ScheduleId);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizActionVo.OTDetailsID);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                bizActionVo.OTDetailsID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));


                if (bizActionVo.OTDetailsID > 0)
                {
                    DbCommand command1;
                    command1 = dbServer.GetStoredProcCommand("CIMS_GetServicesByOTDetailsID");
                    dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
                    reader = (DbDataReader)dbServer.ExecuteReader(command1);

                    if (reader.HasRows)
                    {
                        bizActionVo.ServiceDetails = new List<clsDoctorSuggestedServiceDetailVO>();
                        while (reader.Read())
                        {
                            clsDoctorSuggestedServiceDetailVO objServices = new clsDoctorSuggestedServiceDetailVO();
                            objServices.SelectedPriority = new MasterListItem();
                            objServices.ID = Convert.ToInt64(reader["ID"]);
                            objServices.ServiceType = Convert.ToString(reader["ServiceType"]);
                            objServices.ServiceName = Convert.ToString(reader["ServiceName"]);
                            objServices.ServiceCode = Convert.ToString(reader["ServiceCode"]);
                            objServices.GroupName = Convert.ToString(reader["GroupName"]);
                            objServices.Quantity = Convert.ToDouble(reader["Quantity"]);
                            objServices.SelectedPriority.ID = Convert.ToInt64(reader["Priority"]);

                            bizActionVo.ServiceDetails.Add(objServices);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }

        //public override IValueObject GetDetailTablesOfOTDetailsByOTDetailID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO bizActionVo = valueObject as clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO;
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsForOTDetailsID");

        //        dbServer.AddInParameter(command, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);




        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {

        //                bizActionVo.OTSheetDetailsObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                bizActionVo.OTSheetDetailsObj.FromTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"]));
        //                bizActionVo.OTSheetDetailsObj.ToTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"]));
        //                bizActionVo.OTSheetDetailsObj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
        //                bizActionVo.OTSheetDetailsObj.OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"]));
        //                bizActionVo.OTSheetDetailsObj.TotalHours = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalHours"]));
        //                bizActionVo.OTSheetDetailsObj.AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaTypeID"]));
        //                bizActionVo.OTSheetDetailsObj.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
        //                bizActionVo.OTSheetDetailsObj.OTResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTResultID"]));
        //                bizActionVo.OTSheetDetailsObj.OTStatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTStatusID"]));
        //                bizActionVo.OTSheetDetailsObj.ManPower = Convert.ToInt64(DALHelper.HandleDBNull(reader["ManPower"]));
        //                bizActionVo.OTSheetDetailsObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
        //                bizActionVo.OTSheetDetailsObj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
        //                bizActionVo.OTSheetDetailsObj.OTName = Convert.ToString(DALHelper.HandleDBNull(reader["OTName"]));
        //                bizActionVo.OTSheetDetailsObj.AnesthesiaType = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaType"]));
        //                bizActionVo.OTSheetDetailsObj.ProcedureType = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureType"]));
        //                bizActionVo.OTSheetDetailsObj.OperationResult = Convert.ToString(DALHelper.HandleDBNull(reader["OperationResult"]));
        //                bizActionVo.OTSheetDetailsObj.OperationStatus = Convert.ToString(DALHelper.HandleDBNull(reader["OperationStatus"]));

        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOtDetailsProcedureDetailsVO procObj = new clsOtDetailsProcedureDetailsVO();
        //                procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                procObj.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
        //                procObj.ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                procObj.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                procObj.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
        //                procObj.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
        //                procObj.Quantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["Quantity"]));
        //                procObj.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
        //                procObj.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
        //                procObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                procObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.ProcedureList.Add(procObj);


        //            }
        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsDocDetailsVO docObj = new clsOTDetailsDocDetailsVO();
        //                docObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                docObj.DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DesignationID"]));
        //                docObj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
        //                docObj.DocFees = Convert.ToDouble(DALHelper.HandleDBNull(reader["DocFees"]));
        //                docObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                docObj.docDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"]));
        //                docObj.designationDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Designation"]));
        //                MasterListItem defaultObj = new MasterListItem();
        //                defaultObj.ID = 0;
        //                defaultObj.Description = "--Select--";
        //                docObj.ProcedureList.Add(defaultObj);
        //                bizActionVo.DoctorList.Add(docObj);


        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsStaffDetailsVO StaffObj = new clsOTDetailsStaffDetailsVO();
        //                StaffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

        //                StaffObj.StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffID"]));

        //                StaffObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));

        //                StaffObj.StaffDesc = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]));
        //                MasterListItem defaultObj = new MasterListItem();
        //                defaultObj.ID = 0;
        //                defaultObj.Description = "--Select--";
        //                StaffObj.ProcedureList.Add(defaultObj);
        //                bizActionVo.StaffList.Add(StaffObj);


        //            }

        //            reader.NextResult();


        //            while (reader.Read())
        //            {
        //                foreach (var item in bizActionVo.DoctorList)
        //                {
        //                    MasterListItem procObj = new MasterListItem();
        //                    procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                    procObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                    item.ProcedureList.Add(procObj);

        //                    item.SelectedProcedure.ID = procObj.ID;
        //                    item.SelectedProcedure = item.ProcedureList.FirstOrDefault(q => q.ID == item.SelectedProcedure.ID);

        //                }

        //                foreach (var item in bizActionVo.StaffList)
        //                {
        //                    MasterListItem procObj = new MasterListItem();
        //                    procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                    procObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                    item.ProcedureList.Add(procObj);

        //                    item.SelectedProcedure.ID = procObj.ID;
        //                    item.SelectedProcedure = item.ProcedureList.FirstOrDefault(q => q.ID == item.SelectedProcedure.ID);

        //                }

        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsItemDetailsVO ItemObj = new clsOTDetailsItemDetailsVO();
        //                ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ItemObj.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
        //                ItemObj.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
        //                ItemObj.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
        //                ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                ItemObj.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
        //                ItemObj.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
        //                ItemObj.VatPer = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPer"]));
        //                ItemObj.VatAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
        //                ItemObj.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
        //                ItemObj.IsConsumed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsumed"]));
        //                ItemObj.IsPackeged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackeged"]));
        //                ItemObj.ItemDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                ItemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));


        //                bizActionVo.ItemList.Add(ItemObj);


        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsInstrumentDetailsVO ItemObj = new clsOTDetailsInstrumentDetailsVO();
        //                ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ItemObj.InstrumentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstrumentID"]));
        //                ItemObj.InstrumentDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                ItemObj.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
        //                ItemObj.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));



        //                bizActionVo.InstrumentList.Add(ItemObj);


        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {


        //                bizActionVo.SurgeryNotesObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                bizActionVo.SurgeryNotesObj.SurgeyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["SurgeyNotes"]));


        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {


        //                bizActionVo.AnesthesiaNotesObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                bizActionVo.AnesthesiaNotesObj.AnesthesiaNotes = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));


        //            }

        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}



        public override IValueObject GetConsetDetailsForConsentID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsetDetailsForConsentIDBizActionVO bizActionVo = valueObject as clsGetConsetDetailsForConsentIDBizActionVO;

            DbDataReader reader = null;
            try
            {

                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetConsentDetailsForConsetID");

                dbServer.AddInParameter(command, "ConsentID", DbType.Int64, bizActionVo.ConsentID);




                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        bizActionVo.consentmaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        bizActionVo.consentmaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        bizActionVo.consentmaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        bizActionVo.consentmaster.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]));

                    }

                    reader.NextResult();
                }


                reader.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
            return bizActionVo;
        }


        public override IValueObject AddPatientWiseConsentPrinting(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientWiseConsentPrintingBizActionVO BizActionObj = valueObject as clsAddPatientWiseConsentPrintingBizActionVO;
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {


                DbCommand command;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                command = dbServer.GetStoredProcCommand("CIMS_AddPatientWiseConsentPrinting");
                command.Connection = con;

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ConsetPrintingObj.PatientID);
                dbServer.AddInParameter(command, "ProcDate", DbType.DateTime, BizActionObj.ConsetPrintingObj.ProcDate);
                dbServer.AddInParameter(command, "ConsentID", DbType.Int64, BizActionObj.ConsetPrintingObj.ConsentID);

                dbServer.AddInParameter(command, "TemplateName", DbType.String, BizActionObj.ConsetPrintingObj.TemplateName);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);



                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ConsetPrintingObj.ID);





                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                trans = null;
                con.Close();
            }
            return BizActionObj;

        }



        public override IValueObject UpdateProcedureScheduleStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateOTScheduleStatusBizActionVO BizAction = valueObject as clsUpdateOTScheduleStatusBizActionVO;
            try
            {
                clsPatientProcedureScheduleVO objProcedure = BizAction.UpdateStatusField;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateOTScheduleStatus");
                dbServer.AddInParameter(command, "IsPAC", DbType.Boolean, BizAction.IsCalledForPAC);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objProcedure.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objProcedure.Status);
                int intstatus = dbServer.ExecuteNonQuery(command);
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





        //#region Variables Declaration
        ////Declare the database object
        //private Database dbServer = null;
        ////Declare the LogManager object
        //private LogManager logManager = null;
        //#endregion

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //private clsOTDetailsDAL()
        //{
        //    try
        //    {
        //        #region Create Instance of database,LogManager object and BaseSql object
        //        //Create Instance of the database object and BaseSql object
        //        if (dbServer == null)
        //        {
        //            dbServer = HMSConfigurationManager.GetDatabaseReference();
        //        }

        //        //Create Instance of the LogManager object 
        //        if (logManager == null)
        //        {
        //            logManager = LogManager.GetInstance();
        //        }
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}


        //public override IValueObject GetPatientUnitIDForOtDetails(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    //DbDataReader reader = null;
        //    //clsGetPatientConfigFieldsBizActionVO objItem = valueObject as clsGetPatientConfigFieldsBizActionVO;
        //    //clsPatientFieldsConfigVO objItemVO = null;
        //    //try
        //    //{
        //    //    DbCommand command;
        //    //    command = dbServer.GetStoredProcCommand("CIMS_GetConfig_Patient_Fields");

        //    //    reader = (DbDataReader)dbServer.ExecuteReader(command);
        //    //    if (reader.HasRows)
        //    //    {
        //    //        while (reader.Read())
        //    //        {
        //    //            objItemVO = new clsPatientFieldsConfigVO();
        //    //            objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //    //            objItemVO.TableName = Convert.ToString(DALHelper.HandleDBNull(reader["TableName"]));
        //    //            objItemVO.FieldName = Convert.ToString(DALHelper.HandleDBNull(reader["FieldName"]));
        //    //            objItem.OtPateintConfigFieldsMatserDetails.Add(objItemVO);
        //    //        }
        //    //    }

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw;
        //    //}
        //    //return objItem;

        //    return valueObject;
        //}

        ///// <summary>
        ///// Gets Procedures For Service ID
        ///// </summary>
        //public override IValueObject GetProceduresForServiceID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetProceduresForServiceIdBizActionVO bizActionVo = valueObject as clsGetProceduresForServiceIdBizActionVO;

        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetProceduresForServiceID");
        //        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, bizActionVo.ServiceID);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                clsProcedureMasterVO procObj = new clsProcedureMasterVO();

        //                procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                procObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                procObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));

        //                bizActionVo.procedureList.Add(procObj);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }


        //    return bizActionVo;
        //}


        ///// <summary>
        ///// Gets Services For Procedure ID
        ///// </summary>
        //public override IValueObject GetServicesForProcedureID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    DbDataReader reader = null;
        //    clsGetServicesForProcedureIDBizActionVO bizActionVo = valueObject as clsGetServicesForProcedureIDBizActionVO;

        //    try
        //    {
        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetServicesForProcedureID");
        //        dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, bizActionVo.ProcedureID);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                clsServiceMasterVO serviceObj = new clsServiceMasterVO();
        //                serviceObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
        //                serviceObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                serviceObj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                serviceObj.ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                serviceObj.ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
        //                serviceObj.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
        //                bizActionVo.serviceList.Add(serviceObj);
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsItemDetailsVO itemObj = new clsOTDetailsItemDetailsVO();
        //                itemObj.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                itemObj.ItemDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                itemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                itemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
        //                bizActionVo.ItemList.Add(itemObj);
        //            }

        //        }
        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }


        //    return bizActionVo;
        //}


        //public override IValueObject GetDoctorForOTDetails(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsGetDoctorForOTDetailsBizActionVO BizActionObj = (clsGetDoctorForOTDetailsBizActionVO)valueObject;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorForOTDetails");
        //        dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");


        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
        //        dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentID);
        //        dbServer.AddInParameter(command, "DocTypeID", DbType.Int64, BizActionObj.DocTypeID);


        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
        //        dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
        //        dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);

        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {
        //            if (BizActionObj.DocList == null)
        //                BizActionObj.DocList = new List<clsOTDetailsDocDetailsVO>();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsDocDetailsVO objDoc = new clsOTDetailsDocDetailsVO();

        //                objDoc.DoctorID = (long)DALHelper.HandleDBNull(reader["DoctorID"]);
        //                objDoc.docDesc = (string)DALHelper.HandleDBNull(reader["DoctorName"]);
        //                objDoc.DesignationID = (long)DALHelper.HandleDBNull(reader["ClassificationID"]);
        //                objDoc.designationDesc = (string)DALHelper.HandleDBNull(reader["Designation"]);

        //                BizActionObj.DocList.Add(objDoc);
        //            }

        //        }
        //        reader.NextResult();
        //        BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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

        ///// <summary>
        ///// Add Update OT Details
        ///// </summary>
        //public override IValueObject AddupdatOtDetails(IValueObject valueObject, clsUserVO objUserVO)
        //{
        //    clsAddupdatOtDetailsBizActionVO BizActionObj = valueObject as clsAddupdatOtDetailsBizActionVO;
        //    DbConnection con = null;
        //    DbTransaction trans = null;
        //    try
        //    {

             
        //        DbCommand command;
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();
              
        //        command = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
        //        command.Connection = con;
             
        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.objOTDetails.PatientID);
        //        dbServer.AddInParameter(command, "ScheduleID", DbType.Int64, BizActionObj.objOTDetails.ScheduleID);
        //        dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizActionObj.objOTDetails.DoctorID);
        //        dbServer.AddInParameter(command, "BedCategory", DbType.String, BizActionObj.objOTDetails.BedCategory);
        //        dbServer.AddInParameter(command, "Bed", DbType.String, BizActionObj.objOTDetails.Bed);
        //        dbServer.AddInParameter(command, "Ward", DbType.String, BizActionObj.objOTDetails.Ward);
        //        dbServer.AddInParameter(command, "AdmissionDate", DbType.DateTime, BizActionObj.objOTDetails.AdmissionDate);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "DetailsID", DbType.Int64, BizActionObj.objOTDetails.detailsID);


        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objUserVO.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.ID);


        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);


        //        int intStatus = dbServer.ExecuteNonQuery(command,trans);
        //        BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
        //        BizActionObj.objOTDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

        //        DbCommand command1;
        //        command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSheetDetails");
        //        command1.Connection = con;
        //        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command1, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //        dbServer.AddInParameter(command1, "FromTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.FromTime);
        //        dbServer.AddInParameter(command1, "ToTime", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.ToTime);
        //        dbServer.AddInParameter(command1, "Date", DbType.DateTime, BizActionObj.objOTDetails.objOtSheetDetails.Date);
        //        dbServer.AddInParameter(command1, "OTID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTID);
        //        dbServer.AddInParameter(command1, "TotalHours", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.TotalHours);
        //        dbServer.AddInParameter(command1, "AnesthesiaTypeID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.AnesthesiaTypeID);
        //        dbServer.AddInParameter(command1, "ProcedureTypeID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.ProcedureTypeID);
        //        dbServer.AddInParameter(command1, "OTResultID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTResultID);
        //        dbServer.AddInParameter(command1, "OTStatusID", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.OTStatusID);
        //        dbServer.AddInParameter(command1, "ManPower", DbType.Int64, BizActionObj.objOTDetails.objOtSheetDetails.ManPower);
        //        dbServer.AddInParameter(command1, "IsEmergency", DbType.Boolean, BizActionObj.objOTDetails.objOtSheetDetails.IsEmergency);
        //        dbServer.AddInParameter(command1, "Remark", DbType.String, BizActionObj.objOTDetails.objOtSheetDetails.Remark);
        //        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
        //        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objOtSheetDetails.ID);


        //        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int64, int.MaxValue);
        //        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

        //        foreach (var item in BizActionObj.objOTDetails.ProcedureList)
        //        {

        //            DbCommand command2;
        //            command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSurgeryDetails");
        //            command2.Connection = con;
        //            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, item.ProcedureID);
        //            dbServer.AddInParameter(command2, "Rate", DbType.Double, item.Rate);
        //            dbServer.AddInParameter(command2, "ConcessionPercent", DbType.Double, item.ConcessionPercent);
        //            dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
        //            dbServer.AddInParameter(command2, "ServiceTaxPercent", DbType.Double, item.ServiceTaxPercent);
        //            dbServer.AddInParameter(command2, "ServiceTaxAmount", DbType.Double, item.ServiceTaxAmount);
        //            dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.TotalAmount);
        //            dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);
                 
        //            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command2, trans);

        //        }

        //        foreach (var item in BizActionObj.objOTDetails.DocList)
        //        {

        //            DbCommand command3;
        //            command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsDocDetails");
        //            command3.Connection = con;
        //            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command3, "DesignationID", DbType.Int64, item.DesignationID);
        //            dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, item.DoctorID);
        //            dbServer.AddInParameter(command3, "DocFees", DbType.Double, item.DocFees);
        //            dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, item.ProcedureID);                 

        //            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);

        //        }

        //        foreach (var item in BizActionObj.objOTDetails.StaffList)
        //        {

        //            DbCommand command4;
        //            command4 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsStaffDetails");
        //            command4.Connection = con;
        //            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command4, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command4, "StaffID", DbType.Int64, item.StaffID);
        //            dbServer.AddInParameter(command4, "ProcedureID", DbType.Int64, item.ProcedureID);

        //            dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command4, trans);

        //        }

               

        //        foreach (var item in BizActionObj.objOTDetails.InstrumentList)
        //        {

        //            DbCommand command6;
        //            command6 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsInstrumentDetails");
        //            command6.Connection = con;
        //            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command6, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command6, "InstrumentID", DbType.Int64, item.InstrumentID);

        //            dbServer.AddInParameter(command6, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command6, "Rate", DbType.Double, item.Rate);
        //            dbServer.AddInParameter(command6, "Amount", DbType.Double, item.Amount);


        //            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command6, trans);

        //        }

        //        DbCommand command7;
        //        command7 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsAnesthesiaNotesDetails");
        //        command7.Connection = con;
        //        dbServer.AddInParameter(command7, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command7, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //        dbServer.AddInParameter(command7, "AnesthesiaNotes", DbType.String, BizActionObj.objOTDetails.objAnesthesiaNotes.AnesthesiaNotes);

        


        //        dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
        //        dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objAnesthesiaNotes.ID);


        //        dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int64, int.MaxValue);
        //        int intStatus4 = dbServer.ExecuteNonQuery(command7, trans);

        //        DbCommand command8;
        //        command8 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsSurgeryNotesDetails");
        //        command8.Connection = con;
        //        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command8, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //        dbServer.AddInParameter(command8, "SurgeyNotes", DbType.String, BizActionObj.objOTDetails.objSurgeryNotes.SurgeyNotes);




        //        dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
        //        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.objOTDetails.objSurgeryNotes.ID);


        //        dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int64, int.MaxValue);
        //        int intStatus5 = dbServer.ExecuteNonQuery(command8, trans);


        //        foreach (var item in BizActionObj.objOTDetails.ItemList)
        //        {

        //            DbCommand command5;
        //            command5 = dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsItemDetails");
        //            command5.Connection = con;
        //            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command5, "OTDetailsID", DbType.Int64, BizActionObj.objOTDetails.ID);
        //            dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item.ItemID);
        //            dbServer.AddInParameter(command5, "BatchID", DbType.Int64, item.BatchID);
        //            dbServer.AddInParameter(command5, "BatchCode", DbType.String, item.BatchCode);
        //            dbServer.AddInParameter(command5, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command5, "Rate", DbType.Double, item.Rate);
        //            dbServer.AddInParameter(command5, "Amount", DbType.Double, item.Amount);
        //            dbServer.AddInParameter(command5, "VatPer", DbType.Double, item.VatPer);
        //            dbServer.AddInParameter(command5, "VatAmount", DbType.Double, item.VatAmt);
        //            dbServer.AddInParameter(command5, "NetAmount", DbType.Double, item.NetAmount);
        //            dbServer.AddInParameter(command5, "IsPackeged", DbType.Boolean, item.IsPackeged);
        //            dbServer.AddInParameter(command5, "IsConsumed", DbType.Boolean, item.IsConsumed);

        //            dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
        //            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);


        //            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int64, int.MaxValue);
        //            int intStatus3 = dbServer.ExecuteNonQuery(command5, trans);

        //            item.StockDetails.BatchID = (long)item.BatchID;
        //            item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
        //            item.StockDetails.ItemID = (long)item.ItemID;
        //            item.StockDetails.TransactionTypeID = InventoryTransactionType.OTDetails;
        //            item.StockDetails.TransactionID = BizActionObj.objOTDetails.ID;
        //            item.StockDetails.TransactionQuantity = (double)item.Quantity;
                
                   
                    
        //                item.StockDetails.Time = DateTime.Now;
                   
        //            item.StockDetails.StoreID = item.StoreID;

        //            clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
        //            clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

        //            obj.Details = item.StockDetails;
        //            obj.Details.ID = 0;
        //            obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, objUserVO, con, trans);

        //            if (obj.SuccessStatus == -1)
        //            {
        //                throw new Exception();
        //            }

        //            item.StockDetails.ID = obj.Details.ID;

        //        }
             
                
        //        trans.Commit();

        //    }
        
          

        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        con.Close();
        //        trans = null;
        //        throw;

        //    }
        //    finally
        //    {
        //        con.Close();
        //        trans = null;
        //    }
        //    return BizActionObj;

        //}

        ///// <summary>
        ///// Gets OT Details
        ///// </summary>
       
        //public override IValueObject GetOTDetailsList(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetOTDetailsListizActionVO bizActionVo = valueObject as clsGetOTDetailsListizActionVO;
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetOtDetailsList");

        //        dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizActionVo.IsPagingEnabled);
        //        dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, bizActionVo.StartIndex);
        //        dbServer.AddInParameter(command, "maximumRows", DbType.Int64, bizActionVo.MaximumRows);
        //        dbServer.AddInParameter(command, "sortExpression", DbType.String, bizActionVo.SortExpression);
        //        dbServer.AddInParameter(command, "OTID", DbType.Int64, bizActionVo.OTID);
             
        //        dbServer.AddInParameter(command, "DocID", DbType.Int64, bizActionVo.DocID);
        //        dbServer.AddInParameter(command, "StaffID", DbType.Int64, bizActionVo.StaffID);

        //        dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {
        //                clsOTDetailsVO otDetailsObj = new clsOTDetailsVO();
        //                otDetailsObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                otDetailsObj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                otDetailsObj.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
        //                otDetailsObj.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
        //                otDetailsObj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
        //                otDetailsObj.DocName = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"]));

        //                otDetailsObj.FromTime = Convert.ToDateTime(DALHelper.HandleDate(reader["FromTime"]));
        //                otDetailsObj.ToTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ToTime"]));
        //                otDetailsObj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                       

        //                otDetailsObj.Remnark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
        //                otDetailsObj.OTName= Convert.ToString(DALHelper.HandleDBNull(reader["OTName"]));
        //                otDetailsObj.BedCategory = Convert.ToString(DALHelper.HandleDBNull(reader["BedCategory"]));
        //                otDetailsObj.Bed = Convert.ToString(DALHelper.HandleDBNull(reader["Bed"]));
        //                otDetailsObj.AdmissionDate = Convert.ToDateTime(DALHelper.HandleDate(reader["AdmissionDate"]));
        //                otDetailsObj.Ward = Convert.ToString(DALHelper.HandleDBNull(reader["Ward"]));
        //                otDetailsObj.detailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailsID"]));


        //                bizActionVo.objOTDetails.Add(otDetailsObj);
                        
        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}

        ///// <summary>
        ///// Gets detail tables of OT details by OT ID
        ///// </summary>
       
        //public override IValueObject GetDetailTablesOfOTDetailsByOTDetailID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO bizActionVo = valueObject as clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO;
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetOTDetailsForOTDetailsID");

        //        dbServer.AddInParameter(command, "OTDetailsID", DbType.Int64, bizActionVo.OTDetailsID);
             
             


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {

        //                bizActionVo.OTSheetDetailsObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                bizActionVo.OTSheetDetailsObj.FromTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"]));
        //                bizActionVo.OTSheetDetailsObj.ToTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"]));
        //                bizActionVo.OTSheetDetailsObj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
        //                bizActionVo.OTSheetDetailsObj.OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"]));
        //                bizActionVo.OTSheetDetailsObj.TotalHours = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalHours"]));
        //                bizActionVo.OTSheetDetailsObj.AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaTypeID"]));
        //                bizActionVo.OTSheetDetailsObj.ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]));
        //                bizActionVo.OTSheetDetailsObj.OTResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTResultID"]));
        //                bizActionVo.OTSheetDetailsObj.OTStatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTStatusID"]));
        //                bizActionVo.OTSheetDetailsObj.ManPower = Convert.ToInt64(DALHelper.HandleDBNull(reader["ManPower"]));
        //                bizActionVo.OTSheetDetailsObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
        //                bizActionVo.OTSheetDetailsObj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
        //                bizActionVo.OTSheetDetailsObj.OTName = Convert.ToString(DALHelper.HandleDBNull(reader["OTName"]));
        //                bizActionVo.OTSheetDetailsObj.AnesthesiaType = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaType"]));
        //                bizActionVo.OTSheetDetailsObj.ProcedureType = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureType"]));
        //                bizActionVo.OTSheetDetailsObj.OperationResult = Convert.ToString(DALHelper.HandleDBNull(reader["OperationResult"]));
        //                bizActionVo.OTSheetDetailsObj.OperationStatus = Convert.ToString(DALHelper.HandleDBNull(reader["OperationStatus"]));
                        
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOtDetailsProcedureDetailsVO procObj = new clsOtDetailsProcedureDetailsVO();
        //                procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                procObj.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
        //                procObj.ConcessionPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercent"]));
        //                procObj.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
        //                procObj.ServiceTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxPercent"]));
        //                procObj.ServiceTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ServiceTaxAmount"]));
        //                procObj.Quantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["Quantity"]));
        //                procObj.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
        //                procObj.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
        //                procObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                procObj.ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.ProcedureList.Add(procObj);

                             
        //            }
        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsDocDetailsVO docObj = new clsOTDetailsDocDetailsVO();
        //                docObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                docObj.DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DesignationID"]));
        //                docObj.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
        //                docObj.DocFees = Convert.ToDouble(DALHelper.HandleDBNull(reader["DocFees"]));
        //                docObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                docObj.docDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"]));
        //                docObj.designationDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Designation"]));
        //                MasterListItem defaultObj = new MasterListItem();
        //                defaultObj.ID = 0;
        //                defaultObj.Description = "--Select--";
        //                docObj.ProcedureList.Add(defaultObj);
        //                bizActionVo.DoctorList.Add(docObj);


        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsStaffDetailsVO StaffObj = new clsOTDetailsStaffDetailsVO();
        //                StaffObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

        //                StaffObj.StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffID"]));
                      
        //                StaffObj.ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
                    
        //                StaffObj.StaffDesc = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"]));
        //                MasterListItem defaultObj = new MasterListItem();
        //                defaultObj.ID = 0;
        //                defaultObj.Description = "--Select--";
        //                StaffObj.ProcedureList.Add(defaultObj);
        //                bizActionVo.StaffList.Add(StaffObj);


        //            }

        //            reader.NextResult();
                    

        //            while (reader.Read())
        //            {
        //                foreach (var item in bizActionVo.DoctorList)
        //                {
        //                    MasterListItem procObj = new MasterListItem();
        //                    procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                    procObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                    item.ProcedureList.Add(procObj);
                       
        //                    item.SelectedProcedure.ID = procObj.ID;
        //                    item.SelectedProcedure = item.ProcedureList.FirstOrDefault(q => q.ID == item.SelectedProcedure.ID);
                           
        //                }

        //                foreach (var item in bizActionVo.StaffList)
        //                {
        //                    MasterListItem procObj = new MasterListItem();
        //                    procObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]));
        //                    procObj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                    item.ProcedureList.Add(procObj);

        //                    item.SelectedProcedure.ID = procObj.ID;
        //                    item.SelectedProcedure = item.ProcedureList.FirstOrDefault(q => q.ID == item.SelectedProcedure.ID);

        //                }

        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsItemDetailsVO ItemObj = new clsOTDetailsItemDetailsVO();
        //                ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ItemObj.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
        //                ItemObj.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
        //                ItemObj.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
        //                ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                ItemObj.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
        //                ItemObj.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
        //                ItemObj.VatPer = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPer"]));
        //                ItemObj.VatAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
        //                ItemObj.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
        //                ItemObj.IsConsumed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsumed"]));
        //                ItemObj.IsPackeged = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPackeged"]));
        //                ItemObj.ItemDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                ItemObj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));


        //                bizActionVo.ItemList.Add(ItemObj);


        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {
        //                clsOTDetailsInstrumentDetailsVO ItemObj = new clsOTDetailsInstrumentDetailsVO();
        //                ItemObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ItemObj.InstrumentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstrumentID"]));
        //                ItemObj.InstrumentDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
        //                ItemObj.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
        //                ItemObj.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
        //                ItemObj.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["Amount"]));
                                     


        //                bizActionVo.InstrumentList.Add(ItemObj);


        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {


        //                bizActionVo.SurgeryNotesObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                bizActionVo.SurgeryNotesObj.SurgeyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["SurgeyNotes"]));
                  
                        
        //            }

        //            reader.NextResult();

        //            while (reader.Read())
        //            {


        //                bizActionVo.AnesthesiaNotesObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                bizActionVo.AnesthesiaNotesObj.AnesthesiaNotes = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));


        //            }

        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}

        //public override IValueObject GetConsetDetailsForConsentID(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsGetConsetDetailsForConsentIDBizActionVO bizActionVo = valueObject as             clsGetConsetDetailsForConsentIDBizActionVO ;
       
        //    DbDataReader reader = null;
        //    try
        //    {

        //        DbCommand command;
        //        command = dbServer.GetStoredProcCommand("CIMS_GetConsentDetailsForConsetID");

        //        dbServer.AddInParameter(command, "ConsentID", DbType.Int64, bizActionVo.ConsentID);
             
             


        //        reader = (DbDataReader)dbServer.ExecuteReader(command);
        //        if (reader.HasRows)
        //        {

        //            while (reader.Read())
        //            {

        //                bizActionVo.consentmaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                bizActionVo.consentmaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
        //                bizActionVo.consentmaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
        //                bizActionVo.consentmaster.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]));

        //            }

        //            reader.NextResult();
        //        }

                   
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return bizActionVo;
        //}


        //public override IValueObject AddPatientWiseConsentPrinting(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    clsAddPatientWiseConsentPrintingBizActionVO BizActionObj = valueObject as clsAddPatientWiseConsentPrintingBizActionVO;
        //    DbConnection con = null;
        //    DbTransaction trans = null;
        //    try
        //    {


        //        DbCommand command;
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        command = dbServer.GetStoredProcCommand("CIMS_AddPatientWiseConsentPrinting");
        //        command.Connection = con;

        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ConsetPrintingObj.PatientID);
        //        dbServer.AddInParameter(command, "ProcDate", DbType.DateTime, BizActionObj.ConsetPrintingObj.ProcDate);
        //        dbServer.AddInParameter(command, "ConsentID", DbType.Int64, BizActionObj.ConsetPrintingObj.ConsentID);

        //        dbServer.AddInParameter(command, "TemplateName", DbType.String, BizActionObj.ConsetPrintingObj.TemplateName);

        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, true);



        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, UserVo.ID);
        //        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ConsetPrintingObj.ID);





        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        trans = null;
        //        con.Close();
        //    }
        //    return BizActionObj;
                 
        //}
        
        
        
        






    }
}
