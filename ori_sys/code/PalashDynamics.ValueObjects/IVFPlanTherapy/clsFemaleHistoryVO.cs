using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
   public class clsFemaleHistoryVO : IValueObject,INotifyPropertyChanged
    {
         
        public string ToXml()
        {
            return this.ToString();
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

        #region Property Declarations
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
       
        private double _MarriedSinceYears;
        public double MarriedSinceYears
        {
            get { return _MarriedSinceYears; }
            set
            {
                if (_MarriedSinceYears != value)
                {
                    _MarriedSinceYears = value;
                    OnPropertyChanged("MarriedSinceYears");
                }
            }
        }

        private double _MarriedSinceMonths;
        public double MarriedSinceMonths
        {
            get { return _MarriedSinceMonths; }
            set
            {
                if (_MarriedSinceMonths != value)
                {
                    _MarriedSinceMonths = value;
                    OnPropertyChanged("MarriedSinceMonths");
                }
            }
        }

        private string _Menarche;
        public string Menarche
        {
            get { return _Menarche; }
            set
            {
                if (_Menarche != value)
                {
                    _Menarche = value;
                    OnPropertyChanged("Menarche");
                }
            }
        }

        private double _DurationofRelationship;
        public double DurationofRelationship
        {
            get { return _DurationofRelationship; }
            set
            {
                if (_DurationofRelationship != value)
                {
                    _DurationofRelationship = value;
                    OnPropertyChanged("DurationofRelationship");
                }
            }
        }

        private bool _ContraceptionUsed;
        public  bool ContraceptionUsed
        {
            get { return _ContraceptionUsed; }
            set
            {
                if (_ContraceptionUsed != value)
                {
                    _ContraceptionUsed = value;
                    OnPropertyChanged("ContraceptionUsed");
                }
            }
        }

        private double _DurationofContraceptionUsed;
        public double DurationofContraceptionUsed
        {
            get { return _DurationofContraceptionUsed; }
            set
            {
                if (_DurationofContraceptionUsed != value)
                {
                    _DurationofContraceptionUsed = value;
                    OnPropertyChanged("DurationofContraceptionUsed");
                }
            }
        }

        private string _MethodOfContraception;
        public  string MethodOfContraception
        {
            get { return _MethodOfContraception; }
            set
            {
                if (_MethodOfContraception != value)
                {
                    _MethodOfContraception = value;
                    OnPropertyChanged("MethodOfContraception");
                }
            }
        }

        private double _InfertilitySinceYears;
        public double InfertilitySinceYears
        {
            get { return _InfertilitySinceYears; }
            set
            {
                if (_InfertilitySinceYears != value)
                {
                    _InfertilitySinceYears = value;
                    OnPropertyChanged("InfertilitySinceYears");
                }
            }
        }

        private IVFInfertilityTypes _InfertilityType;
        public  IVFInfertilityTypes InfertilityType
        {
            get { return _InfertilityType; }
            set
            {
                if (_InfertilityType != value)
                {
                    _InfertilityType = value;
                    OnPropertyChanged("InfertilityType");
                }
            }
        }

        private bool _FemaleInfertility;
        public  bool FemaleInfertility
        {
            get { return _FemaleInfertility; }
            set
            {
                if (_FemaleInfertility != value)
                {
                    _FemaleInfertility = value;
                    OnPropertyChanged("FemaleInfertility");
                }
            }
        }

        private bool _MaleInfertility;
        public  bool MaleInfertility
        {
            get { return _MaleInfertility; }
            set
            {
                if (_MaleInfertility != value)
                {
                    _MaleInfertility = value;
                    OnPropertyChanged("MaleInfertility");
                }
            }
        }

        private string _Couple;
        public string Couple
        {
            get { return _Couple; }
            set
            {
                if (_Couple != value)
                {
                    _Couple = value;
                    OnPropertyChanged("Couple");
                }
            }
        }

        private string _MedicationTakenforInfertility;
        public string MedicationTakenforInfertility
        {
            get { return _MedicationTakenforInfertility; }
            set
            {
                if (_MedicationTakenforInfertility != value)
                {
                    _MedicationTakenforInfertility = value;
                    OnPropertyChanged("MedicationTakenforInfertility");
                }
            }
        }

        private bool _SexualDisfunction;
        public bool SexualDisfunction
        {
            get { return _SexualDisfunction; }
            set
            {
                if (_SexualDisfunction != value)
                {
                    _SexualDisfunction = value;
                    OnPropertyChanged("SexualDisfunction");
                }
            }
        }

        private string _SexualDisfunctionRamarks;
        public  string SexualDisfunctionRamarks
        {
            get { return _SexualDisfunctionRamarks; }
            set
            {
                if (_SexualDisfunctionRamarks != value)
                {
                    _SexualDisfunctionRamarks = value;
                    OnPropertyChanged("SexualDisfunctionRamarks");
                }
            }
        }

        private DateTime? _LMP;
        public  DateTime? LMP
        {
            get { return _LMP; }
            set
            {
                if (_LMP != value)
                {
                    _LMP = value;
                    OnPropertyChanged("LMP");
                }
            }
        }

        private bool _Regular;
        public  bool Regular
        {
            get { return _Regular; }
            set
            {
                if (_Regular != value)
                {
                    _Regular = value;
                    OnPropertyChanged("Regular");
                }
            }
        }

        private double _Length;
        public  double Length
        {
            get { return _Length; }
            set
            {
                if (_Length != value)
                {
                    _Length = value;
                    OnPropertyChanged("Length");
                }
            }
        }

        private string _DurationOfPeriod;
        public string DurationOfPeriod
        {
            get { return _DurationOfPeriod; }
            set
            {
                if (_DurationOfPeriod != value)
                {
                    _DurationOfPeriod = value;
                    OnPropertyChanged("DurationOfPeriod");
                }
            }
        }

        private IVFBloodLoss _BloodLoss;
        public IVFBloodLoss BloodLoss
        {
            get { return _BloodLoss; }
            set
            {
                if (_BloodLoss != value)
                {
                    _BloodLoss = value;
                    OnPropertyChanged("BloodLoss");
                }
            }
        }

        private bool _Dymenorhea;
        public  bool Dymenorhea
        {
            get { return _Dymenorhea; }
            set
            {
                if (_Dymenorhea != value)
                {
                    _Dymenorhea = value;
                    OnPropertyChanged("Dymenorhea");
                }
            }
        }

        private bool _MidCyclePain;
        public  bool MidCyclePain
        {
            get { return _MidCyclePain; }
            set
            {
                if (_MidCyclePain != value)
                {
                    _MidCyclePain = value;
                    OnPropertyChanged("MidCyclePain");
                }
            }
        }

        private bool _InterMenstrualBleeding;
        public  bool InterMenstrualBleeding
        {
            get { return _InterMenstrualBleeding; }
            set
            {
                if (_InterMenstrualBleeding != value)
                {
                    _InterMenstrualBleeding = value;
                    OnPropertyChanged("InterMenstrualBleeding");
                }
            }
        }

        private bool _PreMenstrualTension;
        public  bool PreMenstrualTension
        {
            get { return _PreMenstrualTension; }
            set
            {
                if (_PreMenstrualTension != value)
                {
                    _PreMenstrualTension = value;
                    OnPropertyChanged("PreMenstrualTension");
                }
            }
        }

        private bool _Dysparuneia;
        public  bool Dysparuneia
        {
            get { return _Dysparuneia; }
            set
            {
                if (_Dysparuneia != value)
                {
                    _Dysparuneia = value;
                    OnPropertyChanged("Dysparuneia");
                }
            }
        }

        private bool _PostCoitalBleeding;
        public bool PostCoitalBleeding
        {
            get { return _PostCoitalBleeding; }
            set
            {
                if (_PostCoitalBleeding != value)
                {
                    _PostCoitalBleeding = value;
                    OnPropertyChanged("PostCoitalBleeding");
                }
            }
        }

        private string _DetailsOfPastCycles;
        public string DetailsOfPastCycles
        {
            get { return _DetailsOfPastCycles; }
            set
            {
                if (_DetailsOfPastCycles != value)
                {
                    _DetailsOfPastCycles = value;
                    OnPropertyChanged("DetailsOfPastCycles");
                }
            }
        }

        private string _ObstHistoryComplications;
        public  string ObstHistoryComplications
        {
            get { return _ObstHistoryComplications; }
            set
            {
                if (_ObstHistoryComplications != value)
                {
                    _ObstHistoryComplications = value;
                    OnPropertyChanged("ObstHistoryComplications");
                }
            }
        }

        private string _Surgeries;
        public  string Surgeries
        {
            get { return _Surgeries; }
            set
            {
                if (_Surgeries != value)
                {
                    _Surgeries = value;
                    OnPropertyChanged("Surgeries");
                }
            }
        }

        private bool _PreviousIUI;
        public  bool PreviousIUI
        {
            get { return _PreviousIUI; }
            set
            {
                if (_PreviousIUI != value)
                {
                    _PreviousIUI = value;
                    OnPropertyChanged("PreviousIUI");
                }
            }
        }

        private double _IUINoOfTimes;
        public double IUINoOfTimes
        {
            get { return _IUINoOfTimes; }
            set
            {
                if (_IUINoOfTimes != value)
                {
                    _IUINoOfTimes = value;
                    OnPropertyChanged("IUINoOfTimes");
                }
            }
        }

        private string _IUIPlace;
        public  string IUIPlace
        {
            get { return _IUIPlace; }
            set
            {
                if (_IUIPlace != value)
                {
                    _IUIPlace = value;
                    OnPropertyChanged("IUIPlace");
                }
            }
        }

        private bool _IUISuccessfull;
        public  bool IUISuccessfull
        {
            get { return _IUISuccessfull; }
            set
            {
                if (_IUISuccessfull != value)
                {
                    _IUISuccessfull = value;
                    OnPropertyChanged("IUISuccessfull");
                }
            }
        }

        private bool _PreviousIVF;
        public  bool PreviousIVF
        {
            get { return _PreviousIVF; }
            set
            {
                if (_PreviousIVF != value)
                {
                    _PreviousIVF = value;
                    OnPropertyChanged("PreviousIVF");
                }
            }
        }

        private double _IVFNoOfTimes;
        public  double IVFNoOfTimes
        {
            get { return _IVFNoOfTimes; }
            set
            {
                if (_IVFNoOfTimes != value)
                {
                    _IVFNoOfTimes = value;
                    OnPropertyChanged("IVFNoOfTimes");
                }
            }
        }

        private string _IVFPlace;
        public  string IVFPlace
        {
            get { return _IVFPlace; }
            set
            {
                if (_IVFPlace != value)
                {
                    _IVFPlace = value;
                    OnPropertyChanged("IVFPlace");
                }
            }
        }
        private bool _IVFSuccessfull;
        public  bool IVFSuccessfull
        {
            get { return _IVFSuccessfull; }
            set
            {
                if (_IVFSuccessfull != value)
                {
                    _IVFSuccessfull = value;
                    OnPropertyChanged("IVFSuccessfull");
                }
            }
        }

        private string _SpecialNotes;
        public string SpecialNotes
        {
            get { return _SpecialNotes; }
            set
            {
                if (_SpecialNotes != value)
                {
                    _SpecialNotes = value;
                    OnPropertyChanged("SpecialNotes");
                }
            }
        }

        private string _Illness;
        public  string Illness
        {
            get { return _Illness; }
            set
            {
                if (_Illness != value)
                {
                    _Illness = value;
                    OnPropertyChanged("Illness");
                }
            }
        }

        private string _Allergies;
        public  string Allergies
        {
            get { return _Allergies; }
            set
            {
                if (_Allergies != value)
                {
                    _Allergies = value;
                    OnPropertyChanged("Allergies");
                }
            }
        }

        private string _FamilySocialHistory;
        public string FamilySocialHistory
        {
            get { return _FamilySocialHistory; }
            set
            {
                if (_FamilySocialHistory != value)
                {
                    _FamilySocialHistory = value;
                    OnPropertyChanged("FamilySocialHistory");
                }
            }
        }

        #endregion

        #region Common Properties


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

    
        #endregion
    }
}
