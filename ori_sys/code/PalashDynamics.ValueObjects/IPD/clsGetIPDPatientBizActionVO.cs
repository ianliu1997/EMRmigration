/* Added By Sudhir Patil On 05/04/2014 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsGetIPDPatientBizActionVO : IBizActionValueObject 
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDPatientBizAction";
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

        private clsGetIPDPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsGetIPDPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        private clsPatientAttachmentVO objPatientAttachmentDetails = null;
        public clsPatientAttachmentVO PatientAttachmentDetail
        {
            get { return objPatientAttachmentDetails; }
            set { objPatientAttachmentDetails = value; }
        }

        private List<clsGetIPDPatientVO> _IPDPatientList = null;
        public List<clsGetIPDPatientVO> IPDPatientList
        {
            get { return _IPDPatientList; }
            set { _IPDPatientList = value; }
        }
        private List<clsPatientAttachmentVO> objPatientAttachment = null;
        public List<clsPatientAttachmentVO> PatientAttachmentDetailList
        {
            get { return objPatientAttachment; }
            set { objPatientAttachment = value; }
        }
    }

    public class clsGetIPDPatientListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDPatientListBizAction";
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

        public bool RegistrationWise { get; set; }
        public bool VisitWise { get; set; }
        public bool AdmissionWise { get; set; }

        public bool IsLoyaltyMember { get; set; }
        public long LoyaltyProgramID { get; set; }
        public long PatientCategoryID { get; set; }

        //public bool IncludeSpouse { get; set; }

        public string MRNo { get; set; }

        public bool IsFromDischarge { get; set; }
        //private string _FirstName;
        //public string FirstName
        //{
        //    get
        //    {
        //        return _FirstName;
        //    }
        //    set
        //    {
        //        _FirstName = value;
        //    }
        //}

        //private string _AddressLine1;
        //public string AddressLine1
        //{
        //    get
        //    {
        //        return _AddressLine1;
        //    }
        //    set
        //    {
        //        _AddressLine1 = value;
        //    }
        private clsPatientGeneralVO _GeneralDetails = new clsPatientGeneralVO();
        public clsPatientGeneralVO GeneralDetails
        {
            get { return _GeneralDetails; }
            set { _GeneralDetails = value; }
        }
        private List<clsPatientGeneralVO> _IPDPatientList;
        public List<clsPatientGeneralVO> IPDPatientList
        {
            get { return _IPDPatientList; }
            set { _IPDPatientList = value; }
        }
    }
}
