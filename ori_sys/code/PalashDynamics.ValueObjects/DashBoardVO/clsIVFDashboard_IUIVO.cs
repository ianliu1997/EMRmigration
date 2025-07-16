using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
  public  class clsIVFDashboard_IUIVO : IValueObject, INotifyPropertyChanged
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

        private long _Status;
        public long Status
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }

        private bool _Synchronized;
        public bool Synchronized
        {
            get { return _Synchronized; }
            set
            {
                if (_Synchronized != value)
                {
                    _Synchronized = value;
                    OnPropertyChanged("Synchronized");
                }
            }
        }

        #endregion

        #region Properties
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

        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;
                    OnPropertyChanged("PlanTherapyID");
                }
            }
        }

        private long _PlanTherapyUnitID;
        public long PlanTherapyUnitID
        {
            get { return _PlanTherapyUnitID; }
            set
            {
                if (_PlanTherapyUnitID != value)
                {
                    _PlanTherapyUnitID = value;
                    OnPropertyChanged("PlanTherapyUnitID");
                }
            }
        }
        private DateTime? _IUIDate = DateTime.Now;
        public DateTime? IUIDate
        {
            get { return _IUIDate; }
            set
            {
                if (_IUIDate != value)
                {
                    _IUIDate = value;
                    OnPropertyChanged("IUIDate");
                }
            }
        }
        private DateTime? _IUITime = DateTime.Now;
        public DateTime? IUITime
        {
            get { return _IUITime; }
            set
            {
                if (_IUITime != value)
                {
                    _IUITime = value;
                    OnPropertyChanged("IUITime");
                }
            }
        }
        private long _InseminatedByID;
        public long InseminatedByID
        {
            get { return _InseminatedByID; }
            set
            {
                if (_InseminatedByID != value)
                {
                    _InseminatedByID = value;
                    OnPropertyChanged("InseminatedByID");
                }
            }
        }
        private long _WitnessedByID;
        public long WitnessedByID
        {
            get { return _WitnessedByID; }
            set
            {
                if (_WitnessedByID != value)
                {
                    _WitnessedByID = value;
                    OnPropertyChanged("WitnessedByID");
                }
            }
        }
        private long _InseminationLocationID;
        public long InseminationLocationID
        {
            get { return _InseminationLocationID; }
            set
            {
                if (_InseminationLocationID != value)
                {
                    _InseminationLocationID = value;
                    OnPropertyChanged("InseminationLocationID");
                }
            }
        }
        private bool _IsHomologous;
        public bool IsHomologous
        {
            get { return _IsHomologous; }
            set
            {
                if (_IsHomologous != value)
                {
                    _IsHomologous = value;
                    OnPropertyChanged("IsHomologous");
                }
            }
        }
        private DateTime? _CollectionDate= DateTime.Now;
        public DateTime? CollectionDate
        {
            get { return _CollectionDate; }
            set
            {
                if (_CollectionDate != value)
                {
                    _CollectionDate = value;
                    OnPropertyChanged("CollectionDate");
                }
            }
        }
        private DateTime? _PreperationDate = DateTime.Now;
        public DateTime? PreperationDate
        {
            get { return _PreperationDate; }
            set
            {
                if (_PreperationDate != value)
                {
                    _PreperationDate = value;
                    OnPropertyChanged("PreperationDate");
                }
            }
        }
        private DateTime? _ThawingDate = DateTime.Now;
        public DateTime? ThawingDate
        {
            get { return _ThawingDate; }
            set
            {
                if (_ThawingDate != value)
                {
                    _ThawingDate = value;
                    OnPropertyChanged("ThawingDate");
                }
            }
        }
        private string _SampleID;
        public string SampleID
        {
            get { return _SampleID; }
            set
            {
                if (_SampleID != value)
                {
                    _SampleID = value;
                    OnPropertyChanged("SampleID");
                }
            }
        }
        private string _Purpose;
        public string Purpose
        {
            get { return _Purpose; }
            set
            {
                if (_Purpose != value)
                {
                    _Purpose = value;
                    OnPropertyChanged("Purpose");
                }
            }
        }
        private string _Diagnosis;
        public string Diagnosis
        {
            get { return _Diagnosis; }
            set
            {
                if (_Diagnosis != value)
                {
                    _Diagnosis = value;
                    OnPropertyChanged("Diagnosis");
                }
            }
        }
        private long _CollectionMethodID;
        public long CollectionMethodID
        {
            get { return _CollectionMethodID; }
            set
            {
                if (_CollectionMethodID != value)
                {
                    _CollectionMethodID = value;
                    OnPropertyChanged("CollectionMethodID");
                }
            }
        }
        private double _InseminatedAmounts;
        public double InseminatedAmounts
        {
            get { return _InseminatedAmounts; }
            set
            {
                if (_InseminatedAmounts != value)
                {
                    _InseminatedAmounts = value;
                    OnPropertyChanged("InseminatedAmounts");
                }
            }
        }
        private double _NumberofMotileSperm;
        public double NumberofMotileSperm
        {
            get { return _NumberofMotileSperm; }
            set
            {
                if (_NumberofMotileSperm != value)
                {
                    _NumberofMotileSperm = value;
                    OnPropertyChanged("NumberofMotileSperm");
                }
            }
        }
        private double _NativeAmount;
        public double NativeAmount
        {
            get { return _NativeAmount; }
            set
            {
                if (_NativeAmount != value)
                {
                    _NativeAmount = value;
                    OnPropertyChanged("NativeAmount");
                }
            }
        }
        private double _AfterPrepAmount;
        public double AfterPrepAmount
        {
            get { return _AfterPrepAmount; }
            set
            {
                if (_AfterPrepAmount != value)
                {
                    _AfterPrepAmount = value;
                    OnPropertyChanged("AfterPrepAmount");
                }
            }
        }
        private double _NativeConcentration;
        public double NativeConcentration
        {
            get { return _NativeConcentration; }
            set
            {
                if (_NativeConcentration != value)
                {
                    _NativeConcentration = value;
                    OnPropertyChanged("NativeConcentration");
                }
            }
        }
        private double _AfterPrepConcentration;
        public double AfterPrepConcentration
        {
            get { return _AfterPrepConcentration; }
            set
            {
                if (_AfterPrepConcentration != value)
                {
                    _AfterPrepConcentration = value;
                    OnPropertyChanged("AfterPrepConcentration");
                }
            }
        }
        private double _NativeProgressiveMotatity;
        public double NativeProgressiveMotatity
        {
            get { return _NativeProgressiveMotatity; }
            set
            {
                if (_NativeProgressiveMotatity != value)
                {
                    _NativeProgressiveMotatity = value;
                    OnPropertyChanged("NativeProgressiveMotatity");
                }
            }
        }
        private double _AfterPrepProgressiveMotatity;
        public double AfterPrepProgressiveMotatity
        {
            get { return _AfterPrepProgressiveMotatity; }
            set
            {
                if (_AfterPrepProgressiveMotatity != value)
                {
                    _AfterPrepProgressiveMotatity = value;
                    OnPropertyChanged("AfterPrepProgressiveMotatity");
                }
            }
        }
        private double _NativeOverallMotality;
        public double NativeOverallMotality
        {
            get { return _NativeOverallMotality; }
            set
            {
                if (_NativeOverallMotality != value)
                {
                    _NativeOverallMotality = value;
                    OnPropertyChanged("NativeOverallMotality");
                }
            }
        }
        private double _AfterPrepOverallMotality;
        public double AfterPrepOverallMotality
        {
            get { return _AfterPrepOverallMotality; }
            set
            {
                if (_AfterPrepOverallMotality != value)
                {
                    _AfterPrepOverallMotality = value;
                    OnPropertyChanged("AfterPrepOverallMotality");
                }
            }
        }
        private double _NativeNormalForms;
        public double NativeNormalForms
        {
            get { return _NativeNormalForms; }
            set
            {
                if (_NativeNormalForms != value)
                {
                    _NativeNormalForms = value;
                    OnPropertyChanged("NativeNormalForms");
                }
            }
        }
        private double _AfterPrepNormalForms;
        public double AfterPrepNormalForms
        {
            get { return _AfterPrepNormalForms; }
            set
            {
                if (_AfterPrepNormalForms != value)
                {
                    _AfterPrepNormalForms = value;
                    OnPropertyChanged("AfterPrepNormalForms");
                }
            }
        }
        private double _NativeTotalNoOfSperms;
        public double NativeTotalNoOfSperms
        {
            get { return _NativeTotalNoOfSperms; }
            set
            {
                if (_NativeTotalNoOfSperms != value)
                {
                    _NativeTotalNoOfSperms = value;
                    OnPropertyChanged("NativeTotalNoOfSperms");
                }
            }
        }
        private double _AfterPrepTotalNoOfSperms;
        public double AfterPrepTotalNoOfSperms
        {
            get { return _AfterPrepTotalNoOfSperms; }
            set
            {
                if (_AfterPrepTotalNoOfSperms != value)
                {
                    _AfterPrepTotalNoOfSperms = value;
                    OnPropertyChanged("AfterPrepTotalNoOfSperms");
                }
            }
        }
        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }
        #endregion
    }
}
