using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsAddPatientPrescriptionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientPrescriptionBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPatientPrescriptionVO objPatientPrescription = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public clsPatientPrescriptionVO PatientPrescriptionSummary
        {
            get { return objPatientPrescription; }
            set { objPatientPrescription = value; }
        }

        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetail
        {
            get { return objPatientPrescriptionDetail; }
            set { objPatientPrescriptionDetail = value; }
        }


    }

    //added by neena
    public class clsAddPatientPrescriptionReasonOnCounterSaleBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientPrescriptionReasonOnCounterSaleBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPatientPrescriptionReasonOncounterSaleVO _PatientPrescriptionReason = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public clsPatientPrescriptionReasonOncounterSaleVO PatientPrescriptionReason
        {
            get { return _PatientPrescriptionReason; }
            set { _PatientPrescriptionReason = value; }
        }

        private List<clsPatientPrescriptionReasonOncounterSaleVO> _PatientPrescriptionReasonList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionReasonOncounterSaleVO> PatientPrescriptionReasonList
        {
            get { return _PatientPrescriptionReasonList; }
            set { _PatientPrescriptionReasonList = value; }
        }


    }
    //

    public class clsAddDoctorSuggestedServiceDetailBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddDoctorSuggestedServiceDetailBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _objPrescriptionID;
        public long PatientPrescriptionID
        {
            get { return _objPrescriptionID; }
            set { _objPrescriptionID = value; }
        }

        public long UnitID { get; set; }

        private List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsDoctorSuggestedServiceDetailVO> DoctorSuggestedServiceDetail
        {
            get { return objDoctorSuggestedServiceDetail; }
            set { objDoctorSuggestedServiceDetail = value; }
        }


    }

    public class FrequencyMaster : NotificationModel
    {
        public FrequencyMaster()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public FrequencyMaster(long Id, string Description)
        {
            this.ID = Id;
            this.Description = Description;
        }

        /// <summary>
        /// MasterListItem
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public FrequencyMaster(long Id, string Description, double Quantity)
        {
            this.ID = Id;
            this.Description = Description;
            this.Quantity = Quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public FrequencyMaster(long Id, string Description, bool Status)
        {
            this.ID = Id;
            this.Description = Description;
            this.Status = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Code">Code</param>
        /// <param name="Description">Field Description</param>
        /// <param name="Status">Record Status</param>
        public FrequencyMaster(long Id, string Code, string Description, bool Status, double Quantity)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;

        }

        private long _Id;
        private string _Code;
        private string _Description = string.Empty;
        private string _PrintDescription = string.Empty;
        private bool _Status = false;
        public bool isChecked { get; set; }
        private double _Quantity;


        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Id To Represent Record.
        /// </summary>
        public long ID { get { return _Id; } set { _Id = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Code Of Record.
        /// </summary>
        public string Code { get { return _Code; } set { _Code = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Description.
        /// </summary>
        public string Description { get { return _Description; } set { _Description = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Description.
        /// </summary>
        public string PrintDescription { get { return _PrintDescription; } set { _PrintDescription = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Status Of Record.
        /// </summary>
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public double Quantity { get { return _Quantity; } set { _Quantity = value; } }

        public override string ToString()
        {
            return this.Description;
        }

    }

    public class clsAddPatientBPControlBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientBPControlBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public bool IsBPControl { get; set; }

        private clsBPControlVO objDetail = null;
        public clsBPControlVO BPControlDetails
        {
            get { return objDetail; }
            set { objDetail = value; }
        }

    }

    public class clsAddPatientVisionControlBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientVisionControlBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public bool IsVisionControl { get; set; }

        private clsVisionVO objDetail = null;
        public clsVisionVO VisionControlDetails
        {
            get { return objDetail; }
            set { objDetail = value; }
        }
    }

    public class clsAddPatientGPControlBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientGPControlBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public bool IsGPControl { get; set; }

        private clsGlassPowerVO objDetail = null;
        public clsGlassPowerVO GPControlDetails
        {
            get { return objDetail; }
            set { objDetail = value; }
        }
    }
    //
    public class clsAddPatientVitalDetailBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientVitalDetailBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public bool IsUpdate { get; set; }

        public long VisitID { get; set; }

        public long TemplateID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }
        public long DoctorID { get; set; }
        private List<clsEMRVitalsVO> objDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsEMRVitalsVO> VitalList
        {
            get { return objDetail; }
            set { objDetail = value; }
        }


    }

    public class clsGetEMRMedicationUserControlBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetEMRMedicationUserControlBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }

        public long TemplateID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }

        private clsPatientPrescriptionVO objPatientPrescription = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public clsPatientPrescriptionVO PatientPrescription
        {
            get { return objPatientPrescription; }
            set { objPatientPrescription = value; }
        }



        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionlist = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetailList
        {
            get { return objPatientPrescriptionlist; }
            set { objPatientPrescriptionlist = value; }
        }

        private List<clsPatientPrescriptionDetailVO> objPatientCurrentMedicationDetail = null;
        public List<clsPatientPrescriptionDetailVO> PatientCurrentMedicationDetailList
        {
            get { return objPatientCurrentMedicationDetail; }
            set { objPatientCurrentMedicationDetail = value; }
        }
        private List<clsDoctorSuggestedServiceDetailVO> objServiceList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsDoctorSuggestedServiceDetailVO> ServiceList
        {
            get { return objServiceList; }
            set { objServiceList = value; }
        }



    }

    public class clsGetVitalMasterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetVitalMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }

        public long TemplateID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        private List<clsEMRVitalsVO> objVital = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsEMRVitalsVO> VitalDetails
        {
            get { return objVital; }
            set { objVital = value; }
        }



        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionlist = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetailList
        {
            get { return objPatientPrescriptionlist; }
            set { objPatientPrescriptionlist = value; }
        }

        private List<clsDoctorSuggestedServiceDetailVO> objServiceList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsDoctorSuggestedServiceDetailVO> ServiceList
        {
            get { return objServiceList; }
            set { objServiceList = value; }
        }



    }

    public class clsGetPatientVitalBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientVitalBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }
        public long UnitID { get; set; }

        public long TemplateID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public Boolean IsOPDIPD { get; set; }

        private int _TotalRows;
        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }
        private int _StartRowIndex = 0;

        public int StartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }
        private int _MaximumRows = 10;

        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        private bool _PagingEnabled;

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }
        private List<clsEMRVitalsVO> objVital = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsEMRVitalsVO> VitalDetails
        {
            get { return objVital; }
            set { objVital = value; }
        }


    }

    public class clsGetDiagnosisMasterBizactionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDiagnosisMasterBizaction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
        private List<clsEMRDiagnosisVO> _DiagnosisDetails;
        public List<clsEMRDiagnosisVO> DiagnosisDetails
        {
            get
            {

                return _DiagnosisDetails;
            }

            set
            {
                _DiagnosisDetails = value;

            }

        }

        public long UnitID { get; set; }
        public string Diagnosis { get; set; }

        public string Code { get; set; }
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

    }

    public class clsAddDiagnosisDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddDiagnosisDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        public bool IsUpdate { get; set; }

        public long VisitID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }

        public long PatientDiagnosisID { get; set; }

        private List<clsEMRAddDiagnosisVO> _objDiagnosisDetails = null;

        public List<clsEMRAddDiagnosisVO> DiagnosisDetails
        {
            get { return _objDiagnosisDetails; }
            set { _objDiagnosisDetails = value; }
        }


    }

    public class clsGetPatientEMRDiagnosisDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientEMRDiagnosisDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }

        private List<clsEMRAddDiagnosisVO> objPatientDiagnosis = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsEMRAddDiagnosisVO> PatientDiagnosisDetails
        {
            get { return objPatientDiagnosis; }
            set { objPatientDiagnosis = value; }
        }

    }

    public class clsAddUpdatePatientAllergiesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientAllergiesBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long AllergyID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public Boolean OPDIPD { get; set; }
        public long UnitID { get; set; }
        public long DoctorID { get; set; }
        public string DoctorCode { get; set; }
        public clsEMRAllergiesVO CurrentAllergies { get; set; }
        private List<clsGetDrugForAllergies> DrugAllergieslist = null;
        public List<clsGetDrugForAllergies> DrugAllergies
        {
             get { return DrugAllergieslist; }
            set { DrugAllergieslist = value; }
        }
    }

    public class clsGetPatientAllergiesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientAllergiesBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }
        public long DoctorID { get; set; }

        public clsEMRAllergiesVO CurrentAllergy { get; set; }

        private List<clsEMRAllergiesVO> objDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientAllergies Which is Added.
        /// </summary>
        public List<clsEMRAllergiesVO> AllergiesList
        {
            get { return objDetail; }
            set { objDetail = value; }
        }
    }

    public class clsGetPatientDrugAllergiesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientDrugAllergiesBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        public long PatientID { get; set; }
        private List<clsGetDrugForAllergies> PatientDrugAllergiesList = null;
        public List<clsGetDrugForAllergies> DrugAllergiesList
        {
            get { return PatientDrugAllergiesList; }
            set { PatientDrugAllergiesList = value; }
        }
    }

    public class clsGetPatientChiefComplaintsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientChiefComplaintsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public long UnitID { get; set; }
        public long DoctorID { get; set; }
        public string DoctorCode { get; set; }

        public clsEMRChiefComplaintsVO CurrentChiefComplaints { get; set; }

        private List<clsEMRChiefComplaintsVO> _ChiefComplaintList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientAllergies Which is Added.
        /// </summary>
        public List<clsEMRChiefComplaintsVO> ChiefComplaintList
        {
            get { return _ChiefComplaintList; }
            set { _ChiefComplaintList = value; }
        }
    }

    public class clsAddUpdatePatientChiefComplaintsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientChiefComplaintsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long DoctorID { get; set; }
        public string DoctorCode { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public long UnitID { get; set; }
        public clsEMRChiefComplaintsVO CurrentChiefComplaints { get; set; }
    }

    public class clsAddUpdatePatientInvestigationsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientInvestigationsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long PatientID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public Boolean IsOtherServices { get; set; }
        public long PatientUnitID { get; set; }
        public Boolean FlagForAddUpdate { get; set; }
        public bool IsOPDIPD { get; set; }
        public long UnitID { get; set; }
        private List<clsDoctorSuggestedServiceDetailVO> _InvestigationList;
        public List<clsDoctorSuggestedServiceDetailVO> InvestigationList
        {
            get { return _InvestigationList; }
            set { _InvestigationList = value; }
        }
    }

    public class clsAddUpdatePatientCurrentMedicationsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientCurrentMedicationsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public bool IsOPDIPD { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        // Flag to define Add or Update transaction
        // For Add = 0
        // For Update = 1

        private List<clsPatientPrescriptionDetailVO> lstPatientCurrentMedicationDetail = null;
        public List<clsPatientPrescriptionDetailVO> PatientCurrentMedicationDetailList
        {
            get { return lstPatientCurrentMedicationDetail; }
            set { lstPatientCurrentMedicationDetail = value; }
        }

    }

    public class ClsGetPatientdrugAllergiesListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.ClsGetPatientdrugAllergiesListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long PatientID { get; set; }
        public long DrugID { get; set; }

        private List<clsGetDrugForAllergies> PatientDrugAllergiesList = null;
        public List<clsGetDrugForAllergies> DrugAllergiesList
        {
            get { return PatientDrugAllergiesList; }
            set { PatientDrugAllergiesList = value; }
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

    }

    public class clsAddUpdatePatientFollowupNotesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientFollowupNotesBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long DoctorID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public long UnitID { get; set; }
        public clsEMRFollowNoteVO CurrentFollowUpNotes { get; set; }
    }

    public class clsAddUpdatePatientcostNoteBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddUpdatePatientCostNoteBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long DoctorID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public long UnitID { get; set; }
        public clsEMRFollowNoteVO CurrentFollowUpNotes { get; set; }
    }

    public class clsGetPatientFollowUpNoteBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientFollowUpNoteBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public long UnitID { get; set; }
        public long DoctorID { get; set; }

        public clsEMRFollowNoteVO CurrentFollowUPNotes { get; set; }
        private List<clsEMRFollowNoteVO> _FollowUpNotesList = null;
        public List<clsEMRFollowNoteVO> FollowUpNotesList
        {
            get { return _FollowUpNotesList; }
            set { _FollowUpNotesList = value; }
        }
    }

    public class clsGetPatientCostBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientcostBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long VisitID { get; set; }
        public long VisitUnitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public long UnitID { get; set; }
        public long DoctorID { get; set; }

        public clsEMRFollowNoteVO CurrentFollowUPNotes { get; set; }
        private List<clsEMRFollowNoteVO> _FollowUpNotesList = null;
        public List<clsEMRFollowNoteVO> FollowUpNotesList
        {
            get { return _FollowUpNotesList; }
            set { _FollowUpNotesList = value; }
        }
    }
}
