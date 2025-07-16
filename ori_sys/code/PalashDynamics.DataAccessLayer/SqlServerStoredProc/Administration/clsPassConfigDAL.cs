using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Administration.Menu;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
   public class clsPassConfigDAL : clsBasePassConfigDAL
   {
       #region Variable Declaration
       //Declare the Database Object
       private Database dbServer = null;

       //Declare the Logmanager Object
       private LogManager logmanager = null;
       #endregion

       private clsPassConfigDAL()
       {
           try
           {
               #region Create instance of Database, Logmanager and BaseSql Object
               //Create instance of Database Object and BaseSql object.
               if (dbServer == null)
               {
                   dbServer = HMSConfigurationManager.GetDatabaseReference();
               }

               //Create instance of Logmanager object.
               if (logmanager == null)
               {
                   logmanager = LogManager.GetInstance();
               }

               #endregion

           }
           catch (Exception ex)
           {
               
               throw;
           }
       }

       public override IValueObject UpdatePasswordConfig(IValueObject valueObject, clsUserVO UserVo)
       {
           bool CurrentMethodExecutionStatus = true;
      //     clsGetPassConfigBizActionVO BizActionObj = valueObject as clsGetPassConfigBizActionVO;
           clsAddPasswordConfigBizActionVO BizActionObj = valueObject as clsAddPasswordConfigBizActionVO;

           try
           {
               clsPassConfigurationVO objPAsswordConfigVO = BizActionObj.PasswordConfig;

             //  if (BizActionObj.PassConfig == null) BizActionObj.PassConfig = new clsPassConfigurationVO();
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePasswordConfig");
              // int MinPass=(int).objPAsswordConfigVO.MinPasswordLength.ToString();
               dbServer.AddInParameter(command, "MinPasswordLength", DbType.Int16, objPAsswordConfigVO.MinPasswordLength);
               dbServer.AddInParameter(command, "MaxPasswordLength", DbType.Int16, objPAsswordConfigVO.MaxPasswordLength);
               dbServer.AddInParameter(command, "MinPasswordAge", DbType.Int16, objPAsswordConfigVO.MinPasswordAge);
               dbServer.AddInParameter(command, "MaxPasswordAge", DbType.Int16, objPAsswordConfigVO.MaxPasswordAge);
               dbServer.AddInParameter(command, "AtLeastOneDigit", DbType.Boolean, objPAsswordConfigVO.AtLeastOneDigit);
               dbServer.AddInParameter(command, "AtLeastOneLowerCaseChar", DbType.Boolean, objPAsswordConfigVO.AtLeastOneLowerCaseChar);
               dbServer.AddInParameter(command, "AtleastOneUpperCaseChar", DbType.Boolean, objPAsswordConfigVO.AtLeastOneUpperCaseChar);
               dbServer.AddInParameter(command, "AtleastOneSpecialChar", DbType.Boolean, objPAsswordConfigVO.AtLeastOneSpecialChar);
               dbServer.AddInParameter(command, "NoOfPasswordsToRemember", DbType.Int16, objPAsswordConfigVO.NoOfPasswordsToRemember);
               dbServer.AddInParameter(command, "AccountLockThreshold", DbType.Int16, objPAsswordConfigVO.AccountLockThreshold);
               dbServer.AddInParameter(command, "AccountLockDuration", DbType.Double, objPAsswordConfigVO.AccountLockDuration);
               dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
               dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
               dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
               dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
               dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);

               int intStatus = dbServer.ExecuteNonQuery(command);
   
           }
           catch (Exception)
           {
               
               throw;
           }
           return valueObject;

       }


       public override IValueObject GetPassConfig(IValueObject valueObject, clsUserVO UserVo)
        {
           clsGetPassConfigBizActionVO BizActionObj  = valueObject as clsGetPassConfigBizActionVO ;
            try
           {
               if (BizActionObj.PassConfig == null) BizActionObj.PassConfig = new clsPassConfigurationVO();
               DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPassConfig");
               DbDataReader reader;

               reader = (DbDataReader)dbServer.ExecuteReader(command);

               if (reader.HasRows)
               {
                   reader.Read();

                     //  clsPassConfigurationVO objVO = new clsPassConfigurationVO();
                   BizActionObj.PassConfig.MinPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MinPasswordLength"]);
                   BizActionObj.PassConfig.MaxPasswordLength = (Int16)DALHelper.HandleDBNull(reader["MaxPasswordLength"]);
                   BizActionObj.PassConfig.MinPasswordAge = (Int16)DALHelper.HandleDBNull(reader["MinPasswordAge"]);
                   BizActionObj.PassConfig.MaxPasswordAge = (Int16)DALHelper.HandleDBNull(reader["MaxPasswordAge"]);
                   BizActionObj.PassConfig.AtLeastOneDigit = (bool)DALHelper.HandleDBNull(reader["AtLeastOneDigit"]);
                   BizActionObj.PassConfig.AtLeastOneLowerCaseChar = (bool)DALHelper.HandleDBNull(reader["AtLeastOneLowerCaseChar"]);
                   BizActionObj.PassConfig.AtLeastOneUpperCaseChar = (bool)DALHelper.HandleDBNull(reader["AtLeastOneUpperCaseChar"]);
                   BizActionObj.PassConfig.AtLeastOneSpecialChar = (bool)DALHelper.HandleDBNull(reader["AtLeastOneSpecialChar"]);
                   BizActionObj.PassConfig.NoOfPasswordsToRemember = (Int16)DALHelper.HandleDBNull(reader["NoOfPasswordsToRemember"]);
                   BizActionObj.PassConfig.AccountLockThreshold = (Int16)DALHelper.HandleDBNull(reader["AccountLockThreshold"]);
                   BizActionObj.PassConfig.AccountLockDuration = (float)(Double)DALHelper.HandleDBNull(reader["AccountLockDuration"]);
                                             
                 }
           }
           catch (Exception ex)
           {
               
               throw;
           }

           return valueObject;

        }
    }
}
