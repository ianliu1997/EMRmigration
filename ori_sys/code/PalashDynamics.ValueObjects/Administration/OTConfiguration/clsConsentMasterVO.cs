using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsConsentMasterVO : IValueObject, INotifyPropertyChanged
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
        private string _TemplateName;
        public string TemplateName
        {
            get
            {
                return _TemplateName;
            }
            set
            {
                _TemplateName = value;
                OnPropertyChanged("TemplateName");
            }
        }
    }
    public class clsConsentDetailsVO : IValueObject, INotifyPropertyChanged
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

        public String PatientInfoHTML { get; set; }

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

        private long _ConsentSummaryID;
        public long ConsentSummaryID
        {
            get
            {
                return _ConsentSummaryID;
            }
            set
            {
                _ConsentSummaryID = value;
                OnPropertyChanged("ConsentSummaryID");
            }
        }

        private long _ConsentType;
        public long ConsentType
        {
            get
            {
                return _ConsentType;
            }
            set
            {
                _ConsentType = value;
                OnPropertyChanged("ConsentType");
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

        private string _TemplateName;
        public string TemplateName
        {
            get
            {
                return _TemplateName;
            }
            set
            {
                _TemplateName = value;
                OnPropertyChanged("TemplateName");
            }
        }

        private long _PatientContachNo;
        public long PatientContachNo
        {
            get
            {
                return _PatientContachNo;
            }
            set
            {
                _PatientContachNo = value;
                OnPropertyChanged("PatientContachNo");
            }
        }

        private long _KinMobileNo;
        public long KinMobileNo
        {
            get
            {
                return _KinMobileNo;
            }
            set
            {
                _KinMobileNo = value;
                OnPropertyChanged("KinMobileNo");
            }
        }

        private string _MRNo;
        public string MRNo
        {
            get
            {
                return _MRNo;
            }
            set
            {
                _MRNo = value;
                OnPropertyChanged("MRNo");
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
        private string _MiddleName;
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                _MiddleName = value;
                OnPropertyChanged("MiddleName");
            }
        }


        private string _VisitorFirstName;
        public string VisitorFirstName
        {
            get
            {
                return _VisitorFirstName;
            }
            set
            {
                _VisitorFirstName = value;
                OnPropertyChanged("VisitorFirstName");
            }
        }
        private string _VisitorMiddleName;
        public string VisitorMiddleName
        {
            get
            {
                return _VisitorMiddleName;
            }
            set
            {
                _VisitorMiddleName = value;
                OnPropertyChanged("VisitorMiddleName");
            }
        }
        private string _VisitorLastName;
        public string VisitorLastName
        {
            get
            {
                return _VisitorLastName;
            }
            set
            {
                _VisitorLastName = value;
                OnPropertyChanged("VisitorLastName");
            }
        }
        private string _DoctorFirstName;
        public string DoctorFirstName
        {
            get
            {
                return _DoctorFirstName;
            }
            set
            {
                _DoctorFirstName = value;
                OnPropertyChanged("DoctorFirstName");
            }
        }
        private string _DoctoLastName;
        public string DoctoLastName
        {
            get
            {
                return _DoctoLastName;
            }
            set
            {
                _DoctoLastName = value;
                OnPropertyChanged("DoctoLastName");
            }
        }
        private string _DoctorMiddleName;
        public string DoctorMiddleName
        {
            get
            {
                return _DoctorMiddleName;
            }
            set
            {
                _DoctorMiddleName = value;
                OnPropertyChanged("DoctorMiddleName");
            }
        }


        private string _Location;
        public string Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
                OnPropertyChanged("Location");
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

        private string _PatientAddress;
        public string PatientAddress
        {
            get
            {
                return _PatientAddress;
            }
            set
            {
                _PatientAddress = value;
                OnPropertyChanged("PatientAddress");
            }
        }

        private string _KinName;
        public string KinName
        {
            get
            {
                return _KinName;
            }
            set
            {
                _KinName = value;
                OnPropertyChanged("KinName");
            }
        }

        private string _KinAddress;
        public string KinAddress
        {
            get
            {
                return _KinAddress;
            }
            set
            {
                _KinAddress = value;
                OnPropertyChanged("KinAddress");
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
        private string _Gender;
        public string Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    OnPropertyChanged("Gender");
                }
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

        private int _Opd_Ipd;
        public int Opd_Ipd
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

        private string _Consent;
        public string Consent
        {
            get
            {
                return _Consent;
            }
            set
            {
                _Consent = value;
                OnPropertyChanged("Consent");
            }
        }

        private DateTime _Date;
        public DateTime Date
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

        private int _Age;
        public int Age
        {
            get
            {
                return _Age;
            }
            set
            {
                _Age = value;
                OnPropertyChanged("Age");
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

        private long _ScheduleID;
        public long ScheduleID
        {
            get
            {
                return _ScheduleID;
            }
            set
            {
                _ScheduleID = value;
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


        #region Common Properties

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

        private string _DOB;
        public string DOB
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
        #endregion

    }
}
