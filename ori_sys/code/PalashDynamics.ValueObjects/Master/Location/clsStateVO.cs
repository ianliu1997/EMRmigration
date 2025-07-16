using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsStateVO : INotifyPropertyChanged, IValueObject
    {
        public clsStateVO()
        {

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

        #region Property Members
        private long _ID;
        public long StateID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("StateID");
                }
            }
        }


        private long _StateDistID;
        public long StateDistID
        {
            get { return _StateDistID; }
            set
            {
                if (value != _StateDistID)
                {
                    _StateDistID = value;
                    OnPropertyChanged("StateDistID");
                }
            }
        }


        private string _State;
        public string StateName
        {
            get { return _State; }
            set
            {
                if (value != _State)
                {
                    _State = value;
                    OnPropertyChanged("StateName");
                }
            }
        }

        private string _MasterZoneCode;
        public string MasterZoneCode
        {
            get { return _MasterZoneCode; }
            set
            {
                if (value != _MasterZoneCode)
                {
                    _MasterZoneCode = value;
                    OnPropertyChanged("MasterZoneCode");
                }
            }
        }

        private bool _SelectedService;
        public bool SelectedService
        {
            get { return _SelectedService; }
            set
            {
                if (value != _SelectedService)
                {
                    _SelectedService = value;
                    OnPropertyChanged("SelectedService");
                }
            }
        }

        private bool _IsSelectedState=true;
        public bool IsSelectedState
        {
            get { return _IsSelectedState; }
            set
            {
                if (value != _IsSelectedState)
                {
                    _IsSelectedState = value;
                    OnPropertyChanged("IsSelectedState");
                }
            }
        }

        private bool _SelectedState = false;
        public bool SelectedState
        {
            get { return _SelectedState; }
            set
            {
                if (value != _SelectedState)
                {
                    _SelectedState = value;
                    OnPropertyChanged("SelectedState");
                }
            }
        }
        #endregion

        #region CommonField

        private bool blnStatus;
        public bool Status
        {
            get { return blnStatus; }
            set
            {
                if (value != blnStatus)
                {
                    blnStatus = value;
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

        private DateTime? _AddedDateTime = DateTime.Now;
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


        private string _UpdatedBy;
        public string UpdatedBy
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

        private DateTime? _UpdatedDateTime = DateTime.Now;
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

        private string _UpdatedWindowsLoginName;
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

        public override string ToString()
        {
                return StateName;
        }
    }
}
