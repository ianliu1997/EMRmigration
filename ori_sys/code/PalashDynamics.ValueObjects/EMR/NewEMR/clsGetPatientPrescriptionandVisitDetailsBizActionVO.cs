//Created Date:01/August/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Geting the Patient EMR Prescription Detail

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System;
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientPrescriptionandVisitDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPrescriptionandVisitDetailsBizAction";
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
        public bool IsFromPresc { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public Boolean ISForPrint { get; set; }
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
        private List<clsVisitEMRDetails> objPatientVisitEMRDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsVisitEMRDetails> PatientVisitEMRDetails
        {
            get { return objPatientVisitEMRDetails; }
            set { objPatientVisitEMRDetails = value; }
        }

        public List<clsVisitEMRDetails> PatientVisitEMRDetailsIPD
        {
            get { return objPatientVisitEMRDetails; }
            set { objPatientVisitEMRDetails = value; }
        }
    }


    public class clsGetPatientCurrentMedicationDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientCurrentMedicationDetailsBizAction";
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
        public bool Status { get; set; }
        public bool IsPrevious { get; set; }
        public bool IsFromPresc { get; set; }
        public bool IsOPDIPD { get; set; }
        private bool _PagingEnabled;

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
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
        private int _TotalRows = 0;

        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
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
        private List<clsPatientPrescriptionDetailVO> objPatientCurrentMedicationDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientCurrentMedicationDetailList
        {
            get { return objPatientCurrentMedicationDetail; }
            set { objPatientCurrentMedicationDetail = value; }
        }
    }


    public class clsGetPatientPastMedicationDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPastMedicationDetailsBizAction";
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
        public bool Status { get; set; }
        public bool IsFromPresc { get; set; }
        public long UnitID { get; set; }

        public bool _IsForCompound = false;
        public bool IsForCompound
        {
            get { return _IsForCompound; }
            set { _IsForCompound = value; }
        }

        public long VisitID { get; set; }
        private int _SuccessStatus;

        public string DoctorCode { get; set; }
        public long DoctorId { get; set; }
        public bool IsOPDIPD { get; set; }

        private bool _PagingEnabled;

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private int _StartRowIndex = 0;

        public int StartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _MaximumRows;

        public int MaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }
        private int _TotalRows = 0;

        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private List<clsPatientPrescriptionDetailVO> _PatientCurrentMedicationDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientMedicationDetailList
        {
            get { return _PatientCurrentMedicationDetail; }
            set { _PatientCurrentMedicationDetail = value; }
        }
    }


    public class clsGetPatientCurrentMedicationDetailListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientCurrentMedicationDetailListBizAction";
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
        public Boolean IsOPDIPD { get; set; }

        private int _SuccessStatus;


        private bool _PagingEnabled;

        public bool PagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
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
        private int _TotalRows = 0;

        public int TotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private List<clsPatientPrescriptionDetailVO> objPatientCurrentMedicationDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientCurrentMedicationDetailList
        {
            get { return objPatientCurrentMedicationDetail; }
            set { objPatientCurrentMedicationDetail = value; }
        }
    }
}
