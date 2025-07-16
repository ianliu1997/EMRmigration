using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    
    public class clsReceivedItemVO : IValueObject, INotifyPropertyChanged
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

        private decimal? _TotalTaxAmount;
        public decimal? TotalTaxAmount
        {
            get
            {
                return _TotalTaxAmount;
            }
            set
            {
                if (value != _TotalTaxAmount)
                {
                    _TotalTaxAmount = value;
                    OnPropertyChanged("TotalTaxAmount");
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
        //By Anjali................................................
        //private bool _IsIndent;
        //public bool IsIndent
        //{
        //    get
        //    {
        //        return _IsIndent;
        //    }
        //    set
        //    {
        //        if (value != _IsIndent)
        //        {
        //            _IsIndent = value;
        //            OnPropertyChanged("IsIndent");

        //        }
        //    }
        //}
        private int _IsIndent;
        public int IsIndent
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
        //...................................................................
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


        private List<clsReceivedItemDetailsVO> _ReceivedItemDetailsList = new List<clsReceivedItemDetailsVO>();
        public List<clsReceivedItemDetailsVO> ReceivedItemDetailsList
        {
            get
            {
                return _ReceivedItemDetailsList;
            }
            set
            {
                if (value != _ReceivedItemDetailsList)
                {
                    _ReceivedItemDetailsList = value;
                    OnPropertyChanged("ReceivedItemDetailsList");
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
