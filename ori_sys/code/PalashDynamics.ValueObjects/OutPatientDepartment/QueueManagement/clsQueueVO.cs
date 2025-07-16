using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement
{
    public class clsQueueVO : INotifyPropertyChanged, IValueObject
    {

        private long _QueueID;
        public long QueueID
        {
            get
            {
                return _QueueID;
            }

            set
            {
                if (value != _QueueID)
                {
                    _QueueID = value;
                    OnPropertyChanged("QueueID");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get
            {
                return _VisitID;
            }

            set
            {
                if (value != _VisitID)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private string _SortOrder;
        public string SortOrder
        {
            get
            {
                return _SortOrder;
            }

            set
            {
                if (value != _SortOrder)
                {
                    _SortOrder = value;
                    OnPropertyChanged("SortOrder");
                }
            }
        }

        private long _PatientId;
        public long PatientId { get; set; }

        private long _SpecialRegID;
        public long SpecialRegID
        {
            get
            {
                return _SpecialRegID;
            }

            set
            {
                if (value != _SpecialRegID)
                {
                    _SpecialRegID = value;
                    OnPropertyChanged("SpecialRegID");
                }
            }
        }
        private string _SpecialReg;
        public string SpecialReg
        {
            get
            {
                return _SpecialReg;
            }

            set
            {
                if (value != _SpecialReg)
                {
                    _SpecialReg= value;
                    OnPropertyChanged("SpecialReg");
                }
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
                if (value != _PatientUnitID)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private string _OPDNO;
        public string OPDNO { get; set; }

        private string _DepartmentCode;
        public string DepartmentCode
        {
            get { return _DepartmentCode; }
            set
            {
                _DepartmentCode = value;
                OnPropertyChanged("DepartmentCode");
            }
        }

        public string Gender { get; set; }

        public long PatientCategoryId { get; set; }

        private string _NoReg;
        public string NoReg
        {
            get { return _NoReg; }
            set
            {
                _NoReg = value;

                OnPropertyChanged("NoReg");
            }
        }

        private string _PatientType;
        public string PatientType
        {
            get
            {
                return _PatientType;
            }

            set
            {
                if (value != _PatientType)
                {
                    _PatientType = value;
                    OnPropertyChanged("PatientType");
                }
            }
        }

        private DateTime? _DateOfBirth;
        public DateTime? DateOfBirth
        {
            get
            {
                return _DateOfBirth;
            }
            set
            {
                if (value != _DateOfBirth)
                {

                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }

        private string _DoctorCode;
        public string DoctorCode
        {
            get { return _DoctorCode; }
            set { _DoctorCode = value; }
        }

        private string _DeptDescription;
        public string DeptDescription
        {
            get { return _DeptDescription; }
            set { _DeptDescription = value; }
        }

        private long _DepartmentID;
        public long DepartmentID
        {
            get
            {
                return _DepartmentID;
            }

            set
            {
                if (value != _DepartmentID)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }


        private long _DoctorID;
        public long DoctorID
        {
            get
            {
                return _DoctorID;
            }

            set
            {
                if (value != _DoctorID)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private long _CabinID;
         public long CabinID
        {
            get
            {
                return _CabinID;
            }

            set
            {
                if (value != _CabinID)
                {
                    _CabinID = value;
                    OnPropertyChanged("CabinID");
                }
            }
        }

         private long _ReferredDoctorID;
         public long ReferredDoctorID
        {
            get
            {
                return _ReferredDoctorID;
            }

            set
            {
                if (value != _ReferredDoctorID)
                {
                    _ReferredDoctorID = value;
                    OnPropertyChanged("ReferredDoctorID");
                }
            }
        }

         private string _ReferredDoctor;
        public string ReferredDoctor
        {
            get
            {
                return _ReferredDoctor;
            }
            set
            {
                if (value != _ReferredDoctor)
                {
                    _ReferredDoctor = value;
                    OnPropertyChanged("ReferredDoctor");
                }
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
                if (value != _Date)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        

        private string _InTime;
        public string InTime
        {
            get
            {
                return _InTime;
            }
            set
            {
                if (value != _InTime)
                {
                    _InTime = value;
                    OnPropertyChanged("InTime");
                }
            }

        }

        private DateTime? _DateTime;
            public DateTime? DateTime
            {
                get
            {
                return _DateTime;
            }
            set
            {
                if (value != _DateTime)
                {
                    _DateTime = value;
                    OnPropertyChanged("DateTime");
                }
            }
            }

        private string strFirstName;
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

        private string strMiddleName;
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

        private string strLastName;
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
        
        private string _PatientName;
        public string PatientName
        {

            get { return _PatientName = strFirstName + " " + strMiddleName + " " + strLastName; }
           

            set
            {
                if (value != _PatientName)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
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
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private string _Discription;
        public string Discription
        {
            get
            {
                return _Discription;
            }

            set
            {
                if (value != _Discription)
                {
                    _Discription = value;
                    OnPropertyChanged("Discription");
                }
            }
        }

        private string _UnitName;
        public string UnitName
        {
            get
            {
                return _UnitName;
            }

            set
            {
                if (value != _UnitName)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }


        private string _ServiceName;
        public string ServiceName
        {
            get
            {
                return _ServiceName;
            }

            set
            {
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get
            {
                return _ServiceID;
            }

            set
            {
                if (value != _ServiceID)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }


        private bool _IsHealthPackage;
        public bool IsHealthPackage
        {
            get
            {
                return _IsHealthPackage;
            }

            set
            {
                if (value != _IsHealthPackage)
                {
                    _IsHealthPackage = value;
                    OnPropertyChanged("IsHealthPackage");
                }
            }
        }
        private string _Complaints;
        public string Complaints
        {
            get
            {
                return _Complaints;
            }

            set
            {
                if (value != _Complaints)
                {
                    _Complaints = value;
                    OnPropertyChanged("Complaints");
                }
            }
        }

        private string _Cabin;
        public string Cabin
        {
            get
            {
                return _Cabin;
            }

            set
            {
                if (value != _Cabin)
                {
                    _Cabin = value;
                    OnPropertyChanged("Cabin");
                }
            }
        }


        private string _VisitNotes;
        public string VisitNotes
        {
            get
            {
                return _VisitNotes;
            }

            set
            {
                if (value != _VisitNotes)
                {
                    _VisitNotes = value;
                    OnPropertyChanged("VisitNotes");
                }
            }
        }

        private string _VisitType;
        public string VisitType
        {
            get
            {
                return _VisitType;
            }

            set
            {
                if (value != _VisitType)
                {
                    _VisitType = value;
                    OnPropertyChanged("VisitType");
                }
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
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                    OnPropertyChanged("CloseVisit");
                }
            }
        }

        private bool _VisitStatus;
        public bool VisitStatus
                {
                    get
                    {
                        return _VisitStatus;
                    }

                    set
                    {
                        if (value != _VisitStatus)
                        {
                            _VisitStatus = value;
                            OnPropertyChanged("VisitStatus");
                        }
                    }
                }
        
        private long? _VisitTypeID;
        public long? VisitTypeID
        {
            get { return _VisitTypeID; }
            set
            {
                if (_VisitTypeID != value)
                {
                    _VisitTypeID = value;
                    OnPropertyChanged("VisitTypeID");
                }
            }
        }
        

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitId;
        public long UnitID
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        public long TariffID { get; set; }
        public long CompanyID { get; set; }

      
        public string _MRNO;
        public string MRNO
        {
            get { return _MRNO; }
            set { _MRNO = value; }
        }

        public VisitCurrentStatus _CurrentVisitStatus;
        public VisitCurrentStatus CurrentVisitStatus
        {
            get { return _CurrentVisitStatus; }
            set
            {
                if (_CurrentVisitStatus != value)
                {
                    _CurrentVisitStatus = value;
                    OnPropertyChanged("CurrentVisitStatus");
                }
                
            }
        }


        //public string _CrVisitStatus;
        public string CrVisitStatus
        {
            get { return _CurrentVisitStatus.ToString(); }
            //set
            //{
            //    if (_CrVisitStatus != value)
            //    {
            //        _CrVisitStatus = value;
            //        OnPropertyChanged("CrVisitStatus");
            //    }

            //}
        }


        public bool _CloseVisit;
        public bool CloseVisit
        {
            get
            {
                _CloseVisit = !_Status;
                return _CloseVisit; // Math.Round(val, 2).ToString();
              
            }
            set
            {
                if (_CloseVisit != value)
                {
                    _CloseVisit = value;
                    OnPropertyChanged("CloseVisit");
                }

            }
        }

        private string _Remark;
        public string Remark
        {
            get
            {
                return _Remark;
            }

            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        #region Common Property


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

        private string _AddedOn;
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

        private DateTime? _AddedDateTime;
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

        private string _UpdatedOn;
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

        private DateTime? _UpdatedDateTime;
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
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

#endregion

        #region IValueObject Members

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

        //***//-----------------------
        private DateTime? _VisitDate;
        public DateTime? VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (value != _VisitDate)
                {
                    _VisitDate = value;
                    OnPropertyChanged("VisitDate");
                }
            }
        }

        //Add property ContactNo to show on Patient Queue grid : by AniketK on 16/10/2018
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

    }
}
