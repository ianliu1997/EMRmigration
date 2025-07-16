using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetReturnListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetReturnListBizAction";
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

        public long? ReturnFromStoreId { get; set; }
        public long? ReturnToStoreId { get; set; }
        public long ReturnUnitId { get; set; }

        public DateTime? ReturnFromDate { get; set; }
        public DateTime? ReturnToDate { get; set; }

        public String ReturnNumberSrc { get; set; } // All - Indent - Without Indent

        public List<clsReturnListVO> ReturnList { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public String InputSortExpression { get; set; }
        public long UnitId { get; set; }
        public long UserId { get; set; }
        public InventoryTransactionType transactionType { get; set; }

    }

    public class clsReturnListVO : INotifyPropertyChanged
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


        private long? _ReturnId;
        public long? ReturnId
        {
            get
            {
                return _ReturnId;
            }
            set
            {
                if (value != _ReturnId)
                {
                    _ReturnId = value;
                    OnPropertyChanged("ReturnId");
                }
            }
        }


        private String _ReturnNumber;
        public String ReturnNumber
        {
            get
            {
                return _ReturnNumber;
            }
            set
            {
                if (value != _ReturnNumber)
                {
                    _ReturnNumber = value;
                    OnPropertyChanged("ReturnNumber");
                }
            }
        }


        private DateTime? _ReturnDate;
        public DateTime? ReturnDate
        {
            get
            {
                return _ReturnDate;
            }
            set
            {
                if (value != _ReturnDate)
                {
                    _ReturnDate = value;
                    OnPropertyChanged("ReturnDate");
                }
            }
        }

        public long UnitId { get; set; }
        private long? _ReturnFromStoreId;
        public long? ReturnFromStoreId
        {
            get
            {
                return _ReturnFromStoreId;
            }
            set
            {
                if (value != _ReturnFromStoreId)
                {
                    _ReturnFromStoreId = value;
                    OnPropertyChanged("ReturnFromStoreId");
                }
            }
        }

        private String _ReturnFromStoreName;
        public String ReturnFromStoreName
        {
            get
            {
                return _ReturnFromStoreName;
            }
            set
            {
                if (value != _ReturnFromStoreName)
                {
                    _ReturnFromStoreName = value;
                    OnPropertyChanged("ReturnFromStoreName");
                }
            }
        }

        private long? _ReturnToStoreId;
        public long? ReturnToStoreId
        {
            get
            {
                return _ReturnToStoreId;
            }
            set
            {
                if (value != _ReturnToStoreId)
                {
                    _ReturnToStoreId = value;
                    OnPropertyChanged("ReturnToStoreId");
                }
            }
        }

        private String _ReturnToStoreName;
        public String ReturnToStoreName
        {
            get
            {
                return _ReturnToStoreName;
            }
            set
            {
                if (value != _ReturnToStoreName)
                {
                    _ReturnToStoreName = value;
                    OnPropertyChanged("ReturnToStoreName");
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

        //private long? _ReceivedById;
        //public long? ReceivedById
        //{
        //    get
        //    {
        //        return _ReceivedById;
        //    }
        //    set
        //    {
        //        if (value != _ReceivedById)
        //        {
        //            _ReceivedById = value;
        //            OnPropertyChanged("ReceivedById");
        //        }
        //    }
        //}


        //private String _ReceivedByName;
        //public String ReceivedByName
        //{
        //    get
        //    {
        //        return _ReceivedByName;
        //    }
        //    set
        //    {
        //        if (value != _ReceivedByName)
        //        {
        //            _ReceivedByName = value;
        //            OnPropertyChanged("ReceivedByName");
        //        }
        //    }
        //}


        private bool _IsIndent;
        public bool IsIndent
        {
            get
            {
                return _IsIndent;
            }
            set
            {
                if (value != _IsIndent)
                {
                    _IsIndent = value;
                    OnPropertyChanged("IsIndent");

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

        private string _ReturnByName;
        public string ReturnByName
        {
            get
            {
                return _ReturnByName;
            }
            set
            {
                _ReturnByName = value;
            }
        }
    }

}
