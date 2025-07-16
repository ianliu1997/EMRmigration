using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.Agency
{
    public class clsAgencyClinicLinkVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
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

        #region Property Declaration
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

        private long _AgencyID;
        public long AgencyID
        {
            get
            {
                return _AgencyID;
            }
            set
            {
                _AgencyID = value;
                OnPropertyChanged("AgencyID");
            }
        }

        private long _ApplicableUnitID;
        public long ApplicableUnitID
        {
            get
            {
                return _ApplicableUnitID;
            }
            set
            {
                _ApplicableUnitID = value;
                OnPropertyChanged("ApplicableUnitID");
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

        private long _ClinicID;
        public long ClinicID
        {
            get
            {
                return _ClinicID;
            }
            set
            {
                _ClinicID = value;
                OnPropertyChanged("ClinicID");
            }
        }

        private long _SpecializationID;
        public long SpecializationID
        {
            get
            {
                return _SpecializationID;
            }
            set
            {
                _SpecializationID = value;
                OnPropertyChanged("SpecializationID");
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

        private string _ClinicName;
        public string ClinicName
        {
            get
            {
                return _ClinicName;
            }
            set
            {
                _ClinicName = value;
                OnPropertyChanged("ClinicName");
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

        private bool _IsDefault;
        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }
            set
            {
                _IsDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        private bool _IsModifyLink = false;
        public bool IsModifyLink
        {
            get
            {
                return _IsModifyLink;
            }
            set
            {
                _IsModifyLink = value;
                OnPropertyChanged("IsModifyLink");
            }
        }
        #endregion

        #region Common Property Declaration
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
        #endregion
    }
}
