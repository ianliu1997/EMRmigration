using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    
     public class clsGetReceivedListAgainstReturnBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetReceivedAgainstReturnListBizAction";
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

        public long? ReceivedFromStoreId { get; set; }
        public long? ReceivedToStoreId { get; set; }
        public long ReturnUnitId { get; set; }
        public DateTime? ReceivedFromDate { get; set; }
        public DateTime? ReceivedToDate { get; set; }

        public String ReceivedNumberSrc { get; set; } // All - Indent - Without Indent

        public List<clsReceivedAgainstReturnListVO> ReceivedList { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public String InputSortExpression { get; set; }
        public long UnitId { get; set; }

    }

     public class clsReceivedAgainstReturnListVO : INotifyPropertyChanged
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


        private long? _ReceivedId;
        public long? ReceivedId
        {
            get
            {
                return _ReceivedId;
            }
            set
            {
                if (value != _ReceivedId)
                {
                    _ReceivedId = value;
                    OnPropertyChanged("ReceivedId");
                }
            }
        }


        private String _ReceivedNumber;
        public String ReceivedNumber
        {
            get
            {
                return _ReceivedNumber;
            }
            set
            {
                if (value != _ReceivedNumber)
                {
                    _ReceivedNumber = value;
                    OnPropertyChanged("ReceivedNumber");
                }
            }
        }


        private DateTime? _ReceivedDate;
        public DateTime? ReceivedDate
        {
            get
            {
                return _ReceivedDate;
            }
            set
            {
                if (value != _ReceivedDate)
                {
                    _ReceivedDate = value;
                    OnPropertyChanged("ReceivedDate");
                }
            }
        }


        private long? _ReceivedFromStoreId;
        public long? ReceivedFromStoreId
        {
            get
            {
                return _ReceivedFromStoreId;
            }
            set
            {
                if (value != _ReceivedFromStoreId)
                {
                    _ReceivedFromStoreId = value;
                    OnPropertyChanged("ReceivedFromStoreId");
                }
            }
        }

        private String _ReceivedFromStoreName;
        public String ReceivedFromStoreName
        {
            get
            {
                return _ReceivedFromStoreName;
            }
            set
            {
                if (value != _ReceivedFromStoreName)
                {
                    _ReceivedFromStoreName = value;
                    OnPropertyChanged("ReceivedFromStoreName");
                }
            }
        }

        private long? _ReceivedToStoreId;
        public long? ReceivedToStoreId
        {
            get
            {
                return _ReceivedToStoreId;
            }
            set
            {
                if (value != _ReceivedToStoreId)
                {
                    _ReceivedToStoreId = value;
                    OnPropertyChanged("ReceivedToStoreId");
                }
            }
        }

        private String _ReceivedToStoreName;
        public String ReceivedToStoreName
        {
            get
            {
                return _ReceivedToStoreName;
            }
            set
            {
                if (value != _ReceivedToStoreName)
                {
                    _ReceivedToStoreName = value;
                    OnPropertyChanged("ReceivedToStoreName");
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
        private long? _ReturnUnitID;
        public long? ReturnUnitID
        {
            get
            {
                return _ReturnUnitID;
            }
            set
            {
                if (value != _ReturnUnitID)
                {
                    _ReturnUnitID = value;
                    OnPropertyChanged("ReturnUnitID");
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

        public long UnitId { get; set; }
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
    }
}
