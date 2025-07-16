using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.DepartmentScheduleMaster
{
    public class clsDepartmentScheduleVO : IValueObject, INotifyPropertyChanged
    {
        public clsDepartmentScheduleVO()
        {
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if(value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long lngDepartmentID;
        public long DepartmentID
        {
            get { return lngDepartmentID; }
            set
            {
                if(value != lngDepartmentID)
                {
                    lngDepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private string strDepartmentName = null;
        public string DepartmentName
        {
            get { return strDepartmentName; }
            set
            {
                if(value != strDepartmentName)
                {
                    strDepartmentName = value;
                    OnPropertyChanged("DepartmentName");
                }
            }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if(value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        public string UnitName { get; set; }

        //private long lngDepartmentId;
        //public long DepartmentID
        //{
        //    get { return lngDepartmentId; }
        //    set
        //    {
        //        if(value != lngDepartmentId)
        //        {
        //            lngDepartmentId = value;
        //            OnPropertyChanged("DepartmentID");
        //        }
        //    }
        //}

        //public string DepartmentName { get; set; }

        private List<clsDepartmentScheduleDetailsVO> _List;
        public List<clsDepartmentScheduleDetailsVO> DepartmentScheduleDetailsList
        {
            get
            {
                if(_List == null)
                    _List = new List<clsDepartmentScheduleDetailsVO>();

                return _List;
            }

            set
            {

                _List = value;
            }
        }


        #region CommonField

        private bool? blnStatus = null;
        public bool? Status
        {
            get { return blnStatus; }
            set
            {
                if(value != blnStatus)
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
                if(value != _CreatedUnitID)
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
                if(value != _UpdatedUnitID)
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
                if(value != _AddedBy)
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
                if(value != _AddedOn)
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
                if(_AddedDateTime != value)
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
                if(value != _AddedWindowsLoginName)
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
                if(value != _UpdatedBy)
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
                if(value != _UpdatedOn)
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
                if(_UpdatedDateTime != value)
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
                if(value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }


        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsDepartmentScheduleDetailsVO : INotifyPropertyChanged, IValueObject
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if(value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _DepartmentScheduleID;
        public long DepartmentScheduleID
        {
            get { return _DepartmentScheduleID; }
            set
            {
                if(value != _DepartmentScheduleID)
                {
                    _DepartmentScheduleID = value;
                    OnPropertyChanged("DepartmentScheduleID");
                }
            }
        }

        private long _DayID;
        public long DayID
        {
            get { return _DayID; }
            set
            {
                if(value != _DayID)
                {
                    _DayID = value;
                    OnPropertyChanged("DayID");
                }
            }
        }

        private string _Day;
        public string Day
        {
            get { return _Day; }
            set
            {
                if(value != _Day)
                {
                    _Day = value;
                    OnPropertyChanged("Day");
                }
            }
        }

        private long _ScheduleID;
        public long ScheduleID
        {
            get { return _ScheduleID; }
            set
            {
                if(value != _ScheduleID)
                {
                    _ScheduleID = value;
                    OnPropertyChanged("ScheduleID");
                }
            }
        }

        private string _Schedule;
        public string Schedule
        {
            get { return _Schedule; }
            set
            {
                if(value != _Schedule)
                {
                    _Schedule = value;
                    OnPropertyChanged("Schedule");
                }
            }
        }


        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set
            {
                if(value != _StartTime)
                {
                    _StartTime = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }


        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set
            {
                if(value != _EndTime)
                {
                    _EndTime = value;
                    OnPropertyChanged("EndTime");
                }
            }
        }


        private bool _ApplyToAllDay;
        public bool ApplyToAllDay
        {
            get { return _ApplyToAllDay; }
            set
            {
                if(value != _ApplyToAllDay)
                {
                    _ApplyToAllDay = value;
                    OnPropertyChanged("ApplyToAllDay");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if(value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        public string UnitName { get; set; }

        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if(value != _DepartmentID)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private string strDepartmentName = null;
        public string DepartmentName
        {
            get { return strDepartmentName; }
            set
            {
                if(value != strDepartmentName)
                {
                    strDepartmentName = value;
                    OnPropertyChanged("DepartmentName");
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
                if(value != _Status)
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
                if(value != _CreatedUnitID)
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
                if(value != _UpdatedUnitID)
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
                if(value != _AddedBy)
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
                if(value != _AddedOn)
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
                if(value != _AddedDateTime)
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
                if(value != _AddedWindowsLoginName)
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
                if(value != _UpdatedBy)
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
                if(value != _UpdatedOn)
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
                if(value != _UpdatedDateTime)
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
                if(value != _UpdatedWindowsLoginName)
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

            if(null != handler)
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
