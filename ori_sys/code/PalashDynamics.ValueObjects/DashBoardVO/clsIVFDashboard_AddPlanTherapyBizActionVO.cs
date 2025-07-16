using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_AddPlanTherapyBizActionVO : IBizActionValueObject
    {
         #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddPlanTherapyBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        //by vikrant
        private List<clsEMRAddDiagnosisVO> _objDiagnosisDetails = null;
        public List<clsEMRAddDiagnosisVO> DiagnosisDetails
        {
            get { return _objDiagnosisDetails; }
            set { _objDiagnosisDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        
    }
    public class clsIVFDashboard_AddDiagnosisBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddDiagnosisBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsDignosisVO _DiagnosisDetails = new clsDignosisVO();
        public clsDignosisVO DiagnosisDetails
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
        private List<clsDignosisVO> _DiagnosisList = new List<clsDignosisVO>();
        public List<clsDignosisVO> DiagnosisList
        {
            get { return _DiagnosisList; }
            set { _DiagnosisList = value; }
        }
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }
    public class clsGetIVFDashboardPatientDiagnosisDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            //return "PalashDynamics.BusinessLayer.clsGetPatientDiagnosisDataBizAction";
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetIVFDashboardPatientDiagnosisDataBizAction";
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
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public long VisitID { get; set; }
        public bool IsICDX { get; set; }
        public bool IsICDXhistory { get; set; }
        public bool IsOPDIPD { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool Ishistory { get; set; }
        public long UnitID { get; set; }
        public bool ISDashBoard { get; set; }
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


    //added by neena
    public class clsGetIVFDashboardPatientPrescriptionDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            //return "PalashDynamics.BusinessLayer.clsGetPatientDiagnosisDataBizAction";
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetIVFDashboardPatientPrescriptionDataBizAction";
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
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public long VisitID { get; set; }
        public bool IsICDX { get; set; }
        public bool IsICDXhistory { get; set; }
        public bool IsOPDIPD { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool Ishistory { get; set; }
        public long UnitID { get; set; }
        public bool ISDashBoard { get; set; }
        private int _TotalRows;

        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;                    
                }
            }
        }

        private long _PlanTherapyUnitID;
        public long PlanTherapyUnitID
        {
            get { return _PlanTherapyUnitID; }
            set
            {
                if (_PlanTherapyUnitID != value)
                {
                    _PlanTherapyUnitID = value;                   
                }
            }
        }
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

        //added by neena
        private List<clsPatientPrescriptionDetailVO> _PatientPrescriptionList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionList
        {
            get { return _PatientPrescriptionList; }
            set { _PatientPrescriptionList = value; }
        }
        //

    }

    public class clsGetIVFDashboardPatientInvestigationDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            //return "PalashDynamics.BusinessLayer.clsGetPatientDiagnosisDataBizAction";
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetIVFDashboardPatientInvestigationDataBizAction";
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
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public long VisitID { get; set; }
        public bool IsICDX { get; set; }
        public bool IsICDXhistory { get; set; }
        public bool IsOPDIPD { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool Ishistory { get; set; }
        public long UnitID { get; set; }
        public bool ISDashBoard { get; set; }
        private int _TotalRows;
        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;
                }
            }
        }

        private long _PlanTherapyUnitID;
        public long PlanTherapyUnitID
        {
            get { return _PlanTherapyUnitID; }
            set
            {
                if (_PlanTherapyUnitID != value)
                {
                    _PlanTherapyUnitID = value;
                   
                }
            }
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

        private List<clsServiceMasterVO> _PatientInvestigationList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsServiceMasterVO> PatientInvestigationList
        {
            get { return _PatientInvestigationList; }
            set { _PatientInvestigationList = value; }
        }

       

    }

    //
    public class clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO: IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetIVFDashboardCurrentPatientDiagnosisDataBizAction";
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
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public long VisitID { get; set; }
        public bool IsICDX { get; set; }
        public bool IsICDXhistory { get; set; }
        public bool IsOPDIPD { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool Ishistory { get; set; }
        public long UnitID { get; set; }
        public bool ISDashBoard { get; set; }
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
    public class clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizAction";
        }
        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }
    public class clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizAction";
        }
        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public long TherapyID { get; set; }

        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }

    public class clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetPlanTherapyCPOEServicesBizAction";
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
        public long TherapyID { get; set; }
        public long UnitID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public int _SuccessStatus;
        public bool IsOtherServices { get; set; }
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

    public class clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizAction";
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
        public long PatientID { get; set; }
        public long TherapyID { get; set; }
        public Boolean IsOtherServices { get; set; }
        public Boolean FlagForAddUpdate { get; set; }
        public long UnitID { get; set; }
        private List<clsDoctorSuggestedServiceDetailVO> _InvestigationList;
        public List<clsDoctorSuggestedServiceDetailVO> InvestigationList
        {
            get { return _InvestigationList; }
            set { _InvestigationList = value; }
        }
    }

    //added by neena
    public class clsIVFDashboard_GetManagementVisibleBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetManagementVisibleBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        //by vikrant
        private List<clsEMRAddDiagnosisVO> _objDiagnosisDetails = null;
        public List<clsEMRAddDiagnosisVO> DiagnosisDetails
        {
            get { return _objDiagnosisDetails; }
            set { _objDiagnosisDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }


    }

    public class clsIVFDashboard_GetPACVisibleBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsIVFDashboard_GetPACVisibleBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private clsPlanTherapyVO _TherapyDetails = new clsPlanTherapyVO();
        public clsPlanTherapyVO TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }

        //by vikrant
        private List<clsEMRAddDiagnosisVO> _objDiagnosisDetails = null;
        public List<clsEMRAddDiagnosisVO> DiagnosisDetails
        {
            get { return _objDiagnosisDetails; }
            set { _objDiagnosisDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }


    }
    //


 }

