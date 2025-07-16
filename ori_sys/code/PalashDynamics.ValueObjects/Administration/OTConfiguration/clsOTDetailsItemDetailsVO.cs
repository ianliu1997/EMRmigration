using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsOTDetailsItemDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject
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
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        public long StoreID { get;set;}
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                _UnitID = value;
                OnPropertyChanged("UnitID");
            }
        }

        private long _OTDetailsID;
        public long OTDetailsID
        {
            get
            {
                return _OTDetailsID;
            }
            set
            {
                _OTDetailsID = value;
                OnPropertyChanged("OTDetailsID");
            }
        }

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }

        public string ItemDesc { get; set; }
        public string ItemCode { get; set; }
        private long? _ItemID;
        public long? ItemID
        {
            get
            {
                return _ItemID;
            }
            set
            {
                _ItemID = value;
                OnPropertyChanged("ItemID");
            }
        }

        private long? _BatchID;
        public long? BatchID
        {
            get
            {
                return _BatchID;
            }
            set
            {
                _BatchID = value;
                OnPropertyChanged("BatchID");
            }
        }

        private string _BatchCode;
        public string BatchCode
        {
            get
            {
                return _BatchCode;
            }
            set
            {
                _BatchCode = value;
                OnPropertyChanged("BatchCode");
            }
        }
        private double? _Quantity;
        public double? Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("Amount");
                OnPropertyChanged("VatPer");
                OnPropertyChanged("VatAmt");
                OnPropertyChanged("NetAmount");
            }
        }

        private double? _Rate;
        public double? Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                _Rate = value;
                OnPropertyChanged("Rate");
                OnPropertyChanged("Amount");
                OnPropertyChanged("VatPer");
                OnPropertyChanged("VatAmt");
                OnPropertyChanged("NetAmount");
            }
        }
        private double? _Amount;
        public double? Amount
        {
            get
            {
                return _Quantity*_Rate;
            }
            set
            {
                _Amount = value;
                OnPropertyChanged("Amount");
                OnPropertyChanged("VatPer");
                OnPropertyChanged("VatAmt");
                OnPropertyChanged("NetAmount");
            }
        }
        private double? _VatPer;
        public double? VatPer
        {
            get
            {
                return _VatPer;
            }
            set
            {
                _VatPer = value;
                OnPropertyChanged("VatPer");
                OnPropertyChanged("VatAmt");
                OnPropertyChanged("NetAmount");
            }
        }
        private double? _VatAmt;
        public double? VatAmt
        {
            get
            {

                return ((_Quantity*_Rate)*_VatPer)/100;
            }
            set
            {
                _VatAmt = value;
                OnPropertyChanged("VatAmt");
            }
        }
        private double? _NetAmount;
        public double? NetAmount
        {
            get
            {
                _VatAmt = ((_Quantity*Rate) * _VatPer) / 100;
                return (_Quantity * Rate) + _VatAmt;
            }
            set
            {
                _NetAmount = value;
                OnPropertyChanged("NetAmount");
            }
        }


        private bool _IsPackeged;
        public bool IsPackeged
        {
            get
            {
                return _IsPackeged;
            }
            set
            {
                _IsPackeged = value;
                OnPropertyChanged("IsPackeged");
            }
        }

        private bool _IsConsumed;
        public bool IsConsumed
        {
            get
            {
                return _IsConsumed;
            }
            set
            {
                _IsConsumed = value;
                OnPropertyChanged("IsConsumed");
            }
        }


       
    }
}
