using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsGetPatientBillBalanceAmountBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsGetPatientBillBalanceAmountBizAction";
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
        //public bool IsAllCouple { get; set; }
        //private double _BalanceAmountSelf;
        //public double BalanceAmountSelf
        //{
        //    get { return _BalanceAmountSelf; }
        //    set
        //    {
        //        if (_BalanceAmountSelf != value)
        //        {
        //            _BalanceAmountSelf = value;
        //            //OnPropertyChanged("BalanceAmountSelf");
        //        }
        //    }
        //}

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

        //private clsPatientVO objPatient = null;
        ///// <summary>
        ///// Output Property.
        ///// This Property Contains OPDPatient Details Which is Added.
        ///// </summary>
        //public clsPatientVO PatientDetails
        //{
        //    get { return objPatient; }
        //    set { objPatient = value; }
        //}



        private clsPatientGeneralVO objPatientGeneral = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientGeneralVO PatientGeneralDetails
        {
            get { return objPatientGeneral; }
            set { objPatientGeneral = value; }
        }

        //private clsPatientAttachmentVO objPatientAttachmentDetails = null;
        //public clsPatientAttachmentVO PatientAttachmentDetail
        //{
        //    get { return objPatientAttachmentDetails; }
        //    set { objPatientAttachmentDetails = value; }
        //}

        //private List<clsPatientAttachmentVO> objPatientAttachment = null;
        //public List<clsPatientAttachmentVO> PatientAttachmentDetailList
        //{
        //    get { return objPatientAttachment; }
        //    set { objPatientAttachment = value; }
        //}
       
        //public long SurrogateID { get; set; }
        //private bool _IsFromSearchWindow = false;
        //public bool IsFromSearchWindow
        //{
        //    get { return _IsFromSearchWindow; }
        //    set { _IsFromSearchWindow = value; }
        //}
    }
}
