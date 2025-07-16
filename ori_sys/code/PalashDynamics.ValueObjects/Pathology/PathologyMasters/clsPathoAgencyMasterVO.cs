using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology.PathologyMasters
{
    public class clsPathoAgencyMasterVO : IValueObject, INotifyPropertyChanged
    {
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
        private string _AgencyTestName = "";
        public string AgencyTestName
        {
            get { return _AgencyTestName; }
            set
            {
                if (value != _AgencyTestName)
                {
                    _AgencyTestName = value;
                    OnPropertyChanged("AgencyTestName");
                }
            }
        }

        private bool _Chk;
        public bool Chk
        {
            get { return _Chk; }
            set
            {
                if (value != _Chk)
                {
                    _Chk = value;
                    OnPropertyChanged("Chk");
                }
            }
        }

        private bool _IsAllSelected;
        public bool IsAllSelected
        {
            get { return _IsAllSelected; }
            set
            {
                if (value != _IsAllSelected)
                {
                    _IsAllSelected = value;
                    OnPropertyChanged("IsAllSelected");
                }
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private bool _IsEnabled=true;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (value != _IsEnabled)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        private bool _IsSaved;
        public bool IsSaved
        {
            get { return _IsSaved; }
            set
            {
                if (value != _IsSaved)
                {
                    _IsSaved = value;
                    OnPropertyChanged("IsSaved");
                }
            }
        }

        //public bool IsAllSelected { get; set; }
        //public bool IsSelected { get; set; }
        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (value != _Code)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }


        private string _Address1;
        public string Address1
        {
            get { return _Address1; }
            set
            {
                if (value != _Address1)
                {
                    _Address1 = value;
                    OnPropertyChanged("Address1");
                }
            }
        }

        private string _Address2;
        public string Address2
        {
            get { return _Address2; }
            set
            {
                if (value != _Address2)
                {
                    _Address2 = value;
                    OnPropertyChanged("Address2");
                }
            }
        }

        private string _Address3;
        public string Address3
        {
            get { return _Address3; }
            set
            {
                if (value != _Address3)
                {
                    _Address3 = value;
                    OnPropertyChanged("Address3");
                }
            }
        }

        private string _City;
        public string City
        {
            get { return _City; }
            set
            {
                if (value != _City)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _Country;
        public string Country
        {
            get { return _Country; }
            set
            {
                if (value != _Country)
                {
                    _Country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private string _State;
        public string State
        {
            get { return _State; }
            set
            {
                if (value != _State)
                {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        private string _Pincode;
        public string Pincode
        {
            get { return _Pincode; }
            set
            {
                if (value != _Pincode)
                {
                    _Pincode = value;
                    OnPropertyChanged("Pincode");
                }
            }
        }

        private string _ContactPerson1Name;
        public string ContactPerson1Name
        {
            get { return _ContactPerson1Name; }
            set
            {
                if (value != _ContactPerson1Name)
                {
                    _ContactPerson1Name = value;
                    OnPropertyChanged("ContactPerson1Name");
                }
            }
        }

        private string _ContactPerson1MobileNo;
        public string ContactPerson1MobileNo
        {
            get { return _ContactPerson1MobileNo; }
            set
            {
                if (value != _ContactPerson1MobileNo)
                {
                    _ContactPerson1MobileNo = value;
                    OnPropertyChanged("ContactPerson1MobileNo");
                }
            }
        }

        private string _ContactPerson1Email;
        public string ContactPerson1Email
        {
            get { return _ContactPerson1Email; }
            set
            {
                if (value != _ContactPerson1Email)
                {
                    _ContactPerson1Email = value;
                    OnPropertyChanged("ContactPerson1Email");
                }
            }
        }

        private string _ContactPerson2Name;
        public string ContactPerson2Name
        {
            get { return _ContactPerson2Name; }
            set
            {
                if (value != _ContactPerson2Name)
                {
                    _ContactPerson2Name = value;
                    OnPropertyChanged("ContactPerson2Name");
                }
            }
        }

        private string _ContactPerson2MobileNo;
        public string ContactPerson2MobileNo
        {
            get { return _ContactPerson2MobileNo; }
            set
            {
                if (value != _ContactPerson2MobileNo)
                {
                    _ContactPerson2MobileNo = value;
                    OnPropertyChanged("ContactPerson2MobileNo");
                }
            }
        }

        private string _ContactPerson2Email;
        public string ContactPerson2Email
        {
            get { return _ContactPerson2Email; }
            set
            {
                if (value != _ContactPerson2Email)
                {
                    _ContactPerson2Email = value;
                    OnPropertyChanged("ContactPerson2Email");
                }
            }
        }

        private string _PhoneNo;
        public string PhoneNo
        {
            get { return _PhoneNo; }
            set
            {
                if (value != _PhoneNo)
                {
                    _PhoneNo = value;
                    OnPropertyChanged("PhoneNo");
                }
            }
        }

        private string _Fax;
        public string Fax
        {
            get { return _Fax; }
            set
            {
                if (value != _Fax)
                {
                    _Fax = value;
                    OnPropertyChanged("Fax");
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

        private long _ApplicableUnitID;
        public long ApplicableUnitID
        {
            get { return _ApplicableUnitID; }
            set
            {
                if (value != _ApplicableUnitID)
                {
                    _ApplicableUnitID = value;
                    OnPropertyChanged("ApplicableUnitID");
                }
            }
        }

        private string _UnitName = "";
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (value != _UnitName)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn;
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
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

        public bool EditMode { get; set; }

        public bool ApplySpecial { get; set; }
        public List<long> UnitIDList { get; set; }
        public string UnitIDs { get; set; }

        public List<MasterListItem> SelectedUnitList = new List<MasterListItem>();
        public List<MasterListItem> SelectedUnitListForService = new List<MasterListItem>();
        public List<MasterListItem> AddSelectedUnitList = new List<MasterListItem>();
   

    }

    public class clsAgencyAppicableUnitsMasterVO : IValueObject, INotifyPropertyChanged
    {

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
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _AgencyID;
        public long AgencyID
        {
            get { return _AgencyID; }
            set
            {
                if (value != _AgencyID)
                {
                    _AgencyID = value;
                    OnPropertyChanged("AgencyID");
                }
            }
        }


        private long _ApplicableUnitID;
        public long ApplicableUnitID
        {
            get { return _ApplicableUnitID; }
            set
            {
                if (value != _ApplicableUnitID)
                {
                    _ApplicableUnitID = value;
                    OnPropertyChanged("ApplicableUnitID");
                }
            }
        }

        private string _ApplicableUnitName = "";
        public string ApplicableUnitName
        {
            get { return _ApplicableUnitName; }
            set
            {
                if (value != _ApplicableUnitName)
                {
                    _ApplicableUnitName = value;
                    OnPropertyChanged("ApplicableUnitName");
                }
            }
        }

        #region CommonFileds


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


        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }

            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }

            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }

            }

        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }

        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }

            }

        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }

        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #endregion


        #region INotifyPropertyChanged Members

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

    public class clsServiceAgencyMasterVO : IValueObject, INotifyPropertyChanged
    {
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
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (value != _ServiceID)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }


        private long _AgencyID;
        public long AgencyID
        {
            get { return _AgencyID; }
            set
            {
                if (value != _AgencyID)
                {
                    _AgencyID = value;
                    OnPropertyChanged("AgencyID");
                }
            }
        }

        private long _ApplicableUnitID;
        public long ApplicableUnitID
        {
            get { return _ApplicableUnitID; }
            set
            {
                if (value != _ApplicableUnitID)
                {
                    _ApplicableUnitID = value;
                    OnPropertyChanged("ApplicableUnitID");
                }
            }
        }

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }


        private string _AgencyName = "";
        public string AgencyName
        {
            get { return _AgencyName; }
            set
            {
                if (value != _AgencyName)
                {
                    _AgencyName = value;
                    OnPropertyChanged("AgencyName");
                }
            }
        }

        private string _AgencyCode = "";
        public string AgencyCode
        {
            get { return _AgencyCode; }
            set
            {
                if (value != _AgencyCode)
                {
                    _AgencyCode = value;
                    OnPropertyChanged("AgencyCode");
                }
            }
        }

        private string _AgencyTestName = "";
        public string AgencyTestName
        {
            get { return _AgencyTestName; }
            set
            {
                if (value != _AgencyTestName)
                {
                    _AgencyTestName = value;
                    OnPropertyChanged("AgencyTestName");
                }
            }
        }

        private string _UnitName = "";
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (value != _UnitName)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        #region CommonFileds


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

        private bool _IsDefaultAgency;
        public bool IsDefaultAgency
        {
            get { return _IsDefaultAgency; }
            set
            {
                if (value != _IsDefaultAgency)
                {
                    _IsDefaultAgency = value;
                    OnPropertyChanged("IsDefaultAgency");
                }

            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }

            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }

            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }

            }

        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }

        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }

            }

        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }

        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        #endregion


        #region INotifyPropertyChanged Members

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
