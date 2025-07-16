using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster
{
    public class clsGetPatientSponsorListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSponsorListBizAction";
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
        private List<clsPatientSponsorVO> objPatientSponsor = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientSponsorVO> PatientSponsorDetails
        {
            get { return objPatientSponsor; }
            set { objPatientSponsor = value; }
        }
    }

    // Added By CDS 
    public class clsGetPatientSponsorCompanyListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSponsorCompanyListBizAction";
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

    public class clsGetPatientSponsorTariffListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientSponsorTariffListBizAction";
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
        public long PatientCompanyID { get; set; }
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

    public class clsGetPatientPackageInfoListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientPackageInfoListBizAction";
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

        public long PatientID1 { get; set; }
        public long PatientUnitID1 { get; set; }

        public long PatientID2 { get; set; }
        public long PatientUnitID2 { get; set; }

        public long PatientSourceID { get; set; }
        public bool IsfromCounterSale { get; set; }
        public long PatientTariffID { get; set; }

        public long PatientCompanyID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public DateTime? CheckDate { get; set; }
        public long PatientTypeID { get; set; }

        public long ChargeID { get; set; }

        #region Package New Changes Added on 28042018
        // Use when call from Counter Sale to check whether Pharmacy Consumed Amount > Pharmacy  Component
        public long PackageBillID { get; set; }         
        public long PackageBillUnitID { get; set; }

        public bool IsForPackage { get; set; }   // For Package New Changes Added on 19062018

        #endregion

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }
    }
}
