namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.EMR;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsDrugDAL : clsBaseDrugDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsDrugDAL()
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
                throw;
            }
        }

        public override IValueObject AddBPControlDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientBPControlBizActionVO nvo = valueObject as clsAddPatientBPControlBizActionVO;
            clsBPControlVO bPControlDetails = nvo.BPControlDetails;
            try
            {
                if (nvo.IsBPControl && (nvo.BPControlDetails != null))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBPControletails");
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, bPControlDetails.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, bPControlDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, bPControlDetails.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, bPControlDetails.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, bPControlDetails.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientEMRDataID", DbType.Int64, bPControlDetails.PatientEMRDataID);
                    this.dbServer.AddInParameter(storedProcCommand, "BP1", DbType.Int32, bPControlDetails.BP1);
                    this.dbServer.AddInParameter(storedProcCommand, "BP2", DbType.Int32, bPControlDetails.BP2);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, bPControlDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, bPControlDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bPControlDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.BPControlDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddCaseReferral(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCaseReferralBizActionVO nvo = valueObject as clsAddCaseReferralBizActionVO;
            try
            {
                clsCaseReferralVO caseReferralDetails = nvo.CaseReferralDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddEMR_CaseReferral");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, caseReferralDetails.LinkServer);
                if (caseReferralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, caseReferralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, caseReferralDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, caseReferralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, caseReferralDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, caseReferralDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToDoctorID", DbType.Int64, caseReferralDetails.ReferredToDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToClinicID", DbType.Int64, caseReferralDetails.ReferredToClinicID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToMobile", DbType.String, caseReferralDetails.ReferredToMobile);
                this.dbServer.AddInParameter(storedProcCommand, "ProvisionalDiagnosis", DbType.String, caseReferralDetails.ProvisionalDiagnosis);
                this.dbServer.AddInParameter(storedProcCommand, "ChiefComplaints", DbType.String, caseReferralDetails.ChiefComplaints);
                this.dbServer.AddInParameter(storedProcCommand, "Summary", DbType.String, caseReferralDetails.Summary);
                this.dbServer.AddInParameter(storedProcCommand, "Observations", DbType.String, caseReferralDetails.Observations);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, caseReferralDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, caseReferralDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, caseReferralDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.CaseReferralDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddDoctorSuggestedServiceDetailBizActionVO nvo = valueObject as clsAddDoctorSuggestedServiceDetailBizActionVO;
            try
            {
                List<clsDoctorSuggestedServiceDetailVO> doctorSuggestedServiceDetail = nvo.DoctorSuggestedServiceDetail;
                int count = doctorSuggestedServiceDetail.Count;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDeatail");
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, doctorSuggestedServiceDetail[i].LinkServer);
                        if (doctorSuggestedServiceDetail[i].LinkServer != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, doctorSuggestedServiceDetail[i].LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PatientPrescriptionID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, doctorSuggestedServiceDetail[i].ServiceID);
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, doctorSuggestedServiceDetail[i].ServiceName);
                        this.dbServer.AddInParameter(storedProcCommand, "IsOther", DbType.Boolean, doctorSuggestedServiceDetail[i].IsOther);
                        this.dbServer.AddInParameter(storedProcCommand, "Reason", DbType.String, doctorSuggestedServiceDetail[i].Reason);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorSuggestedServiceDetail[i].ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        nvo.DoctorSuggestedServiceDetail[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddGPControlDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientGPControlBizActionVO nvo = valueObject as clsAddPatientGPControlBizActionVO;
            clsGlassPowerVO gPControlDetails = nvo.GPControlDetails;
            try
            {
                if (nvo.IsGPControl && (nvo.GPControlDetails != null))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateGlassPower");
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, gPControlDetails.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, gPControlDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, gPControlDetails.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, gPControlDetails.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, gPControlDetails.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientEMRDataID", DbType.Int64, gPControlDetails.PatientEMRDataID);
                    this.dbServer.AddInParameter(storedProcCommand, "SPH1", DbType.Double, gPControlDetails.SPH1);
                    this.dbServer.AddInParameter(storedProcCommand, "CYL1", DbType.Double, gPControlDetails.CYL1);
                    this.dbServer.AddInParameter(storedProcCommand, "Axis1", DbType.Double, gPControlDetails.Axis1);
                    this.dbServer.AddInParameter(storedProcCommand, "VA1", DbType.Double, gPControlDetails.VA1);
                    this.dbServer.AddInParameter(storedProcCommand, "SPH2", DbType.Double, gPControlDetails.SPH2);
                    this.dbServer.AddInParameter(storedProcCommand, "CYL2", DbType.Double, gPControlDetails.CYL2);
                    this.dbServer.AddInParameter(storedProcCommand, "Axis2", DbType.Double, gPControlDetails.Axis2);
                    this.dbServer.AddInParameter(storedProcCommand, "VA2", DbType.Double, gPControlDetails.VA2);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, gPControlDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, gPControlDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, gPControlDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.GPControlDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddPatientPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientPrescriptionBizActionVO nvo = valueObject as clsAddPatientPrescriptionBizActionVO;
            try
            {
                clsPatientPrescriptionVO patientPrescriptionSummary = nvo.PatientPrescriptionSummary;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPrescription");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientPrescriptionSummary.LinkServer);
                if (patientPrescriptionSummary.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientPrescriptionSummary.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientPrescriptionSummary.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientPrescriptionSummary.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientPrescriptionSummary.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, patientPrescriptionSummary.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, patientPrescriptionSummary.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFrom", DbType.Int32, patientPrescriptionSummary.IsFrom);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, patientPrescriptionSummary.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientPrescriptionSummary.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientPrescriptionSummary.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientPrescriptionSummary.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (nvo.PatientPrescriptionSummary.ID != 0L)
                {
                    List<clsPatientPrescriptionDetailVO> patientPrescriptionDetail = nvo.PatientPrescriptionDetail;
                    int count = patientPrescriptionDetail.Count;
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDeatail");
                            this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientPrescriptionDetail[i].LinkServer);
                            if (patientPrescriptionDetail[i].LinkServer != null)
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientPrescriptionDetail[i].LinkServer.Replace(@"\", "_"));
                            }
                            this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, patientPrescriptionSummary.UnitID);
                            this.dbServer.AddInParameter(command2, "PrescriptionID", DbType.Int64, nvo.PatientPrescriptionSummary.ID);
                            this.dbServer.AddInParameter(command2, "DrugID", DbType.Int64, patientPrescriptionDetail[i].DrugID);
                            this.dbServer.AddInParameter(command2, "Dose", DbType.String, patientPrescriptionDetail[i].Dose);
                            this.dbServer.AddInParameter(command2, "Route", DbType.String, patientPrescriptionDetail[i].Route);
                            this.dbServer.AddInParameter(command2, "Frequency", DbType.String, patientPrescriptionDetail[i].Frequency);
                            this.dbServer.AddInParameter(command2, "Days", DbType.Int64, patientPrescriptionDetail[i].Days);
                            this.dbServer.AddInParameter(command2, "Quantity", DbType.Int64, patientPrescriptionDetail[i].Quantity);
                            this.dbServer.AddInParameter(command2, "ItemName", DbType.String, patientPrescriptionDetail[i].DrugName);
                            this.dbServer.AddInParameter(command2, "IsOther", DbType.Boolean, patientPrescriptionDetail[i].IsOther);
                            this.dbServer.AddInParameter(command2, "Reason", DbType.String, patientPrescriptionDetail[i].Reason);
                            this.dbServer.AddInParameter(command2, "PendingQuantity", DbType.Int64, patientPrescriptionDetail[i].Quantity);
                            this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientPrescriptionDetail[i].ID);
                            this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command2);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                            nvo.PatientPrescriptionDetail[i].ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddPatientPrescriptionResason(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientPrescriptionReasonOnCounterSaleBizActionVO nvo = valueObject as clsAddPatientPrescriptionReasonOnCounterSaleBizActionVO;
            try
            {
                clsPatientPrescriptionReasonOncounterSaleVO patientPrescriptionReason = nvo.PatientPrescriptionReason;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePresciptionReasonOnCounterSale");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, patientPrescriptionReason.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, patientPrescriptionReason.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionUnitID", DbType.Int64, patientPrescriptionReason.PrescriptionUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Reason", DbType.String, patientPrescriptionReason.Reason);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.String, patientPrescriptionReason.Status);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientPrescriptionReason.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientPrescriptionReason.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddPCR(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPCRBizActionVO nvo = valueObject as clsAddPCRBizActionVO;
            try
            {
                clsPCRVO pCRDetails = nvo.PCRDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddEMR_PCR");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, pCRDetails.LinkServer);
                if (pCRDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, pCRDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, pCRDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, pCRDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, pCRDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, pCRDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "ComplaintReported", DbType.String, pCRDetails.ComplaintReported);
                this.dbServer.AddInParameter(storedProcCommand, "ChiefComplaints", DbType.String, pCRDetails.ChiefComplaints);
                this.dbServer.AddInParameter(storedProcCommand, "PastHistory", DbType.String, pCRDetails.PastHistory);
                this.dbServer.AddInParameter(storedProcCommand, "DrugHistory", DbType.String, pCRDetails.DrugHistory);
                this.dbServer.AddInParameter(storedProcCommand, "Allergies", DbType.String, pCRDetails.Allergies);
                this.dbServer.AddInParameter(storedProcCommand, "Investigations", DbType.String, pCRDetails.Investigations);
                this.dbServer.AddInParameter(storedProcCommand, "PovisionalDiagnosis", DbType.String, pCRDetails.PovisionalDiagnosis);
                this.dbServer.AddInParameter(storedProcCommand, "FinalDiagnosis", DbType.String, pCRDetails.FinalDiagnosis);
                this.dbServer.AddInParameter(storedProcCommand, "Hydration", DbType.String, pCRDetails.Hydration);
                this.dbServer.AddInParameter(storedProcCommand, "IVHydration4", DbType.String, pCRDetails.IVHydration4);
                this.dbServer.AddInParameter(storedProcCommand, "ZincSupplement", DbType.String, pCRDetails.ZincSupplement);
                this.dbServer.AddInParameter(storedProcCommand, "Nutritions", DbType.String, pCRDetails.Nutritions);
                this.dbServer.AddInParameter(storedProcCommand, "AdvisoryAttached", DbType.String, pCRDetails.AdvisoryAttached);
                this.dbServer.AddInParameter(storedProcCommand, "WhenToVisitHospital", DbType.String, pCRDetails.WhenToVisitHospital);
                this.dbServer.AddInParameter(storedProcCommand, "SpecificInstructions", DbType.String, pCRDetails.SpecificInstructions);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpDate", DbType.DateTime, pCRDetails.FollowUpDate);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpAt", DbType.String, pCRDetails.FollowUpAt);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTo", DbType.String, pCRDetails.ReferralTo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, pCRDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, pCRDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, pCRDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PCRDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddVisionControlDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientVisionControlBizActionVO nvo = valueObject as clsAddPatientVisionControlBizActionVO;
            clsVisionVO visionControlDetails = nvo.VisionControlDetails;
            try
            {
                if (nvo.IsVisionControl && (nvo.VisionControlDetails != null))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateVisionDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, visionControlDetails.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, visionControlDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, visionControlDetails.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, visionControlDetails.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, visionControlDetails.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientEMRDataID", DbType.Int64, visionControlDetails.PatientEMRDataID);
                    this.dbServer.AddInParameter(storedProcCommand, "SPH1", DbType.Double, visionControlDetails.SPH);
                    this.dbServer.AddInParameter(storedProcCommand, "CYL1", DbType.Double, visionControlDetails.CYL);
                    this.dbServer.AddInParameter(storedProcCommand, "Axis1", DbType.Double, visionControlDetails.Axis);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, visionControlDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, visionControlDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, visionControlDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.VisionControlDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDrugList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDrugListBizActionVO nvo = valueObject as clsGetDrugListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDrugListByCategoryID");
                this.dbServer.AddInParameter(storedProcCommand, "TheraputicID", DbType.Int64, nvo.TheraputicID);
                this.dbServer.AddInParameter(storedProcCommand, "MoleculeID", DbType.Int64, nvo.MoleculeID);
                this.dbServer.AddInParameter(storedProcCommand, "GroupID", DbType.Int64, nvo.GroupID);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "PregnancyID", DbType.Int64, nvo.PregnancyID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objDrugList == null)
                    {
                        nvo.objDrugList = new List<clsDrugVO>();
                    }
                    while (reader.Read())
                    {
                        clsDrugVO item = new clsDrugVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            CategoryID = (long) DALHelper.HandleDBNull(reader["TherClass"])
                        };
                        nvo.objDrugList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetFrequencyList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRFrequencyBizActionVO nvo = valueObject as clsGetEMRFrequencyBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFrequencyList");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.FrequencyList == null)
                    {
                        nvo.FrequencyList = new List<FrequencyMaster>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        FrequencyMaster item = new FrequencyMaster {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Quantity = (double) DALHelper.HandleDBNull(reader["QuntityPerDay"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.FrequencyList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetItemMoleculeNameList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemMoleculeNameBizActionVO nvo = valueObject as clsGetItemMoleculeNameBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand;
                if (!nvo.isOtherDrug)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAllItemsWithMoleculeName");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_GetNonAvailableStockItemsWithMoleculeName]");
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.String, nvo.StoreID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ItemsName", DbType.String, nvo.ItemName);
                this.dbServer.AddInParameter(storedProcCommand, "MoleculeID", DbType.Int64, nvo.MoleculeID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemMoleculeDetailsList == null)
                    {
                        nvo.ItemMoleculeDetailsList = new List<clsItemMoleculeDetails>();
                    }
                    while (reader.Read())
                    {
                        clsItemMoleculeDetails item = new clsItemMoleculeDetails {
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            MoleculeName = Convert.ToString(DALHelper.HandleDBNull(reader["Molecule"])),
                            RouteID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RouteID"])),
                            Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"]))
                        };
                        nvo.ItemMoleculeDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
            }
            return nvo;
        }

        public override IValueObject GetPatientPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader;
            clsGetPatientPrescriptionBizActionVO nvo = valueObject as clsGetPatientPrescriptionBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescription");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.PatientPrescriptionSummary.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientPrescriptionSummary.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientPrescriptionSummary.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.PatientPrescriptionSummary.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.PatientPrescriptionSummary.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.PatientPrescriptionSummary.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    reader.Read();
                    nvo.PatientPrescriptionSummary.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            reader.Close();
            return valueObject;
        }

        public override IValueObject GetPatientPrescriptionDetailByVisitID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader;
            double num = 0.0;
            clsGetPatientPrescriptionDetailByVisitIDBizActionVO nvo = valueObject as clsGetPatientPrescriptionDetailByVisitIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionByVisitID");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsFrom", DbType.Int32, nvo.IsFrom);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientPrescriptionDetail == null)
                    {
                        nvo.PatientPrescriptionDetail = new List<clsPatientPrescriptionDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            nvo.TotalNewPendingQuantity = num;
                            break;
                        }
                        clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                            DrugID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                            DrugName = (string) DALHelper.HandleDBNull(reader["DrugName"]),
                            Dose = (string) DALHelper.HandleDBNull(reader["Dose"]),
                            Route = (string) DALHelper.HandleDBNull(reader["Route"]),
                            Frequency = (string) DALHelper.HandleDBNull(reader["Frequency"]),
                            Days = (int?) DALHelper.HandleDBNull(reader["Days"]),
                            Quantity = (int) DALHelper.HandleDBNull(reader["Quantity"]),
                            IsBatchRequired = (bool?) DALHelper.HandleDBNull(reader["BatchesRequired"]),
                            PendingQuantity = Convert.ToInt32(DALHelper.HandleDBNull(reader["PendingQuantity"])),
                            NewPendingQuantity = Convert.ToInt32(DALHelper.HandleDBNull(reader["NewPendingQuantity"]))
                        };
                        num += item.NewPendingQuantity;
                        item.TotalNewPendingQuantity += item.NewPendingQuantity;
                        item.UOM = (string) DALHelper.HandleDBNull(reader["UOM"]);
                        item.Manufacture = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"]));
                        item.PregnancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"]));
                        item.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));
                        item.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                        item.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));
                        item.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));
                        item.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                        item.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        item.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                        item.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                        item.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                        item.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                        item.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                        item.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        item.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));
                        item.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        item.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                        item.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                        item.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        item.SGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTTaxSale"]));
                        item.CGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTTaxSale"]));
                        item.IGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTTaxSale"]));
                        item.SGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtypeSale"]));
                        item.SGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableonSale"]));
                        item.CGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtypeSale"]));
                        item.CGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableonSale"]));
                        item.IGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtypeSale"]));
                        item.IGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableonSale"]));
                        item.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        item.Billed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BillDone"]));
                        nvo.PatientPrescriptionDetail.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            reader.Close();
            return valueObject;
        }

        public override IValueObject GetPatientPrescriptionDetailByVisitIDForPrint(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader;
            clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO nvo = valueObject as clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionDetailsForPrint");
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionDetailsID", DbType.String, nvo.SendPrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionDetailsUnitID", DbType.Int64, nvo.PatientPrescriptionDetailObj.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (nvo.PatientPrescriptionDetail == null)
                {
                    nvo.PatientPrescriptionDetail = new List<clsPatientPrescriptionDetailVO>();
                }
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                            ID = (long) DALHelper.HandleDBNull(reader["PrescriptionDetailsID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["PrescriptionDetailsUnitID"]),
                            SaleQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["SaleQuantity"]))
                        };
                        nvo.PatientPrescriptionDetail.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            reader.Close();
            return valueObject;
        }

        public override IValueObject GetPatientPrescriptionID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader;
            clsGetPrescriptionIDBizActionVO nvo = valueObject as clsGetPrescriptionIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionIDByVisitID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.PatientPrescriptionReason.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.PatientPrescriptionReason.VisitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PatientPrescriptionReason.PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]);
                        nvo.PatientPrescriptionReason.PrescriptionUnitID = (long) DALHelper.HandleDBNull(reader["PrescriptionUnitID"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            reader.Close();
            return valueObject;
        }

        public override IValueObject GetPatientPrescriptionReason(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader;
            clsGetPrescriptionReasonOnCounterSaleBizActionVO nvo = valueObject as clsGetPrescriptionReasonOnCounterSaleBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPresciptionReasonOnCounterSale");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.PatientPrescriptionReason.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PatientPrescriptionReason.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionUnitID", DbType.Int64, nvo.PatientPrescriptionReason.PrescriptionUnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientPrescriptionReasonList == null)
                    {
                        nvo.PatientPrescriptionReasonList = new List<clsPatientPrescriptionReasonOncounterSaleVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientPrescriptionReasonOncounterSaleVO item = new clsPatientPrescriptionReasonOncounterSaleVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                            PrescriptionUnitID = (long) DALHelper.HandleDBNull(reader["PrescriptionUnitID"]),
                            Reason = (string) DALHelper.HandleDBNull(reader["Reason"]),
                            AddedDateTime = (DateTime?) DALHelper.HandleDBNull(reader["AddedDateTime"])
                        };
                        nvo.PatientPrescriptionReasonList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            reader.Close();
            return valueObject;
        }

        public override IValueObject GetPatientVital(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientVitalBizActionVO nvo = valueObject as clsGetPatientVitalBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientVital");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VitalDetails == null)
                    {
                        nvo.VitalDetails = new List<clsEMRVitalsVO>();
                    }
                    while (reader.Read())
                    {
                        clsEMRVitalsVO item = new clsEMRVitalsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["VitalID"]),
                            PatientVitalID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Vital"]),
                            Unit = (string) DALHelper.HandleDBNull(reader["Unit"]),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            Time = (DateTime?) DALHelper.HandleDBNull(reader["Time"]),
                            Value = (double) DALHelper.HandleDBNull(reader["Value"])
                        };
                        nvo.VitalDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject UpdateCaseReferral(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateCaseReferralBizActionVO nvo = valueObject as clsUpdateCaseReferralBizActionVO;
            try
            {
                clsCaseReferralVO caseReferralDetails = nvo.CaseReferralDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEMR_CaseReferral");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, caseReferralDetails.LinkServer);
                if (caseReferralDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, caseReferralDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, caseReferralDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, caseReferralDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, caseReferralDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, caseReferralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, caseReferralDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToDoctorID", DbType.Int64, caseReferralDetails.ReferredToDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToClinicID", DbType.Int64, caseReferralDetails.ReferredToClinicID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferredToMobile", DbType.String, caseReferralDetails.ReferredToMobile);
                this.dbServer.AddInParameter(storedProcCommand, "ProvisionalDiagnosis", DbType.String, caseReferralDetails.ProvisionalDiagnosis);
                this.dbServer.AddInParameter(storedProcCommand, "ChiefComplaints", DbType.String, caseReferralDetails.ChiefComplaints);
                this.dbServer.AddInParameter(storedProcCommand, "Summary", DbType.String, caseReferralDetails.Summary);
                this.dbServer.AddInParameter(storedProcCommand, "Observations", DbType.String, caseReferralDetails.Observations);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, caseReferralDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, caseReferralDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdateDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateDoctorSuggestedServiceBizActionVO nvo = valueObject as clsUpdateDoctorSuggestedServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorSuggestedServiceDeatail");
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                List<clsDoctorSuggestedServiceDetailVO> doctorSuggestedServiceDetail = nvo.DoctorSuggestedServiceDetail;
                int count = doctorSuggestedServiceDetail.Count;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDeatail");
                        this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, doctorSuggestedServiceDetail[i].LinkServer);
                        if (doctorSuggestedServiceDetail[i].LinkServer != null)
                        {
                            this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, doctorSuggestedServiceDetail[i].LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(command2, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                        this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, doctorSuggestedServiceDetail[i].ServiceID);
                        this.dbServer.AddInParameter(command2, "ServiceName", DbType.String, doctorSuggestedServiceDetail[i].ServiceName);
                        this.dbServer.AddInParameter(command2, "IsOther", DbType.Boolean, doctorSuggestedServiceDetail[i].IsOther);
                        this.dbServer.AddInParameter(command2, "Reason", DbType.String, doctorSuggestedServiceDetail[i].Reason);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, doctorSuggestedServiceDetail[i].ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        nvo.DoctorSuggestedServiceDetail[i].ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdatePatientPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientPrescriptionBizActionVO nvo = valueObject as clsUpdatePatientPrescriptionBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientPrescriptionDeatail");
                this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                List<clsPatientPrescriptionDetailVO> patientPrescriptionDetail = nvo.PatientPrescriptionDetail;
                int count = patientPrescriptionDetail.Count;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDeatail");
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientPrescriptionDetail[i].LinkServer);
                        if (patientPrescriptionDetail[i].LinkServer != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientPrescriptionDetail[i].LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(command2, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                        this.dbServer.AddInParameter(command2, "DrugID", DbType.Int64, patientPrescriptionDetail[i].DrugID);
                        this.dbServer.AddInParameter(command2, "Dose", DbType.String, patientPrescriptionDetail[i].Dose);
                        this.dbServer.AddInParameter(command2, "Route", DbType.String, patientPrescriptionDetail[i].Route);
                        this.dbServer.AddInParameter(command2, "Frequency", DbType.String, patientPrescriptionDetail[i].Frequency);
                        this.dbServer.AddInParameter(command2, "Days", DbType.Int64, patientPrescriptionDetail[i].Days);
                        this.dbServer.AddInParameter(command2, "Quantity", DbType.Int64, patientPrescriptionDetail[i].Quantity);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(command2, "ItemName", DbType.String, patientPrescriptionDetail[i].DrugName);
                        this.dbServer.AddInParameter(command2, "IsOther", DbType.Boolean, patientPrescriptionDetail[i].IsOther);
                        this.dbServer.AddInParameter(command2, "Reason", DbType.String, patientPrescriptionDetail[i].Reason);
                        this.dbServer.AddInParameter(command2, "PendingQuantity", DbType.Int64, patientPrescriptionDetail[i].Quantity);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientPrescriptionDetail[i].ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        nvo.PatientPrescriptionDetail[i].ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdatePCR(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePCRBizActionVO nvo = valueObject as clsUpdatePCRBizActionVO;
            try
            {
                clsPCRVO pCRDetails = nvo.PCRDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEMR_PCR");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, pCRDetails.LinkServer);
                if (pCRDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, pCRDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, pCRDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, pCRDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, pCRDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, pCRDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, pCRDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ComplaintReported", DbType.String, pCRDetails.ComplaintReported);
                this.dbServer.AddInParameter(storedProcCommand, "ChiefComplaints", DbType.String, pCRDetails.ChiefComplaints);
                this.dbServer.AddInParameter(storedProcCommand, "PastHistory", DbType.String, pCRDetails.PastHistory);
                this.dbServer.AddInParameter(storedProcCommand, "DrugHistory", DbType.String, pCRDetails.DrugHistory);
                this.dbServer.AddInParameter(storedProcCommand, "Allergies", DbType.String, pCRDetails.Allergies);
                this.dbServer.AddInParameter(storedProcCommand, "Investigations", DbType.String, pCRDetails.Investigations);
                this.dbServer.AddInParameter(storedProcCommand, "PovisionalDiagnosis", DbType.String, pCRDetails.PovisionalDiagnosis);
                this.dbServer.AddInParameter(storedProcCommand, "FinalDiagnosis", DbType.String, pCRDetails.FinalDiagnosis);
                this.dbServer.AddInParameter(storedProcCommand, "Hydration", DbType.String, pCRDetails.Hydration);
                this.dbServer.AddInParameter(storedProcCommand, "IVHydration4", DbType.String, pCRDetails.IVHydration4);
                this.dbServer.AddInParameter(storedProcCommand, "ZincSupplement", DbType.String, pCRDetails.ZincSupplement);
                this.dbServer.AddInParameter(storedProcCommand, "Nutritions", DbType.String, pCRDetails.Nutritions);
                this.dbServer.AddInParameter(storedProcCommand, "AdvisoryAttached", DbType.String, pCRDetails.AdvisoryAttached);
                this.dbServer.AddInParameter(storedProcCommand, "WhenToVisitHospital", DbType.String, pCRDetails.WhenToVisitHospital);
                this.dbServer.AddInParameter(storedProcCommand, "SpecificInstructions", DbType.String, pCRDetails.SpecificInstructions);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpDate", DbType.DateTime, pCRDetails.FollowUpDate);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpAt", DbType.String, pCRDetails.FollowUpAt);
                this.dbServer.AddInParameter(storedProcCommand, "ReferralTo", DbType.String, pCRDetails.ReferralTo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, pCRDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, pCRDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }
    }
}

