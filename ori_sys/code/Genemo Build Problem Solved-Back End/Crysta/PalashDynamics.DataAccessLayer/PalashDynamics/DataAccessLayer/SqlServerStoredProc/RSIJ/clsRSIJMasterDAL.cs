namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.RSIJ
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RSIJ;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.EMR;
    using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
    using PalashDynamics.ValueObjects.RSIJ;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    internal class clsRSIJMasterDAL : clsBaseRSIJMasterDAL
    {
        private Database dbServer;
        public bool chkFlag = true;

        private clsRSIJMasterDAL()
        {
            try
            {
                this.dbServer = HMSConfigurationManager.GetDatabaseReference();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetDiagnosisList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRSIJDiagnosisMasterBizactionVO nvo = (clsGetRSIJDiagnosisMasterBizactionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDiagnosisMasterDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                if ((nvo.Code != null) && ((nvo.Code != "") && (nvo.Code.Length != 0)))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, "%" + nvo.Code + "%");
                }
                if ((nvo.Diagnosis != null) && ((nvo.Diagnosis != "") && (nvo.Diagnosis.Length != 0)))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Diagnosis", DbType.String, "%" + nvo.Diagnosis + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsICD9", DbType.Boolean, nvo.IsICD9);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DiagnosisDetails == null)
                    {
                        nvo.DiagnosisDetails = new List<clsEMRDiagnosisVO>();
                    }
                    if (!nvo.IsICD9)
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                break;
                            }
                            clsEMRDiagnosisVO item = new clsEMRDiagnosisVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["Diagnosis"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                            };
                            nvo.DiagnosisDetails.Add(item);
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                break;
                            }
                            clsEMRDiagnosisVO item = new clsEMRDiagnosisVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                Categori = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["Diagnosis"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                            };
                            nvo.DiagnosisDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                }
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.dbServer = null;
            }
            return nvo;
        }

        public override IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRSIJDoctorDepartmentDetailsBizActionVO nvo = (clsGetRSIJDoctorDepartmentDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand;
                if (nvo.IsForReferral)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_FillDoctorComboboxSpecialisationWise");
                    this.dbServer.AddInParameter(storedProcCommand, "SpecialCode", DbType.String, nvo.SpecialCode);
                }
                else
                {
                    if (nvo.IsServiceWiseDoctorList)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillServiceWiseDoctorCombobox");
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceId", DbType.Int64, nvo.ServiceId);
                        this.dbServer.AddInParameter(storedProcCommand, "AllRecord", DbType.Boolean, nvo.AllRecord);
                    }
                    else if (nvo.FromRoster)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillDoctorForRosterCombobox");
                        this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Date);
                    }
                    else
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_FillDoctorCombobox");
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, nvo.DoctorCode);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "DepartmentCode", DbType.String, nvo.DepartmentCode);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if (!nvo.FromBill && !nvo.AllRecord)
                        {
                            if (!nvo.IsServiceWiseDoctorList)
                            {
                                nvo.MasterList.Add(new MasterListItem(Convert.ToString(reader["Code"]), Convert.ToString(reader["Description"])));
                                continue;
                            }
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"])), reader["Description"].ToString(), true, Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]))));
                            continue;
                        }
                        if (nvo.AllRecord)
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), (long) reader["ServiceID"], Convert.ToDouble(Convert.ToDecimal(reader["Rate"]))));
                            continue;
                        }
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), (long) reader["SpecializationID"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO)
        {
            throw new NotImplementedException();
        }

        public override IValueObject GetItemList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetRSIJItemListBizActionVO nvo = valueObject as clsGetRSIJItemListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand;
                if (!nvo.IsInsuraceDrug)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_GetItemListMultipleFiltered");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_NewGetItemListMultipleFiltered");
                    this.dbServer.AddInParameter(storedProcCommand, "OPDNO", DbType.String, nvo.OPDNO);
                }
                this.dbServer.AddInParameter(storedProcCommand, "MoleculeCode", DbType.String, nvo.MoleculeCode);
                this.dbServer.AddInParameter(storedProcCommand, "BrandName", DbType.String, nvo.BrandName);
                this.dbServer.AddInParameter(storedProcCommand, "IsQtyShow", DbType.Boolean, nvo.IsQtyShow);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromCurrentMedication", DbType.Boolean, nvo.IsFromCurrentMed);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsRSIJItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsRSIJItemMasterVO item = new clsRSIJItemMasterVO {
                            DrugCode = Convert.ToString(reader["DrugCode"]),
                            DrugName = Convert.ToString(reader["DrugName"]),
                            StockQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            MoleculeName = Convert.ToString(reader["MoleculeName"])
                        };
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetRSIJMasterListBizActionVO nvo = (clsGetRSIJMasterListBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (nvo.IsActive != null)
                {
                    builder.Append("Status = '" + nvo.IsActive.Value + "'");
                }
                if (nvo.Parent != null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" And ");
                    }
                    builder.Append(nvo.Parent.Value.ToString() + "='" + nvo.Parent.Key.ToString() + "'");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_GetMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.CodeColumn);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.DescriptionColumn);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem(Convert.ToString(reader["Code"]), reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetRSIJLaboratoryServiceBizActionVO nvo = (clsGetRSIJLaboratoryServiceBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = null;
                if (nvo.ServiceType == "Pathology")
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_GetLaboratoryDetailsList");
                    this.dbServer.AddInParameter(storedProcCommand, "GroupName", DbType.String, "%" + nvo.GroupName + "%");
                    this.dbServer.AddInParameter(storedProcCommand, "SpecializationCode", DbType.String, "%" + nvo.SpecializationCode + "%");
                }
                else if (nvo.ServiceType == "Radiology")
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_GetRadiologyDetailsList");
                }
                else if (nvo.ServiceType == "Diagnostik")
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_GetDiagnostikDetailsList");
                    if (!string.IsNullOrEmpty(nvo.ServiceDepartment))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceDepartment", DbType.String, "%" + nvo.ServiceDepartment + "%");
                    }
                }
                if ((nvo.ServiceCode != null) && ((nvo.ServiceCode != "") && (nvo.ServiceCode.Length != 0)))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceCode", DbType.String, "%" + nvo.ServiceCode + "%");
                }
                if ((nvo.Description != null) && ((nvo.Description != "") && (nvo.Description.Length != 0)))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, "%" + nvo.Description + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceDetails == null)
                    {
                        nvo.ServiceDetails = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Group = Convert.ToString(reader["GroupName"]),
                            SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationCode"]))
                        };
                        nvo.ServiceDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOPDQueueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRSIJQueueListBizActionVO nvo = (clsGetRSIJQueueListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("EMR_GetPatientQueueList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "DeptCode", DbType.String, nvo.DeptCode);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, nvo.DoctorCode);
                if ((nvo.FirstName != null) && (nvo.PatientName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientName", DbType.String, nvo.PatientName + "%");
                }
                if ((nvo.MRNo != null) && (nvo.MRNo.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo + "%");
                }
                if ((nvo.NoReg != null) && (nvo.NoReg.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "RegNo", DbType.String, nvo.NoReg + "%");
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.QueueList == null)
                    {
                        nvo.QueueList = new List<clsQueueVO>();
                    }
                    while (reader.Read())
                    {
                        clsQueueVO item = new clsQueueVO {
                            VisitID = Convert.ToInt64(reader["VisitID"]),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            PatientId = Convert.ToInt64(reader["PatientID"]),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            OPDNO = Convert.ToString(reader["OPDNO"]),
                            DepartmentCode = Convert.ToString(reader["DepartmentID"]),
                            DeptDescription = Convert.ToString(reader["DepartmentName"]),
                            NoReg = Convert.ToString(reader["NoReg"]),
                            MRNO = Convert.ToString(reader["MRNo"]),
                            DoctorCode = Convert.ToString(reader["DoctorID"]),
                            DoctorName = Convert.ToString(reader["DoctorName"])
                        };
                        item.DateTime = item.Date;
                        item.PatientType = "OPD";
                        item.Complaints = Convert.ToString(reader["complaints"]);
                        item.ReferredDoctor = Convert.ToString(reader["ReferredDoctor"]);
                        item.DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"]);
                        item.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        item.VisitType = Convert.ToString(reader["VisitType"]);
                        item.VisitTypeID = new long?(Convert.ToInt64(reader["VisitTypeID"]));
                        item.VisitStatus = Convert.ToBoolean(reader["CurrentVisitStatus"]);
                        item.PatientCategoryId = Convert.ToInt64(reader["PatientCategoryID"]);
                        nvo.QueueList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

