using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDBedMasterVO:IValueObject, INotifyPropertyChanged
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
        public string ToXml()
        {
            return this.ToXml();
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

        private Boolean _IsForReservation;
        public Boolean IsForReservation
        {
            get
            {
                return _IsForReservation;
            }
            set
            {
                _IsForReservation = value;
            }
        }
        
        private long _BedID;
        public long BedID
        {
            get
            {
                return _BedID;
            }
            set
            {
                _BedID = value;
                OnPropertyChanged("BedID");
            }
        }
        private string _Facilities;
        public string Facilities
        {
            get { return _Facilities; }
            set
            {
                _Facilities = value;
                OnPropertyChanged("Facilities");
            }
        }
        private long _GenderID;
        public long GenderID
        {
            get
            {
                return _GenderID;
            }
            set
            {
                _GenderID = value;
                OnPropertyChanged("GenderID");
            }
        }

        private long _BedUnitID;
        public long BedUnitID
        {
            get
            {
                return _BedUnitID;
            }
            set
            {
                _BedUnitID = value;
                OnPropertyChanged("BedUnitID");
            }
        }

        private bool _IsNonCensus;
        public bool IsNonCensus
        {
            get { return _IsNonCensus; }
            set
            {
                _IsNonCensus = value;
                OnPropertyChanged("IsNonCensus");
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

        //private string _UnitName;
        //public string UnitName
        //{
        //    get { return _UnitName; }
        //    set
        //    {
        //        _UnitName = value;
        //        OnPropertyChanged("UnitName");
        //    }
        //}

        #region Commented
        //private long _OTTheatreID;
        //public long OTTheatreID
        //{
        //    get
        //    {
        //        return _OTTheatreID;
        //    }
        //    set
        //    {
        //        _OTTheatreID = value;
        //        OnPropertyChanged("OTTheatreID");
        //    }
        //}

        //public string TheatreName { get; set; }
        #endregion

        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        private long _WardID;
        public long WardID
        {
            get
            {
                return _WardID;
            }
            set
            {
                _WardID = value;
                OnPropertyChanged("WardID");
            }
        }

        private string _WardName;
        public string WardName
        {
            get { return _WardName; }
            set
            {
                _WardName = value;
                OnPropertyChanged("WardName");
            }
        }

        private long _RoomID;
        public long RoomID
        {
            get
            {
                return _RoomID;
            }
            set
            {
                _RoomID = value;
                OnPropertyChanged("RoomID");
            }
        }

        private string _RoomName;
        public string RoomName
        {
            get { return _RoomName; }
            set
            {
                _RoomName = value;
                OnPropertyChanged("RoomName");
            }
        }

        private long _BedCategoryID;
        public long BedCategoryID
        {
            get
            {
                return _BedCategoryID;
            }
            set
            {
                _BedCategoryID = value;
                OnPropertyChanged("BedCategoryID");
            }
        }

        //Use For Billing Class ID
        private long _BillingToBedCategoryID;
        public long BillingToBedCategoryID
        {
            get
            {
                return _BillingToBedCategoryID;
            }
            set
            {
                _BillingToBedCategoryID = value;
                OnPropertyChanged("BillingToBedCategoryID");
            }
        }

        private string _BedCategoryName;
        public string BedCategoryName
        {
            get { return _BedCategoryName; }
            set
            {
                _BedCategoryName = value;
                OnPropertyChanged("BedCategoryName");
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

        private bool _Occupied;
        public bool Occupied
        {
            get
            {
                return _Occupied;
            }
            set
            {
                _Occupied = value;
                OnPropertyChanged("Occupied");
            }
        }

        private List<MasterListItem> _AmmenityDetails;
        public List<MasterListItem> AmmenityDetails
        {
            get
            {
                if (_AmmenityDetails == null)
                    _AmmenityDetails = new List<MasterListItem>();
                return _AmmenityDetails;
            }
            set
            { _AmmenityDetails = value; }
        }

        private bool _IsAmmenity;
        public bool IsAmmenity
        {
            get { return _IsAmmenity; }
            set
            {
                if (value != _IsAmmenity)
                {
                    _IsAmmenity = value;
                    OnPropertyChanged("IsAmmenity");
                }
            }
        }
        
        private double _Rate;
        public double Rate
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

        private long? _CreatedUnitID;
        public long? CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                _CreatedUnitID = value;
                OnPropertyChanged("CreatedUnitID");
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                _UpdatedUnitID = value;
                OnPropertyChanged("UpdatedUnitID");
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                _AddedBy = value;
                OnPropertyChanged("AddedBy");
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                _AddedOn = value;
                OnPropertyChanged("AddedOn");
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                _AddedDateTime = value;
                OnPropertyChanged("AddedDateTime");
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                _UpdatedBy = value;
                OnPropertyChanged("UpdatedBy");
            }
        }


        private string _UpdatedOn;
        public string UpdatedOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {

                _UpdatedOn = value;
                OnPropertyChanged("UpdatedOn");
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                _UpdatedDateTime = value;
                OnPropertyChanged("UpdatedDateTime");
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                _AddedWindowsLoginName = value;
                OnPropertyChanged("AddedWindowsLoginName");
            }
        }

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                _UpdateWindowsLoginName = value;
                OnPropertyChanged("UpdateWindowsLoginName");
            }
        }

        // Added By CDS

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                _PatientName = value;
                OnPropertyChanged("PatientName");
            }
        }

        private string _MrNo;
        public string MrNo
        {
            get { return _MrNo; }
            set
            {
                _MrNo = value;
                OnPropertyChanged("MrNo");
            }
        }

        private DateTime? _AdmissionDate;
        public DateTime? AdmissionDate
        {
            get
            {
                return _AdmissionDate;
            }
            set
            {
                _AdmissionDate = value;
                OnPropertyChanged("AdmissionDate");
            }
        }

        private string _PatientIPDNo;
        public string PatientIPDNo
        {
            get { return _PatientIPDNo; }
            set
            {
                _PatientIPDNo = value;
                OnPropertyChanged("PatientIPDNo");
            }
        }

    }

    public class clsIPDBedAmmenitiesMasterVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToXml();
        }

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

        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
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

        private long? _CreatedUnitID;
        public long? CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                _CreatedUnitID = value;
                OnPropertyChanged("CreatedUnitID");
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                _UpdatedUnitID = value;
                OnPropertyChanged("UpdatedUnitID");
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                _AddedBy = value;
                OnPropertyChanged("AddedBy");
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                _AddedOn = value;
                OnPropertyChanged("AddedOn");
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                _AddedDateTime = value;
                OnPropertyChanged("AddedDateTime");
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                _UpdatedBy = value;
                OnPropertyChanged("UpdatedBy");
            }
        }


        private string _UpdatedOn;
        public string UpdatedOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {

                _UpdatedOn = value;
                OnPropertyChanged("UpdatedOn");
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                _UpdatedDateTime = value;
                OnPropertyChanged("UpdatedDateTime");
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                _AddedWindowsLoginName = value;
                OnPropertyChanged("AddedWindowsLoginName");
            }
        }

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                _UpdateWindowsLoginName = value;
                OnPropertyChanged("UpdateWindowsLoginName");
            }
        }

    }

}
