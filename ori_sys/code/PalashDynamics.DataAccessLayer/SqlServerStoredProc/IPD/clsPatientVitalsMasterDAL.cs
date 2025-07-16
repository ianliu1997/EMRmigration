using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.IPD;
using System.Data.Common;
using System.Data;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    public class clsPatientVitalsMasterDAL : clsBasePatientVitalsMasterDAL
    {
        #region Variables Declaration
        private Database dbServer = null;
        #endregion

        private clsPatientVitalsMasterDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
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


        public override IValueObject AddUpdatePatientVitalMaster(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVO)
        {
            clsIPDPatientVitalsMasterVO ObjVitalsMasterVO = new clsIPDPatientVitalsMasterVO();
            clsAddUpdateIPDPatientVitalsMasterBIzActionVO objItem = valueObject as clsAddUpdateIPDPatientVitalsMasterBIzActionVO;

            try
            {
                ObjVitalsMasterVO = objItem.PatientVitalDetailList[0];

                objItem.PatientVitalDetails = new clsIPDPatientVitalsMasterVO();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientVitalsMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjVitalsMasterVO.ID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjVitalsMasterVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, ObjVitalsMasterVO.Description.Trim());
                dbServer.AddInParameter(command, "IsModify", DbType.Boolean, ObjVitalsMasterVO.IsModify);
                dbServer.AddInParameter(command, "DefaultValue", DbType.Double, ObjVitalsMasterVO.DefaultValue);
                dbServer.AddInParameter(command, "MinValue", DbType.Double, ObjVitalsMasterVO.MinValue);
                dbServer.AddInParameter(command, "MaxValue", DbType.Double, ObjVitalsMasterVO.MaxValue);
                dbServer.AddInParameter(command, "Unit", DbType.String, ObjVitalsMasterVO.Unit);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, ObjVitalsMasterVO.Status);


                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, ObjVitalsMasterVO.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, ObjVitalsMasterVO.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, ObjVitalsMasterVO.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, ObjVitalsMasterVO.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjVitalsMasterVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, ObjVitalsMasterVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, ObjVitalsMasterVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, ObjVitalsMasterVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, ObjVitalsMasterVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, ObjVitalsMasterVO.UpdateWindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.PatientVitalDetails.SuccessStatus = Convert.ToInt64(dbServer.GetParameterValue(command, "ResultStatus"));

            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;

        }

        public override IValueObject GetPatientVitalMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDPatientVitalsMasterBIzActionVO BizActionObj = (clsGetIPDPatientVitalsMasterBIzActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("[CIMS_GetPatientVitalMasterList]");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "TotalRows", DbType.Int64, BizActionObj.TotalRows);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.VitalDetailsList == null)
                        BizActionObj.VitalDetailsList = new List<clsIPDPatientVitalsMasterVO>();
                    while (reader.Read())
                    {
                        clsIPDPatientVitalsMasterVO VisitMasterVO = new clsIPDPatientVitalsMasterVO();
                        VisitMasterVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        VisitMasterVO.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        VisitMasterVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        VisitMasterVO.DefaultValue = Convert.ToDouble(reader["DefaultValue"]);
                        VisitMasterVO.MinValue = Convert.ToDouble(reader["MinValue"]);
                        VisitMasterVO.MaxValue = Convert.ToDouble(reader["MaxValue"]);
                        VisitMasterVO.Unit = (string)DALHelper.HandleDBNull(reader["Unit"]);
                        VisitMasterVO.Status = (bool)reader["Status"];
                        BizActionObj.VitalDetailsList.Add(VisitMasterVO);
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

        public override IValueObject UpdateStatusPatientVitalMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusIPDPatientVitalsMasterBIzActionVO BizActionObj = (clsUpdateStatusIPDPatientVitalsMasterBIzActionVO)valueObject;
            try
            {
                clsIPDPatientVitalsMasterVO objVitalsVO = BizActionObj.VitalDetails;
                DbCommand command = dbServer.GetStoredProcCommand("[CIMS_UpdateStatusPatientVitalMaster]");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objVitalsVO.ID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objVitalsVO.Status);
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
    }
}
