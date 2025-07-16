using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsDoctorShareRangeVO : IValueObject, INotifyPropertyChanged
    {
        public clsDoctorShareRangeVO()
        {
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
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
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private decimal _UpperLimit;
        public decimal UpperLimit
        {
            get { return _UpperLimit; }
            set
            {
                if (value != _UpperLimit)
                {
                    _UpperLimit = value;
                    OnPropertyChanged("UpperLimit");
                }
            }
        }

        private decimal _LowerLimit;
        public decimal LowerLimit
        {
            get { return _LowerLimit; }
            set
            {
                if (value != _LowerLimit)
                {
                    _LowerLimit = value;
                    OnPropertyChanged("LowerLimit");
                }
            }
        }

        private decimal _SharePercentage;
        public decimal SharePercentage
        {
            get { return _SharePercentage; }
            set
            {
                if (value != _SharePercentage)
                {
                    _SharePercentage = value;
                    OnPropertyChanged("SharePercentage");
                }
            }
        }

        private decimal _ShareAmount;
        public decimal ShareAmount
        {
            get { return _ShareAmount; }
            set
            {
                if (value != _ShareAmount)
                {
                    _ShareAmount = value;
                    OnPropertyChanged("ShareAmount");
                }
            }
        }

        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private bool _IsPercentageEnable;
        public bool IsPercentageEnable
        {
            get { return _IsPercentageEnable; }
            set
            {
                if (value != _IsPercentageEnable)
                {
                    _IsPercentageEnable = value;
                    OnPropertyChanged("IsPercentageEnable");
                }
            }
        }

        private bool _IsAmountEnable;
        public bool IsAmountEnable
        {
            get { return _IsAmountEnable; }
            set
            {
                if (value != _IsAmountEnable)
                {
                    _IsAmountEnable = value;
                    OnPropertyChanged("IsAmountEnable");
                }
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }
}
