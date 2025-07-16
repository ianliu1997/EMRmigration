using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.ClientDetails
{
    public class clsClientModuleDetailsVO : IValueObject, INotifyPropertyChanged
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

        public string MenuName { get; set; }

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

        private long? _MenuID;
        public long? MenuID
        {
            get { return _MenuID; }
            set
            {
                if (_MenuID != value)
                {
                    _MenuID = value;
                    OnPropertyChanged("MenuID");
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
    }
}
