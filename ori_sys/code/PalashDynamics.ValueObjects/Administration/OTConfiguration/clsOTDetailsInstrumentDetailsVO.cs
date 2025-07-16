using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsOTDetailsInstrumentDetailsVO : IValueObject, INotifyPropertyChanged
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

        public string InstrumentDesc { get; set; }
        private long? _InstrumentID;
        public long? InstrumentID
        {
            get
            {
                return _InstrumentID;
            }
            set
            {
                _InstrumentID = value;
                OnPropertyChanged("InstrumentID");
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
            }
        }

    }
}
