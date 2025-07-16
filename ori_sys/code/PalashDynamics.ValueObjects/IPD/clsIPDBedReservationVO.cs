using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration.IPD;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDBedReservationVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private List<clsIPDBedMasterVO> _objBedOccupiedList;
        public List<clsIPDBedMasterVO> objBedOccupiedList
        {
            get
            {
                return _objBedOccupiedList;
            }
            set
            {
                if (value != null)
                {
                    _objBedOccupiedList = value;
                }
            }
        }

        private List<clsIPDBedReservationVO> _BedReservation;
        public List<clsIPDBedReservationVO> BedReservation
        {
            get
            {
                return _BedReservation;
            }
            set
            {
                if (value != null)
                {
                    _BedReservation = value;
                }
            }
        }

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

        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }
            }
        }
        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }
        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }
        }
        private DateTime? _CallingDate;
        public DateTime? CallingDate
        {
            get { return _CallingDate; }
            set
            {
                if (_CallingDate != value)
                {
                    _CallingDate = value;
                    OnPropertyChanged("_CallingDate");
                }
            }
        }
        private DateTime? _CallingTime;
        public DateTime? CallingTime
        {
            get { return _CallingTime; }
            set
            {
                if (_CallingTime != value)
                {
                    _CallingTime = value;
                    OnPropertyChanged("_CallingTime");
                }
            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _Source;
        public string Source
        {
            get { return _Source; }
            set
            {
                if (_Source != value)
                {
                    _Source = value;
                    OnPropertyChanged("_Source");
                }
            }
        }


        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    OnPropertyChanged("_UserName");
                }
            }
        }

        private string _BedCode;
        public string BedCode
        {
            get { return _BedCode; }
            set
            {
                if (_BedCode != value)
                {
                    _BedCode = value;
                    OnPropertyChanged("BedCode");
                }
            }
        }

        private string _BedNo;
        public string BedNo
        {
            get { return _BedNo; }
            set
            {
                if (_BedNo != value)
                {
                    _BedNo = value;
                    OnPropertyChanged("BedNo");
                }
            }
        }

        private long _BedID;
        public long BedID
        {
            get { return _BedID; }
            set
            {
                if (_BedID != value)
                {
                    _BedID = value;
                    OnPropertyChanged("BedID");
                }
            }
        }
        private long _BedUnitID;
        public long BedUnitID
        {
            get { return _BedUnitID; }
            set
            {
                if (_BedUnitID != value)
                {
                    _BedUnitID = value;
                    OnPropertyChanged("BedUnitID");
                }
            }
        }


        private long _ClassID;
        public long ClassID
        {
            get { return _ClassID; }
            set
            {
                if (_ClassID != value)
                {
                    _ClassID = value;
                    OnPropertyChanged("ClassID");
                }
            }
        }

        private string _ClassName;
        public string ClassName
        {
            get { return _ClassName; }
            set
            {
                if (_ClassName != value)
                {
                    _ClassName = value;
                    OnPropertyChanged("ClassName");
                }
            }
        }

        private long _WardID;
        public long WardID
        {
            get { return _WardID; }
            set
            {
                if (_WardID != value)
                {
                    _WardID = value;
                    OnPropertyChanged("WardID");
                }
            }
        }

        private string _Ward;
        public string Ward
        {
            get { return _Ward; }
            set
            {
                if (_Ward != value)
                {
                    _Ward = value;
                    OnPropertyChanged("Ward");
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


        private DateTime? _AdmissionDate;
        public DateTime? AdmissionDate
        {
            get { return _AdmissionDate; }
            set
            {
                if (_AdmissionDate != value)
                {
                    _AdmissionDate = value;
                    OnPropertyChanged("AdmissionDate");
                }
            }
        }

        private string strFirstName = "";
        public string FirstName
        {
            get { return strFirstName; }
            set
            {
                if (value != strFirstName)
                {

                    strFirstName = value;
                    OnPropertyChanged("FirstName");


                }

            }
        }
        private string strLastName = "";
        public string LastName
        {
            get { return strLastName; }
            set
            {
                if (value != strLastName)
                {
                    strLastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private string _IPDNo;
        public string IPDNo
        {
            get { return _IPDNo; }
            set
            {
                if (_IPDNo != value)
                {
                    _IPDNo = value;
                    OnPropertyChanged("IPDNo");
                }
            }
        }


        private string strMiddleName = "";

        public string MiddleName
        {
            get { return strMiddleName; }
            set
            {
                if (value != strMiddleName)
                {
                    strMiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        
        
        
        private string strPatientName = "";
        public string PatientName
        {
            get 
            {
                if (strMiddleName == "")
                    return strPatientName = strFirstName + " " + strLastName;
                else
                    return strPatientName = strFirstName + " " + strMiddleName + " " + strLastName;
            }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
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

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        #region Common Properties

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

        private bool _IsFromReminderLog;
        public bool IsFromReminderLog
        {
            get { return _IsFromReminderLog; }
            set
            {
                if (_IsFromReminderLog != value)
                {
                    _IsFromReminderLog = value;
                    OnPropertyChanged("IsFromReminderLog");
                }
            }
        }

        private bool _IsFromReminderLogForGet;
        public bool IsFromReminderLogForGet
        {
            get { return _IsFromReminderLogForGet; }
            set
            {
                if (_IsFromReminderLogForGet != value)
                {
                    _IsFromReminderLogForGet = value;
                    OnPropertyChanged("_IsFromReminderLogForGet");
                }
            }
        }

        private string _UnResRemark;
        public string UnResRemark
        {
            get { return _UnResRemark; }
            set
            {
                if (_UnResRemark != value)
                {
                    _UnResRemark = value;
                    OnPropertyChanged("UnResRemark");
                }
            }
        }
        private bool _UnResStatus = false;
        public bool UnResStatus
        {
            get { return _UnResStatus; }
            set
            {
                if (_UnResStatus != value)
                {
                    _UnResStatus = value;
                    OnPropertyChanged("UnResStatus");
                }
            }
        }

        private bool _IsEnabled = false;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
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
    }

    public class clsIPDBedAmmenitiesMasterVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToXml();
        }

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

    }

    public class clsIPDBedUnReservationVO : IValueObject, INotifyPropertyChanged
    {
        private List<clsIPDBedReservationVO> _BedList;
        public List<clsIPDBedReservationVO> BedList
        {
            get { return _BedList; }
            set { _BedList = value; }
        }

        public string ToXml()
        {
            return this.ToString();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private List<clsIPDBedUnReservationVO> _BedUnReservation;
        public List<clsIPDBedUnReservationVO> BedUnReservation
        {
            get
            {
                return _BedUnReservation;
            }
            set
            {
                if (value != null)
                {
                    _BedUnReservation = value;
                }
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
                if (_ID != value)
                    _ID = value;
                OnPropertyChanged("ID");
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
        private long _BedReservationID;
        public long BedReservationID
        {
            get
            {
                return _BedReservationID;
            }
            set
            {
                if (_BedReservationID != value)
                    _BedReservationID = value;
                OnPropertyChanged("BedReservationID");
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get
            {
                return _UnitId;
            }
            set
            {
                if (_UnitId != value)
                    _UnitId = value;
                OnPropertyChanged("UnitId");
            }
        }

        private long _BedReservationUnitID;
        public long BedReservationUnitID
        {
            get
            {
                return _BedReservationUnitID;
            }
            set
            {
                if (_BedReservationUnitID != value)
                    _BedReservationUnitID = value;
                OnPropertyChanged("BedReservationUnitID");
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


        #region Common Properties

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
    }
}

