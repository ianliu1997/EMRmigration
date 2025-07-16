using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetIssueListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetIssueListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {

                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        private clsIssueListVO _IssueDetailsVO = new clsIssueListVO();
        public clsIssueListVO IssueDetailsVO
        {
            get { return _IssueDetailsVO; }
            set
            {
                if (_IssueDetailsVO != value)
                {
                    _IssueDetailsVO = value;
                }
            }
        }

        public long? IssueFromStoreId { get; set; }
        public long? IssueToStoreId { get; set; }

        public DateTime? IssueFromDate { get; set; }
        public DateTime? IssueToDate { get; set; }

        public int? IndentCriteria { get; set; } // All - Indent - Without Indent

        public List<clsIssueListVO> IssueList { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public String InputSortExpression { get; set; }
        public bool flagReceiveIssue { get; set; }
        public bool? IsFromReceiveIssue { get; set; }
        public long? UserID { get; set; }
        public long? UnitID { get; set; }

        public bool IsQSOnly { get; set; } // Use to get already saved Issued Items to Quarantine Stores Only
        public bool IsForGRNQS = false;  // set on ReceiveGRNToQS form while getting Records for Issue.

        public int? intIsIndent { get; set; }

        public long PatientID { get; set; }
        public long PatientunitID { get; set; }
        public bool IsAgainstPatient { get; set; }

        //***//
        public string MRNo { get; set; }
        public string PatientName { get; set; }

    }

    public class clsIssueListVO : INotifyPropertyChanged
    {

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

        private bool? _IsToStoreQuarantine;
        public bool? IsToStoreQuarantine
        {
            get
            {
                return _IsToStoreQuarantine;
            }
            set
            {
                if (value != _IsToStoreQuarantine)
                {
                    _IsToStoreQuarantine = value;
                    OnPropertyChanged("IsToStoreQuarantine");
                }
            }
        }
        private bool? _IsFromStoreQuarantine;
        public bool? IsFromStoreQuarantine
        {
            get
            {
                return _IsFromStoreQuarantine;
            }
            set
            {
                if (value != _IsFromStoreQuarantine)
                {
                    _IsFromStoreQuarantine = value;
                    OnPropertyChanged("IsFromStoreQuarantine");
                }
            }
        }

        private long? _IssueId;
        public long? IssueId
        {
            get
            {
                return _IssueId;
            }
            set
            {
                if (value != _IssueId)
                {
                    _IssueId = value;
                    OnPropertyChanged("IssueId");
                }
            }
        }

       

        private String _IssueNumber;
        public String IssueNumber
        {
            get
            {
                return _IssueNumber;
            }
            set
            {
                if (value != _IssueNumber)
                {
                    _IssueNumber = value;
                    OnPropertyChanged("IssueNumber");
                }
            }
        }


        private DateTime? _IssueDate;
        public DateTime? IssueDate
        {
            get
            {
                return _IssueDate;
            }
            set
            {
                if (value != _IssueDate)
                {
                    _IssueDate = value;
                    OnPropertyChanged("IssueDate");
                }
            }
        }


        private long? _IssueFromStoreId;
        public long? IssueFromStoreId
        {
            get
            {
                return _IssueFromStoreId;
            }
            set
            {
                if (value != _IssueFromStoreId)
                {
                    _IssueFromStoreId = value;
                    OnPropertyChanged("IssueFromStoreId");
                }
            }
        }

        private long _IssueUnitID;
        public long IssueUnitID
        {
            get
            {
                return _IssueUnitID;
            }
            set
            {
                if (value != _IssueUnitID)
                {
                    _IssueUnitID = value;
                    OnPropertyChanged("IssueUnitID");
                }
            }
        }

        private String _IssueFromStoreName;
        public String IssueFromStoreName
        {
            get
            {
                return _IssueFromStoreName;
            }
            set
            {
                if (value != _IssueFromStoreName)
                {
                    _IssueFromStoreName = value;
                    OnPropertyChanged("IssueFromStoreName");
                }
            }
        }

        private long? _IssueToStoreId;
        public long? IssueToStoreId
        {
            get
            {
                return _IssueToStoreId;
            }
            set
            {
                if (value != _IssueToStoreId)
                {
                    _IssueToStoreId = value;
                    OnPropertyChanged("IssueToStoreId");
                }
            }
        }

        private String _IssueToStoreName;
        public String IssueToStoreName
        {
            get
            {
                return _IssueToStoreName;
            }
            set
            {
                if (value != _IssueToStoreName)
                {
                    _IssueToStoreName = value;
                    OnPropertyChanged("IssueToStoreName");
                }
            }
        }


        private decimal? _TotalItems;
        public decimal? TotalItems
        {
            get
            {
                return _TotalItems;
            }
            set
            {
                if (value != _TotalItems)
                {
                    _TotalItems = value;
                    OnPropertyChanged("TotalItems");
                }
            }
        }

        private decimal? _TotalAmount;
        public decimal? TotalAmount
        {
            get
            {
                return _TotalAmount;
            }
            set
            {
                if (value != _TotalAmount)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        private long? _ReceivedById;
        public long? ReceivedById
        {
            get
            {
                return _ReceivedById;
            }
            set
            {
                if (value != _ReceivedById)
                {
                    _ReceivedById = value;
                    OnPropertyChanged("ReceivedById");
                }
            }
        }


        private String _ReceivedByName;
        public String ReceivedByName
        {
            get
            {
                return _ReceivedByName;
            }
            set
            {
                if (value != _ReceivedByName)
                {
                    _ReceivedByName = value;
                    OnPropertyChanged("ReceivedByName");
                }
            }
        }



        private long? _IndentId;
        public long? IndentId
        {
            get
            {
                return _IndentId;
            }
            set
            {
                if (value != _IndentId)
                {
                    _IndentId = value;
                    OnPropertyChanged("IndentId");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        //By Anjali...................................
        public int IsIndent { get; set; }
        //public bool IsIndent { get; set; }
        //........................................

        private String _IndentNumber;
        public String IndentNumber
        {
            get
            {
                return _IndentNumber;
            }
            set
            {
                if (value != _IndentNumber)
                {
                    _IndentNumber = value;
                    OnPropertyChanged("IndentNumber");
                }
            }
        }


        private String _Remark;
        public String Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }


        private String _ReasonForIssueName;  //use to show whether its a issue against Damaged,Obsolete,Scrap,Expired (DOSE)
        public String ReasonForIssueName
        {
            get
            {
                return _ReasonForIssueName;
            }
            set
            {
                if (value != _ReasonForIssueName)
                {
                    _ReasonForIssueName = value;
                    OnPropertyChanged("ReasonForIssueName");
                }
            }
        }

        #region PatientIndent
        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get
            {
                return _PatientUnitID;
            }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private bool _IsAgainstPatient;
        public bool IsAgainstPatient
        {
            get
            {
                return _IsAgainstPatient;
            }
            set
            {
                if (_IsAgainstPatient != value)
                {
                    _IsAgainstPatient = value;
                    OnPropertyChanged("IsAgainstPatient");
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _MRNo;
        public string MRNo
        {
            get
            {
                return _MRNo;
            }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }
            }
        }

        public bool IsApproved { get; set; } //***//
        #endregion

    }


}
