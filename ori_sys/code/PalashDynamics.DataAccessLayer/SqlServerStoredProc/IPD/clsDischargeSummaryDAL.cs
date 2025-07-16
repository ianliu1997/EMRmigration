using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using PalashDynamics.ValueObjects;
using System.Data;
using System.Data.Common;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster;
namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    class clsDischargeSummaryDAL : clsBaseDischargeSummaryDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsDischargeSummaryDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {

            }
        }

        public override IValueObject GetIPDDischargeSummaryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDDischargeSummaryBizActionVO BizActionObj = valueObject as clsGetIPDDischargeSummaryBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDischargeSummaryList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, BizActionObj.AdmID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, BizActionObj.AdmUnitID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, 0);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DischargeSummaryList == null)
                        BizActionObj.DischargeSummaryList = new List<clsIPDDischargeSummaryVO>();

                    while (reader.Read())
                    {
                        BizActionObj.DischargeSummaryDetails = new clsIPDDischargeSummaryVO();
                        clsIPDDischargeSummaryVO objDischarge = new clsIPDDischargeSummaryVO();
                        objDischarge.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objDischarge.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objDischarge.TextDocument = reader["TextDocument"].ToString();
                        objDischarge.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"]));
                        objDischarge.AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"]));
                        objDischarge.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objDischarge.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objDischarge.DischargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateID"]));
                        objDischarge.DischargeTemplateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateUnitID"]));
                        objDischarge.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        objDischarge.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        objDischarge.IsCancel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancel"]));
                        DbCommand command1 = null;
                        command1 = dbServer.GetStoredProcCommand("CIMS_GetDischargeSummaryDetails");
                        DbDataReader reader1;
                        dbServer.AddInParameter(command1, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                        dbServer.AddInParameter(command1, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                        dbServer.AddInParameter(command1, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                        dbServer.AddInParameter(command1, "TotalRows", DbType.Int64, BizActionObj.TotalRows);
                        dbServer.AddInParameter(command1, "DischargeSummaryID", DbType.String, objDischarge.ID.ToString());
                        dbServer.AddInParameter(command1, "DischargeSummaryUnitID", DbType.String, objDischarge.UnitID.ToString());

                        reader1 = (DbDataReader)dbServer.ExecuteReader(command1);

                        if (objDischarge.DischargeSummaryDetailsList == null)
                            objDischarge.DischargeSummaryDetailsList = new List<clsDischargeSummaryDetailsVO>();
                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                clsDischargeSummaryDetailsVO objDischargeDetails = new clsDischargeSummaryDetailsVO();
                                objDischarge.DischargeSummaryDetails = new clsDischargeSummaryDetailsVO();
                                objDischargeDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ID"]));
                                objDischargeDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["UnitID"]));
                                objDischargeDetails.DisChargeSummaryID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["DischargeSummaryID"]));
                                objDischargeDetails.DisChargeSummaryUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["DisChargeSummaryUnitID"]));
                                objDischargeDetails.DischargeTempDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["DischargeTempDetailID"]));
                                objDischargeDetails.DischargeTempDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["DischargeTempDetailUnitID"]));
                                objDischargeDetails.ApplicableFont = reader1["ApplicableFont"].ToString();
                                objDischargeDetails.FieldName = reader1["Field"].ToString();
                                objDischargeDetails.FieldDesc = reader1["FieldDesc"].ToString();
                                objDischargeDetails.FieldText = reader1["FieldText"].ToString();
                                objDischargeDetails.ParameterID = Convert.ToInt64(reader1["ParameterID"]);
                                objDischargeDetails.BindingControl = Convert.ToInt64(reader1["BindingControl"]);

                                if (objDischargeDetails.ParameterID == Convert.ToInt64(DischargeParameter.Advise))
                                {
                                    objDischargeDetails.ParameterName = Convert.ToString(DischargeParameter.Advise);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterID == Convert.ToInt64(DischargeParameter.Clinical_Findings))
                                {
                                    objDischargeDetails.ParameterName = Convert.ToString(DischargeParameter.Clinical_Findings);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterID == Convert.ToInt64(DischargeParameter.Diagnosis))
                                {
                                    objDischargeDetails.ParameterName = Convert.ToString(DischargeParameter.Diagnosis);
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterID == Convert.ToInt64(DischargeParameter.Investigation))
                                {
                                    objDischargeDetails.ParameterName = Convert.ToString(DischargeParameter.Investigation);
                                    objDischargeDetails.IsTextBoxEnable = false;
                                }
                                else if (objDischargeDetails.ParameterID == Convert.ToInt64(DischargeParameter.Medication))
                                {
                                    objDischargeDetails.ParameterName = Convert.ToString(DischargeParameter.Medication);
                                    objDischargeDetails.IsTextBoxEnable = false;
                                }
                                else if (objDischargeDetails.ParameterID == Convert.ToInt64(DischargeParameter.Note))
                                {
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else if (objDischargeDetails.ParameterID == Convert.ToInt64(DischargeParameter.Operating_Notes))
                                {
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                else
                                {
                                    objDischargeDetails.IsTextBoxEnable = true;
                                }
                                objDischarge.DischargeSummaryDetails = objDischargeDetails;
                                objDischarge.DischargeSummaryDetailsList.Add(objDischargeDetails);
                            }
                        }
                        reader1.Close();

                        BizActionObj.DischargeSummaryDetails = objDischarge;
                        BizActionObj.DischargeSummaryList.Add(objDischarge);
                    }
                }

                DbCommand command2 = null;
                command2 = dbServer.GetStoredProcCommand("CIMS_GetAdmPatientDetailsByPatientID");
                DbDataReader reader2;
                dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command2, "AdmID", DbType.Int64, BizActionObj.AdmID);
                dbServer.AddInParameter(command2, "AdmUnitID", DbType.Int64, BizActionObj.AdmUnitID);
                dbServer.AddInParameter(command2, "OPD_IPD", DbType.Int64, 1);
                dbServer.AddInParameter(command2, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command2, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command2, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command2, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command2, "TotalRows", DbType.Int64, 0);
                reader2 = (DbDataReader)dbServer.ExecuteReader(command2);
                if (reader2.HasRows)
                {
                    BizActionObj.AdmPatientDetails = new clsIPDDischargeSummaryVO();
                    while (reader2.Read())
                    {
                        clsIPDDischargeSummaryVO objItem = new clsIPDDischargeSummaryVO();
                        objItem.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ID"]));
                        objItem.AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"]));
                        objItem.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PatientID"]));
                        objItem.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PatientUnitID"]));
                        objItem.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DoctorID"]));
                        BizActionObj.AdmPatientDetails = objItem;
                    }
                }
                reader2.Close();
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

        public override IValueObject AddUpdateDischargeSummary(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddIPDDischargeSummaryBizActionVO BizActionObj = valueObject as clsAddIPDDischargeSummaryBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsIPDDischargeSummaryVO objDischarge = BizActionObj.DischargeSummary;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDischargeSummary");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDischarge.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDischarge.UnitID);
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, objDischarge.AdmID);
                dbServer.AddInParameter(command, "AdmUnitID", DbType.Int64, objDischarge.AdmUnitID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDischarge.DoctorID);
                dbServer.AddInParameter(command, "DischargeTemplateID", DbType.Int64, objDischarge.DischargeTemplateID);
                dbServer.AddInParameter(command, "DischargeTemplateUnitID", DbType.Int64, objDischarge.DischargeTemplateUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDischarge.Date);
                dbServer.AddInParameter(command, "TextDocument", DbType.String, objDischarge.TextDocument);
                dbServer.AddInParameter(command, "IsCancel", DbType.Boolean, objDischarge.IsCancel);
                dbServer.AddInParameter(command, "FollowUpDate", DbType.DateTime, objDischarge.FollowUpDate);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objDischarge.CreatedUnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objDischarge.UpdatedUnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objDischarge.AddedBy);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, objDischarge.AddedOn);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDischarge.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int32, objDischarge.UpdatedBy);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objDischarge.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDischarge.UpdatedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objDischarge.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, objDischarge.UpdatedWindowsLoginName);

                dbServer.AddParameter(command, "DischargeSummaryID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.DischargeSummary.DischargeSummaryID = (long)dbServer.GetParameterValue(command, "DischargeSummaryID");
                BizActionObj.ResultStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");


                if (BizActionObj.IsModify == true)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_DeleteDischargeSummaryDetail");
                    dbServer.AddInParameter(command1, "DischargeSummaryID", DbType.Int64, objDischarge.ID);
                    dbServer.AddInParameter(command1, "DischargeSummaryUnitID", DbType.Int64, objDischarge.UnitID);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                }
                AddUpdateDischargeSummaryDetails(BizActionObj, objUserVO, trans);


                trans.Commit();
                BizActionObj.ResultStatus = 0;
            }
            catch (Exception ex)
            {
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

        public void AddUpdateDischargeSummaryDetails(clsAddIPDDischargeSummaryBizActionVO BizActionObj, clsUserVO objUserVO, DbTransaction trans)
        {
            if (BizActionObj.DischargeSummary.DischargeSummaryDetailsList != null)
            {
                foreach (clsDischargeSummaryDetailsVO objDischarge in BizActionObj.DischargeSummary.DischargeSummaryDetailsList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDischargeSummaryDetail");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDischarge.UnitID);
                    dbServer.AddInParameter(command, "DischargeSummaryID", DbType.String, BizActionObj.DischargeSummary.DischargeSummaryID);
                    dbServer.AddInParameter(command, "DischargeSummaryUnitID", DbType.String, BizActionObj.DischargeSummary.UnitID);
                    dbServer.AddInParameter(command, "ParameterID", DbType.Int64, objDischarge.ParameterID);
                    dbServer.AddInParameter(command, "DischargeTempDetailID", DbType.Int64, objDischarge.DischargeTempDetailID);
                    dbServer.AddInParameter(command, "DischargeTempDetailUnitID", DbType.Int64, objDischarge.DischargeTempDetailUnitID);
                    dbServer.AddInParameter(command, "Field", DbType.String, objDischarge.FieldName);
                    dbServer.AddInParameter(command, "FieldDesc", DbType.String, objDischarge.FieldDesc);
                    dbServer.AddInParameter(command, "FieldText", DbType.String, objDischarge.FieldText);
                    dbServer.AddInParameter(command, "BindingControl", DbType.Int64, objDischarge.BindingControl);

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);

                    BizActionObj.ResultStatus = (long)dbServer.GetParameterValue(command, "ResultStatus");
                }
            }
        }

        public override IValueObject GetPatientsDischargeSummaryInfoInHTML(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO BizActionObj = valueObject as clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                DbDataReader reader;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientsDischargeSummaryInfoInHTML");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.PrintID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, BizActionObj.AdmID);
                dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, BizActionObj.AdmUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DischargeSummaryDetails == null)
                        BizActionObj.DischargeSummaryDetails = new clsIPDDischargeSummaryVO();
                    while (reader.Read())
                    {
                        BizActionObj.DischargeSummaryDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.DischargeSummaryDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizActionObj.DischargeSummaryDetails.DisDestination = Convert.ToString((reader["PatientName"]));
                        BizActionObj.DischargeSummaryDetails.Date = Convert.ToDateTime((reader["DischargeSummaryDate"]));
                        BizActionObj.DischargeSummaryDetails.TextDocument = Convert.ToString((reader["TextDocument"]));
                        BizActionObj.DischargeSummaryDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        BizActionObj.DischargeSummaryDetails.Address = Convert.ToString((reader["Address"]));
                        BizActionObj.DischargeSummaryDetails.Age = (int)(reader["Age"]);
                        BizActionObj.DischargeSummaryDetails.Gender = Convert.ToString((reader["Gender"]));
                        BizActionObj.DischargeSummaryDetails.AgeWithSex = (BizActionObj.DischargeSummaryDetails.Age + " / " + BizActionObj.DischargeSummaryDetails.Gender);  // for printing (Age/Sex)
                        BizActionObj.DischargeSummaryDetails.DoctorName = Convert.ToString((reader["DoctorName"]));
                        BizActionObj.DischargeSummaryDetails.DischargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateID"]));
                        BizActionObj.DischargeSummaryDetails.AdmDate = Convert.ToDateTime((reader["AdmissionDate"]));
                        BizActionObj.DischargeSummaryDetails.DischargeDate = Convert.ToDateTime(DALHelper.HandleDBNull((reader["DischargeDate"])));
                        BizActionObj.DischargeSummaryDetails.DischargeTime = Convert.ToDateTime(DALHelper.HandleDBNull((reader["DischargeTime"])));
                        //Added by AJ Date 16/11/2016
                        BizActionObj.DischargeSummaryDetails.Education = Convert.ToString((reader["Education"]));
                        //***//-------------------------
                        BizActionObj.DischargeSummaryDetails.UnitName = Convert.ToString((reader["ClinicName"]));
                        BizActionObj.DischargeSummaryDetails.AdressLine1 = Convert.ToString((reader["address"]));
                        BizActionObj.DischargeSummaryDetails.AddressLine2 = Convert.ToString((reader["AddressLine2"]));
                        BizActionObj.DischargeSummaryDetails.AddressLine3 = Convert.ToString((reader["AddressLine3"]));
                        BizActionObj.DischargeSummaryDetails.Email = Convert.ToString((reader["Email"]));
                        BizActionObj.DischargeSummaryDetails.PinCode = Convert.ToString((reader["PinCode"]));
                        BizActionObj.DischargeSummaryDetails.TinNo = Convert.ToString((reader["TinNo"]));
                        BizActionObj.DischargeSummaryDetails.RegNo = Convert.ToString((reader["RegNo"]));
                        BizActionObj.DischargeSummaryDetails.City = Convert.ToString((reader["City"]));
                        BizActionObj.DischargeSummaryDetails.ContactNo = Convert.ToString((reader["MobileNO"]));
                        BizActionObj.DischargeSummaryDetails.MobileNo = Convert.ToString((reader["MobNo"]));
                        BizActionObj.DischargeSummaryDetails.IPDNO = Convert.ToString((reader["IPDNO"]));
                        BizActionObj.DischargeSummaryDetails.TreatingDr = Convert.ToString((reader["TreatingDoctorName"]));

                    }
                }


                trans.Commit();
            }
            catch (Exception ex)
            {
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

        public override IValueObject FillDataGridDischargeSummaryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsFillDataGridDischargeSummaryListBizActionVO BizActionObj = valueObject as clsFillDataGridDischargeSummaryListBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
                DbDataReader reader;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillDataGridDischargeSummaryList");

                dbServer.AddInParameter(command, "FromDate", DbType.String, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.String, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "MRNO", DbType.String, BizActionObj.MrNo);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DischargeSummaryList == null)
                        BizActionObj.DischargeSummaryList = new List<clsIPDDischargeSummaryVO>();
                    while (reader.Read())
                    {
                        clsIPDDischargeSummaryVO objVO = new clsIPDDischargeSummaryVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.AdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmID"]));
                        objVO.AdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmUnitID"]));
                        objVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objVO.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["PFirstName"]));
                        objVO.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["PMiddleName"]));
                        objVO.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["PLastName"]));
                        objVO.DisDestination = objVO.FirstName + ' ' + objVO.MiddleName + ' ' + objVO.LastName;    //PatientName
                        objVO.Date = Convert.ToDateTime((reader["DischargeSummaryDate"]));
                        objVO.TextDocument = Convert.ToString((reader["TextDocument"]));
                        objVO.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objVO.IPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        objVO.DoctorName = Convert.ToString((reader["DFirstName"])) + ' ' + Convert.ToString((reader["DMiddleName"])) + ' ' + Convert.ToString((reader["DLastName"]));
                        objVO.DischargeTemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DischargeTemplateID"]));
                        objVO.TemplateName = Convert.ToString((reader["TemplateName"]));
                        objVO.AdmDate = Convert.ToDateTime((reader["AdmissionDate"]));
                        objVO.IsCancel = Convert.ToBoolean(reader["IsCancel"]);
                        BizActionObj.DischargeSummaryList.Add(objVO);

                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                trans.Commit();
            }
            catch (Exception ex)
            {
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
    }
}
