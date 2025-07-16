using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.ClientDetails
{
    public class clsClientSubScriptionDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private long? _ClientID;
        public long? ClientID
        {
            get { return _ClientID; }
            set
            {
                if (_ClientID != value)
                {
                    _ClientID = value;
                    OnPropertyChanged("ClientID");
                }
            }
        }

        private int? _SubscriptionType;
        public int? SubscriptionType
        {
            get { return _SubscriptionType; }
            set
            {
                if (_SubscriptionType != value)
                {
                    _SubscriptionType = value;
                    OnPropertyChanged("SubscriptionType");
                }
            }
        }

        private DateTime? _SubscriptionStartDate;
        public DateTime? SubscriptionStartDate
        {
            get { return _SubscriptionStartDate; }
            set
            {
                if (_SubscriptionStartDate != value)
                {
                    _SubscriptionStartDate = value;
                    OnPropertyChanged("SubscriptionStartDate");
                }
            }
        }

        private DateTime? _SubscriptionEndtDate;
        public DateTime? SubscriptionEndtDate
        {
            get { return _SubscriptionEndtDate; }
            set
            {
                if (_SubscriptionEndtDate != value)
                {
                    _SubscriptionEndtDate = value;
                    OnPropertyChanged("SubscriptionEndtDate");
                }
            }
        }

        private DateTime? _SubscriptionRenewtDate;
        public DateTime? SubscriptionRenewtDate
        {
            get { return _SubscriptionRenewtDate; }
            set
            {
                if (_SubscriptionRenewtDate != value)
                {
                    _SubscriptionEndtDate = value;
                    OnPropertyChanged("SubscriptionRenewtDate");
                }
            }
        }

        private int? _RenewtAlertsDays;
        public int? RenewtAlertsDays
        {
            get { return _RenewtAlertsDays; }
            set
            {
                if (_RenewtAlertsDays != value)
                {
                    _RenewtAlertsDays = value;
                    OnPropertyChanged("RenewtAlertsDays");
                }
            }
        }

        private double? _Amount;
        public double? Amount
        {
            get { return _Amount; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        private bool? _Status;
        public bool? Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private bool? _IsRenewd;
        public bool? IsRenewd
        {
            get { return _IsRenewd; }
            set
            {
                if (_IsRenewd != value)
                {
                    _IsRenewd = value;
                    OnPropertyChanged("IsRenewd");
                }
            }
        }

        private DateTime? _ActivationDate;
        public DateTime? ActivationDate
        {
            get { return _ActivationDate; }
            set
            {
                if (_ActivationDate != value)
                {
                    _ActivationDate = value;
                    OnPropertyChanged("ActivationDate");
                }
            }
        }

      


    }
}
