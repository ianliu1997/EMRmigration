namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsDischargeTemplateMasterDAL : clsBaseDischargeTemplateMasterDAL
    {
        private Database dbServer;

        private clsDischargeTemplateMasterDAL()
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

        public void AddUpdateDischargeTemplateDetails(clsAddDischargeTemplateMasterBizActionVO BizActionObj, clsUserVO objUserVO, DbTransaction trans)
        {
            if (BizActionObj.DischargeTemplateDetailsList != null)
            {
                foreach (clsDischargeTemplateDetailsVO svo in BizActionObj.DischargeTemplateDetailsList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDischargeTemplateDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, svo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DischargeTemplateID", DbType.String, BizActionObj.DischargeTemplateMaster.DischargeTemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "DischargeTemplateUnitID", DbType.String, BizActionObj.DischargeTemplateMaster.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "FieldName", DbType.String, svo.FieldName);
                    this.dbServer.AddInParameter(storedProcCommand, "ParameterName", DbType.String, svo.ParameterName);
                    this.dbServer.AddInParameter(storedProcCommand, "TextData", DbType.String, svo.TextData);
                    this.dbServer.AddInParameter(storedProcCommand, "ApplicableFont", DbType.String, svo.ApplicableFont);
                    this.dbServer.AddInParameter(storedProcCommand, "BindingControl", DbType.Int64, svo.BindingControl);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, trans);
                    BizActionObj.ResultStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
            }
        }

        public override IValueObject AddUpdateDischargeTemplateMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDischargeTemplateMasterBizActionVO bizActionObj = valueObject as clsAddDischargeTemplateMasterBizActionVO;
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
                clsDischargeTemplateMasterVO dischargeTemplateMaster = bizActionObj.DischargeTemplateMaster;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDischargeTemplateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dischargeTemplateMaster.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, dischargeTemplateMaster.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, dischargeTemplateMaster.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, dischargeTemplateMaster.Description);
                this.dbServer.AddInParameter(storedProcCommand, "IsTextTemplate", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, dischargeTemplateMaster.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, dischargeTemplateMaster.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, dischargeTemplateMaster.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, dischargeTemplateMaster.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, dischargeTemplateMaster.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, dischargeTemplateMaster.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, dischargeTemplateMaster.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, dischargeTemplateMaster.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, dischargeTemplateMaster.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, dischargeTemplateMaster.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, dischargeTemplateMaster.UpdatedWindowsLoginName);
                this.dbServer.AddParameter(storedProcCommand, "DischargeTemplateID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                bizActionObj.DischargeTemplateMaster.DischargeTemplateID = (long) this.dbServer.GetParameterValue(storedProcCommand, "DischargeTemplateID");
                bizActionObj.ResultStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                bizActionObj.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (bizActionObj.IsModify)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDischargeTemplateDetails");
                    this.dbServer.AddInParameter(command2, "DischargeTemplateID", DbType.Int64, dischargeTemplateMaster.ID);
                    this.dbServer.AddInParameter(command2, "DischargeTemplateUnitID", DbType.Int64, dischargeTemplateMaster.UnitID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if (!bizActionObj.IsCheckBox)
                {
                    this.AddUpdateDischargeTemplateDetails(bizActionObj, objUserVO, transaction);
                }
                transaction.Commit();
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

        public override IValueObject CheckCountDischargeSummaryByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDischargeTemplateMasterListBizActionVO nvo = valueObject as clsGetDischargeTemplateMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckCountDischargeSummaryByID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Count = Convert.ToInt64(DALHelper.HandleDBNull(reader["CountID"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDischargeTemplateMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDischargeTemplateMasterListBizActionVO nvo = valueObject as clsGetDischargeTemplateMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDischargeTemplateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DischargeTemplateMasterList == null)
                    {
                        nvo.DischargeTemplateMasterList = new List<clsDischargeTemplateMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsDischargeTemplateMasterVO item = new clsDischargeTemplateMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            UnitName = (string) reader["UnitName"],
                            Code = reader["Code"].ToString(),
                            Description = reader["Description"].ToString(),
                            IsTextTemplate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTextTemplate"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        DbCommand command2 = null;
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_GetDischargeTemplateDetails");
                        this.dbServer.AddInParameter(command2, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                        this.dbServer.AddInParameter(command2, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                        this.dbServer.AddInParameter(command2, "maximumRows", DbType.Int64, 200);
                        this.dbServer.AddInParameter(command2, "TotalRows", DbType.Int64, nvo.TotalRows);
                        this.dbServer.AddInParameter(command2, "DischargeTemplateID", DbType.String, item.ID.ToString());
                        this.dbServer.AddInParameter(command2, "DischargeTemplateUnitID", DbType.String, item.UnitID.ToString());
                        DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                        if (reader2.HasRows)
                        {
                            if (item.DischargeTemplateDetailsList == null)
                            {
                                item.DischargeTemplateDetailsList = new List<clsDischargeTemplateDetailsVO>();
                            }
                            while (reader2.Read())
                            {
                                clsDischargeTemplateDetailsVO svo = new clsDischargeTemplateDetailsVO();
                                item.DischargeTemplateDetails = new clsDischargeTemplateDetailsVO();
                                svo.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ID"]));
                                svo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"]));
                                svo.DisChargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DischargeTemplateID"]));
                                svo.DisChargeTemplateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DischargeTemplateUnitID"]));
                                svo.FieldName = reader2["FieldName"].ToString();
                                svo.ParameterName = reader2["ParameterName"].ToString();
                                svo.ApplicableFont = Convert.ToString(DALHelper.HandleDBNull(reader2["ApplicableFont"]));
                                svo.BindingControl = Convert.ToInt64(DALHelper.HandleDBNull(reader2["BindingControl"]));
                                if (svo.ParameterName.Equals(DischargeParameter.Advise.ToString()))
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.Advise);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (svo.ParameterName.Equals(DischargeParameter.Clinical_Findings.ToString()))
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.Clinical_Findings);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (svo.ParameterName.Equals(DischargeParameter.Diagnosis.ToString()))
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.Diagnosis);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (svo.ParameterName.Equals(DischargeParameter.Investigation.ToString()))
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.Investigation);
                                    svo.IsTextBoxEnable = false;
                                }
                                else if (svo.ParameterName.Equals(DischargeParameter.Medication.ToString()))
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.Medication);
                                    svo.IsTextBoxEnable = false;
                                }
                                else if (svo.ParameterName.Equals(DischargeParameter.Note.ToString()))
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.Note);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (svo.ParameterName.Equals(DischargeParameter.Operating_Notes.ToString()))
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.Operating_Notes);
                                    svo.IsTextBoxEnable = true;
                                }
                                else if (!svo.ParameterName.Equals(DischargeParameter.FollowUp.ToString()))
                                {
                                    svo.IsTextBoxEnable = true;
                                }
                                else
                                {
                                    svo.ParameterID = Convert.ToInt64(DischargeParameter.FollowUp);
                                    svo.IsTextBoxEnable = true;
                                }
                                svo.TextData = reader2["TextData"].ToString();
                                item.DischargeTemplateDetails = svo;
                                item.DischargeTemplateDetailsList.Add(svo);
                            }
                        }
                        reader2.Close();
                        nvo.DischargeTemplateMasterList.Add(item);
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
    }
}

