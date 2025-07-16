using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsDoctorScheduleVO : IValueObject, INotifyPropertyChanged
    {
        public clsDoctorScheduleVO()
        {

        }

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

        private long lngDoctorID;
        public long DoctorID
        {
            get { return lngDoctorID; }
            set
            {
                if (value != lngDoctorID)
                {
                    lngDoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }
        private string strDoctorName =null;
        public string DoctorName
        {
            get { return strDoctorName; }
            set
            {
                if (value != strDoctorName)
                {
                    strDoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }


        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        public string UnitName { get; set; }

        private long lngDepartmentId;
        public long DepartmentID
        {
            get { return lngDepartmentId; }
            set
            {
                if (value != lngDepartmentId)
                {
                    lngDepartmentId = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        public string DepartmentName { get; set; }

        private List<clsDoctorScheduleDetailsVO> _List;
        public List<clsDoctorScheduleDetailsVO> DoctorScheduleDetailsList
        {
            get
            {
                if (_List == null)
                    _List = new List<clsDoctorScheduleDetailsVO>();

                return _List;
            }

            set
            {

                _List = value;

            }
        }

        private clsDoctorScheduleDetailsVO _ListItem;
        public clsDoctorScheduleDetailsVO DoctorScheduleDetailsListItem     // added on 14032018 for New Doctor Schedule
        {
            get
            {
                if (_ListItem == null)
                    _ListItem = new clsDoctorScheduleDetailsVO();

                return _ListItem;
            }

            set
            {

                _ListItem = value;

            }
        }

        #region CommonField

        private bool? blnStatus = null;
        public bool? Status
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
                if (_AddedDateTime != value)
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
                if (_UpdatedDateTime != value)
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

        #region For New Doctor Schedule added on 13032018

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set
            {
                if (_StartTime != value)
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
                if (_EndTime != value)
                {
                    _EndTime = value;
                    OnPropertyChanged("EndTime");
                }
            }
        }

        private string _ScheduleType;
        public string ScheduleType
        {
            get { return _ScheduleType; }
            set
            {
                if (value != _ScheduleType)
                {
                    _ScheduleType = value;
                    OnPropertyChanged("ScheduleType");
                }
            }
        }

        #endregion

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
    }

    public class clsDoctorScheduleDetailsVO : INotifyPropertyChanged, IValueObject
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


        private long _DoctorScheduleID;
        public long DoctorScheduleID
        {
            get { return _DoctorScheduleID; }
            set
            {
                if (value != _DoctorScheduleID)
                {
                    _DoctorScheduleID = value;
                    OnPropertyChanged("DoctorScheduleID");
                }
            }
        }

        private long _DayID;
        public long DayID
        {
            get { return _DayID; }
            set
            {
                if (value != _DayID)
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
                if (value != _Day)
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
                if (value != _ScheduleID)
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
                if (value != _Schedule)
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
                if (value != _StartTime)
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
                if (value != _EndTime)
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
                if (value != _ApplyToAllDay)
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
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        public string UnitName { get; set; }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (value != _DoctorID)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private string strDoctorName = null;
        public string DoctorName
        {
            get { return strDoctorName; }
            set
            {
                if (value != strDoctorName)
                {
                    strDoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private long lngDepartmentId;
        public long DepartmentID
        {
            get { return lngDepartmentId; }
            set
            {
                if (value != lngDepartmentId)
                {
                    lngDepartmentId = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private string strDeptName = null;
        public string DepartmentName
        {
            get { return strDeptName; }
            set
            {
                if (value != strDeptName)
                {
                    strDeptName = value;
                    OnPropertyChanged("DepartmentName");
                }
            }
        }

        //added on 14032018 for New Doctor Schedule
        private string _DayIDnew;
        public string DayIDnew
        {
            get { return _DayIDnew; }
            set
            {
                if (value != _DayIDnew)
                {
                    _DayIDnew = value;
                    OnPropertyChanged("DayIDnew");
                }
            }
        }

        //added on 14032018 for New Doctor Schedule
        private long _MonthDayNo;
        public long MonthDayNo
        {
            get { return _MonthDayNo; }
            set
            {
                if (value != _MonthDayNo)
                {
                    _MonthDayNo = value;
                    OnPropertyChanged("MonthDayNo");
                }
            }
        }

        

        //added on 14032018 for New Doctor Schedule
        private bool _IsDayNo;
        public bool IsDayNo
        {
            get { return _IsDayNo; }
            set
            {
                if (value != _IsDayNo)
                {
                    _IsDayNo = value;
                    OnPropertyChanged("IsDayNo");
                }
            }
        }

        //added on 14032018 for New Doctor Schedule
        private long _MonthWeekNoID;
        public long MonthWeekNoID
        {
            get { return _MonthWeekNoID; }
            set
            {
                if (value != _MonthWeekNoID)
                {
                    _MonthWeekNoID = value;
                    OnPropertyChanged("MonthWeekNoID");
                }
            }
        }

        //added on 14032018 for New Doctor Schedule
        private long _MonthWeekDayID;
        public long MonthWeekDayID
        {
            get { return _MonthWeekDayID; }
            set
            {
                if (value != _MonthWeekDayID)
                {
                    _MonthWeekDayID = value;
                    OnPropertyChanged("MonthWeekDayID");
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

    public class clsDoctorDepartmentUnitListVO : IValueObject, INotifyPropertyChanged
    {
        public clsDoctorDepartmentUnitListVO()
        {

        }

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

        private long lngDoctorID;
        public long DoctorID
        {
            get { return lngDoctorID; }
            set
            {
                if (value != lngDoctorID)
                {
                    lngDoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }
        private string strDoctorName =null;
        public string DoctorName
        {
            get { return strDoctorName; }
            set
            {
                if (value != strDoctorName)
                {
                    strDoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }


        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        public string UnitName { get; set; }

        private long lngDepartmentId;
        public long DepartmentID
        {
            get { return lngDepartmentId; }
            set
            {
                if (value != lngDepartmentId)
                {
                    lngDepartmentId = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        public string DepartmentName { get; set; }

        private List<clsDoctorScheduleDetailsVO> _List;
        public List<clsDoctorScheduleDetailsVO> DoctorScheduleDetailsList
        {
            get
            {
                if (_List == null)
                    _List = new List<clsDoctorScheduleDetailsVO>();

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
                if (_AddedDateTime != value)
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
                if (_UpdatedDateTime != value)
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
    }

}
