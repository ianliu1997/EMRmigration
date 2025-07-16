using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
namespace PalashDynamics.ValueObjects.Patient
{
    public class clsTESEVO : IValueObject, INotifyPropertyChanged
    {
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
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

        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
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

        private long _SpouseID;
        public long SpouseID
        {
            get { return _SpouseID; }
            set
            {
                if (_SpouseID != value)
                {
                    _SpouseID = value;
                    OnPropertyChanged("SpouseID");
                }
            }
        }

        private long _PatientTypeID;
        public long PatientTypeID
        {
            get { return _PatientTypeID; }
            set
            {
                if (_PatientTypeID != value)
                {
                    _PatientTypeID = value;
                    OnPropertyChanged("PatientTypeID");
                }
            }
        }

        private long _TissueSideID;
        public long TissueSideID
        {
            get { return _TissueSideID; }
            set
            {
                if (_TissueSideID != value)
                {
                    _TissueSideID = value;
                    OnPropertyChanged("TissueSideID");
                }
            }
        }

        private long _LabInchargeID;
        public long LabInchargeID
        {
            get { return _LabInchargeID; }
            set
            {
                if (_LabInchargeID != value)
                {
                    _LabInchargeID = value;
                    OnPropertyChanged("LabInchargeID");
                }
            }
        }

        private long _EmbroLogistID;
        public long EmbroLogistID
        {
            get { return _EmbroLogistID; }
            set
            {
                if (_EmbroLogistID != value)
                {
                    _EmbroLogistID = value;
                    OnPropertyChanged("EmbroLogistID");
                }
            }
        }
        private DateTime? _RegistrationDate = DateTime.Now;
        public DateTime? RegistrationDate
        {
            get { return _RegistrationDate; }
            set
            {
                if (_RegistrationDate != value)
                {
                    _RegistrationDate = value;
                    OnPropertyChanged("RegistarationDate");
                }
            }
        }

        private DateTime? _CryoDate = DateTime.Now.Date;
        public DateTime? CryoDate
        {
            get { return _CryoDate; }
            set
            {
                if (_CryoDate != value)
                {
                    _CryoDate = value;
                    OnPropertyChanged("CryoDate");
                }
            }
        }

        private DateTime? _CryoTime = DateTime.Now;
        public DateTime? CryoTime
        {
            get { return _CryoTime; }
            set
            {
                if (_CryoTime != value)
                {
                    _CryoTime = value;
                    OnPropertyChanged("CryoTime");
                }
            }
        }

        private string _Tissue = "";
        public string Tissue
        {
            get { return _Tissue; }
            set
            {
                if (_Tissue != value)
                {
                    _Tissue = value;
                    OnPropertyChanged("Tissue");
                }
            }
        }

        private bool _Status;
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
        public clsCoupleVO CoupleDetail { get; set; }
    }

    public class clsTESEDetailsVO : IValueObject, INotifyPropertyChanged
    {
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
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

        private long _TESE_ID;
        public long TESE_ID
        {
            get { return _TESE_ID; }
            set
            {
                if (_TESE_ID != value)
                {
                    _TESE_ID = value;
                    OnPropertyChanged("TESE_ID");
                }
            }
        }

        private long _TESE_UnitID;
        public long TESE_UnitID
        {
            get { return _TESE_UnitID; }
            set
            {
                if (_TESE_UnitID != value)
                {
                    _TESE_UnitID = value;
                    OnPropertyChanged("TESE_UnitID");
                }
            }
        }

        private long _No_of_VailsUsed;
        public long No_of_VailsUsed
        {
            get { return _No_of_VailsUsed; }
            set
            {
                if (_No_of_VailsUsed != value)
                {
                    _No_of_VailsUsed = value;
                    OnPropertyChanged("No_of_VailsUsed");
                }
            }
        }

        private long _HolderNumber;
        public long HolderNumber
        {
            get { return _HolderNumber; }
            set
            {
                if (_HolderNumber != value)
                {
                    _HolderNumber = value;
                    OnPropertyChanged("HolderNumber");
                }
            }
        }

        private long _ContainerNumber;
        public long ContainerNumber
        {
            get { return _ContainerNumber; }
            set
            {
                if (_ContainerNumber != value)
                {
                    _ContainerNumber = value;
                    OnPropertyChanged("ContainerNumber");
                }
            }
        }

        private long _NoofVailsFrozen;
        public long NoofVailsFrozen
        {
            get { return _NoofVailsFrozen; }
            set
            {
                if (_NoofVailsFrozen != value)
                {
                    _NoofVailsFrozen = value;
                    OnPropertyChanged("NoofVailsFrozen");
                }
            }
        }

        private long _No_of_VailsRemain;
        public long No_of_VailsRemain
        {
            get { return _No_of_VailsRemain; }
            set
            {
                if (_No_of_VailsRemain != value)
                {
                    _No_of_VailsRemain = value;
                    OnPropertyChanged("No_of_VailsRemain");
                }
            }
        }

        private DateTime? _CryoDate = DateTime.Now.Date;
        public DateTime? CryoDate
        {
            get { return _CryoDate; }
            set
            {
                if (_CryoDate != value)
                {
                    _CryoDate = value;
                    OnPropertyChanged("CryoDate");
                }
            }
        }

        private DateTime? _CryoTime = DateTime.Now;
        public DateTime? CryoTime
        {
            get { return _CryoTime; }
            set
            {
                if (_CryoTime != value)
                {
                    _CryoTime = value;
                    OnPropertyChanged("CryoTime");
                }
            }
        }
        private DateTime? _RenewalDate = DateTime.Now;
        public DateTime? RenewalDate
        {
            get { return _RenewalDate; }
            set
            {
                if (_RenewalDate != value)
                {
                    _RenewalDate = value;
                    OnPropertyChanged("RenewalDate");
                }
            }
        }

        private string _Tissue;
        public string Tissue
        {
            get { return _Tissue; }

            set
            {
                if (_Tissue != value)
                {
                    _Tissue = value;
                    OnPropertyChanged("Tissue");
                }
            }
        }

        private long _TissueSideID;
        public long TissueSideID
        {
            get { return _TissueSideID; }
            set
            {
                if (_TissueSideID != value)
                {
                    _TissueSideID = value;
                    OnPropertyChanged("TissueSideID");
                }
            }
        }



        private long _LabInchargeID;
        public long LabInchargeID
        {
            get { return _LabInchargeID; }
            set
            {
                if (_LabInchargeID != value)
                {
                    _LabInchargeID = value;
                    OnPropertyChanged("LabInchargeID");
                }
            }
        }

        private long _EmbroLogistID;
        public long EmbroLogistID
        {
            get { return _EmbroLogistID; }
            set
            {
                if (_EmbroLogistID != value)
                {
                    _EmbroLogistID = value;
                    OnPropertyChanged("EmbroLogistID");
                }
            }
        }

        private string _EmbroLogist;
        public string EmbroLogist
        {
            get { return _EmbroLogist; }
            set
            {
                if (_EmbroLogist != value)
                {
                    _EmbroLogist = value;
                    OnPropertyChanged("EmbroLogist");
                }
            }
        }
        private string _LabIncharge;
        public string LabIncharge
        {
            get { return _LabIncharge; }
            set
            {
                if (_LabIncharge != value)
                {
                    _LabIncharge = value;
                    OnPropertyChanged("LabIncharge");
                }
            }
        }
    }

}
