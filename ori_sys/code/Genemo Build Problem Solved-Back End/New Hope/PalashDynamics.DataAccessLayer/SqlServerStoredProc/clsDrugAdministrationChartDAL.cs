namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.NursingStation;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.NursingStation;
    using PalashDynamics.ValueObjects.NursingStation.EMR;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;

    internal class clsDrugAdministrationChartDAL : clsBaseDrugAdministrationChartDAL
    {
        private Database dbServer;

        private clsDrugAdministrationChartDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetCurrentPrescriptionList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsDrugAdministrationChartBizActionVO nvo = (clsDrugAdministrationChartBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPrescriptionForPatient");
                this.dbServer.AddInParameter(storedProcCommand, "AdmisionID", DbType.Int64, nvo.DrugAdministrationChart.AdmissionID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmisionUnitID", DbType.Int64, nvo.DrugAdministrationChart.AdmissionUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PrescriptionMasterList == null)
                    {
                        nvo.PrescriptionMasterList = new List<clsPrescriptionMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPrescriptionMasterVO item = new clsPrescriptionMasterVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            OPD_IPD = Convert.ToInt16(reader["Opd_Ipd"]),
                            Opd_Ipd_Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_Id"])),
                            Opd_Ipd_UnitID = Convert.ToInt64(reader["Opd_Ipd_UnitID"]),
                            PrescriptionDate = Convert.ToString(reader["Date"]),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]))
                        };
                        nvo.PrescriptionMasterList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDrugFeedingDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetFeedingDetailsBizActionVO nvo = (clsGetFeedingDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFeedingDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionUnitID", DbType.Int64, nvo.PrescriptionUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_Id", DbType.Int64, nvo.Opd_Ipd_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_UnitId", DbType.Int64, nvo.Opd_Ipd_UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd", DbType.Int64, nvo.OPD_IPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DrugFeedingList == null)
                    {
                        nvo.DrugFeedingList = new List<clsPrescriptionDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPrescriptionDetailsVO item = new clsPrescriptionDetailsVO {
                            RowID = nvo.DrugFeedingList.Count + 1,
                            ID = Convert.ToInt64(reader["Id"]),
                            PrescriptionID = Convert.ToInt64(reader["PrescriptionId"]),
                            PrescriptionUnitID = Convert.ToInt64(reader["PrescriptionUnitId"]),
                            Opd_Ipd_Id = Convert.ToString(reader["Opd_Ipd_Id"]),
                            Opd_Ipd_UnitId = Convert.ToString(reader["Opd_Ipd_UnitId"]),
                            DrugId = Convert.ToInt64(reader["DrugId"]),
                            DrugName = Convert.ToString(reader["DrugName"]),
                            Date = new DateTime?(Convert.ToDateTime(reader["DrugDate"])),
                            TakenID = Convert.ToInt64(reader["TakenID"]),
                            TakenBy = Convert.ToString(reader["TakenBy"]),
                            Description = Convert.ToString(reader["TakenBy"]),
                            Quantity = Convert.ToDouble(reader["Quantity"]),
                            Remark = Convert.ToString(reader["Remark"]),
                            IsFreeze = Convert.ToBoolean(reader["IsFreeze"]),
                            Frequency = Convert.ToString(reader["Frequency"]),
                            Route = Convert.ToString(reader["Route"]),
                            UOM = Convert.ToString(reader["UOM"]),
                            DoctorName = Convert.ToString(reader["DoctorName"])
                        };
                        if (!item.IsFreeze)
                        {
                            item.IsDelete = true;
                        }
                        nvo.DrugFeedingList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDrugListForDrugChart(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDrugListForDrugChartBizActionVO nvo = (clsGetDrugListForDrugChartBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPrescriptionDrugList");
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionUnitID", DbType.Int64, nvo.PrescriptionUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DrugList == null)
                    {
                        nvo.DrugList = new List<clsPrescriptionDetailsVO>();
                    }
                    int num = 1;
                    while (reader.Read())
                    {
                        clsPrescriptionDetailsVO item = new clsPrescriptionDetailsVO {
                            SrNo = num++,
                            ID = Convert.ToInt64(reader["ID"]),
                            UnitID = Convert.ToInt64(reader["UnitID"]),
                            DrugName = Convert.ToString(reader["DrugName"]),
                            MoleculeName = Convert.ToString(reader["MoleculeName"]),
                            Despense = Convert.ToString(reader["Despense"]),
                            Dose = Convert.ToString(reader["Dose"]),
                            Duration = Convert.ToDouble(reader["Duration"]),
                            Quantity = Convert.ToDouble(reader["Quantity"]),
                            Instruction = Convert.ToString(reader["Instruction"]),
                            DrugId = Convert.ToInt64(reader["DrugId"]),
                            DoseId = Convert.ToInt64(reader["DoseId"]),
                            Route = Convert.ToString(reader["Route"]),
                            Frequency = Convert.ToString(reader["Frequency"]),
                            Remark = Convert.ToString(reader["Remark"]),
                            ConsumeQuantity = Convert.ToDouble(reader["ConsumeQty"])
                        };
                        nvo.DrugList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject SaveDrugFeedingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSaveDrugFeedingDetailsBizActionVO nvo = valueObject as clsSaveDrugFeedingDetailsBizActionVO;
            try
            {
                 ObservableCollection<clsPrescriptionDetailsVO> drugFeedingListObserv = nvo.DrugFeedingListObserv;

                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteFeedingDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionId", DbType.Int64, nvo.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionUnitId", DbType.Int64, nvo.PrescriptionUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_Id", DbType.Int64, nvo.Opd_Ipd_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_UnitId", DbType.Int64, nvo.Opd_Ipd_UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd", DbType.Int64, nvo.OPD_IPD);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                foreach (clsPrescriptionDetailsVO svo in drugFeedingListObserv)
                {
                    if (svo.IsFreeze)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDrugFeedingDetail");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "PrescriptionId", DbType.Int64, nvo.PrescriptionID);
                        this.dbServer.AddInParameter(command2, "PrescriptionUnitId", DbType.Int64, nvo.PrescriptionUnitID);
                        this.dbServer.AddInParameter(command2, "Opd_Ipd_Id", DbType.Int64, nvo.Opd_Ipd_Id);
                        this.dbServer.AddInParameter(command2, "Opd_Ipd_UnitId", DbType.Int64, nvo.Opd_Ipd_UnitID);
                        this.dbServer.AddInParameter(command2, "Opd_Ipd", DbType.Int64, nvo.OPD_IPD);
                        this.dbServer.AddInParameter(command2, "DrugId", DbType.Int64, svo.DrugId);
                        this.dbServer.AddInParameter(command2, "EmployeeId", DbType.Int64, svo.DepartmentID);
                        this.dbServer.AddInParameter(command2, "DrugDate", DbType.DateTime, svo.Date);
                        this.dbServer.AddInParameter(command2, "DrugTime", DbType.DateTime, svo.Date);
                        this.dbServer.AddInParameter(command2, "Quantity", DbType.Int64, svo.Quantity);
                        this.dbServer.AddInParameter(command2, "Remark", DbType.String, svo.Remark);
                        this.dbServer.AddInParameter(command2, "IsFreeze", DbType.Boolean, svo.IsFreeze);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddInParameter(command2, "DoctorName", DbType.String, svo.DoctorName);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception exception1)
            {
                string message = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject UpdateFeedingDetailsFreeze(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateFeedingDetailsIsFreezeBizActionVO nvo = (clsUpdateFeedingDetailsIsFreezeBizActionVO) valueObject;
            try
            {
                clsPrescriptionDetailsVO prescriptionDetails = nvo.PrescriptionDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDrugFeedingIsFreeze");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, prescriptionDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreeze", DbType.Boolean, prescriptionDetails.IsFreeze);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

