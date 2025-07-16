using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsGetPathOrderBookingDetailListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathOrderBookingDetailListBizAction";
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
        public long OrderID { get; set; }
        public long UnitID { get; set; }
        public bool CheckExtraCriteria { get; set; }
        public bool CheckSampleType { get; set; }
        public bool SampleType { get; set; }
        public bool CheckUploadStatus { get; set; }
        public bool IsUploaded { get; set; }
        public bool CheckDeliveryStatus { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsExternalPatient { get; set; }
        //Added By Bhushanp 19012017 For Date Filter
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public long StatusID { get; set; }
        public long AgencyID { get; set; }
        #region Newly Added Properties 
        public long TestCategoryID { get; set; }
        public long AuthenticationLevel { get; set; }

        public bool IsFromDispatch
        {
            get { return _IsFromDispatch; }
            set
            {
                if (_IsFromDispatch != value)
                {
                    _IsFromDispatch = value;
                    
                }
            }
        }

        public bool IsFromReceive
        {
            get { return _IsFromReceive; }
            set
            {
                if (_IsFromReceive != value)
                {
                    _IsFromReceive = value;
                    
                }
            }
        }
      
        public string IsFrom
        {
            get { return _IsFrom; }
            set
            {
                if (_IsFrom != value)
                {
                    _IsFrom = value;

                }
            }
        }

        public bool IsFromCollection
        {
            get { return _IsFromCollection; }
            set
            {
                if (_IsFromCollection != value)
                {
                    _IsFromCollection = value;
                    
                }
            }
        }
        public bool IsFromAcceptRejct
        {
            get { return _IsFromAcceptRejct; }
            set
            {
                if (_IsFromAcceptRejct != value)
                {
                    _IsFromAcceptRejct = value;
                   
                }
            }
        }

        public bool IsFromResult
        {
            get { return _IsFromResult; }
            set
            {
                if (_IsFromResult != value)
                {
                    _IsFromResult = value;
                   
                }
            }
        }

        public bool IsFromAuthorization
        {
            get { return _IsFromAuthorization; }
            set
            {
                if (_IsFromAuthorization != value)
                {
                    _IsFromAuthorization = value;
                   
                }
            }
        }

        public bool IsFromUpload
        {
            get { return _IsFromUpload; }
            set
            {
                if (_IsFromUpload != value)
                {
                    _IsFromUpload = value;
                   
                }
            }
        }
        private bool _IsFromDispatch = false;
        private bool _IsFromReceive = false;
        private bool _IsFromCollection = false;
        private bool _IsFromAcceptRejct = false;
        private bool _IsFromResult = false;
        private bool _IsFromAuthorization = false;
        private bool _IsFromUpload = false;
        private string _IsFrom = "";

        private List<clsPathOrderBookingDetailVO> _objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> objOutsourceOrderBookingDetail
        {
            get { return _objOutsourceOrderBookingDetail; }
            set { _objOutsourceOrderBookingDetail = value; }
        }

        //added by rohini dated 11.2.16

        private clsPathOrderBookingDetailVO Detail = null;
        public clsPathOrderBookingDetailVO OrderDetail
        {
            get { return Detail; }
            set { Detail = value; }
        }

        //
        #endregion 


        private List<clsPathOrderBookingDetailVO> objOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathOrderBookingDetailVO> OrderBookingDetailList
        {
            get { return objOrderBookingDetail; }
            set { objOrderBookingDetail = value; }
        }
    }

    public class clsGetPathoTestItemDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsPathoTestItemDetailsVO> objItemList = null;
        public List<clsPathoTestItemDetailsVO> ItemList
        {
            get { return objItemList; }
            set { objItemList = value; }
        }

        public long TestID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoTestItemDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    public class clsGetPathoTestDetailsForResultEntryBizActionVO : IBizActionValueObject
    {
        private List<clsPathoTestParameterVO> objTestList = null;
        public List<clsPathoTestParameterVO> TestList
        {
            get { return objTestList; }
            set { objTestList = value; }
        }

        List<MasterListItem> _HelpValueList = new List<MasterListItem>();
        public List<MasterListItem> HelpValueList
        {
            get
            {
                return _HelpValueList;
            }
            set
            {
                if (value != _HelpValueList)
                {
                    _HelpValueList = value;
                }
            }

        }

        public long TestID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoTestDetailsForResultEntryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetPathoTestParameterAndSubTestDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsPathoTestParameterVO> objTestList = null;
        public List<clsPathoTestParameterVO> TestList
        {
            get { return objTestList; }
            set { objTestList = value; }
        }

        #region Newly added

        private clsPathoTestParameterVO _ParameterDetails = null;
        public clsPathoTestParameterVO ParameterDetails
        {
            get { return _ParameterDetails; }
            set { _ParameterDetails = value; }
        }

        #endregion


        List<MasterListItem> _HelpValueList = new List<MasterListItem>();
        public List<MasterListItem> HelpValueList
        {
            get
            {
                return _HelpValueList;
            }
            set
            {
                if (value != _HelpValueList)
                {
                    _HelpValueList = value;
                }
            }

        }
        public int TotalRows { get; set; }
        public string DetailID { get; set; }
        public string TestID { get; set; }
        public long CategoryID { get; set; }
        public string FootNote { get; set; }
        public string Note { get; set; }
        public long AgeInDays { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MultipleSampleNo { get; set; }
        public string SampleNo { get; set; }
        [DefaultValue(false)]
        public bool IsForResultEntryLog { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        // Added by Anumani on 22.02.2016 
        public long PatientId { get; set; }
        public long PatientUnitId { get; set; }

        //





        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoTestParameterAndSubTestDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    public class clsGetHelpValuesFroResultEntryBizActionVO : IBizActionValueObject
    {
        List<clsPathoTestParameterVO> _HelpValueList = new List<clsPathoTestParameterVO>();
        public List<clsPathoTestParameterVO> HelpValueList
        {
            get
            {
                return _HelpValueList;
            }
            set
            {
                if (value != _HelpValueList)
                {
                    _HelpValueList = value;
                }
            }

        }

        public long ParameterID { get; set; }
        public clsPathoTestParameterVO ParameterDetails { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetHelpValuesFroResultEntryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    #region For Pathology Additions

    //Added BY Somnath
    public class clsGetPathOrderBookingDetailReportDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathOrderBookingDetailReportDetailsBizAction";
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
        public long OrderID { get; set; }
        public long UnitID { get; set; }
        public bool CheckExtraCriteria { get; set; }
        public bool CheckSampleType { get; set; }
        public bool SampleType { get; set; }
        public bool CheckUploadStatus { get; set; }
        public bool IsUploaded { get; set; }
        public bool CheckDeliveryStatus { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsForReport { get; set; }
        private List<clsPathOrderBookingDetailVO> objOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathOrderBookingDetailVO> OrderBookingDetailList
        {
            get { return objOrderBookingDetail; }
            set { objOrderBookingDetail = value; }
        }

        private clsPathOrderBookingDetailVO _OrderBookingDetail = new clsPathOrderBookingDetailVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathOrderBookingDetailVO OrderBookingDetail
        {
            get { return _OrderBookingDetail; }
            set { _OrderBookingDetail = value; }
        }
    }
    //End
    #endregion

    //added by rohini dated 12.2.16 for test detail

    public class clsGetPathOrderTestDetailListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathOrderTestDetailListBizAction";
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
        public long OrderID { get; set; }
        public long UnitID { get; set; }
         public long TestID { get; set; }
         public string SampleNo { get; set; }
         public long OrderDetailID { get; set; }
        

        private List<clsPathOrderBookingDetailVO> _objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> objOutsourceOrderBookingDetail
        {
            get { return _objOutsourceOrderBookingDetail; }
            set { _objOutsourceOrderBookingDetail = value; }
        }

        //added by rohini dated 11.2.16

        private clsPathOrderBookingDetailVO Detail = null;
        public clsPathOrderBookingDetailVO OrderDetail
        {
            get { return Detail; }
            set { Detail = value; }
        }

        //
       


        private List<clsPathOrderBookingDetailVO> objCollectionOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathOrderBookingDetailVO> CollectionOrderBookingDetailList
        {
            get { return objCollectionOrderBookingDetail; }
            set { objCollectionOrderBookingDetail = value; }
        }
        private List<clsPathOrderBookingDetailVO> objReslutEntryEditList = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> ReslutEntryEditList
        {
            get { return objReslutEntryEditList; }
            set { objReslutEntryEditList = value; }
        }
        private List<clsPathOrderBookingDetailVO> objAuthorizedOrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> AuthorizedOrderBookingDetailList
        {
            get { return objAuthorizedOrderBookingDetailList; }
            set { objAuthorizedOrderBookingDetailList = value; }
        }
        private List<clsPathOrderBookingDetailVO> objResultOrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> ResultOrderBookingDetailList
        {
            get { return objResultOrderBookingDetailList; }
            set { objResultOrderBookingDetailList = value; }
        }

        //private List<clsPathOrderBookingDetailVO> objDispatchOrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();      
        //public List<clsPathOrderBookingDetailVO> DispatchOrderBookingDetailList
        //{
        //    get { return objDispatchOrderBookingDetailList; }
        //    set { objDispatchOrderBookingDetailList = value; }
        //}

        //private List<clsPathOrderBookingDetailVO> objReciveOrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();       
        //public List<clsPathOrderBookingDetailVO> ReciveOrderBookingDetailList
        //{
        //    get { return objReciveOrderBookingDetailList; }
        //    set { objReciveOrderBookingDetailList = value; }
        //}
        //private List<clsPathOrderBookingDetailVO> objAcceptRejectOrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
        //public List<clsPathOrderBookingDetailVO> AcceptRejectOrderBookingDetailList
        //{
        //    get { return objAcceptRejectOrderBookingDetailList; }
        //    set { objAcceptRejectOrderBookingDetailList = value; }
        //}
        //private List<clsPathOrderBookingDetailVO> objOutsourceOrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();   
        //public List<clsPathOrderBookingDetailVO> OutsourceOrderBookingDetailList
        //{
        //    get { return objOutsourceOrderBookingDetailList; }
        //    set { objOutsourceOrderBookingDetailList = value; }
        //}
       
    }


    public class clsGetDispachReciveDetailListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDispachReciveDetailListBizAction";
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
     
        //added by rohini dated 11.2.16

        private clsPathOrderBookingDetailVO Detail = null;
        public clsPathOrderBookingDetailVO OrderDetail
        {
            get { return Detail; }
            set { Detail = value; }
        }

        // 
         public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public int TotalRows { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string BatchNo { get; set; }
        public string SampleNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MRNo { get; set; }
        public long ClinicID { get; set; }
        public Boolean IsPending { get; set; }
        public Boolean IsCollected { get; set; }
        public long IsDispatchByID { get; set; }
        public long SampleStatus { get; set; }
        public int BillType { get; set; }
        public long SampleAcceptRejectBy { get; set; }
        public long UnitID { get; set; }
        public bool IsSampleDispatched { get; set; }
        public bool IsAcceptReject { get; set; }
        private List<clsPathOrderBookingDetailVO> objOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
    
        public List<clsPathOrderBookingDetailVO> OrderBookingDetailList
        {
            get { return objOrderBookingDetail; }
            set { objOrderBookingDetail = value; }
        }
        private List<clsPathOrderBookingDetailVO> objOrderBookingList = new List<clsPathOrderBookingDetailVO>();

        public List<clsPathOrderBookingDetailVO> OrderBookingList
        {
            get { return objOrderBookingList; }
            set { objOrderBookingList = value; }
        }
        
    }
    //
    //Added By Anumani
    public class clsGetPreviousParameterValueBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPreviousParameterValueBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsPathoTestParameterVO PathoTestParameter
        {
            get;
            set;
        }

        public clsPathOrderBookingDetailVO PathTestId
        {
            get;
            set;
        }
        public clsPathOrderBookingVO PathPatientDetail
        {
            get;
            set;
        }

        private long _ParameterId;
        public long ParameterID
        {
            get { return _ParameterId; }
            set
            {
                if (_ParameterId != value)
                {
                    _ParameterId = value;
                }
            }
        }

        private string _ParameterName;
        public string ParameterValue
        {
            get { return _ParameterName; }
            set
            {
                if (_ParameterName != value)
                {
                    _ParameterName = value;
                }
            }

        }

        private string _ResultValue;
        public string ResultValue
        {
            get { return _ResultValue; }
            set
            {
                if (_ResultValue != value)
                {
                    _ResultValue = value;
                }
            }
        }

        public List<clsGetPreviousParameterValueBizActionVO> ParameterList
        {
            get;
            set;
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set{
                if (_Date != value)
                {
                    _Date = value;
                }
            }
        }



    }

    public class clsGetDeltaCheckValueBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDeltaCheckValueBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsPathoTestParameterVO PathoTestParameter
        {
            get;
            set;
        }

        public clsPathOrderBookingDetailVO PathTestId
        {
            get;
            set;
        }
        public clsPathOrderBookingVO PathPatientDetail
        {
            get;
            set;
        }

        public string DetailID { get; set; }
        private long _ParameterId;
        public long ParameterID
        {
            get { return _ParameterId; }
            set
            {
                if (_ParameterId != value)
                {
                    _ParameterId = value;
                }
            }
        }

        private string _ParameterName;
        public string ParameterValue
        {
            get { return _ParameterName; }
            set
            {
                if (_ParameterName != value)
                {
                    _ParameterName = value;
                }
            }

        }

        private string _CurrentValue;
        public string CurrentValue
        {
            get { return _CurrentValue; }
            set
            {
                if (_CurrentValue != value)
                {
                    _CurrentValue = value;
                }
            }
        }

        private string _ResultValue;
        public string ResultValue
        {
            get { return _ResultValue; }
            set
            {
                if (_ResultValue != value)
                {
                    _ResultValue = value;
                }
            }
        }

        private float _MasterDeltaValue;
        public float MasterDeltaValue
        {
            get { return _MasterDeltaValue; }
            set
            {
                if (_MasterDeltaValue != value)
                {
                    _MasterDeltaValue = value;

                }
            }
        }

        private double _CalDeltaValue;
        public double CalDeltaValue
        {
            get { return _CalDeltaValue; }
            set
            {
                if (_CalDeltaValue != value)
                {
                    _CalDeltaValue = value;

                }
            }
        }

        private string _CheckDeltaFlag;
        public string CheckDeltaFlag
        {
            get { return _CheckDeltaFlag; }
            set
            {
                if (_CheckDeltaFlag != value)
                {
                    _CheckDeltaFlag = value;

                }
            }
        }
        public List<clsGetDeltaCheckValueBizActionVO> List
        {
            get;
            set;
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                }
            }
        }



    }
    public class clsGetReflexTestingServiceParameterBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetReflexTestingServiceParameterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private long _ServiceId;
        public long ServiceID
        {
            get { return _ServiceId; }
            set
            {
                if (_ServiceId != value)
                {
                    _ServiceId = value;
                }
            }

        }

        private long _ParameterId;
        public long ParameterID
        {
            get { return _ParameterId; }
            set
            {
                if (_ParameterId != value)
                {
                    _ServiceId = value;
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                }
            }
        }


        private string _ParameterName;
        public string ParameterName
        {
            get { return _ParameterName; }
            set
            {
                if (_ParameterName != value)
                {
                    _ParameterName = value;
                }
            }
        }

        public List<clsGetReflexTestingServiceParameterBizActionVO> ServiceList
    {  get;
        set;
    }

    }
}
