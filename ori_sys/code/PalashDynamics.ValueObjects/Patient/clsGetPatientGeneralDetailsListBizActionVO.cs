using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsGetPatientGeneralDetailsListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientGeneralDetailsListBizAction";
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
        public DateTime? VisitFromDate { get; set; }
        public DateTime? VisitToDate { get; set; }

        
        public long IdentityID { get; set; }
        public long SpecialRegID { get; set; }
        
       
        public string IdentityNumber { get; set; }
        public bool IsDocAttached { get; set; }
        
        public string Email { get; set; }

        public bool ISDonorSerch { get; set; } //***//

        public bool IsSearchForRegistration { get; set; }
        public DateTime? AdmissionFromDate { get; set; }
        public DateTime? AdmissionToDate { get; set; }

        public DateTime? DOBFromDate { get; set; }
        public DateTime? DOBToDate { get; set; }

        public bool RegistrationWise { get; set; }
        public bool VisitWise { get; set; }
        public bool AdmissionWise { get; set; }


        //Added by AJ Date 29/12/2016
        public bool isfromMaterialConsumpation { get; set; }
        public bool isfromCouterSaleStaff { get; set; }
        public long EmpNO { get; set; }

        //***//----------
        public long StoreID { get; set; }
        public bool DOBWise { get; set; }

        public bool IsLoyaltyMember { get; set; }
        public long LoyaltyProgramID { get; set; }
        public long PatientCategoryID { get; set; }
        public bool IsFromSurrogacyModule { get; set; }
        //public bool IncludeSpouse { get; set; }
        public int IsSelfAndDonor { get; set; }
        public string MRNo { get; set; }
        public bool ShowOutSourceDonor { get; set; }
        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FamilyName { get; set; }

        public string LinkServer { get; set; }

        public string OPDNo { get; set; }

        public Boolean ISFromQueeManagment { get; set; }

        public string IPDNo { get; set; }

        public string ContactNo { get; set; }

        public string Country { get; set; }

        //added by neena
        public string DonarCode { get; set; }
        public string OldRegistrationNo { get; set; }
        //

        public string State { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        public long GenderID { get; set; }

        public string CivilID { get; set; }

        public long UnitID { get; set; }
      //  public long RegTypeID { get; set; }
        public string RegType { get; set; }
        public string DonorCode { get; set; }
        public bool SearchInAnotherClinic { get; set; }

        public long RegistrationTypeID { get; set; }

        public bool IsCurrentAdmitted { get; set; }

        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientGeneralVO> PatientDetailsList
        {
            get;
            set;
        }

        public bool IsFromFindPatient { get; set; }
        public long SearchUnitID { get; set; }
        public DateTime? DOB { get; set; }
        public string ReferenceNo { get; set; }
        public long PQueueUnitID { get; set; }

        private string _AddressLine1;
        public string AddressLine1
        {
            get
            {
                return _AddressLine1;
            }
            set
            {
                _AddressLine1 = value;
            }
        }

    }

    public class clsGetCoupleGeneralDetailsListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetCoupleGeneralDetailsListBizAction";
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
        public DateTime? VisitFromDate { get; set; }
        public DateTime? VisitToDate { get; set; }

        public DateTime? AdmissionFromDate { get; set; }
        public DateTime? AdmissionToDate { get; set; }

        public DateTime? DOBFromDate { get; set; }
        public DateTime? DOBToDate { get; set; }

        public bool RegistrationWise { get; set; }
        public bool VisitWise { get; set; }
        public bool AdmissionWise { get; set; }

        public bool DOBWise { get; set; }

        public bool IsLoyaltyMember { get; set; }
        public long LoyaltyProgramID { get; set; }
        public long PatientCategoryID { get; set; }

        //public bool IncludeSpouse { get; set; }

        public string MRNo { get; set; }
        public string SearchKeyword { get; set; }
        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FamilyName { get; set; }

        public string LinkServer { get; set; }

        public string OPDNo { get; set; }

        public string ContactNo { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        public long GenderID { get; set; }

        public string CivilID { get; set; }

        public long UnitID { get; set; }

        public bool SearchInAnotherClinic { get; set; }

        public long RegistrationTypeID { get; set; }

        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientGeneralVO> PatientDetailsList
        {
            get;
            set;
        }

        //***//-----------
        public string DonorID { get; set; }
        public long DonorUnitID { get; set; }
        public bool IsDonorLinkCouple { get; set; }

    }

    public class clsGetPatientGeneralDetailsListForSurrogacyBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientGeneralDetailsListForSurrogacyBizAction";
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
        public DateTime? VisitFromDate { get; set; }
        public DateTime? VisitToDate { get; set; }

        public DateTime? AdmissionFromDate { get; set; }
        public DateTime? AdmissionToDate { get; set; }

        public DateTime? DOBFromDate { get; set; }
        public DateTime? DOBToDate { get; set; }

        public bool RegistrationWise { get; set; }
        public bool VisitWise { get; set; }
        public bool AdmissionWise { get; set; }

        public bool DOBWise { get; set; }

        public bool IsLoyaltyMember { get; set; }
        public long LoyaltyProgramID { get; set; }
        public long PatientCategoryID { get; set; }

        //public bool IncludeSpouse { get; set; }

        public string MRNo { get; set; }

        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FamilyName { get; set; }

        public string LinkServer { get; set; }

        public string OPDNo { get; set; }

        public string ContactNo { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        public long GenderID { get; set; }

        public string CivilID { get; set; }

        public long UnitID { get; set; }

        public bool SearchInAnotherClinic { get; set; }

        public long RegistrationTypeID { get; set; }

        public long AgencyID { get; set; }

        public bool IsFromSurrogacyModule { get; set; }
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientGeneralVO> PatientDetailsList
        {
            get;
            set;
        }

    }

    public class clsGetOTPatientGeneralDetailsListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOTPatientGeneralDetailsListBizAction";
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
        public DateTime? VisitFromDate { get; set; }
        public DateTime? VisitToDate { get; set; }

        public bool IsSearchForRegistration { get; set; }
        public DateTime? AdmissionFromDate { get; set; }
        public DateTime? AdmissionToDate { get; set; }

        public DateTime? DOBFromDate { get; set; }
        public DateTime? DOBToDate { get; set; }

        public bool RegistrationWise { get; set; }
        public bool VisitWise { get; set; }
        public bool AdmissionWise { get; set; }

        public bool DOBWise { get; set; }

        public bool IsLoyaltyMember { get; set; }
        public long LoyaltyProgramID { get; set; }
        public long PatientCategoryID { get; set; }
        public bool IsFromSurrogacyModule { get; set; }
        //public bool IncludeSpouse { get; set; }
        public int IsSelfAndDonor { get; set; }
        public string MRNo { get; set; }
        public bool ShowOutSourceDonor { get; set; }
        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FamilyName { get; set; }

        public string LinkServer { get; set; }

        public string OPDNo { get; set; }

        public string IPDNo { get; set; }

        public string ContactNo { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Pincode { get; set; }

        public long GenderID { get; set; }

        public string CivilID { get; set; }

        public long UnitID { get; set; }
        public string DonorCode { get; set; }
        public bool SearchInAnotherClinic { get; set; }

        public long RegistrationTypeID { get; set; }

        public bool IsCurrentAdmitted { get; set; }

        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientGeneralVO> PatientDetailsList
        {
            get;
            set;
        }

        public bool IsFromFindPatient { get; set; }
        public long SearchUnitID { get; set; }
        public DateTime? DOB { get; set; }
        public string ReferenceNo { get; set; }
        public long PQueueUnitID { get; set; }

        private string _AddressLine1;
        public string AddressLine1
        {
            get
            {
                return _AddressLine1;
            }
            set
            {
                _AddressLine1 = value;
            }
        }

    }
}
