using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsAddPathPatientReportBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPathPatientReportBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private bool _AutoDeductStock;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public bool AutoDeductStock
        {
            get { return _AutoDeductStock; }
            set { _AutoDeductStock = value; }
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

        private clsPathPatientReportVO objPathPatientReportDetail = new clsPathPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathPatientReportVO OrderPathPatientReportList
        {
            get { return objPathPatientReportDetail; }
            set { objPathPatientReportDetail = value; }
        }

        #region Newly Added
        private List<clsPathPatientReportVO> _OrderList = new List<clsPathPatientReportVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathPatientReportVO> OrderList
        {
            get { return _OrderList; }
            set { _OrderList = value; }
        }

        private List<clsPathoTestParameterVO> _TestList = new List<clsPathoTestParameterVO>();
        public List<clsPathoTestParameterVO> TestList
        {
            get { return _TestList; }
            set { _TestList = value; }
        }
        public long UnitID { get; set; }

        // Added by Anumani for autoGeneration of Pathology 
        // Added on 26.04.2016

        private List<clsPathOrderBookingDetailVO> _MasterList = null;
        public List<clsPathOrderBookingDetailVO> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }            

       
        #endregion

    }

    public class clsAddPathoRadResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPathoRadResultEntryBizAction";
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

        private clsPathPatientReportVO objPathPatientReportDetail = new clsPathPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathPatientReportVO ResultEntryDetails
        {
            get { return objPathPatientReportDetail; }
            set { objPathPatientReportDetail = value; }
        }
    }

    public class clsGetPathoResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoResultEntryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        public long ID { get; set; }
        public long DetailID { get; set; }
        public long UnitID { get; set; }

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

        private clsPathPatientReportVO objPathPatientReportDetail = new clsPathPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathPatientReportVO ResultEntryDetails
        {
            get { return objPathPatientReportDetail; }
            set { objPathPatientReportDetail = value; }
        }



    }

    #region Newly Added 
    
    public class clsGetPathoFinalizedEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoFinalizedEntryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        public long ID { get; set; }
        public string DetailID { get; set; }
        public long UnitID { get; set; }
        public bool GetResultFromMachine { get; set; }
        public string ErrorMessage { get; set; }
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

        private clsPathPatientReportVO objPathPatientReportDetail = new clsPathPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathPatientReportVO ResultEntryDetails
        {
            get { return objPathPatientReportDetail; }
            set { objPathPatientReportDetail = value; }
        }




        private List<clsPathPatientReportVO> lst = new List<clsPathPatientReportVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathPatientReportVO> ResultList
        {
            get { return lst; }
            set { lst = value; }
        }


    }

    public class clsApprovePathPatientReortBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsApprovePathPatientReortBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private clsPathPatientReportVO objPathPatientReportDetail = new clsPathPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathPatientReportVO OrderPathPatientReportList
        {
            get { return objPathPatientReportDetail; }
            set { objPathPatientReportDetail = value; }
        }
        private List<clsPathPatientReportVO> _OrderList = new List<clsPathPatientReportVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathPatientReportVO> OrderList
        {
            get { return _OrderList; }
            set { _OrderList = value; }
        }

        private List<clsPathoTestParameterVO> _TestList = new List<clsPathoTestParameterVO>();
        public List<clsPathoTestParameterVO> TestList
        {
            get { return _TestList; }
            set { _TestList = value; }
        }
        public string OrderDetailsID { get; set; }
        public bool IsDigitalSignatureRequired { get; set; }

        private bool _IsSecondLevelApproval;
        [DefaultValue(false)]
        public bool IsSecondLevelApproval
        {
            get { return _IsSecondLevelApproval; }
            set { _IsSecondLevelApproval = value; }
        }

        private bool _IsThirdLevelApproval;
        [DefaultValue(false)]
        public bool IsThirdLevelApproval
        {
            get { return _IsThirdLevelApproval; }
            set { _IsThirdLevelApproval = value; }
        }

        private bool _IsForCheckResults;
        [DefaultValue(false)]
        public bool IsForCheckResults
        {
            get { return _IsForCheckResults; }
            set { _IsForCheckResults = value; }
        }

        private bool _IsThirdLevelCheckResult;
        [DefaultValue(false)]
        public bool IsThirdLevelCheckResult
        {
            get { return _IsThirdLevelCheckResult; }
            set { _IsThirdLevelCheckResult = value; }
        }

        private String _checkResultValueMessage = String.Empty;
        public String checkResultValueMessage
        {
            get { return _checkResultValueMessage; }
            set { _checkResultValueMessage = value; }
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
    }

     #endregion


    #region For Pathology Additions

    public class clsAddPathPatientReportDetailsForEmailSendingBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPathPatientReportDetailsForEmailSendingBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private bool _AutoDeductStock;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public bool AutoDeductStock
        {
            get { return _AutoDeductStock; }
            set { _AutoDeductStock = value; }
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

        private clsPathPatientReportVO objPathPatientReportDetail = new clsPathPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathPatientReportVO OrderPathPatientReportList
        {
            get { return objPathPatientReportDetail; }
            set { objPathPatientReportDetail = value; }
        }


        private clsPathOrderBookingDetailVO objPathOrderBookingDetail = new clsPathOrderBookingDetailVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathOrderBookingDetailVO PathOrderBookingDetailList
        {
            get { return objPathOrderBookingDetail; }
            set { objPathOrderBookingDetail = value; }
        }

        public long AttachmentID { get; set; }
        public long UnitID { get; set; }
        public long OrderID { get; set; }
        public long PathOrderBookingDetailID { get; set; }
        public long PathPatientReportID { get; set; }
        public string PatientEmailID { get; set; }
        public string DoctorEmailID { get; set; }
        public bool Status { get; set; }
        public byte[] Report { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public string PatientName { get; set; }
        public DateTime? VisitDate { get; set; }
        public string SourceURL { get; set; }

        private List<clsPathOrderBookingDetailVO> objPathOrder = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> PathOrderBookList
        {
            get { return objPathOrder; }
            set { objPathOrder = value; }
        }
    }

    #endregion

}
