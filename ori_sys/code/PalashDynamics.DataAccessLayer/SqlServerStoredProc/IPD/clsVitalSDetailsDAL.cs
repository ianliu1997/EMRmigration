using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Administration.StaffMaster;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{

    public class clsIPDVitalSDetailsDAL : clsBaseIPDVitalSDetailsDAL
    {

        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIPDVitalSDetailsDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if(dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 
                if(logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch(Exception ex)
            {

                //  throw;
            }
        }

        public override IValueObject GetUnitWiseEmpDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetUnitWiseEmpBizActionVO BizAction = (clsGetUnitWiseEmpBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillStaffByUnitID");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if(reader.HasRows)
                {
                    if(BizAction.MasterList == null)
                    {
                        BizAction.MasterList = new List<MasterListItem>();
                        BizAction.StaffMasterList = new List<clsStaffMasterVO>();
                    }
                    while(reader.Read())
                    {
                        clsStaffMasterVO StaffMasterVO = new clsStaffMasterVO();
                        StaffMasterVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        StaffMasterVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        StaffMasterVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        StaffMasterVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        StaffMasterVO.DOB = (DateTime?)DALHelper.HandleDate(reader["DOB"]);
                        StaffMasterVO.EmailId = reader["EmailId"].ToString();
                        BizAction.StaffMasterList.Add(StaffMasterVO);          
              
                        //Reading the record from reader and stores in list
                        BizAction.MasterList.Add(new MasterListItem((long)reader["ID"], reader["Description"].ToString()));
                    }
                }
            }

            catch(Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizAction;
        }

        public override IValueObject GetTPRDetailsListByAdmIDAdmUnitID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO BizActionObj = valueObject as clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetTPRDetailsByAdmIdAndAdmUnitID");

                dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.GetVitalSDetails.VisitAdmID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.GetVitalSDetails.VisitAdmUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.GetVitalSDetails.Date);

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.GetVitalSDetails.InputPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.GetVitalSDetails.InputStartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.GetVitalSDetails.InputMaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.GetVitalSDetails.SortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.GetVitalSDetailsList == null)
                        BizActionObj.GetVitalSDetailsList = new List<clsIPDVitalSDetailsVO>();
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO objAdvVO = new clsIPDVitalSDetailsVO();

                        objAdvVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdvVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objAdvVO.VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"]));
                        objAdvVO.VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"]));
                        
                        objAdvVO.Date= (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        objAdvVO.Time1 = Convert.ToString(DALHelper.HandleDBNull(reader["Time1"]));
                        
                        objAdvVO.Temperature = Convert.ToInt64(DALHelper.HandleDBNull(reader["Temperature"]));
                        objAdvVO.Pulse = Convert.ToInt64(DALHelper.HandleDBNull(reader["Height"]));
                        objAdvVO.Status = (bool)reader["Status"];

                        objAdvVO.BP_Sys = Convert.ToInt64(DALHelper.HandleDBNull(reader["BP_Sys"]));
                        objAdvVO.BP_Dia = Convert.ToInt64(DALHelper.HandleDBNull(reader["BP_Dia"]));
                        objAdvVO.Respiration = Convert.ToInt64(DALHelper.HandleDBNull(reader["Respiration"]));

                       //objAdvVO.TakenName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])) + Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));

                        objAdvVO.IsEncounter = (bool)DALHelper.HandleDBNull(reader["IsEncounter"]);

                        BizActionObj.GetVitalSDetailsList.Add(objAdvVO);
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

        public override IValueObject GetVitalsDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalSDetailsListBizActionVO BizActionObj = valueObject as clsGetVitalSDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVitalsDetailList");
              
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.GetVitalSDetailsList == null)
                        BizActionObj.GetVitalSDetailsList = new List<clsIPDVitalSDetailsVO>();
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO objAdvVO = new clsIPDVitalSDetailsVO();

                        objAdvVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdvVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        
                        objAdvVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        objAdvVO.DefaultValue = Convert.ToDouble(reader["DefaultValue"]);
                        objAdvVO.MinValue = Convert.ToDouble(reader["MinValue"]);
                        objAdvVO.MaxValue = Convert.ToDouble(reader["MaxValue"]);

                        objAdvVO.Unit = Convert.ToString(DALHelper.HandleDBNull(reader["Unit"]));

                        BizActionObj.GetVitalSDetailsList.Add(objAdvVO);
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

        public override IValueObject AddVitalSDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDVitalSDetailsVO objItemVO = new clsIPDVitalSDetailsVO();
            clsAddVitalSDetailsBizActionVO BizActionObj = valueObject as clsAddVitalSDetailsBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                if(con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddVitalDetails");
                //dbServer.AddOutParameter(command, "ID", DbType.Int64, item.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.AddVitalDetails.VisitAdmID);
                dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.AddVitalDetails.VisitAdmUnitID);
                dbServer.AddInParameter(command, "Opd_Ipd", DbType.Int16, BizActionObj.AddVitalDetails.Opd_Ipd);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.AddVitalDetails.Date);

                dbServer.AddInParameter(command, "IsEncounter", DbType.Boolean, BizActionObj.AddVitalDetails.IsEncounter);
                dbServer.AddInParameter(command, "TakenBy", DbType.Int64, BizActionObj.AddVitalDetails.TakenBy);
                dbServer.AddInParameter(command, "Status ", DbType.Boolean,true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.UserGeneralDetailVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserGeneralDetailVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objUserVO.UserGeneralDetailVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.AddVitalDetails.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                foreach (clsIPDVitalSDetailsVO item in BizActionObj.AddVitalDetailsList)
                {                 
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddVitalsDetails");
                   // dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "VitalsID", DbType.Int64, BizActionObj.AddVitalDetails.ID);
                    dbServer.AddInParameter(command1, "VitalsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "VitalSignID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command1, "Value", DbType.Double, item.DefaultValue);
                    dbServer.AddInParameter(command1, "Remark", DbType.String, item.Remark);

                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objUserVO.UserGeneralDetailVO.AddedBy);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, objUserVO.UserGeneralDetailVO.AddedOn);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objUserVO.UserGeneralDetailVO.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                   
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch(Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetListofVitalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalSDetailsListBizActionVO BizActionObj = valueObject as clsGetVitalSDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetListofVitalDetails");

                dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.GetVitalSDetails.VisitAdmID);
                dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.GetVitalSDetails.VisitAdmUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.GetVitalSDetails.Date);
                
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.GetVitalSDetailsList == null)
                        BizActionObj.GetVitalSDetailsList = new List<clsIPDVitalSDetailsVO>();
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO objAdvVO = new clsIPDVitalSDetailsVO();

                        objAdvVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        objAdvVO.Temperature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temperature"]));
                        objAdvVO.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        objAdvVO.BP_Sys = Convert.ToDouble(DALHelper.HandleDBNull(reader["BP_Sys"]));
                        objAdvVO.BP_Dia = Convert.ToDouble(DALHelper.HandleDBNull(reader["BP_Dia"]));
                        objAdvVO.Respiration = Convert.ToDouble(DALHelper.HandleDBNull(reader["Respiration"]));


                        objAdvVO.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        objAdvVO.Time1 = Convert.ToString(DALHelper.HandleDBNull(reader["Time1"]));
                        //objAdvVO.TakenBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["TakenBy"]));
                        objAdvVO.TakenByName = Convert.ToString(DALHelper.HandleDBNull(reader["TakenByName"]));
                        objAdvVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.GetVitalSDetailsList.Add(objAdvVO);
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

        public override IValueObject UpdateStatusVitalDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusVitalDetailsBizActionVO BizActionObj = (clsUpdateStatusVitalDetailsBizActionVO)valueObject;
            try
            {
                clsIPDVitalSDetailsVO ObjVitalVO = BizActionObj.GetVitalSDetails;


                DbCommand command = dbServer.GetStoredProcCommand("[CIMS_UpdateStatusVitalDetails]");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjVitalVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjVitalVO.Status);
                int intstatus = dbServer.ExecuteNonQuery(command);
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

        public override IValueObject GetGraphDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGraphDetailsBizActionVO BizActionObj = valueObject as clsGetGraphDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGraphDetails");
                dbServer.AddInParameter(command, "VisitAdmID", DbType.Int64, BizActionObj.GetGraphDetails.VisitAdmID);
                dbServer.AddInParameter(command, "VisitAdmUnitID", DbType.Int64, BizActionObj.GetGraphDetails.VisitAdmUnitID);
                dbServer.AddInParameter(command, "VitalSignID", DbType.Int64, BizActionObj.GetGraphDetails.VitalSignID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.GetGraphDetails.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.GetGraphDetails.ToDate);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.GetGraphDetailsList == null)
                        BizActionObj.GetGraphDetailsList = new List<clsIPDVitalSDetailsVO>();
                    while (reader.Read())
                    {
                        clsIPDVitalSDetailsVO objAdvVO = new clsIPDVitalSDetailsVO();

                        objAdvVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objAdvVO.VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmID"]));
                        objAdvVO.VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmUnitID"]));
                        objAdvVO.VitalsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitalsID"]));
                        objAdvVO.VitalsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitalsUnitID"]));
                        objAdvVO.VitalSignID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitalSignID"]));
                        objAdvVO.Value = Convert.ToDouble(DALHelper.HandleDBNull(reader["Value"]));

                        //objAdvVO.Temperature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temperature"]));
                        //objAdvVO.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        //objAdvVO.BP_Sys = Convert.ToDouble(DALHelper.HandleDBNull(reader["BP_Sys"]));
                        //objAdvVO.BP_Dia = Convert.ToDouble(DALHelper.HandleDBNull(reader["BP_Dia"]));
                        //objAdvVO.Respiration = Convert.ToDouble(DALHelper.HandleDBNull(reader["Respiration"]));
                        objAdvVO.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        //objAdvVO.Time1 = Convert.ToString(DALHelper.HandleDBNull(reader["Time1"]));
                        ////objAdvVO.TakenBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["TakenBy"]));
                        //objAdvVO.TakenByName = Convert.ToString(DALHelper.HandleDBNull(reader["TakenByName"]));
                        //objAdvVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.GetGraphDetailsList.Add(objAdvVO);
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
    }
}
