using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.Data.Common;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Patient
{
    public class clsHSGDAL : clsBaseHSGDAL
    {
         #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsHSGDAL()
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
                throw ex;
            }

        }


        public override IValueObject AddUpdateHSG(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateHSGBizActionVO BizActionObj = valueObject as clsAddUpdateHSGBizActionVO;
            clsCoupleVO coupleDetails = new clsCoupleVO();
           
            coupleDetails = BizActionObj.HSGDetails.CoupleDetail;
            try
            {
                clsHSGVO HSGDetails = BizActionObj.HSGDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateHSG");

                if (BizActionObj.HSGDetails.ID > 0)
                {
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.HSGDetails.ID);
                }
                else
                {
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, coupleDetails.CoupleId);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, coupleDetails.CoupleUnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, HSGDetails.PatientID);
                dbServer.AddInParameter(command, "PatientTypeID", DbType.Int64, coupleDetails.MalePatient.PatientTypeID);
                dbServer.AddInParameter(command, "Uterus", DbType.String, HSGDetails.Uterus);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, HSGDetails.HSGDate);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, HSGDetails.HSGTime);
                dbServer.AddInParameter(command, "Image", DbType.Binary, HSGDetails.Image);
                dbServer.AddInParameter(command, "cavity", DbType.Boolean, HSGDetails.cavity);
                dbServer.AddInParameter(command, "Patent_Tube", DbType.Boolean, HSGDetails.Patent_Tube);
                dbServer.AddInParameter(command, "Blocked_tube", DbType.Boolean, HSGDetails.Blocked_tube);
                dbServer.AddInParameter(command, "Cornul_blockage", DbType.Boolean, HSGDetails.Cornul_blockage);
                dbServer.AddInParameter(command, "Isthmic_Blockage", DbType.Boolean, HSGDetails.Isthmic_Blockage);
                dbServer.AddInParameter(command, "Ampullary_Blockage", DbType.Boolean, HSGDetails.Ampullary_Blockage);
                dbServer.AddInParameter(command, "Fimbrial_Blockage", DbType.Boolean, HSGDetails.Fimbrial_Blockage);
                dbServer.AddInParameter(command, "Remark", DbType.String, HSGDetails.Remark);
                dbServer.AddInParameter(command, "Hydrosalplnx", DbType.Boolean, HSGDetails.Hydrosalplnx);
                dbServer.AddInParameter(command, "Other_Patho", DbType.String, HSGDetails.Other_Patho);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, HSGDetails.IsFreezed);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy ", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddInParameter(command, "Description", DbType.String, HSGDetails.Description);
                dbServer.AddInParameter(command, "Title", DbType.String, HSGDetails.Title);
                dbServer.AddInParameter(command, "AttachedFileName", DbType.String, HSGDetails.AttachedFileName);
                dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, HSGDetails.AttachedFileContent);
                dbServer.AddInParameter(command, "IsDeleted", DbType.Boolean, HSGDetails.IsDeleted);


            //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, HSGDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.HSGDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
     
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetHSGDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetHSGBizActionVO BizActionObj = new clsGetHSGBizActionVO();
      
            BizActionObj = valueObject as clsGetHSGBizActionVO;
            clsHSGVO objHSG = BizActionObj.HSGDetails;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVE_GetHSG");
                DbDataReader reader;
                BizActionObj.HSGList = new List<clsHSGVO>();
                
                dbServer.AddInParameter(command, "ID", DbType.Int64, objHSG.PatientID);
                
             
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsHSGVO BizObjGeneral = new clsHSGVO();
                        BizActionObj.HSGDetails.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        BizActionObj.HSGDetails.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"]));
                        BizActionObj.HSGDetails.Uterus = Convert.ToString(DALHelper.HandleDBNull(reader["Uterus"]));
                        BizActionObj.HSGDetails.HSGDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizActionObj.HSGDetails.HSGTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        BizActionObj.HSGDetails.Image = (Byte[])DALHelper.HandleDBNull(reader["Image"]);
                        BizActionObj.HSGDetails.cavity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["cavity"]));
                        BizActionObj.HSGDetails.Patent_Tube = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Patent_Tube"]));

                        BizActionObj.HSGDetails.Blocked_tube = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Blocked_tube"]));
                        BizActionObj.HSGDetails.Cornul_blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Cornul_blockage"]));

                        BizActionObj.HSGDetails.Isthmic_Blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isthmic_Blockage"]));
                        BizActionObj.HSGDetails.Ampullary_Blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ampullary_Blockage"]));
                        BizActionObj.HSGDetails.Fimbrial_Blockage = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Fimbrial_Blockage"]));
                        BizActionObj.HSGDetails.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        BizActionObj.HSGDetails.Hydrosalplnx = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hydrosalplnx"]));
                        BizActionObj.HSGDetails.Other_Patho = Convert.ToString(DALHelper.HandleDBNull(reader["Other_Patho"]));
                        BizActionObj.HSGDetails.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));

                        BizActionObj.HSGDetails.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.HSGDetails.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        BizActionObj.HSGDetails.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        BizActionObj.HSGDetails.AttachedFileContent = (Byte[])DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        BizActionObj.HSGDetails.IsDeleted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDeleted"]));

                        //BizActionObj.HSGList.Add(BizObjGeneral);
                       
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valueObject;
        }
    }
}
