using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsPreffixMasterVO: INotifyPropertyChanged, IValueObject
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
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
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

        private long _ApplicableGender;
        public long ApplicableGender
        {
            get { return _ApplicableGender; }
            set
            {
                if (value != _ApplicableGender)
                {
                    _ApplicableGender = value;
                    OnPropertyChanged("ApplicableGender");
                }
            }
        }

        private string _GetApplicableGender;
        public string GetApplicableGender
        {
            get { return _GetApplicableGender; }
            set
            {
                if (value != _GetApplicableGender)
                {
                    _GetApplicableGender = value;
                    OnPropertyChanged("GetApplicableGender");
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

        private string _AddOn;
        public string AddOn
        {
            get { return _AddOn; }
            set
            {
                if (value != _AddOn)
                {
                    _AddOn = value;
                    OnPropertyChanged("AddOn");
                }
            }
        }

        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
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

        private DateTime _UpdatedDateTime;
        public DateTime UpdatedDateTime
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

        private long _Synchronized;
        public long Synchronized
        {
            get { return _Synchronized; }
            set
            {
                if (value != _Synchronized)
                {
                    _Synchronized = value;
                    OnPropertyChanged("Synchronized");
                }
            }
        }
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
