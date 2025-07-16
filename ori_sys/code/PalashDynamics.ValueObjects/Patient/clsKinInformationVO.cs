using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsKinInformationVO : IValueObject, INotifyPropertyChanged
    {
        private long _KinID;
        public long KinID
        {
            get { return _KinID; }
            set
            {
                if (_KinID != value)
                {
                    _KinID = value;
                    OnPropertyChanged("KinID");
                }
            }
        }

        private long _KinUnitID;
        public long KinUnitID
        {
            get { return _KinUnitID; }
            set
            {
                if (_KinUnitID != value)
                {
                    _KinUnitID = value;
                    OnPropertyChanged("KinUnitID");
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


        private bool _IsRegisteredPatient;
        public bool IsRegisteredPatient
        {
            get { return _IsRegisteredPatient; }
            set
            {
                if (_IsRegisteredPatient != value)
                {
                    _IsRegisteredPatient = value;
                    OnPropertyChanged("IsRegisteredPatient");
                }
            }
        }

        //useful for registered patient
        private long _KinPatientID;
        public long KinPatientID
        {
            get { return _KinPatientID; }
            set
            {
                if (_KinPatientID != value)
                {
                    _KinPatientID = value;
                    OnPropertyChanged("KinPatientID");
                }
            }
        }

        //useful for registered patient
        private long _KinPatientUnitID;
        public long KinPatientUnitID
        {
            get { return _KinPatientUnitID; }
            set
            {
                if (_KinPatientUnitID != value)
                {
                    _KinPatientUnitID = value;
                    OnPropertyChanged("KinPatientUnitID");
                }
            }
        }


        private String _FamilyCode;
        public String FamilyCode
        {
            get { return _FamilyCode; }
            set
            {
                if (_FamilyCode != value)
                {
                    _FamilyCode = value;
                    OnPropertyChanged("FamilyCode");
                }
            }
        }



        private String _MRCode;
        public String MRCode
        {
            get { return _MRCode; }
            set
            {
                if (_MRCode != value)
                {
                    _MRCode = value;
                    OnPropertyChanged("MRCode");
                }
            }
        }


        private String _KinFirstName;
        public String KinFirstName
        {
            get { return _KinFirstName; }
            set
            {
                if (_KinFirstName != value)
                {
                    _KinFirstName = value;
                    OnPropertyChanged("KinFirstName");
                }
            }
        }

        private String _KinMiddleName;
        public String KinMiddleName
        {
            get { return _KinMiddleName; }
            set
            {
                if (_KinMiddleName != value)
                {
                    _KinMiddleName = value;
                    OnPropertyChanged("KinMiddleName");
                }
            }
        }


        private String _KinLastName;
        public String KinLastName
        {
            get { return _KinLastName; }
            set
            {
                if (_KinLastName != value)
                {
                    _KinLastName = value;
                    OnPropertyChanged("KinLastName");
                }
            }
        }


        private String _MemberName;
        public String MemberName
        {
            get { return KinFirstName + " " + KinMiddleName + " " + KinLastName; }
            //set
            //{

            //    _MemberName = KinFirstName + " " + KinMiddleName + " " + KinLastName;

            //}
        }



        private String _KinMobileNumber;
        public String KinMobileNumber
        {
            get { return MobileCountryCode.ToString() + " " + MobileNumber.ToString(); }
            //set
            //{

            //    _KinMobileNumber = MobileCountryCode.ToString() + " " + MobileCountryCode.ToString();

            //}
        }


        private long? _RelationshipID;
        public long? RelationshipID
        {
            get { return _RelationshipID; }
            set
            {
                if (_RelationshipID != value)
                {
                    _RelationshipID = value;
                    OnPropertyChanged("RelationshipID");
                }
            }
        }



        private String _Relationship;
        public String Relationship
        {
            get { return _Relationship; }
            set
            {
                if (_Relationship != value)
                {
                    _Relationship = value;
                    OnPropertyChanged("Relationship");
                }
            }
        }


        private long? _MobileCountryCode;
        public long? MobileCountryCode
        {
            get { return _MobileCountryCode; }
            set
            {
                if (_MobileCountryCode != value)
                {
                    _MobileCountryCode = value;
                    OnPropertyChanged("MobileCountryCode");
                }
            }
        }


        private string _MobileNumber;
        public string MobileNumber
        {
            get { return _MobileNumber; }
            set
            {
                if (_MobileNumber != value)
                {
                    _MobileNumber = value;
                    OnPropertyChanged("MobileNumber");
                }
            }
        }

        private String _Address;
        public String Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    OnPropertyChanged("Address");
                }
            }
        }


        #region Common Properties




        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
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

        public string ToXml()
        {
            return this.ToString();
        }
    }
}
