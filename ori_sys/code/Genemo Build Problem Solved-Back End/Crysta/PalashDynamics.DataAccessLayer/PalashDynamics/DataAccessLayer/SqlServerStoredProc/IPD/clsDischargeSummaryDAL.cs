namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class clsDischargeSummaryDAL : clsBaseDischargeSummaryDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsDischargeSummaryDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
            }
        }

        public override IValueObject AddUpdateDischargeSummary(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddIPDDischargeSummaryBizActionVO bizActionObj = valueObject as clsAddIPDDischargeSummaryBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsIPDDischargeSummaryVO dischargeSummary = bizActionObj.DischargeSummary;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDischargeSummary");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dischargeSummary.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, dischargeSummary.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, dischargeSummary.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, dischargeSummary.AdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, dischargeSummary.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeTemplateID", DbType.Int64, dischargeSummary.DischargeTemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "DischargeTemplateUnitID", DbType.Int64, dischargeSummary.DischargeTemplateUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dischargeSummary.Date);
                this.dbServer.AddInParameter(storedProcCommand, "TextDocument", DbType.String, dischargeSummary.TextDocument);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancel", DbType.Boolean, dischargeSummary.IsCancel);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpDate", DbType.DateTime, dischargeSummary.FollowUpDate);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, dischargeSummary.CreatedUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, dischargeSummary.UpdatedUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, dischargeSummary.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, dischargeSummary.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, dischargeSummary.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, dischargeSummary.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, dischargeSummary.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, dischargeSummary.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, dischargeSummary.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, dischargeSummary.UpdatedWindowsLoginName);
                this.dbServer.AddParameter(storedProcCommand, "DischargeSummaryID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                bizActionObj.DischargeSummary.DischargeSummaryID = (long) this.dbServer.GetParameterValue(storedProcCommand, "DischargeSummaryID");
                bizActionObj.ResultStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (bizActionObj.IsModify)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDischargeSummaryDetail");
                    this.dbServer.AddInParameter(command2, "DischargeSummaryID", DbType.Int64, dischargeSummary.ID);
                    this.dbServer.AddInParameter(command2, "DischargeSummaryUnitID", DbType.Int64, dischargeSummary.UnitID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                this.AddUpdateDischargeSummaryDetails(bizActionObj, objUserVO, transaction);
                transaction.Commit();
                bizActionObj.ResultStatus = 0L;
            }
            catch (Exception)
            {
                bizActionObj.ResultStatus = -1L;
                transaction.Rollback();
                bizActionObj = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return bizActionObj;
        }

        public void AddUpdateDischargeSummaryDetails(clsAddIPDDischargeSummaryBizActionVO BizActionObj, clsUserVO objUserVO, DbTransaction trans)
        {
            if (BizActionObj.DischargeSummary.DischargeSummaryDetailsList != null)
            {
                foreach (clsDischargeSummaryDetailsVO svo in BizActionObj.DischargeSummary.DischargeSummaryDetailsList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDischargeSummaryDetail");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, svo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DischargeSummaryID", DbType.String, BizActionObj.DischargeSummary.DischargeSummaryID);
                    this.dbServer.AddInParameter(storedProcCommand, "DischargeSummaryUnitID", DbType.String, BizActionObj.DischargeSummary.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, svo.ParameterID);
                    this.dbServer.AddInParameter(storedProcCommand, "DischargeTempDetailID", DbType.Int64, svo.DischargeTempDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "DischargeTempDetailUnitID", DbType.Int64, svo.DischargeTempDetailUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Field", DbType.String, svo.FieldName);
                    this.dbServer.AddInParameter(storedProcCommand, "FieldDesc", DbType.String, svo.FieldDesc);
                    this.dbServer.AddInParameter(storedProcCommand, "FieldText", DbType.String, svo.FieldText);
                    this.dbServer.AddInParameter(storedProcCommand, "BindingControl", DbType.Int64, svo.BindingControl);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, trans);
                    BizActionObj.ResultStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
            }
        }

        public override IValueObject FillDataGridDischargeSummaryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsFillDataGridDischargeSummaryListBizActionVO nvo = valueObject as clsFillDataGridDischargeSummaryListBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillDataGridDischargeSummaryList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.String, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.String, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MrNo);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DischargeSummaryList == null)
                    {
                        nvo.DischargeSummaryList = new List<clsIPDDischargeSummaryVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDDischargeSummaryVO item = new clsIPDDischargeSummaryVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"])),
                            AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["PFirstName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["PMiddleName"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["PLastName"]))
                        };
                        object[] objArray = new object[] { item.FirstName, ' ', item.MiddleName, ' ', item.LastName };
                        item.DisDestination = string.Concat(objArray);
                        item.Date = new DateTime?(Convert.ToDateTime(reader["DischargeSummaryDate"]));
                        item.TextDocument = Convert.ToString(reader["TextDocument"]);
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        item.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        object[] objArray2 = new object[] { Convert.ToString(reader["DFirstName"]), ' ', Convert.ToString(reader["DMiddleName"]), ' ', Convert.ToString(reader["DLastName"]) };
                        item.DoctorName = string.Concat(objArray2);
                        item.DischargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateID"]));
                        item.TemplateName = Convert.ToString(reader["TemplateName"]);
                        item.AdmDate = new DateTime?(Convert.ToDateTime(reader["AdmissionDate"]));
                        item.IsCancel = Convert.ToBoolean(reader["IsCancel"]);
                        nvo.DischargeSummaryList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetIPDDischargeSummaryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDDischargeSummaryBizActionVO nvo = valueObject as clsGetIPDDischargeSummaryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDischargeSummaryList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmUnitID", DbType.Int64, nvo.AdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DischargeSummaryList == null)
                    {
                        nvo.DischargeSummaryList = new List<clsIPDDischargeSummaryVO>();
                    }
                    while (reader.Read())
                    {
                        nvo.DischargeSummaryDetails = new clsIPDDischargeSummaryVO();
                        clsIPDDischargeSummaryVO item = new clsIPDDischargeSummaryVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            TextDocument = reader["TextDocument"].ToString(),
                            AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"])),
                            AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            DischargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateID"])),
                            DischargeTemplateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateUnitID"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"])),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            IsCancel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancel"]))
                        };
                        DbCommand command2 = null;
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_GetDischargeSummaryDetails");
                        this.dbServer.AddInParameter(command2, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                        this.dbServer.AddInParameter(command2, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                        this.dbServer.AddInParameter(command2, "maximumRows", DbType.Int64, nvo.MaximumRows);
                        this.dbServer.AddInParameter(command2, "TotalRows", DbType.Int64, nvo.TotalRows);
                        this.dbServer.AddInParameter(command2, "DischargeSummaryID", DbType.String, item.ID.ToString());
                        this.dbServer.AddInParameter(command2, "DischargeSummaryUnitID", DbType.String, item.UnitID.ToString());
                        DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                        if (item.DischargeSummaryDetailsList == null)
                        {
                            item.DischargeSummaryDetailsList = new List<clsDischargeSummaryDetailsVO>();
                        }
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                clsDischargeSummaryDetailsVO svo = new clsDischargeSummaryDetailsVO();
                                item.DischargeSummaryDetails = new clsDischargeSummaryDetailsVO();
                                svo.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ID"]));
                                svo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"]));
                                svo.DisChargeSummaryID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DischargeSummaryID"]));
                                svo.DisChargeSummaryUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DisChargeSummaryUnitID"]));
                                svo.DischargeTempDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DischargeTempDetailID"]));
                                svo.DischargeTempDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DischargeTempDetailUnitID"]));
                                svo.ApplicableFont = reader2["ApplicableFont"].ToString();
                                svo.FieldName = reader2["Field"].ToString();
                                svo.FieldDesc = reader2["FieldDesc"].ToString();
                                svo.FieldText = reader2["FieldText"].ToString();
                                svo.ParameterID = Convert.ToInt64(reader2["ParameterID"]);
                                svo.BindingControl = Convert.ToInt64(reader2["BindingControl"]);
                                if (svo.ParameterID == Convert.ToInt64(DischargeParameter.Advise))
                                {
                                    svo.ParameterName = Convert.ToString(DischargeParameter.Advise);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (svo.ParameterID == Convert.ToInt64(DischargeParameter.Clinical_Findings))
                                {
                                    svo.ParameterName = Convert.ToString(DischargeParameter.Clinical_Findings);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (svo.ParameterID == Convert.ToInt64(DischargeParameter.Diagnosis))
                                {
                                    svo.ParameterName = Convert.ToString(DischargeParameter.Diagnosis);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (svo.ParameterID == Convert.ToInt64(DischargeParameter.Investigation))
                                {
                                    svo.ParameterName = Convert.ToString(DischargeParameter.Investigation);
                                    svo.IsTextBoxEnable = false;
                                }
                                else if (svo.ParameterID != Convert.ToInt64(DischargeParameter.Medication))
                                {
                                    svo.IsTextBoxEnable = (svo.ParameterID != Convert.ToInt64(DischargeParameter.Note)) ? ((svo.ParameterID != Convert.ToInt64(DischargeParameter.Operating_Notes)) || true) : true;
                                }
                                else
                                {
                                    svo.ParameterName = Convert.ToString(DischargeParameter.Medication);
                                    svo.IsTextBoxEnable = false;
                                }
                                item.DischargeSummaryDetails = svo;
                                item.DischargeSummaryDetailsList.Add(svo);
                            }
                        }
                        reader2.Close();
                        nvo.DischargeSummaryDetails = item;
                        nvo.DischargeSummaryList.Add(item);
                    }
                }
                DbCommand command3 = null;
                command3 = this.dbServer.GetStoredProcCommand("CIMS_GetAdmPatientDetailsByPatientID");
                this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(command3, "AdmID", DbType.Int64, nvo.AdmID);
                this.dbServer.AddInParameter(command3, "AdmUnitID", DbType.Int64, nvo.AdmUnitID);
                this.dbServer.AddInParameter(command3, "OPD_IPD", DbType.Int64, 1);
                this.dbServer.AddInParameter(command3, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(command3, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(command3, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(command3, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(command3, "TotalRows", DbType.Int64, 0);
                DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(command3);
                if (reader3.HasRows)
                {
                    nvo.AdmPatientDetails = new clsIPDDischargeSummaryVO();
                    while (reader3.Read())
                    {
                        clsIPDDischargeSummaryVO yvo2 = new clsIPDDischargeSummaryVO {
                            AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ID"])),
                            AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["UnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PatientUnitID"])),
                            DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["DoctorID"]))
                        };
                        nvo.AdmPatientDetails = yvo2;
                    }
                }
                reader3.Close();
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

        public override IValueObject GetPatientsDischargeSummaryInfoInHTML(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO nvo = valueObject as clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientsDischargeSummaryInfoInHTML");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.PrintID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionID", DbType.Int64, nvo.AdmID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, nvo.AdmUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DischargeSummaryDetails == null)
                    {
                        nvo.DischargeSummaryDetails = new clsIPDDischargeSummaryVO();
                    }
                    while (reader.Read())
                    {
                        nvo.DischargeSummaryDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.DischargeSummaryDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.DischargeSummaryDetails.DisDestination = Convert.ToString(reader["PatientName"]);
                        nvo.DischargeSummaryDetails.Date = new DateTime?(Convert.ToDateTime(reader["DischargeSummaryDate"]));
                        nvo.DischargeSummaryDetails.TextDocument = Convert.ToString(reader["TextDocument"]);
                        nvo.DischargeSummaryDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        nvo.DischargeSummaryDetails.Address = Convert.ToString(reader["Address"]);
                        nvo.DischargeSummaryDetails.Age = (int) reader["Age"];
                        nvo.DischargeSummaryDetails.Gender = Convert.ToString(reader["Gender"]);
                        nvo.DischargeSummaryDetails.AgeWithSex = nvo.DischargeSummaryDetails.Age + " / " + nvo.DischargeSummaryDetails.Gender;
                        nvo.DischargeSummaryDetails.DoctorName = Convert.ToString(reader["DoctorName"]);
                        nvo.DischargeSummaryDetails.DischargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateID"]));
                        nvo.DischargeSummaryDetails.AdmDate = new DateTime?(Convert.ToDateTime(reader["AdmissionDate"]));
                        nvo.DischargeSummaryDetails.DischargeDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DischargeDate"]));
                        nvo.DischargeSummaryDetails.DischargeTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DischargeTime"]));
                        nvo.DischargeSummaryDetails.Education = Convert.ToString(reader["Education"]);
                        nvo.DischargeSummaryDetails.UnitName = Convert.ToString(reader["ClinicName"]);
                        nvo.DischargeSummaryDetails.AdressLine1 = Convert.ToString(reader["address"]);
                        nvo.DischargeSummaryDetails.AddressLine2 = Convert.ToString(reader["AddressLine2"]);
                        nvo.DischargeSummaryDetails.AddressLine3 = Convert.ToString(reader["AddressLine3"]);
                        nvo.DischargeSummaryDetails.Email = Convert.ToString(reader["Email"]);
                        nvo.DischargeSummaryDetails.PinCode = Convert.ToString(reader["PinCode"]);
                        nvo.DischargeSummaryDetails.TinNo = Convert.ToString(reader["TinNo"]);
                        nvo.DischargeSummaryDetails.RegNo = Convert.ToString(reader["RegNo"]);
                        nvo.DischargeSummaryDetails.City = Convert.ToString(reader["City"]);
                        nvo.DischargeSummaryDetails.ContactNo = Convert.ToString(reader["MobileNO"]);
                        nvo.DischargeSummaryDetails.MobileNo = Convert.ToString(reader["MobNo"]);
                        nvo.DischargeSummaryDetails.IPDNO = Convert.ToString(reader["IPDNO"]);
                        nvo.DischargeSummaryDetails.TreatingDr = Convert.ToString(reader["TreatingDoctorName"]);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }
    }
}

