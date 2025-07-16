using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.CRM;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsCampServiceDAL : clsBaseCampServiceDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsCampServiceDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql Object
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

        public override IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCampServiceBizActionVO BizAction = (clsAddCampServiceBizActionVO)valueObject;
            //try
            //{
            //    clsCampServiceVO objCampServiceVO = BizAction.CampService;

            //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCampService");
            //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objCampServiceVO.UnitID);
            //        dbServer.AddInParameter(command, "CampDetailID", DbType.Int64, objCampServiceVO.CampDetailID);
            //        dbServer.AddInParameter(command, "ServiceID", DbType.Int64, objCampServiceVO.ServiceID);
            //        dbServer.AddInParameter(command, "IsFree", DbType.Boolean, objCampServiceVO.IsFree);
            //        dbServer.AddInParameter(command, "IsConcession", DbType.Boolean, objCampServiceVO.IsConcession);
            //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objCampServiceVO.Status);
            //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objCampServiceVO.CreatedUnitID);
            //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objCampServiceVO.AddedBy);
            //        dbServer.AddInParameter(command, "AddedOn", DbType.String, objCampServiceVO.AddedOn);
            //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objCampServiceVO.AddedDateTime);
            //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objCampServiceVO.AddedWindowsLoginName);

            //        //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
            //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCampServiceVO.ID);

            //        int intStatus = dbServer.ExecuteNonQuery(command);

            //         //BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            //        BizAction.CampService.ID = (long)dbServer.GetParameterValue(command, "ID");

            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //    }
                return BizAction;
            }

        
    }
}
