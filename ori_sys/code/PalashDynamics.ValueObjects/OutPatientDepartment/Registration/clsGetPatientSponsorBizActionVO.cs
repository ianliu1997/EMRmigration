using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster
{
    public class clsGetPatientSponsorBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSponsorBizAction";
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

        public bool GetLatest { get; set; }
        public bool UnitID { get; set; }

        private clsPatientSponsorVO objPatientSponsor = new clsPatientSponsorVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientSponsorVO PatientSponsorDetails
        {
            get { return objPatientSponsor; }
            set { objPatientSponsor = value; }
        }
    }


    public class clsGetPatientSponsorGroupListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSponsorGroupListBizAction";
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

        public long SponsorID { get; set; }

      

        private List<clsPatientSponsorGroupDetailsVO> objDetailsList = new List<clsPatientSponsorGroupDetailsVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientSponsorGroupDetailsVO> DetailsList
        {
            get { return objDetailsList; }
            set { objDetailsList = value; }
        }
    }


    public class clsGetPatientSponsorServiceListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSponsorServiceListBizAction";
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

        public long SponsorID { get; set; }

        private List<clsPatientSponsorServiceDetailsVO> objDetailsList = new List<clsPatientSponsorServiceDetailsVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientSponsorServiceDetailsVO> DetailsList
        {
            get { return objDetailsList; }
            set { objDetailsList = value; }
        }
    }

    public class clsGetPatientSponsorCardListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSponsorCardListBizAction";
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

        public long SponsorID { get; set; }

        private List<clsPatientSponsorCardDetailsVO> objDetailsList = new List<clsPatientSponsorCardDetailsVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientSponsorCardDetailsVO> DetailsList
        {
            get { return objDetailsList; }
            set { objDetailsList = value; }
        }
    }

    // Added By CDS For FollowUp

    public class clsGetFollowUpStatusByPatientIdBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetFollowUpStatusByPatientIdBizAction";
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
        public long UnitID { get; set; }
        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        public long TariffID { get; set; }
        public bool IsFollowUpAdded { get; set; }
        public bool IsPackageDetailsAdded { get; set; }

    }
    
    public class clsAddFollowUpStatusByPatientIdBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddFollowUpStatusByPatientIdBizAction";
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
        public long UnitID { get; set; }


        public long TariffID { get; set; }


        public long SponsorID { get; set; }
        public long SponsorUnitID { get; set; }

        public bool Status { get; set; }
        public bool ResultStatus { get; set; }

        public DateTime AddedDateTime { get; set; }

    }
}
