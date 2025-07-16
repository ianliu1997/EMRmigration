using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Log;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration
{
    public class clsAddRefundBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddRefundBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        // For the Activity Log List
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
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

        private clsRefundVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsRefundVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        #region Refund to Advance 20042017

        public bool IsRefundToAdvance { get; set; }                 
        public long RefundToAdvancePatientID { get; set; }          
        public long RefundToAdvancePatientUnitID { get; set; }

        public bool IsExchangeMaterial { get; set; }
        #endregion
    }


    public class clsGetRefundListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetRefundListBizAction";
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

        public long ID { get; set; }

        public long UnitID { get; set; }

        public long CostingDivisionID { get; set; }

        public long PatientID { get; set; }

        public bool AllCompanies { get; set; }

        public long CompanyID { get; set; }

        public long PatientUnitID { get; set; }

        private List<clsRefundVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsRefundVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }


    public class clsDeleteRefundBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsDeleteRefundBizAction";
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

        public long ID { get; set; }

        public long AdvanceID { get; set; }
    }


    public class clsSendRequestForApprovalVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsSendRequestForApproval";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        #region INotifyPropertyChanged Members
        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public bool IsRefundRequest { get; set; }

        private int _SuccessStatus;        
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsRefundVO objDetails = null;        
        public clsRefundVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }

        }



        private clsAddBillBizActionVO objBillDetails = null;
        public clsAddBillBizActionVO BillDetails
        {
            get { return objBillDetails; }
            set { objBillDetails = value; }
        }

        // For the Activity Log List
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
        }

        #region Properties

        private bool _IsForApproval = false;
        public bool IsForApproval
        {
            get { return _IsForApproval; }
            set
            {
                if (_IsForApproval != value)
                {
                    _IsForApproval = value;
                    OnPropertyChanged("IsForApproval");
                }
            }
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        private long _RefundID;
        public long RefundID
        {
            get { return _RefundID; }
            set
            {
                if (_RefundID != value)
                {
                    _RefundID = value;
                    OnPropertyChanged("RefundID");
                }
            }
        }

        private bool _IsSetApprovalReq = true;
        public bool IsSetApprovalReq
        {
            get { return _IsSetApprovalReq; }
            set
            {
                if (_IsSetApprovalReq != value)
                {
                    _IsSetApprovalReq = value;
                    OnPropertyChanged("IsSetApprovalReq");
                }
            }
        }

        private long _RequestedBy;
        public long RequestedBy
        {
            get { return _RequestedBy; }
            set
            {
                if (_RequestedBy != value)
                {
                    _RequestedBy = value;
                    OnPropertyChanged("RequestedBy");
                }
            }
        }

        private string _RequestedOn = "";
        public string RequestedOn
        {
            get { return _RequestedOn; }
            set
            {
                if (_RequestedOn != value)
                {
                    _RequestedOn = value;
                    OnPropertyChanged("RequestedOn");
                }
            }
        }

        private DateTime? _RequestedDateTime;
        public DateTime? RequestedDateTime
        {
            get { return _RequestedDateTime; }
            set
            {
                if (_RequestedDateTime != value)
                {
                    _RequestedDateTime = value;
                    OnPropertyChanged("RequestedDateTime");
                }
            }
        }

        private bool _IsFirstApproval = true;
        public bool IsFirstApproval
        {
            get { return _IsFirstApproval; }
            set
            {
                if (_IsFirstApproval != value)
                {
                    _IsFirstApproval = value;
                    OnPropertyChanged("IsFirstApproval");
                }
            }
        }

        private long _FirstApprovalyBy;
        public long FirstApprovalyBy
        {
            get { return _FirstApprovalyBy; }
            set
            {
                if (_FirstApprovalyBy != value)
                {
                    _FirstApprovalyBy = value;
                    OnPropertyChanged("FirstApprovalyBy");
                }
            }
        }

        private string _FirstApprovalOn = "";
        public string FirstApprovalOn
        {
            get { return _FirstApprovalOn; }
            set
            {
                if (_FirstApprovalOn != value)
                {
                    _FirstApprovalOn = value;
                    OnPropertyChanged("FirstApprovalOn");
                }
            }
        }

        private DateTime? _FirstApprovalDateTime;
        public DateTime? FirstApprovalDateTime
        {
            get { return _FirstApprovalDateTime; }
            set
            {
                if (_FirstApprovalDateTime != value)
                {
                    _FirstApprovalDateTime = value;
                    OnPropertyChanged("FirstApprovalDateTime");
                }
            }
        }
        
        private bool _IsSecondApproval = true;
        public bool IsSecondApproval
        {
            get { return _IsSecondApproval; }
            set
            {
                if (_IsSecondApproval != value)
                {
                    _IsSecondApproval = value;
                    OnPropertyChanged("IsSecondApproval");
                }
            }
        }

        private long _SecondApprovalyBy;
        public long SecondApprovalyBy
        {
            get { return _SecondApprovalyBy; }
            set
            {
                if (_SecondApprovalyBy != value)
                {
                    _SecondApprovalyBy = value;
                    OnPropertyChanged("SecondApprovalyBy");
                }
            }
        }

        private string _SecondApprovalOn = "";
        public string SecondApprovalOn
        {
            get { return _SecondApprovalOn; }
            set
            {
                if (_SecondApprovalOn != value)
                {
                    _SecondApprovalOn = value;
                    OnPropertyChanged("SecondApprovalOn");
                }
            }
        }

        private DateTime? _SecondApprovalDateTime;
        public DateTime? SecondApprovalDateTime
        {
            get { return _SecondApprovalDateTime; }
            set
            {
                if (_SecondApprovalDateTime != value)
                {
                    _SecondApprovalDateTime = value;
                    OnPropertyChanged("SecondApprovalDateTime");
                }
            }
        }



        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }        

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        #endregion


        private List<clsChargeVO> ObjList = null;
        public List<clsChargeVO> ChargeList
        {
            get { return ObjList; }
            set { ObjList = value; }
        }

        private List<clsChargeVO> ObjApprovalList = null;
        public List<clsChargeVO> ApprovalChargeList
        {
            get { return ObjApprovalList; }
            set { ObjApprovalList = value; }
        }
        //Added By Bhushanp 23032017 For Refund aginst Bill

        private bool _IsAgainstBill;
        public bool IsAgainstBill
        {
            get { return _IsAgainstBill; }
            set { _IsAgainstBill = value; }
        }

        private double _RefundAmount;

        public double RefundAmount
        {
            get { return _RefundAmount; }
            set { _RefundAmount = value; }
        }

        private bool _IsAdvanceRefund;

        public bool IsAdvanceRefund
        {
            get { return _IsAdvanceRefund; }
            set { _IsAdvanceRefund = value; }
        }
    }

    public class clsApproveSendRequestVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsApproveSendRequest";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        #region INotifyPropertyChanged Members
        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        // For the Activity Log List
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
        }

        private clsAddBillBizActionVO objBillDetails = null;
        public clsAddBillBizActionVO BillDetails
        {
            get { return objBillDetails; }
            set { objBillDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsRefundVO objDetails = null;
        public clsRefundVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        #region Properties
        public bool IsRefundRequest { get; set; }

        private bool _IsForApproval = false;
        public bool IsForApproval
        {
            get { return _IsForApproval; }
            set
            {
                if (_IsForApproval != value)
                {
                    _IsForApproval = value;
                    OnPropertyChanged("IsForApproval");
                }
            }
        }
        private long _ApprovalRequestID;
        public long ApprovalRequestID
        {
            get { return _ApprovalRequestID; }
            set
            {
                if (_ApprovalRequestID != value)
                {
                    _ApprovalRequestID = value;
                    OnPropertyChanged("ApprovalRequestID");
                }
            }
        }
        private long _ApprovalRequestUnitID;
        public long ApprovalRequestUnitID
        {
            get { return _ApprovalRequestUnitID; }
            set
            {
                if (_ApprovalRequestUnitID != value)
                {
                    _ApprovalRequestUnitID = value;
                    OnPropertyChanged("ApprovalRequestUnitID");
                }
            }
        }
        private long _ApprovedRequestID;
        public long ApprovedRequestID
        {
            get { return _ApprovedRequestID; }
            set
            {
                if (_ApprovedRequestID != value)
                {
                    _ApprovedRequestID = value;
                    OnPropertyChanged("ApprovedRequestID");
                }
            }
        }
        private long _ApprovedRequestUnitID;
        public long ApprovedRequestUnitID
        {
            get { return _ApprovedRequestUnitID; }
            set
            {
                if (_ApprovedRequestUnitID != value)
                {
                    _ApprovedRequestUnitID = value;
                    OnPropertyChanged("ApprovedRequestUnitID");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        private long _RefundID;
        public long RefundID
        {
            get { return _RefundID; }
            set
            {
                if (_RefundID != value)
                {
                    _RefundID = value;
                    OnPropertyChanged("RefundID");
                }
            }
        }

        private bool _IsSetApprovalReq = true;
        public bool IsSetApprovalReq
        {
            get { return _IsSetApprovalReq; }
            set
            {
                if (_IsSetApprovalReq != value)
                {
                    _IsSetApprovalReq = value;
                    OnPropertyChanged("IsSetApprovalReq");
                }
            }
        }

        private long _RequestedBy;
        public long RequestedBy
        {
            get { return _RequestedBy; }
            set
            {
                if (_RequestedBy != value)
                {
                    _RequestedBy = value;
                    OnPropertyChanged("RequestedBy");
                }
            }
        }

        private string _RequestedOn = "";
        public string RequestedOn
        {
            get { return _RequestedOn; }
            set
            {
                if (_RequestedOn != value)
                {
                    _RequestedOn = value;
                    OnPropertyChanged("RequestedOn");
                }
            }
        }

        private DateTime? _RequestedDateTime;
        public DateTime? RequestedDateTime
        {
            get { return _RequestedDateTime; }
            set
            {
                if (_RequestedDateTime != value)
                {
                    _RequestedDateTime = value;
                    OnPropertyChanged("RequestedDateTime");
                }
            }
        }

        private bool _IsFirstApproval = true;
        public bool IsFirstApproval
        {
            get { return _IsFirstApproval; }
            set
            {
                if (_IsFirstApproval != value)
                {
                    _IsFirstApproval = value;
                    OnPropertyChanged("IsFirstApproval");
                }
            }
        }

        private long _FirstApprovalyBy;
        public long FirstApprovalyBy
        {
            get { return _FirstApprovalyBy; }
            set
            {
                if (_FirstApprovalyBy != value)
                {
                    _FirstApprovalyBy = value;
                    OnPropertyChanged("FirstApprovalyBy");
                }
            }
        }

        private string _FirstApprovalOn = "";
        public string FirstApprovalOn
        {
            get { return _FirstApprovalOn; }
            set
            {
                if (_FirstApprovalOn != value)
                {
                    _FirstApprovalOn = value;
                    OnPropertyChanged("FirstApprovalOn");
                }
            }
        }

        private DateTime? _FirstApprovalDateTime;
        public DateTime? FirstApprovalDateTime
        {
            get { return _FirstApprovalDateTime; }
            set
            {
                if (_FirstApprovalDateTime != value)
                {
                    _FirstApprovalDateTime = value;
                    OnPropertyChanged("FirstApprovalDateTime");
                }
            }
        }

        private bool _IsSecondApproval = true;
        public bool IsSecondApproval
        {
            get { return _IsSecondApproval; }
            set
            {
                if (_IsSecondApproval != value)
                {
                    _IsSecondApproval = value;
                    OnPropertyChanged("IsSecondApproval");
                }
            }
        }

        private long _SecondApprovalyBy;
        public long SecondApprovalyBy
        {
            get { return _SecondApprovalyBy; }
            set
            {
                if (_SecondApprovalyBy != value)
                {
                    _SecondApprovalyBy = value;
                    OnPropertyChanged("SecondApprovalyBy");
                }
            }
        }
        private long _LevelID;
        public long LevelID
        {
            get { return _LevelID; }
            set
            {
                if (_LevelID != value)
                {
                    _LevelID = value;
                    OnPropertyChanged("LevelID");
                }
            }
        }
        private long _ApprovedByID;
        public long ApprovedByID
        {
            get { return _ApprovedByID; }
            set
            {
                if (_ApprovedByID != value)
                {
                    _ApprovedByID = value;
                    OnPropertyChanged("ApprovedByID");
                }
            }
        }

        private string _ApprovedByName;
        public string ApprovedByName
        {
            get { return _ApprovedByName; }
            set
            {
                if (_ApprovedByName != value)
                {
                    _ApprovedByName = value;
                    OnPropertyChanged("ApprovedByName");
                }
            }
        }
        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }
        private DateTime? _ApprovedDateTime;
        public DateTime? ApprovedDateTime
        {
            get { return _ApprovedDateTime; }
            set
            {
                if (_ApprovedDateTime != value)
                {
                    _ApprovedDateTime = value;
                    OnPropertyChanged("SecondApprovalDateTime");
                }
            }
        }
        private string _SecondApprovalOn = "";
        public string SecondApprovalOn
        {
            get { return _SecondApprovalOn; }
            set
            {
                if (_SecondApprovalOn != value)
                {
                    _SecondApprovalOn = value;
                    OnPropertyChanged("SecondApprovalOn");
                }
            }
        }
        
             private string _ApprovalRemark ;
        public string ApprovalRemark
        {
            get { return _ApprovalRemark; }
            set
            {
                if (_ApprovalRemark != value)
                {
                    _ApprovalRemark = value;
                    OnPropertyChanged("ApprovalRemark");
                }
            }
        }
        private DateTime? _SecondApprovalDateTime;
        public DateTime? SecondApprovalDateTime
        {
            get { return _SecondApprovalDateTime; }
            set
            {
                if (_SecondApprovalDateTime != value)
                {
                    _SecondApprovalDateTime = value;
                    OnPropertyChanged("SecondApprovalDateTime");
                }
            }
        }



        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        #endregion


        private List<clsChargeVO> ObjList = null;
        public List<clsChargeVO> ChargeList
        {
            get { return ObjList; }
            set { ObjList = value; }
        }

        private List<clsChargeVO> ObjApprovalList = null;
        public List<clsChargeVO> ApprovalChargeList
        {
            get { return ObjApprovalList; }
            set { ObjApprovalList = value; }
        }
        //Added By Bhushanp 23032017 For Refund

        private double _RefundAmount;
        
        public double RefundAmount
        {
            get { return _RefundAmount; }
            set { _RefundAmount = value; }
        }
        //Added By Bhushanp 01062017 For Advance Refund

        private bool _IsAdvanceRefund;

        public bool IsAdvanceRefund
        {
            get { return _IsAdvanceRefund; }
            set { _IsAdvanceRefund = value; }
        }
    }


    public class clsDeleteApprovalRequestVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsDeleteApprovalRequest";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        #region INotifyPropertyChanged Members
        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private clsAddBillBizActionVO objBillDetails = null;
        public clsAddBillBizActionVO BillDetails
        {
            get { return objBillDetails; }
            set { objBillDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsRefundVO objDetails = null;
        public clsRefundVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        #region Properties
        public bool IsRefundRequest { get; set; }

        private bool _IsForApproval = false;
        public bool IsForApproval
        {
            get { return _IsForApproval; }
            set
            {
                if (_IsForApproval != value)
                {
                    _IsForApproval = value;
                    OnPropertyChanged("IsForApproval");
                }
            }
        }
        private long _ApprovalRequestID;
        public long ApprovalRequestID
        {
            get { return _ApprovalRequestID; }
            set
            {
                if (_ApprovalRequestID != value)
                {
                    _ApprovalRequestID = value;
                    OnPropertyChanged("ApprovalRequestID");
                }
            }
        }
        private long _ApprovalRequestUnitID;
        public long ApprovalRequestUnitID
        {
            get { return _ApprovalRequestUnitID; }
            set
            {
                if (_ApprovalRequestUnitID != value)
                {
                    _ApprovalRequestUnitID = value;
                    OnPropertyChanged("ApprovalRequestUnitID");
                }
            }
        }
        private long _ApprovedRequestID;
        public long ApprovedRequestID
        {
            get { return _ApprovedRequestID; }
            set
            {
                if (_ApprovedRequestID != value)
                {
                    _ApprovedRequestID = value;
                    OnPropertyChanged("ApprovedRequestID");
                }
            }
        }
        private long _ApprovedRequestUnitID;
        public long ApprovedRequestUnitID
        {
            get { return _ApprovedRequestUnitID; }
            set
            {
                if (_ApprovedRequestUnitID != value)
                {
                    _ApprovedRequestUnitID = value;
                    OnPropertyChanged("ApprovedRequestUnitID");
                }
            }
        }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        private long _RefundID;
        public long RefundID
        {
            get { return _RefundID; }
            set
            {
                if (_RefundID != value)
                {
                    _RefundID = value;
                    OnPropertyChanged("RefundID");
                }
            }
        }

        private bool _IsSetApprovalReq = true;
        public bool IsSetApprovalReq
        {
            get { return _IsSetApprovalReq; }
            set
            {
                if (_IsSetApprovalReq != value)
                {
                    _IsSetApprovalReq = value;
                    OnPropertyChanged("IsSetApprovalReq");
                }
            }
        }

        private long _RequestedBy;
        public long RequestedBy
        {
            get { return _RequestedBy; }
            set
            {
                if (_RequestedBy != value)
                {
                    _RequestedBy = value;
                    OnPropertyChanged("RequestedBy");
                }
            }
        }

        private string _RequestedOn = "";
        public string RequestedOn
        {
            get { return _RequestedOn; }
            set
            {
                if (_RequestedOn != value)
                {
                    _RequestedOn = value;
                    OnPropertyChanged("RequestedOn");
                }
            }
        }

        private DateTime? _RequestedDateTime;
        public DateTime? RequestedDateTime
        {
            get { return _RequestedDateTime; }
            set
            {
                if (_RequestedDateTime != value)
                {
                    _RequestedDateTime = value;
                    OnPropertyChanged("RequestedDateTime");
                }
            }
        }

        private bool _IsFirstApproval = true;
        public bool IsFirstApproval
        {
            get { return _IsFirstApproval; }
            set
            {
                if (_IsFirstApproval != value)
                {
                    _IsFirstApproval = value;
                    OnPropertyChanged("IsFirstApproval");
                }
            }
        }

        private long _FirstApprovalyBy;
        public long FirstApprovalyBy
        {
            get { return _FirstApprovalyBy; }
            set
            {
                if (_FirstApprovalyBy != value)
                {
                    _FirstApprovalyBy = value;
                    OnPropertyChanged("FirstApprovalyBy");
                }
            }
        }

        private string _FirstApprovalOn = "";
        public string FirstApprovalOn
        {
            get { return _FirstApprovalOn; }
            set
            {
                if (_FirstApprovalOn != value)
                {
                    _FirstApprovalOn = value;
                    OnPropertyChanged("FirstApprovalOn");
                }
            }
        }

        private DateTime? _FirstApprovalDateTime;
        public DateTime? FirstApprovalDateTime
        {
            get { return _FirstApprovalDateTime; }
            set
            {
                if (_FirstApprovalDateTime != value)
                {
                    _FirstApprovalDateTime = value;
                    OnPropertyChanged("FirstApprovalDateTime");
                }
            }
        }

        private bool _IsSecondApproval = true;
        public bool IsSecondApproval
        {
            get { return _IsSecondApproval; }
            set
            {
                if (_IsSecondApproval != value)
                {
                    _IsSecondApproval = value;
                    OnPropertyChanged("IsSecondApproval");
                }
            }
        }

        private long _SecondApprovalyBy;
        public long SecondApprovalyBy
        {
            get { return _SecondApprovalyBy; }
            set
            {
                if (_SecondApprovalyBy != value)
                {
                    _SecondApprovalyBy = value;
                    OnPropertyChanged("SecondApprovalyBy");
                }
            }
        }
        private long _LevelID;
        public long LevelID
        {
            get { return _LevelID; }
            set
            {
                if (_LevelID != value)
                {
                    _LevelID = value;
                    OnPropertyChanged("LevelID");
                }
            }
        }
        private long _ApprovedByID;
        public long ApprovedByID
        {
            get { return _ApprovedByID; }
            set
            {
                if (_ApprovedByID != value)
                {
                    _ApprovedByID = value;
                    OnPropertyChanged("ApprovedByID");
                }
            }
        }

        private string _ApprovedByName;
        public string ApprovedByName
        {
            get { return _ApprovedByName; }
            set
            {
                if (_ApprovedByName != value)
                {
                    _ApprovedByName = value;
                    OnPropertyChanged("ApprovedByName");
                }
            }
        }
        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }
        private DateTime? _ApprovedDateTime;
        public DateTime? ApprovedDateTime
        {
            get { return _ApprovedDateTime; }
            set
            {
                if (_ApprovedDateTime != value)
                {
                    _ApprovedDateTime = value;
                    OnPropertyChanged("SecondApprovalDateTime");
                }
            }
        }
        private string _SecondApprovalOn = "";
        public string SecondApprovalOn
        {
            get { return _SecondApprovalOn; }
            set
            {
                if (_SecondApprovalOn != value)
                {
                    _SecondApprovalOn = value;
                    OnPropertyChanged("SecondApprovalOn");
                }
            }
        }

        private string _ApprovalRemark;
        public string ApprovalRemark
        {
            get { return _ApprovalRemark; }
            set
            {
                if (_ApprovalRemark != value)
                {
                    _ApprovalRemark = value;
                    OnPropertyChanged("ApprovalRemark");
                }
            }
        }
        private DateTime? _SecondApprovalDateTime;
        public DateTime? SecondApprovalDateTime
        {
            get { return _SecondApprovalDateTime; }
            set
            {
                if (_SecondApprovalDateTime != value)
                {
                    _SecondApprovalDateTime = value;
                    OnPropertyChanged("SecondApprovalDateTime");
                }
            }
        }



        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        #endregion


        private List<clsChargeVO> ObjList = null;
        public List<clsChargeVO> ChargeList
        {
            get { return ObjList; }
            set { ObjList = value; }
        }

        private List<clsChargeVO> ObjApprovalList = null;
        public List<clsChargeVO> ApprovalChargeList
        {
            get { return ObjApprovalList; }
            set { ObjApprovalList = value; }
        }


    }


    //Begin::Added by AniketK on 30-Jan-2019
    public class clsGetRefundReceiptListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetRefundReceiptListBizAction";
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

        public long BillID { get; set; }

        public long BillUnitID { get; set; }

        public long Opd_Ipd_External { get; set; } //Added by AniketK on 15JAN2018

        private List<clsRefundVO> objDetails = null;
        public List<clsRefundVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

    }
    //End::Added by AniketK on 30-Jan-2019

}
