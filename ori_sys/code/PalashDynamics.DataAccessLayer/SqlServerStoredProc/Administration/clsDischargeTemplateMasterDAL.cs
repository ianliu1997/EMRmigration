using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using System.Data.Common;
using PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Administration
{
    public class clsDischargeTemplateMasterDAL : clsBaseDischargeTemplateMasterDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsDischargeTemplateMasterDAL()
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

        public override IValueObject AddUpdateDischargeTemplateMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddDischargeTemplateMasterBizActionVO BizActionObj = valueObject as clsAddDischargeTemplateMasterBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsDischargeTemplateMasterVO objDischarge = BizActionObj.DischargeTemplateMaster;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDischargeTemplateMaster");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDischarge.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDischarge.UnitID);
                dbServer.AddInParameter(command, "Code", DbType.String, objDischarge.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objDischarge.Description);

                dbServer.AddInParameter(command, "IsTextTemplate", DbType.Boolean, true);          //objDischarge.IsTextTemplate);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDischarge.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objDischarge.CreatedUnitID);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objDischarge.UpdatedUnitID);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objDischarge.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objDischarge.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDischarge.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objDischarge.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objDischarge.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDischarge.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objDischarge.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objDischarge.UpdatedWindowsLoginName);

                dbServer.AddParameter(command, "DischargeTemplateID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.DischargeTemplateMaster.DischargeTemplateID = (long)dbServer.GetParameterValue(command, "DischargeTemplateID");
                BizActionObj.ResultStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.SuccessStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.IsModify == true)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_DeleteDischargeTemplateDetails");
                    dbServer.AddInParameter(command1, "DischargeTemplateID", DbType.Int64, objDischarge.ID);
                    dbServer.AddInParameter(command1, "DischargeTemplateUnitID", DbType.Int64, objDischarge.UnitID);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                }

                if (BizActionObj.IsCheckBox == false)
                {
                    AddUpdateDischargeTemplateDetails(BizActionObj, objUserVO, trans);
                }

                trans.Commit();
                //BizActionObj.ResultStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.ResultStatus = -1;
                trans.Rollback();
                BizActionObj = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        public void AddUpdateDischargeTemplateDetails(clsAddDischargeTemplateMasterBizActionVO BizActionObj, clsUserVO objUserVO, DbTransaction trans)
        {
            if (BizActionObj.DischargeTemplateDetailsList != null)
            {
                foreach (clsDischargeTemplateDetailsVO objDischarge in BizActionObj.DischargeTemplateDetailsList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDischargeTemplateDetails");
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDischarge.UnitID);
                    dbServer.AddInParameter(command, "DischargeTemplateID", DbType.String, BizActionObj.DischargeTemplateMaster.DischargeTemplateID);
                    dbServer.AddInParameter(command, "DischargeTemplateUnitID", DbType.String, BizActionObj.DischargeTemplateMaster.UnitID);

                    dbServer.AddInParameter(command, "FieldName", DbType.String, objDischarge.FieldName);
                    dbServer.AddInParameter(command, "ParameterName", DbType.String, objDischarge.ParameterName);
                    dbServer.AddInParameter(command, "TextData", DbType.String, objDischarge.TextData);
                    dbServer.AddInParameter(command, "ApplicableFont", DbType.String, objDischarge.ApplicableFont);
                    dbServer.AddInParameter(command, "BindingControl", DbType.Int64, objDischarge.BindingControl);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);

                    BizActionObj.ResultStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
                }
            }
        }

        public override IValueObject GetDischargeTemplateMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDischargeTemplateMasterListBizActionVO BizActionObj = valueObject as clsGetDischargeTemplateMasterListBizActionVO;
            //CIMS_GetDischargeTemplateMaster
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDischargeTemplateMaster");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizActionObj.SearchExpression);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DischargeTemplateMasterList == null)
                        BizActionObj.DischargeTemplateMasterList = new List<clsDischargeTemplateMasterVO>();

                    while (reader.Read())
                    {
                        clsDischargeTemplateMasterVO objDischarge = new clsDischargeTemplateMasterVO();
                        objDischarge.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDischarge.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objDischarge.UnitName = (string)reader["UnitName"];
                        objDischarge.Code = reader["Code"].ToString();
                        objDischarge.Description = reader["Description"].ToString();
                        objDischarge.IsTextTemplate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTextTemplate"]));
                        objDischarge.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        DbCommand command1 = null;
                        command1 = dbServer.GetStoredProcCommand("CIMS_GetDischargeTemplateDetails");
                        DbDataReader reader1;
                        //command1.Parameters.Clear();
                        dbServer.AddInParameter(command1, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                        dbServer.AddInParameter(command1, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                        dbServer.AddInParameter(command1, "maximumRows", DbType.Int64, 200);//BizActionObj.MaximumRows);
                        dbServer.AddInParameter(command1, "TotalRows", DbType.Int64, BizActionObj.TotalRows);
                        dbServer.AddInParameter(command1, "DischargeTemplateID", DbType.String, objDischarge.ID.ToString());
                        dbServer.AddInParameter(command1, "DischargeTemplateUnitID", DbType.String, objDischarge.UnitID.ToString());

                        reader1 = (DbDataReader)dbServer.ExecuteReader(command1);
                        if (reader1.HasRows)
                        {
                            if (objDischarge.DischargeTemplateDetailsList == null)
                                objDischarge.DischargeTemplateDetailsList = new List<clsDischargeTemplateDetailsVO>();

                            while (reader1.Read())
                            {
                                clsDischargeTemplateDetailsVO objDischargeDetails = new clsDischargeTemplateDetailsVO();
                                objDischarge.DischargeTemplateDetails = new clsDischargeTemplateDetailsVO();
                                objDischargeDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ID"]));
                                objDischargeDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["UnitID"]));
                                objDischargeDetails.DisChargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["DischargeTemplateID"]));
                                objDischargeDetails.DisChargeTemplateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["DischargeTemplateUnitID"]));
                                objDischargeDetails.FieldName = reader1["FieldName"].ToString();
                                objDischargeDetails.ParameterName = reader1["ParameterName"].ToString();
                                objDischargeDetails.ApplicableFont = Convert.ToString(DALHelper.HandleDBNull(reader1["ApplicableFont"]));
                                objDischargeDetails.BindingControl = Convert.ToInt64(DALHelper.HandleDBNull(reader1["BindingControl"]));

                                //objDischargeDetails.IsTextBoxEnable = false;
                                if (objDischargeDetails.ParameterName.Equals(DischargeParameter.Advise.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.Advise);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterName.Equals(DischargeParameter.Clinical_Findings.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.Clinical_Findings);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterName.Equals(DischargeParameter.Diagnosis.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.Diagnosis);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterName.Equals(DischargeParameter.Investigation.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.Investigation);
                                    objDischargeDetails.IsTextBoxEnable = false;
                                }
                                else if (objDischargeDetails.ParameterName.Equals(DischargeParameter.Medication.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.Medication);
                                    objDischargeDetails.IsTextBoxEnable = false;
                                }
                                else if (objDischargeDetails.ParameterName.Equals(DischargeParameter.Note.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.Note);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterName.Equals(DischargeParameter.Operating_Notes.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.Operating_Notes);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterName.Equals(DischargeParameter.FollowUp.ToString()))
                                {
                                    objDischargeDetails.ParameterID = Convert.ToInt64(DischargeParameter.FollowUp);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else
                                {
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }

                                objDischargeDetails.TextData = reader1["TextData"].ToString();
                                objDischarge.DischargeTemplateDetails = objDischargeDetails;
                                objDischarge.DischargeTemplateDetailsList.Add(objDischargeDetails);
                            }
                        }
                        reader1.Close();

                        BizActionObj.DischargeTemplateMasterList.Add(objDischarge);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject CheckCountDischargeSummaryByID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDischargeTemplateMasterListBizActionVO BizActionObj = valueObject as clsGetDischargeTemplateMasterListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckCountDischargeSummaryByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.Count = Convert.ToInt64(DALHelper.HandleDBNull(reader["CountID"]));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}
