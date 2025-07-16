using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
    public class clsCompoundDrugDetailVO:INotifyPropertyChanged,IValueObject
    {

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyPropertyChanged

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


        #region Properties
        private bool _IsChecked;
        public bool IsChecked
        {
            get 
            {
                return _IsChecked;
            }
            set 
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                  
                }
            }
        }
        private double _AvailableStock;
        public double AvailableStock
        {
            get { return _AvailableStock; }
            set
            {
                if (_AvailableStock != value)
                {
                    _AvailableStock = value;
                    OnPropertyChanged("AvailableStock");
                }
            }
        }
        private string _CompoundDrug;
        public string CompoundDrug
        {
            get
            {
                return _CompoundDrug;
            }
            set
            {
                if (_CompoundDrug != value)
                {
                    _CompoundDrug = value;
                    OnPropertyChanged("CompoundDrug");
                }
            }
        }

        private string _ItemName;
        public String ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }
        private string _ItemCode;
        public String ItemCode
        {
            get
            {
                return _ItemCode;
            }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
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
            get
            {
                return _UnitID;
            }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private long _CompoundDrugID;
        public long CompoundDrugID
        {
            get
            {
                return _CompoundDrugID;
            }
            set
            {
                if (_CompoundDrugID != value)
                {
                    _CompoundDrugID = value;
                    OnPropertyChanged("CompoundDrugID");
                }
            }
        }
        private long _CompoundDrugUnitID;
        public long CompoundDrugUnitID
        {
            get
            {
                return _CompoundDrugUnitID;
            }
            set
            {
                if (_CompoundDrugUnitID != value)
                {
                    _CompoundDrugUnitID = value;
                    OnPropertyChanged("CompoundDrugUnitID");
                }
            }
        }
        private long _ItemID;
        public long ItemID
        {
            get
            {
                return _ItemID;
            }
            set
            {
                if (_ItemID != value)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }
        private long _ItemUnitID;
        public long ItemUnitID
        {
            get
            {
                return _ItemUnitID;
            }
            set
            {
                if (_ItemUnitID != value)
                {
                    _ItemUnitID = value;
                    OnPropertyChanged("ItemUnitID");
                }
            }
        }
        private float _Quantity;
        public float Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }
        #endregion
    }
}
