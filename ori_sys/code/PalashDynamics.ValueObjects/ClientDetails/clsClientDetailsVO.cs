using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.ClientDetails
{
    public class clsClientDetailsVO : IValueObject, INotifyPropertyChanged
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

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _CustomerCode;
        public string CustomerCode
        {
            get { return _CustomerCode; }
            set
            {
                if (_CustomerCode != value)
                {
                    _CustomerCode = value;
                    OnPropertyChanged("CustomerCode");
                }
            }
        }

        private string _County;
        public string County
        {
            get { return _County; }
            set
            {
                if (_County != value)
                {
                    _County = value;
                    OnPropertyChanged("County");
                }
            }
        }
        private string _State;
        public string State
        {
            get { return _State; }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        private string _City;
        public string City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }
        private string _EmailID;
        public string EmailID
        {
            get { return _EmailID; }
            set
            {
                if (_EmailID != value)
                {
                    _EmailID = value;
                    OnPropertyChanged("EmailID");
                }
            }
        }

       

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        private string _ContactNo1;
        public string ContactNo1
        {
            get { return _ContactNo1; }
            set
            {
                if (_ContactNo1 != value)
                {
                    _ContactNo1 = value;
                    OnPropertyChanged("ContactNo1");
                }
            }
        }

        private string _ContactNo2;
        public string ContactNo2
        {
            get { return _ContactNo2; }
            set
            {
                if (_ContactNo2 != value)
                {
                    _ContactNo2 = value;
                    OnPropertyChanged("ContactNo2");
                }
            }
        }

        private string _FaxNo;
        public string FaxNo
        {
            get { return _FaxNo; }
            set
            {
                if (_FaxNo != value)
                {
                    _FaxNo = value;
                    OnPropertyChanged("FaxNo");
                }
            }
        }

        private string _ContactPerson1;
        public string ContactPerson1
        {
            get { return _ContactPerson1; }
            set
            {
                if (_ContactPerson1 != value)
                {
                    _ContactPerson1 = value;
                    OnPropertyChanged("ContactPerson1");
                }
            }
        }

        private string _ContactPerson2;
        public string ContactPerson2
        {
            get { return _ContactPerson2; }
            set
            {
                if (_ContactPerson2 != value)
                {
                    _ContactPerson2 = value;
                    OnPropertyChanged("ContactPerson2");
                }
            }
        }

        private string _ContactPerson1MobNo;
        public string ContactPerson1MobNo
        {
            get { return _ContactPerson1MobNo; }
            set
            {
                if (_ContactPerson1MobNo != value)
                {
                    _ContactPerson1MobNo = value;
                    OnPropertyChanged("ContactPerson1MobNo");
                }
            }
        }

        private string _ContactPerson2MobNo;
        public string ContactPerson2MobNo
        {
            get { return _ContactPerson2MobNo; }
            set
            {
                if (_ContactPerson2MobNo != value)
                {
                    _ContactPerson2MobNo = value;
                    OnPropertyChanged("ContactPerson2MobNo");
                }
            }
        }

        private int? _ClientType;
        public int? ClientType
        {
            get { return _ClientType; }
            set
            {
                if (_ClientType != value)
                {
                    _ClientType = value;
                    OnPropertyChanged("ClientType");
                }
            }
        }

        private bool? _IsMultilocation;
        public bool? IsMultilocation
        {
            get { return _IsMultilocation; }
            set
            {
                if (_IsMultilocation != value)
                {
                    _IsMultilocation = value;
                    OnPropertyChanged("IsMultilocation");
                }
            }
        }

        private int? _NoOfLocations;
        public int? NoOfLocations
        {
            get { return _NoOfLocations; }
            set
            {
                if (_NoOfLocations != value)
                {
                    _NoOfLocations = value;
                    OnPropertyChanged("NoOfLocations");
                }
            }
        }

        private int? _NoOfUsers;
        public int? NoOfUsers
        {
            get { return _NoOfUsers; }
            set
            {
                if (_NoOfUsers != value)
                {
                    _NoOfUsers = value;
                    OnPropertyChanged("NoOfUsers");
                }
            }
        }

        private string _Address1;
        public string Address1
        {
            get { return _Address1; }
            set
            {
                if (_Address1 != value)
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
                if (_Address2 != value)
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
                if (_Address3 != value)
                {
                    _Address3 = value;
                    OnPropertyChanged("Address3");
                }
            }
        }

        private string _FolderName;
        public string FolderName
        {
            get { return _FolderName; }
            set
            {
                if (_FolderName != value)
                {
                    _FolderName = value;
                    OnPropertyChanged("FolderName");
                }
            }
        }

        private bool? _IsActivated;
        public bool? IsActivated
        {
            get { return _IsActivated; }
            set
            {
                if (_IsActivated != value)
                {
                    _IsActivated = value;
                    OnPropertyChanged("IsActivated");
                }
            }
        }

        private bool? _ActivationDate;
        public bool? ActivationDate
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

        #region detailslist
        private List<clsClientModuleDetailsVO> _ClientModuleList;
        public List<clsClientModuleDetailsVO> ClientModuleList
        {
            get
            {
                if (_ClientModuleList == null)
                    _ClientModuleList = new List<clsClientModuleDetailsVO>();

                return _ClientModuleList;
            }

            set
            {

                _ClientModuleList = value;

            }
        }

        private clsClientSubScriptionDetailsVO _ClientSubscription;
        public clsClientSubScriptionDetailsVO ClientSubscription
        {
            get
            {
                if (_ClientSubscription == null)
                    _ClientSubscription = new clsClientSubScriptionDetailsVO();

                return _ClientSubscription;
            }

            set
            {

                _ClientSubscription = value;

            }
        }
        #endregion
    }
}
