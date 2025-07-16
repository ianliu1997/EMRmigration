using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsPatientKinDetailsDAL : clsBasePatientKinDetailsDAL 
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion
        
        private clsPatientKinDetailsDAL()
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

        public override IValueObject AddPatientKinDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientKinDetailsBizActionVO BizActionObj = valueObject as clsAddPatientKinDetailsBizActionVO;
            try
            {
                clsPatientKinDetailsVO objPatientKinDetailsVO = BizActionObj.KinDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientKinDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objPatientKinDetailsVO.PatientId);
                dbServer.AddInParameter(command, "Name", DbType.String, objPatientKinDetailsVO.Name);
                dbServer.AddInParameter(command, "RelationId", DbType.Int64, objPatientKinDetailsVO.RelationId);
                dbServer.AddInParameter(command, "ContactNo1", DbType.String, objPatientKinDetailsVO.ContactNo1);
                dbServer.AddInParameter(command, "ContactNo2", DbType.String, objPatientKinDetailsVO.ContactNo2);
                dbServer.AddInParameter(command, "AddressLine1", DbType.String, objPatientKinDetailsVO.AddressLine1);
                dbServer.AddInParameter(command, "AddressLine2", DbType.String, objPatientKinDetailsVO.AddressLine2);
                dbServer.AddInParameter(command, "AddressLine3", DbType.String, objPatientKinDetailsVO.AddressLine3);
                dbServer.AddInParameter(command, "CountryId", DbType.Int64, objPatientKinDetailsVO.CountryId);
                dbServer.AddInParameter(command, "StateId", DbType.Int64, objPatientKinDetailsVO.StateId);
                dbServer.AddInParameter(command, "DistrictID", DbType.Int64, objPatientKinDetailsVO.DistrictID);
                dbServer.AddInParameter(command, "TalukaID", DbType.Int64, objPatientKinDetailsVO.TalukaID);
                dbServer.AddInParameter(command, "CityId", DbType.Int64, objPatientKinDetailsVO.CityId);
                dbServer.AddInParameter(command, "AreaId", DbType.Int64, objPatientKinDetailsVO.AreaId);
                dbServer.AddInParameter(command, "Pincode", DbType.String, objPatientKinDetailsVO.Pincode);
               
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPatientKinDetailsVO.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objPatientKinDetailsVO.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objPatientKinDetailsVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objPatientKinDetailsVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPatientKinDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objPatientKinDetailsVO.AddedWindowsLoginName);

                dbServer.AddOutParameter(command, "KinId", DbType.Int64, int.MaxValue);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.KinDetails.KinId = (long)dbServer.GetParameterValue(command, "KinId");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return valueObject;
        }
    }
}
