using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration.IPD;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDBedTransferVO : IValueObject, INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
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

        private bool _IsDischargedBedSave;
        public bool IsDischargedBedSave
        {
            get { return _IsDischargedBedSave; }
            set
            {
                if (_IsDischargedBedSave != value)
                {
                    _IsDischargedBedSave = value;
                    OnPropertyChanged("IsDischargedBedSave");
                }
            }
        }

        private bool _ISDischarged;
        public bool ISDischarged
        {
            get { return _ISDischarged; }
            set
            {
                if (_ISDischarged != value)
                {
                    _ISDischarged = value;
                    OnPropertyChanged("ISDischarged");
                }
            }
        }

        private bool _IsMultipleBed;
        public bool IsMultipleBed
        {
            get { return _IsMultipleBed; }
            set
            {
                if (_IsMultipleBed != value)
                {
                    _IsMultipleBed = value;
                    OnPropertyChanged("IsMultipleBed");
                }
            }
        }

        private bool _IsEnabled;
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

        private DateTime? _TransferDate;
        public DateTime? TransferDate
        {
            get { return _TransferDate; }
            set
            {
                if (_TransferDate != value)
                {
                    _TransferDate = value;
                    OnPropertyChanged("TransferDate");
                }
            }
        }

        private DateTime? _TransferTime;
        public DateTime? TransferTime
        {
            get { return _TransferTime; }
            set
            {
                if (_TransferTime != value)
                {
                    _TransferTime = value;
                    OnPropertyChanged("TransferTime");
                }
            }
        }

        private long _BedCategoryID;
        public long BedCategoryID
        {
            get { return _BedCategoryID; }
            set
            {
                if (_BedCategoryID != value)
                {
                    _BedCategoryID = value;
                    OnPropertyChanged("BedCategoryID");
                }
            }
        }

        private long _BillingBedCategoryID;
        public long BillingBedCategoryID
        {
            get { return _BillingBedCategoryID; }
            set
            {
                if (_BillingBedCategoryID != value)
                {
                    _BillingBedCategoryID = value;
                    OnPropertyChanged("BillingBedCategoryID");
                }
            }
        }

        private string _BedCategory;
        public string BedCategory
        {
            get { return _BedCategory; }
            set
            {
                if (_BedCategory != value)
                {
                    _BedCategory = value;
                    OnPropertyChanged("BedCategory");
                }
            }
        }

        //Use For Billing Class ID
        private long _BillingToBedCategoryID;
        public long BillingToBedCategoryID
        {
            get
            {
                return _BillingToBedCategoryID;
            }
            set
            {
                _BillingToBedCategoryID = value;
                OnPropertyChanged("BillingToBedCategoryID");
            }
        }

        private long _DischargeTypeID;
        public long DischargeTypeID
        {
            get
            {
                return _DischargeTypeID;
            }

            set
            {
                if (value != _DischargeTypeID)
                {
                    _DischargeTypeID = value;
                    OnPropertyChanged("DischargeTypeID");
                }
            }
        }
        private long _DisChargeDestinationID;
        public long DisChargeDestinationID
        {
            get
            {
                return _DisChargeDestinationID;
            }

            set
            {
                if (value != _DisChargeDestinationID)
                {
                    _DischargeTypeID = value;
                    OnPropertyChanged("DisChargeDestinationID");
                }
            }
        }

        //------------------------------------------------------------------
        //Added By Santosh Patil
        private long _AdmID;
        public long AdmID
        {
            get { return _AdmID; }
            set
            {
                if (_AdmID != value)
                {
                    _AdmID = value;
                    OnPropertyChanged("AdmID");
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

        private long _AdmUnitID;
        public long AdmUnitID
        {
            get { return _AdmUnitID; }
            set
            {
                if (_AdmUnitID != value)
                {
                    _AdmUnitID = value;
                    OnPropertyChanged("AdmUnitID");
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

        private long _AdmissionType;
        public long AdmissionType
        {
            get { return _AdmissionType; }
            set
            {
                if (_AdmissionType != value)
                {
                    _AdmissionType = value;
                    OnPropertyChanged("AdmissionType");
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

        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (_DepartmentID != value)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
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
                    OnPropertyChanged("DepartmentName");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
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

        private string _BillingClass;
        public string BillingClass
        {
            get { return _BillingClass; }
            set
            {
                if (_BillingClass != value)
                {
                    _BillingClass = value;
                    OnPropertyChanged("BillingClass");
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

        private long _FromClassID;
        public long FromClassID
        {
            get { return _FromClassID; }
            set
            {
                if (_FromClassID != value)
                {
                    _FromClassID = value;
                    OnPropertyChanged("FromClassID");
                }
            }
        }

        private long _ToClassID;
        public long ToClassID
        {
            get { return _ToClassID; }
            set
            {
                if (_ToClassID != value)
                {
                    _ToClassID = value;
                    OnPropertyChanged("ToClassID");
                }
            }
        }

        private long _FromWardID;
        public long FromWardID
        {
            get { return _FromWardID; }
            set
            {
                if (_FromWardID != value)
                {
                    _FromWardID = value;
                    OnPropertyChanged("FromWardID");
                }
            }
        }

        private long _ToWardID;
        public long ToWardID
        {
            get { return _ToWardID; }
            set
            {
                if (_ToWardID != value)
                {
                    _ToWardID = value;
                    OnPropertyChanged("ToWardID");
                }
            }
        }

        private string _FromClass;
        public string FromClass
        {
            get { return _FromClass; }
            set
            {
                if (_FromClass != value)
                {
                    _FromClass = value;
                    OnPropertyChanged("FromClass");
                }
            }
        }

        private string _ToClass;
        public string ToClass
        {
            get { return _ToClass; }
            set
            {
                if (_ToClass != value)
                {
                    _ToClass = value;
                    OnPropertyChanged("ToClass");
                }
            }
        }

        private string _FromWard;
        public string FromWard
        {
            get { return _FromWard; }
            set
            {
                if (_FromWard != value)
                {
                    _FromWard = value;
                    OnPropertyChanged("FromWard");
                }
            }
        }

        private string _ToWard;
        public string ToWard
        {
            get { return _ToWard; }
            set
            {
                if (_ToWard != value)
                {
                    _ToWard = value;
                    OnPropertyChanged("ToWard");
                }
            }
        }

        private long _FromBedID;
        public long FromBedID
        {
            get { return _FromBedID; }
            set
            {
                if (_FromBedID != value)
                {
                    _FromBedID = value;
                    OnPropertyChanged("FromBedID");
                }
            }
        }

        private long _FromBedUnitID;
        public long FromBedUnitID
        {
            get { return _FromBedUnitID; }
            set
            {
                if (_FromBedUnitID != value)
                {
                    _FromBedUnitID = value;
                    OnPropertyChanged("FromBedUnitID");
                }
            }
        }

        private string _FromBed;
        public string FromBed
        {
            get { return _FromBed; }
            set
            {
                if (_FromBed != value)
                {
                    _FromBed = value;
                    OnPropertyChanged("FromBed");
                }
            }
        }

        private long _ToBedID;
        public long ToBedID
        {
            get { return _ToBedID; }
            set
            {
                if (_ToBedID != value)
                {
                    _ToBedID = value;
                    OnPropertyChanged("ToBedID");
                }
            }
        }

        private long _ToBedUnitID;
        public long ToBedUnitID
        {
            get { return _ToBedUnitID; }
            set
            {
                if (_ToBedUnitID != value)
                {
                    _ToBedUnitID = value;
                    OnPropertyChanged("ToBedUnitID");
                }
            }
        }

        private string _ToBed;
        public string ToBed
        {
            get { return _ToBed; }
            set
            {
                if (_ToBed != value)
                {
                    _ToBed = value;
                    OnPropertyChanged("ToBed");
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

        private string _BedName;
        public string BedName
        {
            get { return _BedName; }
            set
            {
                if (_BedName != value)
                {
                    _BedName = value;
                    OnPropertyChanged("BedName");
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

        private DateTime? _FromTime;
        public DateTime? FromTime
        {
            get { return _FromTime; }
            set
            {
                if (_FromTime != value)
                {
                    _FromTime = value;
                    OnPropertyChanged("FromTime");
                }
            }
        }

        private DateTime? _ToTime;
        public DateTime? ToTime
        {
            get { return _ToTime; }
            set
            {
                if (_ToTime != value)
                {
                    _ToTime = value;
                    OnPropertyChanged("ToTime");
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

        private bool? _IsSecondaryBed;
        public bool? IsSecondaryBed
        {
            get { return _IsSecondaryBed; }
            set
            {
                if (_IsSecondaryBed != value)
                {
                    _IsSecondaryBed = value;
                    OnPropertyChanged("IsSecondaryBed");
                }
            }
        }

        private bool _CurrentVisitStatus;
        public bool CurrentVisitStatus
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

        private string _ReleaseRemark;
        public string ReleaseRemark
        {
            get { return _ReleaseRemark; }
            set
            {
                if (_ReleaseRemark != value)
                {
                    _ReleaseRemark = value;
                    OnPropertyChanged("ReleaseRemark");
                }
            }
        }

        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged("IsClosed");
                }
            }
        }

        private bool _IsCancel;
        public bool IsCancel
        {
            get { return _IsCancel; }
            set
            {
                if (_IsCancel != value)
                {
                    _IsCancel = value;
                    OnPropertyChanged("IsCancel");
                }
            }
        }

        private bool _InterORFinal;
        public bool InterORFinal
        {
            get { return _InterORFinal; }
            set
            {
                if (_InterORFinal != value)
                {
                    _InterORFinal = value;
                    OnPropertyChanged("InterORFinal");
                }
            }
        }

        private bool _IsOccupied;
        public bool IsOccupied
        {
            get { return _IsOccupied; }
            set
            {
                if (_IsOccupied != value)
                {
                    _IsOccupied = value;
                    OnPropertyChanged("IsOccupied");
                }
            }
        }

        //---------------------------------------
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

        private string _PatientNameForTransfer = "";
        public string PatientNameForTransfer
        {
            get { return _PatientNameForTransfer; }
            set
            {
                if (value != _PatientNameForTransfer)
                {
                    _PatientNameForTransfer = value;
                    OnPropertyChanged("PatientNameForTransfer");
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

        private string strFamilyName = "";
        public string FamilyName
        {
            get { return strFamilyName; }
            set
            {
                if (value != strFamilyName)
                {
                    strFamilyName = value;
                    OnPropertyChanged("FamilyName");
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

        private long _TransFerID;
        public long TransFerID
        {
            get { return _TransFerID; }
            set
            {
                if (_TransFerID != value)
                {
                    _TransFerID = value;
                    OnPropertyChanged("TransFerID");
                }
            }
        }

        private long _TransferUnitID;
        public long TransferUnitID
        {
            get { return _TransferUnitID; }
            set
            {
                if (_TransferUnitID != value)
                {
                    _TransferUnitID = value;
                    OnPropertyChanged("TransferUnitID");
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
