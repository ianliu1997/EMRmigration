using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsPatientProcedureScheduleVO : IValueObject, INotifyPropertyChanged
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
        public string OT { get; set; }
        public string OTTable { get; set; }
        public long OTID { get; set; }
        public long OTUnitID { get; set; }
        public long OTTableID { get; set; }
        public long DocID { get; set; }
        public long StaffID { get; set; }

        //***//
        public long BillClearanceID { get; set; }
        public long BillClearanceUnitID { get; set; }
        public bool BillClearanceIsFreezed { get; set; }
        //-----


        private long _ConsentID;
        public long ConsentID
        {
            get
            {
                return _ConsentID;
            }
            set
            {
                _ConsentID = value;
                OnPropertyChanged("ConsentID");
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

        private long _EstimatedProcedureTime;
        public long EstimatedProcedureTime
        {
            get
            {
                return _EstimatedProcedureTime;
            }
            set
            {
                _EstimatedProcedureTime = value;
                OnPropertyChanged("EstimatedProcedureTime");
            }
        }

        private long _OTDetailsUnitID;
        public long OTDetailsUnitID
        {
            get
            {
                return _OTDetailsUnitID;
            }
            set
            {
                _OTDetailsUnitID = value;
                OnPropertyChanged("OTDetailsUnitID");
            }
        }

        private long _OTDetailsID;
        public long OTDetailsID
        {
            get
            {
                return _OTDetailsID;
            }
            set
            {
                _OTDetailsID = value;
                OnPropertyChanged("OTDetailsID");
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

        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
                OnPropertyChanged("PatientID");
            }
        }

        private string _Gender;
        public string Gender
        {
            get
            {
                return _Gender;
            }
            set
            {
                _Gender = value;
                OnPropertyChanged("Gender");
            }
        }


        private DateTime? _DOB;
        public DateTime? DOB
        {
            get
            {
                return _DOB;
            }
            set
            {
                _DOB = value;
                OnPropertyChanged("DOB");
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get
            {
                return _PatientUnitID;
            }
            set
            {
                _PatientUnitID = value;
                OnPropertyChanged("PatientUnitID");
            }
        }

        private long _VisitAdmID;
        public long VisitAdmID
        {
            get
            {
                return _VisitAdmID;
            }
            set
            {
                _VisitAdmID = value;
                OnPropertyChanged("VisitAdmID");
            }
        }

        private long _VisitAdmUnitID;
        public long VisitAdmUnitID
        {
            get
            {
                return _VisitAdmUnitID;
            }
            set
            {
                _VisitAdmUnitID = value;
                OnPropertyChanged("VisitAdmUnitID");
            }
        }

        private bool _Opd_Ipd;
        public bool Opd_Ipd
        {
            get
            {
                return _Opd_Ipd;
            }
            set
            {
                _Opd_Ipd = value;
                OnPropertyChanged("Opd_Ipd");
            }
        }

        private bool _IsCancelled;
        public bool IsCancelled
        {
            get
            {
                return _IsCancelled;
            }
            set
            {
                _IsCancelled = value;
                OnPropertyChanged("IsCancelled");
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

        private string _Remarks;
        public string Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                _Remarks = value;
                OnPropertyChanged("Remarks");
            }
        }
        private string _SpecialRequirement;
        public string SpecialRequirement
        {
            get
            {
                return _SpecialRequirement;
            }
            set
            {
                _SpecialRequirement = value;
                OnPropertyChanged("SpecialRequirement");
            }
        }
        private long _CancelledBy;
        public long CancelledBy
        {
            get
            {
                return _CancelledBy;
            }
            set
            {
                _CancelledBy = value;
                OnPropertyChanged("CancelledBy");
            }
        }

        private string _CancelledReason;
        public string CancelledReason
        {
            get
            {
                return _CancelledReason;
            }
            set
            {
                _CancelledReason = value;
                OnPropertyChanged("CancelledReason");
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

        private bool _IsEmergency;
        public bool IsEmergency
        {
            get
            {
                return _IsEmergency;
            }
            set
            {
                _IsEmergency = value;
                OnPropertyChanged("IsEmergency");
            }
        }


        private bool _IsPACDone;
        public bool IsPACDone
        {
            get
            {
                return _IsPACDone;
            }
            set
            {
                _IsPACDone = value;
                OnPropertyChanged("IsPACDone");
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

        private string _MRNO;
        public string MRNO
        {
            get
            {
                return _MRNO;
            }
            set
            {
                _MRNO = value;
                OnPropertyChanged("MRNO");
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get
            {
                return _DoctorName;
            }
            set
            {
                _DoctorName = value;
                OnPropertyChanged("DoctorName");
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                _PatientName = value;
                OnPropertyChanged("PatientName");
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string _LastName;
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
                OnPropertyChanged("LastName");
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

        private string _MaritalStatus;
        public string MaritalStatus
        {
            get
            {
                return _MaritalStatus;
            }
            set
            {
                _MaritalStatus = value;
                OnPropertyChanged("MaritalStatus");
            }
        }

        private string _Education;
        public string Education
        {
            get
            {
                return _Education;
            }
            set
            {
                _Education = value;
                OnPropertyChanged("Education");
            }
        }

        private string _Religion;
        public string Religion
        {
            get
            {
                return _Religion;
            }
            set
            {
                _Religion = value;
                OnPropertyChanged("Religion");
            }
        }

        private string _ContactNo1;
        public string ContactNo1
        {
            get
            {
                return _ContactNo1;
            }
            set
            {
                _ContactNo1 = value;
                OnPropertyChanged("ContactNo1");
            }
        }

        public byte[] Photo { get; set; }


        //added by neena
        private string _ImageName;
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                if (value != _ImageName)
                {
                    _ImageName = value;
                }
            }
        }

        public string AnesthetistName { get; set; } //***//

        //


        //public Image Photo1 { get; set; }


        private List<clsPatientProcedureVO> _PatientProcList = new List<clsPatientProcedureVO>();
        public List<clsPatientProcedureVO> PatientProcList
        {
            get
            {
                return _PatientProcList;
            }
            set
            {
                _PatientProcList = value;
            }
        }

        private List<clsPatientProcDocScheduleDetailsVO> _DocScheduleList = new List<clsPatientProcDocScheduleDetailsVO>();
        public List<clsPatientProcDocScheduleDetailsVO> DocScheduleList
        {
            get
            {
                return _DocScheduleList;
            }
            set
            {
                _DocScheduleList = value;
            }
        }

        private List<clsPatientProcStaffDetailsVO> _StaffList = new List<clsPatientProcStaffDetailsVO>();
        public List<clsPatientProcStaffDetailsVO> StaffList
        {
            get
            {
                return _StaffList;
            }
            set
            {
                _StaffList = value;
            }
        }

        private List<clsPatientProcedureScheduleVO> _OTScedhuleList = new List<clsPatientProcedureScheduleVO>();
        public List<clsPatientProcedureScheduleVO> OTScedhuleList
        {
            get
            {
                return _OTScedhuleList;
            }
            set
            {
                _OTScedhuleList = value;
            }
        }

        private List<clsPatientProcedureChecklistDetailsVO> _PatientProcCheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        public List<clsPatientProcedureChecklistDetailsVO> PatientProcCheckList
        {
            get
            {
                return _PatientProcCheckList;
            }
            set
            {
                _PatientProcCheckList = value;
            }
        }



        #region Added by Ashutosh For OT scheduling doctor order
        private string _TestName;
        public string TestName
        {
            get { return _TestName; }
            set
            {
                if (_TestName != value)
                {
                    _TestName = value;
                    OnPropertyChanged("TestName");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set
            {
                if (_ServiceCode != value)
                {
                    _ServiceCode = value;
                    OnPropertyChanged("ServiceCode");
                }
            }
        }

        private string _Group;
        public string Group
        {
            get { return _Group; }
            set
            {
                if (_Group != value)
                {
                    _Group = value;
                    OnPropertyChanged("Group");
                }
            }
        }

        private string _PatientType;
        public string PatientType
        {
            get { return _PatientType; }
            set
            {
                if (_PatientType != value)
                {
                    _PatientType = value;
                    OnPropertyChanged("PatientType");
                }
            }
        }


        private string _PriorityName;
        public string PriorityName
        {
            get { return _PriorityName; }
            set
            {
                if (_PriorityName != value)
                {
                    _PriorityName = value;
                    OnPropertyChanged("PriorityName");
                }
            }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }

        #endregion

    }
}
