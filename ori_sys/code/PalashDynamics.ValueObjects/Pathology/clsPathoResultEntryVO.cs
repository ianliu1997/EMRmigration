using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathoResultEntryVO: IValueObject, INotifyPropertyChanged
    {
        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

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

        #region Property Declaration

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

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set
            {
                if (_CategoryID != value)
                {
                    _CategoryID = value;
                    OnPropertyChanged("CategoryID");
                }
            }
        }

        private string _Category;
        public string Category
        {
            get { return _Category; }
            set
            {
                if (_Category != value)
                {
                    _Category = value;
                    OnPropertyChanged("Category");
                }
            }
        }


        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }

      

        private long _ParameterUnitID;
        public long ParameterUnitID
        {
            get { return _ParameterUnitID; }
            set
            {
                if (_ParameterUnitID != value)
                {
                    _ParameterUnitID = value;
                    OnPropertyChanged("ParameterUnitID");
                }
            }
        }
        private string _ParameterUnitName;
        public string ParameterUnitName
        {
            get { return _ParameterUnitName; }
            set
            {
                if (_ParameterUnitName != value)
                {
                    _ParameterUnitName = value;
                    OnPropertyChanged("ParameterUnitName");
                }
            }
        }


        private long _ParameterUnitId;
        public long ParameterUnitId
        {
            get { return _ParameterUnitId; }
            set
            {
                if (_ParameterUnitId != value)
                {
                    _ParameterUnitId = value;
                    OnPropertyChanged("ParameterUnitId");
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

        private long _LabID;
        public long LabID
        {
            get { return _LabID; }
            set
            {
                if (_LabID != value)
                {
                    _LabID = value;
                    OnPropertyChanged("LabID");
                }
            }
        }

        private string _LabName;
        public string LabName
        {
            get { return _LabName; }
            set
            {
                if (_LabName != value)
                {
                    _LabName = value;
                    OnPropertyChanged("LabName");
                }
            }
        }

        private string _sDate;
        public string sDate
        {
            get { return _sDate; }
            set
            {
                if (_sDate != value)
                {
                    _sDate = value;
                    OnPropertyChanged("sDate");
                }
            }
        }

        private DateTime _Date;
        public DateTime Date
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


        private DateTime _Time;
        public DateTime Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private string _EllapsedTime;
        public string EllapsedTime
        {
            get { return _EllapsedTime; }
            set
            {
                if (_EllapsedTime != value)
                {
                    _EllapsedTime = value;
                    OnPropertyChanged("EllapsedTime");
                }
            }
        }

        private string _ResultValue;
        public string ResultValue
        {
            get { return _ResultValue; }
            set
            {
                if (_ResultValue != value)
                {
                    _ResultValue = value;
                    OnPropertyChanged("ResultValue");
                }
            }
        }

        private long _ResultType;
        public long ResultType
        {
            get { return _ResultType; }
            set
            {
                if (_ResultType != value)
                {
                    _ResultType = value;
                    OnPropertyChanged("ResultType");
                }
            }
        }

        private string _ResultTypeName;
        public string ResultTypeName
        {
            get { return _ResultTypeName; }
            set
            {
                if (_ResultTypeName != value)
                {
                    _ResultTypeName = value;
                    OnPropertyChanged("ResultTypeName");
                }
            }
        }

        private bool _IsNumeric;
        public bool IsNumeric
        {
            get { return _IsNumeric; }
            set
            {
                if (_IsNumeric != value)
                {
                    _IsNumeric = value;
                    OnPropertyChanged("IsNumeric");
                }
            }
        }

        public byte[] Attachment { get; set; }

        public string AttachmentFileName { get; set; }

        private string _Note;
        public string Note
        {
            get { return _Note; }
            set
            {
                if (_Note != value)
                {
                    _Note = value;
                    OnPropertyChanged("Note");
                }
            }
        }

        private bool _Status;
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
            }
        }

        // By BHUSHAN..

        private bool _IsAgeApplicable;
        public bool IsAgeApplicable
        {
            get { return _IsAgeApplicable; }
            set
            {
                if (_IsAgeApplicable != value)
                {
                    _IsAgeApplicable = value;
                    OnPropertyChanged("IsAgeApplicable");
                }
            }
        }

        private double _AgeFrom;
        public double AgeFrom
        {
            get { return _AgeFrom; }
            set
            {
                if (_AgeFrom != value)
                {
                    _AgeFrom = value;
                    OnPropertyChanged("AgeFrom");
                }
            }
        }

        private double _AgeTo;
        public double AgeTo
        {
            get { return _AgeTo; }
            set
            {
                if (_AgeTo != value)
                {
                    _AgeTo = value;
                    OnPropertyChanged("AgeTo");
                }
            }
        }

        private double _MinValue;
        public double MinValue
        {
            get { return _MinValue; }
            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;
                    OnPropertyChanged("MinValue");
                }
            }
        }

        private double _MaxValue;
        public double MaxValue
        {
            get { return _MaxValue; }
            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                    OnPropertyChanged("MaxValue");
                }
            }
        }

        private double _DefaultValue;
        public double DefaultValue
        {
            get { return _DefaultValue; }
            set
            {
                if (_DefaultValue != value)
                {
                    _DefaultValue = value;
                    OnPropertyChanged("DefaultValue");
                }
            }
        }


        #endregion


    }
}
