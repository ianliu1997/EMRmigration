using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsPatientProcDocScheduleDetailsVO : IValueObject, INotifyPropertyChanged
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

        public string Specialization { get; set; }
        public string SpecializationCode { get; set; }
        private string _DoctorCode;
        public string DoctorCode
        {
            get
            {
                return _DoctorCode;
            }
            set
            {
                _DoctorCode = value;
                OnPropertyChanged("DoctorCode");
            }
        }

        private string _ProcedureName;
        public string ProcedureName
        {
            get
            {
                return _ProcedureName;
            }
            set
            {
                _ProcedureName = value;
                OnPropertyChanged("ProcedureName");
            }
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

        private long _ProcedureID;
        public long ProcedureID
        {
            get
            {
                return _ProcedureID;
            }
            set
            {
                _ProcedureID = value;
                OnPropertyChanged("ProcedureID");
            }
        }

        private long _PatientProcScheduleID;
        public long PatientProcScheduleID
        {
            get
            {
                return _PatientProcScheduleID;
            }
            set
            {
                _PatientProcScheduleID = value;
                OnPropertyChanged("PatientProcScheduleID");
            }
        }

        public string docTypeDesc { get; set; }
        private long _DocTypeID;
        public long DocTypeID
        {
            get
            {
                return _DocTypeID;
            }
            set
            {
                _DocTypeID = value;
                OnPropertyChanged("DocTypeID");
            }
        }

        public string docDesc { get; set; }
        private long _DocID;
        public long DocID
        {
            get
            {
                return _DocID;
            }
            set
            {
                _DocID = value;
                OnPropertyChanged("DocID");
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
        private DateTime _StartTime;
        public DateTime StartTime
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

        private DateTime _EndTime;
        public DateTime EndTime
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

        public string _StrStartTime;
        public string StrStartTime
        {
            get
            {
                return _StrStartTime;
            }
            set
            {
                _StrStartTime = value;
            }
        }

        public string _StrEndTime;
        public string StrEndTime
        {
            get
            {
                return _StrEndTime;
            }
            set
            {
                _StrEndTime = value;
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
    }
}
