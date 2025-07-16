using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsVisitTypeDAL : clsBaseVisitTypeDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        public bool chkFlag = true;
        #endregion

        private clsVisitTypeDAL()
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

        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetVisitTypeBizActionVO BizActionObj = (clsGetVisitTypeBizActionVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVisitType");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<clsVisitTypeVO>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        clsVisitTypeVO objVO = new clsVisitTypeVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objVO.ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]));
                        objVO.IsClinical = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClinical"]));
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        //objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        //objVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        //objVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        //objVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        //objVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        //objVO.IsClinical = (bool)DALHelper.HandleDBNull(reader["IsClinical"]);
                        //objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.List.Add(objVO); //new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {

                //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;
        }


        public override IValueObject AddVisitType(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddVisitTypeBizActionVO BizActionObj = (clsAddVisitTypeBizActionVO)valueObject;
            if (BizActionObj.Details.ID == 0)
            {
                BizActionObj = AddVisit(BizActionObj, UserVO);
            }

            else
            {
                BizActionObj = UpdateVisit(BizActionObj, UserVO);

            }

            return BizActionObj;
        }

      

        private clsAddVisitTypeBizActionVO AddVisit(clsAddVisitTypeBizActionVO BizActionObj, clsUserVO UserVO)
        {
            try
            {

                clsVisitTypeVO ObjVisitVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddVisitTypeMaster");

                dbServer.AddInParameter(command, "Code", DbType.String, ObjVisitVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjVisitVO.Description.Trim());
                if (ObjVisitVO.ServiceID != null)
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, ObjVisitVO.ServiceID);
                dbServer.AddInParameter(command, "IsClinical", DbType.Boolean, ObjVisitVO.IsClinical);

                dbServer.AddInParameter(command, "FreeDaysDuration", DbType.Int64, ObjVisitVO.FreeDaysDuration);
                dbServer.AddInParameter(command, "IsFree", DbType.Boolean, ObjVisitVO.IsFree);
                dbServer.AddInParameter(command, "ConsultationVisitType", DbType.Int64, ObjVisitVO.ConsultationVisitType);


                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjVisitVO.Status);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                dbServer.AddParameter(command, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjVisitVO.ID);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "Id");
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));


            }

            catch (Exception ex)
            {
                throw;

            }
            return BizActionObj;
        }


        private clsAddVisitTypeBizActionVO UpdateVisit(clsAddVisitTypeBizActionVO BizActionObj, clsUserVO UserVO)
        {
            try
            {
                clsVisitTypeVO ObjVisitVO = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateVisitTypeMaster");

                dbServer.AddInParameter(command, "Id", DbType.Int64, ObjVisitVO.ID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjVisitVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjVisitVO.Description.Trim());
                if (ObjVisitVO.ServiceID != null)
                    dbServer.AddInParameter(command, "ServiceID", DbType.Int64, ObjVisitVO.ServiceID);
                dbServer.AddInParameter(command, "IsClinical", DbType.Boolean, ObjVisitVO.IsClinical);

                dbServer.AddInParameter(command, "FreeDaysDuration", DbType.Int64, ObjVisitVO.FreeDaysDuration);
                dbServer.AddInParameter(command, "IsFree", DbType.Boolean, ObjVisitVO.IsFree);
                dbServer.AddInParameter(command, "ConsultationVisitType", DbType.Int64, ObjVisitVO.ConsultationVisitType);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjVisitVO.Status);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
            }

            catch (Exception ex)
            {
                throw;

            }
            return BizActionObj;
        }


        public override IValueObject GetVisitTypeMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetAllVisitTypeMasetrBizActionVO BizActionObj = (clsGetAllVisitTypeMasetrBizActionVO)valueObject;

            try
            {
             DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVisitTypeMaster");
              dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
             

              dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

              dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
              dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
              dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
              dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
              dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

              dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.List == null)
                    {
                        BizActionObj.List = new List<clsVisitTypeVO>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        clsVisitTypeVO objVO = new clsVisitTypeVO();
                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        objVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        objVO.IsClinical = (bool)DALHelper.HandleDBNull(reader["IsClinical"]);
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                      //  objVO.IsFree = (bool)DALHelper.HandleDBNull(reader["IsFree"]);

                        objVO.IsFree = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"]));
                        objVO.FreeDaysDuration = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreeDaysDuration"]));

                        objVO.ConsultationVisitType = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsultationVisitTypeID"]));


                        BizActionObj.List.Add(objVO); //new MasterListItem((long)reader["Id"], reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));
                    }
                }

                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();

            }
        
            catch (Exception ex)
            {
                
            }
            finally
            {
                
            }

            return BizActionObj;
        }

        public override IValueObject CheckVisitTypeMappedWithPackageService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckVisitTypeMappedWithPackageServiceBizActionVO BizActionObj = (clsCheckVisitTypeMappedWithPackageServiceBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckVisitTypeMappedWithPackageService");
                dbServer.AddInParameter(command, "VisitTypeID", DbType.Int64, BizActionObj.VisitTypeID);
                dbServer.AddInParameter(command, "IsPackage", DbType.Boolean, BizActionObj.IsPackage);
                 DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                 if (reader.HasRows)
                 {
                     if (BizActionObj.VisitTypeDetails == null)
                         BizActionObj.VisitTypeDetails = new clsVisitTypeVO();
                     while (reader.Read())
                     {
                         BizActionObj.VisitTypeDetails.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                         BizActionObj.VisitTypeDetails.IsPackage = (bool)DALHelper.HandleDBNull(reader["IsPackage"]);
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


      

    }

}
