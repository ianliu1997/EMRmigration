using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
   public class clsIPDDischargeVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
        }

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

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }
        private string _DischargeRemark;
        public string DischargeRemark
        {
            get { return _DischargeRemark; }
            set
            {
                if (_DischargeRemark != value)
                {
                    _DischargeRemark = value;
                    OnPropertyChanged("DischargeRemark");
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

        private long _IPDAdmissionID;
        public long IPDAdmissionID
        {
            get { return _IPDAdmissionID; }
            set
            {
                if (_IPDAdmissionID != value)
                {
                    _IPDAdmissionID = value;
                    OnPropertyChanged("IPDAdmissionID");
                }
            }
        }

        private string _IPDAdmissionNo;
        public string IPDAdmissionNo
        {
            get { return _IPDAdmissionNo; }
            set
            {
                if (_IPDAdmissionNo != value)
                {
                    _IPDAdmissionNo = value;
                    OnPropertyChanged("IPDAdmissionNo");
                }
            }
        }

        private DateTime _DischargeDate;
        public DateTime DischargeDate
        {
            get { return _DischargeDate; }
            set
            {
                if (_DischargeDate != value)
                {
                    _DischargeDate = value;
                    OnPropertyChanged("DischargeDate");
                }
            }
        }

        private DateTime _DischargeTime;
        public DateTime DischargeTime
        {
            get { return _DischargeTime; }
            set
            {
                if (_DischargeTime != value)
                {
                    _DischargeTime = value;
                    OnPropertyChanged("DischargeTime");
                }
            }
        }
        private bool _IsDeathdischarge = true;
        public bool IsDeathdischarge
        {
            get { return _IsDeathdischarge; }
            set
            {
                if (_IsDeathdischarge != value)
                {
                    _IsDeathdischarge = value;
                    OnPropertyChanged("IsDeathdischarge");
                }
            }
        }

        private long _DischargeDoctor;
        public long DischargeDoctor
        {
            get { return _DischargeDoctor; }
            set
            {
                if (_DischargeDoctor != value)
                {
                    _DischargeDoctor = value;
                    OnPropertyChanged("DischargeDoctor");
                }
            }
        }


        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (_DoctorName != value)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }


        private string _Destination;
        public string Destination
        {
            get { return _Destination; }
            set
            {
                if (_Destination != value)
                {
                    _Destination = value;
                    OnPropertyChanged("Destination");
                }
            }
        }

        private long _DischargeType;
        public long DischargeType
        {
            get { return _DischargeType; }
            set
            {
                if (_DischargeType != value)
                {
                    _DischargeType = value;
                    OnPropertyChanged("DischargeType");
                }
            }
        }



        private long _DischargeDestination;
        public long DischargeDestination
        {
            get { return _DischargeDestination; }
            set
            {
                if (_DischargeDestination != value)
                {
                    _DischargeDestination = value;
                    OnPropertyChanged("DischargeDestination");
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

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = strFirstName + " " + strMiddleName + " " + strLastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
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

        
        private DateTime _DateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }


        private long _GenderID;
        public long GenderID
        {
            get { return _GenderID; }
            set
            {
                if (_GenderID != value)
                {
                    _GenderID = value;
                    OnPropertyChanged("GenderID");
                }
            }
        }


        private long _AdmID;
        public long AdmID
        {
            get { return _AdmID; }
            set
            {
                if (_AdmID != value)
                {
                    _AdmID = value;
                    OnPropertyChanged("_AdmID");
                }
            }
        }

        private long _rownum;
        public long rownum
        {
            get { return _rownum; }
            set
            {
                if (_rownum != value)
                {
                    _rownum = value;
                    OnPropertyChanged("_rownum");
                }
            }
        }
       

        private long _AdmUnitID;
        public long AdmUnitID
        {
            get { return _AdmUnitID; }
            set
            {
                if (_AdmUnitID != value)
                {
                    _AdmUnitID = value;
                    OnPropertyChanged("_AdmUnitID");
                }
            }
        }


        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (_DepartmentID != value)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("_DepartmentID");
                }
            }
        }


        private string _DepartmentName;
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set
            {
                if (_DepartmentName != value)
                {
                    _DepartmentName = value;
                    OnPropertyChanged("_DepartmentName");
                }
            }
        }


        private string _AdviseAuthorityName;
        public string AdviseAuthorityName
        {
            get { return _AdviseAuthorityName; }
            set
            {
                if (_AdviseAuthorityName != value)
                {
                    _AdviseAuthorityName = value;
                    OnPropertyChanged("_AdviseAuthorityName");
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
                    OnPropertyChanged("_Remark");
                }
            }
        }







        #endregion

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

    }
}
