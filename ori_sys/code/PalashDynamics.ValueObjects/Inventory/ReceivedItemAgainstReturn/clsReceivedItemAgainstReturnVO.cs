using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{

    public class clsReceivedItemAgainstReturnVO : IValueObject, INotifyPropertyChanged
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region Property

        public String LinkServer { get; set; }

        private long? _ReceivedID;
        public long? ReceivedID
        {
            get
            {
                return _ReceivedID;
            }
            set
            {
                if (value != _ReceivedID)
                {
                    _ReceivedID = value;
                    OnPropertyChanged("ReceivedID");
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
        private clsItemStockVO _StockDetails = new clsItemStockVO();
        public clsItemStockVO StockDetails
        {
            get { return _StockDetails; }
            set
            {
                if (_StockDetails != value)
                {
                    _StockDetails = value;
                    OnPropertyChanged("StockDetails");
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

        public long ReturnUnitId { get; set; }
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


        private decimal? _TotalVATAmount;
        public decimal? TotalVATAmount
        {
            get
            {
                return _TotalVATAmount;
            }
            set
            {
                if (value != _TotalVATAmount)
                {
                    _TotalVATAmount = value;
                    OnPropertyChanged("TotalVATAmount");
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


        public long ReturnId { get; set; }

        public bool IsIndent { get; set; }
        private long? _ReturnID;
        public long? ReturnID
        {
            get
            {
                return _ReturnID;
            }
            set
            {
                if (value != _ReturnID)
                {
                    _ReturnID = value;
                    OnPropertyChanged("ReturnID");
                }
            }
        }


        private List<clsReceivedItemAgainstReturnDetailsVO> _ReceivedItemAgainstReturnDetailsList = new List<clsReceivedItemAgainstReturnDetailsVO>();
        public List<clsReceivedItemAgainstReturnDetailsVO> ReceivedItemAgainstReturnDetailsList
        {
            get
            {
                return _ReceivedItemAgainstReturnDetailsList;
            }
            set
            {
                if (value != _ReceivedItemAgainstReturnDetailsList)
                {
                    _ReceivedItemAgainstReturnDetailsList = value;
                    OnPropertyChanged("ReceivedItemAgainstReturnDetailsList");
                }
            }
        }

        private long _ReceivedByID;
        public long ReceivedByID
        {
            get
            {
                return _ReceivedByID;
            }
            set
            {
                if (_ReceivedByID != value)
                    _ReceivedByID = value;
            }
        }

        #endregion
    }
}

