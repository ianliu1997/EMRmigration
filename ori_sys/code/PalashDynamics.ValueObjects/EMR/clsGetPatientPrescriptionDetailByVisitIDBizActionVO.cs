using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientPrescriptionDetailByVisitIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientPrescriptionDetailByVisitIDBizAction";
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

        public int IsFrom { get; set; }
        public long VisitID { get; set; }
        public long StoreID { get; set; }
        
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

        //by neena
        private double _TotalNewPendingQuantity;
        public double TotalNewPendingQuantity
        {
            get { return _TotalNewPendingQuantity; }
            set
            {
                if (_TotalNewPendingQuantity != value)
                {
                    _TotalNewPendingQuantity = value;                    
                }
            }
        }
        //


    }

    //added by neena
    public class clsGetPatientPrescriptionDetailByVisitIDForPrintBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientPrescriptionDetailByVisitIDForPrintBizAction";
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

        public int IsFrom { get; set; }
        public long VisitID { get; set; }
        public long StoreID { get; set; }

        private clsPatientPrescriptionDetailVO _PatientPrescriptionDetailObj;
        public clsPatientPrescriptionDetailVO PatientPrescriptionDetailObj
        {
            get { return _PatientPrescriptionDetailObj; }
            set { _PatientPrescriptionDetailObj = value; }
        }

        private string _SendPrescriptionID;
        public string SendPrescriptionID
        {
            get
            {
                return _SendPrescriptionID;
            }
            set
            {
                if (_SendPrescriptionID != value)
                {
                    _SendPrescriptionID = value;                   
                }
            }
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

    public class clsGetPrescriptionReasonOnCounterSaleBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPrescriptionReasonOnCounterSaleBizAction";
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

    public class clsGetPrescriptionIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPrescriptionIDBizAction";
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
    }
    //

    public class clsGetDoctorSuggestedServiceDetailByVisitIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetDoctorSuggestedServiceDetailByVisitIDBizAction";
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

    public class clsGetEMRFrequencyBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetEMRFrequencyBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        private List<FrequencyMaster> objPatientPrescription = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<FrequencyMaster> FrequencyList
        {
            get { return objPatientPrescription; }
            set { objPatientPrescription = value; }
        }





    }
}
