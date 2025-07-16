
//Created Date:23/July/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Add and Update the Patient EMR CPOE Detail

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using PalashDynamics.ValueObjects.CompoundDrug;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsAddUpdatePatientCPOEDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsAddUpdatePatientCPOEDetailsBizAction";
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
        public long DoctorID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public long DepartmentID { get; set; }
        public string Advice { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string FollowUpRemark { get; set; }
        public long FollowUpID { get; set; }
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
        public bool FalgForAddUpdate { get; set; }

        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetailList
        {
            get { return objPatientPrescriptionDetail; }
            set { objPatientPrescriptionDetail = value; }
        }

        private List<clsDoctorSuggestedServiceDetailVO> objPatientServiceDetailDetail = null;
        public List<clsDoctorSuggestedServiceDetailVO> PatientServiceDetailDetailList
        {
            get { return objPatientServiceDetailDetail; }
            set { objPatientServiceDetailDetail = value; }
        }
    }

    public class clsGetPatientCPOEDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientCPOEDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long PrescriptionID { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public long DoctorID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public string DoctorCode { get; set; }
        public bool IsOPDIPD { get; set; }

        public string Advice { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string FollowUpRemark { get; set; }

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
        public bool FalgForAddUpdate { get; set; }

        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetailList
        {
            get { return objPatientPrescriptionDetail; }
            set { objPatientPrescriptionDetail = value; }
        }

        private List<clsDoctorSuggestedServiceDetailVO> objPatientServiceDetailDetail = null;
        public List<clsDoctorSuggestedServiceDetailVO> PatientServiceDetailDetailList
        {
            get { return objPatientServiceDetailDetail; }
            set { objPatientServiceDetailDetail = value; }
        }
    }

    public class clsAddUpdateCompoundPrescriptionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsAddUpdateCompoundPrescriptionBizAction";
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
        public long DoctorID { get; set; }
        public bool IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public string DepartmentCode { get; set; }
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

        private List<clsCompoundDrugMasterVO> _CompoundDrugMaster = new List<clsCompoundDrugMasterVO>();
        public List<clsCompoundDrugMasterVO> CompoundDrugMaster
        {
            get { return _CompoundDrugMaster; }
            set { _CompoundDrugMaster = value; }
        }

        private List<clsPatientCompoundPrescriptionVO> CoumpoundDrug = new List<clsPatientCompoundPrescriptionVO>();

        public List<clsPatientCompoundPrescriptionVO> CoumpoundDrugList
        {
            get { return CoumpoundDrug; }
            set { CoumpoundDrug = value; }
        }
    }

    public class clsGetCompoundPrescriptionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetCompoundPrescriptionBizAction";
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
        public long DoctorID { get; set; }
        public string DoctorCode { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public string DepartmentCode { get; set; }
        public bool IsOPDIPD { get; set; }
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

        private List<clsCompoundDrugMasterVO> _CompoundDrugMaster = new List<clsCompoundDrugMasterVO>();
        public List<clsCompoundDrugMasterVO> CompoundDrugMaster
        {
            get { return _CompoundDrugMaster; }
            set { _CompoundDrugMaster = value; }
        }

        private List<clsPatientPrescriptionDetailVO> CoumpoundDrug = new List<clsPatientPrescriptionDetailVO>();

        public List<clsPatientPrescriptionDetailVO> CoumpoundDrugList
        {
            get { return CoumpoundDrug; }
            set { CoumpoundDrug = value; }
        }
    }

    public class clsGetCPOEServicItemDiagnosisWiseBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetCPOEServicItemDiagnosisWiseBizAction";
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
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public string DiagnosisIds { get; set; }
        public long TemplateID { get; set; }


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
        public bool FalgForAddUpdate { get; set; }

        private List<clsGetServiceBySelectedDiagnosisVO> objServiceBySelectedDiagnosisDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsGetServiceBySelectedDiagnosisVO> ServiceBySelectedDiagnosisDetailList
        {
            get { return objServiceBySelectedDiagnosisDetail; }
            set { objServiceBySelectedDiagnosisDetail = value; }
        }

        private List<clsGetItemBySelectedDiagnosisVO> objItemBySelectedDiagnosisDetail = null;
        public List<clsGetItemBySelectedDiagnosisVO> ItemBySelectedDiagnosisDetailList
        {
            get { return objItemBySelectedDiagnosisDetail; }
            set { objItemBySelectedDiagnosisDetail = value; }
        }
    }

    public class clsDeleteCPOEServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsDeleteCPOEServiceBizAction";
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
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public long ServiceID { get; set; }
        public long PrescriptionID { get; set; }
        public long ID { get; set; }

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

    public class clsDeleteCPOEMedicineBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsDeleteCPOEMedicineBizAction";
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
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public long DrugID { get; set; }
        public long PrescriptionID { get; set; }
        public long ID { get; set; }

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

    public class clsGetServiceCPOEDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetServiceCPOEDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long PrescriptionID { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public long DoctorID { get; set; }
        public string Advice { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string FollowUpRemark { get; set; }
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
        public bool FalgForAddUpdate { get; set; }

        private List<clsDoctorSuggestedServiceDetailVO> objPatientServiceDetailDetail = null;
        public List<clsDoctorSuggestedServiceDetailVO> PatientServiceDetailDetailList
        {
            get { return objPatientServiceDetailDetail; }
            set { objPatientServiceDetailDetail = value; }
        }

        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetailList = null;
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetailList
        {
            get { return objPatientPrescriptionDetailList; }
            set { objPatientPrescriptionDetailList = value; }
        }

    }

    public class clsGetPreviousCPOEServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPreviousCPOEServiceBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public bool Status { get; set; }
        public long StartIndex { get; set; }
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

        private List<clsDoctorSuggestedServiceDetailVO> objPreviousServiceDetails = null;
        public List<clsDoctorSuggestedServiceDetailVO> PreviousServiceDetailsList
        {
            get { return objPreviousServiceDetails; }
            set { objPreviousServiceDetails = value; }
        }
    }

    public class clsAddUpdateServicesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsAddUpdateServicesBizAction";
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
        public long DoctorID { get; set; }
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
        public bool FalgForAddUpdate { get; set; }

        private List<clsDoctorSuggestedServiceDetailVO> objOtherServiceDetails = null;
        public List<clsDoctorSuggestedServiceDetailVO> ServiceDetails
        {
            get { return objOtherServiceDetails; }
            set { objOtherServiceDetails = value; }
        }
    }

    public class clsAddUpdateFollowUpDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsAddUpdateFollowUpDetailsBizAction";
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
        public string Advice { get; set; }
        public bool IsOPDIPD { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string FollowUpRemark { get; set; }
        public long FollowUpID { get; set; }
        public long DepartmentID { get; set; }
        public string DepartmentCode { get; set; }
        public long DoctorID { get; set; }
        public string DoctorCode { get; set; }
        public long SuccessStatus { get; set; }
        public Boolean FolloWUPRequired { get; set; }
        public long AppoinmentReson { get; set; }
        public long UnitID { get; set; }        // Added on 08032017 for Patient FollwUp List on Dashboard

        //***//---------
        public bool ISFollowUpNewQueueList { get; set; }

    }

    public class clsGetPatientFollowUpDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientFollowUpDetailsBizAction";
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
        public string FollowUpAdvice { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string FollowUpRemark { get; set; }
        public long FollowUpID { get; set; }
        public long PrescriptionID { get; set; }
        public long DoctorID { get; set; }
        public Boolean Isopdipd { get; set; }
        public Boolean NoFollowReq { get; set; }
        public long AppoinmentReson { get; set; }

        #region To Get FollowUpList on Dashboard 08032017

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long? UnitId { get; set; }
        public long? DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string MrNo { get; set; }

        private bool _PagingEnabled;
        public bool InputPagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private int _StartRowIndex = 0;
        public int InputStartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _MaximumRows = 10;
        public int InputMaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        private List<clsFollowUpVO> myVar = new List<clsFollowUpVO>();

        public List<clsFollowUpVO> FollowUpDetailsList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public bool IsFromDashBoard { get; set; }

        private int _TotalRows = 0;

        public int OutputTotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        #endregion

    }

    #region To Get FollowUpList on Dashboard 08032017

    public class clsFollowUpVO : INotifyPropertyChanged, IValueObject
    {
        #region Members

        private long lngFollowUpID;
        public long FollowUpID
        {
            get
            {
                return lngFollowUpID;
            }
            set
            {
                lngFollowUpID = value;
            }
        }

        private DateTime? dtpFollowUpDate;
        public DateTime? FollowUpDate
        {
            get { return dtpFollowUpDate; }
            set
            {
                if (value != dtpFollowUpDate)
                {
                    dtpFollowUpDate = value;
                    OnPropertyChanged("FollowUpDate");
                }
            }
        }

        private long lngPatientId;
        public long PatientId
        {
            get { return lngPatientId; }
            set
            {
                if (value != lngPatientId)
                {
                    lngPatientId = value;
                    OnPropertyChanged("PatientId");
                }
            }
        }

        private long lngPatientUnitId;
        public long PatientUnitId
        {
            get { return lngPatientUnitId; }
            set
            {
                if (value != lngPatientUnitId)
                {
                    lngPatientUnitId = value;
                    OnPropertyChanged("PatientUnitId");
                }
            }
        }

        private string strFirstName = "";
        public string FirstName
        {
            get { return strFirstName; }
            set
            {
                if (value != strFirstName)
                {

                    strFirstName = value;
                    OnPropertyChanged("FirstName");


                }

            }
        }

        private string strMiddleName = "";

        public string MiddleName
        {
            get { return strMiddleName; }
            set
            {
                if (value != strMiddleName)
                {
                    strMiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string strLastName = "";
        public string LastName
        {
            get { return strLastName; }
            set
            {
                if (value != strLastName)
                {
                    strLastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private string strFamilyName = "";
        public string FamilyName
        {
            get { return strFamilyName; }
            set
            {
                if (value != strFamilyName)
                {

                    strFamilyName = value;
                    OnPropertyChanged("FamilyName");
                }
            }
        }

        private string strContactNo1 = "";
        public string ContactNo1
        {
            get { return strContactNo1; }
            set
            {
                if (value != strContactNo1)
                {
                    strContactNo1 = value;
                    OnPropertyChanged("ContactNo1");
                }
            }
        }

        private string strContactNo2 = "";
        public string ContactNo2
        {
            get { return strContactNo2; }
            set
            {
                if (value != strContactNo2)
                {
                    strContactNo2 = value;
                    OnPropertyChanged("ContactNo2");
                }
            }
        }

        private string _MobileCountryCode;
        public string MobileCountryCode
        {
            get { return _MobileCountryCode; }
            set
            {
                if (_MobileCountryCode != value)
                {
                    _MobileCountryCode = value;
                    OnPropertyChanged("MobileCountryCode");
                }
            }
        }

        private long _ResiNoCountryCode;
        public long ResiNoCountryCode
        {
            get { return _ResiNoCountryCode; }
            set
            {
                if (_ResiNoCountryCode != value)
                {
                    _ResiNoCountryCode = value;
                    OnPropertyChanged("ResiNoCountryCode");
                }
            }
        }

        private long _ResiSTDCode;
        public long ResiSTDCode
        {
            get { return _ResiSTDCode; }
            set
            {
                if (_ResiSTDCode != value)
                {
                    _ResiSTDCode = value;
                    OnPropertyChanged("ResiSTDCode");
                }
            }
        }

        private string strFaxNo = "";
        public string FaxNo
        {
            get { return strFaxNo; }
            set
            {
                if (value != strFaxNo)
                {
                    strFaxNo = value;
                    OnPropertyChanged("FaxNo");
                }
            }
        }

        private string strEmail = "";
        public string Email
        {
            get { return strEmail; }
            set
            {
                if (value != strEmail)
                {
                    strEmail = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        private long lngUnitId;
        public long UnitId
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private long lngDepartmentId;
        public long DepartmentId
        {
            get { return lngDepartmentId; }
            set
            {
                if (value != lngDepartmentId)
                {
                    lngDepartmentId = value;
                    OnPropertyChanged("DepartmentId");
                }
            }
        }

        public string UnitName { get; set; }

        private long lngAppointmentReasonId;
        public long AppointmentReasonId
        {
            get { return lngAppointmentReasonId; }
            set
            {
                if (value != lngAppointmentReasonId)
                {
                    lngAppointmentReasonId = value;
                    OnPropertyChanged("AppointmentReasonId");
                }
            }
        }
        private string _AppointmentReason;
        public string AppointmentReason
        {
            get { return _AppointmentReason; }
            set
            {
                if (value != _AppointmentReason)
                {
                    _AppointmentReason = value;
                    OnPropertyChanged("AppointmentReason");
                }
            }
        }

        private string strRemark = "";
        public string Remark
        {
            get { return strRemark; }
            set
            {
                if (value != strRemark)
                {
                    strRemark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _RegRemark = " ";
        public string RegRemark
        {
            get { return _RegRemark; }
            set
            {
                if (value != _RegRemark)
                {

                    _RegRemark = value;
                    OnPropertyChanged("RegRemark");


                }

            }
        }

        private string strDoctorName = "";
        public string DoctorName
        {
            get { return strDoctorName; }
            set
            {
                if (value != strDoctorName)
                {
                    strDoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        public string Description { get; set; }

        private long lngDoctorId;
        public long DoctorId
        {
            get { return lngDoctorId; }
            set
            {
                if (value != lngDoctorId)
                {
                    lngDoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }

        public string _MrNo = "";
        public string MrNo
        {
            get { return _MrNo; }
            set { _MrNo = value; }
        }

        public DateTime? AddedDateTime { get; set; } 
        public DateTime? UpdatedDateTime { get; set; }
        public string createdByName { get; set; }
        public string ModifiedByName { get; set; }
        public bool IsAge { get; set; }

        private DateTime? _DateOfBirthFromAge = null;
        public DateTime? DateOfBirthFromAge
        {
            get
            {
                return _DateOfBirthFromAge;
            }
            set
            {
                if (value != _DateOfBirthFromAge)
                {

                    _DateOfBirthFromAge = value;
                    OnPropertyChanged("DateOfBirthFromAge");
                }
            }
        }
        //[Required(ErrorMessage = "Birth date is Required")]
        private DateTime? dtpDOB;
        public DateTime? DOB
        {
            get { return dtpDOB; }
            set
            {
                if (value != dtpDOB)
                {
                    dtpDOB = value;

                    //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "DOB" });
                    OnPropertyChanged("DOB");
                }
            }
        }

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = strFirstName + " " + strMiddleName + " " + strLastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        //***//
        private string _OPDNo;
        public string OPDNo
        {
            get { return _OPDNo; }
            set
            {
                if (_OPDNo != value)
                {
                    _OPDNo = value;
                    OnPropertyChanged("OPDNo");
                }
            }
        }
       
        private DateTime? _VisitDate;
        public DateTime? VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (value != _VisitDate)
                {
                    _VisitDate = value;
                    OnPropertyChanged("VisitDate");
                }
            }
        }

        public long VisitID { get; set; }

        public bool ISAppointment { get; set; } //***//
    }



    #endregion

    public class clsChiefComplaintsVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private int _ComplaintStatus;

        public int ComplaintStatus
        {
            get
            {
                return _ComplaintStatus;
            }
            set
            {
                value = _ComplaintStatus;
                OnPropertyChanged("ComplaintStatus");
            }
        }

        public DateTime VisitDate { get; set; }

        public long VisitID { get; set; }
        public long DoctorID { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpec { get; set; }
        public long UnitID { get; set; }
        public string Datetime { get; set; }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
    public class clsGetPatientPastChiefComplaintsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPastChiefComplaintsBizAction";
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
        public long DoctorID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public List<clsChiefComplaintsVO> ChiefComplaintList { get; set; }
    }

    public class clsGetPatientCurrentServicesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientCurrentServicesBizAction";
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
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public int _SuccessStatus;
        public bool IsOtherServices { get; set; }
        public bool IsOPDIPD { get; set; }
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsDoctorSuggestedServiceDetailVO> objOtherServiceDetails = null;
        public List<clsDoctorSuggestedServiceDetailVO> ServiceDetails
        {
            get { return objOtherServiceDetails; }
            set { objOtherServiceDetails = value; }
        }
    }

    public class clsGetPatientPastFollowUPNotesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPastFollowUPNotesBizAction";
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
        public long DoctorID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        private int _TotalRows;
        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }
        private bool _PagingEnabled;
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
        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        public List<clsPastFollowUpnoteVO> PastFollowUPList { get; set; }
    }

    public class clsGetPatientPastcostBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPastcostBizAction";
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
        public long DoctorID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        private int _TotalRows;
        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }
        private bool _PagingEnabled;
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
        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        public List<clsPastFollowUpnoteVO> PastFollowUPList { get; set; }
    }

    public class clsPastFollowUpnoteVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }

        public DateTime VisitDate { get; set; }
        public long VisitID { get; set; }
        public long DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpec { get; set; }
        public long UnitID { get; set; }
        public string Datetime { get; set; }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


}
