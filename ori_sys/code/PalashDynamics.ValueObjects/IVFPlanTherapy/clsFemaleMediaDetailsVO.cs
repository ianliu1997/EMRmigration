using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsFemaleMediaDetailsVO : IValueObject, INotifyPropertyChanged
    {
        

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


        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set
            {
                if (_StoreID != value)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }


        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }
        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (value != _ItemID)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private string _Company;
        public string Company
        {
            get { return _Company; }
            set
            {
                if (_Company != value)
                {
                    _Company = value;
                    OnPropertyChanged("Company");
                }
            }
        }


        private string _BatchCode;//Lot Number
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (_BatchCode != value)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }

        private long _BatchID;
        public long BatchID
        {
            get { return _BatchID; }
            set
            {
                if (_BatchID != value)
                {
                    _BatchID = value;
                    OnPropertyChanged("BatchID");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }


        private bool _PH;
        public bool PH 
        {
            get { return _PH; }
            set
            {
                if (_PH != value)
                {
                    _PH = value;
                    OnPropertyChanged("PH ");
                }
            }
        }



        private bool _OSM ;
        public bool OSM
        {
            get { return _OSM; }
            set
            {
                if (_OSM != value)
                {
                    _OSM = value;
                    OnPropertyChanged("OSM ");
                }
            }
        }

        private string _VolumeUsed;
        public string VolumeUsed
        {
            get { return _VolumeUsed; }
            set
            {
                if (_VolumeUsed != value)
                {
                    _VolumeUsed = value;
                    OnPropertyChanged("VolumeUsed ");
                }
            }
        }

        


        List<MasterListItem> _Status = new List<MasterListItem> 
        { 
            new MasterListItem{ ID=0,Description="--Select--"} ,
            new MasterListItem{ ID=1,Description="Used"} ,
            new MasterListItem{ ID=2,Description="Descarded"} ,
            new MasterListItem{ ID=3,Description="Infection"} ,
            new MasterListItem{ ID=4,Description="FollowUp"} ,
           
        };

        public List<MasterListItem> Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                }
            }

        }

        MasterListItem _SelectedStatus = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedStatus
        {
            get
            {
                return _SelectedStatus;
            }
            set
            {
                if (value != _SelectedStatus)
                {
                    _SelectedStatus = value;
                    OnPropertyChanged("SelectedStatus");
                }
            }


        }

        public long FertilizationID { get; set; }
        public long StatusID { get; set; }
        public long DetailedID { get; set; }
        public long MianID { get; set; }

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
    }
}
