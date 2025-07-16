using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.NursingStation;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.NursingStation;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.NursingStation.EMR;
using System.Collections.ObjectModel;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc//.NursingStation
{
    class clsDrugAdministrationChartDAL : clsBaseDrugAdministrationChartDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsDrugAdministrationChartDAL()
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

        public override IValueObject GetCurrentPrescriptionList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsDrugAdministrationChartBizActionVO BizActionObj = (clsDrugAdministrationChartBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPrescriptionForPatient");
                dbServer.AddInParameter(command, "AdmisionID", DbType.Int64, BizActionObj.DrugAdministrationChart.AdmissionID);
                dbServer.AddInParameter(command, "AdmisionUnitID", DbType.Int64, BizActionObj.DrugAdministrationChart.AdmissionUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PrescriptionMasterList == null)
                        BizActionObj.PrescriptionMasterList = new List<clsPrescriptionMasterVO>();

                    while (reader.Read())
                    {
                        clsPrescriptionMasterVO objPrescriptionVO = new clsPrescriptionMasterVO();

                        objPrescriptionVO.ID = Convert.ToInt64(reader["ID"]);
                        objPrescriptionVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        objPrescriptionVO.OPD_IPD = Convert.ToInt16(reader["Opd_Ipd"]);
                        objPrescriptionVO.Opd_Ipd_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_Id"]));
                        objPrescriptionVO.Opd_Ipd_UnitID = Convert.ToInt64(reader["Opd_Ipd_UnitID"]);

                        objPrescriptionVO.PrescriptionDate = Convert.ToString(reader["Date"]);
                        objPrescriptionVO.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));

                        BizActionObj.PrescriptionMasterList.Add(objPrescriptionVO);
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

        public override IValueObject GetDrugListForDrugChart(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDrugListForDrugChartBizActionVO BizActionObj = (clsGetDrugListForDrugChartBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPrescriptionDrugList");
                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                dbServer.AddInParameter(command, "PrescriptionUnitID", DbType.Int64, BizActionObj.PrescriptionUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DrugList == null)
                        BizActionObj.DrugList = new List<clsPrescriptionDetailsVO>();
                    int i = 1;
                    while (reader.Read())
                    {
                        clsPrescriptionDetailsVO objPrescriptionVO = new clsPrescriptionDetailsVO();

                        objPrescriptionVO.SrNo = i++;
                        objPrescriptionVO.ID = Convert.ToInt64(reader["ID"]);
                        objPrescriptionVO.UnitID = Convert.ToInt64(reader["UnitID"]);

                        objPrescriptionVO.DrugName = Convert.ToString(reader["DrugName"]);
                        objPrescriptionVO.MoleculeName = Convert.ToString(reader["MoleculeName"]);
                        objPrescriptionVO.Despense = Convert.ToString(reader["Despense"]);

                        objPrescriptionVO.Dose = Convert.ToString(reader["Dose"]);
                        objPrescriptionVO.Duration = Convert.ToDouble(reader["Duration"]);
                        objPrescriptionVO.Quantity = Convert.ToDouble(reader["Quantity"]);

                        objPrescriptionVO.Instruction = Convert.ToString(reader["Instruction"]);
                        objPrescriptionVO.DrugId = Convert.ToInt64(reader["DrugId"]);
                        objPrescriptionVO.DoseId = Convert.ToInt64(reader["DoseId"]);

                        objPrescriptionVO.Route = Convert.ToString(reader["Route"]);   //Added on 08032017

                        objPrescriptionVO.Frequency = Convert.ToString(reader["Frequency"]);  //Added By YK
                        objPrescriptionVO.Remark = Convert.ToString(reader["Remark"]);  //Added By YK
                        objPrescriptionVO.ConsumeQuantity = Convert.ToDouble(reader["ConsumeQty"]);//Added By YK
                         
                        BizActionObj.DrugList.Add(objPrescriptionVO);
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

        public override IValueObject SaveDrugFeedingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSaveDrugFeedingDetailsBizActionVO BizActionObj = valueObject as clsSaveDrugFeedingDetailsBizActionVO;
            try
            {
                //List<clsPrescriptionDetailsVO> objEmergencyVO = BizActionObj.DrugFeedingList;

                ObservableCollection<clsPrescriptionDetailsVO> objEmergencyVO = BizActionObj.DrugFeedingListObserv;

                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_DeleteFeedingDetails");

                dbServer.AddInParameter(command1, "PrescriptionId", DbType.Int64, BizActionObj.PrescriptionID);
                dbServer.AddInParameter(command1, "PrescriptionUnitId", DbType.Int64, BizActionObj.PrescriptionUnitID);

                dbServer.AddInParameter(command1, "Opd_Ipd_Id", DbType.Int64, BizActionObj.Opd_Ipd_Id);
                dbServer.AddInParameter(command1, "Opd_Ipd_UnitId", DbType.Int64, BizActionObj.Opd_Ipd_UnitID);
                dbServer.AddInParameter(command1, "Opd_Ipd", DbType.Int64, BizActionObj.OPD_IPD);

                int intStatus = dbServer.ExecuteNonQuery(command1);



                foreach (var item in objEmergencyVO)
                {
                    if (item.IsFreeze == true)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDrugFeedingDetail");

                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "PrescriptionId", DbType.Int64, BizActionObj.PrescriptionID);
                        dbServer.AddInParameter(command, "PrescriptionUnitId", DbType.Int64, BizActionObj.PrescriptionUnitID);

                        dbServer.AddInParameter(command, "Opd_Ipd_Id", DbType.Int64, BizActionObj.Opd_Ipd_Id);
                        dbServer.AddInParameter(command, "Opd_Ipd_UnitId", DbType.Int64, BizActionObj.Opd_Ipd_UnitID);
                        dbServer.AddInParameter(command, "Opd_Ipd", DbType.Int64, BizActionObj.OPD_IPD);

                        dbServer.AddInParameter(command, "DrugId", DbType.Int64, item.DrugId);
                        dbServer.AddInParameter(command, "EmployeeId", DbType.Int64, item.DepartmentID);
                        //dbServer.AddInParameter(command, "EmployeeId", DbType.Int64, item.SelectedTakenBy.ID);
                        dbServer.AddInParameter(command, "DrugDate", DbType.DateTime, item.Date);

                        dbServer.AddInParameter(command, "DrugTime", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command, "Quantity", DbType.Int64, item.Quantity);
                        dbServer.AddInParameter(command, "Remark", DbType.String, item.Remark);

                        dbServer.AddInParameter(command, "IsFreeze", DbType.Boolean, item.IsFreeze);

                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        dbServer.AddInParameter(command, "DoctorName", DbType.String, item.DoctorName);

                        intStatus = dbServer.ExecuteNonQuery(command);
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }


            return BizActionObj;
        }

        public override IValueObject GetDrugFeedingDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetFeedingDetailsBizActionVO BizActionObj = (clsGetFeedingDetailsBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFeedingDetails");
                dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, BizActionObj.PrescriptionID);
                dbServer.AddInParameter(command, "PrescriptionUnitID", DbType.Int64, BizActionObj.PrescriptionUnitID);

                dbServer.AddInParameter(command, "Opd_Ipd_Id", DbType.Int64, BizActionObj.Opd_Ipd_Id);
                dbServer.AddInParameter(command, "Opd_Ipd_UnitId", DbType.Int64, BizActionObj.Opd_Ipd_UnitID);
                dbServer.AddInParameter(command, "Opd_Ipd", DbType.Int64, BizActionObj.OPD_IPD);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DrugFeedingList == null)
                        BizActionObj.DrugFeedingList = new List<clsPrescriptionDetailsVO>();
                    
                    while (reader.Read())
                    {
                        clsPrescriptionDetailsVO objPrescriptionVO = new clsPrescriptionDetailsVO();
                        objPrescriptionVO.RowID = BizActionObj.DrugFeedingList.Count + 1;
                        objPrescriptionVO.ID = Convert.ToInt64(reader["Id"]);
                        //objPrescriptionVO.UnitID = Convert.ToInt64(reader["UnitId"]);
                        objPrescriptionVO.PrescriptionID = Convert.ToInt64(reader["PrescriptionId"]);
                        objPrescriptionVO.PrescriptionUnitID = Convert.ToInt64(reader["PrescriptionUnitId"]);

                        objPrescriptionVO.Opd_Ipd_Id = Convert.ToString(reader["Opd_Ipd_Id"]);
                        objPrescriptionVO.Opd_Ipd_UnitId = Convert.ToString(reader["Opd_Ipd_UnitId"]);
                        objPrescriptionVO.Opd_Ipd_Id = Convert.ToString(reader["Opd_Ipd_Id"]);

                        objPrescriptionVO.DrugId = Convert.ToInt64(reader["DrugId"]);
                        objPrescriptionVO.DrugName = Convert.ToString(reader["DrugName"]);
                        objPrescriptionVO.Date = Convert.ToDateTime(reader["DrugDate"]);

                        objPrescriptionVO.TakenID = Convert.ToInt64(reader["TakenID"]);
                        objPrescriptionVO.TakenBy = Convert.ToString(reader["TakenBy"]);
                        objPrescriptionVO.Description = Convert.ToString(reader["TakenBy"]);
                        objPrescriptionVO.Quantity = Convert.ToDouble(reader["Quantity"]);
                        objPrescriptionVO.Remark = Convert.ToString(reader["Remark"]);
                        objPrescriptionVO.IsFreeze = Convert.ToBoolean(reader["IsFreeze"]);
                        objPrescriptionVO.Frequency = Convert.ToString(reader["Frequency"]);
                        objPrescriptionVO.Route = Convert.ToString(reader["Route"]);
                        objPrescriptionVO.UOM = Convert.ToString(reader["UOM"]);
                        objPrescriptionVO.DoctorName = Convert.ToString(reader["DoctorName"]);
                        if (!objPrescriptionVO.IsFreeze)
                        {
                            objPrescriptionVO.IsDelete = true;
                        }
                        BizActionObj.DrugFeedingList.Add(objPrescriptionVO);
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

        public override IValueObject UpdateFeedingDetailsFreeze(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateFeedingDetailsIsFreezeBizActionVO BizActionObj = (clsUpdateFeedingDetailsIsFreezeBizActionVO)valueObject;
            try
            {
                clsPrescriptionDetailsVO objVO = BizActionObj.PrescriptionDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDrugFeedingIsFreeze");

                dbServer.AddInParameter(command, "Id", DbType.Int64, objVO.ID);

                dbServer.AddInParameter(command, "IsFreeze", DbType.Boolean,objVO.IsFreeze);
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
