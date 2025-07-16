using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.MachineParameter
{
    public class clsMachineParameterMasterVO : IValueObject, INotifyPropertyChanged
    {
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

        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        #region Common Properties

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
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

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
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
                if (_AddedOn != value)
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
                if (_AddedDateTime != value)
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
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
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
                if (_UpdatedOn != value)
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
                if (_UpdatedDateTime != value)
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
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }
        #endregion

        #region Property Declaration Section

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

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        private string _Description;
        public string ParameterDesc
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _MachineName;
        public string MachineName
        {
            get { return _MachineName; }
            set
            {
                if (_MachineName != value)
                {
                    _MachineName = value;
                    OnPropertyChanged("MachineName");
                }
            }
        }

        private long _MachineId;
        public long MachineId
        {
            get { return _MachineId; }
            set
            {
                if (_MachineId != value)
                {
                    _MachineId = value;
                    OnPropertyChanged("MachineId");
                }
            }
        }

        private bool _Freezed;
        public bool Freezed
        {
            get { return _Freezed; }
            set
            {
                if (_Freezed != value)
                {
                    _Freezed = value;
                    OnPropertyChanged("Freezed");
                }
            }
        }

        #endregion
    }


    public class clsParameterLinkingVO : IValueObject, INotifyPropertyChanged
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

        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        #region Common Properties

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private string _MachineName;
        public string MachineName
        {
            get { return _MachineName; }
            set
            {
                if (_MachineName != value)
                {
                    _MachineName = value;
                    OnPropertyChanged("MachineName");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
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

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
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
                if (_AddedOn != value)
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
                if (_AddedDateTime != value)
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
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
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
                if (_UpdatedOn != value)
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
                if (_UpdatedDateTime != value)
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
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }
        #endregion

        #region Property Declaration Section

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

        private string _MachineParameter;
        public string MachineParameter
        {
            get { return _MachineParameter; }
            set
            {
                if (_MachineParameter != value)
                {
                    _MachineParameter = value;
                    OnPropertyChanged("MachineParameter");
                }
            }
        }

        private long _MachineParameterID;
        public long MachineParameterID
        {
            get { return _MachineParameterID; }
            set
            {
                if (_MachineParameterID != value)
                {
                    _MachineParameterID = value;
                    OnPropertyChanged("MachineParameterID");
                }
            }
        }

        private long _MachineID;
        public long MachineID
        {
            get { return _MachineID; }
            set
            {
                if (_MachineID != value)
                {
                    _MachineID = value;
                    OnPropertyChanged("MachineID");
                }
            }
        }
        private string _ParameterName;
        public string ParameterName
        {
            get { return _ParameterName; }
            set
            {
                if (_ParameterName != value)
                {
                    _ParameterName = value;
                    OnPropertyChanged("ParameterName");
                }
            }
        }

        private long _ParameterID;
        public long ParameterID
        {
            get { return _ParameterID; }
            set
            {
                if (_ParameterID != value)
                {
                    _ParameterID = value;
                    OnPropertyChanged("ParameterID");
                }
            }
        }

        private bool _Freezed;
        public bool Freezed
        {
            get { return _Freezed; }
            set
            {
                if (_Freezed != value)
                {
                    _Freezed = value;
                    OnPropertyChanged("Freezed");
                }
            }
        }

        #endregion

    }
}
