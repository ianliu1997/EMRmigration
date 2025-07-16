using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsPatientProcOTScheduleDetailsVO : IValueObject, INotifyPropertyChanged
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
        private long _PatientProcedureScheduleID;
        public long PatientProcedureScheduleID
        {
            get
            {
                return _PatientProcedureScheduleID;
            }
            set
            {
                _PatientProcedureScheduleID = value;
                OnPropertyChanged("PatientProcedureScheduleID");
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
        public string OTDesc { get; set; }
        private long? _OTID;
        public long? OTID
        {
            get
            {
                return _OTID;
            }
            set
            {
                _OTID = value;
                OnPropertyChanged("OTID");
            }
        }

        public string OTTableDesc { get; set; }
        private long? _OTTableID;
        public long? OTTableID
        {
            get
            {
                return _OTTableID;
            }
            set
            {
                _OTTableID = value;
                OnPropertyChanged("OTTableID");
            }
        }
        private long? _DayID;
        public long? DayID
        {
            get
            {
                return _DayID;
            }
            set
            {
                _DayID = value;
                OnPropertyChanged("DayID");
            }
        }
        private long? _ScheduleID;
        public long? ScheduleID
        {
            get
            {
                return _ScheduleID;
            }
            set
            {
                _ScheduleID = value;
                OnPropertyChanged("ScheduleID");
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                OnPropertyChanged("Date");
            }
        }
        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                _EndTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        private object _ApColor;
        public object ApColor
        {
            get
            {
                return _ApColor;
            }
            set
            {
                _ApColor = value;
                OnPropertyChanged("ApColor");
            }
        }

        private bool? _ApplyToAllDay;
        public bool? ApplyToAllDay
        {
            get
            {
                return _ApplyToAllDay;
            }
            set
            {
                _ApplyToAllDay = value;
                OnPropertyChanged("ApplyToAllDay");
            }
        }
        private bool? _Status;
        public bool? Status
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
        
    }
}
