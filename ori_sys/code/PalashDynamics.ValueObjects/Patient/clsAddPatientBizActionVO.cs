using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsAddPatientBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        public bool IsSaveSponsor { get; set; }  // properrty use to save IPD Sponsor with Transaction
        public clsAddPatientSponsorBizActionVO BizActionVOSaveSponsor { get; set; }  // properrty use to save IPD Sponsor with Transaction
        public clsAddPatientSponsorBizActionVO BizActionVOSaveSponsorForMale { get; set; }  // properrty use to save Sponsor with Transaction for male in couple

        public bool IsSaveAdmission { get; set; }  // properrty use to save IPD Admission with Transaction
        public clsSaveIPDAdmissionBizActionVO BizActionVOSaveAdmission { get; set; }  // properrty use to save IPD Admission with Transaction

        public bool IsAddWithtransaction { get; set; }  // properrty use to save IPD Patient Details with Transaction
        public bool IsSavePatientFromIPD { get; set; } // set to decide save Patient Detials from IPD or not
        public bool IsSavePatientFromOPD { get; set; }  // set to decide save Patient Detials from OPD or not
        public bool IsSaveInTRegistration { get; set; } // set to decide save Patient Detials In T_Registration Table 

        //***//
        private clsBankDetailsInfoVO objBankDetails = null;
        public clsBankDetailsInfoVO BankDetails
        {
            get { return objBankDetails; }
            set { objBankDetails = value; }
        }
       

    }
    public class clsAddNewCoupleBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddNewCoupleBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
    }
    public class clsAddPatientLinkFileBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPatientLinkFileBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public bool FROMEMR { get; set; }

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

        private List<clsPatientLinkFileBizActionVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientLinkFileBizActionVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

    }
    public class clsGetPatientLinkFileBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientLinkFileBizAction";
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
        public long PatientUnitID { get; set; }
        public long VisitID { get; set; }

        private List<clsPatientLinkFileBizActionVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientLinkFileBizActionVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

    }

    public class clsAddPatientProspectBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientProspectBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

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
        private clsPatientProspectSalesInfoVO objSaleDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public clsPatientProspectSalesInfoVO SaleDetails
        {
            get { return objSaleDetail; }
            set { objSaleDetail = value; }
        }

    }

    public class clsGetPatientProspectBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientProspectBizAction";
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
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string MobileNo { get; set; }
        public long EventID { get; set; }
        public long SalesPersonID { get; set; }
        public long ZoneID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FFromDate { get; set; }
        public DateTime? FToDate { get; set; }

        public bool IsRegistered { get; set; }
        public string MiddleName { get; set; }
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }

            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;

                }
            }
        }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        private List<clsPatientVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }


    }

    public class clsGetProspectFollowUpBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetProspectFollowUpBizAction";
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
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string MobileNo { get; set; }
        public long EventID { get; set; }
        public long SalesPersonID { get; set; }
        public long ZoneID { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FFromDate { get; set; }
        public DateTime? FToDate { get; set; }

        public bool IsRegistered { get; set; }
        public string MiddleName { get; set; }
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }

            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;

                }
            }
        }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        private List<clsPatientVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }


    }

    public class clsGetPatientProspectByIDBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientProspectByIDBizAction";
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
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FFromDate { get; set; }
        public DateTime? FToDate { get; set; }

        public string MiddleName { get; set; }
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }

            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;

                }
            }
        }

        public long ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        private List<clsPatientVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsPatientVO objProspectt = null;
        public clsPatientVO Prospect
        {
            get { return objProspectt; }
            set { objProspectt = value; }
        }


    }

    public class clsAddPatientFileBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPatientFileBizAction";
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

        private List<clsTemplateImageBizActionVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsTemplateImageBizActionVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        private clsTemplateImageBizActionVO objPatientFile;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsTemplateImageBizActionVO PatientLinkFile
        {
            get { return objPatientFile; }
            set { objPatientFile = value; }
        }
        public bool IsModify { get; set; }
    }

    public class clsGetPatientProspectOtherBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientProspectOtherBizAction";
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


        public long ProspectID { get; set; }
        public long ProspectUnitId { get; set; }
        public long PatientId { get; set; }
        public long PatientUnitId { get; set; }

        private clsPatientProspectSalesInfoVO ObjSales = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public clsPatientProspectSalesInfoVO SalesDetails
        {
            get { return ObjSales; }
            set { ObjSales = value; }
        }

        private List<MasterListItem> ObjIn = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<MasterListItem> InterstedDetails
        {
            get { return ObjIn; }
            set { ObjIn = value; }
        }

        private List<MasterListItem> ObjService = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<MasterListItem> ServiceDetais
        {
            get { return ObjService; }
            set { ObjService = value; }
        }
        private List<clsEMRVitalsVO> ObjVital = null;
        public List<clsEMRVitalsVO> VitalList
        {
            get { return ObjVital; }
            set { ObjVital = value; }
        }
    }
    public class clsAddEMRTemplateImageBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddEMRTemplateImageBizAction";
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

        private List<clsTemplateImageBizActionVO> objPatient = new List<clsTemplateImageBizActionVO>();
        public List<clsTemplateImageBizActionVO> Details
        {
            get { return objPatient; }
            set { objPatient = value; }
        }


        public bool IsModify { get; set; }
    }

    public class clsGetPatientLinkFileViewDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientLinkFileViewDetailsBizAction";
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
        public long ReportID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long VisitID { get; set; }
        public byte[] Report { get; set; }
        public string SourceURL { get; set; }
        public long TemplateID { get; set; }
        public bool FROMEMR { get; set; }
        public bool IsOPDIPD { get; set; }
        public string Remarks { get; set; }
        private List<clsPatientLinkFileBizActionVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientLinkFileBizActionVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
    }

    public class clsDeletePatientLinkFileBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsDeletePatientLinkFileBizAction";
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
        public long ID { get; set; }
        public long PatientID { get; set; }
        public long UnitID { get; set; }
        public long PatientUnitID { get; set; }
        public long VisitID { get; set; }
        public string Reason { get; set; }

    }
    public class clsAddPatientFollowUpBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientFollowUpBizAction";
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

        private List<clsPatientVO> objPatient;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
    }

    public class clsUpdatePatientFollowUpBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdatePatientFollowUpBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        private List<clsPatientVO> objFollow = new List<clsPatientVO>();
        public List<clsPatientVO> FollowUpList
        {
            get { return objFollow; }
            set { objFollow = value; }
        }

        public bool IsMultiple { get; set; }


    }
    public class clsChangeSchedulePatientFollowUpBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsChangeSchedulePatientFollowUpBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
        private clsPatientFollowUpVO _FollowUpDetails = new clsPatientFollowUpVO();
        public clsPatientFollowUpVO FollowUpDetails
        {
            get { return _FollowUpDetails; }
            set { _FollowUpDetails = value; }
        }
        private List<clsPatientVO> objFollow = new List<clsPatientVO>();
        public List<clsPatientVO> FollowUpList
        {
            get { return objFollow; }
            set { objFollow = value; }
        }

        public bool IsMultiple { get; set; }


    }
    public class clsUpdatProspectFollowUpBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdatProspectFollowUpBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        private List<clsPatientVO> objFollow = new List<clsPatientVO>();
        public List<clsPatientVO> FollowUpList
        {
            get { return objFollow; }
            set { objFollow = value; }
        }

        public bool IsMultiple { get; set; }


    }
    public class clsGetPatientFollowUpBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientFollowUpBizAction";
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

        private List<clsPatientVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientVO> PatientList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }


        private clsPatientVO objPatientDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatientDetails; }
            set { objPatientDetails = value; }
        }

        public long FollowUpStatus { get; set; }
        public bool? Postpone { get; set; }
        public bool? Completed { get; set; }
        public bool ViewPatient { get; set; }
        public bool FollowupPatientlist { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long DepartmentID { get; set; }
        public long DoctorID { get; set; }
        public long TariffID { get; set; }
        public string Service { get; set; }
        public long SpecilizationID { get; set; }
        public string MRNO { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PatientName { get; set; }
        public long PatientID { get; set; }
        public long UnitId { get; set; }
        public long PatientUnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string SortExpression { get; set; }
    }


    public class clsGetFollowUpRenewBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetFollowUpRenewBizAction";
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

        private List<clsPatientVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientVO> PatientList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }


        private clsPatientVO objPatientDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatientDetails; }
            set { objPatientDetails = value; }
        }

        public long FollowUpStatus { get; set; }
        public bool? Postpone { get; set; }
        public bool? Completed { get; set; }
        public bool ViewPatient { get; set; }
        public bool FollowupPatientlist { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long DepartmentID { get; set; }
        public long DoctorID { get; set; }
        public long TariffID { get; set; }
        public string Service { get; set; }
        public long SpecilizationID { get; set; }
        public string MRNO { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long PatientID { get; set; }
        public long UnitId { get; set; }
        public long PatientUnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string SortExpression { get; set; }
    }

    public class clsGetFollowUp_DashBoardBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetFollowUp_DashBoardBizAction";
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

        private List<clsPatientVO> objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientVO> PatientList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }


        private clsPatientVO objPatientDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatientDetails; }
            set { objPatientDetails = value; }
        }

        public long FollowUpStatus { get; set; }
        public bool? Postpone { get; set; }
        public bool? Completed { get; set; }
        public bool ViewPatient { get; set; }
        public bool FollowupPatientlist { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long DepartmentID { get; set; }
        public long DoctorID { get; set; }
        public long TariffID { get; set; }
        public string Service { get; set; }
        public long SpecilizationID { get; set; }
        public string MRNO { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long PatientID { get; set; }
        public long UnitId { get; set; }
        public long PatientUnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string SortExpression { get; set; }

    }

    public class clsAddAudioFileBookBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddAudioFileBookBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }

        public bool Status { get; set; }
        public byte[] AudioFile { get; set; }
        public string FileName { get; set; }


        #region Common Properties


        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;

                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;

                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;

                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;

                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;

                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                }
            }
        }

        #endregion
    }



    public class clsAddPatientForPathologyBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientForPathologyBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        public bool IsSaveSponsor { get; set; }  // properrty use to save IPD Sponsor with Transaction
        public clsAddPatientSponsorBizActionVO BizActionVOSaveSponsor { get; set; }  // properrty use to save IPD Sponsor with Transaction
        public clsAddPatientSponsorBizActionVO BizActionVOSaveSponsorForMale { get; set; }  // properrty use to save Sponsor with Transaction for male in couple

        public clsAddVisitBizActionVO BizActionVOSaveVisit { get; set; }  
   

        public bool IsSaveAdmission { get; set; }  // properrty use to save IPD Admission with Transaction
        public clsSaveIPDAdmissionBizActionVO BizActionVOSaveAdmission { get; set; }  // properrty use to save IPD Admission with Transaction

        public bool IsAddWithtransaction { get; set; }  // properrty use to save IPD Patient Details with Transaction
        public bool IsSavePatientFromIPD { get; set; } // set to decide save Patient Detials from IPD or not
        public bool IsSavePatientFromOPD { get; set; }  // set to decide save Patient Detials from OPD or not
        public bool IsSaveInTRegistration { get; set; } // set to decide save Patient Detials In T_Registration Table 

    }

    //added by neena
    public class clsAddDonorCodeBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddDonorCodeBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }       

    }
    //


}
