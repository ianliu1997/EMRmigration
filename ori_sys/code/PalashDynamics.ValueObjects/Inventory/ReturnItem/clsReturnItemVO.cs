using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsReturnItemVO : IValueObject, INotifyPropertyChanged
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
        private long _ReceivedID;
        public long ReceivedID
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
        //By Anjali....................................
       // public bool IsIndent { get; set; }
        public int IsIndent { get; set; }
        //..............................................
        private long? _IssueUnitID;
        public long? IssueUnitID
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


        private long? _IssueID;
        public long? IssueID
        {
            get
            {
                return _IssueID;
            }
            set
            {
                if (value != _IssueID)
                {
                    _IssueID = value;
                    OnPropertyChanged("IssueID");
                }
            }
        }


        private List<clsReturnItemDetailsVO> _ReturnItemDetailsList = new List<clsReturnItemDetailsVO>();
        public List<clsReturnItemDetailsVO> ReturnItemDetailsList
        {
            get
            {
                return _ReturnItemDetailsList;
            }
            set
            {
                if (value != _ReturnItemDetailsList)
                {
                    _ReturnItemDetailsList = value;
                    OnPropertyChanged("ReturnItemDetailsList");
                }
            }
        }

        public bool IsIssue { get; set; }
        #endregion

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
        #endregion
    }
}
