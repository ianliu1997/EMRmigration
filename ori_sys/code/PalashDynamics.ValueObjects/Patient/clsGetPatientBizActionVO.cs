using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsGetPatientBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientBizAction";
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

        private clsPatientAttachmentVO objPatientAttachmentDetails = null;
        public clsPatientAttachmentVO PatientAttachmentDetail
        {
            get { return objPatientAttachmentDetails; }
            set { objPatientAttachmentDetails = value; }
        }

        private List<clsPatientAttachmentVO> objPatientAttachment = null;
        public List<clsPatientAttachmentVO> PatientAttachmentDetailList
        {
            get { return objPatientAttachment; }
            set { objPatientAttachment = value; }
        }
        public long SurrogateID { get; set; }
        private bool _IsFromSearchWindow = false;
        public bool IsFromSearchWindow
        {
            get { return _IsFromSearchWindow; }
            set { _IsFromSearchWindow = value; }
        }

        private clsBankDetailsInfoVO objBankDetails = null;
        public clsBankDetailsInfoVO BankDetails
        {
            get { return objBankDetails; }
            set { objBankDetails = value; }
        }




    }

    public class clsGetPatientTariffsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientTariffsBizAction";
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
        public long PatientSourceID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public DateTime? CheckDate { get; set; }
        public long PatientTypeID { get; set; }

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
    }

    //Added by Saily P
    public class clsGetPatientListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientListBizAction";
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
        public List<clsPatientVO> PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        public List<clsPatientGeneralVO> PatientDetailsList
        { get; set; }
        public long GenderID { get; set; }

        public bool SelectedStatus { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string Description { get; set; }
        public string SortExpression { get; set; }
        public bool SearchName { get; set; } // If SearchName = 0 Then Search by Name, Else Search by MR. No.
    }

    public class clsCheckPatientDuplicacyBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsCheckPatientDuplicacyBizAction";
        }

        #endregion
        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
        public bool ResultStatus { get; set; }
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPatientVO objPatient = null;
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        private List<clsPatientGeneralVO> Obj = null;
        public List<clsPatientGeneralVO> PatientList
        {
            get { return Obj; }
            set { Obj = value; }
        }

        public bool PatientEditMode { get; set; }

    }
}
